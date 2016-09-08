using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.SellerAdmin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using ChemCloud.DBUtility;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class ProductController : BaseSellerController
    {
        private long shopId = 2;

        public ProductController()
        {
            if (base.CurrentSellerManager != null)
            {
                shopId = base.CurrentSellerManager.ShopId;
            }
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult BatchOnSale(string ids)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            ServiceHelper.Create<IProductService>().OnSale(nums, base.CurrentSellerManager.ShopId);
            IOperationLogService operationLogService = ServiceHelper.Create<IOperationLogService>();
            LogInfo logInfo = new LogInfo()
            {
                Date = DateTime.Now,
                Description = string.Concat("商家商品批量上架，Ids=", ids),
                IPAddress = base.Request.UserHostAddress,
                PageUrl = "/Product/BatchOnSale",
                UserName = base.CurrentSellerManager.UserName,
                ShopId = base.CurrentSellerManager.ShopId
            };
            operationLogService.AddSellerOperationLog(logInfo);
            return Json(new { success = true });
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult BatchSaleOff(string ids)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            ServiceHelper.Create<IProductService>().SaleOff(nums, base.CurrentSellerManager.ShopId);
            IOperationLogService operationLogService = ServiceHelper.Create<IOperationLogService>();
            LogInfo logInfo = new LogInfo()
            {
                Date = DateTime.Now,
                Description = string.Concat("商家商品批量下架，Ids=", ids),
                IPAddress = base.Request.UserHostAddress,
                PageUrl = "/Product/BatchSaleOff",
                UserName = base.CurrentSellerManager.UserName,
                ShopId = base.CurrentSellerManager.ShopId
            };
            operationLogService.AddSellerOperationLog(logInfo);
            return Json(new { success = true });
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult BindTemplates(string ids, long? topTemplateId, long? bottomTemplateId)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            ServiceHelper.Create<IProductService>().BindTemplate(topTemplateId, bottomTemplateId, nums);
            return Json(new { success = true });
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult Browse(long? categoryId, int? auditStatus, string ids, int page, int rows, string keyWords, int? saleStatus, bool? isShopCategory, bool isLimitTimeBuy = false, bool showSku = false)
        {
            long? nullable;
            long? nullable1;
            IEnumerable<long> nums;
            Func<SKUInfo, SKUModel> func = null;
            ProductQuery productQuery = new ProductQuery()
            {
                PageSize = rows,
                PageNo = page,
                KeyWords = keyWords
            };
            ProductQuery productQuery1 = productQuery;
            if (isShopCategory.GetValueOrDefault())
            {
                nullable = null;
            }
            else
            {
                nullable = categoryId;
            }
            productQuery1.CategoryId = nullable;
            ProductQuery productQuery2 = productQuery;
            if (isShopCategory.GetValueOrDefault())
            {
                nullable1 = categoryId;
            }
            else
            {
                nullable1 = null;
            }
            productQuery2.ShopCategoryId = nullable1;
            ProductQuery productQuery3 = productQuery;
            if (string.IsNullOrWhiteSpace(ids))
            {
                nums = null;
            }
            else
            {
                char[] chrArray = new char[] { ',' };
                nums =
                    from item in ids.Split(chrArray)
                    select long.Parse(item);
            }
            productQuery3.Ids = nums;
            productQuery.ShopId = new long?(base.CurrentSellerManager.ShopId);
            productQuery.IsLimitTimeBuy = isLimitTimeBuy;
            ProductQuery value = productQuery;
            if (auditStatus.HasValue)
            {
                value.AuditStatus = new ProductInfo.ProductAuditStatus?((ProductInfo.ProductAuditStatus)auditStatus.GetValueOrDefault());
            }
            if (saleStatus.HasValue)
            {
                value.SaleStatus = new ProductInfo.ProductSaleStatus?((ProductInfo.ProductSaleStatus)saleStatus.Value);
            }
            PageModel<ProductInfo> products = ServiceHelper.Create<IProductService>().GetProducts(value);
            ICategoryService categoryService = ServiceHelper.Create<ICategoryService>();
            IBrandService brandService = ServiceHelper.Create<IBrandService>();
            var collection = products.Models.ToArray().Select((ProductInfo item) =>
            {
                string str;
                string str1;
                IEnumerable<SKUModel> sKUModels;
                BrandInfo brand = brandService.GetBrand(item.BrandId);
                CategoryInfo category = categoryService.GetCategory(item.CategoryId);
                string productName = item.ProductName;
                str = (item.BrandId == 0 || brand == null ? "" : brand.Name);
                str1 = (brand == null ? "" : category.Name);
                long id = item.Id;
                string image = item.GetImage(ProductInfo.ImageSize.Size_50, 1);
                decimal minSalePrice = item.MinSalePrice;
                if (!showSku)
                {
                    sKUModels = null;
                }
                else
                {
                    ICollection<SKUInfo> sKUInfo = item.SKUInfo;
                    if (func == null)
                    {
                        func = (SKUInfo a) => new SKUModel()
                        {
                            Id = a.Id,
                            SalePrice = a.SalePrice,
                            Size = a.Size,
                            Stock = a.Stock,
                            Version = a.Version,
                            Color = a.Color,
                            Sku = a.Sku,
                            AutoId = a.AutoId,
                            ProductId = a.ProductId
                        };
                    }
                    sKUModels = sKUInfo.Select<SKUInfo, SKUModel>(func);
                }
                return new { name = productName, brandName = str, categoryName = str1, id = id, imgUrl = image, price = minSalePrice, skus = sKUModels };
            });
            return Json(new { rows = collection, total = products.Total });
        }

        private SKUSpecModel DeepClone(SKUSpecModel obj)
        {
            SKUSpecModel sKUSpecModel;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                IFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);
                memoryStream.Seek(0, SeekOrigin.Begin);
                sKUSpecModel = binaryFormatter.Deserialize(memoryStream) as SKUSpecModel;
            }
            return sKUSpecModel;
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult Delete(string ids)
        {
            JsonResult jsonResult;
            try
            {
                char[] chrArray = new char[] { ',' };
                IEnumerable<long> nums =
                    from item in ids.Split(chrArray)
                    select long.Parse(item);
                ServiceHelper.Create<IProductService>().DeleteProduct(nums, base.CurrentSellerManager.ShopId);

                jsonResult = Json(new { success = true });
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                jsonResult = Json(new { success = false, msg = exception.Message });
            }
            return jsonResult;
        }

        private void DeleteDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                DirectoryInfo[] directories = directoryInfo.GetDirectories();
                for (int i = 0; i < directories.Length; i++)
                {
                    directories[i].Delete(true);
                }
                directoryInfo.Delete(true);
            }
        }

        [UnAuthorize]
        public JsonResult GetAttributes(long categoryId, long productId = 0L, long isCategoryId = 0L)
        {
            long categoryIdd = 35;
            Dictionary<long, string> nums = new Dictionary<long, string>();
            if (isCategoryId == 1)
            {
                List<TypeAttributesModel> plateformAttr = GetPlateformAttr(categoryIdd);
                return Json(new { json = plateformAttr }, JsonRequestBehavior.AllowGet);
            }
            List<TypeAttributesModel> typeAttributesModels = new List<TypeAttributesModel>();
            IQueryable<ProductAttributeInfo> productAttribute = ServiceHelper.Create<IProductService>().GetProductAttribute(productId);
            if (productAttribute == null || productAttribute.Count() == 0)
            {
                typeAttributesModels = GetPlateformAttr(categoryIdd);
                return Json(new { json = typeAttributesModels }, JsonRequestBehavior.AllowGet);
            }
            ProductAttributeInfo[] array = productAttribute.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                ProductAttributeInfo productAttributeInfo = array[i];
                if (nums.ContainsKey(productAttributeInfo.AttributeId))
                {
                    long attributeId = productAttributeInfo.AttributeId;
                    string item = nums[productAttributeInfo.AttributeId];
                    long valueId = productAttributeInfo.ValueId;
                    nums[attributeId] = string.Concat(item, ",", valueId.ToString());
                }
                else
                {
                    nums.Add(productAttributeInfo.AttributeId, productAttributeInfo.ValueId.ToString());
                    AttributeInfo attributesInfo = productAttributeInfo.AttributesInfo;
                    ICollection<AttributeValueInfo> attributeValueInfo = ServiceHelper.Create<ITypeService>().GetType(attributesInfo.TypeId).AttributeInfo.FirstOrDefault((AttributeInfo a) => a.Id == attributesInfo.Id).AttributeValueInfo;
                    TypeAttributesModel typeAttributesModel = new TypeAttributesModel()
                    {
                        Name = attributesInfo.Name,
                        AttrId = productAttributeInfo.AttributeId,
                        Selected = "",
                        IsMulti = attributesInfo.IsMulti,
                        AttrValues = new List<TypeAttrValue>()
                    };
                    TypeAttributesModel typeAttributesModel1 = typeAttributesModel;
                    AttributeValueInfo[] attributeValueInfoArray = attributeValueInfo.ToArray();
                    for (int j = 0; j < attributeValueInfoArray.Length; j++)
                    {
                        AttributeValueInfo attributeValueInfo1 = attributeValueInfoArray[j];
                        List<TypeAttrValue> attrValues = typeAttributesModel1.AttrValues;
                        TypeAttrValue typeAttrValue = new TypeAttrValue()
                        {
                            Id = attributeValueInfo1.Id.ToString(),
                            Name = attributeValueInfo1.Value
                        };
                        attrValues.Add(typeAttrValue);
                    }
                    categoryId = ServiceHelper.Create<IProductService>().GetProduct(productId).CategoryId;
                    typeAttributesModels.Add(typeAttributesModel1);
                }
            }

            List<TypeAttributesModel> plateformAttr1 = GetPlateformAttr(categoryIdd);
            foreach (TypeAttributesModel item1 in typeAttributesModels)
            {
                item1.Selected = nums[item1.AttrId];
                plateformAttr1.Remove(plateformAttr1.FirstOrDefault((TypeAttributesModel a) => a.AttrId == item1.AttrId));
            }
            typeAttributesModels.AddRange(plateformAttr1);
            return Json(new { json = typeAttributesModels }, JsonRequestBehavior.AllowGet);
        }
        [UnAuthorize]
        public JsonResult GetAttributess(long productId = 0L)
        {
            Dictionary<long, string> nums = new Dictionary<long, string>();

            List<TypeAttributesModel> typeAttributesModels = new List<TypeAttributesModel>();
            IQueryable<ProductAttributeInfo> productAttribute = ServiceHelper.Create<IProductService>().GetProductAttribute(productId);

            ProductAttributeInfo[] array = productAttribute.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                ProductAttributeInfo productAttributeInfo = array[i];
                if (nums.ContainsKey(productAttributeInfo.AttributeId))
                {
                    long attributeId = productAttributeInfo.AttributeId;
                    string item = nums[productAttributeInfo.AttributeId];
                    long valueId = productAttributeInfo.ValueId;
                    nums[attributeId] = string.Concat(item, ",", valueId.ToString());
                }
                else
                {
                    nums.Add(productAttributeInfo.AttributeId, productAttributeInfo.ValueId.ToString());
                    AttributeInfo attributesInfo = productAttributeInfo.AttributesInfo;
                    ICollection<AttributeValueInfo> attributeValueInfo = ServiceHelper.Create<ITypeService>().GetType(attributesInfo.TypeId).AttributeInfo.FirstOrDefault((AttributeInfo a) => a.Id == attributesInfo.Id).AttributeValueInfo;
                    TypeAttributesModel typeAttributesModel = new TypeAttributesModel()
                    {
                        Name = attributesInfo.Name,
                        AttrId = productAttributeInfo.AttributeId,
                        Selected = "",
                        IsMulti = attributesInfo.IsMulti,
                        AttrValues = new List<TypeAttrValue>()
                    };
                    TypeAttributesModel typeAttributesModel1 = typeAttributesModel;
                    AttributeValueInfo[] attributeValueInfoArray = attributeValueInfo.ToArray();
                    for (int j = 0; j < attributeValueInfoArray.Length; j++)
                    {
                        AttributeValueInfo attributeValueInfo1 = attributeValueInfoArray[j];
                        List<TypeAttrValue> attrValues = typeAttributesModel1.AttrValues;
                        TypeAttrValue typeAttrValue = new TypeAttrValue()
                        {
                            Id = attributeValueInfo1.Id.ToString(),
                            Name = attributeValueInfo1.Value
                        };
                        attrValues.Add(typeAttrValue);
                    }

                    typeAttributesModels.Add(typeAttributesModel1);
                }
            }

            return Json(new { json = typeAttributesModels }, JsonRequestBehavior.AllowGet);
        }
        /*验证产品货号是否存在，编辑的时候不验证 始终返回false*/
        [HttpPost]
        public JsonResult IsExitsProductCode(string productCode, string isedit = "0", long shopId = 0)
        {
            bool result = false;
            if (isedit == "0")
            {
                result = ServiceHelper.Create<IProductService>().IsExitsProductCode(productCode, base.CurrentSellerManager.ShopId);
            }
            return Json(new { success = result });
        }
        [UnAuthorize]
        public JsonResult GetAuthorizationCategory()
        {
            List<long> nums = new List<long>();
            ICategoryService categoryService = ServiceHelper.Create<ICategoryService>();
            IQueryable<CategoryInfo> businessCategory = ServiceHelper.Create<IShopCategoryService>().GetBusinessCategory(shopId);
            List<CategoryJsonModel> categoryJsonModels = new List<CategoryJsonModel>();
            foreach (CategoryInfo categoryInfo in businessCategory)
            {
                string[] strArrays = categoryInfo.Path.Split(new char[] { '|' });
                long num = long.Parse(strArrays.Length > 0 ? strArrays[0] : "0");
                long num1 = long.Parse(strArrays.Length > 1 ? strArrays[1] : "0");
                CategoryInfo category = categoryService.GetCategory(num);
                CategoryJsonModel categoryJsonModel = new CategoryJsonModel()
                {
                    Name = category.Name,
                    Id = category.Id.ToString(),
                    SubCategory = new List<SecondLevelCategory>()
                };
                CategoryJsonModel categoryJsonModel1 = categoryJsonModel;
                SecondLevelCategory secondLevelCategory = null;
                ThirdLevelCategoty thirdLevelCategoty = null;
                CategoryInfo category1 = categoryService.GetCategory(num1);
                if (category1 != null)
                {
                    SecondLevelCategory secondLevelCategory1 = new SecondLevelCategory()
                    {
                        Name = category1.Name,
                        Id = category1.Id.ToString(),
                        SubCategory = new List<ThirdLevelCategoty>()
                    };
                    secondLevelCategory = secondLevelCategory1;
                    if (strArrays.Length >= 3)
                    {
                        ThirdLevelCategoty thirdLevelCategoty1 = new ThirdLevelCategoty()
                        {
                            Id = categoryInfo.Id.ToString(),
                            Name = categoryInfo.Name
                        };
                        thirdLevelCategoty = thirdLevelCategoty1;
                    }
                    if (thirdLevelCategoty != null)
                    {
                        secondLevelCategory.SubCategory.Add(thirdLevelCategoty);
                    }
                }
                CategoryJsonModel categoryJsonModel2 = categoryJsonModels.FirstOrDefault((CategoryJsonModel j) => j.Id == category.Id.ToString());
                if (categoryJsonModel2 != null && category1 != null && categoryJsonModel2.SubCategory.Any((SecondLevelCategory j) => j.Id == category1.Id.ToString()))
                {
                    categoryJsonModel2.SubCategory.FirstOrDefault((SecondLevelCategory j) => j.Id == category1.Id.ToString()).SubCategory.Add(thirdLevelCategoty);
                }
                else if (categoryJsonModel2 != null && secondLevelCategory != null)
                {
                    categoryJsonModel2.SubCategory.Add(secondLevelCategory);
                }
                else if (secondLevelCategory != null)
                {
                    categoryJsonModel1.SubCategory.Add(secondLevelCategory);
                }
                if (categoryJsonModels.FirstOrDefault((CategoryJsonModel j) => j.Id == category.Id.ToString()) != null)
                {
                    continue;
                }
                categoryJsonModels.Add(categoryJsonModel1);
            }
            return Json(new { json = categoryJsonModels }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFreightTemplate()
        {
            IQueryable<FreightTemplateInfo> shopFreightTemplate = ServiceHelper.Create<IFreightTemplateService>().GetShopFreightTemplate(base.CurrentSellerManager.ShopId);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Selected = false,
                Text = "请选择运费模板...",
                Value = "0"
            };
            selectListItems.Add(selectListItem);
            List<SelectListItem> selectListItems1 = selectListItems;
            foreach (FreightTemplateInfo freightTemplateInfo in shopFreightTemplate)
            {
                SelectListItem selectListItem1 = new SelectListItem()
                {
                    Text = string.Concat(freightTemplateInfo.Name, "【", freightTemplateInfo.ValuationMethod.ToDescription(), "】"),
                    Value = freightTemplateInfo.Id.ToString()
                };
                selectListItems1.Add(selectListItem1);
            }
            return Json(new { success = true, model = selectListItems1 });
        }

        private List<TypeAttributesModel> GetPlateformAttr(long categoryId)
        {
            CategoryInfo category = ServiceHelper.Create<ICategoryService>().GetCategory(categoryId);
            ProductTypeInfo type = ServiceHelper.Create<ITypeService>().GetType(category.TypeId);
            List<TypeAttributesModel> typeAttributesModels = new List<TypeAttributesModel>();
            foreach (AttributeInfo attributeInfo in type.AttributeInfo)
            {
                TypeAttributesModel typeAttributesModel = new TypeAttributesModel()
                {
                    Name = attributeInfo.Name,
                    AttrId = attributeInfo.Id,
                    Selected = "",
                    IsMulti = attributeInfo.IsMulti,
                    AttrValues = new List<TypeAttrValue>()
                };
                TypeAttributesModel typeAttributesModel1 = typeAttributesModel;
                foreach (AttributeValueInfo attributeValueInfo in attributeInfo.AttributeValueInfo)
                {
                    List<TypeAttrValue> attrValues = typeAttributesModel1.AttrValues;
                    TypeAttrValue typeAttrValue = new TypeAttrValue()
                    {
                        Id = attributeValueInfo.Id.ToString(),
                        Name = attributeValueInfo.Value
                    };
                    attrValues.Add(typeAttrValue);
                }
                typeAttributesModels.Add(typeAttributesModel1);
            }
            return typeAttributesModels;
        }

        private SpecJosnModel GetPlatformSpec(long categoryId, long productId = 0L)
        {
            SpecJosnModel specJosnModel = new SpecJosnModel()
            {
                json = new List<TypeSpecificationModel>()
            };
            tableDataModel _tableDataModel = new tableDataModel()
            {
                cost = new List<SKUSpecModel>(),
                mallPrice = new List<SKUSpecModel>(),
                productId = productId,
                sku = new List<SKUSpecModel>(),
                stock = new List<SKUSpecModel>()
            };
            specJosnModel.tableData = _tableDataModel;
            SpecJosnModel specJosnModel1 = specJosnModel;
            CategoryInfo category = ServiceHelper.Create<ICategoryService>().GetCategory(categoryId);
            ProductTypeInfo type = ServiceHelper.Create<ITypeService>().GetType(category.TypeId);
            foreach (object value in Enum.GetValues(typeof(SpecificationType)))
            {
                TypeSpecificationModel typeSpecificationModel = new TypeSpecificationModel()
                {
                    Name = Enum.GetNames(typeof(SpecificationType))[(int)value - 1],
                    Values = new List<Specification>(),
                    SpecId = (int)value
                };
                TypeSpecificationModel typeSpecificationModel1 = typeSpecificationModel;
                foreach (SpecificationValueInfo specificationValueInfo in
                    from s in type.SpecificationValueInfo
                    where (int)s.Specification == (int)value
                    orderby s.Value
                    select s)
                {
                    List<Specification> values = typeSpecificationModel1.Values;
                    Specification specification = new Specification()
                    {
                        Id = specificationValueInfo.Id.ToString(),
                        Name = specificationValueInfo.Value,
                        isPlatform = true,
                        Selected = false
                    };
                    values.Add(specification);
                }
                specJosnModel1.json.Add(typeSpecificationModel1);
            }
            IOrderedQueryable<SKUInfo> sKUs =
                from s in ServiceHelper.Create<IProductService>().GetSKUs(productId)
                orderby s.Color, s.Size, s.Version
                select s;
            InitialTableData(sKUs, specJosnModel1);
            return specJosnModel1;
        }

        private List<SKUInfo> GetProducrSpec(IQueryable<SKUInfo> skuList)
        {
            List<SKUInfo> sKUInfos = new List<SKUInfo>();
            foreach (SKUInfo sKUInfo in skuList)
            {
                string[] strArrays = (string.IsNullOrWhiteSpace(sKUInfo.Id) ? new string[] { "" } : sKUInfo.Id.Split(new char[] { '\u005F' }));
                List<SKUInfo> sKUInfos1 = sKUInfos;
                SKUInfo sKUInfo1 = new SKUInfo()
                {
                    Color = strArrays.Length >= 2 ? strArrays[1] : "",
                    Size = strArrays.Length >= 3 ? strArrays[2] : "",
                    Version = strArrays.Length >= 4 ? strArrays[3] : "",
                    Id = sKUInfo.Id
                };
                sKUInfos1.Add(sKUInfo1);
            }
            return sKUInfos;
        }

        public string GetQrCodeImagePath(long productId)
        {
            object[] authority = new object[] { "http://", base.HttpContext.Request.Url.Authority, "/m-wap/product/detail/", productId };
            Bitmap bitmap = QRCodeHelper.Create(string.Concat(authority));
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Gif);
            string str = string.Concat("data:image/gif;base64,", Convert.ToBase64String(memoryStream.ToArray()));
            return str;
        }

        [UnAuthorize]
        private List<CategoryJsonModel> GetShopCategoryJson(long shopId)
        {
            ShopCategoryInfo[] array = ServiceHelper.Create<IShopCategoryService>().GetShopCategory(shopId).ToArray();
            List<CategoryJsonModel> categoryJsonModels = new List<CategoryJsonModel>();
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
                categoryJsonModels.Add(categoryJsonModel1);
            }
            return categoryJsonModels;
        }

        [UnAuthorize]
        public JsonResult GetShopProductCategory(long productId = 0L)
        {
            ShopProductCategoryModel shopProductCategoryModel = new ShopProductCategoryModel()
            {
                SelectedCategory = new List<SelectedCategory>(),
                Data = GetShopCategoryJson(shopId)
            };
            if (0 != productId)
            {
                IQueryable<ProductShopCategoryInfo> productShopCategories = ServiceHelper.Create<IProductService>().GetProductShopCategories(productId);
                foreach (CategoryJsonModel datum in shopProductCategoryModel.Data)
                {
                    long num = long.Parse(datum.Id);
                    if (productShopCategories.Any((ProductShopCategoryInfo c) => c.ShopCategoryId == num))
                    {
                        List<SelectedCategory> selectedCategory = shopProductCategoryModel.SelectedCategory;
                        SelectedCategory selectedCategory1 = new SelectedCategory()
                        {
                            Id = datum.Id,
                            Level = "1"
                        };
                        selectedCategory.Add(selectedCategory1);
                    }
                    foreach (SecondLevelCategory subCategory in datum.SubCategory)
                    {
                        num = long.Parse(subCategory.Id);
                        if (!productShopCategories.Any((ProductShopCategoryInfo c) => c.ShopCategoryId == num))
                        {
                            continue;
                        }
                        List<SelectedCategory> selectedCategories = shopProductCategoryModel.SelectedCategory;
                        SelectedCategory selectedCategory2 = new SelectedCategory()
                        {
                            Id = subCategory.Id,
                            Level = "2"
                        };
                        selectedCategories.Add(selectedCategory2);
                    }
                }
            }
            return Json(new { json = shopProductCategoryModel }, JsonRequestBehavior.AllowGet);
        }

        [UnAuthorize]
        public JsonResult GetSpecifications(long categoryId, long productId = 0L, long isCategoryId = 0L)
        {
            Dictionary<long, string> nums = new Dictionary<long, string>();
            CategoryInfo category = ServiceHelper.Create<ICategoryService>().GetCategory(categoryId);
            SpecJosnModel platformSpec = GetPlatformSpec(categoryId, productId);
            IQueryable<SellerSpecificationValueInfo> sellerSpecifications = ServiceHelper.Create<IProductService>().GetSellerSpecifications(shopId, category.TypeId);
            IQueryable<SKUInfo> sKUs = ServiceHelper.Create<IProductService>().GetSKUs(productId);
            List<SKUInfo> producrSpec = GetProducrSpec(sKUs);
            foreach (TypeSpecificationModel typeSpecificationModel in platformSpec.json)
            {
                IQueryable<SellerSpecificationValueInfo> specification =
                    from s in sellerSpecifications
                    where (int)s.Specification == typeSpecificationModel.SpecId
                    select s;
                Specification value = null;
                foreach (SellerSpecificationValueInfo sellerSpecificationValueInfo in specification)
                {
                    value = typeSpecificationModel.Values.FirstOrDefault((Specification s) => s.Id == sellerSpecificationValueInfo.ValueId.ToString());
                    if (value != null && value.Id == sellerSpecificationValueInfo.ValueId.ToString())
                    {
                        value.Name = sellerSpecificationValueInfo.Value;
                        value.isPlatform = false;
                    }
                    if (!sKUs.Any((SKUInfo s) => s.Color.Equals(sellerSpecificationValueInfo.Value)))
                    {
                        if (!sKUs.Any((SKUInfo s) => s.Size.Equals(sellerSpecificationValueInfo.Value)))
                        {
                            if (!sKUs.Any((SKUInfo s) => s.Version.Equals(sellerSpecificationValueInfo.Value)))
                            {
                                continue;
                            }
                        }
                    }
                    value.Selected = true;
                }
                foreach (Specification color in typeSpecificationModel.Values)
                {
                    if (typeSpecificationModel.Name == "Color")
                    {
                        if (producrSpec.Any((SKUInfo s) => color.Id == s.Color))
                        {
                            SKUInfo sKUInfo = producrSpec.FirstOrDefault((SKUInfo s) => color.Id == s.Color);
                            color.Name = sKUs.FirstOrDefault((SKUInfo p) => p.Id == sKUInfo.Id).Color;
                            color.isPlatform = false;
                            color.Selected = true;
                        }
                    }
                    if (typeSpecificationModel.Name == "Size")
                    {
                        if (producrSpec.Any((SKUInfo s) => color.Id == s.Size))
                        {
                            SKUInfo sKUInfo1 = producrSpec.FirstOrDefault((SKUInfo s) => color.Id == s.Size);
                            color.Name = sKUs.FirstOrDefault((SKUInfo p) => p.Id == sKUInfo1.Id).Size;
                            color.isPlatform = false;
                            color.Selected = true;
                        }
                    }
                    if (typeSpecificationModel.Name != "Version")
                    {
                        continue;
                    }
                    if (!producrSpec.Any((SKUInfo s) => color.Id == s.Version))
                    {
                        continue;
                    }
                    SKUInfo sKUInfo2 = producrSpec.FirstOrDefault((SKUInfo s) => color.Id == s.Version);
                    color.Name = sKUs.FirstOrDefault((SKUInfo p) => p.Id == sKUInfo2.Id).Version;
                    color.isPlatform = false;
                    color.Selected = true;
                }
            }
            return Json(new { data = platformSpec }, JsonRequestBehavior.AllowGet);
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
                        System.IO.File.Copy(str3, Path.Combine(mapPath, Path.GetFileName(str3)), true);
                    }
                    Directory.Delete(mapPath1, true);
                }
            }
            return content;
        }

        private void InitialTableData(IQueryable<SKUInfo> skus, SpecJosnModel data)
        {
            if (skus.Count() == 0)
            {
                return;
            }
            int num = 0;
            string version = "";
            SKUInfo[] array = skus.ToArray();
            if (!string.IsNullOrWhiteSpace(array[0].Version))
            {
                num = 2;
                version = array[0].Version;
            }
            if (!string.IsNullOrWhiteSpace(array[0].Size))
            {
                num = 1;
                version = array[0].Size;
            }
            if (!string.IsNullOrWhiteSpace(array[0].Color))
            {
                num = 0;
                version = array[0].Color;
            }
            if (string.IsNullOrWhiteSpace(version))
            {
                return;
            }
            SKUSpecModel sKUSpecModel = new SKUSpecModel()
            {
                ValueSet = new List<string>()
            };
            SKUSpecModel sKUSpecModel1 = new SKUSpecModel()
            {
                ValueSet = new List<string>()
            };
            SKUSpecModel sKUSpecModel2 = new SKUSpecModel()
            {
                ValueSet = new List<string>()
            };
            SKUSpecModel sKUSpecModel3 = new SKUSpecModel()
            {
                ValueSet = new List<string>()
            };
            foreach (SKUInfo sku in skus)
            {
                string color = "";
                switch (num)
                {
                    case 0:
                        {
                            color = sku.Color;
                            break;
                        }
                    case 1:
                        {
                            color = sku.Size;
                            break;
                        }
                    case 2:
                        {
                            color = sku.Version;
                            break;
                        }
                }
                if (!color.Equals(version))
                {
                    data.tableData.cost.Add(DeepClone(sKUSpecModel));
                    data.tableData.stock.Add(DeepClone(sKUSpecModel1));
                    data.tableData.sku.Add(DeepClone(sKUSpecModel2));
                    data.tableData.mallPrice.Add(DeepClone(sKUSpecModel3));
                    sKUSpecModel = new SKUSpecModel()
                    {
                        ValueSet = new List<string>()
                    };
                    sKUSpecModel1 = new SKUSpecModel()
                    {
                        ValueSet = new List<string>()
                    };
                    sKUSpecModel2 = new SKUSpecModel()
                    {
                        ValueSet = new List<string>()
                    };
                    sKUSpecModel3 = new SKUSpecModel()
                    {
                        ValueSet = new List<string>()
                    };
                    sKUSpecModel.ValueSet.Add((sku.CostPrice == new decimal(0) ? "" : sku.CostPrice.ToString("f2")));
                    sKUSpecModel.index = color;
                    sKUSpecModel1.ValueSet.Add((sku.Stock == 0 ? "" : sku.Stock.ToString("f2")));
                    sKUSpecModel1.index = color;
                    sKUSpecModel2.ValueSet.Add(sku.Sku);
                    sKUSpecModel2.index = color;
                    sKUSpecModel3.ValueSet.Add((sku.SalePrice == new decimal(0) ? "" : sku.SalePrice.ToString("f2")));
                    sKUSpecModel3.index = color;
                    version = color;
                }
                else
                {
                    sKUSpecModel.ValueSet.Add((sku.CostPrice == new decimal(0) ? "" : sku.CostPrice.ToString("f2")));
                    sKUSpecModel.index = color;
                    sKUSpecModel1.ValueSet.Add((sku.Stock == 0 ? "" : sku.Stock.ToString("f2")));
                    sKUSpecModel1.index = color;
                    sKUSpecModel2.ValueSet.Add(sku.Sku);
                    sKUSpecModel2.index = color;
                    sKUSpecModel3.ValueSet.Add((sku.SalePrice == new decimal(0) ? "" : sku.SalePrice.ToString("f2")));
                    sKUSpecModel3.index = color;
                }
            }
            data.tableData.cost.Add(DeepClone(sKUSpecModel));
            data.tableData.stock.Add(DeepClone(sKUSpecModel1));
            data.tableData.sku.Add(DeepClone(sKUSpecModel2));
            data.tableData.mallPrice.Add(DeepClone(sKUSpecModel3));
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult List(long? categoryId, string productCode, string brandName, int? productAuthor,
            int? saleStatus, string ids, int page, int rows, string keyWords, string ename,
            DateTime? startDate, DateTime? endDate, string CASNo, string Volume = "")
        {
            ProductInfo.ProductSaleStatus? nullable;
            IEnumerable<long> nums;
            SellerProductQuery productQuery = new SellerProductQuery();
            SellerProductQuery productQuery1 = productQuery;
            int? nullable1 = saleStatus;
            if (nullable1.HasValue)
            {
                nullable = new ProductInfo.ProductSaleStatus?((ProductInfo.ProductSaleStatus)nullable1.GetValueOrDefault());
            }
            else
            {
                nullable = null;
            }
            productQuery1.SaleStatus = nullable;
            productQuery1.CASNo = CASNo;
            productQuery.PageSize = rows;
            productQuery.PageNo = page;
            productQuery.BrandNameKeyword = brandName;
            productQuery.KeyWords = keyWords;
            productQuery.engname = ename;
            productQuery.ShopCategoryId = categoryId;
            SellerProductQuery productQuery2 = productQuery;
            if (string.IsNullOrWhiteSpace(ids))
            {
                nums = null;
            }
            else
            {
                char[] chrArray = new char[] { ',' };
                nums =
                    from item in ids.Split(chrArray)
                    select long.Parse(item);
            }
            productQuery2.Ids = nums;
            productQuery.ShopId = new long?(base.CurrentSellerManager.ShopId);
            productQuery.StartDate = startDate;
            productQuery.EndDate = endDate;
            productQuery.ProductCode = productCode;
            productQuery.CASNo = CASNo;
            productQuery.Volume = Volume;
            SellerProductQuery value = productQuery;
            if (productAuthor.HasValue)
            {
                value.productAuthor = productAuthor;
            }
            if (saleStatus.HasValue)
            {
                int? nullable2 = saleStatus;
                value.SaleStatus = (ProductInfo.ProductSaleStatus)saleStatus.Value;

            }

            PageModel<ProductInfo> products = ServiceHelper.Create<IProductService>().GetSellerProducts(value);
            IShopCategoryService shopCategoryService = ServiceHelper.Create<IShopCategoryService>();
            IBrandService brandService = ServiceHelper.Create<IBrandService>();
            IEnumerable<ProductEXModel> list = (
                from item in products.Models.ToArray()
                select new ProductEXModel()
                {
                    Name = item.ProductName,
                    EProductName = item.EProductName,
                    CASNo = item.CASNo,
                    Purity = item.Purity,
                    PackagingLevel = item.PackagingLevel,
                    MolecularFormula = item.MolecularFormula,
                    Id = item.Id,
                    Image = item.GetImage(ProductInfo.ImageSize.Size_50, 1),
                    Price = item.Price,
                    Url = "",
                    PublishTime = item.AddedDate.ToString("yyyy-MM-dd HH:mm"),
                    SaleState = (int)item.SaleStatus,
                    CategoryId = item.CategoryId,
                    BrandId = item.BrandId,
                    AuditState = (int)item.AuditStatus,
                    AuditReason = (item.ProductDescriptionInfo != null ? item.ProductDescriptionInfo.AuditReason : ""),
                    ProductCode = item.ProductCode,
                    QrCode = GetQrCodeImagePath(item.Id),
                    SaleCount = item.SaleCounts,
                    Unit = item.MeasureUnit,
                    ProductAuth = item.AuthStatus,
                    Uid = base.CurrentSellerManager.Id
                }).ToList();
            IEnumerable<ProductEXModel> productModels = list.Select<ProductEXModel, ProductEXModel>((ProductEXModel item) =>
            {
                BrandInfo brand = brandService.GetBrand(item.BrandId);
                ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
                ShopCategoryInfo categoryByProductId = shopCategoryService.GetCategoryByProductId(item.Id);
                return new ProductEXModel()
                {
                    Name = item.Name,
                    EProductName = item.EProductName,
                    CASNo = item.CASNo,
                    Purity = item.Purity,
                    PackagingLevel = item.PackagingLevel,
                    MolecularFormula = item.MolecularFormula,
                    Id = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    Url = "",
                    PublishTime = item.PublishTime,
                    SaleState = item.SaleState,
                    AuditState = item.AuditState,
                    AuditReason = (item.AuditReason != null ? item.AuditReason : ""),
                    ProductCode = item.ProductCode,
                    QrCode = GetQrCodeImagePath(item.Id),
                    SaleCount = item.SaleCount,
                    Unit = item.Unit,
                    ProductAuth = item.ProductAuth,
                    Uid = CurrentSellerManager.Id,
                    CategoryName = (categoryByProductId == null ? "" : categoryByProductId.Name),
                    BrandName = (item.BrandId == 0 || brand == null ? "" : brand.Name),
                    ShopStatus = (int)shop.ShopStatus
                };
            }).ToList();
            DataGridModel<ProductEXModel> dataGridModel = new DataGridModel<ProductEXModel>()
            {
                rows = productModels,
                total = products.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult Management()
        {
            int produtAuditOnOff = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings().ProdutAuditOnOff;
            ViewBag.AuditOnOff = produtAuditOnOff;
            return View();
        }

        public ActionResult SpecialOfferList()
        {
            int produtAuditOnOff = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings().ProdutAuditOnOff;
            ViewBag.AuditOnOff = produtAuditOnOff;
            return View();
        }

        public ActionResult ProApplyList()
        {
            int produtAuditOnOff = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings().ProdutAuditOnOff;
            ViewBag.AuditOnOff = produtAuditOnOff;
            return View();
        }

        public ActionResult Print(long Id)
        {
            ProductInfo productInfo = ServiceHelper.Create<IProductService>().GetProduct(Id);

            ShopInfo _shopinfo = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId);
            if (_shopinfo != null)
            {
                ViewBag.shopName = _shopinfo.ShopName;
                ViewBag.userName = _shopinfo.ContactsName;
                ViewBag.tel = _shopinfo.ContactsPhone;
                ViewBag.fax = _shopinfo.Fax;
                ViewBag.phone = _shopinfo.ContactsPhone;
                ViewBag.email = _shopinfo.ContactsEmail;
                ViewBag.address = _shopinfo.CompanyAddress;
            }
            else
            {
                ViewBag.shopName = "";
                ViewBag.userName = "";
                ViewBag.tel = "";
                ViewBag.fax = "";
                ViewBag.phone = "";
                ViewBag.email = "";
                ViewBag.address = "";
            }
            return View(productInfo);
        }
        private void ProcessSKU(ProductDetailModel m, ProductInfo result)
        {
            if (m.specificationsValue == null || m.specificationsValue.Count == 0)
            {
                m.specificationsValue = new List<SpecificationsValue>();
                List<SpecificationsValue> specificationsValues = m.specificationsValue;
                SpecificationsValue specificationsValue = new SpecificationsValue()
                {
                    colorId = "",
                    colorName = "",
                    sizeId = "",
                    sizeName = "",
                    versionId = "",
                    versionName = "",
                    cost = m.cost,
                    mallPrice = m.mallPrce,
                    sku = "",
                    stock = m.stock.ToString()
                };
                specificationsValues.Add(specificationsValue);
            }
            foreach (SpecificationsValue specificationsValue1 in m.specificationsValue)
            {
                decimal num = new decimal(0);
                decimal num1 = new decimal(0);
                decimal num2 = new decimal(0);
                decimal.TryParse(specificationsValue1.cost, out num);
                decimal.TryParse(specificationsValue1.stock, out num1);
                decimal.TryParse(specificationsValue1.mallPrice, out num2);
                ICollection<SKUInfo> sKUInfo = result.SKUInfo;
                SKUInfo sKUInfo1 = new SKUInfo()
                {
                    Color = specificationsValue1.colorName,
                    Size = specificationsValue1.sizeName,
                    Version = specificationsValue1.versionName,
                    CostPrice = num,
                    ProductId = result.Id,
                    SalePrice = num2,
                    Sku = specificationsValue1.sku.ToString(),
                    Stock = (long)num1
                };
                SKUInfo sKUInfo2 = sKUInfo1;
                object[] id = new object[] { result.Id, null, null, null };
                id[1] = (string.IsNullOrWhiteSpace(specificationsValue1.colorId) ? "0" : specificationsValue1.colorId);
                id[2] = (string.IsNullOrWhiteSpace(specificationsValue1.sizeId) ? "0" : specificationsValue1.sizeId);
                id[3] = (string.IsNullOrWhiteSpace(specificationsValue1.versionId) ? "0" : specificationsValue1.versionId);
                sKUInfo2.Id = string.Format("{0}_{1}_{2}_{3}", id);
                sKUInfo.Add(sKUInfo1);
            }
        }

        public ActionResult PublicStepOne()
        {
            return View();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="categoryNames"></param>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [UnAuthorize]
        public ActionResult PublicStepTwo(string categoryNames = "", long categoryId = 0L, long productId = 0L)
        {
            long num;
            long num1;

            IProductService productService = ServiceHelper.Create<IProductService>();
            ICategoryService categoryService = ServiceHelper.Create<ICategoryService>();
            string str2 = "0";
            string str3 = "0";
            string str4 = "0";
            ProductInfo productInfo = new ProductInfo();
            if (productId != 0)
            {
                productInfo = productService.GetProductSpec(productId);
                if (productInfo == null || productInfo.ShopId != base.CurrentSellerManager.ShopId)
                {
                    throw new HimallException(string.Concat(productId, ",该商品已删除或者不属于该供应商"));
                }

                if (categoryId == 0)
                {
                    categoryId = productInfo.CategoryId;
                }
            }
            ServiceHelper.Create<ITypeService>().GetType(categoryId);
            IBrandService brandService = ServiceHelper.Create<IBrandService>();
            long num5 = shopId;
            long[] numArray = new long[] { categoryId };
            IEnumerable<BrandInfo> brandsByCategoryIds = brandService.GetBrandsByCategoryIds(num5, numArray);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Selected = false,
                Text = "请选择品牌...",
                Value = "0"
            };
            selectListItems.Add(selectListItem);
            List<SelectListItem> selectListItems1 = selectListItems;
            foreach (BrandInfo brandsByCategoryId in brandsByCategoryIds)
            {
                List<SelectListItem> selectListItems2 = selectListItems1;
                SelectListItem selectListItem1 = new SelectListItem()
                {
                    Selected = (productId == 0 || productInfo == null ? false : productInfo.BrandId == brandsByCategoryId.Id),
                    Text = brandsByCategoryId.Name,
                    Value = brandsByCategoryId.Id.ToString()
                };
                selectListItems2.Add(selectListItem1);
            }
            IQueryable<FreightTemplateInfo> shopFreightTemplate = ServiceHelper.Create<IFreightTemplateService>().GetShopFreightTemplate(base.CurrentSellerManager.ShopId);
            List<SelectListItem> selectListItems3 = new List<SelectListItem>();
            SelectListItem selectListItem2 = new SelectListItem()
            {
                Selected = false,
                Text = "请选择运费模板...",
                Value = "0"
            };
            selectListItems3.Add(selectListItem2);
            List<SelectListItem> selectListItems4 = selectListItems3;
            foreach (FreightTemplateInfo freightTemplateInfo in shopFreightTemplate)
            {
                List<SelectListItem> selectListItems5 = selectListItems4;
                SelectListItem selectListItem3 = new SelectListItem()
                {
                    Selected = (productId == 0 || productInfo == null ? false : productInfo.FreightTemplateId == freightTemplateInfo.Id),
                    Text = string.Concat(freightTemplateInfo.Name, "【", freightTemplateInfo.ValuationMethod.ToDescription(), "】"),
                    Value = freightTemplateInfo.Id.ToString()
                };
                selectListItems5.Add(selectListItem3);
            }
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
            ViewBag.FreightTemplates = selectListItems4;
            ViewBag.BrandDrop = selectListItems1;
            dynamic viewBag = base.ViewBag;
            num = 0 == productInfo.Id ? 0 : 0;
            viewBag.TopId = num;
            dynamic obj = base.ViewBag;
            num1 = 0 == productInfo.Id ? 0 : 0;
            obj.BottomId = num1;
            ViewBag.CategoryNames = categoryNames;
            ViewBag.CategoryId = categoryId;

            string cateforypath = productInfo.CategoryPath;
            string categorylevel1 = "";
            string categorylevel2 = "";
            string categorylevel3 = "";
            if (!string.IsNullOrWhiteSpace(cateforypath))
            {
                string[] arry = cateforypath.Split('|');
                if (arry.Length == 3)
                {
                    if (!string.IsNullOrWhiteSpace(arry[0])) { categorylevel1 = arry[0]; }
                    if (!string.IsNullOrWhiteSpace(arry[1])) { categorylevel2 = arry[1]; }
                    if (!string.IsNullOrWhiteSpace(arry[2])) { categorylevel3 = arry[2]; }
                }
            }
            ViewBag.CoinDic = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(1);
            ViewBag.categorylevel1 = categorylevel1;
            ViewBag.categorylevel2 = categorylevel2;
            ViewBag.categorylevel3 = categorylevel3;

            ViewBag.CategoryId = categoryId;

            ViewBag.ProductId = productId;
            base.ViewBag.IsCategory = (productId == 0 ? 1 : 0);
            ViewBag.ShopId = shopId;
            ViewBag.SalePrice = str2;
            ViewBag.Stock = str3;
            ViewBag.CostPrice = str4;
            ViewBag.ShopStatus = shop.ShopStatus;
            DataTable dtseo = DbHelperSQL.QueryDataTable(string.Format("SELECT *  FROM ChemCloud_CasSeo where ProductId={0} AND Pub_CID={1}", productInfo.Id, productInfo.Pub_CID));
            if (dtseo != null && dtseo.Rows.Count > 0)
            {
                ViewBag.Meta_Title = dtseo.Rows[0]["Meta_Title"];
                ViewBag.Meta_Description = dtseo.Rows[0]["Meta_Description"];
                ViewBag.Meta_Keywords = dtseo.Rows[0]["Meta_Keywords"];
            }
            else
            {
                ViewBag.Meta_Title = "";
                ViewBag.Meta_Description = "";
                ViewBag.Meta_Keywords = "";

            }
            return View(productInfo);
        }

        public JsonResult getProductLevel()
        {
            return Json(ServiceHelper.Create<IProductLevelService>().GetProductLevel());
        }

        /// <summary>  
        /// 加载数据
        /// </summary>
        /// <param name="categoryNames"></param>`
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [UnAuthorize]
        public ActionResult PublicStepThree(string categoryNames = "", long categoryId = 0L, long productId = 0L)
        {

            string str;
            string str1;
            IProductService productService = ServiceHelper.Create<IProductService>();
            ICategoryService categoryService = ServiceHelper.Create<ICategoryService>();
            string str2 = "0";
            string str3 = "0";
            string str4 = "0";
            ProductInfo productInfo = new ProductInfo();
            if (productId != 0)
            {
                productInfo = productService.GetProductSpec(productId);
                if (productInfo == null || productInfo.ShopId != base.CurrentSellerManager.ShopId)
                {
                    throw new HimallException(string.Concat(productId, ",该商品已删除或者不属于该店铺"));
                }
                if (productInfo.SKUInfo.Count() > 0)
                {
                    IEnumerable<SKUInfo> sKUInfo =
                        from s in productInfo.SKUInfo
                        where s.SalePrice > new decimal(0)
                        select s;
                    IEnumerable<SKUInfo> sKUInfos =
                        from s in productInfo.SKUInfo
                        where s.CostPrice > new decimal(0)
                        select s;
                    if (sKUInfo.Count() == 0)
                    {
                        str = productInfo.MinSalePrice.ToString("f3");
                    }
                    else
                    {
                        decimal num2 = sKUInfo.Min<SKUInfo>((SKUInfo s) => s.SalePrice);
                        str = num2.ToString();
                    }
                    str2 = str;
                    long num3 = productInfo.SKUInfo.Sum<SKUInfo>((SKUInfo s) => s.Stock);
                    str3 = num3.ToString();
                    if (sKUInfos.Count() == 0)
                    {
                        str1 = str4;
                    }
                    else
                    {
                        decimal num4 = sKUInfos.Min<SKUInfo>((SKUInfo s) => s.CostPrice);
                        str1 = num4.ToString();
                    }
                    str4 = str1;
                }
                if (string.IsNullOrWhiteSpace(categoryNames))
                {
                    string[] strArrays = productInfo.CategoryPath.Split(new char[] { '|' });
                    for (int i = 0; i < strArrays.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(strArrays[i]))
                        {
                            CategoryInfo category = categoryService.GetCategory(long.Parse(strArrays[i]));
                            categoryNames = string.Concat(categoryNames, string.Format("{0} {1} ", (category == null ? "" : category.Name), (i == strArrays.Length - 1 ? "" : " > ")));
                        }
                    }
                }
                if (categoryId == 0)
                {
                    categoryId = productInfo.CategoryId;
                }
            }

            ServiceHelper.Create<ITypeService>().GetType(categoryId);
            IBrandService brandService = ServiceHelper.Create<IBrandService>();
            long num5 = shopId;
            long[] numArray = new long[] { categoryId };
            IEnumerable<BrandInfo> brandsByCategoryIds = brandService.GetBrandsByCategoryIds(num5, numArray);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Selected = false,
                Text = "请选择品牌...",
                Value = "0"
            };
            selectListItems.Add(selectListItem);
            List<SelectListItem> selectListItems1 = selectListItems;
            foreach (BrandInfo brandsByCategoryId in brandsByCategoryIds)
            {
                List<SelectListItem> selectListItems2 = selectListItems1;
                SelectListItem selectListItem1 = new SelectListItem()
                {
                    Selected = (productId == 0 || productInfo == null ? false : productInfo.BrandId == brandsByCategoryId.Id),
                    Text = brandsByCategoryId.Name,
                    Value = brandsByCategoryId.Id.ToString()
                };
                selectListItems2.Add(selectListItem1);
            }
            IQueryable<FreightTemplateInfo> shopFreightTemplate = ServiceHelper.Create<IFreightTemplateService>().GetShopFreightTemplate(base.CurrentSellerManager.ShopId);
            List<SelectListItem> selectListItems3 = new List<SelectListItem>();
            SelectListItem selectListItem2 = new SelectListItem()
            {
                Selected = false,
                Text = "请选择运费模板...",
                Value = "0"
            };
            selectListItems3.Add(selectListItem2);
            List<SelectListItem> selectListItems4 = selectListItems3;
            foreach (FreightTemplateInfo freightTemplateInfo in shopFreightTemplate)
            {
                List<SelectListItem> selectListItems5 = selectListItems4;
                SelectListItem selectListItem3 = new SelectListItem()
                {
                    Selected = (productId == 0 || productInfo == null ? false : productInfo.FreightTemplateId == freightTemplateInfo.Id),
                    Text = string.Concat(freightTemplateInfo.Name, "【", freightTemplateInfo.ValuationMethod.ToDescription(), "】"),
                    Value = freightTemplateInfo.Id.ToString()
                };
                selectListItems5.Add(selectListItem3);
            }
            ViewBag.FreightTemplates = selectListItems4;
            ViewBag.BrandDrop = selectListItems1;
            dynamic viewBag = base.ViewBag;
            //num = 0 == productInfo.Id ? 0 : productInfo.ProductDescriptionInfo.DescriptionPrefixId;
            //viewBag.TopId = num;
            //dynamic obj = base.ViewBag;
            //num1 = 0 == productInfo.Id ? 0 : productInfo.ProductDescriptionInfo.DescriptiondSuffixId;
            //obj.BottomId = num1;
            ViewBag.CategoryNames = categoryNames;
            ViewBag.CategoryId = categoryId;

            string cateforypath = productInfo.CategoryPath;
            string categorylevel1 = "";
            string categorylevel2 = "";
            string categorylevel3 = "";
            if (!string.IsNullOrWhiteSpace(cateforypath))
            {
                string[] arry = cateforypath.Split('|');
                if (arry.Length == 3)
                {
                    if (!string.IsNullOrWhiteSpace(arry[0])) { categorylevel1 = arry[0]; }
                    if (!string.IsNullOrWhiteSpace(arry[1])) { categorylevel2 = arry[1]; }
                    if (!string.IsNullOrWhiteSpace(arry[2])) { categorylevel3 = arry[2]; }
                }
            }
            List<ChemCloud_Dictionaries> listDictionaries = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(1);
            ViewBag.CoinDic = listDictionaries;
            ViewBag.CoinDic2 = listDictionaries;

            ViewBag.categorylevel1 = categorylevel1;
            ViewBag.categorylevel2 = categorylevel2;
            ViewBag.categorylevel3 = categorylevel3;


            ViewBag.ProductId = productId;

            /*获取mol*/
            string mol = "";
            if (productInfo.Id != 0)
            {
                if (productInfo.ProductDescriptionInfo != null)
                {
                    if (!string.IsNullOrEmpty(productInfo.ProductDescriptionInfo.MobileDescription))
                    {
                        mol = productInfo.ProductDescriptionInfo.MobileDescription;

                        mol = mol.Replace('*', '\n');

                    }
                }
            }

            System.Text.StringBuilder input = new System.Text.StringBuilder();
            input.AppendLine("");
            input.AppendLine("  -OEChem-04191619132D");
            input.AppendLine("");
            input.AppendLine("  9  8  0     0  0  0  0  0  0999 V2000");
            input.AppendLine("    2.5369   -0.2500    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.4030    0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.2690   -0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.8015    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.0044    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.9590   -0.7869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.8059   -0.5600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.5790    0.2869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    2.0000    0.0600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("  1  2  1  0  0  0  0");
            input.AppendLine("  1  9  1  0  0  0  0");
            input.AppendLine("  2  3  1  0  0  0  0");
            input.AppendLine("  2  4  1  0  0  0  0");
            input.AppendLine("  2  5  1  0  0  0  0");
            input.AppendLine("  3  6  1  0  0  0  0");
            input.AppendLine("  3  7  1  0  0  0  0");
            input.AppendLine("  3  8  1  0  0  0  0");
            input.AppendLine("M  END");

            ViewBag.mol = mol;

            base.ViewBag.IsCategory = (productId == 0 ? 1 : 0);
            ViewBag.ShopId = shopId;
            ViewBag.SalePrice = str2;
            ViewBag.Stock = str3;
            ViewBag.CostPrice = str4;
            ViewBag.ISCASNo = productInfo.ISCASNo;
            DataTable dtseo = DbHelperSQL.QueryDataTable(string.Format("SELECT *  FROM ChemCloud_CasSeo where ProductId={0} AND Pub_CID={1}", productInfo.Id, productInfo.Pub_CID));
            if (dtseo != null && dtseo.Rows.Count > 0)
            {
                ViewBag.Meta_Title = dtseo.Rows[0]["Meta_Title"];
                ViewBag.Meta_Description = dtseo.Rows[0]["Meta_Description"];
                ViewBag.Meta_Keywords = dtseo.Rows[0]["Meta_Keywords"];
            }
            else
            {
                ViewBag.Meta_Title = "";
                ViewBag.Meta_Description = "";
                ViewBag.Meta_Keywords = "";

            }
            return View(productInfo);
        }

        public JsonResult Test()
        {

            StringBuilder input = new StringBuilder();
            input.AppendLine("");
            input.AppendLine("  -OEChem-04191619132D");
            input.AppendLine("");
            input.AppendLine("  9  8  0     0  0  0  0  0  0999 V2000");
            input.AppendLine("    2.5369   -0.2500    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.4030    0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.2690   -0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.8015    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.0044    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.9590   -0.7869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.8059   -0.5600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.5790    0.2869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    2.0000    0.0600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("  1  2  1  0  0  0  0");
            input.AppendLine("  1  9  1  0  0  0  0");
            input.AppendLine("  2  3  1  0  0  0  0");
            input.AppendLine("  2  4  1  0  0  0  0");
            input.AppendLine("  2  5  1  0  0  0  0");
            input.AppendLine("  3  6  1  0  0  0  0");
            input.AppendLine("  3  7  1  0  0  0  0");
            input.AppendLine("  3  8  1  0  0  0  0");
            input.AppendLine("M  END");
            Result res = new Result();
            res.msg = input.ToString();
            return Json(res);
        }

        [HttpPost]
        [UnAuthorize]
        [ValidateInput(false)]
        public JsonResult SaveProductDetail(string productDetail)
        {
            IProductService productService = ServiceHelper.Create<IProductService>();
            ICategoryService categoryService = ServiceHelper.Create<ICategoryService>();
            ProductDetailModel productDetailModel = JsonConvert.DeserializeObject<ProductDetailModel>(productDetail);
            long num = productDetailModel.productId;
            ProductInfo productInfo = ProductDetailModel.GetProductInfo(productDetailModel, num);
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);

            ProductInfo.ProductEditStatus editStatus = productService.GetEditStatus(num, productInfo);
            productInfo.EditStatus = (short)editStatus;

            int newId = 0;


            /*根据cas#查询chemcloud_cas表数据*/
            var Pub = ServiceHelper.Create<ICASInfoService>().GetCASByCASNO(productInfo.CASNo);
            if (Pub.CAS == null)
            {
                newId = DbHelperSQLCAS.GetMaxID("PUB_CID", "ChemCloud_CAS");
                if (newId >= 200000000)
                {
                    newId = newId + 1;
                }
                else
                {
                    newId = 200000000;
                }
                productInfo.Pub_CID = newId;
            }
            else
            {
                productInfo.Pub_CID = Pub.Pub_CID;
            }
            if (productDetailModel.productId != 0)
            {
                productInfo.ShopId = shopId;
                productInfo.ImagePath = "";
                productInfo.CategoryPath = categoryService.GetCategory(productInfo.CategoryId).Path;
                /*修改*/
                productService.UpdateProduct(productInfo);

                /*新增状态下，cas存在的话，则根据pub_cid更新当前的cas库信息*/
                if (Pub.Pub_CID != 0)
                {
                    string Density = productDetailModel.Density;
                    if (!string.IsNullOrEmpty(Pub.Density))
                    {
                        Density = Pub.Density + "," + Density;
                    }
                    string BoilingPoint = productDetailModel.BoilingPoint;
                    if (!string.IsNullOrEmpty(Pub.Boiling_Point))
                    {
                        BoilingPoint = Pub.Boiling_Point + "," + BoilingPoint;
                    }
                    string FusingPoint = productDetailModel.FusingPoint;
                    if (!string.IsNullOrEmpty(Pub.Melting_Point))
                    {
                        FusingPoint = Pub.Melting_Point + "," + FusingPoint;
                    }
                    string FlashPoint = productDetailModel.FlashPoint;
                    if (!string.IsNullOrEmpty(Pub.Flash_Point))
                    {
                        FlashPoint = Pub.Flash_Point + "," + FlashPoint;
                    }
                    string VapourPressure = productDetailModel.VapourPressure;
                    if (!string.IsNullOrEmpty(Pub.Vapor_Pressure))
                    {
                        VapourPressure = Pub.Vapor_Pressure + "," + VapourPressure;
                    }

                    string Alias = productDetailModel.Alias;
                    if (!string.IsNullOrEmpty(Pub.CHINESE_ALIAS))
                    {
                        Alias = Pub.CHINESE_ALIAS + "," + Alias;
                    }
                    string Ealias = productDetailModel.Ealias;
                    if (!string.IsNullOrEmpty(Pub.Record_Description))
                    {
                        Ealias = Pub.Record_Description + "," + Ealias;
                    }

                    string Shape = productDetailModel.Shape;
                    if (!string.IsNullOrEmpty(Pub.Physical_Description))
                    {
                        Shape = Pub.Physical_Description + "," + Shape;
                    }

                    string RefractiveIndex = productDetailModel.RefractiveIndex;
                    if (!string.IsNullOrEmpty(Pub.Solubility))
                    {
                        RefractiveIndex = Pub.Solubility + "," + RefractiveIndex;
                    }

                    string StorageConditions = productDetailModel.StorageConditions;
                    if (!string.IsNullOrEmpty(Pub.Storage_Conditions))
                    {
                        StorageConditions = Pub.Storage_Conditions + "," + StorageConditions;
                    }

                    string SafetyInstructions = productDetailModel.SafetyInstructions;
                    if (!string.IsNullOrEmpty(Pub.Safety_and_Hazards))
                    {
                        SafetyInstructions = Pub.Safety_and_Hazards + "," + SafetyInstructions;
                    }

                    string strsql = string.Format(@" 
                        UPDATE dbo.ChemCloud_CAS SET CHINESE='{0}',Record_Title='{1}',CHINESE_ALIAS='{2}',Record_Description='{3}',Molecular_Formula='{4}',Molecular_Weight='{5}',
                        Density='{6}',Boiling_Point='{7}',Flash_Point='{8}',Vapor_Pressure='{9}',
                        Exact_Mass='{10}',LogP='{11}',Physical_Description='{12}',Melting_Point='{13}',
                        Solubility='{14}',Storage_Conditions='{15}',Safety_and_Hazards='{16}',HS_CODE='{17}' WHERE PUB_CID='{18}'",
                        productDetailModel.goodsName, productDetailModel.EProductName,
                        Alias, Ealias,
                        productDetailModel.MolecularFormula, productDetailModel.MolecularWeight,
                        Density, BoilingPoint,
                        FlashPoint, VapourPressure,
                        productDetailModel.PASNo, productDetailModel.LogP,
                        Shape, FusingPoint, RefractiveIndex,
                        StorageConditions, SafetyInstructions,
                        productDetailModel.HSCODE, Pub.Pub_CID);
                    int result = DbHelperSQLCAS.ExecuteSql(strsql);
                }
            }
            else
            {
                productInfo.ShopId = base.CurrentSellerManager.ShopId;
                productInfo.MinSalePrice = 0;
                productInfo.CategoryPath = categoryService.GetCategory(productInfo.CategoryId).Path;
                if (productInfo.ISCASNo == false)
                {
                    productInfo.AuditStatus = ProductInfo.ProductAuditStatus.WaitForAuditing;
                }
                else
                {
                    productInfo.AuditStatus = ProductInfo.ProductAuditStatus.Audited;
                }

                //ProductDescriptionInfo prodes = new ProductDescriptionInfo();
                //prodes.Meta_Description = productDetailModel.seoDes;
                //prodes.Meta_Keywords = productDetailModel.seoKey;
                //prodes.Meta_Title = productDetailModel.seoTitle;
                //prodes.ProductId = productInfo.Id;
                //prodes.Meta_Description = productDetailModel.seoTitle;
                //prodes.Id = 0;
                //productInfo.ProductDescriptionInfo = prodes;
                /*添加产品*/
                productService.AddProduct(productInfo);
                Log.Info("添加产品成功 编号：" + productInfo.Id);
                /*CAS号在数据库中不存在，插入新的CAS记录信息*/
                if (Pub.CAS == null)
                {
                    string strsql = string.Format(@" 
                    INSERT INTO dbo.ChemCloud_CAS(PUB_CID,CAS,CHINESE,Record_Title,CHINESE_ALIAS,Record_Description,Molecular_Formula,Molecular_Weight,
                    Density,Boiling_Point,Flash_Point,Vapor_Pressure,Exact_Mass,LogP,Physical_Description,Melting_Point,Solubility,Storage_Conditions,Safety_and_Hazards,HS_CODE,[2D_Structure]) 
                    VALUES('" + newId + "','" + productDetailModel.CASNo + "','" + productDetailModel.goodsName + "','" + productDetailModel.EProductName + "','"
                    + productDetailModel.Alias + "','" + productDetailModel.Ealias + "','" + productDetailModel.MolecularFormula
                    + "','" + productDetailModel.MolecularWeight + "','" + productDetailModel.Density + "','" + productDetailModel.BoilingPoint
                    + "','" + productDetailModel.FlashPoint + "','" + productDetailModel.VapourPressure + "','" + productDetailModel.PASNo + "','"
                    + productDetailModel.LogP + "','" + productDetailModel.Shape + "','" + productDetailModel.FusingPoint + "','"
                    + productDetailModel.RefractiveIndex + "','" + productDetailModel.StorageConditions + "','" + productDetailModel.SafetyInstructions + "','" + productDetailModel.HSCODE + "','')");
                    DbHelperSQLCAS.ExecuteSql(strsql);
                    Pub.Pub_CID = newId;
                }
                else
                {
                    /*新增状态下，cas存在的话，则根据pub_cid更新当前的cas库信息*/
                    if (Pub.Pub_CID != 0)
                    {
                        string Density = productDetailModel.Density;
                        if (!string.IsNullOrEmpty(Pub.Density))
                        {
                            Density = Pub.Density + "," + Density;
                        }
                        string BoilingPoint = productDetailModel.BoilingPoint;
                        if (!string.IsNullOrEmpty(Pub.Boiling_Point))
                        {
                            BoilingPoint = Pub.Boiling_Point + "," + BoilingPoint;
                        }
                        string FusingPoint = productDetailModel.FusingPoint;
                        if (!string.IsNullOrEmpty(Pub.Melting_Point))
                        {
                            FusingPoint = Pub.Melting_Point + "," + FusingPoint;
                        }
                        string FlashPoint = productDetailModel.FlashPoint;
                        if (!string.IsNullOrEmpty(Pub.Flash_Point))
                        {
                            FlashPoint = Pub.Flash_Point + "," + FlashPoint;
                        }
                        string VapourPressure = productDetailModel.VapourPressure;
                        if (!string.IsNullOrEmpty(Pub.Vapor_Pressure))
                        {
                            VapourPressure = Pub.Vapor_Pressure + "," + VapourPressure;
                        }

                        string Alias = productDetailModel.Alias;
                        if (!string.IsNullOrEmpty(Pub.CHINESE_ALIAS))
                        {
                            Alias = Pub.CHINESE_ALIAS + "," + Alias;
                        }
                        string Ealias = productDetailModel.Ealias;
                        if (!string.IsNullOrEmpty(Pub.Record_Description))
                        {
                            Ealias = Pub.Record_Description + "," + Ealias;
                        }

                        string Shape = productDetailModel.Shape;
                        if (!string.IsNullOrEmpty(Pub.Physical_Description))
                        {
                            Shape = Pub.Physical_Description + "," + Shape;
                        }

                        string RefractiveIndex = productDetailModel.RefractiveIndex;
                        if (!string.IsNullOrEmpty(Pub.Solubility))
                        {
                            RefractiveIndex = Pub.Solubility + "," + RefractiveIndex;
                        }

                        string StorageConditions = productDetailModel.StorageConditions;
                        if (!string.IsNullOrEmpty(Pub.Storage_Conditions))
                        {
                            StorageConditions = Pub.Storage_Conditions + "," + StorageConditions;
                        }

                        string SafetyInstructions = productDetailModel.SafetyInstructions;
                        if (!string.IsNullOrEmpty(Pub.Safety_and_Hazards))
                        {
                            SafetyInstructions = Pub.Safety_and_Hazards + "," + SafetyInstructions;
                        }

                        string strsql = string.Format(@" 
                        UPDATE dbo.ChemCloud_CAS SET CHINESE='{0}',Record_Title='{1}',CHINESE_ALIAS='{2}',Record_Description='{3}',Molecular_Formula='{4}',Molecular_Weight='{5}',
                        Density='{6}',Boiling_Point='{7}',Flash_Point='{8}',Vapor_Pressure='{9}',
                        Exact_Mass='{10}',LogP='{11}',Physical_Description='{12}',Melting_Point='{13}',
                        Solubility='{14}',Storage_Conditions='{15}',Safety_and_Hazards='{16}',HS_CODE='{17}' WHERE PUB_CID='{18}'",
                            productDetailModel.goodsName, productDetailModel.EProductName,
                            Alias, Ealias,
                            productDetailModel.MolecularFormula, productDetailModel.MolecularWeight,
                            Density, BoilingPoint,
                            FlashPoint, VapourPressure,
                            productDetailModel.PASNo, productDetailModel.LogP,
                            Shape, FusingPoint, RefractiveIndex,
                            StorageConditions, SafetyInstructions,
                            productDetailModel.HSCODE, Pub.Pub_CID);
                        int result = DbHelperSQLCAS.ExecuteSql(strsql);
                    }
                }

            }
            object seoid = DbHelperSQL.GetSingle(string.Format("SELECT Id  FROM ChemCloud_CasSeo where ProductId={0} AND Pub_CID={1}", productInfo.Id, Pub.Pub_CID));
            if (seoid == null)
            {
                string strsqlseo = string.Format(@" 
                    INSERT INTO dbo.ChemCloud_CasSeo(ProductId,Meta_Title,Meta_Description,Meta_Keywords,Pub_CID) 
                    VALUES('" + productInfo.Id + "','" + productDetailModel.seoTitle + "','" + productDetailModel.seoDes + "','" + productDetailModel.seoKey + "','"
                     + Pub.Pub_CID + "')");
                int result = DbHelperSQL.ExecuteSql(strsqlseo);
            }
            else
            {
                string strsqlseo = string.Format(@" 
                        UPDATE dbo.ChemCloud_CasSeo SET ProductId='{0}',Meta_Title='{1}',Meta_Description='{2}',Meta_Keywords='{3}',Pub_CID='{4}' WHERE Id='{5}'",
                               productInfo.Id, productDetailModel.seoTitle,
                              productDetailModel.seoDes, productDetailModel.seoKey,
                               Pub.Pub_CID, Convert.ToInt32(seoid));
                int result = DbHelperSQL.ExecuteSql(strsqlseo);
            }

            /*记录日志*/
            IOperationLogService operationLogService = ServiceHelper.Create<IOperationLogService>();
            LogInfo logInfo = new LogInfo()
            {
                Date = DateTime.Now,
                Description = string.Concat("商家发布商品，名称=", productInfo.ProductName),
                IPAddress = base.Request.UserHostAddress,
                PageUrl = "/Product/SaveProductDetail",
                UserName = base.CurrentSellerManager.UserName,
                ShopId = base.CurrentSellerManager.ShopId
            };
            operationLogService.AddSellerOperationLog(logInfo);
            return Json(new { success = true, id = productInfo.Id });
        }

        [HttpPost]
        [UnAuthorize]
        [ValidateInput(false)]
        public JsonResult SaveProductDetailNo(string productDetail)
        {
            /*去除换行符*/
            string pattern = @"\n";
            Regex rgx = new Regex(pattern);
            string outputStr = rgx.Replace(productDetail, "*");
            productDetail = outputStr;

            IProductService productService = ServiceHelper.Create<IProductService>();
            ICategoryService categoryService = ServiceHelper.Create<ICategoryService>();
            ProductDetailModel productDetailModel = JsonConvert.DeserializeObject<ProductDetailModel>(productDetail);
            long num = productDetailModel.productId;
            ProductInfo productInfo = ProductDetailModel.GetProductInfo(productDetailModel, num);
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);

            ProductInfo.ProductEditStatus editStatus = productService.GetEditStatus(num, productInfo);
            string dataurl = productInfo.ProductDescriptionInfo.Description;
            /*mol转inchikey*/
            string inchkey = productDetailModel.mdes.Replace('*', '\n');
            inchkey = ChemCloud.Web.InchikeyHelper.GetInchikey(inchkey);
            if (string.IsNullOrEmpty(inchkey))
            {
                inchkey = "";
            }
            int newId = 0;
            /*根据mol获取cas信息*/
            string cas_pubcid = ServiceHelper.Create<ICASInfoService>().GetCASByInchikey(inchkey);
            if (cas_pubcid != "0")
            {
                productInfo.Pub_CID = int.Parse(cas_pubcid);
            }
            else
            {
                newId = DbHelperSQLCAS.GetMaxID("PUB_CID", "ChemCloud_CAS");
                if (newId >= 200000000)
                {
                    newId = newId + 1;
                }
                else
                {
                    newId = 200000000;
                }
                productInfo.Pub_CID = newId;
            }

            productInfo.EditStatus = (short)editStatus;
            if (productDetailModel.productId != 0)
            {
                /*修改产品信息*/
                productInfo.ShopId = shopId;
                productInfo.ImagePath = string.Format("/Storage/Shop/{0}/Products/{1}", shopId, productInfo.Id);
                productService.UpdateProduct(productInfo);
            }
            else
            {
                /*添加产品*/
                productInfo.ShopId = base.CurrentSellerManager.ShopId;
                productInfo.MinSalePrice = 0;
                productInfo.ImagePath = "";
                productInfo.AuditStatus = ProductInfo.ProductAuditStatus.NoCasNoWaitAuditing;
                productService.AddProduct(productInfo);

                /*CAS号在数据库中不存在，插入新的CAS记录信息*/
                if (cas_pubcid == "0")
                {
                    /*无cas号的依据 inchkey*/
                    string strsql = string.Format(@" 
                    INSERT INTO dbo.ChemCloud_CAS(PUB_CID,CAS,CHINESE,Record_Title,CHINESE_ALIAS,Record_Description,Molecular_Formula,Molecular_Weight,
                    Density,Boiling_Point,Flash_Point,Vapor_Pressure,Exact_Mass,LogP,Physical_Description,Melting_Point,Solubility,Storage_Conditions,Safety_and_Hazards,HS_CODE,InChI_Key,[2D_Structure]) 
                    VALUES('" + newId + "','" + productDetailModel.CASNo + "','" + productDetailModel.goodsName + "','" + productDetailModel.EProductName + "','"
                      + productDetailModel.Alias + "','" + productDetailModel.Ealias + "','" + productDetailModel.MolecularFormula
                      + "','" + productDetailModel.MolecularWeight + "','" + productDetailModel.Density + "','" + productDetailModel.BoilingPoint
                      + "','" + productDetailModel.FlashPoint + "','" + productDetailModel.VapourPressure + "','" + productDetailModel.PASNo + "','"
                      + productDetailModel.LogP + "','" + productDetailModel.Shape + "','" + productDetailModel.FusingPoint + "','"
                      + productDetailModel.RefractiveIndex + "','" + productDetailModel.StorageConditions + "','" + productDetailModel.SafetyInstructions + "','" + productDetailModel.HSCODE + "','" + inchkey + "','" + dataurl + "');select @@identity");
                    DbHelperSQLCAS.ExecuteSql(strsql);
                    cas_pubcid = newId.ToString();

                }
            }

            object seoid = DbHelperSQL.GetSingle(string.Format("SELECT Id  FROM ChemCloud_CasSeo where ProductId={0} AND Pub_CID={1}", productInfo.Id, cas_pubcid));
            if (seoid == null)
            {
                string strsqlseo = string.Format(@" 
                    INSERT INTO dbo.ChemCloud_CasSeo(ProductId,Meta_Title,Meta_Description,Meta_Keywords,Pub_CID) 
                    VALUES('" + productInfo.Id + "','" + productDetailModel.seoTitle + "','" + productDetailModel.seoDes + "','" + productDetailModel.seoKey + "','"
                     + cas_pubcid + "')");
                int result = DbHelperSQL.ExecuteSql(strsqlseo);
            }
            else
            {
                string strsqlseo = string.Format(@" 
                        UPDATE dbo.ChemCloud_CasSeo SET ProductId='{0}',Meta_Title='{1}',Meta_Description='{2}',Meta_Keywords='{3}',Pub_CID='{4}' WHERE Id='{5}'",
                               productInfo.Id, productDetailModel.seoTitle,
                              productDetailModel.seoDes, productDetailModel.seoKey,
                               cas_pubcid, Convert.ToInt32(seoid));
                int result = DbHelperSQL.ExecuteSql(strsqlseo);
            }
            IOperationLogService operationLogService = ServiceHelper.Create<IOperationLogService>();
            LogInfo logInfo = new LogInfo()
            {
                Date = DateTime.Now,
                Description = string.Concat("商家发布商品，名称=", productInfo.ProductName),
                IPAddress = base.Request.UserHostAddress,
                PageUrl = "/Product/SaveProductDetail",
                UserName = base.CurrentSellerManager.UserName,
                ShopId = base.CurrentSellerManager.ShopId
            };
            operationLogService.AddSellerOperationLog(logInfo);
            return Json(new { successful = true, id = productInfo.Id });
        }

        public JsonResult GetCASData(string CASNo)
        {
            CASInfo casno = ServiceHelper.Create<ICASInfoService>().GetCASByNo(CASNo);
            return Json(casno);
        }
        /// <summary>
        /// 新增规格
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult SaveSpec(string json, long id)
        {
            try
            {
                ProductInfo model = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductInfo>(json);
                if (model != null)
                {

                    foreach (var item in model._ProductSpec)
                    {
                        item.ProductId = id;
                        item.Purity = item.Purity;
                        item.Price = item.Price;
                        item.Packaging = item.Packaging;
                        item.SpecLevel = item.SpecLevel;
                        item.CoinType = item.CoinType;
                    }

                    ServiceHelper.Create<IProductService>().AddProductSpec(model, id);

                    return Json(new { success = true, msg = "添加成功！" });
                }
                else
                {
                    return Json(new { success = false, msg = "添加失败！" });
                }


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException xx)
            {
                return Json(new { success = false, msg = "添加失败！" });
            }
        }

        public ActionResult ProdcutManagement()
        {
            return View();
        }
        public ActionResult ProductAuthentication()
        {
            return View();
        }

        public ActionResult ProductAuthenticationInfo(long productId = 0L)
        {
            IProductService productService = ServiceHelper.Create<IProductService>();
            ProductInfo productInfo = new ProductInfo();
            if (productId != 0)
            {
                productInfo = productService.GetProduct(productId);
                if (productInfo == null || productInfo.ShopId != base.CurrentSellerManager.ShopId)
                {
                    throw new HimallException(string.Concat(productId, ",该商品已删除"));
                }
            }
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
            string comname = "";
            if (shop != null)
            {
                comname = shop.ShopName;
            }
            ViewBag.ComName = comname;
            ViewBag.ProductId = productId;
            ViewBag.ShopId = shopId;
            IProductAuthenticationService imcs = ServiceHelper.Create<IProductAuthenticationService>();
            ProductAuthentication PAINFO = imcs.GetProductAuthenticationProductId(productId);
            string showbtn = "";
            string AuthStatus = "0";
            string ComAttachment = "";
            if (PAINFO == null)
            {
                showbtn = "提交认证";
            }
            else
            {
                AuthStatus = PAINFO.AuthStatus.ToString();
                ComAttachment = PAINFO.ComAttachment == null ? "" : PAINFO.ComAttachment;
                switch (PAINFO.AuthStatus)
                {
                    case 0: showbtn = "已提交"; break;
                    case 1: showbtn = "已确认"; break;
                    case 2: showbtn = "已支付"; break;
                    case 3: showbtn = "已通过审核"; break;
                    case 4: showbtn = "已拒绝审核"; break;
                }
            }
            ViewBag.AuthStatus = AuthStatus;
            ViewBag.ComAttachment = ComAttachment;
            ViewBag.BtnLalel = showbtn;

            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            ViewBag.OrderId = _orderBO.GenerateOrderNumber();

            return View(productInfo);
        }
        /// <summary>
        /// 供应商产品认证
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListProductAuthentication(int page, int rows, string productCode, string AuthStatus)
        {
            IProductAuthenticationService IPAS = ServiceHelper.Create<IProductAuthenticationService>();
            ProductAuthenticationQuery PAQ = new ProductAuthenticationQuery()
            {
                ProductCode = productCode,
                AuthStatus = AuthStatus,
                PageNo = page,
                PageSize = rows,
                ManageId = base.CurrentSellerManager.Id
            };
            PageModel<ProductAuthentication> pm = IPAS.GetProductAuthenticationList(PAQ);
            IEnumerable<ProductAuthentication> models =
            from item in pm.Models.ToArray()
            select new ProductAuthentication()
            {
                Id = item.Id,
                ManageId = item.ManageId,
                ProductCode = item.ProductCode,
                ProductIMG = item.ProductIMG,
                ProductDesc = item.ProductDesc,
                ProductAuthDate = item.ProductAuthDate,
                AuthStatus = item.AuthStatus,
                AuthAuthor = item.AuthAuthor,
                AuthTime = item.AuthTime,
                AuthDesc = item.AuthDesc
            };
            DataGridModel<ProductAuthentication> dataGridModel = new DataGridModel<ProductAuthentication>()
            {
                rows = models,
                total = pm.Total
            };
            return Json(dataGridModel);
        }
        [HttpPost]
        public JsonResult AddPAuthInfo(string ProductId, string ProductCode, string ProductIMG, string ComName, string COAIMG)
        {
            int i;
            int pid = 0;
            if (int.TryParse(ProductId, out i))
            {
                pid = int.Parse(ProductId);
            }
            IProductAuthenticationService imcs = ServiceHelper.Create<IProductAuthenticationService>();
            ProductAuthentication PAINFO = imcs.GetProductAuthenticationProductId(long.Parse(ProductId));
            if (PAINFO == null)
            {//未提交认证  提交认证
                ProductAuthentication p = new ProductAuthentication
                {
                    ManageId = base.CurrentSellerManager.Id,
                    ProductId = pid,
                    ProductCode = ProductCode,
                    ProductIMG = ProductIMG,
                    ProductDesc = "",
                    ProductAuthDate = DateTime.Now,
                    AuthStatus = 0,
                    AuthTime = DateTime.Now,
                    AuthDesc = "",
                    ComName = ComName,
                    ComAttachment = COAIMG
                };
                imcs.AddProductAuthentication(p);
            }
            else
            {
                //已提交认证，认证拒绝，重新提交
                ProductAuthentication p = new ProductAuthentication
                {
                    ManageId = PAINFO.ManageId,
                    ProductId = PAINFO.ProductId,
                    ProductCode = PAINFO.ProductCode,
                    ProductIMG = ProductIMG,
                    ProductDesc = PAINFO.ProductDesc,
                    ProductAuthDate = DateTime.Now,
                    AuthStatus = 0,
                    AuthTime = DateTime.Now,
                    AuthDesc = "",
                    ComName = ComName,
                    ComAttachment = COAIMG
                };
                imcs.UpdateProductAuthentication(p);
            }
            return Json(new { Successful = true });
        }
        [HttpPost]
        public JsonResult CheckPAuthInfo(string ProductId)
        {
            IProductAuthenticationService IPAS = ServiceHelper.Create<IProductAuthenticationService>();
            ProductAuthentication PAINFO = IPAS.GetProductAuthenticationProductId(long.Parse(ProductId));
            if (PAINFO == null)
            {
                return Json(new { Successful = "" });
            }
            else
            {
                if (PAINFO.AuthStatus == 0)
                {
                    return Json(new { Successful = "此产品正在审核中，请您耐心等待。" });
                }
                else if (PAINFO.AuthStatus == 3)
                {
                    return Json(new { Successful = "此产品已认证通过。" });
                }
                else
                {
                    return Json(new { Successful = "" });
                }
            }

        }

        public JsonResult Getinchkey(string strmol)
        {
            string inchkey = ChemCloud.Web.InchikeyHelper.GetInchikey(strmol);

            bool result = false;
            if (!string.IsNullOrEmpty(inchkey))
            {
                result = true;
            }

            return Json(new { success = result, msg = inchkey });
        }

        public JsonResult DSList(string pname, string pcode, string cas, string pstatus)
        {
            string strsql = "";
            string strsqlwhere = "";
            if (!string.IsNullOrWhiteSpace(pname))
            {
                strsqlwhere += string.Format(" and p.ProductName='{0}' ", pname);
            }
            if (!string.IsNullOrWhiteSpace(pcode))
            {
                strsqlwhere += string.Format(" and p.ProductCode='{0}' ", pcode);
            }
            if (!string.IsNullOrWhiteSpace(cas))
            {
                strsqlwhere += string.Format(" and p.CASNo='{0}' ", cas);
            }
            if (!string.IsNullOrWhiteSpace(pstatus))
            {
                strsqlwhere += string.Format(" and ds.DSStatus='{0}' ", pstatus);
            }
            strsql += string.Format("select p.Id,p.CASNo,p.ProductName,p.ProductCode,p.Purity,ds.DSStatus,ds.Id as pid from chemcloud_products p left join chemcloud_productsds ds on ds.ProductId=p.Id where p.shopid=(select shopid from chemcloud_managers where username='{0}') " + strsqlwhere + " order by p.addeddate desc", base.CurrentUser.UserName);
            DataSet ds = DbHelperSQL.Query(strsql);
            if (ds == null)
            {
                return Json("");
            }
            else
            {
                string reslut = ChemCloud.Core.JsonHelper.GetJsonByDataset(ds);
                return Json(reslut);
            }
        }

        public JsonResult AddDS(long pid)
        {
            ProductInfo pinfo = ServiceHelper.Create<IProductService>().GetProduct(pid);
            if (pinfo == null)
            {
                return Json("");
            }
            ProductsDSQuery query = new ProductsDSQuery();
            query.productid = pinfo.Id;
            query.shopid = pinfo.ShopId;
            query.userid = base.CurrentUser.Id;
            query.usertype = base.CurrentUser.UserType;
            query.dsstatus = 0;
            query.dstime = DateTime.Now;
            if (ServiceHelper.Create<IProductsDSService>().AddProductsDS(query))
            {
                return Json("ok");
            }
            else
            {
                return Json("");
            }
        }

        /*申请特价*/
        [HttpPost]
        [UnAuthorize]
        public JsonResult UpdateSpecicalOffer(string ids, string status)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            ServiceHelper.Create<IProductService>().UpdateSpecicalOffer(nums, base.CurrentSellerManager.ShopId, decimal.Parse(status));

            return Json(new { success = true });
        }

        public JsonResult UpDS(long pid)
        {
            ProductsDSQuery query = new ProductsDSQuery();

            ProductsDS ps = ServiceHelper.Create<IProductsDSService>().GetProductsDSbyId(pid);
            if (ps == null)
            {
                return Json("");
            }
            ProductInfo pinfo = ServiceHelper.Create<IProductService>().GetProduct(ps.ProductId);
            if (pinfo == null)
            {
                return Json("");
            }
            query.id = ps.Id;
            query.productid = pinfo.Id;
            query.shopid = pinfo.ShopId;
            query.userid = base.CurrentUser.Id;
            query.usertype = base.CurrentUser.UserType;
            query.dsstatus = 0;
            query.dstime = DateTime.Now;
            if (ServiceHelper.Create<IProductsDSService>().UpdateProductsDS(query))
            {
                return Json("ok");
            }
            else
            {
                return Json("");
            }
        }


    }

}