using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
	public class ArticleController : BaseSellerController
	{
		public ArticleController()
		{
		}

		[UnAuthorize]
		public ActionResult Details(long id)
		{
			return View(ServiceHelper.Create<IArticleService>().GetArticle(id));
		}

		[UnAuthorize]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[UnAuthorize]
		public JsonResult List(int rows, int page)
		{
			ArticleCategoryInfo specialArticleCategory = ServiceHelper.Create<IArticleCategoryService>().GetSpecialArticleCategory(SpecialCategory.PlatformNews);
            long? CategoryId = 1;
            PageModel<ArticleInfo> pageModel = ServiceHelper.Create<IArticleService>().Find(CategoryId, "", rows, page, false);
			var array = 
				from item in pageModel.Models.ToArray()
                select new { id = item.Id, addDate = item.AddDate.ToString("yyyy-MM-dd"), title = item.Title };
			return Json(new { rows = array, total = pageModel.Total });
		}
	}
}