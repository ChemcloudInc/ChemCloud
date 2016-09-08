using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class OrderComplaintInfo : BaseModel
	{
		private long _id;

		public DateTime ComplaintDate
		{
			get;
			set;
		}

		public string ComplaintReason
		{
			get;
			set;
		}
        [NotMapped]
        public string CompanyName
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

		public long OrderId
		{
			get;
			set;
		}

		public virtual ChemCloud.Model.OrderInfo OrderInfo
		{
			get;
			set;
		}

		public string SellerReply
		{
			get;
			set;
		}

		public long ShopId
		{
			get;
			set;
		}

		public string ShopName
		{
			get;
			set;
		}

		public string ShopPhone
		{
			get;
			set;
		}

		public OrderComplaintInfo.ComplaintStatus Status
		{
			get;
			set;
		}

		public long UserId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string UserPhone
		{
			get;
			set;
		}

		public OrderComplaintInfo()
		{
		}

		public enum ComplaintStatus
		{
			[Description("等待供应商处理")]
			WaitDeal = 1,
            [Description("供应商处理完成")]
			Dealed = 2,
			[Description("等待平台介入")]
			Dispute = 3,
			[Description("已结束")]
			End = 4
		}
	}
}