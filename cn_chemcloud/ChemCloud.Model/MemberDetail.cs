using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class MemberDetail : BaseModel
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
        private long _MemberId;
        public long MemberId
        { 
            get
            {
                return _MemberId;
            } 
            set
            {
                _MemberId = value;
            } 
        }
        [NotMapped]
        public string MemberName
        {
            get;
            set;
        }
        [NotMapped]
        public string Email
        {
            get;
            set;
        }
        public string CompanyName { get; set; }
        public int CityRegionId { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanySign { get; set; }
        public string CompanyProveFile { get; set; }

        public string ZipCode { get; set; }
        //public bool Disabled { get; set; }
        public ShopInfo.ShopAuditStatus Stage { get; set; }
        public DateTime CreateDate { get; set; }
        public string CompanyIntroduction { get; set; }
        [NotMapped]
        public string RefuseReason { get; set; }
        [NotMapped]
        public DateTime StrLastLoginDate
        {
            get;
            set;
        }

    }

}
