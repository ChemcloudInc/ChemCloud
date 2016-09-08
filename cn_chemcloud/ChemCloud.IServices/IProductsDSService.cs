using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
    public interface IProductsDSService : IService, IDisposable
	{
        /// <summary>
        /// ��Ӵ��ۼ�¼
        /// </summary>
        /// <param name="query">����</param>
        /// <returns></returns>
        bool AddProductsDS(ProductsDSQuery query);
        /// <summary>
        /// �޸Ĵ��ۼ�¼
        /// </summary>
        /// <param name="query">����</param>
        /// <returns></returns>
        bool UpdateProductsDS(ProductsDSQuery query);
        /// <summary>
        /// ɾ�����ۼ�¼
        /// </summary>
        /// <param name="query">����</param>
        /// <returns></returns>
        bool DelProductsDS(ProductsDSQuery query);
        /// <summary>
        /// ��ȡ������¼
        /// </summary>
        /// <param name="query">����</param>
        /// <returns></returns>
        ProductsDS GetProductsDSbyId(long id);
        ProductsDS GetProductsDS(long shopid,long productid);
        /// <summary>
        /// ��ȡlist
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PageModel<ProductsDS> GetProductsDSList(ProductsDSQuery query);

	}
}