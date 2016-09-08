using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.OAuth;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Web.Models;
using ChemCloud.Web.Areas.Admin.Models;
using System.IO;
using System.Text;
using ChemCloud.Core.Helper;
using System.Web.Services;
using System.Web;
using ChemCloud.Model.Common;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class TransactionRecordController : BaseAdminController
    {
        HashSet_Common hashSet = new HashSet_Common();

        ITransactionRecordService bannerIndexService = ServiceHelper.Create<ITransactionRecordService>();

        public TransactionRecordController()
        {
        }

        public ViewResult TransactionRecord()
        {
            return new ViewResult();
        }

        #region 新增
        /// <summary>
        /// 新增banner信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string TransactionRecordAdd(TransactionRecordAddQuery query)
        {
            Result_Msg res = new Result_Msg();
            if (base.CurrentManager != null && base.CurrentManager.Id > 0)
            {
                res = bannerIndexService.TransactionRecordAdd(query);
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "您尚未登陆，或登录超时，请重新登陆";
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除banner信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteById(int Id)
        {
            Result_Msg res = new Result_Msg();
            if (base.CurrentManager != null && base.CurrentManager.Id > 0)
            {
                long userId = base.CurrentManager.Id;

                res = bannerIndexService.DeleteById(Id, userId);
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "您尚未登陆，或登录超时，请重新登陆";
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 列表2
        /// <summary>
        /// 获取banner信息列表2
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetTransactionRecordList(TransactionRecordQuery query)
        {
            Result_List_Pager<Result_TransactionRecord> res = bannerIndexService.GetTransactionRecordList(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 根据ID获取指定对象
        [HttpPost]
        public string GetObjectById(int Id)
        {
            Result_TransactionRecord res = new Result_TransactionRecord();
            if (base.CurrentManager != null && base.CurrentManager.Id > 0)
            {
                long userId = base.CurrentManager.Id;
                res = bannerIndexService.GetObjectById(Id);
            }
            else
            {

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }
        #endregion
        #region 修改
        /// <summary>
        /// 修改banner信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string ModifyTransactionRecord(TransactionRecordModifyQuery query)
        {
            Result_Msg res = new Result_Msg();
            if (base.CurrentManager != null && base.CurrentManager.Id > 0)
            {
                res = bannerIndexService.ModifyTransactionRecord(query);
            }
            else
            {

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        /// <summary>
        /// 统计计算数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string ComputeFun(TransactionRecordCompute query)
        {
            Result_Model<TransactionRecord> res = new Result_Model<TransactionRecord>();

            if (base.CurrentManager != null && base.CurrentManager.Id > 0)
            {
                long userId = base.CurrentManager.Id;
                res = bannerIndexService.ComputeFun(query);
            }
            else
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "计算" + query.YearMonth + "数据失败";
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        #region 图片上传相关

        [WebMethod]
        public string UploadImage1()
        {
            ResultModel<ImageInfo> res = new ResultModel<ImageInfo>()
            {
                Msg = new Result_Msg()
                {
                    IsSuccess = true,
                    Message = string.Empty
                },
                Model = new ImageInfo()
            };
            try
            {
                HttpPostedFile postFile = HttpContext.ApplicationInstance.Context.Request.Files[0];
                //var width = Request.Form["imgWidth"];
                //var height = Request.Form["imgHeight"];
                string filerealname = postFile.FileName;//获得文件名
                string fileNameExt = System.IO.Path.GetExtension(filerealname);//获得文件扩展名
                if (!CheckFileExt(fileNameExt))
                {
                    res.Msg.IsSuccess = false;
                    res.Msg.Message += (res.Msg.Message + "\r\n");
                }
                if (res.Msg.IsSuccess)
                {
                    res = UpLoadFile(postFile);
                }
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "图片上传失败，失败原因：" + ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

            //Response.Write(JsonHelper.ObjectToJson(res));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postFile"></param>
        /// <returns></returns>
        public ResultModel<ImageInfo> UpLoadFile(HttpPostedFile postFile)
        {
            return UpLoadFile(postFile, "/UploadBanner/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpFile"></param>
        /// <param name="toFilePath">图片存放路径</param>
        /// <returns></returns>
        public ResultModel<ImageInfo> UpLoadFile(HttpPostedFile httpFile, string toFilePath)
        {
            ResultModel<ImageInfo> img = new ResultModel<ImageInfo>()
            {
                Msg = new Result_Msg() { IsSuccess = true, Message = string.Empty },
                Model = new ImageInfo()
            };
            try
            {
                img.Model.ImageName = httpFile.FileName; //获取要保存的文件信息
                img.Model.ExtensionName = System.IO.Path.GetExtension(img.Model.ImageName);//获得文件扩展名
                if (CheckFileExt(img.Model.ExtensionName))
                {
                    //检查保存的路径 是否有/结尾
                    if (toFilePath.EndsWith("/") == false) toFilePath = toFilePath + "/";
                    //按日期归类保存
                    string datePath = DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd") + "/";
                    if (true)
                    {
                        //toFilePath += datePath;
                        img.Model.PathUrl = toFilePath;
                    }
                    //物理完整路径                    
                    string toFileFullPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + toFilePath;
                    //检查是否有该路径  没有就创建
                    if (!System.IO.Directory.Exists(toFileFullPath))
                    {
                        Directory.CreateDirectory(toFileFullPath);
                    }
                    //得到服务器文件保存路径
                    string toFile = Server.MapPath("~" + toFilePath);
                    string f_file = getName(img.Model.ImageName);
                    //将文件保存至服务器
                    httpFile.SaveAs(toFile + f_file);
                }
                else
                {
                    img.Msg.IsSuccess = false;
                    img.Msg.Message = "文件不合法";
                }
            }
            catch (Exception e)
            {
                img.Msg.IsSuccess = false;
                img.Msg.Message = "文件上传失败,错误原因：" + e.Message;
            }
            return img;
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="fileNamePath"></param>
        /// <returns></returns>
        private string getName(string fileNamePath)
        {
            string[] name = fileNamePath.Split('\\');
            return name[name.Length - 1];
        }

        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        /// <param name="_fileExt"></param>
        /// <returns></returns>
        private bool CheckFileExt(string _fileExt)
        {
            string[] allowExt = new string[] { ".gif", ".jpg", ".jpeg", ".rar", ".png" };
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i] == _fileExt) { return true; }
            }
            return false;
        }

        public static string GetFileName()
        {
            Random rd = new Random();
            StringBuilder serial = new StringBuilder();
            serial.Append(DateTime.Now.ToString("HHmmss"));
            serial.Append(rd.Next(100, 999).ToString());
            return serial.ToString();

        }

        #endregion

    }




}
