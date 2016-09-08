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
using System.Web;
using ChemCloud.Model.Common;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class JobsListController : BaseAdminController
    {
        HashSet_Common hashSet = new HashSet_Common();


        IJobsService bannerIndexService = ServiceHelper.Create<IJobsService>();

        public JobsListController()
        {
        }

        public ViewResult JobsList()
        {
            return new ViewResult();
        }


        #region 删除
        /// <summary>
        /// 删除招聘信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteById_Admin(int Id)
        {
            Result_Msg res = new Result_Msg();
            if (base.CurrentManager != null)
            {
                IJobsService jobsService = ServiceHelper.Create<IJobsService>();
                res = jobsService.DeleteById_Admin(Id);
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "审核失败，失败原因：获取登录状态失败！";
            }
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
            IJobsService jobsService = ServiceHelper.Create<IJobsService>();
            Result_List_Pager<Result_Jobs> res = new Result_List_Pager<Result_Jobs>();
            Result_List_Pager<Jobs> resList = jobsService.GetJobsList_Admin(query);
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
                    UserId = jobsService.GetCompanyInfo_ByUserIdAndUserType(x.UserId),
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
        public string GetObjectById_Admin(int Id)
        {
            Result_Model<Result_Jobs> res = new Result_Model<Result_Jobs>();
            if (base.CurrentManager != null)
            {
                IJobsService jobsService = ServiceHelper.Create<IJobsService>();
                res.Model = jobsService.GetObjectById_Admin(Id);
                res.Msg = new Result_Msg() { IsSuccess = true, Message = string.Empty };
            }
            else
            {
                res.Msg = new Result_Msg()
                {
                    IsSuccess = false,
                    Message = "审核失败，失败原因：获取登录状态失败！"
                };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 审核
        /// <summary>
        /// 新增招聘信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string ModifyJob(QueryCommon<JobsAddQuery> query)
        {
            DateTime now = DateTime.Now;
            Result_Msg res = new Result_Msg();
            if (base.CurrentManager == null)
            {
                res.IsSuccess = false;
                res.Message = "修改失败，您无权限修改该信息。请确认您已登录";
            }
            else
            {
                query.ParamInfo.Reviewer = Convert.ToInt32(base.CurrentManager.Id);
                query.ParamInfo.UpdateDate = now;
                IJobsService jobsService = ServiceHelper.Create<IJobsService>();
                res = jobsService.ModifyJob_Admin(query);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 快速审核
        /// <summary>
        /// 快速审核
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string ModifyJob_Fast(JobsModifyQuery query)
        {
            Result_Msg res = new Result_Msg();
            if (base.CurrentManager == null)
            {
                res.IsSuccess = false;
                res.Message = "修改失败，您无权限修改该信息。请确认您已登录";
            }
            else
            {
                query.Reviewer = Convert.ToInt32(base.CurrentManager.Id);
                query.ApprovalStatus = 3;

                IJobsService jobsService = ServiceHelper.Create<IJobsService>();
                res = jobsService.ModifyJob_Fast(query);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
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

        [WebMethod]
        public string Get_PageInfo_Admin(QueryCommon<JobsQuery> query)
        {
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();


            resModel = bannerIndexService.Get_PageInfo_Admin(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
    }
}
