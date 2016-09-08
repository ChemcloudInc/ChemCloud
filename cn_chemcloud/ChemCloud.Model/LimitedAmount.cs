using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class LimitedAmount : BaseModel
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
        public long RoleId
        {
            get;
            set;
        }
        public decimal Money
        {
            get;
            set;
        }
        public int CoinType
        {
            get;
            set;
        }
        
        [NotMapped]
        public string CoinTypeName
        {
            get;
            set;
        }
    }
}
