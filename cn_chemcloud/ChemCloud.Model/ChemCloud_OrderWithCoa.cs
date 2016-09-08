using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ChemCloud_OrderWithCoa : BaseModel
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

        public string ProductCode { get; set; }

        public string CoaNo { get; set; }
    }
}
