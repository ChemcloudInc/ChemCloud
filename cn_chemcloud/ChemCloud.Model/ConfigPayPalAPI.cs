using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class ConfigPayPalAPI : BaseModel
    {
        public ConfigPayPalAPI() { }

        /// <summary>
        /// 自动标识
        /// </summary>
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
        /// <summary>
        /// API帐户
        /// </summary>
        public string PayPalId { get; set; }
        /// <summary>
        /// API密码
        /// </summary>
        public string PayPalPwd { get; set; }
        /// <summary>
        /// API标识
        /// </summary>
        public string PayPalSinature { get; set; }
        /// <summary>
        /// 环境 live  sandbox
        /// </summary>
        public string PayPalEnvenment { get; set; }
    }
}
