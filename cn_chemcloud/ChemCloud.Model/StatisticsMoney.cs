using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class StatisticsMoney : BaseModel
    {
        public StatisticsMoney() { }

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

        public long UserId { get; set; }

        public string UserName { get; set; }
        public int UserType { get; set; }
        public int TradingType { get; set; }
        public DateTime TradingTime { get; set; }
        public string TradingOrderId { get; set; }
        public decimal TradingPrice { get; set; }
        public string OrderNum { get; set; }
        public int PayType { get; set; }

        public decimal Balance { get; set; }

        public decimal BalanceAble { get; set; }

        public string TradingCardNum { get; set; }
    }
}
