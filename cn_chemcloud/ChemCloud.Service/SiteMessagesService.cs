using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class SiteMessagesService : ServiceBase, ISiteMessagesService, IService, IDisposable
    {
        public SiteMessagesService()
        {

        }
        /// <summary>
        /// 发送订单新增消息
        /// </summary>
        /// <param name="UserId">消息接收人ID</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="OrderCreatTime">订单日期</param>
        /// <param name="SendUserName">消息发送人</param>
        /// <returns></returns>
        public bool SendOrderCreatedMessage(long UserId, DateTime OrderCreatTime, string OrderNo)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 1 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName).Replace("#OrderNo", OrderNo).Replace("#OrderCreatTime", OrderCreatTime.ToString());
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 1,
                    MessageTitle = MessageSetting.MessageModuleStatus.OrderCreated.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 发送订单支付消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OrderNo"></param>
        /// <param name="SendUserName"></param>
        /// <param name="OrderMoney"></param>
        /// <returns></returns>
        public bool SendOrderPayMessage(long UserId, string OrderNo, decimal OrderMoney)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 2 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName).Replace("#OrderNo", OrderNo).Replace("#OrdePayTime", DateTime.Now.ToString()).Replace("#OrderMoney", OrderMoney.ToString());
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 2,
                    MessageTitle = MessageSetting.MessageModuleStatus.OrderPay.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 发送订单发货消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OrderNo"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendOrderShippingMessage(long UserId, string OrderNo)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 3 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName).Replace("#OrderNo", OrderNo);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 3,
                    MessageTitle = MessageSetting.MessageModuleStatus.OrderShipping.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 订单退款消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OrderNo"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendOrderRefundMessage(long UserId, string OrderNo, string SendUserName)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 4 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName).Replace("#OrderNo", OrderNo);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 4,
                    MessageTitle = MessageSetting.MessageModuleStatus.OrderRefund.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 供应商审核消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendShopAuditedMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 6 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 6,
                    MessageTitle = MessageSetting.MessageModuleStatus.ShopAudited.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 供应商审核结果
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendShopResultMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 7 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 7,
                    MessageTitle = MessageSetting.MessageModuleStatus.ShopResult.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 接收实地认证申请
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendCertificationApplyMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 8 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 8,
                    MessageTitle = MessageSetting.MessageModuleStatus.CertificationApply.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 确认收到实地认证款
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendConfirmPayMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 9 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 9,
                    MessageTitle = MessageSetting.MessageModuleStatus.ConfirmPay.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 实地认证结果
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendCertificationResultMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 10 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 10,
                    MessageTitle = MessageSetting.MessageModuleStatus.CertificationResult.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 物流收货
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OrderNo"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendLogisticsReceMessage(long UserId, string OrderNo)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 11 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName).Replace("#OrderNo", OrderNo);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 11,
                    MessageTitle = MessageSetting.MessageModuleStatus.LogisticsRece.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 物流通关
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OrderNo"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendLogisticsClearanceMessage(long UserId, string OrderNo)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 12 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName).Replace("#OrderNo", OrderNo);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 12,
                    MessageTitle = MessageSetting.MessageModuleStatus.LogisticsClearance.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 采购商签收
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OrderNo"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendLogisticsSignInMessage(long UserId, string OrderNo)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 13 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName).Replace("#OrderNo", OrderNo);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 13,
                    MessageTitle = MessageSetting.MessageModuleStatus.LogisticsSignIn.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 采购商注册
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendMemberRegisterMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 14 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 14,
                    MessageTitle = MessageSetting.MessageModuleStatus.MemberRegister.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 3
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        ///供应商注册
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendSupplierRegisterMessage(long UserId)
        {
            string str = "";
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 15 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                if (UserInfo != null)
                {
                    str = MessageContent.Replace("#userName", UserInfo.UserName);
                }
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 15,
                    MessageTitle = MessageSetting.MessageModuleStatus.SupplierRegister.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
         
            return true;
        }
        /// <summary>
        /// 货款到账
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendGoodsPaymentMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 16 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 16,
                    MessageTitle = MessageSetting.MessageModuleStatus.GoodsPayment.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// <summary>
        /// 询盘
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendXunPanMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 19 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 2);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 19,
                    MessageTitle = MessageSetting.MessageModuleStatus.GoodsPayment.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// 限额审核
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendLimitedAmountMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 21 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 21,
                    MessageTitle = MessageSetting.MessageModuleStatus.LimitedAount.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    //采购商是3
                    SendObj = 3
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// 限额审核通过
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendApplyPassMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 22 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 22,
                    MessageTitle = MessageSetting.MessageModuleStatus.ApplyPass.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        /// 限额审核未通过
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SendUserName"></param>
        /// <returns></returns>
        public bool SendApplyNoPassMessage(long UserId)
        {
            string MessageContent = "";
            int Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == 23 && m.Languagetype == Languagetype && m.ActiveStatus == 1);
            if (message != null)
            {
                MessageContent = message.MessageContent;
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == UserId && m.UserType == 3);
                string str = MessageContent.Replace("#userName", UserInfo.UserName);
                MessageDetial md = new MessageDetial()
                {
                    ManagerId = 2,
                    MessageTitleId = 23,
                    MessageTitle = MessageSetting.MessageModuleStatus.ApplyNoPass.ToDescription(),
                    MessageContent = str,
                    LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    MsgType = 2,
                    SendTime = DateTime.Now,
                    SendObj = 2
                };
                SendMessage(md, UserId, null);
            }
            return true;
        }
        public IQueryable<QueryMember> GetMangers(int receType,string Search)
        {
            IQueryable<QueryMember> UserInfo = null;
            if (receType == 1)
            {
               UserInfo = from item in context.UserMemberInfo
                                      where item.UserType == 3 && item.UserName.Contains(Search)
                                      select new QueryMember { Id = item.Id, Username = item.UserName };
                    

            }
            if (receType == 2)
            {
                UserInfo = from item in context.UserMemberInfo
                           where item.UserType == 2 && item.UserName.Contains(Search)
                           select new QueryMember { Id = item.Id, Username = item.UserName };
                    
                
            }
            if (receType == 3)
            {
                UserInfo =from item in context.UserMemberInfo
                          where item.UserType == 3 && item.UserName.Contains(Search)
                    select new QueryMember { Id = item.Id, Username = item.UserName };

            }
            return UserInfo;
        }
        public List<long> GetMemberIds(int receType)
        {
            List<long> MemberIds = new List<long>();
            
        //所有人
            if (receType == 3)
            {
                IQueryable<UserMemberInfo> UserInfo = context.UserMemberInfo;
                if (UserInfo != null)
                {
                    foreach (UserMemberInfo list in UserInfo.ToList())
                    {
                        MemberIds.Add(list.Id);
                    }
                }
            }
            //所有采购商
            else if (receType == 4)
            {
                IQueryable<UserMemberInfo> UserInfo = context.UserMemberInfo.FindBy<UserMemberInfo>((UserMemberInfo m) => m.UserType == 2);
                if (UserInfo != null)
                {
                    foreach (UserMemberInfo list in UserInfo.ToList())
                    {
                        MemberIds.Add(list.Id);
                    }
                }
            }
                //所有采购商
            else if (receType == 5)
            {
                IQueryable<UserMemberInfo> UserInfo = context.UserMemberInfo.FindBy<UserMemberInfo>((UserMemberInfo m) => m.UserType == 3);
                if (UserInfo != null)
                {
                    foreach (UserMemberInfo list in UserInfo.ToList())
                    {
                        MemberIds.Add(list.Id);
                    }
                }
            }
            return MemberIds;
        }
        public PageModel<SiteMessages> PlatformGetMessages(SiteMessagesQuery messageQueryModel)
        {
            IQueryable<SiteMessages> sitemessagelist = from item in base.context.SiteMessages
                                                       select item;
            if (messageQueryModel.MemberId.HasValue)
            {
                sitemessagelist = from item in sitemessagelist
                                  where item.MemberId == messageQueryModel.MemberId
                                  select item;
            }
            if ((messageQueryModel.Status.HasValue) && (messageQueryModel.Status.Value != 0))
            {
                sitemessagelist = from item in sitemessagelist
                                  where item.ReadStatus == messageQueryModel.Status
                                  select item;
            }
            if ((messageQueryModel.ReceStatus.HasValue) && (messageQueryModel.ReceStatus.Value != 0))
            {
                sitemessagelist = from item in sitemessagelist
                                  where (int)item.ReceType == (int)messageQueryModel.ReceStatus
                                  select item;
            }
            if ((messageQueryModel.MessageModule.HasValue) && ((int)messageQueryModel.MessageModule.Value == 20))
            {
                sitemessagelist = from item in sitemessagelist
                                  where (int)item.MessageModule == (int)messageQueryModel.MessageModule
                                  select item;
            }
            int num = sitemessagelist.Count();
            sitemessagelist = sitemessagelist.GetPage(out num, messageQueryModel.PageNo, messageQueryModel.PageSize);
            foreach (SiteMessages list in sitemessagelist.ToList())
            {
                if (list.ReceType.ToDescription().Equals("采购商"))
                {
                    UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.MemberId && m.UserType == 3);
                    list.ReceiveName = (memberInfo == null ? "" : memberInfo.UserName);
                }
                else if (list.ReceType.ToDescription().Equals("供应商"))
                {
                    UserMemberInfo Info = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.MemberId && m.UserType == 2);
                    list.ReceiveName = (Info == null ? "" : Info.UserName);
                }
                else if (list.ReceType.ToDescription().Equals("平台运营"))
                {
                    ManagerInfo Info = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == list.MemberId && m.ShopId == 0);
                    list.ReceiveName = (Info == null ? "" : Info.UserName);
                }
            }
            return new PageModel<SiteMessages>()
            {
                Models = sitemessagelist,
                Total = num
            };
        }
        /// <summary>
        /// 获取多个消息
        /// </summary>
        /// <param name="messageQueryModel"></param>
        /// <returns></returns>
        public PageModel<SiteMessages> GetMessages(SiteMessagesQuery messageQueryModel)
        {
            IQueryable<SiteMessages> sitemessagelist = from item in base.context.SiteMessages
                                                       select item;
            if (messageQueryModel.MemberId.HasValue)
            {
                sitemessagelist = from item in sitemessagelist
                                  where item.MemberId == messageQueryModel.MemberId
                                  select item;
            }
            if ((messageQueryModel.Status.HasValue) && (messageQueryModel.Status.Value != 0))
            {
                sitemessagelist = from item in sitemessagelist
                                  where item.ReadStatus == messageQueryModel.Status
                                  select item;
            }
            if ((messageQueryModel.ReceStatus.HasValue) && (messageQueryModel.ReceStatus.Value != 0))
            {
                sitemessagelist = from item in sitemessagelist
                                  where (int)item.ReceType == (int)messageQueryModel.ReceStatus
                                  select item;
            }
            if ((messageQueryModel.MessageModule.HasValue) && (messageQueryModel.MessageModule.Value != 0))
            {
                sitemessagelist = from item in sitemessagelist
                                  where (int)item.MessageModule == (int)messageQueryModel.MessageModule
                                  select item;
            }
            int num = sitemessagelist.Count();
            sitemessagelist = sitemessagelist.GetPage(out num, messageQueryModel.PageNo, messageQueryModel.PageSize);
            foreach (SiteMessages list in sitemessagelist.ToList())
            {
                if (list.ReceType.ToDescription().Equals("采购商"))
                {
                    UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.MemberId && m.UserType == 3);
                    list.ReceiveName = (memberInfo == null ? "" : memberInfo.UserName);
                }
                else if (list.ReceType.ToDescription().Equals("供应商"))
                {
                    UserMemberInfo Info = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.MemberId && m.UserType == 2);
                    list.ReceiveName = (Info == null ? "" : Info.UserName);
                }
                else if (list.ReceType.ToDescription().Equals("平台运营"))
                {
                    ManagerInfo Info = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == list.MemberId && m.ShopId == 0);
                    list.ReceiveName = (Info == null ? "" : Info.UserName);
                }
            }
            return new PageModel<SiteMessages>()
            {
                Models = sitemessagelist,
                Total = num
            };
        }
        public SiteMessages GetMessage(long Id)
        {
            SiteMessages Message = context.SiteMessages.FindById<SiteMessages>(Id);
            if (Message.ReceType.ToDescription().Equals("采购商"))
            {
                UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Message.MemberId && m.UserType == 3);
                Message.ReceiveName = (memberInfo == null ? "" : memberInfo.UserName);
            }
            else if (Message.ReceType.ToDescription().Equals("供应商"))
            {
                UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Message.MemberId && m.UserType == 2);
                Message.ReceiveName = (memberInfo == null ? "" : memberInfo.UserName);
            }
            else if (Message.ReceType.ToDescription().Equals("平台运营"))
            {
                ManagerInfo memberInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == Message.MemberId && m.ShopId == 0);
                Message.ReceiveName = (memberInfo == null ? "" : memberInfo.UserName);
            }
            return Message;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="setting"></param>
        /// <param name="Status"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool AddMessage(SiteMessages siteMessage)
        {
            context.SiteMessages.Add(siteMessage);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public void SendMessage(MessageDetial md, long userId, string[] urls)
        {
            context.MessageDetial.Add(md);
            context.SaveChanges();
            List<MessageRevice> mrList = new List<MessageRevice>();
            MessageRevice mr = new MessageRevice();
            mr.UserId = userId;
            mr.ReadFlag = 1;
            mr.VisiableFlag = 0;
            mr.MsgId = md.Id;
            mr.SendTime = md.SendTime;
            mr.IsShow = 0;
            mrList.Add(mr);
            List<MessageEnclosure> meList = new List<MessageEnclosure>();
            if (urls != null)
            {
                foreach (string item in urls)
                {
                    MessageEnclosure me = new MessageEnclosure();
                    me.MsgId = md.Id;
                    me.Url = item;
                    meList.Add(me);
                }
                context.MessageEnclosure.AddRange(meList);
            }
            context.MessageRevice.AddRange(mrList);
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {

                throw;
            }

        }
        /// <summary>
        /// 更新阅读状态
        /// </summary>
        /// <param name="Id">消息ID</param>
        /// <param name="status">更新状态</param>
        public bool UpdateStatus(long Id, SiteMessages.Status status)
        {
            SiteMessages nullable = context.SiteMessages.FindById<SiteMessages>(Id);
            nullable.ReadStatus = status;
            nullable.ReadTime = DateTime.Now;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public void UpdateMessage(SiteMessages model)
        {
            SiteMessages nullable = context.SiteMessages.FindById<SiteMessages>(model.Id);
            nullable.ReadStatus = SiteMessages.Status.Readed;
            nullable.IsDisplay = 1;
            nullable.ReadTime = DateTime.Now;
            context.SaveChanges();

        }

        /// <summary>
        /// 获取消息内容
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<SiteMessages> GetMessage(long? UserId)
        {
            List<SiteMessages> siteMessageList = new List<SiteMessages>();
            IQueryable<SiteMessages> gradeId = from item in base.context.SiteMessages
                                               where item.MemberId == UserId && (int)item.ReadStatus == 1
                                               select item;
            foreach (SiteMessages list in gradeId.ToList())
            {
                if (list.ReceType.ToDescription().Equals("采购商"))
                {
                    UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.MemberId && m.UserType == 3);
                    list.ReceiveName = (memberInfo == null ? "" : memberInfo.UserName);
                }
                else if (list.ReceType.ToDescription().Equals("供应商"))
                {
                    ManagerInfo Info = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.ShopId == list.MemberId && m.ShopId != 0);
                    list.ReceiveName = (Info == null ? "" : Info.UserName);
                }
                else if (list.ReceType.ToDescription().Equals("平台运营"))
                {
                    ManagerInfo Info = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.ShopId == list.MemberId && m.ShopId == 0);
                    list.ReceiveName = (Info == null ? "" : Info.UserName);
                }
                siteMessageList.Add(list);
            }
            return siteMessageList;
        }

        public bool SendSiteMessages(long Receid, int MessageModule, string MessageContent, string SendName)
        {
            MemberService MemberService = new Service.MemberService();
            int UserType = MemberService.GetMember(Receid) == null ? 2 : MemberService.GetMember(Receid).UserType;
            SiteMessages model = new SiteMessages();
            model.MemberId = Receid;
            model.ReceType = (SiteMessages.ReceiveType)UserType;
            model.SendTime = DateTime.Now;
            model.ReadStatus = SiteMessages.Status.UnRead;
            model.IsDisplay = 0;
            model.MessageModule = (MessageSetting.MessageModuleStatus)MessageModule;
            model.MessageContent = MessageContent;
            model.SendName = SendName;
            context.SiteMessages.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public int GetMessageCount(long MemberId, int TypeId)
        {
            int count = 0;
            IQueryable<MessageRevice> sitemessages = context.MessageRevice.FindBy((MessageRevice m) => m.UserId == MemberId && (int)m.ReadFlag == 1 && m.IsShow == 0);
            count = sitemessages.ToList().Count;
            return count;
        }

        public int GetMessageCount1(long MemberId, int TypeId)
        {
            int count = 0;
            IQueryable<MessageRevice> sitemessages = context.MessageRevice.FindBy((MessageRevice m) => m.UserId == MemberId && (int)m.ReadFlag == 1 && m.IsShow == 0);
            count = sitemessages.ToList().Count;
            return count;
        }
        public bool DeleteMessage(long MessageId)
        {
            SiteMessages siteMessage = context.SiteMessages.FindBy((SiteMessages item) => item.Id == MessageId).FirstOrDefault();
            context.SiteMessages.Remove(siteMessage);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool BatchDeleteMessages(long[] MessageIds)
        {
            IQueryable<SiteMessages> siteMessageInfo = context.SiteMessages.FindBy((SiteMessages item) => MessageIds.Contains(item.Id));
            context.SiteMessages.RemoveRange(siteMessageInfo);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public void UpdateIsDisplay(long MemberId, int TypeId)
        {
            List<long> DisplayIdsList = new List<long>();
            IQueryable<MessageRevice> sitemessages = context.MessageRevice.FindBy((MessageRevice m) => m.UserId == MemberId && (int)m.ReadFlag == 1 && m.IsShow == 0);
            if (sitemessages != null)
            {
                foreach (MessageRevice item in sitemessages.ToList())
                {
                    DisplayIdsList.Add(item.Id);
                }
                foreach (long id in DisplayIdsList)
                {
                    MessageRevice siteMessages = context.MessageRevice.FindBy((MessageRevice m) => m.Id == id).FirstOrDefault();
                    siteMessages.IsShow = 1;
                    context.SaveChanges();
                }
            }

        }
    }
}
