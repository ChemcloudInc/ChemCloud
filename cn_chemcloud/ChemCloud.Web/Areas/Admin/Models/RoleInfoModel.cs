using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Admin.Models
{
	public class RoleInfoModel
	{
		public long ID
		{
			get;
			set;
		}

        //[Required(ErrorMessage="角色名称必填")]
        //[StringLength(15, ErrorMessage="角色名称在15个字符以内")]
		public string RoleName
		{
			get;
			set;
		}
		public IEnumerable<ChemCloud.Model.RolePrivilegeInfo> RolePrivilegeInfo
		{
			get;
			set;
		}
        public string Description
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public string PassWord
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
		public RoleInfoModel()
		{
		}
	}
}