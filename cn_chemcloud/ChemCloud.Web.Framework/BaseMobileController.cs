using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChemCloud.Web.Framework
{
	public abstract class BaseMobileController : BaseController
	{
		private static Dictionary<string, string> platformTypesStringMap;

		public UserMemberInfo CurrentUser
		{
			get
			{
				long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("ChemCloud-User"), "Mobile");
				if (num == 0)
				{
					return null;
				}
				return ServiceHelper.Create<IMemberService>().GetMember(num);
			}
		}

		public ChemCloud.Core.PlatformType PlatformType
		{
			get
			{
				string lower = base.RouteData.Values["platform"].ToString().ToLower();
				Dictionary<string, string> strs = BaseMobileController.platformTypesStringMap;
				ChemCloud.Core.PlatformType platformType = ChemCloud.Core.PlatformType.Mobile;
				if (strs.ContainsKey(lower))
				{
					platformType = (ChemCloud.Core.PlatformType)Enum.Parse(typeof(ChemCloud.Core.PlatformType), strs[lower]);
				}
				return platformType;
			}
		}

		static BaseMobileController()
		{
		}

		public BaseMobileController()
		{
			if (BaseMobileController.platformTypesStringMap == null)
			{
				Dictionary<int, string> dictionary = EnumHelper.ToDictionary<ChemCloud.Core.PlatformType>();
				BaseMobileController.platformTypesStringMap = new Dictionary<string, string>();
				foreach (KeyValuePair<int, string> value in dictionary)
				{
					BaseMobileController.platformTypesStringMap[value.Value.ToLower()] = value.Value;
				}
			}
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			((dynamic)base.ViewBag).AreaName = string.Format("m-{0}", PlatformType.ToString());
			((dynamic)base.ViewBag).Logo = base.CurrentSiteSetting.Logo;
			((dynamic)base.ViewBag).SiteName = base.CurrentSiteSetting.SiteName;
			string cookie = WebHelper.GetCookie("Himall-Mobile-AppType");
			string str = WebHelper.GetCookie("Himall-VShopId");
			if (cookie == string.Empty && filterContext.HttpContext.Request["shop"] != null)
			{
				cookie = filterContext.HttpContext.Request["shop"].ToString();
				long num = 0;
				if (long.TryParse(cookie, out num))
				{
					WebHelper.SetCookie("Himall-VShopId", (ServiceHelper.Create<IVShopService>().GetVShopByShopId(num) ?? new VShopInfo()).Id.ToString());
				}
				WebHelper.SetCookie("Himall-Mobile-AppType", cookie);
			}
			((dynamic)base.ViewBag).MAppType = cookie;
			((dynamic)base.ViewBag).MVshopId = str;
			base.OnActionExecuting(filterContext);
		}
	}
}