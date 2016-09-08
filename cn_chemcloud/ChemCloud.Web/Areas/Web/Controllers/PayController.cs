using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ChemCloud.DBUtility;
using System.Data;
using System.Data.Common;
using Dapper;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class PayController : BaseWebController
    {
        public PayController()
        {
        }

        [ActionName("CashNotify")]
        [ValidateInput(false)]
        public ContentResult CashPayNotify_Post(string id, string str)
        {
            char[] chrArray = new char[] { '-' };
            decimal num = decimal.Parse(str.Split(chrArray)[0]);
            char[] chrArray1 = new char[] { '-' };
            string str1 = str.Split(chrArray1)[1];
            char[] chrArray2 = new char[] { '-' };
            long num1 = long.Parse(str.Split(chrArray2)[2]);
            id = DecodePaymentId(id);
            string empty = string.Empty;
            string empty1 = string.Empty;
            try
            {
                Plugin<IPaymentPlugin> plugin = PluginsManagement.GetPlugin<IPaymentPlugin>(id);
                PaymentInfo paymentInfo = plugin.Biz.ProcessReturn(base.HttpContext.Request);
                if ((Cache.Get(CacheKeyCollection.PaymentState(string.Join<long>(",", paymentInfo.OrderIds))) == null ? true : false))
                {
                    ICashDepositsService cashDepositsService = ServiceHelper.Create<ICashDepositsService>();
                    CashDepositDetailInfo cashDepositDetailInfo = new CashDepositDetailInfo()
                    {
                        AddDate = DateTime.Now,
                        Balance = num,
                        Description = "充值",
                        Operator = str1
                    };
                    List<CashDepositDetailInfo> cashDepositDetailInfos = new List<CashDepositDetailInfo>()
					{
						cashDepositDetailInfo
					};
                    if (cashDepositsService.GetCashDepositByShopId(num1) != null)
                    {
                        cashDepositDetailInfo.CashDepositId = cashDepositsService.GetCashDepositByShopId(num1).Id;
                        ServiceHelper.Create<ICashDepositsService>().AddCashDepositDetails(cashDepositDetailInfo);
                    }
                    else
                    {
                        CashDepositInfo cashDepositInfo = new CashDepositInfo()
                        {
                            CurrentBalance = num,
                            Date = DateTime.Now,
                            ShopId = num1,
                            TotalBalance = num,
                            EnableLabels = true,
                            ChemCloud_CashDepositDetail = cashDepositDetailInfos
                        };
                        cashDepositsService.AddCashDeposit(cashDepositInfo);
                    }
                    empty1 = plugin.Biz.ConfirmPayResult();
                    string str2 = CacheKeyCollection.PaymentState(string.Join<long>(",", paymentInfo.OrderIds));
                    Cache.Insert(str2, true);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                string message = exception.Message;
                Log.Error("CashPayNotify_Post", exception);
            }
            return base.Content(empty1);
        }

        private string DecodePaymentId(string paymentId)
        {
            return paymentId.Replace("-", ".");
        }

        [ActionName("CapitalChargeNotify")]
        [ValidateInput(false)]
        public ContentResult PayNotify_Charge(string id)
        {
            string empty = string.Empty;
            try
            {
                id = DecodePaymentId(id);
                Plugin<IPaymentPlugin> plugin = PluginsManagement.GetPlugin<IPaymentPlugin>(id);
                if (plugin != null)
                {
                    PaymentInfo paymentInfo = plugin.Biz.ProcessNotify(base.Request);
                    IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
                    ChargeDetailInfo chargeDetail = memberCapitalService.GetChargeDetail(paymentInfo.OrderIds.FirstOrDefault());
                    if (chargeDetail != null && chargeDetail.ChargeStatus != ChargeDetailInfo.ChargeDetailStatus.ChargeSuccess)
                    {
                        chargeDetail.ChargeWay = plugin.PluginInfo.DisplayName;
                        chargeDetail.ChargeStatus = ChargeDetailInfo.ChargeDetailStatus.ChargeSuccess;
                        chargeDetail.ChargeTime = new DateTime?((paymentInfo.TradeTime.HasValue ? paymentInfo.TradeTime.Value : DateTime.Now));
                        memberCapitalService.UpdateChargeDetail(chargeDetail);
                        empty = plugin.Biz.ConfirmPayResult();
                        string str = CacheKeyCollection.PaymentState(chargeDetail.Id.ToString());
                        Cache.Insert(str, true, 15);
                    }
                }
            }
            catch (Exception exception)
            {

            }
            return base.Content(empty);
        }

        [ActionName("Notify")]
        [ValidateInput(false)]
        public ContentResult PayNotify_Post(string id)
        {
            id = DecodePaymentId(id);
            string empty = string.Empty;
            string str = string.Empty;
            try
            {
                Plugin<IPaymentPlugin> plugin = PluginsManagement.GetPlugin<IPaymentPlugin>(id);
                PaymentInfo paymentInfo = plugin.Biz.ProcessNotify(base.HttpContext.Request);
                DateTime? tradeTime = paymentInfo.TradeTime;
                long num = paymentInfo.OrderIds.FirstOrDefault();
                List<long> list = (
                    from item in ServiceHelper.Create<IOrderService>().GetOrderPay(num)
                    select item.OrderId).ToList();
                try
                {
                    IOrderService orderService = ServiceHelper.Create<IOrderService>();
                    DateTime? nullable = paymentInfo.TradeTime;
                    orderService.PaySucceed(list, id, nullable.Value, paymentInfo.TradNo, num);
                    str = plugin.Biz.ConfirmPayResult();
                    string str1 = CacheKeyCollection.PaymentState(string.Join<long>(",", list));
                    Cache.Insert(str1, true, 15);
                    Dictionary<long, ShopBonusInfo> nums = new Dictionary<long, ShopBonusInfo>();
                    string str2 = string.Concat("http://", base.Request.Url.Host.ToString(), "/m-weixin/shopbonus/index/");
                    IShopBonusService shopBonusService = ServiceHelper.Create<IShopBonusService>();
                    foreach (OrderInfo order in ServiceHelper.Create<IOrderService>().GetOrders(list.AsEnumerable<long>()))
                    {
                        Log.Info(string.Concat("ShopID = ", order.ShopId));
                        ShopBonusInfo byShopId = shopBonusService.GetByShopId(order.ShopId);
                        Log.Info(string.Concat("商家活动价格：", byShopId.GrantPrice));
                        Log.Info(string.Concat("买家支付价格：", order.OrderTotalAmount));
                        if (byShopId.GrantPrice > order.OrderTotalAmount)
                        {
                            continue;
                        }
                        object[] objArray = new object[] { byShopId.Id, order.UserId, order.Id, str2 };
                        Log.Info(string.Format("{0} , {1} , {2} , {3} ", objArray));
                        long num1 = shopBonusService.GenerateBonusDetail(byShopId, order.UserId, order.Id, str2);
                        Log.Info(string.Concat("生成红包组，红包Grantid = ", num1));
                        nums.Add(num1, byShopId);
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    string str3 = string.Concat(id, " ", num.ToString());
                    if (paymentInfo.TradeTime.HasValue)
                    {
                        DateTime value = paymentInfo.TradeTime.Value;
                        str3 = string.Concat(str3, " TradeTime:", value.ToString());
                    }
                    str3 = string.Concat(str3, " TradNo:", paymentInfo.TradNo);
                    Log.Error(str3, exception);
                }
            }
            catch (Exception exception3)
            {
                Exception exception2 = exception3;
                string message = exception2.Message;
                Log.Error("PayNotify_Post", exception2);
            }
            return base.Content(str);
        }

        [ActionName("CapitalChargeReturn")]
        [ValidateInput(false)]
        public ActionResult PayReturn_Charge(string id)
        {
            string empty = string.Empty;
            try
            {
                id = DecodePaymentId(id);
                Plugin<IPaymentPlugin> plugin = PluginsManagement.GetPlugin<IPaymentPlugin>(id);
                if (plugin != null)
                {
                    PaymentInfo paymentInfo = plugin.Biz.ProcessReturn(base.Request);
                    IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
                    ChargeDetailInfo chargeDetail = memberCapitalService.GetChargeDetail(paymentInfo.OrderIds.FirstOrDefault());
                    if (chargeDetail != null && chargeDetail.ChargeStatus != ChargeDetailInfo.ChargeDetailStatus.ChargeSuccess)
                    {
                        chargeDetail.ChargeWay = plugin.PluginInfo.DisplayName;
                        chargeDetail.ChargeStatus = ChargeDetailInfo.ChargeDetailStatus.ChargeSuccess;
                        chargeDetail.ChargeTime = new DateTime?((paymentInfo.TradeTime.HasValue ? paymentInfo.TradeTime.Value : DateTime.Now));
                        memberCapitalService.UpdateChargeDetail(chargeDetail);
                        plugin.Biz.ConfirmPayResult();
                        string str = CacheKeyCollection.PaymentState(chargeDetail.Id.ToString());
                        Cache.Insert(str, true, 15);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(string.Concat("预付款充值回调出错：", exception.Message));
            }
            return View();
        }




        /// <summary>
        /// 成功添加支付记录后更新供应商的锁定金额和添加供应商的收入记录
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="price">实付金额</param>
        /// <returns></returns>
        public JsonResult AddInComeAndUpdateManagerWallet(string orderId, string price)
        {

            decimal bxfei = 0;
            OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderId));
            if (oinfo == null)
            {
                Log.Error("获取成功支付的订单信息失败");
                return Json("");
            }
            else
            {
                bxfei = oinfo.Insurancefee;
                ManagerInfo minfo = ServiceHelper.Create<IManagerService>().GetManagerInfoByShopId(oinfo.ShopId);
                if (minfo == null)
                {
                    Log.Error("获取成功支付订单的供应商信息失败.");
                    return Json("");
                }
                else
                {
                    UserMemberInfo uminfo = ServiceHelper.Create<IMemberService>().GetMemberByName(minfo.UserName);
                    if (uminfo == null)
                    {
                        Log.Error("获取成功支付订单的供应商用户编号、用户类型失败.");
                        return Json("");
                    }
                    else
                    {
                        #region 添加供应商的收入记录
                        int montype = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
                        ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                        Finance_InCome fi = new Finance_InCome();
                        fi.InCome_Number = _orderBO.GenerateOrderNumber();
                        fi.InCome_UserId = uminfo.Id;
                        fi.InCome_UserType = uminfo.UserType;
                        fi.InCome_StartTime = DateTime.Now;
                        fi.InCome_EndTime = DateTime.Now.AddDays(int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayoutTime")));
                        fi.InCome_Money = decimal.Parse(price) - bxfei;
                        fi.InCome_MoneyType = montype;
                        fi.InCome_OrderNum = long.Parse(orderId);
                        fi.InCome_Address = ChemCloud.Core.Common.GetIpAddress();
                        fi.InCome_Type = 1;
                        fi.InCome_Status = 1;
                        if (ServiceHelper.Create<IFinance_InComeService>().AddFinance_InCome(fi))
                        {
                            Log.Info(DateTime.Now + "时,成功添加供应商编号" + uminfo.Id + "的收入信息,收入金额：" + (fi.InCome_Money - bxfei) + ",收入币种：" + fi.InCome_MoneyType);
                            //更新供应商锁定金额
                            Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(uminfo.Id, uminfo.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
                            if (fwinfo == null)
                            {
                                return Json("");
                            }
                            fwinfo.Wallet_UserMoneyLock = fwinfo.Wallet_UserMoneyLock + (decimal.Parse(price) - bxfei);
                            if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo))
                            {
                                return Json("ok");
                            }
                            else
                            {
                                return Json("");
                            }

                        }
                        else
                        {
                            Log.Info(DateTime.Now + "时,添加供应商编号" + uminfo.Id + "的收入信息失败.");
                            return Json("");
                        }
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="status"></param>
        /// <returns></returns>

        public JsonResult UpdateOrderStatus(long orderId, string status, string paymodel)
        {
            ServiceHelper.Create<IOrderService>().UpdateOrderStatuNew(orderId, int.Parse(status), paymodel);
            return Json("ok");
        }



        /// <summary>
        /// 添加全额/分期支付记录操作完成更新个人钱包 王传伟
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="price">实际支付的金额</param>
        /// <param name="isWallet">类型 1.是账户余额支付 2.第三方其它支付方式</param>
        /// <returns></returns>
        public JsonResult AddUserPayMentInfoPlatByUser(string orderid, string price, string isWallet, string paytype, long userID, string userName, int userType)
        {
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            int m_type = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            Finance_Payment fp = new Finance_Payment();
            fp.PayMent_Number = _orderBO.GenerateOrderNumber();
            //待处理  
            fp.PayMent_UserId = userID;
            fp.PayMent_UserType = userType;
            fp.PayMent_OrderNum = long.Parse(orderid);
            fp.PayMent_PayTime = DateTime.Now;
            fp.PayMent_PayMoney = decimal.Parse(price);
            if (paytype == "0")
            {
                OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));
                fp.PayMent_TotalMoney = oinfo.ProductTotalAmount;
                fp.PayMent_BXMoney = oinfo.Insurancefee;
                fp.PayMent_YFMoney = oinfo.Freight;
                fp.PayMent_JYMoney = oinfo.Transactionfee;
                fp.PayMent_SXMoney = oinfo.Counterfee;
            }
            else
            {
                fp.PayMent_TotalMoney = decimal.Parse(price);
                fp.PayMent_BXMoney = 0;
                fp.PayMent_YFMoney = 0;
                fp.PayMent_JYMoney = 0;
                fp.PayMent_SXMoney = 0;
            }
            fp.PayMent_PayAddress = ChemCloud.Core.Common.GetIpAddress();
            fp.PayMent_MoneyType = m_type;
            fp.PayMent_Status = 1;
            fp.PayMent_Type = int.Parse(paytype);
            if (ServiceHelper.Create<IFinance_PaymentService>().AddFinance_Payment(fp))
            {
                Log.Info("用户" + userID + "在" + DateTime.Now + "创建支付记录成功,支付单号：" + fp.PayMent_Number + ".");
                if (isWallet == "1")
                {
                    #region 如果是账户余额支付 更新个人的账户信息
                    Finance_Wallet fw = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(userID, userType, m_type);
                    if (fw != null)
                    {
                        if (fw.Wallet_UserLeftMoney < decimal.Parse(price))
                        {
                            Log.Error("用户" + userID + "在" + DateTime.Now + "更新自己的账户时出错,出错信息：个人账户额小于订单应支付的金额.");
                            return Json("no");
                        }
                        else
                        {
                            fw.Wallet_DoIpAddress = ChemCloud.Core.Common.GetIpAddress();
                            fw.Wallet_DoLastTime = DateTime.Now;
                            fw.Wallet_DoUserId = userID;
                            fw.Wallet_DoUserName = userName;
                            fw.Wallet_MoneyType = m_type;
                            fw.Wallet_UserLeftMoney = fw.Wallet_UserLeftMoney - decimal.Parse(price);
                            if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fw))
                            {
                                Log.Info("用户" + userID + "在" + DateTime.Now + "成功更新了自己的账户余额.");
                                return Json("ok");
                            }
                            else
                            {
                                Log.Error("用户" + userID + "在" + DateTime.Now + "更新自己的账户时出错.");
                                return Json("no");
                            }
                        }
                    }
                    else
                    {
                        return Json("no");
                    }
                    #endregion
                }
                else
                {
                    return Json("ok");
                }
            }
            else
            {
                Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "创建支付记录失败.");
                return Json("no");
            }
        }


        /// <summary>
        /// 成功添加支付记录后更新平台的的 锁定金额和 平台的收入记录
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="price">实付金额</param>
        /// <returns></returns>
        public JsonResult AddInComeAndUpdatePlatformWallet(string orderId, string price, string paytype)
        {

            long userid = 0; int usertype = 1;
            /*4定制合成 5代理采购 (此时支付给供应商)*/
            if (paytype == "4")
            {
                string strsql = string.Format("SELECT * FROM dbo.ChemCloud_OrderSynthesis where OrderNumber='" + orderId + "';");
                DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ZhifuImg"] != null)
                    {
                        userid = long.Parse(dt.Rows[0]["ZhifuImg"].ToString());
                        usertype = 2;
                    }
                }
            }

            if (paytype == "5")
            {
                string strsql = string.Format("SELECT * FROM dbo.ChemCloud_OrderPurchasing where OrderNum='" + orderId + "';");
                DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ZhifuImg"] != null)
                    {
                        userid = long.Parse(dt.Rows[0]["ZhifuImg"].ToString());
                        usertype = 2;
                    }
                }
            }

            #region 添加供应商的收入记录
            int montype = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            Finance_InCome fi = new Finance_InCome();
            fi.InCome_Number = _orderBO.GenerateOrderNumber();
            fi.InCome_UserId = userid;
            fi.InCome_UserType = usertype;
            fi.InCome_StartTime = DateTime.Now;
            fi.InCome_EndTime = DateTime.Now.AddDays(int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayoutTime")));
            fi.InCome_Money = decimal.Parse(price);
            fi.InCome_MoneyType = montype;
            fi.InCome_OrderNum = long.Parse(orderId);
            fi.InCome_Address = ChemCloud.Core.Common.GetIpAddress();
            fi.InCome_Type = 1;
            fi.InCome_Status = 1;
            if (ServiceHelper.Create<IFinance_InComeService>().AddFinance_InCome(fi))
            {
                Log.Info(DateTime.Now + "时,成功添加平台的收入信息,收入金额：" + (fi.InCome_Money) + ",收入币种：" + fi.InCome_MoneyType);
                //更新供应商锁定金额
                Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(-1, 1, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
                if (fwinfo == null)
                {
                    return Json("");
                }
                fwinfo.Wallet_UserMoneyLock = fwinfo.Wallet_UserMoneyLock + (decimal.Parse(price));
                if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo))
                {
                    return Json("ok");
                }
                else
                {
                    return Json("");
                }

            }
            else
            {
                Log.Info(DateTime.Now + "时,添加平台的收入信息失败.");
                return Json("");
            }
            #endregion
        }



        /// <summary>
        /// 王传伟 微信 支付 回调 处理
        /// http://bbs.csdn.net/topics/391492706
        /// 模式二 返回数据格式
        /// {
        ///  "appid": "wx2bdc9908deaeff7f",
        ///  "bank_type": "COMM_DEBIT",
        ///  "cash_fee": "1",
        /// "fee_type": "CNY",
        /// "is_subscribe": "Y",
        /// "mch_id": "1288273101",
        ///  "nonce_str": "e0e12859798a4b8b8e6a8977fb7c122b",
        /// "openid": "oT3MNwPdjrC2bkC9iXCXm9rllg7Y",
        /// "out_trade_no": "128827310120151231135120909", //我们传给微信的订单号 也就是我们的订单号
        ///  "result_code": "SUCCESS",
        /// "return_code": "SUCCESS",
        /// "sign": "3E221425A46E3762680AF9E523817AE7",
        /// "time_end": "20151231135246",
        /// "total_fee": "1",
        ///  "trade_type": "NATIVE",
        /// "transaction_id": "1006090192201512312420034665" 微信交易号 查询订单是否存在
        /// }
        /// </summary>
        /// <returns></returns>
        public ActionResult QRResultNotify()
        {

            Log.Info("Begin QRResultNotify");

            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();


            Log.Info("Begin QRResultNotify");
            //
            Log.Info("Begin 微信来的数据：" + builder.ToString());
            WxPayAPI.WxPayData notifyData = new WxPayAPI.WxPayData();
            try
            {
                notifyData.FromXml(builder.ToString());
            }
            catch (WxPayAPI.WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayAPI.WxPayData res = new WxPayAPI.WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);

                Response.Write(res.ToXml());
                Response.End();

                Log.Info("Begin 若签名错误，则立即返回结果给微信支付后台：");
                return null;

            }

            Log.Info("Begin 检查支付结果中transaction_id是否存在");
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayAPI.WxPayData res = new WxPayAPI.WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");


                Response.Write(res.ToXml());
                Response.End();

                Log.Info("Begin 支付结果中微信订单号不存在：");
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            Log.Info("Begin transaction_id" + transaction_id);

            //
            Log.Info("Begin 查询订单，判断订单真实性");
            if (!WxPayAPI.WxPayApi.QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayAPI.WxPayData res = new WxPayAPI.WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");

                Response.Write(res.ToXml());
                Response.End();

                Log.Info("Begin 订单查询失败：");
            }
            //查询订单成功 
            else
            {
                Log.Info("Begin 查询订单成功");

                WxPayAPI.WxPayData res = new WxPayAPI.WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");

                //这里才处理平台业务

                ///参 Pay.cshtml
                ///
                /*
                 *  $.post('AddUserPayMentInfo', { "orderid": orderid, "price": totalmon, "isWallet": 1, "paytype": $("#hidpaytype").val() }, function (result) {
                                                            if (result == "ok") {
                                                                //成功添加支付记录后更新供应商的锁定金额和添加供应商的收入记录
                                                                $.post('/common/PublicMoney/AddInComeAndUpdateManagerWallet', { "orderId": orderid, "price": totalmon, "isWallet": 1 }, function (result) {
                                                                    if (result == "ok") {
                                                                        //成功操作后执行订单状态修改
                                                                        $.post('/common/PublicMoney/UpdateOrderStatus', { "orderId": orderid, "status": paystatus, "paymodel": "余额支付" }, function (result) {
                                                                            if (result == "ok") {
                                                                                loading.close();
                                                                                //全部操作成功完成后执行订单状态修改
                                                                                var strs = '/userCenter?url=/userorder?orderids=' + orderid + '&tar=userorder';
                                                                                //setTimeout("change(" + strs + ")", 3000);
                                                                                $.dialog.succeedTips("您已完成支付！", function () { location.href = strs; });

                                                                            }
                                                                        });
                                                                    }
                                                                });
                                                            }
                                                        });
                 */


                string out_trade_no = notifyData.GetValue("out_trade_no").ToString();

                string payType = notifyData.GetValue("attach").ToString();

                //注意 订单支付

                if (payType == "0")
                {


                    try
                    {

                        var orderInfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(out_trade_no));

                        if (orderInfo.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay)
                        {


                            Response.Write(res.ToXml());
                            Response.End();
                            return null;
                        }


                        JsonResult AddUserPayMentInfoResult = AddUserPayMentInfo(out_trade_no, orderInfo.OrderTotalAmount.ToString(), "0", "4", orderInfo.UserId);



                        if (AddUserPayMentInfoResult.Data.ToString() == "ok")
                        {



                            JsonResult AddInComeAndUpdateManagerWalletResult = AddInComeAndUpdateManagerWallet(out_trade_no, orderInfo.OrderTotalAmount.ToString());

                            if (AddInComeAndUpdateManagerWalletResult.Data.ToString() == "ok")
                            {

                                UpdateOrderStatus(long.Parse(out_trade_no), "2", "微信支付");

                            }
                        }

                    }
                    catch (Exception ee)
                    {

                    }
                }
                //end  订单支付

                else if (payType == "2")//产品认证支付
                {

                    try
                    {

                        Internal_MemberInfo member = GetMemberInfoByProduct(int.Parse(out_trade_no)).FirstOrDefault();

                        string total_fee = notifyData.GetValue("total_fee").ToString();

                        //这里 除了 100
                        AddUserPayMentInfoPlatByUser(out_trade_no, (int.Parse(total_fee) / 100).ToString(), "0", "2", long.Parse(member.UserID.ToString()), member.UserName, member.UserType);

                        AddInComeAndUpdatePlatformWallet(out_trade_no, total_fee, "2");

                        ServiceHelper.Create<IOrderService>().UpdatePayToPlatformStatus("2", out_trade_no);

                    }
                    catch (Exception)
                    {

                        throw;
                    }


                }//end 产品认证支付

                else if (payType == "3")//供应商认证支付
                {

                    try
                    {

                        Internal_MemberInfo member = GetMemberInfoByShop(int.Parse(out_trade_no)).FirstOrDefault();



                        string total_fee = notifyData.GetValue("total_fee").ToString();

                        //这里 除了 100
                        AddUserPayMentInfoPlatByUser(out_trade_no, (int.Parse(total_fee) / 100).ToString(), "0", "3", long.Parse(member.UserID.ToString()), member.UserName, member.UserType);

                        AddInComeAndUpdatePlatformWallet(out_trade_no, total_fee, "3");

                        ServiceHelper.Create<IOrderService>().UpdatePayToPlatformStatus("3", out_trade_no);

                    }
                    catch (Exception)
                    {

                        throw;
                    }


                }//end 供应商认证支付

                else if (payType == "4")//定制合成
                {

                    try
                    {



                        Internal_MemberInfo member = GetMemberInfoByOrderSynthesis(int.Parse(out_trade_no)).FirstOrDefault();


                        string total_fee = notifyData.GetValue("total_fee").ToString();

                        //这里 除了 100
                        AddUserPayMentInfoPlatByUser(out_trade_no, (int.Parse(total_fee) / 100).ToString(), "0", payType, long.Parse(member.UserID.ToString()), member.UserName, member.UserType);

                        AddInComeAndUpdatePlatformWallet(out_trade_no, total_fee, payType);

                        ServiceHelper.Create<IOrderService>().UpdatePayToPlatformStatus(payType, out_trade_no);

                    }
                    catch (Exception)
                    {

                        throw;
                    }


                }//end 定制合成


                else if (payType == "5")//代理采购
                {

                    try
                    {



                        Internal_MemberInfo member = GetMemberInfoByOrderPurchasing(int.Parse(out_trade_no)).FirstOrDefault();


                        string total_fee = notifyData.GetValue("total_fee").ToString();

                        //这里 除了 100
                        AddUserPayMentInfoPlatByUser(out_trade_no, (int.Parse(total_fee) / 100).ToString(), "0", payType, long.Parse(member.UserID.ToString()), member.UserName, member.UserType);

                        AddInComeAndUpdatePlatformWallet(out_trade_no, total_fee, payType);

                        ServiceHelper.Create<IOrderService>().UpdatePayToPlatformStatus(payType, out_trade_no);

                    }
                    catch (Exception)
                    {

                        throw;
                    }


                }//end 代理采购



                Response.Write(res.ToXml());
                Response.End();

                Log.Info("End  微信");
            }


            return null;

        }

        public class Internal_MemberInfo
        {
            /// <summary>
            /// 用户编号
            /// </summary>
            public int UserID { get; set; }

            /// <summary>
            /// 用户名称
            /// </summary>
            public string UserName
            {
                get;
                set;
            }
            /// <summary>
            /// 用户类型
            /// </summary>
            public int UserType { get; set; }
        }



        /// <summary>
        /// 根据  代理采购 编号 get 用户信息
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>

        public static IEnumerable<Internal_MemberInfo> GetMemberInfoByOrderPurchasing(int orderNum)
        {
            string sql = @"SELECT b.id as UserName,b.UserName as UserName,b.UserType as UserType FROM ChemCloud_OrderPurchasing as a,ChemCloud_Members as b
where a.UserId=b.Id and a.OrderNum=@OrderNum";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ChemCloud.Web.Data.DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Internal_MemberInfo>(
                    sql, new
                    {

                        OrderNum = orderNum

                    });
            }
        }


        /// <summary>
        /// 根据 定制合成  编号 get 用户信息
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>

        public static IEnumerable<Internal_MemberInfo> GetMemberInfoByOrderSynthesis(int orderNum)
        {
            string sql = @"SELECT b.id as UserName,b.UserName as UserName,b.UserType as UserType FROM ChemCloud_OrderPurchasing as a,ChemCloud_Members as b
where a.UserId=b.Id and a.OrderNum=@OrderNum";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ChemCloud.Web.Data.DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Internal_MemberInfo>(
                    sql, new
                    {

                        OrderNum = orderNum

                    });
            }
        }

        /// <summary>
        /// 根据产品 编号 get 用户信息
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>

        public static IEnumerable<Internal_MemberInfo> GetMemberInfoByProduct(int productID)
        {
            string sql = @"select a.ProductId,c.Id,c.UserName,c.UserType from  chemcloud_ProductAuthentication  as a left join 
                             ChemCloud_Managers as b on a.ManageId=b.Id left join ChemCloud_Members as c on b.UserName=c.UserName

                            where a.ProductId=@ProductID
                            order by a.ProductId desc";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ChemCloud.Web.Data.DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Internal_MemberInfo>(
                    sql, new
                    {

                        ProductID = productID

                    });
            }
        }



        /// <summary>
        /// 根据Shop 编号 get 用户信息
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>

        public static IEnumerable<Internal_MemberInfo> GetMemberInfoByShop(int shopID)
        {
            string sql = @" select a.ShopId,b.Id as UserID,b.UserName as UserName,b.UserType as UserType from ChemCloud_Managers  as a left join ChemCloud_Members as b
                                 on a.UserName=b.UserName 
                 where a.ShopId=@ShopID";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ChemCloud.Web.Data.DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Internal_MemberInfo>(
                    sql, new
                    {

                        ShopID = shopID

                    });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="out_trade_no">订单号</param>
        /// <param name="paytype">payType :支付类型（3供应商认证支付,2产品认证支付，1排名付费支付,0订单支付)</param>
        /// <returns></returns>
        public decimal GetOrderTotalAmount(string out_trade_no, string paytype = "0", string type = "")
        {



            ///订单支付
            if (paytype == "0")
            {
                if (type == "webcz")
                {
                    Finance_Recharge frinfo = ServiceHelper.Create<IFinance_RechargeService>().GetFinance_RechargeInfo(long.Parse(out_trade_no));
                    if (frinfo != null)
                    {
                        return frinfo.Recharge_Money;
                    }
                    else
                    {

                        return 0;
                    }
                }
                else
                {
                    var orderInfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(out_trade_no));
                    return orderInfo.OrderTotalAmount;
                }

            }


            else if (paytype == "2")//产品认证支付 g userid,username,usertype,
            {
                return decimal.Parse(ServiceHelper.Create<ISiteSettingService>().GetSiteValue("ProductCertificationAmount") == null ? "0" : ServiceHelper.Create<ISiteSettingService>().GetSiteValue("ProductCertificationAmount"));
            }
            else if (paytype == "3")//供应商认证支付 g out_trade_no=shopeid,  userid,username,usertype
            {
                return decimal.Parse(ServiceHelper.Create<ISiteSettingService>().GetSiteValue("SupplierCertificationAmount") == null ? "0" : ServiceHelper.Create<ISiteSettingService>().GetSiteValue("SupplierCertificationAmount"));

            }


            if (paytype == "5")//代理采购 c ChemCloud_OrderPurchasing   memberid,->merbers userid,username,usertype,
            {
                decimal amount = 0;
                string strsql = string.Format("SELECT * FROM dbo.ChemCloud_OrderPurchasing where OrderNum='" + out_trade_no + "';");
                DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ProductPrice"] != null && dt.Rows[0]["ProductCount"] != null)
                    {
                        amount = decimal.Parse(dt.Rows[0]["ProductPrice"].ToString()) * decimal.Parse(dt.Rows[0]["ProductCount"].ToString());
                    }
                }

                return amount;
            }
            else if (paytype == "4") //定制合成 c c ChemCloud_OrderSynthesis   memberid,->merbers userid,username,usertype,
            {
                decimal amount = 0;
                string strsql = string.Format("SELECT * FROM dbo.ChemCloud_OrderSynthesis where OrderNumber='" + out_trade_no + "';");
                DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Price"] != null && dt.Rows[0]["ProductCount"] != null)
                    {
                        amount = decimal.Parse(dt.Rows[0]["Price"].ToString()) * decimal.Parse(dt.Rows[0]["ProductCount"].ToString());
                    }
                }
                return amount;
            }

            return 0;
        }


        /// <summary>
        /// 王传伟 生成订单 微信支付二维码
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public ActionResult GetQRPicture(string out_trade_no, string paytype = "0", string type = "")
        {

            try
            {



                decimal orderTotalAmount = GetOrderTotalAmount(out_trade_no, paytype, type);

                WxPayAPI.WxPayData data = new WxPayAPI.WxPayData();

                //out_trade_no 通知页面处理一
                data.SetValue("out_trade_no", out_trade_no);
                data.SetValue("body", out_trade_no.ToString());
                data.SetValue("total_fee", (int)(orderTotalAmount * 100));
                data.SetValue("trade_type", "NATIVE");

                //附加数据 这里放支付 paytype 
                data.SetValue("attach", paytype);
                data.SetValue("product_id", out_trade_no.ToString());


                /*
                 * https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=6_5
                 * 模式二与模式一相比，
                 * 流程更为简单，不依赖设置的回调支付URL。
                 * 商户后台系统先调用微信支付的统一下单接口，
                 * 微信后台系统返回链接参数code_url，
                 * 商户后台系统将code_url值生成二维码图片，
                 * 用户使用微信客户端扫码后发起支付。
                 * 注意：code_url有效期为2小时，过期后扫码不能再发起支付。
                 */
                WxPayAPI.WxPayData r = WxPayAPI.WxPayApi.UnifiedOrder(data, 20);

                ThoughtWorks.QRCode.Codec.QRCodeEncoder dec = new ThoughtWorks.QRCode.Codec.QRCodeEncoder();

                Image image = dec.Encode(r.GetValue("code_url").ToString());


                MemoryStream mem = new MemoryStream();

                image.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);

                ///生成二维码图片
                return File(mem.ToArray(), "image/jpeg");
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        [HttpGet()]
        /// <summary>
        ///get  订单状况 
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public ActionResult GetOrderStatue(string out_trade_no, string paytype, string type)
        {

            try
            {

                return Json(new { OrderStatus = InternalGetOrderStatue(out_trade_no, paytype, type) }, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ee)
            {
                return Json(new { OrderStatus = -1 }, JsonRequestBehavior.AllowGet);
            }


        }

        /// <summary>
        /// 订单状况  1已经支付了 其它 没有支付
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="paytype"></param>
        /// <returns></returns>
        public int InternalGetOrderStatue(string out_trade_no, string paytype, string type = "")
        {

            if (paytype == "0")
            {
                if (type == "webcz")
                {
                    Finance_Recharge frinfo = ServiceHelper.Create<IFinance_RechargeService>().GetFinance_RechargeInfo(long.Parse(out_trade_no));
                    if (frinfo != null)
                    {
                        if (frinfo.Recharge_Type == 0) { return 0; } else { return 1; }
                    }
                }
                else
                {
                    var orderInfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(out_trade_no));
                    return orderInfo.OrderStatus == OrderInfo.OrderOperateStatus.Close ? 1 : 0;
                }
            }

            ///产品认证支付 
            if (paytype == "2")
            {

                //核状态 0已提交 1已确认 2已支付 3审核通过 4审核拒绝  
                string sql = @"select count(*) from  chemcloud_ProductAuthentication   
                       where Productid=@ProductID and AuthStatus='2'";


                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ChemCloud.Web.Data.DataConstantConfigure.strConn))
                {
                    conn.Open();
                    return conn.ExecuteScalar<int>(
                        sql, new
                        {

                            ProductID = out_trade_no

                        }) > 0 ? 1 : 0;
                }



            }//end 产品认证支付


            //TODO::供应商认证支付
            if (paytype == "3")
            {


                string sql = @"SELECT COUNT(*) FROM ChemCloud_FieldCertification as a,ChemCloud_Managers as b
                     WHERE  a.Id=b.CertificationId and a.Status='6' and b.ShopId=@ShopID;";


                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ChemCloud.Web.Data.DataConstantConfigure.strConn))
                {
                    conn.Open();
                    return conn.ExecuteScalar<int>(
                        sql, new
                        {

                            ShopID = out_trade_no

                        }) > 0 ? 1 : 0;
                }

            }//end 供应商认证支付

            //定制合成
            if (paytype == "4")
            {

                //TODO::状况

                //是否锁定 默认0 未锁定
                string sql = @"SELECT COUNT(*)  FROM ChemCloud_OrderSynthesis
                         WHERE OrderNum=@OrderNum AND Status='3'";


                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ChemCloud.Web.Data.DataConstantConfigure.strConn))
                {
                    conn.Open();
                    return conn.ExecuteScalar<int>(
                        sql, new
                        {

                            OrderNum = out_trade_no

                        }) > 0 ? 1 : 0;
                }


            }//end 定制合成

            //代理采购
            if (paytype == "5")
            {
                //TODO::状况
                string sql = @"SELECT COUNT(*)  FROM ChemCloud_OrderPurchasing
                         WHERE OrderNum=@OrderNum AND Status='3'";


                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ChemCloud.Web.Data.DataConstantConfigure.strConn))
                {
                    conn.Open();
                    return conn.ExecuteScalar<int>(
                        sql, new
                        {

                            OrderNum = out_trade_no

                        }) > 0 ? 1 : 0;
                }

            }//end 代理采购




            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public ActionResult QRPay(string out_trade_no, string paytype = "0", string type = "")
        {
            ViewBag.type = type;

            ///已经支付了
            if (InternalGetOrderStatue(out_trade_no, paytype, type) == 1)
            {

                ViewBag.Name = "微信";


                ViewBag.QRCode = "~/Areas/Web/Images/paid.png";

                ViewBag.out_trade_no = "";

                return View();


            }
            else
            {
                ViewBag.Name = "微信";

                ViewBag.QRCode = "GetQRPicture?out_trade_no=" + out_trade_no + "&paytype=" + paytype + "&type='" + type + "'";
                ViewBag.out_trade_no = out_trade_no;
                ViewBag.paytype = paytype;
                return View();
            }
        }



        public ActionResult Return()
        {
            ViewBag.utype = base.CurrentUser.UserType;
            return View();
        }

        public ActionResult ReturnSuccess(string id)
        {
            ViewBag.OrderIds = base.Request.QueryString[id];
            ViewBag.Logo = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings().Logo;
            return View("Return");
        }
        [HttpPost]
        public JsonResult UpChargeOrderStatus(long orderIds)
        {
            ServiceHelper.Create<IMemberCapitalService>().UpdateChareeOrderStatu(orderIds);
            return Json(new { Successful = true });
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
                Balance = moneybynow + TradingPrice,
                BalanceAble = moneybynow + TradingPrice
            };
            isms.Add(entity);
            return Json(new { Successful = true });
        }
        [HttpPost]
        public JsonResult Account(decimal Price, string Type)
        {
            #region 获取当前用户的金额信息
            long Id;
            decimal? balance;
            decimal? freezeAmount;
            decimal? chargeAmount;
            decimal? newPrice;
            IMemberCapitalService imcs = ServiceHelper.Create<IMemberCapitalService>();
            CapitalInfo capitalInfo = ServiceHelper.Create<IMemberCapitalService>().GetCapitalInfo(base.CurrentUser.Id);
            if (capitalInfo == null)
            {
                CapitalInfo cinfo = new CapitalInfo
                {
                    MemId = base.CurrentUser.Id,
                    Balance = Price,
                    FreezeAmount = 0,
                    ChargeAmount = 0,
                    ManageId = 0
                };
                imcs.AddCapitalInfo(cinfo);
                Id = cinfo.Id;
                balance = cinfo.Balance;
                freezeAmount = cinfo.FreezeAmount;
                chargeAmount = cinfo.ChargeAmount;
            }
            else
            {
                Id = capitalInfo.Id;
                balance = capitalInfo.Balance;
                freezeAmount = capitalInfo.FreezeAmount;
                chargeAmount = capitalInfo.ChargeAmount;
            }
            #endregion
            if (Type == "add")
            {
                newPrice = balance + Price;
            }
            else
            {
                newPrice = balance - Price;
            }
            imcs.UpdateCapitalAmount(Id, base.CurrentUser.Id, newPrice, 0, 0, 0);
            return Json(new { Successful = true });
        }

        public JsonResult UpFQStatusandMoney(string orderid, string fqfirst)
        {
            if (string.IsNullOrEmpty(orderid))
            {
                return Json("");
            }
            else
            {
                FQPayment fqp = ServiceHelper.Create<IFQPaymentService>().GetFQPaymentInfo(long.Parse(orderid), base.CurrentUser.Id);
                if (fqp == null)
                {
                    return Json("");
                }
                if (string.IsNullOrEmpty(fqfirst))
                {
                    fqp.LeftPrice = fqp.LeftPrice - fqp.RealPrice;
                }
                fqp.PayTime = DateTime.Now;
                if (fqp.LeftPrice == 0)
                {
                    fqp.Status = 2;
                }
                else
                {
                    fqp.Status = 1;
                }
                if (ServiceHelper.Create<IFQPaymentService>().UpdateFQPayment(fqp))
                {
                    return Json("" + fqp.LeftPrice);
                }
                else
                {
                    return Json("");
                }
            }
        }
        [HttpPost]
        public JsonResult AddAccount(string ordernum, string orderprice)
        {
            IStatisticsMoneyService imcs = ServiceHelper.Create<IStatisticsMoneyService>();
            //decimal moneynow = imcs.GetMoneyByUidType(base.CurrentUser.Id, 3);
            IManagersAccountService IMAS = ServiceHelper.Create<IManagersAccountService>();
            OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(ordernum));

            decimal price;
            if (decimal.TryParse(orderprice, out price))
            {
                price = decimal.Parse(orderprice);
            }
            ManagersAccount MA = new ManagersAccount()
            {
                Zhuanzhang = price,
                ZhuanzhangName = base.CurrentUser.UserName,
                Tiqu = 0,
                Tiqufeiyong = 0,
                Tiquhuobi = "",
                Huilv = "",
                Tuikuan = 0,
                OrderNum = ordernum,
                Balance = price,
                BalanceAvailable = price,
                Datatime = DateTime.Now,
                ManagerId = oinfo.ShopId,
                Daikuan = 0
            };
            IMAS.AddManagersAccountInfo(MA);
            return Json(new { Successful = true });
        }

        /// <summary>
        /// 添加全额/分期支付记录操作完成更新个人钱包
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="price">实际支付的金额</param>
        /// <param name="isWallet">类型 1.是账户余额支付 2.第三方其它支付方式</param>
        /// <returns></returns>
        public JsonResult AddUserPayMentInfo(string orderid, string price, string isWallet, string paytype, long userid)
        {
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            int m_type = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            Finance_Payment fp = new Finance_Payment();
            fp.PayMent_Number = _orderBO.GenerateOrderNumber();
            //待处理  
            fp.PayMent_UserId = userid;
            fp.PayMent_UserType = 3;
            fp.PayMent_OrderNum = long.Parse(orderid);
            fp.PayMent_PayTime = DateTime.Now;
            fp.PayMent_PayMoney = decimal.Parse(price);
            if (paytype == "0")
            {
                OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));
                fp.PayMent_TotalMoney = oinfo.ProductTotalAmount;
                fp.PayMent_BXMoney = oinfo.Insurancefee;
                fp.PayMent_YFMoney = oinfo.Freight;
                fp.PayMent_JYMoney = oinfo.Transactionfee;
                fp.PayMent_SXMoney = oinfo.Counterfee;
            }
            else
            {
                fp.PayMent_TotalMoney = decimal.Parse(price);
                fp.PayMent_BXMoney = 0;
                fp.PayMent_YFMoney = 0;
                fp.PayMent_JYMoney = 0;
                fp.PayMent_SXMoney = 0;
            }
            fp.PayMent_PayAddress = ChemCloud.Core.Common.GetIpAddress();
            fp.PayMent_MoneyType = m_type;
            fp.PayMent_Status = 1;
            fp.PayMent_Type = int.Parse(paytype);
            if (ServiceHelper.Create<IFinance_PaymentService>().AddFinance_Payment(fp))
            {
                Log.Info("用户" + userid + "在" + DateTime.Now + "创建支付记录成功,支付单号：" + fp.PayMent_Number + ".");
                if (isWallet == "1")
                {
                    #region 如果是账户余额支付 更新个人的账户信息
                    Finance_Wallet fw = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(userid, 3, m_type);
                    if (fw != null)
                    {
                        if (fw.Wallet_UserLeftMoney < decimal.Parse(price))
                        {
                            Log.Error("用户" + userid + "在" + DateTime.Now + "更新自己的账户时出错,出错信息：个人账户额小于订单应支付的金额.");
                            return Json("no");
                        }
                        else
                        {
                            fw.Wallet_DoIpAddress = ChemCloud.Core.Common.GetIpAddress();
                            fw.Wallet_DoLastTime = DateTime.Now;
                            fw.Wallet_DoUserId = base.CurrentUser.Id;
                            fw.Wallet_DoUserName = base.CurrentUser.UserName;
                            fw.Wallet_MoneyType = m_type;
                            fw.Wallet_UserLeftMoney = fw.Wallet_UserLeftMoney - decimal.Parse(price);
                            if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fw))
                            {
                                Log.Info("用户" + userid + "在" + DateTime.Now + "成功更新了自己的账户余额.");
                                return Json("ok");
                            }
                            else
                            {
                                Log.Error("用户" + userid + "在" + DateTime.Now + "更新自己的账户时出错.");
                                return Json("no");
                            }
                        }
                    }
                    else
                    {
                        return Json("no");
                    }
                    #endregion
                }
                else
                {
                    return Json("ok");
                }
            }
            else
            {
                Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "创建支付记录失败.");
                return Json("no");
            }
        }
        /// <summary>
        /// 更新用户充值记录 更新用户的钱包
        /// </summary>
        /// <param name="orderid">充值单号</param>
        /// <returns></returns>
        public JsonResult UpdateUserRechargeInfo(string orderid)
        {
            Finance_Recharge frinfo = ServiceHelper.Create<IFinance_RechargeService>().GetFinance_RechargeInfo(long.Parse(orderid));
            if (frinfo == null)
            {
                Log.Error("在" + DateTime.Now + "充值钱包时,用户" + base.CurrentUser.Id + "获取充值信息失败.");
                return Json("");
            }
            else
            {
                Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
                if (fwinfo == null)
                {
                    Log.Error("在" + DateTime.Now + "充值钱包时,用户" + base.CurrentUser.Id + "获取钱包信息失败.");
                    return Json("");
                }
                else
                {
                    fwinfo.Wallet_UserLeftMoney = frinfo.Recharge_Money + fwinfo.Wallet_UserLeftMoney;
                    if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo))
                    {
                        Log.Info("在" + DateTime.Now + "充值钱包时,用户" + base.CurrentUser.Id + "成功更新了自己钱包的可用余额.");
                        frinfo.Recharge_Type = 1;
                        frinfo.Recharge_MoneyLeft = fwinfo.Wallet_UserLeftMoney;
                        if (ServiceHelper.Create<IFinance_RechargeService>().UpdateFinance_Recharge(frinfo))
                        {
                            Log.Error("在" + DateTime.Now + "充值钱包时,用户" + base.CurrentUser.Id + "成功更新了充值记录.");
                            return Json("ok");
                        }
                        else
                        {
                            Log.Error("在" + DateTime.Now + "充值钱包时,用户" + base.CurrentUser.Id + "更新充值记录失败.");
                            return Json("");
                        }
                    }
                    else
                    {
                        Log.Error("在" + DateTime.Now + "充值钱包时,用户" + base.CurrentUser.Id + "更新自己的钱包可用余额失败.");
                        return Json("");
                    }
                }
            }

        }

        /// <summary>
        /// 添加全额/分期支付记录操作完成更新个人钱包
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="price">实际支付的金额</param>
        /// <param name="isWallet">类型 1.是账户余额支付 2.第三方其它支付方式</param>
        /// <returns></returns>
        public JsonResult AddUserPayMentInfoPlat(string orderid, string price, string isWallet, string paytype)
        {
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            int m_type = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            Finance_Payment fp = new Finance_Payment();
            fp.PayMent_Number = _orderBO.GenerateOrderNumber();
            //待处理  
            fp.PayMent_UserId = base.CurrentUser.Id;
            fp.PayMent_UserType = base.CurrentUser.UserType;
            fp.PayMent_OrderNum = long.Parse(orderid);
            fp.PayMent_PayTime = DateTime.Now;
            fp.PayMent_PayMoney = decimal.Parse(price);
            if (paytype == "0")
            {
                OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));
                fp.PayMent_TotalMoney = oinfo.ProductTotalAmount;
                fp.PayMent_BXMoney = oinfo.Insurancefee;
                fp.PayMent_YFMoney = oinfo.Freight;
                fp.PayMent_JYMoney = oinfo.Transactionfee;
                fp.PayMent_SXMoney = oinfo.Counterfee;
            }
            else
            {
                fp.PayMent_TotalMoney = decimal.Parse(price);
                fp.PayMent_BXMoney = 0;
                fp.PayMent_YFMoney = 0;
                fp.PayMent_JYMoney = 0;
                fp.PayMent_SXMoney = 0;
            }
            fp.PayMent_PayAddress = ChemCloud.Core.Common.GetIpAddress();
            fp.PayMent_MoneyType = m_type;
            fp.PayMent_Status = 1;
            fp.PayMent_Type = int.Parse(paytype);
            if (ServiceHelper.Create<IFinance_PaymentService>().AddFinance_Payment(fp))
            {
                Log.Info("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "创建支付记录成功,支付单号：" + fp.PayMent_Number + ".");
                if (isWallet == "1")
                {
                    #region 如果是账户余额支付 更新个人的账户信息
                    Finance_Wallet fw = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, m_type);
                    if (fw != null)
                    {
                        if (fw.Wallet_UserLeftMoney < decimal.Parse(price))
                        {
                            Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "更新自己的账户时出错,出错信息：个人账户额小于订单应支付的金额.");
                            return Json("no");
                        }
                        else
                        {
                            fw.Wallet_DoIpAddress = ChemCloud.Core.Common.GetIpAddress();
                            fw.Wallet_DoLastTime = DateTime.Now;
                            fw.Wallet_DoUserId = base.CurrentUser.Id;
                            fw.Wallet_DoUserName = base.CurrentUser.UserName;
                            fw.Wallet_MoneyType = m_type;
                            fw.Wallet_UserLeftMoney = fw.Wallet_UserLeftMoney - decimal.Parse(price);
                            if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fw))
                            {
                                Log.Info("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "成功更新了自己的账户余额.");
                                return Json("ok");
                            }
                            else
                            {
                                Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "更新自己的账户时出错.");
                                return Json("no");
                            }
                        }
                    }
                    else
                    {
                        return Json("no");
                    }
                    #endregion
                }
                else
                {
                    return Json("ok");
                }
            }
            else
            {
                Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "创建支付记录失败.");
                return Json("no");
            }
        }

    }
}