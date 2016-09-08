using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.Admin.Models.Product;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        public ProductController()
        {
        }
        [Description("产品审核页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult Auditing(long id)
        {
            ProductQuery ProductQuery = new ProductQuery();
            ProductInfo ProductInfo = ServiceHelper.Create<IProductService>().GetProduct(id);
            ProductModel ProductModel = new ProductModel(ProductInfo);
            ViewBag.Id = id;
            ViewBag.Status = ProductInfo.AuditStatus;
            return View(ProductModel);
        }
        [Description("销售中页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult Salesing(long id)
        {
            ProductQuery ProductQuery = new ProductQuery();
            ProductInfo ProductInfo = ServiceHelper.Create<IProductService>().GetProduct(id);
            ProductModel ProductModel = new ProductModel(ProductInfo);
            ViewBag.Id = id;
            ViewBag.Status = ProductInfo.AuditStatus;
            return View(ProductModel);
        }
        [Description("违规下架页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult SaleOff(long id)
        {
            ProductQuery ProductQuery = new ProductQuery();
            ProductInfo ProductInfo = ServiceHelper.Create<IProductService>().GetProduct(id);
            ProductModel ProductModel = new ProductModel(ProductInfo);
            ViewBag.Id = id;
            ViewBag.Status = ProductInfo.AuditStatus;
            return View(ProductModel);
        }
        [Description("违规下架页面(POST)")]
        [HttpPost]
        [UnAuthorize]
        public JsonResult SaleOff(long Id, int status, string comment = "")
        {
            ServiceHelper.Create<IProductService>().UpdateStatus(Id, (ProductInfo.ProductAuditStatus)status, comment);
            return Json(new { Successful = true });
        }
        [Description("产品审核页面(POST)")]
        [HttpPost]
        [UnAuthorize]
        public JsonResult Auditing(long Id, int status, string CasNo, string comment = "")
        {
            ServiceHelper.Create<IProductService>().UpdateStatus(Id, (ProductInfo.ProductAuditStatus)status, comment);
            ProductInfo productinfo = ServiceHelper.Create<IProductService>().GetProduct(Id);
            CASInfo casproduct = ServiceHelper.Create<ICASInfoService>().GetCASByNo(productinfo.CASNo);
            ICASInfoService casService = ServiceHelper.Create<ICASInfoService>();
            if (casproduct == null)
            {

                CASInfo casproductinfo = new CASInfo()
                {
                    //CAS_NO = Guid.NewGuid().ToString("D"),
                    CAS = productinfo.CASNo,
                    CHINESE = productinfo.ProductName,
                    CHINESE_ALIAS = productinfo.Alias,
                    Record_Title = productinfo.EProductName,
                    Record_Description = productinfo.Ealias,
                    Molecular_Formula = productinfo.MolecularFormula,
                    Molecular_Weight = productinfo.MolecularWeight,
                    //PSA = productinfo.PASNo,
                    Density = productinfo.Density,
                    Boiling_Point = productinfo.BoilingPoint,
                    Flash_Point = productinfo.FlashPoint,
                    //= productinfo.RefractiveIndex,
                    Vapor_Pressure = productinfo.VapourPressure,

                };
                casService.AddCAS(casproductinfo);
            }
            else
            {

                CASInfo casproductinfo = new CASInfo()
                {
                    CAS = productinfo.CASNo,
                    CHINESE = productinfo.ProductName,
                    CHINESE_ALIAS = productinfo.Alias,
                    Record_Title = productinfo.EProductName,
                    Record_Description = productinfo.Ealias,
                    Molecular_Formula = productinfo.MolecularFormula,
                    Molecular_Weight = productinfo.MolecularWeight,
                    //PSA = productinfo.PASNo,
                    Density = productinfo.Density,
                    Boiling_Point = productinfo.BoilingPoint,
                    Flash_Point = productinfo.FlashPoint,
                    //= productinfo.RefractiveIndex,
                    Vapor_Pressure = productinfo.VapourPressure,

                };
                casService.UpdateCAS(casproductinfo);
            }
            return Json(new { Successful = true });
        }
        [Description("明细页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult Details(long id)
        {
            ProductQuery ProductQuery = new ProductQuery();
            ProductInfo ProductInfo = ServiceHelper.Create<IProductService>().GetProduct(id);
            ProductModel productModel = new ProductModel(ProductInfo);
            ViewBag.PassStr = ProductInfo.AuditStatus.ToDescription();
            ViewBag.Status = ProductInfo.AuditStatus.ToDescription();
            return View(productModel);
        }

        [Description("无CASNO产品审核页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult NoCasNoAuditing(long id)
        {
            ProductQuery ProductQuery = new ProductQuery();
            ProductInfo ProductInfo = ServiceHelper.Create<IProductService>().GetProduct(id);
            ProductModel ProductModel = new ProductModel(ProductInfo);
            ViewBag.Id = id;
            ViewBag.Status = ProductInfo.AuditStatus;
            return View(ProductModel);
        }
        [Description("无CASNO产品审核页面(POST)")]
        [HttpPost]
        [UnAuthorize]
        public JsonResult NoCasNoAuditing(long Id, int status, string comment = "")
        {
            ServiceHelper.Create<IProductService>().UpdateStatus(Id, (ProductInfo.ProductAuditStatus)status, comment);
            return Json(new { Successful = true });
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(long? categoryId, string CASNo, string productCode, string productName, int? status, string ids, int page, int rows, string companyName, int? saleStatus, string type = "")
        {
            IEnumerable<long> nums;
            ProductInfo.ProductAuditStatus? nullable;
            PlatProductQuery productQuery = new PlatProductQuery()
            {
                PageSize = rows,
                PageNo = page,
                CategoryId = categoryId,
                CASNo = CASNo,
            };
            PlatProductQuery ProductQuery1 = productQuery;
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
            ProductQuery1.Ids = nums;
            productQuery.CompanyName = companyName;
            productQuery.ProductCode = productCode;
            productQuery.ProductName = productName;
            productQuery.CASNo = CASNo;
            productQuery.NotIncludedInDraft = true;
            PlatProductQuery ProductQuery2 = productQuery;
            if (type.Equals("Auditing"))
            {
                status = 1;
            }
            else
            {
                if (type.Equals("Salesing"))
                {
                    status = 2;
                }

                if (type.Equals("SaleOff"))
                {
                    status = 4;
                }
                if (type.Equals("NoCasNoAuditing"))
                {
                    status = 7;
                }
            }
            if (status == 0)
                status = null;
            int? nullable1 = status;
            if (nullable1.HasValue)
            {
                ProductQuery2.AuditStatus = new ProductInfo.ProductAuditStatus?((ProductInfo.ProductAuditStatus)nullable1.GetValueOrDefault());
                nullable = new ProductInfo.ProductAuditStatus?((ProductInfo.ProductAuditStatus)nullable1.GetValueOrDefault());//new ProductInfo.ProductAuditStatus[] { (ProductInfo.ProductAuditStatus)auditStatus.Value };
                if ((nullable.Value != ProductInfo.ProductAuditStatus.WaitForAuditing ? false : nullable.HasValue))
                {
                    //ProductQuery2.SaleStatus = new ProductInfo.ProductSaleStatus?(ProductInfo.ProductSaleStatus.OnSale);
                }
            }
            else
            {
                nullable = null;
            }
            if (saleStatus.HasValue)
            {
                ProductQuery2.SaleStatus = new ProductInfo.ProductSaleStatus?((ProductInfo.ProductSaleStatus)saleStatus.Value);
            }

            PageModel<ProductInfo> products = ServiceHelper.Create<IProductService>().GetPlatProducts(ProductQuery2);
            ICategoryService categoryService = ServiceHelper.Create<ICategoryService>();
            IShopService shopService = ServiceHelper.Create<IShopService>();
            IBrandService brandService = ServiceHelper.Create<IBrandService>();
            IEnumerable<ProductModel> array = (
                from item in products.Models.ToArray()
                select new ProductModel()
                {
                    Status = item.AuditStatus.ToDescription(),
                    categoryName = (categoryService.GetCategory(item.CategoryId) == null ? "" : categoryService.GetCategory(item.CategoryId).Name),
                    id = item.Id,
                    name = item.ProductName,
                    price = item.MinSalePrice,
                    saleStatus = item.SaleStatus.ToDescription(),
                    state = item.ShowProductState,
                    CASNo = item.CASNo,
                    CompanyName = item.CompanyName,
                    EProductName = item.EProductName,
                    Purity = item.Purity,
                    HSCODE = item.HSCODE,
                    DangerLevel = item.DangerLevel,
                    MolecularFormula = item.MolecularFormula,
                    Alias = item.Alias,
                    Ealias = item.Ealias,
                    MolecularWeight = item.MolecularWeight,
                    PASNo = item.PASNo,
                    LogP = item.LogP,
                    Shape = item.Shape,
                    Density = item.Density,
                    FusingPoint = item.FusingPoint,
                    BoilingPoint = item.BoilingPoint,
                    FlashPoint = item.FlashPoint,
                    RefractiveIndex = item.RefractiveIndex,
                    StorageConditions = item.StorageConditions,
                    VapourPressure = item.VapourPressure,
                    PackagingLevel = item.PackagingLevel,
                    SafetyInstructions = item.SafetyInstructions,
                    DangerousMark = item.DangerousMark,
                    RiskCategoryCode = item.RiskCategoryCode,
                    TransportationNmber = item.TransportationNmber,
                    RETCS = item.RETCS,
                    WGKGermany = item.WGKGermany,
                    SyntheticRoute = item.SyntheticRoute,
                    RelatedProducts = item.RelatedProducts,
                    MSDS = item.MSDS,
                    RefuseReason = item.RefuseReason,
                    ShopId = item.ShopId,
                    ProductCode = item.ProductCode
                }).ToList();
            IEnumerable<ProductModel> productModels = array.Select<ProductModel, ProductModel>((ProductModel item) =>
            {
                ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(item.ShopId, false);

                return new ProductModel()
                {
                    Status = item.Status,
                    categoryName = item.categoryName,
                    id = item.id,
                    name = item.name,
                    price = item.price,
                    saleStatus = item.saleStatus,
                    state = item.state,
                    CASNo = item.CASNo,
                    CompanyName = item.CompanyName,
                    EProductName = item.EProductName,
                    Purity = item.Purity,
                    HSCODE = item.HSCODE,
                    DangerLevel = item.DangerLevel,
                    MolecularFormula = item.MolecularFormula,
                    Alias = item.Alias,
                    Ealias = item.Ealias,
                    MolecularWeight = item.MolecularWeight,
                    PASNo = item.PASNo,
                    LogP = item.LogP,
                    Shape = item.Shape,
                    Density = item.Density,
                    FusingPoint = item.FusingPoint,
                    BoilingPoint = item.BoilingPoint,
                    FlashPoint = item.FlashPoint,
                    RefractiveIndex = item.RefractiveIndex,
                    StorageConditions = item.StorageConditions,
                    VapourPressure = item.VapourPressure,
                    PackagingLevel = item.PackagingLevel,
                    SafetyInstructions = item.SafetyInstructions,
                    DangerousMark = item.DangerousMark,
                    RiskCategoryCode = item.RiskCategoryCode,
                    TransportationNmber = item.TransportationNmber,
                    RETCS = item.RETCS,
                    WGKGermany = item.WGKGermany,
                    SyntheticRoute = item.SyntheticRoute,
                    RelatedProducts = item.RelatedProducts,
                    MSDS = item.MSDS,
                    RefuseReason = item.RefuseReason,
                    ShopStatus = (int)shop.ShopStatus,
                    ProductCode = item.ProductCode
                };
            }).ToList();
            DataGridModel<ProductModel> dataGridModel = new DataGridModel<ProductModel>()
            {
                rows = productModels,
                total = products.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult Management(string type = "")
        {
            IEnumerable<SelectListItem> selectListItems = null;
            SelectList selectList = ProductInfo.ProductAuditStatus.Audited.ToSelectList<ProductInfo.ProductAuditStatus>(true, false);
            dynamic viewBag = base.ViewBag;


            if (type == "Auditing")
            {
                selectListItems = (
                from p in selectList
                where (int.Parse(p.Value) == 1)
                select p);
            }
            if (type == "Salesing")
            {
                selectListItems = (
                from p in selectList
                where (int.Parse(p.Value) == 2)
                select p);
            }
            if (type == "NoCasNoAuditing")
            {
                selectListItems = (
                from p in selectList
                where (int.Parse(p.Value) == 7)
                select p);
            }
            if (type == "SaleOff")
            {
                selectListItems = (
                from p in selectList
                where (int.Parse(p.Value) == 4)
                select p);
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                selectListItems = (
                from p in selectList
                where (int.Parse(p.Value) == 0) || (int.Parse(p.Value) == 1) || (int.Parse(p.Value) == 2) || (int.Parse(p.Value) == 3) || (int.Parse(p.Value) == 4) || (int.Parse(p.Value) == 7)
                select p);
            }

            //状态去掉 违规下架（违规下架直接删除了数据）
            selectListItems = (
                from p in selectListItems
                where (int.Parse(p.Value) != 4)
                select p);

            ViewBag.Status = selectListItems;
            List<SelectListItem> selectListItems1 = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Selected = true,
                Value = 0.ToString(),
                Text = "请选择..."
            };
            selectListItems1.Add(selectListItem);
            ViewBag.Type = type;
            return View();
        }

        public ActionResult ProductAuthentication()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ListProductAuthentication(int page, int rows, string comname, string productCode, string AuthStatus)
        {
            IProductAuthenticationService IPAS = ServiceHelper.Create<IProductAuthenticationService>();
            ProductAuthenticationQuery PAQ = new ProductAuthenticationQuery()
            {
                ComName = comname,
                ProductCode = productCode,
                AuthStatus = AuthStatus,
                PageNo = page,
                PageSize = rows,
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
                AuthDesc = item.AuthDesc,
                ComName = item.ComName,
                ComAttachment = item.ComAttachment
            };
            DataGridModel<ProductAuthentication> dataGridModel = new DataGridModel<ProductAuthentication>()
            {
                rows = models,
                total = pm.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult ProductAuthenticationInfo(long Id = 0L)
        {
            IProductAuthenticationService productService = ServiceHelper.Create<IProductAuthenticationService>();
            ProductAuthentication productInfo = new ProductAuthentication();
            string procode = "";
            string proimg = "";
            string comname = "";
            string comatt = "";
            if (Id != 0)
            {
                productInfo = productService.GetProductAuthenticationId(Id);
                if (productInfo != null)
                {
                    procode = productInfo.ProductCode;
                    proimg = productInfo.ProductIMG;
                    comname = productInfo.ComName;
                    comatt = productInfo.ComAttachment;
                }
            }
            ViewBag.ProductCode = procode;
            ViewBag.ProductIMG = proimg;
            ViewBag.ComName = comname;
            ViewBag.ComAtt = comatt;
            return View(productInfo);
        }
        [HttpPost]
        public JsonResult Success(long Id, string AuthStatus)
        {
            IProductAuthenticationService productService = ServiceHelper.Create<IProductAuthenticationService>();
            ProductAuthentication productInfo = ServiceHelper.Create<IProductAuthenticationService>().GetProductAuthenticationId(Id);
            productInfo.AuthStatus = int.Parse(AuthStatus);
            productInfo.AuthTime = DateTime.Now;
            ServiceHelper.Create<IProductAuthenticationService>().UpdateProductAuthentication(productInfo);
            ProductInfo productInfos = ServiceHelper.Create<IProductService>().GetProduct(productInfo.ProductId);
            productInfos.AuthStatus = productInfo.AuthStatus;
            ServiceHelper.Create<IProductService>().UpdateProduct(productInfos);
            return Json(new { Successful = true });
        }

        [HttpPost]
        public JsonResult NoSuccess(long Id)
        {
            IProductAuthenticationService productService = ServiceHelper.Create<IProductAuthenticationService>();
            ProductAuthentication productInfo = ServiceHelper.Create<IProductAuthenticationService>().GetProductAuthenticationId(Id);
            productInfo.AuthStatus = 2;
            ServiceHelper.Create<IProductAuthenticationService>().UpdateProductAuthentication(productInfo);
            ProductInfo productInfos = ServiceHelper.Create<IProductService>().GetProduct(productInfo.ProductId);
            productInfos.AuthStatus = productInfo.AuthStatus;
            ServiceHelper.Create<IProductService>().UpdateProduct(productInfos);
            return Json(new { Successful = true });
        }


        public ActionResult ProductsDS()
        {
            return View();
        }

        public JsonResult ListDS(string pstatus, int page, int rows)
        {
            ProductsDSQuery query = new ProductsDSQuery();
            query.PageNo = page;
            query.PageSize = rows;
            int i;
            if (int.TryParse(pstatus, out i))
            {
                query.dsstatus = int.Parse(pstatus);
            }
            PageModel<ProductsDS> p = ServiceHelper.Create<IProductsDSService>().GetProductsDSList(query);
            IEnumerable<ProductsDS> models =
                from item in p.Models.ToArray()
                select new ProductsDS()
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    ProductId = item.ProductId,
                    productCode = ServiceHelper.Create<IProductService>().GetProduct(item.ProductId) == null ? "" : ServiceHelper.Create<IProductService>().GetProduct(item.ProductId).ProductCode,
                    sellerusername = ServiceHelper.Create<IMemberService>().GetMember(item.UserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(item.UserId).UserName,
                    productName = ServiceHelper.Create<IProductService>().GetProduct(item.ProductId) == null ? "" : ServiceHelper.Create<IProductService>().GetProduct(item.ProductId).ProductName,
                    cas = ServiceHelper.Create<IProductService>().GetProduct(item.ProductId) == null ? "" : ServiceHelper.Create<IProductService>().GetProduct(item.ProductId).CASNo,
                    DSStatus = item.DSStatus,
                    DSTime = item.DSTime
                };
            DataGridModel<ProductsDS> dataGridModel = new DataGridModel<ProductsDS>()
            {
                rows = models,
                total = p.Total
            };
            return Json(dataGridModel);
        }

        public JsonResult DS(string id, int Status)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json("");
            }
            else
            {
                int i;
                if (!int.TryParse(id, out i))
                {
                    return Json("");
                }
                else
                {
                    ProductsDSQuery query = new ProductsDSQuery();
                    query.id = int.Parse(id);
                    ProductsDS p = ServiceHelper.Create<IProductsDSService>().GetProductsDSbyId(int.Parse(id));
                    query.dsstatus = Status;
                    query.shopid = p.ShopId;
                    query.productid = p.ProductId;
                    query.productcode = p.productCode;
                    query.userid = p.UserId;
                    query.usertype = p.UserType;
                    query.dstime = p.DSTime;
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
    }
}