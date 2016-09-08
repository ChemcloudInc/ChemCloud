using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Models
{
    public class SiteMessagesModel
    {
        /// <summary>
        /// ID
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
        /// 供应商名称
        /// </summary>
        public string ReceiveName
        {
            get;
            set;
        }
        public DateTime? SendTime
        {
            get;
            set;
        }
        public DateTime? ReadTime
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }
        public string ReceType
        {
            get;
            set;
        }
        public string SendName
        {
            get;
            set;
        }
        public string MessageContent
        {
            get;
            set;
        }
        public string MessageModule
        {
            get;
            set;
        }
        public int IsDisplay
        {
            get;
            set;
        }
        public SiteMessagesModel()
        {

        }
        public SiteMessagesModel(SiteMessages m)
            : this()
        {
            Id = m.Id;
            MemberId = m.MemberId;
            SendTime = m.SendTime;
            ReadTime = m.ReadTime;
            Status = m.ReadStatus.ToDescription();
            ReceiveName = m.ReceiveName;
            MessageContent = m.MessageContent;
            MessageModule = m.MessageModule.ToDescription();
            ReceType = m.ReceType.ToDescription();
            IsDisplay = m.IsDisplay;
            SendName = m.SendName;
        }
    }
}