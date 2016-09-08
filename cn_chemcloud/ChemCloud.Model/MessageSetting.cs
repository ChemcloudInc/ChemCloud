using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class MessageSetting : BaseModel
    {
        /// <summary>
        /// 模板ID
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
        /// 消息内容
        /// </summary>
        public string MessageContent
        {
            get;
            set;
        }


        public MessageSetting.MessageModuleStatus MessageNameId
        {
            get;
            set;
        }

        public DateTime? CreatDate
        {
            get;
            set;
        }

        
        public int Languagetype
        {
            get;
            set;
        }

        public int ActiveStatus
        {
            get;
            set;
        }
        [NotMapped]
        public string LanguageName
        {
            get;
            set;
        }
        [NotMapped]
        public string MessageTitle
        {
            get;
            set;
        }
        public enum MessageModuleStatus
        {
            [Description("订单创建时")]
            OrderCreated = 1,
            [Description("订单付款时")]
            OrderPay = 2,
            [Description("订单发货")]
            OrderShipping = 3,
            [Description("订单退款")]
            OrderRefund = 4,
            //[Description("找回密码")]
            //FindPassWord = 5,
            [Description("供应商审核")]
            ShopAudited = 6,
            [Description("供应商审核结果")]
            ShopResult = 7,
            [Description("实地认证申请")]
            CertificationApply = 8,
            [Description("确认付款")]
            ConfirmPay = 9,
            [Description("认证结果")]
            CertificationResult = 10,
            [Description("快递收货")]
            LogisticsRece = 11,
            [Description("物流通关")]
            LogisticsClearance = 12,
            [Description("采购商签收")]
            LogisticsSignIn = 13,
            [Description("采购商注册")]
            MemberRegister = 14,
            [Description("供应商入驻")]
            SupplierRegister = 15,
            [Description("货款到账")]
            GoodsPayment = 16,
            [Description("采购商注册成功，邮件内容")]
            RegisterMailContent = 17,
            [Description("供应商注册成功，邮件内容")]
            RegisterMailContent_GYS = 18,
            [Description("询盘")]
            XunPan = 19,
            [Description("自定义消息")]
            CustomMessage = 20,
            [Description("限额审核")]
            LimitedAount = 21,
            [Description("限额审核通过")]
            ApplyPass = 22,
            [Description("限额审核未通过")]
            ApplyNoPass = 23
        }


        public MessageSetting()
        {

        }
        public MessageSetting(MessageSetting m)
            : this()
        {
            Id = m.Id;
            MessageNameId = m.MessageNameId;
            MessageContent = m.MessageContent;
            CreatDate = m.CreatDate;
            ActiveStatus = m.ActiveStatus;
        }
    }
}
