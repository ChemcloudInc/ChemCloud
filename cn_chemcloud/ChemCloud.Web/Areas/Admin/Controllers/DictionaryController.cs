using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Admin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class DictionaryController : BaseAdminController
    {
        public DictionaryController()
        {

        }

        #region  字典类型
        //字典类型视图
        public ActionResult DictionaryTypeList()
        {
            return View();
        }

        public ActionResult DictionaryTypeEdit(long Id)
        {
            ChemCloud_DictionaryType _ChemCloud_DictionaryType =
                ServiceHelper.Create<IChemCloud_DictionaryTypeService>().GetChemCloud_DictionaryType(Id);
            return View(_ChemCloud_DictionaryType);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult DictionaryTypeEditSave(string Id, string TypeName, string IsEnabled)
        {
            ChemCloud_DictionaryType model = new ChemCloud_DictionaryType();
            model.Id = long.Parse(Id);
            model.TypeName = TypeName;
            model.IsEnabled = IsEnabled;
            bool result = ServiceHelper.Create<IChemCloud_DictionaryTypeService>().UpdateChemCloud_DictionaryType(model);
            return Json(new { success = result });
        }

        [UnAuthorize]
        public JsonResult DictionaryTypePageModelList(int page, int rows, string typename, string isenabled)
        {
            ChemCloud_DictionaryType casewhere = new ChemCloud_DictionaryType()
                      {
                          PageNo = page,
                          PageSize = rows,
                          TypeName = typename,
                          IsEnabled = isenabled
                      };
            PageModel<ChemCloud_DictionaryType> listpage = ServiceHelper.Create<IChemCloud_DictionaryTypeService>().GetPage_ChemCloud_DictionaryType(casewhere);

            var collection =
                from item in listpage.Models.ToList()
                select new
                {
                    Id = item.Id,
                    TypeName = item.TypeName,
                    IsEnabled = item.IsEnabled
                };
            return Json(new { rows = collection, total = listpage.Total });
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult DictionaryTypeAddFuc(string typename, string isenable)
        {
            ChemCloud_DictionaryType model = new ChemCloud_DictionaryType();
            model.TypeName = typename;
            model.IsEnabled = isenable;
            bool result = ServiceHelper.Create<IChemCloud_DictionaryTypeService>().AddChemCloud_DictionaryType(model);
            return Json(new { success = result });
        }

        public JsonResult DeleteType(long id)
        {
            bool flag = ServiceHelper.Create<IChemCloud_DictionaryTypeService>().DeleteChemCloud_DictionaryType(id);
            return Json(new { success = flag });
        }
        [HttpPost]
        public JsonResult BatchDeleteType(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            ServiceHelper.Create<IChemCloud_DictionaryTypeService>().BatchDeleteChemCloud_DictionaryType(nums.ToArray());
            Result result = new Result()
            {
                success = true,
                msg = "批量删除成功！"
            };
            return Json(result);
        }

        #endregion

        #region  字典
        //字典视图
        public ActionResult DictionariesList()
        {
            ChemCloud_DictionaryType querymodel = new ChemCloud_DictionaryType()
            {
                IsEnabled = "1"
            };
            List<ChemCloud_DictionaryType> listtype =
                ServiceHelper.Create<IChemCloud_DictionaryTypeService>().GetChemCloud_DictionaryTypes(querymodel);
            return View(listtype);
        }

        [UnAuthorize]
        public JsonResult DictionariesPageModelList(int page, int rows, string dkey = "", string dvalue_en = "", string typeid = "0")
        {
            ChemCloud_Dictionaries casewhere = new ChemCloud_Dictionaries()
            {
                PageNo = page,
                PageSize = rows,
                DKey = dkey,
                DictionaryTypeId = long.Parse(typeid)
            };
            PageModel<ChemCloud_Dictionaries> listpage =
                ServiceHelper.Create<IChemCloud_DictionariesService>().GetPage_ChemCloud_Dictionaries(casewhere);

            var collection =
                from item in listpage.Models.ToList()
                select new
                {
                    Id = item.Id,
                    DictionaryTypeId = item.DictionaryTypeId,
                    TypeName = ServiceHelper.Create<IChemCloud_DictionaryTypeService>().GetChemCloud_DictionaryType(item.DictionaryTypeId) == null ? "" : ServiceHelper.Create<IChemCloud_DictionaryTypeService>().GetChemCloud_DictionaryType(item.DictionaryTypeId).TypeName,
                    DKey = item.DKey,
                    DValue = item.DValue,
                    DValue_En = item.DValue_En,
                    Remarks = item.Remarks
                };
            return Json(new { rows = collection, total = listpage.Total });
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult DictionaryAddFuc(string DictionaryTypeId = "0", string DKey = "", string DValue = "", string DValue_En = "", string Remarks = "")
        {
            ChemCloud_Dictionaries model = new ChemCloud_Dictionaries();
            model.DictionaryTypeId = long.Parse(DictionaryTypeId);
            model.DKey = DKey;
            model.DValue = DValue;
            model.DValue_En = DValue_En;
            model.Remarks = Remarks;
            bool result = ServiceHelper.Create<IChemCloud_DictionariesService>().AddChemCloud_Dictionaries(model);
            return Json(new { success = result });
        }

        public ActionResult DictionariesEdit(long Id)
        {
            ChemCloud_DictionaryType query = new ChemCloud_DictionaryType() { IsEnabled = "1" };
            List<ChemCloud_DictionaryType> typelist =
                ServiceHelper.Create<IChemCloud_DictionaryTypeService>().GetChemCloud_DictionaryTypes(query);
            ViewBag.typelist = typelist;

            ChemCloud_Dictionaries _ChemCloud_Dictionaries =
                ServiceHelper.Create<IChemCloud_DictionariesService>().GetChemCloud_Dictionaries(Id);
            return View(_ChemCloud_Dictionaries);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult DictionaryEditSave(string Id, string DictionaryTypeId = "0", string DKey = "", string DValue = "", string DValue_En = "", string Remarks = "")
        {
            ChemCloud_Dictionaries model = new ChemCloud_Dictionaries();
            model.Id = long.Parse(Id);
            model.DictionaryTypeId = long.Parse(DictionaryTypeId);
            model.DKey = DKey;
            model.DValue = DValue;
            model.DValue_En = DValue_En;
            model.Remarks = Remarks;
            bool result = ServiceHelper.Create<IChemCloud_DictionariesService>().UpdateChemCloud_Dictionaries(model);
            return Json(new { success = result });
        }

        public JsonResult DeleteDictionary(long id)
        {
            bool flag = ServiceHelper.Create<IChemCloud_DictionariesService>().DeleteChemCloud_Dictionaries(id);
            return Json(new { success = flag });
        }
        [HttpPost]
        public JsonResult BatchDeleteDictionary(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            ServiceHelper.Create<IChemCloud_DictionariesService>().BatchDeleteChemCloud_Dictionaries(nums.ToArray());
            Result result = new Result()
            {
                success = true,
                msg = "批量删除成功！"
            };
            return Json(result);
        }
        #endregion
    }
}