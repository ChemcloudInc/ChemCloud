using ChemCloud.Web.Framework;
using System;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Mobile.Controllers
{
	public class DebugController : BaseMobileTemplatesController
	{
		public DebugController()
		{
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}