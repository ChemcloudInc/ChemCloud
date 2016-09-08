using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class AttentionQuery : QueryBase
    {
        public AttentionQuery() { }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endTime { get; set; }
        public long? UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string compamyName { get; set; }
        public long? ShopId
        {
            get;
            set;
        }
       
        public long? ProductId
        {
            get;
            set;
        }
        
    }
}
