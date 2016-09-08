using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class FloorTablsInfo : BaseModel
	{
		private long _id;

		public long FloorId
		{
			get;
			set;
		}

        public virtual ICollection<FloorTablDetailsInfo> ChemCloud_FloorTablDetails
		{
			get;
			set;
		}

        public virtual HomeFloorInfo ChemCloud_HomeFloors
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

		public string Name
		{
			get;
			set;
		}

		public FloorTablsInfo()
		{
            ChemCloud_FloorTablDetails = new HashSet<FloorTablDetailsInfo>();
		}
	}
}