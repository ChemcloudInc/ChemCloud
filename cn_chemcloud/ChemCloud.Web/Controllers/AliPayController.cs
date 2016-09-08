using ChemCloud.AliPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Controllers
{
    public class AliPayController : Controller
    {
        /// <summary>
        /// 发送支付请求
        /// </summary>
        /// <param name="out_trade_no">订单号</param>
        /// <param name="subject">名称</param>
        /// <param name="total_fee">付款金额</param>
        /// <param name="body">描述</param>
        /// <param name="paytype">支付类型（3供应商认证支付,2产品认证支付，1排名付费支付,0订单支付)</param>
        /// <param name="type">类型 webcz:充值 webzf:支付</param>
        /// <param name="iplm">类型 1.是账户余额支付 2.第三方其它支付方式</param>
        /// <returns></returns>
        public JsonResult SetAliPay(string out_trade_no, string subject, string total_fee, string body, string paytype, string type, string iplm, string targetid, string paymodel)
        {
            string hots = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/";
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", AlipayConfig.service);
            sParaTemp.Add("partner", AlipayConfig.partner);
            sParaTemp.Add("seller_id", AlipayConfig.seller_id);
            sParaTemp.Add("_input_charset", AlipayConfig.input_charset.ToLower());
            sParaTemp.Add("payment_type", AlipayConfig.payment_type);
            sParaTemp.Add("notify_url", AlipayConfig.notify_url);
            sParaTemp.Add("return_url", hots + "/Pay/Return?orderid=" + out_trade_no + "&price=" + total_fee + "&paytype=" + paytype + "&type=" + type + "&iplm=" + iplm + "&targetid=" + targetid + "&paymodel=zfb");
            sParaTemp.Add("anti_phishing_key", AlipayConfig.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", AlipayConfig.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);
            //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.O9yorI&treeId=62&articleId=103740&docType=1
            //如sParaTemp.Add("参数名","参数值");

            //建立请求
            string sHtmlText = AlipaySubmit.BuildRequest(sParaTemp, "get", "确认");
            return Json(sHtmlText);
        }
    }
}