using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Models
{
	public class ManagerInfoModel
	{
		public string Password
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public long RoleId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}
        public string Email
        {
            get;
            set;
        }
		public ManagerInfoModel()
		{
		}
	}
}