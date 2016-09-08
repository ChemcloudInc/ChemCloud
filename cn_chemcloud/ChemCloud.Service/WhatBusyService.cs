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
    public class WhatBusyService : ServiceBase, IWhatBusyService, IService, IDisposable
    {
        HashSet_Common hashSet = new HashSet_Common();
        JobsService job = new JobsService();

        /// <summary>
        /// 根据查询条件获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public Result_List_Pager<WhatBusy> GetWhatBusyList(QueryCommon<WhatBusyQuery> query)
        {
            Result_List_Pager<WhatBusy> res = new Result_List_Pager<WhatBusy>()
            {
                List = new List<WhatBusy>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                int total = 0;
                if (query.ParamInfo.BusyType == 0)
                {
                    total = (from p in context.WhatBusy select p).Count();
                    res.List = (from p in context.WhatBusy select p).OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();
                }
                else
                {
                    total = (from p in context.WhatBusy where p.BusyType == query.ParamInfo.BusyType select p).Count();
                    res.List = (from p in context.WhatBusy where p.BusyType == query.ParamInfo.BusyType select p).OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();
                }
                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
                res.Msg.IsSuccess = true;
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 加载首页Banner
        /// </summary>
        /// <returns></returns>
        public Result_List<Result_WhatBusy> Get_WhatBusy_Top(int count)
        {
            JobsService jobsService = new JobsService();

            Result_List<Result_WhatBusy> resList = new Result_List<Result_WhatBusy>();
            try
            {
                var list1 = (from p in context.WhatBusy select p).OrderByDescending(x => x.CreateDate).Take(count).ToList();
                resList.List = list1.Select(x => new Result_WhatBusy()
               {
                   Id = x.Id,
                   BusyCotent = hashSet.Get_DictionariesList_ByTypeID(19).Where(y => y.DKey == x.BusyType.ToString()).FirstOrDefault().Remarks.Replace("#BusyContent#", x.BusyCotent.Length > 6 ? x.BusyCotent.Substring(0, 6) + "..." : x.BusyCotent),
                   BusyType = x.BusyType,
                   UserName = jobsService.GetCompanyInfo_ByUserIdAndUserType(x.UserId).Length > 6 ? jobsService.GetCompanyInfo_ByUserIdAndUserType(x.UserId).Substring(0, 6)+"..." : jobsService.GetCompanyInfo_ByUserIdAndUserType(x.UserId),
                   BusyTypeName = hashSet.Get_DictionariesList_ByTypeID(19).Where(y => y.DKey == x.BusyType.ToString()).FirstOrDefault().DValue,
                   IsShow = x.IsShow,
                   UserId = x.UserId,
                   TargetUrl = x.TargetUrl,
                   CreateDate = x.CreateDate.ToString("MM-dd")
               }).ToList();

                resList.List.Select(x => x.UserName.Length > 7 ? x.UserName.Substring(0, 7) + "**" : x.UserName);

                resList.Msg = new Result_Msg() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                resList.Msg = new Result_Msg() { IsSuccess = false, Message = "查询交易记录失败，失败原因：" + ex.Message };
            }
            return resList;
        }

        /// <summary>
        /// 删除指定ID数据
        /// </summary>
        /// <param name="Id">实时交易信息ID</param>
        /// <returns></returns>
        public Result_Msg DeleteById(long Id, long userId)
        {
            Result_Msg res = new Result_Msg();
            WhatBusy job = (from p in context.WhatBusy where p.Id == Id select p).FirstOrDefault();
            context.WhatBusy.Remove(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }


        /// <summary>
        /// 获取实时交易信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WhatBusy GetObjectById(int Id)
        {
            WhatBusy model = (from p in context.WhatBusy where p.Id == Id select p).FirstOrDefault();
            WhatBusy res = new WhatBusy()
            {
                Id = model.Id,
                BusyCotent = model.BusyCotent,
                BusyType = model.BusyType,
                IsShow = model.IsShow,
                UserId = model.UserId,
                TargetUrl = model.TargetUrl,
                CreateDate = model.CreateDate
            };

            return res;
        }



        public Result_Model<WhatBusy> UpdateWhatBusy_By_WhatBusyType(QueryCommon<WhatBusyQuery> query)
        {
            Result_Model<WhatBusy> res = new Result_Model<WhatBusy>() { Msg = new Result_Msg(), Model = new WhatBusy() };
            WhatBusy whatBusy = new WhatBusy();

            #region 删除之前记录
            var whatBusyList = from p in context.WhatBusy where p.BusyType == query.ParamInfo.BusyType select p;
            if (whatBusyList.Any())
            {
                foreach (var item in whatBusyList)
                {
                    context.WhatBusy.Remove(item);
                }
                int count = context.SaveChanges();
            }
            #endregion

            #region 新增新记录
            //数据来源类型（1：用户注册；2：定制合成；3：代理采购；4：招聘信息；5：会议信息；6：技术交易；）
            switch (query.ParamInfo.BusyType)
            {
                case 1:
                    #region 用户注册
                    var list1 = (from p in context.UserMemberInfo orderby p.CreateDate descending select p).Take(query.ParamInfo.TopCount).ToList();
                    foreach (var item in list1)
                    {
                        int userType = (from p in context.UserMemberInfo where p.Id == item.Id select p.UserType).FirstOrDefault();
                        whatBusy = new WhatBusy()
                        {
                            BusyType = query.ParamInfo.BusyType,
                            IsShow = 1,
                            TargetUrl = string.Empty,

                            UserId = item.Id,
                            UserName = job.GetCompanyInfo_ByUserIdAndUserType(item.Id),
                            BusyCotent = hashSet.Get_DictionariesList_ByTypeID(15).Where(x => x.DKey == userType.ToString()).FirstOrDefault().DValue,
                            CreateDate = item.CreateDate
                        };
                        context.WhatBusy.Add(whatBusy);
                    }
                    #endregion

                    break;
                case 2:
                    #region 定制合成
                    var list2 = (from p in context.OrderSynthesis orderby p.OrderTime descending select p).Take(query.ParamInfo.TopCount).ToList();
                    foreach (var item in list2)
                    {
                        whatBusy = new WhatBusy()
                        {
                            BusyType = query.ParamInfo.BusyType,
                            IsShow = 1,
                            TargetUrl = string.Empty,

                            UserId = item.UserId,
                            UserName = item.CompanyName,
                            BusyCotent = item.ProductName,
                            CreateDate = item.OrderTime
                        };
                        context.WhatBusy.Add(whatBusy);
                    }
                    #endregion

                    break;
                case 3:
                    #region 代理采购
                    var list3 = (from p in context.OrderPurchasing orderby p.OrderTime descending select p).Take(query.ParamInfo.TopCount).ToList();
                    foreach (var item in list3)
                    {
                        whatBusy = new WhatBusy()
                        {
                            BusyType = query.ParamInfo.BusyType,
                            IsShow = 1,
                            TargetUrl = string.Empty,

                            UserId = item.UserId,
                            UserName = item.CompanyName,
                            BusyCotent = item.ProductName,
                            CreateDate = item.OrderTime
                        };
                        context.WhatBusy.Add(whatBusy);
                    }
                    #endregion

                    break;
                case 4:
                    #region 招聘信息
                    var list4 = (from p in context.Jobs orderby p.CreateDate descending select p).Take(query.ParamInfo.TopCount).ToList();
                    foreach (var item in list4)
                    {
                        whatBusy = new WhatBusy()
                        {
                            BusyType = query.ParamInfo.BusyType,
                            IsShow = 1,
                            TargetUrl = string.Empty,

                            UserId = item.UserId,
                            UserName = job.GetCompanyInfo_ByUserIdAndUserType(item.UserId),
                            BusyCotent = item.JobTitle,
                            CreateDate = item.CreateDate
                        };
                        context.WhatBusy.Add(whatBusy);
                    }
                    #endregion

                    break;
                case 5:
                    #region 会议信息
                    var list5 = (from p in context.MeetingInfo orderby p.CreatDate descending select p).Take(query.ParamInfo.TopCount).ToList();
                    foreach (var item in list5)
                    {
                        whatBusy = new WhatBusy()
                        {
                            BusyType = query.ParamInfo.BusyType,
                            IsShow = 1,
                            TargetUrl = string.Empty,

                            UserId = item.UserId,
                            UserName = job.GetCompanyInfo_ByUserIdAndUserType(item.UserId),
                            BusyCotent = item.Title,
                            CreateDate = item.CreatDate
                        };
                        context.WhatBusy.Add(whatBusy);
                    }
                    #endregion

                    break;
                case 6:
                    #region 技术交易
                    var list6 = (from p in context.TechnicalInfo orderby p.PublishTime descending select p).Take(query.ParamInfo.TopCount).ToList();
                    foreach (var item in list6)
                    {
                        whatBusy = new WhatBusy()
                        {
                            BusyType = query.ParamInfo.BusyType,
                            IsShow = 1,
                            TargetUrl = string.Empty,

                            UserId = item.PublisherId,
                            UserName = job.GetCompanyInfo_ByUserIdAndUserType(item.PublisherId),
                            BusyCotent = item.Title,
                            CreateDate = item.PublishTime
                        };
                        context.WhatBusy.Add(whatBusy);
                    }
                    #endregion

                    break;
                default:

                    break;
            }
            #endregion

            res.Msg.IsSuccess = context.SaveChanges() > 0;
            return res;
        }

        #region 分页信息
        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo(QueryCommon<WhatBusyQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from job in context.Jobs select job;
                int total = 0;
                if (query.ParamInfo.BusyType == 0)
                {
                    total = (from p in context.WhatBusy select p).Count();
                }
                else
                {
                    total = (from p in context.WhatBusy where p.BusyType == query.ParamInfo.BusyType select p).Count();
                }

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
    }
}
