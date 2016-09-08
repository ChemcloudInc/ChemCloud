using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace ChemCloud.Email
{
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
            var smtpServer = "smtp.exmail.qq.com";
            var sendMailId = "info@chemcloud.com";
            var sendMailPassword = "Camchem888";

 
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

        /// <summary>
        /// 自定义XML 读取节点 by薛正根 2016-1-7
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            var doc = new XmlDocument();
            doc.Load(path);
            XmlNode xn = doc.SelectSingleNode(node);
            if (xn != null)
                if (xn.Attributes != null)
                    value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            return value;
        }

        /// <summary>
        /// 获取根目录 by薛正根 2016-1-7
        /// </summary>
        /// <param name="newUrl"></param>
        /// <param name="isabsolute"></param>
        /// <returns></returns>
        public static string GetRootUrl(string newUrl, bool isabsolute = true)
        {
            string appPath = "/";
            HttpContext httpCurrent = HttpContext.Current;
            if (httpCurrent != null)
            {
                HttpRequest req = httpCurrent.Request;
                string urlAuthority = req.Url.GetLeftPart(UriPartial.Authority);
                if (req.ApplicationPath == null || req.ApplicationPath == "/")
                {
                    //直接新建站点   
                    appPath = string.Format("{0}{1}", ((!isabsolute) ? "" : urlAuthority), newUrl);
                }
                else
                {
                    //安装在虚拟子目录下   
                    appPath = string.Format("{0}{1}{2}", ((!isabsolute) ? "" : urlAuthority), req.ApplicationPath, newUrl);
                }
            }
            return appPath;
        }
    }
}
