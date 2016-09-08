using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Web.Mvc;

namespace ChemCloud.Web.Framework
{
	public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
	{
		public UserMemberInfo CurrentUser
		{
			get
			{
				long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("ChemCloud-User"), "Web");
				if (num == 0)
				{
					return null;
				}
				return ServiceHelper.Create<IMemberService>().GetMember(num);
			}
		}

		protected WebViewPage()
		{
		}

		public override void InitHelpers()
		{
			base.InitHelpers();
		}
	}
}