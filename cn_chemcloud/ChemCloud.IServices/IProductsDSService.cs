using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
    public interface IProductsDSService : IService, IDisposable
	{
        /// <summary>
        /// 添加代售记录
        /// </summary>
        /// <param name="query">条件</param>
        /// <returns></returns>
        bool AddProductsDS(ProductsDSQuery query);
        /// <summary>
        /// 修改代售记录
        /// </summary>
        /// <param name="query">条件</param>
        /// <returns></returns>
        bool UpdateProductsDS(ProductsDSQuery query);
        /// <summary>
        /// 删除代售记录
        /// </summary>
        /// <param name="query">条件</param>
        /// <returns></returns>
        bool DelProductsDS(ProductsDSQuery query);
        /// <summary>
        /// 获取单个记录
        /// </summary>
        /// <param name="query">条件</param>
        /// <returns></returns>
        ProductsDS GetProductsDSbyId(long id);
        ProductsDS GetProductsDS(long shopid,long productid);
        /// <summary>
        /// 获取list
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PageModel<ProductsDS> GetProductsDSList(ProductsDSQuery query);

	}
}