using AutoMapper;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class EmailSettingController : BaseAdminController
    {
        // GET: Admin/EmailSetting
        public ActionResult Index()
        {
            ChemCloud.Model.ChemCloud_EmailSetting model = new ChemCloud.Model.ChemCloud_EmailSetting();
            model = ServiceHelper.Create<IEmailSettingService>().GetChemCloud_EmailSetting();
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveSetting(ChemCloud.Model.ChemCloud_EmailSetting model)
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
            if (model.Id > 0)
            {
                if (ServiceHelper.Create<IEmailSettingService>().EditEmailSetting(model))
                {
                    result1.success = true;
                    result1.msg = "编辑成功！";
                }
                else
                {
                    result1.success = false;
                    result1.msg = "编辑失败！";
                }
            }
            else
            {
                if (ServiceHelper.Create<IEmailSettingService>().AddEmailSetting(model))
                {
                    result1.success = true;
                    result1.msg = "添加成功！";
                }
                else
                {
                    result1.success = false;
                    result1.msg = "添加失败！";
                }
            }
            return Json(result1);
        }
    }
}