using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Admin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class ProductLevelController : Controller
    {
        public ProductLevelController()
        {
        }

        public ActionResult Management()
        {
            return View();
        }
        [HttpPost]
        public JsonResult List(int page, int rows, string levelNameCN, string levelNameEN) //string companyName,
        {
            ProductLevelQuery productLevelQuery = new ProductLevelQuery();
            if(!string.IsNullOrWhiteSpace(levelNameCN))
            {
                productLevelQuery.ProductLevelCN = levelNameCN;
            }
            if (!string.IsNullOrWhiteSpace(levelNameEN))
            {
                productLevelQuery.ProductLevelEN = levelNameEN;
            }
            productLevelQuery.PageSize = rows;
            productLevelQuery.PageNo = page;
            PageModel<ProductLevel> productLevels = ServiceHelper.Create<IProductLevelService>().GetProductLevels(productLevelQuery);
            IProductLevelService levelService = ServiceHelper.Create<IProductLevelService>();
            IEnumerable<ProductLevelModel> array =
                from item in productLevels.Models.ToArray()
                select new ProductLevelModel()
                {
                    Id = item.Id,
                    LevelNameCN = item.LevelNameCN,
                    LevelNameEN = item.LevelNameEN
                    
                };
            DataGridModel<ProductLevelModel> dataGridModel = new DataGridModel<ProductLevelModel>()
            {
                rows = array,
                total = productLevels.Total
            };
            return Json(dataGridModel);
        }
        [HttpPost]
        public JsonResult UpdateProductLevel(long Id,string nameCN,string nameEN)
        {
            ProductLevel productLevel = ServiceHelper.Create<IProductLevelService>().GetProductLevel(Id);
            productLevel.LevelNameCN = nameCN;
            productLevel.LevelNameEN = nameEN;
            bool flag = ServiceHelper.Create<IProductLevelService>().UpdateProductLevel(productLevel);
            return Json(new { success = flag });
        }
        [HttpPost]
        public JsonResult AddProductLevel(string nameCN,string nameEN)
        {
            ProductLevel productlevel = new ProductLevel();
            productlevel.LevelNameCN = nameCN;
            productlevel.LevelNameEN = nameEN;
            bool flag = ServiceHelper.Create<IProductLevelService>().AddProductLevel(productlevel);
            return Json(new { success = flag }); 
        }
        [HttpPost]
        public JsonResult DeleteProductLevel(long id)
        {
            bool flag = ServiceHelper.Create<IProductLevelService>().DeleteProductLevel(id);
            return Json(new { success = flag }); 
        }
        [HttpPost]
        public JsonResult BatchDelete(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            ServiceHelper.Create<IProductLevelService>().BatchDelete(nums.ToArray());
            Result result = new Result()
            {
                success = true,
                msg = "批量删除成功！"
            };
            return Json(result);
        }
    }
}