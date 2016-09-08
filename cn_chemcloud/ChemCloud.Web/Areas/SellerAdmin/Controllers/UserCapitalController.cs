using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.DBUtility;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class UserCapitalController : BaseSellerController
    {
        public ActionResult ApplyWithDraw()
        {
            Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
            if (fwinfo == null)
            {
                return RedirectToAction("Index", "Finance");
            }
            ViewBag.ApplyWithMoney = fwinfo.Wallet_UserLeftMoney;
            ViewBag.IsSetPwd = (string.IsNullOrWhiteSpace(fwinfo.Wallet_PayPassword) ? false : true);
            return View();
        }

        /// <summary>
        /// 获取/设置 当前用户的提现信息
        /// </summary>
        /// <param name="bankName">提现银行名称</param>
        /// <param name="bankuserName">提现法人</param>
        /// <param name="bankId">提现银行账户</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPayBankInfo(string bankName, string bankuserName, string bankId)
        {
            Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
            if (fwinfo == null)
            {
                Log.Error("用户：" + base.CurrentUser.Id + "在" + DateTime.Now + "体现时获取自己的提现信息失败.");
                return Json("");
            }
            else
            {
                fwinfo.Wallet_UserBankName = bankName;
                fwinfo.Wallet_UserBankUserName = bankuserName;
                fwinfo.Wallet_UserBankNumber = bankId;
                fwinfo.Wallet_DoIpAddress = ChemCloud.Core.Common.GetIpAddress();
                fwinfo.Wallet_DoLastTime = DateTime.Now;
                fwinfo.Wallet_DoUserId = base.CurrentUser.Id;
                fwinfo.Wallet_DoUserName = base.CurrentUser.UserName;
                if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo))
                {
                    Log.Info("用户：" + base.CurrentUser.Id + "在" + DateTime.Now + "体现时设置提现信息成功.");
                    return Json("ok");
                }
                else
                {
                    Log.Error("用户：" + base.CurrentUser.Id + "在" + DateTime.Now + "体现时设置提现信息失败.");
                    return Json("");
                }
            }
        }
        public JsonResult ApplyWithDrawList(int page, int rows)
        {
            IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
            ApplyWithDrawQuery applyWithDrawQuery = new ApplyWithDrawQuery()
            {
                memberId = new long?(base.CurrentUser.Id),
                PageSize = rows,
                PageNo = page,
                Sort = "ApplyTime"
            };
            PageModel<ApplyWithDrawInfo> applyWithDraw = memberCapitalService.GetApplyWithDraw(applyWithDrawQuery);
            IEnumerable<ApplyWithDrawModel> applyWithDrawModels = applyWithDraw.Models.ToList().Select<ApplyWithDrawInfo, ApplyWithDrawModel>((ApplyWithDrawInfo e) =>
            {
                string empty = string.Empty;
                if (e.ApplyStatus == ApplyWithDrawInfo.ApplyWithDrawStatus.PayFail || e.ApplyStatus == ApplyWithDrawInfo.ApplyWithDrawStatus.WaitConfirm)
                {
                    empty = "提现中";
                }
                else if (e.ApplyStatus == ApplyWithDrawInfo.ApplyWithDrawStatus.Refuse)
                {
                    empty = "提现失败";
                }
                else if (e.ApplyStatus == ApplyWithDrawInfo.ApplyWithDrawStatus.WithDrawSuccess)
                {
                    empty = "提现成功";
                }
                return new ApplyWithDrawModel()
                {
                    Id = e.Id,
                    ApplyAmount = e.ApplyAmount,
                    ApplyStatus = e.ApplyStatus,
                    ApplyStatusDesc = empty,
                    ApplyTime = e.ApplyTime.ToString()
                };
            });
            DataGridModel<ApplyWithDrawModel> dataGridModel = new DataGridModel<ApplyWithDrawModel>()
            {
                rows = applyWithDrawModels,
                total = applyWithDraw.Total
            };
            return Json(dataGridModel);
        }
        /// <summary>
        /// 提交提现操作
        /// </summary>
        /// <param name="amount">提现金额</param>
        /// <param name="pwd">支付密码</param>
        /// <returns></returns>
        public JsonResult ApplyWithDrawSubmit(decimal amount, string pwd)
        {
            Finance_Wallet fwino = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
            if (fwino == null)
            {
                Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "时提现 获取提现信息失败.");
                return Json("");
            }
            else
            {
                if (!pwd.Equals(fwino.Wallet_PayPassword))
                {
                    throw new HimallException("支付密码不对，请重新输入！");
                }
                else
                {
                    if (amount > fwino.Wallet_UserLeftMoney)
                    {
                        throw new HimallException("提现金额不能超出可用金额！");
                    }
                    else
                    {
                        //创建提现记录 并且更新用户提现后的钱包
                        Finance_WithDraw fwdinfo = new Finance_WithDraw();
                        ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                        fwdinfo.Withdraw_Number = _orderBO.GenerateOrderNumber();
                        fwdinfo.Withdraw_UserId = base.CurrentUser.Id;
                        fwdinfo.Withdraw_UserType = base.CurrentUser.UserType;
                        fwdinfo.Withdraw_Money = amount;
                        fwdinfo.Withdraw_MoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"]);
                        fwdinfo.Withdraw_BankName = fwino.Wallet_UserBankName;
                        fwdinfo.Withdraw_BankUserName = fwino.Wallet_UserBankUserName;
                        fwdinfo.Withdraw_Account = fwino.Wallet_UserBankNumber;
                        fwdinfo.Withdraw_Time = DateTime.Now;
                        fwdinfo.Withdraw_Status = 0;
                        fwdinfo.Withdraw_shenhe = 0;
                        fwdinfo.Withdraw_shenheDesc = "";
                        fwdinfo.Withdraw_shenheTime = DateTime.Now;
                        fwdinfo.Withdraw_shenheUid = 0;
                        fwdinfo.Withdraw_shenheUname = "";
                        fwino.Wallet_UserMoneyLock = amount;
                        fwino.Wallet_UserLeftMoney = fwino.Wallet_UserLeftMoney - amount;
                        fwino.Wallet_DoIpAddress = ChemCloud.Core.Common.GetIpAddress();
                        fwino.Wallet_DoLastTime = DateTime.Now;
                        fwino.Wallet_DoUserId = base.CurrentUser.Id;
                        fwino.Wallet_DoUserName = base.CurrentUser.UserName;
                        if (ServiceHelper.Create<IFinance_WithDrawService>().AddFinance_WithDraw(fwdinfo) && ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwino))
                        {
                            return Json("ok");
                        }
                        else
                        {
                            Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "时添加提现记录失败.");
                            return Json("");
                        }

                    }
                }

            }
        }

        public ActionResult CapitalCharge()
        {
            string value = "";
            Finance_Wallet fw = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
            string m_type = ConfigurationManager.AppSettings["CoinType"].ToString();
            if (m_type == "1")
            {
                ViewBag.MoneyType = "CNY";
            }
            else if (m_type == "2")
            {
                ViewBag.MoneyType = "USD";
            }
            else
            {
                ViewBag.MoneyType = "";
            }
            if (fw != null)
            {
                value = fw.Wallet_UserLeftMoney.ToString("F2");
            }
            ViewBag.Balance = value;
            return View();
            //IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
            //CapitalInfo capitalInfo = memberCapitalService.GetCapitalInfo(base.CurrentUser.Id);
            //return View(capitalInfo);
        }

        public JsonResult ChargeList(int page, int rows)
        {
            IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
            ChargeQuery chargeQuery = new ChargeQuery()
            {
                memberId = new long?(base.CurrentUser.Id),
                PageSize = rows,
                PageNo = page
            };
            PageModel<ChargeDetailInfo> chargeLists = memberCapitalService.GetChargeLists(chargeQuery);
            IEnumerable<ChargeDetailModel> list =
                from e in chargeLists.Models.ToList()
                select new ChargeDetailModel()
                {
                    Id = e.Id.ToString(),
                    ChargeAmount = e.ChargeAmount,
                    ChargeStatus = e.ChargeStatus,
                    ChargeStatusDesc = e.ChargeStatus.ToDescription(),
                    ChargeTime = e.ChargeTime.ToString(),
                    CreateTime = e.CreateTime.ToString(),
                    ChargeWay = e.ChargeWay,
                    MemId = e.MemId
                };
            DataGridModel<ChargeDetailModel> dataGridModel = new DataGridModel<ChargeDetailModel>()
            {
                rows = list,
                total = chargeLists.Total
            };
            return Json(dataGridModel);
        }
        /// <summary>
        /// 充值提交
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public JsonResult ChargeSubmit(decimal amount)
        {
            Finance_Wallet Fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
            if (Fwinfo != null)
            {

            }
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            long orderid = _orderBO.GenerateOrderNumber();
            Finance_Recharge frinfo = new Finance_Recharge();
            frinfo.Recharge_Number = orderid;
            frinfo.Recharge_UserId = base.CurrentUser.Id;
            frinfo.Recharge_UserType = base.CurrentUser.UserType;
            frinfo.Recharge_Time = DateTime.Now;
            frinfo.Recharge_Address = ChemCloud.Core.Common.GetIpAddress();
            frinfo.Recharge_Money = amount;
            frinfo.Recharge_MoneyLeft = Fwinfo.Wallet_UserLeftMoney;
            frinfo.Recharge_MoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            frinfo.Recharge_Type = 0;
            frinfo.Recharge_Status = 1;
            if (ServiceHelper.Create<IFinance_RechargeService>().AddFinance_Recharge(frinfo))
            {
                return Json(frinfo.Recharge_Number);
            }
            else
            {
                return Json("");
            }
        }

        private string DecodePaymentId(string paymentId)
        {
            return paymentId.Replace("-", ".");
        }

        private string EncodePaymentId(string paymentId)
        {
            return paymentId.Replace(".", "-");
        }

        public ActionResult Index()
        {
            IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
            CapitalInfo capitalInfo = memberCapitalService.GetCapitalInfo(base.CurrentUser.Id);
            return View(capitalInfo);
        }

        public JsonResult List(CapitalDetailInfo.CapitalDetailType capitalType, int page, int rows)
        {
            IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
            CapitalDetailQuery capitalDetailQuery = new CapitalDetailQuery()
            {
                memberId = base.CurrentUser.Id,
                capitalType = new CapitalDetailInfo.CapitalDetailType?(capitalType),
                PageSize = rows,
                PageNo = page
            };
            PageModel<CapitalDetailInfo> capitalDetails = memberCapitalService.GetCapitalDetails(capitalDetailQuery);
            List<CapitalDetailModel> list = (
                from e in capitalDetails.Models.ToList()
                select new CapitalDetailModel()
                {
                    Id = e.Id,
                    Amount = e.Amount,
                    CapitalID = e.CapitalID,
                    CreateTime = e.CreateTime.Value.ToString(),
                    SourceData = e.SourceData,
                    SourceType = e.SourceType,
                    Remark = string.Concat(e.SourceType.ToDescription(), ",单号：", e.Id),
                    PayWay = e.Remark
                }).ToList();
            DataGridModel<CapitalDetailModel> dataGridModel = new DataGridModel<CapitalDetailModel>()
            {
                rows = list,
                total = capitalDetails.Total
            };
            return Json(dataGridModel);
        }

        public JsonResult PaymentList(decimal balance)
        {
            string str3;
            string scheme = base.Request.Url.Scheme;
            string host = base.HttpContext.Request.Url.Host;
            if (base.HttpContext.Request.Url.Port == 80)
            {
                str3 = "";
            }
            else
            {
                int port = base.HttpContext.Request.Url.Port;
                str3 = string.Concat(":", port.ToString());
            }
            string str4 = string.Concat(scheme, "://", host, str3);
            string str5 = string.Concat(str4, "/pay/CapitalChargeReturn/{0}");
            string str6 = string.Concat(str4, "/pay/CapitalChargeNotify/{0}");
            IEnumerable<Plugin<IPaymentPlugin>> plugins =
                from item in PluginsManagement.GetPlugins<IPaymentPlugin>(true)
                where item.Biz.SupportPlatforms.Contains<PlatformType>(PlatformType.PC)
                select item;
            long num = ServiceHelper.Create<IMemberCapitalService>().CreateCode(CapitalDetailInfo.CapitalDetailType.ChargeAmount);
            string str7 = num.ToString();
            IEnumerable<PaymentModel> paymentModels = plugins.Select<Plugin<IPaymentPlugin>, PaymentModel>((Plugin<IPaymentPlugin> item) =>
            {
                string empty = string.Empty;
                try
                {
                    IPaymentPlugin biz = item.Biz;
                    string str = str5;
                    string[] strArrays = new string[] { EncodePaymentId(item.PluginInfo.PluginId), "-", balance.ToString(), "-", null };
                    strArrays[4] = CurrentUser.Id.ToString();
                    string str1 = string.Format(str, string.Concat(strArrays));
                    string str2 = str6;
                    string[] strArrays1 = new string[] { EncodePaymentId(item.PluginInfo.PluginId), "-", balance.ToString(), "-", null };
                    strArrays1[4] = CurrentUser.Id.ToString();
                    empty = biz.GetRequestUrl(str1, string.Format(str2, string.Concat(strArrays1)), str7, balance, "预付款充值", null);
                }
                catch (Exception exception)
                {
                    Log.Error("支付页面加载支付插件出错", exception);
                }
                return new PaymentModel()
                {
                    Logo = string.Concat("/Plugins/Payment/", item.PluginInfo.ClassFullName.Split(new char[] { ',' })[1], "/", item.Biz.Logo),
                    RequestUrl = empty,
                    UrlType = item.Biz.RequestUrlType,
                    Id = item.PluginInfo.PluginId
                };
            });
            paymentModels =
                from item in paymentModels
                where !string.IsNullOrEmpty(item.RequestUrl)
                select item;
            return Json(paymentModels);
        }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="pwd">支付密码</param>
        /// <returns></returns>
        public JsonResult SavePayPwd(string pwd)
        {
            Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
            if (fwinfo == null)
            {
                Log.Error("在用户：" + base.CurrentUser.Id + "提现时,获取用户支付密码信息失败.");
                return Json("");
            }
            else
            {
                string str = Guid.NewGuid().ToString("N");
                fwinfo.Wallet_PayPassword = pwd;//当前未加密 SecureHelper.MD5(string.Concat(SecureHelper.MD5(pwd), str));
                fwinfo.Wallet_DoUserId = base.CurrentUser.Id;
                fwinfo.Wallet_DoLastTime = DateTime.Now;
                fwinfo.Wallet_DoUserName = base.CurrentUser.UserName;
                fwinfo.Wallet_DoIpAddress = ChemCloud.Core.Common.GetIpAddress();
                if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo))
                {
                    return Json(new { success = true });
                }
                else
                {
                    Log.Error("在用户：" + base.CurrentUser.Id + "提现时,设置用户支付密码失败.");
                    return Json("");
                }
            }
        }

        public ActionResult SetPayPwd()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPayPalId(string pp)
        {
            string strReutrn = "";
            ICapitalUserAccountService ICMAS = ServiceHelper.Create<ICapitalUserAccountService>();
            CapitalUserAccount CMA = ICMAS.GetICapitalUserAccountInfo(base.CurrentUser.Id);
            CapitalUserAccount cm;
            if (CMA == null)
            {
                if (!string.IsNullOrEmpty(pp))
                {
                    cm = new CapitalUserAccount()
                    {
                        userid = base.CurrentUser.Id,
                        CashAccount = "",
                        CashAccountType = 0,
                        userType = base.CurrentUser.UserType
                    };
                }
                else
                {
                    cm = new CapitalUserAccount()
                    {
                        userid = base.CurrentUser.Id,
                        CashAccount = pp,
                        CashAccountType = 0,
                        userType = base.CurrentUser.UserType
                    };
                }
                strReutrn = cm.CashAccount;
                ICMAS.AddICapitalUserAccount(cm);
            }
            else
            {
                CMA.CashAccount = pp;
                ICMAS.UPCapitalUserAccount(CMA);
                strReutrn = CMA.CashAccount;
            }
            return Json(new { result = strReutrn });
        }

        [HttpPost]
        public JsonResult AddStatisticsMoney(int TradingType, decimal TradingPrice, string OrderNum, int PayType, string TypeID)
        {
            IStatisticsMoneyService isms = ServiceHelper.Create<IStatisticsMoneyService>();
            decimal moneybynow = isms.GetMoneyByUidType(base.CurrentUser.Id, base.CurrentUser.UserType);
            StatisticsMoney entity = new StatisticsMoney
            {
                UserId = base.CurrentUser.Id,
                UserName = base.CurrentUser.UserName,
                UserType = base.CurrentUser.UserType,
                TradingTime = DateTime.Now,
                TradingType = TradingType,
                TradingPrice = TradingPrice,
                OrderNum = OrderNum,
                PayType = PayType,
                Balance = moneybynow,
                BalanceAble = moneybynow
            };

            isms.Add(entity);
            return Json(new { Successful = true });
        }

        public ActionResult ToOtherDraw()
        {
            return View();
        }

        /// <summary>
        /// 获取当前用户的可用余额
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyValMoney()
        {
            Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
            if (fwinfo == null)
            {
                return Json("0");
            }
            else
            {
                return Json(fwinfo.Wallet_UserLeftMoney);
            }
        }
        /// <summary>
        /// 判断转账账户是否存在
        /// </summary>
        /// <param name="uname"></param>
        /// <returns></returns>
        public JsonResult Checkuid(string uname)
        {
            if (uname.Contains("'"))
            {
                uname = uname.Replace("'", "");
            }
            if (base.CurrentUser.UserName == uname) { return Json(""); }
            else
            {
                string strsql = string.Format("select * from ChemCloud_Members where username='{0}'", uname);
                DataSet ds = DbHelperSQL.Query(strsql.ToString());
                if (ds == null)
                {
                    return Json("");
                }
                else
                {
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return Json("");
                    }
                    else
                    {
                        return Json("ok");
                    }
                }

            }
        }
        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="uname">转账接收方</param>
        /// <param name="zmoney">转账金额</param>
        /// <returns></returns>
        public JsonResult AddZZinfo(string uname, string zmoney)
        {
            #region 获取转账对方的用户id和用户类型
            int touid = 0;
            int toutype = 0;
            string strsql = string.Format("select * from ChemCloud_Members where username='{0}'", uname);
            DataSet ds = DbHelperSQL.Query(strsql.ToString());
            if (ds == null)
            {
                return Json("");
            }
            else
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    touid = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                    toutype = int.Parse(ds.Tables[0].Rows[0]["UserType"].ToString());
                }
                else
                {
                    return Json("");
                }
            }
            #endregion
            #region 转账方的财务信息
            Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
            fwinfo.Wallet_UserLeftMoney = fwinfo.Wallet_UserLeftMoney - decimal.Parse(zmoney);//获取当前用的可用金额
            #endregion
            #region 转账对方的财务信息
            Finance_Wallet fwinfoto = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(touid, toutype, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
            fwinfoto.Wallet_UserLeftMoney = fwinfoto.Wallet_UserLeftMoney + decimal.Parse(zmoney);//获取当前用的可用金额
            #endregion
            #region 添加财务转账信息
            Finance_Transfer ftinfo = new Finance_Transfer();
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            ftinfo.Trans_Number = _orderBO.GenerateOrderNumber();//创建转账单号
            ftinfo.Trans_UserId = base.CurrentUser.Id;
            ftinfo.Trans_UserType = base.CurrentUser.UserType;
            ftinfo.Trans_Money = decimal.Parse(zmoney);
            ftinfo.Trans_SXMoney = 0;
            ftinfo.Trans_MoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            ftinfo.Trans_Time = DateTime.Now;
            ftinfo.Trans_Address = ChemCloud.Core.Common.GetIpAddress();
            ftinfo.Trans_ToUserId = touid;
            ftinfo.Trans_ToUserType = toutype;
            ftinfo.Trans_Status = 1;

            //对方账户金额转入信息
            Finance_InCome ftcome = new Finance_InCome();
            ftcome.InCome_Number = _orderBO.GenerateOrderNumber();
            ftcome.InCome_UserId = touid;
            ftcome.InCome_UserType = toutype;
            ftcome.InCome_StartTime = DateTime.Now;
            ftcome.InCome_EndTime = DateTime.Now.AddDays(int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayoutTime")));
            ftcome.InCome_Money = decimal.Parse(zmoney);
            ftcome.InCome_MoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            ftcome.InCome_Address = ChemCloud.Core.Common.GetIpAddress();
            ftcome.InCome_Type = 2;/// 收入类型(1交易2转账3退款默认1)
            ftcome.InCome_Status = 1;
            ftcome.InCome_OrderNum = _orderBO.GenerateOrderNumber();
            #endregion
            if (ServiceHelper.Create<IFinance_InComeService>().AddFinance_InCome(ftcome) && ServiceHelper.Create<IFinance_TransferService>().AddFinance_Transfer(ftinfo) && ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo) && ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfoto))
            {
                return Json("ok");
            }
            else
            {
                Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "时转账失败.");
                return Json("");
            }
        }
    }
}