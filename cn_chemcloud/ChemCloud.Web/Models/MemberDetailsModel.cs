using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChemCloud.Web.Models
{
    public class MemberDetailsModel : UserMemberInfo
    {
        
        public long Id
        {
            get;
            set;
        }
        public long MemberId { get; set; }
        //public string MemberName { get; set; }
        public string CompanyName { get; set; }
        public long CityRegionId { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanySign { get; set; }
        public string CompanyProveFile { get; set; }
        public string Stage { get; set; }
        public DateTime CreateDate { get; set; }

        public string ZipCode { get; set; }
        public string CompanyIntroduction { get; set; }
        //[NotMapped]
        //public string RefuseReason { get; set; }
        //public DateTime? StrLastLoginDate
        //{
        //    get;
        //    set;
        //}
        public string MemberName
        {
            get
            {
                return base.UserName;
            }
        }

        public DateTime StrLastLoginDate
        {
            get
            {
                return base.LastLoginDate;
            }
        }
        public string Emails
        {
            get
            {
                return base.Email;
            }
            
        } 
        public MemberDetailsModel()
        {

        }
        
    }
}