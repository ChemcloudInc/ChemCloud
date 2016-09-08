using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
using com.paypal.sdk.util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud
{
    public class PaypalProvider
    {
        /// <summary>
        /// 初始化APIProfile
        /// </summary>
        /// <returns></returns>
        private static IAPIProfile CreateProfile()
        {
            IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();

            profile.APIUsername = ConfigurationManager.AppSettings["APIUserName"];
            profile.APIPassword = ConfigurationManager.AppSettings["APIPassword"];
            profile.APISignature = ConfigurationManager.AppSettings["APISinature"];
            profile.Environment = ConfigurationManager.AppSettings["Environment"];
            profile.Subject = "";
            return profile;
        }

        /// <summary>
        /// 初始化支付服务器调用
        /// </summary>
        /// <returns></returns>
        private static NVPCallerServices InitializeServices()
        {

            NVPCallerServices service = new NVPCallerServices();
            IAPIProfile profile = CreateProfile();
            service.APIProfile = profile;
            return service;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="nvp"></param>
        /// <returns></returns>
        private static NVPCodec SendExpressCheckoutCommand(NVPCodec nvp)
        {
            NVPCallerServices service = InitializeServices();            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            string response = service.Call(nvp.Encode());
            NVPCodec responsenvp = new NVPCodec();
            responsenvp.Decode(response);
            return responsenvp;

        }

        /// <summary>
        /// 发送支付请求
        /// </summary>
        /// <param name="nvp"></param>
        /// <returns></returns>
        public static NVPCodec SetExpressCheckout(NVPCodec nvp)
        {
            nvp.Add("METHOD", "SetExpressCheckout");
            return SendExpressCheckoutCommand(nvp);
        }

        /// <summary>
        /// 得到用户在paypay中填写的详细信息
        /// </summary>
        /// <param name="nvp"></param>
        /// <returns></returns>
        public static NVPCodec GetExpressCheckoutDetails(NVPCodec nvp)
        {
            nvp.Add("METHOD", "GetExpressCheckoutDetails");
            return SendExpressCheckoutCommand(nvp);
        }

        /// <summary>
        /// 付款操作
        /// </summary>
        /// <param name="nvp"></param>
        /// <returns></returns>
        public static NVPCodec DoExpressCheckoutPayment(NVPCodec nvp)
        {
            nvp.Add("METHOD", "DoExpressCheckoutPayment");
            return SendExpressCheckoutCommand(nvp);
        }


        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受  
            return true;
        }


    }
}
