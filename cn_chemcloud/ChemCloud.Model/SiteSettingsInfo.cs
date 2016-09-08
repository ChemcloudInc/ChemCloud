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
        /// ����ɹ����
        /// </summary>
        public string PurchasingAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// ���ƺϳɽ��
        /// </summary>
        public string SynthsisAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// ��Ӧ����֤֧�����
        /// </summary>
        public string SupplierCertificationAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// ��Ʒ��֤֧�����
        /// </summary>
        public string ProductCertificationAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// �������ѽ��
        /// </summary>
        public string SortCost { get; set; }

        #region ��ҳ�˵���ʾ���� by xzg
        [NotMapped]
        /// <summary>
        /// �Ĳ��̳���ʾ����
        /// </summary>
        public bool isShowHaoCai { get; set; }
        [NotMapped]
        /// <summary>
        /// ����������ʾ����
        /// </summary>
        public bool isShowHuiYiZhongXin { get; set; }
        [NotMapped]
        /// <summary>
        /// ������������ʾ����
        /// </summary>
        public bool isShowDaShujuZhongXin { get; set; }
        [NotMapped]
        /// <summary>
        /// ��������������ʾ����
        /// </summary>
        public bool isShowJiShuJiaoYiZhongXin { get; set; }
        [NotMapped]
        /// <summary>
        /// �˲��г���ʾ����
        /// </summary>
        public bool isShowRenCaiShiChang { get; set; }
        [NotMapped]
        /// <summary>
        /// ��̳��ʾ����
        /// </summary>
        public bool isShowLunTan { get; set; }

        [NotMapped]
        /// <summary>
        /// ��̳��ʾ����
        /// </summary>
        public bool isShowFaLvFaGui { get; set; }
        #endregion
        [NotMapped]
        /// <summary>
        /// ����������ϵ��
        /// </summary>
        public string techName { get; set; }

        [NotMapped]
        /// <summary>
        /// ����������ϵ�˵绰
        /// </summary>
        public string techTel { get; set; }

        [NotMapped]
        /// <summary>
        /// ����������ϵ��Email
        /// </summary>
        public string techEmail { get; set; }

        [NotMapped]
        /// <summary>
        /// ���ճɽ������Ԫ
        /// </summary>
        public int FirstAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// ���ճɽ������Ԫ
        /// </summary>
        public int SecondAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// �ۼƳɽ������Ԫ
        /// </summary>
        public int leijiFirstAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// �ۼƳɽ������Ԫ
        /// </summary>
        public int leijiSecondAmount { get; set; }

        [NotMapped]
        /// <summary>
        /// ��վ����ʱ��
        /// </summary>
        public DateTime Runtime { get; set; }
        public SiteSettingsInfo()
        {
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string PlatCall { get; set; }

        public string Operationreport { get; set; }
    }
}