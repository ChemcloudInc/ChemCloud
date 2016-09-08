using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    /// <summary>
    /// 费率请求对象
    /// </summary>
    public class SFRateRequest
    {
        public string OriginCode { get; set; }

        public string DestCode { get; set; }

        public string ParentOriginCode { get; set; }

        public string ParentDestCode { get; set; }

        public decimal Weight { get; set; }

        public string Time { get; set; }

        public int Volume { get; set; }

        public int QueryType { get; set; }

        public string Lang { get; set; }

        public string Region { get; set; }
    }

    /// <summary>
    /// 费率响应对象
    /// </summary>
    public class SFRateRep
    {
        public string cargoTypeCode { get; set; }

        public string cargoTypeName { get; set; }

        public string currencyName { get; set; }

        public string destCurrencyName { get; set; }

        public decimal destFreight { get; set; }

        public decimal destFuelCost { get; set; }

        public decimal freight { get; set; }

        public decimal fuelCost { get; set; }

        public string limitTypeCode { get; set; }

        public string limitTypeName { get; set; }

        public decimal weight { get; set; }

        public string deliverTime { get; set; }

        public string distanceTypeCode { get; set; }

        public string distanceTypeName { get; set; }

        public string expectArriveTm { get; set; }

        public string otherService { get; set; }

        public bool internet { get; set; }

        public string closedTime { get; set; }

        public string orgionView { get; set; }

        public string destView { get; set; }
    }
}
