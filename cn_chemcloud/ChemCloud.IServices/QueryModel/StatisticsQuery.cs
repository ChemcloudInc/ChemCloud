using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class StatisticsQuery : QueryBase
    {
        public StatisticsQuery() { }
        public int OrderBy
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string userType { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal addMoney { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal removeMoney { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long userid { get; set; }
        /// <summary>
        /// shopid
        /// </summary>
        public long shopId { get; set; }
    }
}
