using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;
using ChemCloud.IServices.QueryModel;

namespace ChemCloud.IServices
{
    public interface IProductLevelService : IService, IDisposable
    {
        bool AddProductLevel(ProductLevel productlevel);

        bool UpdateProductLevel(ProductLevel productlevel);

        bool DeleteProductLevel(long id);

        PageModel<ProductLevel> GetProductLevels(ProductLevelQuery productlevelQueryModel);

        ProductLevel GetProductLevel(long id);

        IQueryable<ProductLevel> GetLevelById(long Id);

        bool BatchDelete(long[] ids);

        List<ProductLevel> GetProductLevel();
    }
}
