using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Plugin.OAuth.WeiXin.Model
{
	public class TokenResult
	{
		public string access_token
		{
			get;
			set;
		}

		public int expires_in
		{
			get;
			set;
		}

		public string openid
		{
			get;
			set;
		}

		public string refresh_token
		{
			get;
			set;
		}

		public string scope
		{
			get;
			set;
		}

		public TokenResult()
		{
		}
	}
}