using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class RecommendInfoController : BaseAdminController
    {
        // GET: Admin/RecommendInfo
        public ActionResult Management()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Edit(long id)
        {
            RecommendInfo recommendInfo = ServiceHelper.Create<IRecommendInfoService>().GetRecommendInfo(id);
            return View(recommendInfo);
        }
        [HttpPost]
        public JsonResult AddRecommendInfo(string platcode, string casno, string productName, string structure_2D, decimal price, string uname)
        {
            RecommendInfo recommendInfo = new RecommendInfo()
            {
                Plat_Code = platcode,
                CID = int.Parse(structure_2D),
                CASNO = casno,
                ProductName = productName,
                Structure_2D = "",
                Price = price,
                Status = 2,
                CreateDate = DateTime.Now,
                ActiveTime = DateTime.Now,
                UserId = base.CurrentManager.Id,
                UserName = uname
            };
            bool flag = ServiceHelper.Create<IRecommendInfoService>().AddRecommendInfo(recommendInfo);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult List(int page, int rows, string CID, int? Status, string BeginTime = "", string EndTime = "")
        {
            RecommendInfoQuery model = new RecommendInfoQuery();
            model.Plat_Code = CID;
            model.Status = Status;
            DateTime dt;
            if (DateTime.TryParse(BeginTime, out dt) && DateTime.TryParse(EndTime, out dt))
            {
                model.BeginTime = DateTime.Parse(BeginTime);
                model.EndTime = DateTime.Parse(EndTime);
                model.PageNo = page;
                model.PageSize = rows;
            }
            else
            {
                model.PageNo = page;
                model.PageSize = rows;
            }
            PageModel<RecommendInfo> opModel = ServiceHelper.Create<IRecommendInfoService>().GetRecommendInfos(model);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, CID = item.CID, CASNO = item.CASNO, ProductName = item.ProductName, Structure_2D = item.Structure_2D, Price = item.Price, Status = item.Status, Plat_Code = item.Plat_Code };
            return Json(new { rows = array, total = opModel.Total });
        }
        [HttpPost]
        public JsonResult UpdateRecommendInfo(long id, decimal price)
        {
            bool flag = ServiceHelper.Create<IRecommendInfoService>().UpdateRecommendInfo(id, price);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult DeleteRecommendInfo(long id)
        {
            bool flag = ServiceHelper.Create<IRecommendInfoService>().DeleteRecommendInfo(id);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult BatchDelete(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            bool flag = ServiceHelper.Create<IRecommendInfoService>().BatchDelete(nums.ToArray());
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UpdateRecommendInfoStatus(long id, int status)
        {
            bool flag = ServiceHelper.Create<IRecommendInfoService>().UpdateRecommendInfoStatus(id, status, base.CurrentManager.Id);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult BatchUpdateRecommendInfoStatus(string ids, int status)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            bool flag = ServiceHelper.Create<IRecommendInfoService>().BatchUpdateRecommendInfoStatus(nums.ToArray(), status, base.CurrentManager.Id);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult BatchAddRecommendInfo(string json)
        {
            List<RecommendInfo> modelList = new List<RecommendInfo>();
            RecommendInfo recommendInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<RecommendInfo>(json);
            if (recommendInfo._RecommendInfos != null)
            {
                foreach (RecommendInfo list in recommendInfo._RecommendInfos.ToList())
                {
                    list.Status = 2;
                    list.CreateDate = DateTime.Now;
                    list.ActiveTime = DateTime.Now;
                    list.UserId = base.CurrentManager.Id;
                }
                modelList = recommendInfo._RecommendInfos;
                bool flag = ServiceHelper.Create<IRecommendInfoService>().BatchAddRecommendInfo(modelList);
                if (flag)
                    return Json(new { success = true });
                else
                    return Json(new { success = false });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public JsonResult GetCasData(string platcode)
        {
            ProductInfo pinfo = ServiceHelper.Create<IProductService>().GetProductByPlatCode(platcode);
            if (pinfo != null)
            {
                ShopInfo sinfo = ServiceHelper.Create<IShopService>().GetShop(pinfo.ShopId);
                if (sinfo == null)
                {
                    return Json("");
                }
                else
                {
                    decimal a = ServiceHelper.Create<IProductService>().GetProductPrice(pinfo.Id);
                    return Json(new { CASNo = pinfo.CASNo, ProductName = pinfo.EProductName, STRUCTURE_2D = pinfo.Pub_CID, price = a, ShopName = sinfo.ShopName });
                }
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetRecommendInfosData(string platCode)
        {
            List<ProductInfo> productInfos = ServiceHelper.Create<IRecommendInfoService>().GetRecommendInfosLikePlatCode(platCode);
            List<RecommendInfo> recommendInfos = new List<RecommendInfo>();
            foreach (ProductInfo list in productInfos)
            {
                RecommendInfo recommendInfo = new RecommendInfo();
                recommendInfo.Plat_Code = list.Plat_Code;
                recommendInfo.CID = list.Pub_CID;
                recommendInfo.CASNO = list.CASNo;
                recommendInfo.ProductName = list.ProductName;
                recommendInfo.Price = list.Price;
                recommendInfo.UserName = ServiceHelper.Create<IManagerService>().GetManagerInfoByShopId(list.ShopId).UserName;
                recommendInfos.Add(recommendInfo);
            }
            int count = recommendInfos.Count();
            if (count <= 0)
            {
                return Json(new { success = false, msg = "没有找到数据" });
            }
            return Json(new { success = true, rows = recommendInfos, total = count });
        }
    }
}