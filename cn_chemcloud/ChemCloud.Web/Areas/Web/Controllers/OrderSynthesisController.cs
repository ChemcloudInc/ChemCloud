using ChemCloud.Core;
using ChemCloud.DBUtility;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    /// <summary>
    /// 代购订单
    /// </summary>
    public class OrderSynthesisController : BaseMemberController
    {
        /// <summary>
        /// 发布代购订单
        /// </summary>
        /// <returns></returns>
        public ActionResult AddOrderSynthesis()
        {
            return View();
        }
        public ActionResult uploadfile(long parentId, string attachmentName, string type)
        {
            List<AttachmentInfo> models = ServiceHelper.Create<IOrderSynthesisService>().GetAttachmentInfosById(parentId);
            ViewBag.parentId = parentId;
            ViewBag.type = type;
            ViewBag.attachmentName = attachmentName;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View();
        }
        /// <summary>
        /// 我的订单管理列表
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="orderStatus"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="productName"></param>
        /// <returns></returns>
        public ActionResult ManageMent(int orderStatus = -1, int pageSize = 10, int pageNo = 1, string orderNum = "")
        {
            OrderSynthesisQuery opQuery = new OrderSynthesisQuery();

            opQuery = new OrderSynthesisQuery()
            {
                userId = base.CurrentUser.Id,
                PageSize = pageSize,
                PageNo = pageNo,
                status = orderStatus,
                OrderNumber = orderNum
            };
            PageModel<OrderSynthesis> opModel = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesisList(opQuery);
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = pageNo,
                ItemsPerPage = pageSize,
                TotalItems = opModel.Total
            };
            ViewBag.pageInfo = pagingInfo;
            return View(opModel.Models);
        }
        public JsonResult GetImportOpCount()
        {
            long num = 0;
          //  object obj = Cache.Get("Cache-UserUploadFile");
            //if (obj != null)
            //{
              //  num = (string.IsNullOrEmpty(obj.ToString()) ? 0 : long.Parse(obj.ToString()));
            //}
            return Json(new { Count = num }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
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
                return base.Content("The format is not correct！", "text/html");
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

        private bool IsAllowExt(HttpPostedFileBase item)
        {
            var notAllowExt = "," + ConfigurationManager.AppSettings["NotAllowExt"] + ",";
            var ext = "," + Path.GetExtension(item.FileName).ToLower() + ",";
            if (notAllowExt.Contains(ext))
                return false;
            return true;
        }

        /// <summary>
        /// 添加合成订单
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="productCount"></param>
        /// <param name="productChundu"></param>
        /// <param name="productPrice"></param>
        /// <param name="productDesc"></param>
        /// <param name="orderDate"></param>
        /// <param name="email"></param>
        /// <param name="companyName"></param>
        /// <param name="conUser"></param>
        /// <param name="webSite"></param>
        /// <param name="telPhone"></param>
        /// <returns></returns>
        public JsonResult AddOrderInfo(string productName, string casNo, string chemName, string productCount, string productChundu, string productPrice, string productDesc, string email, string companyName, string conUser, string webSite = "", string telPhone = "", string mol = "", string SynthesisCycle = "")
        {

            IOrderSynthesisService iops = ServiceHelper.Create<IOrderSynthesisService>();
            int i;
            if (!int.TryParse(productCount, out i))
            {
                return Json(new { Successful = "请输入有效的数量" });
            }
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            OrderSynthesis op = new OrderSynthesis
            {
                OrderNumber = _orderBO.GenerateOrderNumber().ToString(),
                ProductName = productName,
                ProductCount = productCount,
                ProductPurity = productChundu,
                Price = productPrice,
                ProductDesc = productDesc,
                Email = email,
                CompanyName = companyName,
                ConUserName = conUser,
                ConWebsite = webSite,
                ConTelPhone = telPhone,
                OrderTime = DateTime.Now,
                UserId = base.CurrentUser.Id,
                Status = 0,
                Mol = mol,
                CASNo = casNo,
                ChemName = chemName,
                SynthesisCycle = SynthesisCycle
            };
            OrderSynthesis models = iops.AddOrderSynthesis(op);
            if (models != null)
                return Json(new { Successful = true, parentId = models.Id });
            else
                return Json(new { Successful = false });
        }
        /// <summary>
        /// 会员获取基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOrderInfo(long id)
        {
            OrderSynthesis opinfo = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesis(id);
            if (opinfo != null)
            {
                return Json(opinfo);
            }
            else
            {
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult AddAttachment(long parentId, string attachmentName)
        {
            AttachmentInfo attachmentInfo = new AttachmentInfo();
            attachmentInfo.ParentId = parentId;
            attachmentInfo.AttachmentName = attachmentName;
            attachmentInfo.UserId = base.CurrentUser.Id;
            attachmentInfo.Type = 4;
            bool flag = ServiceHelper.Create<IOrderSynthesisService>().AddAttachment(attachmentInfo);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        /// <summary>
        /// 会员修改 状态  上传凭证
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="zfimg"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateOrderInfo(long id, int status, string zfimg)
        {
            OrderSynthesis opinfo = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesis(id);
            if (opinfo != null)
            {
                if (!string.IsNullOrWhiteSpace(zfimg))
                {
                    opinfo.ZhifuImg = zfimg;
                }
                opinfo.Status = status;
                ServiceHelper.Create<IOrderSynthesisService>().UpdateOrderSynthesis(opinfo);
                return Json(new { Successful = true });
            }
            else
            {
                return Json(new { Successful = false });
            }
        }
        public JsonResult EditOrderInfo(long id, string productName, string casNo, string chemName, string productCount, string productChundu, string productPrice, string productDesc, string email, string companyName, string conUser, string webSite = "", string telPhone = "", string mol = "", string SynthesisCycle = "")
        {
            OrderSynthesis opinfo = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesis(id);
            if (id != 0)
            {
                OrderSynthesis op = new OrderSynthesis
                {
                    Id = id,
                    OrderNumber = opinfo.OrderNumber,
                    ProductName = productName,
                    ProductCount = productCount,
                    ProductPurity = productChundu,
                    Price = productPrice,
                    ProductDesc = productDesc,
                    Email = email,
                    CompanyName = companyName,
                    ConUserName = conUser,
                    ConWebsite = webSite,
                    ConTelPhone = telPhone,
                    OrderTime = opinfo.OrderTime,
                    UserId = base.CurrentUser.Id,
                    Status = 0,
                    Mol = opinfo.Mol,
                    CASNo = casNo,
                    ChemName = chemName,
                    SynthesisCycle = SynthesisCycle
                };
                ServiceHelper.Create<IOrderSynthesisService>().UpdateOrderSynthesis(op);
                return Json(new { Successful = true });
            }
            else
            {
                return Json(new { Successful = false });
            }
        }
        [HttpPost]
        public JsonResult GetExpressData(string shipOrderNumber)
        {
            if (string.IsNullOrWhiteSpace(shipOrderNumber))
            {
                throw new HimallException("错误的订单信息");
            }
            OrderExpressQuery oe = ServiceHelper.Create<IOrderExpressQueryService>().GetOrderExpressById(shipOrderNumber);
            return Json(oe);
        }

        public JsonResult DelOrderInfo(long id = 0)
        {
            if (id == 0)
            {
                return Json(new { Successful = false });
            }
            else
            {
                ServiceHelper.Create<IOrderSynthesisService>().DelOrderInfo(id);
                return Json(new { Successful = true });
            }
        }
        /// <summary>
        /// 报价列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AcceptCustomizedOrderList(string OrderNumber)
        {
            string strsql = string.Format("SELECT a.*,b.ShopName FROM dbo.ChemCloud_OrderSynthesisOffer as a left join ChemCloud_Shops as b on a.ShopId=b.Id where a.OrderNumber='" + OrderNumber + "';");

            DataTable dt = DbHelperSQL.QueryDataTable(strsql);

            string json = ChemCloud.Core.JsonHelper.DataTable2Json(dt);

            return Json(new { success = true, jsonstr = json });
        }


        [HttpPost]
        public JsonResult UpdateAcceptCustomizedOrder(string orderid, string shopid, string price, string cycel)
        {
            if (!string.IsNullOrEmpty(orderid))
            {
                OrderSynthesis querymodel = new OrderSynthesis();
                querymodel.Id = long.Parse(orderid);
                querymodel.ZhifuImg = shopid;
                querymodel.Price = price;
                querymodel.SynthesisCycle = cycel;
                querymodel.Status = 2;

                if (ServiceHelper.Create<IOrderSynthesisService>().UpdateAcceptCustomizedOrder(querymodel))
                {
                    return Json(new { success = true, msg = "操作成功！" });
                }
                else
                {
                    return Json(new { success = false, msg = "操作失败！" });
                }
            }
            else
            {
                return Json(new { success = false, msg = "操作失败！" });
            }
        }

        [HttpPost]
        public JsonResult OrderSynthesis_Attachment_List(string Id)
        {
            string strsql = string.Format("SELECT Id,AttachmentName FROM ChemCloud.dbo.ChemCloud_Attachment where type=4 and ParentId='" + Id + "';");

            DataTable dt = DbHelperSQL.QueryDataTable(strsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                string json = ChemCloud.Core.JsonHelper.DataTable2Json(dt);
                return Json(new { success = true, jsonstr = json });
            }
            else
            {
                return Json(new { success = false, jsonstr = "" });
            }
        }
    }
}