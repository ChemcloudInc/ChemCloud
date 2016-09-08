using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{

    public class ProductSpec : BaseModel
    {
        public long CoinType { get; set; }

        [NotMapped]
        public string CoinTypeName { get; set; }

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
        public long ProductId
        {
            get;
            set;
        }
        public string Packaging
        {
            get;
            set;
        }
        public string Purity
        {
            get;
            set;
        }
        public Decimal Price
        {
            get;
            set;
        }

        //等级
        public string SpecLevel
        {
            get;
            set;
        }
    }


    public class ProductInfoSpec
    {
        public long CoinType { get; set; }
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string Packaging { get; set; }
        public string Purity { get; set; }
        public Decimal Price { get; set; }
        public string SpecLevel { get; set; }
    }
}
