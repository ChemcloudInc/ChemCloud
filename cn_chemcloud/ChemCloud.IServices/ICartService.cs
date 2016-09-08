using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface ICartService : IService, IDisposable
    {
        void AddToCart(string skuId, int count, long memberId);

        void AddToCart(IEnumerable<ShoppingCartItem> cartItems, long memberId);

        void ClearCart(long memeberId);

        void DeleteCartItem(string skuId, long memberId);

        void DeleteCartItem(IEnumerable<string> skuIds, long memberId);

        ShoppingCartInfo GetCart(long memeberId);

        ShoppingCartInfo GetCart(long memeberId, long id);

        IQueryable<ShoppingCartItem> GetCartItems(IEnumerable<long> cartItemIds);

        IQueryable<ShoppingCartItem> GetCartItems(IEnumerable<string> skuIds, long memberId);

        void UpdateCart(string skuId, int count, long memberId);

        void AddToCartNew(long productid, string packingunit, string purity, int productnum, decimal prodcutprice, string cointype, long memberId, long shopid, string speclevel, string distributiontype, decimal distributioncost, DateTime deliverydate, int regionid, string deliveryaddress, string paymode, decimal Insurancefee, int InvoiceType = 0, string InvoiceTitle = "", string InvoiceContext = "");


        bool AddtoCart(string jsoncart, long memberid);

        List<ShoppingCartItemInfo> GetShoppingCartItemInfo(long userid);
    }
}