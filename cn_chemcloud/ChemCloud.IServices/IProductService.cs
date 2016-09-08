using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface IProductService : IService, IDisposable
    {
        bool AddFavorites(long productId, long userId);
        void UpdateStatus(long Id, ProductInfo.ProductAuditStatus status, string comments = "");
        void AddBrowsingProduct(BrowsingHistoryInfo info);

        void AddFavorite(long productId, long userId, out int status);

        void AddProduct(ProductInfo model);
        void AddProductSpec(ProductInfo model, long id);

        bool AddProductSpecs(ProductSpec model);

        void AddSKU(ProductInfo pInfo);

        bool AddProducts(ProductInfo model);

        bool ApplyForSale(long id);

        void AuditProduct(long productId, ProductInfo.ProductAuditStatus auditStatus, string message);

        void AuditProducts(IEnumerable<long> productIds, ProductInfo.ProductAuditStatus auditStatus, string message);

        void BindTemplate(long? topTemplateId, long? bottomTemplateId, IEnumerable<long> productIds);

        void CancelConcernProducts(IEnumerable<long> ids, long userId);

        void DeleteFavorite(long productId, long userId);

        void DeleteProduct(long id);

        void DeleteProduct(IEnumerable<long> ids, long shopId);

        AttributeInfo GetAttributeInfo(long attrId);

        IQueryable<BrowsingHistoryInfo> GetBrowsingProducts(long userId);

        ProductInfo GetProductByCondition(long shopid, string productcode);

        ProductInfo.ProductEditStatus GetEditStatus(long id, ProductInfo model);

        decimal GetFreight(IEnumerable<long> productIds, IEnumerable<int> counts, int cityId);

        decimal GetFreight(IEnumerable<string> skuIds, IEnumerable<int> counts, int cityId);

        List<decimal> GetFreights(IEnumerable<string> skuIds, IEnumerable<int> counts, int cityId);

        IQueryable<ProductInfo> GetHotConcernedProduct(long shopId, int count = 5);

        IQueryable<ProductInfo> GetHotSaleProduct(long shopId, int count = 5);

        IQueryable<ProductInfo> GetNewSaleProduct(long shopId, int count = 5);

        long GetNextProductId();

        IQueryable<ProductInfo> GetPlatHotSaleProduct(int count = 3);

        IQueryable<ProductInfo> GetPlatHotSaleProductByNearShop(int count, long userId);

        ProductInfo GetProduct(long id);
        ProductInfo GetProductByPlatCode(string Code);

        ProductInfo GetProductSpec(long id);

        IQueryable<ProductAttributeInfo> GetProductAttribute(long productId);

        IQueryable<ProductInfo> GetProductByIds(IEnumerable<long> ids);

        PageModel<ProductInfo> GetProducts(ProductQuery productQueryModel);

        PageModel<ProductInfo> GetPlatProducts(PlatProductQuery productQueryModel);
        PageModel<ProductInfo> GetSellerProducts(SellerProductQuery productQueryModel);

        IQueryable<ProductShopCategoryInfo> GetProductShopCategories(long productId);

        ProductVistiInfo GetProductVistInfo(long pId, ICollection<ProductVistiInfo> pInfo = null);

        IQueryable<SellerSpecificationValueInfo> GetSellerSpecifications(long shopId, long typeId);

        int GetShopAllProducts(long shopId);

        int GetShopOnsaleProducts(long shopId);

        SKUInfo GetSku(string skuId);

        IQueryable<SKUInfo> GetSKUs(long productId);

        IQueryable<SKUInfo> GetSKUs(IEnumerable<long> productIds);

        List<FavoriteInfo> GetUserAllConcern(long userId);

        PageModel<FavoriteInfo> GetUserConcernProducts(long userId, int pageNo, int pageSize);

        bool IsFavorite(long productId, long userId);

        void LogProductVisti(long productId);

        void OnSale(long id, long shopId);

        void OnSale(IEnumerable<long> ids, long shopId);

        void SaleOff(long id, long shopId);

        void SaleOff(IEnumerable<long> ids, long shopId);

        void SaveSellerSpecifications(List<SellerSpecificationValueInfo> info);

        PageModel<ProductInfo> SearchProduct(ProductSearch search);

        void UpdateProduct(ProductInfo model);

        void UpdateProductImagePath(long pId, string path);

        void UpdateSalesCount(string skuId, int addSalesCount);

        void UpdateStock(string skuId, long stockChange);

        ProductInfo GetProductByCasno(string CASNo);

        //首页推荐产品查询
        IQueryable<ProductInfo> RecommendProductList();

        bool IsExitsProductCode(long shopid, string productcode);

        List<ProductSpec> GetDetial(int type, long id);

        bool IsExitsProductCode(string productCode, long shopId);

        decimal GetProductPrice(long pid);

        Result_List<ProductInfoSpec> Get_Products_Top(int count);

        Result_Model<ProductInfo> Get_ProductInfo_ById(long id);

        Result_List<ProductSpec> Get_ProductSpec_ById(long id);

        PageModel<ProductInfo> GetProductsToSearch(ProductQuery productQueryModel);


        void UpdateSpecicalOffer(long id, long shopId, decimal status);
        void UpdateSpecicalOffer(IEnumerable<long> ids, long shopId, decimal status);
        bool UpdateSpecicalOfferOK(long id, decimal status);
    }
}