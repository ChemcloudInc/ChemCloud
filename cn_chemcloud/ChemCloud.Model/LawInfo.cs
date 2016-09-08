using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class LawInfo : BaseModel
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
        public string Author { get; set; }

        public string Title { get; set; }

        public string LawsInfo{get;set;}

        public int Type { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }

        public long Auditor { get; set; }

        public long UserId { get; set; }

        public int LanguageType { get; set; }

        [NotMapped]
        public string Language { get; set; }

        [NotMapped]
        public string AuditorName { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        
    }
    public class Result_LawInfo : BaseModel
    {

        private long _id;

        public long DisplaySequence
        {
            get;
            set;
        }

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
        /// 标题
        /// </summary>
        public string Title { get; set; }

        public string Author { get; set; }

        public string LawsInfo { get; set; }

        public int Type { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }

        public long Auditor { get; set; }

        public long UserId { get; set; }

        public int LanguageType { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string AttachmentName { get; set; }

    }
    #region 公共：返回 对象 + 双列表 + 提示信息 Result_Model
    /// <summary>
    /// 公共：返回 对象 + 双列表 + 提示信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result_Model_LawAttachmentList_PreNextRow
    {
        public Result_LawInfo Model { get; set; }

        public Result_List<Result_AttachmentInfo> AttachmentList { get; set; }

        public Result_List<Result_Model<LawInfo>> PreNextRow { get; set; }

        public Result_Msg Msg { get; set; }

    }
    #endregion
}
