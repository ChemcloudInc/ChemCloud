using ChemCloud.Web;
using System;

namespace ChemCloud.Web.Areas.Web
{
	public static class BizAfterLogin
	{
		public static void Run(long memberId)
		{
			(new CartHelper()).UpdateCartInfoFromCookieToServer(memberId);
		}
	}
}