using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Models
{
    public class SiteSettingModel
    {
        public decimal AdvancePaymentLimit
        {
            get;
            set;
        }

        public decimal AdvancePaymentPercent
        {
            get;
            set;
        }

        public string Keyword { get; set; }
        public string Hotkeywords { get; set; }

        public string CustomerTel
        {
            get;
            set;
        }

        public string FlowScript
        {
            get;
            set;
        }

        public string ICPNubmer
        {
            get;
            set;
        }

        public bool IsValidationService
        {
            get;
            set;
        }

        public string Logo
        {
            get;
            set;
        }

        public string MemberLogo
        {
            get;
            set;
        }

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

        public string Site_SEODescription
        {
            get;
            set;
        }

        public string Site_SEOKeywords
        {
            get;
            set;
        }

        public string Site_SEOTitle
        {
            get;
            set;
        }

        public bool SiteIsOpen
        {
            get;
            set;
        }

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

        public string WeixinAppId
        {
            get;
            set;
        }

        public string WeixinAppSecret
        {
            get;
            set;
        }

        public string WeixinLoginUrl
        {
            get;
            set;
        }

        public string WeixinPartnerID
        {
            get;
            set;
        }

        public string WeixinPartnerKey
        {
            get;
            set;
        }

        public string WeixinToKen
        {
            get;
            set;
        }

        public string WX_MSGGetCouponTemplateId
        {
            get;
            set;
        }

        public string WXLogo
        {
            get;
            set;
        }


        public string PlatformCollectionAddress
        {
            get;
            set;
        }



        public string InsurancefeeMaxValue
        {
            get;
            set;
        }
        public string MaterialMallURL
        {
            get;
            set;
        }
        public string BBSURL
        {
            get;
            set;
        }
        public string ImageFilePath { get; set; }
        public string RecommandIndexImg { get; set; }


        /// <summary>
        /// 代理采购金额
        /// </summary>
        public string PurchasingAmount { get; set; }
        /// <summary>
        /// 定制合成金额
        /// </summary>
        public string SynthsisAmount { get; set; }
        /// <summary>
        /// 供应商认证支付金额
        /// </summary>
        public string SupplierCertificationAmount { get; set; }
        /// <summary>
        /// 产品认证支付金额
        /// </summary>
        public string ProductCertificationAmount { get; set; }
        /// <summary>
        /// 排名付费金额
        /// </summary>
        public string SortCost { get; set; }

        #region 首页菜单显示配置 by xzg
        /// <summary>
        /// 耗材商城显示配置
        /// </summary>
        public bool isShowHaoCai { get; set; }
        /// <summary>
        /// 会议中心显示配置
        /// </summary>
        public bool isShowHuiYiZhongXin { get; set; }
        /// <summary>
        /// 大数据中心显示配置
        /// </summary>
        public bool isShowDaShujuZhongXin { get; set; }
        /// <summary>
        /// 技术交易中心显示配置
        /// </summary>
        public bool isShowJiShuJiaoYiZhongXin { get; set; }
        /// <summary>
        /// 人才市场显示配置
        /// </summary>
        public bool isShowRenCaiShiChang { get; set; }
        /// <summary>
        /// 论坛显示配置
        /// </summary>
        public bool isShowLunTan { get; set; }

        public bool isShowFaLvFaGui { get; set; }

        /// <summary>
        /// 技术资料联系人
        /// </summary>
        public string techName { get; set; }

        /// <summary>
        /// 技术资料联系人电话
        /// </summary>
        public string techTel { get; set; }

        /// <summary>
        /// 技术资料联系人Email
        /// </summary>
        public string techEmail { get; set; }

        /// <summary>
        /// 昨日成交金额亿元
        /// </summary>
        public int FirstAmount { get; set; }

        /// <summary>
        /// 昨日成交金额万元
        /// </summary>
        public int SecondAmount { get; set; }

        /// <summary>
        /// 累计成交金额亿元
        /// </summary>
        public int leijiFirstAmount { get; set; }

        /// <summary>
        /// 累计成交金额万元
        /// </summary>
        public int leijiSecondAmount { get; set; }

        /// <summary>
        /// 网站运行时间
        /// </summary>
        public DateTime Runtime { get; set; }
        #endregion
        public SiteSettingModel()
        {
        }
        /// <summary>
        /// 服务热线
        /// </summary>
        public string PlatCall { get; set; }

        public string Operationreport { get; set; }
    }
}