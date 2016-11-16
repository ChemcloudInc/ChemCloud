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
    public class IWantToBuyController : BaseWebController
    {
        JobsService jobsService = new JobsService();
        HashSet_Common hashSet = new HashSet_Common();
        IIWantToBuyService iWantToBuyService = ServiceHelper.Create<IIWantToBuyService>();
        public IWantToBuyController()
        {

        }

        public ViewResult IWantToBuy()
        {
            return new ViewResult();
        }

        #region 根据ID获取指定对象
        /// <summary>
        /// 根据ID获取指定对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetObjectById_Web(long Id)
        {
            Result_Model_List_IWantToBuy<Result_IWantToBuy> resAll = new Result_Model_List_IWantToBuy<Result_IWantToBuy>()
            {
                Model = new Result_Model<Result_IWantToBuy>(),
                Msg = new Result_Msg() { IsSuccess = true, Message = string.Empty },
                List = new Result_List<Result_Model<Result_IWantToBuy>>()
            };
            try
            {
                Result_Model<IWantToBuy> job = iWantToBuyService.GetObjectById_Web(Id);
                var listHash = hashSet.Get_DictionariesList();

                resAll.Model.Model = new Result_IWantToBuy()
                {
                    Id = job.Model.Id,
                    PurchaseID = job.Model.PurchaseID,
                    ProductName = job.Model.ProductName,
                    PurchaseNum = job.Model.PurchaseNum,
                    Quantity = job.Model.Quantity,
                    Remarks = job.Model.Remarks,
                    TotalPrice = job.Model.TotalPrice,
                    Unit = job.Model.Unit,
                    UnitPrice = job.Model.UnitPrice,
                    Address = job.Model.Address,
                    Status = job.Model.Status,
                    StatusStr = listHash.Where(y => y.DictionaryTypeId == 107 && y.DKey == job.Model.Status.ToString()).FirstOrDefault().DValue,

                    DeliveryDate = job.Model.DeliveryDate.ToString("yyyy-MM-dd hh:mm"),
                    CreateDate = job.Model.CreateDate.ToString("yyyy-MM-dd hh:mm"),
                    StartDate = job.Model.StartDate.ToString("yyyy-MM-dd hh:mm"),
                    EndDate = job.Model.EndDate.ToString("yyyy-MM-dd hh:mm"),
                    UpdateDate = job.Model.UpdateDate.ToString("yyyy-MM-dd hh:mm"),

                    TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == job.Model.TypeOfCurrency.ToString()).FirstOrDefault().Remarks
                };

                if (resAll.Model != null && resAll.Msg.IsSuccess)
                {
                    resAll.List = Get_PreNext_ById_Web(Id);
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

        #region Web

        #region 采购商

        #region 我要采购 列表
        /// <summary>
        /// 我要采购 列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToBuyList_Web(QueryCommon<IWantToBuyQuery> query)
        {
            query.ParamInfo = new IWantToBuyQuery();
            if (base.CurrentUser != null)
            {
                query.ParamInfo.PurchaseID = base.CurrentUser.Id;
            }
            Result_List_Pager<Result_IWantToBuy> res = new Result_List_Pager<Result_IWantToBuy>();
            Result_List_Pager<IWantToBuy> resList = iWantToBuyService.Get_IWantToBuyList_Web_Buy(query);

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
                    //SupplyModel = GetObjectById_Web_Supply(x.Id, base.CurrentUser.Id),
                    SupplyList = Get_SupplyList_ByIWantToBuyId(x.Id).List,

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
        #region 我要采购 分页
        [WebMethod]

        public string Get_PageInfo_Buy_Web(QueryCommon<IWantToBuyQuery> query)
        {
            query.ParamInfo = new IWantToBuyQuery();
            if (base.CurrentUser != null)
            {
                query.ParamInfo.Id = base.CurrentUser.Id;
            }
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            resModel = iWantToBuyService.Get_PageInfo_Web_Buy(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion
        #region 我要采购：新增
        /// <summary>
        /// 我要采购：新增
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToBuy_Add_Web(QueryCommon<IWantToBuyQuery> query)
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
        #region 修改：设为中标，取消中标
        /// <summary>
        /// 修改：设为中标，取消中标
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToBuy_UpdateStatus(QueryCommon<IWantToSupplyQuery> query)
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
            Result_List<Result_IWantToSupply> list = Get_SupplyList_ByIWantToBuyId(query.ParamInfo.IWantToBuyID);
            try
            {
                if (list.Msg.IsSuccess)
                {
                    foreach (var item in list.List)
                    {
                        query1.ParamInfo.Id = item.Id;
                        if (query.ParamInfo.Id == item.Id)
                        {
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
                        }
                        else
                        {
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
        #region 根据 IWantToBuyId 获取 SupplyList
        /// <summary>
        /// 根据 IWantToBuyId 获取 SupplyList
        /// </summary>
        /// <param name="iWantToBuyId"></param>
        /// <returns></returns>
        public Result_List<Result_IWantToSupply> Get_SupplyList_ByIWantToBuyId(long iWantToBuyId)
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

        #endregion

        #region 供应商

        #region 我要报价:新增/修改
        /// <summary>
        /// 我要报价:新增/修改
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToSupply_AddOrUpdate(QueryCommon<IWantToSupplyQuery> query)
        {
            Result_Msg res = new Result_Msg();
            try
            {
                Result_Model<Result_IWantToSupply> resIWantToSupply = GetObjectById_Web_Supply(query.ParamInfo.IWantToBuyID, base.CurrentUser.Id);
                Result_Model<IWantToBuy> resIWantToBuy = iWantToBuyService.GetObjectById_Web_Buy(query.ParamInfo.IWantToBuyID);
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
                    res = iWantToBuyService.AddIWantToSupply(query);
                }
                else
                {
                    //记录已存在：更新记录
                    res = iWantToBuyService.UpdateSupply(query);
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region 所有采购商：我要采购列表
        /// <summary>
        /// 所有采购商：我要采购列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToSupplyList_Web(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<Result_IWantToBuy> res = new Result_List_Pager<Result_IWantToBuy>();
            Result_List_Pager<IWantToBuy> resList = iWantToBuyService.Get_IWantToBuyList_Web_Supply(query);

            if (resList.Msg.IsSuccess)
            {
                var listHash = hashSet.Get_DictionariesList();

                res.PageInfo = resList.PageInfo;
                res.Msg = resList.Msg;
                res.List = resList.List.Select(x => new Result_IWantToBuy()
                {
                    Id = x.Id,
                    SupplyModel = GetObjectById_Web_Supply(x.Id, base.CurrentUser.Id),

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
            }
            else
            {
                res.Msg = resList.Msg;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 获取分页信息

        public string Get_PageInfo_Supply_Web(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            resModel = iWantToBuyService.Get_PageInfo_Web_Supply(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion
        #region 根据IWantToBuyId , supplierId 获取当前登录供应商的竞价信息
        /// <summary>
        /// 根据IWantToBuyId , supplierId 获取当前登录供应商的竞价信息
        /// </summary>
        /// <param name="iWantToBuy"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public Result_Model<Result_IWantToSupply> GetObjectById_Web_Supply(long iWantToBuy, long supplierId)
        {
            var listHash = hashSet.Get_DictionariesList();

            Result_Model<Result_IWantToSupply> res = new Result_Model<Result_IWantToSupply>()
            {
                Msg = new Result_Msg() { },
                Model = new Result_IWantToSupply()
            };
            Result_Model<IWantToSupply> model = iWantToBuyService.GetObjectById_Web_Supply(iWantToBuy, supplierId);
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
            return res;
        }
        #endregion

        #endregion

        #region 默认加载

        #region 所有采购商：我要采购列表
        /// <summary>
        /// 所有采购商：我要采购列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string IWantToSupplyList_Web_Default(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<Result_IWantToBuy> res = new Result_List_Pager<Result_IWantToBuy>();
            Result_List_Pager<IWantToBuy> resList = iWantToBuyService.Get_IWantToBuyList_Web_Supply(query);
            DateTime Now = DateTime.Now;
            String NowTime = Now.ToString("yyyy-MM-dd hh:mm");
            
            if (resList.Msg.IsSuccess)
            {
                var listHash = hashSet.Get_DictionariesList();
                res.NowTime = NowTime;
                res.PageInfo = resList.PageInfo;
                res.Msg = resList.Msg;
                res.List = resList.List.Select(x => new Result_IWantToBuy()
                {
                    Id = x.Id,
                    //SupplyModel = GetObjectById_Web_Supply(x.Id, base.CurrentUser.Id),

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
            }
            else
            {
                res.Msg = resList.Msg;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion
        #region 获取分页信息

        public string Get_PageInfo_Default_Web(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            resModel = iWantToBuyService.Get_PageInfo_Web_Supply(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion

        #endregion

        #region 公共

        #region 生成采购编号：时间+随机数
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
        #endregion
        #region 生成随机数
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <returns></returns>
        public int GetRandomNum()
        {
            Random r = new Random();
            return r.Next(100, 999);
        }
        #endregion
        #region 获取登录用户信息
        /// <summary>
        /// 获取登录用户信息
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
        #endregion

        #endregion

        #endregion

    }
}