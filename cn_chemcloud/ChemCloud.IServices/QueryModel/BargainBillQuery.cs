using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class BargainBillQuery : QueryBase
    {
        public BargainBillQuery()
        {
        }

        public string BillNo { get; set; }

        public string DeliverType { get; set; }

        public string PayMode { get; set; }

        public int? IsDelete
        {
            get;
            set;
        }

        public long? ShopId
        {
            get;
            set;
        }

        public string ShopName
        {
            get;
            set;
        }

        public long? MemberId
        {
            get;
            set;
        }

        public string MemberName
        {
            get;
            set;
        }

        public DateTime? StartDate
        {
            get;
            set;
        }

        public DateTime? EndDate
        {
            get;
            set;
        }

        public string ProductCode
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }
        public IEnumerable<EnumBillStatus> BillStatus
        {
            get;
            set;
        }
    }
}
