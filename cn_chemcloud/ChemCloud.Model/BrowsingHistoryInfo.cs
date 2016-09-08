using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class BrowsingHistoryInfo : BaseModel
    {
        private long _id;

        public DateTime BrowseTime
        {
            get;
            set;
        }

        public virtual UserMemberInfo ChemCloud_Members
        {
            get;
            set;
        }

        public virtual ProductInfo ChemCloud_Products
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

        public long MemberId
        {
            get;
            set;
        }

        public long ProductId
        {
            get;
            set;
        }

        public BrowsingHistoryInfo()
        {
        }
    }
}