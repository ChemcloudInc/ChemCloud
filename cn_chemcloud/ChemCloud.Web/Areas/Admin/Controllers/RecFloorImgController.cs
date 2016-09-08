using AutoMapper;
using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.Core;
using System.Web.Script.Serialization;


namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class RecFloorImgController : Controller
    {
        // GET: Admin/RecFloorImg

        public RecFloorImgController()
        {
        }
        public ActionResult Edit()
        {
            int typeid=Convert.ToInt32(ServiceHelper.Create<ISiteSettingService>().GetSiteValue("RecommandIndexImg"));
            List<RecFloorImg> rfiList = ServiceHelper.Create<IRecFloorImgService>().GetAll(typeid);
            ViewBag.imglist = rfiList;
            ViewBag.typeid = typeid;
            return View();
        }
        [HttpPost]
        public JsonResult Update(string Id, string URL, string tag, string ImageURL)
        {
            RecFloorImg rfi = ServiceHelper.Create<IRecFloorImgService>().GetSingle(Convert.ToInt32(Id));
            rfi.ImageUrl = ImageURL;
            rfi.URL = URL;
            rfi.Tag = tag;
            ServiceHelper.Create<IRecFloorImgService>().Update(rfi);
            Result result = new Result();
            result.msg = "OK";
            result.success = true;
            return Json(result);


        }
        [HttpPost]
        public string GetTypeId(string id)
        {
            int typeid = Convert.ToInt32(id);
            List<RecFloorImg> rfiList = ServiceHelper.Create<IRecFloorImgService>().GetAll(typeid);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(rfiList);
        }
        [HttpPost]
        public JsonResult UpdateSetting(string typeid)
        {
            typeid = typeid.Substring(5);
            SiteSettingsInfo sitesettinginfo = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings("RecommandIndexImg");
            sitesettinginfo.Value = typeid;
            ServiceHelper.Create<ISiteSettingService>().UpdateSiteSettings(sitesettinginfo);
            Result result = new Result();
            result.success = true;
            result.msg = "OK";
            return Json(result);
        }

    }
}