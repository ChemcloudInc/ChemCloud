using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class MemberIntegralRecordAction : BaseModel
	{
		private long _id;

        public virtual MemberIntegralRecord ChemCloud_MemberIntegralRecord
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

		public long IntegralRecordId
		{
			get;
			set;
		}

		public long VirtualItemId
		{
			get;
			set;
		}

		public MemberIntegral.VirtualItemType? VirtualItemTypeId
		{
			get;
			set;
		}

		public MemberIntegralRecordAction()
		{
		}
	}
}