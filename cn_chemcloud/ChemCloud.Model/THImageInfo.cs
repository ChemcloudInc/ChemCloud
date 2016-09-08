using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class THImageInfo : BaseModel
    {

        /*自动标识*/
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
        public string THImage { get; set; }
        public long THMessageId { get; set; }
    }
}
