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
    public class MessageSettingModel 
    {
        public long Id
        {
            get;
            set;
        }
        
        public string MessageContent
        {
            get;
            set;
        }
        
        public string MessageNameId
        {
            get;
            set;
        }
        public DateTime? CreatDate
        {
            get;
            set;
        }
        public int Languagetype
        {
            get;
            set;
        }
        public string LanguageName
        {
            get;
            set;
        }
        public int ActiveStatus
        {
            get;
            set;
        }
        public int TitleId
        {
            get;
            set;
        }
        public MessageSettingModel()
        {

        }
        
        public MessageSettingModel(MessageSetting m):this()
        {
            Id = m.Id;
            MessageNameId = m.MessageNameId.ToDescription();
            MessageContent = m.MessageContent;
            Languagetype = m.Languagetype;
            LanguageName = m.LanguageName;
            CreatDate = m.CreatDate;
            ActiveStatus = m.ActiveStatus;
            TitleId = (int)m.MessageNameId;
        }
    }

}