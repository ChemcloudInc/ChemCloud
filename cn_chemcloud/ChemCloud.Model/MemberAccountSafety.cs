using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class MemberAccountSafety
	{
		public int AccountSafetyLevel
		{
			get;
			set;
		}

		public bool BindEmail
		{
			get;
			set;
		}

		public bool BindPhone
		{
			get;
			set;
		}

		public bool PayPassword
		{
			get;
			set;
		}

		public MemberAccountSafety()
		{
		}
	}
}