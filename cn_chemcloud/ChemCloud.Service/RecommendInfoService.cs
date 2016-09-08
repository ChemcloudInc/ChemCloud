using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class RecommendInfoService : ServiceBase, IService,IRecommendInfoService,IDisposable 
    {
        public PageModel<RecommendInfo> GetRecommendInfos(RecommendInfoQuery model)
        {
            int pageNum = 0;
            IQueryable<RecommendInfo> recommendInfo = from item in base.context.RecommendInfo
                                                 select item;
            string begin = model.BeginTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string end = model.EndTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            if (!string.IsNullOrWhiteSpace(begin) && !begin.Equals("0001/01/01") && !string.IsNullOrWhiteSpace(end) && !end.Equals("0001/01/01"))
            {
                recommendInfo = (from a in recommendInfo where a.ActiveTime >= model.BeginTime && a.ActiveTime <= model.EndTime select a);
            }
            if (model.Status.HasValue && model.Status.Value != 0)
            {
                recommendInfo = (from a in recommendInfo where a.Status == model.Status.Value select a);
            }
            //平台货号查询
            if (!string.IsNullOrWhiteSpace(model.Plat_Code))
            {
                recommendInfo = (from a in recommendInfo where a.Plat_Code == model.Plat_Code select a);
            }
            recommendInfo = recommendInfo.GetPage(out pageNum, model.PageNo, model.PageSize, (IQueryable<RecommendInfo> d) =>
               from o in d
               orderby o.ActiveTime descending
               select o);

            return new PageModel<RecommendInfo>
            {
                Models = recommendInfo,
                Total = pageNum
            };
        }
        public RecommendInfo GetRecommendInfo(long id)
        { 
            RecommendInfo model = context.RecommendInfo.FirstOrDefault((RecommendInfo m)=>m.Id == id);
            return model;
        }
        public RecommendInfo GetRecommendInfoByCID(long cid)
        {
            RecommendInfo model = context.RecommendInfo.FirstOrDefault((RecommendInfo m) => m.CID == cid);
            return model;
        }
        public bool AddRecommendInfo(RecommendInfo model)
        {
            try
            {
                context.RecommendInfo.Add(model);
                int i = context.SaveChanges();
                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            { 
                return false; 
            }
            
        }
        public bool UpdateRecommendInfo(long id,decimal price)
        {
            try
            {
                RecommendInfo model = context.RecommendInfo.FindById(id);
                if (model != null)
                {
                    model.Price = price;
                    int i = context.SaveChanges();
                    if (i > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public List<RecommendInfo> GetRecommendByStatus()
        {
            //RecommendInfo model = 
            List<RecommendInfo> model = (from item in base.context.RecommendInfo.Take(8)
                                              where item.Status == 1
                                              orderby item.CreateDate descending
                                              select item).ToList();
            return model;
        }
        public bool DeleteRecommendInfo(long id)
        {
            try
            {
                RecommendInfo model = context.RecommendInfo.FindById(id);
                if (model != null)
                {
                    context.RecommendInfo.Remove(model);
                    int i = context.SaveChanges();
                    if (i > 0)
                        return true;
                    else
                        return false;

                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool BatchDelete(long[] ids)
        {
            try
            {
                IQueryable<RecommendInfo> model = context.RecommendInfo.FindBy((RecommendInfo m)=>ids.Contains(m.Id));
                if (model != null)
                {
                    context.RecommendInfo.RemoveRange(model);
                    int i = context.SaveChanges();
                    if (i > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateRecommendInfoStatus(long Id, int status, long userId)
        {
            try
            {
                RecommendInfo model = context.RecommendInfo.FindById(Id);
                if (model != null)
                {
                    model.Status = status;
                    model.UserId = userId;
                    int i = context.SaveChanges();
                    if (i > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
            

        }
        public bool BatchUpdateRecommendInfoStatus(long[] ids, int status, long userId)
        {
            try
            {
                IQueryable<RecommendInfo> model = context.RecommendInfo.FindBy((RecommendInfo m) => ids.Contains(m.Id));
                if (model != null)
                {
                    foreach (RecommendInfo list in model.ToList())
                    {
                        list.Status = status;
                        list.UserId = userId;
                    }
                    int i = context.SaveChanges();
                    if (i > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
            
        }
        public bool BatchAddRecommendInfo(List<RecommendInfo> modelList)
        {
            try
            {
                context.RecommendInfo.AddRange(modelList);
                int i = context.SaveChanges();
                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public List<ProductInfo> GetRecommendInfosLikePlatCode(string code)
        {
            IQueryable<ProductInfo> productInfos = from item in base.context.ProductInfo
                                                        where item.Plat_Code.Contains(code)
                                                        select item;
            return productInfos.ToList();
        }
    }
}
