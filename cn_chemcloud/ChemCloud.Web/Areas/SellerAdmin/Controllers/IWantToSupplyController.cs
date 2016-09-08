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
using ChemCloud.Model.Common;
using ChemCloud.Service;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class IWantToSupplyController : BaseWebController
    {
        HashSet_Common hashSet = new HashSet_Common();
        IIWantToBuyService supplyService = ServiceHelper.Create<IIWantToBuyService>();

        public IWantToSupplyController()
        {

        }


        public ViewResult IWantToSupply()
        {
            return new ViewResult();
        }

        #region 新增
        /// <summary>
        /// 新增招聘信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string JobsAdd(QueryCommon<JobsAddQuery> query)
        {
            DateTime now = DateTime.Now;
            query.ParamInfo.UserId = base.CurrentUser.Id;
            query.ParamInfo.UserType = base.CurrentUser.UserType;
            query.ParamInfo.ApprovalStatus = 1;
            query.ParamInfo.CreateDate = now;
            query.ParamInfo.UpdateDate = now;

            IJobsService jobsService = ServiceHelper.Create<IJobsService>();
            Result_Msg res = jobsService.AddJob(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 删除
        /// <summary>
        /// 删除招聘信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteById(int Id)
        {
            long userId = base.CurrentUser.Id;
            IJobsService jobsService = ServiceHelper.Create<IJobsService>();
            Result_Msg res = jobsService.DeleteById(Id, userId);

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }
        #endregion
        #region 修改
        /// <summary>
        /// 新增招聘信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string ModifyJob(QueryCommon<JobsAddQuery> query)
        {
            Result_Msg res = new Result_Msg();
            if (base.CurrentUser.Id != query.ParamInfo.UserId)
            {
                res.IsSuccess = false;
                res.Message = "修改失败，您无权限修改该信息。请确认您已登录";
            }
            else
            {
                query.ParamInfo.UserId = base.CurrentUser.Id;
                query.ParamInfo.UserType = base.CurrentUser.UserType;
                query.ParamInfo.UpdateDate = DateTime.Now;
                query.ParamInfo.ApprovalStatus = 1;
                IJobsService jobsService = ServiceHelper.Create<IJobsService>();
                res = jobsService.ModifyJob(query);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 供应商 中心

        #region 我要采购列表
        /// <summary>
        /// 我要采购列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string SupplyList(QueryCommon<IWantToBuyQuery> query)
        {
            query.ParamInfo.PurchaseID = base.CurrentUser.Id;

            Result_List_Pager<Result_IWantToBuy> res = new Result_List_Pager<Result_IWantToBuy>();

            IIWantToBuyService jobsService = ServiceHelper.Create<IIWantToBuyService>();
            Result_List_Pager<IWantToBuy> resList = jobsService.GetIWantToBuyList_SupplyUser_Pager(query);
            var listHash = hashSet.Get_DictionariesList();

            if (resList.Msg.IsSuccess)
            {
                res.PageInfo = resList.PageInfo;
                res.Msg = resList.Msg;
                res.List = resList.List.Select(x => new Result_IWantToBuy()
                {
                    Id = x.Id,
                    SupplyModel = Get_SupplyModel_ByIWantToBuyID(x.Id),
                    //SupplierID = Get_SupplierID_ByIWantToBuyID(x.Id),

                    //IsMine = Get_SupplierID_ByIWantToBuyID(x.Id) > 0 ? 1 : 0,
                    //ShopName = jobsService.GetCompanyInfo_ByUserIdAndUserType(Get_SupplierID_ByIWantToBuyID(x.Id)).Model.ShopName,

                    PurchaseID = x.PurchaseID,
                    ProductName = x.ProductName,
                    PurchaseNum = x.PurchaseNum,
                    Quantity = x.Quantity,
                    Remarks = x.Remarks,
                    TotalPrice = x.TotalPrice,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    Address = x.Address,
                    Status = x.Status,
                    StatusStr = listHash.Where(y => y.DictionaryTypeId == 107 && y.DKey == x.Status.ToString()).FirstOrDefault().DValue,

                    DeliveryDate = x.DeliveryDate.ToString("yyyy-MM-dd hh:mm"),
                    CreateDate = x.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    StartDate = x.StartDate.ToString("yyyy-MM-dd hh:mm"),
                    EndDate = x.EndDate.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = x.UpdateDate.ToString("yyyy-MM-dd hh:mm"),

                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == x.TypeOfCurrency.ToString()).FirstOrDefault().Remarks
                }).ToList();

                foreach (var item in res.List)
                {
                    if (item.SupplyModel.Msg.IsSuccess)
                    {
                        item.SupplierID = item.SupplyModel.Model.SupplierID;
                        if (item.SupplyModel.Model.SupplierID == base.CurrentUser.Id)
                        {
                            item.IsMine = 1;//0：竞价中；1：我已中标；2：他人中标

                            Result_Model<ShopInfo> res1 = jobsService.GetCompanyInfo_ByUserIdAndUserType(item.SupplyModel.Model.SupplierID);
                            item.ShopName = res1.Model.CompanyName;
                        }
                        else
                        {
                            item.IsMine = 2;//0：竞价中；1：我已中标；2：他人中标

                            Result_Model<ShopInfo> res1 = jobsService.GetCompanyInfo_ByUserIdAndUserType(item.SupplyModel.Model.SupplierID);
                            item.ShopName = res1.Model.CompanyName;
                        }
                    }
                    else if (item.SupplierID == 0)
                    {
                        item.IsMine = 0;//0：竞价中；1：我已中标；2：他人中标
                    }
                    var suppy = Get_SupplyModel_ByUserIdAndIWantToBuyId(Convert.ToInt64(item.Id), base.CurrentUser.Id);
                    if (suppy.Msg.IsSuccess)
                    {
                        item.WhetherParticipation = 1;
                    }
                    else
                    {
                        item.WhetherParticipation = 0;
                    }
                }
            }
            else
            {
                res.Msg = resList.Msg;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 我要采购列表（分页）
        /// <summary>
        /// 我要采购列表（分页）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string Get_PageInfo_Member(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            query.ParamInfo.PurchaseID = base.CurrentUser.Id;

            resModel = supplyService.Get_PageInfo_IWantToBuyList_SupplyUser(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion
        #region 获取“我要采购——中标商家信息”
        /// <summary>
        /// 获取“我要采购——中标商家信息”
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result_Model<Result_IWantToSupply> Get_SupplyModel_ByIWantToBuyID(long userId)
        {
            Result_Model<Result_IWantToSupply> res = new Result_Model<Result_IWantToSupply>() { Msg = new Result_Msg() { IsSuccess = false } };
            Result_Model<IWantToSupply> res1 = supplyService.Get_SupplyModel_ByIWantToBuyID(userId);
            var listHash = hashSet.Get_DictionariesList();

            if (res1.Msg.IsSuccess)
            {
                res.Msg = res1.Msg;
                res.Model = new Result_IWantToSupply()
                {
                    Id = res1.Model.Id,
                    PurchaseNum = res1.Model.PurchaseNum.ToString(),
                    IWantToBuyID = res1.Model.IWantToBuyID,
                    UnitPrice = res1.Model.UnitPrice,
                    Quantity = res1.Model.Quantity,
                    Unit = res1.Model.Unit,
                    TotalPrice = res1.Model.TotalPrice,
                    SupplierID = res1.Model.SupplierID,
                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == res1.Model.TypeOfCurrency.ToString()).FirstOrDefault().Remarks,
                    ShopName = supplyService.GetCompanyInfo_ByUserIdAndUserType(res1.Model.SupplierID).Model.CompanyName,
                    IsMine = res1.Model.SupplierID == base.CurrentUser.Id ? 1 : 0,

                    CreateDate = res1.Model.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    BidDate = res1.Model.BidDate.ToString("yyyy-MM-dd hh:mm"),
                    LatestDeliveryTime = res1.Model.LatestDeliveryTime.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = res1.Model.UpdateDate.ToString("yyyy-MM-dd hh:mm"),
                    Status = res1.Model.Status,
                    StatusStr = listHash.Where(y => y.DictionaryTypeId == 108 && y.DKey == res1.Model.Status.ToString()).FirstOrDefault().DValue

                };
            }
            return res;
        }
        public Result_Model<IWantToSupply> Get_SupplyModel_ByUserIdAndIWantToBuyId(long iWantToBuyId, long userId)
        {
            return supplyService.Get_SupplyModel_ByUserIdAndIWantToBuyId(iWantToBuyId, userId);
        }

        #endregion
        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToSupplyAdd(QueryCommon<IWantToSupplyQuery> query)
        {
            Result_Msg res = new Result_Msg();
            try
            {
                Result_Model<IWantToSupply> resIWantToSupply = supplyService.GetObjectById_Web_Supply(query.ParamInfo.IWantToBuyID, base.CurrentUser.Id);
                Result_Model<IWantToBuy> resIWantToBuy = supplyService.GetObjectById_Web_Buy(query.ParamInfo.IWantToBuyID);
                DateTime now = DateTime.Now;

                query.ParamInfo.PurchaseNum = resIWantToBuy.Model.PurchaseNum;
                query.ParamInfo.Unit = resIWantToBuy.Model.Unit;
                query.ParamInfo.TypeOfCurrency = resIWantToBuy.Model.TypeOfCurrency;
                query.ParamInfo.SupplierID = base.CurrentUser.Id;

                query.ParamInfo.CreateDate = now;
                query.ParamInfo.UpdateDate = now;
                query.ParamInfo.BidDate = now;

                if (resIWantToSupply.Msg.IsSuccess == false)
                {
                    //记录不存在：新增记录
                    res = supplyService.AddIWantToSupply(query);
                }
                else
                {
                    //记录已存在：更新记录
                    res = supplyService.UpdateSupply(query);
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region 发货
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToSupply_DeliverGoods(QueryCommon<IWantToBuy_Orders> query)
        {
            Result_Msg res = new Result_Msg();
            try
            {
                query.ParamInfo.Status = 6;//已发货
                res = supplyService.UpdateOrders(query);
            }
            catch (Exception ex)
            {
                res = new Result_Msg() { IsSuccess = false, Message = "发货失败：" + ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #endregion

        #region 获取当前登录采购商的竞价详情
        /// <summary>
        /// 获取当前登录采购商的竞价详情
        /// </summary>
        /// <param name="iWantToBuy"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetObjectById_Supply(QueryCommon<IWantToSupplyQuery> query)
        {
            Result_Model<Result_IWantToSupply> res = new Result_Model<Result_IWantToSupply>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                Model = new Result_IWantToSupply()
            };
            var listHash = hashSet.Get_DictionariesList();
            Result_Model<IWantToSupply> model = supplyService.GetObjectById_Web_Supply(query.ParamInfo.IWantToBuyID, base.CurrentUser.Id);
            if (model.Msg.IsSuccess)
            {
                res.Model = new Result_IWantToSupply()
                {
                    Id = model.Model.Id,
                    IWantToBuyID = model.Model.IWantToBuyID,
                    PurchaseNum = model.Model.PurchaseNum.ToString(),
                    UnitPrice = model.Model.UnitPrice,
                    Quantity = model.Model.Quantity,
                    TotalPrice = model.Model.TotalPrice,
                    Unit = model.Model.Unit,

                    Status = model.Model.Status,
                    SupplierID = model.Model.SupplierID,
                    CreateDate = model.Model.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    LatestDeliveryTime = model.Model.LatestDeliveryTime.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = model.Model.UpdateDate.ToString("yyyy-MM-dd hh:mm"),
                    BidDate = model.Model.BidDate.ToString("yyyy-MM-dd hh:mm"),
                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == model.Model.TypeOfCurrency.ToString()).FirstOrDefault().Remarks,
                };
            }
            else
            {
                res.Msg = model.Msg;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 根据SupplyId，获取订单详情
        /// <summary>
        /// 根据SupplyId，获取订单详情
        /// </summary>
        /// <param name="iWantToBuy"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetObjectById_Supply_DeliverGoods(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<Result_IWantToBuy_Orders> res = new Result_Model<Result_IWantToBuy_Orders>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                Model = new Result_IWantToBuy_Orders()
            };
            var listHash = hashSet.Get_DictionariesList();
            Result_Model<IWantToSupply> model1 = supplyService.GetObjectById_Web_Supply(query.ParamInfo.Id, base.CurrentUser.Id);
            Result_Model<IWantToBuy_Orders> model = supplyService.GetObjectById_Web_Supply_DeliverGoods(model1.Model.Id);
            if (model.Msg.IsSuccess)
            {
                res.Model = new Result_IWantToBuy_Orders()
                {
                    Id = model.Model.Id,
                    ProductName = model.Model.ProductName,
                    PurchaseNum = model.Model.PurchaseNum.ToString(),
                    UnitPrice = model.Model.UnitPrice,
                    Quantity = model.Model.Quantity,
                    TotalPrice = model.Model.TotalPrice,
                    Unit = model.Model.Unit,

                    Status = model.Model.Status,
                    CreateDate = model.Model.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    PayDate = model.Model.PayDate.ToString("yyyy-MM-dd hh:mm"),
                    IWantToSupplyID = model.Model.IWantToSupplyID,
                    LogisticsNum = model.Model.LogisticsNum,
                    LogisticsType = model.Model.LogisticsType,
                    LogisticsDes = model.Model.LogisticsDes
                };
            }
            else
            {
                res.Msg = model.Msg;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion


        #region 我要采购：竞价列表
        /// <summary>
        /// 我要采购：竞价列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToSupply_List(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<Result_IWantToSupply> res = new Result_List_Pager<Result_IWantToSupply>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                List = new List<Result_IWantToSupply>(),
                PageInfo = new PageInfo()
            };
            Result_List_Pager<IWantToSupply> list = supplyService.GetObjectById_Web_SupplyList_Pager(query);
            if (list.Msg.IsSuccess)
            {
                var listHash = hashSet.Get_DictionariesList();
                res.PageInfo = list.PageInfo;

                res.List = list.List.Select(x => new Result_IWantToSupply()
                {
                    Id = x.Id,
                    PurchaseNum = x.PurchaseNum.ToString(),
                    IWantToBuyID = x.IWantToBuyID,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    TotalPrice = x.TotalPrice,
                    SupplierID = x.SupplierID,
                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == x.TypeOfCurrency.ToString()).FirstOrDefault().Remarks,
                    ShopName = supplyService.GetCompanyInfo_ByUserIdAndUserType(x.SupplierID).Model.CompanyName,
                    IsMine = x.SupplierID == base.CurrentUser.Id ? 1 : 0,

                    CreateDate = x.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    BidDate = x.BidDate.ToString("yyyy-MM-dd hh:mm"),
                    LatestDeliveryTime = x.LatestDeliveryTime.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = x.UpdateDate.ToString("yyyy-MM-dd hh:mm"),
                    Status = x.Status,
                    StatusStr = listHash.Where(y => y.DictionaryTypeId == 108 && y.DKey == x.Status.ToString()).FirstOrDefault().DValue
                }).ToList();

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }
        #endregion
        #region 我要采购：竞价列表 分页信息
        /// <summary>
        /// 我要采购：竞价列表 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string Get_PageInfo_IWantToSupply_List(QueryCommon<IWantToBuyQuery> query)
        {
            //if (base.CurrentUser != null)
            //{
            //    query.ParamInfo.PurchaseID = base.CurrentUser.Id;
            //}
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            resModel = supplyService.Get_PageInfo_IWantToSupply_List(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion
        #region 我要采购：竞价列表(我要采购详情)
        /// <summary>
        /// 我要采购：竞价列表(我要采购详情)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Get_SupplyList(long id)
        {
            Result_Model<Result_IWantToBuy> res = new Result_Model<Result_IWantToBuy>()
            {
                Msg = new Result_Msg(),
                Model = new Result_IWantToBuy()
            };
            Result_Model<IWantToBuy> res1 = supplyService.GetObjectById_Web_Buy(id);
            if (res1.Msg.IsSuccess)
            {
                var listHash = hashSet.Get_DictionariesList();

                res.Model = new Result_IWantToBuy()
                {
                    Id = res1.Model.Id,
                    PurchaseID = res1.Model.PurchaseID,
                    ProductName = res1.Model.ProductName,
                    PurchaseNum = res1.Model.PurchaseNum,
                    Quantity = res1.Model.Quantity,
                    Remarks = res1.Model.Remarks,
                    TotalPrice = res1.Model.TotalPrice,
                    Unit = res1.Model.Unit,
                    UnitPrice = res1.Model.UnitPrice,
                    Address = res1.Model.Address,
                    Status = res1.Model.Status,
                    StatusStr = listHash.Where(y => y.DictionaryTypeId == 107 && y.DKey == res1.Model.Status.ToString()).FirstOrDefault().DValue,

                    DeliveryDate = res1.Model.DeliveryDate.ToString("yyyy-MM-dd"),
                    CreateDate = res1.Model.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    StartDate = res1.Model.StartDate.ToString("yyyy-MM-dd hh:mm"),
                    EndDate = res1.Model.EndDate.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = res1.Model.UpdateDate.ToString("yyyy-MM-dd hh:mm"),

                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == res1.Model.TypeOfCurrency.ToString()).FirstOrDefault().Remarks
                };
            }
            return View(res);
        }
        #endregion

        public string GetSelectOptionList()
        {
            Result_List<ChemCloud_Dictionaries> res = new Result_List<ChemCloud_Dictionaries>()
            {
                List = hashSet.Get_DictionariesList(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
    }
}