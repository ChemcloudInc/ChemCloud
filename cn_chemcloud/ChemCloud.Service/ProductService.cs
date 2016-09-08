﻿using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Model.Common;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChemCloud.Service
{
    public class ProductService : ServiceBase, IProductService, IService, IDisposable
    {
        int cointype = 1;
        public ProductService()
        {
        }
        public void UpdateStatus(long Id, ProductInfo.ProductAuditStatus status, string comments = "")
        {
            ProductInfo nullable = context.ProductInfo.FindById<ProductInfo>(Id);
            nullable.AuditStatus = status;
            if (!string.IsNullOrWhiteSpace(comments))
            {
                nullable.RefuseReason = comments;
            }
            context.SaveChanges();
        }
        public void AddBrowsingProduct(BrowsingHistoryInfo info)
        {
            BrowsingHistoryInfo browseTime = context.BrowsingHistoryInfo.FirstOrDefault((BrowsingHistoryInfo a) => a.ProductId == info.ProductId && a.MemberId == info.MemberId);
            if (browseTime != null)
            {
                browseTime.BrowseTime = info.BrowseTime;
            }
            else if (context.BrowsingHistoryInfo.Count((BrowsingHistoryInfo a) => a.MemberId == info.MemberId) >= 20)
            {
                BrowsingHistoryInfo browsingHistoryInfo = (
                    from a in context.BrowsingHistoryInfo
                    where a.MemberId == info.MemberId
                    orderby a.BrowseTime
                    select a).FirstOrDefault();
                context.BrowsingHistoryInfo.Remove(browsingHistoryInfo);
                context.BrowsingHistoryInfo.Add(info);
            }
            else
            {
                context.BrowsingHistoryInfo.Add(info);
            }
            context.SaveChanges();
        }
        public bool AddFavorites(long productId, long userId)
        {
            FavoriteInfo favoriteInfo = new FavoriteInfo();
            int i = 0;
            if (context.FavoriteInfo.FirstOrDefault((FavoriteInfo f) => f.ProductId.Equals(productId) && f.UserId.Equals(userId)) == null)
            {
                favoriteInfo.Date = DateTime.Now;
                favoriteInfo.ProductId = productId;
                favoriteInfo.UserId = userId;
                favoriteInfo.Tags = "";
                context.FavoriteInfo.Add(favoriteInfo);
                i = context.SaveChanges();
                //throw new HimallException("您已经关注过该商品");
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public void AddFavorite(long productId, long userId, out int status)
        {
            FavoriteInfo favoriteInfo = context.FavoriteInfo.FirstOrDefault((FavoriteInfo f) => f.ProductId.Equals(productId) && f.UserId.Equals(userId));
            if (favoriteInfo == null || favoriteInfo.ProductId != productId)
            {
                DbSet<FavoriteInfo> favoriteInfos = context.FavoriteInfo;
                FavoriteInfo favoriteInfo1 = new FavoriteInfo()
                {
                    Date = DateTime.Now,
                    ProductId = productId,
                    UserId = userId,
                    Tags = ""
                };
                favoriteInfos.Add(favoriteInfo1);
                status = 0;
            }
            else
            {
                status = 1;
            }
            context.SaveChanges();
        }

        public void AddProduct(ProductInfo model)
        {
            model.SaleStatus = ProductInfo.ProductSaleStatus.OnSale;
            model.AuditStatus = ProductInfo.ProductAuditStatus.Audited;
            model.EditStatus = 3;
            context.ProductInfo.Add(model);
            context.SaveChanges();
        }
        public bool AddProducts(ProductInfo model)
        {
            int i = 0;
            try
            {
                model.SaleStatus = ProductInfo.ProductSaleStatus.OnSale;
                model.AuditStatus = ProductInfo.ProductAuditStatus.Audited;
                context.ProductInfo.Add(model);
                i = context.SaveChanges();
                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }
        public void AddProductSpec(ProductInfo model, long id)
        {

            IQueryable<ProductSpec> spec =
                    from s in context.ProductSpec
                    where s.ProductId == id
                    select s;
            context.ProductSpec.RemoveRange(spec);
            int i = context.SaveChanges();
            foreach (var item in model._ProductSpec)
            {
                context.ProductSpec.Add(item);
                context.SaveChanges();
            }
        }
        public bool AddProductSpecs(ProductSpec model)
        {
            context.ProductSpec.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public List<ProductSpec> GetDetial(int type, long id)
        {
            return (from s in context.ProductSpec where s.ProductId == id && s.CoinType == type select s).ToList<ProductSpec>();
        }
        public void AddSKU(ProductInfo pInfo)
        {
            IQueryable<SKUInfo> sKUInfo =
                from s in context.SKUInfo
                where s.ProductId == pInfo.Id
                select s;
            context.SKUInfo.RemoveRange(sKUInfo);
            context.SKUInfo.AddRange(pInfo.SKUInfo);
            context.SaveChanges();
        }

        public bool ApplyForSale(long id)
        {
            ProductInfo productInfo = context.ProductInfo.FindById<ProductInfo>(id);
            ProductInfo.ProductSaleStatus saleStatus = productInfo.SaleStatus;
            bool flag = true;
            if (productInfo.AuditStatus == ProductInfo.ProductAuditStatus.InfractionSaleOff)
            {
                productInfo.EditStatus = 4;
                flag = false;
            }
            if (productInfo.SaleStatus == ProductInfo.ProductSaleStatus.InDelete)
            {
                productInfo.EditStatus = 4;
                flag = false;
            }
            if (Instance<ISiteSettingService>.Create.GetSiteSettings().ProdutAuditOnOff == 0)
            {
                if (productInfo.AuditStatus == ProductInfo.ProductAuditStatus.InfractionSaleOff)
                {
                    throw new HimallException("违规下架的产品不能申请免审核上架！");
                }
                if (flag && productInfo.EditStatus < 4)
                {
                    productInfo.EditStatus = 0;
                }
            }
            productInfo.SaleStatus = ProductInfo.ProductSaleStatus.OnSale;
            if (productInfo.EditStatus == 3 || productInfo.EditStatus == 2 || productInfo.EditStatus == 4 || productInfo.EditStatus == 5)
            {

                if (productInfo.CASNo == null || productInfo.CASNo == "")
                {
                    productInfo.AuditStatus = ProductInfo.ProductAuditStatus.NoCasNoWaitAuditing;
                }
                //else
                //{
                //    productInfo.AuditStatus = ProductInfo.ProductAuditStatus.WaitForAuditing;
                //}

            }
            else
            {
                productInfo.AuditStatus = ProductInfo.ProductAuditStatus.Audited;
            }
            context.SaveChanges();
            return true;
        }

        public void AuditProduct(long productId, ProductInfo.ProductAuditStatus auditStatus, string message)
        {
            ProductInfo productInfo = context.ProductInfo.FindById<ProductInfo>(productId);
            if (productInfo == null)
            {
                throw new HimallException("此产品不存在");
            }
            if (productInfo.SaleStatus == ProductInfo.ProductSaleStatus.InDelete)
            {
                throw new HimallException("此产品已被删除");
            }
            AuditProducts(new long[] { productId }, auditStatus, message);
        }

        public void AuditProducts(IEnumerable<long> productIds, ProductInfo.ProductAuditStatus auditStatus, string message)
        {
            foreach (ProductInfo info in (from item in base.context.ProductInfo
                                          where productIds.Contains(item.Id) && (((int)item.SaleStatus) != 4)
                                          select item).ToList())
            {
                if (info.SaleStatus == ProductInfo.ProductSaleStatus.InDelete)
                {
                    continue;
                }
                info.AuditStatus = auditStatus;
                switch (auditStatus)
                {
                    case ProductInfo.ProductAuditStatus.Audited:
                        info.EditStatus = 0;
                        break;

                    case ProductInfo.ProductAuditStatus.InfractionSaleOff:
                        info.EditStatus = 4;
                        break;
                }
                if (!string.IsNullOrWhiteSpace(message))
                {
                    if (info.ProductDescriptionInfo == null)
                    {
                        info.ProductDescriptionInfo = new ProductDescriptionInfo();
                    }
                    info.ProductDescriptionInfo.AuditReason = message;
                }
            }
            base.context.SaveChanges();
        }

        public void BindTemplate(long? topTemplateId, long? bottomTemplateId, IEnumerable<long> productIds)
        {
            ProductInfo[] array = (
                from item in context.ProductInfo
                where productIds.Contains(item.Id) && (int)item.SaleStatus != 4
                select item).ToArray();
            ProductInfo[] productInfoArray = array;
            for (int i = 0; i < productInfoArray.Length; i++)
            {
                ProductInfo productDescriptionInfo = productInfoArray[i];
                if (productDescriptionInfo.ProductDescriptionInfo == null)
                {
                    productDescriptionInfo.ProductDescriptionInfo = new ProductDescriptionInfo();
                }
                if (topTemplateId.HasValue)
                {
                    productDescriptionInfo.ProductDescriptionInfo.DescriptionPrefixId = topTemplateId.Value;
                }
                if (bottomTemplateId.HasValue)
                {
                    productDescriptionInfo.ProductDescriptionInfo.DescriptiondSuffixId = bottomTemplateId.Value;
                }
            }
            context.SaveChanges();
        }

        public void CancelConcernProducts(IEnumerable<long> ids, long userId)
        {
            context.FavoriteInfo.OrderBy((FavoriteInfo a) => a.UserId == userId && ids.Contains(a.Id));
            context.SaveChanges();
        }

        private void CheckWhenGetFreight(IEnumerable<string> skuIds, IEnumerable<int> counts, int cityId)
        {
            if (skuIds == null || skuIds.Count() == 0)
            {
                throw new InvalidPropertyException("待计算运费的产品不能这空");
            }
            if (counts == null || counts.Count() == 0)
            {
                throw new InvalidPropertyException("待计算运费的产品数量不能这空");
            }
            if (counts.Count((int item) => item <= 0) > 0)
            {
                throw new InvalidPropertyException("待计算运费的产品数量必须都大于0");
            }
            if (skuIds.Count() != counts.Count())
            {
                throw new InvalidPropertyException("产品数量不一致");
            }
            if (cityId <= 0)
            {
                throw new InvalidPropertyException("收货地址无效");
            }
            IProductService create = Instance<IProductService>.Create;
            for (int i = 0; i < skuIds.Count(); i++)
            {
                SKUInfo sku = create.GetSku(skuIds.ElementAt<string>(i));
                if (sku == null)
                {
                    throw new HimallException(string.Concat("未找到", sku, "对应的产品"));
                }
            }
        }

        private void CheckWhenGetFreight(IEnumerable<long> productIds, IEnumerable<int> counts, int cityId)
        {
            if (productIds == null || productIds.Count() == 0)
            {
                throw new InvalidPropertyException("待计算运费的产品不能这空");
            }
            if (counts == null || counts.Count() == 0)
            {
                throw new InvalidPropertyException("待计算运费的产品数量不能这空");
            }
            if (counts.Count((int item) => item <= 0) > 0)
            {
                throw new InvalidPropertyException("待计算运费的产品数量必须都大于0");
            }
            if (productIds.Count() != counts.Count())
            {
                throw new InvalidPropertyException("产品数量不一致");
            }
            if (cityId <= 0)
            {
                throw new InvalidPropertyException("收货地址无效");
            }
            IProductService create = Instance<IProductService>.Create;
            for (int i = 0; i < productIds.Count(); i++)
            {
                ProductInfo product = create.GetProduct(productIds.ElementAt<long>(i));
                if (product == null)
                {
                    throw new HimallException(string.Concat("未找到", product.ProductName, "对应的产品"));
                }
            }
        }

        public void DeleteFavorite(long productId, long userId)
        {
            context.FavoriteInfo.OrderBy((FavoriteInfo item) => item.UserId == userId && item.ProductId == productId);
            context.SaveChanges();
        }

        public void DeleteProduct(long id)
        {
            context.ProductInfo.Remove(context.ProductInfo.FindById<ProductInfo>(id));
            context.ProductDescriptionInfo.Remove(context.ProductDescriptionInfo.FirstOrDefault((ProductDescriptionInfo p) => p.ProductId == id));
            context.SaveChanges();
        }

        public void DeleteProduct(IEnumerable<long> ids, long shopId)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                List<ProductInfo> list = (
                    from item in context.ProductInfo
                    where ids.Contains(item.Id) && (int)item.SaleStatus != 4
                    select item).ToList();
                foreach (ProductInfo productInfo in list)
                {
                    productInfo.SaleStatus = ProductInfo.ProductSaleStatus.InDelete;
                    productInfo.EditStatus = 4;
                }
                List<MobileHomeProductsInfo> mobileHomeProductsInfos = (
                    from d in context.MobileHomeProductsInfo
                    where ids.Contains(d.ProductId)
                    select d).ToList();
                context.MobileHomeProductsInfo.RemoveRange(mobileHomeProductsInfos);
                List<ModuleProductInfo> moduleProductInfos = (
                    from d in context.ModuleProductInfo
                    where ids.Contains(d.ProductId)
                    select d).ToList();
                context.ModuleProductInfo.RemoveRange(moduleProductInfos);
                context.SaveChanges();
                transactionScope.Complete();
            }
        }

        public AttributeInfo GetAttributeInfo(long attrId)
        {
            return context.AttributeInfo.FindById<AttributeInfo>(attrId);
        }

        public IQueryable<BrowsingHistoryInfo> GetBrowsingProducts(long userId)
        {
            return
                from p in context.BrowsingHistoryInfo.Include("ChemCloud_Products")
                where p.MemberId == userId
                select p;
        }

        public ProductInfo.ProductEditStatus GetEditStatus(long id, ProductInfo model)
        {
            ProductInfo.ProductEditStatus editStatus = ProductInfo.ProductEditStatus.PendingAudit;
            ProductInfo productInfo = context.ProductInfo.FirstOrDefault((ProductInfo d) => d.Id == id);
            if (productInfo != null)
            {
                editStatus = (ProductInfo.ProductEditStatus)productInfo.EditStatus;
                if (productInfo.ProductName != model.ProductName)
                {
                    editStatus = (editStatus <= ProductInfo.ProductEditStatus.EditedAndPending ? ProductInfo.ProductEditStatus.EditedAndPending : ProductInfo.ProductEditStatus.CompelPendingHasEdited);
                }
            }
            return editStatus;
        }

        public decimal GetFreight(IEnumerable<long> productIds, IEnumerable<int> counts, int cityId)
        {
            IEnumerable<int> array = counts;
            CheckWhenGetFreight(productIds, array, cityId);
            decimal freight2 = new decimal(0);
            productIds = productIds.ToArray();
            array = array.ToArray();
            IProductService create = Instance<IProductService>.Create;
            IFreightTemplateService freightTemplateService = Instance<IFreightTemplateService>.Create;
            int num2 = 0;
            var list = productIds.Select((long productId) =>
            {
                ProductInfo product = create.GetProduct(productId);
                IEnumerable<int> nums = array;
                int num = num2;
                int num1 = num;
                num2 = num + 1;
                return new { Product = product, Quantity = nums.ElementAt<int>(num1) };
            }).ToList();
            var freightTemplateId =
                from item in list
                group item by item.Product.FreightTemplateId;
            foreach (var nums1 in freightTemplateId)
            {
                FreightTemplateInfo freightTemplate = freightTemplateService.GetFreightTemplate(nums1.Key);
                if (freightTemplate == null || freightTemplate.IsFree != FreightTemplateInfo.FreightTemplateType.SelfDefine)
                {
                    continue;
                }
                FreightAreaContentInfo freightAreaContentInfo = (
                    from item in freightTemplate.ChemCloud_FreightAreaContent
                    where item.AreaContent.Split(new char[] { ',' }).Contains<string>(cityId.ToString())
                    select item).FirstOrDefault() ?? freightTemplate.ChemCloud_FreightAreaContent.Where((FreightAreaContentInfo item) =>
                    {
                        byte? isDefault = item.IsDefault;
                        if (isDefault.GetValueOrDefault() != 1)
                        {
                            return false;
                        }
                        return isDefault.HasValue;
                    }).FirstOrDefault();
                if (freightTemplate.ValuationMethod == FreightTemplateInfo.ValuationMethodType.Weight)
                {
                    decimal num3 = nums1.Sum((item) =>
                    {
                        if (!item.Product.Weight.HasValue)
                        {
                            return new decimal(0);
                        }
                        return item.Product.Weight.Value * item.Quantity;
                    });
                    int value = freightAreaContentInfo.FirstUnit.Value;
                    decimal value1 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int value2 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? accumulationUnitMoney = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num3, value, value1, value2, (decimal)accumulationUnitMoney.Value);
                }
                else if (freightTemplate.ValuationMethod != FreightTemplateInfo.ValuationMethodType.Bulk)
                {
                    int num4 = nums1.Sum((item) => item.Quantity);
                    decimal num5 = num4;
                    int value3 = freightAreaContentInfo.FirstUnit.Value;
                    decimal value4 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int value5 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? nullable = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num5, value3, value4, value5, (decimal)nullable.Value);
                }
                else
                {
                    decimal num6 = nums1.Sum((item) =>
                    {
                        if (!item.Product.Volume.HasValue)
                        {
                            return new decimal(0);
                        }
                        return item.Product.Volume.Value * item.Quantity;
                    });
                    int value6 = freightAreaContentInfo.FirstUnit.Value;
                    decimal value7 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int num7 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? accumulationUnitMoney1 = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num6, value6, value7, num7, (decimal)accumulationUnitMoney1.Value);
                }
            }
            return freight2;
        }

        public decimal GetFreight(IEnumerable<string> skuIds, IEnumerable<int> counts, int cityId)
        {
            IEnumerable<int> array = counts;
            CheckWhenGetFreight(skuIds, array, cityId);
            decimal freight2 = new decimal(0);
            skuIds = skuIds.ToArray();
            array = array.ToArray();
            IProductService create = Instance<IProductService>.Create;
            IFreightTemplateService freightTemplateService = Instance<IFreightTemplateService>.Create;
            int num2 = 0;
            var list = skuIds.Select((string skuId) =>
            {
                SKUInfo sku = create.GetSku(skuId);
                ProductInfo product = create.GetProduct(sku.ProductId);
                IEnumerable<int> nums = array;
                int num = num2;
                int num1 = num;
                num2 = num + 1;
                return new { SKU = sku, Product = product, Quantity = nums.ElementAt<int>(num1) };
            }).ToList();
            var freightTemplateId =
                from item in list
                group item by item.Product.FreightTemplateId;
            foreach (var nums1 in freightTemplateId)
            {
                FreightTemplateInfo freightTemplate = freightTemplateService.GetFreightTemplate(nums1.Key);
                if (freightTemplate == null || freightTemplate.IsFree != FreightTemplateInfo.FreightTemplateType.SelfDefine)
                {
                    continue;
                }
                FreightAreaContentInfo freightAreaContentInfo = (
                    from item in freightTemplate.ChemCloud_FreightAreaContent
                    where item.AreaContent.Split(new char[] { ',' }).Contains<string>(cityId.ToString())
                    select item).FirstOrDefault() ?? freightTemplate.ChemCloud_FreightAreaContent.Where((FreightAreaContentInfo item) =>
                    {
                        byte? isDefault = item.IsDefault;
                        if (isDefault.GetValueOrDefault() != 1)
                        {
                            return false;
                        }
                        return isDefault.HasValue;
                    }).FirstOrDefault();
                if (freightTemplate.ValuationMethod == FreightTemplateInfo.ValuationMethodType.Weight)
                {
                    decimal num3 = nums1.Sum((item) =>
                    {
                        if (!item.Product.Weight.HasValue)
                        {
                            return new decimal(0);
                        }
                        return item.Product.Weight.Value * item.Quantity;
                    });
                    int value = freightAreaContentInfo.FirstUnit.Value;
                    decimal value1 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int value2 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? accumulationUnitMoney = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num3, value, value1, value2, (decimal)accumulationUnitMoney.Value);
                }
                else if (freightTemplate.ValuationMethod != FreightTemplateInfo.ValuationMethodType.Bulk)
                {
                    int num4 = nums1.Sum((item) => item.Quantity);
                    decimal num5 = num4;
                    int value3 = freightAreaContentInfo.FirstUnit.Value;
                    decimal value4 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int value5 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? nullable = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num5, value3, value4, value5, (decimal)nullable.Value);
                }
                else
                {
                    decimal num6 = nums1.Sum((item) =>
                    {
                        if (!item.Product.Volume.HasValue)
                        {
                            return new decimal(0);
                        }
                        return item.Product.Volume.Value * item.Quantity;
                    });
                    int value6 = freightAreaContentInfo.FirstUnit.Value;
                    decimal value7 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int num7 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? accumulationUnitMoney1 = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num6, value6, value7, num7, (decimal)accumulationUnitMoney1.Value);
                }
            }
            return freight2;
        }

        private decimal GetFreight1(decimal count, int firstUnit, decimal firstUnitMonry, int accumulationUnit, decimal accumulationUnitMoney)
        {
            decimal num = new decimal(0);
            if (count > firstUnit)
            {
                decimal num1 = (count - firstUnit) / accumulationUnit;
                num = firstUnitMonry + (num1 * accumulationUnitMoney);
            }
            else
            {
                num = firstUnitMonry;
            }
            return num;
        }

        private decimal GetFreight2(decimal count, int firstUnit, decimal firstUnitMonry, int accumulationUnit, decimal accumulationUnitMoney)
        {
            decimal num = new decimal(0);
            if (count > firstUnit)
            {
                decimal num1 = (count - firstUnit) / accumulationUnit;
                decimal num2 = Math.Truncate(num1);
                decimal num3 = num1 - num2;
                decimal num4 = num2 * accumulationUnitMoney;
                decimal num5 = new decimal(0);
                if (num3 > new decimal(0))
                {
                    num5 = new decimal(1) * accumulationUnitMoney;
                }
                num = (firstUnitMonry + num4) + num5;
            }
            else
            {
                num = firstUnitMonry;
            }
            return num;
        }

        public List<decimal> GetFreights(IEnumerable<string> skuIds, IEnumerable<int> counts, int cityId)
        {
            IEnumerable<int> array = counts;
            CheckWhenGetFreight(skuIds, array, cityId);
            List<decimal> nums1 = new List<decimal>();
            decimal freight2 = new decimal(0);
            skuIds = skuIds.ToArray();
            array = array.ToArray();
            IProductService create = Instance<IProductService>.Create;
            IFreightTemplateService freightTemplateService = Instance<IFreightTemplateService>.Create;
            int num2 = 0;
            var list = skuIds.Select((string skuId) =>
            {
                SKUInfo sku = create.GetSku(skuId);
                ProductInfo product = create.GetProduct(sku.ProductId);
                IEnumerable<int> nums = array;
                int num = num2;
                int num1 = num;
                num2 = num + 1;
                return new { SKU = sku, Product = product, Quantity = nums.ElementAt<int>(num1) };
            }).ToList();
            var freightTemplateId =
                from item in list
                group item by item.Product.FreightTemplateId;
            foreach (var nums2 in freightTemplateId)
            {
                FreightTemplateInfo freightTemplate = freightTemplateService.GetFreightTemplate(nums2.Key);
                if (freightTemplate != null && freightTemplate.IsFree == FreightTemplateInfo.FreightTemplateType.SelfDefine)
                {
                    FreightAreaContentInfo freightAreaContentInfo = (
                        from item in freightTemplate.ChemCloud_FreightAreaContent
                        where item.AreaContent.Split(new char[] { ',' }).Contains<string>(cityId.ToString())
                        select item).FirstOrDefault() ?? freightTemplate.ChemCloud_FreightAreaContent.Where((FreightAreaContentInfo item) =>
                        {
                            byte? isDefault = item.IsDefault;
                            if (isDefault.GetValueOrDefault() != 1)
                            {
                                return false;
                            }
                            return isDefault.HasValue;
                        }).FirstOrDefault();
                    if (freightTemplate.ValuationMethod == FreightTemplateInfo.ValuationMethodType.Weight)
                    {
                        decimal num3 = nums2.Sum((item) =>
                        {
                            if (!item.Product.Weight.HasValue)
                            {
                                return new decimal(0);
                            }
                            return item.Product.Weight.Value * item.Quantity;
                        });
                        int value = freightAreaContentInfo.FirstUnit.Value;
                        decimal value1 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                        int value2 = freightAreaContentInfo.AccumulationUnit.Value;
                        float? accumulationUnitMoney = freightAreaContentInfo.AccumulationUnitMoney;
                        freight2 = GetFreight2(num3, value, value1, value2, (decimal)accumulationUnitMoney.Value);
                    }
                    else if (freightTemplate.ValuationMethod != FreightTemplateInfo.ValuationMethodType.Bulk)
                    {
                        int num4 = nums2.Sum((item) => item.Quantity);
                        decimal num5 = num4;
                        int value3 = freightAreaContentInfo.FirstUnit.Value;
                        decimal value4 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                        int value5 = freightAreaContentInfo.AccumulationUnit.Value;
                        float? nullable = freightAreaContentInfo.AccumulationUnitMoney;
                        freight2 = GetFreight2(num5, value3, value4, value5, (decimal)nullable.Value);
                    }
                    else
                    {
                        decimal num6 = nums2.Sum((item) =>
                        {
                            if (!item.Product.Volume.HasValue)
                            {
                                return new decimal(0);
                            }
                            return item.Product.Volume.Value * item.Quantity;
                        });
                        int value6 = freightAreaContentInfo.FirstUnit.Value;
                        decimal value7 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                        int num7 = freightAreaContentInfo.AccumulationUnit.Value;
                        float? accumulationUnitMoney1 = freightAreaContentInfo.AccumulationUnitMoney;
                        freight2 = GetFreight2(num6, value6, value7, num7, (decimal)accumulationUnitMoney1.Value);
                    }
                }
                nums1.Add(freight2);
            }
            return nums1;
        }

        public IQueryable<ProductInfo> GetHotConcernedProduct(long shopId, int count = 5)
        {
            string str = CacheKeyCollection.HotConcernedProduct(shopId);
            if (Cache.Get(str) != null)
            {
                return
                    from p in ((List<ProductInfo>)Cache.Get(str)).AsQueryable<ProductInfo>()
                    orderby p.ConcernedCount descending
                    select p;
            }
            var collection = (
                from p in context.FavoriteInfo
                where p.ProductInfo.ShopId == shopId && (int)p.ProductInfo.SaleStatus == 1 && (int)p.ProductInfo.AuditStatus == 2
                group p by new { ProductInfo = p.ProductInfo } into g
                select new { Product = g.Key.ProductInfo, Count = g.Count() } into c
                orderby c.Count descending
                select c).Take(count);
            foreach (var variable in collection)
            {
                variable.Product.ConcernedCount = variable.Count;
            }
            IQueryable<ProductInfo> productInfos = (
                from c in collection
                select c.Product).AsQueryable<ProductInfo>();
            List<ProductInfo> list = productInfos.ToList();
            DateTime now = DateTime.Now;
            Cache.Insert(str, list, now.AddMinutes(5));
            return productInfos;
        }

        public IQueryable<ProductInfo> GetHotSaleProduct(long shopId, int count = 5)
        {
            string str = CacheKeyCollection.HotSaleProduct(shopId);
            if (Cache.Get(str) != null)
            {
                return
                    from h in ((List<ProductInfo>)Cache.Get(str)).AsQueryable<ProductInfo>()
                    orderby h.SaleCounts descending
                    select h;
            }
            IQueryable<ProductInfo> productInfos =
                from p in context.ProductInfo.Include("ChemCloud_ProductVistis")
                where p.ShopId.Equals(shopId) && (int)p.SaleStatus == 1 && (int)p.AuditStatus == 2
                select p;
            List<ProductInfo> list = (
                from s in productInfos
                orderby s.SaleCounts descending
                select s).Take(count).ToList();
            DateTime now = DateTime.Now;
            Cache.Insert(str, list, now.AddMinutes(5));
            return list.AsQueryable<ProductInfo>();
        }

        public IQueryable<ProductInfo> GetNewSaleProduct(long shopId, int count = 5)
        {
            string str = CacheKeyCollection.NewSaleProduct(shopId);
            if (Cache.Get(str) != null)
            {
                return ((List<ProductInfo>)Cache.Get(str)).AsQueryable<ProductInfo>();
            }
            IQueryable<ProductInfo> productInfos = (
                from p in context.ProductInfo
                where p.ShopId.Equals(shopId) && (int)p.SaleStatus == 1 && (int)p.AuditStatus == 2
                orderby p.AddedDate descending
                select p).Take(count);
            if (productInfos.Count() == 0)
            {
                return null;
            }
            List<ProductInfo> list = productInfos.ToList();
            DateTime now = DateTime.Now;
            Cache.Insert(str, list, now.AddMinutes(5));
            return productInfos;
        }

        public long GetNextProductId()
        {
            if (context.ProductInfo.Count() == 0)
            {
                return 1;
            }
            return context.ProductInfo.Max<ProductInfo, long>((ProductInfo p) => p.Id) + 1;
        }

        public IQueryable<ProductInfo> GetPlatHotSaleProduct(int count = 3)
        {
            return (
                from a in context.ProductInfo
                where (int)a.SaleStatus == 1 && (int)a.AuditStatus == 2
                select a into p
                orderby p.SaleCounts descending
                select p).Take(count);
        }

        public IQueryable<ProductInfo> GetPlatHotSaleProductByNearShop(int count, long userId)
        {
            OrderInfo orderInfo = (
                from c in context.OrderInfo
                where c.UserId == userId
                orderby c.Id descending
                select c).FirstOrDefault();
            if (orderInfo == null)
            {
                return
                    from c in context.ProductInfo
                    where false
                    select c;
            }
            else
            {
                return (
                    from c in context.ProductInfo
                    where c.ShopId == orderInfo.ShopId && (int)c.SaleStatus != 4
                    orderby c.SaleCounts descending
                    select c).Take(count);
            }
        }

        public ProductInfo GetProduct(long id)
        {
            ProductInfo productInfo = (
                from P in context.ProductInfo.Include("SKUInfo")
                where P.Id == id
                select P).FirstOrDefault();

            return productInfo;
        }

        public ProductInfo GetProductByPlatCode(string Code)
        {
            ProductInfo productInfo = (
                from P in context.ProductInfo
                where P.Plat_Code.Equals(Code)
                select P).FirstOrDefault();
            return productInfo;
        }
        public ProductInfo GetProductByCondition(long shopid, string productcode)
        {
            ProductInfo productInfo = (
                from P in context.ProductInfo.FindAll()
                where P.ProductCode == productcode && P.ShopId == shopid
                select P).FirstOrDefault();
            return productInfo;
        }
        public bool IsExitsProductCode(long shopid, string productcode)
        {
            ProductInfo productInfo = (
                from P in context.ProductInfo.FindAll()
                where P.ProductCode == productcode && P.ShopId == shopid
                select P).FirstOrDefault();
            if (productInfo != null)
                return true;
            else
                return false;
        }
        public ProductInfo GetProductSpec(long id)
        {
            ProductInfo productInfo = (
                from P in context.ProductInfo.Include("SKUInfo")
                where P.Id == id
                select P).FirstOrDefault();
            List<ProductSpec> _ProductSpec = (from q in context.ProductSpec
                                              where q.ProductId == id
                                              select q).ToList();

            var Dictionaries = from item in base.context.ChemCloud_Dictionaries
                               where item.DictionaryTypeId.Equals(1)
                               select item;

            foreach (ProductSpec mode in _ProductSpec)
            {
                ChemCloud_Dictionaries _ChemCloud_Dictionaries =
                    Dictionaries.Where(q => q.DictionaryTypeId.Equals(1) && q.DValue == mode.CoinType.ToString()).ToList().FirstOrDefault();
                string cointypename = _ChemCloud_Dictionaries == null ? "" : _ChemCloud_Dictionaries.DKey;
                mode.CoinTypeName = cointypename;

                context.ChemCloud_Dictionaries.Where(q => q.DictionaryTypeId == 1).FirstOrDefault();
            }

            productInfo._ProductSpec = _ProductSpec;
            return productInfo;
        }

        public IQueryable<ProductAttributeInfo> GetProductAttribute(long productId)
        {
            return
                from p in context.ProductAttributeInfo.Include("AttributesInfo").Include("AttributesInfo.AttributeValueInfo").Include("AttributesInfo.ProductAttributeInfo")
                where p.ProductId == productId && p.ValueId != 0
                select p;
        }

        public IQueryable<ProductInfo> GetProductByIds(IEnumerable<long> ids)
        {
            return
                from item in context.ProductInfo
                where ids.Contains(item.Id) && (int)item.SaleStatus != 4
                select item;
        }

        public PageModel<ProductInfo> GetProducts(ProductQuery productQueryModel)
        {
            int num;
            IQueryable<ProductInfo> productsByQueryModel = GetProductsByQueryModel(productQueryModel, out num);
            return new PageModel<ProductInfo>()
            {
                Models = productsByQueryModel,
                Total = num
            };
        }
        public PageModel<ProductInfo> GetPlatProducts(PlatProductQuery productQueryModel)
        {
            int num;
            IQueryable<ProductInfo> productsByQueryModel = GetPlatProductsByQueryModel(productQueryModel, out num);
            return new PageModel<ProductInfo>()
            {
                Models = productsByQueryModel,
                Total = num
            };
        }
        public PageModel<ProductInfo> GetSellerProducts(SellerProductQuery productQueryModel)
        {
            int num;
            IQueryable<ProductInfo> productsByQueryModel = GetSProductsByQueryModel(productQueryModel, out num);
            return new PageModel<ProductInfo>()
            {
                Models = productsByQueryModel,
                Total = num
            };
        }
        private IQueryable<ProductInfo> GetSProductsByQueryModel(SellerProductQuery productQueryModel, out int total)
        {
            cointype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"] == null ? "1" : System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());

            IQueryable<ProductInfo> source = from item in base.context.ProductInfo
                                             where true
                                             where ((int)item.SaleStatus) != 4
                                             select item;
            /*查询特价商品*/
            if (!string.IsNullOrWhiteSpace(productQueryModel.Volume))
            {
                decimal Volume = decimal.Parse(productQueryModel.Volume);
                source = from item in source
                         where item.Volume == Volume
                         select item;
            }
            else
            {
                source = from item in source
                         where item.Volume != 3
                         select item;
            }

            if ((productQueryModel.Ids != null) && (productQueryModel.Ids.Count() > 0))
            {
                source = from item in source
                         where productQueryModel.Ids.Contains(item.Id)
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.ProductCode))
            {
                source = from item in source
                         where item.ProductCode == productQueryModel.ProductCode
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.CASNo))
            {
                source = from item in source
                         where item.CASNo == productQueryModel.CASNo
                         select item;
            }

            if (!string.IsNullOrWhiteSpace(productQueryModel.ProductName))
            {
                source = from item in source
                         where item.ProductName.Contains(productQueryModel.ProductName) || item.EProductName.Contains(productQueryModel.ProductName)
                         orderby item.MarketPrice descending
                         select item;
            }

            if (productQueryModel.ShopId.HasValue)
            {
                source = from item in source
                         where item.ShopId == productQueryModel.ShopId
                         select item;
            }
            if (productQueryModel.AuditStatus != null)
            {
                source = from item in source
                         where productQueryModel.AuditStatus.Contains<ProductInfo.ProductAuditStatus>(item.AuditStatus)
                         select item;
            }

            if (productQueryModel.SaleStatus != null)
            {
                if (((int?)productQueryModel.SaleStatus == 1) || ((int?)productQueryModel.SaleStatus == 2))
                {
                    source = from item in source
                             where ((int?)item.SaleStatus) == ((int?)productQueryModel.SaleStatus)
                             select item;
                }
                if ((int?)productQueryModel.SaleStatus == 5)
                {
                    source = from item in source
                             select item;
                }
            }
            if (productQueryModel.CategoryId.HasValue)
            {
                source = from item in source
                         where ("|" + item.CategoryPath + "|").Contains("|" + productQueryModel.CategoryId.Value + "|")
                         select item;
            }
            if (productQueryModel.NotIncludedInDraft)
            {
                source = from item in source
                         where ((int)item.SaleStatus) != 3
                         select item;
            }
            if (productQueryModel.productAuthor.HasValue)
            {
                if (productQueryModel.productAuthor == 3)
                {
                    source = from item in source
                             select item;
                }
                else
                {
                    source = from item in source
                             join c in context.ProductAuthentication on item.Id.ToString() equals c.ProductId.ToString()
                             where c.AuthStatus == productQueryModel.productAuthor
                             select item;
                }
            }

            if (!string.IsNullOrWhiteSpace(productQueryModel.KeyWords))
            {
                source = from item in source
                         where item.ProductName.Contains(productQueryModel.KeyWords)
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.engname))
            {
                source = from item in source
                         where item.EProductName.Contains(productQueryModel.engname)
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.CompanyName))
            {
                IQueryable<long> shopIds = from item in base.context.ShopInfo.FindBy(item => item.CompanyName.Contains(productQueryModel.CompanyName)) select item.Id;
                source = from item in source
                         where shopIds.Contains(item.ShopId)
                         select item;
            }
            if (productQueryModel.IsLimitTimeBuy)
            {
                IQueryable<long> limits = from l in base.context.LimitTimeMarketInfo
                                          where ((int)l.AuditStatus) == 2
                                          select l.ProductId;
                source = from p in source
                         where !limits.Contains(p.Id)
                         select p;
            }
            long shopCateogryId = productQueryModel.ShopCategoryId.GetValueOrDefault();
            if (!string.IsNullOrWhiteSpace(productQueryModel.BrandNameKeyword))
            {
                IQueryable<long> productIds = from item in
                                                  (((from product in source
                                                     join brand in base.context.BrandInfo on product.BrandId equals brand.Id
                                                     select new { Id = product.Id, Name = brand.Name, ShopCategories = from item in product.ChemCloud_ProductShopCategories select item.ShopCategoryInfo })).FindBy(item => item.Name.Contains(productQueryModel.BrandNameKeyword) && (((shopCateogryId == 0L) || (from t in item.ShopCategories select t.ParentCategoryId).Contains(shopCateogryId)) || (from t in item.ShopCategories select t.Id).Contains(shopCateogryId)), productQueryModel.PageNo, productQueryModel.PageSize, out total, item => item.Id, false))
                                              select item.Id;
                source = from item in base.context.ProductInfo
                         where productIds.Contains(item.Id)
                         select item;
            }
            else
            {
                IEnumerable<long> productIds = new long[0];
                if (productQueryModel.ShopCategoryId.HasValue)
                {
                    productIds = from item in base.context.ProductShopCategoryInfo
                                 where (item.ShopCategoryInfo.Id == shopCateogryId) || (item.ShopCategoryInfo.ParentCategoryId == shopCateogryId)
                                 select item.ProductId;
                }
                total = source.Count();
                source = source.FindBy<ProductInfo, long>(item => (shopCateogryId == 0L) || productIds.Contains(item.Id), productQueryModel.PageNo, productQueryModel.PageSize, out total, item => item.Id, false);
            }

            foreach (ProductInfo info in source.ToList())
            {
                info.ShopCateogryInfos = from item in info.ChemCloud_ProductShopCategories select item.ShopCategoryInfo;
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(info.ShopId));
                info.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
                info.ShopName = (shopInfo == null ? "" : shopInfo.ShopName);
                ProductAuthentication productAuthentication = context.ProductAuthentication.FirstOrDefault((ProductAuthentication m) => m.ProductId.Equals(info.Id));
                if (productAuthentication != null)
                {
                    info.AuthStatus = productAuthentication.AuthStatus;
                }

                /*注意当前语言版本对应的货币种类*/
                ProductSpec ProductSpecinfo = context.ProductSpec.FirstOrDefault((ProductSpec p) => p.ProductId.Equals(info.Id) && p.CoinType.Equals(cointype));
                info.Packaging = (ProductSpecinfo == null ? "" : ProductSpecinfo.Packaging);
                info.Purity = (ProductSpecinfo == null ? "" : ProductSpecinfo.Purity);
                info.SpecLevel = (ProductSpecinfo == null ? "" : ProductSpecinfo.SpecLevel);
                info.Price = (ProductSpecinfo == null ? 0 : ProductSpecinfo.Price);
            }
            return source;
        }
        private IQueryable<ProductInfo> GetProductsByQueryModel(ProductQuery productQueryModel, out int total)
        {
            cointype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"] == null ? "1" : System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());

            IQueryable<ProductInfo> source = from item in base.context.ProductInfo
                                             join itemshop in base.context.ShopInfo
                                             on item.ShopId equals itemshop.Id
                                             where true
                                             where ((int)item.SaleStatus) == 1 && ((int)item.AuditStatus) == 2 && ((int)itemshop.ShopStatus) == 7
                                             orderby itemshop.SortType descending
                                             select item;

            if ((productQueryModel.Ids != null) && (productQueryModel.Ids.Count() > 0))
            {
                source = from item in source
                         where productQueryModel.Ids.Contains(item.Id)
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.ProductCode))
            {
                source = from item in source
                         where item.ProductCode == productQueryModel.ProductCode
                         select item;
            }
            if (productQueryModel.CASNo != 0)
            {
                source = from item in source
                         where item.Pub_CID == productQueryModel.CASNo
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.CompanyName))
            {
                ShopInfo shopinfo = (
                  from P in context.ShopInfo
                  where P.CompanyName == productQueryModel.CompanyName
                  select P).FirstOrDefault();
                source = from item in source
                         where item.ShopId == shopinfo.Id
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.ProductName))
            {
                source = from item in source
                         where item.ProductName.Contains(productQueryModel.ProductName) || item.EProductName.Contains(productQueryModel.ProductName)
                         orderby item.MarketPrice descending
                         select item;
            }

            if (productQueryModel.ShopId.HasValue)
            {
                source = from item in source
                         where item.ShopId == productQueryModel.ShopId
                         select item;
            }
            if (productQueryModel.AuditStatus != null)
            {
                source = from item in source
                         where (int)item.AuditStatus == (int)productQueryModel.AuditStatus.Value//.Contains<ProductInfo.ProductAuditStatus>(item.AuditStatus)
                         select item;
            }

            if (productQueryModel.SaleStatus.HasValue)
            {
                source = from item in source
                         where ((int?)item.SaleStatus) == ((int?)productQueryModel.SaleStatus)
                         select item;
            }
            if (productQueryModel.CategoryId.HasValue)
            {
                source = from item in source
                         where ("|" + item.CategoryPath + "|").Contains("|" + productQueryModel.CategoryId.Value + "|")
                         select item;
            }
            if (productQueryModel.NotIncludedInDraft)
            {
                source = from item in source
                         where ((int)item.SaleStatus) != 3
                         select item;
            }
            if (productQueryModel.StartDate.HasValue)
            {
                source = from item in source
                         where item.AddedDate > productQueryModel.StartDate
                         select item;
            }
            if (productQueryModel.EndDate.HasValue)
            {
                source = from item in source
                         where item.AddedDate <= productQueryModel.EndDate
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.KeyWords))
            {
                source = from item in source
                         where item.ProductName.Contains(productQueryModel.KeyWords)
                         select item;
            }
            if (productQueryModel.ShopType != null)
            {
                if (productQueryModel.ShopType != "0")
                {
                    source = from item in source
                             join itemshop in base.context.ShopInfo
                                 on item.ShopId equals itemshop.Id
                             where true
                             where itemshop.SortType.ToString() == productQueryModel.ShopType
                             select item;
                }
            }

            if (!string.IsNullOrWhiteSpace(productQueryModel.CompanyName))
            {
                IQueryable<long> shopIds = from item in base.context.ShopInfo.FindBy(item => item.CompanyName.Contains(productQueryModel.CompanyName)) select item.Id;
                source = from item in source
                         where shopIds.Contains(item.ShopId)
                         select item;
            }
            if (productQueryModel.IsLimitTimeBuy)
            {
                IQueryable<long> limits = from l in base.context.LimitTimeMarketInfo
                                          where ((int)l.AuditStatus) == 2
                                          select l.ProductId;
                source = from p in source
                         where !limits.Contains(p.Id)
                         select p;
            }
            long shopCateogryId = productQueryModel.ShopCategoryId.GetValueOrDefault();
            if (!string.IsNullOrWhiteSpace(productQueryModel.BrandNameKeyword))
            {
                IQueryable<long> productIds = from item in
                                                  (((from product in source
                                                     join brand in base.context.BrandInfo on product.BrandId equals brand.Id
                                                     select new { Id = product.Id, Name = brand.Name, ShopCategories = from item in product.ChemCloud_ProductShopCategories select item.ShopCategoryInfo })).FindBy(item => item.Name.Contains(productQueryModel.BrandNameKeyword) && (((shopCateogryId == 0L) || (from t in item.ShopCategories select t.ParentCategoryId).Contains(shopCateogryId)) || (from t in item.ShopCategories select t.Id).Contains(shopCateogryId)), productQueryModel.PageNo, productQueryModel.PageSize, out total, item => item.Id, false))
                                              select item.Id;
                source = from item in base.context.ProductInfo
                         where productIds.Contains(item.Id)
                         select item;
            }
            else
            {
                IEnumerable<long> productIds = new long[0];
                if (productQueryModel.ShopCategoryId.HasValue)
                {
                    productIds = from item in base.context.ProductShopCategoryInfo
                                 where (item.ShopCategoryInfo.Id == shopCateogryId) || (item.ShopCategoryInfo.ParentCategoryId == shopCateogryId)
                                 select item.ProductId;
                }
                total = source.Count();
                source = source.FindBy<ProductInfo, long>(item => (shopCateogryId == 0L) || productIds.Contains(item.Id), productQueryModel.PageNo, productQueryModel.PageSize, out total, item => item.Id, false);
            }

            foreach (ProductInfo info in source.ToList())
            {
                info.ShopCateogryInfos = from item in info.ChemCloud_ProductShopCategories select item.ShopCategoryInfo;
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(info.ShopId));
                info.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
                info.ShopName = (shopInfo == null ? "" : shopInfo.ShopName);
                info.ShopId = info.ShopId;
                /*注意当前语言版本对应的货币种类*/
                ProductSpec ProductSpecinfo = context.ProductSpec.FirstOrDefault((ProductSpec p) => p.ProductId.Equals(info.Id) && p.CoinType.Equals(cointype));
                info.Packaging = (ProductSpecinfo == null ? "" : ProductSpecinfo.Packaging);
                info.Purity = (ProductSpecinfo == null ? "" : ProductSpecinfo.Purity);
                info.SpecLevel = (ProductSpecinfo == null ? "" : ProductSpecinfo.SpecLevel);
                info.Price = (ProductSpecinfo == null ? 0 : ProductSpecinfo.Price);
            }



            return source;
        }

        private IQueryable<ProductInfo> GetPlatProductsByQueryModel(PlatProductQuery productQueryModel, out int total)
        {
            cointype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"] == null ? "1" : System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());

            IQueryable<ProductInfo> source = from item in base.context.ProductInfo
                                             join itemshop in base.context.ShopInfo
                                             on item.ShopId equals itemshop.Id
                                             where true
                                             where ((int)item.SaleStatus) == 1 && ((int)item.AuditStatus) == 2 && ((int)itemshop.ShopStatus) == 7
                                             orderby itemshop.SortType descending
                                             select item;

            if ((productQueryModel.Ids != null) && (productQueryModel.Ids.Count() > 0))
            {
                source = from item in source
                         where productQueryModel.Ids.Contains(item.Id)
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.ProductCode))
            {
                source = from item in source
                         where item.ProductCode.Contains(productQueryModel.ProductCode)
                         select item;
            }
            if (!string.IsNullOrEmpty(productQueryModel.CASNo))
            {
                source = from item in source
                         where item.CASNo.Contains(productQueryModel.CASNo)
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.CompanyName))
            {
                ShopInfo shopinfo = (
                  from P in context.ShopInfo
                  where P.CompanyName.Contains(productQueryModel.CompanyName)
                  select P).FirstOrDefault();
                source = from item in source
                         where item.ShopId == shopinfo.Id
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.ProductName))
            {
                source = from item in source
                         where item.ProductName.Contains(productQueryModel.ProductName)
                         //where item.ProductName.Contains(productQueryModel.ProductName) || item.EProductName.Contains(productQueryModel.ProductName)
                         orderby item.MarketPrice descending
                         select item;
            }

            if (productQueryModel.ShopId.HasValue)
            {
                source = from item in source
                         where item.ShopId == productQueryModel.ShopId
                         select item;
            }
            if (productQueryModel.AuditStatus != null)
            {
                source = from item in source
                         where (int)item.AuditStatus == (int)productQueryModel.AuditStatus.Value//.Contains<ProductInfo.ProductAuditStatus>(item.AuditStatus)
                         select item;
            }

            if (productQueryModel.SaleStatus.HasValue)
            {
                source = from item in source
                         where ((int?)item.SaleStatus) == ((int?)productQueryModel.SaleStatus)
                         select item;
            }
            if (productQueryModel.CategoryId.HasValue)
            {
                source = from item in source
                         where ("|" + item.CategoryPath + "|").Contains("|" + productQueryModel.CategoryId.Value + "|")
                         select item;
            }
            if (productQueryModel.NotIncludedInDraft)
            {
                source = from item in source
                         where ((int)item.SaleStatus) != 3
                         select item;
            }
            if (productQueryModel.StartDate.HasValue)
            {
                source = from item in source
                         where item.AddedDate > productQueryModel.StartDate
                         select item;
            }
            if (productQueryModel.EndDate.HasValue)
            {
                source = from item in source
                         where item.AddedDate <= productQueryModel.EndDate
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.KeyWords))
            {
                source = from item in source
                         where item.ProductName.Contains(productQueryModel.KeyWords)
                         select item;
            }
            if (productQueryModel.ShopType != null)
            {
                if (productQueryModel.ShopType != "0")
                {
                    source = from item in source
                             join itemshop in base.context.ShopInfo
                                 on item.ShopId equals itemshop.Id
                             where true
                             where itemshop.SortType.ToString() == productQueryModel.ShopType
                             select item;
                }
            }


            if (!string.IsNullOrWhiteSpace(productQueryModel.CompanyName))
            {
                IQueryable<long> shopIds = from item in base.context.ShopInfo.FindBy(item => item.CompanyName.Contains(productQueryModel.CompanyName)) select item.Id;
                source = from item in source
                         where shopIds.Contains(item.ShopId)
                         select item;
            }
            if (productQueryModel.IsLimitTimeBuy)
            {
                IQueryable<long> limits = from l in base.context.LimitTimeMarketInfo
                                          where ((int)l.AuditStatus) == 2
                                          select l.ProductId;
                source = from p in source
                         where !limits.Contains(p.Id)
                         select p;
            }
            long shopCateogryId = productQueryModel.ShopCategoryId.GetValueOrDefault();
            if (!string.IsNullOrWhiteSpace(productQueryModel.BrandNameKeyword))
            {
                IQueryable<long> productIds = from item in
                                                  (((from product in source
                                                     join brand in base.context.BrandInfo on product.BrandId equals brand.Id
                                                     select new { Id = product.Id, Name = brand.Name, ShopCategories = from item in product.ChemCloud_ProductShopCategories select item.ShopCategoryInfo })).FindBy(item => item.Name.Contains(productQueryModel.BrandNameKeyword) && (((shopCateogryId == 0L) || (from t in item.ShopCategories select t.ParentCategoryId).Contains(shopCateogryId)) || (from t in item.ShopCategories select t.Id).Contains(shopCateogryId)), productQueryModel.PageNo, productQueryModel.PageSize, out total, item => item.Id, false))
                                              select item.Id;
                source = from item in base.context.ProductInfo
                         where productIds.Contains(item.Id)
                         select item;
            }
            else
            {
                IEnumerable<long> productIds = new long[0];
                if (productQueryModel.ShopCategoryId.HasValue)
                {
                    productIds = from item in base.context.ProductShopCategoryInfo
                                 where (item.ShopCategoryInfo.Id == shopCateogryId) || (item.ShopCategoryInfo.ParentCategoryId == shopCateogryId)
                                 select item.ProductId;
                }
                total = source.Count();
                source = source.FindBy<ProductInfo, long>(item => (shopCateogryId == 0L) || productIds.Contains(item.Id), productQueryModel.PageNo, productQueryModel.PageSize, out total, item => item.Id, false);
            }

            foreach (ProductInfo info in source.ToList())
            {
                info.ShopCateogryInfos = from item in info.ChemCloud_ProductShopCategories select item.ShopCategoryInfo;
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(info.ShopId));
                info.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
                info.ShopName = (shopInfo == null ? "" : shopInfo.ShopName);
                info.ShopId = info.ShopId;

                /*注意当前语言版本对应的货币种类*/
                ProductSpec ProductSpecinfo = context.ProductSpec.FirstOrDefault((ProductSpec p) => p.ProductId.Equals(info.Id) && p.CoinType.Equals(cointype));
                info.Packaging = (ProductSpecinfo == null ? "" : ProductSpecinfo.Packaging);
                info.Purity = (ProductSpecinfo == null ? "" : ProductSpecinfo.Purity);
                info.SpecLevel = (ProductSpecinfo == null ? "" : ProductSpecinfo.SpecLevel);
                info.Price = (ProductSpecinfo == null ? 0 : ProductSpecinfo.Price);
            }



            return source;
        }
        public IQueryable<ProductShopCategoryInfo> GetProductShopCategories(long productId)
        {
            return context.ProductShopCategoryInfo.FindBy((ProductShopCategoryInfo p) => p.ProductId == productId);
        }

        public ProductVistiInfo GetProductVistInfo(long pId, ICollection<ProductVistiInfo> pInfo = null)
        {
            ProductVistiInfo productVistiInfo = new ProductVistiInfo();
            if (pInfo != null)
            {
                var variable = (
                    from pv in pInfo
                    group pv by pv.ProductId into G
                    select new { SaleCount = G.Sum<ProductVistiInfo>((ProductVistiInfo v1) => v1.SaleCounts), SaleAmounts = G.Sum<ProductVistiInfo>((ProductVistiInfo v1) => v1.SaleAmounts), ProductId = G.Key }).FirstOrDefault();
                if (variable != null && variable.ProductId == pId)
                {
                    productVistiInfo.ProductId = pId;
                    productVistiInfo.SaleAmounts = variable.SaleAmounts;
                    productVistiInfo.SaleCounts = variable.SaleCount;
                    productVistiInfo.Date = DateTime.Now;
                }
            }
            else
            {
                ProductVistiInfo productVistiInfo1 = context.ProductVistiInfo.FirstOrDefault((ProductVistiInfo v) => v.ProductId == pId);
                if (productVistiInfo1 != null)
                {
                    productVistiInfo.ProductId = pId;
                    productVistiInfo.SaleAmounts = productVistiInfo1.SaleAmounts;
                    productVistiInfo.SaleCounts = productVistiInfo1.SaleCounts;
                    productVistiInfo.Date = DateTime.Now;
                }
            }
            return productVistiInfo;
        }

        public IQueryable<SellerSpecificationValueInfo> GetSellerSpecifications(long shopId, long typeId)
        {
            return context.SellerSpecificationValueInfo.FindBy((SellerSpecificationValueInfo p) => p.ShopId == shopId && p.TypeId == typeId);
        }

        public int GetShopAllProducts(long shopId)
        {
            int num = (
                from item in context.ProductInfo
                where item.ShopId == shopId && (int)item.SaleStatus != 4
                select item).Count();
            return num;
        }

        public int GetShopOnsaleProducts(long shopId)
        {
            return (
                from item in context.ProductInfo
                where item.ShopId == shopId && (int)item.SaleStatus == 1 && (int)item.AuditStatus == 2
                select item).Count();
        }

        public SKUInfo GetSku(string skuId)
        {
            return context.SKUInfo.FindBy((SKUInfo s) => s.Id == skuId).FirstOrDefault();
        }

        public IQueryable<SKUInfo> GetSKUs(long productId)
        {
            return context.SKUInfo.FindBy((SKUInfo s) => s.ProductId == productId);
        }

        public IQueryable<SKUInfo> GetSKUs(IEnumerable<long> productIds)
        {
            return context.SKUInfo.FindBy((SKUInfo s) => productIds.Contains(s.ProductId));
        }

        public List<FavoriteInfo> GetUserAllConcern(long userId)
        {
            List<FavoriteInfo> list = (
                from p in context.FavoriteInfo
                where p.UserId == userId
                select p).ToList();
            if (list == null)
            {
                return new List<FavoriteInfo>();
            }
            return list;
        }

        public PageModel<FavoriteInfo> GetUserConcernProducts(long userId, int pageNo, int pageSize)
        {
            int num = 0;
            IQueryable<FavoriteInfo> favoriteInfos = context.FavoriteInfo.FindBy<FavoriteInfo, DateTime>((FavoriteInfo a) => a.UserId == userId, pageNo, pageSize, out num, (FavoriteInfo a) => a.Date, false);
            return new PageModel<FavoriteInfo>()
            {
                Models = favoriteInfos,
                Total = num
            };
        }

        private string HTMLProcess(string content, string path)
        {
            string str = Path.Combine(path, "Details").Replace("\\", "/");
            string mapPath = IOHelper.GetMapPath(str);
            string str1 = Path.Combine(path, "Temp").Replace("\\", "/");
            string mapPath1 = IOHelper.GetMapPath(str1);
            try
            {
                string str2 = str;
                string mapPath2 = IOHelper.GetMapPath(str2);
                content = HtmlContentHelper.TransferToLocalImage(content, IOHelper.GetMapPath("/"), mapPath2, string.Concat(str2, "/"));
                content = HtmlContentHelper.RemoveScriptsAndStyles(content);
                if (Directory.Exists(mapPath1))
                {
                    Directory.Delete(mapPath1, true);
                }
            }
            catch
            {
                if (Directory.Exists(mapPath1))
                {
                    string[] files = Directory.GetFiles(mapPath1);
                    for (int i = 0; i < files.Length; i++)
                    {
                        string str3 = files[i];
                        File.Copy(str3, Path.Combine(mapPath, Path.GetFileName(str3)), true);
                    }
                    Directory.Delete(mapPath1, true);
                }
            }
            return content;
        }

        public bool IsFavorite(long productId, long userId)
        {
            bool flag = false;
            if (userId <= 0)
            {
                throw new HimallException("用户ID不存在！");
            }
            if (context.FavoriteInfo.FindBy((FavoriteInfo f) => f.ProductId.Equals(productId) && f.UserId.Equals(userId)).Count() >= 1)
            {
                flag = true;
            }
            return flag;
        }

        public void LogProductVisti(long productId)
        {
            DateTime now = DateTime.Now;
            long shopId = 0;
            ProductVistiInfo productVistiInfo = (
                from p in context.ProductVistiInfo.Include("ProductInfo")
                where p.ProductId.Equals(productId) && p.Date.Year.Equals(now.Year) && p.Date.Month.Equals(now.Month) && p.Date.Day.Equals(now.Day)
                select p).FirstOrDefault();
            if (productVistiInfo == null || !productVistiInfo.ProductId.Equals(productId))
            {
                shopId = context.ProductInfo.FirstOrDefault((ProductInfo p) => p.Id == productId).ShopId;
                DbSet<ProductVistiInfo> productVistiInfos = context.ProductVistiInfo;
                ProductVistiInfo productVistiInfo1 = new ProductVistiInfo()
                {
                    ProductId = productId,
                    Date = DateTime.Now,
                    VistiCounts = 1,
                    SaleAmounts = new decimal(0),
                    SaleCounts = 0
                };
                productVistiInfos.Add(productVistiInfo1);
            }
            else
            {
                ProductVistiInfo vistiCounts = productVistiInfo;
                vistiCounts.VistiCounts = vistiCounts.VistiCounts + 1;
                shopId = productVistiInfo.ProductInfo.ShopId;
            }
            ShopVistiInfo shopVistiInfo = context.ShopVistiInfo.FindBy((ShopVistiInfo item) => item.ShopId == shopId && item.Date.Year == now.Year && item.Date.Month == now.Month && item.Date.Day == now.Day).FirstOrDefault();
            if (shopVistiInfo == null)
            {
                shopVistiInfo = new ShopVistiInfo()
                {
                    ShopId = shopId,
                    Date = DateTime.Now.Date
                };
                context.ShopVistiInfo.Add(shopVistiInfo);
            }
            ShopVistiInfo vistiCounts1 = shopVistiInfo;
            vistiCounts1.VistiCounts = vistiCounts1.VistiCounts + 1;
            context.SaveChanges();
        }

        public void OnSale(long id, long shopId)
        {
            OnSale(new long[] { id }, shopId);
        }

        public void OnSale(IEnumerable<long> ids, long shopId)
        {
            IQueryable<ProductInfo> productInfo =
                from item in context.ProductInfo
                where ids.Contains(item.Id) && (int)item.SaleStatus != 4
                select item;
            if (productInfo.Count((ProductInfo item) => item.ShopId != shopId) > 0)
            {
                throw new HimallException("只能上架指定供应商的产品");
            }
            foreach (long list in (
                from d in productInfo
                select d.Id).ToList())
            {
                ApplyForSale(list);
            }
        }

        public void SaleOff(long id, long shopid)
        {
            ProductInfo productInfo = context.ProductInfo.FindById<ProductInfo>(id);
            if (productInfo.ShopId != shopid)
            {
                throw new HimallException("只能下架指定供应商的产品");
            }
            productInfo.SaleStatus = ProductInfo.ProductSaleStatus.InStock;
            context.SaveChanges();
        }

        public void SaleOff(IEnumerable<long> ids, long shopid)
        {
            ProductInfo[] array = (
                from item in context.ProductInfo
                where ids.Contains(item.Id) && (int)item.SaleStatus != 4
                select item).ToArray();
            if (array.Count((ProductInfo item) => item.ShopId != shopid) > 0)
            {
                throw new HimallException("只能下架指定供应商的产品");
            }
            ProductInfo[] productInfoArray = array;
            for (int i = 0; i < productInfoArray.Length; i++)
            {
                ProductInfo productInfo = productInfoArray[i];
                productInfo.SaleStatus = ProductInfo.ProductSaleStatus.InStock;
                //productInfo.AuditStatus = ProductInfo.ProductAuditStatus.WaitForAuditing;
            }
            context.SaveChanges();
        }

        public void SaveSellerSpecifications(List<SellerSpecificationValueInfo> info)
        {
            if (info != null && info.Count() > 0)
            {
                long shopId = info[0].ShopId;
                long typeId = info[0].TypeId;
                IQueryable<SellerSpecificationValueInfo> sellerSpecificationValueInfo =
                    from s in context.SellerSpecificationValueInfo
                    where s.ShopId == shopId && s.TypeId == typeId
                    select s;
                foreach (SellerSpecificationValueInfo specification in info)
                {
                    if (!sellerSpecificationValueInfo.Any((SellerSpecificationValueInfo s) => s.ValueId == specification.ValueId))
                    {
                        specification.Specification = context.SpecificationValueInfo.FindById<SpecificationValueInfo>(specification.ValueId).Specification;
                        context.SellerSpecificationValueInfo.Add(specification);
                    }
                    else
                    {
                        sellerSpecificationValueInfo.FirstOrDefault((SellerSpecificationValueInfo s) => s.ValueId == specification.ValueId).Value = specification.Value;
                    }
                }
                context.SaveChanges();
            }
        }

        public PageModel<ProductInfo> SearchProduct(ProductSearch search)
        {
            char[] chrArray;
            int i;
            var productInfo =
                from item in context.ProductInfo
                select new { Id = item.Id, ProductName = item.ProductName, AuditStatus = item.AuditStatus, SaleStatus = item.SaleStatus, ChemCloud_ProductShopCategories = item.ChemCloud_ProductShopCategories, ShopId = item.ShopId, CategoryPath = item.CategoryPath, CategoryId = item.CategoryId, BrandId = item.BrandId, MinSalePrice = item.MinSalePrice, SaleCounts = item.SaleCounts, ProductConsultationInfo = item.ProductConsultationInfo, AddedDate = item.AddedDate, ChemCloud_ProductVistis = item.ChemCloud_ProductVistis, Himall_ProductComments = item.ChemCloud_ModuleProducts };
            StringBuilder stringBuilder = new StringBuilder();
            productInfo = (search.shopId == 0 ?
                from p in productInfo
                where (int)p.AuditStatus == 2 && (int)p.SaleStatus == 1
                select p :
                from p in productInfo
                where p.ShopId.Equals(search.shopId) && (int)p.AuditStatus == 2 && (int)p.SaleStatus == 1
                select p);
            if (search.CategoryId > 0)
            {
                long categoryId = search.CategoryId;
                if (search.shopId <= 0)
                {
                    productInfo =
                        from p in productInfo
                        where (("|" + p.CategoryPath) + "|").Contains(("|" + search.CategoryId.ToString()) + "|")
                        select p into s
                        orderby s.CategoryId
                        select s;
                }
                else
                {
                    productInfo =
                        from p in productInfo
                        where p.ChemCloud_ProductShopCategories.Any((ProductShopCategoryInfo s) => s.ShopCategoryId.Equals(search.CategoryId))
                        select p;
                }
            }
            if (search.ShopCategoryId.HasValue)
            {
                long? shopCategoryId = search.ShopCategoryId;
                if ((shopCategoryId.GetValueOrDefault() <= 0 ? false : shopCategoryId.HasValue))
                {
                    IEnumerable<long> productShopCategoryInfo = new long[0];
                    productShopCategoryInfo =
                        from item in context.ProductShopCategoryInfo
                        where item.ShopCategoryInfo.Id == search.ShopCategoryId || item.ShopCategoryInfo.ParentCategoryId == search.ShopCategoryId
                        select item.ProductId;
                    productInfo =
                        from p in productInfo
                        where productShopCategoryInfo.Contains(p.Id)
                        select p;
                }
            }
            if (search.BrandId > 0)
            {
                productInfo =
                    from p in productInfo
                    where p.BrandId.Equals(search.BrandId)
                    select p;
            }
            foreach (string attrId in search.AttrIds)
            {
                long num = 0;
                chrArray = new char[] { '\u005F' };
                long.TryParse(attrId.Split(chrArray)[0], out num);
                char[] chrArray1 = new char[] { '\u005F' };
                if (attrId.Split(chrArray1).Length <= 1)
                {
                    continue;
                }
                long num1 = 0;
                char[] chrArray2 = new char[] { '\u005F' };
                long.TryParse(attrId.Split(chrArray2)[1], out num1);
                IQueryable<long> productAttributeInfo =
                    from p in context.ProductAttributeInfo
                    where p.AttributeId == num && p.ValueId == num1
                    select p.ProductId;
                productInfo =
                    from p in productInfo
                    where productAttributeInfo.Contains(p.Id)
                    select p;
            }
            if (search.CategoryId == 0 && !string.IsNullOrWhiteSpace(search.Keyword))
            {
                string str = search.Keyword.Replace("\t", " ").Replace("\u3000", " ");
                chrArray = new char[] { ' ' };
                string[] strArrays = str.Split(chrArray);
                var collection = productInfo;
                var collection1 = productInfo;
                bool flag = true;
                string[] strArrays1 = strArrays;
                for (i = 0; i < strArrays1.Length; i++)
                {
                    string str1 = strArrays1[i];
                    if (!string.IsNullOrWhiteSpace(str1))
                    {
                        collection1 = (!flag ? collection1.Concat(
                            from p in collection
                            where p.ProductName.ToUpper().Contains(str1.ToUpper())
                            select p) :
                            from p in productInfo
                            where p.ProductName.ToUpper().Contains(str1.ToUpper())
                            select p);
                        flag = false;
                    }
                }
                productInfo = collection1.AsQueryable();
            }
            if (search.startPrice >= new decimal(0) && search.EndPrice > search.startPrice && search.EndPrice != new decimal(-1, -1, -1, false, 0))
            {
                productInfo =
                    from r in productInfo
                    where r.MinSalePrice >= search.startPrice && r.MinSalePrice <= search.EndPrice
                    select r;
            }
            if (!string.IsNullOrWhiteSpace(search.Ex_Keyword))
            {
                productInfo =
                    from p in productInfo
                    where p.ProductName.Contains(search.Ex_Keyword)
                    select p;
            }
            var shopInfo =
                from p in productInfo
                join shop in context.ShopInfo on p.ShopId equals shop.Id
                where shop.EndDate.Value > DateTime.Now
                select p;
            IQueryable<long> nums = (
                from p in shopInfo
                orderby p.Id
                select p into item
                select item.Id).Distinct<long>();
            IQueryable<ProductInfo> orderCounts =
                from item in context.ProductInfo
                where nums.Contains(item.Id)
                select item;
            int num2 = orderCounts.Count();
            if (search.PageSize == 0)
            {
                search.PageSize = 10;
            }
            if ((int)Math.Ceiling(orderCounts.Count() / (double)search.PageSize) < search.PageNumber)
            {
                PageModel<ProductInfo> pageModel = new PageModel<ProductInfo>()
                {
                    Total = num2,
                    Models = (new ProductInfo[0]).AsQueryable<ProductInfo>()
                };
                return pageModel;
            }
            switch (search.OrderKey)
            {
                case 2:
                    {
                        orderCounts =
                            from p in orderCounts
                            orderby p.ChemCloud_ProductVistis.FirstOrDefault().OrderCounts descending
                            select p;
                        break;
                    }
                case 3:
                    {
                        if (search.OrderType)
                        {
                            orderCounts =
                                from p in orderCounts
                                orderby p.MinSalePrice
                                select p;
                            break;
                        }
                        else
                        {
                            orderCounts =
                                from p in orderCounts
                                orderby p.MinSalePrice descending
                                select p;
                            break;
                        }
                    }
                case 4:
                    {
                        orderCounts =
                            from p in orderCounts
                            orderby p.ChemCloud_ProductComments.Count() descending
                            select p;
                        break;
                    }
                case 5:
                    {
                        orderCounts =
                            from p in orderCounts
                            orderby p.AddedDate descending
                            select p;
                        break;
                    }
                default:
                    {
                        orderCounts =
                            from item in orderCounts
                            orderby item.Id descending
                            select item;
                        break;
                    }
            }
            orderCounts = orderCounts.Skip((search.PageNumber - 1) * search.PageSize).Take(search.PageSize);
            ProductInfo[] array = orderCounts.ToArray();
            for (i = 0; i < array.Length; i++)
            {
                ProductInfo shopName = array[i];
                long freightTemplateId = shopName.FreightTemplateId;
                shopName.ShopName = shopName.ChemCloud_Shops.ShopName;
                if (shopName.ChemCloud_ProductVistis != null)
                {
                    shopName.SaleCounts = shopName.ChemCloud_ProductVistis.Sum<ProductVistiInfo>((ProductVistiInfo s) =>
                    {
                        if (!s.OrderCounts.HasValue)
                        {
                            return 0;
                        }
                        return s.OrderCounts.Value;
                    });
                }
                else
                {
                    shopName.SaleCounts = 0;
                }
                shopName.Address = "";
                if (freightTemplateId != 0)
                {
                    int value = Instance<IFreightTemplateService>.Create.GetFreightTemplate(freightTemplateId).SourceAddress.Value;
                    shopName.Address = Instance<IRegionService>.Create.GetRegionShortName(value);
                }
            }
            return new PageModel<ProductInfo>()
            {
                Total = num2,
                Models = orderCounts
            };
        }

        private void UpdateAttr(ProductInfo model)
        {
            IQueryable<ProductAttributeInfo> productAttributeInfo =
                from s in context.ProductAttributeInfo
                where s.ProductId == model.Id
                select s;
            context.ProductAttributeInfo.RemoveRange(productAttributeInfo);
            context.ProductAttributeInfo.AddRange(model.ProductAttributeInfo);
            context.SaveChanges();
        }

        private void UpdateCategory(ProductInfo model)
        {
            IQueryable<ProductShopCategoryInfo> productShopCategoryInfo =
                from s in context.ProductShopCategoryInfo
                where s.ProductId == model.Id
                select s;
            context.ProductShopCategoryInfo.RemoveRange(productShopCategoryInfo);
            context.ProductShopCategoryInfo.AddRange(model.ChemCloud_ProductShopCategories);
            context.SaveChanges();
        }

        private void UpdateCommon(ProductInfo model)
        {
            ProductInfo brandId = context.ProductInfo.FindById<ProductInfo>(model.Id);
            if (Instance<ISiteSettingService>.Create.GetSiteSettings().ProdutAuditOnOff == 0 && brandId.AuditStatus == ProductInfo.ProductAuditStatus.InfractionSaleOff)
            {
                throw new HimallException("违规下架的产品不能执行此操作！");
            }
            brandId.BrandId = model.BrandId;
            brandId.CategoryId = model.CategoryId;
            brandId.CategoryPath = model.CategoryPath;
            brandId.MarketPrice = model.MarketPrice;
            brandId.MinSalePrice = model.MinSalePrice;
            brandId.ProductCode = model.ProductCode;
            brandId.ProductName = model.ProductName;

            brandId.EProductName = model.EProductName;
            brandId.Purity = model.Purity;
            brandId.CASNo = model.CASNo;
            brandId.HSCODE = model.HSCODE;
            brandId.DangerLevel = model.DangerLevel;
            brandId.MolecularFormula = model.MolecularFormula;
            brandId.ISCASNo = model.ISCASNo;
            brandId.RefuseReason = model.RefuseReason;
            brandId.Alias = model.Alias;
            brandId.Ealias = model.Ealias;
            brandId.MolecularWeight = model.MolecularWeight;
            brandId.PASNo = model.PASNo;
            brandId.LogP = model.LogP;
            brandId.Shape = model.Shape;
            brandId.Density = model.Density;
            brandId.FusingPoint = model.FusingPoint;
            brandId.BoilingPoint = model.BoilingPoint;
            brandId.FlashPoint = model.FlashPoint;
            brandId.RefractiveIndex = model.RefractiveIndex;
            brandId.StorageConditions = model.StorageConditions;
            brandId.VapourPressure = model.VapourPressure;
            brandId.PackagingLevel = model.PackagingLevel;
            brandId.SafetyInstructions = model.SafetyInstructions;
            brandId.DangerousMark = model.DangerousMark;
            brandId.RiskCategoryCode = model.RiskCategoryCode;
            brandId.TransportationNmber = model.TransportationNmber;
            brandId.RETCS = model.RETCS;
            brandId.WGKGermany = model.WGKGermany;
            brandId.STRUCTURE_2D = model.STRUCTURE_2D;
            brandId.ShortDescription = model.ShortDescription;
            brandId.TypeId = model.TypeId;
            brandId.FreightTemplateId = model.FreightTemplateId;
            brandId.Volume = model.Volume;
            brandId.Weight = model.Weight;
            brandId.MeasureUnit = model.MeasureUnit;
            brandId.ImagePath = model.ImagePath;
            brandId.EditStatus = model.EditStatus;
            //ProductDescriptionInfo descriptionPrefixId = context.ProductDescriptionInfo.FirstOrDefault((ProductDescriptionInfo p) => p.ProductId == model.Id);
            //descriptionPrefixId.Description = HTMLProcess(model.ProductDescriptionInfo.Description, model.ImagePath);
            //descriptionPrefixId.DescriptionPrefixId = model.ProductDescriptionInfo.DescriptionPrefixId;
            //descriptionPrefixId.DescriptiondSuffixId = model.ProductDescriptionInfo.DescriptiondSuffixId;
            //descriptionPrefixId.Meta_Description = model.ProductDescriptionInfo.Meta_Description;
            //descriptionPrefixId.Meta_Keywords = model.ProductDescriptionInfo.Meta_Keywords;
            //descriptionPrefixId.Meta_Title = model.ProductDescriptionInfo.Meta_Title;
            //descriptionPrefixId.MobileDescription = HTMLProcess(model.ProductDescriptionInfo.MobileDescription, model.ImagePath);

            context.SaveChanges();
            if (model.SaleStatus == ProductInfo.ProductSaleStatus.OnSale)
            {
                ApplyForSale(brandId.Id);
            }
        }

        public void UpdateProduct(ProductInfo model)
        {
            UpdateCommon(model);
            UpdateAttr(model);
            UpdateCategory(model);
        }

        public void UpdateProductImagePath(long pId, string path)
        {
            ProductInfo productInfo = context.ProductInfo.FindById<ProductInfo>(pId);
            if (productInfo != null)
            {
                productInfo.ImagePath = path;
                productInfo.ProductDescriptionInfo.Description = HTMLProcess(productInfo.ProductDescriptionInfo.Description, productInfo.ImagePath);
                productInfo.ProductDescriptionInfo.MobileDescription = HTMLProcess(productInfo.ProductDescriptionInfo.MobileDescription, productInfo.ImagePath);
            }
            context.SaveChanges();
        }

        public void UpdateSalesCount(string skuId, int addSalesCount)
        {
            ProductInfo productInfo = context.SKUInfo.FirstOrDefault((SKUInfo item) => item.Id == skuId).ProductInfo;
            if (productInfo != null)
            {
                ProductInfo saleCounts = productInfo;
                saleCounts.SaleCounts = saleCounts.SaleCounts + addSalesCount;
                context.SaveChanges();
            }
        }

        private void UpdateSKUs(ProductInfo model)
        {
            IQueryable<SKUInfo> sKUInfo =
                from s in context.SKUInfo
                where s.ProductId == model.Id
                select s;
            context.SKUInfo.RemoveRange(sKUInfo);
            context.SKUInfo.AddRange(model.SKUInfo);
            context.SaveChanges();
        }

        public void UpdateStock(string skuId, long stockChange)
        {
            SKUInfo sKUInfo = context.SKUInfo.FirstOrDefault((SKUInfo item) => item.Id == skuId);
            if (sKUInfo != null)
            {
                SKUInfo stock = sKUInfo;
                stock.Stock = stock.Stock + stockChange;
                if (sKUInfo.Stock < 0)
                {
                    throw new HimallException("产品库存不足");
                }
            }
            context.SaveChanges();
        }

        public ProductInfo GetProductByCasno(string CASNo)
        {
            ProductInfo productInfo = (
                from P in context.ProductInfo
                where P.CASNo == CASNo
                select P).FirstOrDefault();
            return productInfo;
        }

        //首页推荐产品查询
        public IQueryable<ProductInfo> RecommendProductList()
        {
            // chemcloud_products 表字段 Volume 临时启用 2表示为推荐产品
            return (from p in context.ProductInfo where p.Volume == 2 select p).Take(8);
        }

        public decimal GetProductPrice(long pid)
        {
            ProductSpec pinfo = (
                from P in context.ProductSpec
                where P.ProductId == pid
                select P).FirstOrDefault();
            if (pinfo == null)
            {
                return 0;
            }
            else
            {
                return pinfo.Price;
            }
        }
        public bool IsExitsProductCode(string productCode, long shopId)
        {
            IQueryable<ProductInfo> productInfo = context.ProductInfo.FindBy((ProductInfo m) => m.ProductCode == productCode && m.ShopId == shopId);
            if (productInfo.ToList().Count > 0)
                return true;
            else
                return false;
        }
        #region 首页：最新报价

        #region 前N条最新报价商品
        /// <summary>
        /// 前N条最新报价商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result_List<ProductInfoSpec> Get_Products_Top(int count)
        {
            Result_List<ProductInfoSpec> res = new Result_List<ProductInfoSpec>()
            {
                List = new List<ProductInfoSpec>(),
                Msg = new Result_Msg() { IsSuccess = false }
            };

            Result_List<ProductInfo> resProducts = new Result_List<ProductInfo>()
            {
                List = new List<ProductInfo>(),
                Msg = new Result_Msg() { IsSuccess = false }
            };
            resProducts.List = (from P in context.ProductInfo where P.ProductName != "" && P.ProductName != null orderby P.AddedDate descending select P).Take(count).ToList();

            if (resProducts.List != null)
            {
                foreach (var item in resProducts.List)
                {
                    Result_Model<ProductInfo> modelProduct = Get_ProductInfo_ById(item.Id);
                    Result_List<ProductSpec> resList = Get_ProductSpec_ById(item.Id);
                    if (modelProduct != null && resList != null && modelProduct.Msg.IsSuccess && resList.Msg.IsSuccess && resList.List.Count > 0)
                    {
                        var modelSpec = resList.List.FirstOrDefault();
                        ProductInfoSpec resItem = new ProductInfoSpec()
                        {
                            Id = modelSpec.Id,
                            CoinType = modelSpec.CoinType,
                            Packaging = modelSpec.Packaging,
                            Price = modelSpec.Price,
                            ProductId = modelSpec.ProductId,
                            Purity = modelSpec.Purity,
                            SpecLevel = modelSpec.SpecLevel,
                            ProductName = modelProduct.Model.ProductName
                        };
                        res.List.Add(resItem);
                    }
                }
            }
            else
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "获取产品价格列表失败" };
            }
            return res;
        }
        #endregion

        #region 获取指定商品信息
        /// <summary>
        /// 获取指定商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result_Model<ProductInfo> Get_ProductInfo_ById(long id)
        {
            Result_Model<ProductInfo> resList = new Result_Model<ProductInfo>()
            {
                Model = new ProductInfo(),
                Msg = new Result_Msg() { IsSuccess = false }
            };
            resList.Model = (from P in context.ProductInfo where P.Id == id select P).FirstOrDefault();
            if (resList.Model != null)
            {
                resList.Msg = new Result_Msg() { IsSuccess = true };
            }
            else
            {
                resList.Msg = new Result_Msg() { IsSuccess = false, Message = "获取产品信息失败" };
            }
            return resList;
        }
        #endregion

        #region 获取指定商品价格列表
        /// <summary>
        /// 获取指定商品价格列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result_List<ProductSpec> Get_ProductSpec_ById(long id)
        {
            Result_List<ProductSpec> resList = new Result_List<ProductSpec>()
            {
                List = new List<ProductSpec>(),
                Msg = new Result_Msg() { IsSuccess = false }
            };
            resList.List = (from P in context.ProductSpec where P.ProductId == id select P).ToList();
            if (resList.List != null)
            {
                resList.Msg = new Result_Msg() { IsSuccess = true };
            }
            else
            {
                resList.Msg = new Result_Msg() { IsSuccess = false, Message = "获取产品价格列表失败" };
            }
            return resList;
        }
        #endregion

        #endregion

        /// <summary>
        /// 首页产品搜索供应商列表
        /// </summary>
        /// <param name="productQueryModel"></param>
        /// <returns></returns>
        public PageModel<ProductInfo> GetProductsToSearch(ProductQuery productQueryModel)
        {
            int num;
            IQueryable<ProductInfo> productsByQueryModel = GetProductsByQueryModelToSearch(productQueryModel, out num);
            return new PageModel<ProductInfo>()
            {
                Models = productsByQueryModel,
                Total = num
            };
        }
        /// <summary>
        /// 首页产品搜索供应商列表
        /// </summary>
        /// <param name="productQueryModel"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private IQueryable<ProductInfo> GetProductsByQueryModelToSearch(ProductQuery productQueryModel, out int total)
        {
            cointype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"] == null ? "1" : System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            IQueryable<ProductInfo> source = from item in base.context.ProductInfo
                                             join itemshop in base.context.ShopInfo
                                             on item.ShopId equals itemshop.Id
                                             where true
                                             where ((int)item.SaleStatus) == 1 && ((int)item.AuditStatus) == 2 && ((int)itemshop.ShopStatus) == 7
                                             orderby itemshop.SortType descending, item.Id descending
                                             select item;

            if ((productQueryModel.Ids != null) && (productQueryModel.Ids.Count() > 0))
            {
                source = from item in source
                         where productQueryModel.Ids.Contains(item.Id)
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.ProductCode))
            {
                source = from item in source
                         where item.ProductCode == productQueryModel.ProductCode
                         select item;
            }
            if (productQueryModel.CASNo != 0)
            {
                source = from item in source
                         where item.Pub_CID == productQueryModel.CASNo
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.CompanyName))
            {
                ShopInfo shopinfo = (
                  from P in context.ShopInfo
                  where P.CompanyName == productQueryModel.CompanyName
                  select P).FirstOrDefault();
                source = from item in source
                         where item.ShopId == shopinfo.Id
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.ProductName))
            {
                source = from item in source
                         where item.ProductName.Contains(productQueryModel.ProductName) || item.EProductName.Contains(productQueryModel.ProductName)
                         orderby item.MarketPrice descending
                         select item;
            }

            if (productQueryModel.ShopId.HasValue)
            {
                source = from item in source
                         where item.ShopId == productQueryModel.ShopId
                         select item;
            }
            if (productQueryModel.AuditStatus != null)
            {
                source = from item in source
                         where (int)item.AuditStatus == (int)productQueryModel.AuditStatus.Value//.Contains<ProductInfo.ProductAuditStatus>(item.AuditStatus)
                         select item;
            }

            if (productQueryModel.SaleStatus.HasValue)
            {
                source = from item in source
                         where ((int?)item.SaleStatus) == ((int?)productQueryModel.SaleStatus)
                         select item;
            }
            if (productQueryModel.CategoryId.HasValue)
            {
                source = from item in source
                         where ("|" + item.CategoryPath + "|").Contains("|" + productQueryModel.CategoryId.Value + "|")
                         select item;
            }
            if (productQueryModel.NotIncludedInDraft)
            {
                source = from item in source
                         where ((int)item.SaleStatus) != 3
                         select item;
            }
            if (productQueryModel.StartDate.HasValue)
            {
                source = from item in source
                         where item.AddedDate > productQueryModel.StartDate
                         select item;
            }
            if (productQueryModel.EndDate.HasValue)
            {
                source = from item in source
                         where item.AddedDate <= productQueryModel.EndDate
                         select item;
            }
            if (!string.IsNullOrWhiteSpace(productQueryModel.KeyWords))
            {
                source = from item in source
                         where item.ProductName.Contains(productQueryModel.KeyWords)
                         select item;
            }
            if (productQueryModel.ShopType != null)
            {
                if (productQueryModel.ShopType != "0")
                {
                    source = from item in source
                             join itemshop in base.context.ShopInfo
                                 on item.ShopId equals itemshop.Id
                             where true
                             where itemshop.SortType.ToString() == productQueryModel.ShopType
                             select item;
                }
            }
            if (source != null)
            {
                source = source.GroupBy(q => q.ShopId).Select(s => s.FirstOrDefault());
            }


            if (!string.IsNullOrWhiteSpace(productQueryModel.CompanyName))
            {
                IQueryable<long> shopIds = from item in base.context.ShopInfo.FindBy(item => item.CompanyName.Contains(productQueryModel.CompanyName)) select item.Id;
                source = from item in source
                         where shopIds.Contains(item.ShopId)
                         select item;
            }
            if (productQueryModel.IsLimitTimeBuy)
            {
                IQueryable<long> limits = from l in base.context.LimitTimeMarketInfo
                                          where ((int)l.AuditStatus) == 2
                                          select l.ProductId;
                source = from p in source
                         where !limits.Contains(p.Id)
                         select p;
            }
            long shopCateogryId = productQueryModel.ShopCategoryId.GetValueOrDefault();
            if (!string.IsNullOrWhiteSpace(productQueryModel.BrandNameKeyword))
            {
                IQueryable<long> productIds = from item in
                                                  (((from product in source
                                                     join brand in base.context.BrandInfo on product.BrandId equals brand.Id
                                                     select new { Id = product.Id, Name = brand.Name, ShopCategories = from item in product.ChemCloud_ProductShopCategories select item.ShopCategoryInfo })).FindBy(item => item.Name.Contains(productQueryModel.BrandNameKeyword) && (((shopCateogryId == 0L) || (from t in item.ShopCategories select t.ParentCategoryId).Contains(shopCateogryId)) || (from t in item.ShopCategories select t.Id).Contains(shopCateogryId)), productQueryModel.PageNo, productQueryModel.PageSize, out total, item => item.Id, false))
                                              select item.Id;
                source = from item in base.context.ProductInfo
                         where productIds.Contains(item.Id)
                         select item;
            }
            else
            {
                IEnumerable<long> productIds = new long[0];
                if (productQueryModel.ShopCategoryId.HasValue)
                {
                    productIds = from item in base.context.ProductShopCategoryInfo
                                 where (item.ShopCategoryInfo.Id == shopCateogryId) || (item.ShopCategoryInfo.ParentCategoryId == shopCateogryId)
                                 select item.ProductId;
                }
                total = source.Count();
                source = source.FindBy<ProductInfo, long>(item => (shopCateogryId == 0L) || productIds.Contains(item.Id), productQueryModel.PageNo, productQueryModel.PageSize, out total, item => item.Id, false);
            }

            foreach (ProductInfo info in source.ToList())
            {
                info.ShopCateogryInfos = from item in info.ChemCloud_ProductShopCategories select item.ShopCategoryInfo;
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(info.ShopId));
                info.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
                info.ShopName = (shopInfo == null ? "" : shopInfo.ShopName);
                info.ShopId = info.ShopId;
                /*注意当前语言版本对应的货币种类*/
                ProductSpec ProductSpecinfo = context.ProductSpec.FirstOrDefault((ProductSpec p) => p.ProductId.Equals(info.Id) && p.CoinType.Equals(cointype));
                info.Packaging = (ProductSpecinfo == null ? "" : ProductSpecinfo.Packaging);
                info.Purity = (ProductSpecinfo == null ? "" : ProductSpecinfo.Purity);
                info.SpecLevel = (ProductSpecinfo == null ? "" : ProductSpecinfo.SpecLevel);
                info.Price = (ProductSpecinfo == null ? 0 : ProductSpecinfo.Price);
            }

            return source;
        }


        /*特价*/
        public void UpdateSpecicalOffer(long id, long shopId, decimal status)
        {
            UpdateSpecicalOffer(new long[] { id }, shopId, status);
        }
        /*特价*/
        public void UpdateSpecicalOffer(IEnumerable<long> ids, long shopId, decimal status)
        {
            IQueryable<ProductInfo> productInfo =
                from item in context.ProductInfo
                where ids.Contains(item.Id)
                select item;

            foreach (long list in (
                from d in productInfo
                select d.Id).ToList())
            {
                UpdateSpecicalOfferOK(list, status);
            }
        }
        /*特价*/
        public bool UpdateSpecicalOfferOK(long id, decimal status)
        {
            ProductInfo productInfo = context.ProductInfo.FindById<ProductInfo>(id);
            productInfo.Weight = status;
            productInfo.Volume = status;
            context.SaveChanges();
            return true;
        }
    }
}