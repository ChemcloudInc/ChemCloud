using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class SiteSettingsInfo : BaseModel
    {
        private long _id;

        [NotMapped]
        public decimal AdvancePaymentLimit
        {
            get;
            set;
        }

        [NotMapped]
        public decimal AdvancePaymentPercent
        {
            get;
            set;
        }

        [NotMapped]
        public string CustomerTel
        {
            get;
            set;
        }

        [NotMapped]
        public string FlowScript
        {
            get;
            set;
        }

        [NotMapped]
        public string Hotkeywords
        {
            get;
            set;
        }

        [NotMapped]
        public string ICPNubmer
        {
            get;
            set;
        }

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

        public string Key
        {
            get;
            set;
        }

        [NotMapped]
        public string Keyword
        {
            get;
            set;
        }

        [NotMapped]
        public string Logo
        {
            get;
            set;
        }

        [NotMapped]
        public string MemberLogo
        {
            get;
            set;
        }

        [NotMapped]
        public bool MobileVerifOpen
        {
            get;
            set;
        }

        public int NoReceivingTimeout
        {
            get;
            set;
        }

        [NotMapped]
        public string PageFoot
        {
            get;
            set;
        }

        public int ProdutAuditOnOff
        {
            get;
            set;
        }

        [NotMapped]
        public string QRCode
        {
            get;
            set;
        }

        public int SalesReturnTimeout
        {
            get;
            set;
        }

        public string SellerAdminAgreement
        {
            get;
            set;
        }

        [NotMapped]
        public string Site_SEODescription
        {
            get;
            set;
        }

        [NotMapped]
        public string Site_SEOKeywords
        {
            get;
            set;
        }

        [NotMapped]
        public string Site_SEOTitle
        {
            get;
            set;
        }

        [NotMapped]
        public bool SiteIsClose
        {
            get;
            set;
        }

        [NotMapped]
        public string SiteName
        {
            get;
            set;
        }

        public int UnpaidTimeout
        {
            get;
            set;
        }

        [NotMapped]
        public string UserCookieKey
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        [NotMapped]
        public int WeekSettlement
        {
            get;
            set;
        }

        [NotMapped]
        public string WeixinAppId
        {
            get;
            set;
        }

        [NotMapped]
        public string WeixinAppSecret
        {
            get;
            set;
        }

        [NotMapped]
        public bool WeixinIsValidationService
        {
            get;
            set;
        }

        [NotMapped]
        public string WeixinLoginUrl
        {
            get;
            set;
        }

        [NotMapped]
        public string WeixinPartnerID
        {
            get;
            set;
        }

        [NotMapped]
        public string WeixinPartnerKey
        {
            get;
            set;
        }

        [NotMapped]
        public string WeixinToken
        {
            get;
            set;
        }

        [NotMapped]
        public string WX_MSGGetCouponTemplateId
        {
            get;
            set;
        }

        [NotMapped]
        public string WXLogo
        {
            get;
            set;
        }

        [NotMapped]
        public string PlatformCollectionAddress
        {
            get;
            set;
        }


        [NotMapped]
        public string InsurancefeeMaxValue
        {
            get;
            set;
        }
        [NotMapped]
        public string MaterialMallURL
        {
            get;
            set;
        }
        [NotMapped]
        public string BBSURL
        {
            get;
            set;
        }
        [NotMapped]
        public string ImageFilePath { get; set; }
        [NotMapped]
        public string RecommandIndexImg { get; set; }

        [NotMapped]
        /// <summary>
        /// 代理采购金额
        /// </summary>
        public string PurchasingAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// 定制合成金额
        /// </summary>
        public string SynthsisAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// 供应商认证支付金额
        /// </summary>
        public string SupplierCertificationAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// 产品认证支付金额
        /// </summary>
        public string ProductCertificationAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// 排名付费金额
        /// </summary>
        public string SortCost { get; set; }

        #region 首页菜单显示配置 by xzg
        [NotMapped]
        /// <summary>
        /// 耗材商城显示配置
        /// </summary>
        public bool isShowHaoCai { get; set; }
        [NotMapped]
        /// <summary>
        /// 会议中心显示配置
        /// </summary>
        public bool isShowHuiYiZhongXin { get; set; }
        [NotMapped]
        /// <summary>
        /// 大数据中心显示配置
        /// </summary>
        public bool isShowDaShujuZhongXin { get; set; }
        [NotMapped]
        /// <summary>
        /// 技术交易中心显示配置
        /// </summary>
        public bool isShowJiShuJiaoYiZhongXin { get; set; }
        [NotMapped]
        /// <summary>
        /// 人才市场显示配置
        /// </summary>
        public bool isShowRenCaiShiChang { get; set; }
        [NotMapped]
        /// <summary>
        /// 论坛显示配置
        /// </summary>
        public bool isShowLunTan { get; set; }

        [NotMapped]
        /// <summary>
        /// 论坛显示配置
        /// </summary>
        public bool isShowFaLvFaGui { get; set; }
        #endregion
        [NotMapped]
        /// <summary>
        /// 技术资料联系人
        /// </summary>
        public string techName { get; set; }

        [NotMapped]
        /// <summary>
        /// 技术资料联系人电话
        /// </summary>
        public string techTel { get; set; }

        [NotMapped]
        /// <summary>
        /// 技术资料联系人Email
        /// </summary>
        public string techEmail { get; set; }

        [NotMapped]
        /// <summary>
        /// 昨日成交金额亿元
        /// </summary>
        public int FirstAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// 昨日成交金额万元
        /// </summary>
        public int SecondAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// 累计成交金额亿元
        /// </summary>
        public int leijiFirstAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// 累计成交金额万元
        /// </summary>
        public int leijiSecondAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// 网站运行时间
        /// </summary>
        public DateTime Runtime { get; set; }
        public SiteSettingsInfo()
        {
        }

        /// <summary>
        /// 服务热线
        /// </summary>
        public string PlatCall { get; set; }

        public string Operationreport { get; set; }
    }
}