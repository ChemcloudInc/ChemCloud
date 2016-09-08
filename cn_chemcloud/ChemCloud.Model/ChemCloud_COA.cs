using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ChemCloud_COA : BaseModel
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

        public string CertificateNumber { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public DateTime DateofManufacture { get; set; }

        [NotMapped]
        public string strDateofManufacture { get; set; }

        public string Supplier { get; set; }

        public string ManufacturersBatchNo { get; set; }

        public string SydcosLabBatchNo { get; set; }

        public DateTime ExpiryDate { get; set; }

        [NotMapped]
        public string strExpiryDate { get; set; }

        public DateTime DATEOFRELEASE { get; set; }

        [NotMapped]
        public string strDATEOFRELEASE { get; set; }

        public string LABORATORYMANAGER { get; set; }

        [NotMapped]
        public List<ChemCloud_COADetails> ChemCloud_COADetails { get; set; }
    }

    public class ChemCloud_COADetails : BaseModel
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

        public string CertificateNumber { get; set; }

        public string Appearance { get; set; }

        public string IIdentity { get; set; }

        public string Carbonates { get; set; }

        public string Solubility { get; set; }

        public string Ammonium { get; set; }

        public string Calcium { get; set; }

        public string Arsenic { get; set; }

        public string HeavyMetals { get; set; }

        public string Iron { get; set; }

        public string Sulphates { get; set; }

        public string Chlorides { get; set; }

        public string Appearanceofsolution { get; set; }

        public string MicroStatus { get; set; }
    }
}
