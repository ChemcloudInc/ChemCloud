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
using ChemCloud.Model.Common;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class LimitedApplyController : BaseWebController
    {
        public ActionResult Management()
        {
            return View();
        }

        /// <summary>
        /// 限额申请列表
        /// </summary>
        /// <param name="Status">申请状态</param>
        /// <param name="Start_datetime">开始时间</param>
        /// <param name="End_datetime">结束时间</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="Applicant">申请人（0：子账户申请；1：我的申请）</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(int? Status, DateTime? Start_datetime, DateTime? End_datetime, int page, int rows, int Applicant)
        {
            PageModel<ApplyAmountInfo> ApplyAmount = ServiceHelper.Create<IApplyAmountService>().GetApplyAmounts(Status, Start_datetime, End_datetime, page, rows, base.CurrentUser.Id, Applicant);
            IEnumerable<ApplyAmountInfo> array =
                from item in ApplyAmount.Models.ToArray()
                select new ApplyAmountInfo()
                {
                    ApplyUserId = item.ApplyUserId,
                    ApplyAmount = item.ApplyAmount,
                    ApplyDate = item.ApplyDate,
                    ApplyStatus = item.ApplyStatus,
                    ApplyName = item.ApplyName,
                    OrderId = item.OrderId,
                    Id = item.Id,
                    AuthDate = item.AuthDate,
                    AuthorId = item.AuthorId,
                    AuthorName = item.AuthorName,
                    CoinType = item.CoinType,
                    CoinName = item.CoinName,
                    Applicant = Applicant
                };
            DataGridModel<ApplyAmountInfo> dataGridModel = new DataGridModel<ApplyAmountInfo>()
            {
                rows = array,
                total = ApplyAmount.Total
            };
            return Json(dataGridModel);
        }
        public ActionResult Detail(long Id)
        {
            ApplyAmountInfo model = ServiceHelper.Create<IApplyAmountService>().GetApplyById(Id);
            return View(model);
        }

        public ActionResult Auth(long Id)
        {
            ApplyAmountInfo model = ServiceHelper.Create<IApplyAmountService>().GetApplyById(Id);
            OrderInfo orderModel = ServiceHelper.Create<IOrderService>().GetOrder(model.OrderId);
            Organization orgInfo = ServiceHelper.Create<IOrganizationService>().GetOrganizationByUserId(base.CurrentUser.Id);
            if (orgInfo.RoleName != "管理员" && orgInfo.RoleName != "Admin")
            {
                LimitedAmount limited = ServiceHelper.Create<IOrganizationService>().GetlimitedByRoleId(orgInfo.RoleId, model.CoinType);
                ViewBag.Money = limited.Money;
            }
            ViewBag.AuthorId = base.CurrentUser.Id;
            ViewBag.Id = Id;
            ViewBag.RoleName = orgInfo.RoleName;
            ViewBag.orderId = orderModel.Id;
            return View(model);
        }
        [HttpPost]

        public JsonResult Delete(long Id)
        {
            bool flag = ServiceHelper.Create<IApplyAmountService>().Delete(Id);
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
            for (int i = 0; i < strArrays.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays[i]));
            }
            bool flag = ServiceHelper.Create<IApplyAmountService>().BatchDelete(nums.ToArray());
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult AddApplyAmount(long applyUserId, decimal orderAmount, long orderId, int coinType)
        {
            ApplyAmountInfo model = new ApplyAmountInfo()
            {
                ApplyUserId = applyUserId,
                AuthorId = base.CurrentUser.ParentSellerId,
                ApplyAmount = orderAmount,
                ApplyDate = DateTime.Now,
                CoinType = coinType,
                OrderId = orderId
            };
            bool flag = ServiceHelper.Create<IApplyAmountService>().AddApplyAmount(model);
            ServiceHelper.Create<ISiteMessagesService>().SendLimitedAmountMessage(base.CurrentUser.Id);
            if (ServiceHelper.Create<IMemberService>().GetMemberByName(base.CurrentUser.UserName) != null)
            {
                string email = ServiceHelper.Create<IMemberService>().GetMemberByName(base.CurrentUser.UserName).Email;
                string mailsubject = "您的限额申请审核已通过";
                string mailcontent = "";
                long LanguageType = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
                MessageSetting models = ServiceHelper.Create<IMessageSettingService>().GetSettingByLanguageType(ChemCloud.Model.MessageSetting.MessageModuleStatus.LimitedAount, LanguageType);
                if (models != null)
                {
                    mailcontent = models.MessageContent == null ? mailcontent : models.MessageContent;
                }
                string str = mailcontent.Replace("#userName", base.CurrentUser.UserName);
                ChemCloud.Service.SendMail.SendEmail(email, mailsubject, str);
            }
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UpdateAuthor(long Id)
        {
            bool flag = ServiceHelper.Create<IApplyAmountService>().UpdateAuthor(Id, base.CurrentUser.ParentSellerId);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UpdateApplyStatus(long Id, int status, long AuthorId)
        {
            bool flag = ServiceHelper.Create<IApplyAmountService>().UpdateApplyStatus(Id, status, base.CurrentUser.Id);
            ApplyAmountInfo ApplyAmount = ServiceHelper.Create<IApplyAmountService>().GetApplyById(Id);
            if (flag)
            {
                if (status == 1)
                    ServiceHelper.Create<ISiteMessagesService>().SendApplyPassMessage(ApplyAmount == null ? 0 : ApplyAmount.ApplyUserId);
                else if (status == 2)
                    ServiceHelper.Create<ISiteMessagesService>().SendApplyNoPassMessage(ApplyAmount == null ? 0 : ApplyAmount.ApplyUserId);
                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult IsLimitedAmountOver(long Id, int Cointype)
        {
            ApplyAmountInfo ApplyAmount = ServiceHelper.Create<IApplyAmountService>().GetApplyById(Id);
            Organization orgInfo = ServiceHelper.Create<IOrganizationService>().GetOrganizationByUserId(base.CurrentUser.Id);
            LimitedAmount limited = ServiceHelper.Create<IOrganizationService>().GetlimitedByRoleId(orgInfo.RoleId, Cointype);
            if (ApplyAmount.ApplyAmount > limited.Money)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult IsPassAuth(long UserId, long OrderId)
        {
            bool flag = ServiceHelper.Create<IApplyAmountService>().IsPassAuth(UserId, OrderId);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult SendMessage()
        {
            bool flag = ServiceHelper.Create<ISiteMessagesService>().SendLimitedAmountMessage(base.CurrentUser.Id);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }


        #region 限额申请 An

        #region 获取制定ID的限额申请
        /// <summary>
        /// 获取制定ID的限额申请
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [WebMethod]
        public string Detail_Apply(QueryCommon<ApplyAmountInfo> query)
        {
            Result_Model<ApplyAmountInfo> res = new Result_Model<ApplyAmountInfo>()
            {
                Model = new ApplyAmountInfo(),
                Msg = new Result_Msg()
            };
            res.Model = ServiceHelper.Create<IApplyAmountService>().GetApplyById(query.ParamInfo.Id);
            if (res.Model != null)
            {
                switch (res.Model.ApplyStatus)
                {
                    case 0:
                        res.Msg = new Result_Msg() { IsSuccess = true };
                        break;
                    case 1:
                        res.Msg = new Result_Msg() { IsSuccess = false, Message="审核已通过，不能修改"  };
                        break;
                    case 2:
                        res.Msg = new Result_Msg() { IsSuccess = true, };
                        break;
                    default:
                        break;
                }
            }
            else
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "未找到记录" };
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 修改我的限额申请

        /// <summary>
        /// 修改我的限额申请
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string Update_Apply(QueryCommon<ApplyAmountInfo> query)
        {
            query.ParamInfo.ApplyStatus = 0;
            query.ParamInfo.ApplyDate = DateTime.Now;
            Result_Msg res = ServiceHelper.Create<IApplyAmountService>().Update_Apply(query);

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #endregion
    }

}