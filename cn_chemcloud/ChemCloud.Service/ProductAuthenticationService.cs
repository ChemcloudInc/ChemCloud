using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class ProductAuthenticationService : ServiceBase, IProductAuthenticationService, IService, IDisposable
    {
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="PAQuery"></param>
        /// <returns></returns>
        public PageModel<ProductAuthentication> GetProductAuthenticationList(ProductAuthenticationQuery PAQuery)
        {
            int num = 0;

            IQueryable<ProductAuthentication> PA = context.ProductAuthentication.AsQueryable<ProductAuthentication>();
            if (!string.IsNullOrWhiteSpace(PAQuery.ComName))
            {
                PA =
                    from d in PA
                    where d.ComName.Contains(PAQuery.ComName)
                    orderby d.ProductAuthDate descending
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(PAQuery.ProductCode))
            {
                PA =
                    from d in PA
                    where d.ProductCode.Equals(PAQuery.ProductCode)
                    orderby d.ProductAuthDate descending
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(PAQuery.AuthStatus))
            {
                int i;
                if (int.TryParse(PAQuery.AuthStatus, out i))
                {
                    int k = int.Parse(PAQuery.AuthStatus);
                    PA =
                    from d in PA
                    where d.AuthStatus == k
                    orderby d.ProductAuthDate descending
                    select d;
                }
            }
            if (!string.IsNullOrWhiteSpace(PAQuery.ManageId.ToString()) && PAQuery.ManageId != 0)
            {
                PA =
                    from d in PA
                    where d.ManageId == PAQuery.ManageId
                    orderby d.ProductAuthDate descending
                    select d;
            }
            PA = PA.GetPage(out num, PAQuery.PageNo, PAQuery.PageSize, (IQueryable<ProductAuthentication> d) =>
                from o in d
                orderby o.ProductAuthDate descending
                select o);
            return new PageModel<ProductAuthentication>()
            {

                Models = PA,
                Total = num
            };
        }
        /// <summary>
        /// 根据编号取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductAuthentication GetProductAuthenticationId(long Id)
        {
            ProductAuthentication PAInfo = (
                from p in context.ProductAuthentication
                where p.Id.Equals(Id)
                select p).FirstOrDefault();
            return PAInfo;
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="PAInfo"></param>
        public void AddProductAuthentication(ProductAuthentication PAInfo)
        {
            if (PAInfo == null)
            {
                return;
            }
            context.ProductAuthentication.Add(PAInfo);
            context.SaveChanges();
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="PAInfo"></param>
        public void UpdateProductAuthentication(ProductAuthentication PAInfo)
        {
            ProductAuthentication PA = context.ProductAuthentication.FirstOrDefault((ProductAuthentication m) => m.Id == PAInfo.Id);
            if (PA == null)
            {
                return;
            }
            PA.ManageId = PAInfo.ManageId;
            PA.ProductId = PAInfo.ProductId;
            PA.ProductCode = PAInfo.ProductCode;
            PA.ProductIMG = PAInfo.ProductIMG;
            PA.ProductDesc = PAInfo.ProductDesc;
            PA.ProductAuthDate = PAInfo.ProductAuthDate;
            PA.AuthStatus = PAInfo.AuthStatus;
            PA.AuthAuthor = PAInfo.AuthAuthor;
            PA.AuthTime = PAInfo.AuthTime;
            PA.AuthDesc = PAInfo.AuthDesc;
            PA.ComName = PAInfo.ComName;
            PA.ComAttachment = PAInfo.ComAttachment;
            context.SaveChanges();
        }

        /// <summary>
        /// 根据产品编号取信息
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public ProductAuthentication GetProductAuthenticationProductId(long ProductId)
        {
            ProductAuthentication PAInfo = (
                from p in context.ProductAuthentication
                where p.ProductId.Equals(ProductId)
                select p).FirstOrDefault();
            return PAInfo;
        }
    }
}
