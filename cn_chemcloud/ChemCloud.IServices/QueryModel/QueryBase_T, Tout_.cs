using ChemCloud.Model;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class QueryBase<T, Tout>
	where T : BaseModel
	{
		public bool IsAsc
		{
			get;
			set;
		}

		public int PageNo
		{
			get;
			set;
		}

		public int PageSize
		{
			get;
			set;
		}

		public Expression<Func<T, Tout>> Sort
		{
			get;
			set;
		}

		public QueryBase()
		{
		}
	}
}