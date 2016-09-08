using ChemCloud.Model.Common;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Data.Entity.Validation;

namespace ChemCloud.Service
{
    public class ProductAnalysisService : ServiceBase, IProductAnalysisService, IService, IDisposable
    {
        /// <summary>
        /// 提交分析报告
        /// </summary>
        /// <param name="op"></param>
        public ProductAnalysis AddProductAnalysis(ProductAnalysis op)
        {
            ProductAnalysis _ProductAnalysis = new ProductAnalysis();
            if (op == null || op.Id != 0)
            {
                return _ProductAnalysis;
            }
            else
            {
                try
                {
                    _ProductAnalysis = context.ProductAnalysis.Add(op);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                }

                return _ProductAnalysis;
            }
        }


        /// <summary>
        /// 分析报告查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductAnalysis GetProductAnalysis(long Id)
        {
            return context.ProductAnalysis.FindBy((ProductAnalysis m) => m.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// 分析报告列表查询列表
        /// </summary>
        /// <param name="opQuery"></param>
        /// <returns></returns>
        public Model.PageModel<ProductAnalysis> GetProductAnalysisList(ProductAnalysis paQuery)
        {
            int num = 0;
            IQueryable<ProductAnalysis> _ProductAnalysis = context.ProductAnalysis.AsQueryable<ProductAnalysis>();


            if (paQuery.AnalysisStatus != 0)
            {
                _ProductAnalysis =
                from d in _ProductAnalysis
                where d.AnalysisStatus.Equals(paQuery.AnalysisStatus)
                orderby d.Id descending
                select d;
            }

            if (paQuery.Memberid != 0)
            {
                _ProductAnalysis =
                from d in _ProductAnalysis
                where d.Memberid.Equals(paQuery.Memberid)
                orderby d.Id descending
                select d;
            }
            if (!string.IsNullOrEmpty(paQuery.ClientName))
            {
                _ProductAnalysis =
                    from d in _ProductAnalysis
                    where d.ClientName.Equals(paQuery.ClientName)
                    orderby d.Id descending
                    select d;
            }

            if (!string.IsNullOrEmpty(paQuery.SampleName))
            {
                _ProductAnalysis =
                    from d in _ProductAnalysis
                    where d.SampleName.Equals(paQuery.SampleName)
                    orderby d.Id descending
                    select d;
            }

            _ProductAnalysis = _ProductAnalysis.GetPage(out num, paQuery.PageNo, paQuery.PageSize, (IQueryable<ProductAnalysis> d) =>
                from o in d orderby o.Id descending select o);

            return new PageModel<ProductAnalysis>()
            {

                Models = _ProductAnalysis,
                Total = num
            };
        }

        /// <summary>
        /// 删除分析鉴定
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(long Id)
        {
            bool result = false;
            ProductAnalysis model = context.ProductAnalysis.FindBy((ProductAnalysis item) => item.Id == Id).FirstOrDefault();
            if (model != null)
            {
                context.ProductAnalysis.Remove(model);
                int i = context.SaveChanges();
                if (i > 0)
                { result = true; }
            }
            return result;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateAnalysisStatus(long Id, int status)
        {
            bool result = false;
            ProductAnalysis model = context.ProductAnalysis.FindBy((ProductAnalysis item) => item.Id == Id).FirstOrDefault();
            if (model != null)
            {
                model.AnalysisStatus = status;
                int i = context.SaveChanges();
                if (i > 0)
                { result = true; }
            }
            return result;
        }

        /// <summary>
        /// 提交附件
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AnalysisAttachments"></param>
        /// <returns></returns>
        public bool UpdateAnalysisAddAttachment(long Id, string AnalysisAttachments)
        {
            bool result = false;
            ProductAnalysis model = context.ProductAnalysis.FindBy((ProductAnalysis item) => item.Id == Id).FirstOrDefault();
            if (model != null)
            {
                model.AnalysisAttachments = AnalysisAttachments;
                model.AnalysisStatus = 5;
                int i = context.SaveChanges();
                if (i > 0)
                { result = true; }
            }
            return result;
        }
    }
}
