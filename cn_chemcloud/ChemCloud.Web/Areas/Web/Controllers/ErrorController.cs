using ChemCloud.Web.Framework;
using System;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
	public class ErrorController : BaseController
	{
		public ErrorController()
		{
		}

		public ActionResult Error404()
		{
			return View();
		}
	}
}