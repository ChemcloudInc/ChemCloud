using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Service.Market.Business;
using ChemCloud.ServiceProvider;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Xml;

namespace ChemCloud.Service
{
    public class EmailSettingService : ServiceBase, IEmailSettingService, IService, IDisposable
    {

        public ChemCloud_EmailSetting GetChemCloud_EmailSetting()
        {
            return context.ChemCloud_EmailSetting.FirstOrDefault();
        }

        public bool EditEmailSetting(ChemCloud_EmailSetting model)
        {
            bool result = false;
            ChemCloud_EmailSetting _ChemCloud_EmailSetting = context.ChemCloud_EmailSetting.FindById(model.Id);
            if (_ChemCloud_EmailSetting != null)
            {
                _ChemCloud_EmailSetting.SmtpServer = model.SmtpServer;
                _ChemCloud_EmailSetting.SendMailId = model.SendMailId;
                _ChemCloud_EmailSetting.SendMailPassword = model.SendMailPassword;
                int count = context.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool AddEmailSetting(ChemCloud_EmailSetting model)
        {
            bool result = false;
            context.ChemCloud_EmailSetting.Add(model);
            int count = context.SaveChanges();
            if (count > 0) { result = true; }
            return result;
        }
    }


    public class SendMail
    {
        /// <summary>
        /// 发送邮件函数 by 薛正根 2015-12-01
        /// </summary>
        /// <param name="mailTo">要发送的邮箱 多个邮箱请用";"隔开</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {
            var smtpServer = "smtp.qq.com";
            var sendMailId = "229882570@qq.com";
            var sendMailPassword = "zhenggen00";

            EmailSettingService _EmailSettingService = new EmailSettingService();
            ChemCloud.Model.ChemCloud_EmailSetting model = _EmailSettingService.GetChemCloud_EmailSetting();

            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.SmtpServer) && !string.IsNullOrWhiteSpace(model.SendMailId) && !string.IsNullOrWhiteSpace(model.SendMailPassword))
                {
                    smtpServer = model.SmtpServer;
                    sendMailId = model.SendMailId;
                    sendMailPassword = model.SendMailPassword;
                }
            }

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(sendMailId, sendMailPassword);//用户名和密码
            #region 发送邮件设置
            //发送给多个邮件
            if (mailTo.Contains(";"))
            {
                string[] mailToId = mailTo.Split(';');
                for (int k = 0; k < mailToId.Length; k++)
                {
                    if (mailToId[k].Trim() == "")
                    {
                        break;
                    }
                    MailMessage mailMessage = new MailMessage(sendMailId, mailToId[k].Trim()); // 发送人和收件人
                    mailMessage.Subject = mailSubject;//邮件主题
                    mailMessage.Body = mailContent;//邮件正文 
                    mailMessage.BodyEncoding = Encoding.UTF8;//正文编码格式
                    mailMessage.IsBodyHtml = true;//设置为HTML格式
                    mailMessage.Priority = MailPriority.Low;//邮件的优先级
                    try
                    {
                        smtpClient.Send(mailMessage); // 发送邮件
                        continue;
                    }
                    catch (SmtpException ex)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                //单个邮件发送
                MailMessage mailMessage = new MailMessage(sendMailId, mailTo); // 发送人和收件人
                mailMessage.Subject = mailSubject;//邮件主题
                mailMessage.Body = mailContent;//邮件正文 
                mailMessage.BodyEncoding = Encoding.UTF8;//正文编码格式
                mailMessage.IsBodyHtml = true;//设置为HTML格式
                mailMessage.Priority = MailPriority.Low;//邮件的优先级

                try
                {
                    smtpClient.Send(mailMessage); // 发送邮件
                    return true;
                }
                catch (SmtpException ex)
                {
                    return false;
                }
            }
            #endregion
        }

    }
}
