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

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class IWantToBuyUserController : BaseWebController
    {
        JobsService jobsService = new JobsService();
        HashSet_Common hashSet = new HashSet_Common();
        IIWantToBuyService iWantToBuyService = ServiceHelper.Create<IIWantToBuyService>();
        public IWantToBuyUserController()
        {

        }

        public ViewResult IWantToBuyUser()
        {
            return new ViewResult();
        }

        #region 根据ID获取指定对象
        /// <summary>
        /// 根据ID获取指定对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetObjectById_User(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model_List_IWantToBuy<Result_IWantToBuy> resAll = new Result_Model_List_IWantToBuy<Result_IWantToBuy>()
            {
                Model = new Result_Model<Result_IWantToBuy>(),
                Msg = new Result_Msg() { IsSuccess = true, Message = string.Empty },
                List = new Result_List<Result_Model<Result_IWantToBuy>>()
            };
            try
            {
                Result_Model<IWantToBuy> job = iWantToBuyService.GetObjectById_Web(query.ParamInfo.Id);
                var listHash = hashSet.Get_DictionariesList();

                resAll.Model.Model = new Result_IWantToBuy()
                {
                    Id = job.Model.Id,
                    PurchaseID = job.Model.PurchaseID,
                    ProductName = job.Model.ProductName,
                    PurchaseNum = job.Model.PurchaseNum,
                    Quantity = job.Model.Quantity,
                    Remarks = job.Model.Remarks == null ? string.Empty : job.Model.Remarks,
                    TotalPrice = job.Model.TotalPrice,
                    Unit = job.Model.Unit,
                    UnitPrice = job.Model.UnitPrice,
                    Address = job.Model.Address,
                    Status = job.Model.Status,
                    StatusStr = listHash.Where(y => y.DictionaryTypeId == 107 && y.DKey == job.Model.Status.ToString()).FirstOrDefault().DValue,

                    DeliveryDate = job.Model.DeliveryDate.ToString("yyyy-MM-dd"),
                    CreateDate = job.Model.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    StartDate = job.Model.StartDate.ToString("yyyy-MM-dd hh:mm"),
                    EndDate = job.Model.EndDate.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = job.Model.UpdateDate.ToString("yyyy-MM-dd hh:mm"),

                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == job.Model.TypeOfCurrency.ToString()).FirstOrDefault().Remarks
                };

                if (resAll.Model != null && resAll.Msg.IsSuccess)
                {
                    resAll.List = Get_PreNext_ById_Web(query.ParamInfo.Id);
                    if (!resAll.List.Msg.IsSuccess)
                    {
                        resAll.Msg.IsSuccess = false;
                        resAll.Msg.Message += "获取上一页、下一页失败！\n\r";
                    }
                }
            }
            catch (Exception ex)
            {
                resAll.Msg = new Result_Msg() { IsSuccess = false, Message = "读取失败，失败原因：" + ex.Message };
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(resAll);
        }
        #endregion

        #region 根据ID 前后两条信息
        /// <summary>
        /// 根据ID 前后两条信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result_List<Result_Model<Result_IWantToBuy>> Get_PreNext_ById_Web(long Id)
        {
            Result_List<Result_Model<Result_IWantToBuy>> res = new Result_List<Result_Model<Result_IWantToBuy>>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                List = new List<Result_Model<Result_IWantToBuy>>()
            };

            try
            {
                var listHash = hashSet.Get_DictionariesList();

                Result_List<Result_Model<IWantToBuy>> resIWantToBuy = iWantToBuyService.Get_PreNext_ById_Web(Id);
                res.Msg = resIWantToBuy.Msg;
                if (resIWantToBuy != null && resIWantToBuy.Msg.IsSuccess)
                {
                    foreach (var item in resIWantToBuy.List)
                    {
                        if (item.Msg.IsSuccess)
                        {
                            res.List.Add(new Result_Model<Result_IWantToBuy>()
                            {
                                Model = new Result_IWantToBuy()
                                {
                                    Id = item.Model.Id,
                                    //EndDate = item.Model.EndDate.ToString("yyyy-MM-dd HH;mm"),
                                    //CreateDate = item.Model.CreateDate.ToString("yyyy-MM-dd HH;mm"),
                                    //StartDate = item.Model.StartDate.ToString("yyyy-MM-dd HH;mm"),
                                    //UpdateDate = item.Model.UpdateDate.ToString("yyyy-MM-dd HH;mm")
                                    //UserId = item.Model.UserId,
                                    //LanguageType = item.Model.LanguageType == 1 ? listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == item.Model.LanguageType.ToString()).FirstOrDefault().DKey : listHash.Where(y => y.DictionaryTypeId == 10 && y.DValue == item.Model.LanguageType.ToString()).FirstOrDefault().Remarks,
                                },
                                Msg = item.Msg
                            });
                        }
                        else
                        {
                            res.List.Add(new Result_Model<Result_IWantToBuy>()
                            {
                                Model = new Result_IWantToBuy(),
                                Msg = item.Msg
                            });
                        }
                    }
                }
                else
                {
                    res.Msg = resIWantToBuy.Msg;
                }
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return res;
        }
        #endregion

        #region 采购商后台

        #region 当前采购商：新增-我要采购
        /// <summary>
        /// 当前采购商：新增-我要采购
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToBuyAdd(QueryCommon<IWantToBuyQuery> query)
        {

            DateTime now = DateTime.Now;
            query.ParamInfo.PurchaseID = base.CurrentUser.Id;
            query.ParamInfo.CreateDate = DateTime.Now;
            query.ParamInfo.UpdateDate = DateTime.Now;
            query.ParamInfo.StartDate = DateTime.Now;
            query.ParamInfo.EndDate = DateTime.Now;
            query.ParamInfo.PurchaseNum = GetPurchaseNum();
            query.ParamInfo.Status = 0;

            Result_Msg res = iWantToBuyService.IWantToBuy_Add_Web(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 当前采购商：我要采购列表
        /// <summary>
        /// 当前采购商：我要采购列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToBuyList_User(QueryCommon<IWantToBuyQuery> query)
        {
            if (base.CurrentUser != null)
            {
                query.ParamInfo.PurchaseID = base.CurrentUser.Id;
            }
            Result_List_Pager<Result_IWantToBuy> res = new Result_List_Pager<Result_IWantToBuy>();
            Result_List_Pager<IWantToBuy> resList = iWantToBuyService.IWantToBuyList_User(query);

            if (resList.Msg.IsSuccess)
            {
                var listHash = hashSet.Get_DictionariesList();

                res.PageInfo = resList.PageInfo;
                res.Msg = resList.Msg;
                res.List = resList.List.Select(x => new Result_IWantToBuy()
                {
                    Id = x.Id,
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
                    //SupplyModel = GetObjectById_Web_Supply(x.Id),
                    //SupplyList = GetObjectById_Web_SupplyList(x.Id).List,

                    DeliveryDate = x.DeliveryDate.ToString("yyyy-MM-dd hh:mm"),
                    CreateDate = x.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    StartDate = x.StartDate.ToString("yyyy-MM-dd hh:mm"),
                    EndDate = x.EndDate.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = x.UpdateDate.ToString("yyyy-MM-dd hh:mm"),

                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == x.TypeOfCurrency.ToString()).FirstOrDefault().Remarks
                }).ToList();
            }
            else
            {
                res.Msg = resList.Msg;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 当前采购商：我要采购列表 分页信息
        /// <summary>
        /// 当前采购商：我要采购列表 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string Get_PageInfo_User(QueryCommon<IWantToBuyQuery> query)
        {
            if (base.CurrentUser != null)
            {
                query.ParamInfo.PurchaseID = base.CurrentUser.Id;
            }
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            resModel = iWantToBuyService.Get_PageInfo_User(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion
        #region 终止公式，废弃采购，
        /// <summary>
        /// 终止公式，废弃采购，
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToBuy_UpdateStatus(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Msg res = new Result_Msg() { IsSuccess = true };
            DateTime dtNow = DateTime.Now;
            query.ParamInfo.UpdateDate = dtNow;
            int status = query.ParamInfo.Status;
            try
            {
                iWantToBuyService.UpdateBuy_Status(query);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 修改：我要采购
        /// <summary>
        /// 修改：我要采购
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToBuy_Update(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Msg res = new Result_Msg() { IsSuccess = true };
            DateTime dtNow = DateTime.Now;
            query.ParamInfo.UpdateDate = dtNow;

            try
            {
                Result_Model<IWantToBuy> resIWantToBuy = iWantToBuyService.GetObjectById_Web_Buy(query.ParamInfo.Id);
                if (resIWantToBuy.Msg.IsSuccess)
                {
                    switch (resIWantToBuy.Model.Status)
                    {
                        case 0:
                            break;
                        case 1:
                            res.Message = "“废弃采购”采购单不能修改！";
                            res.IsSuccess = false;
                            break;
                        case 2:
                            res.Message = "“终止公示”采购单不能修改！";
                            res.IsSuccess = false;
                            break;
                        case 3:
                            res.Message = "“已确定”采购单不能修改！";
                            res.IsSuccess = false;

                            break;
                        default:
                            break;
                    }
                    if (res.IsSuccess)
                    {
                        res = iWantToBuyService.IWantToBuy_Update(query);
                    }
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 设为中标，取消中标
        /// <summary>
        /// 设为中标，取消中标
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToSupply_UpdateStatus(QueryCommon<IWantToSupplyQuery> query)
        {
            DateTime dtNow = DateTime.Now;
            QueryCommon<IWantToSupplyQuery> query1 = new QueryCommon<IWantToSupplyQuery>()
            {
                ParamInfo = new IWantToSupplyQuery()
            };
            query1.ParamInfo.UpdateDate = dtNow;
            query1.ParamInfo.BidDate = dtNow;
            int status = query.ParamInfo.Status;

            QueryCommon<IWantToBuyQuery> query2 = new QueryCommon<IWantToBuyQuery>()
            {
                ParamInfo = new IWantToBuyQuery()
            };
            query2.ParamInfo.UpdateDate = dtNow;
            query2.ParamInfo.Id = query.ParamInfo.IWantToBuyID;

            Result_Msg res = new Result_Msg() { IsSuccess = true };
            Result_List<Result_IWantToSupply> list = GetObjectById_Web_SupplyList(query.ParamInfo.IWantToBuyID);
            try
            {
                if (list.Msg.IsSuccess)
                {
                    Result_Model<IWantToSupply> max = iWantToBuyService.MaxStatusSupply_By_Num(Convert.ToInt64(list.List.FirstOrDefault().PurchaseNum));
                    if (max.Msg.IsSuccess)
                    {
                        switch (max.Model.Status)
                        {
                            case 4:
                                res.Message = "已下单，无法修改中标状态";
                                break;
                            case 5:
                                res.Message = "已支付，无法修改中标状态";
                                break;
                            case 6:
                                res.Message = "已发货，无法修改中标状态";
                                break;
                            case 7:
                                res.Message = "已签收，无法修改中标状态";
                                break;
                            default:
                                break;
                        }
                        res.IsSuccess = false;
                        return Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    }

                    foreach (var item in list.List)
                    {
                        query1.ParamInfo.Id = item.Id;
                        if (query.ParamInfo.Id == item.Id)
                        {
                            #region 修改当前操作信息

                            switch (status)
                            {
                                case 0://设为：竞价成功
                                    query1.ParamInfo.Status = 1;
                                    iWantToBuyService.UpdateSupply_Status(query1);

                                    query2.ParamInfo.Status = 3;
                                    iWantToBuyService.UpdateBuy_Status(query2);

                                    break;
                                case 1://设为：竞价失败
                                    query1.ParamInfo.Status = 2;
                                    iWantToBuyService.UpdateSupply_Status(query1);

                                    query2.ParamInfo.Status = 0;
                                    iWantToBuyService.UpdateBuy_Status(query2);
                                    break;
                                case 2://设为：竞价成功
                                    query1.ParamInfo.Status = 1;
                                    iWantToBuyService.UpdateSupply_Status(query1);

                                    query2.ParamInfo.Status = 3;
                                    iWantToBuyService.UpdateBuy_Status(query2);

                                    break;
                                case 3://——————
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 修改其他信息
                            switch (status)
                            {
                                case 0://设为：竞价失败
                                    if (item.Status != 3)
                                    {
                                        query1.ParamInfo.Status = 2;
                                        iWantToBuyService.UpdateSupply_Status(query1);
                                    }
                                    else
                                    {

                                    }

                                    break;
                                case 1://——————

                                    break;
                                case 2://设为：竞价失败
                                    if (item.Status != 3)
                                    {
                                        query1.ParamInfo.Status = 2;
                                        iWantToBuyService.UpdateSupply_Status(query1);
                                    }
                                    else
                                    {

                                    }
                                    break;
                                case 3://——————
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 签收
        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToSupply_UpdateStatus7(QueryCommon<IWantToSupplyQuery> query)
        {
            DateTime dtNow = DateTime.Now;
            QueryCommon<IWantToSupplyQuery> query1 = new QueryCommon<IWantToSupplyQuery>()
            {
                ParamInfo = new IWantToSupplyQuery()
            };
            query1.ParamInfo.Id = query.ParamInfo.Id;
            query1.ParamInfo.UpdateDate = dtNow;
            query1.ParamInfo.BidDate = dtNow;

            QueryCommon<IWantToBuyQuery> query2 = new QueryCommon<IWantToBuyQuery>()
            {
                ParamInfo = new IWantToBuyQuery()
            };
            query2.ParamInfo.UpdateDate = dtNow;
            query2.ParamInfo.Id = query.ParamInfo.IWantToBuyID;

            QueryCommon<IWantToBuy_Orders> query3 = new QueryCommon<IWantToBuy_Orders>()
            {
                ParamInfo = new IWantToBuy_Orders()
            };
            query3.ParamInfo.IWantToSupplyID = query.ParamInfo.Id;

            Result_Msg res = new Result_Msg() { IsSuccess = true };
            Result_List<Result_IWantToSupply> list = GetObjectById_Web_SupplyList(query.ParamInfo.IWantToBuyID);
            try
            {
                if (list.Msg.IsSuccess)
                {
                    Result_Model<IWantToSupply> max = iWantToBuyService.MaxStatusSupply_By_Num(Convert.ToInt64(list.List.FirstOrDefault().PurchaseNum));
                    if (max.Msg.IsSuccess && max.Model.Status != 6)
                    {
                        res.Message = "当前订单异常（报价状态异常）";
                        res.IsSuccess = false;
                        return Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    }

                    query2.ParamInfo.Status = 7;
                    iWantToBuyService.UpdateBuy_Status(query2);//签收采购单

                    query1.ParamInfo.Status = 7;
                    iWantToBuyService.UpdateSupply_Status(query1);//签收竞价单

                    query3.ParamInfo.Status = 7;
                    iWantToBuyService.UpdateOrders_Status(query3);//签收订单
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 查看物流
        /// <summary>
        /// 查看物流
        /// </summary>
        /// <param name="query"></param>
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
            Result_Model<IWantToSupply> model1 = iWantToBuyService.GetObjectById_Supply(query.ParamInfo.Id);
            Result_Model<IWantToBuy_Orders> model = iWantToBuyService.GetObjectById_Web_Supply_DeliverGoods(model1.Model.Id);
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
            Result_List_Pager<IWantToSupply> list = iWantToBuyService.GetObjectById_Web_SupplyList_Pager(query);
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
                    ShopName = iWantToBuyService.GetCompanyInfo_ByUserIdAndUserType(x.SupplierID).Model.CompanyName,
                    ShopId = iWantToBuyService.GetCompanyInfo_ByUserIdAndUserType(x.SupplierID).Model.Id,

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
            Result_Model<IWantToBuy> res1 = iWantToBuyService.GetObjectById_Web_Buy(id);
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
        #region 我要采购：竞价列表 分页信息
        /// <summary>
        /// 我要采购：竞价列表 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string Get_PageInfo_IWantToSupply_List(QueryCommon<IWantToBuyQuery> query)
        {
            if (base.CurrentUser != null)
            {
                query.ParamInfo.PurchaseID = base.CurrentUser.Id;
            }
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            resModel = iWantToBuyService.Get_PageInfo_IWantToSupply_List(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion



        #region 我要采购：竞价列表 分页信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iWantToBuyId"></param>
        /// <returns></returns>
        public Result_List<Result_IWantToSupply> GetObjectById_Web_SupplyList(long iWantToBuyId)
        {
            Result_List<Result_IWantToSupply> res = new Result_List<Result_IWantToSupply>() { Msg = new Result_Msg() { IsSuccess = true }, List = new List<Result_IWantToSupply>() };
            Result_List<IWantToSupply> list = iWantToBuyService.GetObjectById_Web_SupplyList(iWantToBuyId);
            if (list.Msg.IsSuccess)
            {
                var listHash = hashSet.Get_DictionariesList();

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
                    ShopName = jobsService.GetCompanyInfo_ByUserIdAndUserType(x.SupplierID),

                    CreateDate = x.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    BidDate = x.BidDate.ToString("yyyy-MM-dd hh:mm"),
                    LatestDeliveryTime = x.LatestDeliveryTime.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = x.UpdateDate.ToString("yyyy-MM-dd hh:mm"),
                    Status = x.Status,
                    StatusStr = listHash.Where(y => y.DictionaryTypeId == 108 && y.DKey == x.Status.ToString()).FirstOrDefault().DValue
                }).ToList();

            }

            return res;
        }
        #endregion


        #region 当前采购商：生成订单
        /// <summary>
        /// 当前采购商：生成订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToBuyOrderAdd(QueryCommon<IWantToSupplyQuery> query)
        {
            DateTime now = DateTime.Now;
            Result_Model<IWantToSupply> res = iWantToBuyService.GetObjectById_Supply(query.ParamInfo.Id);

            Result_Model<IWantToBuy_Orders> res1 = iWantToBuyService.AddIWantToBuyOrder(res.Model);

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 根据采购单号，获取订单信息
        /// <summary>
        /// 根据采购单号，获取订单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public Result_Model<IWantToBuy_Orders> GetOrders_ByPurchaseNum(long purchaseNum)
        {
            DateTime now = DateTime.Now;

            Result_Model<IWantToBuy_Orders> res1 = iWantToBuyService.GetOrders_ByPurchaseNum(purchaseNum);

            if (res1.Msg.IsSuccess)
            {
                //if (res1.Model.PayStatus==1)
                //{
                //    return 4;
                //}
                //else
                //{

                //}
            }
            return res1;
        }
        #endregion

        #endregion

        #region 公共
        /// <summary>
        /// 生成采购编号：时间+随机数
        /// </summary>
        /// <returns></returns>
        public long GetPurchaseNum()
        {
            return
            Convert.ToInt64(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
            + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Millisecond.ToString("000")
            + GetRandomNum());
        }
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <returns></returns>
        public int GetRandomNum()
        {
            Random r = new Random();
            return r.Next(100, 999);
        }
        /// <summary>
        /// 获取当前登录用户类型信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string GetCurrentUserType(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<CurrentUserInfo> res = new Result_Model<CurrentUserInfo>() { Model = new CurrentUserInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            if (base.CurrentUser != null)
            {
                res.Model.Id = base.CurrentUser.Id;
                res.Model.UserType = base.CurrentUser.UserType;
                res.Model.UserName = base.CurrentUser.UserName;
            }
            else
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "获取当前用户登陆信息失败" };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        /// <summary>
        /// 下拉框赋值
        /// </summary>
        /// <returns></returns>
        public string GetSelectOptionList()
        {
            Result_List<ChemCloud_Dictionaries> res = new Result_List<ChemCloud_Dictionaries>()
            {
                List = hashSet.Get_DictionariesList(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion



        #region WEB前端

        #region 供应商

        #endregion

        #region 采购商

        #endregion

        #endregion

        #region WEB后端

        #region 供应商

        #endregion

        #region 采购商

        #endregion

        #endregion

    }
}