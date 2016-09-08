using ChemCloud.Model.Common;
using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class BannersIndexService : ServiceBase, IBannersIndexService, IService, IDisposable
    {
        HashSet_Common hashSet = new HashSet_Common();

        /// <summary>
        /// 加载首页Banner
        /// </summary>
        /// <returns></returns>
        public Result_List<Result_BannersIndex> GetBannerList_IndexPage(int languageType)
        {
            var listHash = hashSet.Get_DictionariesList();
            Result_List<Result_BannersIndex> resList = new Result_List<Result_BannersIndex>() { Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var total = (from p in context.BannersIndex where p.Url != null && p.LanguageType == languageType select p).ToList();
                resList.List = total.Select(x => new Result_BannersIndex()
                {
                    Id = x.Id,
                    ShopId = x.ShopId,
                    BannerTitle = x.BannerTitle,
                    BannerDes = x.BannerDes,
                    Url = x.Url,
                    ManagerId = x.ManagerId,
                    TargetName = x.TargetName,
                    LanguageType = x.LanguageType == 1 ? listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == x.LanguageType.ToString()).FirstOrDefault().DKey : listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == x.LanguageType.ToString()).FirstOrDefault().Remarks,
                }).ToList();
            }
            catch (Exception ex)
            {
                resList.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resList;
        }
        public Result_List_Pager<BannersIndex> GetBannerList(QueryCommon<BannerIndexQuery> query)
        {
            Result_List_Pager<BannersIndex> res = new Result_List_Pager<BannersIndex>()
            {
                List = new List<BannersIndex>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {

                var linqList = from banner in context.BannersIndex select banner;
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.BannerTitle != null && query.ParamInfo.BannerTitle != string.Empty)
                {
                    linqList = linqList.Where(x => x.BannerTitle.IndexOf(query.ParamInfo.BannerTitle) != -1);
                }
                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.Id).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }

        #region 分页信息
        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo(QueryCommon<BannerIndexQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from banner in context.BannersIndex select banner;

                #region 拼接 Linq 查询条件
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.BannerTitle != null && query.ParamInfo.BannerTitle != string.Empty)
                {
                    linqList = linqList.Where(x => x.BannerTitle.IndexOf(query.ParamInfo.BannerTitle) != -1);
                }
                #endregion

                int total = linqList.Count();

                resPage.Model.CurrentPage = query.PageInfo.CurrentPage;
                resPage.Model.PageSize = query.PageInfo.PageSize;
                resPage.Model.Total = total;
                resPage.Model.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                resPage.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resPage;
        }
        #endregion

        /// <summary>
        /// 删除指定ID数据
        /// </summary>
        /// <param name="Id">幻灯片信息ID</param>
        /// <returns></returns>
        public Result_Msg DeleteById(int Id, long userId)
        {
            Result_Msg res = new Result_Msg();
            BannersIndex job = (from p in context.BannersIndex where p.Id == Id select p).FirstOrDefault();
            context.BannersIndex.Remove(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }

        /// <summary>
        /// 新增幻灯片信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg BannerAdd(QueryCommon<BannerIndexQuery> query)
        {
            Result_Msg res = new Result_Msg();
            BannersIndex job = new BannersIndex()
            {
                BannerTitle = query.ParamInfo.BannerTitle,
                BannerDes = query.ParamInfo.BannerDes,
                ManagerId = query.ParamInfo.ManagerId,
                TargetName = query.ParamInfo.TargetName,
                Url = query.ParamInfo.Url,
                LanguageType = query.ParamInfo.LanguageType
            };

            context.BannersIndex.Add(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }

        /// <summary>
        /// 获取幻灯片信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result_BannersIndex GetObjectById(int Id, long userId)
        {
            var listHash = hashSet.Get_DictionariesList();
            BannersIndex job = (from p in context.BannersIndex where p.Id == Id select p).FirstOrDefault();
            Result_BannersIndex res = new Result_BannersIndex()
            {
                Id = job.Id,
                ShopId = job.ShopId,
                BannerTitle = job.BannerTitle,
                BannerDes = job.BannerDes,
                ManagerId = job.ManagerId,
                TargetName = job.TargetName,
                LanguageType = job.LanguageType.ToString(),
                Url = job.Url
            };

            return res;
        }

        /// <summary>
        /// 修改幻灯片信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg ModifyBannerIndex(QueryCommon<BannerIndexQuery> query)
        {
            Result_Msg res = new Result_Msg();

            BannersIndex bannerIndexOld = context.BannersIndex.FindById<BannersIndex>(query.ParamInfo.Id);
            if (bannerIndexOld != null)
            {
                bannerIndexOld.BannerTitle = query.ParamInfo.BannerTitle;
                bannerIndexOld.BannerDes = query.ParamInfo.BannerDes;
                bannerIndexOld.ManagerId = query.ParamInfo.ManagerId;
                bannerIndexOld.TargetName = query.ParamInfo.TargetName;
                bannerIndexOld.Url = query.ParamInfo.Url;
                bannerIndexOld.LanguageType = query.ParamInfo.LanguageType;
                bannerIndexOld.ShopId = 1;
                try
                {
                    res.IsSuccess = context.SaveChanges() == 1;
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }

    }
}
