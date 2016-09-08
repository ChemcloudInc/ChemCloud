using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class FinanceController : BaseSellerController
    {
        // GET: SellerAdmin/Finance
        public ActionResult Index()
        {
            ViewBag.LeftMoney = "0.00";
            ViewBag.LockMoney = "0.00";
            ViewBag.ShowIDLock = "有效";
            ViewBag.DoTime = "";
            ViewBag.DoIP = "";
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
            Finance_Wallet fw = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
            if (fw != null)
            {
                ViewBag.LeftMoney = fw.Wallet_UserLeftMoney.ToString("F2");
                ViewBag.LockMoney = fw.Wallet_UserMoneyLock.ToString("F2");
                ViewBag.DoTime = fw.Wallet_DoLastTime;
                ViewBag.DoIP = fw.Wallet_DoIpAddress;
                if (fw.Wallet_Status == 1)
                {
                    ViewBag.ShowIDLock = "有效";
                }
                else
                {
                    ViewBag.ShowIDLock = "锁定";
                }
            }
            ViewBag.userid = base.CurrentUser.Id;
            ViewBag.usertype = base.CurrentUser.UserType;
            return View();
        }
    }
}