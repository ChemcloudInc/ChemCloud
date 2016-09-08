using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class CartController : BaseWebController
    {
        public CartController()
        {
        }

        public ActionResult AddedToCart(string skuId)
        {
            ProductInfo productInfo = null;
            SKUInfo sKUInfo = null;
            long num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            try
            {
                CartHelper cartHelper = new CartHelper();
                string str = skuId;
                char[] chrArray = new char[] { '\u005F' };
                string str1 = str.Split(chrArray)[0];
                ViewBag.ProductId = str1;
                IProductService productService = ServiceHelper.Create<IProductService>();
                ShoppingCartInfo cart = cartHelper.GetCart(num);
                IEnumerable<CartItemModel> cartItemModels = cart.Items.Select<ShoppingCartItem, CartItemModel>((ShoppingCartItem item) =>
                {
                    productInfo = productService.GetProduct(item.ProductId);
                    sKUInfo = productService.GetSku(item.SkuId);
                    return new CartItemModel()
                    {
                        skuId = item.SkuId,
                        id = productInfo.Id,
                        imgUrl = string.Concat(productInfo.ImagePath, "/1_50.png"),
                        name = productInfo.ProductName,
                        price = sKUInfo.SalePrice,
                        count = item.Quantity
                    };
                });
                base.ViewBag.Current = cartItemModels.FirstOrDefault((CartItemModel item) => item.skuId == skuId);
                base.ViewBag.Others =
                    from item in cartItemModels
                    where item.skuId != skuId
                    select item;
                base.ViewBag.Amount = cartItemModels.Sum<CartItemModel>((CartItemModel item) => item.price * item.count);
                base.ViewBag.TotalCount = cartItemModels.Sum<CartItemModel>((CartItemModel item) => item.count);
            }
            catch
            {
            }
            return View("AddToCart");
        }

        [HttpPost]
        //public JsonResult AddProductToCart(string skuId, int count)
        public JsonResult AddProductToCart(long productid, string packingunit, string purity, int productnum, decimal prodcutprice, string prodcutcointypename, long shopid, string speclevel,
          string distributiontype, decimal distributioncost, DateTime deliverydate, int regionid, string deliveryaddress, string paymode, decimal Insurancefee,
            int InvoiceType = 0, string InvoiceTitle = "", string InvoiceContext = "")
        {
            JsonResult jsonResult;
            long num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            try
            {
                //(new CartHelper()).AddToCart(skuId, count, num);
                (new CartHelper()).AddToCartNew(productid, packingunit, purity, productnum, prodcutprice, prodcutcointypename, num, shopid, speclevel,
                    distributiontype, distributioncost, deliverydate, regionid, deliveryaddress, paymode, Insurancefee, InvoiceType, InvoiceTitle, InvoiceContext);
                jsonResult = Json(new { success = true });
            }
            catch
            {
                jsonResult = Json(new { success = false });
            }
            return jsonResult;
        }

        public ActionResult AddToCart(string skuId, int count)
        {
            long num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            try
            {
                (new CartHelper()).AddToCart(skuId, count, num);
            }
            catch
            {
            }
            return RedirectToAction("AddedToCart", new { skuId = skuId });
        }

        public ActionResult BatchAddToCart(string skuIds, string counts)
        {
            string[] strArrays = skuIds.Split(new char[] { ',' });
            char[] chrArray = new char[] { ',' };
            IEnumerable<int> nums =
                from item in counts.Split(chrArray)
                select int.Parse(item);
            CartHelper cartHelper = new CartHelper();
            long num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            for (int i = 0; i < strArrays.Count(); i++)
            {
                cartHelper.AddToCart(strArrays.ElementAt<string>(i), nums.ElementAt<int>(i), num);
            }
            return RedirectToAction("cart");
        }

        [HttpPost]
        public JsonResult BatchRemoveFromCart(string skuIds)
        {
            long num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            string[] strArrays = skuIds.Split(new char[] { ',' });
            (new CartHelper()).RemoveFromCart(strArrays, num);
            return Json(new { success = true });
        }

        public ActionResult Cart()
        {
            ViewBag.Logo = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings().Logo;
            ViewBag.Step = 1;
            long num = num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            ViewBag.Member = num;
            return View();
        }

        [HttpPost]
        public JsonResult GetCartProducts()
        {
            long num;
            num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            ShoppingCartInfo cart = (new CartHelper()).GetCart(num);
            IProductService productService = ServiceHelper.Create<IProductService>();
            IShopService shopService = ServiceHelper.Create<IShopService>();
            var collection = cart.Items.Select((ShoppingCartItem item) =>
            {
                ProductInfo product = productService.GetProduct(item.ProductId);
                ShopInfo shop = shopService.GetShop(product.ShopId, false);
                if (shop == null)
                {
                    return null;
                }
                return new
                {
                    cartItemId = item.Id,
                    ProductId = product.Id,
                    imgUrl = ServiceHelper.Create<IProductService>().GetProduct(item.ProductId) == null ? "https://pubchem.ncbi.nlm.nih.gov/image/imgsrv.fcgi?cid=0&t=l" : "https://pubchem.ncbi.nlm.nih.gov/image/imgsrv.fcgi?cid=" + ServiceHelper.Create<IProductService>().GetProduct(item.ProductId).Pub_CID + "&t=l",
                    name = product.ProductName,
                    productstatus = product.SaleStatus,
                    productauditstatus = product.AuditStatus,
                    Quantity = item.Quantity,
                    shopId = shop.Id,
                    shopName = shop.ShopName,
                    productcode = product.ProductCode,
                    PackingUnit = item.PackingUnit,
                    Purity = item.Purity,
                    ProductTotalAmount = item.ProductTotalAmount,
                    CoinType = item.CoinType,
                    ShopId = item.ShopId,
                    SpecLevel = item.SpecLevel
                };
            }).Where((p) => p != null).OrderBy((s) => s.shopId);
            var variable = new
            {
                products = collection,
                amount = collection.Where((item) =>
                {
                    if (item.productstatus != ProductInfo.ProductSaleStatus.OnSale)
                    {
                        return false;
                    }
                    return item.productauditstatus != ProductInfo.ProductAuditStatus.InfractionSaleOff;
                }).Sum((item) => item.ProductTotalAmount),
                //totalCount = collection.Where((item) =>
                //{
                //    if (item.productstatus != ProductInfo.ProductSaleStatus.OnSale)
                //    {
                //        return false;
                //    }
                //    return item.productauditstatus != ProductInfo.ProductAuditStatus.InfractionSaleOff;
                //}).GroupBy((s) => s.shopId).ToList().Count

                totalCount = ServiceHelper.Create<ICartService>().GetShoppingCartItemInfo(num).Sum(q => q.Quantity)
            };
            return Json(variable);
        }

        [HttpPost]
        public JsonResult GetSkuByID(string skuId)
        {
            SKUInfo skuByID = ServiceHelper.Create<IOrderService>().GetSkuByID(skuId);
            var variable = new { Color = skuByID.Color, Size = skuByID.Size, Version = skuByID.Version };
            return Json(variable);
        }

        [HttpPost]
        public JsonResult RemoveFromCart(string skuId)
        {
            long num;
            num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            (new CartHelper()).RemoveFromCart(skuId, num);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult UpdateCartItem(string skuId, int count)
        {
            long num;
            num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            (new CartHelper()).UpdateCartItem(skuId, count, num);
            return Json(new { success = true });
        }


        [HttpPost]
        public JsonResult addtocart(string json)
        {
            JsonResult jsonResult;
            long num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            try
            {
                bool result = ServiceHelper.Create<ICartService>().AddtoCart(json, num);
                jsonResult = Json(new { success = result });
            }
            catch
            {
                jsonResult = Json(new { success = false });
            }
            return jsonResult;
        }

    }
}