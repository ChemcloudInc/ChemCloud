using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.OAuth;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Web.Models;
using ChemCloud.Web.Areas.Admin.Models;
using System.IO;
using System.Text;
using ChemCloud.Core.Helper;
using System.Web.Services;
using ChemCloud.Model.Common;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class JobsController : BaseWebController
    {
        HashSet_Common hashSet = new HashSet_Common();
        IJobsService jobsService = ServiceHelper.Create<IJobsService>();

        public JobsController()
        {

        }


        public ViewResult Jobs()
        {
            return new ViewResult();
        }

        #region 新增
        /// <summary>
        /// 新增招聘信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string JobsAdd(QueryCommon<JobsAddQuery> query)
        {
            DateTime now = DateTime.Now;
            query.ParamInfo.UserId = base.CurrentUser.Id;
            query.ParamInfo.UserType = base.CurrentUser.UserType;
            query.ParamInfo.ApprovalStatus = 1;
            query.ParamInfo.CreateDate = now;
            query.ParamInfo.UpdateDate = now;

            IJobsService jobsService = ServiceHelper.Create<IJobsService>();
            Result_Msg res = jobsService.AddJob(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除招聘信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteById(int Id)
        {
            long userId = base.CurrentUser.Id;
            IJobsService jobsService = ServiceHelper.Create<IJobsService>();
            Result_Msg res = jobsService.DeleteById(Id, userId);

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }
        #endregion

        #region 列表2
        /// <summary>
        /// 获取招聘信息列表2
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string JobsList1(QueryCommon<JobsQuery> query)
        {
            Result_List_Pager<Result_Jobs> res = new Result_List_Pager<Result_Jobs>();
            query.ParamInfo.UserId = base.CurrentUser.Id;
            query.ParamInfo.UserType = base.CurrentUser.UserType;

            IJobsService jobsService = ServiceHelper.Create<IJobsService>();
            Result_List_Pager<Jobs> resList = jobsService.GetJobsList_Member(query);
            var listHash = hashSet.Get_DictionariesList();

            if (resList.Msg.IsSuccess)
            {
                res.PageInfo = resList.PageInfo;
                res.Msg = resList.Msg;
                res.List = resList.List.Select(x => new Result_Jobs()
                {
                    Id = x.Id,
                    JobTitle = x.JobTitle,
                    UserType = listHash.Where(y => y.DictionaryTypeId == 15 && y.DKey == x.UserType.ToString()).FirstOrDefault().DValue,
                    JobContent = x.JobContent,
                    UserId = x.UserId.ToString(),
                    CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                    UpdateDate = x.UpdateDate.ToString("yyyy-MM-dd"),
                    StartDate = x.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                    ApprovalStatus = listHash.Where(y => y.DictionaryTypeId == 14 && y.DKey == x.ApprovalStatus.ToString()).FirstOrDefault().DValue,
                    Reviewer = x.Reviewer,

                    Payrol_LowHigh = (x.PayrolLow.ToString() + '-' + x.PayrolHigh.ToString()),
                    PayrollType = listHash.Where(y => y.DictionaryTypeId == 18 && y.DKey == x.PayrollType.ToString()).FirstOrDefault().DValue,
                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == x.TypeOfCurrency.ToString()).FirstOrDefault().Remarks,
                    WorkType = listHash.Where(y => y.DictionaryTypeId == 17 && y.DKey == x.WorkType.ToString()).FirstOrDefault().DValue,
                    CompanyTel = x.CompanyTel,
                    CompanyEmail = x.CompanyEmail,
                    WorkPlace = x.WorkPlace,
                    LanguageType = x.LanguageType == 1 ? listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == x.LanguageType.ToString()).FirstOrDefault().DKey : listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == x.LanguageType.ToString()).FirstOrDefault().Remarks,
                }).ToList();
            }
            else
            {
                res.Msg = resList.Msg;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 根据ID获取指定对象
        [HttpPost]
        public string GetObjectById(int Id)
        {
            long userId = base.CurrentUser.Id;
            IJobsService jobsService = ServiceHelper.Create<IJobsService>();
            Jobs job = jobsService.GetObjectById(Id, userId);
            var listHash = hashSet.Get_DictionariesList();

            Result_Jobs res = new Result_Jobs()
            {
                Id = job.Id,
                JobTitle = job.JobTitle,
                JobContent = job.JobContent,

                CreateDate = job.CreateDate.ToString(),
                UpdateDate = job.UpdateDate.ToString(),
                StartDate = job.StartDate.ToString(),
                EndDate = job.EndDate.ToString(),

                UserId = job.UserId.ToString(),
                UserType = listHash.Where(y => y.DictionaryTypeId == 15 && y.DKey == job.UserType.ToString()).FirstOrDefault().DValue,
                ApprovalStatus = listHash.Where(y => y.DictionaryTypeId == 14 && y.DKey == job.ApprovalStatus.ToString()).FirstOrDefault().DValue,
                Reviewer = job.Reviewer,

                PayrolHigh = job.PayrolHigh,
                PayrolLow = job.PayrolLow,
                Payrol_LowHigh = (job.PayrolLow.ToString() + '-' + job.PayrolHigh.ToString()),
                //PayrollType = listHash.Where(y => y.DictionaryTypeId == 18 &&  y.Key == job.PayrollType).FirstOrDefault().Value,
                //TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 2 &&  y.DValue == job.TypeOfCurrency).FirstOrDefault().Remarks,
                //WorkType = listHash.Where(y => y.DictionaryTypeId == 17 &&  y.Key == job.WorkType).FirstOrDefault().Value,
                PayrollType = job.PayrollType.ToString(),
                TypeOfCurrency = job.TypeOfCurrency.ToString(),
                WorkType = job.WorkType.ToString(),
                CompanyTel = job.CompanyTel,
                CompanyEmail = job.CompanyEmail,
                WorkPlace = job.WorkPlace,
                LanguageType=job.LanguageType.ToString()
                //LanguageType = job.LanguageType == 1 ? listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == job.LanguageType.ToString()).FirstOrDefault().DKey : listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == job.LanguageType.ToString()).FirstOrDefault().Remarks,
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }
        #endregion
        #region 修改
        /// <summary>
        /// 新增招聘信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string ModifyJob(QueryCommon<JobsAddQuery> query)
        {
            Result_Msg res = new Result_Msg();
            if (base.CurrentUser.Id != query.ParamInfo.UserId)
            {
                res.IsSuccess = false;
                res.Message = "修改失败，您无权限修改该信息。请确认您已登录";
            }
            else
            {
                query.ParamInfo.UserId = base.CurrentUser.Id;
                query.ParamInfo.UserType = base.CurrentUser.UserType;
                query.ParamInfo.UpdateDate = DateTime.Now;
                query.ParamInfo.ApprovalStatus = 1;
                IJobsService jobsService = ServiceHelper.Create<IJobsService>();
                res = jobsService.ModifyJob(query);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 获取分页信息
        /// <summary>
        /// 获取分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string Get_PageInfo_Member(QueryCommon<JobsQuery> query)
        {
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            query.ParamInfo.UserId = base.CurrentUser.Id;
            query.ParamInfo.UserType = base.CurrentUser.UserType;

            resModel = jobsService.Get_PageInfo_Member(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion

        public string GetSelectOptionList()
        {
            Result_List<ChemCloud_Dictionaries> res = new Result_List<ChemCloud_Dictionaries>()
            {
                List = hashSet.Get_DictionariesList(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
    }
}