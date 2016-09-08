using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class TransactionRecord : BaseModel
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

        public string XName_CN { get; set; }
        public string XName_Eng { get; set; }
        public decimal Y_CompleteAmount { get; set; }
        public decimal Y_OrderAmount { get; set; }
        public int Y_CompleteNumber { get; set; }
        public int Y_OrderQuantity { get; set; }
        public int CurveType { get; set; }
        public DateTime YearMonth { get; set; }
        public int IsTrue { get; set; }
    }


    public class Result_TransactionRecord : BaseModel
    {

        public string XName_CN { get; set; }
        public string XName_Eng { get; set; }
        public decimal Y_CompleteAmount { get; set; }
        public decimal Y_OrderAmount { get; set; }
        public int Y_CompleteNumber { get; set; }
        public int Y_OrderQuantity { get; set; }
        public string CurveType { get; set; }
        public string CurveTypeName { get; set; }
        public string YearMonth { get; set; }
        public int IsTrue { get; set; }

    }

}
