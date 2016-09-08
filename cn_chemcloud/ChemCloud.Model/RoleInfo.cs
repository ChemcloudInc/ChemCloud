using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class RoleInfo : BaseModel
	{
		private long _id;

		public string Description
		{
			get;
			set;
		}

		public new long Id
		{
			get
			{
				return _id;
			}
			set
			{
                _id = value;
				base.Id = value;
			}
		}
        [NotMapped]
        public string UserName
        {
            get;
            set;
        }
        [NotMapped]
        public string PassWord
        {
            get;
            set;
        }
        [NotMapped]
        public string Emails
        {
            get;
            set;
        }
		public string RoleName
		{
			get;
			set;
		}
        public virtual ICollection<ChemCloud.Model.RolePrivilegeInfo> RolePrivilegeInfo
		{
			get;
			set;
		}

		public long ShopId
		{
			get;
			set;
		}

		public RoleInfo()
		{
            RolePrivilegeInfo = new HashSet<ChemCloud.Model.RolePrivilegeInfo>();
		}
	}
}