using System;

namespace ChemCloud.Model
{
    public enum AdminPrivilege
    {
        [Privilege("产品", "产品管理", 2001, "product/management", "product", "")]
        ProductManage = 2001,
        //[Privilege("产品", "产品审核", 2002, "product/management?type=Auditing", "product", "")]
        //ProductAuditManage = 2002, 
        [Privilege("产品", "产品认证审核", 2003, "product/ProductAuthentication", "product", "")]
        ProductAuthentication = 2003,//
        [Privilege("产品", "产品等级管理", 2004, "productlevel/management", "productlevel", "")]
        ProductLevel = 2004,

        [Privilege("产品", "分析鉴定", 2005, "Analysis/Index", "Analysis", "")]
        Analysis = 2005,

        [Privilege("分类", "类型管理", 3002, "ProductType/management", "ProductType", "")]
        ProductTypeManage = 3002,
        [Privilege("分类", "分类管理", 3001, "category/management", "category", "")]
        CategoryManage = 3001,
        [Privilege("交易", "订单管理", 4001, "Order/management", "order", "")]
        OrderManage = 4001,
        [Privilege("交易", "代购订单管理", 4002, "OrderPurchasing/Index", "order", "")]
        OrderSynthesisManage = 4002,
        [Privilege("交易", "合成订单管理", 4003, "OrderSynthesis/Index", "order", "")]
        OrderPurchasingIndex = 4003,
        [Privilege("交易", "交易评价", 4004, "OrderComment/management", "ordercomment", "")]
        OrderComment = 4004,
        //[Privilege("交易", "交易投诉", 4005, "OrderComplaint/management", "ordercomplaint", "")]
        //OrderComplaint = 4005,
        [Privilege("交易", "交易设置", 4006, "AdvancePayment/edit", "AdvancePayment", "")]
        PaymentManageSet = 4006,
        [Privilege("交易", "退货处理", 4007, "OrderRefund/TH", "orderrefund", "")]
        ReturnGoodsManage = 4007,
        [Privilege("交易", "退款处理", 4008, "OrderRefund/TK", "orderrefund", "")]
        ReturnRefundManage = 4008,
        [Privilege("交易", "平台代发管理", 4010, "Order/ManageShip", "order", "")]
        OrderShipManage = 4010,
        //[Privilege("交易", "平台代售管理", 4011, "Product/ProductsDS", "order", "")]
        //ProductProductsDS = 4011,
        //[Privilege("交易", "发票管理", 4009, "Order/InvoiceContext", "Order", "")]
        //InvoiceContextManage = 4009,

        [Privilege("会员", "会员激活管理", 5000, "MemberDetail/AccountManager", "MemberDetail", "")]
        AccountManager = 5000,
        [Privilege("会员", "采购商管理", 5001, "MemberDetail/Management", "MemberDetail", "")]
        MemberManage = 5001,
        //[Privilege("会员", "采购商积分", 5002, "MemberIntegral/search", "MemberIntegral", "")]
        //MemberIntegral = 5002,
        //[Privilege("会员", "采购商规则", 5003, "IntegralRule/management", "IntegralRule", "")]
        //IntegralRule = 5003,
        //[Privilege("会员", "采购商等级", 5004, "MemberGrade/management", "MemberGrade", "")]
        //MemberGrade = 5004,
        //[Privilege("会员", "采购商推广", 5005, "MemberInvite/Setting", "MemberInvite", "")]
        //MemberInvite = 5005,
        [Privilege("会员", "供应商管理", 5006, "shop/management?type=Auditing", "Shop", "")]
        ShopManage = 5006,
        [Privilege("会员", "实地认证", 5007, "FieldCertification/management?type=", "FieldCertification", "")]
        FieldCertification = 5007,
        [Privilege("统计", "采购商统计", 8001, "Statistics/Member", "statistics", "member")]
        MemberStatistics = 8001,
        [Privilege("统计", "供应商统计", 8002, "Statistics/NewShop", "statistics", "newshop")]
        ShopStatistics = 8002,
        [Privilege("统计", "销量分析", 8003, "Statistics/ProductSaleRanking", "statistics", "productsaleranking")]
        SalesAnalysis = 8003,

        //[Privilege("财务", "财务管理", 9001, "Statistics/AdminMoneyList", "statistics", "")]
        //StatisticsMoneyList = 9001,
        [Privilege("财务", "财务管理", 9001, "Finance/Index", "Finance", "")]
        StatisticsMoneyList = 9001,

