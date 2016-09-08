using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.SellerAdmin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class ProductConcernController : BaseMemberController
    {
        public ProductConcernController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取关注列表数据
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="productName"></param>
        /// <param name="userName"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult List(string starttime, string endtime, string productName, string userName, int rows, int page, string companyName) //string companyName,
        {
            AttentionQuery AttentionQueryModel = new AttentionQuery()
            {
                ShopId = base.CurrentSellerManager.ShopId,
                beginTime = starttime,
                endTime = endtime,
                userName = userName,
                productName = productName,
                compamyName = companyName,
                PageSize = rows,
                PageNo = page,
            };
            PageModel<Attention> AttentionShops = ServiceHelper.Create<IAttentionService>().SupplierGetAttentions(AttentionQueryModel);
            IEnumerable<AttentionModel> array =
                from m in AttentionShops.Models.ToArray()
                select new AttentionModel()
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    ShopId = m.ShopId,
                    ProductId = m.ProductId,
                    CreatDate = m.CreatDate,
                    ProductName = m.ProductName,
                    ImagePath = m.ImagePath,
                    MarketPrice = m.MarketPrice,
                    MinSalePrice = m.MinSalePrice,
                    MeasureUnit = m.MeasureUnit,
                    EProductName = m.EProductName,
                    Purity = m.Purity,
                    CASNo = m.CASNo,
                    HSCODE = m.HSCODE,
                    DangerLevel = m.DangerLevel,
                    MolecularFormula = m.MolecularFormula,
                    MolecularWeight = m.MolecularWeight,
                    PASNo = m.PASNo,
                    LogP = m.LogP,
                    Shape = m.Shape,
                    Density = m.Density,
                    FusingPoint = m.FusingPoint,
                    BoilingPoint = m.BoilingPoint,
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
                    RefuseReason = m.RefuseReason,
                    STRUCTURE_2D = m.STRUCTURE_2D,
                    UserName = m.UserName,
                    CompanyName = m.CompanyName
                };
            DataGridModel<AttentionModel> dataGridModel = new DataGridModel<AttentionModel>()
            {
                rows = array,
                total = AttentionShops.Total
            };
            return Json(dataGridModel);
        }
        
        public ActionResult Detail(long Id)
        {
            Attention AttentionInfo = ServiceHelper.Create<IAttentionService>().GetAttention(Id);
            return View(AttentionInfo);
        }


        
    }
}