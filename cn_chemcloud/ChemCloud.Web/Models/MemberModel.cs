using ChemCloud.Model;
using System;

namespace ChemCloud.Web.Models
{
	public class MemberModel : UserMemberInfo
	{
		public string StrCreateDate
		{
			get
			{
				return base.CreateDate.ToString("yyyy-MM-dd HH:mm");
			}
		}

		public string StrLastLoginDate
		{
			get
			{
				return base.LastLoginDate.ToString("yyyy-MM-dd HH:mm");
			}
		}

		public MemberModel()
		{
		}
	}
}