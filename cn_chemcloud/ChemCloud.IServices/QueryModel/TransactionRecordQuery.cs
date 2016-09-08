using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class TransactionRecordQuery : QueryBase
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurveType { get; set; }
    }

    public class TransactionRecordAddQuery : QueryBase
    {

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

    public class TransactionRecordModifyQuery : TransactionRecordAddQuery
    {
        public long ID { get; set; }

    }


    public class TransactionRecordCompute : QueryBase
    {
        public DateTime YearMonth { get; set; }
        public int CurveType { get; set; }
        public int Id { get; set; }

    }
}
