using Himall.Core;
using Himall.Core.Helper;
using Himall.Core.Plugins;
using Himall.Core.Plugins.Message;
using Himall.MessagePlugin;
using Himall.Plugin.Message.SiteMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace HiMall.Message.SiteMessage
{
    public class Service : IEmailPlugin, IMessagePlugin, IPlugin
    {
        private MessageStatus messageStatus;

        private Dictionary<MessageTypeEnum, StatusEnum> dic = new Dictionary<MessageTypeEnum, StatusEnum>();

        public bool EnableLog
        {
            get
            {
                return false;
            }
        }

        public bool IsSettingsValid
        {
            get
            {
                bool flag;
                try
                {
                    flag = true;
                }
                catch
                {
                    flag = false;
                }
                return flag;
            }
        }

        public string Logo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SiteMessageCore.WorkDirectory))
                {
                    throw new MissingFieldException("没有设置插件工作目录");
                }
                return string.Concat(SiteMessageCore.WorkDirectory, "/Data/logo.png");
            }
        }

        public string ShortName
        {
            get
            {
                return "邮箱";
            }
        }

        public string WorkDirectory
        {
            set
            {
                SiteMessageCore.WorkDirectory = value;
            }
        }

        public Service()
        {
            if (!string.IsNullOrEmpty(SiteMessageCore.WorkDirectory))
            {
                InitMessageStatus();
            }
        }

        

        public bool CheckDestination(string destination)
        {
            return true;
        }

        public void Disable(MessageTypeEnum e)
        {
            if ((
                from a in dic
                where a.Key == e
                select a).FirstOrDefault().Value == (StatusEnum)3)
            {
                throw new HimallException("该功能已被禁止，不能进行设置");
            }
            SetMessageStatus(e, (StatusEnum)2);
            FileStream fileStream = new FileStream(string.Concat(SiteMessageCore.WorkDirectory, "/Data/config.xml"), FileMode.Create);
            try
            {
                (new XmlSerializer(typeof(MessageStatus))).Serialize(fileStream, messageStatus);
            }
            finally
            {
                if (fileStream != null)
                {
                    ((IDisposable)fileStream).Dispose();
                }
            }
        }

        public void Enable(MessageTypeEnum e)
        {
            if ((
                from a in dic
                where a.Key == e
                select a).FirstOrDefault().Value == (StatusEnum)3)
            {
                throw new HimallException("该功能已被禁止，不能进行设置");
            }
            SetMessageStatus(e, (StatusEnum)1);
            FileStream fileStream = new FileStream(string.Concat(SiteMessageCore.WorkDirectory, "/Data/config.xml"), FileMode.Create);
            try
            {
                (new XmlSerializer(typeof(MessageStatus))).Serialize(fileStream, messageStatus);
            }
            finally
            {
                if (fileStream != null)
                {
                    ((IDisposable)fileStream).Dispose();
                }
            }
        }

        public Dictionary<MessageTypeEnum, StatusEnum> GetAllStatus()
        {
            InitMessageStatus();
            return dic;
        }
        public StatusEnum GetStatus(MessageTypeEnum e)
        {
            InitMessageStatus();
            KeyValuePair<MessageTypeEnum, StatusEnum> keyValuePair = dic.FirstOrDefault((KeyValuePair<MessageTypeEnum, StatusEnum> a) => a.Key == e);
            return keyValuePair.Value;
        }
        private void InitMessageStatus()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(SiteMessageCore.WorkDirectory);
            FileInfo fileInfo = directoryInfo.GetFiles("Data/config.xml").FirstOrDefault();
            if (fileInfo != null)
            {
                FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open);
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(MessageStatus));
                    messageStatus = (MessageStatus)xmlSerializer.Deserialize(fileStream);
                    dic.Clear();
                    dic.Add((MessageTypeEnum)5, (StatusEnum)messageStatus.FindPassWord);
                    dic.Add((MessageTypeEnum)1, (StatusEnum)messageStatus.OrderCreated);
                    dic.Add((MessageTypeEnum)2, (StatusEnum)messageStatus.OrderPay);
                    dic.Add((MessageTypeEnum)4, (StatusEnum)messageStatus.OrderRefund);
                    dic.Add((MessageTypeEnum)3, (StatusEnum)messageStatus.OrderShipping);
                    dic.Add((MessageTypeEnum)6, (StatusEnum)messageStatus.ShopAudited);
                    dic.Add((MessageTypeEnum)7, (StatusEnum)messageStatus.ShopSuccess);
                    dic.Add((MessageTypeEnum)8, (StatusEnum)messageStatus.ReceApply);
                    dic.Add((MessageTypeEnum)9, (StatusEnum)messageStatus.StartAuthentic);
                    dic.Add((MessageTypeEnum)10, (StatusEnum)messageStatus.FinishAuthentic);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        ((IDisposable)fileStream).Dispose();
                    }
                }
            }
        }

        private void SendMessage(string[] destination, string title, string body)
        {
            if (destination.Length > 0)
            {
                (new SendMail()).SendEmail(title, destination, body, true);
            }
        }

        private void SendMessage(string destination, string title, string body, bool async = false)
        {
            if (!string.IsNullOrWhiteSpace(destination))
            {
                SendMail sendMail = new SendMail();
                string[] strArrays = new string[] { destination };
                sendMail.SendEmail(title, strArrays, body, async);
            }
        }

        public string SendMessageCode(string destination, MessageUserInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.Bind.Replace("#userName#", info.UserName).Replace("#checkCode#", info.CheckCode).Replace("#siteName#", info.SiteName);
            SendMessage(destination, string.Concat(info.SiteName, "邮箱验证码"), str, false);
            return str;
        }

        public string SendMessageOnFindPassWord(string destination, MessageUserInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.FindPassWord.Replace("#userName#", info.UserName).Replace("#checkCode#", info.CheckCode).Replace("#siteName#", info.SiteName);
            SendMessage(destination, string.Concat(info.SiteName, "找回密码验证"), str, false);
            return str;
        }

        public string SendMessageOnOrderCreate(string destination, MessageOrderInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.OrderCreated.Replace("#userName#", info.UserName).Replace("#orderId#", info.OrderId).Replace("#siteName#", info.SiteName);
            SendMessage(destination, string.Concat(info.SiteName, "订单创建成功"), str, false);
            return str;
        }

        public string SendMessageOnOrderPay(string destination, MessageOrderInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.OrderPay.Replace("#userName#", info.UserName).Replace("#orderId#", info.OrderId).Replace("#siteName#", info.SiteName);
            decimal totalMoney = info.TotalMoney;
            string str1 = str.Replace("#Total#", totalMoney.ToString("F2"));
            SendMessage(destination, string.Concat(info.SiteName, "订单支付成功"), str1, false);
            return str1;
        }

        public string SendMessageOnOrderRefund(string destination, MessageOrderInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.OrderRefund.Replace("#userName#", info.UserName).Replace("#orderId#", info.OrderId).Replace("#siteName#", info.SiteName);
            decimal refundMoney = info.RefundMoney;
            string str1 = str.Replace("#RefundMoney#", refundMoney.ToString("F2"));
            SendMessage(destination, string.Concat(info.SiteName, "订单(", info.OrderId, ")退款已处理"), str1, false);
            return str1;
        }

        public string SendMessageOnOrderShipping(string destination, MessageOrderInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.OrderShipping.Replace("#userName#", info.UserName).Replace("#orderId#", info.OrderId).Replace("#siteName#", info.SiteName).Replace("#shippingCompany#", info.ShippingCompany).Replace("#shippingNumber", info.ShippingNumber);
            SendMessage(destination, string.Concat(info.SiteName, "订单(", info.OrderId, ")已发货"), str, false);
            return str;
        }

        public string SendMessageOnShopAudited(string destination, MessageShopInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.ShopAudited.Replace("#userName#", info.UserName).Replace("#companyName#", info.CompanyName).Replace("#siteName#", info.SiteName);
            SendMessage(destination, string.Concat(info.SiteName, "供应商(", info.CompanyName, ")已成功提交审核"), str, false);
            return str;
        }

        public string SendMessageOnShopSuccess(string destination, MessageShopInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.ShopSuccess.Replace("#userName#", info.UserName).Replace("#companyName#", info.CompanyName).Replace("#siteName#", info.SiteName);
            SendMessage(destination, string.Concat(info.SiteName, "供应商(", info.CompanyName, ")审核已完成"), str, false);
            return str;
        }
        public string SendMessageOnReceApply(string destination, MessageShopInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.ShopSuccess.Replace("#userName#", info.UserName).Replace("#companyName#", info.CompanyName).Replace("#siteName#", info.SiteName);
            SendMessage(destination, string.Concat(info.SiteName, "供应商(", info.CompanyName, ")实地认证申请已接受"), str, false);
            return str;
        }
        public string SendMessageOnStartAuthentic(string destination, MessageShopInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.ShopSuccess.Replace("#userName#", info.UserName).Replace("#companyName#", info.CompanyName).Replace("#siteName#", info.SiteName);
            SendMessage(destination, string.Concat(info.SiteName, "供应商(", info.CompanyName, ")已经开始实地认证"), str, false);
            return str;
        }
        public string SendMessageOnFinishAuthentic(string destination, MessageShopInfo info)
        {
            MessageContent messageContentConfig = SiteMessageCore.GetMessageContentConfig();
            string str = messageContentConfig.ShopSuccess.Replace("#userName#", info.UserName).Replace("#companyName#", info.CompanyName).Replace("#siteName#", info.SiteName);
            SendMessage(destination, string.Concat(info.SiteName, "供应商(", info.CompanyName, ")已经完成实地认证"), str, false);
            return str;
        }
        public void SendMessages(string[] destination, string content, string title)
        {
            SendMessage(destination, title, content);
        }

        public string SendTestMessage(string destination, string content, string title)
        {
            string message = "发送成功";
            string str = content;
            try
            {
                SendMessage(destination, title, str, false);
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public void SetAllStatus(Dictionary<MessageTypeEnum, StatusEnum> dic)
        {
            foreach (KeyValuePair<MessageTypeEnum, StatusEnum> keyValuePair in dic)
            {
                SetMessageStatus(keyValuePair.Key, keyValuePair.Value);
            }
            FileStream fileStream = new FileStream(string.Concat(SiteMessageCore.WorkDirectory, "/Data/config.xml"), FileMode.Create);
            try
            {
                (new XmlSerializer(typeof(MessageStatus))).Serialize(fileStream, messageStatus);
            }
            finally
            {
                if (fileStream != null)
                {
                    ((IDisposable)fileStream).Dispose();
                }
            }
        }

        private void SetMessageStatus(MessageTypeEnum e, StatusEnum s)
        {
            switch (e)
            {
                case (MessageTypeEnum)1:
                    {
                        messageStatus.OrderCreated = (int)s;
                        break;
                    }
                case (MessageTypeEnum)2:
                    {
                        messageStatus.OrderPay = (int)s;
                        break;
                    }
                case (MessageTypeEnum)3:
                    {
                        messageStatus.OrderShipping = (int)s;
                        break;
                    }
                case (MessageTypeEnum)4:
                    {
                        messageStatus.OrderRefund = (int)s;
                        break;
                    }
                case (MessageTypeEnum)5:
                    {
                        messageStatus.FindPassWord = (int)s;
                        break;
                    }
                case (MessageTypeEnum)6:
                    {
                        messageStatus.ShopAudited = (int)s;
                        break;
                    }
                case (MessageTypeEnum)7:
                    {
                        messageStatus.ShopSuccess = (int)s;
                        break;
                    }
                case (MessageTypeEnum)8:
                    {
                        messageStatus.ReceApply = (int)s;
                        break;
                    }
                case (MessageTypeEnum)9:
                    {
                        messageStatus.StartAuthentic = (int)s;
                        break;
                    }
                case (MessageTypeEnum)10:
                    {
                        messageStatus.FinishAuthentic = (int)s;
                        break;
                    }
            }
        }
    }
}
