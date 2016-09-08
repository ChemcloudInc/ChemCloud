using ChemCloud.Core.Plugins.Payment;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Web.Models
{
	public class PaymentModel
	{
		public string Id
		{
			get;
			set;
		}

		public string Logo
		{
			get;
			set;
		}

		public string RequestUrl
		{
			get;
			set;
		}

		public ChemCloud.Core.Plugins.Payment.UrlType UrlType
		{
			get;
			set;
		}

		public PaymentModel()
		{
		}
	}
}