        //[Privilege("财务", "采购商统计", 9002, "Statistics/AddMoneyList", "statistics", "")]
        //StatisticsAddMoneyList = 9002,

        //[Privilege("财务", "供应商统计", 9003, "Statistics/RemoveMoneyList", "statistics", "")]
        //StatisticsRemoveMoneyList = 9003,

        [Privilege("财务", "提现审核", 9004, "Finance/AuditFinance_WithDrawList", "Finance", "")]
        AuditFinance_WithDrawList = 9004,

        [Privilege("网站", "文章管理", 10001, "Article/management", "article", "")]
        ArticleManage = 10001,

        [Privilege("网站", "文章分类", 10002, "ArticleCategory/management", "articlecategory", "")]
        ArticleCategoryManage = 10002,

        [Privilege("网站", "首页幻灯片", 10003, "BannersIndex/BannersIndex", "BannersIndex", "")]
        BannerList = 10003,

        [Privilege("网站", "首页实时交易", 10004, "TransactionRecord/TransactionRecord", "TransactionRecord", "")]
        TransactionRecordList = 10004,

        [Privilege("网站", "人才市场", 10005, "JobsList/JobsList", "JobsList", "")]
        JobsList = 10005,

        [Privilege("网站", "大家忙什么", 10006, "WhatBusy/WhatBusyList", "WhatBusy", "")]
        WhatBusyList = 10006,

        //[Privilege("网站", "页面设置", 10003, "PageSettings/HomeCategory", "PageSettings", "")]
        //PageSetting = 10003,
        [Privilege("系统", "站点设置", 11001, "SiteSetting/Edit", "SiteSetting", "")]
        SiteSetting = 11001,
        [Privilege("系统", "管理员", 11002, "Manager/management", "Manager", "")]
        AdminManage = 11002,
        [Privilege("系统", "权限组", 11003, "Privilege/management", "privilege", "")]
        PrivilegesManage = 11003,
        [Privilege("系统", "操作日志", 11004, "OperationLog/management", "OperationLog", "")]
        OperationLog = 11004,
        [Privilege("系统", "协议管理", 11005, "Agreement/Management", "Agreement", "")]
        Agreement = 11005,
        [Privilege("系统", "首页配置", 11006, "RecFloorImg/Edit", "RecFloorImg", "")]
        RecFloorImg = 11006,
        [Privilege("系统", "字典配置", 11007, "Dictionary/DictionaryTypeList", "Dictionary", "")]
        Dictionary = 11007,
        [Privilege("系统", "会议中心", 11008, "MeetingInfo/Management", "MeetingInfo", "")]
        MeetingInfo = 11008,

        [Privilege("系统", "技术资料", 11009, "TechInfo/Management", "TechInfo", "")]
        TechInfo = 11009,
        [Privilege("系统", "热卖商品配置", 11010, "RecommendInfo/Management", "RecommendInfo", "")]
        RecommendInfo = 11010,
        [Privilege("系统", "法律法规", 11011, "LawInfo/Management", "LawInfo", "")]
        LawInfo = 11011,
        //[Privilege("CAS", "审核通过", 12001, "CAS/management", "CAS", "")]
        //CasManagement = 12001,
        //[Privilege("CAS", "审核不通过", 12002, "CAS/dmanagement", "CAS", "")]
        //DManagement = 12002,
        //[Privilege("CAS", "待审核", 12003, "CAS/wmanagement", "CAS", "")]
        //WManagement = 12003,
        [Privilege("消息", "通知模板配置", 13001, "MessageSetting/management?type=", "MessageSetting", "")]
        ModuleConfig = 13001,
        [Privilege("消息", "通知列表", 13002, "SiteMessages/management", "SiteMessages", "")]
        ActiveModuleConfig = 13002,
        [Privilege("消息", "消息列表", 13003, "MessageDetial/management", "MessageDetial", "")]
        ActiveMessage = 13003,
        [Privilege("消息", "邮箱设置", 13004, "EmailSetting/Index", "EmailSetting", "")]
        EmailSetting = 13004,
        [Privilege("消息", "PAYPALAPI设置", 13005, "ConfigPPAPI/Index", "ConfigPPAPI", "")]
        ConfigPPAPI = 13005,
        [Privilege("消息", "在线消息", 13006, "../../ChatMessage/Index", "ChatMessage", "")]
        ChatMessage = 13006,
        [Privilege("消息", "积分管理", 13007, "../../BonusPoint/Index", "ChatMessage", "")]
        BonusPoint = 13007
    }
}