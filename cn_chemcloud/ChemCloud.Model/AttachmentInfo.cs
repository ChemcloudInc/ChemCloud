using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public  class AttachmentInfo : BaseModel
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
        /// 附件相对应表的ID
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 附件URL相对路径
        /// </summary>
        public string AttachmentName { get; set; }
        /// <summary>
        /// 附件类型
        /// 1、会议中心2、法律法规3、技术交易中心4、定制合成5、代理采购
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 上传人的ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        
        [NotMapped]
        public List<AttachmentInfo> _AttachmentInfos { get; set; }
    }



    public class Result_AttachmentInfo : BaseModel
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
        /// 附件相对应表的ID
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 附件URL相对路径
        /// </summary>
        public string AttachmentName { get; set; }
        /// <summary>
        /// 附件类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 上传人的ID
        /// </summary>
        public long UserId { get; set; }

        public string FileName { get; set; }

    }

}
