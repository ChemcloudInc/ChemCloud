using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class ProductAuthentication : BaseModel
    {
        public ProductAuthentication() { }
        /// <summary>
        /// 编号 自动标识
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
        /// 产品编号
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public long ManageId { get; set; }
        /// <summary>
        /// 货号
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 电汇凭证
        /// </summary>
        public string ProductIMG { get; set; }
        /// <summary>
        /// 货号说明
        /// </summary>
        public string ProductDesc { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime ProductAuthDate { get; set; }
        /// <summary>
        /// 审核状态 0已提交 1已确认 2已支付 3审核通过 4审核拒绝  
        /// </summary>
        public int AuthStatus { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public long AuthAuthor { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuthTime { get; set; }
        /// <summary>
        /// 审核说明
        /// </summary>
        public string AuthDesc { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string ComName { get; set; }
        /// <summary>
        /// 附件（COA产品分析报告）
        /// </summary>
        public string ComAttachment { get; set; }
    }
}
