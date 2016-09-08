using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Service;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class CertificationController : BaseSellerController
    {
        // GET: SellerAdmin/Certification
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EditCertification(FieldCertification certification)
        {
            FieldCertification fieldfertification = new FieldCertification()
            {
                Id = base.CurrentSellerManager.CertificationId,
                EnterpriseHonor = certification.EnterpriseHonor,
                ProductDetails = certification.ProductDetails,
                ApplicationDate = DateTime.Now,
                Status = FieldCertification.CertificationStatus.Submit
            };
            ServiceHelper.Create<ICertification>().UpdateCertification(fieldfertification);
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult EditCertificationPay(string telegraphicMoneyOrder)
        {
            FieldCertification fieldfertification = new FieldCertification()
            {
                Id = base.CurrentSellerManager.CertificationId,
                TelegraphicMoneyOrder = telegraphicMoneyOrder,
                Status = FieldCertification.CertificationStatus.PayandWaitAudit,
                FeedbackDate = DateTime.Now
            };
            ServiceHelper.Create<ICertification>().UpdateCertification(fieldfertification);
            return Json(new { success = true });
        }

        public ActionResult Management()
        {
            FieldCertification.CertificationStatus fcstatus = FieldCertification.CertificationStatus.Unusable;
            FieldCertification fcf = ServiceHelper.Create<ICertification>().GetCertification(base.CurrentSellerManager.CertificationId);
            if (fcf != null)
            {
                fcstatus = fcf.Status;
            }
            else
            {
                fcf = new FieldCertification();
            }
 
            ViewBag.Status = fcstatus;

            /*供应商信息*/
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
            ViewBag.ShopStatus = shop.ShopStatus;
            ViewBag.ShopId = shop.Id;

            /*自定义支付单号*/
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            ViewBag.OrderId = _orderBO.GenerateOrderNumber();

            return View(fcf);
        }
    }
}
