using ChemCloud.Model.Common;
using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class JobsService : ServiceBase, IJobsService, IService, IDisposable
    {
        HashSet_Common hashSet = new HashSet_Common();

        #region Admin 后台

        #region 获取招聘信息列表
        /// <summary>
        /// 获取招聘信息列表——Admin
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<Jobs> GetJobsList_Admin(QueryCommon<JobsQuery> query)
        {
            Result_List_Pager<Jobs> res = new Result_List_Pager<Jobs>()
            {
                List = new List<Jobs>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = from job in context.Jobs select job;
                if (query.ParamInfo.UserType != 0)
                {
                    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                }
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.ApprovalStatus != 0)
                {
                    linqList = linqList.Where(x => x.ApprovalStatus == query.ParamInfo.ApprovalStatus);
                }
                if (query.ParamInfo.JobTitle != null && query.ParamInfo.JobTitle != string.Empty)
                {
                    linqList = linqList.Where(x => x.JobTitle.IndexOf(query.ParamInfo.JobTitle) != -1);
                }

                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

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
        #endregion

        #region 快速审核
        /// <summary>
        /// 快速审核
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg ModifyJob_Fast(JobsModifyQuery query)
        {
            Result_Msg res = new Result_Msg() { IsSuccess = true };

            Jobs jobOld = context.Jobs.FindById<Jobs>(query.Id);
            if (jobOld != null)
            {
                if (jobOld.ApprovalStatus == query.ApprovalStatus)
                {
                    res.IsSuccess = false;
                    res.Message = "修改失败：当前状态已经为审核通过";
                }
                else
                {
                    jobOld.ApprovalStatus = query.ApprovalStatus;
                    jobOld.Reviewer = query.Reviewer;
                    try
                    {
                        res.IsSuccess = context.SaveChanges() == 1;
                    }
                    catch (Exception ex)
                    {
                        res.IsSuccess = false;
                        res.Message = "修改失败：" + ex.Message;
                    }
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion

        #region 根据Id获取详情
        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result_Jobs GetObjectById_Admin(int Id)
        {
            var listHash = hashSet.Get_DictionariesList();

            Jobs job = (from p in context.Jobs where p.Id == Id select p).FirstOrDefault();
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
                UserType = job.UserType.ToString(),
                //UserType = listHash.Where(y => y.DictionaryTypeId == 15 && y.DKey == job.UserType.ToString()).FirstOrDefault().DValue,
                //ApprovalStatus = listHash.Where(y => y.DictionaryTypeId == 14 && y.DKey == job.ApprovalStatus.ToString()).FirstOrDefault().DValue,
                Reviewer = job.Reviewer,

                PayrolHigh = job.PayrolHigh,
                PayrolLow = job.PayrolLow,
                PayrollType = job.PayrollType.ToString(),
                TypeOfCurrency = job.TypeOfCurrency.ToString(),
                WorkType = job.WorkType.ToString(),

                //PayrollType = listHash.Where(y => y.DictionaryTypeId == 18 && y.DKey == job.PayrollType.ToString()).FirstOrDefault().DValue,
                //TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == job.TypeOfCurrency.ToString()).FirstOrDefault().Remarks,
                //WorkType = listHash.Where(y => y.DictionaryTypeId == 17 && y.DKey == job.WorkType.ToString()).FirstOrDefault().DValue,
                WorkPlace = job.WorkPlace,
                CompanyEmail = job.CompanyEmail,
                CompanyTel = job.CompanyTel,
                LanguageType = job.LanguageType.ToString()
            };

            return res;
        }
        #endregion

        #region 删除记录——admin

        /// <summary>
        /// 删除记录——admin
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Result_Msg DeleteById_Admin(int Id)
        {
            Result_Msg res = new Result_Msg();
            Jobs job = (from p in context.Jobs where p.Id == Id select p).FirstOrDefault();
            context.Jobs.Remove(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg ModifyJob_Admin(QueryCommon<JobsAddQuery> query)
        {
            Result_Msg res = new Result_Msg();

            Jobs jobOld = context.Jobs.FindById<Jobs>(query.ParamInfo.Id);
            if (jobOld != null)
            {
                jobOld.Id = query.ParamInfo.Id;
                jobOld.JobTitle = query.ParamInfo.JobTitle;
                jobOld.JobContent = query.ParamInfo.JobContent;
                jobOld.CreateDate = jobOld.CreateDate;
                jobOld.UpdateDate = query.ParamInfo.UpdateDate;
                jobOld.StartDate = query.ParamInfo.StartDate;

                jobOld.EndDate = query.ParamInfo.EndDate;
                jobOld.UserId = jobOld.UserId;
                jobOld.UserType = jobOld.UserType;
                jobOld.ApprovalStatus = query.ParamInfo.ApprovalStatus;
                jobOld.Reviewer = query.ParamInfo.Reviewer;

                jobOld.CompanyEmail = query.ParamInfo.CompanyEmail;
                jobOld.CompanyTel = query.ParamInfo.CompanyTel;
                jobOld.PayrolHigh = query.ParamInfo.PayrolHigh;
                jobOld.PayrolLow = query.ParamInfo.PayrolLow;
                jobOld.PayrollType = query.ParamInfo.PayrollType;

                jobOld.WorkPlace = query.ParamInfo.WorkPlace;
                jobOld.WorkType = query.ParamInfo.WorkType;
                jobOld.TypeOfCurrency = query.ParamInfo.TypeOfCurrency;
                jobOld.LanguageType = query.ParamInfo.LanguageType;
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
        #endregion

        #region 分页信息
        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_Admin(QueryCommon<JobsQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from job in context.Jobs select job;

                #region 拼接 Linq 查询条件
                if (query.ParamInfo.UserType != 0)
                {
                    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                }
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.ApprovalStatus != 0)
                {
                    linqList = linqList.Where(x => x.ApprovalStatus == query.ParamInfo.ApprovalStatus);
                }
                if (query.ParamInfo.JobTitle != null && query.ParamInfo.JobTitle != string.Empty)
                {
                    linqList = linqList.Where(x => x.JobTitle.IndexOf(query.ParamInfo.JobTitle) != -1);
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


        #endregion

        #region Web 后台

        #region 根据查询条件获取数据列表

        /// <summary>
        /// 根据查询条件获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public Result_List_Pager<Jobs> GetJobsList_Member(QueryCommon<JobsQuery> query)
        {
            Result_List_Pager<Jobs> res = new Result_List_Pager<Jobs>()
            {
                List = new List<Jobs>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {

                var linqList = from job in context.Jobs where job.UserType == query.ParamInfo.UserType && job.UserId == query.ParamInfo.UserId select job;

                if (query.ParamInfo.WorkType != 3)
                {
                    linqList = linqList.Where(x => x.WorkType == query.ParamInfo.WorkType);
                }
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.JobTitle != null && query.ParamInfo.JobTitle != string.Empty)
                {
                    linqList = linqList.Where(x => x.JobTitle.IndexOf(query.ParamInfo.JobTitle) != -1);
                }
                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

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
        #endregion

        #region 分页信息
        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_Member(QueryCommon<JobsQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from job in context.Jobs where job.UserType == query.ParamInfo.UserType && job.UserId == query.ParamInfo.UserId select job;

                #region 拼接 Linq 查询条件
                if (query.ParamInfo.WorkType != 3)
                {
                    linqList = linqList.Where(x => x.WorkType == query.ParamInfo.WorkType);
                }
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.JobTitle != null && query.ParamInfo.JobTitle != string.Empty)
                {
                    linqList = linqList.Where(x => x.JobTitle.IndexOf(query.ParamInfo.JobTitle) != -1);
                }
                int total = linqList.Count();
                #endregion

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

        #region 新增招聘信息
        /// <summary>
        /// 新增招聘信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg AddJob(QueryCommon<JobsAddQuery> query)
        {
            Result_Msg res = new Result_Msg();
            Jobs job = new Jobs()
            {
                JobTitle = query.ParamInfo.JobTitle,
                JobContent = query.ParamInfo.JobContent,
                CreateDate = query.ParamInfo.CreateDate,
                UpdateDate = query.ParamInfo.UpdateDate,
                StartDate = query.ParamInfo.StartDate,
                EndDate = query.ParamInfo.EndDate,
                UserId = query.ParamInfo.UserId,
                UserType = query.ParamInfo.UserType,
                ApprovalStatus = query.ParamInfo.ApprovalStatus,
                Reviewer = query.ParamInfo.Reviewer,
                PayrolHigh = query.ParamInfo.PayrolHigh,
                PayrolLow = query.ParamInfo.PayrolLow,
                PayrollType = query.ParamInfo.PayrollType,
                TypeOfCurrency = query.ParamInfo.TypeOfCurrency,
                WorkPlace = query.ParamInfo.WorkPlace,
                WorkType = query.ParamInfo.WorkType,
                CompanyTel = query.ParamInfo.CompanyTel,
                CompanyEmail = query.ParamInfo.CompanyEmail,
                LanguageType = query.ParamInfo.LanguageType
            };

            context.Jobs.Add(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }
        #endregion

        #region 删除指定ID数据
        /// <summary>
        /// 删除指定ID数据
        /// </summary>
        /// <param name="Id">招聘信息ID</param>
        /// <returns></returns>
        public Result_Msg DeleteById(int Id, long userId)
        {
            Result_Msg res = new Result_Msg();
            Jobs job = (from p in context.Jobs where p.Id == Id && p.UserId == userId select p).FirstOrDefault();
            context.Jobs.Remove(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }
        #endregion

        #region 获取招聘信息详情
        /// <summary>
        /// 获取招聘信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Jobs GetObjectById(int Id, long userId)
        {
            Jobs job = (from p in context.Jobs where p.Id == Id && p.UserId == userId select p).FirstOrDefault();
            return job;
        }
        #endregion

        #region 修改招聘信息
        /// <summary>
        /// 修改招聘信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg ModifyJob(QueryCommon<JobsAddQuery> query)
        {
            Result_Msg res = new Result_Msg();

            Jobs jobOld = context.Jobs.FindById<Jobs>(query.ParamInfo.Id);
            if (jobOld != null)
            {
                jobOld.Id = query.ParamInfo.Id;
                jobOld.JobTitle = query.ParamInfo.JobTitle;
                jobOld.JobContent = query.ParamInfo.JobContent;
                jobOld.CreateDate = jobOld.CreateDate;
                jobOld.UpdateDate = query.ParamInfo.UpdateDate;
                jobOld.StartDate = query.ParamInfo.StartDate;

                jobOld.EndDate = query.ParamInfo.EndDate;
                jobOld.UserId = query.ParamInfo.UserId;
                jobOld.UserType = query.ParamInfo.UserType;
                jobOld.ApprovalStatus = query.ParamInfo.ApprovalStatus;
                jobOld.Reviewer = query.ParamInfo.Reviewer;

                jobOld.CompanyEmail = query.ParamInfo.CompanyEmail;
                jobOld.CompanyTel = query.ParamInfo.CompanyTel;
                jobOld.PayrolHigh = query.ParamInfo.PayrolHigh;
                jobOld.PayrolLow = query.ParamInfo.PayrolLow;
                jobOld.PayrollType = query.ParamInfo.PayrollType;

                jobOld.WorkPlace = query.ParamInfo.WorkPlace;
                jobOld.WorkType = query.ParamInfo.WorkType;
                jobOld.TypeOfCurrency = query.ParamInfo.TypeOfCurrency;
                jobOld.LanguageType = query.ParamInfo.LanguageType;
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
        #endregion


        #endregion

        #region Web 前台展示

        #region 获取列表
        /// <summary>
        /// 获取招聘信息列表——Web
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<Jobs> GetJobsList_Web(QueryCommon<JobsQuery> query)
        {
            Result_List_Pager<Jobs> res = new Result_List_Pager<Jobs>()
            {
                List = new List<Jobs>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = from job in context.Jobs where job.ApprovalStatus == 3 select job;

                #region 拼接 Linq 查询条件
                if (query.ParamInfo.UserType != 0)
                {
                    //发布者类型（1：平台管理员？；2：供应商；3：采购商；）
                    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                }
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.PayrolLow == null && query.ParamInfo.PayrolHigh == null)
                {
                    //linqList = linqList.Where(x => x.PayrolLow == query.PayrolLow);
                }
                else if (query.ParamInfo.PayrolLow == null && query.ParamInfo.PayrolHigh >= 0)
                {
                    linqList = linqList.Where(x => x.PayrolHigh <= query.ParamInfo.PayrolHigh);
                }
                else if (query.ParamInfo.PayrolLow >= 0 && query.ParamInfo.PayrolHigh == null)
                {
                    linqList = linqList.Where(x => x.PayrolLow >= query.ParamInfo.PayrolLow);
                }
                else if (query.ParamInfo.PayrolLow <= query.ParamInfo.PayrolHigh)
                {
                    linqList = linqList.Where(x => x.PayrolLow >= query.ParamInfo.PayrolLow && x.PayrolHigh <= query.ParamInfo.PayrolHigh);
                }

                if (query.ParamInfo.WorkType != 3)
                {
                    //工作类型（0：全职；1：兼职；2：外包）
                    linqList = linqList.Where(x => x.WorkType == query.ParamInfo.WorkType);
                }
                if (query.ParamInfo.JobTitle != null && query.ParamInfo.JobTitle != string.Empty)
                {
                    linqList = linqList.Where(x => x.JobTitle.IndexOf(query.ParamInfo.JobTitle) != -1);
                }
                #endregion

                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

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
        #endregion

        #region 获取招聘信息详情
        /// <summary>
        /// 获取招聘信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result_Model<Jobs> GetObjectById_Web(int Id)
        {
            Result_Model<Jobs> res = new Result_Model<Jobs>()
            {
                Model = new Jobs(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                Jobs job = (from p in context.Jobs where p.Id == Id select p).FirstOrDefault();
                if (job == null)
                {
                    res.Msg = new Result_Msg() { IsSuccess = false, Message = "查询记录失败，记录不存在" };
                }
                else
                {
                    res.Model = job;
                }

            }
            catch (Exception ex)
            {

                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }

            return res;
        }
        #endregion

        #region 前后两条记录
        /// <summary>
        /// 前后两条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result_List<Result_Model<Jobs>> Get_PreNext_ById_Web(long id, int workType, int languageType)
        {
            Result_List<Result_Model<Jobs>> res = new Result_List<Result_Model<Jobs>>()
            {
                List = new List<Result_Model<Jobs>>(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            List<Jobs> linqList = new List<Jobs>();
            try
            {
                if (workType == 3)//所有
                {
                    linqList = (from listMeet in context.Jobs where listMeet.ApprovalStatus == 3 && listMeet.LanguageType == languageType select listMeet).OrderBy(p => p.EndDate).OrderByDescending(p => p.Id).ToList();
                }
                else
                {
                    linqList = (from listMeet in context.Jobs where listMeet.ApprovalStatus == 3 && listMeet.LanguageType == languageType && listMeet.WorkType == workType select listMeet).OrderBy(p => p.EndDate).OrderByDescending(p => p.Id).ToList();
                }

                int indexNo = linqList.IndexOf(linqList.First(x => x.Id == id));

                if (indexNo > 0)
                {
                    Jobs pre = linqList.Skip(indexNo - 1).Take(1).First();
                    if (pre != null)
                    {
                        res.List.Add(new Result_Model<Jobs>()
                        {
                            Model = pre,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<Jobs>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取上一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<Jobs>()
                    {
                        Msg = new Result_Msg() { IsSuccess = false, Message = "上一条没有了" }
                    });
                }

                if (indexNo < (linqList.Count - 1))
                {
                    Jobs next = linqList.Skip(indexNo + 1).Take(1).First();
                    //Jobs next = linqList.Skip(indexNo + 1).Take(1).First();

                    if (next != null)
                    {
                        res.List.Add(new Result_Model<Jobs>()
                        {
                            Model = next,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<Jobs>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取下一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<Jobs>()
                    {
                        Msg = new Result_Msg() { IsSuccess = false, Message = "下一条没有了" }
                    });
                }

            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = ex.Message;
            }
            return res;

        }
        #endregion

        #region 分页信息
        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo(QueryCommon<JobsQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from job in context.Jobs where job.ApprovalStatus == 3 select job;

                #region 拼接 Linq 查询条件
                if (query.ParamInfo.UserType != 0)
                {
                    //发布者类型（1：平台管理员？；2：供应商；3：采购商；）
                    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                }
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.PayrolLow == null && query.ParamInfo.PayrolHigh == null)
                {
                    //linqList = linqList.Where(x => x.PayrolLow == query.PayrolLow);
                }
                else if (query.ParamInfo.PayrolLow == null && query.ParamInfo.PayrolHigh >= 0)
                {
                    linqList = linqList.Where(x => x.PayrolHigh <= query.ParamInfo.PayrolHigh);
                }
                else if (query.ParamInfo.PayrolLow >= 0 && query.ParamInfo.PayrolHigh == null)
                {
                    linqList = linqList.Where(x => x.PayrolLow >= query.ParamInfo.PayrolLow);
                }
                else if (query.ParamInfo.PayrolLow <= query.ParamInfo.PayrolHigh)
                {
                    linqList = linqList.Where(x => x.PayrolLow >= query.ParamInfo.PayrolLow && x.PayrolHigh <= query.ParamInfo.PayrolHigh);
                }

                if (query.ParamInfo.WorkType != 3)
                {
                    //工作类型（0：全职；1：兼职；2：外包）
                    linqList = linqList.Where(x => x.WorkType == query.ParamInfo.WorkType);
                }
                if (query.ParamInfo.JobTitle != null && query.ParamInfo.JobTitle != string.Empty)
                {
                    linqList = linqList.Where(x => x.JobTitle.IndexOf(query.ParamInfo.JobTitle) != -1);
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

        #endregion

        #region Web、Admin 公共方法

        #region 根据用户ID，用户类型，获取公司名称（店铺名称）
        /// <summary>
        /// 根据用户ID，用户类型，获取公司信息（店铺名称）
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetCompanyInfo_ByUserIdAndUserType(long userId)
        {
            string companyName = string.Empty;
            UserMemberInfo memberInfo = (from p in context.UserMemberInfo where p.Id == userId select p).FirstOrDefault();

            if (memberInfo != null && memberInfo.UserName.Length > 0)
            {
                if (memberInfo.UserType == 3)//采购商
                {
                    MemberDetail model = (from p in context.MemberDetail where p.MemberId == userId select p).FirstOrDefault();
                    if (model != null && model.CompanyName.Length > 0)
                    {
                        companyName = model.CompanyName;
                    }
                    else
                    {
                        companyName = "采购商公司名称未设置";
                    }
                }
                else if (memberInfo.UserType == 2)//供应商
                {
                    ManagerInfo manager = (from p in context.ManagerInfo where p.UserName == memberInfo.UserName select p).FirstOrDefault();
                    ShopInfo shop = (from p in context.ShopInfo where p.Id == manager.ShopId select p).FirstOrDefault();
                    if (shop != null && shop.CompanyName != null && shop.CompanyName.Length > 0)
                    {
                        companyName = shop.CompanyName;
                    }
                    else
                    {
                        companyName = "店铺名称未设置";
                    }
                }
            }
            else
            {
                companyName = "会员信息获取失败";
            }

            return companyName;
        }
        #endregion

        #region 根据用户ID，用户类型，获取公司信息
        /// <summary>
        /// 根据用户ID，用户类型，获取公司信息
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string Get_ShopInfo_ByUserIdAndUserType(long userId)
        {
            string companyName = string.Empty;
            UserMemberInfo memberInfo = (from p in context.UserMemberInfo where p.Id == userId select p).FirstOrDefault();

            if (memberInfo != null && memberInfo.UserName.Length > 0)
            {
                if (memberInfo.UserType == 3)//采购商
                {
                    MemberDetail model = (from p in context.MemberDetail where p.MemberId == userId select p).FirstOrDefault();
                    if (model != null && model.CompanyName.Length > 0)
                    {
                        companyName = model.CompanyName;
                    }
                    else
                    {
                        companyName = "采购商公司名称未设置";
                    }
                }
                else if (memberInfo.UserType == 2)//供应商
                {
                    ManagerInfo manager = (from p in context.ManagerInfo where p.UserName == memberInfo.UserName select p).FirstOrDefault();
                    ShopInfo shop = (from p in context.ShopInfo where p.Id == manager.ShopId select p).FirstOrDefault();
                    if (shop != null && shop.CompanyName != null && shop.CompanyName.Length > 0)
                    {
                        companyName = shop.CompanyName;
                    }
                    else
                    {
                        companyName = "店铺名称未设置";
                    }
                }
            }
            else
            {
                companyName = "会员信息获取失败";
            }

            return companyName;
        }
        #endregion


        #endregion
    }
}
