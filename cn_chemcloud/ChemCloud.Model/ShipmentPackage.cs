using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class ShipmentPackage : BaseModel
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


        public Int64 ShipmentId { get; set; }

      

        public decimal Weight { get; set; }

        public decimal Length { get; set; }

        public string ShipWeightUnit { get; set; }

        public string Currency { get; set; }

        public string PackageType { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public decimal InsuredValue { get; set; }

        public string ShipLinearUnit { get; set; }
    }
}
