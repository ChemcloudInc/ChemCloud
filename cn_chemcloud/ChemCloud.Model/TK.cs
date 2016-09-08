using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class TK : BaseModel
    {
        private long _id;
        /// <summary>
        /// 编号(自动标识)
        /// </summary>
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
        public string TKResion { get; set; }
        public long OrderId { get; set; }
        public decimal TKAmont { get; set; }
        public string TKInstruction { get; set; }
        /// <summary>
        /// 1 退款中 2仲裁中 3 取消
        /// </summary>
        public int TKType { get; set; }
        public DateTime TKDate { get; set; }
        public DateTime EndDate { get; set; }

        public long UserId { get; set; }
        public long SellerUserId { get; set; }
        [NotMapped]
        public string BuyerName { get; set; }
        [NotMapped]
        public string SellerName { get; set; }

        public int ReasonType { get; set; }

    }
}
