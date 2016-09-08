using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class AccountInfo : BaseModel
	{
		private long _id;

		[NotMapped]
		public decimal AccountAmount
		{
			get
			{
				return new decimal(0);
			}
		}

		public DateTime AccountDate
		{
			get;
			set;
		}

		public decimal AdvancePaymentAmount
		{
			get;
			set;
		}

		public decimal CommissionAmount
		{
			get;
			set;
		}

		public DateTime EndDate
		{
			get;
			set;
		}

		public decimal FreightAmount
		{
			get;
			set;
		}

		public virtual ICollection<AccountDetailInfo> ChemCloud_AccountDetails
		{
			get;
			set;
		}

		public virtual ICollection<AccountPurchaseAgreementInfo> ChemCloud_AccountPurchaseAgreement
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

		public decimal PeriodSettlement
		{
			get;
			set;
		}

		public decimal ProductActualPaidAmount
		{
			get;
			set;
		}

		public decimal RefundAmount
		{
			get;
			set;
		}

		public decimal RefundCommissionAmount
		{
			get;
			set;
		}

		public string Remark
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

		public DateTime StartDate
		{
			get;
			set;
		}

		public AccountInfo.AccountStatus Status
		{
			get;
			set;
		}

		public AccountInfo()
		{
            ChemCloud_AccountPurchaseAgreement = new HashSet<AccountPurchaseAgreementInfo>();
            ChemCloud_AccountDetails = new HashSet<AccountDetailInfo>();
		}

		public enum AccountStatus
		{
			[Description("未结算")]
			UnAccount,
			[Description("已结算")]
			Accounted
		}
	}
}