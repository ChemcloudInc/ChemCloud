using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
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
    public class FieldCertificationController : BaseAdminController
    {
        public FieldCertificationController()
        {
        }
        [Description("实地认证审核页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult Auditing(long id)
        {
            FieldCertificationQuery FieldCertificationQuery = new FieldCertificationQuery();
            FieldCertification Certification = ServiceHelper.Create<ICertification>().GetCertification(id);
            FieldCertificationModel FieldCertificationModel = new FieldCertificationModel(Certification);
            return View(FieldCertificationModel);
        }
        [Description("实地认证审核页面(POST)")]
        [HttpPost]
        [UnAuthorize]
        public JsonResult Auditing(long Id, int status, string comment = "")
        {
            //更新实地认证状态
            ServiceHelper.Create<ICertification>().UpdateStatus(Id, (FieldCertification.CertificationStatus)status, DateTime.Now, comment);

            IOperationLogService operationLogService = ServiceHelper.Create<IOperationLogService>();
            LogInfo logInfo = new LogInfo()
            {
                Date = DateTime.Now,
                Description = string.Format("实地认证审核页面，供应商Id={0},状态为：{1}, 说明是：{2}", Id, (FieldCertification.CertificationStatus)status, comment),
                IPAddress = base.Request.UserHostAddress,
                PageUrl = string.Concat("/FieldCertification/Auditing/", Id),
                UserName = base.CurrentManager.UserName,
                Id = 0
            };
            operationLogService.AddPlatformOperationLog(logInfo);
            ManagerInfo Info = ServiceHelper.Create<ICertification>().GetManager(Id);
            if (Info != null)
            {
                UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(Info.UserName);
                //ServiceHelper.Create<ISiteMessagesService>().SendCertificationResultMessage(userInfo.Id, base.CurrentManager.UserName);
            }
            //更新供应商认证状态
            ServiceHelper.Create<IShopService>().UpdateShopGrade(Info.ShopId, (FieldCertification.CertificationStatus)status);
            return Json(new { Successful = true });
        }
        [Description("实地认证接受页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult Receing(long id)
        {
            FieldCertificationQuery FieldCertificationQuery = new FieldCertificationQuery();
            FieldCertification Certification = ServiceHelper.Create<ICertification>().GetCertification(id);
            FieldCertificationModel FieldCertificationModel = new FieldCertificationModel(Certification);
            ViewBag.Id = id;
            ViewBag.Status = Certification.Status;
            return View(FieldCertificationModel);
        }
        [Description("实地认证接收页面(POST)")]
        [HttpPost]
        [UnAuthorize]
        public JsonResult Receing(long Id, int status, string comment = "")
        {
            /*状态*/
            ServiceHelper.Create<ICertification>().UpdateStatus(Id, (FieldCertification.CertificationStatus)status, DateTime.Now, comment);

            /*日志*/
            IOperationLogService operationLogService = ServiceHelper.Create<IOperationLogService>();
            LogInfo logInfo = new LogInfo()
            {
                Date = DateTime.Now,
                Description = string.Format("实地认证接收页面，供应商Id={0},状态为：{1}, 说明是：{2}", Id, (FieldCertification.CertificationStatus)status, comment),
                IPAddress = base.Request.UserHostAddress,
                PageUrl = string.Concat("/FieldCertification/Receing/", Id),
                UserName = base.CurrentManager.UserName,
                Id = 0
            };
            operationLogService.AddPlatformOperationLog(logInfo);

            return Json(new { Successful = true });
        }
        [Description("实地认证付款页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult Paying(long id)
        {
            FieldCertificationQuery FieldCertificationQuery = new FieldCertificationQuery();
            FieldCertification Certification = ServiceHelper.Create<ICertification>().GetCertification(id);
            FieldCertificationModel FieldCertificationModel = new FieldCertificationModel(Certification);
            ViewBag.Id = id;
            ViewBag.Status = Certification.Status;
            return View(FieldCertificationModel);
        }
        [Description("实地认证接收页面(POST)")]
        [HttpPost]
        [UnAuthorize]
        public JsonResult Paying(long Id, int status, string comment = "")
        {
            ServiceHelper.Create<ICertification>().UpdateStatus(Id, (FieldCertification.CertificationStatus)status, DateTime.Now, comment);
            IOperationLogService operationLogService = ServiceHelper.Create<IOperationLogService>();
            LogInfo logInfo = new LogInfo()
            {
                Date = DateTime.Now,
                Description = string.Format("实地认证付款页面，供应商Id={0},状态为：{1}, 说明是：{2}", Id, (FieldCertification.CertificationStatus)status, comment),
                IPAddress = base.Request.UserHostAddress,
                PageUrl = string.Concat("/FieldCertification/Paying/", Id),
                UserName = base.CurrentManager.UserName,
                Id = 0
            };
            operationLogService.AddPlatformOperationLog(logInfo);
            ManagerInfo Info = ServiceHelper.Create<ICertification>().GetManager(Id);
            if (Info != null)
            {
                UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(Info.UserName);
                ServiceHelper.Create<ISiteMessagesService>().SendConfirmPayMessage(userInfo.Id);
            }
            return Json(new { Successful = true });
        }
        [HttpPost]
        [UnAuthorize]
        public JsonResult List(long? Id, int? Status, int page, int rows, string companyName, string type = "")
        {
            FieldCertification.CertificationStatus? nullable;
            FieldCertificationQuery FieldCertificationQuery = new FieldCertificationQuery();
            FieldCertificationQuery FieldCertificationQuery1 = FieldCertificationQuery;
            //SelectList selectList = FieldCertification.CertificationStatus.Open.ToSelectList<FieldCertification.CertificationStatus>(true, false);
            //待审核 5
            if (type.Equals("Auditing"))
            {
                Status = 5;
            }
            else
            {
                //已提交 2
                if (type.Equals("Receing"))
                {
                    Status = 2;
                }
                //待付款 4
                if (type.Equals("Paying"))
                {
                    Status = 4;
                }
            }
            if (Status == 0)
                Status = null;
            int? nullable1 = Status;
            if (nullable1.HasValue)
            {
                nullable = new FieldCertification.CertificationStatus?((FieldCertification.CertificationStatus)nullable1.GetValueOrDefault());
            }
            else
            {
                nullable = null;
            }
            FieldCertificationQuery1.Status = nullable;
            FieldCertificationQuery1.PageSize = rows;
            FieldCertificationQuery.PageNo = page;
            FieldCertificationQuery.CompanyName = companyName;
            FieldCertificationQuery.Id = Id;
            PageModel<FieldCertification> CertificationShops = ServiceHelper.Create<ICertification>().GetCertifications(FieldCertificationQuery);
            ICertification shopService = ServiceHelper.Create<ICertification>();
            IEnumerable<FieldCertificationModel> array =
                from item in CertificationShops.Models.ToArray()
                select new FieldCertificationModel()
                {
                    Id = item.Id,
                    Status = item.Status.ToDescription(),
                    TelegraphicMoneyOrder = item.TelegraphicMoneyOrder,
                    ApplicationDate = item.ApplicationDate,
                    ToAcceptTheDate = item.ToAcceptTheDate,
                    CertificationDate = item.CertificationDate,
                    FeedbackDate = item.FeedbackDate,
                    Certificationcost = item.Certificationcost,
                    AnnualSales = item.AnnualSales,
                    Certification = item.Certification,
                    EnterpriseHonor = item.EnterpriseHonor,
                    ProductDetails = item.ProductDetails,
                    RefuseReason = item.RefuseReason,
                    CompanyName = item.CompanyName,
                    LegalPerson = item.LegalPerson,
                    CompanyAddress = item.CompanyAddress,
                    CompanyRegisteredCapital = item.CompanyRegisteredCapital,
                    //EndDate = (type == "Auditing" ? "--" : (item.EndDate.HasValue ? item.EndDate.Value.ToString("yyyy-MM-dd") : "")),
                };
            DataGridModel<FieldCertificationModel> dataGridModel = new DataGridModel<FieldCertificationModel>()
            {
                rows = array,
                total = CertificationShops.Total
            };
            return Json(dataGridModel);
        }
        public ActionResult Management(string type = "")
        {
            /*
            [Description("不可用")]
            Unusable = 1,
            [Description("已提交")]
            Submit = 2,
            [Description("已接收")]
            Receive = 3,
            [Description("已付款待审核")]
            PayandWaitAudit = 4,
            [Description("审核通过")]
            Open = 5,
            [Description("已拒绝")]
            Refuse = 6
             */

            IEnumerable<SelectListItem> selectListItems = null;
            SelectList selectList = FieldCertification.CertificationStatus.Open.ToSelectList<FieldCertification.CertificationStatus>(true, false);
            dynamic viewBag = base.ViewBag;

            if (type == "Receing")
            {
                selectListItems = (
                from p in selectList
                where (int.Parse(p.Value) == 3)
                select p);
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                selectListItems = (
                from p in selectList
                where (int.Parse(p.Value) != 1)
                select p);
            }
            ViewBag.Status = selectListItems;
            List<SelectListItem> selectListItems1 = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Selected = true,
                Value = 0.ToString(),
                Text = "请选择..."
            };
            selectListItems1.Add(selectListItem);
            ViewBag.Type = type;
            return View();
        }
        public ActionResult Details(long id)
        {
            FieldCertificationQuery FieldCertificationQuery = new FieldCertificationQuery();
            FieldCertification Certification = ServiceHelper.Create<ICertification>().GetCertification(id);
            FieldCertificationModel FieldCertificationModel = new FieldCertificationModel(Certification);
            ViewBag.PassStr = Certification.Status.ToDescription();
            ViewBag.Status = Certification.Status.ToDescription();
            return View(FieldCertificationModel);
        }
    }
}