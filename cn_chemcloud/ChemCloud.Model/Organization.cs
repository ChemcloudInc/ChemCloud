using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;


namespace ChemCloud.Model
{
    public class Organization : BaseModel
    {
        private long _id;

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
        public long UserId
        {
            get;
            set;
        }
        public long RoleId
        {
            get;
            set;
        }
        public long ParentRoleId
        {
            get;
            set;
        }
        public long ParentId
        {
            get;
            set;
        }
        [NotMapped]
        public string UserName
        {
            get;
            set;
        }
        [NotMapped]
        public string RealName
        {
            get;
            set;
        }
        [NotMapped]
        public string ParentName
        {
            get;
            set;
        }
        [NotMapped]
        public string RoleName
        {
            get;
            set;
        }
        [NotMapped]
        public string ParentRoleName
        {
            get;
            set;
        }
    }
}
