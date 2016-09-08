using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class CategoryInfo : BaseModel
	{
		private long _id;

		public virtual ICollection<ChemCloud.Model.BusinessCategoryInfo> BusinessCategoryInfo
		{
			get;
			set;
		}

		public decimal CommisRate
		{
			get;
			set;
		}

		public int Depth
		{
			get;
			set;
		}

		public long DisplaySequence
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.FloorCategoryInfo> FloorCategoryInfo
		{
			get;
			set;
		}

		public bool HasChildren
		{
			get;
			set;
		}

        public virtual CategoryCashDepositInfo ChemCloud_CategoryCashDeposit
		{
			get;
			set;
		}

        public virtual ICollection<ProductInfo> ChemCloud_Products
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.HomeCategoryInfo> HomeCategoryInfo
		{
			get;
			set;
		}

		public string Icon
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

		public string Meta_Description
		{
			get;
			set;
		}

		public string Meta_Keywords
		{
			get;
			set;
		}

		public string Meta_Title
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public long ParentCategoryId
		{
			get;
			set;
		}

		public string Path
		{
			get;
			set;
		}

		public virtual ChemCloud.Model.ProductTypeInfo ProductTypeInfo
		{
			get;
			set;
		}

		public string RewriteName
		{
			get;
			set;
		}

		public long TypeId
		{
			get;
			set;
		}
        public string ENName
        {
            get;
            set;
        }
		public CategoryInfo()
		{
            FloorCategoryInfo = new HashSet<ChemCloud.Model.FloorCategoryInfo>();
            HomeCategoryInfo = new HashSet<ChemCloud.Model.HomeCategoryInfo>();
            BusinessCategoryInfo = new HashSet<ChemCloud.Model.BusinessCategoryInfo>();
            ChemCloud_Products = new HashSet<ProductInfo>();
		}
	}
}