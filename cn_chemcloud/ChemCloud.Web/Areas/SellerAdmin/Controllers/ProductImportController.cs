using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChemCloud.DBUtility;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using System.Diagnostics;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class ProductImportController : BaseSellerController
    {
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string filePath)
        {
            DataTable data = new DataTable();
            int startRow = 0;
            IWorkbook wk = null;
            string extension = System.IO.Path.GetExtension(filePath);
            try
            {
                FileStream fs = System.IO.File.OpenRead(filePath);
                if (extension.Equals(".xls") || extension.Equals(".XLS"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }

                fs.Close();
                //读取当前表数据
                ISheet sheet = wk.GetSheetAt(0);

                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {
                    //先设置Cell的类型
                    GetCellValue(firstRow.GetCell(i));

                    DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                    data.Columns.Add(column);
                }
                startRow = sheet.FirstRowNum + 1;

                //最后一列的标号
                int rowCount = sheet.LastRowNum;
                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　

                    DataRow dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                        {
                            //先设置Cell的类型
                            GetCellValue(firstRow.GetCell(j));

                            dataRow[j] = row.GetCell(j).ToString();
                        }
                    }
                    data.Rows.Add(dataRow);
                }

                return data;
            }
            catch (Exception e)
            {
                //只在Debug模式下才输出
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /*获取cell的数据，并设置为对应的数据类型*/
        public object GetCellValue(ICell cell)
        {
            object value = null;
            try
            {
                if (cell.CellType != CellType.Blank)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            // Date Type的数据CellType是Numeric
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                value = cell.DateCellValue;
                            }
                            else
                            {
                                // Numeric type
                                value = cell.NumericCellValue;
                            }
                            break;
                        case CellType.Boolean:
                            // Boolean type
                            value = cell.BooleanCellValue;
                            break;
                        default:
                            // String type
                            value = cell.StringCellValue;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                value = "";
            }

            return value;
        }

        private long _shopid;

        private long _userid;
        public ProductImportController()
        {
            _shopid = base.CurrentSellerManager.ShopId;
            _userid = base.CurrentSellerManager.Id;
        }

        public JsonResult GetImportCount()
        {
            long num = 0;
            long num1 = 0;
            int num2 = 0;
            GetImportCountFromCache(out num, out num1);
            if (num1 == num && num1 > 0)
            {
                num2 = 1;
            }
            return Json(new { Count = num, Total = num1, Success = num2 }, JsonRequestBehavior.AllowGet);
        }

        private void GetImportCountFromCache(out long count, out long total)
        {
            object obj = Cache.Get(CacheKeyCollection.UserImportProductCount(_userid));
            object obj1 = Cache.Get(CacheKeyCollection.UserImportProductTotal(_userid));
            count = (obj == null ? 0 : long.Parse(obj.ToString()));
            total = (obj1 == null ? 0 : long.Parse(obj1.ToString()));
            if (count == total && total > 0)
            {
                Cache.Remove(CacheKeyCollection.UserImportProductCount(_userid));
                Cache.Remove(CacheKeyCollection.UserImportProductTotal(_userid));
            }
        }

        public JsonResult GetImportOpCount()
        {
            long num = 0;
            object obj = Cache.Get("Cache-UserImportOpCount");
            if (obj != null)
            {
                num = (string.IsNullOrEmpty(obj.ToString()) ? 0 : long.Parse(obj.ToString()));
            }
            return Json(new { Count = num }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult GetPlatFormCategory(long? key = null, int? level = -1)
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
            IEnumerable<CategoryInfo> categoryByParentId = ServiceHelper.Create<ICategoryService>().GetCategoryByParentId(key.Value);
            long? nullable1 = key;
            if ((nullable1.GetValueOrDefault() != 0 ? false : nullable1.HasValue))
            {
                IShopService shopService = ServiceHelper.Create<IShopService>();
                if (!(shopService.GetShop(base.CurrentSellerManager.ShopId, false) ?? new ShopInfo()).IsSelf)
                {
                    IQueryable<long> businessCategory =
                        from e in shopService.GetBusinessCategory(base.CurrentSellerManager.ShopId)
                        select e.CategoryId;
                    categoryByParentId = ServiceHelper.Create<ICategoryService>().GetTopLevelCategories(businessCategory);
                }
            }
            IEnumerable<KeyValuePair<long, string>> keyValuePair =
                from item in categoryByParentId
                select new KeyValuePair<long, string>(item.Id, item.Name);
            return Json(keyValuePair);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult GetPlatFormCategoryNew(long? key = null, int? level = -1)
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
            IEnumerable<CategoryInfo> categoryByParentId = ServiceHelper.Create<ICategoryService>().GetCategoryByParentId(key.Value);
            long? nullable1 = key;
            if ((nullable1.GetValueOrDefault() != 0 ? false : nullable1.HasValue))
            {
                IShopService shopService = ServiceHelper.Create<IShopService>();
                if (!(shopService.GetShop(base.CurrentSellerManager.ShopId, false) ?? new ShopInfo()).IsSelf)
                {
                    IQueryable<long> businessCategory =
                        from e in shopService.GetBusinessCategory(base.CurrentSellerManager.ShopId)
                        select e.CategoryId;
                    categoryByParentId = ServiceHelper.Create<ICategoryService>().GetTopLevelCategoriesNew(businessCategory);
                }
            }
            IEnumerable<KeyValuePair<long, string>> keyValuePair =
                from item in categoryByParentId
                select new KeyValuePair<long, string>(item.Id, item.Name);
            return Json(keyValuePair);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult GetShopBrand(long categoryId)
        {
            IBrandService brandService = ServiceHelper.Create<IBrandService>();
            long shopId = base.CurrentSellerManager.ShopId;
            long[] numArray = new long[] { categoryId };
            IEnumerable<KeyValuePair<long, string>> brandsByCategoryIds =
                from item in brandService.GetBrandsByCategoryIds(shopId, numArray)
                select new KeyValuePair<long, string>(item.Id, item.Name);
            return Json(brandsByCategoryIds);
        }

        public ActionResult ImportManage()
        {
            long num = 0;
            long num1 = 0;
            int num2 = 0;
            GetImportCountFromCache(out num, out num1);
            if (num1 == num && num1 > 0)
            {
                num2 = 1;
            }
            ViewBag.Count = num;
            ViewBag.Total = num1;
            ViewBag.Success = num2;
            ViewBag.shopid = _shopid;
            ViewBag.userid = _userid;
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel()
        {
            string str = "NoExcel";
            HttpPostedFileBase item = null;
            if (base.Request.Files.Count > 0)
            {
                var objfile = base.Request.Files[0];
                if (objfile != null)
                {
                    item = objfile;
                }
                //HttpPostedFileBase item = base.Request.Files[0];
                if (item != null)
                {
                    string serverFilepath = Server.MapPath(string.Format("/Storage/Shop/{0}/ExcelTemplate ", base.CurrentSellerManager.ShopId));
                    string serverFileName = "BatchImportProduct" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".XLS";
                    string File = serverFilepath + '\\' + serverFileName;
                    if (!System.IO.Directory.Exists(serverFilepath))
                    {
                        System.IO.Directory.CreateDirectory(serverFilepath);
                    }
                    try
                    {
                        object obj = Cache.Get("Cache-UserImportOpCount");
                        if (obj != null)
                        {
                            Cache.Insert("Cache-UserImportOpCount", int.Parse(obj.ToString()) + 1);
                        }
                        else
                        {
                            Cache.Insert("Cache-UserImportOpCount", 1);
                        }
                        item.SaveAs(File);
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        object obj1 = Cache.Get("Cache-UserImportOpCount");
                        if (obj1 != null)
                        {
                            Cache.Insert("Cache-UserImportOpCount", int.Parse(obj1.ToString()) - 1);
                        }
                        Log.Error(string.Concat("产品导入上传文件异常：", exception.Message));
                        str = "Error";
                    }
                    str = File;

                }
                else
                {
                    str = "文件长度为0,格式异常。";
                }
            }
            return base.Content(str, "text/html", System.Text.Encoding.UTF8);
        }
        private DataTable GetExcelData(string FilePath)
        {
            DataTable dt = ExcelToDataTable(FilePath);
            return dt;
        }

        [HttpPost]
        public JsonResult UpLoadFile(long shopid, string filename)
        {
            /*供应商产品批量导入*/
            if (!string.IsNullOrWhiteSpace(filename))
            {
                int ErrorCount = 0, SubErrorCount = 0, RepeatCount = 0, SuccessCount = 0, NullCount = 0;
                DataTable dt = GetExcelData(filename);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Product_Number"] == null || dt.Rows[i]["Chemical_Name"] == null
                            || dt.Rows[i]["FirstCategory"] == null || dt.Rows[i]["SecondCategory"] == null || dt.Rows[i]["ThirdCategory"] == null)
                        {
                            continue;
                        }
                        if ((ServiceHelper.Create<IProductService>().IsExitsProductCode(shopid, dt.Rows[i]["Product_Number"].ToString())))
                        {
                            RepeatCount = RepeatCount + 1;
                            continue;
                        }
                        try
                        {
                            CategoryInfo categoryInfo = ServiceHelper.Create<ICategoryService>().GetCategoryByName(dt.Rows[i]["ThirdCategory"].ToString().Trim());
                            ProductInfo Product = new ProductInfo();
                            /*供应商编号*/
                            Product.ShopId = shopid;
                            /*产品分类*/
                            Product.CategoryId = (categoryInfo == null ? 0 : categoryInfo.Id);
                            Product.CategoryPath = (categoryInfo == null ? "查找不到路径" : categoryInfo.Path);
                            /*产品货号*/
                            Product.ProductCode = dt.Rows[i]["Product_Number"] == null ? "" : dt.Rows[i]["Product_Number"].ToString();
                            /*产品名*/
                            Product.EProductName = dt.Rows[i]["Chemical_Name"] == null ? "" : dt.Rows[i]["Chemical_Name"].ToString();
                            /*中文名*/
                            Product.ProductName = dt.Rows[i]["Chemical_ChineseName"] == null ? "" : dt.Rows[i]["Chemical_ChineseName"].ToString();
                            /*cas#*/
                            if (dt.Rows[i]["CASNO"] != null && !string.IsNullOrWhiteSpace(dt.Rows[i]["CASNO"].ToString()))
                            {
                                Product.CASNo = dt.Rows[i]["CASNO"].ToString();
                                /*Pub_CID*/
                                CASInfo cas = ServiceHelper.Create<ICASInfoService>().GetCASByCASNO(Product.CASNo);
                                if (cas == null)
                                {
                                    Product.Pub_CID = 0;
                                }
                                else
                                {
                                    Product.Pub_CID = cas.Pub_CID;
                                }
                            }
                            else
                            {
                                Product.CASNo = "";
                            }


                            Product.FreightTemplateId = 138;
                            Product.AddedDate = DateTime.Now;
                            Product.PackagingLevel = dt.Rows[i]["Quantity1"] == null ? "" : dt.Rows[i]["Quantity1"].ToString();
                            Product.Purity = dt.Rows[i]["Purity1"] == null ? "" : dt.Rows[i]["Purity1"].ToString();
                            CASInfo casInfo = ServiceHelper.Create<ICASInfoService>().GetCASByNo(dt.Rows[i]["CASNO"].ToString());
                            if (casInfo != null)
                            {
                                Product.MolecularFormula = casInfo.Molecular_Formula;
                                Product.MolecularWeight = casInfo.Molecular_Weight;
                                Product.LogP = casInfo.LogP;
                                Product.FusingPoint = casInfo.UN_Number;
                                Product.BoilingPoint = casInfo.RTECS_Number;
                                Product.FlashPoint = casInfo.LogS;
                                Product.VapourPressure = casInfo.Vapor_Pressure;
                                Product.DangerousMark = casInfo.Density;
                            }
                            if (!string.IsNullOrWhiteSpace(dt.Rows[i]["Price1"].ToString()))
                                Product.MinSalePrice = Convert.ToDecimal(dt.Rows[i]["Price1"].ToString());
                            else
                                Product.MinSalePrice = 0;
                            Product.Plat_Code = DateTime.Now.ToString("yyyyMMddHHmmssffff").ToString();
                            ServiceHelper.Create<IProductService>().AddProduct(Product);
                            ProductInfo productInfo = ServiceHelper.Create<IProductService>().GetProductByCondition(shopid, dt.Rows[i]["Product_Number"].ToString());
                            if (productInfo == null)
                            {
                                NullCount = NullCount + 1;
                            }
                            else
                            {
                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(dt.Rows[i]["Grade1"].ToString()))
                                    {
                                        ProductSpec productSpec1 = new ProductSpec();
                                        productSpec1.ProductId = productInfo.Id;
                                        productSpec1.Packaging = dt.Rows[i]["Quantity1"].ToString();
                                        productSpec1.Purity = dt.Rows[i]["Purity1"].ToString();
                                        if (!string.IsNullOrWhiteSpace(dt.Rows[i]["Price1"].ToString()))
                                            productSpec1.Price = Convert.ToDecimal(dt.Rows[i]["Price1"].ToString());
                                        else
                                            productSpec1.Price = 0;
                                        productSpec1.SpecLevel = dt.Rows[i]["Grade1"].ToString();
                                        if (dt.Rows[i]["Currency1"].ToString() == "CNY")
                                            productSpec1.CoinType = 1;
                                        if (dt.Rows[i]["Currency1"].ToString() == "USD")
                                            productSpec1.CoinType = 2;
                                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["Currency1"].ToString()))
                                        {
                                            int LanguageId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
                                            if (LanguageId == 1)
                                                productSpec1.CoinType = 1;
                                            if (LanguageId == 2)
                                                productSpec1.CoinType = 2;
                                        }
                                        ServiceHelper.Create<IProductService>().AddProductSpecs(productSpec1);
                                    }

                                    if (!string.IsNullOrWhiteSpace(dt.Rows[i]["Grade2"].ToString()))
                                    {
                                        ProductSpec productSpec2 = new ProductSpec();
                                        productSpec2.ProductId = productInfo.Id;
                                        productSpec2.Packaging = dt.Rows[i]["Quantity2"].ToString();
                                        productSpec2.Purity = dt.Rows[i]["Purity2"].ToString();
                                        if (!string.IsNullOrWhiteSpace(dt.Rows[i]["Price2"].ToString()))
                                            productSpec2.Price = Convert.ToDecimal(dt.Rows[i]["Price2"].ToString());
                                        else
                                            productSpec2.Price = 0;
                                        productSpec2.SpecLevel = dt.Rows[i]["Grade2"].ToString();
                                        if (dt.Rows[i]["Currency2"].ToString() == "CNY")
                                            productSpec2.CoinType = 1;
                                        if (dt.Rows[i]["Currency2"].ToString() == "USD")
                                            productSpec2.CoinType = 2;
                                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["Currency2"].ToString()))
                                        {
                                            int LanguageId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
                                            if (LanguageId == 1)
                                                productSpec2.CoinType = 1;
                                            if (LanguageId == 2)
                                                productSpec2.CoinType = 2;
                                        }
                                        ServiceHelper.Create<IProductService>().AddProductSpecs(productSpec2);
                                    }

                                    if (!string.IsNullOrWhiteSpace(dt.Rows[i]["Grade3"].ToString()))
                                    {
                                        ProductSpec productSpec3 = new ProductSpec();
                                        productSpec3.ProductId = productInfo.Id;
                                        productSpec3.Packaging = dt.Rows[i]["Quantity3"].ToString();
                                        productSpec3.Purity = dt.Rows[i]["Purity3"].ToString();
                                        if (!string.IsNullOrWhiteSpace(dt.Rows[i]["Price3"].ToString()))
                                            productSpec3.Price = Convert.ToDecimal(dt.Rows[i]["Price3"].ToString());
                                        else
                                            productSpec3.Price = 0;
                                        productSpec3.SpecLevel = dt.Rows[i]["Grade3"].ToString();
                                        if (dt.Rows[i]["Currency3"].ToString() == "CNY")
                                            productSpec3.CoinType = 1;
                                        if (dt.Rows[i]["Currency3"].ToString() == "USD")
                                            productSpec3.CoinType = 2;
                                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["Currency3"].ToString()))
                                        {
                                            int LanguageId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
                                            if (LanguageId == 1)
                                                productSpec3.CoinType = 1;
                                            if (LanguageId == 2)
                                                productSpec3.CoinType = 2;
                                        }
                                        ServiceHelper.Create<IProductService>().AddProductSpecs(productSpec3);
                                    }
                                }
                                catch
                                {
                                    SubErrorCount = SubErrorCount + 1;
                                }
                            }
                        }
                        catch
                        {
                            ErrorCount = ErrorCount + 1;
                        }
                        if (ErrorCount == 0 && SubErrorCount == 0 && NullCount == 0 && RepeatCount == 0)
                        { SuccessCount = SuccessCount + 1; }
                    }
                }
                if (ErrorCount > 0 && SubErrorCount > 0)
                {
                    return Json(new { success = true, message = 2, ErrorCount = ErrorCount, SubErrorCount = SubErrorCount, SuccessCount = SuccessCount });//5商品已经导入," + "有" + ErrorCount + "条数据有误," + "有" + SubErrorCount + "条等级相关数据有错误。
                }
                else if (ErrorCount > 0)
                {
                    return Json(new { success = true, message = 3, ErrorCount = ErrorCount, SuccessCount = SuccessCount });//"4商品已经导入," + "有" + ErrorCount + "条数据有误。"
                }
                else if (SubErrorCount > 0)
                {
                    return Json(new { success = true, message = 4, SubErrorCount = SubErrorCount, SuccessCount = SuccessCount });//"3商品已经导入," + "有" + SubErrorCount + "条等级相关数据有错误。"
                }
                else if (RepeatCount > 0)
                {
                    return Json(new { success = true, message = 1, RepeatCount = RepeatCount, SuccessCount = SuccessCount });
                }
                else if (NullCount > 0)
                {
                    return Json(new { success = true, message = 7, NullCount = NullCount, SuccessCount = SuccessCount });
                }
                else
                {
                    return Json(new { success = true, message = 5, SuccessCount = SuccessCount });//"1所有数据成功导入商品！"
                }
            }
            else
            {
                return Json(new { success = false, message = 6 });//"2没有找到文件！"
            }
        }



        [HttpPost]
        public JsonResult NewImportProducts(long shopid, string filename)
        {
            /*供应商产品批量导入*/
            if (!string.IsNullOrWhiteSpace(filename))
            {
                /*计数统计*/
                int ErrorCount = 0, SubErrorCount = 0, RepeatCount = 0, SuccessCount = 0, NullCount = 0;
                /*读取excel*/
                DataTable dt = GetExcelData(filename);

                dt.Rows.RemoveAt(0); //移除第一行

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        /*必填项 ,如果存在空的值，就跳过*/
                        if (dt.Rows[i]["SupplierProductId"] == null
                            || dt.Rows[i]["EnglishDescription"] == null
                            || dt.Rows[i]["PackSize"] == null
                            || dt.Rows[i]["RegulatedProduct"] == null
                            || dt.Rows[i]["RegulatoryCountry"] == null
                            || dt.Rows[i]["RMB_CN"] == null)
                        {
                            NullCount = NullCount + 1;
                            continue;
                        }



                        /*产品货号唯一*/
                        if ((ServiceHelper.Create<IProductService>().IsExitsProductCode(shopid, dt.Rows[i]["SupplierProductId"].ToString())))
                        {
                            RepeatCount = RepeatCount + 1;
                            continue;
                        }
                        try
                        {
                            ProductInfo Product = new ProductInfo();
                            Product.ShopId = shopid; /*供应商编号*/
                            Product.CategoryId = 80;/*产品分类*/
                            Product.CategoryPath = "2|76|80";
                            Product.ProductCode = dt.Rows[i]["SupplierProductId"] == null ? "" : dt.Rows[i]["SupplierProductId"].ToString();  /*产品货号*/
                            Product.EProductName = dt.Rows[i]["EnglishDescription"] == null ? "" : dt.Rows[i]["EnglishDescription"].ToString();/*英文描述*/
                            Product.ProductName = dt.Rows[i]["ChineseDescription"] == null ? "" : dt.Rows[i]["ChineseDescription"].ToString();/*中文名*/

                            Product.ShortDescription = dt.Rows[i]["RegulatedProduct"].ToString();/*是否属于管制品(Y/N)*/
                            Product.RelatedProducts = dt.Rows[i]["RegulatoryCountry"].ToString();/*管制国家*/

                            Product.Purity = dt.Rows[i]["Purity"] == null ? "" : dt.Rows[i]["Purity"].ToString();
                            Product.PackagingLevel = dt.Rows[i]["PackingGroup"] == null ? "" : dt.Rows[i]["PackingGroup"].ToString();
                            Product.Packaging = dt.Rows[i]["PackSize"] == null ? "" : dt.Rows[i]["PackSize"].ToString();

                            Product.MeasureUnit = dt.Rows[i]["SKU"] == null ? "" : dt.Rows[i]["SKU"].ToString();
                            string count = dt.Rows[i]["TotalStockAmount"] == null ? "0" : dt.Rows[i]["TotalStockAmount"].ToString();
                            if (string.IsNullOrEmpty(count)) { Product.SaleCounts = 0; } else { Product.SaleCounts = long.Parse(count); }

                            //Formula	 --分子式 MolecularFormula
                            Product.MolecularFormula = dt.Rows[i]["Formula"] == null ? "" : dt.Rows[i]["Formula"].ToString();
                            //MolecularWeight	 --分子量 MolecularWeight
                            Product.MolecularWeight = dt.Rows[i]["MolecularWeight"] == null ? "" : dt.Rows[i]["MolecularWeight"].ToString();
                            //PhysicalForm	 --物理状态 Shape
                            Product.Shape = dt.Rows[i]["PhysicalForm"] == null ? "" : dt.Rows[i]["PhysicalForm"].ToString();
                            //Solubility	--溶解性 SyntheticRoute
                            Product.SyntheticRoute = dt.Rows[i]["Solubility"] == null ? "" : dt.Rows[i]["Solubility"].ToString();
                            //Density	 --密度 Density
                            Product.Density = dt.Rows[i]["Density"] == null ? "" : dt.Rows[i]["Density"].ToString();
                            //MeltingPoint	 --熔点 FusingPoint
                            Product.FusingPoint = dt.Rows[i]["MeltingPoint"] == null ? "" : dt.Rows[i]["MeltingPoint"].ToString();
                            //BoilingPoint	 --沸点 BoilingPoint
                            Product.BoilingPoint = dt.Rows[i]["BoilingPoint"] == null ? "" : dt.Rows[i]["BoilingPoint"].ToString();
                            //FlashPoint	--闪点	 FlashPoint
                            Product.FlashPoint = dt.Rows[i]["FlashPoint"] == null ? "" : dt.Rows[i]["FlashPoint"].ToString();
                            //RefractiveIndex	 --折射率 RefractiveIndex
                            Product.RefractiveIndex = dt.Rows[i]["RefractiveIndex"] == null ? "" : dt.Rows[i]["RefractiveIndex"].ToString();
                            //StorageConditions	 --存储条件 StorageConditions
                            Product.StorageConditions = dt.Rows[i]["StorageConditions"] == null ? "" : dt.Rows[i]["StorageConditions"].ToString();
                            //EINECS	 --EINECS编码 RETCS
                            Product.RETCS = dt.Rows[i]["EINECS"] == null ? "" : dt.Rows[i]["EINECS"].ToString();
                            //HazardClass	 --危险等级 DangerLevel
                            Product.DangerLevel = dt.Rows[i]["HazardClass"] == null ? "" : dt.Rows[i]["HazardClass"].ToString();
                            //HSCode	 --海关编码 HSCODE
                            Product.HSCODE = dt.Rows[i]["HSCode"] == null ? "" : dt.Rows[i]["HSCode"].ToString();
                            //ShelfLife	 --保质期 NMRSpectrum
                            Product.NMRSpectrum = dt.Rows[i]["ShelfLife"] == null ? "" : dt.Rows[i]["ShelfLife"].ToString();
                            //MSDSFile --MSDS文件 MSDS
                            Product.MSDS = dt.Rows[i]["MSDSFile"] == null ? "" : dt.Rows[i]["MSDSFile"].ToString();
                            //CoAFile	--质检文件 RefuseReason
                            Product.RefuseReason = dt.Rows[i]["CoAFile"] == null ? "" : dt.Rows[i]["CoAFile"].ToString();

                            /*cas#*/
                            if (dt.Rows[i]["CAS#"] != null && !string.IsNullOrWhiteSpace(dt.Rows[i]["CAS#"].ToString()))
                            {
                                Product.CASNo = dt.Rows[i]["CAS#"].ToString();
                                /*Pub_CID*/
                                int newId = 0;
                                /*根据cas#查询chemcloud_cas表数据*/
                                string pub_cid = DbHelperSQLCAS.ExecuteScalar("SELECT PUB_CID FROM ChemCloud_CAS with(nolock) WHERE CAS='" + Product.CASNo + "'");
                                if (!string.IsNullOrEmpty(pub_cid))
                                {
                                    Product.Pub_CID = int.Parse(pub_cid);
                                }
                                else
                                {
                                    /*cas库不存在，插入cas库*/
                                    newId = DbHelperSQLCAS.GetMaxID("PUB_CID", "ChemCloud_CAS");
                                    if (newId >= 200000000)
                                    {
                                        newId = newId + 1;
                                    }
                                    else
                                    {
                                        newId = 200000000;
                                    }
                                    Product.Pub_CID = newId;


                                    string strsql = string.Format(@" 
                                    INSERT INTO dbo.ChemCloud_CAS(PUB_CID,CAS,CHINESE,Record_Title,CHINESE_ALIAS,Record_Description,Molecular_Formula,Molecular_Weight,
                                    Density,Boiling_Point,Flash_Point,Vapor_Pressure,Exact_Mass,LogP,Physical_Description,Melting_Point,Solubility,Storage_Conditions,Safety_and_Hazards,HS_CODE,[2D_Structure]) 
                                    VALUES('" + newId + "','" + Product.CASNo + "','" + Product.ProductName + "','" + Product.EProductName + "','"
                                   + Product.Alias + "','" + Product.Ealias + "','" + Product.MolecularFormula
                                   + "','" + Product.MolecularWeight + "','" + Product.Density + "','" + Product.BoilingPoint
                                   + "','" + Product.FlashPoint + "','" + Product.VapourPressure + "','" + Product.PASNo + "','"
                                   + Product.LogP + "','" + Product.Shape + "','" + Product.FusingPoint + "','"
                                   + Product.RefractiveIndex + "','" + Product.StorageConditions + "','" + Product.SafetyInstructions + "','" + Product.HSCODE + "','')");

                                    int result = DbHelperSQLCAS.ExecuteSql(strsql);

                                }
                            }
                            else
                            {
                                /*Pub_CID*/
                                int newId = 0;
                                /*根据英文查询chemcloud_cas表数据*/
                                string pub_cid = DbHelperSQLCAS.ExecuteScalar("SELECT PUB_CID FROM ChemCloud_CAS with(nolock) WHERE Record_Title='" + Product.EProductName + "'");
                                if (!string.IsNullOrEmpty(pub_cid))
                                {
                                    Product.Pub_CID = int.Parse(pub_cid);
                                }
                                else
                                {
                                    /*cas库不存在，插入cas库*/
                                    newId = DbHelperSQLCAS.GetMaxID("PUB_CID", "ChemCloud_CAS");
                                    if (newId >= 200000000)
                                    {
                                        newId = newId + 1;
                                    }
                                    else
                                    {
                                        newId = 200000000;
                                    }
                                    Product.Pub_CID = newId;

                                    string strsql = string.Format(@" 
                                    INSERT INTO dbo.ChemCloud_CAS(PUB_CID,CAS,CHINESE,Record_Title,CHINESE_ALIAS,Record_Description,Molecular_Formula,Molecular_Weight,
                                    Density,Boiling_Point,Flash_Point,Vapor_Pressure,Exact_Mass,LogP,Physical_Description,Melting_Point,Solubility,Storage_Conditions,Safety_and_Hazards,HS_CODE,[2D_Structure]) 
                                    VALUES('" + newId + "','" + Product.CASNo + "','" + Product.ProductName + "','" + Product.EProductName + "','"
                                + Product.Alias + "','" + Product.Ealias + "','" + Product.MolecularFormula
                                + "','" + Product.MolecularWeight + "','" + Product.Density + "','" + Product.BoilingPoint
                                + "','" + Product.FlashPoint + "','" + Product.VapourPressure + "','" + Product.PASNo + "','"
                                + Product.LogP + "','" + Product.Shape + "','" + Product.FusingPoint + "','"
                                + Product.RefractiveIndex + "','" + Product.StorageConditions + "','" + Product.SafetyInstructions + "','" + Product.HSCODE + "','')");

                                    int result = DbHelperSQLCAS.ExecuteSql(strsql);

                                }
                            }
                            Product.FreightTemplateId = 138;
                            Product.AddedDate = DateTime.Now;//产品日期
                            Product.Plat_Code = DateTime.Now.ToString("yyyyMMddHHmmssffff").ToString();//平台货号
                            ServiceHelper.Create<IProductService>().AddProduct(Product); /*保存产品*/

                            /*查询已经成功添加的产品*/
                            ProductInfo productInfo =
                                ServiceHelper.Create<IProductService>().GetProductByCondition(
                                shopid, dt.Rows[i]["SupplierProductId"].ToString());
                            if (productInfo == null)
                            {
                                NullCount = NullCount + 1;
                            }
                            else
                            {
                                /*产品报价*/
                                try
                                {
                                    /*RMB_CN*/
                                    if (dt.Rows[i]["RMB_CN"] != null)
                                    {
                                        ProductSpec _ProductSpec = new ProductSpec();
                                        _ProductSpec.ProductId = productInfo.Id;
                                        _ProductSpec.Packaging = dt.Rows[i]["PackSize"] == null ? "" : dt.Rows[i]["PackSize"].ToString();
                                        _ProductSpec.Purity = dt.Rows[i]["Purity"] == null ? "" : dt.Rows[i]["Purity"].ToString();
                                        string price = dt.Rows[i]["RMB_CN"] == null ? "" : dt.Rows[i]["RMB_CN"].ToString();
                                        if (string.IsNullOrEmpty(price))
                                        {
                                            _ProductSpec.Price = 0;
                                        }
                                        else
                                        {
                                            _ProductSpec.Price = Convert.ToDecimal(price);
                                        }

                                        _ProductSpec.SpecLevel = dt.Rows[i]["PackingGroup"] == null ? "" : dt.Rows[i]["PackingGroup"].ToString();
                                        _ProductSpec.CoinType = 1;
                                        ServiceHelper.Create<IProductService>().AddProductSpecs(_ProductSpec);
                                    }
                                    /*USD_USPrice*/
                                    if (dt.Rows[i]["USD_USPrice"] != null)
                                    {
                                        ProductSpec _ProductSpec = new ProductSpec();
                                        _ProductSpec.ProductId = productInfo.Id;
                                        _ProductSpec.Packaging = dt.Rows[i]["PackSize"] == null ? "" : dt.Rows[i]["PackSize"].ToString();
                                        _ProductSpec.Purity = dt.Rows[i]["Purity"] == null ? "" : dt.Rows[i]["Purity"].ToString();

                                        string price = dt.Rows[i]["USD_USPrice"] == null ? "" : dt.Rows[i]["USD_USPrice"].ToString();
                                        if (string.IsNullOrEmpty(price))
                                        {
                                            _ProductSpec.Price = 0;
                                        }
                                        else
                                        {
                                            _ProductSpec.Price = Convert.ToDecimal(price);
                                        }

                                        _ProductSpec.SpecLevel = dt.Rows[i]["PackingGroup"] == null ? "" : dt.Rows[i]["PackingGroup"].ToString();
                                        _ProductSpec.CoinType = 2;
                                        ServiceHelper.Create<IProductService>().AddProductSpecs(_ProductSpec);
                                    }
                                }
                                catch
                                {
                                    SubErrorCount = SubErrorCount + 1;
                                }
                            }
                        }
                        catch
                        {
                            ErrorCount = ErrorCount + 1;
                        }

                        if (ErrorCount == 0 && SubErrorCount == 0 && NullCount == 0 && RepeatCount == 0)
                        { SuccessCount = SuccessCount + 1; }
                    }
                }
                if (ErrorCount > 0 && SubErrorCount > 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = 2,
                        ErrorCount = ErrorCount,
                        SubErrorCount = SubErrorCount,
                        SuccessCount = SuccessCount
                    });//5商品已经导入," + "有" + ErrorCount + "条数据有误," + "有" + SubErrorCount + "条等级相关数据有错误。
                }
                else if (ErrorCount > 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = 3,
                        ErrorCount = ErrorCount,
                        SuccessCount = SuccessCount
                    });//"4商品已经导入," + "有" + ErrorCount + "条数据有误。"
                }
                else if (SubErrorCount > 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = 4,
                        SubErrorCount = SubErrorCount,
                        SuccessCount = SuccessCount
                    });//"3商品已经导入," + "有" + SubErrorCount + "条等级相关数据有错误。"
                }
                else if (RepeatCount > 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = 1,
                        RepeatCount = RepeatCount,
                        SuccessCount = SuccessCount
                    });
                }
                else if (NullCount > 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = 7,
                        NullCount = NullCount,
                        SuccessCount = SuccessCount
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = true,
                        message = 5,
                        SuccessCount = SuccessCount
                    });//"1所有数据成功导入商品！"
                }
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = 6
                });//"2没有找到文件！"
            }
        }
    }
}