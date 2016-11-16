using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.SellerAdmin.Models;
using ChemCloud.Web.Areas.Web;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.Web.Models;
using System.Configuration;
using ChemCloud.Model.Common;
using System.Web.Services;
using System.Data;
using System.Text;
using ChemCloud.DBUtility;
using System.Text.RegularExpressions;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class SearchController : BaseWebController
    {
        public SearchController()
        {

        }
        private void InitialCategory(List<CategoryJsonModel> model, long f_cateId, long sub_cateId)
        {
            string name = ServiceHelper.Create<ICategoryService>().GetCategory(f_cateId).Name;
            string str = ServiceHelper.Create<ICategoryService>().GetCategory(sub_cateId).Name;
            if (model.Any((CategoryJsonModel c) => c.Id == f_cateId.ToString()))
            {
                List<SecondLevelCategory> subCategory = model.FirstOrDefault((CategoryJsonModel c) => c.Id == f_cateId.ToString()).SubCategory;
                SecondLevelCategory secondLevelCategory = new SecondLevelCategory()
                {
                    Name = str,
                    Id = sub_cateId.ToString()
                };
                subCategory.Add(secondLevelCategory);
                return;
            }
            CategoryJsonModel categoryJsonModel = new CategoryJsonModel()
            {
                Name = name,
                Id = f_cateId.ToString()
            };
            List<SecondLevelCategory> secondLevelCategories = new List<SecondLevelCategory>();
            SecondLevelCategory secondLevelCategory1 = new SecondLevelCategory()
            {
                Id = sub_cateId.ToString(),
                Name = str
            };
            secondLevelCategories.Add(secondLevelCategory1);
            categoryJsonModel.SubCategory = secondLevelCategories;
            model.Add(categoryJsonModel);
        }

        public JsonResult Chat(int shopId)
        {
            Result result = new Result();
            long userId = 0;
            ManagerInfo manaInfo = ServiceHelper.Create<IManagerService>().GetManagerInfoByShopId(shopId);
            if (manaInfo != null)
            {
                string UserName = manaInfo.UserName;
                UserMemberInfo memberInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(UserName);
                if (memberInfo != null)
                {
                    userId = memberInfo.Id;
                }
                if (memberInfo == null)
                {
                    result.success = false;
                }
                if (base.CurrentUser == null)
                {
                    result.success = false;
                }
                else
                {
                    if (userId == base.CurrentUser.Id)
                    {
                        result.success = false;
                    }
                    else
                    {
                        ServiceHelper.Create<IChatRelationShipService>().UpdateChatRelationShip(userId, base.CurrentUser.Id);
                        result.success = true;
                    }
                }
            }
            else
            {
                result.success = false;
            }
            return Json(result);
        }

        [ActionName("Index")]
        public ActionResult Search(string keywords = "", string exp_keywords = "", long cid = 0L, long b_id = 0L, string a_id = "", int orderKey = 1, int orderType = 1, int pageNo = 1, int pageSize = 60, string islike = "0")
        {

            ViewBag.keywords = keywords;
            ViewBag.orderKey = "1";
            ViewBag.exp_keywords = "1";
            ViewBag.cid = cid;
            ViewBag.b_id = b_id;
            ViewBag.a_id = a_id;
            ViewBag.orderKey = orderKey;
            ViewBag.orderType = orderType;

            ViewBag.islike = islike;//是否模糊查询
            ViewBag.keywords = keywords;//关键字
            ViewBag.exp_keywords = exp_keywords;//关键字

            ((dynamic)base.ViewBag).SEOKeyword = keywords;
            return View();
        }

        [HttpPost]
        public JsonResult SearchResult(string keywords, string islikevalue, int pagesize, int pageno, string exp_keywords = "")
        {
            #region SQL屏蔽'
            if (keywords.Contains("'"))
            {
                keywords = keywords.Replace("'", "").Trim();
            }
            if (exp_keywords.Contains("'"))
            {
                exp_keywords = exp_keywords.Replace("'", "");
            }
            #endregion
            string strwhere = "";

            if (!string.IsNullOrEmpty(exp_keywords))
            {
                strwhere += string.Format(" InChI_Key ='{0}'", exp_keywords);
            }
            else
            {
                if (islikevalue == "0")
                {
                    if (Regex.IsMatch(keywords, @"[0-9?-]") && !Regex.IsMatch(keywords, @"[A-Za-z]+") && !Regex.IsMatch(keywords, @"[\u4E00-\u9FA5]"))
                    {
                        //strwhere += string.Format(" CAS = '{0}' ", keywords);
                        /*cas#查询 */
                        if (keywords.Contains("~"))
                        {
                            string sqlsace = "(";
                            string[] array = keywords.Split('~');
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(array[i]))
                                {
                                    sqlsace = sqlsace + "'" + array[i] + "',";
                                }
                            }
                            sqlsace = sqlsace.Substring(0, sqlsace.Length - 1);
                            sqlsace = sqlsace + ")";
                            strwhere += string.Format(" CAS in {0} ", sqlsace);
                        }
                        else
                        {
                            strwhere += string.Format(" CAS = '{0}' ", keywords);
                        }
                    }
                    else if (Regex.IsMatch(keywords, @"[\u4E00-\u9FA5]"))
                    {
                        strwhere += string.Format(" (CHINESE = '{0}' OR CHINESE_ALIAS = '{0}') ", keywords);
                    }
                    else
                    {
                        strwhere += string.Format("  (Record_Title = '{0}'  OR Molecular_Formula ='{0}' OR INCHI_KEY ='{0}' ) ", keywords);
                    }
                }
                else
                {
                    if (Regex.IsMatch(keywords, @"[0-9?-]") && !Regex.IsMatch(keywords, @"[A-Za-z]+") && !Regex.IsMatch(keywords, @"[\u4E00-\u9FA5]"))
                    {
                        strwhere += string.Format(" CAS like '{0}%' ", keywords);
                    }
                    else if (Regex.IsMatch(keywords, @"[\u4E00-\u9FA5]"))
                    {
                        strwhere += string.Format(" (CHINESE like '{0}%' OR CHINESE_ALIAS like '{0}%') ", keywords);
                    }
                    else
                    {
                        strwhere += string.Format("  (Record_Title like '{0}%'  OR Molecular_Formula like '{0}%'  OR INCHI_KEY like '{0}%') ", keywords);
                    }
                }
            }


            DataSet _ds = GetListByPage(strwhere, "PUB_CID", (pageno - 1) * pagesize + 1, pageno * pagesize);
            if (_ds == null || _ds.Tables[0].Rows.Count < 1)
            {
                return Json("");
            }
            else
            {
                string reslut = ChemCloud.Core.JsonHelper.GetJsonByDataset(_ds);


                return Json(reslut);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.Pub_CID");
            }
            strSql.Append(")AS Row,T.PUB_CID,T.Record_Title,T.CAS,T.Molecular_Formula,T.Molecular_Weight,T.[2D_Structure] as Dataurl,T.CHINESE  from ChemCloud_CAS T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQLCAS.Query(strSql.ToString());

        }
        [HttpPost]
        public JsonResult SearchResultCount(string keywords, string islikevalue, string exp_keywords = "")
        {
            #region SQL屏蔽'
            if (keywords.Contains("'"))
            {
                keywords = keywords.Replace("'", "");
            }
            if (exp_keywords.Contains("'"))
            {
                exp_keywords = exp_keywords.Replace("'", "");
            }
            #endregion
            string strsql = string.Format("SELECT COUNT(0) AS COUNT FROM ChemCloud_CAS where ");
            if (!string.IsNullOrEmpty(exp_keywords))
            {
                strsql += string.Format(" InChI_Key ='{0}'", exp_keywords);
            }
            else
            {
                if (islikevalue == "0")
                {
                    if (Regex.IsMatch(keywords, @"[0-9?-]") && !Regex.IsMatch(keywords, @"[A-Za-z]+") && !Regex.IsMatch(keywords, @"[\u4E00-\u9FA5]"))
                    {
                        /*cas#查询 */
                        if (keywords.Contains("~"))
                        {
                            string sqlsace = "(";
                            string[] array = keywords.Split('~');
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(array[i]))
                                {
                                    sqlsace = sqlsace + "'" + array[i] + "',";
                                }
                            }
                            sqlsace = sqlsace.Substring(0, sqlsace.Length - 1);
                            sqlsace = sqlsace + ")";
                            strsql += string.Format(" CAS in {0} ", sqlsace);
                        }
                        else
                        {
                            strsql += string.Format(" CAS = '{0}' ", keywords);
                        }
                    }
                    else if (Regex.IsMatch(keywords, @"[\u4E00-\u9FA5]"))
                    {
                        strsql += string.Format(" (CHINESE = '{0}' OR CHINESE_ALIAS = '{0}') ", keywords);
                    }
                    else
                    {
                        strsql += string.Format("  (Record_Title = '{0}'  OR Molecular_Formula ='{0}' OR INCHI_KEY ='{0}') ", keywords);
                    }
                }
                else
                {
                    if (Regex.IsMatch(keywords, @"[0-9?-]") && !Regex.IsMatch(keywords, @"[A-Za-z]+") && !Regex.IsMatch(keywords, @"[\u4E00-\u9FA5]"))
                    {
                        strsql += string.Format(" CAS like '{0}%' ", keywords);
                    }
                    else if (Regex.IsMatch(keywords, @"[\u4E00-\u9FA5]"))
                    {
                        strsql += string.Format(" (CHINESE like '{0}%' OR CHINESE_ALIAS like '{0}%') ", keywords);
                    }
                    else
                    {
                        strsql += string.Format("  (Record_Title like '{0}%'  OR Molecular_Formula like '{0}%' OR INCHI_KEY like '{0}%') ", keywords);
                    }
                }
            }

            DataSet ds = DbHelperSQLCAS.Query(strsql);
            if (ds == null)
            {
                return Json("0");
            }
            else
            {
                return Json(ds.Tables[0].Rows[0]["COUNT"].ToString());
            }
        }
        /// <summary>
        /// 产品-供应商列表页 视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Search_Product_Shops(string id)
        {

            int i;
            int k = 0;
            if (int.TryParse(id, out i))
            {
                k = int.Parse(id);
            }

            CASInfo _CASInfo = ServiceHelper.Create<ICASInfoService>().GetCASByPUB_CID(k);
            string s = _CASInfo.C2D_Structure;
            ViewBag.CASNo = id;
            ViewBag.CurrentUserType = 3;
            if (this.CurrentUser != null)
            {
                if (this.CurrentUser.UserType == 2)
                {
                    ViewBag.CurrentUserType = 2;
                }
            }
            string seostrsql = string.Format(@"SELECT Meta_Title,Meta_Description,Meta_Keywords FROM dbo.ChemCloud_CasSeo where Pub_CID='{0}'", id);
            List<string> listKeywords = new List<string>();
            List<string> listDescription = new List<string>();
            List<string> listTitle = new List<string>();
            DataSet ds = DbHelperSQL.Query(seostrsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(item["Meta_Keywords"])))
                    {
                        listKeywords.Add(Convert.ToString(item["Meta_Keywords"]));

                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(item["Meta_Description"])))
                    {
                        listDescription.Add(Convert.ToString(item["Meta_Description"]));

                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(item["Meta_Title"])))
                    {
                        listTitle.Add(Convert.ToString(item["Meta_Title"]));

                    }

                }
                if (listKeywords.Count > 0)
                {

                    ((dynamic)base.ViewBag).SEOKeyword = string.Join(",", listKeywords).Length > 72 ? string.Join(",", listKeywords).Substring(0, 72) : string.Join(",", listKeywords);
                }
                if (listDescription.Count > 0)
                {
                    ((dynamic)base.ViewBag).SEODescription = string.Join(",", listDescription).Length > 220 ? string.Join(",", listDescription).Substring(0, 220) : string.Join(",", listDescription); ;
                }
                if (listTitle.Count > 0)
                {
                    ((dynamic)base.ViewBag).GetTitle = string.Join(",", listTitle).Length > 60 ? string.Join(",", listTitle).Substring(0, 60) : string.Join(",", listTitle); ;
                }
            }
            return View(_CASInfo);
        }

        /// <summary>
        /// 产品详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SearchProductInfo(string id)
        {
            int i;
            int k = 0;
            if (int.TryParse(id, out i))
            {
                k = int.Parse(id);
            }
            CASInfo _CASInfo = ServiceHelper.Create<ICASInfoService>().GetCASByPUB_CID(k);

            //if (RegExHtml(_CASInfo.Record_Description))
            //{
            //    _CASInfo.Record_Description = string.Empty;
            //}
            ViewBag.Title = "ChemCloud产品详细";
            ViewBag.CASNo = id;
            string seostrsql = string.Format(@"SELECT Meta_Title,Meta_Description,Meta_Keywords FROM dbo.ChemCloud_CasSeo where 
            Pub_CID={0}", id);
            List<string> listKeywords = new List<string>();
            List<string> listDescription = new List<string>();
            List<string> listTitle = new List<string>();
            DataSet ds = DbHelperSQL.Query(seostrsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(item["Meta_Keywords"])))
                    {
                        listKeywords.Add(Convert.ToString(item["Meta_Keywords"]));

                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(item["Meta_Description"])))
                    {
                        listDescription.Add(Convert.ToString(item["Meta_Description"]));

                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(item["Meta_Title"])))
                    {
                        listTitle.Add(Convert.ToString(item["Meta_Title"]));

                    }

                }
                if (listKeywords.Count > 0)
                {

                    ((dynamic)base.ViewBag).SEOKeyword = string.Join(",", listKeywords).Length > 72 ? string.Join(",", listKeywords).Substring(0, 72) : string.Join(",", listKeywords);
                }
                if (listDescription.Count > 0)
                {
                    ((dynamic)base.ViewBag).SEODescription = string.Join(",", listDescription).Length > 220 ? string.Join(",", listDescription).Substring(0, 220) : string.Join(",", listDescription); ;
                }
                if (listTitle.Count > 0)
                {
                    ((dynamic)base.ViewBag).GetTitle = string.Join(",", listTitle).Length > 60 ? string.Join(",", listTitle).Substring(0, 60) : string.Join(",", listTitle); ;
                }
            }
            return View(_CASInfo);
        }

        private bool RegExHtml(string param)
        {
            bool result = false;
            string pattern = @"<[^>]+>";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = reg.Matches(param);

            if (matches.Count > 0)
            {
                result = true;
            }
            return result;

        }



        /// <summary>
        /// 产品-供应商查询 方法
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="productname"></param>
        /// <returns></returns>
        public JsonResult ProductWithShops(int page, int rows, string CASNo, string ShopType)
        {
            ProductQuery productquery = new ProductQuery()
            {
                CASNo = Convert.ToInt32(CASNo),
                PageSize = rows,
                PageNo = page,
                ShopType = ShopType
            };
            PageModel<ProductInfo> products = ServiceHelper.Create<IProductService>().GetProductsToSearch(productquery);
            var array = from item in products.Models.ToArray()
                        select new
                        {
                            Id = item.Id,
                            ProductCode = item.ProductCode,
                            ProductName = item.ProductName,
                            EProductName = item.EProductName,
                            ShopId = item.ShopId,
                            ShopName = (ServiceHelper.Create<IShopService>().GetShopName(item.ShopId) == null ? "" : ServiceHelper.Create<IShopService>().GetShopName(item.ShopId)),
                            GMPPhoto = (ServiceHelper.Create<IShopService>().GetShopBasicInfo(item.ShopId)) == null ? "" : ServiceHelper.Create<IShopService>().GetShopBasicInfo(item.ShopId).GMPPhoto,
                            FDAPhoto = (ServiceHelper.Create<IShopService>().GetShopBasicInfo(item.ShopId)) == null ? "" : ServiceHelper.Create<IShopService>().GetShopBasicInfo(item.ShopId).FDAPhoto,
                            ISOPhoto = (ServiceHelper.Create<IShopService>().GetShopBasicInfo(item.ShopId)) == null ? "" : ServiceHelper.Create<IShopService>().GetShopBasicInfo(item.ShopId).ISOPhoto,
                            ShortDescription = item.ShortDescription,
                            CASNo = item.CASNo,
                            HSCODE = item.HSCODE,
                            DangerLevel = item.DangerLevel,
                            MolecularFormula = item.MolecularFormula,
                            ISCASNo = item.ISCASNo,
                            EditStatus = item.EditStatus,
                            MeasureUnit = item.MeasureUnit,
                            Quantity = item.Quantity,
                            Volume = item.Volume,
                            Weight = item.Weight,
                            Packaging = item.Packaging,
                            Purity = item.Purity,
                            SpecLevel = item.SpecLevel,
                            Price = item.Price,
                            showshop = ServiceHelper.Create<IProductsDSService>().GetProductsDS(item.ShopId, item.Id) == null ? "" : ServiceHelper.Create<IProductsDSService>().GetProductsDS(item.ShopId, item.Id).DSStatus.ToString()
                        };

            array = from p in array where p.Packaging != "" select p;
            return Json(new
            {
                rows = array,
                total = products.Total
            });
        }


        /// <summary>
        /// 高级搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult SuperSearch()
        {

            return View();
        }

        public JsonResult Test()
        {

            StringBuilder input = new StringBuilder();
            input.AppendLine("");
            input.AppendLine("  -OEChem-04191619132D");
            input.AppendLine("");
            input.AppendLine("  9  8  0     0  0  0  0  0  0999 V2000");
            input.AppendLine("    2.5369   -0.2500    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.4030    0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.2690   -0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.8015    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.0044    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.9590   -0.7869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.8059   -0.5600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.5790    0.2869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    2.0000    0.0600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("  1  2  1  0  0  0  0");
            input.AppendLine("  1  9  1  0  0  0  0");
            input.AppendLine("  2  3  1  0  0  0  0");
            input.AppendLine("  2  4  1  0  0  0  0");
            input.AppendLine("  2  5  1  0  0  0  0");
            input.AppendLine("  3  6  1  0  0  0  0");
            input.AppendLine("  3  7  1  0  0  0  0");
            input.AppendLine("  3  8  1  0  0  0  0");
            input.AppendLine("M  END");
            Result res = new Result();
            res.msg = input.ToString();
            return Json(res);
        }

        public JsonResult Test2(string src)
        {

            StringBuilder input = new StringBuilder();
            input.AppendLine("");
            input.AppendLine("  -OEChem-04191619132D");
            input.AppendLine("");
            input.AppendLine("  9  8  0     0  0  0  0  0  0999 V2000");
            input.AppendLine("    2.5369   -0.2500    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.4030    0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.2690   -0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.8015    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.0044    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    3.9590   -0.7869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.8059   -0.5600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    4.5790    0.2869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("    2.0000    0.0600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0");
            input.AppendLine("  1  2  1  0  0  0  0");
            input.AppendLine("  1  9  1  0  0  0  0");
            input.AppendLine("  2  3  1  0  0  0  0");
            input.AppendLine("  2  4  1  0  0  0  0");
            input.AppendLine("  2  5  1  0  0  0  0");
            input.AppendLine("  3  6  1  0  0  0  0");
            input.AppendLine("  3  7  1  0  0  0  0");
            input.AppendLine("  3  8  1  0  0  0  0");
            input.AppendLine("M  END");
            Result res = new Result();
            res.msg = input.ToString();
            return Json(res);
        }

        /// <summary>
        /// COA搜索页面
        /// </summary>
        /// <returns></returns>
        public ActionResult COASearch(string keywords = "", int pageNo = 1, int pageSize = 60)
        {
            ViewBag.keywords = keywords;
            return View();
        }

        [HttpPost]
        public int SearchCOACount(string keywords = "", int pageNo = 1, int pageSize = 60)
        {
            return ServiceHelper.Create<ICOAService>().SearchCOAListCount(keywords, pageNo, pageSize);
        }

        [HttpPost]
        public JsonResult SearchCOA(string keywords = "", int pageNo = 1, int pageSize = 60)
        {
            PageModel<ChemCloud_COA> pageModel = ServiceHelper.Create<ICOAService>().SearchCOAList(keywords, pageNo, pageSize);
            IEnumerable<ChemCloud_COA> models =
                from item in pageModel.Models.ToArray()
                select new ChemCloud_COA()
                {
                    Id = item.Id,
                    CertificateNumber = item.CertificateNumber,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    strDateofManufacture = item.DateofManufacture == null ? DateTime.Now.ToString("yyyy-MM-dd") : item.DateofManufacture.ToString("yyyy-MM-dd"),
                    Supplier = item.Supplier,
                    ManufacturersBatchNo = item.ManufacturersBatchNo,
                    SydcosLabBatchNo = item.SydcosLabBatchNo,
                    strExpiryDate = item.ExpiryDate == null ? DateTime.Now.ToString("yyyy-MM-dd") : item.ExpiryDate.ToString("yyyy-MM-dd"),
                    strDATEOFRELEASE = item.DATEOFRELEASE == null ? DateTime.Now.ToString("yyyy-MM-dd") : item.DATEOFRELEASE.ToString("yyyy-MM-dd"),
                    LABORATORYMANAGER = item.LABORATORYMANAGER
                };
            DataGridModel<ChemCloud_COA> dataGridModel = new DataGridModel<ChemCloud_COA>()
            {
                rows = models,
                total = pageModel.Total
            };
            return Json(dataGridModel);
        }


        public JsonResult ShowDetial(long productid)
        {
            string language = ConfigurationManager.AppSettings["Language"].ToString();
            List<ProductSpec> ps = ServiceHelper.Create<IProductService>().GetDetial(Convert.ToInt32(language), productid);
            return Json(ps);
        }

        public ActionResult Search_COA(string key)
        {
            COAList Coa = ServiceHelper.Create<ICOAListService>().GetCoAReportInfo(key);
            if (Coa != null)
            {
                ViewBag.Url = Coa.URL;
            }
            return View();
        }

        public JsonResult SearchCoaR(string key)
        {
            Result res = new Result();
            res.success = ServiceHelper.Create<ICOAListService>().IsExits(key);
            return Json(res);
        }
        public ActionResult SearchLaw(string key)
        {
            ViewBag.key = key;
            if (base.CurrentUser != null)
                ViewBag.userId = base.CurrentUser.Id;
            else
                ViewBag.userId = null;
            return View();
        }
        public ActionResult ManagementWeb()
        {
            return View();
        }

        public ActionResult ManagementWeb_1()
        {
            return View();
        }

        public ActionResult ManagementWebDes()
        {
            return View();
        }
        //[HttpPost]
        //public JsonResult ListforIndex(string title, int page, int rows)
        //{
        //    LawInfoQuery models = new LawInfoQuery();
        //    models = new LawInfoQuery()
        //    {
        //        Title = title,
        //        PageNo = page,
        //        PageSize = rows
        //    };
        //    PageModel<LawInfo> opModel = ServiceHelper.Create<ILawInfoService>().GetLawInfosforIndex(models);
        //    var array =
        //        from item in opModel.Models.ToArray()
        //        select new { Id = item.Id, Title = item.Title, CreateTime = item.CreateTime };
        //    return Json(new { rows = array, total = opModel.Total });
        //}
        public ActionResult Details(long Id)
        {
            LawInfo model = ServiceHelper.Create<ILawInfoService>().GetLawInfo(Id);
            AttachmentInfo models = ServiceHelper.Create<ILawInfoService>().GetAttachmentInfo(Id);
            ViewBag.parentId = (models == null ? 0 : models.ParentId);
            ViewBag.attachment = (models == null ? "0" : models.AttachmentName);
            ViewBag.attachmentId = (models == null ? 0 : models.Id);
            ViewBag.userId = (models == null ? 0 : models.UserId);
            return View(model);
        }
        #region 前台法律法规

        #region 根据ID获取指定对象
        /// <summary>
        /// 根据ID获取指定对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetObjectById_Web(int Id)
        {

            Result_Model_LawAttachmentList_PreNextRow resAll = new Result_Model_LawAttachmentList_PreNextRow()
            {
                Model = new Result_LawInfo(),
                AttachmentList = new Result_List<Result_AttachmentInfo>(),
                Msg = new Result_Msg() { IsSuccess = true, Message = string.Empty },
                PreNextRow = new Result_List<Result_Model<LawInfo>>()
            };
            try
            {
                ILawInfoService jobsService = ServiceHelper.Create<ILawInfoService>();
                resAll.Model = jobsService.GetObjectById_Web(Id);
                if (resAll.Model != null && resAll.Msg.IsSuccess)
                {
                    resAll.AttachmentList = GetObjectList_ById_Web(Id);
                    if (!resAll.AttachmentList.Msg.IsSuccess)
                    {
                        resAll.Msg.IsSuccess = false;
                        resAll.Msg.Message += "获取附件列表失败\n\r";
                    }
                    else
                    {
                        foreach (var item in resAll.AttachmentList.List)
                        {
                            try
                            {
                                item.FileName = item.AttachmentName.Substring(item.AttachmentName.LastIndexOf('/') + 1, item.AttachmentName.Length - item.AttachmentName.LastIndexOf('/') - 1);
                            }
                            catch (Exception)
                            {
                                item.FileName = "附件信息获取失败";
                            }
                        }
                    }
                    resAll.PreNextRow = Get_PreNext_ById_Web(Id);
                    if (!resAll.PreNextRow.Msg.IsSuccess)
                    {
                        resAll.Msg.IsSuccess = false;
                        resAll.Msg.Message += "获取上一页、下一页\n\r";
                    }
                }
                else
                {
                    resAll.Msg.IsSuccess = false;
                    resAll.Msg.Message += "获取会议数据失败\n\r";
                }
            }
            catch (Exception ex)
            {
                resAll.Msg = new Result_Msg() { IsSuccess = false, Message = "读取失败，失败原因：" + ex.Message };
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(resAll);
        }
        #endregion

        #region 会议中心  列表
        /// <summary>
        /// 会议中心  列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetLawInfoList_Web(QueryCommon<LawInfoQuery_Web> query)
        {
            ILawInfoService jobsService = ServiceHelper.Create<ILawInfoService>();
            Result_List_Pager<Result_LawInfo> res = jobsService.GetLawInfoList_Web(query);
            List<Result_AttachmentInfo> resItem = new List<Result_AttachmentInfo>();
            foreach (var item in res.List)
            {
                resItem = GetObjectList_ById_Web(item.Id).List.Where(x => x.AttachmentName != null && x.AttachmentName != "" && x.AttachmentName.LastIndexOf('.') > 0).ToList();
                if (resItem != null && resItem.Count > 0)
                {
                    Result_AttachmentInfo imgItem = resItem.Where(x => HashSet_Common.ImageTypeArr.Contains(x.AttachmentName.Substring(x.AttachmentName.LastIndexOf('.'), x.AttachmentName.Length - x.AttachmentName.LastIndexOf('.')))).FirstOrDefault();
                    if (imgItem != null)
                    {
                        item.AttachmentName = imgItem.AttachmentName;

                    }
                    else
                    {
                        item.AttachmentName = "";
                    }
                }
                else
                {
                    item.AttachmentName = "";
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 根据ID获取附件列表
        /// <summary>
        /// 根据ID获取附件列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result_List<Result_AttachmentInfo> GetObjectList_ById_Web(long Id)
        {
            Result_List<Result_AttachmentInfo> res = new Result_List<Result_AttachmentInfo>();

            try
            {
                ILawInfoService jobsService = ServiceHelper.Create<ILawInfoService>();
                res = jobsService.GetObjectList_ById_Web(Id);
                //res.List.Select(x => x.FileName == System.IO.Path.GetFileName(x.AttachmentName));
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "读取失败，失败原因：" + ex.Message };
            }

            return res;
        }
        #endregion

        #region 根据ID 前后两条信息
        /// <summary>
        /// 根据ID 前后两条信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result_List<Result_Model<LawInfo>> Get_PreNext_ById_Web(long Id)
        {
            Result_List<Result_Model<LawInfo>> res = new Result_List<Result_Model<LawInfo>>() { Msg = new Result_Msg() { IsSuccess = true } };

            try
            {
                ILawInfoService jobsService = ServiceHelper.Create<ILawInfoService>();
                res = jobsService.Get_PreNext_ById_Web(Id);
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "读取失败，失败原因：" + ex.Message };
            }
            return res;
        }
        #endregion

        #region 获取分页信息
        [WebMethod]

        public string Get_PageInfo(QueryCommon<LawInfoQuery_Web> query)
        {
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            ILawInfoService jobsService = ServiceHelper.Create<ILawInfoService>();
            resModel = jobsService.Get_PageInfo_Web(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion

        #endregion

        /// <summary>
        /// 结构式搜索
        /// </summary>
        /// <param name="strmol"></param>
        /// <returns></returns>
        public JsonResult Getinchkey(string strmol)
        {
            string inchkey = ChemCloud.Web.InchikeyHelper.GetInchikey(strmol);

            bool result = false;
            if (!string.IsNullOrEmpty(inchkey))
            {
                result = true;
            }

            return Json(new { success = result, msg = inchkey });
        }

    }
}