using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class MeetingInfo : BaseModel
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
        /// 会议时间
        /// </summary>
        public DateTime MeetingTime { get; set; }
        /// <summary>
        /// 会议地点
        /// </summary>
        public string MeetingPlace { get; set; }
        /// <summary>
        /// 会议内容
        /// </summary>
        public string MeetingContent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatDate { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 会议持续时间
        /// </summary>
        public string ContinueTime { get; set; }

        public int LanguageType { get; set; }
    }


    public class Result_MeetingInfo :BaseModel
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
        public string MeetingTime { get; set; }
        /// <summary>
        /// 会议地点
        /// </summary>
        public string MeetingPlace { get; set; }
        /// <summary>
        /// 会议内容
        /// </summary>
        public string MeetingContent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatDate { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long UserId { get; set; }


        public string ContinueTime { get; set; }

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
    public class Result_Model_AttachmentList_PreNextRow
    {
        public Result_MeetingInfo Model { get; set; }

        public Result_List<Result_AttachmentInfo> AttachmentList { get; set; }

        public Result_List<Result_Model<MeetingInfo>> PreNextRow { get; set; }

        public Result_Msg Msg { get; set; }

    }
    #endregion

}
