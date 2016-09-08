using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Framework
{
	public abstract class BaseMobileMemberController : BaseMobileTemplatesController
	{
		protected BaseMobileMemberController()
		{
		}

		private bool BindOpenIdToUser(ActionExecutingContext filterContext)
		{
			string str;
			bool flag = true;
			IMobileOAuth weixinOAuth = null;
			if (base.PlatformType == ChemCloud.Core.PlatformType.WeiXin)
			{
				weixinOAuth = new WeixinOAuth();
			}
			string.Format("/m-{0}/Login/Entrance?returnUrl={1}", base.PlatformType.ToString(), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
			if (weixinOAuth == null || GetRequestType(filterContext.HttpContext.Request) != ChemCloud.Core.PlatformType.WeiXin)
			{
				flag = false;
			}
			else
			{
				WXShopInfo wXShopInfo = new WXShopInfo();
				string item = filterContext.HttpContext.Request["shop"];
				MemberOpenIdInfo.AppIdTypeEnum appIdTypeEnum = MemberOpenIdInfo.AppIdTypeEnum.Normal;
				if (!string.IsNullOrEmpty(item))
				{
					long num = 0;
					long.TryParse(item, out num);
					if (num > 0)
					{
						wXShopInfo = ServiceHelper.Create<IVShopService>().GetVShopSetting(num);
					}
				}
				if (string.IsNullOrEmpty(wXShopInfo.AppId) || string.IsNullOrEmpty(wXShopInfo.AppSecret))
				{
					WXShopInfo wXShopInfo1 = new WXShopInfo()
					{
						AppId = base.CurrentSiteSetting.WeixinAppId,
						AppSecret = base.CurrentSiteSetting.WeixinAppSecret,
						Token = base.CurrentSiteSetting.WeixinToken
					};
					wXShopInfo = wXShopInfo1;
					appIdTypeEnum = MemberOpenIdInfo.AppIdTypeEnum.Payment;
				}
				MobileOAuthUserInfo userInfoBequiet = weixinOAuth.GetUserInfo_bequiet(filterContext, out str, wXShopInfo);
				if (!string.IsNullOrWhiteSpace(str))
				{
					filterContext.Result = Redirect(str);
				}
				else
				{
					flag = false;
					if (userInfoBequiet != null && !string.IsNullOrWhiteSpace(userInfoBequiet.OpenId))
					{
						if (appIdTypeEnum == MemberOpenIdInfo.AppIdTypeEnum.Payment)
						{
							string str1 = SecureHelper.AESEncrypt(userInfoBequiet.OpenId, "Mobile");
							WebHelper.SetCookie("Himall-User_OpenId", str1);
						}
						IMemberService memberService = ServiceHelper.Create<IMemberService>();
						UserMemberInfo memberByOpenId = null;
						if (memberByOpenId == null)
						{
							memberByOpenId = memberService.GetMemberByOpenId(userInfoBequiet.LoginProvider, userInfoBequiet.OpenId);
						}
						if (memberByOpenId == null)
						{
							memberService.BindMember(base.CurrentUser.Id, "ChemCloud.Plugin.OAuth.WeiXin", userInfoBequiet.OpenId, appIdTypeEnum, null, userInfoBequiet.UnionId);
						}
						else
						{
							string str2 = UserCookieEncryptHelper.Encrypt(memberByOpenId.Id, "Mobile");
							WebHelper.SetCookie("ChemCloud-User", str2);
						}
					}
				}
			}
			return flag;
		}

		private ChemCloud.Core.PlatformType GetRequestType(HttpRequestBase request)
		{
			ChemCloud.Core.PlatformType platformType = ChemCloud.Core.PlatformType.Wap;
			if (request.UserAgent.ToLower().Contains("micromessenger"))
			{
				platformType = ChemCloud.Core.PlatformType.WeiXin;
			}
			return platformType;
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			bool flag;
			if (filterContext.IsChildAction)
			{
				return;
			}
			if (base.CurrentUser == null)
			{
				flag = (!WebHelper.IsAjax() ? ProcessInvalidUser_NormalRequest(filterContext) : ProcessInvalidUser_Ajax(filterContext));
				if (flag)
				{
					return;
				}
			}
			else if (!WebHelper.IsAjax() && BindOpenIdToUser(filterContext))
			{
				return;
			}
			base.OnActionExecuting(filterContext);
		}

		private bool ProcessInvalidUser_Ajax(ActionExecutingContext filterContext)
		{
			BaseController.Result result = new BaseController.Result()
			{
				msg = "登录超时,请重新登录！",
				success = false
			};
			filterContext.Result = base.Json(result);
			return true;
		}

		private bool ProcessInvalidUser_NormalRequest(ActionExecutingContext filterContext)
		{
			string str;
			bool flag = true;
			IMobileOAuth weixinOAuth = null;
			if (base.PlatformType == ChemCloud.Core.PlatformType.WeiXin)
			{
				weixinOAuth = new WeixinOAuth();
			}
			string str1 = string.Format("/m-{0}/Login/Entrance?returnUrl={1}", base.PlatformType.ToString(), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
			if (weixinOAuth == null || GetRequestType(filterContext.HttpContext.Request) != ChemCloud.Core.PlatformType.WeiXin)
			{
				filterContext.Result = Redirect(str1);
			}
			else
			{
				WXShopInfo wXShopInfo = new WXShopInfo();
				string item = filterContext.HttpContext.Request["shop"];
				MemberOpenIdInfo.AppIdTypeEnum appIdTypeEnum = MemberOpenIdInfo.AppIdTypeEnum.Normal;
				if (!string.IsNullOrEmpty(item))
				{
					long num = 0;
					long.TryParse(item, out num);
					if (num > 0)
					{
						wXShopInfo = ServiceHelper.Create<IVShopService>().GetVShopSetting(num);
					}
				}
				if (string.IsNullOrEmpty(wXShopInfo.AppId) || string.IsNullOrEmpty(wXShopInfo.AppSecret))
				{
					WXShopInfo wXShopInfo1 = new WXShopInfo()
					{
						AppId = base.CurrentSiteSetting.WeixinAppId,
						AppSecret = base.CurrentSiteSetting.WeixinAppSecret,
						Token = base.CurrentSiteSetting.WeixinToken
					};
					wXShopInfo = wXShopInfo1;
					appIdTypeEnum = MemberOpenIdInfo.AppIdTypeEnum.Payment;
				}
				MobileOAuthUserInfo userInfo = weixinOAuth.GetUserInfo(filterContext, out str, wXShopInfo);
				if (!string.IsNullOrWhiteSpace(str))
				{
					filterContext.Result = Redirect(str);
				}
				else if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.OpenId))
				{
					filterContext.Result = Redirect(str1);
				}
				else
				{
					if (appIdTypeEnum == MemberOpenIdInfo.AppIdTypeEnum.Payment)
					{
						string str2 = SecureHelper.AESEncrypt(userInfo.OpenId, "Mobile");
						WebHelper.SetCookie("Himall-User_OpenId", str2);
					}
					UserMemberInfo memberByOpenId = null;
					if (memberByOpenId == null)
					{
						memberByOpenId = ServiceHelper.Create<IMemberService>().GetMemberByOpenId(userInfo.LoginProvider, userInfo.OpenId);
					}
					if (memberByOpenId == null)
					{
						object[] objArray = new object[] { base.PlatformType.ToString(), userInfo.OpenId, "ChemCloud.Plugin.OAuth.WeiXin", HttpUtility.UrlEncode(userInfo.NickName), HttpUtility.UrlEncode(userInfo.RealName), HttpUtility.UrlEncode(userInfo.Headimgurl), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()), appIdTypeEnum, userInfo.UnionId };
						str1 = string.Format("/m-{0}/Login/Entrance?openId={1}&serviceProvider={2}&nickName={3}&realName={4}&headimgurl={5}&returnUrl={6}&AppidType={7}&unionid={8}", objArray);
						filterContext.Result = Redirect(str1);
					}
					else
					{
						string str3 = UserCookieEncryptHelper.Encrypt(memberByOpenId.Id, "Mobile");
						WebHelper.SetCookie("ChemCloud-User", str3);
					}
				}
			}
			return flag;
		}
	}
}