using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class MessageSettingQuery : QueryBase
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public long? Id
        {
            get;
            set;
        }
        public MessageSetting.MessageModuleStatus? MessageNameId
        {
            get;
            set;
        }
        public int Languagetype
        {
            get;
            set;
        } 
        public DateTime? CreatDate
        {
            get;
            set;
        }
        public int? ActiveStatus 
        { 
            get; 
            set; 
        }
        public int? Status
        {
            get;
            set;
        }
     
        public MessageSettingQuery()
        {

        }
    }
    public class MessageSettingAddQuery : QueryBase
    {
        public int MessageNameId { get; set; }
        public string MessageContent { get; set; }
        public int LanguageType { get; set; }       
    }
    public class MessageSettingModifyQuery : MessageSettingAddQuery
    {
        public long Id { get; set; }

    }
}
