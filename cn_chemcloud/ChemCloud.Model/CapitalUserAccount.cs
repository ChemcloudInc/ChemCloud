using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class CapitalUserAccount : BaseModel
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
        public long userid { get; set; }
        public string CashAccount { get; set; }
        public int CashAccountType { get; set; }
        /// <summary>
        /// 用户类型 3采购商  2供应商
        /// </summary>
        public int userType { get; set; }
        public CapitalUserAccount() { }
    }
}
