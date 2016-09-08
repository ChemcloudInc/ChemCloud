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
    public class WhatBusyController : BaseAdminController
    {
        HashSet_Common hashSet = new HashSet_Common();
        IWhatBusyService whatBuys = ServiceHelper.Create<IWhatBusyService>();

        public WhatBusyController()
        {
        }

        public ViewResult WhatBusyList()
        {
            return new ViewResult();
        }

        #region 删除
        /// <summary>
        /// 删除banner信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteById(QueryCommon<WhatBusyQuery> query)
        {
            Result_Msg res = new Result_Msg();
            if (base.CurrentManager != null && base.CurrentManager.Id > 0)
            {
                long userId = base.CurrentManager.Id;

                res = whatBuys.DeleteById(query.ParamInfo.Id, userId);
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "您尚未登陆，或登录超时，请重新登陆";
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 列表2
        /// <summary>
        /// 获取banner信息列表2
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetWhatBusyList(QueryCommon<WhatBusyQuery> query)
        {
            IJobsService jobsService = ServiceHelper.Create<IJobsService>();

            Result_List_Pager<Result_WhatBusy> res = new Result_List_Pager<Result_WhatBusy>()
            {
                List = new List<Result_WhatBusy>(),
                Msg = new Result_Msg(),
                PageInfo = new PageInfo()
            };
            Result_List_Pager<WhatBusy> resDal = whatBuys.GetWhatBusyList(query);
            if (resDal != null && resDal.Msg.IsSuccess)
            {
                res.Msg = resDal.Msg;
                res.PageInfo = resDal.PageInfo;
                res.List = resDal.List.Select(x => new Result_WhatBusy()
                {
                    Id = x.Id,
                    BusyCotent = hashSet.Get_DictionariesList_ByTypeID(19).Where(y => y.DKey == x.BusyType.ToString()).FirstOrDefault().Remarks.Replace("#BusyContent#", x.BusyCotent.Length > 10 ? x.BusyCotent.Substring(0, 10) + "**" : x.BusyCotent),
                    BusyType = x.BusyType,
                    UserName = jobsService.GetCompanyInfo_ByUserIdAndUserType(x.UserId),
                    BusyTypeName = hashSet.Get_DictionariesList_ByTypeID(19).Where(y => y.DKey == x.BusyType.ToString()).FirstOrDefault().DValue,
                    IsShow = x.IsShow,
                    UserId = x.UserId,
                    TargetUrl = x.TargetUrl,
                    CreateDate = x.CreateDate.ToString("yyyy-MM-dd HH:mm")
                }).ToList();
            }
            else
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "数据库数据读取失败" };
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 根据ID获取指定对象
        [HttpPost]
        public string GetObjectById(int Id)
        {
            WhatBusy res = new WhatBusy();
            if (base.CurrentManager != null && base.CurrentManager.Id > 0)
            {
                long userId = base.CurrentManager.Id;
                res = whatBuys.GetObjectById(Id);
            }
            else
            {

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }
        #endregion

        #region 更新数据
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string UpdateWhatBusy_By_WhatBusyType(QueryCommon<WhatBusyQuery> query)
        {
            Result_Model<WhatBusy> res = new Result_Model<WhatBusy>();

            if (base.CurrentManager != null && base.CurrentManager.Id > 0)
            {
                long userId = base.CurrentManager.Id;
                res = whatBuys.UpdateWhatBusy_By_WhatBusyType(query);
            }
            else
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "计算" + hashSet.Get_DictionariesList_ByTypeID(19).Where(y => y.DKey == query.ParamInfo.BusyType.ToString()).FirstOrDefault().DValue + "数据失败";
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        /// <summary>
        /// 获取分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string Get_PageInfo(QueryCommon<WhatBusyQuery> query)
        {
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();

            resModel = whatBuys.Get_PageInfo(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion

    }


}
