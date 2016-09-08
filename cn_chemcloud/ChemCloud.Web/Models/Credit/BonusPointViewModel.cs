using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemCloud.Web.Models.Credit
{
    /// <summary>
    /// 积分
    /// </summary>
    public class BonusPointViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int BonusPointUserID { get; set; }

        /// <summary>
        /// 商家 就是商家的名称 ，采购 是 采购人
        /// </summary>
        public string BonusPointUserName { get; set; }

        /// <summary>
        /// 积分值
        /// </summary>
        public int BonusPointValue { get; set; }
    }
}