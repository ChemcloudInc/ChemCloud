using ChemCloud.Model.Common;
using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class TransactionRecordService : ServiceBase, ITransactionRecordService, IService, IDisposable
    {
        HashSet_Common hashSet = new HashSet_Common();

        /// <summary>
        /// 根据查询条件获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public Result_List_Pager<Result_TransactionRecord> GetTransactionRecordList(TransactionRecordQuery query)
        {
            Result_List_Pager<Result_TransactionRecord> res = new Result_List_Pager<Result_TransactionRecord>()
            {
                List = new List<Result_TransactionRecord>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                if (query.StartDate > query.EndDate)
                {
                    res.Msg.IsSuccess = false;
                    res.Msg.Message = "开始时间不能大于结束时间";
                }
                else
                {
                    //开始月份与结束月份相同或开始月份小于结束月份时，查询开始月份第一天至结束月份最后一天内的数据

                    query.StartDate = query.StartDate.Date;
                    query.EndDate = query.EndDate.Date.AddMonths(1);
                    int total = 0;
                    List<TransactionRecord> list;
                    if (query.CurveType == 0)
                    {
                        total = (from p in context.TransactionRecord where p.YearMonth >= query.StartDate && p.YearMonth < query.EndDate select p).Count();
                        list = (from p in context.TransactionRecord where p.YearMonth >= query.StartDate && p.YearMonth < query.EndDate select p).OrderBy(p => p.YearMonth).Skip((query.PageNo - 1) * query.PageSize).Take(query.PageSize).ToList();
                    }
                    else
                    {
                        total = (from p in context.TransactionRecord where p.YearMonth >= query.StartDate && p.YearMonth < query.EndDate && p.CurveType == query.CurveType select p).Count();
                        list = (from p in context.TransactionRecord where p.YearMonth >= query.StartDate && p.YearMonth < query.EndDate && p.CurveType == query.CurveType select p).OrderBy(p => p.YearMonth).Skip((query.PageNo - 1) * query.PageSize).Take(query.PageSize).ToList();
                    }
                    res.List = list.Select(x => new Result_TransactionRecord()
                    {
                        Id = x.Id,
                        XName_CN = x.XName_CN,
                        XName_Eng = x.XName_Eng,
                        Y_CompleteAmount = x.Y_CompleteAmount,
                        Y_CompleteNumber = x.Y_CompleteNumber,
                        Y_OrderAmount = x.Y_OrderAmount,
                        Y_OrderQuantity = x.Y_OrderQuantity,
                        CurveType = HashSet_Common.hashCurveType.Where(y => y.Key == x.CurveType).FirstOrDefault().Value,
                        YearMonth = x.YearMonth.Year.ToString() + "-" + x.YearMonth.Month.ToString(),
                        IsTrue = x.IsTrue
                    }).ToList();

                    res.PageInfo.CurrentPage = query.PageNo;
                    res.PageInfo.PageSize = query.PageSize;
                    res.PageInfo.Total = total;
                    res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageSize));


                    res.Msg.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }

        /// <summary>
        /// 加载首页Banner
        /// </summary>
        /// <returns></returns>
        public Result_List1<Result_TransactionRecord> GetTransactionRecordList_Web(int count)
        {
            Result_List1<Result_TransactionRecord> resList = new Result_List1<Result_TransactionRecord>();
            try
            {
                //1：代理采购；
                var list1 = (from p in context.TransactionRecord where p.CurveType == 1 select p).OrderByDescending(x => x.YearMonth).Take(count).ToList();
                resList.List1 = list1.Select(x => new Result_TransactionRecord()
               {
                   Id = x.Id,
                   CurveType = HashSet_Common.hashCurveType.Where(y => y.Key == x.CurveType).FirstOrDefault().Value,
                   XName_CN = x.XName_CN,
                   XName_Eng = x.XName_Eng,
                   Y_CompleteAmount = x.Y_CompleteAmount,
                   Y_OrderAmount = x.Y_OrderAmount,
                   Y_CompleteNumber = x.Y_CompleteNumber,
                   Y_OrderQuantity = x.Y_OrderQuantity,
                   YearMonth = x.YearMonth.ToString("yyyy-MM"),
                   IsTrue = x.IsTrue
               }).ToList();

                //2：定制合成
                var list2 = (from p in context.TransactionRecord where p.CurveType == 2 select p).OrderByDescending(x => x.YearMonth).Take(count).ToList();
                resList.List2 = list2.Select(x => new Result_TransactionRecord()
                {
                    Id = x.Id,
                    CurveType = HashSet_Common.hashCurveType.Where(y => y.Key == x.CurveType).FirstOrDefault().Value,
                    XName_CN = x.XName_CN,
                    XName_Eng = x.XName_Eng,
                    Y_CompleteAmount = x.Y_CompleteAmount,
                    Y_OrderAmount = x.Y_OrderAmount,
                    Y_CompleteNumber = x.Y_CompleteNumber,
                    Y_OrderQuantity = x.Y_OrderQuantity,
                    YearMonth = x.YearMonth.ToString("yyyy-MM"),
                    IsTrue = x.IsTrue
                }).ToList();
                resList.Msg = new Result_Msg() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                resList.Msg = new Result_Msg() { IsSuccess = false, Message = "查询交易记录失败，失败原因：" + ex.Message };
            }
            return resList;
        }

        /// <summary>
        /// 删除指定ID数据
        /// </summary>
        /// <param name="Id">实时交易信息ID</param>
        /// <returns></returns>
        public Result_Msg DeleteById(int Id, long userId)
        {
            Result_Msg res = new Result_Msg();
            TransactionRecord job = (from p in context.TransactionRecord where p.Id == Id select p).FirstOrDefault();
            context.TransactionRecord.Remove(job);
            res.IsSuccess = context.SaveChanges() == 1;
            return res;
        }

        /// <summary>
        /// 新增实时交易信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg TransactionRecordAdd(TransactionRecordAddQuery query)
        {
            Result_Msg res = new Result_Msg();
            Result_Model<TransactionRecord> model = GetTransactionRecordByYearMonthAndCurveType(query.CurveType, query.YearMonth);
            if (!model.Msg.IsSuccess)
            {
                TransactionRecord job = new TransactionRecord()
                {
                    CurveType = query.CurveType,
                    YearMonth = query.YearMonth,
                    XName_CN = query.XName_CN,
                    XName_Eng = query.XName_Eng,
                    Y_CompleteAmount = query.Y_CompleteAmount,
                    Y_OrderAmount = query.Y_OrderAmount,
                    Y_CompleteNumber = query.Y_CompleteNumber,
                    Y_OrderQuantity = query.Y_OrderQuantity,
                    IsTrue = 1//1：真是；0:伪造
                };
                context.TransactionRecord.Add(job);
                res.IsSuccess = context.SaveChanges() == 1;
            }
            else
            {
                res.IsSuccess = false;
                res.Message = model.Msg.Message;
            }

            return res;
        }

        /// <summary>
        /// 获取实时交易信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result_TransactionRecord GetObjectById(int Id)
        {
            TransactionRecord model = (from p in context.TransactionRecord where p.Id == Id select p).FirstOrDefault();
            Result_TransactionRecord res = new Result_TransactionRecord()
            {
                Id = model.Id,
                XName_CN = model.XName_CN,
                XName_Eng = model.XName_Eng,
                Y_CompleteAmount = model.Y_CompleteAmount,
                Y_OrderAmount = model.Y_OrderAmount,
                Y_CompleteNumber = model.Y_CompleteNumber,
                Y_OrderQuantity = model.Y_OrderQuantity,
                CurveTypeName = HashSet_Common.hashCurveType.Where(y => y.Key == model.CurveType).FirstOrDefault().Value,
                CurveType = model.CurveType.ToString(),
                IsTrue = model.IsTrue,
                YearMonth = model.YearMonth.ToString("yyyy-MM")
            };

            return res;
        }

        /// <summary>
        /// 修改实时交易信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg ModifyTransactionRecord(TransactionRecordModifyQuery query)
        {
            Result_Msg res = new Result_Msg();
            TransactionRecord modelOld = context.TransactionRecord.FindById<TransactionRecord>(query.ID);

            try
            {
                if (modelOld != null)
                {
                    modelOld.Id = query.ID;
                    modelOld.XName_CN = query.XName_CN;
                    modelOld.XName_Eng = query.XName_Eng;
                    modelOld.Y_CompleteAmount = query.Y_CompleteAmount;
                    modelOld.Y_OrderAmount = query.Y_OrderAmount;
                    modelOld.Y_CompleteNumber = query.Y_CompleteNumber;
                    modelOld.Y_OrderQuantity = query.Y_OrderQuantity;
                    modelOld.YearMonth = query.YearMonth;
                    modelOld.IsTrue = query.IsTrue;
                    modelOld.CurveType = query.CurveType;
                    res.IsSuccess = context.SaveChanges() == 1;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = "修改失败，数据库中不存在该记录";
                }

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = "修改失败，失败原因：" + ex.Message;
            }

            return res;
        }


        /// <summary>
        /// 根据参数，统计指定的实时交易数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<TransactionRecord> ComputeFun(TransactionRecordCompute query)
        {
            Result_Model<TransactionRecord> res = new Result_Model<TransactionRecord>() { Msg = new Result_Msg(), Model = new TransactionRecord() };
            try
            {
                res.Model = new TransactionRecord()
                {
                    CurveType = query.CurveType,
                    YearMonth = query.YearMonth,
                    Y_CompleteAmount = Get_Y_CompleteAmount(query),
                    Y_OrderAmount = Get_Y_OrderAmount(query),
                    Y_CompleteNumber = Get_Y_CompleteNumber(query),
                    Y_OrderQuantity = Get_Y_OrderQuantity(query)
                };
                res.Msg.IsSuccess = true;
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = ex.Message;
            }
            return res;
        }

        #region 根据年月、订单类型，查找实时交易统计数据
        /// <summary>
        /// 根据年月、订单类型，查找实时交易统计数据
        /// </summary>
        /// <param name="yaarMonth"></param>
        /// <param name="curveType"></param>
        /// <returns></returns>
        public Result_Model<TransactionRecord> GetTransactionRecordByYearMonthAndCurveType(int curveType, DateTime yearMonth)
        {
            string curveTypeName = curveType == 1 ? "代理采购" : "定制合成";
            Result_Model<TransactionRecord> res = new Result_Model<TransactionRecord>() { Msg = new Result_Msg() };

            res.Model = (from p in context.TransactionRecord where p.CurveType == curveType && p.YearMonth == yearMonth select p).FirstOrDefault();
            if (res.Model != null)
            {
                res.Msg.IsSuccess = true;
                res.Msg.Message = "交易类型为：" + curveTypeName + ",时间为：" + yearMonth.ToString() + " 的记录已存在。";
            }
            else
            {
                res.Msg.IsSuccess = false;
            }

            return res;
        }
        #endregion

        #region 获取指定的数据
        /// <summary>
        /// 完成总金额（已确认）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public decimal Get_Y_CompleteAmount(TransactionRecordCompute query)
        {
            DateTime dtStart = query.YearMonth;
            DateTime endStart = query.YearMonth.AddMonths(1);
            decimal val = 0;

            if (query.CurveType == 0)
            {
                //代理采购
                var list = (from p in context.OrderPurchasing where p.OrderTime >= dtStart && p.OrderTime < endStart && p.Status == 1 select p).ToList();
                foreach (var item in list)
                {
                    val += (item.ProductCount * Convert.ToDecimal(item.ProductPrice));
                }
            }
            else
            {
                //定制合成
                var list = (from p in context.OrderSynthesis where p.OrderTime >= dtStart && p.OrderTime < endStart && p.Status == 1 select p).ToList();
                foreach (var item in list)
                {
                    val += (Convert.ToInt32(item.ProductCount) * Convert.ToDecimal(item.Price));
                }
            }
            return val;
        }
        /// <summary>
        /// 下单总金额（未确认）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public decimal Get_Y_OrderAmount(TransactionRecordCompute query)
        {
            DateTime dtStart = query.YearMonth;
            DateTime endStart = query.YearMonth.AddMonths(1);

            decimal val = 0;
            if (query.CurveType == 0)
            {
                //代理采购
                var list = (from p in context.OrderPurchasing where p.OrderTime >= dtStart && p.OrderTime < endStart && p.Status == 0 select p).ToList();
                foreach (var item in list)
                {
                    val += (item.ProductCount * Convert.ToDecimal(item.ProductPrice));
                }
            }
            else
            {
                //定制合成
                var list = (from p in context.OrderSynthesis where p.OrderTime >= dtStart && p.OrderTime < endStart && p.Status == 0 select p).ToList();
                foreach (var item in list)
                {
                    val += (Convert.ToInt32(item.ProductCount) * Convert.ToDecimal(item.Price));
                }
            }

            return val;
        }
        /// <summary>
        /// 完成订单数（已确认）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int Get_Y_CompleteNumber(TransactionRecordCompute query)
        {
            DateTime dtStart = query.YearMonth;
            DateTime endStart = query.YearMonth.AddMonths(1);

            int val = 0;
            if (query.CurveType == 0)
            {
                //代理采购
                var list = (from p in context.OrderPurchasing where p.OrderTime >= dtStart && p.OrderTime < endStart && p.Status == 1 select p).ToList();
                foreach (var item in list)
                {
                    val += item.ProductCount;
                }
            }
            else
            {
                //定制合成
                var list = (from p in context.OrderSynthesis where p.OrderTime >= dtStart && p.OrderTime < endStart && p.Status == 1 select p).ToList();
                foreach (var item in list)
                {
                    val += Convert.ToInt32(item.ProductCount);
                }
            }
            return val;
        }
        /// <summary>
        /// 下单订单数（未确认）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int Get_Y_OrderQuantity(TransactionRecordCompute query)
        {
            DateTime dtStart = query.YearMonth;
            DateTime endStart = query.YearMonth.AddMonths(1);

            int val = 0;
            if (query.CurveType == 0)
            {
                //代理采购
                var list = (from p in context.OrderPurchasing where p.OrderTime >= dtStart && p.OrderTime < endStart && p.Status == 0 select p).ToList();
                foreach (var item in list)
                {
                    val += item.ProductCount;
                }
            }
            else
            {
                //定制合成
                var list = (from p in context.OrderSynthesis where p.OrderTime >= dtStart && p.OrderTime < endStart && p.Status == 0 select p).ToList();
                foreach (var item in list)
                {
                    val += Convert.ToInt32(item.ProductCount);
                }
            }
            return val;
        }
        #endregion

    }
}
