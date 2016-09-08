using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using ChemColud.Shipping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChemCloud.Service
{
    public class MargainBillService : ServiceBase, IMargainBillService, IService, IDisposable
    {

        //添加议价单
        public void AddBill(MargainBill model)
        {
            context.MargainBill.Add(model);
            context.SaveChanges();
            foreach (var item in model._MargainBillDetail)
            {
                context.MargainBillDetail.Add(item);
                context.SaveChanges();
            }
        }

        //添加议价记录  chenq
        public void AddMargainDetail(MargainBillDetail model)
        {
            context.MargainBillDetail.Add(model);
            context.SaveChanges();
        }

        //修改卖家议价详细的回复信息  chenq
        public void UpdateMargainDetailMessageReply(MargainBillDetail model)
        {
            MargainBillDetail item = context.MargainBillDetail.FindById<MargainBillDetail>(model.Id);
            item.MessageReply = model.MessageReply;
            item.PurchasePrice = model.PurchasePrice;
            item.Num = model.Num;
            context.SaveChanges();
        }

        //修改买家议价详细的留言  chenq
        public void UpdateMargainDetailBuyerMessage(MargainBillDetail model)
        {
            MargainBillDetail item = context.MargainBillDetail.FindById<MargainBillDetail>(model.Id);
            item.BuyerMessage = model.BuyerMessage;
            context.SaveChanges();
        }


        //修改议价单
        public void UpdateBill(MargainBill model)
        {
            MargainBill _MargainBill = context.MargainBill.FindById<MargainBill>(model.Id);
            _MargainBill.TotalAmount = model.TotalAmount;

            UpdateBillDetail(model._MargainBillDetail.FirstOrDefault());

            context.SaveChanges();

        }
        //修改议价单状态
        public void UpdateBillStatu(long Id)
        {
            MargainBill model = context.MargainBill.FindById<MargainBill>(Id);
            model.BillStatus = EnumBillStatus.Bargaining;
            context.SaveChanges();

        }

        public void UpdateBillStatu(long Id, EnumBillStatus status)
        {
            MargainBill model = context.MargainBill.FindById<MargainBill>(Id);
            model.BillStatus = status;
            context.SaveChanges();

        }
        //修改议价详细
        public void UpdateBillDetail(MargainBillDetail model)
        {

            MargainBillDetail _MargainBillDetail = (from q in context.MargainBillDetail where q.BillNo == model.BillNo select q).FirstOrDefault();
            _MargainBillDetail.Num = model.Num;
            _MargainBillDetail.PurchasePrice = model.PurchasePrice;
            context.SaveChanges();
        }

        //议价单转订单
        public void TransferOrder(long MargainBillId)
        {
            try
            {
                MargainBill margainbill = GetBillById(MargainBillId, 0);

                ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                long orderid = _orderBO.GenerateOrderNumber();

                ShippingAddressService _ShippingAddressService = new ShippingAddressService();
                ShippingAddressInfo addressinfo = _ShippingAddressService.GetUserShippingAddress(long.Parse(margainbill.DeliverAddress));

                ShopService _ShopService = new ShopService();
                string shopname = _ShopService.GetShopName(margainbill.ShopId);

                MemberService _MemberService = new MemberService();
                string username = _MemberService.GetMember(margainbill.MemberId).UserName;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    OrderInfo orderInfo = new OrderInfo();
                    orderInfo.Id = orderid; //订单号
                    orderInfo.ShopId = margainbill.ShopId; //供应商编号ChemCloud_Shops
                    orderInfo.ShopName = shopname;
                    orderInfo.UserId = margainbill.MemberId; //采购商ChemCloud_Members
                    orderInfo.UserName = username;
                    orderInfo.OrderDate = DateTime.Now;//订单日期
                    orderInfo.Address = margainbill.DeliverAddress; //收获地址 shippingaddress 编号
                    orderInfo.ExpressCompanyName = margainbill.DeliverType;
                    orderInfo.Freight = margainbill.DeliverCost;
                    orderInfo.ShippingDate = margainbill.DeliverDate;
                    orderInfo.PaymentTypeName = margainbill.PayMode;
                    orderInfo.ProductTotalAmount = margainbill.TotalAmount;
                    orderInfo.CoinType = margainbill.CoinType;
                    orderInfo.ShipTo = addressinfo.ShipTo;//收货人
                    orderInfo.TopRegionId = int.Parse("1");
                    orderInfo.RegionId = addressinfo.RegionId;
                    orderInfo.RegionFullName = addressinfo.RegionFullName;

                    orderInfo.OrderStatus = OrderInfo.OrderOperateStatus.WaitPay;
                    orderInfo.IsPrinted = false;
                    orderInfo.RefundTotalAmount = new decimal(0);
                    orderInfo.CommisTotalAmount = new decimal(0);
                    orderInfo.RefundCommisAmount = new decimal(0);
                    orderInfo.Platform = PlatformType.PC;
                    orderInfo.InvoiceType = InvoiceType.OrdinaryInvoices;
                    orderInfo.InvoiceTitle = "InvoiceTitle";
                    orderInfo.InvoiceContext = "InvoiceContext";
                    orderInfo.DiscountAmount = new decimal(0);
                    orderInfo.ActiveType = OrderInfo.ActiveTypes.None;
                    orderInfo.IntegralDiscount = new decimal(0);

                    context.OrderInfo.Add(orderInfo);
                    context.SaveChanges();

                    MargainBillDetail billdetail = margainbill._MargainBillDetail.LastOrDefault();
                    OrderItemInfo orderInfoitem = new OrderItemInfo();
                    orderInfoitem.OrderId = orderid;
                    orderInfoitem.ShopId = margainbill.ShopId;
                    orderInfoitem.ProductId = billdetail.ProductId;
                    orderInfoitem.ProductName = billdetail.ProductName;
                    orderInfoitem.Quantity = billdetail.Num;
                    orderInfoitem.SalePrice = billdetail.PurchasePrice;
                    orderInfoitem.ReturnQuantity = long.Parse("1");
                    orderInfoitem.CostPrice = 0;

                    orderInfoitem.DiscountAmount = 0;
                    orderInfoitem.RealTotalPrice = 0;
                    orderInfoitem.RefundPrice = 0;
                    orderInfoitem.CommisRate = 0;
                    orderInfoitem.IsLimitBuy = false;

                    context.OrderItemInfo.Add(orderInfoitem);
                    context.SaveChanges();

                    //下单成功 更改状态
                    margainbill.BillStatus = EnumBillStatus.BargainSucceed;
                    context.SaveChanges();

                    transactionScope.Complete();
                }
            }
            catch (Exception)
            {
            }
        }

        public MargainBill GetBillById(long id, long userid)
        {
            MargainBill MargainBillInfo = (
                from P in context.MargainBill
                where P.Id == id
                select P).FirstOrDefault();
            List<MargainBillDetail> _MargainBillDetail = (from q in context.MargainBillDetail
                                                          where q.BillNo == MargainBillInfo.BillNo
                                                          orderby q.CreateDate ascending
                                                          select q).ToList();
            MargainBillInfo._MargainBillDetail = _MargainBillDetail;

            return MargainBillInfo;
        }
        public MargainBill GetBillByNo(string BillNo)
        {
            MargainBill MargainBillInfo = (
                from P in context.MargainBill
                where P.BillNo == BillNo
                select P).FirstOrDefault();

            List<MargainBillDetail> _MargainBillDetail = (from q in context.MargainBillDetail
                                                          where q.BillNo == MargainBillInfo.BillNo
                                                          select q).ToList();
            MargainBillInfo._MargainBillDetail = _MargainBillDetail;

            return MargainBillInfo;
        }


        public PageModel<MargainBill> GetBargain<Tout>(BargainQuery bargainQuery)
        {
            IQueryable<MargainBill> platform = context.MargainBill.AsQueryable<MargainBill>();
            if (bargainQuery.StartDate.HasValue)
            {
                DateTime value = bargainQuery.StartDate.Value;
                platform =
                    from d in platform
                    where d.CreateDate >= value
                    select d;
            }
            if (bargainQuery.EndDate.HasValue)
            {
                DateTime dateTime = bargainQuery.EndDate.Value;
                DateTime dateTime1 = dateTime.AddDays(1).AddSeconds(-1);
                platform =
                    from d in platform
                    where d.CreateDate <= dateTime1
                    select d;
            }
            if (bargainQuery.ShopId.HasValue)
            {
                platform = from item in platform
                           where item.ShopId == bargainQuery.ShopId
                           select item;
            }

            if (!string.IsNullOrWhiteSpace(bargainQuery.BillNo))
            {
                platform = from item in platform
                           where item.BillNo == bargainQuery.BillNo
                           select item;
            }

            if (bargainQuery.BillStatus.HasValue)
            {
                platform = from item in platform
                           where item.BillStatus == bargainQuery.BillStatus
                           select item;
            }
            //筛选掉已删除的询盘
            platform = from item in platform
                       where item.BillStatus != EnumBillStatus.BargainDelete
                       select item;

            int num = platform.Count();
            platform = platform.GetPage(out num, bargainQuery.PageNo, bargainQuery.PageSize);
            foreach (MargainBill list in platform.ToList())
            {
                UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(list.MemberId));
                list.MemberName = memberInfo.UserName;
                ChemCloud_Dictionaries dictionaryInfo = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 1 && m.DValue == list.CoinType.ToString());
                list.CoinTypeName = (dictionaryInfo == null ? "" : dictionaryInfo.DKey);
            }

            return new PageModel<MargainBill>()
            {
                Models = platform,
                Total = num
            };
        }


        public void UpdateBargainPrice(long BargainDId, decimal freight)
        {
            MargainBillDetail bargain = context.MargainBillDetail.FindById<MargainBillDetail>(BargainDId);
            bargain.ShopPirce = freight;
            context.SaveChanges();
            MargainBill MargainBillInfo = (
               from P in context.MargainBill
               where P.BillNo == bargain.BillNo
               select P).FirstOrDefault();
            MargainBillInfo.BillStatus = EnumBillStatus.Bargaining;
            context.SaveChanges();

        }
        public void SellerCloseBargain(string bargainno)
        {
            MargainBill MargainBillInfo = (
                from P in context.MargainBill
                where P.BillNo == bargainno
                select P).FirstOrDefault();
            MargainBillInfo.BillStatus = EnumBillStatus.BargainDelete;
            context.SaveChanges();

        }
        public void SellerBatchCloseBargain(string bargainno)
        {
            bargainno = bargainno.Substring(1, bargainno.Length - 2);
            List<long> listcas = new List<long>();
            string[] array = bargainno.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(array[i]))
                {
                    listcas.Add(long.Parse(array[i]));
                }
            }
            List<MargainBill> list = (
                from P in context.MargainBill
                where listcas.Contains(P.Id)
                select P).ToList();

            foreach (var item in list)
            {
                item.BillStatus = EnumBillStatus.BargainDelete;
                context.SaveChanges();
            }
        }

        public PageModel<MargainBill> GetBill(BargainBillQuery billQueryModel)
        {
            int num;
            IQueryable<MargainBill> billsByQueryModel = GetbillsByQueryModel(billQueryModel, out num);
            foreach (MargainBill list in billsByQueryModel.ToList())
            {
                UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(list.ShopId));
                list.ShopName = (memberInfo == null ? "" : memberInfo.UserName);
                ChemCloud_Dictionaries dictionaryInfo = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 1 && m.DValue == list.CoinType.ToString());
                list.CoinTypeName = (dictionaryInfo == null ? "" : dictionaryInfo.DKey);
            }
            return new PageModel<MargainBill>()
            {
                Models = billsByQueryModel,
                Total = num
            };
        }


        private IQueryable<MargainBill> GetbillsByQueryModel(BargainBillQuery billQueryModel, out int total)
        {
            IQueryable<MargainBill> source = from item in base.context.MargainBill
                                             where item.BillStatus != EnumBillStatus.BargainDelete
                                             select item;

            if (billQueryModel.IsDelete != null)
            {
                source = from item in source
                         where item.IsDelete == billQueryModel.IsDelete
                         select item;
            }

            if (billQueryModel.MemberId.HasValue)
            {
                source = from item in source
                         where item.MemberId == billQueryModel.MemberId
                         select item;
            }

            if (billQueryModel.ShopId.HasValue)
            {
                source = from item in source
                         where item.ShopId == billQueryModel.ShopId
                         select item;
            }
            if (billQueryModel.BillStatus != null)
            {
                source = from item in source
                         where billQueryModel.BillStatus.Contains<EnumBillStatus>(item.BillStatus)
                         select item;
            }

            if (billQueryModel.StartDate.HasValue)
            {
                source = from item in source
                         where item.CreateDate > billQueryModel.StartDate
                         select item;
            }
            if (billQueryModel.EndDate.HasValue)
            {
                source = from item in source
                         where item.CreateDate <= billQueryModel.EndDate
                         select item;
            }

            if (!string.IsNullOrWhiteSpace(billQueryModel.ShopName))
            {
                IQueryable<long> shopIds = from item in base.context.ShopInfo.FindBy(item => item.ShopName.Contains(billQueryModel.ShopName)) select item.Id;
                source = from item in source
                         where shopIds.Contains(item.ShopId)
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(billQueryModel.DeliverType))
            {
                source = from item in source
                         where item.DeliverType == billQueryModel.DeliverType
                         select item;
            }

            if (!string.IsNullOrWhiteSpace(billQueryModel.BillNo))
            {
                source = from item in source
                         where item.BillNo.Contains(billQueryModel.BillNo)
                         select item;
            }

            total = source.Count();
            return source;
        }

        /// <summary>
        /// 结束议价
        /// </summary>
        /// <param name="Id"></OverBargainparam>
        public void OverBargain(long Id)
        {
            MargainBill model = context.MargainBill.FindById<MargainBill>(Id);
            model.BillStatus = EnumBillStatus.BargainOver;
            context.SaveChanges();
        }

        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="model"></param>
        public void SubmitOrder(MargainBill margainbill)
        {
            try
            {
                //订单号
                ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                long orderid = _orderBO.GenerateOrderNumber();

                //收货信息
                ShippingAddressService _ShippingAddressService = new ShippingAddressService();
                ShippingAddressInfo addressinfo = _ShippingAddressService.GetUserShippingAddress(long.Parse(margainbill.RegionId.ToString()));

                //供应商
                ShopService _ShopService = new ShopService();
                string shopname = _ShopService.GetShopName(margainbill.ShopId);

                //会员
                MemberService _MemberService = new MemberService();
                string username = _MemberService.GetMember(margainbill.MemberId) == null ? "" : (_MemberService.GetMember(margainbill.MemberId).UserName == null ? "" : _MemberService.GetMember(margainbill.MemberId).UserName);

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    OrderInfo orderInfo = new OrderInfo();
                    orderInfo.Id = orderid; //订单号
                    orderInfo.ShopId = margainbill.ShopId; //供应商编号ChemCloud_Shops
                    orderInfo.ShopName = shopname;
                    orderInfo.UserId = margainbill.MemberId; //采购商ChemCloud_Members
                    orderInfo.UserName = username;
                    orderInfo.OrderDate = DateTime.Now;//订单日期

                    orderInfo.ExpressCompanyName = margainbill.DeliverType; //物流配送方式
                    orderInfo.Freight = margainbill.DeliverCost; //运费
                    //orderInfo.ShippingDate = margainbill.DeliverDate; //发货日期

                    orderInfo.PaymentTypeName = margainbill.PayMode; //支付方式

                    orderInfo.ProductTotalAmount = margainbill.TotalAmount; //产品金额
                    orderInfo.CoinType = margainbill.CoinType;//货币种类

                    orderInfo.ShipTo = addressinfo.ShipTo;//收货人
                    orderInfo.TopRegionId = addressinfo.RegionId;//RegionId
                    orderInfo.RegionId = addressinfo.RegionId; //RegionId 
                    orderInfo.RegionFullName = addressinfo.RegionFullName; //省市区街道
                    orderInfo.Address = addressinfo.Address;
                    orderInfo.CellPhone = addressinfo.Phone;

                    orderInfo.OrderStatus = OrderInfo.OrderOperateStatus.WaitPay;//订单
                    orderInfo.IsPrinted = false;
                    orderInfo.RefundTotalAmount = new decimal(0);
                    orderInfo.CommisTotalAmount = new decimal(0);
                    orderInfo.RefundCommisAmount = new decimal(0);
                    orderInfo.Platform = PlatformType.PC;

                    if (margainbill.InvoiceType == 0)
                    {
                        orderInfo.InvoiceType = InvoiceType.None;
                    }
                    else if (margainbill.InvoiceType == 1)
                    {
                        orderInfo.InvoiceType = InvoiceType.VATInvoice;
                    }
                    else if (margainbill.InvoiceType == 2)
                    {
                        orderInfo.InvoiceType = InvoiceType.OrdinaryInvoices;
                    }
                    orderInfo.InvoiceTitle = margainbill.InvoiceTitle;
                    orderInfo.InvoiceContext = margainbill.InvoiceContext;

                    orderInfo.DiscountAmount = new decimal(0);
                    orderInfo.ActiveType = OrderInfo.ActiveTypes.None;
                    orderInfo.IntegralDiscount = new decimal(0);

                    orderInfo.IsInsurance = margainbill.IsInsurance; //保险费
                    orderInfo.Insurancefee = margainbill.Insurancefee;
                    orderInfo.Transactionfee = 0;//交易费
                    orderInfo.Counterfee = 0;//手续费

                    context.OrderInfo.Add(orderInfo);
                    context.SaveChanges();

                    MargainBillDetail billdetail = margainbill._MargainBillDetail.LastOrDefault();
                    OrderItemInfo orderInfoitem = new OrderItemInfo();
                    orderInfoitem.OrderId = orderid;
                    orderInfoitem.ShopId = margainbill.ShopId;
                    orderInfoitem.ProductId = billdetail.ProductId;
                    orderInfoitem.ProductName = billdetail.ProductName;
                    orderInfoitem.Quantity = billdetail.Num;
                    orderInfoitem.SalePrice = billdetail.PurchasePrice;
                    orderInfoitem.PackingUnit = billdetail.PackingUnit;
                    orderInfoitem.Purity = billdetail.Purity;
                    orderInfoitem.SpecLevel = billdetail.SpecLevel;
                    orderInfoitem.ReturnQuantity = long.Parse("1");
                    orderInfoitem.CostPrice = 0;

                    orderInfoitem.DiscountAmount = 0;
                    orderInfoitem.RealTotalPrice = 0;
                    orderInfoitem.RefundPrice = 0;
                    orderInfoitem.CommisRate = 0;
                    orderInfoitem.IsLimitBuy = false;

                    context.OrderItemInfo.Add(orderInfoitem);
                    context.SaveChanges();

                    //下单成功 更改状态
                    margainbill.BillStatus = EnumBillStatus.BargainSucceed;
                    context.SaveChanges();

                    transactionScope.Complete();
                }
            }
            catch (Exception)
            {
            }
        }



        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="model"></param>
        public void SubmitOrder(MargainBill margainbill, ShipmentEx shipment)
        {
            try
            {
                //订单号
                ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                long orderid = _orderBO.GenerateOrderNumber();

                //收货信息
                ShippingAddressService _ShippingAddressService = new ShippingAddressService();
                ShippingAddressInfo addressinfo = _ShippingAddressService.GetUserShippingAddress(long.Parse(margainbill.RegionId.ToString()));

                //供应商
                ShopService _ShopService = new ShopService();
                string shopname = _ShopService.GetShopName(margainbill.ShopId);

                //会员
                MemberService _MemberService = new MemberService();
                string username = _MemberService.GetMember(margainbill.MemberId) == null ? "" : (_MemberService.GetMember(margainbill.MemberId).UserName == null ? "" : _MemberService.GetMember(margainbill.MemberId).UserName);

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    OrderInfo orderInfo = new OrderInfo();
                    orderInfo.Id = orderid; //订单号
                    orderInfo.ShopId = margainbill.ShopId; //供应商编号ChemCloud_Shops
                    orderInfo.ShopName = shopname;
                    orderInfo.UserId = margainbill.MemberId; //采购商ChemCloud_Members
                    orderInfo.UserName = username;
                    orderInfo.OrderDate = DateTime.Now;//订单日期
                    orderInfo.ExpressCompanyName = margainbill.DeliverType; //物流配送方式
                    orderInfo.Freight = margainbill.DeliverCost; //运费
                    //orderInfo.ShippingDate = margainbill.DeliverDate; //发货日期
                    orderInfo.PaymentTypeName = margainbill.PayMode; //支付方式
                    orderInfo.ProductTotalAmount = margainbill.TotalAmount; //产品金额
                    orderInfo.CoinType = margainbill.CoinType;//货币种类
                    orderInfo.ShipTo = addressinfo.ShipTo;//收货人
                    orderInfo.TopRegionId = addressinfo.RegionId;//RegionId
                    orderInfo.RegionId = addressinfo.RegionId; //RegionId 
                    orderInfo.RegionFullName = addressinfo.RegionFullName; //省市区街道
                    orderInfo.Address = addressinfo.Address;
                    orderInfo.CellPhone = addressinfo.Phone;
                    orderInfo.OrderStatus = OrderInfo.OrderOperateStatus.WaitPay;//订单
                    orderInfo.IsPrinted = false;
                    orderInfo.RefundTotalAmount = new decimal(0);
                    orderInfo.CommisTotalAmount = new decimal(0);
                    orderInfo.RefundCommisAmount = new decimal(0);
                    orderInfo.Platform = PlatformType.PC;

                    /*发票信息*/
                    if (margainbill.InvoiceType == 0)
                    {
                        orderInfo.InvoiceType = InvoiceType.None;
                    }
                    else if (margainbill.InvoiceType == 1)
                    {
                        orderInfo.InvoiceType = InvoiceType.VATInvoice;
                    }
                    else if (margainbill.InvoiceType == 2)
                    {
                        orderInfo.InvoiceType = InvoiceType.OrdinaryInvoices;
                    }
                    else if (margainbill.InvoiceType == 3)
                    {
                        orderInfo.InvoiceType = InvoiceType.SpecialTicket;
                    }
                    orderInfo.InvoiceTitle = margainbill.InvoiceTitle; //名称
                    orderInfo.InvoiceContext = margainbill.InvoiceContext;//纳税人识别号
                    orderInfo.SellerPhone = margainbill.SellerPhone; //电话
                    orderInfo.SellerRemark = margainbill.SellerRemark; //开户行及账号
                    orderInfo.SellerAddress = margainbill.SellerAddress; //地址
                    /*发票信息*/

                    orderInfo.DiscountAmount = new decimal(0);
                    orderInfo.ActiveType = OrderInfo.ActiveTypes.None;
                    orderInfo.IntegralDiscount = new decimal(0);
                    orderInfo.IsInsurance = margainbill.IsInsurance;
                    orderInfo.Insurancefee = margainbill.Insurancefee;//保险费
                    orderInfo.Transactionfee = 0;//交易费
                    orderInfo.Counterfee = 0;//手续费
                    context.OrderInfo.Add(orderInfo);
                    context.SaveChanges();

                    foreach (MargainBillDetail billdetail in margainbill._MargainBillDetail)
                    {
                        OrderItemInfo orderInfoitem = new OrderItemInfo();
                        orderInfoitem.OrderId = orderid;
                        orderInfoitem.ShopId = margainbill.ShopId;
                        orderInfoitem.ProductId = billdetail.ProductId;
                        orderInfoitem.ProductName = billdetail.ProductName;
                        orderInfoitem.Quantity = billdetail.Num;
                        orderInfoitem.SalePrice = billdetail.PurchasePrice;
                        orderInfoitem.PackingUnit = billdetail.PackingUnit;
                        orderInfoitem.Purity = billdetail.Purity;
                        orderInfoitem.SpecLevel = billdetail.SpecLevel;
                        orderInfoitem.ReturnQuantity = long.Parse("1");
                        orderInfoitem.CostPrice = 0;
                        orderInfoitem.DiscountAmount = 0;
                        orderInfoitem.RealTotalPrice = 0;
                        orderInfoitem.RefundPrice = 0;
                        orderInfoitem.CommisRate = 0;
                        orderInfoitem.IsLimitBuy = false;
                        context.OrderItemInfo.Add(orderInfoitem);
                        context.SaveChanges();
                    }

                    //下单成功 更改状态
                    margainbill.BillStatus = EnumBillStatus.BargainSucceed;
                    context.SaveChanges();

                    //保存物流信息
                    if (shipment != null)
                    {
                        ShipmentService ship = new ShipmentService();
                        ship.SaveShipmentEx(shipment, orderid);

                    }
                    transactionScope.Complete();
                }

                //保存物流信息
                //if (shipment != null)
                //{
                //    ShipmentService ship = new ShipmentService();
                //    ship.SaveShipment(shipment, orderid);
                //}
            }
            catch (DbEntityValidationException ex)
            {
            }
        }


        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="model"></param>
        public void SubmitOrderBatch(MargainBill margainbill)
        {
            try
            {
                //订单号
                ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                long orderid = _orderBO.GenerateOrderNumber();

                //收货信息
                ShippingAddressService _ShippingAddressService = new ShippingAddressService();
                ShippingAddressInfo addressinfo = _ShippingAddressService.GetUserShippingAddress(long.Parse(margainbill.RegionId.ToString()));

                //供应商
                ShopService _ShopService = new ShopService();
                string shopname = _ShopService.GetShopName(margainbill.ShopId);

                //会员
                MemberService _MemberService = new MemberService();
                string username = _MemberService.GetMember(margainbill.MemberId) == null ? "" : (_MemberService.GetMember(margainbill.MemberId).UserName == null ? "" : _MemberService.GetMember(margainbill.MemberId).UserName);

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    OrderInfo orderInfo = new OrderInfo();
                    orderInfo.Id = orderid; //订单号
                    orderInfo.ShopId = margainbill.ShopId; //供应商编号ChemCloud_Shops
                    orderInfo.ShopName = shopname;
                    orderInfo.UserId = margainbill.MemberId; //采购商ChemCloud_Members
                    orderInfo.UserName = username;
                    orderInfo.OrderDate = DateTime.Now;//订单日期

                    orderInfo.ExpressCompanyName = margainbill.DeliverType; //物流配送方式

                    orderInfo.Freight = margainbill.DeliverCost; //运费
                    //orderInfo.ShippingDate = margainbill.DeliverDate; //发货日期
                    //orderInfo.ShippingDate = DateTime.Now.AddDays(3); //发货日期
                    orderInfo.PaymentTypeName = margainbill.PayMode; //支付方式

                    orderInfo.ProductTotalAmount = margainbill.TotalAmount; //产品金额
                    orderInfo.CoinType = margainbill.CoinType;//货币种类

                    orderInfo.ShipTo = addressinfo.ShipTo;//收货人
                    orderInfo.TopRegionId = addressinfo.RegionId;//RegionId
                    orderInfo.RegionId = addressinfo.RegionId; //RegionId 
                    orderInfo.RegionFullName = addressinfo.RegionFullName; //省市区街道
                    orderInfo.Address = addressinfo.Address;
                    orderInfo.CellPhone = addressinfo.Phone;

                    orderInfo.OrderStatus = OrderInfo.OrderOperateStatus.WaitPay;//订单
                    orderInfo.IsPrinted = false;
                    orderInfo.RefundTotalAmount = new decimal(0);
                    orderInfo.CommisTotalAmount = new decimal(0);
                    orderInfo.RefundCommisAmount = new decimal(0);
                    orderInfo.Platform = PlatformType.PC;

                    //orderInfo.InvoiceType = InvoiceType.OrdinaryInvoices;
                    //orderInfo.InvoiceTitle = "InvoiceTitle";
                    //orderInfo.InvoiceContext = "InvoiceContext";

                    if (margainbill.InvoiceType == 0)
                    {
                        orderInfo.InvoiceType = InvoiceType.None;
                    }
                    else if (margainbill.InvoiceType == 1)
                    {
                        orderInfo.InvoiceType = InvoiceType.VATInvoice;
                    }
                    else if (margainbill.InvoiceType == 2)
                    {
                        orderInfo.InvoiceType = InvoiceType.OrdinaryInvoices;
                    }
                    orderInfo.InvoiceTitle = margainbill.InvoiceTitle;
                    orderInfo.InvoiceContext = margainbill.InvoiceContext;

                    orderInfo.DiscountAmount = new decimal(0);
                    orderInfo.ActiveType = OrderInfo.ActiveTypes.None;
                    orderInfo.IntegralDiscount = new decimal(0);

                    orderInfo.IsInsurance = margainbill.IsInsurance; //保险费
                    orderInfo.Insurancefee = margainbill.Insurancefee;
                    orderInfo.Transactionfee = 0;//交易费
                    orderInfo.Counterfee = 0;//手续费

                    context.OrderInfo.Add(orderInfo);
                    context.SaveChanges();

                    MargainBillService _MargainBillService = new MargainBillService();

                    OrderItemInfo orderInfoitem = new OrderItemInfo();
                    foreach (MargainBillDetail billdetail in margainbill._MargainBillDetail)
                    {
                        orderInfoitem.OrderId = orderid;
                        orderInfoitem.ShopId = margainbill.ShopId;
                        orderInfoitem.ProductId = billdetail.ProductId;
                        orderInfoitem.ProductName = billdetail.ProductName;
                        orderInfoitem.Quantity = billdetail.Num;
                        orderInfoitem.SalePrice = billdetail.PurchasePrice;
                        orderInfoitem.PackingUnit = billdetail.PackingUnit;
                        orderInfoitem.SpecLevel = billdetail.SpecLevel;
                        orderInfoitem.Purity = billdetail.Purity;
                        orderInfoitem.ReturnQuantity = long.Parse("1");
                        orderInfoitem.CostPrice = 0;
                        orderInfoitem.DiscountAmount = 0;
                        orderInfoitem.RealTotalPrice = 0;
                        orderInfoitem.RefundPrice = 0;
                        orderInfoitem.CommisRate = 0;
                        orderInfoitem.IsLimitBuy = false;
                        context.OrderItemInfo.Add(orderInfoitem);
                        context.SaveChanges();

                        _MargainBillService.RemoveCartAfterOrder(orderInfo.UserId, billdetail.ProductId); //提交成功后，删除购物车内容
                    }

                    transactionScope.Complete();
                }
            }
            catch (Exception)
            {
            }
        }

        public void RemoveCartAfterOrder(long userid, long productid)
        {

            ShoppingCartItemInfo model = context.ShoppingCartItemInfo_.FirstOrDefault(q => q.UserId == userid && q.ProductId == productid);
            if (model != null)
            {
                context.ShoppingCartItemInfo_.Remove(model);
                context.SaveChanges();
            }
        }



        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="model"></param>
        public bool CartSubmitOrder(List<MargainBill> listbill)
        {
            bool result = false;
            try
            {
                foreach (MargainBill margainbill in listbill)
                {
                    //订单号
                    ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                    long orderid = _orderBO.GenerateOrderNumber();

                    //收货信息
                    ShippingAddressService _ShippingAddressService = new ShippingAddressService();
                    ShippingAddressInfo addressinfo = _ShippingAddressService.GetUserShippingAddress(long.Parse(margainbill.RegionId.ToString()));

                    //供应商
                    ShopService _ShopService = new ShopService();
                    string shopname = _ShopService.GetShopName(margainbill.ShopId);

                    //会员
                    MemberService _MemberService = new MemberService();
                    string username = _MemberService.GetMember(margainbill.MemberId) == null ? "" : (_MemberService.GetMember(margainbill.MemberId).UserName == null ? "" : _MemberService.GetMember(margainbill.MemberId).UserName);

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        OrderInfo orderInfo = new OrderInfo();
                        orderInfo.Id = orderid; //订单号
                        orderInfo.ShopId = margainbill.ShopId; //供应商编号ChemCloud_Shops
                        orderInfo.ShopName = shopname;
                        orderInfo.UserId = margainbill.MemberId; //采购商ChemCloud_Members
                        orderInfo.UserName = username;
                        orderInfo.OrderDate = DateTime.Now;//订单日期

                        orderInfo.ExpressCompanyName = margainbill.DeliverType; //物流配送方式

                        orderInfo.Freight = margainbill.DeliverCost; //运费
                        //orderInfo.ShippingDate = margainbill.DeliverDate; //发货日期
                        //orderInfo.ShippingDate = null; //发货日期  尚未支付
                        orderInfo.PaymentTypeName = margainbill.PayMode; //支付方式

                        orderInfo.ProductTotalAmount = margainbill.TotalAmount; //产品金额
                        orderInfo.CoinType = margainbill.CoinType;//货币种类

                        orderInfo.ShipTo = addressinfo.ShipTo;//收货人
                        orderInfo.TopRegionId = addressinfo.RegionId;//RegionId
                        orderInfo.RegionId = addressinfo.RegionId; //RegionId 
                        orderInfo.RegionFullName = addressinfo.RegionFullName; //省市区街道
                        orderInfo.Address = addressinfo.Address;
                        orderInfo.CellPhone = addressinfo.Phone;
                        orderInfo.OrderStatus = OrderInfo.OrderOperateStatus.WaitPay;//订单
                        orderInfo.IsPrinted = false;
                        orderInfo.RefundTotalAmount = new decimal(0);
                        orderInfo.CommisTotalAmount = new decimal(0);
                        orderInfo.RefundCommisAmount = new decimal(0);
                        orderInfo.Platform = PlatformType.PC;

                        /*发票信息*/
                        if (margainbill.InvoiceType == 0)
                        {
                            orderInfo.InvoiceType = InvoiceType.None;
                        }
                        else if (margainbill.InvoiceType == 1)
                        {
                            orderInfo.InvoiceType = InvoiceType.VATInvoice;
                        }
                        else if (margainbill.InvoiceType == 2)
                        {
                            orderInfo.InvoiceType = InvoiceType.OrdinaryInvoices;
                        }
                        else if (margainbill.InvoiceType == 3)
                        {
                            orderInfo.InvoiceType = InvoiceType.SpecialTicket;
                        }
                        orderInfo.InvoiceTitle = margainbill.InvoiceTitle; //名称
                        orderInfo.InvoiceContext = margainbill.InvoiceContext;//纳税人识别号
                        orderInfo.SellerPhone = margainbill.SellerPhone; //电话
                        orderInfo.SellerRemark = margainbill.SellerRemark; //开户行及账号
                        orderInfo.SellerAddress = margainbill.SellerAddress; //地址
                        /*发票信息*/

                        orderInfo.DiscountAmount = new decimal(0);
                        orderInfo.ActiveType = OrderInfo.ActiveTypes.None;
                        orderInfo.IntegralDiscount = new decimal(0);

                        orderInfo.IsInsurance = margainbill.IsInsurance; //保险费
                        orderInfo.Insurancefee = margainbill.Insurancefee;
                        orderInfo.Transactionfee = 0;//交易费
                        orderInfo.Counterfee = 0;//手续费

                        context.OrderInfo.Add(orderInfo);
                        context.SaveChanges();
                        MargainBillService _MargainBillService = new MargainBillService();
                        OrderItemInfo orderInfoitem = new OrderItemInfo();
                        foreach (MargainBillDetail billdetail in margainbill._MargainBillDetail)
                        {
                            orderInfoitem.OrderId = orderid;
                            orderInfoitem.ShopId = margainbill.ShopId;
                            orderInfoitem.ProductId = billdetail.ProductId;
                            orderInfoitem.ProductName = billdetail.ProductName;
                            orderInfoitem.Quantity = billdetail.Num;
                            orderInfoitem.SalePrice = billdetail.PurchasePrice;
                            orderInfoitem.PackingUnit = billdetail.PackingUnit;
                            orderInfoitem.SpecLevel = billdetail.SpecLevel;
                            orderInfoitem.Purity = billdetail.Purity;
                            orderInfoitem.ReturnQuantity = long.Parse("1");
                            orderInfoitem.CostPrice = 0;
                            orderInfoitem.DiscountAmount = 0;
                            orderInfoitem.RealTotalPrice = 0;
                            orderInfoitem.RefundPrice = 0;
                            orderInfoitem.CommisRate = 0;
                            orderInfoitem.IsLimitBuy = false;
                            context.OrderItemInfo.Add(orderInfoitem);
                            context.SaveChanges();
                            _MargainBillService.RemoveCartAfterOrder(orderInfo.UserId, billdetail.ProductId); //提交成功后，删除购物车内容
                        }
                        transactionScope.Complete();
                    }
                }
                result = true;
            }
            catch (Exception)
            {
                return false;
            }

            return result;
        }
    }
}
