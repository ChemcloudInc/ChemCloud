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

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class JobsWebController : BaseWebController
    {
        HashSet_Common hashSet = new HashSet_Common();
        IJobsService jobsService = ServiceHelper.Create<IJobsService>();

        public JobsWebController()
        {

        }

        public ViewResult JobsWeb()
        {
            return new ViewResult();
        }

        public ViewResult JobWebDes()
        {
            return new ViewResult();
        }

        #region 列表2
        /// <summary>
        /// 获取招聘信息列表2
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string JobsList_Web(QueryCommon<JobsQuery> query)
        {
            Result_List_Pager<Result_Jobs> res = new Result_List_Pager<Result_Jobs>();
            Result_List_Pager<Jobs> resList = jobsService.GetJobsList_Web(query);

            if (resList.Msg.IsSuccess)
            {
                var listHash = hashSet.Get_DictionariesList();

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

        #region 获取分页信息
        [WebMethod]

        public string Get_PageInfo(QueryCommon<JobsQuery> query)
        {
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            resModel = jobsService.Get_PageInfo(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion

        #region 根据ID获取指定对象
        /// <summary>
        /// 根据ID获取指定对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetObjectById_Web(int Id, int WorkType, int LanguageType)
        {

            Result_Model_List_Jobs<Result_Jobs> resAll = new Result_Model_List_Jobs<Result_Jobs>()
            {
                Model = new Result_Model<Result_Jobs>(),
                Msg = new Result_Msg() { IsSuccess = true, Message = string.Empty },
                List = new Result_List<Result_Model<Result_Jobs>>()
            };
            try
            {
                Result_Model<Jobs> job = jobsService.GetObjectById_Web(Id);
                var listHash = hashSet.Get_DictionariesList();

                resAll.Model.Model = new Result_Jobs()
                {
                    Id = job.Model.Id,
                    JobTitle = job.Model.JobTitle,
                    UserType = listHash.Where(y => y.DictionaryTypeId == 15 && y.DKey == job.Model.UserType.ToString()).FirstOrDefault().DValue,
                    JobContent = job.Model.JobContent,
                    UserId = jobsService.GetCompanyInfo_ByUserIdAndUserType(job.Model.UserId),
                    CreateDate = job.Model.CreateDate.ToString("yyyy-MM-dd"),
                    UpdateDate = job.Model.UpdateDate.ToString("yyyy-MM-dd"),
                    StartDate = job.Model.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = job.Model.EndDate.ToString("yyyy-MM-dd"),
                    ApprovalStatus = listHash.Where(y => y.DictionaryTypeId == 14 && y.DKey == job.Model.ApprovalStatus.ToString()).FirstOrDefault().DValue,
                    Reviewer = job.Model.Reviewer,

                    Payrol_LowHigh = (job.Model.PayrolLow.ToString() + '-' + job.Model.PayrolHigh.ToString() + listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == job.Model.TypeOfCurrency.ToString()).FirstOrDefault().Remarks + "/" + listHash.Where(y => y.DictionaryTypeId == 18 && y.DKey == job.Model.PayrollType.ToString()).FirstOrDefault().DValue),
                    PayrollType = listHash.Where(y => y.DictionaryTypeId == 18 && y.DKey == job.Model.PayrollType.ToString()).FirstOrDefault().DValue,
                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == job.Model.TypeOfCurrency.ToString()).FirstOrDefault().Remarks,
                    WorkType = listHash.Where(y => y.DictionaryTypeId == 17 && y.DKey == job.Model.WorkType.ToString()).FirstOrDefault().DValue,
                    CompanyTel = job.Model.CompanyTel,
                    CompanyEmail = job.Model.CompanyEmail,
                    WorkPlace = job.Model.WorkPlace,
                    LanguageType = job.Model.LanguageType == 1 ? listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == job.Model.LanguageType.ToString()).FirstOrDefault().DKey : listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == job.Model.LanguageType.ToString()).FirstOrDefault().Remarks,
                };

                if (resAll.Model != null && resAll.Msg.IsSuccess)
                {
                    resAll.List = Get_PreNext_ById_Web(Id, WorkType, LanguageType);
                    if (!resAll.List.Msg.IsSuccess)
                    {
                        resAll.Msg.IsSuccess = false;
                        resAll.Msg.Message += "获取上一页、下一页失败！\n\r";
                    }
                }
            }
            catch (Exception ex)
            {
                resAll.Msg = new Result_Msg() { IsSuccess = false, Message = "读取失败，失败原因：" + ex.Message };
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(resAll);
        }
        #endregion

        #region 根据ID 前后两条信息
        /// <summary>
        /// 根据ID 前后两条信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result_List<Result_Model<Result_Jobs>> Get_PreNext_ById_Web(long Id, int workType, int languageType)
        {
            Result_List<Result_Model<Result_Jobs>> res = new Result_List<Result_Model<Result_Jobs>>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                List = new List<Result_Model<Result_Jobs>>()
            };

            try
            {
                var listHash = hashSet.Get_DictionariesList();

                Result_List<Result_Model<Jobs>> resJobs = jobsService.Get_PreNext_ById_Web(Id, workType, languageType);
                res.Msg = resJobs.Msg;
                if (resJobs != null && resJobs.Msg.IsSuccess)
                {
                    foreach (var item in resJobs.List)
                    {
                        if (item.Msg.IsSuccess)
                        {
                            res.List.Add(new Result_Model<Result_Jobs>()
                            {
                                Model = new Result_Jobs()
                                {
                                    Id = item.Model.Id,
                                    JobTitle = item.Model.JobTitle,
                                    EndDate = item.Model.EndDate.ToString("yyyy-MM-dd HH;mm"),
                                    CreateDate = item.Model.CreateDate.ToString("yyyy-MM-dd HH;mm"),
                                    JobContent = item.Model.JobContent,
                                    ApprovalStatus = listHash.Where(y => y.DictionaryTypeId == 14 && y.DKey == item.Model.ApprovalStatus.ToString()).FirstOrDefault().DValue,
                                    Reviewer = item.Model.Reviewer,
                                    StartDate = item.Model.StartDate.ToString("yyyy-MM-dd HH;mm"),
                                    UpdateDate = item.Model.UpdateDate.ToString("yyyy-MM-dd HH;mm"),
                                    //UserId = item.Model.UserId,
                                    UserType = listHash.Where(y => y.DictionaryTypeId == 15 && y.DKey == item.Model.UserType.ToString()).FirstOrDefault().DValue,
                                    LanguageType = item.Model.LanguageType == 1 ? listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == item.Model.LanguageType.ToString()).FirstOrDefault().DKey : listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == item.Model.LanguageType.ToString()).FirstOrDefault().Remarks,
                                },
                                Msg = item.Msg
                            });
                        }
                        else
                        {
                            res.List.Add(new Result_Model<Result_Jobs>()
                            {
                                Model = new Result_Jobs(),
                                Msg = item.Msg
                            });
                        }
                    }
                }
                else
                {
                    res.Msg = resJobs.Msg;
                }
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return res;
        }
        #endregion

    }
}