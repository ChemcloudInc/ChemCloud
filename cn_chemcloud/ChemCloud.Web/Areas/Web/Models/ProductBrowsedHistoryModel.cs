using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Web.Models
{
    public class ProductBrowsedHistoryModel
    {
        public DateTime BrowseTime
        {
            get;
            set;
        }

        public string ImagePath
        {
            get;
            set;
        }

        public long ProductId
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }

        public decimal ProductPrice
        {
            get;
            set;
        }

        public long UserId
        {
            get;
            set;
        }

        public string CASNo { get; set; }

        public ProductBrowsedHistoryModel()
        {
        }
    }
}