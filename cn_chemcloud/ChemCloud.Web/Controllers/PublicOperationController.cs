using ChemCloud.Core;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Controllers
{
    public class PublicOperationController : Controller
    {
        public PublicOperationController()
        {
        }

        public ActionResult TestCache()
        {
            string str = "无";
            if (Cache.Get("tt") == null)
            {
                str = "失效";
                Log.Info("缓存已经失效");
                Cache.Insert("tt", "zhangsan", 7000);
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            string str = "";
            string filename = "";
            if (base.Request.Files.Count == 0)
            {
                return base.Content("NoFile", "text/html");
            }
            HttpPostedFileBase item = base.Request.Files[0];
            if (item == null || item.ContentLength <= 0 || !IsAllowExt(item))
            {
                return base.Content("格式不正确！", "text/html");
            }
            if (item != null)
            {
                string extension = System.IO.Path.GetExtension(item.FileName);
                string serverFilepath = Server.MapPath("/Storage/FileTemplate");
                string serverFileName = System.IO.Path.GetFileNameWithoutExtension(item.FileName);
                serverFileName = serverFileName + DateTime.Now.ToString("yyyyMMddHHmmssffff") + extension;
                if (!System.IO.Directory.Exists(serverFilepath))
                {
                    System.IO.Directory.CreateDirectory(serverFilepath);
                }
                string File = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Storage\\FileTemplate\\", serverFileName);
                filename = File;
                try
                {
                    object obj = Cache.Get("Cache-UserUploadFile");
                    if (obj != null)
                    {
                        Cache.Insert("Cache-UserUploadFile", int.Parse(obj.ToString()) + 1);
                    }
                    else
                    {
                        Cache.Insert("Cache-UserUploadFile", 1);
                    }
                    item.SaveAs(File);
                    str = ImgHelper.PostImg(File, "file");//文件路径
                }
                catch
                {

                }

            }
            return base.Content(str, "text/html", System.Text.Encoding.UTF8);//将文件名回传给页面的innerHTML
        }
        [HttpPost]
        public ActionResult UploadPictoPDF()
        {
            string str = "";
            string str1 = "";
            List<string> strs = new List<string>();
            if (base.Request.Files.Count == 0)
            {
                return base.Content("NoFile", "text/html");
            }
            for (int i = 0; i < base.Request.Files.Count; i++)
            {
                HttpPostedFileBase item = base.Request.Files[i];
                if (item == null || item.ContentLength <= 0 || !IsAllowExt(item))
                {
                    return base.Content("格式不正确！", "text/html");
                }

                //文件名
                Random random = new Random();
                DateTime now = DateTime.Now;
                str1 = string.Concat(now.ToString("yyyyMMddHHmmssffffff"), i, Path.GetExtension(item.FileName));

                //临时文件夹
                string str2 = Server.MapPath("~/temp/");
                if (!Directory.Exists(str2))
                {
                    Directory.CreateDirectory(str2);
                }

                str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/temp/", str1);


                try
                {
                    item.SaveAs(str);
                    strs.Add(str);
                }
                catch (Exception)
                {
                }
            }
            return base.Content(string.Join(",", strs), "text/html");
        }
        [HttpPost]
        public ActionResult UploadPic()
        {
            string str = "";
            string str1 = "";
            List<string> strs = new List<string>();
            if (base.Request.Files.Count == 0)
            {
                return base.Content("NoFile", "text/html");
            }
            for (int i = 0; i < base.Request.Files.Count; i++)
            {
                HttpPostedFileBase item = base.Request.Files[i];
                if (item == null || item.ContentLength <= 0 || !IsAllowExt(item))
                {
                    return base.Content("格式不正确！", "text/html");
                }

                //文件名
                Random random = new Random();
                DateTime now = DateTime.Now;
                str1 = string.Concat(now.ToString("yyyyMMddHHmmssffffff"), i, Path.GetExtension(item.FileName));

                //临时文件夹
                string str2 = Server.MapPath("~/temp/");
                if (!Directory.Exists(str2))
                {
                    Directory.CreateDirectory(str2);
                }
                str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/temp/", str1);
                try
                {
                    item.SaveAs(str);

                    string httpimg = ChemCloud.Core.ImgHelper.PostImg(str, "cerpic");

                    strs.Add(httpimg);
                }
                catch (Exception)
                {
                }
            }
            return base.Content(string.Join(",", strs), "text/html");
        }

        [HttpPost]
        public ActionResult UploadPicMember()
        {
            string str = "";
            string str1 = "";
            List<string> strs = new List<string>();
            if (base.Request.Files.Count == 0)
            {
                return base.Content("NoFile", "text/html");
            }
            for (int i = 0; i < base.Request.Files.Count; i++)
            {
                HttpPostedFileBase item = base.Request.Files[i];
                if (item == null || item.ContentLength <= 0 || !IsAllowExt(item))
                {
                    return base.Content("格式不正确！", "text/html");
                }
                Random random = new Random();
                DateTime now = DateTime.Now;
                str1 = string.Concat(now.ToString("yyyyMMddHHmmssffffff"), i, Path.GetExtension(item.FileName));
                string str2 = Server.MapPath("/Storage/MemberInfo/");
                if (!Directory.Exists(str2))
                {
                    Directory.CreateDirectory(str2);
                }

                try
                {
                    //str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/Storage/MemberInfo/");
                    str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/Storage/MemberInfo/", str1);

                    //拼接保存在数据的路径
                    //strs.Add(string.Concat("/Storage/MemberInfo/", str1));

                    //item.SaveAs(Path.Combine(str, str1));
                    item.SaveAs(str);

                    string httpimg = ChemCloud.Core.ImgHelper.PostImg(str, "cerpic");

                    strs.Add(httpimg);
                }
                catch (Exception)
                {
                }
            }
            return base.Content(string.Join(",", strs), "text/html");
        }

        public ActionResult UploadPictures()
        {
            return View();
        }

        public bool IsAllowExt(HttpPostedFileBase item)
        {
            var notAllowExt = "," + ConfigurationManager.AppSettings["NotAllowExt"] + ",";
            var ext = "," + Path.GetExtension(item.FileName).ToLower() + ",";
            if (notAllowExt.Contains(ext))
                return false;
            return true;
        }

        [HttpPost]
        public ActionResult UploadPicRec()
        {
            string str = "";
            string str1 = "";
            List<string> strs = new List<string>();
            if (base.Request.Files.Count == 0)
            {
                return base.Content("NoFile", "text/html");
            }
            for (int i = 0; i < base.Request.Files.Count; i++)
            {
                HttpPostedFileBase item = base.Request.Files[i];
                if (item == null || item.ContentLength <= 0 || !IsAllowExt(item))
                {
                    return base.Content("格式不正确！", "text/html");
                }
                Random random = new Random();
                DateTime now = DateTime.Now;
                str1 = string.Concat(now.ToString("yyyyMMddHHmmssffffff"), i, Path.GetExtension(item.FileName));
                string str2 = Server.MapPath("/Storage/Plat/RecImg/");
                if (!Directory.Exists(str2))
                {
                    Directory.CreateDirectory(str2);
                }
                //str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/Storage/Plat/RecImg/");
                str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/Storage/Plat/RecImg/", str1);
                //strs.Add(string.Concat("/Storage/Plat/RecImg/", str1));
                try
                {
                    //item.SaveAs(Path.Combine(str, str1));
                    

                    //拼接保存在数据的路径
                    //strs.Add(string.Concat("/Storage/MemberInfo/", str1));

                    //item.SaveAs(Path.Combine(str, str1));
                    item.SaveAs(str);

                    string httpimg = ChemCloud.Core.ImgHelper.PostImg(str, "cerpic");

                    strs.Add(httpimg);
                }
                catch (Exception)
                {
                }
            }
            return base.Content(string.Join(",", strs), "text/html");
        }
    }
}