using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemCloud.Model
{
    public class Messages:BaseModel
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
        //public Int64 Id { get; set; }

        public long SendUserId { get; set; }

        public long ReviceUserID { get; set; }

       

        public DateTime SendTime { get; set; }

        public string SendUserName { get; set; }

        public string ReviceUserName { get; set; }

        public int IsRead { get; set; }

        public string MessageContent { get; set; }

        public Messages()
        {
            
        }
    }
}
