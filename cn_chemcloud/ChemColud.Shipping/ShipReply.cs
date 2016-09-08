using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    /// <summary>
    /// 创建运单响应
    /// </summary>
    public class ShipReply
    {
        /// <summary>
        /// 物流单号FormId
        /// </summary>
        public string TrackFormId { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string TrackNumber { get; set; }

        /// <summary>
        /// 是否是主单
        /// </summary>
        public bool Master { get; set; }

        public string MasterTrackFormId { get; set; }

        public string MasterTrackNumber { get; set; }

        /// <summary>
        /// 面单数据
        /// </summary>
        public byte[] LabelImage { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
