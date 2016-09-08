using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class THMessageInfo : BaseModel
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

        public long THId { get; set; }
        public string MessageContent { get; set; }
        public DateTime MessageDate { get; set; }
        public int MessageAttitude { get; set; }
        public long UserId { get; set; }
        public string ReturnName { get; set; }

    }
}
