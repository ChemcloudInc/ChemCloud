using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class ShipmentAddress : BaseModel
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

        public long ShipmentId { get; set; }

        public string State { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }
    }
}
