using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Model.Common;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.Admin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Services;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class MeetingInfoController : BaseWebController
    {
        // GET: SellerAdmin/MeetingInfo
        public ActionResult Management()
        {
            return View();
        }
        public ActionResult ManagementWeb()
        {
            return View();
        }
        public ActionResult ManagementWebDes()
        {
            return View();
        }
        

        #region 前台会议中心

        #region 根据ID获取指定对象
        /// <summary>
        /// 根据ID获取指定对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetObjectById_Web(int Id)
        {

            Result_Model_AttachmentList_PreNextRow resAll = new Result_Model_AttachmentList_PreNextRow()
            {
                Model = new Result_MeetingInfo(),
                AttachmentList = new Result_List<Result_AttachmentInfo>(),
                Msg = new Result_Msg() { IsSuccess = true, Message = string.Empty },
                PreNextRow = new Result_List<Result_Model<MeetingInfo>>()
            };
            try
            {
                IMeetInfoService jobsService = ServiceHelper.Create<IMeetInfoService>();
                resAll.Model = jobsService.GetObjectById_Web(Id);
                if (resAll.Model != null && resAll.Msg.IsSuccess)
                {
                    resAll.AttachmentList = GetObjectList_ById_Web(Id);
                    if (!resAll.AttachmentList.Msg.IsSuccess)
                    {
                        resAll.Msg.IsSuccess = false;
                        resAll.Msg.Message += "获取附件列表失败\n\r";
                    }
                    else
                    {
                        foreach (var item in resAll.AttachmentList.List)
                        {
                            try
                            {
                                item.FileName = item.AttachmentName.Substring(item.AttachmentName.LastIndexOf('/') + 1, item.AttachmentName.Length - item.AttachmentName.LastIndexOf('/') - 1);
                            }
                            catch (Exception)
                            {
                                item.FileName = "附件信息获取失败";
                            }
                        }
                    }
                    resAll.PreNextRow = Get_PreNext_ById_Web(Id);
                    if (!resAll.PreNextRow.Msg.IsSuccess)
                    {
                        resAll.Msg.IsSuccess = false;
                        resAll.Msg.Message += "获取上一页、下一页\n\r";
                    }
                }
                else
                {
                    resAll.Msg.IsSuccess = false;
                    resAll.Msg.Message += "获取会议数据失败\n\r";
                }
            }
            catch (Exception ex)
            {
                resAll.Msg = new Result_Msg() { IsSuccess = false, Message = "读取失败，失败原因：" + ex.Message };
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(resAll);
        }
        #endregion

        #region 会议中心  列表
        /// <summary>
        /// 会议中心  列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string MeetingsList_Web(QueryCommon<MeetingInfoQuery_Web> query)
        {
            if (query.ParamInfo.Type > 0)
            {
                query.ParamInfo.EndTime = DateTime.Now.Date;
            }
            //else
            //{
            //    query.ParamInfo.StartTime = query.ParamInfo.StartTime.Date;
            //    query.ParamInfo.EndTime = query.ParamInfo.EndTime.Date.AddDays(1);
            //}

            IMeetInfoService jobsService = ServiceHelper.Create<IMeetInfoService>();
            Result_List_Pager<Result_MeetingInfo> res = jobsService.GetMeetingsList_Web(query);
            List<Result_AttachmentInfo> resItem = new List<Result_AttachmentInfo>();
            foreach (var item in res.List)
            {
                resItem = GetObjectList_ById_Web(item.Id).List.Where(x => x.AttachmentName != null && x.AttachmentName != "" && x.AttachmentName.LastIndexOf('.') > 0).ToList();
                if (resItem != null && resItem.Count > 0)
                {
                    Result_AttachmentInfo imgItem = resItem.Where(x => HashSet_Common.ImageTypeArr.Contains(x.AttachmentName.Substring(x.AttachmentName.LastIndexOf('.'), x.AttachmentName.Length - x.AttachmentName.LastIndexOf('.')))).FirstOrDefault();
                    if (imgItem != null)
                    {
                        item.AttachmentName = imgItem.AttachmentName;

                    }
                    else
                    {
                        item.AttachmentName = "";
                    }
                }
                else
                {
                    item.AttachmentName = "";
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 根据ID获取附件列表
        /// <summary>
        /// 根据ID获取附件列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result_List<Result_AttachmentInfo> GetObjectList_ById_Web(long Id)
        {
            Result_List<Result_AttachmentInfo> res = new Result_List<Result_AttachmentInfo>();

            try
            {
                IMeetInfoService jobsService = ServiceHelper.Create<IMeetInfoService>();
                res = jobsService.GetObjectList_ById_Web(Id);
                //res.List.Select(x => x.FileName == System.IO.Path.GetFileName(x.AttachmentName));
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "读取失败，失败原因：" + ex.Message };
            }

            return res;
        }
        #endregion

        #region 根据ID 前后两条信息
        /// <summary>
        /// 根据ID 前后两条信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result_List<Result_Model<MeetingInfo>> Get_PreNext_ById_Web(long Id)
        {
            Result_List<Result_Model<MeetingInfo>> res = new Result_List<Result_Model<MeetingInfo>>();

            try
            {
                IMeetInfoService jobsService = ServiceHelper.Create<IMeetInfoService>();
                res = jobsService.Get_PreNext_ById_Web(Id);
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "读取失败，失败原因：" + ex.Message };
            }

            return res;
        }
        #endregion

        #region 获取分页信息
        [WebMethod]

        public string Get_PageInfo(QueryCommon<MeetingInfoQuery_Web> query)
        {
            if (query.ParamInfo.Type > 0)
            {
                query.ParamInfo.EndTime = DateTime.Now.Date;
            }
            //else
            //{
            //    query.ParamInfo.StartTime = query.ParamInfo.StartTime.Date;
            //    query.ParamInfo.EndTime = query.ParamInfo.EndTime.Date.AddDays(1);
            //}

            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            IMeetInfoService jobsService = ServiceHelper.Create<IMeetInfoService>();
            resModel = jobsService.Get_PageInfo_Web(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion

        #endregion
    }
}