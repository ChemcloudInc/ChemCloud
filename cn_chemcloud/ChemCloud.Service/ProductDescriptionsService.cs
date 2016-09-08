using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class ProductDescriptionsService : ServiceBase, IProductDescriptionsService, IService, IDisposable
    {
        /*根据产品ＩＤ获取产品描述*/
        public ProductDescriptionInfo Get_ProductDescriptionInfo_Id(long id)
        {
            ProductDescriptionInfo _ProductDescriptionInfo = new ProductDescriptionInfo();
            _ProductDescriptionInfo = context.ProductDescriptionInfo.FindById(id);
            return _ProductDescriptionInfo;
        }
    }
}
