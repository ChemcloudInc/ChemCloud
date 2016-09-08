using ChemCloud.IServices;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ChemCloud.Web.Controllers
{
	public class RegionAPIController : Controller
	{
		public RegionAPIController()
		{
		}

		[HttpPost]
		public JsonResult GetRegion(long? key = null, int? level = -1)
		{
			int? nullable = level;
			if ((nullable.GetValueOrDefault() != -1 ? false : nullable.HasValue))
			{
				key = new long?(0);
			}
			if (!key.HasValue)
			{
				return Json(new object[0]);
			}
			IEnumerable<KeyValuePair<long, string>> region = Instance<IRegionService>.Create.GetRegion(key.Value);
			return Json(region);
		}
	}
}