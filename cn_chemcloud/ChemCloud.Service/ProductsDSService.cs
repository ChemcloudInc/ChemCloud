using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class ProductsDSService : ServiceBase, IProductsDSService, IService, IDisposable
    {
        public ProductsDSService()
        {

        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool AddProductsDS(ProductsDSQuery query)
        {
            int i = 0;
            ProductsDS p = new ProductsDS();
            p.ProductId = query.productid;
            p.ShopId = query.shopid;
            p.UserId = query.userid;
            p.UserType = query.usertype;
            p.DSStatus = query.dsstatus;
            p.DSTime = query.dstime;
            try
            {
                // 写数据库
                context.ProductsDS.Add(p);
                i = context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

            }
            if (i > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool UpdateProductsDS(ProductsDSQuery query)
        {
            ProductsDS p = context.ProductsDS.FirstOrDefault((ProductsDS m) => m.Id == query.id);
            if (p == null)
            {
                return false;
            }
            p.ProductId = query.productid;
            p.ShopId = query.shopid;
            p.UserId = query.userid;
            p.UserType = query.usertype;
            p.DSStatus = query.dsstatus;
            p.DSTime = query.dstime;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// del
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool DelProductsDS(ProductsDSQuery query)
        {
            ProductsDS p = context.ProductsDS.FirstOrDefault((ProductsDS m) => m.Id == query.id);
            if (p == null)
            {
                return false;
            }
            context.ProductsDS.Remove(p);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public PageModel<ProductsDS> GetProductsDSList(ProductsDSQuery query)
        {
            int num = 0;
            IQueryable<ProductsDS> p = context.ProductsDS.AsQueryable<ProductsDS>();
            //产品货号
            if (!string.IsNullOrWhiteSpace(query.productcode))
            {
                p =
                    from d in p
                    where d.productCode.Equals(query.productcode)
                    select d;
            }
            //产品名称
            if (!string.IsNullOrWhiteSpace(query.productname))
            {
                p =
                    from d in p
                    where d.productName.Equals(query.productname)
                    select d;
            }
            //cas
            if (!string.IsNullOrWhiteSpace(query.CAS))
            {
                p =
                    from d in p
                    where d.cas.Equals(query.CAS)
                    select d;
            }
            //供应商名称
            if (!string.IsNullOrWhiteSpace(query.sellerusername))
            {
                p =
                    from d in p
                    where d.sellerusername.Equals(query.sellerusername)
                    select d;
            }
            //供应商编号
            if (query.userid != 0)
            {
                p =
                    from d in p
                    where d.UserId.Equals(query.userid)
                    select d;
            }
            //状态
            if (query.dsstatus >= 0)
            {
                p =
                    from d in p
                    where d.DSStatus.Equals(query.dsstatus)
                    select d;
            }
            p = p.GetPage(out num, query.PageNo, query.PageSize, (IQueryable<ProductsDS> d) =>
                from o in d
                orderby o.DSTime descending
                select o);
            return new PageModel<ProductsDS>()
            {
                Models = p,
                Total = num
            };
        }


        public ProductsDS GetProductsDS(long shopid, long productid)
        {
            ProductsDS p = (from ps in context.ProductsDS where ps.ShopId == shopid && ps.ProductId == productid select ps).FirstOrDefault();
            return p;
        }


        public ProductsDS GetProductsDSbyId(long id)
        {
            ProductsDS p = (from ps in context.ProductsDS where ps.Id == id select ps).FirstOrDefault();
            return p;
        }
    }
}