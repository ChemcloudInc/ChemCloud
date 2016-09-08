using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class ProductLevelQuery : QueryBase
    {
        public long Id
        {
            get;
            set;
        }
        public string ProductLevelCN
        {
            get;
            set;
        }
        public string ProductLevelEN
        {
            get;
            set;
        }
    }
}
