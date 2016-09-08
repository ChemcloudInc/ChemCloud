using ChemCloud.Core;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class CartService : ServiceBase, ICartService, IService, IDisposable
    {
        public CartService()
        {
        }

        public void AddToCart(string skuId, int count, long memberId)
        {
            if (count != 0)
            {
                CheckCartItem(skuId, count, memberId);
                ShoppingCartItemInfo shoppingCartItemInfo =
                    context.ShoppingCartItemInfo_.FirstOrDefault((ShoppingCartItemInfo item) => item.UserId == memberId && (item.SkuId == skuId));
                if (shoppingCartItemInfo != null)
                {
                    ShoppingCartItemInfo quantity = shoppingCartItemInfo;
                    quantity.Quantity = quantity.Quantity + count;
                }
                else if (count > 0)
                {
                    string str = skuId;
                    char[] chrArray = new char[] { '\u005F' };
                    long num = long.Parse(str.Split(chrArray)[0]);
                    DbSet<ShoppingCartItemInfo> shoppingCartItemInfos = context.ShoppingCartItemInfo_;
                    ShoppingCartItemInfo shoppingCartItemInfo1 = new ShoppingCartItemInfo()
                    {
                        UserId = memberId,
                        Quantity = count,
                        SkuId = skuId,
                        ProductId = num,
                        AddTime = DateTime.Now
                    };
                    shoppingCartItemInfos.Add(shoppingCartItemInfo1);
                }
                context.SaveChanges();
            }
        }

        public void AddToCart(IEnumerable<ShoppingCartItem> cartItems, long memberId)
        {
            foreach (ShoppingCartItem list in cartItems.ToList())
            {
                CheckCartItem(list.SkuId, list.Quantity, memberId);
                ShoppingCartItemInfo shoppingCartItemInfo = context.ShoppingCartItemInfo_.FirstOrDefault((ShoppingCartItemInfo item) => item.UserId == memberId && (item.SkuId == list.SkuId));
                if (shoppingCartItemInfo == null)
                {
                    string skuId = list.SkuId;
                    char[] chrArray = new char[] { '\u005F' };
                    long num = long.Parse(skuId.Split(chrArray)[0]);
                    DbSet<ShoppingCartItemInfo> shoppingCartItemInfos = context.ShoppingCartItemInfo_;
                    ShoppingCartItemInfo shoppingCartItemInfo1 = new ShoppingCartItemInfo()
                    {
                        UserId = memberId,
                        Quantity = list.Quantity,
                        SkuId = list.SkuId,
                        ProductId = num,
                        AddTime = DateTime.Now
                    };
                    shoppingCartItemInfos.Add(shoppingCartItemInfo1);
                }
                else
                {
                    ShoppingCartItemInfo quantity = shoppingCartItemInfo;
                    quantity.Quantity = quantity.Quantity + list.Quantity;
                }
            }
            context.SaveChanges();
        }

        private void CheckCartItem(string skuId, int count, long memberId)
        {
            if (string.IsNullOrWhiteSpace(skuId))
            {
                throw new InvalidPropertyException("SKUId不能为空");
            }
            if (count < 0)
            {
                throw new InvalidPropertyException("产品数量不能小于0");
            }
            if (context.UserMemberInfo.FirstOrDefault((UserMemberInfo item) => item.Id == memberId) == null)
            {
                throw new InvalidPropertyException(string.Concat("会员Id", memberId, "不存在"));
            }
        }

        public void ClearCart(long memeberId)
        {
            context.ShoppingCartItemInfo_.OrderBy((ShoppingCartItemInfo item) => item.UserId == memeberId);
            context.SaveChanges();
        }

        public void DeleteCartItem(string skuId, long memberId)
        {
            ShoppingCartItemInfo model = context.ShoppingCartItemInfo_.FindById<ShoppingCartItemInfo>(long.Parse(skuId));
            if (model != null)
            {
                context.ShoppingCartItemInfo_.Remove(model);
                context.SaveChanges();
            }
        }

        public void DeleteCartItem(IEnumerable<string> skuIds, long memberId)
        {
            IQueryable<ShoppingCartItemInfo> _ShoppingCartItemInfo
               = context.ShoppingCartItemInfo_.FindBy((ShoppingCartItemInfo item) => skuIds.Contains(item.Id.ToString()));
            context.ShoppingCartItemInfo_.RemoveRange(_ShoppingCartItemInfo);
            context.SaveChanges();
        }

        public ShoppingCartInfo GetCart(long memeberId)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo()
            {
                MemberId = memeberId
            };
            IQueryable<ShoppingCartItemInfo> shoppingCartItemInfo =
                from item in context.ShoppingCartItemInfo_
                where item.UserId == memeberId && (item.CoinType.Equals("CNY") || item.CoinType.Equals("RMB"))
                select item;
            shoppingCartInfo.Items =
                from item in shoppingCartItemInfo
                select new ShoppingCartItem()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    AddTime = item.AddTime,
                    PackingUnit = item.PackingUnit,
                    SpecLevel = item.SpecLevel,
                    Purity = item.Purity,
                    ProductTotalAmount = item.ProductTotalAmount,
                    CoinType = item.CoinType,
                    ShopId = item.ShopId,
                    ExpressCompanyName = item.ExpressCompanyName,
                    Freight = item.Freight,
                    RegionId = item.RegionId,
                    ShippingAddress = item.ShippingAddress,
                    PaymentTypeName = item.PaymentTypeName,
                    Insurancefee = item.Insurancefee,
                    InvoiceType = item.InvoiceType,
                    InvoiceTitle = item.InvoiceTitle,
                    InvoiceContext = item.InvoiceContext
                };
            return shoppingCartInfo;
        }

        public ShoppingCartInfo GetCart(long memeberId, long id)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo()
            {
                MemberId = memeberId
            };
            IQueryable<ShoppingCartItemInfo> shoppingCartItemInfo =
                from item in context.ShoppingCartItemInfo_
                where item.UserId == memeberId && item.ShopId == id
                select item;
            shoppingCartInfo.Items =
                from item in shoppingCartItemInfo
                select new ShoppingCartItem()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    AddTime = item.AddTime,
                    PackingUnit = item.PackingUnit,
                    SpecLevel = item.SpecLevel,
                    Purity = item.Purity,
                    ProductTotalAmount = item.ProductTotalAmount,
                    CoinType = item.CoinType,
                    ShopId = item.ShopId,
                    ExpressCompanyName = item.ExpressCompanyName,
                    Freight = item.Freight,
                    RegionId = item.RegionId,
                    ShippingAddress = item.ShippingAddress,
                    PaymentTypeName = item.PaymentTypeName,
                    Insurancefee = item.Insurancefee,
                    InvoiceType = item.InvoiceType,
                    InvoiceTitle = item.InvoiceTitle,
                    InvoiceContext = item.InvoiceContext
                };
            return shoppingCartInfo;
        }

        public IQueryable<ShoppingCartItem> GetCartItems(IEnumerable<long> cartItemIds)
        {
            return
                from item in context.ShoppingCartItemInfo_.FindBy((ShoppingCartItemInfo item) => cartItemIds.Contains(item.Id))
                select new ShoppingCartItem()
                {
                    Id = item.Id,
                    SkuId = item.SkuId,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    AddTime = item.AddTime
                };
        }

        public IQueryable<ShoppingCartItem> GetCartItems(IEnumerable<string> skuIds, long memberId)
        {
            return
                from item in context.ShoppingCartItemInfo_
                where item.UserId == memberId && skuIds.Contains<string>(item.SkuId)
                select new ShoppingCartItem()
                {
                    Id = item.Id,
                    SkuId = item.SkuId,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    AddTime = item.AddTime
                };
        }

        public void UpdateCart(string skuId, int count, long memberId)
        {
            CheckCartItem(skuId, count, memberId);
            ShoppingCartItemInfo shoppingCartItemInfo = context.ShoppingCartItemInfo_.FirstOrDefault((ShoppingCartItemInfo item) => item.UserId == memberId && (item.SkuId == skuId));
            if (shoppingCartItemInfo != null)
            {
                if (count != 0)
                {
                    shoppingCartItemInfo.Quantity = count;
                }
                else
                {
                    DbSet<ShoppingCartItemInfo> shoppingCartItemInfos = context.ShoppingCartItemInfo_;
                    object[] id = new object[] { shoppingCartItemInfo.Id };
                    shoppingCartItemInfos.Remove(id);
                }
            }
            else if (count > 0)
            {
                string str = skuId;
                char[] chrArray = new char[] { '\u005F' };
                long num = long.Parse(str.Split(chrArray)[0]);
                DbSet<ShoppingCartItemInfo> shoppingCartItemInfo1 = context.ShoppingCartItemInfo_;
                ShoppingCartItemInfo shoppingCartItemInfo2 = new ShoppingCartItemInfo()
                {
                    UserId = memberId,
                    Quantity = count,
                    SkuId = skuId,
                    ProductId = num,
                    AddTime = DateTime.Now
                };
                shoppingCartItemInfo1.Add(shoppingCartItemInfo2);
            }
            context.SaveChanges();
        }


        //public void AddToCartNew(string skuId, int count, long memberId)
        public void AddToCartNew(long productid, string packingunit, string purity, int productnum, decimal prodcutprice, string cointype, long memberId, long shopid, string speclevel,
            string distributiontype, decimal distributioncost, DateTime deliverydate, int regionid, string deliveryaddress, string paymode,
            decimal Insurancefee, int InvoiceType = 0, string InvoiceTitle = "", string InvoiceContext = "")
        {
            if (productid != 0)
            {
                ShoppingCartItemInfo shoppingCartItemInfo =
                    context.ShoppingCartItemInfo_.FirstOrDefault(
                    (ShoppingCartItemInfo item) => item.UserId == memberId
                        && item.ProductId == productid && item.PackingUnit.Equals(packingunit) && item.Purity.Equals(purity)
                        && item.Quantity.Equals(productnum) && item.ProductTotalAmount == prodcutprice && item.CoinType.Equals(cointype)
                        );

                if (shoppingCartItemInfo != null)
                {
                    ShoppingCartItemInfo quantity = shoppingCartItemInfo;
                    quantity.Quantity = quantity.Quantity + productnum;
                    quantity.ProductTotalAmount = quantity.ProductTotalAmount + prodcutprice;
                }
                else
                {

                    ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                    long orderid = _orderBO.GenerateOrderNumber();

                    DbSet<ShoppingCartItemInfo> shoppingCartItemInfos = context.ShoppingCartItemInfo_;
                    ShoppingCartItemInfo shoppingCartItemInfo1 = new ShoppingCartItemInfo()
                    {
                        UserId = memberId,
                        ProductId = productid,
                        Quantity = productnum,
                        AddTime = DateTime.Now,
                        PackingUnit = packingunit,
                        Purity = purity,
                        ProductTotalAmount = prodcutprice,
                        CoinType = cointype,
                        ShopId = shopid,
                        SpecLevel = speclevel,
                        ExpressCompanyName = distributiontype,
                        Freight = distributioncost,
                        ShippingDate = deliverydate,
                        RegionId = regionid,
                        ShippingAddress = deliveryaddress,
                        PaymentTypeName = paymode,
                        Insurancefee = Insurancefee,
                        CartNo = orderid,
                        InvoiceType = InvoiceType,
                        InvoiceTitle = InvoiceTitle,
                        InvoiceContext = InvoiceContext
                    };
                    shoppingCartItemInfos.Add(shoppingCartItemInfo1);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="jsoncart"></param>
        public bool AddtoCart(string jsoncart, long memberid)
        {
            bool result = false;
            try
            {
                cartlist _catlist = Newtonsoft.Json.JsonConvert.DeserializeObject<cartlist>(jsoncart);
                long shopid = 0;
                long productid = 0;
                if (_catlist != null)
                {
                    shopid = _catlist.shopid;
                    productid = _catlist.productid;
                    if (_catlist.cartproductlist.Count > 0)
                    {
                        foreach (cartproductlist _cartproductlist in _catlist.cartproductlist)
                        {
                            ShoppingCartItemInfo shoppingCartItemInfo =
                                context.ShoppingCartItemInfo_.FirstOrDefault(
                                (ShoppingCartItemInfo item) => item.UserId == memberid
                                    && item.ProductId == productid && item.PackingUnit.Equals(_cartproductlist.PackingUnit) && item.Purity.Equals(_cartproductlist.Purity)
                                    && item.Quantity.Equals(_cartproductlist.Num) && item.ProductTotalAmount == _cartproductlist.PurchasePrice && item.CoinType.Equals(_cartproductlist.CoinType)
                                    );
                            if (shoppingCartItemInfo != null)
                            {
                                ShoppingCartItemInfo quantity = shoppingCartItemInfo;
                                quantity.Quantity = quantity.Quantity + _cartproductlist.Num;
                                quantity.ProductTotalAmount = quantity.ProductTotalAmount + _cartproductlist.PurchasePrice;
                            }
                            else
                            {

                                ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                                long orderid = _orderBO.GenerateOrderNumber();

                                DbSet<ShoppingCartItemInfo> shoppingCartItemInfos = context.ShoppingCartItemInfo_;
                                ShoppingCartItemInfo shoppingCartItemInfo1 = new ShoppingCartItemInfo()
                                {
                                    UserId = memberid,
                                    ProductId = productid,
                                    Quantity = _cartproductlist.Num,
                                    AddTime = DateTime.Now,
                                    PackingUnit = _cartproductlist.PackingUnit,
                                    Purity = _cartproductlist.Purity,
                                    ProductTotalAmount = _cartproductlist.PurchasePrice,
                                    CoinType = _cartproductlist.CoinType,
                                    ShopId = shopid,
                                    SpecLevel = _cartproductlist.SpecLevel,
                                    ExpressCompanyName = "0",
                                    Freight = 0,
                                    ShippingDate = DateTime.Now,
                                    RegionId = 0,
                                    ShippingAddress = "",
                                    PaymentTypeName = "",
                                    Insurancefee = 0,
                                    CartNo = orderid,
                                    InvoiceType = 0,
                                    InvoiceTitle = "",
                                    InvoiceContext = ""
                                };
                                shoppingCartItemInfos.Add(shoppingCartItemInfo1);
                            }
                            context.SaveChanges();
                            result = true;
                        }
                    }
                }
            }
            catch (Exception) { }
            return result;
        }

        /// <summary>
        /// 获取采购商购物车
        /// </summary>
        /// <param name="userid">采购商ＩＤ</param>
        /// <returns></returns>
        public List<ShoppingCartItemInfo> GetShoppingCartItemInfo(long userid)
        {
            return (from p in context.ShoppingCartItemInfo_ where p.UserId.Equals(userid) select p).ToList();
        }
    }

    public class cartlist
    {
        public long shopid { get; set; }
        public long productid { get; set; }
        public List<cartproductlist> cartproductlist { get; set; }
    }

    public class cartproductlist
    {
        //"ProductId": "1112",
        public long ProductId { get; set; }
        //"ProductName": "16-羟基棕榈酸",
        public string ProductName { get; set; }
        //"Num": "1",
        public int Num { get; set; }
        //"PurchasePrice": "1.00",
        public decimal PurchasePrice { get; set; }
        //"CoinType": "CNY",
        public string CoinType { get; set; }
        //"PackingUnit": "1-g",
        public string PackingUnit { get; set; }
        //"SpecLevel": "1",
        public string SpecLevel { get; set; }
        //"Purity": "11"
        public string Purity { get; set; }
    }

}

