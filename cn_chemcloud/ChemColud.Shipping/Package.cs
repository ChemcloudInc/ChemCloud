using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    /// <summary>
    ///    包裹描述
    /// </summary>
    public class Package
    {
        public Package()
        {

        }
      
        /// <summary>
        ///     创建包裹对象
        /// </summary>
        /// <param name="length">包裹长度</param>
        /// <param name="width">包裹宽度</param>
        /// <param name="height">包裹高度</param>
        /// <param name="weight">包裹重量</param>
        /// <param name="insuredValue">保险费用</param>
        public Package(decimal length, decimal width, decimal height, decimal weight, decimal insuredValue)
            : this(length, width, height, (decimal)weight, insuredValue, WeightUnit.KG, LinearUnit.CM, "CNY")
        {
        }

        /// <param name="length">包裹长度</param>
        /// <param name="width">包裹宽度</param>
        /// <param name="height">包裹高度</param>
        /// <param name="weight">包裹重量</param>
        /// <param name="insuredValue">保险费用</param>
        /// <param name="weiUnit">重量单位</param>
        /// <param name="linearUnit">长度单位</param>
        public Package(decimal length, decimal width, decimal height, decimal weight, decimal insuredValue, WeightUnit weiUnit, LinearUnit linearUnit)
            : this(length, width, height, (decimal)weight, insuredValue, weiUnit, linearUnit, "CNY")
        {
        }

        /// <summary>
        ///     创建包裹对象
        /// </summary>
        /// <param name="length">包裹长度</param>
        /// <param name="width">包裹宽度</param>
        /// <param name="height">包裹高度</param>
        /// <param name="weight">包裹重量</param>
        /// <param name="insuredValue">保险费用</param>
        /// <param name="weiUnit">重量单位</param>
        /// <param name="linearUnit">长度单位</param>
        /// <param name="currency">货币</param>
        public Package(decimal length, decimal width, decimal height, decimal weight, decimal insuredValue, WeightUnit weiUnit = WeightUnit.KG, LinearUnit linearUnit = LinearUnit.CM, string currency = "CNY")
        {
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            InsuredValue = insuredValue;
            ShipWeightUnit = weiUnit;
            ShipLinearUnit = linearUnit;
            Currency = currency;
        }

        public int PackageCount { get; set; }
        public string Currency
        {
            get;
            set;
        }
        public decimal CalculatedGirth
        {
            get
            {
                var result = (Width * 2) + (Height * 2);
                return Math.Ceiling(result);
            }
        }
        public decimal Height { get; set; }

        public string PackageType { get; set; }

        public WeightUnit ShipWeightUnit { get;set;}
        public LinearUnit ShipLinearUnit { get; set; }
        public decimal InsuredValue { get; set; }
        public bool IsOversize { get; set; }
        public decimal Length { get; set; }
        public decimal RoundedHeight
        {
            get { return Height; }
        }
        public decimal RoundedLength
        {
            get { return Length; }//Math.Ceiling
        }
        public decimal RoundedWeight
        {
            get { return Weight; }
        }
        public decimal RoundedWidth
        {
            get { return Width; }
        }
        public decimal Weight { get; set; }
        public decimal Width { get; set; }
        public string Container { get; set; }


        /// <summary>
        /// 包裹容量
        /// </summary>
        public int TotalSize { get; set; }
        

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
       
    }
}
