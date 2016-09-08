using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Admin.Models.Product
{
	public class CategoryTreeModel
	{
		public IEnumerable<CategoryTreeModel> Children
		{
			get;
			set;
		}

		public int Depth
		{
			get;
			set;
		}

		public long Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}
        public string ENName
        {
            get;
            set;
        }
		public long ParentCategoryId
		{
			get;
			set;
		}

		public CategoryTreeModel()
		{
		}
	}
}