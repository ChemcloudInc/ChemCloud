using ChemCloud.IServices;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class ConfigPPAPIController : Controller
    {
        // GET: Admin/ConfigPPAPI
        public ActionResult Index()
        {
            ChemCloud.Model.ConfigPayPalAPI model = new ChemCloud.Model.ConfigPayPalAPI();
            model = ServiceHelper.Create<IConfigPayPalAPIService>().GetConfigPayPalAPIInfo(1);
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveSetting(ChemCloud.Model.ConfigPayPalAPI model)
        {
            if (!base.ModelState.IsValid)
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "数据验证错误！"
                };
                return Json(result);
            }
            Result result1 = new Result();
            ServiceHelper.Create<IConfigPayPalAPIService>().UpdateConfigPayPalAPIInfo(model);
            return Json(new { Successful = true });
        }

    }
}
