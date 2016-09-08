using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class BargainQuery : QueryBase
    {
        /// <summary>
        /// 询盘单号
        /// </summary>
        public string BillNo { get; set; }

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
        public DateTime? CreateDate { get; set; }
        public long? BargainId
        {
            get;
            set;
        }
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
        public long? UserId
		{
			get;
			set;
		}

		public string UserName
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

        public string ProductCode
        {
            get;
            set;
        }
        public string CASNo
        {
            get;
            set;
        }
        public string ProductName
        {
            get;
            set;
        }
        public EnumBillStatus? BillStatus { get; set; }
        public long? Id { get; set; }
        public BargainQuery()
		{
		}

    }
}
