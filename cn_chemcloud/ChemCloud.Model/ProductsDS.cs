using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class ProductsDS : BaseModel
    {
        public ProductsDS() { }
        /// <summary>
        /// 编号
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
        /// 店铺编号
        /// </summary>
        public long ShopId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 状态0审核中1平台代售中2平台未代售 默认0
        /// </summary>
        public int DSStatus { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime DSTime { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        [NotMapped]
        public string sellerusername { get; set; }
        /// <summary>
        /// cas
        /// </summary>
        [NotMapped]
        public string cas { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [NotMapped]
        public string productName { get; set; }
        /// <summary>
        /// 产品货号
        /// </summary>
        [NotMapped]
        public string productCode { get; set; }
    }
}
