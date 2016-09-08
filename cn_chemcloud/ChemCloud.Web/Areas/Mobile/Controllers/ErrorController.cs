using ChemCloud.Web.Framework;
using System;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Mobile.Controllers
{
	public class ErrorController : BaseMobileTemplatesController
	{
		public ErrorController()
		{
		}

		public ActionResult Error()
		{
			return View();
		}
	}
}