using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.PaymentPlugin;
using ChemCloud.WeixinPaymentBase;
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;

namespace ChemCloud.Plugin.Payment.WeiXinPay_Native
{
	public class WeiXinPay : ServiceBase, IPaymentPlugin, IPlugin
	{
		private string _HelpImage = string.Empty;

		public string HelpImage
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_HelpImage))
				{
                    _HelpImage = Utility<Config>.GetConfig(base.WorkDirectory).HelpImage;
				}
				return _HelpImage;
			}
		}

		public UrlType RequestUrlType
		{
			get
			{
				return UrlType.QRCode;
			}
		}

		public WeiXinPay()
		{
		}

		public string GetRequestUrl(string returnUrl, string notifyUrl, string orderId, decimal totalFee, string productInfo, string openId = null)
		{
			string empty = string.Empty;
			Config config = Utility<Config>.GetConfig(base.WorkDirectory);
			if (string.IsNullOrEmpty(config.AppId))
			{
				throw new PluginException("未设置AppId");
			}
			if (string.IsNullOrEmpty(config.MCHID))
			{
				throw new PluginException("未设置MCHID");
			}
			string noncestr = TenPayUtil.GetNoncestr();
			string str = DateTime.Now.ToString("yyyyMMddHHmmss");
			RequestHandler requestHandler = new RequestHandler();
			requestHandler.SetParameter("appid", config.AppId);
			requestHandler.SetParameter("mch_id", config.MCHID);
			requestHandler.SetParameter("device_info", string.Empty);
			requestHandler.SetParameter("nonce_str", noncestr);
			requestHandler.SetParameter("body", productInfo);
			requestHandler.SetParameter("attach", string.Empty);
			requestHandler.SetParameter("out_trade_no", orderId);
			int num = (int)(totalFee * new decimal(100));
			requestHandler.SetParameter("total_fee", num.ToString());
			requestHandler.SetParameter("spbill_create_ip", "222.240.184.122");
			requestHandler.SetParameter("time_start", str);
			requestHandler.SetParameter("time_expire", string.Empty);
			requestHandler.SetParameter("goods_tag", string.Empty);
			requestHandler.SetParameter("notify_url", notifyUrl);
			requestHandler.SetParameter("trade_type", "NATIVE");
			requestHandler.SetParameter("openid", openId);
			requestHandler.SetParameter("product_id", orderId);
			string str1 = requestHandler.CreateMd5Sign("key", config.Key);
			requestHandler.SetParameter("sign", str1);
			string str2 = requestHandler.ParseXML();
			XDocument xDocument = XDocument.Parse(TenPayV3.Unifiedorder(str2));
			if (xDocument == null)
			{
				throw new PluginException(string.Concat("调用统一支付接口(Native)时出错：", str2));
			}
			XElement xElement = xDocument.Element("xml").Element("return_code");
			XElement xElement1 = xDocument.Element("xml").Element("return_msg");
			if (xElement == null)
			{
				throw new PluginException("调用统一支付接口(Native)时,返回参数异常");
			}
			if (!(xElement.Value == "SUCCESS"))
			{
				throw new PluginException(string.Concat("调用统一支付接口(Native)时,接口返回异常:", xElement1.Value));
			}
			xElement1 = xDocument.Element("xml").Element("result_code");
			XElement xElement2 = xDocument.Element("xml").Element("err_code_des");
			if (!(xElement1.Value == "SUCCESS"))
			{
				throw new PluginException(string.Concat("调用统一支付接口(Native)时,接口返回异常:", xElement2.Value));
			}
			xElement1 = xDocument.Element("xml").Element("code_url");
			empty = xElement1.Value;
			return empty;
		}

		void ChemCloud.Core.Plugins.IPlugin.CheckCanEnable()
		{
			base.CheckCanEnable();
		}


		string ChemCloud.Core.Plugins.Payment.IPaymentPlugin.ConfirmPayResult()
		{
			return base.ConfirmPayResult();
		}

		void ChemCloud.Core.Plugins.Payment.IPaymentPlugin.Disable(PlatformType platformType)
		{
			base.Disable(platformType);
		}

		void ChemCloud.Core.Plugins.Payment.IPaymentPlugin.Enable(PlatformType platformType)
		{
			base.Enable(platformType);
		}

	

		FormData ChemCloud.Core.Plugins.Payment.IPaymentPlugin.GetFormData()
		{
			return base.GetFormData();
		}

		bool ChemCloud.Core.Plugins.Payment.IPaymentPlugin.IsEnable(PlatformType platformType)
		{
			return base.IsEnable(platformType);
		}

		PaymentInfo ChemCloud.Core.Plugins.Payment.IPaymentPlugin.ProcessNotify(HttpRequestBase httpRequestBase)
		{
			return base.ProcessNotify(httpRequestBase);
		}

		PaymentInfo ChemCloud.Core.Plugins.Payment.IPaymentPlugin.ProcessReturn(HttpRequestBase httpRequestBase)
		{
			return base.ProcessReturn(httpRequestBase);
		}

	
		void ChemCloud.Core.Plugins.Payment.IPaymentPlugin.SetFormValues(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
		{
			base.SetFormValues(keyValuePairs);
		}
	}
}