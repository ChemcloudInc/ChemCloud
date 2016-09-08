using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChemCloud.Service
{
    public class COAService : ServiceBase, ICOAService, IService, IDisposable
    {
        //添加COA报告
        public void AddCOA(ChemCloud_COA model)
        {
            context.ChemCloud_COA.Add(model);
            context.SaveChanges();
            foreach (var item in model.ChemCloud_COADetails)
            {
                context.ChemCloud_COADetails.Add(item);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// COA查询页数
        /// </summary>
        /// <param name="CertificateNumber"></param>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public int SearchCOAListCount(string CertificateNumber, int PageNo, int PageSize)
        {
            int num = 0;
            try
            {
                IQueryable<ChemCloud_COA> cas = context.ChemCloud_COA.AsQueryable<ChemCloud_COA>();
                if (!string.IsNullOrWhiteSpace(CertificateNumber))
                {
                    cas = (from u in cas
                           where u.CertificateNumber.Contains(CertificateNumber)
                           orderby u.DATEOFRELEASE descending
                           select u).Skip(PageSize * (PageNo - 1)).Take(PageSize);
                }
                cas = cas.GetPage(out num, PageNo, PageSize, (IQueryable<ChemCloud_COA> d) =>
                    from o in d
                    orderby o.DATEOFRELEASE descending
                    select o);
                return num;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// 翻页查询
        /// </summary>
        /// <param name="CertificateNumber"></param>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public PageModel<ChemCloud_COA> SearchCOAList(string CertificateNumber, int PageNo, int PageSize)
        {
            int num = 0;
            try
            {
                IQueryable<ChemCloud_COA> cas = context.ChemCloud_COA.AsQueryable<ChemCloud_COA>();

                if (!string.IsNullOrWhiteSpace(CertificateNumber))
                {
                    cas = (from u in cas
                           where u.CertificateNumber.Contains(CertificateNumber)
                           orderby u.DATEOFRELEASE descending
                           select u).Skip(PageSize * (PageNo - 1)).Take(PageSize);
                }
                return new PageModel<ChemCloud_COA>()
                {
                    Models = cas,
                    Total = num
                };
            }
            catch (Exception) { return null; }
        }

    }
}
