using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    /// <summary>
    ///   Rate
    /// </summary>
    public class Rate 
    {
        /// <summary>
        ///    创建rate对象
        /// </summary>
        /// <param name="provider">服务</param>
        /// <param name="providerCode">服务code</param>
        /// <param name="name">名称</param>
        /// <param name="totalCharges">费用</param>
        /// <param name="delivery">交货日期</param>
        public Rate(string provider, string providerCode, string name, decimal totalCharges, DateTime? delivery)
        {
            Provider = provider;
            ProviderCode = providerCode;
            Name = name;
            TotalCharges = totalCharges;
            GuaranteedDelivery = delivery;
            GurDeliveryDate = delivery.HasValue?Convert.ToDateTime(delivery).ToString("yyyy-MM-dd"):string.Empty;
        }

        public Rate()
        {

        }

        /// <summary>
        ///     交货日期
        /// </summary>
        public DateTime? GuaranteedDelivery { get; set; }

        public string GurDeliveryDate { get; set; }
        /// <summary>
        ///   名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///    计算方式
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        ///    
        /// </summary>
        public string ProviderCode { get; set; }
        /// <summary>
        ///    费用
        /// </summary>
        public decimal TotalCharges { get; set; }

       

       

        public override string ToString()
        {
            return Provider + Environment.NewLine + "\t" + ProviderCode + Environment.NewLine + "\t" + Name + Environment.NewLine + "\t" + TotalCharges + Environment.NewLine + "\t" + GuaranteedDelivery;
        }
    }
}
