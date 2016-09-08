using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
	public class OrderCommentController : BaseAdminController
	{
		public OrderCommentController()
		{
		}

		[HttpPost]
		[OperationLog(Message="确认结算")]
		[UnAuthorize]
		public JsonResult Delete(int id)
		{
			ServiceHelper.Create<ITradeCommentService>().DeleteOrderComment(id);
			Result result = new Result()
			{
				success = true,
				msg = "删除成功！"
			};
			return Json(result);
		}

		[HttpPost]
		[UnAuthorize]
		public JsonResult List(DateTime? startDate, DateTime? endDate, long? orderId, string shopName, string userName, int page, int rows)
		{
			OrderCommentQuery orderCommentQuery = new OrderCommentQuery()
			{
				StartDate = startDate,
				EndDate = endDate,
				OrderId = orderId,
				//ShopName = shopName,
				UserName = userName,
				PageSize = rows,
				PageNo = page
			};
			PageModel<OrderCommentInfo> orderComments = ServiceHelper.Create<ITradeCommentService>().GetOrderComments(orderCommentQuery);
			var array = 
				from item in orderComments.Models.ToArray()
                select new { Id = item.Id, OrderId = item.OrderId, ShopName = item.ShopName,CompanyName = item.CompanyName,UserName = item.UserName, CommentDate = item.CommentDate.ToShortDateString(), PackMark = item.PackMark, DeliveryMark = item.DeliveryMark, ServiceMark = item.ServiceMark };
			return Json(new { rows = array, total = orderComments.Total });
		}

		public ActionResult Management()
		{
			return View();
		}
	}
}