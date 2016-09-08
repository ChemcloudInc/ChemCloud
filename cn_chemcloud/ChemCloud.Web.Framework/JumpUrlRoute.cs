using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Framework
{
	public class JumpUrlRoute
	{
		public string Action
		{
			get;
			set;
		}

		public string Area
		{
			get;
			set;
		}

		public string Controller
		{
			get;
			set;
		}

		public bool IsSpecial
		{
			get;
			set;
		}

		public string PC
		{
			get;
			set;
		}

		public string WAP
		{
			get;
			set;
		}

		public string WX
		{
			get;
			set;
		}

		public JumpUrlRoute()
		{
		}
	}
}