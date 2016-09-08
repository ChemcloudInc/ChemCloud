using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using O2S.Components.PDFRender4NET;
using iTextSharp.text;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Configuration;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class COAListController : BaseSellerController
    {
        //
        // GET: /SellerAdmin/COAList/
        public ActionResult Management()
        {
            return View();
        }
        [HttpPost]
        public JsonResult List(int page, int rows, string orderNum, string startTime, string endTime, string CASNo)
        {
            COAListQuery opQuery = new COAListQuery();
            DateTime dt;
            if (DateTime.TryParse(startTime, out dt) && DateTime.TryParse(endTime, out dt))
            {
                opQuery = new COAListQuery()
                {
                    PageSize = rows,
                    PageNo = page,
                    BeginTime = DateTime.Parse(startTime),
                    EndTime = DateTime.Parse(endTime),
                    CoANo = orderNum,
                    UserId = base.CurrentUser.Id,
                    CASNo = CASNo

                };
            }
            else
            {
                opQuery = new COAListQuery()
                {
                    PageSize = rows,
                    PageNo = page,
                    CoANo = orderNum,
                    CASNo = CASNo,
                    UserId = base.CurrentUser.Id
                };
            }
            PageModel<COAList> opModel = ServiceHelper.Create<ICOAListService>().GetCoAReportInfo(opQuery, base.CurrentUser.Id);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, AddTime = item.Addtime, CoANo = item.CoANo, URL = item.URL, CASNo = item.CASNo };
            return Json(new { rows = array, total = opModel.Total });
        }

        public ActionResult AddStepOne()
        {
            return View();
        }

        public ActionResult AddStepTwo()
        {
            int count = ServiceHelper.Create<ICOAListService>().GetUserCount(base.CurrentUser.Id);
            ViewBag.CoANo = getCoANo(base.CurrentUser.Id, count);
            return View();
        }
        public ActionResult AddStepFive()
        {
            int count = ServiceHelper.Create<ICOAListService>().GetUserCount(base.CurrentUser.Id);
            ViewBag.CoANo = getCoANo(base.CurrentUser.Id, count);
            return View();
        }
        public JsonResult GetUserCount()
        {
            int count = ServiceHelper.Create<ICOAListService>().GetUserCount(base.CurrentUser.Id);
            string CoaNo = getCoANo(base.CurrentUser.Id, count);
            return Json(new { CoaNo = CoaNo });
        }
        public JsonResult GetCoaReport(string Casno)
        {
            List<COAList> CoaInfos = ServiceHelper.Create<ICOAListService>().GetCoaCountByCASNo(base.CurrentUser.Id, Casno);
            return Json(new { CoaInfo = CoaInfos });
        }
        public JsonResult AddPdf(string CoANo, string filename, string CASNo)
        {
            
            COAList cr = new COAList()
            {
                Addtime = DateTime.Now,
                CoANo = CoANo,
                URL =  filename,
                UserId = base.CurrentUser.Id,
                AddUserType = 2,
                CASNo = CASNo
            };
            ServiceHelper.Create<ICOAListService>().AddCoAReportInfo(cr);
            Result res = new Result();
            res.success = true;
            return Json(res);
        }
        public ActionResult AddStepThree()
        {
            int count = ServiceHelper.Create<ICOAListService>().GetUserCount(base.CurrentUser.Id);
            ViewBag.CoANo = getCoANo(base.CurrentUser.Id, count);
            return View();
        }

        public JsonResult AddPdfBypic(string CoANo, string path, string CASNo)
        {
            string serverFilepath = Server.MapPath(string.Format("/Storage/PDF/{0}/ ", base.CurrentUser.Id));
            if (!System.IO.Directory.Exists(serverFilepath))
            {
                System.IO.Directory.CreateDirectory(serverFilepath);
            }
            string serverFileName = "CoAReports" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf";
            string pdfpath = serverFilepath + serverFileName;
            string[] list = path.Split(';');
            Result res = new Result();
            res.success = ExportDataIntoPDF(pdfpath, list.ToList<string>());
            if (res.success)
            {
                string str = ImgHelper.PostImg(pdfpath, "coapic");
                COAList cr = new COAList()
                {
                    Addtime = DateTime.Now,
                    CoANo = CoANo,
                    URL = str,
                    UserId = base.CurrentUser.Id,
                    AddUserType = 2,
                    CASNo = CASNo
                };
                ServiceHelper.Create<ICOAListService>().AddCoAReportInfo(cr);
            }
            
            return Json(res);
        }
        public ActionResult AddStepFour()
        {
            int count = ServiceHelper.Create<ICOAListService>().GetUserCount(base.CurrentUser.Id);
            ViewBag.CoANo = getCoANo(base.CurrentUser.Id, count);
            return View();
        }
        [HttpPost]
        public JsonResult AddPdfByHtml(string CoANo, string Html, string titles, string CASNo, string Specification)
        {
            titles = titles.Substring(0, titles.Length - 1);
            Specification = Specification.Substring(0, Specification.Length - 1);
            ChemCloud_COA model = Newtonsoft.Json.JsonConvert.DeserializeObject<ChemCloud_COA>(Html);
            List<string> titlelist = titles.Split(';').ToList<string>();
            List<string> SpecificationList = Specification.Split(';').ToList<string>();
            string serverFilepath = Server.MapPath(string.Format("/Storage/PDF/{0}/ ", base.CurrentUser.Id));
            if (!System.IO.Directory.Exists(serverFilepath))
            {
                System.IO.Directory.CreateDirectory(serverFilepath);
            }
            string serverFileName = "CoAReports" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf";
            Result res = new Result();
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4);
            try
            {
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(serverFilepath + serverFileName, FileMode.Create));
                document.Open();
                iTextSharp.text.pdf.BaseFont bfChinese = iTextSharp.text.pdf.BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font Titlefont = new iTextSharp.text.Font(bfChinese, 16, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font titleDetial = new iTextSharp.text.Font(bfChinese, 11, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font normall = new iTextSharp.text.Font(bfChinese, 11, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font newtable = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font newtableInner = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL);
                Paragraph p = new Paragraph("CERTIFICATE OF ANALYSIS" + "\n\n", Titlefont);
                p.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                document.Add(p);
                PdfPTable table = new PdfPTable(4);
                table.DefaultCell.MinimumHeight = 30;
                table.SetWidths(new int[] { 35, 15, 35, 15 });
                PdfPCell cell;
                cell = new PdfPCell();
                Paragraph pCell;
                pCell = new Paragraph("CertificateNumber", titleDetial);
                table.AddCell(pCell);
                pCell = new Paragraph(model.CertificateNumber, normall);
                table.AddCell(pCell);
                pCell = new Paragraph("Suppiler", titleDetial);
                table.AddCell(pCell);
                pCell = new Paragraph(ServiceHelper.Create<IShopService>().GetShopName(base.CurrentSellerManager.ShopId), normall);
                table.AddCell(pCell);
                cell = new PdfPCell();
                pCell = new Paragraph("Product Code", titleDetial);
                table.AddCell(pCell);
                pCell = new Paragraph(model.ProductCode, normall);
                table.AddCell(pCell);
                pCell = new Paragraph("Manufacturer's Batch #", titleDetial);
                table.AddCell(pCell);
                pCell = new Paragraph(model.ManufacturersBatchNo, normall);
                table.AddCell(pCell);
                cell = new PdfPCell();
                pCell = new Paragraph("Product", titleDetial);
                table.AddCell(pCell);
                pCell = new Paragraph(model.ProductName, normall);
                table.AddCell(pCell);
                pCell = new Paragraph("Sydco's Lab.Batch #", titleDetial);
                table.AddCell(pCell);
                pCell = new Paragraph(model.SydcosLabBatchNo, normall);
                table.AddCell(pCell);
                cell = new PdfPCell();
                pCell = new Paragraph("Date of Manufacture", titleDetial);
                table.AddCell(pCell);
                pCell = new Paragraph(model.DateofManufacture.ToShortDateString(), normall);
                table.AddCell(pCell);
                pCell = new Paragraph("ExpiryDate", titleDetial);
                table.AddCell(pCell);
                pCell = new Paragraph(model.ExpiryDate.ToShortDateString(), normall);
                table.AddCell(pCell);
                document.Add(table);
                Paragraph pfoot = new Paragraph("\n           The undersigned hereby certifies the following data to be true specification" + "\n\n\n\n\n", titleDetial);
                p.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                document.Add(pfoot);
                foreach (ChemCloud_COADetails item in model.ChemCloud_COADetails)
                {
                    PdfPTable table1 = new PdfPTable(3);
                    table1.DefaultCell.MinimumHeight = 30;
                    table1.SetWidths(new int[] { 40, 20, 35 });
                    pCell = new Paragraph("Tests", newtable);

                    table1.AddCell(pCell);
                    pCell = new Paragraph("Results", newtable);
                    table1.AddCell(pCell);
                    pCell = new Paragraph("Specifications(BP)", newtable);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[0], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Appearance, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[0], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[1], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.IIdentity, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[1], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[2], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Carbonates, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[2], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[3], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Solubility, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[3], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[4], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Ammonium, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[4], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[5], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Calcium, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[5], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[6], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Arsenic, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[6], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[7], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.HeavyMetals, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[7], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[8], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Iron, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[8], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[9], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Sulphates, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[9], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[10], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Chlorides, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[10], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[11], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.Appearanceofsolution, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[11], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(titlelist[12], newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(item.MicroStatus, newtableInner);
                    table1.AddCell(pCell);
                    pCell = new Paragraph(SpecificationList[12], newtableInner);
                    table1.AddCell(pCell);
                    document.Add(table1);
                    Paragraph ppp = new Paragraph("\n\n\n\n\n\n", titleDetial);
                    ppp.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    document.Add(ppp);
                }
            }

            catch (Exception de)
            {
                res.success = false;
                return Json(res);
            }
            document.Close();
            res.success=true;
            //string pdfpath1 = string.Format("/Storage/PDF/{0}/", base.CurrentUser.Id);
            string str = ImgHelper.PostImg(serverFilepath + serverFileName, "coapic");
            COAList cr = new COAList()
            {
                Addtime = DateTime.Now,
                CoANo = CoANo,
                URL = str,
                UserId = base.CurrentUser.Id,
                AddUserType = 2,
                CASNo = CASNo
            };
            ServiceHelper.Create<ICOAListService>().AddCoAReportInfo(cr);
            return Json(res);
        }  

        public JsonResult Delete(int id)
        {
            ServiceHelper.Create<ICOAListService>().DeleteCoAReportInfo(id);
            Result res = new Result()
            {
                msg = "删除成功!",
                success = true
            };
            return Json(res);   
        }

        public string getCoANo(long id, int count)
        {
            string CoANo = "";
            if (id > 100000)
            {
                CoANo = id.ToString();
            }
            else
            {
                CoANo = id.ToString().PadLeft(6, '0');
            }
            CoANo += DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            count++;
            if (count > 100)
            {
                CoANo += count.ToString();
            }
            else
            {
                CoANo += count.ToString().PadLeft(3, '0');
            }
            return CoANo;
        }


        public JsonResult GetImportOpCount()
        {
            long num = 0;
            //object obj = Cache.Get("Cache-UserUploadPDF");
            //if (obj != null)
            //{
            //    num = (string.IsNullOrEmpty(obj.ToString()) ? 0 : long.Parse(obj.ToString()));
            //}
            return Json(new { Count = num }, JsonRequestBehavior.AllowGet);
        }
        private bool IsAllowExt(HttpPostedFileBase item)
        {
            var notAllowExt = "," + ConfigurationManager.AppSettings["NotAllowExt"] + ",";
            var ext = "," + Path.GetExtension(item.FileName).ToLower() + ",";
            if (notAllowExt.Contains(ext))
                return false;
            return true;
        }
        public ActionResult UpLoadPDF()
        {
            string str = "NoPDF";
            HttpPostedFileBase item = null;
            if (base.Request.Files.Count > 0)
            {
                if (base.Request.Files.Count == 0)
                {
                    return base.Content("NoFile", "text/html");
                }
                item = base.Request.Files[0];
                if (item == null || item.ContentLength <= 0 || !IsAllowExt(item))
                {
                    return base.Content("格式不正确！", "text/html");
                }
                if (item != null)
                {
                    string serverFilepath = Server.MapPath(string.Format("/Storage/PDF/{0}/ ", base.CurrentUser.Id));
                    string serverFileName = "CoAReports" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf";
                    string PDF = serverFilepath + '\\' + serverFileName;
                    if (!System.IO.Directory.Exists(serverFilepath))
                    {
                        System.IO.Directory.CreateDirectory(serverFilepath);
                    }
                    try
                    {
                        object obj = Cache.Get("Cache-UserUploadPDF");
                        if (obj != null)
                        {
                            Cache.Insert("Cache-UserUploadPDF", int.Parse(obj.ToString()) + 1);
                        }
                        else
                        {
                            Cache.Insert("Cache-UserUploadPDF", 1);
                        }
                        item.SaveAs(PDF);
                        str = ImgHelper.PostImg(PDF, "coapic");//文件路径
                        
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        object obj1 = Cache.Get("Cache-UserUploadPDF");
                        if (obj1 != null)
                        {
                            Cache.Insert("Cache-UserUploadPDF", int.Parse(obj1.ToString()) - 1);
                        }
                        Log.Error(string.Concat("产品导入上传文件异常：", exception.Message));
                        str = "Error";
                    }
                }
                else
                {
                    str = "文件长度为0,格式异常。";
                }
            }
            return base.Content(str, "text/html", System.Text.Encoding.UTF8);//将文件名回传给页面的innerHTML
        }

        public bool ExportDataIntoPDF(string pathName, List<string> path)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            try
            {
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(pathName, FileMode.CreateNew));
                document.Open();
                foreach (string item in path)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }
                    string picpath = item;
                    iTextSharp.text.Image je = iTextSharp.text.Image.GetInstance(picpath);
                    document.Add(je);
                }

            }
            catch (Exception de)
            {
                return false;
            }
            document.Close();
            return true;
        }
    }
}
