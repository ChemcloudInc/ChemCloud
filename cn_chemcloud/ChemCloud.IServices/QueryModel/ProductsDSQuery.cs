using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class ProductsDSQuery : QueryBase
    {
        public ProductsDSQuery()
        {

        }
        /// <summary>
        /// 代售编号
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public long productid { get; set; }
        /// <summary>
        /// 店铺编号
        /// </summary>
        public long shopid { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long userid { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int usertype { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int dsstatus { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime dstime { get; set; }
        /// <summary>
        /// CAS
        /// </summary>
        public string CAS { get; set; }
        /// <summary>
        /// 产品货号
        /// </summary>
        public string productcode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string productname { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string sellerusername { get; set; }
    }
}