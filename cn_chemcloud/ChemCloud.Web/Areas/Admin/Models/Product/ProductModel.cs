using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Models.Product
{
    public class ProductModel
    {
        public string RefuseReason
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string brandName
        {
            get;
            set;
        }

        public string categoryName
        {
            get;
            set;
        }

        public long id
        {
            get;
            set;
        }

        public string imgUrl
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public decimal price
        {
            get;
            set;
        }

        public string productCode
        {
            get;
            set;
        }

        public string saleStatus
        {
            get;
            set;
        }

        public string shopName
        {
            get;
            set;
        }

        public string state
        {
            get;
            set;
        }

        public string url
        {
            get;
            set;
        }
        public string CASNo
        {
            get;
            set;
        }
        public string EProductName
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
        public string CompanyName
        { get; set; }
        public string NMRSpectrum
        {
            get;
            set;
        }
        public int ShopStatus
        {
            get;
            set;
        }
        public long ShopId
        {
            get;
            set;
        }
        public string ProductCode
        {
            get;
            set;
        }
        public int CID
        {
            get;
            set;
        }
        public ProductModel()
        {
        }

        public int AuditStatus { get; set; }

        public string AuditReason { get; set; }

        public ProductModel(ProductInfo m)
            : this()
        {
            Status = m.AuditStatus.ToDescription();
            RefuseReason = m.RefuseReason;
            brandName = m.BrandName;
            categoryName = m.CategoryName;
            name = m.ProductName;
            id = m.Id;
            saleStatus = m.SaleStatus.ToDescription();
            shopName = m.ShopName;
            state = m.ShowProductState;
            CASNo = m.CASNo;
            price = m.MinSalePrice;
            EProductName = m.EProductName;
            Purity = m.Purity;
            HSCODE = m.HSCODE;
            DangerLevel = m.DangerLevel;
            MolecularFormula = m.MolecularFormula;
            PASNo = m.PASNo;
            Alias = m.Alias;
            Ealias = m.Ealias;
            MolecularWeight = m.MolecularFormula;
            LogP = m.LogP;
            Shape = m.Shape;
            Density = m.Density;
            FusingPoint = m.FusingPoint;
            BoilingPoint = m.BoilingPoint;
            FlashPoint = m.FlashPoint;
            RefractiveIndex = m.RefractiveIndex;
            StorageConditions = m.StorageConditions;
            VapourPressure = m.VapourPressure;
            PackagingLevel = m.PackagingLevel;
            SafetyInstructions = m.SafetyInstructions;
            DangerousMark = m.DangerousMark;
            RiskCategoryCode = m.RiskCategoryCode;
            TransportationNmber = m.TransportationNmber;
            RETCS = m.RETCS;
            WGKGermany = m.WGKGermany;
            SyntheticRoute = m.SyntheticRoute;
            RelatedProducts = m.RelatedProducts;
            MSDS = m.MSDS;
            NMRSpectrum = m.NMRSpectrum;
            CompanyName = m.CompanyName;
            ProductCode = m.ProductCode;
            CID = m.Pub_CID;
        }
    }
}