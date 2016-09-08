using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class Finance_WalletQuery : QueryBase
    {
        public Finance_WalletQuery() { }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endtime { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long userid { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int usertype { get; set; }
        /// <summary>
        /// 币种类型
        /// </summary>
        public int moneytype { get; set; }
    }
}
