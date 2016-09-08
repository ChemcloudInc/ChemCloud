using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class TechnicalInfo : BaseModel
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
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 技术资料内容
        /// </summary>
        public string TechContent { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int Status { get; set; }
       
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public long Auditor { get; set; }

        public long PublisherId { get; set; }

        public int LanguageType { get; set; }

        [NotMapped]
        public string Language { get; set; }
        [NotMapped]
        public string AuditorName { get; set; }
        [NotMapped]
        public string PublisherName { get; set; }
    }
    public class Result_TechnicalInfo : BaseModel
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
        /// <summary>
        /// 会议时间
        /// </summary>
        public string TechContent { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        public long UserId { get; set; }


        public string techName { get; set; }

        public string techTel { get; set; }

        public string techEmail { get; set; }

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
    public class Result_Model_TechAttachmentList_PreNextRow
    {
        public Result_TechnicalInfo Model { get; set; }

        public Result_List<Result_AttachmentInfo> AttachmentList { get; set; }

        public Result_List<Result_Model<TechnicalInfo>> PreNextRow { get; set; }

        public Result_Msg Msg { get; set; }

    }
    #endregion
}
