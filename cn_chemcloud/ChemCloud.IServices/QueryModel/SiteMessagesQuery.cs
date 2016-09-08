using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class SiteMessagesQuery : QueryBase
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public long? Id
        {
            get;
            set;
        }
        /// <summary>
        /// 消息接收人ID
        /// </summary>
        public long? MemberId
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderBy
        {
            get;
            set;
        }
        public SiteMessages.Status? Status
        {
            get;
            set;
        }
        public SiteMessages.ReceiveType? ReceStatus
        {
            get;
            set;
        }
        public MessageSetting.MessageModuleStatus? MessageModule
        {
            get;
            set;
        }
      
        public SiteMessagesQuery()
		{
		}
    }
    public class SiteMessageAddQuery : QueryBase
    {
        public int ReceType { get; set; }

        public int MessageTitleId { get; set; }

        public string MessageContent { get; set; }

        public int LanguageType { get; set; }

        public string[] Ids { get; set; }

    }
}
