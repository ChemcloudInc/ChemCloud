using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.Admin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class SiteServiceController : BaseWebController
    {
        // GET: Web/SiteService
        public ActionResult CustomsClearanceAndTax()
        {
            return View();
        }

        public ActionResult FieldCertification()
        {
            return View();
        }

        public ActionResult FinancialService()
        {
            return View();
        }

        public ActionResult RecommendEnterprise()
        {
            return View();
        }
        public ActionResult ProductCertification()
        {
            return View();
        }
        public ActionResult RegionalAndOverseasStorag()
        {
            return View();
        }
        public ActionResult OnBehalfOf()
        {
            return View();
        }
    }
}