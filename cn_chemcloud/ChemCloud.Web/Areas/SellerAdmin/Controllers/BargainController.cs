using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Express;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class BargainController : BaseSellerController
    {
        // GET: SellerAdmin/Bargain
        public ActionResult Index()
        {
            return View();
        }
        public BargainController()
        {
        }

        public ActionResult Management()
        {

            BargainQuery queryall = new BargainQuery()
            {
                ShopId = long.Parse(base.CurrentSellerManager.ShopId.ToString()),

            };
            int barginbillcount = ServiceHelper.Create<IMargainBillService>().GetBargain<MargainBill>(queryall).Total;

            ViewBag.barginbillcount = barginbillcount;

            BargainQuery querynoreply = new BargainQuery()
            {
                ShopId = long.Parse(base.CurrentSellerManager.ShopId.ToString()),
                BillStatus = EnumBillStatus.SubmitBargain,

            };
            int barginbillnoreplycount = ServiceHelper.Create<IMargainBillService>().GetBargain<MargainBill>(querynoreply).Total;

            ViewBag.barginbillnoreplycount = barginbillnoreplycount;

            return View();
        }
        // GET: SellerAdmin/Bargain/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SellerAdmin/Bargain/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SellerAdmin/Bargain/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SellerAdmin/Bargain/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SellerAdmin/Bargain/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SellerAdmin/Bargain/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SellerAdmin/Bargain/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult List(DateTime? startDate, DateTime? endDate, int? BillStatus, string billno, string buyName, int page, int rows)
        {

            EnumBillStatus? nullable ;
            BargainQuery bargainQuery = new BargainQuery()
            {
                StartDate = startDate,
                EndDate = endDate,
                // MemberName = buyName,
                UserId = base.CurrentUser.Id,
                BillNo = billno
            };
            BargainQuery bargainQuery1 = bargainQuery;
            int? nullable1 = BillStatus;
            if (nullable1.HasValue)
            {
                nullable = new EnumBillStatus?((EnumBillStatus)nullable1.GetValueOrDefault());
            }
            else
            {
                nullable = null;
            }
            bargainQuery1.BillStatus = nullable;
            bargainQuery1.MemberName = buyName;
            bargainQuery1.ShopId = new long?(base.CurrentSellerManager.ShopId);
            bargainQuery1.PageSize = rows;
            bargainQuery1.PageNo = page;
            PageModel<MargainBill> margain = ServiceHelper.Create<IMargainBillService>().GetBargain<MargainBill>(bargainQuery1);
            IEnumerable<MargainBill> array =
                from item in margain.Models.ToArray()
                select new MargainBill()
                {
                    Id = item.Id,
                    BillNo = item.BillNo,
                    BillStatus = item.BillStatus,
                    CreateDate = item.CreateDate,
                    MemberId = item.MemberId,
                    IsDelete = item.IsDelete,
                    strCreateDate = item.CreateDate.ToString("yyyy-MM-dd"),
                    TotalAmount = item.TotalAmount,
                    DeliverType = item.DeliverType,
                    strDeliverDate = item.DeliverDate.ToString("yyyy-MM-dd HH"),
                    DeliverAddress = item.DeliverAddress,
                    DeliverCost = item.DeliverCost,
                    PayMode = item.PayMode,
                    MemberName = (ServiceHelper.Create<IMemberService>().GetMember(item.MemberId).UserName) == null ? "" :
                    (ServiceHelper.Create<IMemberService>().GetMember(item.MemberId).UserName),
                    ShopId = item.ShopId,
                    ShopName = (ServiceHelper.Create<IShopService>().GetShop(item.ShopId).ShopName) == null ? "" :
                    (ServiceHelper.Create<IShopService>().GetShop(item.ShopId).ShopName),
                    CoinType = item.CoinType,
                    CoinTypeName = item.CoinTypeName,
                    BuyerEmail = (ServiceHelper.Create<IMemberService>().GetMember(item.MemberId).Email) == null ? "" :
                    (ServiceHelper.Create<IMemberService>().GetMember(item.MemberId).Email),
                    CASNo = ServiceHelper.Create<IMargainBillService>().GetBillByNo(item.BillNo)._MargainBillDetail.OrderBy(q => q.Id).FirstOrDefault().CASNo,
                    ProudctNum = ServiceHelper.Create<IMargainBillService>().GetBillByNo(item.BillNo)._MargainBillDetail.OrderBy(q => q.Id).FirstOrDefault().Num,
                    BuyerMessage = ServiceHelper.Create<IMargainBillService>().GetBillByNo(item.BillNo)._MargainBillDetail.OrderBy(q => q.Id).FirstOrDefault().BuyerMessage,
                    MessageReply = ServiceHelper.Create<IMargainBillService>().GetBillByNo(item.BillNo)._MargainBillDetail.OrderBy(q => q.Id).FirstOrDefault().MessageReply
                };

            DataGridModel<MargainBill> dataGridModel = new DataGridModel<MargainBill>()
            {
                rows = array,
                total = margain.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult Detail(long id, bool updatePrice = false)
        {
            string isreview = "false";
            MargainBill _MargainBill = ServiceHelper.Create<IMargainBillService>().GetBillById(id, base.CurrentUser.Id);

            if (_MargainBill.CoinType == 1)
            {
                _MargainBill.CoinTypeName = "CNY";
            }
            else
            {
                _MargainBill.CoinTypeName = "USD";
            }

            _MargainBill.MemberName = ServiceHelper.Create<IMemberService>().GetMember(_MargainBill.MemberId).RealName == null ? "" :
                ServiceHelper.Create<IMemberService>().GetMember(_MargainBill.MemberId).RealName;

            _MargainBill.ShopName = ServiceHelper.Create<IShopService>().GetShop(_MargainBill.ShopId).ShopName == null ? "" :
                ServiceHelper.Create<IShopService>().GetShop(_MargainBill.ShopId).ShopName;

            foreach (var item in _MargainBill._MargainBillDetail)
            {
                item.BidderName = ServiceHelper.Create<IMemberService>().GetMember(item.Bidder) == null ?
                    ServiceHelper.Create<IShopService>().GetShopName(item.Bidder) : ServiceHelper.Create<IMemberService>().GetMember(item.Bidder).RealName;
            }
            if (_MargainBill.BillStatus == EnumBillStatus.Bargaining || _MargainBill.BillStatus == EnumBillStatus.SubmitBargain)
            {
                isreview = "true";
            }
            if (!string.IsNullOrEmpty(_MargainBill.DeliverAddress) && !"null".Equals(_MargainBill.DeliverAddress))
            {
                ShippingAddressInfo addressinfo = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddress(long.Parse(_MargainBill.DeliverAddress));
                if (addressinfo != null)
                {
                    _MargainBill.DeliverAddress = addressinfo.RegionFullName + ">" + addressinfo.Address;
                }
            }
            else
            {
                _MargainBill.DeliverAddress = "" + ">" + "";
            }

            ViewBag.CurrentUser = base.CurrentUser.Id;
            ViewBag.isreview = isreview;
            ViewBag.UpdatePrice = updatePrice;
            return View(_MargainBill);
        }

        [HttpPost]
        public JsonResult UpdateBargainPrice(long BargainDId, decimal UpPrice)
        {
            ServiceHelper.Create<IMargainBillService>().UpdateBargainPrice(BargainDId, UpPrice);
            return Json(new { success = true });
        }
        [HttpPost]
        [ShopOperationLog(Message = "供应商取消议单")]
        public JsonResult CloseBargain(string bargainno)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IMargainBillService>().SellerCloseBargain(bargainno);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }


        [HttpPost]
        [ShopOperationLog(Message = "批量删除询盘")]
        public JsonResult BatchCloseBargain(string Ids)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IMargainBillService>().SellerBatchCloseBargain(Ids);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 修改议价单方法
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult UpdateBargaion(long Id, int Num, decimal PurchasePrice, string MessageReply)
        {
            try
            {
                MargainBill model = ServiceHelper.Create<IMargainBillService>().GetBillById(Id, base.CurrentUser.Id);
                if (model != null)
                {
                    //更新回复信息
                    MargainBillDetail item = model._MargainBillDetail.OrderByDescending(q => q.Id).FirstOrDefault();
                    item.MessageReply = MessageReply;
                    item.PurchasePrice = PurchasePrice;
                    item.Num = Num;
                    ServiceHelper.Create<IMargainBillService>().UpdateMargainDetailMessageReply(item);

                    //更改询盘状态
                    ServiceHelper.Create<IMargainBillService>().UpdateBillStatu(model.Id);

                    ////添加新的询盘记录
                    //MargainBillDetail modeldetail = model._MargainBillDetail.LastOrDefault();
                    //MargainBillDetail detail = new MargainBillDetail();
                    //detail.CreateDate = DateTime.Now;
                    //detail.IsDelete = 0;
                    //detail.BillNo = modeldetail.BillNo;
                    //detail.ProductId = modeldetail.ProductId;
                    //detail.ProductName = modeldetail.ProductName;
                    //detail.MarketPrice = modeldetail.MarketPrice;
                    //detail.PackingUnit = modeldetail.PackingUnit;
                    //detail.Purity = modeldetail.Purity;
                    //detail.Num = Num;
                    //detail.PurchasePrice = PurchasePrice;
                    //detail.Bidder = base.CurrentUser.Id;
                    //detail.CASNo = modeldetail.CASNo;
                    //ServiceHelper.Create<IMargainBillService>().AddMargainDetail(detail);

                    return Json(new { success = true, msg = "修改成功！" });
                }
                else
                {
                    return Json(new { success = false, msg = "修改失败！" });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "修改失败！" });
            }
        }
        /// <summary>
        /// 结束议价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult OverBargain(long Id)
        {
            try
            {
                ServiceHelper.Create<IMargainBillService>().OverBargain(Id);
                return Json(new { success = true, msg = "成功！" });
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "失败！" });
            }
        }
    }
}
