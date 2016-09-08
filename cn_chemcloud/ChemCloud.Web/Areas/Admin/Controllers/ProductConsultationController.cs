using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
	public class ProductConsultationController : BaseAdminController
	{
		public ProductConsultationController()
		{
		}

		[HttpPost]
		[UnAuthorize]
		public JsonResult Delete(long id)
		{
			ServiceHelper.Create<IConsultationService>().DeleteConsultation(id);
			Result result = new Result()
			{
				success = true,
				msg = "删除成功！"
			};
			return Json(result);
		}

		[HttpPost]
		[UnAuthorize]
		public JsonResult Detail(long id)
		{
			ProductConsultationInfo consultation = ServiceHelper.Create<IConsultationService>().GetConsultation(id);
			return Json(new { ConsulationContent = consultation.ConsultationContent, ReplyContent = consultation.ReplyContent });
		}

		public static string HTMLEncode(string txt)
		{
			if (string.IsNullOrEmpty(txt))
			{
				return string.Empty;
			}
			string str = txt.Replace(" ", "&nbsp;");
			str = str.Replace("<", "&lt;");
			str = str.Replace(">", "&gt;");
			str = str.Replace("\"", "&quot;");
			return str.Replace("'", "&#39;").Replace("\n", "<br>");
		}

		[UnAuthorize]
		public JsonResult List(int page, int rows, string Keywords, int shopid = 0, bool? isReply = null)
		{
			ConsultationQuery consultationQuery = new ConsultationQuery()
			{
				PageNo = page,
				PageSize = rows,
				KeyWords = Keywords,
				ShopID = shopid,
				IsReply = isReply
			};
			PageModel<ProductConsultationInfo> consultations = ServiceHelper.Create<IConsultationService>().GetConsultations(consultationQuery);
			IEnumerable<ProductConsultationModel> array = 
				from item in consultations.Models.ToArray()
                select new ProductConsultationModel()
				{
					Id = item.Id,
					ConsultationContent = ProductConsultationController.HTMLEncode(item.ConsultationContent),
					Date = item.ConsultationDate.ToString("yyyy-MM-dd HH:mm:ss"),
					ProductName = item.ProductInfo.ProductName,
					ProductId = item.ProductId,
					UserName = item.UserName,
					ReplyContent = ProductConsultationController.HTMLEncode(item.ReplyContent),
					ImagePath = item.ProductInfo.GetImage(ProductInfo.ImageSize.Size_50, 1),
					ReplyDate = item.ReplyDate
				};
			DataGridModel<ProductConsultationModel> dataGridModel = new DataGridModel<ProductConsultationModel>()
			{
				rows = array,
				total = consultations.Total
			};
			return Json(dataGridModel);
		}

		public ActionResult Management()
		{
			return View();
		}
	}
}