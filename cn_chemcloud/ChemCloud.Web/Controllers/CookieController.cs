using ChemCloud.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Controllers
{
    public class CookieController : Controller
    {
        // GET: Cookie
        public JsonResult logOut()
        {
            WebHelper.DeleteCookie("ChemCloud-User");
            WebHelper.DeleteCookie("ChemCloud-SellerManager");
            return Json(new { success = true });
        } 
    }
}