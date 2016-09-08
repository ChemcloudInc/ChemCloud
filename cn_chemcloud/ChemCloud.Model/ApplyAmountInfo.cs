using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ApplyAmountInfo : BaseModel
    {
        public ApplyAmountInfo() { }
        /// <summary>
        /// 自动标识
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
        /// 申请人
        /// </summary>
        public long ApplyUserId
        {
            get;
            set;
        }
        /// <summary>
        /// 申请金额
        /// </summary>
        public decimal ApplyAmount
        {
            get;
            set;
        }

        public long OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDate
        {
            get;
            set;
        }
        /// <summary>
        /// 限额审核状态 0未审核 1审核通过 2审核不通过
        /// </summary>
        public int ApplyStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuthDate
        {
            get;
            set;
        }
        /// <summary>
        /// 审核人
        /// </summary>
        public long AuthorId
        {
            get;
            set;
        }
        /// <summary>
        /// 申请人名称
        /// </summary>
        [NotMapped]
        public string ApplyName
        {
            get;
            set;
        }
        /// <summary>
        /// 审核人名称
        /// </summary>
        [NotMapped]
        public string AuthorName
        {
            get;
            set;
        }
        /// <summary>
        /// 货币类型
        /// </summary>
        public int CoinType
        {
            get;
            set;
        }
        /// <summary>
        /// 货币名称
        /// </summary>
        [NotMapped]
        public string CoinName
        {
            get;
            set;
        }
        /// <summary>
        /// 申请人（0：子账户申请；1：我的申请）
        /// </summary>
        [NotMapped]
        public int Applicant
        {
            get;
            set;
        }
    }
}
