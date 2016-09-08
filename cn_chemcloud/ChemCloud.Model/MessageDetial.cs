using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class MessageDetial : BaseModel
    {
        public MessageDetial()
        {
            MessageRevice = new HashSet<MessageRevice>();
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

        public int MessageTitleId { get; set; }

        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        /// <summary>
        /// 消息发送人ID
        /// </summary>
        public long ManagerId { get; set; }
        [NotMapped]
        public string SendName { get; set; }
        public DateTime? SendTime { get; set; }

        //1所有人 2所有供应商 3 所有采购商 4 自定义发送
        public int SendObj { get; set; }
        //1消息 2通知
        public int MsgType { get; set; }

        public int LanguageType { get; set; }
        [NotMapped]
        //已读数量
        public int ReadCount { get; set; }
        [NotMapped]
        //未读数量
        public int NoreadCount { get; set; }
        [NotMapped]
        public List<MessageDetial> _MessageDetials { get; set; }

        public virtual ICollection<MessageRevice> MessageRevice
        {
            get;
            set;
        }
    }
}
