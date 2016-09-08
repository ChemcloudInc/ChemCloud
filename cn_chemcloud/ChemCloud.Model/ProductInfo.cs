using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ProductInfo : BaseModel
    {
        //Packaging,Purity,SpecLevel,Price 

        /// <summary>
        /// 纯度(价格等级表 ChemCloud_ProductSpec)
        /// </summary>
        public string Purity { get; set; }

        [NotMapped]
        /// <summary>
        /// 包装规格(价格等级表 ChemCloud_ProductSpec)
        /// </summary>
        public string Packaging { get; set; }

        [NotMapped]
        /// <summary>
        /// 工业等级 (价格等级表 ChemCloud_ProductSpec)
        /// </summary>
        public string SpecLevel { get; set; }

        [NotMapped]
        /// <summary>
        /// 工业等级 (价格等级表 ChemCloud_ProductSpec)
        /// </summary>
        public decimal Price { get; set; }


        [NotMapped]
        public long CoinType { get; set; }

        [NotMapped]
        public string CoinTypeName { get; set; }

        private long _id;

        public DateTime AddedDate
        {
            get;
            set;
        }

        [NotMapped]
        public string Address
        {
            get;
            set;
        }

        public ProductInfo.ProductAuditStatus AuditStatus
        {
            get;
            set;
        }

        public long BrandId
        {
            get;
            set;
        }
        [NotMapped]
        public string CompanyName
        {
            get;
            set;
        }
        [NotMapped]
        public string CategoryName
        {
            get;
            set;
        }
        [NotMapped]
        public string BrandName
        {
            get;
            set;
        }
        [NotMapped]
        public ProductInfo.ProductAuditStatus ShowProductStatus
        {
            get
            {
                ProductInfo.ProductAuditStatus productStatus = ProductInfo.ProductAuditStatus.WaitForAuditing;
                if (this != null)
                {
                    productStatus = AuditStatus;
                }
                return productStatus;
            }
        }
        public long CategoryId
        {
            get;
            set;
        }

        public string CategoryPath
        {
            get;
            set;
        }

        public int ConcernedCount
        {
            get;
            set;
        }

        public long DisplaySequence
        {
            get;
            set;
        }

        public int EditStatus
        {
            get;
            set;
        }

        public long FreightTemplateId
        {
            get;
            set;
        }

        public bool HasSKU
        {
            get;
            set;
        }

        public virtual ICollection<BrowsingHistoryInfo> ChemCloud_BrowsingHistory
        {
            get;
            set;
        }

        public virtual CategoryInfo ChemCloud_Categories
        {
            get;
            set;
        }

        public virtual ICollection<CollocationPoruductInfo> ChemCloud_CollocationPoruducts
        {
            get;
            set;
        }

        public virtual ICollection<CollocationSkuInfo> ChemCloud_CollocationSkus
        {
            get;
            set;
        }

        public virtual ICollection<FavoriteInfo> ChemCloud_Favorites
        {
            get;
            set;
        }

        public virtual ICollection<FloorProductInfo> ChemCloud_FloorProducts
        {
            get;
            set;
        }

        public virtual ICollection<FloorTablDetailsInfo> ChemCloud_FloorTablDetails
        {
            get;
            set;
        }

        public virtual FreightTemplateInfo ChemCloud_FreightTemplate
        {
            get;
            set;
        }

        public virtual ICollection<MobileHomeProductsInfo> ChemCloud_MobileHomeProducts
        {
            get;
            set;
        }

        public virtual ICollection<ModuleProductInfo> ChemCloud_ModuleProducts
        {
            get;
            set;
        }

        public virtual ICollection<ProductCommentInfo> ChemCloud_ProductComments
        {
            get;
            set;
        }

        public virtual ICollection<ProductShopCategoryInfo> ChemCloud_ProductShopCategories
        {
            get;
            set;
        }

        public virtual ICollection<ProductVistiInfo> ChemCloud_ProductVistis
        {
            get;
            set;
        }

        public virtual ICollection<ShopHomeModuleProductInfo> ChemCloud_ShopHomeModuleProducts
        {
            get;
            set;
        }

        internal virtual ICollection<ShoppingCartItemInfo> ChemCloud_ShoppingCarts
        {
            get;
            set;
        }

        public virtual ShopInfo ChemCloud_Shops
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

        private string imagePath
        {
            get;
            set;
        }

        [NotMapped]
        public string ImagePath
        {
            get
            {
                return string.Concat(ImageServerUrl, imagePath);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(ImageServerUrl))
                {
                    imagePath = value;
                    return;
                }
                imagePath = value.Replace(ImageServerUrl, "");
            }
        }

        public decimal MarketPrice
        {
            get;
            set;
        }

        public string MeasureUnit
        {
            get;
            set;
        }

        public decimal MinSalePrice
        {
            get;
            set;
        }

        [NotMapped]
        public long OrderCounts
        {
            get;
            set;
        }

        public virtual ICollection<ChemCloud.Model.ProductAttributeInfo> ProductAttributeInfo
        {
            get;
            set;
        }

        public string ProductCode
        {
            get;
            set;
        }

        public virtual ICollection<ChemCloud.Model.ProductConsultationInfo> ProductConsultationInfo
        {
            get;
            set;
        }

        public virtual ChemCloud.Model.ProductDescriptionInfo ProductDescriptionInfo
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }

        public int? Quantity
        {
            get;
            set;
        }

        public long SaleCounts
        {
            get;
            set;
        }

        public ProductInfo.ProductSaleStatus SaleStatus
        {
            get;
            set;
        }

        [NotMapped]
        public IEnumerable<ShopCategoryInfo> ShopCateogryInfos
        {
            get;
            set;
        }

        public long ShopId
        {
            get;
            set;
        }

        [NotMapped]
        public string ShopName
        {
            get;
            set;
        }

        public string ShortDescription
        {
            get;
            set;
        }
        [NotMapped]
        public string ShowProductState
        {
            get
            {
                string description = "错误数据";
                if (this != null)
                {
                    if (AuditStatus != ProductInfo.ProductAuditStatus.WaitForAuditing)
                    {
                        description = AuditStatus.ToDescription();
                    }
                    else
                    {
                        description = (SaleStatus == ProductInfo.ProductSaleStatus.OnSale ? ProductInfo.ProductAuditStatus.WaitForAuditing.ToDescription() : ProductInfo.ProductSaleStatus.InStock.ToDescription());
                    }
                }
                return description;
            }
        }

        public string EProductName
        {
            get;
            set;
        }

        public string CASNo
        {
            get;
            set;
        }
        public string HSCODE
        {
            get;
            set;
        }

        public string DangerLevel
        {
            get;
            set;
        }
        public string MolecularFormula
        {
            get;
            set;
        }
        public bool ISCASNo
        {
            get;
            set;
        }
        public string Alias
        {
            get;
            set;
        }
        public string Ealias
        {
            get;
            set;
        }
        public string MolecularWeight
        {
            get;
            set;
        }
        public string PASNo
        {
            get;
            set;
        }
        public string LogP
        {
            get;
            set;
        }
        public string Shape
        {
            get;
            set;
        }
        public string Density
        {
            get;
            set;
        }
        public string FusingPoint
        {
            get;
            set;
        }
        public string BoilingPoint
        {
            get;
            set;
        }
        public string FlashPoint
        {
            get;
            set;
        }
        public string RefractiveIndex
        {
            get;
            set;
        }
        public string StorageConditions
        {
            get;
            set;
        }
        public string VapourPressure
        {
            get;
            set;
        }
        public string PackagingLevel
        {
            get;
            set;
        }
        public string SafetyInstructions
        {
            get;
            set;
        }
        public string DangerousMark
        {
            get;
            set;
        }
        public string RiskCategoryCode
        {
            get;
            set;
        }
        public string TransportationNmber
        {
            get;
            set;
        }
        public string RETCS
        {
            get;
            set;
        }
        public string WGKGermany
        {
            get;
            set;
        }
        public string SyntheticRoute
        {
            get;
            set;
        }
        public string RelatedProducts
        {
            get;
            set;
        }
        public string MSDS
        {
            get;
            set;
        }
        public string NMRSpectrum
        {
            get;
            set;
        }

        public string RefuseReason { get; set; }
        public virtual ICollection<ChemCloud.Model.SKUInfo> SKUInfo
        {
            get;
            set;
        }

        public long TypeId
        {
            get;
            set;
        }

        public long VistiCounts
        {
            get;
            set;
        }
        public string STRUCTURE_2D
        {
            get;
            set;
        }
        /// <summary>
        /// 平台货号
        /// </summary>
        public string Plat_Code { get; set; }

        public decimal? Volume
        {
            get;
            set;
        }



        public decimal? Weight
        {
            get;
            set;
        }
        [NotMapped]
        public int AuthStatus
        {
            get;
            set;
        }
        public string imagePath1 { get; set; }
        [NotMapped]
        public string showshop { get; set; }

        public ProductInfo()
        {
            ProductConsultationInfo = new HashSet<ChemCloud.Model.ProductConsultationInfo>();
            ProductAttributeInfo = new HashSet<ChemCloud.Model.ProductAttributeInfo>();
            SKUInfo = new HashSet<ChemCloud.Model.SKUInfo>();
            ChemCloud_Favorites = new HashSet<FavoriteInfo>();
            ChemCloud_FloorProducts = new HashSet<FloorProductInfo>();
            ChemCloud_ProductShopCategories = new HashSet<ProductShopCategoryInfo>();
            ChemCloud_ShoppingCarts = new HashSet<ShoppingCartItemInfo>();
            ChemCloud_ProductComments = new HashSet<ProductCommentInfo>();
            ChemCloud_ModuleProducts = new HashSet<ModuleProductInfo>();
            ChemCloud_ShopHomeModuleProducts = new HashSet<ShopHomeModuleProductInfo>();
            ChemCloud_ProductVistis = new HashSet<ProductVistiInfo>();
            ChemCloud_BrowsingHistory = new HashSet<BrowsingHistoryInfo>();
            ChemCloud_MobileHomeProducts = new HashSet<MobileHomeProductsInfo>();
            ChemCloud_FloorTablDetails = new HashSet<FloorTablDetailsInfo>();
            ChemCloud_CollocationPoruducts = new HashSet<CollocationPoruductInfo>();
            ChemCloud_CollocationSkus = new HashSet<CollocationSkuInfo>();
        }

        public string GetImage(ProductInfo.ImageSize imageSize, int imageIndex = 1)
        {
            return string.Format(string.Concat(ImagePath, "/{0}_{1}.png"), imageIndex, (int)imageSize);
        }

        public enum ImageSize
        {
            [Description("50×50")]
            Size_50 = 50,
            [Description("100×100")]
            Size_100 = 100,
            [Description("150×150")]
            Size_150 = 150,
            [Description("220×220")]
            Size_220 = 220,
            [Description("350×350")]
            Size_350 = 350
        }

        [NotMapped]
        public List<ProductSpec> _ProductSpec { get; set; }

        public enum ProductAuditStatus
        {

            [Description("待审核")]
            WaitForAuditing = 1,
            [Description("上架")]
            Audited = 2,
            [Description("未通过")]
            AuditFailed = 3,
            [Description("违规下架")]
            InfractionSaleOff = 4,
            [Description("未审核")]
            UnAudit = 5,
            [Description("审核通过")]
            AuditPass = 6,
            [Description("待审核(结构式)")]
            NoCasNoWaitAuditing = 7,
            [Description("审核通过(结构式)")]
            NoCasNoAuditPass = 8
        }

        public enum ProductEditStatus
        {
            [Description("正常")]
            Normal,
            [Description("己修改")]
            Edited,
            [Description("待审核")]
            PendingAudit,
            [Description("己修改待审核")]
            EditedAndPending,
            [Description("强制待审核")]
            CompelPendingAudit,
            [Description("强制待审己修改")]
            CompelPendingHasEdited
        }

        public enum ProductSaleStatus
        {
            [Description("原始状态")]
            RawState,
            [Description("出售中")]
            OnSale,
            [Description("仓库中")]
            InStock,
            [Description("草稿箱")]
            InDraft,
            [Description("已删除")]
            InDelete,
            [Description("全部")]
            InAll
        }

        public int Pub_CID { get; set; }
    }
}