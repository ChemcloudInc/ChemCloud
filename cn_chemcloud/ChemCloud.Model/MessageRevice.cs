using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class MessageRevice : BaseModel
    {
        public MessageRevice()
        {
            //MessageDetial = new MessageDetial();
        }

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

        public long MsgId { get; set; }
        /// <summary>
        /// 消息接收人ID
        /// </summary>
        public long UserId { get; set; }

        [NotMapped]
        public string UserName { get; set; }
        public DateTime? ReadTime { get; set; }
        /// <summary>
        /// 阅读状态
        /// </summary>
        public int ReadFlag { get; set; }

        public int VisiableFlag { get; set; }

        public DateTime? SendTime { get; set; }

        public int? IsShow { get; set; }

        [NotMapped]
        public MessageDetial MessageDetial { get; set; }

    }
}
