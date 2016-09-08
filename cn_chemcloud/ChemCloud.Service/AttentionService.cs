using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class AttentionService : ServiceBase, IAttentionService, IService, IDisposable
    {
        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ShopId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public bool AddAttention(long UserId,long ShopId,long ProductId)
        {
            Attention AttentionInfo = new Attention();
            //context.Attention.FirstOrDefault((Attention A) => A.ProductId.Equals(ProductId) && A.UserId.Equals(UserId) && A.ShopId.Equals(ShopId));
            int i = 0;
            if (context.Attention.FirstOrDefault((Attention m) => m.ProductId.Equals(ProductId) && m.UserId.Equals(UserId) && m.ShopId.Equals(ShopId)) == null)
            {
                AttentionInfo.CreatDate = DateTime.Now;
                AttentionInfo.ProductId = ProductId;
                AttentionInfo.UserId = UserId;
                AttentionInfo.ShopId = ShopId;
                context.Attention.Add(AttentionInfo);
                i = context.SaveChanges();
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteAttention(long Id)
        {
            Attention AttentionInfo = context.Attention.FindBy((Attention item) => item.Id == Id).FirstOrDefault();
            context.Attention.Remove(AttentionInfo);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public Attention GetAttention(long Id)
        {
            Attention AttentionInfo = context.Attention.FindBy((Attention item) => item.Id == Id).FirstOrDefault();
            ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(AttentionInfo.ShopId));
            if (shopInfo != null)
            {
                AttentionInfo.CompanyName = shopInfo.ShopName;
            }
            UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(AttentionInfo.UserId) && m.UserType == 3);
            if (UserInfo != null)
            {
                AttentionInfo.UserName = UserInfo.UserName;
            }
            ProductInfo productInfo = context.ProductInfo.FirstOrDefault((ProductInfo m) => m.Id.Equals(AttentionInfo.ProductId));
            if (productInfo != null)
            {
                AttentionInfo.ProductName = (productInfo == null ? "" : productInfo.ProductName);
                AttentionInfo.ImagePath = (productInfo == null ? "" : productInfo.ImagePath);
                AttentionInfo.MarketPrice = productInfo.MarketPrice;
                AttentionInfo.MinSalePrice = productInfo.MinSalePrice;
                AttentionInfo.MeasureUnit = productInfo.MeasureUnit;
                AttentionInfo.EProductName = productInfo.EProductName;
                AttentionInfo.Purity = productInfo.Purity;
                AttentionInfo.CASNo = productInfo.CASNo;
                AttentionInfo.HSCODE = productInfo.HSCODE;
                AttentionInfo.DangerLevel = productInfo.DangerLevel;
                AttentionInfo.MolecularFormula = productInfo.MolecularFormula;
                AttentionInfo.MolecularWeight = productInfo.MolecularWeight;
                AttentionInfo.PASNo = productInfo.PASNo;
                AttentionInfo.LogP = productInfo.LogP;
                AttentionInfo.Shape = productInfo.Shape;
                AttentionInfo.Density = productInfo.Density;
                AttentionInfo.FusingPoint = productInfo.FusingPoint;
                AttentionInfo.BoilingPoint = productInfo.BoilingPoint;
                AttentionInfo.RefractiveIndex = productInfo.RefractiveIndex;
                AttentionInfo.StorageConditions = productInfo.StorageConditions;
                AttentionInfo.VapourPressure = productInfo.VapourPressure;
                AttentionInfo.PackagingLevel = productInfo.PackagingLevel;
                AttentionInfo.SafetyInstructions = productInfo.SafetyInstructions;
                AttentionInfo.DangerousMark = productInfo.DangerousMark;
                AttentionInfo.RiskCategoryCode = productInfo.RiskCategoryCode;
                AttentionInfo.TransportationNmber = productInfo.TransportationNmber;
                AttentionInfo.RETCS = productInfo.RETCS;
                AttentionInfo.WGKGermany = productInfo.WGKGermany;
                AttentionInfo.SyntheticRoute = productInfo.SyntheticRoute;
                AttentionInfo.RelatedProducts = productInfo.RelatedProducts;
                AttentionInfo.MSDS = productInfo.MSDS;
                AttentionInfo.NMRSpectrum = productInfo.NMRSpectrum;
                AttentionInfo.RefuseReason = productInfo.RefuseReason;
                AttentionInfo.STRUCTURE_2D = productInfo.STRUCTURE_2D;
            }
            return AttentionInfo;
        }
        public PageModel<Attention> SupplierGetAttentions(AttentionQuery AttentionQueryModel)
        {
            IQueryable<Attention> AttentionShop = from item in base.context.Attention
                                                  where item.ShopId == AttentionQueryModel.ShopId.Value
                                                  select item;
            if (!string.IsNullOrWhiteSpace(AttentionQueryModel.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(AttentionQueryModel.beginTime, out dt))
                {
                    DateTime value = DateTime.Parse(AttentionQueryModel.beginTime);
                    AttentionShop =
                    from d in AttentionShop
                    where d.CreatDate >= value
                    select d;
                }
            }
            //end time
            if (!string.IsNullOrWhiteSpace(AttentionQueryModel.endTime))
            {
                DateTime dt;
                if (DateTime.TryParse(AttentionQueryModel.endTime, out dt))
                {
                    DateTime value = DateTime.Parse(AttentionQueryModel.endTime);
                    AttentionShop =
                    from d in AttentionShop
                    where d.CreatDate <= value
                    select d;
                }
            }
            if (!string.IsNullOrEmpty(AttentionQueryModel.productName))
            {
                AttentionShop =
                from item in AttentionShop
                where item.ProductName.Contains(AttentionQueryModel.productName)
                select item;
            }
            if (!string.IsNullOrEmpty(AttentionQueryModel.compamyName))
            {
                AttentionShop =
                from item in AttentionShop
                where item.CompanyName.Contains(AttentionQueryModel.compamyName)
                select item;
            }
            if (!string.IsNullOrEmpty(AttentionQueryModel.userName))
            {
                AttentionShop =
                from item in AttentionShop
                where item.UserName.Contains(AttentionQueryModel.userName)
                select item;
            }
            Func<IQueryable<Attention>, IOrderedQueryable<Attention>> func = null;
            func = (IQueryable<Attention> d) =>
                    from o in d
                    orderby o.CreatDate descending
                    select o;
            int num = AttentionShop.Count();
            AttentionShop = AttentionShop.GetPage(out num, AttentionQueryModel.PageNo, AttentionQueryModel.PageSize, func);
            foreach (Attention list in AttentionShop.ToList())
            {
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(list.ShopId));
                if (shopInfo != null)
                {
                    list.CompanyName = shopInfo.ShopName;
                }
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(list.UserId) && m.UserType == 3);
                if (UserInfo != null)
                {
                    list.UserName = UserInfo.UserName;
                }
                ProductInfo productInfo = context.ProductInfo.FirstOrDefault((ProductInfo m) => m.Id.Equals(list.ProductId));
                if (productInfo != null)
                {
                    list.ProductName = (productInfo == null ? "" : productInfo.ProductName);
                    list.ImagePath = (productInfo == null ? "" : productInfo.ImagePath);
                    list.MarketPrice = productInfo.MarketPrice;
                    list.MinSalePrice = productInfo.MinSalePrice;
                    list.MeasureUnit = productInfo.MeasureUnit;
                    list.EProductName = productInfo.EProductName;
                    list.Purity = productInfo.Purity;
                    list.CASNo = productInfo.CASNo;
                    list.HSCODE = productInfo.HSCODE;
                    list.DangerLevel = productInfo.DangerLevel;
                    list.MolecularFormula = productInfo.MolecularFormula;
                    list.MolecularWeight = productInfo.MolecularWeight;
                    list.PASNo = productInfo.PASNo;
                    list.LogP = productInfo.LogP;
                    list.Shape = productInfo.Shape;
                    list.Density = productInfo.Density;
                    list.FusingPoint = productInfo.FusingPoint;
                    list.BoilingPoint = productInfo.BoilingPoint;
                    list.RefractiveIndex = productInfo.RefractiveIndex;
                    list.StorageConditions = productInfo.StorageConditions;
                    list.VapourPressure = productInfo.VapourPressure;
                    list.PackagingLevel = productInfo.PackagingLevel;
                    list.SafetyInstructions = productInfo.SafetyInstructions;
                    list.DangerousMark = productInfo.DangerousMark;
                    list.RiskCategoryCode = productInfo.RiskCategoryCode;
                    list.TransportationNmber = productInfo.TransportationNmber;
                    list.RETCS = productInfo.RETCS;
                    list.WGKGermany = productInfo.WGKGermany;
                    list.SyntheticRoute = productInfo.SyntheticRoute;
                    list.RelatedProducts = productInfo.RelatedProducts;
                    list.MSDS = productInfo.MSDS;
                    list.NMRSpectrum = productInfo.NMRSpectrum;
                    list.RefuseReason = productInfo.RefuseReason;
                    list.STRUCTURE_2D = productInfo.STRUCTURE_2D;
                }
            }
            return new PageModel<Attention>()
            {
                Models = AttentionShop,
                Total = num
            };
        }
        /// <summary>
        /// 获取关注记录
        /// </summary>
        /// <param name="AttentionQueryModel"></param>
        /// <returns></returns>
        public PageModel<Attention> GetAttentions(AttentionQuery AttentionQueryModel)
        {
            IQueryable<Attention> AttentionShop = from item in base.context.Attention
                                                  where item.UserId == AttentionQueryModel.UserId.Value
                                                  select item;
            if (!string.IsNullOrWhiteSpace(AttentionQueryModel.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(AttentionQueryModel.beginTime, out dt))
                {
                    DateTime value = DateTime.Parse(AttentionQueryModel.beginTime);
                    AttentionShop =
                    from d in AttentionShop
                    where d.CreatDate >= value
                    select d;
                }
            }
            //end time
            if (!string.IsNullOrWhiteSpace(AttentionQueryModel.endTime))
            {
                DateTime dt;
                if (DateTime.TryParse(AttentionQueryModel.endTime, out dt))
                {
                    DateTime value = DateTime.Parse(AttentionQueryModel.endTime);
                    AttentionShop =
                    from d in AttentionShop
                    where d.CreatDate <= value
                    select d;
                }
            }
            if (!string.IsNullOrEmpty(AttentionQueryModel.productName))
            {
                AttentionShop =
                from item in AttentionShop
                where item.ProductName.Contains(AttentionQueryModel.productName)
                select item;
            }
            if (!string.IsNullOrEmpty(AttentionQueryModel.compamyName))
            {
                AttentionShop =
                from item in AttentionShop
                where item.CompanyName.Contains(AttentionQueryModel.compamyName)
                select item;
            }
            if (!string.IsNullOrEmpty(AttentionQueryModel.userName))
            {
                AttentionShop =
                from item in AttentionShop
                where item.UserName.Contains(AttentionQueryModel.userName)
                select item;
            }
            Func<IQueryable<Attention>, IOrderedQueryable<Attention>> func = null;
            func = (IQueryable<Attention> d) =>
                    from o in d
                    orderby o.CreatDate descending
                    select o;
            int num = AttentionShop.Count();
            AttentionShop = AttentionShop.GetPage(out num, AttentionQueryModel.PageNo, AttentionQueryModel.PageSize, func);
            foreach (Attention list in AttentionShop.ToList())
            {
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(list.ShopId));
                if (shopInfo != null)
                {
                    list.CompanyName = shopInfo.ShopName;
                }
                UserMemberInfo UserInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(list.UserId) && m.UserType == 3);
                if (UserInfo != null)
                {
                    list.UserName = UserInfo.UserName;
                }
                ProductInfo productInfo = context.ProductInfo.FirstOrDefault((ProductInfo m) => m.Id.Equals(list.ProductId));
                if (productInfo != null)
                {
                    list.ProductName = (productInfo == null ? "" : productInfo.ProductName);
                    list.ImagePath = (productInfo == null ? "" : productInfo.ImagePath);
                    list.MarketPrice = productInfo.MarketPrice;
                    list.MinSalePrice = productInfo.MinSalePrice;
                    list.MeasureUnit = productInfo.MeasureUnit;
                    list.EProductName = productInfo.EProductName;
                    list.Purity = productInfo.Purity;
                    list.CASNo = productInfo.CASNo;
                    list.HSCODE = productInfo.HSCODE;
                    list.DangerLevel = productInfo.DangerLevel;
                    list.MolecularFormula = productInfo.MolecularFormula;
                    list.MolecularWeight = productInfo.MolecularWeight;
                    list.PASNo = productInfo.PASNo;
                    list.LogP = productInfo.LogP;
                    list.Shape = productInfo.Shape;
                    list.Density = productInfo.Density;
                    list.FusingPoint = productInfo.FusingPoint;
                    list.BoilingPoint = productInfo.BoilingPoint;
                    list.RefractiveIndex = productInfo.RefractiveIndex;
                    list.StorageConditions = productInfo.StorageConditions;
                    list.VapourPressure = productInfo.VapourPressure;
                    list.PackagingLevel = productInfo.PackagingLevel;
                    list.SafetyInstructions = productInfo.SafetyInstructions;
                    list.DangerousMark = productInfo.DangerousMark;
                    list.RiskCategoryCode = productInfo.RiskCategoryCode;
                    list.TransportationNmber = productInfo.TransportationNmber;
                    list.RETCS = productInfo.RETCS;
                    list.WGKGermany = productInfo.WGKGermany;
                    list.SyntheticRoute = productInfo.SyntheticRoute;
                    list.RelatedProducts = productInfo.RelatedProducts;
                    list.MSDS = productInfo.MSDS;
                    list.NMRSpectrum = productInfo.NMRSpectrum;
                    list.RefuseReason = productInfo.RefuseReason;
                    list.STRUCTURE_2D = productInfo.STRUCTURE_2D;
                }
            }
            return new PageModel<Attention>()
            {
                Models = AttentionShop,
                Total = num
            };
        }
    }
}
