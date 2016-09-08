using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChemCloud.DBUtility;
using System.Data;

namespace ChemCloud.Web.Controllers
{
    public class PublicMoneyController : Controller
    {
        public PublicMoneyController()
        {
        }

        /// <summary>
        /// 添加用户钱包
        /// </summary>
        /// <param name="uid">用户编号</param>
        /// <param name="utype">用户了类别</param>
        /// <param name="uname">用户姓名</param>
        /// <returns></returns>
        public JsonResult AddUserWallet(long uid, int utype, string uname)
        {
            List<Finance_Wallet> list = ServiceHelper.Create<IFinance_WalletService>().GetWalletList(uid, utype);
            List<ChemCloud_Dictionaries> cd = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(1);
            if (list.Count < 1)
            {
                if (uid != 0 && utype != 0 && !string.IsNullOrEmpty(uname))
                {
                    if (cd.Count > 0)
                    {
                        #region 循环添加用户币种账户
                        for (int i = 0; i < cd.Count; i++)
                        {
                            Finance_Wallet fw = new Finance_Wallet();
                            fw.Wallet_UserId = uid;
                            fw.Wallet_UserType = utype;
                            fw.Wallet_DoIpAddress = ChemCloud.Core.Common.GetIpAddress();
                            fw.Wallet_DoLastTime = DateTime.Now;
                            fw.Wallet_DoUserId = uid;
                            fw.Wallet_DoUserName = uname;
                            fw.Wallet_Status = 1;
                            fw.Wallet_MoneyType = int.Parse(cd[i].DValue);//读后台币种配置信息
                            if (ServiceHelper.Create<IFinance_WalletService>().AddFinance_Wallet(fw))
                            {
                                Log.Info("用户(" + uname + ")在IP(" + fw.Wallet_DoIpAddress + ")于" + fw.Wallet_DoLastTime + "创建钱包成功.");
                            }
                            else
                            {
                                Log.Error("用户(" + uname + ")在IP(" + fw.Wallet_DoIpAddress + ")于" + fw.Wallet_DoLastTime + "创建钱包时失败.");
                            }
                        }
                        return Json("");
                        #endregion
                    }
                    else
                    {
                        return Json("");
                    }
                }
                else
                {
                    return Json("");
                }
            }
            else
            {
                return Json("");
            }

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
                            Log.Info(DateTime.Now + "时,成功添加供应商编号" + uminfo.Id + "的收入信息,收入金额：" + (fi.InCome_Money) + ",收入币种：" + fi.InCome_MoneyType);
                            //更新供应商锁定金额
                            Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(uminfo.Id, uminfo.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
                            if (fwinfo == null)
                            {
                                return Json("");
                            }
                            fwinfo.Wallet_UserMoneyLock = fwinfo.Wallet_UserMoneyLock + (fi.InCome_Money);
                            if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo))
                            {
                                #region 保险费付给平台
                                Finance_InCome managerFi = new Finance_InCome()
                                {
                                    InCome_Number = _orderBO.GenerateOrderNumber(),
                                    InCome_UserId = -1,//平台
                                    InCome_UserType = 1,//平台
                                    InCome_StartTime = DateTime.Now,
                                    InCome_EndTime = DateTime.Now.AddDays(int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayoutTime"))),
                                    InCome_Money = bxfei,//保险费付给平台
                                    InCome_MoneyType = montype,
                                    InCome_OrderNum = long.Parse(orderId),
                                    InCome_Address = fi.InCome_Address,
                                    InCome_Type = 1,
                                    InCome_Status = 1
                                };
                                if (ServiceHelper.Create<IFinance_InComeService>().AddFinance_InCome(managerFi))
                                {
                                    Log.Info(DateTime.Now + "时,成功添加收入信息,收入金额：" + (fi.InCome_Money - bxfei) + ",收入币种：" + fi.InCome_MoneyType);
                                    //更新平台账户金额 如果保险费用大于0
                                    if (bxfei > 0)
                                    {
                                        Finance_Wallet pingtai = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(-1, 1, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
                                        if (pingtai == null)
                                        {
                                            Log.Info(DateTime.Now + "时,获取平台账号信息失败.");
                                            return Json("");
                                        }
                                        pingtai.Wallet_UserMoneyLock = pingtai.Wallet_UserMoneyLock + bxfei;
                                        if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(pingtai))
                                        {
                                            return Json("ok");
                                        }
                                        else
                                        {
                                            Log.Info(DateTime.Now + "时,更新平台账号信息错误.");
                                            return Json("");
                                        }
                                    }
                                    else
                                    {
                                        return Json("ok");
                                    }

                                }
                                else
                                {
                                    Log.Info(DateTime.Now + "时,平台账户的保险金收入信息失败.");
                                    return Json("");
                                }
                                #endregion
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
        [HttpPost]
        public JsonResult UpdateOrderStatus(long orderId, string status, string paymodel)
        {
            ServiceHelper.Create<IOrderService>().UpdateOrderStatuNew(orderId, int.Parse(status), paymodel);
            return Json("ok");
        }

        /// <summary>
        /// 获取支付列表信息
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">筛选开始时间</param>
        /// <param name="endtime">筛选结束时间</param>
        /// <param name="moneytype">筛选币种类型</param>
        /// <param name="userid">用户编号</param>
        /// <param name="usertype">用户类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListPay(int page, int rows, string starttime, string endtime, int moneytype, long userid, int usertype)
        {
            Finance_PaymentQuery fpq = new Finance_PaymentQuery();
            fpq.userid = userid;
            fpq.usertype = usertype;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Payment> fp = ServiceHelper.Create<IFinance_PaymentService>().GetFinance_PaymentListInfo(fpq);
            IEnumerable<Finance_Payment> models =
            from item in fp.Models.ToArray()
            select new Finance_Payment()
            {
                Id = item.Id,
                PayMent_Number = item.PayMent_Number,
                PayMent_UserId = item.PayMent_UserId,
                PayMent_UserType = item.PayMent_UserType,
                PayMent_OrderNum = item.PayMent_OrderNum,
                PayMent_PayTime = item.PayMent_PayTime,
                PayMent_PayMoney = item.PayMent_PayMoney,
                PayMent_TotalMoney = item.PayMent_TotalMoney,
                PayMent_BXMoney = item.PayMent_BXMoney,
                PayMent_YFMoney = item.PayMent_YFMoney,
                PayMent_JYMoney = item.PayMent_JYMoney,
                PayMent_SXMoney = item.PayMent_SXMoney,
                PayMent_PayAddress = item.PayMent_PayAddress,
                PayMent_MoneyType = item.PayMent_MoneyType,
                PayMent_Status = item.PayMent_Status,
                PayMent_Type = item.PayMent_Type
            };
            DataGridModel<Finance_Payment> dataGridModel = new DataGridModel<Finance_Payment>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 获取收入列表信息
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">筛选开始时间</param>
        /// <param name="endtime">筛选结束时间</param>
        /// <param name="moneytype">筛选币种类型</param>
        /// <param name="userid">用户编号</param>
        /// <param name="usertype">用户类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListInCome(int page, int rows, string starttime, string endtime, int moneytype, long userid, int usertype)
        {
            Finance_InComeQuery fpq = new Finance_InComeQuery();
            fpq.userid = userid;
            fpq.usertype = usertype;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_InCome> fp = ServiceHelper.Create<IFinance_InComeService>().GetFinance_InComeListInfo(fpq);
            IEnumerable<Finance_InCome> models =
            from item in fp.Models.ToArray()
            select new Finance_InCome()
            {

                Id = item.Id,
                InCome_Number = item.InCome_Number,
                InCome_UserId = item.InCome_UserId,
                InCome_UserType = item.InCome_UserType,
                InCome_StartTime = item.InCome_StartTime,
                InCome_EndTime = item.InCome_EndTime,
                InCome_Money = item.InCome_Money,
                InCome_MoneyType = item.InCome_MoneyType,
                InCome_OrderNum = item.InCome_OrderNum,
                InCome_Address = item.InCome_Address,
                InCome_Type = item.InCome_Type,
                InCome_Status = item.InCome_Status
            };
            DataGridModel<Finance_InCome> dataGridModel = new DataGridModel<Finance_InCome>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 获取充值列表信息
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">筛选开始时间</param>
        /// <param name="endtime">筛选结束时间</param>
        /// <param name="moneytype">筛选币种类型</param>
        /// <param name="userid">用户编号</param>
        /// <param name="usertype">用户类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListRecharge(int page, int rows, string starttime, string endtime, int moneytype, long userid, int usertype)
        {
            Finance_RechargeQuery fpq = new Finance_RechargeQuery();
            fpq.userid = userid;
            fpq.usertype = usertype;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Recharge> fp = ServiceHelper.Create<IFinance_RechargeService>().GetFinance_RechargeListInfo(fpq);
            IEnumerable<Finance_Recharge> models =
            from item in fp.Models.ToArray()
            select new Finance_Recharge()
            {
                Id = item.Id,
                Recharge_Number = item.Recharge_Number,
                Recharge_UserId = item.Recharge_UserId,
                Recharge_UserType = item.Recharge_UserType,
                Recharge_Time = item.Recharge_Time,
                Recharge_Address = item.Recharge_Address,
                Recharge_Money = item.Recharge_Money,
                Recharge_MoneyLeft = item.Recharge_MoneyLeft,
                Recharge_MoneyType = item.Recharge_MoneyType,
                Recharge_Type = item.Recharge_Type,
                Recharge_Status = item.Recharge_Status
            };
            DataGridModel<Finance_Recharge> dataGridModel = new DataGridModel<Finance_Recharge>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 获取提现列表信息
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">筛选开始时间</param>
        /// <param name="endtime">筛选结束时间</param>
        /// <param name="moneytype">筛选币种类型</param>
        /// <param name="userid">用户编号</param>
        /// <param name="usertype">用户类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListWithDraw(int page, int rows, string starttime, string endtime, int moneytype, long userid, int usertype)
        {
            Finance_WithDrawQuery fpq = new Finance_WithDrawQuery();
            fpq.userid = userid;
            fpq.usertype = usertype;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_WithDraw> fp = ServiceHelper.Create<IFinance_WithDrawService>().GetFinance_WithDrawListInfo(fpq);
            IEnumerable<Finance_WithDraw> models =
            from item in fp.Models.ToArray()
            select new Finance_WithDraw()
            {

                Id = item.Id,
                Withdraw_Number = item.Withdraw_Number,
                Withdraw_UserId = item.Withdraw_UserId,
                Withdraw_UserName = ServiceHelper.Create<IMemberService>().GetMember(item.Withdraw_UserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(item.Withdraw_UserId).UserName,
                Withdraw_UserType = item.Withdraw_UserType,
                Withdraw_Money = item.Withdraw_Money,
                Withdraw_MoneyType = item.Withdraw_MoneyType,
                Withdraw_Account = item.Withdraw_Account,
                Withdraw_BankName = item.Withdraw_BankName,
                Withdraw_BankUserName = item.Withdraw_BankUserName,
                Withdraw_Time = item.Withdraw_Time,
                Withdraw_Status = item.Withdraw_Status,
                Withdraw_shenhe = item.Withdraw_shenhe,
                Withdraw_shenheUid = item.Withdraw_shenheUid,
                Withdraw_shenheUname = item.Withdraw_shenheUname,
                Withdraw_shenheDesc = item.Withdraw_shenheDesc,
                Withdraw_shenheTime = item.Withdraw_shenheTime
            };
            DataGridModel<Finance_WithDraw> dataGridModel = new DataGridModel<Finance_WithDraw>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 获取采购商退款列表信息
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">筛选开始时间</param>
        /// <param name="endtime">筛选结束时间</param>
        /// <param name="moneytype">筛选币种类型</param>
        /// <param name="userid">用户编号</param>
        /// <param name="usertype">用户类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListMoneyRefund(int page, int rows, string starttime, string endtime, int moneytype, long userid, int usertype)
        {
            Finance_RefundQuery fpq = new Finance_RefundQuery();
            fpq.muserid = userid;
            fpq.musertype = usertype;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Refund> fp = ServiceHelper.Create<IFinance_RefundService>().GetFinance_RefundListInfo(fpq);
            IEnumerable<Finance_Refund> models =
            from item in fp.Models.ToArray()
            select new Finance_Refund()
            {
                Id = item.Id,
                Refund_Number = item.Refund_Number,
                Refund_OrderNum = item.Refund_OrderNum,
                Refund_UserId = item.Refund_UserId,
                Refund_UserType = item.Refund_UserType,
                Refund_UserName = item.Refund_UserName,
                Refund_Money = item.Refund_Money,
                Refund_MoneyType = item.Refund_MoneyType,
                Refund_SXMoney = item.Refund_SXMoney,
                Refund_ISChujing = item.Refund_ISChujing,
                Refund_Address = item.Refund_Address,
                Refund_Time = item.Refund_Time,
                Refund_Status = item.Refund_Status,
                Refund_ToUserId = item.Refund_ToUserId,
                Refund_ToUserType = item.Refund_ToUserType,
                Refund_ToUserName = item.Refund_ToUserName
            };
            DataGridModel<Finance_Refund> dataGridModel = new DataGridModel<Finance_Refund>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 获取供应商退款列表信息
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">筛选开始时间</param>
        /// <param name="endtime">筛选结束时间</param>
        /// <param name="moneytype">筛选币种类型</param>
        /// <param name="userid">用户编号</param>
        /// <param name="usertype">用户类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListMoneyRefundGYS(int page, int rows, string starttime, string endtime, int moneytype, long userid, int usertype)
        {
            Finance_RefundQuery fpq = new Finance_RefundQuery();
            fpq.userid = userid;
            fpq.usertype = usertype;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Refund> fp = ServiceHelper.Create<IFinance_RefundService>().GetFinance_RefundListInfo(fpq);
            IEnumerable<Finance_Refund> models =
            from item in fp.Models.ToArray()
            select new Finance_Refund()
            {
                Id = item.Id,
                Refund_Number = item.Refund_Number,
                Refund_OrderNum = item.Refund_OrderNum,
                Refund_UserId = item.Refund_UserId,
                Refund_UserType = item.Refund_UserType,
                Refund_UserName = item.Refund_UserName,
                Refund_Money = item.Refund_Money,
                Refund_MoneyType = item.Refund_MoneyType,
                Refund_SXMoney = item.Refund_SXMoney,
                Refund_ISChujing = item.Refund_ISChujing,
                Refund_Address = item.Refund_Address,
                Refund_Time = item.Refund_Time,
                Refund_Status = item.Refund_Status,
                Refund_ToUserId = item.Refund_ToUserId,
                Refund_ToUserType = item.Refund_ToUserType,
                Refund_ToUserName = item.Refund_ToUserName
            };
            DataGridModel<Finance_Refund> dataGridModel = new DataGridModel<Finance_Refund>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 获取转账列表信息
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">筛选开始时间</param>
        /// <param name="endtime">筛选结束时间</param>
        /// <param name="moneytype">筛选币种类型</param>
        /// <param name="userid">用户编号</param>
        /// <param name="usertype">用户类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListTrans(int page, int rows, string starttime, string endtime, int moneytype, long userid, int usertype)
        {
            Finance_TransferQuery fpq = new Finance_TransferQuery();
            fpq.userid = userid;
            fpq.usertype = usertype;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Transfer> fp = ServiceHelper.Create<IFinance_TransferService>().GetFinance_TransferListInfo(fpq);
            IEnumerable<Finance_Transfer> models =
            from item in fp.Models.ToArray()
            select new Finance_Transfer()
            {
                Id = item.Id,
                Trans_Number = item.Trans_Number,
                Trans_UserId = item.Trans_UserId,
                Trans_UserName = ServiceHelper.Create<IMemberService>().GetMember(item.Trans_UserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(item.Trans_UserId).UserName,
                Trans_UserType = item.Trans_UserType,
                Trans_Money = item.Trans_Money,
                Trans_SXMoney = item.Trans_SXMoney,
                Trans_MoneyType = item.Trans_MoneyType,
                Trans_Time = item.Trans_Time,
                Trans_Address = item.Trans_Address,
                Trans_ToUserId = item.Trans_ToUserId,
                Trans_ToUserName = ServiceHelper.Create<IMemberService>().GetMember(item.Trans_ToUserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(item.Trans_ToUserId).UserName,
                Trans_ToUserType = item.Trans_ToUserType,
                Trans_Status = item.Trans_Status
            };
            DataGridModel<Finance_Transfer> dataGridModel = new DataGridModel<Finance_Transfer>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 平台统计支付信息 [chenqi 2016-03-29 17:06]
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">筛选开始时间</param>
        /// <param name="endtime">筛选结束时间</param>
        /// <param name="moneytype">筛选币种类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListPayStatistics(int page, int rows, string starttime, string endtime, int moneytype)
        {
            Finance_PaymentQuery fpq = new Finance_PaymentQuery();
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Payment> fp = ServiceHelper.Create<IFinance_PaymentService>().GetFinance_PaymentList_Statistics(fpq);
            IEnumerable<Finance_Payment> models =
            from item in fp.Models.ToArray()
            select new Finance_Payment()
            {

                Id = item.Id,
                PayMent_Number = item.PayMent_Number,
                PayMent_UserId = item.PayMent_UserId,
                PayMent_UserType = item.PayMent_UserType,
                PayMent_OrderNum = item.PayMent_OrderNum,
                PayMent_PayTime = item.PayMent_PayTime,
                PayMent_PayMoney = item.PayMent_PayMoney,
                PayMent_TotalMoney = item.PayMent_TotalMoney,
                PayMent_BXMoney = item.PayMent_BXMoney,
                PayMent_YFMoney = item.PayMent_YFMoney,
                PayMent_JYMoney = item.PayMent_JYMoney,
                PayMent_SXMoney = item.PayMent_SXMoney,
                PayMent_PayAddress = item.PayMent_PayAddress,
                PayMent_MoneyType = item.PayMent_MoneyType,
                PayMent_Status = item.PayMent_Status
            };
            DataGridModel<Finance_Payment> dataGridModel = new DataGridModel<Finance_Payment>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 平台统计收入信息 [chenqi 2016-03-30 10：00]
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="moneytype"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListInComeStatistics(int page, int rows, string starttime, string endtime, int moneytype)
        {
            Finance_InComeQuery fpq = new Finance_InComeQuery();
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_InCome> fp = ServiceHelper.Create<IFinance_InComeService>().GetFinance_InComeList_Statistics(fpq);
            IEnumerable<Finance_InCome> models =
            from item in fp.Models.ToArray()
            select new Finance_InCome()
            {

                Id = item.Id,
                InCome_Number = item.InCome_Number,
                InCome_UserId = item.InCome_UserId,
                InCome_UserType = item.InCome_UserType,
                InCome_StartTime = item.InCome_StartTime,
                InCome_EndTime = item.InCome_EndTime,
                InCome_Money = item.InCome_Money,
                InCome_MoneyType = item.InCome_MoneyType,
                InCome_OrderNum = item.InCome_OrderNum,
                InCome_Address = item.InCome_Address,
                InCome_Type = item.InCome_Type,
                InCome_Status = item.InCome_Status
            };
            DataGridModel<Finance_InCome> dataGridModel = new DataGridModel<Finance_InCome>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        ///  平台统计提现信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="moneytype"></param>
        /// <param name="userid"></param>
        /// <param name="usertype"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListWithDrawStatistics(int page, int rows, string starttime, string endtime, int moneytype)
        {
            Finance_WithDrawQuery fpq = new Finance_WithDrawQuery();
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_WithDraw> fp = ServiceHelper.Create<IFinance_WithDrawService>().GetFinance_WithDrawList_Statistics(fpq);
            IEnumerable<Finance_WithDraw> models =
            from item in fp.Models.ToArray()
            select new Finance_WithDraw()
            {

                Id = item.Id,
                Withdraw_Number = item.Withdraw_Number,
                Withdraw_UserId = item.Withdraw_UserId,
                Withdraw_UserType = item.Withdraw_UserType,
                Withdraw_Money = item.Withdraw_Money,
                Withdraw_MoneyType = item.Withdraw_MoneyType,
                Withdraw_Account = item.Withdraw_Account,
                Withdraw_Time = item.Withdraw_Time,
                Withdraw_Status = item.Withdraw_Status,
                Withdraw_shenhe = item.Withdraw_shenhe,
                Withdraw_shenheUid = item.Withdraw_shenheUid,
                Withdraw_shenheUname = item.Withdraw_shenheUname,
                Withdraw_shenheDesc = item.Withdraw_shenheDesc,
                Withdraw_shenheTime = item.Withdraw_shenheTime
            };
            DataGridModel<Finance_WithDraw> dataGridModel = new DataGridModel<Finance_WithDraw>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }

        /// <summary>
        /// 平台统计退款信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="moneytype"></param>
        /// <param name="userid"></param>
        /// <param name="usertype"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListMoneyRefundStatistics(int page, int rows, string starttime, string endtime, int moneytype)
        {
            Finance_RefundQuery fpq = new Finance_RefundQuery();
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Refund> fp = ServiceHelper.Create<IFinance_RefundService>().GetFinance_RefundList_Statistics(fpq);
            IEnumerable<Finance_Refund> models =
            from item in fp.Models.ToArray()
            select new Finance_Refund()
            {
                Id = item.Id,
                Refund_Number = item.Refund_Number,
                Refund_OrderNum = item.Refund_OrderNum,
                Refund_UserId = item.Refund_UserId,
                Refund_UserType = item.Refund_UserType,
                Refund_Money = item.Refund_Money,
                Refund_MoneyType = item.Refund_MoneyType,
                Refund_SXMoney = item.Refund_SXMoney,
                Refund_ISChujing = item.Refund_ISChujing,
                Refund_Address = item.Refund_Address,
                Refund_Time = item.Refund_Time,
                Refund_Status = item.Refund_Status
            };
            DataGridModel<Finance_Refund> dataGridModel = new DataGridModel<Finance_Refund>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }


        [HttpPost]
        public JsonResult ListTransStatistics(int page, int rows, string starttime, string endtime, int moneytype)
        {
            Finance_TransferQuery fpq = new Finance_TransferQuery();
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.moneytype = moneytype;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Transfer> fp = ServiceHelper.Create<IFinance_TransferService>().GetFinance_TransferList_Statistics(fpq);
            IEnumerable<Finance_Transfer> models =
            from item in fp.Models.ToArray()
            select new Finance_Transfer()
            {
                Id = item.Id,
                Trans_Number = item.Trans_Number,
                Trans_UserId = item.Trans_UserId,
                Trans_UserType = item.Trans_UserType,
                Trans_Money = item.Trans_Money,
                Trans_SXMoney = item.Trans_SXMoney,
                Trans_MoneyType = item.Trans_MoneyType,
                Trans_Time = item.Trans_Time,
                Trans_Address = item.Trans_Address,
                Trans_ToUserId = item.Trans_ToUserId,
                Trans_ToUserType = item.Trans_ToUserType,
                Trans_Status = item.Trans_Status
            };
            DataGridModel<Finance_Transfer> dataGridModel = new DataGridModel<Finance_Transfer>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
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

            /*我要采购 支付*/
            if (paytype == "7")
            {
                //orderId

                string strsql = string.Format("SELECT SupplierID FROM dbo.ChemCloud_IWantToSupply where Id='" + orderId + "';");
                DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SupplierID"] != null)
                    {
                        userid = long.Parse(dt.Rows[0]["SupplierID"].ToString());
                        usertype = 2;
                    }
                }

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
                    return Json("");
                }
            }
            else
            {

                #region 平台的收入记录
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

        }

        /// <summary>
        /// 更新其他支付的支付状态
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePayToPlatformStatus(string paytype, string targetid)
        {
            ServiceHelper.Create<IOrderService>().UpdatePayToPlatformStatus(paytype, targetid);
            return Json("ok");
        }
    }
}