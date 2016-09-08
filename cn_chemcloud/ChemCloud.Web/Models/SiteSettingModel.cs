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
        /// ����ɹ����
        /// </summary>
        public string PurchasingAmount { get; set; }
        /// <summary>
        /// ���ƺϳɽ��
        /// </summary>
        public string SynthsisAmount { get; set; }
        /// <summary>
        /// ��Ӧ����֤֧�����
        /// </summary>
        public string SupplierCertificationAmount { get; set; }
        /// <summary>
        /// ��Ʒ��֤֧�����
        /// </summary>
        public string ProductCertificationAmount { get; set; }
        /// <summary>
        /// �������ѽ��
        /// </summary>
        public string SortCost { get; set; }

        #region ��ҳ�˵���ʾ���� by xzg
        /// <summary>
        /// �Ĳ��̳���ʾ����
        /// </summary>
        public bool isShowHaoCai { get; set; }
        /// <summary>
        /// ����������ʾ����
        /// </summary>
        public bool isShowHuiYiZhongXin { get; set; }
        /// <summary>
        /// ������������ʾ����
        /// </summary>
        public bool isShowDaShujuZhongXin { get; set; }
        /// <summary>
        /// ��������������ʾ����
        /// </summary>
        public bool isShowJiShuJiaoYiZhongXin { get; set; }
        /// <summary>
        /// �˲��г���ʾ����
        /// </summary>
        public bool isShowRenCaiShiChang { get; set; }
        /// <summary>
        /// ��̳��ʾ����
        /// </summary>
        public bool isShowLunTan { get; set; }

        public bool isShowFaLvFaGui { get; set; }

        /// <summary>
        /// ����������ϵ��
        /// </summary>
        public string techName { get; set; }

        /// <summary>
        /// ����������ϵ�˵绰
        /// </summary>
        public string techTel { get; set; }

        /// <summary>
        /// ����������ϵ��Email
        /// </summary>
        public string techEmail { get; set; }

        /// <summary>
        /// ���ճɽ������Ԫ
        /// </summary>
        public int FirstAmount { get; set; }

        /// <summary>
        /// ���ճɽ������Ԫ
        /// </summary>
        public int SecondAmount { get; set; }

        /// <summary>
        /// �ۼƳɽ������Ԫ
        /// </summary>
        public int leijiFirstAmount { get; set; }

        /// <summary>
        /// �ۼƳɽ������Ԫ
        /// </summary>
        public int leijiSecondAmount { get; set; }

        /// <summary>
        /// ��վ����ʱ��
        /// </summary>
        public DateTime Runtime { get; set; }
        #endregion
        public SiteSettingModel()
        {
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string PlatCall { get; set; }

        public string Operationreport { get; set; }
    }
}