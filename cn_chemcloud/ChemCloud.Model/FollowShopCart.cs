using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class FollowShopCart
    {
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

        public int Quantity { get; set; }

        public FollowShopCart()
        {
        }
    }
}