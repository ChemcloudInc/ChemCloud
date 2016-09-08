using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class MemberIntegralRecord : BaseModel
	{
		private long _id;

        public virtual ICollection<MemberIntegralRecordAction> ChemCloud_MemberIntegralRecordAction
		{
			get;
			set;
		}

        public virtual UserMemberInfo ChemCloud_Members
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

		public int Integral
		{
			get;
			set;
		}

		public long MemberId
		{
			get;
			set;
		}

		public DateTime? RecordDate
		{
			get;
			set;
		}

		public string ReMark
		{
			get;
			set;
		}

		public MemberIntegral.IntegralType TypeId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public MemberIntegralRecord()
		{
            ChemCloud_MemberIntegralRecordAction = new HashSet<MemberIntegralRecordAction>();
		}
	}
}