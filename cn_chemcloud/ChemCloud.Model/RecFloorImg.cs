using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class RecFloorImg : BaseModel
    {
        private long _id;
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
        public string URL { get; set; }
        public int TypeId { get; set; }
        public int ChildId { get; set; }
        public string Tag { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string  ImageUrl { get; set; }



    }
}
