using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    /// <summary>
    ///  物流信息
    /// </summary>
    public class ShipmentEx
    {

        public ICollection<IRateAdjuster> RateAdjusters;
        private readonly List<Rate> _rates;
        private Rate _rate;
        /// <summary>
        /// 收货地址
        /// </summary>
        public  Address DestinationAddress{get;set;}

        /// <summary>
        /// 寄货地址
        /// </summary>
        public Address OriginAddress { get; set; }

        public ShipmentEx(Address originAddress, Address destinationAddress, List<Package> packages)
        {
            OriginAddress = originAddress;
            DestinationAddress = destinationAddress;
            Packages = packages;
            _rates = new List<Rate>();
          
        }
        /// <summary>
        /// 包裹信息
        /// </summary>
        public List<Package> Packages{get;set;}

        
        public int PackageCount
        {
            get { return Packages.Count; }
        }

        public Rate RateValue
        {
            get;
            set;
        }
        public List<Rate> Rates
        {
            get { return _rates; }
        }
        public decimal TotalPackageWeight
        {
            get { return Packages.Sum(x => x.Weight); }
        }

        public string Currency { get; set; }

     
    }
}
