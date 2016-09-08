using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class ManagerQuery : QueryBase
	{
		public long ShopID
		{
			get;
			set;
		}

		public long userID
		{
			get;
			set;
		}
        public long roleID
        {
            get;
            set;
        }
		public ManagerQuery()
		{
		}
	}
}