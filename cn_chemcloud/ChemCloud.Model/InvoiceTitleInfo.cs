using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemCloud.Model
{
    public class InvoiceTitleInfo : BaseModel
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

        public byte IsDefault
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public long UserId
        {
            get;
            set;
        }

        public InvoiceTitleInfo()
        {
        }
    }
}