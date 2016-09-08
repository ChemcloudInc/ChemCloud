using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class ManagersAccount : BaseModel
    {
        public ManagersAccount() { }

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

        public decimal Zhuanzhang { get; set; }
        public string ZhuanzhangName { get; set; }
        public decimal Tiqu { get; set; }
        public decimal Tiqufeiyong { get; set; }
        public string Tiquhuobi { get; set; }
        public string Huilv { get; set; }
        public decimal Tuikuan { get; set; }
        public string OrderNum { get; set; }
        public decimal Balance { get; set; }
        public decimal BalanceAvailable { get; set; }
        public long ManagerId { get; set; }
        public DateTime Datatime { get; set; }

        public decimal Daikuan { get; set; }

    }
}
