using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class THQuery : QueryBase
    {
        public THQuery() { }
        /// <summary>
        /// 订单号
        /// </summary>
        public long orderNum { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endtime { get; set; }
        /// <summary>
        /// 采购商用户编号
        /// </summary>
        public long userid { get; set; }
        /// <summary>
        /// 采购商用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 采购商用户类型
        /// </summary>
        public int usertype { get; set; }
        /// <summary>
        /// 供应商用户编号
        /// </summary>
        public long muserid { get; set; }
        /// <summary>
        /// 供应商用户名
        /// </summary>
        public string musername { get; set; }
        /// <summary>
        /// 供应商用户类型
        /// </summary>
        public int musertype { get; set; }        
        /// <summary>
        /// 币种类型
        /// </summary>
        public int moneytype { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public int Status { get; set; }
    }
}
