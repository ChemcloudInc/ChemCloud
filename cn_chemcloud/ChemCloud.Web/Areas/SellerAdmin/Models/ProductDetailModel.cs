using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.SellerAdmin.Models
{
    public class ProductDetailModel
    {
        public string adWord
        {
            get;
            set;
        }

        public List<AttrSelectData> attrSelectData
        {
            get;
            set;
        }

        public long brandId
        {
            get;
            set;
        }

        public long categoryId
        {
            get;
            set;
        }

        public string cost
        {
            get;
            set;
        }

        public string des
        {
            get;
            set;
        }

        public long FreightTemplateId
        {
            get;
            set;
        }

        public List<long> goodsCategory
        {
            get;
            set;
        }

        public string goodsName
        {
            get;
            set;
        }
        public string STRUCTURE_2D
        {
            get;
            set;
        }
        public string Plat_Code { get; set; }
        public string mallPrce
        {
            get;
            set;
        }

        public string marketPrice
        {
            get;
            set;
        }

        public string mdes
        {
            get;
            set;
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string MeasureUnit
        {
            get;
            set;
        }

        public List<string> pic
        {
            get;
            set;
        }

        /// <summary>
        /// 产品货号
        /// </summary>
        public string productCode
        {
            get;
            set;
        }

        public long productId
        {
            get;
            set;
        }

        public decimal rebate
        {
            get;
            set;
        }

        public int saleStatus
        {
            get;
            set;
        }

        public string seoDes
        {
            get;
            set;
        }

        public string seoKey
        {
            get;
            set;
        }

        public string seoTitle
        {
            get;
            set;
        }

        public List<Specifications> specifications
        {
            get;
            set;
        }

        public List<SpecificationsValue> specificationsValue
        {
            get;
            set;
        }

        /// <summary>
        /// 库存
        /// </summary>
        public long stock
        {
            get;
            set;
        }

        public List<long> styleTemplateId
        {
            get;
            set;
        }

        public decimal Volume
        {
            get;
            set;
        }

        public decimal Weight
        {
            get;
            set;
        }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EProductName
        {
            get;
            set;
        }

        /// <summary>
        /// /*产品CASNo*/    
        /// </summary>
        public string CASNo
        {
            get;
            set;
        }
        public string Purity
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
        public ProductDetailModel()
        {
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
        public static ProductInfo GetProductInfo(ProductDetailModel m, long productId)
        {
            CategoryInfo category = ServiceHelper.Create<ICategoryService>().GetCategory(m.categoryId);
            ProductInfo productInfo = new ProductInfo()
            {
                Id = productId,
                TypeId = category.TypeId,
                AddedDate = DateTime.Now,
                BrandId = 0,
                CategoryId = m.categoryId,
                CategoryPath = category.Path,
                MarketPrice = decimal.Parse(m.marketPrice),
                ShortDescription = m.adWord,
                ProductCode = m.productCode,
                ImagePath = "",
                DisplaySequence = 1,
                ProductName = m.goodsName,
                MinSalePrice = 0,
                ShopId = 1,
                HasSKU = true,
                EProductName = m.EProductName,
                CASNo = m.CASNo,
                Purity = m.Purity,
                HSCODE = m.HSCODE,
                DangerLevel = m.DangerLevel,
                MolecularFormula = m.MolecularFormula,
                ISCASNo = m.ISCASNo,
                Alias = m.Alias,
                Ealias = m.Ealias,
                MolecularWeight = m.MolecularWeight,
                PASNo = m.PASNo,
                LogP = m.LogP,
                Shape = m.Shape,
                Density = m.Density,
                FusingPoint = m.FusingPoint,
                BoilingPoint = m.BoilingPoint,
                FlashPoint = m.FlashPoint,
                RefractiveIndex = m.RefractiveIndex,
                StorageConditions = m.StorageConditions,
                VapourPressure = m.VapourPressure,
                PackagingLevel = m.PackagingLevel,
                SafetyInstructions = m.SafetyInstructions,
                DangerousMark = m.DangerousMark,
                RiskCategoryCode = m.RiskCategoryCode,
                TransportationNmber = m.TransportationNmber,
                RETCS = m.RETCS,
                WGKGermany = m.WGKGermany,
                SyntheticRoute = m.SyntheticRoute,
                RelatedProducts = m.RelatedProducts,
                MSDS = m.MSDS,
                NMRSpectrum = m.NMRSpectrum,
                STRUCTURE_2D = m.STRUCTURE_2D,
                Plat_Code = m.Plat_Code,
                ProductAttributeInfo = new List<ProductAttributeInfo>(),
                ChemCloud_ProductShopCategories = new List<ProductShopCategoryInfo>()
            };
            ProductDescriptionInfo productDescriptionInfo = new ProductDescriptionInfo()
            {
                Id = 0,
                AuditReason = "",
                Description = m.des,
                MobileDescription = m.mdes,
                DescriptionPrefixId = 0,
                DescriptiondSuffixId = 0,
                Meta_Description = m.seoDes,
                Meta_Keywords = m.seoKey,
                Meta_Title = m.seoTitle,
                ProductId = productId
            };
            productInfo.ProductDescriptionInfo = productDescriptionInfo;
            productInfo.SKUInfo = new List<SKUInfo>();
            productInfo.SaleStatus = (ProductInfo.ProductSaleStatus)m.saleStatus;
            productInfo.FreightTemplateId = 138;
            productInfo.MeasureUnit = m.MeasureUnit;
            productInfo.Volume = new decimal?(m.Volume);
            productInfo.Weight = new decimal?(m.Weight);
            ProductInfo productInfo1 = productInfo;  
            
            return productInfo1;
        }
    }
}