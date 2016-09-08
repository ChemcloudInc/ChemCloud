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

namespace ChemCloud.Web.Areas.SellerAdmin.Models
{
    public class AttentionModel
    {
        public long Id
        {
            get;
            set;
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
        public string UserName
        {
            get;
            set;
        }
        public string CompanyName 
        {
            get;
            set;
        }
        public string ProductName
        {
            get;
            set;
        }

        public string ImagePath
        {
            get;
            set;
        }

        public decimal? MarketPrice
        {
            get;
            set;
        }

        public decimal? MinSalePrice
        {
            get;
            set;
        }

        public string MeasureUnit
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

        public string RefuseReason
        {
            get;
            set;
        }

        public string STRUCTURE_2D
        {
            get;
            set;
        }
        public string ProductCode
        {
            get;
            set;
        }
        public AttentionModel()
        {

        }
        public AttentionModel(Attention m)
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