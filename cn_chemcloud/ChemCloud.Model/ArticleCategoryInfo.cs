using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ArticleCategoryInfo : BaseModel
	{
		private long _id;

		public virtual ICollection<ChemCloud.Model.ArticleInfo> ArticleInfo
		{
			get;
			set;
		}

		public long DisplaySequence
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

		public bool IsDefault
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

        public string NameEn
        {
            get;
            set;
        }

		public long ParentCategoryId
		{
			get;
			set;
		}

		public ArticleCategoryInfo()
		{
            ArticleInfo = new HashSet<ChemCloud.Model.ArticleInfo>();
		}
	}
}