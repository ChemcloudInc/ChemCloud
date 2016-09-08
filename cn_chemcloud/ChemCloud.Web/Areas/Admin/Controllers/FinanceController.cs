using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class FinanceController : BaseAdminController
    {
        // GET: Admin/Finance
        public ActionResult Index()
        {
            ViewBag.LeftMoney = "0.00";
            ViewBag.LockMoney = "0.00";

            ViewBag.userid = "0";
            ViewBag.usertype = "0";
 


            string money_type = ConfigurationManager.AppSettings["CoinType"].ToString();
            if (money_type == "1")
            {
                ViewBag.M_type = "CNY";
            }
            else if (money_type == "2")
            {
                ViewBag.M_type = "USD";
            }
            else
            {
                ViewBag.M_type = "";
            }
            decimal LeftMoney = 0;
            decimal LockMoney = 0;

            List<Finance_Wallet> listwallet = ServiceHelper.Create<IFinance_WalletService>().GetWalletList(int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));

            if (listwallet != null)
            {
                foreach (Finance_Wallet item in listwallet)
                {
                    LeftMoney = LeftMoney + item.Wallet_UserLeftMoney;
                    LockMoney = LockMoney + item.Wallet_UserMoneyLock;
                }
                ViewBag.LeftMoney = LeftMoney;
                ViewBag.LockMoney = LockMoney;
            }

            return View();
        }

        public ActionResult AuditFinance_WithDrawList()
        {
            return View();
        }

        public ActionResult AuditFinance_WithDrawList_Detail(long id)
        {
            Finance_WithDraw model = ServiceHelper.Create<IFinance_WithDrawService>().GetFinance_WithDrawInfo(id);

            model.Withdraw_UserName = ServiceHelper.Create<IMemberService>().GetMember(model.Withdraw_UserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(model.Withdraw_UserId).UserName;
            return View(model);
        }

        [HttpPost]
        public JsonResult SubmitWithdraw(string Id, string Withdraw_shenhe, string Withdraw_shenheDesc)
        {
            Finance_WithDraw model = ServiceHelper.Create<IFinance_WithDrawService>().GetFinance_WithDrawInfo(long.Parse(Id));
            model.Withdraw_shenhe = int.Parse(Withdraw_shenhe);
            model.Withdraw_shenheDesc = Withdraw_shenheDesc;
            if (Withdraw_shenhe == "1")
            {
                model.Withdraw_Status = 1;
            }
            model.Withdraw_shenheTime = DateTime.Now;
            model.Withdraw_shenheUid = CurrentManager.Id;
            model.Withdraw_shenheUname = CurrentManager.UserName;

            bool result = ServiceHelper.Create<IFinance_WithDrawService>().UpdateFinance_WithDraw(model);

            if (result)
            {
                Finance_Wallet _Finance_Wallet = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(model.Withdraw_UserId, model.Withdraw_UserType, model.Withdraw_MoneyType);
                if (Withdraw_shenhe == "1")
                {
                    //提现通过
                    _Finance_Wallet.Wallet_UserMoneyLock = _Finance_Wallet.Wallet_UserMoneyLock - model.Withdraw_Money;
                }
                else if (Withdraw_shenhe == "2")
                {
                    //拒绝提现
                    _Finance_Wallet.Wallet_UserMoneyLock = _Finance_Wallet.Wallet_UserMoneyLock - model.Withdraw_Money;
                    _Finance_Wallet.Wallet_UserLeftMoney = _Finance_Wallet.Wallet_UserLeftMoney + model.Withdraw_Money;
                }
                ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(_Finance_Wallet);
            }
            return Json(new { success = result });
        }
    }
}