using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service 
{
    public class ProductLevelService :  ServiceBase, IProductLevelService, IService, IDisposable
    {
        public bool AddProductLevel(ProductLevel productlevel)
        {
            context.ProductLevel.Add(productlevel);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool UpdateProductLevel(ProductLevel productlevel)
        {
            int i = 0;
            ProductLevel level = context.ProductLevel.FirstOrDefault((ProductLevel m)=>m.Id == productlevel.Id);
            if(level != null)
            {
                level.LevelNameCN = productlevel.LevelNameCN;
                level.LevelNameEN = productlevel.LevelNameEN;
                i = context.SaveChanges();
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool DeleteProductLevel(long id)
        {
            int i = 0;
            ProductLevel level = context.ProductLevel.FirstOrDefault((ProductLevel m)=>m.Id == id);
            if(level != null)
            {
                context.ProductLevel.Remove(level);
                i = context.SaveChanges();
            }
            if (i > 0)
                return true;
            else
                return false;
        }

        public IQueryable<ProductLevel> GetLevelById(long Id)
        {
            return context.ProductLevel.FindBy((ProductLevel item) => item.Id == Id);
        }

        public ProductLevel GetProductLevel(long id)
        {
            ProductLevel productLevel = context.ProductLevel.FirstOrDefault((ProductLevel m) => m.Id == id);
            return productLevel;
        }
        public PageModel<ProductLevel> GetProductLevels(ProductLevelQuery ProductLevelQueryModel)
        {
            //  IQueryable<FieldCertification> CertificationShop = context.FieldCertification.AsQueryable<FieldCertification>();
            IQueryable<ProductLevel> ProductLevels = from item in base.context.ProductLevel
                                                     select item;
            long Id = ProductLevelQueryModel.Id;
            if ((Id > 0))
            {
                ProductLevels =
                    from item in ProductLevels
                    where item.Id == ProductLevelQueryModel.Id
                    select item;
            }
            if (!string.IsNullOrWhiteSpace(ProductLevelQueryModel.ProductLevelCN))
            {
                ProductLevels = from d in ProductLevels
                                where d.LevelNameCN.Contains(ProductLevelQueryModel.ProductLevelCN)
                                select d;
            }
            if (!string.IsNullOrWhiteSpace(ProductLevelQueryModel.ProductLevelEN))
            {
                ProductLevels = from d in ProductLevels
                                where d.LevelNameEN.Contains(ProductLevelQueryModel.ProductLevelEN)
                                select d;
            }
            Func<IQueryable<ProductLevel>, IOrderedQueryable<ProductLevel>> func = null;
            func = (IQueryable<ProductLevel> d) =>
                    from o in d
                    orderby o.Id descending
                    select o;
            int num = ProductLevels.Count();
            ProductLevels = ProductLevels.GetPage(out num, ProductLevelQueryModel.PageNo, ProductLevelQueryModel.PageSize, func);
            return new PageModel<ProductLevel>()
            {
                Models = ProductLevels,
                Total = num
            };
        }
        public bool BatchDelete(long[] ids)
        {
            IQueryable<ProductLevel> productLevel = context.ProductLevel.FindBy((ProductLevel item) => ids.Contains(item.Id));
            context.ProductLevel.RemoveRange(productLevel);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }



        public List<ProductLevel> GetProductLevel()
        {
            return (from a in context.ProductLevel select a).ToList<ProductLevel>();
        }
    }
}
