using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemCloud.Model
{
    /// <summary>
    /// 财务 钱包
    /// </summary>
    public class Finance_Wallet : BaseModel
    {
        public Finance_Wallet() { }
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
        /// 用户编号
        /// </summary>
        public long Wallet_UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 用户类型(0平台2卖家3买家)
        /// </summary>
        public int Wallet_UserType
        {
            set;
            get;
        }
        /// <summary>
        /// 钱包余额
        /// </summary>
        public decimal Wallet_UserMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 锁定金额
        /// </summary>
        public decimal Wallet_UserMoneyLock 
        { 
            get; set; 
        }
        /// <summary>
        /// 钱包可用余额
        /// </summary>
        public decimal Wallet_UserLeftMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 用户开户行
        /// </summary>
        public string Wallet_UserBankName
        {
            set;
            get;
        }
        /// <summary>
        /// 开户行卡号
        /// </summary>
        public string Wallet_UserBankNumber
        {
            set;
            get;
        }
        /// <summary>
        /// 开户行地址
        /// </summary>
        public string Wallet_UserBankAddress
        {
            set;
            get;
        }
        /// <summary>
        /// 开户的法人姓名
        /// </summary>
        public string Wallet_UserBankUserName
        {
            set;
            get;
        }
        /// <summary>
        /// 最后次操作时间
        /// </summary>
        public DateTime Wallet_DoLastTime
        {
            set;
            get;
        }
        /// <summary>
        /// 最后次操作地点/IP
        /// </summary>
        public string Wallet_DoIpAddress
        {
            set;
            get;
        }
        /// <summary>
        /// 最后次操作人名字
        /// </summary>
        public string Wallet_DoUserName
        {
            set;
            get;
        }
        /// <summary>
        /// 最后次操作人编号
        /// </summary>
        public long Wallet_DoUserId
        {
            set;
            get;
        }
        /// <summary>
        /// 钱包币种
        /// </summary>
        public int Wallet_MoneyType
        {
            set;
            get;
        }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Wallet_PayPassword
        {
            get;
            set;
        }
        /// <summary>
        /// 钱包状态(0锁定1正常默认1)
        /// </summary>
        public int Wallet_Status
        {
            set;
            get;
        }
    }
}
