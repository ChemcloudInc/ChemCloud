using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Web.Http;

namespace ChemCloud.Web.Framework
{
	public abstract class HiAPIController : ApiController
	{
		public UserMemberInfo CurrentUser
		{
			get
			{
				long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("ChemCloud-User"), "Mobile");
				if (num == 0)
				{
					return null;
				}
				return Instance<IMemberService>.Create.GetMember(num);
			}
		}

		protected HiAPIController()
		{
		}
	}
}