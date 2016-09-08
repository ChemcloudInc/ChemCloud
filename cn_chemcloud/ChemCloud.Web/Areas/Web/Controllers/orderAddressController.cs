using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class orderAddressController : BaseMemberController
    {
        public orderAddressController()
        {
        }

        [HttpPost]
        public JsonResult AddorderAddress(orderAddressInfo info)
        {
            info.UserId = base.CurrentUser.Id;
            ServiceHelper.Create<IorderAddressService>().AddorderAddress(info);
            return Json(new { success = true, msg = "添加成功", id = info.Id });
        }

        [HttpPost]
        public JsonResult DeleteorderAddress(long id)
        {
            long num = base.CurrentUser.Id;
            ServiceHelper.Create<IorderAddressService>().DeleteorderAddress(id, num);
            Result result = new Result()
            {
                success = true,
                msg = "删除成功"
            };
            return Json(result);
        }

        [HttpPost]
        public JsonResult EditorderAddress(orderAddressInfo info)
        {
            info.UserId = base.CurrentUser.Id;
            ServiceHelper.Create<IorderAddressService>().UpdateorderAddress(info);
            return Json(new { success = true, msg = "修改成功", id = info.Id });
        }

        [HttpPost]
        public JsonResult GetorderAddress(long id)
        {
            orderAddressInfo userorderAddress = ServiceHelper.Create<IorderAddressService>().GetUserorderAddress(id);
            var variable = new { id = userorderAddress.Id, fullRegionName = userorderAddress.RegionFullName, address = userorderAddress.Address, phone = userorderAddress.Phone, shipTo = userorderAddress.ShipTo, fullRegionIdPath = userorderAddress.RegionIdPath };
            return Json(variable);
        }

        public ActionResult Index(string fromorder = "0")
        {
            if (fromorder == "1")
            {
                ViewBag.fromorder = 1;
            }
            else
            {
                ViewBag.fromorder = 0;
            }
            long id = base.CurrentUser.Id;
            return View(ServiceHelper.Create<IorderAddressService>().GetUserorderAddressByUserId(id));
        }

        [HttpPost]
        public JsonResult SetDefaultorderAddress(long id)
        {
            long num = base.CurrentUser.Id;
            ServiceHelper.Create<IorderAddressService>().SetDefaultorderAddress(id, num);
            Result result = new Result()
            {
                success = true,
                msg = "设置成功"
            };
            return Json(result);
        }

        [HttpPost]
        public JsonResult SetQuickorderAddress(long id)
        {
            long num = base.CurrentUser.Id;
            ServiceHelper.Create<IorderAddressService>().SetQuickorderAddress(id, num);
            Result result = new Result()
            {
                success = true,
                msg = "设置成功"
            };
            return Json(result);
        }
    }
}