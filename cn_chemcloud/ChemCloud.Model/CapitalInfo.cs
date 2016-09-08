using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class CapitalInfo : BaseModel
    {
        private long _id;

        public decimal? Balance
        {
            get;
            set;
        }

        public decimal? ChargeAmount
        {
            get;
            set;
        }

        public decimal? FreezeAmount
        {
            get;
            set;
        }

        public virtual ICollection<CapitalDetailInfo> ChemCloud_CapitalDetail
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

        public long MemId
        {
            get;
            set;
        }

        public long ManageId { get; set; }

        public CapitalInfo()
        {
            ChemCloud_CapitalDetail = new HashSet<CapitalDetailInfo>();
        }
    }
}