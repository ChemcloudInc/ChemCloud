using ChemCloud.Core;
using ChemCloud.DBUtility;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class MoneyController : BaseMemberController
    {
        // GET: Web/Money
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult MoneyList()
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

        public JsonResult GetUserName(long uid) {
            string strsql = string.Format("select * from ChemCloud_Members where id='{0}'", uid);
            DataSet ds = DbHelperSQL.Query(strsql.ToString());
            if (ds == null)
            {
                return Json("");
            }
            else
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    return Json(ds.Tables[0].Rows[0]["UserName"].ToString());
                }
                else
                {
                    return Json("");
                }
            }
        }
    }
}
