using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
	public class InviteController : Controller
	{
		public InviteController()
		{
		}

		public ActionResult Index()
		{
			return View(ServiceHelper.Create<IMemberInviteService>().GetInviteRule());
		}
	}
}