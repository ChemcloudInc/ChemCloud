using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class FloorProductInfo : BaseModel
    {
        private long _id;

        public long FloorId
        {
            get;
            set;
        }

        public virtual ICollection<HomeFloorInfo> HomeFloorInfo
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

        public long ProductId
        {
            get;
            set;
        }

        public virtual ChemCloud.Model.ProductInfo ProductInfo
        {
            get;
            set;
        }

        public int Tab
        {
            get;
            set;
        }

        public FloorProductInfo()
        {
            //HomeFloorInfo = new HashSet<HomeFloorInfo>();
        }
    }
}