using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Plugin.OAuth.WeiXin.Model
{
	public class ErrResult
	{
		public int errcode
		{
			get;
			set;
		}

		public string errmsg
		{
			get;
			set;
		}

		public ErrResult()
		{
		}
	}
}