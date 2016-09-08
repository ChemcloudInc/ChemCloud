using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class SiteMessage : BaseModel
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
        public long ShopId
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public string MessageContent
        { 
            get;
            set;
        }
        
        public DateTime? SendDate
        {
            get;
            set;
        }
        
        public SiteMessage()
        {

        }
        public SiteMessage(SiteMessage s): this()
        {
            Id = s.Id;
            ShopId = s.ShopId;
            MessageContent = s.MessageContent;
            SendDate = s.SendDate;
        }
        
    }
}
