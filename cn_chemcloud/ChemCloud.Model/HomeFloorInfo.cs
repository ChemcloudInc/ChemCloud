using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class HomeFloorInfo : BaseModel
	{
		private long _id;

		public string DefaultTabName
		{
			get;
			set;
		}

		public long DisplaySequence
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.FloorBrandInfo> FloorBrandInfo
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.FloorCategoryInfo> FloorCategoryInfo
		{
			get;
			set;
		}

		public string FloorName
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.FloorProductInfo> FloorProductInfo
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.FloorTopicInfo> FloorTopicInfo
		{
			get;
			set;
		}

        public virtual ICollection<FloorTablsInfo> ChemCloud_FloorTabls
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

		public bool IsShow
		{
			get;
			set;
		}

		public long StyleLevel
		{
			get;
			set;
		}

		public string SubName
		{
			get;
			set;
		}

		public HomeFloorInfo()
		{
            FloorBrandInfo = new HashSet<ChemCloud.Model.FloorBrandInfo>();
            FloorCategoryInfo = new HashSet<ChemCloud.Model.FloorCategoryInfo>();
            FloorProductInfo = new HashSet<ChemCloud.Model.FloorProductInfo>();
            FloorTopicInfo = new HashSet<ChemCloud.Model.FloorTopicInfo>();
            ChemCloud_FloorTabls = new HashSet<FloorTablsInfo>();
		}
	}
}