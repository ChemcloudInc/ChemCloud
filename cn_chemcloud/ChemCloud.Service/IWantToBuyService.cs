using ChemCloud.Model.Common;
using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class IWantToBuyService : ServiceBase, IIWantToBuyService, IService, IDisposable
    {
        HashSet_Common hashSet = new HashSet_Common();

        #region Admin 后台

        #region 获取招聘信息列表
        /// <summary>
        /// 获取招聘信息列表——Admin
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<IWantToBuy> GetIWantToBuyList_Admin(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<IWantToBuy> res = new Result_List_Pager<IWantToBuy>()
            {
                List = new List<IWantToBuy>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy select iWantToBuy;
                #region 拼接 Linq 查询条件
                //if (query.ParamInfo.UserType != 0)
                //{
                //    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                //}
                #endregion

                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }
        #endregion

        #region 快速审核
        /// <summary>
        /// 快速审核
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg UpdateSupply_Fast(IWantToBuyModifyQuery query)
        {
            Result_Msg res = new Result_Msg();

            IWantToBuy jobOld = context.IWantToBuy.FindById<IWantToBuy>(query.Id);
            if (jobOld != null)
            {
                try
                {
                    res.IsSuccess = context.SaveChanges() == 1;
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion

        #region 根据Id获取详情
        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result_IWantToBuy GetObjectById_Admin(int Id)
        {
            var listHash = hashSet.Get_DictionariesList();

            IWantToBuy job = (from p in context.IWantToBuy where p.Id == Id select p).FirstOrDefault();
            Result_IWantToBuy res = new Result_IWantToBuy()
            {
                Id = job.Id,


                //CreateDate = job.CreateDate.ToString(),
                //UpdateDate = job.UpdateDate.ToString(),
                //StartDate = job.StartDate.ToString(),
                //EndDate = job.EndDate.ToString(),

                //UserType = listHash.Where(y => y.DictionaryTypeId == 15 && y.DKey == job.UserType.ToString()).FirstOrDefault().DValue,
                //ApprovalStatus = listHash.Where(y => y.DictionaryTypeId == 14 && y.DKey == job.ApprovalStatus.ToString()).FirstOrDefault().DValue,

                //TypeOfCurrency = job.TypeOfCurrency.ToString(),

                //PayrollType = listHash.Where(y => y.DictionaryTypeId == 18 && y.DKey == job.PayrollType.ToString()).FirstOrDefault().DValue,
                //TypeOfCurrency = listHash.Where(y => y.DictionaryTypeId == 1 && y.DValue == job.TypeOfCurrency.ToString()).FirstOrDefault().Remarks,
                //WorkType = listHash.Where(y => y.DictionaryTypeId == 17 && y.DKey == job.WorkType.ToString()).FirstOrDefault().DValue,

            };

            return res;
        }
        #endregion

        #region 删除记录——admin

        /// <summary>
        /// 删除记录——admin
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Result_Msg DeleteById_Admin(int Id)
        {
            Result_Msg res = new Result_Msg();
            IWantToBuy job = (from p in context.IWantToBuy where p.Id == Id select p).FirstOrDefault();
            context.IWantToBuy.Remove(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg UpdateSupply_Admin(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Msg res = new Result_Msg();

            IWantToBuy jobOld = context.IWantToBuy.FindById<IWantToBuy>(query.ParamInfo.Id);
            if (jobOld != null)
            {
                jobOld.Id = query.ParamInfo.Id;
                jobOld.CreateDate = jobOld.CreateDate;
                jobOld.UpdateDate = query.ParamInfo.UpdateDate;
                jobOld.StartDate = query.ParamInfo.StartDate;

                jobOld.EndDate = query.ParamInfo.EndDate;
                jobOld.TypeOfCurrency = query.ParamInfo.TypeOfCurrency;
                try
                {
                    res.IsSuccess = context.SaveChanges() == 1;
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion

        #region 分页信息
        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_Admin(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy select iWantToBuy;

                #region 拼接 Linq 查询条件
                //if (query.ParamInfo.UserType != 0)
                //{
                //    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                //}
                #endregion

                int total = linqList.Count();

                resPage.Model.CurrentPage = query.PageInfo.CurrentPage;
                resPage.Model.PageSize = query.PageInfo.PageSize;
                resPage.Model.Total = total;
                resPage.Model.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                resPage.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resPage;
        }
        #endregion


        #endregion

        #region Web 后台

        #region 采购商

        #region 我要采购 列表

        /// <summary>
        /// 我要采购 列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public Result_List_Pager<IWantToBuy> IWantToBuyList_User(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<IWantToBuy> res = new Result_List_Pager<IWantToBuy>()
            {
                List = new List<IWantToBuy>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy where iWantToBuy.PurchaseID == query.ParamInfo.PurchaseID select iWantToBuy;

                #region 拼接 Linq 查询条件
                if (!string.IsNullOrEmpty(query.ParamInfo.ProductName))
                {
                    linqList = linqList.Where(x => x.ProductName.IndexOf(query.ParamInfo.ProductName) != -1);
                }

                if (query.ParamInfo.CreateDate1 != null && query.ParamInfo.CreateDate2 != null)
                {
                    linqList = linqList.Where(x => x.CreateDate >= query.ParamInfo.CreateDate1 && x.CreateDate <= query.ParamInfo.CreateDate2);
                }
                else if (query.ParamInfo.CreateDate1 != null && query.ParamInfo.CreateDate2 == null)
                {
                    linqList = linqList.Where(x => x.CreateDate >= query.ParamInfo.CreateDate1);
                }
                else if (query.ParamInfo.CreateDate1 == null && query.ParamInfo.CreateDate2 != null)
                {
                    linqList = linqList.Where(x => x.CreateDate <= query.ParamInfo.CreateDate2);
                }

                if (query.ParamInfo.DeliveryDate1 != null && query.ParamInfo.DeliveryDate2 != null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate >= query.ParamInfo.DeliveryDate1 && x.DeliveryDate <= query.ParamInfo.DeliveryDate2);
                }
                else if (query.ParamInfo.DeliveryDate1 != null && query.ParamInfo.DeliveryDate2 == null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate >= query.ParamInfo.DeliveryDate1);
                }
                else if (query.ParamInfo.DeliveryDate1 == null && query.ParamInfo.DeliveryDate2 != null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate <= query.ParamInfo.DeliveryDate2);
                }
                #endregion

                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }
        #endregion
        #region 我要采购 分页
        /// <summary>
        /// 我要采购 分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_User(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy where iWantToBuy.PurchaseID == query.ParamInfo.PurchaseID select iWantToBuy;

                #region 拼接 Linq 查询条件
                if (!string.IsNullOrEmpty(query.ParamInfo.ProductName))
                {
                    linqList = linqList.Where(x => x.ProductName.IndexOf(query.ParamInfo.ProductName) != -1);
                }

                if (query.ParamInfo.CreateDate1 != null && query.ParamInfo.CreateDate2 != null)
                {
                    query.ParamInfo.CreateDate2 = Convert.ToDateTime(query.ParamInfo.CreateDate2).AddDays(1);
                    linqList = linqList.Where(x => x.CreateDate >= query.ParamInfo.CreateDate1 && x.CreateDate < query.ParamInfo.CreateDate2);
                }
                else if (query.ParamInfo.CreateDate1 != null && query.ParamInfo.CreateDate2 == null)
                {
                    linqList = linqList.Where(x => x.CreateDate >= query.ParamInfo.CreateDate1);
                }
                else if (query.ParamInfo.CreateDate1 == null && query.ParamInfo.CreateDate2 != null)
                {
                    query.ParamInfo.CreateDate2 = Convert.ToDateTime(query.ParamInfo.CreateDate2).AddDays(1);
                    linqList = linqList.Where(x => x.CreateDate < query.ParamInfo.CreateDate2);
                }

                if (query.ParamInfo.DeliveryDate1 != null && query.ParamInfo.DeliveryDate2 != null)
                {
                    query.ParamInfo.DeliveryDate2 = Convert.ToDateTime(query.ParamInfo.DeliveryDate2).AddDays(1);
                    linqList = linqList.Where(x => x.DeliveryDate >= query.ParamInfo.DeliveryDate1 && x.DeliveryDate <= query.ParamInfo.DeliveryDate2);
                }
                else if (query.ParamInfo.DeliveryDate1 != null && query.ParamInfo.DeliveryDate2 == null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate >= query.ParamInfo.DeliveryDate1);
                }
                else if (query.ParamInfo.DeliveryDate1 == null && query.ParamInfo.DeliveryDate2 != null)
                {
                    query.ParamInfo.DeliveryDate2 = Convert.ToDateTime(query.ParamInfo.DeliveryDate2).AddDays(1);
                    linqList = linqList.Where(x => x.DeliveryDate < query.ParamInfo.DeliveryDate2);
                }
                #endregion

                int total = linqList.Count();

                resPage.Model.CurrentPage = query.PageInfo.CurrentPage;
                resPage.Model.PageSize = query.PageInfo.PageSize;
                resPage.Model.Total = total;
                resPage.Model.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                resPage.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resPage;
        }
        #endregion
        #region 新增：我要竞价
        /// <summary>
        /// 新增：我要竞价
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg AddIWantToSupply(QueryCommon<IWantToSupplyQuery> query)
        {
            Result_Msg res = new Result_Msg();

            try
            {
                IWantToSupply iWantToBuy = new IWantToSupply()
                {
                    PurchaseNum = query.ParamInfo.PurchaseNum,
                    IWantToBuyID = query.ParamInfo.IWantToBuyID,
                    SupplierID = query.ParamInfo.SupplierID,
                    UnitPrice = query.ParamInfo.UnitPrice,
                    Quantity = query.ParamInfo.Quantity,
                    Unit = query.ParamInfo.Unit,
                    TotalPrice = query.ParamInfo.TotalPrice,

                    UpdateDate = query.ParamInfo.UpdateDate,
                    CreateDate = query.ParamInfo.CreateDate,
                    BidDate = query.ParamInfo.BidDate,
                    Status = query.ParamInfo.Status,
                    LatestDeliveryTime = query.ParamInfo.LatestDeliveryTime,
                    TypeOfCurrency = query.ParamInfo.TypeOfCurrency
                };

                context.IWantToSupply.Add(iWantToBuy);
                res.IsSuccess = context.SaveChanges() == 1;
                res.Message = "新增成功";
            }
            catch (Exception ex)
            {
                res = new Result_Msg() { IsSuccess = false, Message = "新增失败" };
            }

            return res;
        }
        #endregion
        #region 删除指定ID数据
        /// <summary>
        /// 删除指定ID数据
        /// </summary>
        /// <param name="Id">招聘信息ID</param>
        /// <returns></returns>
        public Result_Msg DeleteById(int Id, long userId)
        {
            Result_Msg res = new Result_Msg();
            IWantToBuy job = (from p in context.IWantToBuy where p.Id == Id && p.PurchaseID == userId select p).FirstOrDefault();
            context.IWantToBuy.Remove(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }
        #endregion
        #region 获取招聘信息详情
        /// <summary>
        /// 获取招聘信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IWantToBuy GetObjectById(int Id, long userId)
        {
            IWantToBuy job = (from p in context.IWantToBuy where p.Id == Id && p.PurchaseID == userId select p).FirstOrDefault();
            return job;
        }
        #endregion
        #region 分页信息：我要采购对用竞价列表
        /// <summary>
        /// 分页信息：我要采购对用竞价列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_IWantToSupply_List(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from iWantToBuy in context.IWantToSupply where iWantToBuy.IWantToBuyID == query.ParamInfo.Id select iWantToBuy;

                int total = linqList.Count();

                resPage.Model.CurrentPage = query.PageInfo.CurrentPage;
                resPage.Model.PageSize = query.PageInfo.PageSize;
                resPage.Model.Total = total;
                resPage.Model.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                resPage.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resPage;
        }
        #endregion

        #region 根据ID，获取Supply详情
        /// <summary>
        /// 根据ID，获取Supply详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result_Model<IWantToSupply> GetObjectById_Supply(long id)
        {
            Result_Model<IWantToSupply> res = new Result_Model<IWantToSupply>()
            {
                Model = new IWantToSupply(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                IWantToSupply job = (from p in context.IWantToSupply where p.Id == id select p).FirstOrDefault();
                if (job == null)
                {
                    res.Msg = new Result_Msg() { IsSuccess = false, Message = "查询记录失败，记录不存在" };
                }
                else
                {
                    res.Model = job;
                }
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }

            return res;
        }
        #endregion
        #region 根据purchaseNum，获取order详情
        /// <summary>
        /// 根据purchaseNum，获取order详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result_Model<IWantToBuy_Orders> GetOrders_ByPurchaseNum(long purchaseNum)
        {
            Result_Model<IWantToBuy_Orders> res = new Result_Model<IWantToBuy_Orders>()
            {
                Model = new IWantToBuy_Orders(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                IWantToBuy_Orders job = (from p in context.IWantToBuy_Orders where p.PurchaseNum == purchaseNum select p).FirstOrDefault();
                if (job == null)
                {
                    res.Msg = new Result_Msg() { IsSuccess = false, Message = "查询记录失败，记录不存在" };
                }
                else
                {
                    res.Model = job;
                }
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }

            return res;
        }
        #endregion

        #region 生成订单
        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<IWantToBuy_Orders> AddIWantToBuyOrder(IWantToSupply query)
        {
            Result_Model<IWantToBuy_Orders> res = new Result_Model<IWantToBuy_Orders>()
            {
                Msg = new Result_Msg() { IsSuccess = false },
                Model = new IWantToBuy_Orders()
            };
            DateTime dtNow = DateTime.Now;
            try
            {
                IWantToBuy_Orders iWantToBuyOrder = new IWantToBuy_Orders()
                {
                    //Id = query.PurchaseNum,
                    PayDate = dtNow,
                    Status = 4,
                    IWantToSupplyID = query.Id,
                    //ProductName = query.PurchaseNum,
                    PurchaseNum = query.PurchaseNum,
                    UnitPrice = query.UnitPrice,
                    Quantity = query.Quantity,
                    Unit = query.Unit,
                    TotalPrice = query.TotalPrice,
                    CreateDate = dtNow,
                    LogisticsNum = string.Empty,
                    LogisticsType = 0,
                    ProductName = string.Empty
                };

                context.IWantToBuy_Orders.Add(iWantToBuyOrder);

                if (context.SaveChanges() == 1)
                {
                    res.Msg.IsSuccess = true;
                    res.Msg.Message = "订单生成成功";
                    res.Model = iWantToBuyOrder;

                    //更改IWantToBuy数据状态
                    QueryCommon<IWantToBuyQuery> buy = new QueryCommon<IWantToBuyQuery>()
                    {
                        ParamInfo = new IWantToBuyQuery()
                        {
                            Id = query.IWantToBuyID,
                            UpdateDate = dtNow,
                            Status = 4
                        }
                    };
                    UpdateBuy_Status(buy);

                    //更改IWantToSupply数据状态
                    QueryCommon<IWantToSupplyQuery> supply = new QueryCommon<IWantToSupplyQuery>()
                    {
                        ParamInfo = new IWantToSupplyQuery()
                        {
                            Id = query.Id,
                            UpdateDate = dtNow,
                            Status = 4
                        }
                    };
                    UpdateSupply_Status(supply);
                }
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "订单生成失败";
            }

            return res;
        }
        #endregion

        #endregion

        #region 供应商

        #region 供应商：获取所有采购商采购信息列表
        /// <summary>
        /// 供应商：获取所有采购商采购信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<IWantToBuy> GetIWantToBuyList_SupplyUser_Pager(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<IWantToBuy> res = new Result_List_Pager<IWantToBuy>()
            {
                List = new List<IWantToBuy>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy select iWantToBuy;

                #region 拼接 Linq 查询条件
                if (!string.IsNullOrEmpty(query.ParamInfo.ProductName))
                {
                    linqList = linqList.Where(x => x.ProductName.IndexOf(query.ParamInfo.ProductName) != -1);
                }

                if (query.ParamInfo.CreateDate1 != null && query.ParamInfo.CreateDate2 != null)
                {
                    linqList = linqList.Where(x => x.CreateDate >= query.ParamInfo.CreateDate1 && x.CreateDate <= query.ParamInfo.CreateDate2);
                }
                else if (query.ParamInfo.CreateDate1 != null && query.ParamInfo.CreateDate2 == null)
                {
                    linqList = linqList.Where(x => x.CreateDate >= query.ParamInfo.CreateDate1);
                }
                else if (query.ParamInfo.CreateDate1 == null && query.ParamInfo.CreateDate2 != null)
                {
                    linqList = linqList.Where(x => x.CreateDate <= query.ParamInfo.CreateDate2);
                }

                if (query.ParamInfo.DeliveryDate1 != null && query.ParamInfo.DeliveryDate2 != null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate >= query.ParamInfo.DeliveryDate1 && x.DeliveryDate <= query.ParamInfo.DeliveryDate2);
                }
                else if (query.ParamInfo.DeliveryDate1 != null && query.ParamInfo.DeliveryDate2 == null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate >= query.ParamInfo.DeliveryDate1);
                }
                else if (query.ParamInfo.DeliveryDate1 == null && query.ParamInfo.DeliveryDate2 != null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate <= query.ParamInfo.DeliveryDate2);
                }
                #endregion


                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }
        #endregion
        #region 供应商：获取所有采购商采购信息（分页）
        /// <summary>
        /// 供应商：获取所有采购商采购信息（分页）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_IWantToBuyList_SupplyUser(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy select iWantToBuy;

                #region 拼接 Linq 查询条件
                if (!string.IsNullOrEmpty(query.ParamInfo.ProductName))
                {
                    linqList = linqList.Where(x => x.ProductName.IndexOf(query.ParamInfo.ProductName) != -1);
                }

                if (query.ParamInfo.CreateDate1 != null && query.ParamInfo.CreateDate2 != null)
                {
                    linqList = linqList.Where(x => x.CreateDate >= query.ParamInfo.CreateDate1 && x.CreateDate <= query.ParamInfo.CreateDate2);
                }
                else if (query.ParamInfo.CreateDate1 != null && query.ParamInfo.CreateDate2 == null)
                {
                    linqList = linqList.Where(x => x.CreateDate >= query.ParamInfo.CreateDate1);
                }
                else if (query.ParamInfo.CreateDate1 == null && query.ParamInfo.CreateDate2 != null)
                {
                    linqList = linqList.Where(x => x.CreateDate <= query.ParamInfo.CreateDate2);
                }

                if (query.ParamInfo.DeliveryDate1 != null && query.ParamInfo.DeliveryDate2 != null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate >= query.ParamInfo.DeliveryDate1 && x.DeliveryDate <= query.ParamInfo.DeliveryDate2);
                }
                else if (query.ParamInfo.DeliveryDate1 != null && query.ParamInfo.DeliveryDate2 == null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate >= query.ParamInfo.DeliveryDate1);
                }
                else if (query.ParamInfo.DeliveryDate1 == null && query.ParamInfo.DeliveryDate2 != null)
                {
                    linqList = linqList.Where(x => x.DeliveryDate <= query.ParamInfo.DeliveryDate2);
                }
                #endregion


                int total = linqList.Count();

                resPage.Model.CurrentPage = query.PageInfo.CurrentPage;
                resPage.Model.PageSize = query.PageInfo.PageSize;
                resPage.Model.Total = total;
                resPage.Model.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                resPage.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resPage;
        }
        #endregion
        #region 供应商：获取中标供应商ID
        /// <summary>
        /// 供应商：获取中标供应商ID
        /// </summary>
        /// <returns></returns>
        public Result_Model<IWantToSupply> Get_SupplyModel_ByIWantToBuyID(long iWantToBuyId)
        {
            Result_Model<IWantToSupply> res = new Result_Model<IWantToSupply>()
            {
                Msg = new Result_Msg { IsSuccess = true },
                Model = new IWantToSupply()
            };
            var linqModel = (from iWantToSupply in context.IWantToSupply where iWantToSupply.IWantToBuyID == iWantToBuyId && (iWantToSupply.Status == 1 || iWantToSupply.Status >= 4) select iWantToSupply).FirstOrDefault();
            if (linqModel != null)
            {
                res.Model = linqModel;
            }
            else
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查找中标记录失败";
            }
            return res;
        }
        #endregion
        #region 供应商：获取中标供应商ID
        public Result_Model<IWantToSupply> Get_SupplyModel_ByUserIdAndIWantToBuyId(long iWantToBuyId, long userId)
        {
            Result_Model<IWantToSupply> res = new Result_Model<IWantToSupply>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                Model = new IWantToSupply(),
            };

            //判断当前登录供应商，参与竞价的竞价
            res.Model = (from iWantToSupply in context.IWantToSupply where iWantToSupply.IWantToBuyID == iWantToBuyId && iWantToSupply.SupplierID == userId select iWantToSupply).FirstOrDefault();
            if (res.Model == null)
            {
                res.Msg = new Result_Msg()
                {
                    IsSuccess = false,
                    Message = "后去当前供应商报价信息失败"
                };
            }
            return res;
        }
        #endregion
        #region 查询我参与的“我要采购列表”
        /// <summary>
        /// 查询我参与的“我要采购列表”
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<IWantToSupply> SupplyList_Pager(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<IWantToSupply> res = new Result_List_Pager<IWantToSupply>()
            {
                List = new List<IWantToSupply>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            int total = 0;
            try
            {
                var linqList = from iWantToSupply in context.IWantToSupply select iWantToSupply;

                #region 拼接 Linq 查询条件

                if (query.ParamInfo.AllOrMin == 1)
                {
                    linqList = linqList.Where(x => x.SupplierID == query.ParamInfo.PurchaseID);
                }

                #endregion

                total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }
        #endregion
        #region 修改物流信息（发货）
        /// <summary>
        /// 修改物流信息（发货）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg UpdateOrders(QueryCommon<IWantToBuy_Orders> query)
        {
            Result_Msg res = new Result_Msg();

            IWantToBuy_Orders jobOrder = context.IWantToBuy_Orders.FindById<IWantToBuy_Orders>(query.ParamInfo.Id);

            if (jobOrder != null)
            {
                jobOrder.Status = query.ParamInfo.Status;
                jobOrder.LogisticsNum = query.ParamInfo.LogisticsNum;
                jobOrder.LogisticsType = query.ParamInfo.LogisticsType;
                jobOrder.LogisticsDes = query.ParamInfo.LogisticsDes;

                IWantToSupply jobSupply = context.IWantToSupply.Where<IWantToSupply>(x => x.Id == jobOrder.IWantToSupplyID).FirstOrDefault();
                IWantToBuy jobBuy = context.IWantToBuy.Where<IWantToBuy>(x => x.Id == jobSupply.IWantToBuyID).FirstOrDefault();

                jobSupply.Status = query.ParamInfo.Status;
                jobBuy.Status = query.ParamInfo.Status;

                try
                {
                    res.IsSuccess = context.SaveChanges() > 0;
                    res.Message = "修改成功";
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion

        #endregion

        #endregion

        #region Web 前台展示

        #region 采购商

        #region 我要采购：列表（当前登录采购商）
        /// <summary>
        /// 我要采购：列表（当前登录采购商）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<IWantToBuy> Get_IWantToBuyList_Web_Buy(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<IWantToBuy> res = new Result_List_Pager<IWantToBuy>()
            {
                List = new List<IWantToBuy>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy where iWantToBuy.PurchaseID == query.ParamInfo.PurchaseID select iWantToBuy;

                #region 拼接 Linq 查询条件
                //if (query.ParamInfo.UserType != 0)
                //{
                //    //发布者类型（1：平台管理员？；2：供应商；3：采购商；）
                //    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                //}
                #endregion

                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }
        #endregion
        #region 我要采购：分页（当前登录采购商）
        /// <summary>
        /// 我要采购：分页（当前登录采购商）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_Web_Buy(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy where iWantToBuy.PurchaseID == query.ParamInfo.Id select iWantToBuy;

                #region 拼接 Linq 查询条件
                //if (query.ParamInfo.UserType != 0)
                //{
                //    //发布者类型（1：平台管理员？；2：供应商；3：采购商；）
                //    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                //}
                #endregion

                int total = linqList.Count();

                resPage.Model.CurrentPage = query.PageInfo.CurrentPage;
                resPage.Model.PageSize = query.PageInfo.PageSize;
                resPage.Model.Total = total;
                resPage.Model.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                resPage.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resPage;
        }
        #endregion
        #region 我要采购：新增
        /// <summary>
        /// 我要采购：新增
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg IWantToBuy_Add_Web(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Msg res = new Result_Msg();
            IWantToBuy iWantToBuy = new IWantToBuy()
            {
                ProductName = query.ParamInfo.ProductName,
                UnitPrice = query.ParamInfo.UnitPrice,
                Quantity = query.ParamInfo.Quantity,
                Unit = query.ParamInfo.Unit,
                TotalPrice = query.ParamInfo.TotalPrice,
                PurchaseID = query.ParamInfo.PurchaseID,
                Remarks = query.ParamInfo.Remarks,
                Status = query.ParamInfo.Status,
                StartDate = query.ParamInfo.StartDate,
                UpdateDate = query.ParamInfo.UpdateDate,
                Address = query.ParamInfo.Address,
                CreateDate = query.ParamInfo.CreateDate,
                DeliveryDate = query.ParamInfo.DeliveryDate,
                EndDate = query.ParamInfo.EndDate,
                PurchaseNum = query.ParamInfo.PurchaseNum,
                TypeOfCurrency = query.ParamInfo.TypeOfCurrency
            };

            context.IWantToBuy.Add(iWantToBuy);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }
        #endregion

        #region 修改 Supply Status
        /// <summary>
        /// 修改 Supply Status
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg UpdateSupply_Status(QueryCommon<IWantToSupplyQuery> query)
        {
            Result_Msg res = new Result_Msg();

            IWantToSupply jobOld = context.IWantToSupply.FindById<IWantToSupply>(query.ParamInfo.Id);
            if (jobOld != null)
            {
                try
                {
                    jobOld.Status = query.ParamInfo.Status;
                    jobOld.UpdateDate = query.ParamInfo.UpdateDate;
                    if (jobOld.Status == 1)
                    {
                        jobOld.BidDate = query.ParamInfo.BidDate;
                    }
                    res.IsSuccess = context.SaveChanges() == 1;
                    res.Message = "修改成功";
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion
        #region 修改 Buy Status
        /// <summary>
        /// 修改 Buy Status
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg UpdateBuy_Status(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Msg res = new Result_Msg();

            IWantToBuy jobOld = context.IWantToBuy.FindById<IWantToBuy>(query.ParamInfo.Id);
            if (jobOld != null)
            {
                jobOld.Status = query.ParamInfo.Status;
                jobOld.UpdateDate = query.ParamInfo.UpdateDate;

                try
                {
                    res.IsSuccess = context.SaveChanges() == 1;
                    res.Message = "修改成功";
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion
        #region 修改 Buy
        /// <summary>
        /// 修改 Buy
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg IWantToBuy_Update(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Msg res = new Result_Msg();

            IWantToBuy jobOld = context.IWantToBuy.FindById<IWantToBuy>(query.ParamInfo.Id);
            if (jobOld != null)
            {
                jobOld.Status = query.ParamInfo.Status;
                jobOld.UpdateDate = query.ParamInfo.UpdateDate;

                jobOld.Address = query.ParamInfo.Address;
                jobOld.DeliveryDate = query.ParamInfo.DeliveryDate;
                jobOld.ProductName = query.ParamInfo.ProductName;
                jobOld.Quantity = query.ParamInfo.Quantity;
                jobOld.Remarks = query.ParamInfo.Remarks;
                jobOld.UnitPrice = query.ParamInfo.UnitPrice;
                jobOld.Unit = query.ParamInfo.Unit;
                jobOld.TotalPrice = query.ParamInfo.TotalPrice;

                try
                {
                    res.IsSuccess = context.SaveChanges() == 1;
                    res.Message = "修改成功";
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion
        #region 修改 Orders Status
        /// <summary>
        /// 修改 Orders Status
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg UpdateOrders_Status(QueryCommon<IWantToBuy_Orders> query)
        {
            Result_Msg res = new Result_Msg();

            IWantToBuy_Orders jobOld = context.IWantToBuy_Orders.Where<IWantToBuy_Orders>(x => x.IWantToSupplyID == query.ParamInfo.IWantToSupplyID).FirstOrDefault();
            if (jobOld != null)
            {
                jobOld.Status = query.ParamInfo.Status;

                try
                {
                    res.IsSuccess = context.SaveChanges() == 1;
                    res.Message = "修改成功";
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion


        #region 根据 purchaseNum，获取status最大的数据
        /// <summary>
        /// 根据 purchaseNum，获取status最大的数据
        /// </summary>
        /// <param name="purchaseNum"></param>
        /// <returns></returns>
        public Result_Model<IWantToSupply> MaxStatusSupply_By_Num(long purchaseNum)
        {
            Result_Model<IWantToSupply> res = new Result_Model<IWantToSupply>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                Model = new IWantToSupply()
            };
            res.Model = context.IWantToSupply.Where(x => x.Status >= 4 && x.PurchaseNum == purchaseNum).FirstOrDefault();
            if (res.Model != null)
            {
                res.Msg.IsSuccess = true;
            }
            else
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "数据不存在";
            }
            return res;
        }
        #endregion

        #endregion

        #region 供应商

        #region 供应商：获取所有采购商采购信息列表
        /// <summary>
        /// 供应商：获取所有采购商采购信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<IWantToBuy> Get_IWantToBuyList_Web_Supply(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<IWantToBuy> res = new Result_List_Pager<IWantToBuy>()
            {
                List = new List<IWantToBuy>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy select iWantToBuy;

                #region 拼接 Linq 查询条件
                //if (query.ParamInfo.UserType != 0)
                //{
                //    //发布者类型（1：平台管理员？；2：供应商；3：采购商；）
                //    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                //}
                #endregion

                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }
        #endregion
        #region 供应商：获取所有采购商采购信息（分页）
        /// <summary>
        /// 供应商：获取所有采购商采购信息（分页）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_Web_Supply(QueryCommon<IWantToBuyQuery> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from iWantToBuy in context.IWantToBuy select iWantToBuy;

                #region 拼接 Linq 查询条件
                //if (query.ParamInfo.UserType != 0)
                //{
                //    //发布者类型（1：平台管理员？；2：供应商；3：采购商；）
                //    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                //}
                #endregion

                int total = linqList.Count();

                resPage.Model.CurrentPage = query.PageInfo.CurrentPage;
                resPage.Model.PageSize = query.PageInfo.PageSize;
                resPage.Model.Total = total;
                resPage.Model.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                resPage.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resPage;
        }
        #endregion
        #region 修改报价信息
        /// <summary>
        /// 修改报价信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg UpdateSupply(QueryCommon<IWantToSupplyQuery> query)
        {
            Result_Msg res = new Result_Msg();

            IWantToSupply jobOld = context.IWantToSupply.FindById<IWantToSupply>(query.ParamInfo.Id);
            if (jobOld != null)
            {
                //jobOld.Id = query.ParamInfo.Id;
                jobOld.Status = query.ParamInfo.Status;
                //jobOld.SupplierID = query.ParamInfo.SupplierID;
                //jobOld.PurchaseNum = query.ParamInfo.PurchaseNum;

                jobOld.UnitPrice = query.ParamInfo.UnitPrice;
                jobOld.Quantity = query.ParamInfo.Quantity;
                jobOld.TotalPrice = query.ParamInfo.TotalPrice;
                jobOld.Unit = query.ParamInfo.Unit;

                //jobOld.CreateDate = jobOld.CreateDate;
                jobOld.UpdateDate = query.ParamInfo.UpdateDate;
                //jobOld.BidDate = query.ParamInfo.BidDate;
                jobOld.LatestDeliveryTime = query.ParamInfo.LatestDeliveryTime;

                jobOld.TypeOfCurrency = query.ParamInfo.TypeOfCurrency;
                try
                {
                    res.IsSuccess = context.SaveChanges() == 1;
                    res.Message = "修改成功";
                }
                catch (Exception ex)
                {
                    res.Message = "修改失败：" + ex.Message;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改失败，数据库中不存在该记录";
            }
            return res;
        }
        #endregion

        #endregion

        #region 获取招聘信息详情
        /// <summary>
        /// 获取招聘信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result_Model<IWantToBuy> GetObjectById_Web(long Id)
        {
            Result_Model<IWantToBuy> res = new Result_Model<IWantToBuy>()
            {
                Model = new IWantToBuy(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                IWantToBuy job = (from p in context.IWantToBuy where p.Id == Id select p).FirstOrDefault();
                if (job == null)
                {
                    res.Msg = new Result_Msg() { IsSuccess = false, Message = "查询记录失败，记录不存在" };
                }
                else
                {
                    res.Model = job;
                }

            }
            catch (Exception ex)
            {

                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }

            return res;
        }
        public Result_Model<IWantToBuy> GetObjectById_Web_Buy(long Id)
        {
            Result_Model<IWantToBuy> res = new Result_Model<IWantToBuy>()
            {
                Model = new IWantToBuy(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                IWantToBuy job = (from p in context.IWantToBuy where p.Id == Id select p).FirstOrDefault();
                if (job == null)
                {
                    res.Msg = new Result_Msg() { IsSuccess = false, Message = "查询记录失败，记录不存在" };
                }
                else
                {
                    res.Model = job;
                }

            }
            catch (Exception ex)
            {

                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }

            return res;
        }

        #region 根据 iWantToBuy ，supplierId 获取竞价详情
        /// <summary>
        /// 根据 iWantToBuy ，supplierId 获取竞价详情
        /// </summary>
        /// <param name="iWantToBuy"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public Result_Model<IWantToSupply> GetObjectById_Web_Supply(long iWantToBuy, long supplierId)
        {
            Result_Model<IWantToSupply> res = new Result_Model<IWantToSupply>()
            {
                Model = new IWantToSupply(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                IWantToSupply job = (from p in context.IWantToSupply where p.IWantToBuyID == iWantToBuy && p.SupplierID == supplierId select p).FirstOrDefault();
                if (job == null)
                {
                    res.Msg = new Result_Msg() { IsSuccess = false, Message = "查询记录失败，记录不存在" };
                }
                else
                {
                    res.Model = job;
                }
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }

            return res;
        }
        #endregion
        #region 根据 id ，supplierId 获取竞价详情
        /// <summary>
        /// 根据 id ，supplierId 获取竞价详情
        /// </summary>
        /// <param name="iWantToBuy"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public Result_Model<IWantToBuy_Orders> GetObjectById_Web_Supply_DeliverGoods(long supplyId)
        {
            Result_Model<IWantToBuy_Orders> res = new Result_Model<IWantToBuy_Orders>()
            {
                Model = new IWantToBuy_Orders(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                IWantToBuy_Orders job = (from p in context.IWantToBuy_Orders where p.IWantToSupplyID == supplyId select p).FirstOrDefault();
                if (job == null)
                {
                    res.Msg = new Result_Msg() { IsSuccess = false, Message = "查询记录失败，未登录或记录不存在" };
                }
                else
                {
                    res.Model = job;
                }
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }

            return res;
        }
        #endregion

        #region 根据 iWantToBuy 获取竞价列表
        /// <summary>
        /// 根据 iWantToBuy 获取竞价列表
        /// </summary>
        /// <param name="iWantToBuyId"></param>
        /// <returns></returns>
        public Result_List<IWantToSupply> GetObjectById_Web_SupplyList(long iWantToBuyId)
        {
            Result_List<IWantToSupply> res = new Result_List<IWantToSupply>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                List = new List<IWantToSupply>()
            };
            try
            {
                List<IWantToSupply> list = (from p in context.IWantToSupply where p.IWantToBuyID == iWantToBuyId select p).ToList();
                if (list == null)
                {
                    res.Msg = new Result_Msg() { IsSuccess = false, Message = "查询记录失败，记录不存在" };
                }
                else
                {
                    res.List = list;
                }

            }
            catch (Exception ex)
            {

                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }

            return res;
        }
        #endregion

        public Result_List_Pager<IWantToSupply> GetObjectById_Web_SupplyList_Pager(QueryCommon<IWantToBuyQuery> query)
        {
            Result_List_Pager<IWantToSupply> res = new Result_List_Pager<IWantToSupply>()
            {
                List = new List<IWantToSupply>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = from iWantToSupply in context.IWantToSupply where iWantToSupply.IWantToBuyID == query.ParamInfo.Id select iWantToSupply;

                #region 拼接 Linq 查询条件
                //if (query.ParamInfo.UserType != 0)
                //{
                //    //发布者类型（1：平台管理员？；2：供应商；3：采购商；）
                //    linqList = linqList.Where(x => x.UserType == query.ParamInfo.UserType);
                //}
                #endregion

                int total = linqList.Count();
                res.List = linqList.OrderByDescending(p => p.CreateDate).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }

        #endregion
        #region 前后两条记录
        /// <summary>
        /// 前后两条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result_List<Result_Model<IWantToBuy>> Get_PreNext_ById_Web(long id)
        {
            Result_List<Result_Model<IWantToBuy>> res = new Result_List<Result_Model<IWantToBuy>>()
            {
                List = new List<Result_Model<IWantToBuy>>(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = (from listMeet in context.IWantToBuy select listMeet).OrderBy(p => p.EndDate).OrderByDescending(p => p.Id).ToList();
                int indexNo = linqList.IndexOf(linqList.First(x => x.Id == id));

                if (indexNo > 0)
                {
                    IWantToBuy pre = linqList.Skip(indexNo - 1).Take(1).First();
                    if (pre != null)
                    {
                        res.List.Add(new Result_Model<IWantToBuy>()
                        {
                            Model = pre,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<IWantToBuy>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取上一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<IWantToBuy>()
                    {
                        Msg = new Result_Msg() { IsSuccess = false, Message = "上一条没有了" }
                    });
                }

                if (indexNo < (linqList.Count - 1))
                {
                    IWantToBuy next = linqList.Skip(indexNo + 1).Take(1).First();

                    if (next != null)
                    {
                        res.List.Add(new Result_Model<IWantToBuy>()
                        {
                            Model = next,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<IWantToBuy>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取下一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<IWantToBuy>()
                    {
                        Msg = new Result_Msg() { IsSuccess = false, Message = "下一条没有了" }
                    });
                }

            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = ex.Message;
            }
            return res;

        }
        #endregion

        #endregion

        #region Web、Admin 公共方法

        #region 根据用户ID，用户类型，获取公司信息
        /// <summary>
        /// 根据用户ID，用户类型，获取公司信息
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result_Model<ShopInfo> GetCompanyInfo_ByUserIdAndUserType(long userId)
        {
            Result_Model<ShopInfo> res = new Result_Model<ShopInfo>()
            {
                Msg = new Result_Msg() { IsSuccess = true },
                Model = new ShopInfo()
            };
            UserMemberInfo memberInfo = (from p in context.UserMemberInfo where p.Id == userId select p).FirstOrDefault();

            if (memberInfo != null && memberInfo.UserName.Length > 0)
            {
                if (memberInfo.UserType == 3)//采购商
                {
                    MemberDetail model = (from p in context.MemberDetail where p.MemberId == userId select p).FirstOrDefault();
                    if (model != null && model.CompanyName.Length > 0)
                    {
                        res.Model.CompanyName = model.CompanyName;
                    }
                    else
                    {
                        res.Msg.Message = "采购商公司名称未设置";
                        res.Msg.IsSuccess = false;
                    }
                }
                else if (memberInfo.UserType == 2)//供应商
                {
                    ManagerInfo manager = (from p in context.ManagerInfo where p.UserName == memberInfo.UserName select p).FirstOrDefault();
                    ShopInfo shop = (from p in context.ShopInfo where p.Id == manager.ShopId select p).FirstOrDefault();
                    if (shop != null && shop.CompanyName != null && shop.CompanyName.Length > 0)
                    {
                        res.Model.CompanyName = shop.CompanyName;
                        res.Model.Id = shop.Id;
                    }
                    else
                    {
                        res.Msg.Message = "店铺名称未设置";
                        res.Msg.IsSuccess = false;
                    }
                }
            }
            else
            {
                res.Msg.Message = "会员信息获取失败";
                res.Msg.IsSuccess = false;
            }

            return res;
        }
        #endregion

        #endregion
    }
}
