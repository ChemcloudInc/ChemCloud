using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class Attention : BaseModel
    {
        private long _id;
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
        public long UserId
        {
            get;
            set;
        }
        public long ShopId
        {
            get;
            set;
        }
        public long ProductId
        {
            get;
            set;
        }
        public DateTime? CreatDate
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
        public string UserName
        {
            get;
            set;
        }
        [NotMapped]
        public string ProductName
        {
            get;
            set;
        }
        [NotMapped]
        public string ImagePath
        {
            get;
            set;
        }
        [NotMapped]
        public string ProductCode
        {
            get;
            set;
        }
        [NotMapped]
        public decimal MarketPrice
        {
            get;
            set;
        }
        [NotMapped]
        public decimal MinSalePrice
        {
            get;
            set;
        }
        [NotMapped]
        public string MeasureUnit
        {
            get;
            set;
        }
        [NotMapped]
        public string EProductName
        {
            get;
            set;
        }
        [NotMapped]
        public string Purity
        {
            get;
            set;
        }
        [NotMapped]
        public string CASNo
        {
            get;
            set;
        }
        [NotMapped]
        public string HSCODE
        {
            get;
            set;
        }
        [NotMapped]
        public string DangerLevel
        {
            get;
            set;
        }
        [NotMapped]
        public string MolecularFormula
        {
            get;
            set;
        }
        [NotMapped]
        public string Alias
        {
            get;
            set;
        }
        [NotMapped]
        public string Ealias
        {
            get;
            set;
        }
        
        [NotMapped]
        public string MolecularWeight
        {
            get;
            set;
        }
        [NotMapped]
        public string PASNo
        {
            get;
            set;
        }
        [NotMapped]
        public string LogP
        {
            get;
            set;
        }
        [NotMapped]
        public string Shape
        {
            get;
            set;
        }
        [NotMapped]
        public string Density
        {
            get;
            set;
        }
        [NotMapped]
        public string FusingPoint
        {
            get;
            set;
        }
        [NotMapped]
        public string BoilingPoint
        {
            get;
            set;
        }
        [NotMapped]
        public string FlashPoint
        {
            get;
            set;
        }
        [NotMapped]
        public string RefractiveIndex
        {
            get;
            set;
        }
        [NotMapped]
        public string StorageConditions
        {
            get;
            set;
        }
        [NotMapped]
        public string VapourPressure
        {
            get;
            set;
        }
        [NotMapped]
        public string PackagingLevel
        {
            get;
            set;
        }
        [NotMapped]
        public string SafetyInstructions
        {
            get;
            set;
        }
        [NotMapped]
        public string DangerousMark
        {
            get;
            set;
        }
        [NotMapped]
        public string RiskCategoryCode
        {
            get;
            set;
        }
        [NotMapped]
        public string TransportationNmber
        {
            get;
            set;
        }
        [NotMapped]
        public string RETCS
        {
            get;
            set;
        }
        [NotMapped]
        public string WGKGermany
        {
            get;
            set;
        }
        [NotMapped]
        public string SyntheticRoute
        {
            get;
            set;
        }
        [NotMapped]
        public string RelatedProducts
        {
            get;
            set;
        }
        [NotMapped]
        public string MSDS
        {
            get;
            set;
        }
        [NotMapped]
        public string NMRSpectrum
        {
            get;
            set;
        }
        [NotMapped]
        public string RefuseReason
        {
            get;
            set;
        }
        [NotMapped]
        public string STRUCTURE_2D
        {
            get;
            set;
        }
        public Attention()
        {

        }
        public Attention(Attention m)
            : this()
        {
            Id = m.Id;
            UserId = m.UserId;
            ShopId = m.ShopId;
            ProductId = m.ProductId;
            CreatDate = m.CreatDate;
            ProductName = m.ProductName;
            ImagePath = m.ImagePath;
            MarketPrice = m.MarketPrice;
            MinSalePrice = m.MinSalePrice;
            MeasureUnit = m.MeasureUnit;
            EProductName = m.EProductName;
            Purity = m.Purity;
            CASNo = m.CASNo;
            HSCODE = m.HSCODE;
            DangerLevel = m.DangerLevel;
            MolecularFormula = m.MolecularFormula;
            MolecularWeight = m.MolecularWeight;
            PASNo = m.PASNo;
            LogP = m.LogP;
            Shape = m.Shape;
            Density = m.Density;
            FusingPoint = m.FusingPoint;
            BoilingPoint = m.BoilingPoint;
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
            RefuseReason = m.RefuseReason;
            STRUCTURE_2D = m.STRUCTURE_2D;
            UserName = m.UserName;
            CompanyName = m.CompanyName;
            ProductCode = m.ProductCode;
        } 
    }
}
