using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class OrderMoneyModel
    {
        public OrderMoneyModel() { }

        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public decimal TradingPrice { get; set; }
    }
}
