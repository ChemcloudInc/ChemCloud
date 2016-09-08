using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Service.Order.Business;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class OrderService : ServiceBase, IOrderService, IService, IDisposable
    {
        private OrderBO _orderBO;

        public OrderService()
        {
            _orderBO = new OrderBO();
        }

        private void AddIntegral(UserMemberInfo member, long orderId, decimal orderTotal)
        {
            MemberIntegralExchangeRules integralChangeRule = Instance<IMemberIntegralService>.Create.GetIntegralChangeRule();
            if (integralChangeRule == null)
            {
                return;
            }
            int moneyPerIntegral = integralChangeRule.MoneyPerIntegral;
            int num = Convert.ToInt32(Math.Floor(orderTotal / moneyPerIntegral));
            MemberIntegralRecord memberIntegralRecord = new MemberIntegralRecord()
            {
                UserName = member.UserName,
                MemberId = member.Id,
                RecordDate = new DateTime?(DateTime.Now),
                TypeId = MemberIntegral.IntegralType.Consumption
            };
            MemberIntegralRecordAction memberIntegralRecordAction = new MemberIntegralRecordAction()
            {
                VirtualItemTypeId = new MemberIntegral.VirtualItemType?(MemberIntegral.VirtualItemType.Consumption),
                VirtualItemId = orderId
            };
            memberIntegralRecord.ChemCloud_MemberIntegralRecordAction.Add(memberIntegralRecordAction);
            IConversionMemberIntegralBase conversionMemberIntegralBase = Instance<IMemberIntegralConversionFactoryService>.Create.Create(MemberIntegral.IntegralType.Consumption, num);
            Instance<IMemberIntegralService>.Create.AddMemberIntegral(memberIntegralRecord, conversionMemberIntegralBase);
        }

        private void AddOrderOperationLog(long orderId, string userName, string operateContent)
        {
            OrderOperationLogInfo orderOperationLogInfo = new OrderOperationLogInfo()
            {
                Operator = userName,
                OrderId = orderId,
                OperateDate = DateTime.Now,
                OperateContent = operateContent
            };
            context.OrderOperationLogInfo.Add(orderOperationLogInfo);
            context.SaveChanges();
        }

        public void AgreeToRefundBySeller(long orderId)
        {
            OrderInfo orderInfo = context.OrderInfo.FindById<OrderInfo>(orderId);
            if (orderInfo.OrderStatus != OrderInfo.OrderOperateStatus.WaitDelivery)
            {
                throw new HimallException("只可以关闭待发货订单！");
            }
            orderInfo.OrderStatus = OrderInfo.OrderOperateStatus.Close;
            orderInfo.CloseReason = "商家同意退款，取消订单";
            ReturnStock(orderInfo);
            context.SaveChanges();
        }

        public void AutoCloseOrder()
        {
            int num;
            try
            {
                DateTime now = DateTime.Now;
                SiteSettingsInfo siteSettings = Instance<ISiteSettingService>.Create.GetSiteSettings();
                List<OrderInfo> list = (
                    from a in context.OrderInfo
                    where (a.OrderDate < now) && (int)a.OrderStatus == 1
                    select a).ToList();
                IProductService create = Instance<IProductService>.Create;
                foreach (OrderInfo orderInfo in list)
                {
                    if (siteSettings == null)
                    {
                        num = 2;
                    }
                    else
                    {
                        num = (siteSettings.UnpaidTimeout == 0 ? 2 : siteSettings.UnpaidTimeout);
                    }
                    if (DateTime.Now.Subtract(orderInfo.OrderDate).Hours < num)
                    {
                        continue;
                    }
                    orderInfo.OrderStatus = OrderInfo.OrderOperateStatus.Close;
                    orderInfo.CloseReason = "过期没付款，自动关闭";
                    List<OrderItemInfo> orderItemInfos = (
                        from c in context.OrderItemInfo
                        where c.OrderId == orderInfo.Id
                        select c).ToList();
                    foreach (OrderItemInfo orderItemInfo in orderItemInfos)
                    {
                        create.UpdateStock(orderItemInfo.SkuId, orderItemInfo.Quantity);
                    }
                    UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo a) => a.Id == orderInfo.UserId);
                    CancelIntegral(userMemberInfo, orderInfo.Id, orderInfo.IntegralDiscount);
                }
                context.SaveChanges();
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("AutoCloseOrder:", exception.Message, "/r/n", exception.StackTrace));
            }
        }

        public void AutoConfirmOrder()
        {
            int num;
            try
            {
                SiteSettingsInfo siteSettings = Instance<ISiteSettingService>.Create.GetSiteSettings();
                if (siteSettings == null)
                {
                    num = 7;
                }
                else
                {
                    num = (siteSettings.NoReceivingTimeout == 0 ? 7 : siteSettings.NoReceivingTimeout);
                }
                DateTime dateTime = DateTime.Now.AddDays(-num);
                List<OrderInfo> list = (
                    from a in context.OrderInfo
                    where (a.ShippingDate < dateTime) && (int)a.OrderStatus == 3
                    select a).ToList();
                foreach (OrderInfo nullable in list)
                {
                    nullable.OrderStatus = OrderInfo.OrderOperateStatus.Finish;
                    nullable.CloseReason = "完成过期未确认收货的订单";
                    nullable.FinishDate = new DateTime?(DateTime.Now);
                    UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo a) => a.Id == nullable.UserId);
                    AddIntegral(userMemberInfo, nullable.Id, (nullable.ProductTotalAmount - nullable.DiscountAmount) - nullable.IntegralDiscount);
                }
                context.SaveChanges();
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("AutoConfirmOrder:", exception.Message, "/r/n", exception.StackTrace));
            }
        }

        public void CalculateOrderItemRefund(long orderId, bool isCompel = false)
        {
            OrderInfo order = GetOrder(orderId);
            if (order != null && !isCompel)
            {
                if (order.OrderItemInfo.FirstOrDefault().EnabledRefundAmount.HasValue)
                {
                    decimal? enabledRefundAmount = order.OrderItemInfo.FirstOrDefault().EnabledRefundAmount;
                    if ((enabledRefundAmount.GetValueOrDefault() > new decimal(0) ? true : !enabledRefundAmount.HasValue))
                    {
                        if (isCompel)
                        {
                            order.OrderItemInfo.Count();
                            int num = 0;
                            decimal productTotalAmount = order.ProductTotalAmount;
                            decimal discountAmount = order.DiscountAmount;
                            decimal num1 = productTotalAmount - discountAmount;
                            decimal num2 = new decimal(0);
                            long id = 0;
                            foreach (OrderItemInfo orderItemInfo in order.OrderItemInfo)
                            {
                                decimal realTotalPrice = orderItemInfo.RealTotalPrice;
                                decimal num3 = new decimal(0);
                                if (num != 0)
                                {
                                    num3 = Math.Round(realTotalPrice - ((discountAmount / productTotalAmount) * realTotalPrice), 2);
                                    if (num3 < new decimal(0))
                                    {
                                        num3 = new decimal(0);
                                    }
                                }
                                else
                                {
                                    num3 = new decimal(0);
                                    id = orderItemInfo.Id;
                                }
                                orderItemInfo.EnabledRefundAmount = new decimal?(num3);
                                num2 = num2 + num3;
                                num++;
                            }
                            OrderItemInfo nullable = order.OrderItemInfo.FirstOrDefault((OrderItemInfo d) => d.Id == id);
                            nullable.EnabledRefundAmount = new decimal?(num1 - num2);
                            context.SaveChanges();
                        }
                    }
                }
                isCompel = true;
            }

        }

        private void CancelIntegral(UserMemberInfo member, long orderId, decimal integralDiscount)
        {
            if (integralDiscount == new decimal(0))
            {
                return;
            }
            MemberIntegralExchangeRules integralChangeRule = Instance<IMemberIntegralService>.Create.GetIntegralChangeRule();
            if (integralChangeRule == null)
            {
                return;
            }
            int integralPerMoney = integralChangeRule.IntegralPerMoney;
            int num = Convert.ToInt32(Math.Floor(integralDiscount * integralPerMoney));
            MemberIntegralRecord memberIntegralRecord = new MemberIntegralRecord()
            {
                UserName = member.UserName,
                MemberId = member.Id,
                RecordDate = new DateTime?(DateTime.Now),
                TypeId = MemberIntegral.IntegralType.Cancel,
                ReMark = string.Concat("订单号:", orderId.ToString())
            };
            MemberIntegralRecordAction memberIntegralRecordAction = new MemberIntegralRecordAction()
            {
                VirtualItemTypeId = new MemberIntegral.VirtualItemType?(MemberIntegral.VirtualItemType.Cancel),
                VirtualItemId = orderId
            };
            memberIntegralRecord.ChemCloud_MemberIntegralRecordAction.Add(memberIntegralRecordAction);
            IConversionMemberIntegralBase conversionMemberIntegralBase = Instance<IMemberIntegralConversionFactoryService>.Create.Create(MemberIntegral.IntegralType.Cancel, num);
            Instance<IMemberIntegralService>.Create.AddMemberIntegral(memberIntegralRecord, conversionMemberIntegralBase);
        }

        public void CheckWhenCreateOrder(long userId, IEnumerable<string> skuIds, IEnumerable<int> counts, long recieveAddressId)
        {
            if (userId <= 0)
            {
                throw new InvalidPropertyException("会员Id无效");
            }
            if (skuIds == null || skuIds.Count() == 0)
            {
                throw new InvalidPropertyException("待提交订单的产品不能这空");
            }
            if (counts == null || counts.Count() == 0)
            {
                throw new InvalidPropertyException("待提交订单的产品数量不能这空");
            }
            if (counts.Count((int item) => item <= 0) > 0)
            {
                throw new InvalidPropertyException("待提交订单的产品数量必须都大于0");
            }
            if (skuIds.Count() != counts.Count())
            {
                throw new InvalidPropertyException("产品数量不一致");
            }
            if (recieveAddressId <= 0)
            {
                throw new InvalidPropertyException("收货地址无效");
            }
            IProductService create = Instance<IProductService>.Create;
            ILimitTimeBuyService limitTimeBuyService = Instance<ILimitTimeBuyService>.Create;
            if (skuIds.Count() == 1)
            {
                SKUInfo sku = create.GetSku(skuIds.ElementAt<string>(0));
                LimitTimeMarketInfo limitTimeMarketItemByProductId = limitTimeBuyService.GetLimitTimeMarketItemByProductId(sku.ProductId);
                if (limitTimeMarketItemByProductId != null)
                {
                    int marketSaleCountForUserId = limitTimeBuyService.GetMarketSaleCountForUserId(sku.ProductId, userId);
                    if (limitTimeMarketItemByProductId != null && limitTimeMarketItemByProductId.MaxSaleCount < marketSaleCountForUserId + counts.ElementAt<int>(0))
                    {
                        throw new HimallException("您购买数量超过限时购限定的最大数！");
                    }
                }
            }
            for (int i = 0; i < skuIds.Count(); i++)
            {
                SKUInfo sKUInfo = create.GetSku(skuIds.ElementAt<string>(i));
                if (sKUInfo == null)
                {
                    throw new HimallException(string.Concat("未找到", sKUInfo, "对应的产品"));
                }
                if (sKUInfo.Stock < counts.ElementAt<int>(i))
                {
                    ProductInfo product = create.GetProduct(sKUInfo.ProductId);
                    object[] productName = new object[] { "产品“", product.ProductName, "”库存不够，仅剩", sKUInfo.Stock, "件" };
                    throw new HimallException(string.Concat(productName));
                }
            }
        }

        public void CheckWhenCreateOrder(long userId, IEnumerable<long> cartItemIds, long recieveAddressId)
        {
            if (userId <= 0)
            {
                throw new InvalidPropertyException("会员Id无效");
            }
            if (cartItemIds == null || cartItemIds.Count() == 0)
            {
                throw new InvalidPropertyException("待结算购物车产品不能为空");
            }
            if (recieveAddressId <= 0)
            {
                throw new InvalidPropertyException("收货地址无效");
            }
            IProductService create = Instance<IProductService>.Create;
            IQueryable<ShoppingCartItemInfo> shoppingCartItemInfo =
                from item in context.ShoppingCartItemInfo_
                where cartItemIds.Contains(item.Id)
                select item;
            foreach (ShoppingCartItemInfo list in shoppingCartItemInfo.ToList())
            {
                SKUInfo sku = create.GetSku(list.SkuId);
                if (sku == null)
                {
                    throw new HimallException(string.Concat("未找到", sku, "对应的产品"));
                }
                if (sku.Stock >= list.Quantity)
                {
                    continue;
                }
                ProductInfo product = create.GetProduct(sku.ProductId);
                object[] productName = new object[] { "产品“", product.ProductName, "”库存不够，仅剩", sku.Stock, "件" };
                throw new HimallException(string.Concat(productName));
            }
        }

        public void ConfirmZeroOrder(IEnumerable<long> Ids, long userId)
        {
            IQueryable<OrderInfo> orderInfo =
                from item in context.OrderInfo
                where Ids.Contains(item.Id) && item.UserId == userId && (int)item.OrderStatus == 1
                select item;
            foreach (OrderInfo nullable in orderInfo)
            {
                if (nullable.OrderTotalAmount == new decimal(0))
                {
                    nullable.OrderStatus = OrderInfo.OrderOperateStatus.WaitDelivery;
                }
                nullable.PayDate = new DateTime?(DateTime.Now);
            }
            context.SaveChanges();
        }

        private OrderCreateAdditional CreateAdditional(IEnumerable<OrderService.CartSkuInfo> cartinfos, OrderCreateModel model)
        {
            IEnumerable<BaseAdditionalCoupon> ordersCoupons = GetOrdersCoupons(model.CurrentUser.Id, model.CouponIdsStr);
            decimal integralDiscountAmount = GetIntegralDiscountAmount(model.Integral, model.CurrentUser.Id);
            ShippingAddressInfo userShippingAddress = Instance<IShippingAddressService>.Create.GetUserShippingAddress(model.ReceiveAddressId);
            OrderCreateAdditional orderCreateAdditional = new OrderCreateAdditional()
            {
                BaseCoupons = ordersCoupons,
                Address = userShippingAddress,
                IntegralTotal = integralDiscountAmount,
                CreateDate = DateTime.Now
            };
            return orderCreateAdditional;
        }

        private IEnumerable<OrderService.CartSkuInfo> CreateCart(OrderCreateModel model)
        {
            IEnumerable<OrderService.CartSkuInfo> cartInfo;
            if (model.CartItemIds == null || model.CartItemIds.Count() <= 0)
            {
                CheckWhenCreateOrder(model.CurrentUser.Id, model.SkuIds, model.Counts, model.ReceiveAddressId);
                cartInfo = GetCartInfo(model.SkuIds, model.Counts, model.CollPids);
            }
            else
            {
                CheckWhenCreateOrder(model.CurrentUser.Id, model.CartItemIds, model.ReceiveAddressId);
                cartInfo = GetCartInfo(model.CartItemIds);
            }
            return cartInfo;
        }

        public List<OrderInfo> CreateOrder(OrderCreateModel model)
        {
            IEnumerable<OrderService.CartSkuInfo> cartSkuInfos = CreateCart(model);
            OrderCreateAdditional orderCreateAdditional = CreateAdditional(cartSkuInfos, model);
            IEnumerable<IGrouping<long, OrderService.CartSkuInfo>> shopId =
                from item in cartSkuInfos
                group item by item.Product.ShopId;
            List<OrderInfo> orderInfos = new List<OrderInfo>();
            foreach (IGrouping<long, OrderService.CartSkuInfo> nums in shopId)
            {
                orderInfos.Add(CreateOrderInfo(nums, model, orderCreateAdditional));
            }
            decimal num = orderInfos.Sum<OrderInfo>((OrderInfo a) => a.ProductTotalAmount - a.DiscountAmount);
            decimal num1 = orderInfos.Sum<OrderInfo>((OrderInfo a) => a.ProductTotalAmount);
            if (orderCreateAdditional.IntegralTotal > num)
            {
                throw new HimallException("积分抵扣金额不能超过产品总金额！");
            }
            int count = orderInfos.Count;
            decimal integralDiscount = new decimal(0);
            for (int i = 0; i < orderInfos.Count; i++)
            {
                OrderInfo orderInfo = orderInfos[i];
                if (i >= count - 1)
                {
                    orderInfos[i].IntegralDiscount = orderCreateAdditional.IntegralTotal - integralDiscount;
                }
                else
                {
                    orderInfos[i].IntegralDiscount = GetShopIntegralDiscount(orderInfos[i].ProductTotalAmount, num1, orderCreateAdditional.IntegralTotal);
                    integralDiscount = integralDiscount + orderInfos[i].IntegralDiscount;
                }
            }
            long[] array = (
                from item in orderInfos
                select item.Id).ToArray();
            using (TransactionScope transactionScope = new TransactionScope())
            {
                ICartService create = Instance<ICartService>.Create;
                context.OrderInfo.AddRange(orderInfos);
                string[] strArrays = (
                    from a in cartSkuInfos
                    select a.SKU.Id).ToArray();
                create.DeleteCartItem(strArrays, model.CurrentUser.Id);
                context.SaveChanges();
                ICouponService couponService = Instance<ICouponService>.Create;
                if (model.CouponIds != null && model.CouponIds.Count() > 0)
                {
                    UseCoupon(orderInfos.ToList(), orderCreateAdditional.BaseCoupons.ToList(), model.CurrentUser.Id);
                }
                IProductService productService = Instance<IProductService>.Create;
                foreach (OrderService.CartSkuInfo cartSkuInfo in cartSkuInfos)
                {
                    productService.UpdateStock(cartSkuInfo.SKU.Id, -1 * cartSkuInfo.Quantity);
                }
                DeductionIntegral(model.CurrentUser, array, model.Integral);
                transactionScope.Complete();
            }
            SendMessage(orderInfos);
            return orderInfos;
        }

        private OrderInfo CreateOrderInfo(IGrouping<long, OrderService.CartSkuInfo> groupItem, OrderCreateModel model, OrderCreateAdditional additional)
        {
            int cityId = 0;
            if (additional.Address != null)
            {
                cityId = Instance<IRegionService>.Create.GetCityId(additional.Address.RegionIdPath);
            }
            UserMemberInfo currentUser = model.CurrentUser;
            ShopInfo shop = Instance<IShopService>.Create.GetShop(groupItem.Key, false);
            IEnumerable<long> list =
                from item in groupItem.ToList<OrderService.CartSkuInfo>()
                select item.Product.Id;
            decimal num = groupItem.Sum<OrderService.CartSkuInfo>((OrderService.CartSkuInfo item) => GetSalePrice(item.Product.Id, item.SKU, new long?(item.ColloPid), list.Count()) * item.Quantity);
            IEnumerable<int> nums =
                from item in groupItem.ToList<OrderService.CartSkuInfo>()
                select (int)item.Quantity;
            decimal freight = Instance<IProductService>.Create.GetFreight(list, nums, cityId);
            OrderInfo orderInfo = new OrderInfo()
            {
                Id = _orderBO.GenerateOrderNumber(),
                ShopId = groupItem.Key,
                ShopName = shop.ShopName,
                UserId = currentUser.Id,
                UserName = currentUser.UserName,
                OrderDate = additional.CreateDate,
                RegionId = additional.Address.RegionId,
                ShipTo = additional.Address.ShipTo,
                Address = additional.Address.Address,
                RegionFullName = additional.Address.RegionFullName,
                CellPhone = additional.Address.Phone
            };
            string regionIdPath = additional.Address.RegionIdPath;
            char[] chrArray = new char[] { ',' };
            orderInfo.TopRegionId = int.Parse(regionIdPath.Split(chrArray)[0]);
            orderInfo.OrderStatus = OrderInfo.OrderOperateStatus.WaitPay;
            orderInfo.Freight = freight;
            orderInfo.IsPrinted = false;
            orderInfo.ProductTotalAmount = num;
            orderInfo.RefundTotalAmount = new decimal(0);
            orderInfo.CommisTotalAmount = new decimal(0);
            orderInfo.RefundCommisAmount = new decimal(0);
            orderInfo.Platform = model.PlatformType;
            orderInfo.InvoiceType = model.Invoice;
            orderInfo.InvoiceTitle = model.InvoiceTitle;
            orderInfo.InvoiceContext = model.InvoiceContext;
            orderInfo.DiscountAmount = GetShopCouponDiscount(additional.BaseCoupons, groupItem.Key);
            OrderInfo nullable = orderInfo;
            if (model.CollPids != null && model.CollPids.Count() > 1)
            {
                nullable.OrderType = new OrderInfo.OrderTypes?(OrderInfo.OrderTypes.Collocation);
            }
            if (model.IslimitBuy)
            {
                nullable.OrderType = new OrderInfo.OrderTypes?(OrderInfo.OrderTypes.LimitBuy);
            }
            if (_orderBO.IsFullFreeFreight(shop, nullable.ProductTotalAmount - nullable.DiscountAmount))
            {
                nullable.Freight = new decimal(0);
            }
            int num1 = list.Count();
            foreach (OrderService.CartSkuInfo cartSkuInfo in groupItem)
            {
                if (cartSkuInfo.Product.SaleStatus != ProductInfo.ProductSaleStatus.OnSale || cartSkuInfo.Product.AuditStatus != ProductInfo.ProductAuditStatus.Audited)
                {
                    throw new HimallException("订单中有失效的产品，请返回重新提交！");
                }
                OrderItemInfo orderItemInfo = new OrderItemInfo()
                {
                    OrderId = nullable.Id,
                    ShopId = nullable.ShopId,
                    ProductId = cartSkuInfo.Product.Id,
                    SkuId = cartSkuInfo.SKU.Id,
                    Quantity = cartSkuInfo.Quantity,
                    SKU = cartSkuInfo.SKU.Sku,
                    ReturnQuantity = 0,
                    CostPrice = cartSkuInfo.SKU.CostPrice,
                    SalePrice = GetSalePrice(cartSkuInfo.Product.Id, cartSkuInfo.SKU, new long?(cartSkuInfo.ColloPid), num1),
                    IsLimitBuy = model.IslimitBuy,
                    DiscountAmount = new decimal(0),

                    RefundPrice = new decimal(0),
                    CommisRate = GetCommisRate(cartSkuInfo.Product.CategoryId, cartSkuInfo.Product.ShopId) / new decimal(100),
                    Color = cartSkuInfo.SKU.Color,
                    Size = cartSkuInfo.SKU.Size,
                    Version = cartSkuInfo.SKU.Version,
                    ProductName = cartSkuInfo.Product.ProductName,
                    ThumbnailsUrl = cartSkuInfo.Product.GetImage(ProductInfo.ImageSize.Size_100, 1)
                };
                orderItemInfo.RealTotalPrice = orderItemInfo.SalePrice * cartSkuInfo.Quantity;
                nullable.OrderItemInfo.Add(orderItemInfo);
            }
            return nullable;
        }

        private void DeductionIntegral(UserMemberInfo member, IEnumerable<long> Ids, int integral)
        {
            if (integral == 0)
            {
                return;
            }
            MemberIntegralRecord memberIntegralRecord = new MemberIntegralRecord()
            {
                UserName = member.UserName,
                MemberId = member.Id,
                RecordDate = new DateTime?(DateTime.Now)
            };
            string str = "订单号:";
            memberIntegralRecord.TypeId = MemberIntegral.IntegralType.Exchange;
            foreach (long id in Ids)
            {
                str = string.Concat(str, id, ",");
                MemberIntegralRecordAction memberIntegralRecordAction = new MemberIntegralRecordAction()
                {
                    VirtualItemTypeId = new MemberIntegral.VirtualItemType?(MemberIntegral.VirtualItemType.Exchange),
                    VirtualItemId = id
                };
                memberIntegralRecord.ChemCloud_MemberIntegralRecordAction.Add(memberIntegralRecordAction);
            }
            char[] chrArray = new char[] { ',' };
            memberIntegralRecord.ReMark = str.TrimEnd(chrArray);
            IConversionMemberIntegralBase conversionMemberIntegralBase = Instance<IMemberIntegralConversionFactoryService>.Create.Create(MemberIntegral.IntegralType.Exchange, integral);
            Instance<IMemberIntegralService>.Create.AddMemberIntegral(memberIntegralRecord, conversionMemberIntegralBase);
        }

        public void DeleteInvoiceContext(long id)
        {
            context.InvoiceContextInfo.Remove(new object[] { id });
            context.SaveChanges();
        }

        public void DeleteInvoiceTitle(long id)
        {
            context.InvoiceTitleInfo.Remove(new object[] { id });
            context.SaveChanges();
        }

        private IEnumerable<OrderService.CartSkuInfo> GetCartInfo(IEnumerable<string> skuIds, IEnumerable<int> counts, IEnumerable<long> colloPids = null)
        {
            ICartService create = Instance<ICartService>.Create;
            IProductService productService = Instance<IProductService>.Create;
            int num = -1;
            List<OrderService.CartSkuInfo> list = skuIds.Select<string, OrderService.CartSkuInfo>((string skuId) =>
            {
                SKUInfo sku = productService.GetSku(skuId);
                ProductInfo product = productService.GetProduct(sku.ProductId);
                num++;
                return new OrderService.CartSkuInfo()
                {
                    SKU = sku,
                    Product = product,
                    Quantity = counts.ElementAt<int>(num),
                    ColloPid = (colloPids == null ? 0 : colloPids.ElementAt<long>(num))
                };
            }).ToList<OrderService.CartSkuInfo>();
            return list;
        }

        private IEnumerable<OrderService.CartSkuInfo> GetCartInfo(IEnumerable<long> cartItemIds)
        {
            List<ShoppingCartItem> list = Instance<ICartService>.Create.GetCartItems(cartItemIds).ToList();
            IProductService create = Instance<IProductService>.Create;
            return
                from a in list
                select new OrderService.CartSkuInfo()
                {
                    SKU = create.GetSku(a.SkuId),
                    Product = create.GetProduct(a.ProductId),
                    Quantity = a.Quantity
                };
        }

        private decimal GetCommisRate(long categoryId, long shopId)
        {
            decimal num = (
                from b in context.BusinessCategoryInfo
                where b.CategoryId == categoryId && b.ShopId == shopId
                select b into a
                select a.CommisRate).FirstOrDefault();
            return num;
        }

        public OrderInfo GetFirstFinishedOrderForSettlement()
        {
            OrderInfo orderInfo = (
                from c in context.OrderInfo
                where (int)c.OrderStatus == 5
                orderby c.FinishDate
                select c).FirstOrDefault();
            return orderInfo;
        }

        private decimal GetIntegralDiscountAmount(int integral, long userId)
        {
            if (integral == 0)
            {
                return new decimal(0);
            }
            IMemberIntegralService create = Instance<IMemberIntegralService>.Create;
            MemberIntegral memberIntegral = create.GetMemberIntegral(userId);
            if ((memberIntegral == null ? 0 : memberIntegral.AvailableIntegrals) < integral)
            {
                throw new HimallException("用户积分不足不能抵扣订单");
            }
            MemberIntegralExchangeRules integralChangeRule = create.GetIntegralChangeRule();
            int num = (integralChangeRule == null ? 0 : integralChangeRule.IntegralPerMoney);
            if (num == 0)
            {
                return new decimal(0);
            }
            return Math.Round((decimal)integral / num, 2, MidpointRounding.AwayFromZero);
        }

        public List<InvoiceContextInfo> GetInvoiceContexts()
        {
            return context.InvoiceContextInfo.ToList();
        }

        public List<InvoiceTitleInfo> GetInvoiceTitles(long userid)
        {
            List<InvoiceTitleInfo> list = (
                from p in context.InvoiceTitleInfo
                where p.UserId == userid
                select p).ToList();
            if (list == null)
            {
                return new List<InvoiceTitleInfo>();
            }
            return list;
        }

        public OrderInfo GetOrder(long orderId, long userId)
        {
            OrderInfo _OrderInfo = (
                from a in context.OrderInfo
                where a.Id == orderId
                select a).FirstOrDefault();
            ChemCloud_Dictionaries _ChemCloud_Dictionaries = context.ChemCloud_Dictionaries.Where(q => q.DictionaryTypeId.Equals(1) && q.DValue == _OrderInfo.CoinType.ToString()).FirstOrDefault();
            _OrderInfo.CoinTypeName = _ChemCloud_Dictionaries == null ? "" : _ChemCloud_Dictionaries.DKey;

            return _OrderInfo;
        }

        public OrderInfo GetOrder(long orderId)
        {
            OrderInfo _OrderInfo = context.OrderInfo.FindById<OrderInfo>(orderId);
            if (_OrderInfo != null)
            {
                _OrderInfo.CoinTypeName = "CNY";
                ProductService _ProductService = new ProductService();
                if (_OrderInfo.OrderItemInfo.Count > 0)
                {
                    foreach (OrderItemInfo item in _OrderInfo.OrderItemInfo)
                    {
                        string casno = "";
                        string imgpath = "";
                        ProductInfo _ProductInfo = _ProductService.GetProduct(item.ProductId);
                        if (_ProductInfo != null)
                        {
                            if (!string.IsNullOrWhiteSpace(_ProductInfo.CASNo))
                            {
                                casno = _ProductInfo.CASNo;
                            }

                            if (!string.IsNullOrWhiteSpace(_ProductInfo.ImagePath))
                            {
                                imgpath = _ProductInfo.ImagePath;
                            }
                        }
                        item.CASNo = casno;

                        item.ThumbnailsUrl = imgpath;
                    }
                }
                return _OrderInfo;
            }
            else
            {
                return null;
            }
        }

        public OrderItemInfo GetOrderItem(long orderItemId)
        {
            return context.OrderItemInfo.FindById<OrderItemInfo>(orderItemId);
        }

        public OrderItemInfo GetOrderItemInfo(long orderId)
        {
            OrderItemInfo info = new OrderItemInfo();
            info = (
            from p in context.OrderItemInfo
            where p.OrderId.Equals(orderId)
            select p).FirstOrDefault();
            return info;
        }

        public Dictionary<long, OrderItemInfo> GetOrderItems(IEnumerable<long> ids)
        {
            IQueryable<OrderInfo> orderInfo =
                from item in context.OrderInfo
                where ids.Contains(item.Id)
                select item;
            Dictionary<long, OrderItemInfo> nums = new Dictionary<long, OrderItemInfo>();
            foreach (OrderInfo list in orderInfo.ToList())
            {
                foreach (OrderItemInfo orderItemInfo in list.OrderItemInfo.ToList())
                {
                    if (!nums.Keys.Contains(orderItemInfo.Id))
                    {
                        nums.Add(orderItemInfo.Id, orderItemInfo);
                    }
                    else
                    {
                        OrderItemInfo quantity = nums[orderItemInfo.Id];
                        quantity.Quantity = quantity.Quantity + orderItemInfo.Quantity;
                    }
                }
            }
            return nums;
        }

        public IQueryable<OrderPayInfo> GetOrderPay(long id)
        {
            return
                from item in context.OrderPayInfo
                where item.PayId == id
                select item;
        }

        public PageModel<OrderInfo> GetOrdersAdv<Tout>(OrderQuery orderQuery, Expression<Func<OrderInfo, Tout>> sort = null)
        {
            long num;
            bool flag = long.TryParse(orderQuery.SearchKeyWords, out num);
            List<long> nums = new List<long>();

            IQueryable<OrderInfo> platform = null;

            if (orderQuery.UserId != null)
            {
                //1.判断orderQuery.UserId 是否为管理员
                MemberService _MemberService = new MemberService();
                UserMemberInfo _UserMemberInfo = _MemberService.GetMember((long.Parse(orderQuery.UserId.ToString())));
                if (_UserMemberInfo != null)
                {

                    string[] arrays = _UserMemberInfo.UserName.Split(':');
                    if (!string.IsNullOrEmpty(arrays[0]))
                    {
                        orderQuery.UserName = arrays[0];
                    }
                }

                platform = context.OrderInfo.FindBy(
                (OrderInfo item) => (!orderQuery.OrderId.HasValue || orderQuery.OrderId == item.Id)
                    && (!orderQuery.ShopId.HasValue || orderQuery.ShopId == item.ShopId)
                    && (orderQuery.ShopName == null || (orderQuery.ShopName.Trim() == "") || item.ShopName.Contains(orderQuery.ShopName))
                    && (item.UserName.Contains(orderQuery.UserName))
                    && (orderQuery.PaymentTypeName == null || (orderQuery.PaymentTypeName.Trim() == "") || item.PaymentTypeName.Contains(orderQuery.PaymentTypeName))
                    && (orderQuery.PaymentTypeGateway == null || (orderQuery.PaymentTypeGateway.Trim() == "") || item.PaymentTypeGateway.Contains(orderQuery.PaymentTypeGateway))
                    && (orderQuery.SearchKeyWords == null || (orderQuery.SearchKeyWords.Trim() == "") || flag && item.Id == num || item.OrderItemInfo.Any((OrderItemInfo a) => a.ProductName.Contains(orderQuery.SearchKeyWords)) || flag
                    && item.OrderItemInfo.Any((OrderItemInfo a) => a.ProductId == num))
                    && (!orderQuery.Commented.HasValue || (orderQuery.Commented.Value ? item.OrderCommentInfo.Count > 0 : item.OrderCommentInfo.Count == 0)));

            }
            else
            {
                platform = context.OrderInfo.FindBy(
                  (OrderInfo item) => (!orderQuery.OrderId.HasValue || orderQuery.OrderId == item.Id)
                  && (!orderQuery.ShopId.HasValue || orderQuery.ShopId == item.ShopId)
                  && (orderQuery.ShopName == null || (orderQuery.ShopName.Trim() == "") || item.ShopName.Contains(orderQuery.ShopName))
                  && (orderQuery.UserName == null || (orderQuery.UserName.Trim() == "") || item.UserName.Contains(orderQuery.UserName))
                  && (!orderQuery.UserId.HasValue || item.UserId == orderQuery.UserId)
                  && (!orderQuery.UserId.HasValue || nums.Contains(item.UserId))
                  && (orderQuery.PaymentTypeName == null || (orderQuery.PaymentTypeName.Trim() == "") || item.PaymentTypeName.Contains(orderQuery.PaymentTypeName))
                  && (orderQuery.PaymentTypeGateway == null || (orderQuery.PaymentTypeGateway.Trim() == "") || item.PaymentTypeGateway.Contains(orderQuery.PaymentTypeGateway))
                  && (orderQuery.SearchKeyWords == null || (orderQuery.SearchKeyWords.Trim() == "") || flag && item.Id == num || item.OrderItemInfo.Any((OrderItemInfo a) => a.ProductName.Contains(orderQuery.SearchKeyWords)) || flag
                  && item.OrderItemInfo.Any((OrderItemInfo a) => a.ProductId == num))
                  && (!orderQuery.Commented.HasValue || (orderQuery.Commented.Value ? item.OrderCommentInfo.Count > 0 : item.OrderCommentInfo.Count == 0)));
            }


            int num1 = 0;
            if (orderQuery.Commented.HasValue && !orderQuery.Commented.Value)
            {
                IQueryable<long> productCommentInfo =
                    from p in context.ProductCommentInfo
                    select p.ChemCloud_OrderItems.OrderId;
                platform =
                    from item in platform
                    where !productCommentInfo.Contains(item.Id)
                    select item;
            }
            if (orderQuery.OrderType.HasValue)
            {
                platform =
                    from item in platform
                    where (int)item.Platform == orderQuery.OrderType
                    select item;
            }
            if (orderQuery.Status.HasValue)
            {
                Expression<Func<OrderInfo, bool>> defaultPredicate = platform.GetDefaultPredicate<OrderInfo>(false);
                defaultPredicate = defaultPredicate.Or<OrderInfo>((OrderInfo d) => (int?)d.OrderStatus == (int?)orderQuery.Status);
                if (orderQuery.MoreStatus != null)
                {
                    foreach (OrderInfo.OrderOperateStatus moreStatu in orderQuery.MoreStatus)
                    {
                        defaultPredicate = defaultPredicate.Or<OrderInfo>((OrderInfo d) => (int)d.OrderStatus == (int)moreStatu);
                    }
                }
                platform = platform.FindBy(defaultPredicate);
            }
            if (orderQuery.StartDate.HasValue)
            {
                DateTime value = orderQuery.StartDate.Value;
                platform =
                    from d in platform
                    where d.OrderDate >= value
                    select d;
            }
            if (orderQuery.EndDate.HasValue)
            {
                DateTime dateTime = orderQuery.EndDate.Value;
                DateTime dateTime1 = dateTime.AddDays(1).AddSeconds(-1);
                platform =
                    from d in platform
                    where d.OrderDate <= dateTime1
                    select d;
            }

            //根据cas号查询
            if (!string.IsNullOrWhiteSpace(orderQuery.CASNo))
            {

                List<ProductInfo> ProductInfoList = context.ProductInfo.Where(q => q.CASNo.Contains(orderQuery.CASNo)).ToList();

                List<OrderItemInfo> OrderItemInfoList = context.OrderItemInfo.ToList();

                List<OrderItemInfo> qlist = (from q in OrderItemInfoList
                                             where (from q2 in ProductInfoList select q2.Id).Contains(q.ProductId)
                                             select q).ToList();
                List<long> listcas = new List<long>();

                foreach (var item in qlist)
                {
                    listcas.Add(item.OrderId);
                }

                if (OrderItemInfoList != null)
                {
                    platform =
                   from item in platform
                   where (from p in listcas select p).Contains(item.Id)
                   select item;
                }
            }
            //查询平台代发
            platform = platform.Where(x => x.IsBehalfShip == "1");
            if (!string.IsNullOrEmpty(orderQuery.ProcessStatus))
            {
                switch (orderQuery.ProcessStatus)
                {
                    case "1":
                        platform = platform.Where(x => x.BehalfShipNumber == null);
                        break;
                    case "2":
                        platform = platform.Where(x => x.BehalfShipNumber != null);
                        break;
                }
            }


            platform = (sort != null ? platform.FindBy<OrderInfo, Tout>((OrderInfo item) => item.Id > 0, orderQuery.PageNo, orderQuery.PageSize, out num1, sort, true) : platform.FindBy<OrderInfo, DateTime>((OrderInfo item) => item.Id > 0, orderQuery.PageNo, orderQuery.PageSize, out num1, (OrderInfo item) => item.OrderDate, false));
            foreach (OrderInfo info in platform.ToList())
            {
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(info.ShopId));
                info.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
                ChemCloud_Dictionaries dictionaryInfo = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 1 && m.DValue == info.CoinType.ToString());
                info.CoinTypeName = (dictionaryInfo == null ? "" : dictionaryInfo.DKey);
                foreach (OrderItemInfo item in info.OrderItemInfo)
                {
                    if (item != null)
                    {
                        if (context.ProductInfo.Where(q => q.Id == item.ProductId).FirstOrDefault() != null)
                        {
                            item.CASNo = context.ProductInfo.Where(q => q.Id == item.ProductId).FirstOrDefault().CASNo;
                        }

                    }
                }
            }

            platform = from p in platform orderby p.OrderDate descending, p.UserId descending, p.ShopId descending select p;

            return new PageModel<OrderInfo>()
            {
                Models = platform,
                Total = num1
            };
        }

        public PageModel<OrderInfo> GetOrders<Tout>(OrderQuery orderQuery, Expression<Func<OrderInfo, Tout>> sort = null)
        {
            long num;
            bool flag = long.TryParse(orderQuery.SearchKeyWords, out num);
            List<long> nums = new List<long>();

            IQueryable<OrderInfo> platform = null;

            if (orderQuery.UserId != null)
            {
                //1.判断orderQuery.UserId 是否为管理员
                MemberService _MemberService = new MemberService();
                UserMemberInfo _UserMemberInfo = _MemberService.GetMember((long.Parse(orderQuery.UserId.ToString())));
                if (_UserMemberInfo != null)
                {
                    //if (_UserMemberInfo.ParentSellerId == 0)
                    //{
                    //    orderQuery.UserName = _UserMemberInfo.UserName;
                    //    nums.Add(_UserMemberInfo.Id);
                    //    List<UserMemberInfo> list = _MemberService.GetMemberList(_UserMemberInfo.Id);
                    //    foreach (var item in list)
                    //    {
                    //        if (item.Id != 0)
                    //        {
                    //            nums.Add(item.Id);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (_MemberService.GetMember(_UserMemberInfo.ParentSellerId) != null)
                    //    {
                    //        orderQuery.UserName = _UserMemberInfo.UserName;
                    //        nums.Add(_MemberService.GetMember(_UserMemberInfo.ParentSellerId).Id);
                    //    }
                    //    List<UserMemberInfo> list = _MemberService.GetMemberList(_UserMemberInfo.Id);
                    //    if (list.Count > 0)
                    //    {
                    //        foreach (var item in list)
                    //        {
                    //            if (item.Id != 0)
                    //            {
                    //                nums.Add(item.Id);
                    //            }
                    //        }
                    //    }
                    //}

                    string[] arrays = _UserMemberInfo.UserName.Split(':');
                    if (!string.IsNullOrEmpty(arrays[0]))
                    {
                        orderQuery.UserName = arrays[0];
                    }
                }

                platform = context.OrderInfo.FindBy(
                (OrderInfo item) => (!orderQuery.OrderId.HasValue || orderQuery.OrderId == item.Id)
                    && (!orderQuery.ShopId.HasValue || orderQuery.ShopId == item.ShopId)
                    && (orderQuery.ShopName == null || (orderQuery.ShopName.Trim() == "") || item.ShopName.Contains(orderQuery.ShopName))
                    && (item.UserName.Contains(orderQuery.UserName))
                    && (orderQuery.PaymentTypeName == null || (orderQuery.PaymentTypeName.Trim() == "") || item.PaymentTypeName.Contains(orderQuery.PaymentTypeName))
                    && (orderQuery.PaymentTypeGateway == null || (orderQuery.PaymentTypeGateway.Trim() == "") || item.PaymentTypeGateway.Contains(orderQuery.PaymentTypeGateway))
                    && (orderQuery.SearchKeyWords == null || (orderQuery.SearchKeyWords.Trim() == "") || flag && item.Id == num || item.OrderItemInfo.Any((OrderItemInfo a) => a.ProductName.Contains(orderQuery.SearchKeyWords)) || flag
                    && item.OrderItemInfo.Any((OrderItemInfo a) => a.ProductId == num))
                    && (!orderQuery.Commented.HasValue || (orderQuery.Commented.Value ? item.OrderCommentInfo.Count > 0 : item.OrderCommentInfo.Count == 0)));

            }
            else
            {
                platform = context.OrderInfo.FindBy(
                  (OrderInfo item) => (!orderQuery.OrderId.HasValue || orderQuery.OrderId == item.Id)
                  && (!orderQuery.ShopId.HasValue || orderQuery.ShopId == item.ShopId)
                  && (orderQuery.ShopName == null || (orderQuery.ShopName.Trim() == "") || item.ShopName.Contains(orderQuery.ShopName))
                  && (orderQuery.UserName == null || (orderQuery.UserName.Trim() == "") || item.UserName.Contains(orderQuery.UserName))
                  && (!orderQuery.UserId.HasValue || item.UserId == orderQuery.UserId)
                  && (!orderQuery.UserId.HasValue || nums.Contains(item.UserId))
                  && (orderQuery.PaymentTypeName == null || (orderQuery.PaymentTypeName.Trim() == "") || item.PaymentTypeName.Contains(orderQuery.PaymentTypeName))
                  && (orderQuery.PaymentTypeGateway == null || (orderQuery.PaymentTypeGateway.Trim() == "") || item.PaymentTypeGateway.Contains(orderQuery.PaymentTypeGateway))
                  && (orderQuery.SearchKeyWords == null || (orderQuery.SearchKeyWords.Trim() == "") || flag && item.Id == num || item.OrderItemInfo.Any((OrderItemInfo a) => a.ProductName.Contains(orderQuery.SearchKeyWords)) || flag
                  && item.OrderItemInfo.Any((OrderItemInfo a) => a.ProductId == num))
                  && (!orderQuery.Commented.HasValue || (orderQuery.Commented.Value ? item.OrderCommentInfo.Count > 0 : item.OrderCommentInfo.Count == 0)));
            }


            int num1 = 0;
            if (orderQuery.Commented.HasValue && !orderQuery.Commented.Value)
            {
                IQueryable<long> productCommentInfo =
                    from p in context.ProductCommentInfo
                    select p.ChemCloud_OrderItems.OrderId;
                platform =
                    from item in platform
                    where !productCommentInfo.Contains(item.Id)
                    select item;
            }
            if (orderQuery.OrderType.HasValue)
            {
                platform =
                    from item in platform
                    where (int)item.Platform == orderQuery.OrderType
                    select item;
            }
            if (orderQuery.Status.HasValue)
            {
                Expression<Func<OrderInfo, bool>> defaultPredicate = platform.GetDefaultPredicate<OrderInfo>(false);
                defaultPredicate = defaultPredicate.Or<OrderInfo>((OrderInfo d) => (int?)d.OrderStatus == (int?)orderQuery.Status);
                if (orderQuery.MoreStatus != null)
                {
                    foreach (OrderInfo.OrderOperateStatus moreStatu in orderQuery.MoreStatus)
                    {
                        defaultPredicate = defaultPredicate.Or<OrderInfo>((OrderInfo d) => (int)d.OrderStatus == (int)moreStatu);
                    }
                }
                platform = platform.FindBy(defaultPredicate);
            }
            if (orderQuery.StartDate.HasValue)
            {
                DateTime value = orderQuery.StartDate.Value;
                platform =
                    from d in platform
                    where d.OrderDate >= value
                    select d;
            }
            if (orderQuery.EndDate.HasValue)
            {
                DateTime dateTime = orderQuery.EndDate.Value;
                DateTime dateTime1 = dateTime.AddDays(1).AddSeconds(-1);
                platform =
                    from d in platform
                    where d.OrderDate <= dateTime1
                    select d;
            }

            //根据cas号查询
            if (!string.IsNullOrWhiteSpace(orderQuery.CASNo))
            {

                List<ProductInfo> ProductInfoList = context.ProductInfo.Where(q => q.CASNo.Contains(orderQuery.CASNo)).ToList();

                List<OrderItemInfo> OrderItemInfoList = context.OrderItemInfo.ToList();

                List<OrderItemInfo> qlist = (from q in OrderItemInfoList
                                             where (from q2 in ProductInfoList select q2.Id).Contains(q.ProductId)
                                             select q).ToList();
                List<long> listcas = new List<long>();

                foreach (var item in qlist)
                {
                    listcas.Add(item.OrderId);
                }

                if (OrderItemInfoList != null)
                {
                    platform =
                   from item in platform
                   where (from p in listcas select p).Contains(item.Id)
                   select item;
                }
            }

            platform = (sort != null ? platform.FindBy<OrderInfo, Tout>((OrderInfo item) => item.Id > 0, orderQuery.PageNo, orderQuery.PageSize, out num1, sort, true) : platform.FindBy<OrderInfo, DateTime>((OrderInfo item) => item.Id > 0, orderQuery.PageNo, orderQuery.PageSize, out num1, (OrderInfo item) => item.OrderDate, false));
            foreach (OrderInfo info in platform.ToList())
            {
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(info.ShopId));
                info.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
                ChemCloud_Dictionaries dictionaryInfo = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 1 && m.DValue == info.CoinType.ToString());
                info.CoinTypeName = (dictionaryInfo == null ? "" : dictionaryInfo.DKey);
                foreach (OrderItemInfo item in info.OrderItemInfo)
                {
                    if (item != null)
                    {
                        if (context.ProductInfo.Where(q => q.Id == item.ProductId).FirstOrDefault() != null)
                        {
                            item.CASNo = context.ProductInfo.Where(q => q.Id == item.ProductId).FirstOrDefault().CASNo;
                        }

                    }
                }
            }

            platform = from p in platform orderby p.OrderDate descending, p.UserId descending, p.ShopId descending select p;

            return new PageModel<OrderInfo>()
            {
                Models = platform,
                Total = num1
            };
        }

        public IEnumerable<OrderInfo> GetOrders(IEnumerable<long> ids)
        {
            return
                from item in context.OrderInfo
                where ids.Contains(item.Id)
                select item;
        }

        private IEnumerable<CouponRecordInfo> GetOrdersCoupons(long userId, IEnumerable<long> couponIds)
        {
            if (couponIds == null || couponIds.Count() <= 0)
            {
                return null;
            }
            return Instance<ICouponService>.Create.GetOrderCoupons(userId, couponIds);
        }

        private IEnumerable<BaseAdditionalCoupon> GetOrdersCoupons(long userId, IEnumerable<string[]> couponIdsStr)
        {
            BaseAdditionalCoupon baseAdditionalCoupon;
            ICouponService create = Instance<ICouponService>.Create;
            IShopBonusService shopBonusService = Instance<IShopBonusService>.Create;
            if (couponIdsStr == null || couponIdsStr.Count<string[]>() <= 0)
            {
                return null;
            }
            List<BaseAdditionalCoupon> baseAdditionalCoupons = new List<BaseAdditionalCoupon>();
            foreach (string[] strArrays in couponIdsStr)
            {
                if (int.Parse(strArrays[1]) == 0)
                {
                    long[] numArray = new long[] { long.Parse(strArrays[0]) };
                    CouponRecordInfo couponRecordInfo = create.GetOrderCoupons(userId, numArray).FirstOrDefault();
                    baseAdditionalCoupon = new BaseAdditionalCoupon()
                    {
                        Type = 0,
                        Coupon = couponRecordInfo,
                        ShopId = couponRecordInfo.ShopId
                    };
                }
                else if (int.Parse(strArrays[1]) != 1)
                {
                    baseAdditionalCoupon = new BaseAdditionalCoupon()
                    {
                        Type = 99
                    };
                }
                else
                {
                    ShopBonusReceiveInfo detailById = shopBonusService.GetDetailById(userId, long.Parse(strArrays[0]));
                    baseAdditionalCoupon = new BaseAdditionalCoupon()
                    {
                        Type = 1,
                        Coupon = detailById,
                        ShopId = detailById.ChemCloud_ShopBonusGrant.ChemCloud_ShopBonus.ShopId
                    };
                }
                baseAdditionalCoupons.Add(baseAdditionalCoupon);
            }
            return baseAdditionalCoupons;
        }

        public decimal GetRecentMonthAveragePrice(long shopId, long productId)
        {
            decimal num = new decimal(0);
            DateTime dateTime = DateTime.Now.AddMonths(-1);
            IQueryable<OrderItemInfo> orderItemInfos = (
                from o in context.OrderInfo
                join oi in context.OrderItemInfo on o.Id equals oi.OrderId
                where o.ShopId == shopId && (int)o.OrderStatus == 5 && (o.PayDate >= dateTime) && (o.PayDate <= DateTime.Now) && oi.ProductId == productId
                select oi).Take(30);
            num = (orderItemInfos.Count() != 0 ? orderItemInfos.Average<OrderItemInfo>((OrderItemInfo s) => s.RealTotalPrice - s.DiscountAmount) : context.ProductInfo.FindById<ProductInfo>(productId).MinSalePrice);
            return num;
        }

        private decimal GetSalePrice(long productId, SKUInfo sku, long? collid, int SkuCount)
        {
            decimal salePrice = sku.SalePrice;
            if (collid.HasValue && collid.Value != 0 && SkuCount > 1)
            {
                CollocationSkuInfo colloSku = Instance<ICollocationService>.Create.GetColloSku(collid.Value, sku.Id);
                if (colloSku != null)
                {
                    salePrice = colloSku.Price;
                }
            }
            else if (SkuCount == 1)
            {
                LimitTimeMarketInfo limitTimeMarketItemByProductId = Instance<ILimitTimeBuyService>.Create.GetLimitTimeMarketItemByProductId(productId);
                if (limitTimeMarketItemByProductId != null)
                {
                    salePrice = limitTimeMarketItemByProductId.Price;
                }
            }
            return salePrice;
        }

        private decimal GetShopCouponDiscount(IEnumerable<CouponRecordInfo> coupons, long shopId)
        {
            if (coupons == null)
            {
                return new decimal(0);
            }
            return (
                from a in coupons.ToList()
                where a.ShopId == shopId
                select a.ChemCloud_Coupon.Price).FirstOrDefault();
        }

        private decimal GetShopCouponDiscount(IEnumerable<BaseAdditionalCoupon> coupons, long shopId)
        {
            if (coupons != null)
            {
                BaseAdditionalCoupon baseAdditionalCoupon = (
                    from p in coupons
                    where p.ShopId == shopId
                    select p).FirstOrDefault();
                if (baseAdditionalCoupon == null)
                {
                    return new decimal(0);
                }
                if (baseAdditionalCoupon.Type == 0)
                {
                    return (baseAdditionalCoupon.Coupon as CouponRecordInfo).ChemCloud_Coupon.Price;
                }
                if (baseAdditionalCoupon.Type == 1)
                {
                    return (baseAdditionalCoupon.Coupon as ShopBonusReceiveInfo).Price.Value;
                }
            }
            return new decimal(0);
        }

        private decimal GetShopIntegralDiscount(decimal orderTotal, decimal ordersTotal, decimal integralTotal)
        {
            decimal num = Math.Round((integralTotal * orderTotal) / ordersTotal, 2);
            return num;
        }

        public SKUInfo GetSkuByID(string skuid)
        {
            return context.SKUInfo.FindById<SKUInfo>(skuid);
        }

        public int GetSuccessOrderCountByProductID(long productId = 0L, OrderInfo.OrderOperateStatus orserStatus = (OrderInfo.OrderOperateStatus)5)
        {
            ProductVistiInfo productVistiInfo = context.ProductVistiInfo.FindBy((ProductVistiInfo p) => p.ProductId == productId).FirstOrDefault();
            if (productVistiInfo == null)
            {
                return 0;
            }
            if (!productVistiInfo.OrderCounts.HasValue)
            {
                return 0;
            }
            return (int)productVistiInfo.OrderCounts.Value;
        }

        public IQueryable<OrderInfo> GetTopOrders(int top, long userId)
        {
            return (
                from a in context.OrderInfo
                where a.UserId == userId
                orderby a.OrderDate descending
                select a).Take(top);
        }

        public bool IsHaveNoOnSaleProduct(long id)
        {
            bool flag = false;
            List<long> list = (
                from d in context.OrderItemInfo.FindBy((OrderItemInfo d) => d.OrderId == id)
                select d.ProductId).ToList();
            if (list != null && list.Count > 0)
            {
                flag = context.ProductInfo.Count((ProductInfo d) => list.Contains(d.Id) && ((int)d.SaleStatus != 1 || (int)d.AuditStatus != 2)) > 0;
            }
            return flag;
        }

        public void MembeConfirmOrder(long orderId, string memberName)
        {
            OrderInfo orderInfo = (
                from a in context.OrderInfo
                where a.Id == orderId && (a.UserName == memberName)
                select a).FirstOrDefault();
            _orderBO.SetStateToConfirm(orderInfo);
            context.SaveChanges();
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo a) => a.UserName == memberName);
            AddIntegral(userMemberInfo, orderInfo.Id, ((orderInfo.ProductTotalAmount - orderInfo.DiscountAmount) - orderInfo.IntegralDiscount) - orderInfo.RefundTotalAmount);
            AddOrderOperationLog(orderId, memberName, "会员确认收货");
            UpdateProductVistiOrderCount(orderId);
        }

        public void MemberCloseOrder(long orderId, string memberName, bool isBackStock = false)
        {
            OrderInfo orderInfo = (
                from a in context.OrderInfo
                where a.Id == orderId && (a.UserName == memberName)
                select a).FirstOrDefault();
            if (orderInfo == null)
            {
                throw new HimallException("该订单不属于该用户！");
            }
            string str = "会员取消订单";
            _orderBO.CloseOrder(orderInfo);
            if (isBackStock)
            {
                ReturnStock(orderInfo);
            }
            context.SaveChanges();
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo a) => a.Id == orderInfo.UserId);
            CancelIntegral(userMemberInfo, orderInfo.Id, orderInfo.IntegralDiscount);
            AddOrderOperationLog(orderId, memberName, str);
        }

        public void PayCapital(IEnumerable<long> orderIds, string payNo = null, long payId = 0L)
        {
            decimal? nullable;
            OrderInfo[] array = context.OrderInfo.FindBy((OrderInfo item) => orderIds.Contains(item.Id)).ToArray();
            decimal num = ((IEnumerable<OrderInfo>)array).Sum<OrderInfo>((OrderInfo e) => e.OrderTotalAmount);
            long userId = array.FirstOrDefault().UserId;
            CapitalInfo capitalInfo = context.CapitalInfo.FirstOrDefault((CapitalInfo e) => e.MemId == userId);
            decimal? balance = capitalInfo.Balance;
            if ((balance.GetValueOrDefault() >= num ? false : balance.HasValue))
            {
                throw new HimallException("预付款金额少于订单金额");
            }
            OrderInfo[] orderInfoArray = array;
            for (int i = 0; i < orderInfoArray.Length; i++)
            {
                OrderInfo empty = orderInfoArray[i];
                if (empty != null && empty.OrderStatus == OrderInfo.OrderOperateStatus.WaitPay)
                {
                    OrderPayInfo orderPayInfo = context.OrderPayInfo.FirstOrDefault((OrderPayInfo item) => item.OrderId == empty.Id && item.PayId == payId);
                    CapitalDetailInfo capitalDetailInfo = new CapitalDetailInfo()
                    {
                        Amount = -empty.OrderTotalAmount,
                        CapitalID = capitalInfo.Id,
                        CreateTime = new DateTime?(DateTime.Now),
                        SourceType = CapitalDetailInfo.CapitalDetailType.Consume,
                        SourceData = empty.Id.ToString(),
                        Id = _orderBO.GenerateOrderNumber()
                    };
                    CapitalDetailInfo capitalDetailInfo1 = capitalDetailInfo;
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        empty.PayDate = new DateTime?(DateTime.Now);
                        empty.PaymentTypeGateway = string.Empty;
                        empty.PaymentTypeName = "预付款支付";
                        empty.OrderStatus = OrderInfo.OrderOperateStatus.WaitDelivery;
                        if (orderPayInfo != null)
                        {
                            orderPayInfo.PayState = true;
                            orderPayInfo.PayTime = new DateTime?(DateTime.Now);
                        }
                        CapitalInfo capitalInfo1 = capitalInfo;
                        decimal? balance1 = capitalInfo1.Balance;
                        decimal orderTotalAmount = empty.OrderTotalAmount;
                        if (balance1.HasValue)
                        {
                            nullable = new decimal?(balance1.GetValueOrDefault() - orderTotalAmount);
                        }
                        else
                        {
                            nullable = null;
                        }
                        capitalInfo1.Balance = nullable;
                        context.CapitalDetailInfo.Add(capitalDetailInfo1);
                        UpdateShopVisti(empty);
                        UpdateProductVisti(empty);
                        UpdateLimitTimeBuyLog(empty);
                        context.SaveChanges();
                        transactionScope.Complete();
                    }
                }
            }
            MessageOrderInfo messageOrderInfo = new MessageOrderInfo()
            {
                OrderId = string.Join<long>(",", orderIds),
                ShopId = 0,
                SiteName = Instance<ISiteSettingService>.Create.GetSiteSettings().SiteName,
                TotalMoney = ((IEnumerable<OrderInfo>)array).Sum<OrderInfo>((OrderInfo a) => a.OrderTotalAmount),
                UserName = array.FirstOrDefault().UserName
            };
            long userId1 = array.FirstOrDefault().UserId;
            Task.Factory.StartNew(() => Instance<IMessageService>.Create.SendMessageOnOrderPay(userId1, messageOrderInfo));
        }

        public void PaySucceed(IEnumerable<long> orderIds, string paymentId, DateTime payTime, string payNo = null, long payId = 0L)
        {
            OrderInfo[] array = context.OrderInfo.FindBy((OrderInfo item) => orderIds.Contains(item.Id)).ToArray();
            Plugin<IPaymentPlugin> plugin = PluginsManagement.GetPlugin<IPaymentPlugin>(paymentId);
            OrderInfo[] orderInfoArray = array;
            for (int i = 0; i < orderInfoArray.Length; i++)
            {
                OrderInfo nullable = orderInfoArray[i];
                if (nullable != null && nullable.OrderStatus == OrderInfo.OrderOperateStatus.WaitPay)
                {
                    OrderPayInfo orderPayInfo = context.OrderPayInfo.FirstOrDefault((OrderPayInfo item) => item.OrderId == nullable.Id && item.PayId == payId);
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        nullable.PayDate = new DateTime?(payTime);
                        nullable.PaymentTypeGateway = paymentId;
                        nullable.PaymentTypeName = plugin.PluginInfo.DisplayName;
                        nullable.OrderStatus = OrderInfo.OrderOperateStatus.WaitDelivery;
                        if (orderPayInfo != null)
                        {
                            orderPayInfo.PayState = true;
                            orderPayInfo.PayTime = new DateTime?(payTime);
                        }
                        UpdateShopVisti(nullable);
                        UpdateProductVisti(nullable);
                        UpdateLimitTimeBuyLog(nullable);
                        nullable.GatewayOrderId = payNo;
                        context.SaveChanges();
                        transactionScope.Complete();
                    }
                }
            }
            MessageOrderInfo messageOrderInfo = new MessageOrderInfo()
            {
                OrderId = string.Join<long>(",", orderIds),
                ShopId = 0,
                SiteName = Instance<ISiteSettingService>.Create.GetSiteSettings().SiteName,
                TotalMoney = ((IEnumerable<OrderInfo>)array).Sum<OrderInfo>((OrderInfo a) => a.OrderTotalAmount),
                UserName = array.FirstOrDefault().UserName
            };
            long userId = array.FirstOrDefault().UserId;
            Task.Factory.StartNew(() => Instance<IMessageService>.Create.SendMessageOnOrderPay(userId, messageOrderInfo));
        }

        public void PlatformCloseOrder(long orderId, string managerName, string closeReason = "")
        {
            OrderInfo orderInfo = context.OrderInfo.FindById<OrderInfo>(orderId);
            if (string.IsNullOrWhiteSpace(closeReason))
            {
                closeReason = "平台取消订单";
            }
            orderInfo.CloseReason = closeReason;
            _orderBO.CloseOrder(orderInfo);
            ReturnStock(orderInfo);
            context.SaveChanges();
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo a) => a.Id == orderInfo.UserId);
            CancelIntegral(userMemberInfo, orderInfo.Id, orderInfo.IntegralDiscount);
            AddOrderOperationLog(orderId, managerName, closeReason);
        }

        public void PlatformConfirmOrderPay(long orderId, string payRemark, string managerName)
        {
            OrderInfo nullable = context.OrderInfo.FindById<OrderInfo>(orderId);
            if (nullable.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay)
            {
                throw new HimallException("只有待付款状态的订单才能进行付款操作");
            }
            nullable.OrderStatus = OrderInfo.OrderOperateStatus.WaitDelivery;
            nullable.PayRemark = payRemark;
            nullable.PaymentTypeName = string.Format("平台线下收款", managerName);
            nullable.PayDate = new DateTime?(DateTime.Now);
            AddOrderOperationLog(orderId, managerName, "平台确认收到订单货款");
            UpdateShopVisti(nullable);
            UpdateProductVisti(nullable);
            UpdateLimitTimeBuyLog(nullable);
        }

        private void ReturnStock(OrderInfo order)
        {
            IProductService create = Instance<IProductService>.Create;
            foreach (OrderItemInfo orderItemInfo in order.OrderItemInfo)
            {
                SKUInfo sKUInfo = context.SKUInfo.FindById<SKUInfo>(orderItemInfo.SkuId);
                if (sKUInfo == null)
                {
                    continue;
                }
                SKUInfo stock = sKUInfo;
                stock.Stock = stock.Stock + orderItemInfo.Quantity;
            }
            context.SaveChanges();
        }

        public void SaveInvoiceContext(InvoiceContextInfo info)
        {
            if (info.Id < 0)
            {
                context.InvoiceContextInfo.Add(info);
            }
            else
            {
                InvoiceContextInfo name = context.InvoiceContextInfo.First<InvoiceContextInfo>((InvoiceContextInfo p) => p.Id == info.Id);
                name.Name = info.Name;
            }
            context.SaveChanges();
        }

        public long SaveInvoiceTitle(InvoiceTitleInfo info)
        {
            InvoiceTitleInfo invoiceTitleInfo = context.InvoiceTitleInfo.Add(info);
            context.SaveChanges();
            return invoiceTitleInfo.Id;
        }

        public long SaveOrderPayInfo(IEnumerable<OrderPayInfo> model, PlatformType platform)
        {
            long orderId = model.FirstOrDefault().OrderId;
            int num = (int)platform;
            long num1 = long.Parse(string.Concat(orderId.ToString(), num.ToString()));
            long num2 = (model.Count() == 1 ? num1 : _orderBO.GetOrderPayId());
            foreach (OrderPayInfo orderPayInfo in model)
            {
                if (context.OrderPayInfo.FirstOrDefault((OrderPayInfo item) => item.PayId == num2 && item.OrderId == orderPayInfo.OrderId) != null)
                {
                    continue;
                }
                OrderPayInfo orderPayInfo1 = new OrderPayInfo()
                {
                    OrderId = orderPayInfo.OrderId,
                    PayId = num2
                };
                context.OrderPayInfo.Add(orderPayInfo1);
            }
            context.SaveChanges();
            return num2;
        }

        public void SellerCloseOrder(long orderId, string sellerName)
        {
            OrderInfo orderInfo = context.OrderInfo.FindById<OrderInfo>(orderId);
            orderInfo.CloseReason = "商家取消订单";
            _orderBO.CloseOrder(orderInfo);
            ReturnStock(orderInfo);
            context.SaveChanges();
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo a) => a.Id == orderInfo.UserId);
            CancelIntegral(userMemberInfo, orderInfo.Id, orderInfo.IntegralDiscount);
            AddOrderOperationLog(orderId, sellerName, orderInfo.CloseReason);
        }



        public void SellerSendGood(long orderId, string sellerName, string companyName, string shipOrderNumber, int isreplacedeliver, string replacedeliveraddress)
        {
            OrderInfo nullable = context.OrderInfo.FindById<OrderInfo>(orderId);
            if (nullable.OrderStatus != OrderInfo.OrderOperateStatus.WaitDelivery)
            {
                throw new HimallException("只有待发货状态的订单才能发货");
            }
            nullable.OrderStatus = OrderInfo.OrderOperateStatus.WaitReceiving;
            nullable.ExpressCompanyName = companyName;
            nullable.ShipOrderNumber = shipOrderNumber;
            nullable.ShippingDate = new DateTime?(DateTime.Now);
            nullable.Isreplacedeliver = isreplacedeliver;
            nullable.Replacedeliveraddress = replacedeliveraddress;




            OrderRefundInfo orderRefundInfo = context.OrderRefundInfo.FirstOrDefault((OrderRefundInfo d) => d.OrderId == orderId && (int)d.RefundMode == 1 && (int)d.SellerAuditStatus == 1);
            if (orderRefundInfo != null)
            {
                Instance<IRefundService>.Create.SellerDealRefund(orderRefundInfo.Id, OrderRefundInfo.OrderRefundAuditStatus.UnAudit, "商家己发货", sellerName);
            }
            context.SaveChanges();
            MessageOrderInfo messageOrderInfo = new MessageOrderInfo()
            {
                OrderId = nullable.Id.ToString(),
                ShopId = nullable.ShopId,
                ShopName = nullable.ShopName,
                SiteName = Instance<ISiteSettingService>.Create.GetSiteSettings().SiteName,
                TotalMoney = nullable.OrderTotalAmount,
                ShippingCompany = companyName,
                ShippingNumber = shipOrderNumber,

            };
            Task.Factory.StartNew(() => Instance<IMessageService>.Create.SendMessageOnOrderShipping(nullable.UserId, messageOrderInfo));
            AddOrderOperationLog(orderId, sellerName, "商家发货");
        }


        public void SellerSendGoodFreight(long orderId, string sellerName, string companyName, string shipOrderNumber, int isreplacedeliver, string replacedeliveraddress, int freightId)
        {
            OrderInfo nullable = context.OrderInfo.FindById<OrderInfo>(orderId);

            //if (nullable.OrderStatus != OrderInfo.OrderOperateStatus.WaitDelivery)
            //{
            //    throw new HimallException("只有待发货状态的订单才能发货");
            //}

            /*若当前订单状态为未支付，仍然选择发货，则设置支付方式为货到付款*/
            if (nullable.OrderStatus == OrderInfo.OrderOperateStatus.WaitPay)
            {
                nullable.PaymentTypeName = "货到付款";
            }

            /*订单状态改为待收货*/
            nullable.OrderStatus = OrderInfo.OrderOperateStatus.WaitReceiving;

            nullable.ExpressCompanyName = companyName;
            nullable.ShipOrderNumber = shipOrderNumber;
            nullable.ShippingDate = new DateTime?(DateTime.Now);
            nullable.Isreplacedeliver = isreplacedeliver;
            nullable.Replacedeliveraddress = replacedeliveraddress;
            /*平台代发*/
            nullable.IsBehalfShip = isreplacedeliver.ToString();
            /*计算物流费用*/
            //nullable.Freight = GetFreight(freightId, nullable.OrderItemInfo.ToList(), nullable.RegionId);

            OrderRefundInfo orderRefundInfo = context.OrderRefundInfo.FirstOrDefault((OrderRefundInfo d) => d.OrderId == orderId && (int)d.RefundMode == 1 && (int)d.SellerAuditStatus == 1);
            if (orderRefundInfo != null)
            {
                Instance<IRefundService>.Create.SellerDealRefund(orderRefundInfo.Id, OrderRefundInfo.OrderRefundAuditStatus.UnAudit, "商家己发货", sellerName);
            }
            context.SaveChanges();
            MessageOrderInfo messageOrderInfo = new MessageOrderInfo()
            {
                OrderId = nullable.Id.ToString(),
                ShopId = nullable.ShopId,
                ShopName = nullable.ShopName,
                SiteName = Instance<ISiteSettingService>.Create.GetSiteSettings().SiteName,
                TotalMoney = nullable.OrderTotalAmount,
                ShippingCompany = companyName,
                ShippingNumber = shipOrderNumber,

            };
            Task.Factory.StartNew(() => Instance<IMessageService>.Create.SendMessageOnOrderShipping(nullable.UserId, messageOrderInfo));
            AddOrderOperationLog(orderId, sellerName, "商家发货");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private decimal GetFreight(int frenghtId, List<OrderItemInfo> itemList, int cityId)
        {
            decimal freight2 = 0;
            IFreightTemplateService freightTemplateService = Instance<IFreightTemplateService>.Create;
            foreach (var orderItem in itemList)
            {
                FreightTemplateInfo freightTemplate = freightTemplateService.GetFreightTemplate(frenghtId);
                if (freightTemplate == null || freightTemplate.IsFree != FreightTemplateInfo.FreightTemplateType.SelfDefine)
                {
                    continue;
                }
                FreightAreaContentInfo freightAreaContentInfo = (
                    from item in freightTemplate.ChemCloud_FreightAreaContent
                    where item.AreaContent.Split(new char[] { ',' }).Contains<string>(cityId.ToString())
                    select item).FirstOrDefault() ?? freightTemplate.ChemCloud_FreightAreaContent.Where((FreightAreaContentInfo item) =>
                    {
                        byte? isDefault = item.IsDefault;
                        if (isDefault.GetValueOrDefault() != 1)
                        {
                            return false;
                        }
                        return isDefault.HasValue;
                    }).FirstOrDefault();
                if (freightTemplate.ValuationMethod == FreightTemplateInfo.ValuationMethodType.Weight)
                {
                    decimal num3 = orderItem.Quantity;
                    int value = freightAreaContentInfo.FirstUnit.Value;
                    decimal value1 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int value2 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? accumulationUnitMoney = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num3, value, value1, value2, (decimal)accumulationUnitMoney.Value);
                }
                else if (freightTemplate.ValuationMethod != FreightTemplateInfo.ValuationMethodType.Bulk)
                {
                    int num4 = Convert.ToInt32(orderItem.Quantity);
                    decimal num5 = num4;
                    int value3 = freightAreaContentInfo.FirstUnit.Value;
                    decimal value4 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int value5 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? nullable = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num5, value3, value4, value5, (decimal)nullable.Value);
                }
                else
                {
                    decimal num6 = orderItem.Quantity;
                    int value6 = freightAreaContentInfo.FirstUnit.Value;
                    decimal value7 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int num7 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? accumulationUnitMoney1 = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num6, value6, value7, num7, (decimal)accumulationUnitMoney1.Value);
                }
            }
            return freight2;
        }

        private decimal GetFreight2(decimal count, int firstUnit, decimal firstUnitMonry, int accumulationUnit, decimal accumulationUnitMoney)
        {
            decimal num = new decimal(0);
            if (count > firstUnit)
            {
                decimal num1 = (count - firstUnit) / accumulationUnit;
                decimal num2 = Math.Truncate(num1);
                decimal num3 = num1 - num2;
                decimal num4 = num2 * accumulationUnitMoney;
                decimal num5 = new decimal(0);
                if (num3 > new decimal(0))
                {
                    num5 = new decimal(1) * accumulationUnitMoney;
                }
                num = (firstUnitMonry + num4) + num5;
            }
            else
            {
                num = firstUnitMonry;
            }
            return num;
        }

        public void SellerUpdateAddress(long orderId, string sellerName, string shipTo, string cellPhone, int topRegionId, int regionId, string regionFullName, string address)
        {
            OrderInfo orderInfo = context.OrderInfo.FindById<OrderInfo>(orderId);
            if (orderInfo.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay && orderInfo.OrderStatus != OrderInfo.OrderOperateStatus.WaitDelivery)
            {
                throw new HimallException("只有待付款或待发货状态的订单才能修改收货地址");
            }
            orderInfo.ShipTo = shipTo;
            orderInfo.CellPhone = cellPhone;
            orderInfo.TopRegionId = topRegionId;
            orderInfo.RegionId = regionId;
            orderInfo.RegionFullName = regionFullName;
            orderInfo.Address = address;
            context.SaveChanges();
            AddOrderOperationLog(orderId, sellerName, "商家修改订单的收货地址");
        }

        public void SellerUpdateItemDiscountAmount(long orderItemId, decimal discountAmount, string sellerName)
        {
            OrderItemInfo realTotalPrice = context.OrderItemInfo.FindById<OrderItemInfo>(orderItemId);
            OrderInfo orderInfo = context.OrderInfo.FindById<OrderInfo>(realTotalPrice.OrderId);
            OrderItemInfo orderItemInfo = realTotalPrice;
            orderItemInfo.DiscountAmount = orderItemInfo.DiscountAmount + discountAmount;
            realTotalPrice.RealTotalPrice = _orderBO.GetRealTotalPrice(orderInfo, realTotalPrice, discountAmount);
            context.SaveChanges();
            realTotalPrice = context.OrderItemInfo.FindById<OrderItemInfo>(orderItemId);
            realTotalPrice.OrderInfo.ProductTotalAmount = realTotalPrice.OrderInfo.ProductTotalAmount - discountAmount;
            realTotalPrice.OrderInfo.CommisTotalAmount = (
                from i in context.OrderItemInfo
                where i.OrderId == realTotalPrice.OrderId
                select i).Sum<OrderItemInfo>((OrderItemInfo i) => i.RealTotalPrice * i.CommisRate);
            context.SaveChanges();
            AddOrderOperationLog(realTotalPrice.OrderId, sellerName, "商家修改订单产品的优惠金额");
        }

        public void SellerUpdateOrderFreight(long orderId, decimal freight)
        {
            OrderInfo orderInfo = context.OrderInfo.FindById<OrderInfo>(orderId);
            _orderBO.SetFreight(orderInfo, freight);
            context.SaveChanges();
        }

        private void SendMessage(IEnumerable<OrderInfo> infos)
        {
            if (infos == null || infos.Count() == 0)
            {
                return;
            }
            MessageOrderInfo messageOrderInfo = new MessageOrderInfo();
            long[] array = (
                from item in infos
                select item.Id).ToArray();
            messageOrderInfo.OrderId = string.Join<long>(",", array);
            OrderInfo orderInfo = infos.FirstOrDefault();
            if (array.Length <= 1)
            {
                messageOrderInfo.ShopId = orderInfo.ShopId;
                messageOrderInfo.ShopName = orderInfo.ShopName;
            }
            else
            {
                messageOrderInfo.ShopId = 0;
            }
            long userId = orderInfo.UserId;
            messageOrderInfo.SiteName = Instance<ISiteSettingService>.Create.GetSiteSettings().SiteName;
            messageOrderInfo.TotalMoney = infos.Sum<OrderInfo>((OrderInfo a) => a.OrderTotalAmount);
            messageOrderInfo.UserName = orderInfo.UserName;
            Task.Factory.StartNew(() => Instance<IMessageService>.Create.SendMessageOnOrderCreate(userId, messageOrderInfo));
        }

        public void SetOrderExpressInfo(long shopId, string expressName, string startCode, IEnumerable<long> orderIds)
        {
            IExpress express = Instance<IExpressService>.Create.GetExpress(expressName);
            if (!express.CheckExpressCodeIsValid(startCode))
            {
                throw new HimallException("起始快递单号格式不正确");
            }
            OrderInfo[] array = context.OrderInfo.FindBy((OrderInfo item) => item.ShopId == shopId && orderIds.Contains(item.Id)).ToArray();
            IEnumerable<OrderInfo> orderInfos =
                from item in orderIds
                select array.FirstOrDefault((OrderInfo t) => item == t.Id) into item
                where item != null
                select item;
            int num = 0;
            string empty = string.Empty;
            ShopInfo shop = Instance<IShopService>.Create.GetShop(shopId, false);
            IRegionService create = Instance<IRegionService>.Create;
            int? senderRegionId = shop.SenderRegionId;
            string str = string.Concat(create.GetRegionFullName(senderRegionId.Value, " "), " ", shop.SenderAddress);
            foreach (OrderInfo senderPhone in orderInfos)
            {
                int num1 = num;
                num = num1 + 1;
                empty = (num1 != 0 ? express.NextExpressCode(empty) : startCode);
                senderPhone.ShipOrderNumber = empty;
                senderPhone.ExpressCompanyName = expressName;
                senderPhone.SellerPhone = shop.SenderPhone;
                senderPhone.SellerAddress = str;
            }
            context.SaveChanges();
        }

        private void UpdateLimitTimeBuyLog(OrderInfo order)
        {
            ILimitTimeBuyService create = Instance<ILimitTimeBuyService>.Create;
            foreach (OrderItemInfo orderItemInfo in order.OrderItemInfo)
            {
                if (!create.IsLimitTimeMarketItem(orderItemInfo.ProductId))
                {
                    continue;
                }
                LimitTimeMarketInfo limitTimeMarketInfo = context.LimitTimeMarketInfo.FirstOrDefault((LimitTimeMarketInfo m) => m.ProductId == orderItemInfo.ProductId && (int)m.AuditStatus == 2);
                if (limitTimeMarketInfo == null)
                {
                    continue;
                }
                LimitTimeMarketInfo saleCount = limitTimeMarketInfo;
                saleCount.SaleCount = saleCount.SaleCount + (int)orderItemInfo.Quantity;
            }
            context.SaveChanges();
        }

        public void UpdateMemberOrderInfo(long userId, [DecimalConstant(0, 0, 0, 0, 0)] decimal addOrderAmount = default(decimal), int addOrderCount = 1)
        {
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo item) => item.Id == userId);
            UserMemberInfo orderNumber = userMemberInfo;
            orderNumber.OrderNumber = orderNumber.OrderNumber + addOrderCount;
            UserMemberInfo expenditure = userMemberInfo;
            expenditure.Expenditure = expenditure.Expenditure + addOrderAmount;
            context.SaveChanges();
        }

        private void UpdateProductVisti(OrderInfo order)
        {
            DateTime date = DateTime.Now.Date;
            DateTime dateTime = DateTime.Now.Date.AddDays(1);
            foreach (OrderItemInfo list in order.OrderItemInfo.ToList())
            {
                ProductVistiInfo productVistiInfo = context.ProductVistiInfo.FindBy((ProductVistiInfo item) => item.ProductId == list.ProductId && (item.Date >= date) && (item.Date <= dateTime)).FirstOrDefault();
                if (productVistiInfo == null)
                {
                    productVistiInfo = new ProductVistiInfo()
                    {
                        ProductId = list.ProductId,
                        Date = DateTime.Now.Date,
                        OrderCounts = new long?(0)
                    };
                    context.ProductVistiInfo.Add(productVistiInfo);
                }
                DbSet<ProductInfo> productInfo = context.ProductInfo;
                object[] productId = new object[] { list.ProductId };
                ProductInfo productInfo1 = productInfo.Find(productId);
                if (productInfo1 != null)
                {
                    ProductInfo saleCounts = productInfo1;
                    saleCounts.SaleCounts = saleCounts.SaleCounts + list.Quantity;
                }
                ProductVistiInfo saleCounts1 = productVistiInfo;
                saleCounts1.SaleCounts = saleCounts1.SaleCounts + list.Quantity;
                ProductVistiInfo saleAmounts = productVistiInfo;
                saleAmounts.SaleAmounts = saleAmounts.SaleAmounts + list.RealTotalPrice;
            }
            context.SaveChanges();
        }

        private void UpdateProductVistiOrderCount(long orderId)
        {
            long? nullable;
            IQueryable<OrderItemInfo> orderItemInfo =
                from o in context.OrderItemInfo
                where o.OrderId == orderId
                select o;
            foreach (OrderItemInfo list in orderItemInfo.ToList())
            {
                ProductVistiInfo productVistiInfo = context.ProductVistiInfo.FindBy((ProductVistiInfo p) => p.ProductId == list.ProductId).FirstOrDefault();
                if (productVistiInfo == null)
                {
                    continue;
                }
                ProductVistiInfo productVistiInfo1 = productVistiInfo;
                long? nullable1 = (!productVistiInfo.OrderCounts.HasValue ? new long?(0) : productVistiInfo.OrderCounts);
                if (nullable1.HasValue)
                {
                    nullable = new long?(nullable1.GetValueOrDefault() + 1);
                }
                else
                {
                    nullable = null;
                }
                productVistiInfo1.OrderCounts = nullable;
                context.SaveChanges();
            }
        }

        private void UpdateShopVisti(OrderInfo order)
        {
            DateTime date = DateTime.Now.Date;
            ShopVistiInfo shopVistiInfo = context.ShopVistiInfo.FindBy((ShopVistiInfo item) => item.ShopId == order.ShopId && item.Date.Year == date.Year && item.Date.Month == date.Month && item.Date.Day == date.Day).FirstOrDefault();
            if (shopVistiInfo == null)
            {
                shopVistiInfo = new ShopVistiInfo()
                {
                    ShopId = order.ShopId,
                    Date = DateTime.Now.Date
                };
                context.ShopVistiInfo.Add(shopVistiInfo);
            }
            ShopVistiInfo saleCounts = shopVistiInfo;
            saleCounts.SaleCounts = saleCounts.SaleCounts + order.OrderProductQuantity;
            ShopVistiInfo saleAmounts = shopVistiInfo;
            saleAmounts.SaleAmounts = saleAmounts.SaleAmounts + order.ProductTotalAmount;
            context.SaveChanges();
        }

        private void UseCoupon(List<OrderInfo> orders, List<BaseAdditionalCoupon> coupons, long userid)
        {
            ICouponService create = Instance<ICouponService>.Create;
            IShopBonusService shopBonusService = Instance<IShopBonusService>.Create;
            foreach (BaseAdditionalCoupon coupon in coupons)
            {
                if (coupon.Type != 0)
                {
                    if (coupon.Type != 1)
                    {
                        continue;
                    }
                    shopBonusService.SetBonusToUsed(userid, orders, (coupon.Coupon as ShopBonusReceiveInfo).Id);
                }
                else
                {
                    long id = (coupon.Coupon as CouponRecordInfo).Id;
                    create.UseCoupon(userid, new long[] { id }, orders);
                }
            }
        }
        public void UpdateOrderStatu(long orderId, int orderstatus)
        {
            OrderInfo nullable = context.OrderInfo.FirstOrDefault((OrderInfo m) => m.Id == orderId);
            if (nullable == null)
            {
                return;
            }
            nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.WaitDelivery;
            if (orderstatus == 7)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.FQPAY;
            }

            if (orderstatus == 5)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.Finish;
            }

            if (orderstatus == 8)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.THing;
            }
            if (orderstatus == 9)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TH;
            }
            if (orderstatus == 10)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TKing;
            }
            if (orderstatus == 11)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TK;
            }
            if (orderstatus == 12)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TKRefuse;
            }
            if (orderstatus == 13)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.THRefuse;
            }
            if (nullable.OrderStatus != ChemCloud.Model.OrderInfo.OrderOperateStatus.WaitPay && nullable.OrderStatus != ChemCloud.Model.OrderInfo.OrderOperateStatus.FQPAY)
            {
                nullable.FinishDate = DateTime.Now;
            }
            context.SaveChanges();
        }
        public void UpdateOrderStatuOnly(long orderId, int orderstatus)
        {
            //仅仅更该状态，不更改订单完成时间，如评价时，订单完成时间不能随着评价时间而变化，应该还是订单的签收时间
            OrderInfo nullable = context.OrderInfo.FirstOrDefault((OrderInfo m) => m.Id == orderId);
            if (nullable == null)
            {
                return;
            }
            nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.WaitDelivery;
            if (orderstatus == 7)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.FQPAY;
            }

            if (orderstatus == 5)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.Finish;
            }

            if (orderstatus == 8)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.THing;
            }
            if (orderstatus == 9)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TH;
            }
            if (orderstatus == 10)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TKing;
            }
            if (orderstatus == 11)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TK;
            }
            if (orderstatus == 12)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TKRefuse;
            }
            if (orderstatus == 13)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.THRefuse;
            }
            context.SaveChanges();
        }
        public void UpdateOrderStatuNew(long orderId, int orderstatus, string paymodel)
        {
            OrderInfo nullable = context.OrderInfo.FirstOrDefault((OrderInfo m) => m.Id == orderId);
            if (nullable == null)
            {
                return;
            }
            nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.WaitDelivery;
            nullable.PayDate = DateTime.Now;

            if (string.IsNullOrEmpty(paymodel)) { paymodel = "支付宝"; }
            nullable.PaymentTypeName = paymodel;

            if (orderstatus == 7)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.FQPAY;
            }
            if (orderstatus == 8)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.THing;
            }
            if (orderstatus == 9)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TH;
            }
            if (orderstatus == 10)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TKing;
            }
            if (orderstatus == 11)
            {
                nullable.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.TK;
            }
            //if (nullable.OrderStatus != ChemCloud.Model.OrderInfo.OrderOperateStatus.WaitPay && nullable.OrderStatus != ChemCloud.Model.OrderInfo.OrderOperateStatus.FQPAY)
            //{
            //    nullable.FinishDate = DateTime.Now;
            //}
            //关闭：4，已完结：5，已签收：6
            if (orderstatus == 4 || orderstatus == 5 || orderstatus == 6 || orderstatus == 2)
            {
                nullable.FinishDate = DateTime.Now;
            }
            context.SaveChanges();
        }


        public List<OrderItemInfo> GetOrderItemInfoDetailWithProductCode(long orderId)
        {

            ProductService _ProductService = new ProductService();

            List<OrderItemInfo> list =
                (from item in base.context.OrderItemInfo
                 where item.OrderId == orderId
                 select item).ToList();
            foreach (OrderItemInfo model in list)
            {
                model.ProductCode = _ProductService.GetProduct(model.ProductId) == null ? "" : _ProductService.GetProduct(model.ProductId).ProductCode;
                model.CASNo = _ProductService.GetProduct(model.ProductId) == null ? "" : _ProductService.GetProduct(model.ProductId).CASNo;
            }
            return list;
        }

        public bool UpdateOrderShipNum(OrderInfo oinfo)
        {
            int i = 0;
            OrderInfo fw = context.OrderInfo.FirstOrDefault((OrderInfo m) => m.Id == oinfo.Id);
            if (fw == null)
            {
                return false;
            }
            fw.SellerRemark = oinfo.SellerRemark;
            fw.ShipOrderNumber = oinfo.ShipOrderNumber;
            i = context.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 其他支付完成时候，对应更新状态
        /// </summary>
        /// <param name="paytype"></param>
        /// <param name="targetid"></param>
        public void UpdatePayToPlatformStatus(string paytype, string targetid)
        {
            if (string.IsNullOrEmpty(paytype) || string.IsNullOrEmpty(targetid))
            { return; }

            long id = long.Parse(targetid);

            //3供应商认证支付,2产品认证支付，1排名付费支付 ,4定制合成 5代理采购

            if (paytype == "1")
            {
                ShopInfo model = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id == id);
                //SortType；0默认/1审核后/2实地/3付费；
                if (model == null) { return; }
                model.SortType = 3;//已付款
            }
            else if (paytype == "2")
            {
                //产品认证支付，更新状态 
                ProductAuthentication model = context.ProductAuthentication.FirstOrDefault((ProductAuthentication m) => m.ProductId == id);
                model.AuthStatus = 2;
                model.AuthTime = DateTime.Now;
                ProductInfo productInfo = context.ProductInfo.FirstOrDefault((ProductInfo a) => a.Id == id);
                productInfo.AuthStatus = 2;
            }
            else if (paytype == "3")
            {
                ManagerInfo _ManagerInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.ShopId == id);
                if (_ManagerInfo == null) { return; }

                //实地认证，更新状态 
                FieldCertification _FieldCertification = context.FieldCertification.FirstOrDefault((FieldCertification m) => m.Id == _ManagerInfo.CertificationId);
                _FieldCertification.Status = FieldCertification.CertificationStatus.PayandWaitAudit;//付款完毕，等待管理员确认是否审核通过

                _FieldCertification.FeedbackDate = DateTime.Now;
                context.SaveChanges();

                //付款完毕，还要等待管理员审核才能变为认证供应商，此时仍是普通供应商
                //ShopInfo _ShopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id == id);
                ////SortType；0默认/1审核后/2实地/3付费；
                //if (_ShopInfo == null) { return; }
                //_ShopInfo.GradeId = 3;
                //_ShopInfo.SortType = 2;
                //context.SaveChanges();

            }
            else if (paytype == "4")
            {
                //定制合成
                OrderSynthesis model = context.OrderSynthesis.FirstOrDefault((OrderSynthesis m) => m.OrderNumber == id.ToString());
                if (model == null) { return; }
                model.Status = 3;//已付款
            }
            else if (paytype == "5")
            {
                //代理采购
                OrderPurchasing model = context.OrderPurchasing.FirstOrDefault((OrderPurchasing m) => m.OrderNum == id.ToString());
                if (model == null) { return; }
                model.Status = 3;//已付款
            }

            else if (paytype == "6")
            {
                //分析鉴定
                ProductAnalysis model = context.ProductAnalysis.FirstOrDefault((ProductAnalysis m) => m.Id == id);
                if (model == null) { return; }
                model.AnalysisStatus = 3;//已支付
            }

            else if (paytype == "7")
            {
                //我要采购
                IWantToSupply model = context.IWantToSupply.FirstOrDefault((IWantToSupply m) => m.Id == id);
                if (model == null) { return; }
                else
                {
                    model.Status = 5;//已支付
                    long PurchaseNum = model.PurchaseNum;

                    IWantToBuy _IWantToBuy = context.IWantToBuy.FirstOrDefault((IWantToBuy m) => m.PurchaseNum == PurchaseNum);
                    _IWantToBuy.Status = 5;//已支付

                    IWantToBuy_Orders _IWantToBuy_Orders = context.IWantToBuy_Orders.FirstOrDefault((IWantToBuy_Orders m) => m.PurchaseNum == PurchaseNum);
                    _IWantToBuy_Orders.Status = 5;//已支付
                    _IWantToBuy_Orders.PayDate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException) { }
        }


        public List<OrderItemInfo> GetOrdersByIds(params long[] ids)
        {

            ProductService _ProductService = new ProductService();

            List<OrderItemInfo> list =
                (from item in base.context.OrderItemInfo
                 where ids.Contains(item.OrderId)
                 select item).ToList();
            foreach (OrderItemInfo model in list)
            {
                model.ProductCode = _ProductService.GetProduct(model.ProductId) == null ? "" : _ProductService.GetProduct(model.ProductId).ProductCode;
                model.CASNo = _ProductService.GetProduct(model.ProductId) == null ? "" : _ProductService.GetProduct(model.ProductId).CASNo;
            }
            return list;
        }

        private class CartSkuInfo
        {
            public long ColloPid
            {
                get;
                set;
            }

            public ProductInfo Product
            {
                get;
                set;
            }

            public long Quantity
            {
                get;
                set;
            }

            public SKUInfo SKU
            {
                get;
                set;
            }

            public CartSkuInfo()
            {
            }
        }


        public bool updateorderproductamount(long id, decimal price)
        {
            bool result = false;
            try
            {
                OrderInfo _OrderInfo = context.OrderInfo.FindById(id);
                if (_OrderInfo != null)
                {
                    _OrderInfo.ProductTotalAmount = price;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex) { result = false; }

            return result;
        }


        public bool updateorderitemprice(long id, decimal price)
        {
            bool result = false;
            try
            {
                OrderItemInfo _OrderInfo = context.OrderItemInfo.FindById(id);
                if (_OrderInfo != null)
                {
                    _OrderInfo.SalePrice = price;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex) { result = false; }

            return result;
        }

        public bool UpdateShip(OrderInfo order)
        {
            bool result = false;
            try
            {
                OrderInfo orderInfo = context.OrderInfo.FindById(order.Id);
                if (orderInfo != null)
                {
                    orderInfo.ExpressCompanyName = order.ExpressCompanyName;
                    orderInfo.ShipOrderNumber = order.ShipOrderNumber;
                    orderInfo.ShippingDate = order.ShippingDate;
                    orderInfo.OrderStatus = order.OrderStatus;
                    orderInfo.BehalfShipType = order.BehalfShipType;
                    orderInfo.BehalfShipNumber = order.BehalfShipNumber;
                    context.SaveChanges();
                    result = true;

                }

            }
            catch (Exception ex) { result = false; }

            return result;
        }

    }
}