using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
	public class OperationLogController : BaseSellerController
	{
		public OperationLogController()
		{
		}

		[Description("关键字获取管理员用户名列表")]
		[UnAuthorize]
		public JsonResult GetManagers(string keyWords)
		{
			long shopId = base.CurrentSellerManager.ShopId;
			IQueryable<ManagerInfo> managers = 
				from item in ServiceHelper.Create<IManagerService>().GetManagers(keyWords)
				where item.ShopId == shopId
				select item;
			var variable = 
				from item in managers
				select new { key = item.Id, @value = item.UserName };
			return Json(variable);
		}

		[Description("分页获取日志的JSON数据")]
		[UnAuthorize]
		public JsonResult List(int page, string userName, int rows, DateTime? startDate, DateTime? endDate)
		{
			OperationLogQuery operationLogQuery = new OperationLogQuery()
			{
				UserName = userName,
				PageNo = page,
				ShopId = base.CurrentSellerManager.ShopId,
				PageSize = rows,
				StartDate = startDate,
				EndDate = endDate
			};
			PageModel<LogInfo> platformOperationLogs = ServiceHelper.Create<IOperationLogService>().GetPlatformOperationLogs(operationLogQuery);
			var list = 
				from item in platformOperationLogs.Models.ToList()
				select new { Id = item.Id, UserName = item.UserName, PageUrl = item.PageUrl, Description = item.Description, Date = item.Date.ToString("yyyy-MM-dd HH:mm"), IPAddress = item.IPAddress };
			return Json(new { rows = list, total = platformOperationLogs.Total });
		}

		[Description("商家日志管理页面")]
		public ActionResult Management()
		{
			return View();
		}
	}
}