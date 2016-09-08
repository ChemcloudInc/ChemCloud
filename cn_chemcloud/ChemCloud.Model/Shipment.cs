using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class Shipment : BaseModel
    {
        private long _id;
        public new long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                base.Id = value;
            }
        }

        
        public long OrderId { get; set; }

        public int PackageCount { get; set; }

        public decimal TotalPackageWeight { get; set; }

        public DateTime GurDeliveryDate { get; set; }

        public string RateName { get; set; }

        public string RateProvider { get; set; }

        public string RateProviderCode { get; set; }

        public decimal TotalCharges { get; set; }
    }
}
