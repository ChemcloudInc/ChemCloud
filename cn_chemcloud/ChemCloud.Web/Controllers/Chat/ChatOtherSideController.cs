//-------------------------------------------------------  
// <copyright  company='南京沃斯多克'> 
// Copyright 南京沃斯多克 All rights reserved.
// </copyright>  
//------------------------------------------------------- 
namespace ChemCloud.Web.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Razor;
    using System.Web.Routing;
    using System.Linq;
    using System.Linq.Expressions;
    using ChemCloud.Web.Models;
    /// <summary>
    ///  卖方 买方 客服 控制器
    /// </summary>
    public class ChatOtherSideController : Controller
    {
     
        [HttpGet()]
        /// <summary>
        /// get 供应商 ID，UserName 
        /// </summary>
        /// <param name="shopID"></param>
        /// <returns></returns>
        public JsonResult GetOtherSideInfoByShop(string shopID)
        {
           
           
            var data = Data.ChatOtherSide.GetOtherSideInfoByShop(shopID);

            if (data.Count() > 0)
            {
                return Json(data.First(),JsonRequestBehavior.AllowGet);
            }
            else
            {
                //没有这个商家
                return Json(new Models.OtherSideViewModel() {
                
                    Result=-1
                
                }, JsonRequestBehavior.AllowGet);
            }

            
        }

        /// <summary>
        /// 搜索好友页面
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public ActionResult GetSearchOtherSidePage(string searchText, int customerLimit, int memberLimit)
        {

            int i = 10;
            var data = Data.ChatOtherSide.GetOtherSide(searchText, customerLimit, memberLimit);

            return this.PartialView("~/Views/Chat/SearchOtherSidePage.cshtml", data);
 
        }
       
    }
}



