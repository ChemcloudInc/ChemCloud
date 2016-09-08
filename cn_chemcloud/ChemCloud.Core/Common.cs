using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ChemCloud.Core
{
    /// <summary>
    /// 基类 薛正根1 added 2016-03-23
    /// </summary>
    public class Common
    {
        public Common() { }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            string lRet = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            if (string.IsNullOrEmpty(lRet))
                lRet = Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            if (string.IsNullOrEmpty(lRet))
                lRet = Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(lRet))
                lRet = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(lRet) || !IsIp(lRet))
                return "127.0.0.1";
            return lRet;
        }

        #region ===是否为IP===
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

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
        /// <summary>
        /// 保留指定字符串长度
        /// </summary>
        /// <param name="str">指定的字符串</param>
        /// <param name="length">指定的长度</param>
        /// <returns></returns>
        public static string CutString(string str, int length)
        {
            if (str.Length > length)
                str = str.Substring(0, length - 2) + "...";
            return str;
        }

    }
}
