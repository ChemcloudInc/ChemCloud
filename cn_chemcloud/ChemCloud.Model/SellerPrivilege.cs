using System;

namespace ChemCloud.Model
{
    public enum SellerPrivilege
    {
        [Privilege("产品管理", "产品发布", 2001, "product/PublicStepOne", "product", "PublicStepOne")]
        ProductCASAdd = 2001,

        [Privilege("产品管理", "COA报告", 2002, "COAList/Management", "COAList", "Management")]
        CoAReport = 2002,
        [Privilege("产品管理", "产品列表", 2003, "product/management", "product", "")]
        ProductManage = 2003,
        //[Privilege("产品管理", "产品认证", 2004, "product/ProductAuthentication", "product", "")]
        //ProdcutManagement = 2004,
        //[Privilege("产品管理", "分类管理", 2005, "category/management", "category", "")]
        //ProductCategory = 2005,
        //[Privilege("产品", "咨询管理", 2006, "productconsultation/management", "productconsultation", "")]
        //ConsultationManage = 2006,
        [Privilege("产品管理", "评价管理", 2007, "ProductComment/management", "ProductComment", "")]
        CommentManage = 2007,
        [Privilege("产品管理", "批量导入", 2008, "ProductImport/ImportManage", "ProductImport", "")]
        ProductImportManage = 2008,

        [Privilege("产品管理", "分析鉴定", 2009, "Analysis/Index", "Analysis", "")]
        Analysis = 2009,

        //[Privilege("产品管理", "平台代售", 2010, "Product/ProApplyList", "Product", "")]
        //ProApplyList = 2010,

        [Privilege("交易管理", "询盘管理", 3001, "Bargain/Management", "Bargain", "")]
        BargainManage = 3001,
        [Privilege("交易管理", "订单管理", 3002, "order/management", "order", "")]
        OrderManage = 3002,
        [Privilege("交易管理", "退款退货", 3003, "TK/Management", "TK", "")]
        OrderRefund = 3003,

        [Privilege("交易管理", "定制合成订单", 3004, "OrderSynthesis/Index", "OrderSynthesis", "")]
        OrderSynthesis = 3004,
        [Privilege("交易管理", "代理采购订单", 3005, "OrderPurchasing/Index", "OrderPurchasing", "")]
        OrderPurchasing = 3005,
        [Privilege("交易管理", "运费模板管理", 3006, "FreightTemplate/Index", "FreightTemplate", "")]
        FreightTemplate = 3006,
        [Privilege("交易管理", "我的竞价", 3007, "IWantToSupply/IWantToSupply", "IWantToSupply", "")]
        IWantToSupply_SupplyList = 3007,
        //[Privilege("交易管理", "竞价订单", 3008, "IWantToSupply/OrderList", "IWantToSupply", "")]
        //IWantToSupply_OrderList = 3008,


        //[Privilege("交易管理", "退货处理", 3004, "orderrefund/TH", "orderrefund", "")]
        //OrderGoodsRefund = 3004,
        //[Privilege("交易管理", "交易投诉", 3005, "ordercomplaint/management", "ordercomplaint", "")]
        //OrderComplaint = 3005,
        //[Privilege("企业信息", "页面设置", 4001, "PageSettings/management", "PageSettings", "")]
        //PageSetting = 4001,
        //[Privilege("企业信息", "企业信息", 4002, "shop/ShopDetail", "shop", "ShopDetail")]
        //ShopInfo = 4002,
        //[Privilege("企业信息", "密码修改", 4003, "shop/ChangePassword", "shop", "ChangePassword")]
        //ChangePassWord = 4003,
        //[Privilege("企业", "客服管理", 4004, "CustomerService/management", "CustomerService", "")]
        //CustomerService = 4004,
        //[Privilege("企业信息", "结算管理", 4005, "Account/management", "Account", "")]
        //SettlementManage = 4005,
        //[Privilege("企业信息", "保证金管理", 4006, "CashDeposit/Management", "CashDeposit", "")]
        //CashDepositManagement = 4006,
        //[Privilege("企业信息", "实地认证", 4007, "Certification/Management", "Certification", "")]
        //CertificationManagement = 4007,
        //[Privilege("账户管理", "账户管理", 5001, "Statistics/SticsMoneyList", "Statistics", "")]
        //SticsMoneyList = 5001,
        //[Privilege("统计信息", "财务管理", 5002, "statistics/MoneyList", "statistics", "")]
        [Privilege("账户管理", "财务管理", 5002, "Finance/Index", "Finance", "")]

        MoneyListstatistics = 5002,
        //[Privilege("统计信息", "流量统计", 5003, "statistics/shopflow", "statistics", "shopflow")]
        //TrafficStatistics = 5003,
        //[Privilege("统计信息", "关注产品统计", 5004, "ProductConcern/Index", "ProductConcern", "")]
        //ProductConcern = 5004,
        //[Privilege("统计信息", "企业统计", 5005, "statistics/shopsale", "statistics", "shopsale")]
        //ShopStatistics = 5005,
        //[Privilege("统计信息", "成交转化率", 5006, "statistics/DealConversionRate", "statistics", "DealConversionRate")]
        //SalesAnalysis = 5006,

        [Privilege("权限管理", "权限组", 6002, "Privilege/management", "Privilege", "")]
        PrivilegesManage = 6002,
        [Privilege("权限管理", "管理员", 6001, "Manager/management", "Manager", "")]
        AdminManage = 6001,
        [Privilege("技术资料", "技术资料", 7003, "TechInfo/Management", "TechInfo", "")]
        OperationLog = 7003,
        [Privilege("人才招聘", "招聘信息", 8001, "Jobs/Jobs", "Jobs", "")]
        JobList = 8001
        //[Privilege("权限管理", "消息管理", 6004, "Message/Management", "Message", "")]
        //Message = 6004

    }
}