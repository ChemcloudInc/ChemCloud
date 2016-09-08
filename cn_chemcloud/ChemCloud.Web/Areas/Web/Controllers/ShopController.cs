using ChemCloud.Core;
using ChemCloud.DBUtility;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.SellerAdmin.Models;
using ChemCloud.Web.Areas.Web;
using ChemCloud.Web.Areas.Web.Helper;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class ShopController : BaseWebController
    {
        public ShopController()
        {
        }

        [HttpPost]
        public JsonResult AddFavorite(long shopId)
        {
            ServiceHelper.Create<IShopService>().AddFavoriteShop(base.CurrentUser.Id, shopId);
            return Json(new { success = true });
        }

        public ActionResult Home(string id)
        {
            int i;
            long num = 0;
            ShopInfo shop = null;
            if (!long.TryParse(id, out num))
            {
                return RedirectToAction("Error404", "Error", new { area = "Web" });
            }
            shop = ServiceHelper.Create<IShopService>().GetShop(num, false);
            if (shop == null)
            {
                return RedirectToAction("Error404", "Error", new { area = "Web" });
            }
            ShopHomeModel shopHomeModel = new ShopHomeModel()
            {
                HotAttentionProducts = new List<HotProductInfo>(),
                HotSaleProducts = new List<HotProductInfo>(),
                Floors = new List<ShopHomeFloor>(),
                Navignations = new List<BannerInfo>(),
                Shop = new ShopInfoModel(),
                ShopCategory = new List<CategoryJsonModel>(),
                Slides = new List<SlideAdInfo>(),
                Logo = ""
            };
            ShopHomeModel shopName = shopHomeModel;
            IQueryable<StatisticOrderCommentsInfo> shopStatisticOrderComments = ServiceHelper.Create<IShopService>().GetShopStatisticOrderComments(num);
            StatisticOrderCommentsInfo statisticOrderCommentsInfo = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 1
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo1 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 9
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo2 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 5
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo3 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 2
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo4 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 10
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo5 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 6
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo6 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 3
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo7 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 4
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo8 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 11
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo9 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 12
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo10 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 7
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo11 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 8
                select c).FirstOrDefault();
            int num1 = 5;
            if (statisticOrderCommentsInfo == null || statisticOrderCommentsInfo3 == null)
            {
                ViewBag.ProductAndDescription = num1;
                ViewBag.ProductAndDescriptionPeer = num1;
                ViewBag.ProductAndDescriptionMin = num1;
                ViewBag.ProductAndDescriptionMax = num1;
            }
            else
            {
                ViewBag.ProductAndDescription = statisticOrderCommentsInfo.CommentValue;
                ViewBag.ProductAndDescriptionPeer = statisticOrderCommentsInfo3.CommentValue;
                ViewBag.ProductAndDescriptionMin = statisticOrderCommentsInfo7.CommentValue;
                ViewBag.ProductAndDescriptionMax = statisticOrderCommentsInfo6.CommentValue;
            }
            if (statisticOrderCommentsInfo1 == null || statisticOrderCommentsInfo4 == null)
            {
                ViewBag.SellerServiceAttitude = num1;
                ViewBag.SellerServiceAttitudePeer = num1;
                ViewBag.SellerServiceAttitudeMax = num1;
                ViewBag.SellerServiceAttitudeMin = num1;
            }
            else
            {
                ViewBag.SellerServiceAttitude = statisticOrderCommentsInfo1.CommentValue;
                ViewBag.SellerServiceAttitudePeer = statisticOrderCommentsInfo4.CommentValue;
                ViewBag.SellerServiceAttitudeMax = statisticOrderCommentsInfo8.CommentValue;
                ViewBag.SellerServiceAttitudeMin = statisticOrderCommentsInfo9.CommentValue;
            }
            if (statisticOrderCommentsInfo5 == null || statisticOrderCommentsInfo2 == null)
            {
                ViewBag.SellerDeliverySpeed = num1;
                ViewBag.SellerDeliverySpeedPeer = num1;
                ViewBag.SellerDeliverySpeedMax = num1;
                ViewBag.sellerDeliverySpeedMin = num1;
            }
            else
            {
                ViewBag.SellerDeliverySpeed = statisticOrderCommentsInfo2.CommentValue;
                ViewBag.SellerDeliverySpeedPeer = statisticOrderCommentsInfo5.CommentValue;
                ViewBag.SellerDeliverySpeedMax = statisticOrderCommentsInfo10.CommentValue;
                ViewBag.sellerDeliverySpeedMin = statisticOrderCommentsInfo11.CommentValue;
            }
            ShopServiceMarkModel shopComprehensiveMark = ShopServiceMark.GetShopComprehensiveMark(shop.Id);
            shopName.Shop.Name = shop.ShopName;
            shopName.Shop.CompanyName = shop.CompanyName;
            shopName.Shop.Id = shop.Id;
            shopName.Shop.PackMark = shopComprehensiveMark.PackMark;
            shopName.Shop.ServiceMark = shopComprehensiveMark.ServiceMark;
            shopName.Shop.ComprehensiveMark = shopComprehensiveMark.ComprehensiveMark;
            shopName.Shop.Phone = shop.CompanyPhone;
            shopName.Shop.Address = ServiceHelper.Create<IRegionService>().GetRegionFullName(shop.CompanyRegionId, " ");
            shopName.Logo = shop.Logo;
            shopName.Navignations = ServiceHelper.Create<INavigationService>().GetSellerNavigations(shop.Id, PlatformType.PC).ToList();
            shopName.ImageAds = (
                from item in ServiceHelper.Create<ISlideAdsService>().GetImageAds(shop.Id)
                orderby item.Id
                select item).ToList();
            shopName.Slides = ServiceHelper.Create<ISlideAdsService>().GetSlidAds(shop.Id, SlideAdInfo.SlideAdType.ShopHome).ToList();
            List<ShopCategoryInfo> list = ServiceHelper.Create<IShopCategoryService>().GetShopCategory(shop.Id).ToList();
            foreach (ShopCategoryInfo shopCategoryInfo in
                from s in list
                where s.ParentCategoryId == 0
                select s)
            {
                CategoryJsonModel categoryJsonModel = new CategoryJsonModel()
                {
                    Name = shopCategoryInfo.Name,
                    Id = shopCategoryInfo.Id.ToString(),
                    SubCategory = new List<SecondLevelCategory>()
                };
                CategoryJsonModel categoryJsonModel1 = categoryJsonModel;
                foreach (ShopCategoryInfo shopCategoryInfo1 in
                    from s in list
                    where s.ParentCategoryId == shopCategoryInfo.Id
                    select s)
                {
                    SecondLevelCategory secondLevelCategory = new SecondLevelCategory()
                    {
                        Name = shopCategoryInfo1.Name,
                        Id = shopCategoryInfo1.Id.ToString()
                    };
                    categoryJsonModel1.SubCategory.Add(secondLevelCategory);
                }
                shopName.ShopCategory.Add(categoryJsonModel1);
            }
            ShopHomeModuleInfo[] array = ServiceHelper.Create<IShopHomeModuleService>().GetAllShopHomeModuleInfos(shop.Id).ToArray();
            ShopHomeModuleInfo[] shopHomeModuleInfoArray = array;
            for (i = 0; i < shopHomeModuleInfoArray.Length; i++)
            {
                ShopHomeModuleInfo shopHomeModuleInfo = shopHomeModuleInfoArray[i];
                List<ShopHomeFloorProduct> shopHomeFloorProducts = new List<ShopHomeFloorProduct>();
                foreach (ShopHomeModuleProductInfo shopHomeModuleProductInfo in shopHomeModuleInfo.ShopHomeModuleProductInfo.ToList())
                {
                    ShopHomeFloorProduct shopHomeFloorProduct = new ShopHomeFloorProduct()
                    {
                        Id = shopHomeModuleProductInfo.ProductId,
                        Name = shopHomeModuleProductInfo.ProductInfo.ProductName,
                        Pic = shopHomeModuleProductInfo.ProductInfo.ImagePath,
                        Price = shopHomeModuleProductInfo.ProductInfo.MinSalePrice.ToString("f2"),
                        SaleCount = (int)ServiceHelper.Create<IProductService>().GetProductVistInfo(shopHomeModuleProductInfo.ProductInfo.Id, null).SaleCounts
                    };
                    shopHomeFloorProducts.Add(shopHomeFloorProduct);
                }
                ShopHomeFloor shopHomeFloor = new ShopHomeFloor()
                {
                    FloorName = shopHomeModuleInfo.Name,
                    Products = shopHomeFloorProducts
                };
                shopName.Floors.Add(shopHomeFloor);
            }
            IQueryable<ProductInfo> hotSaleProduct = ServiceHelper.Create<IProductService>().GetHotSaleProduct(shop.Id, 5);
            if (hotSaleProduct != null)
            {
                ProductInfo[] productInfoArray = hotSaleProduct.ToArray();
                for (i = 0; i < productInfoArray.Length; i++)
                {
                    ProductInfo productInfo = productInfoArray[i];
                    List<HotProductInfo> hotSaleProducts = shopName.HotSaleProducts;
                    HotProductInfo hotProductInfo = new HotProductInfo()
                    {
                        ImgPath = productInfo.ImagePath,
                        Name = productInfo.ProductName,
                        Price = productInfo.MinSalePrice,
                        Id = productInfo.Id,
                        SaleCount = (int)productInfo.SaleCounts
                    };
                    hotSaleProducts.Add(hotProductInfo);
                }
            }
            IQueryable<ProductInfo> hotConcernedProduct = ServiceHelper.Create<IProductService>().GetHotConcernedProduct(shop.Id, 5);
            if (hotConcernedProduct != null)
            {
                foreach (ProductInfo list1 in hotConcernedProduct.ToList())
                {
                    List<HotProductInfo> hotAttentionProducts = shopName.HotAttentionProducts;
                    HotProductInfo hotProductInfo1 = new HotProductInfo()
                    {
                        ImgPath = list1.ImagePath,
                        Name = list1.ProductName,
                        Price = list1.MinSalePrice,
                        Id = list1.Id,
                        SaleCount = list1.ConcernedCount
                    };
                    hotAttentionProducts.Add(hotProductInfo1);
                }
            }
            ServiceHelper.Create<IShopService>().LogShopVisti(shop.Id);
            ViewBag.IsExpired = ServiceHelper.Create<IShopService>().IsExpiredShop(num);
            return View(shopName);
        }

        [HttpPost]
        public JsonResult ReceiveCoupons(long couponId, long shopId)
        {
            ICouponService couponService = ServiceHelper.Create<ICouponService>();
            CouponInfo couponInfo = couponService.GetCouponInfo(shopId, couponId);
            if (base.CurrentUser == null)
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "请登录后领取",
                    status = -1
                };
                return Json(result);
            }
            if (couponInfo.EndTime < DateTime.Now.Date)
            {
                Result result1 = new Result()
                {
                    success = false,
                    msg = "此优惠券已过期",
                    status = -2
                };
                return Json(result1);
            }
            int userReceiveCoupon = couponService.GetUserReceiveCoupon(couponId, base.CurrentUser.Id);
            if (couponInfo.PerMax != 0 && userReceiveCoupon >= couponInfo.PerMax)
            {
                Result result2 = new Result()
                {
                    success = false,
                    msg = string.Concat("每人最多领取", couponInfo.PerMax, "张该类型的优惠券"),
                    status = -3
                };
                return Json(result2);
            }
            if (couponInfo.ChemCloud_CouponRecord.Count >= couponInfo.Num)
            {
                Result result3 = new Result()
                {
                    success = false,
                    msg = "优惠券已领完",
                    status = -3
                };
                return Json(result3);
            }
            if (couponInfo.ReceiveType == CouponInfo.CouponReceiveType.IntegralExchange)
            {
                int availableIntegrals = base.CurrentUser.AvailableIntegrals;
                int needIntegral = couponInfo.NeedIntegral;
            }
            CouponRecordInfo couponRecordInfo = new CouponRecordInfo()
            {
                UserId = base.CurrentUser.Id,
                UserName = base.CurrentUser.UserName,
                ShopId = shopId,
                CouponId = couponId
            };
            couponService.AddCouponRecord(couponRecordInfo);
            Result result4 = new Result()
            {
                success = true,
                msg = "领取成功",
                status = 1
            };
            return Json(result4);
        }

        public ActionResult Search(string sid, long cid = 0L, string keywords = "", int pageNo = 1, [DecimalConstant(0, 0, 0, 0, 0)] decimal startPrice = default(decimal), decimal endPrice = 0)
        {
            long num;
            int num1 = 40;
            long num2 = 0;
            ShopInfo shop = null;
            endPrice = (endPrice <= new decimal(0) || endPrice < startPrice ? new decimal(-1, -1, -1, false, 0) : endPrice);
            startPrice = (startPrice < new decimal(0) ? new decimal(0) : startPrice);
            if (!long.TryParse(sid, out num2))
            {
                return RedirectToAction("Error404", "Error", new { area = "Web" });
            }
            shop = ServiceHelper.Create<IShopService>().GetShop(num2, false);
            if (shop == null)
            {
                return RedirectToAction("Error404", "Error", new { area = "Web" });
            }
            ShopHomeModel shopHomeModel = new ShopHomeModel()
            {
                HotAttentionProducts = new List<HotProductInfo>(),
                HotSaleProducts = new List<HotProductInfo>(),
                Floors = new List<ShopHomeFloor>(),
                Navignations = new List<BannerInfo>(),
                Shop = new ShopInfoModel(),
                ShopCategory = new List<CategoryJsonModel>(),
                Slides = new List<SlideAdInfo>(),
                Logo = ""
            };
            ShopHomeModel list = shopHomeModel;
            list.Navignations = ServiceHelper.Create<INavigationService>().GetSellerNavigations(shop.Id, PlatformType.PC).ToList();
            list.ImageAds = (
                from item in ServiceHelper.Create<ISlideAdsService>().GetImageAds(shop.Id)
                orderby item.Id
                select item).ToList();
            list.Slides = ServiceHelper.Create<ISlideAdsService>().GetSlidAds(shop.Id, SlideAdInfo.SlideAdType.ShopHome).ToList();
            ShopCategoryInfo[] array = ServiceHelper.Create<IShopCategoryService>().GetShopCategory(shop.Id).ToArray();
            foreach (ShopCategoryInfo shopCategoryInfo in
                from s in array
                where s.ParentCategoryId == 0
                select s)
            {
                CategoryJsonModel categoryJsonModel = new CategoryJsonModel()
                {
                    Name = shopCategoryInfo.Name,
                    Id = shopCategoryInfo.Id.ToString(),
                    SubCategory = new List<SecondLevelCategory>()
                };
                CategoryJsonModel categoryJsonModel1 = categoryJsonModel;
                foreach (ShopCategoryInfo shopCategoryInfo1 in
                    from s in array
                    where s.ParentCategoryId == shopCategoryInfo.Id
                    select s)
                {
                    SecondLevelCategory secondLevelCategory = new SecondLevelCategory()
                    {
                        Name = shopCategoryInfo1.Name,
                        Id = shopCategoryInfo1.Id.ToString()
                    };
                    categoryJsonModel1.SubCategory.Add(secondLevelCategory);
                }
                list.ShopCategory.Add(categoryJsonModel1);
            }
            ShopServiceMarkModel shopComprehensiveMark = ShopServiceMark.GetShopComprehensiveMark(shop.Id);
            list.Shop.Name = shop.ShopName;
            list.Shop.CompanyName = shop.CompanyName;
            list.Shop.Id = shop.Id;
            list.Shop.PackMark = shopComprehensiveMark.PackMark;
            list.Shop.ServiceMark = shopComprehensiveMark.ServiceMark;
            list.Shop.ComprehensiveMark = shopComprehensiveMark.ComprehensiveMark;
            list.Shop.Phone = shop.CompanyPhone;
            list.Shop.Address = ServiceHelper.Create<IRegionService>().GetRegionFullName(shop.CompanyRegionId, " ");
            list.Logo = shop.Logo;
            ProductSearch productSearch = new ProductSearch()
            {
                startPrice = startPrice,
                EndPrice = endPrice,
                shopId = num2,
                BrandId = 0,
                ShopCategoryId = new long?(cid),
                Ex_Keyword = "",
                Keyword = keywords,
                OrderKey = 0,
                OrderType = true,
                AttrIds = new List<string>(),
                PageSize = num1,
                PageNumber = pageNo
            };
            PageModel<ProductInfo> pageModel = ServiceHelper.Create<IProductService>().SearchProduct(productSearch);
            int total = pageModel.Total;
            ProductInfo[] productInfoArray = pageModel.Models.ToArray();
            ProductInfo[] productInfoArray1 = productInfoArray;
            for (int i = 0; i < productInfoArray1.Length; i++)
            {
                ProductInfo saleCounts = productInfoArray1[i];
                saleCounts.SaleCounts = ServiceHelper.Create<IProductService>().GetProductVistInfo(saleCounts.Id, null).SaleCounts;
            }
            list.Products = ((IEnumerable<ProductInfo>)(productInfoArray ?? new ProductInfo[0])).ToList();
            IQueryable<ProductInfo> hotSaleProduct = ServiceHelper.Create<IProductService>().GetHotSaleProduct(shop.Id, 5);
            if (hotSaleProduct != null)
            {
                foreach (ProductInfo productInfo in hotSaleProduct)
                {
                    List<HotProductInfo> hotSaleProducts = list.HotSaleProducts;
                    HotProductInfo hotProductInfo = new HotProductInfo()
                    {
                        ImgPath = productInfo.ImagePath,
                        Name = productInfo.ProductName,
                        Price = productInfo.MinSalePrice,
                        Id = productInfo.Id,
                        SaleCount = (int)productInfo.SaleCounts
                    };
                    hotSaleProducts.Add(hotProductInfo);
                }
            }
            List<ProductInfo> productInfos = ServiceHelper.Create<IProductService>().GetHotConcernedProduct(shop.Id, 5).ToList();
            if (productInfos != null)
            {
                foreach (ProductInfo productInfo1 in productInfos)
                {
                    List<HotProductInfo> hotAttentionProducts = list.HotAttentionProducts;
                    HotProductInfo hotProductInfo1 = new HotProductInfo()
                    {
                        ImgPath = productInfo1.ImagePath,
                        Name = productInfo1.ProductName,
                        Price = productInfo1.MinSalePrice,
                        Id = productInfo1.Id,
                        SaleCount = productInfo1.ConcernedCount
                    };
                    hotAttentionProducts.Add(hotProductInfo1);
                }
            }
            IQueryable<StatisticOrderCommentsInfo> shopStatisticOrderComments = ServiceHelper.Create<IShopService>().GetShopStatisticOrderComments(num2);
            StatisticOrderCommentsInfo statisticOrderCommentsInfo = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 1
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo1 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 9
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo2 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 5
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo3 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 2
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo4 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 10
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo5 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 6
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo6 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 3
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo7 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 4
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo8 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 11
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo9 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 12
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo10 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 7
                select c).FirstOrDefault();
            StatisticOrderCommentsInfo statisticOrderCommentsInfo11 = (
                from c in shopStatisticOrderComments
                where (int)c.CommentKey == 8
                select c).FirstOrDefault();
            int num3 = 5;
            if (statisticOrderCommentsInfo == null || statisticOrderCommentsInfo3 == null)
            {
                ViewBag.ProductAndDescription = num3;
                ViewBag.ProductAndDescriptionPeer = num3;
                ViewBag.ProductAndDescriptionMin = num3;
                ViewBag.ProductAndDescriptionMax = num3;
            }
            else
            {
                ViewBag.ProductAndDescription = statisticOrderCommentsInfo.CommentValue;
                ViewBag.ProductAndDescriptionPeer = statisticOrderCommentsInfo3.CommentValue;
                ViewBag.ProductAndDescriptionMin = statisticOrderCommentsInfo7.CommentValue;
                ViewBag.ProductAndDescriptionMax = statisticOrderCommentsInfo6.CommentValue;
            }
            if (statisticOrderCommentsInfo1 == null || statisticOrderCommentsInfo4 == null)
            {
                ViewBag.SellerServiceAttitude = num3;
                ViewBag.SellerServiceAttitudePeer = num3;
                ViewBag.SellerServiceAttitudeMax = num3;
                ViewBag.SellerServiceAttitudeMin = num3;
            }
            else
            {
                ViewBag.SellerServiceAttitude = statisticOrderCommentsInfo1.CommentValue;
                ViewBag.SellerServiceAttitudePeer = statisticOrderCommentsInfo4.CommentValue;
                ViewBag.SellerServiceAttitudeMax = statisticOrderCommentsInfo8.CommentValue;
                ViewBag.SellerServiceAttitudeMin = statisticOrderCommentsInfo9.CommentValue;
            }
            if (statisticOrderCommentsInfo5 == null || statisticOrderCommentsInfo2 == null)
            {
                ViewBag.SellerDeliverySpeed = num3;
                ViewBag.SellerDeliverySpeedPeer = num3;
                ViewBag.SellerDeliverySpeedMax = num3;
                ViewBag.sellerDeliverySpeedMin = num3;
            }
            else
            {
                ViewBag.SellerDeliverySpeed = statisticOrderCommentsInfo2.CommentValue;
                ViewBag.SellerDeliverySpeedPeer = statisticOrderCommentsInfo5.CommentValue;
                ViewBag.SellerDeliverySpeedMax = statisticOrderCommentsInfo10.CommentValue;
                ViewBag.sellerDeliverySpeedMin = statisticOrderCommentsInfo11.CommentValue;
            }
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = pageNo,
                ItemsPerPage = num1,
                TotalItems = total
            };
            ViewBag.pageInfo = pagingInfo;
            string empty = string.Empty;
            if (keywords == string.Empty && cid != 0)
            {
                empty = (ServiceHelper.Create<IShopCategoryService>().GetCategory(cid) ?? new ShopCategoryInfo()).Name;
            }
            ViewBag.CategoryName = empty;
            ViewBag.Keyword = keywords;
            ViewBag.cid = cid;
            dynamic viewBag = base.ViewBag;
            num = (base.CurrentUser == null ? 0 : base.CurrentUser.Id);
            viewBag.BrowsedHistory = BrowseHistrory.GetBrowsingProducts(13, num);
            return View(list);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopStore(long shopId)
        {
            ShopStore homeModel = new ShopStore();

            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(shopId, false);
            if (shop != null)
            {
                ViewBag.SortType = shop.SortType;
                ViewBag.Logo = base.CurrentSiteSetting.MemberLogo;
                homeModel.ShopId = shop.Id;
                homeModel.ShopLogo = shop.Logo;
                homeModel.CompanyAddress = shop.CompanyAddress;// +"/" + shop.ECompanyAddress + "</span>";
                homeModel.ShopName = shop.ShopName;// +"/" + shop.ECompanyName;
                homeModel.ContactsEmail = shop.ContactsEmail;
                homeModel.ContactsPhone = shop.ContactsPhone;
                homeModel.CompanyFoundingDate = shop.CompanyFoundingDate.HasValue ? shop.CompanyFoundingDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                homeModel.URL = shop.URL;
                homeModel.CompanyFoundingDate = shop.CreateDate.ToShortDateString();
                homeModel.GMPPhoto = shop.GMPPhoto;
                homeModel.ISOPhoto = shop.ISOPhoto;
                homeModel.FDAPhoto = shop.FDAPhoto;
                homeModel.Fax = shop.Fax;
                homeModel.OrganizationCodePhoto = shop.OrganizationCodePhoto;
                homeModel.TaxRegistrationCertificatePhoto = shop.TaxRegistrationCertificatePhoto;
                homeModel.BusinessLicenceNumberPhoto = shop.BusinessLicenceNumberPhoto;
                homeModel.ShopAccount = shop.ShopAccount;
                homeModel.ContactsName = shop.ContactsName;// +"/" + shop.EContactsName;
                homeModel.BankName = shop.BankName;
                homeModel.BankAccountNumber = shop.BankAccountNumber;
               // homeModel.Price=
                homeModel.ShopEndDate = (shop.EndDate.HasValue ? shop.EndDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                ShopGradeInfo shopGradeInfo = (
                    from c in ServiceHelper.Create<IShopService>().GetShopGrades()
                    where c.Id == shop.GradeId
                    select c).FirstOrDefault();
                homeModel.ShopGradeName = (shopGradeInfo != null ? shopGradeInfo.Name : string.Empty);


                IQueryable<StatisticOrderCommentsInfo> shopStatisticOrderComments = ServiceHelper.Create<IShopService>().GetShopStatisticOrderComments(shopId);
                StatisticOrderCommentsInfo statisticOrderCommentsInfo = (
                    from c in shopStatisticOrderComments
                    where (int)c.CommentKey == 13
                    select c).FirstOrDefault(); //质量与描述满意度
                StatisticOrderCommentsInfo statisticOrderCommentsInfo1 = (
                    from c in shopStatisticOrderComments
                    where (int)c.CommentKey == 14
                    select c).FirstOrDefault();//发货速度满意度
                StatisticOrderCommentsInfo statisticOrderCommentsInfo2 = (
                    from c in shopStatisticOrderComments
                    where (int)c.CommentKey == 15
                    select c).FirstOrDefault();//服务态度满意度
                StatisticOrderCommentsInfo statisticOrderCommentsInfo3 = (
                    from c in shopStatisticOrderComments
                    where (int)c.CommentKey == 16//包装质量满意度
                    select c).FirstOrDefault();
                string str = "5";//如果无评价数据，默认为5分
                homeModel.ProductAndDescription = (statisticOrderCommentsInfo != null ? string.Format("{0:F}", statisticOrderCommentsInfo.CommentValue) : str);
                homeModel.SellerServiceAttitude = (statisticOrderCommentsInfo1 != null ? string.Format("{0:F}", statisticOrderCommentsInfo1.CommentValue) : str);
                homeModel.SellerDeliverySpeed = (statisticOrderCommentsInfo2 != null ? string.Format("{0:F}", statisticOrderCommentsInfo2.CommentValue) : str);
                homeModel.PackingQuality = (statisticOrderCommentsInfo3 != null ? string.Format("{0:F}", statisticOrderCommentsInfo3.CommentValue) : str);


                DateTime date = DateTime.Now.AddDays(-1).Date;
                DateTime dateTime = date.AddDays(1).AddMilliseconds(-1);
                ShopInfo.ShopVistis shopVistiInfo = ServiceHelper.Create<IShopService>().GetShopVistiInfo(date, dateTime, shopId);
                List<EchartsData> echartsDatas = new List<EchartsData>();
                if (shopVistiInfo == null)
                {
                    string str1 = (new decimal(0)).ToString();
                    ViewBag.VistiCounts = str1;
                    ViewBag.OrderCounts = str1;
                    ViewBag.SaleAmounts = str1;
                }
                else
                {
                    ViewBag.VistiCounts = shopVistiInfo.VistiCounts;
                    ViewBag.OrderCounts = shopVistiInfo.OrderCounts;
                    ViewBag.SaleAmounts = shopVistiInfo.SaleAmounts;
                }
            }

            return View(homeModel);
        }

        [HttpPost]
        public JsonResult SearchResult(long shopId,int pagesize, int pageno)
        {
            DataSet _ds = GetListByPage(shopId, (pageno - 1) * pagesize + 1, pageno * pagesize);
            if (_ds == null || _ds.Tables[0].Rows.Count < 1)
            {
                return Json("");
            }
            else
            {
                string reslut = ChemCloud.Core.JsonHelper.GetJsonByDatasetAdv(_ds);
                 
                return Json(reslut);
            }
        }

       

        public DataSet GetListByPage(long shopId, int startIndex, int endIndex)
        {
          
            StringBuilder strSql = new StringBuilder();

            strSql.AppendFormat(@"with product as
                      (
                       select Id,CasNo,ShopId,ProductCode,Purity,ProductName,MinSalePrice as Price,Pub_CID,EproductName from 
                       ( SELECT ROW_NUMBER() OVER (order by T.CasNo ASC)AS Row,
                       T.casno,T.id,T.ShopId,T.ProductCode,T.Purity,T.ProductName,T.MinSalePrice,T.Pub_CID,T.EproductName from chemcloud_products 
                     T where ShopId={0}   ) 
                      TT WHERE TT.Row between {1} and {2}
                      ),
                      castable as
                      (
                      SELECT Distinct cas.PUB_CID,Record_Title,CAS,Molecular_Formula,Molecular_Weight
                      from ChemCloud_CAS cas inner join product on cas.CAS=product.CASNo and cas.pub_cid=product.pub_cid
                      )
                     select p.*,cas.* from product p left join castable cas
                     on p.casno=cas.cas", shopId,startIndex,endIndex);

            return DbHelperSQL.Query(strSql.ToString());
        }

        [HttpPost]
        public JsonResult SearchResultCount(long shopId)
        {
            StringBuilder sb = new StringBuilder();

            //sb.AppendFormat(@"Select COUNT(0) as COUNT FROM chemcloud_products where ShopId={0} and CASNo!=''", shopId);
            sb.AppendFormat(@"Select COUNT(0) as COUNT FROM chemcloud_products where ShopId={0}", shopId);

            DataSet ds = DbHelperSQL.Query(sb.ToString());
            if (ds == null)
            {
                return Json("0");
            }
            else
            {
                return Json(ds.Tables[0].Rows[0]["COUNT"].ToString());
            }
        }
    }
}