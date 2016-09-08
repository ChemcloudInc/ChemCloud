using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class StatisticsMoneyService : ServiceBase, IStatisticsMoneyService, IService, IDisposable
    {

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="statisQuery"></param>
        /// <returns></returns>
        public ChemCloud.Model.PageModel<StatisticsMoney> GetList(StatisticsQuery statisQuery)
        {
            int num = 0;
            IQueryable<StatisticsMoney> sm = context.StatisticsMoney.AsQueryable<StatisticsMoney>();
            //start time
            if (!string.IsNullOrWhiteSpace(statisQuery.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(statisQuery.beginTime, out dt))
                {
                    DateTime value = DateTime.Parse(statisQuery.beginTime);
                    sm =
                    from d in sm
                    where d.TradingTime >= value
                    select d;
                }
            }
            //end time
            if (!string.IsNullOrWhiteSpace(statisQuery.endTime))
            {
                DateTime dt;
                if (DateTime.TryParse(statisQuery.endTime, out dt))
                {
                    DateTime value = DateTime.Parse(statisQuery.endTime);
                    sm =
                    from d in sm
                    where d.TradingTime <= value
                    select d;
                }
            }
            //user id
            if (!string.IsNullOrWhiteSpace(statisQuery.userid.ToString()) && statisQuery.userid != 0)
            {
                long uid = statisQuery.userid;
                sm =
                    from d in sm
                    where d.UserId == uid
                    select d;
            }
            //用户类型
            if (!string.IsNullOrWhiteSpace(statisQuery.userType))
            {
                int i;
                if (int.TryParse(statisQuery.userType, out i))
                {
                    i = int.Parse(statisQuery.userType);
                    sm =
                        from d in sm
                        where d.UserType == i
                        select d;
                }
            }
            //交易类型
            if (!string.IsNullOrWhiteSpace(statisQuery.type))
            {
                int i;
                if (int.TryParse(statisQuery.type, out i))
                {
                    i = int.Parse(statisQuery.type);
                    sm =
                    from d in sm
                    where d.TradingType == i
                    select d;
                }
            }

            sm = sm.GetPage(out num, statisQuery.PageNo, statisQuery.PageSize, (IQueryable<StatisticsMoney> d) =>
                from o in d
                orderby o.TradingTime descending
                select o);
            //addMoney = db.Orders.Select(o => o.Freight).Sum();
            return new PageModel<StatisticsMoney>()
            {

                Models = sm,
                Total = num
            };
        }

        /// <summary>
        /// 充值金额
        /// </summary>
        /// <param name="smQuery"></param>
        /// <returns></returns>
        public decimal GetAddMoney(StatisticsQuery smQuery)
        {
            IQueryable<StatisticsMoney> sm = context.StatisticsMoney.AsQueryable<StatisticsMoney>();
            //start time
            if (!string.IsNullOrWhiteSpace(smQuery.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(smQuery.beginTime, out dt))
                {
                    DateTime value = DateTime.Parse(smQuery.beginTime);
                    sm =
                    from d in sm
                    where d.TradingTime >= value
                    select d;
                }
            }
            //end time
            if (!string.IsNullOrWhiteSpace(smQuery.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(smQuery.endTime, out dt))
                {
                    DateTime value = DateTime.Parse(smQuery.endTime);
                    sm =
                    from d in sm
                    where d.TradingTime <= value
                    select d;
                }
            }
            //user id
            if (!string.IsNullOrWhiteSpace(smQuery.userid.ToString()) && smQuery.userid != 0)
            {
                long uid = smQuery.userid;
                sm =
                    from d in sm
                    where d.UserId == uid
                    select d;
            }
            //用户类型
            if (!string.IsNullOrWhiteSpace(smQuery.userType))
            {
                int i;
                if (int.TryParse(smQuery.userType, out i))
                {
                    i = int.Parse(smQuery.userType);
                    sm =
                        from d in sm
                        where d.UserType == i
                        select d;
                }
            }
            //交易类型 0
            sm =
            from d in sm
            where d.TradingType != 1
            select d;
            if (sm == null)
            {
                return 0;
            }
            else
            {
                return sm.Select(o => (decimal?)o.TradingPrice).Sum().GetValueOrDefault();
            }
        }


        /// <summary>
        /// 提现金额
        /// </summary>
        /// <param name="smQuery"></param>
        /// <returns></returns>
        public decimal GetRemoveMoney(StatisticsQuery smQuery)
        {
            IQueryable<StatisticsMoney> sm = context.StatisticsMoney.AsQueryable<StatisticsMoney>();
            //start time
            if (!string.IsNullOrWhiteSpace(smQuery.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(smQuery.beginTime, out dt))
                {
                    DateTime value = DateTime.Parse(smQuery.beginTime);
                    sm =
                    from d in sm
                    where d.TradingTime >= value
                    select d;
                }
            }
            //end time
            if (!string.IsNullOrWhiteSpace(smQuery.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(smQuery.endTime, out dt))
                {
                    DateTime value = DateTime.Parse(smQuery.endTime);
                    sm =
                    from d in sm
                    where d.TradingTime <= value
                    select d;
                }
            }
            //user id
            if (!string.IsNullOrWhiteSpace(smQuery.userid.ToString()) && smQuery.userid != 0)
            {
                long uid = smQuery.userid;
                sm =
                    from d in sm
                    where d.UserId == uid
                    select d;
            }
            //用户类型
            if (!string.IsNullOrWhiteSpace(smQuery.userType))
            {
                int i;
                if (int.TryParse(smQuery.userType, out i))
                {
                    i = int.Parse(smQuery.userType);
                    sm =
                        from d in sm
                        where d.UserType == i
                        select d;
                }
            }
            //交易类型 1
            sm =
            from d in sm
            where d.TradingType == 1
            select d;
            return sm.Select(o => (decimal?)o.TradingPrice).Sum().GetValueOrDefault();
        }

        public void Add(StatisticsMoney sm)
        {
            if (sm == null)
            {
                return;
            }
            context.StatisticsMoney.Add(sm);
            context.SaveChanges();
        }

        /// <summary>
        /// 供应商查询
        /// </summary>
        /// <param name="statisQuery"></param>
        /// <returns></returns>
        public PageModel<OrderMoneyModel> GetList1(StatisticsQuery statisQuery)
        {
            int num;
            IQueryable<OrderMoneyModel> sm;
            //start time
            if (!string.IsNullOrWhiteSpace(statisQuery.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(statisQuery.beginTime, out dt))
                {
                    DateTime value = DateTime.Parse(statisQuery.beginTime);
                    sm =
                        from a in context.OrderInfo
                        join c in context.StatisticsMoney on a.Id.ToString() equals c.OrderNum
                        where a.ShopId == statisQuery.shopId && a.OrderDate > value //&& a.OrderStatus == OrderInfo.OrderOperateStatus.Finish
                        select new OrderMoneyModel
                        {
                            Id = a.Id,
                            OrderDate = a.OrderDate,
                            UserId = a.UserId,
                            UserName = a.UserName,
                            TradingPrice = c.TradingPrice
                        };
                }
            }
            //end time
            if (!string.IsNullOrWhiteSpace(statisQuery.endTime))
            {
                DateTime dt;
                if (DateTime.TryParse(statisQuery.endTime, out dt))
                {
                    DateTime value = DateTime.Parse(statisQuery.endTime);
                    sm =
                    from a in context.OrderInfo
                    join c in context.StatisticsMoney on a.Id.ToString() equals c.OrderNum
                    where a.ShopId == statisQuery.shopId && a.OrderDate <= value //&& a.OrderStatus == OrderInfo.OrderOperateStatus.Finish
                    select new OrderMoneyModel
                    {
                        Id = a.Id,
                        OrderDate = a.OrderDate,
                        UserId = a.UserId,
                        UserName = a.UserName,
                        TradingPrice = c.TradingPrice
                    };
                }
            }
            sm =
                from a in context.OrderInfo
                join c in context.StatisticsMoney on a.Id.ToString() equals c.OrderNum
                where a.ShopId == statisQuery.shopId //&& a.OrderStatus == OrderInfo.OrderOperateStatus.Finish
                select new OrderMoneyModel
                {
                    Id = a.Id,
                    OrderDate = a.OrderDate,
                    UserId = a.UserId,
                    UserName = a.UserName,
                    TradingPrice = c.TradingPrice
                };
            IQueryable<OrderMoneyModel> accountMetaModels = sm.FindBy<OrderMoneyModel, long>((OrderMoneyModel item) => true, statisQuery.PageNo, statisQuery.PageSize, out num, (OrderMoneyModel item) => item.Id, false);
            return new PageModel<OrderMoneyModel>()
            {

                Models = sm,
                Total = num
            };
        }

        /// <summary>
        /// 供应商收入总额
        /// </summary>
        /// <param name="smQuery"></param>
        /// <returns></returns>
        public decimal GetMoney(StatisticsQuery smQuery)
        {
            IQueryable<OrderMoneyModel> sm;
            //start time
            if (!string.IsNullOrWhiteSpace(smQuery.beginTime))
            {
                DateTime dt;
                if (DateTime.TryParse(smQuery.beginTime, out dt))
                {
                    DateTime value = DateTime.Parse(smQuery.beginTime);
                    sm =
                        from a in context.OrderInfo
                        join c in context.StatisticsMoney on a.Id.ToString() equals c.OrderNum
                        where a.ShopId == smQuery.shopId && (a.OrderDate > value)// && (a.OrderStatus == OrderInfo.OrderOperateStatus.Finish) 
                        select new OrderMoneyModel
                        {
                            Id = a.Id,
                            OrderDate = a.OrderDate,
                            UserId = a.UserId,
                            UserName = a.UserName,
                            TradingPrice = c.TradingPrice
                        };
                }
            }
            //end time
            if (!string.IsNullOrWhiteSpace(smQuery.endTime))
            {
                DateTime dt;
                if (DateTime.TryParse(smQuery.endTime, out dt))
                {
                    DateTime value = DateTime.Parse(smQuery.endTime);
                    sm =
                    from a in context.OrderInfo
                    join c in context.StatisticsMoney on a.Id.ToString() equals c.OrderNum
                    where a.ShopId == smQuery.shopId && (a.OrderDate <= value)//&& (a.OrderStatus == OrderInfo.OrderOperateStatus.Finish)
                    select new OrderMoneyModel
                    {
                        Id = a.Id,
                        OrderDate = a.OrderDate,
                        UserId = a.UserId,
                        UserName = a.UserName,
                        TradingPrice = c.TradingPrice
                    };
                }
            }
            sm = from a in context.OrderInfo
                 join c in context.StatisticsMoney on a.Id.ToString() equals c.OrderNum
                 where a.ShopId == smQuery.shopId //&& a.OrderStatus == OrderInfo.OrderOperateStatus.Finish
                 select new OrderMoneyModel
                 {
                     Id = a.Id,
                     OrderDate = a.OrderDate,
                     UserId = a.UserId,
                     UserName = a.UserName,
                     TradingPrice = c.TradingPrice
                 };
            return sm.Select(o => (decimal?)o.TradingPrice).Sum().GetValueOrDefault();
        }


        public StatisticsMoney GetLastStatisticsMoneyByUid(long uid, int utype)
        {
            if (uid == 0)
            {
                return null;
            }
            StatisticsMoney smInfo = (
                from a in context.StatisticsMoney
                where a.UserId == uid && (!a.OrderNum.Equals("")) && !(a.OrderNum.Equals(null)) && a.UserType == utype
                orderby a.TradingTime descending
                select a).FirstOrDefault();
            return smInfo;
        }


        public decimal getMyMoney(long uid, int userType)
        {
            CapitalInfo SMInfo = (
                 from a in context.CapitalInfo

                 where a.ManageId == uid

                 select a).FirstOrDefault();
            decimal aa;
            if (SMInfo != null)
            {
                aa = decimal.Parse(SMInfo.Balance.ToString());
            }
            else
            {
                aa = 0;
            }
            return aa;
        }


        public decimal GetMoneyByUidType(long uid, int userType)
        {
            CapitalInfo SMInfo;
            decimal aa;
            if (userType == 2)
            {
                SMInfo = (
                 from a in context.CapitalInfo
                 where a.ManageId == uid
                 select a).FirstOrDefault();
            }
            else if (userType == 3)
            {
                SMInfo = (
                 from a in context.CapitalInfo
                 where a.MemId == uid
                 select a).FirstOrDefault();
            }
            else
            {
                SMInfo = (
                 from a in context.CapitalInfo
                 where a.MemId == 0 || a.ManageId == 0
                 select a).FirstOrDefault();
            }
            if (SMInfo != null)
            {
                aa = decimal.Parse(SMInfo.Balance.ToString());
            }
            else
            {
                aa = 0;
            }
            return aa;
        }
    }
}
