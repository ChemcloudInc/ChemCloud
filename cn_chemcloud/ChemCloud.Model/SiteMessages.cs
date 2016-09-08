using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class SiteMessages : BaseModel
    {
        /// <summary>
        /// 消息ID
        /// </summary>
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
        /// <summary>
        /// 消息接收人ID
        /// </summary>
        public long? MemberId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? SendTime
        {
            get;
            set;
        }
        /// <summary>
        ///阅读时间
        /// </summary>
        public DateTime? ReadTime
        {
            get;
            set;
        }
        public string SendName
        {
            get;
            set;
        }
        /// <summary>
        /// 阅读状态
        /// </summary>
        public SiteMessages.Status ReadStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 供应商名称
        /// 不映射
        /// </summary>
        [NotMapped]
        public string ReceiveName
        {
            get;
            set;
        }
        public string MessageContent
        {
            get;
            set;
        }
        /// <summary>
        /// 获取枚举初始状态
        /// 不映射
        /// </summary>
        [NotMapped]
        public SiteMessages.Status ShowReadStatus
        {
            get
            {
                SiteMessages.Status readStatus = SiteMessages.Status.UnRead;
                if (this != null)
                {
                    readStatus = ReadStatus;
                }
                return readStatus;
            }
        }
        public SiteMessages.ReceiveType ReceType
        {
            get;
            set;
        }
        [NotMapped]
        public SiteMessages.ReceiveType ShowRece
        {
            get
            {
                SiteMessages.ReceiveType receType = SiteMessages.ReceiveType.PlatformOperation;
                if(this != null)
                {
                    receType = ReceType;
                }
                return receType;
            }
        }
        public MessageSetting.MessageModuleStatus MessageModule
        {
            get;
            set;
        }
        public int IsDisplay
        {
            get;
            set;
        }
        [NotMapped]
        public MessageSetting.MessageModuleStatus ShowModule
        {
            get
            {
                MessageSetting.MessageModuleStatus Module = MessageSetting.MessageModuleStatus.OrderCreated;
                if (this != null)
                {
                    Module = MessageModule;
                }
                return Module;
            }
        }

        /// <summary>
        /// 阅读状态枚举
        /// </summary>
        public enum Status
        {
            [Description("未读")]
            UnRead = 1,
            [Description("已读")]
            Readed = 2
        }
        
        public enum ReceiveType
        {
            [Description("平台运营")]
            PlatformOperation = 1,
            [Description("供应商")]
            Supplier = 2,
            [Description("采购商")]
            Purchase = 3,
            [Description("所有平台运营")]
            AllPlatformOperation = 4,
            [Description("所有供应商")]
            AllSupplier = 5,
            [Description("所有采购商")]
            AllPurchase = 6
        }
        public SiteMessages()
        {
        }
        public SiteMessages(SiteMessages m): this()
        {
            Id = m.Id;
            MemberId = m.MemberId;
            SendTime = m.SendTime;
            ReadTime = m.ReadTime;
            ReadStatus = m.ReadStatus;
            ReceiveName = m.ReceiveName;
            SendName = m.SendName;
            MessageContent = m.MessageContent;
            MessageModule = m.MessageModule;
            ReceType = m.ReceType;
            IsDisplay = m.IsDisplay;
        }
    }
}
