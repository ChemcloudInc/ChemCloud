using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemCloud.Model
{
    /// <summary>
    /// 财务 转账
    /// </summary>
    public class Finance_Transfer : BaseModel
    {
        public Finance_Transfer() { }
        private long _id;
        /// <summary>
        /// 编号(自动标识)
        /// </summary>
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
        /// 转账单号
        /// </summary>
        public long Trans_Number
        {
            set;
            get;
        }
        /// <summary>
        /// 转账人
        /// </summary>
        public long Trans_UserId
        {
            set;
            get;
        }

        /// <summary>
        ///  转账名称
        /// </summary>
        [NotMapped]
        public string Trans_UserName { get; set; }

        /// <summary>
        /// 转账人类型
        /// </summary>
        public int Trans_UserType
        {
            set;
            get;
        }
        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal Trans_Money
        {
            set;
            get;
        }
        /// <summary>
        /// 转账手续费
        /// </summary>
        public decimal Trans_SXMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 转账币种
        /// </summary>
        public int Trans_MoneyType
        {
            set;
            get;
        }
        /// <summary>
        /// 转账时间
        /// </summary>
        public DateTime Trans_Time
        {
            set;
            get;
        }
        /// <summary>
        /// 转账IP
        /// </summary>
        public string Trans_Address
        {
            set;
            get;
        }
        /// <summary>
        /// 转账接收人
        /// </summary>
        public long Trans_ToUserId
        {
            set;
            get;
        }

        /// <summary>
        ///  转账接收人名称
        /// </summary>
        [NotMapped]
        public string Trans_ToUserName { get; set; }

        /// <summary>
        /// 转账接收人类型
        /// </summary>
        public int Trans_ToUserType
        {
            set;
            get;
        }
        /// <summary>
        /// 转账状态(0失败1已成功默认1)
        /// </summary>
        public int Trans_Status
        {
            set;
            get;
        }
    }
}
