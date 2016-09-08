using ChemCloud.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Runtime.CompilerServices;

namespace ChemCloud.Entity
{
    public class Entities : DbContext
    {
        public virtual DbSet<ChemCloud.Model.ProductsDS> ProductsDS { get; set; }
        /*·ÖÎö¼ø¶¨*/
        public virtual DbSet<ChemCloud.Model.ProductAnalysis> ProductAnalysis { get; set; }

        public virtual DbSet<ChemCloud.Model.AccountDetailInfo> AccountDetailInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.AccountInfo> AccountInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.AccountMetaInfo> AccountMetaInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.AccountPurchaseAgreementInfo> AccountPurchaseAgreementInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ActiveMarketServiceInfo> ActiveMarketServiceInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.AgreementInfo> AgreementInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ApplyWithDrawInfo> ApplyWithDrawInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ArticleCategoryInfo> ArticleCategoryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ArticleInfo> ArticleInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.AttributeInfo> AttributeInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.AttributeValueInfo> AttributeValueInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.BannerInfo> BannerInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.BonusInfo> BonusInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.BonusReceiveInfo> BonusReceiveInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.BrandInfo> BrandInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.BrowsingHistoryInfo> BrowsingHistoryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.BusinessCategoryInfo> BusinessCategoryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CapitalDetailInfo> CapitalDetailInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CapitalInfo> CapitalInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CashDepositDetailInfo> CashDepositDetailInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CashDepositInfo> CashDepositInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CategoryCashDepositInfo> CategoryCashDepositInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CategoryInfo> CategoryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ChargeDetailInfo> ChargeDetailInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CollocationInfo> CollocationInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CollocationPoruductInfo> CollocationPoruductInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CollocationSkuInfo> CollocationSkuInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CouponInfo> CouponInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CouponRecordInfo> CouponRecordInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CouponSettingInfo> CouponSettingInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CustomerServiceInfo> CustomerServiceInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FavoriteInfo> FavoriteInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FavoriteShopInfo> FavoriteShopInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FloorBrandInfo> FloorBrandInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FloorCategoryInfo> FloorCategoryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FloorProductInfo> FloorProductInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FloorTablDetailsInfo> FloorTablDetailsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FloorTablsInfo> FloorTablsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FloorTopicInfo> FloorTopicInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FreightAreaContentInfo> FreightAreaContentInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FreightTemplateInfo> FreightTemplateInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.GiftInfo> GiftInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.GiftOrderInfo> GiftOrderInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.GiftOrderItemInfo> GiftOrderItemInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.HandSlideAdInfo> HandSlideAdInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.HomeCategoryInfo> HomeCategoryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.HomeCategoryRowInfo> HomeCategoryRowInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.HomeFloorInfo> HomeFloorInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ImageAdInfo> ImageAdInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.InviteRecordInfo> InviteRecordInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.InviteRuleInfo> InviteRuleInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.InvoiceContextInfo> InvoiceContextInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.InvoiceTitleInfo> InvoiceTitleInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.LimitTimeMarketInfo> LimitTimeMarketInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.LogInfo> LogInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ManagerInfo> ManagerInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MarketServiceRecordInfo> MarketServiceRecordInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MarketSettingInfo> MarketSettingInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MarketSettingMetaInfo> MarketSettingMetaInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberContactsInfo> MemberContactsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberGrade> MemberGrade { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberIntegral> MemberIntegral { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberIntegralExchangeRules> MemberIntegralExchangeRules { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberIntegralRecord> MemberIntegralRecord { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberIntegralRecordAction> MemberIntegralRecordAction { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberIntegralRule> MemberIntegralRule { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberOpenIdInfo> MemberOpenIdInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MenuInfo> MenuInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MessageLog> MessageLog { get; set; }
        public virtual DbSet<ChemCloud.Model.MobileHomeProductsInfo> MobileHomeProductsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MobileHomeTopicsInfo> MobileHomeTopicsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ModuleProductInfo> ModuleProductInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OpenIdsInfo> OpenIdsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderCommentInfo> OrderCommentInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderComplaintInfo> OrderComplaintInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderInfo> OrderInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderItemInfo> OrderItemInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderOperationLogInfo> OrderOperationLogInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderPayInfo> OrderPayInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderRefundInfo> OrderRefundInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductAttributeInfo> ProductAttributeInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductCommentInfo> ProductCommentInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductConsultationInfo> ProductConsultationInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductDescriptionInfo> ProductDescriptionInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductDescriptionTemplateInfo> ProductDescriptionTemplateInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductInfo> ProductInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductShopCategoryInfo> ProductShopCategoryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductTypeInfo> ProductTypeInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductVistiInfo> ProductVistiInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.RoleInfo> RoleInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.RolePrivilegeInfo> RolePrivilegeInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.SellerSpecificationValueInfo> SellerSpecificationValueInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.SensitiveWordsInfo> SensitiveWordsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShippingAddressInfo> ShippingAddressInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopBonusGrantInfo> ShopBonusGrantInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopBonusInfo> ShopBonusInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopBonusReceiveInfo> ShopBonusReceiveInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopBrandApplysInfo> ShopBrandApplysInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopBrandsInfo> ShopBrandsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopCategoryInfo> ShopCategoryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopGradeInfo> ShopGradeInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopHomeModuleInfo> ShopHomeModuleInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopHomeModuleProductInfo> ShopHomeModuleProductInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopInfo> ShopInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ShoppingCartItemInfo> ShoppingCartItemInfo_ { get; set; }
        public virtual DbSet<ChemCloud.Model.ShopVistiInfo> ShopVistiInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.SiteSettingsInfo> SiteSettingsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.SKUInfo> SKUInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.SlideAdInfo> SlideAdInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.SpecificationValueInfo> SpecificationValueInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.StatisticOrderCommentsInfo> StatisticOrderCommentsInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.TopicInfo> TopicInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.TopicModuleInfo> TopicModuleInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.TypeBrandInfo> TypeBrandInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.UserMemberInfo> UserMemberInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.VShopExtendInfo> VShopExtendInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.VShopInfo> VShopInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.WeiXinBasicInfo> WeiXinBasicInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.WXAccTokenInfo> WXAccTokenInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.WXCardCodeLogInfo> WXCardCodeLogInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.WXCardLogInfo> WXCardLogInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.WXShopInfo> WXShopInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.FieldCertification> FieldCertification { get; set; }
        public virtual DbSet<ChemCloud.Model.MemberDetail> MemberDetail { get; set; }
        public virtual DbSet<ChemCloud.Model.MargainBill> MargainBill { get; set; }
        public virtual DbSet<ChemCloud.Model.MargainBillDetail> MargainBillDetail { get; set; }
        public virtual DbSet<ChemCloud.Model.CAS_MAIN_INFO> CAS_MAIN_INFO { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductSpec> ProductSpec { get; set; }
        public virtual DbSet<ChemCloud.Model.SiteMessages> SiteMessages { get; set; }
        public virtual DbSet<ChemCloud.Model.MessageSetting> MessageSetting { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderExpressQuery> OrderExpressQuery { get; set; }
        public virtual DbSet<ChemCloud.Model.StatisticsMoney> StatisticsMoney { get; set; }
        public virtual DbSet<ChemCloud.Model.RecFloorImg> RecFloorImg { get; set; }
        public virtual DbSet<ChemCloud.Model.Attention> Attention { get; set; }
        public virtual DbSet<ChemCloud.Model.Messages> Messages { get; set; }
        public virtual DbSet<ChemCloud.Model.ChatRelationShip> ChatRelationShip { get; set; }
        public virtual DbSet<ChemCloud.Model.CapitalUserAccount> CapitalUserAccount { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductAuthentication> ProductAuthentication { get; set; }
        public virtual DbSet<ChemCloud.Model.ManagersAccount> ManagersAccount { get; set; }
        public virtual DbSet<ChemCloud.Model.CountryInfo> CountryInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.ChemCloud_EmailSetting> ChemCloud_EmailSetting { get; set; }
        public virtual DbSet<ChemCloud.Model.ProductLevel> ProductLevel { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderPurchasing> OrderPurchasing { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderSynthesis> OrderSynthesis { get; set; }
        public virtual DbSet<ChemCloud.Model.ChemCloud_COA> ChemCloud_COA { get; set; }
        public virtual DbSet<ChemCloud.Model.ChemCloud_COADetails> ChemCloud_COADetails { get; set; }
        public virtual DbSet<ChemCloud.Model.ConfigPayPalAPI> ConfigPayPalAPI { get; set; }
        public virtual DbSet<ChemCloud.Model.ChemCloud_DictionaryType> ChemCloud_DictionaryType { get; set; }
        public virtual DbSet<ChemCloud.Model.ChemCloud_Dictionaries> ChemCloud_Dictionaries { get; set; }
        public virtual DbSet<ChemCloud.Model.COAList> COAList { get; set; }
        public virtual DbSet<ChemCloud.Model.FQPayment> FQPayment { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderPayDesc> OrderPayDesc { get; set; }
        public virtual DbSet<ChemCloud.Model.ChemCloud_OrderWithCoa> ChemCloud_OrderWithCoa { get; set; }
        public virtual DbSet<ChemCloud.Model.LimitedAmount> LimitedAmount { get; set; }
        public virtual DbSet<ChemCloud.Model.Organization> Organization { get; set; }
        public virtual DbSet<ChemCloud.Model.ApplyAmountInfo> ApplyAmountInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.PurchaseRolesInfo> PurchaseRolesInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MessageDetial> MessageDetial { get; set; }
        public virtual DbSet<ChemCloud.Model.MessageEnclosure> MessageEnclosure { get; set; }
        public virtual DbSet<ChemCloud.Model.MessageRevice> MessageRevice { get; set; }
        public virtual DbSet<ChemCloud.Model.Finance_Wallet> Finance_Wallet { get; set; }
        public virtual DbSet<ChemCloud.Model.Finance_InCome> Finance_InCome { get; set; }
        public virtual DbSet<ChemCloud.Model.Finance_Payment> Finance_Payment { get; set; }
        public virtual DbSet<ChemCloud.Model.Finance_Refund> Finance_Refund { get; set; }
        public virtual DbSet<ChemCloud.Model.Finance_WithDraw> Finance_WithDraw { get; set; }
        public virtual DbSet<ChemCloud.Model.Finance_Transfer> Finance_Transfer { get; set; }
        public virtual DbSet<ChemCloud.Model.Finance_Recharge> Finance_Recharge { get; set; }
        public virtual DbSet<ChemCloud.Model.TH> TH { get; set; }
        public virtual DbSet<ChemCloud.Model.THMessageInfo> THMessageInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.THImageInfo> THImageInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.MeetingInfo> MeetingInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.AttachmentInfo> AttachmentInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.TechnicalInfo> TechnicalInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.RecommendInfo> RecommendInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.LawInfo> LawInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.Jobs> Jobs { get; set; }
        public virtual DbSet<ChemCloud.Model.BannersIndex> BannersIndex { get; set; }
        public virtual DbSet<ChemCloud.Model.TransactionRecord> TransactionRecord { get; set; }
        public virtual DbSet<ChemCloud.Model.WhatBusy> WhatBusy { get; set; }
        public virtual DbSet<ChemCloud.Model.Shipment> Shipment { get; set; }
        public virtual DbSet<ChemCloud.Model.ShipmentAddress> ShipmentAddress { get; set; }
        public virtual DbSet<ChemCloud.Model.ShipmentPackage> ShipmentPackage { get; set; }
        public virtual DbSet<ChemCloud.Model.TK> TK { get; set; }
        public virtual DbSet<ChemCloud.Model.TKMessage> TKMessage { get; set; }
        public virtual DbSet<ChemCloud.Model.TKImageInfo> TKImageInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.CASInfo> CASInfo { get; set; }
        public virtual DbSet<ChemCloud.Model.OrderShip> OrderShip { get; set; }
        public virtual DbSet<ChemCloud.Model.IWantToBuy> IWantToBuy { get; set; }
        public virtual DbSet<ChemCloud.Model.IWantToBuy_Orders> IWantToBuy_Orders { get; set; }
        public virtual DbSet<ChemCloud.Model.IWantToSupply> IWantToSupply { get; set; }
        public virtual DbSet<ChemCloud.Model.orderAddressInfo> orderAddressInfo { get; set; }
        
        public Entities() : base("name=Entities") { ShoppingCartItemInfo_ = base.Set<ChemCloud.Model.ShoppingCartItemInfo>(); }
        public virtual int Job_Account(DateTime? startDate, DateTime? endDate)
        {
            ObjectParameter objectParameter = (startDate.HasValue ? new ObjectParameter("StartDate", startDate) : new ObjectParameter("StartDate", typeof(DateTime)));
            ObjectParameter objectParameter1 = (endDate.HasValue ? new ObjectParameter("EndDate", endDate) : new ObjectParameter("EndDate", typeof(DateTime)));
            System.Data.Entity.Core.Objects.ObjectContext objectContext = ((IObjectContextAdapter)this).ObjectContext;
            ObjectParameter[] objectParameterArray = new ObjectParameter[] { objectParameter, objectParameter1 };
            return objectContext.ExecuteFunction("Job_Account", objectParameterArray);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
       
    }
}