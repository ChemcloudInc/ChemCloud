using ChemCloud.Core;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class THService : ServiceBase, ITHService, IService, IDisposable
    {
        public THService() { }

        /// <summary>
        /// list search
        /// </summary>
        /// <param name="fQuery">Query</param>
        /// <returns></returns>
        public Model.PageModel<TH> GetTHListInfo(THQuery fQuery)
        {
            int num = 0;
            IQueryable<TH> fp = from a in context.TH select a;

            /*已审核 未审核*/
            if (fQuery.Status == 6)
            {
                fp =
                       from d in fp
                       where d.TH_Status.Equals(fQuery.Status)
                       select d;
            }
            else
            {
                fp =
                          from d in fp
                          where d.TH_Status != 6
                          select d;
            }

            if (fQuery.orderNum != 0)
            {//订单号
                fp =
                    from d in fp
                    where d.TH_OrderNum.Equals(fQuery.orderNum)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(fQuery.productName))
            {//产品
                fp =
                    from d in fp
                    where d.TH_ProductName.Equals(fQuery.productName)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.TH_Time >= dt
                    select d;
                }
            }
            if (!string.IsNullOrWhiteSpace(fQuery.endtime))
            {//结束时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.endtime, out dt))
                {
                    dt = DateTime.Parse(fQuery.endtime);
                    fp =
                    from d in fp
                    where d.TH_Time <= dt
                    select d;
                }
            }
            if (!string.IsNullOrWhiteSpace(fQuery.username))
            {//采购商名字
                fp =
                    from d in fp
                    where d.TH_UserName.Equals(fQuery.username)
                    select d;
            }
            if (fQuery.muserid != 0)
            {//供应商用户编号
                fp =
                    from d in fp
                    where d.TH_ToUserId.Equals(fQuery.muserid)
                    select d;
            }
            if (fQuery.musertype != 0)
            {//供应商用户类型
                fp =
                    from d in fp
                    where d.TH_ToUserType.Equals(fQuery.musertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.TH_ProductMoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<TH> d) =>
                from o in d
                orderby o.TH_Time descending
                select o);
            return new PageModel<TH>()
            {

                Models = fp,
                Total = num
            };
        }

        public Model.PageModel<TH> GetTHListInfo1(THQuery fQuery)
        {
            int num = 0;
            IQueryable<TH> fp = from a in context.TH select a;
            if (fQuery.orderNum != 0)
            {//订单号
                fp =
                    from d in fp
                    where d.TH_OrderNum.Equals(fQuery.orderNum)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(fQuery.productName))
            {//产品
                fp =
                    from d in fp
                    where d.TH_ProductName.Equals(fQuery.productName)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.TH_Time >= dt
                    select d;
                }
            }
            if (!string.IsNullOrWhiteSpace(fQuery.endtime))
            {//结束时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.endtime, out dt))
                {
                    dt = DateTime.Parse(fQuery.endtime);
                    fp =
                    from d in fp
                    where d.TH_Time <= dt
                    select d;
                }
            }
            if (!string.IsNullOrWhiteSpace(fQuery.username))
            {//采购商名字
                fp =
                    from d in fp
                    where d.TH_UserName.Equals(fQuery.username)
                    select d;
            }
            if (fQuery.muserid != 0)
            {//供应商用户编号
                fp =
                    from d in fp
                    where d.TH_ToUserId.Equals(fQuery.muserid)
                    select d;
            }
            if (fQuery.musertype != 0)
            {//供应商用户类型
                fp =
                    from d in fp
                    where d.TH_ToUserType.Equals(fQuery.musertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.TH_ProductMoneyType.Equals(fQuery.moneytype)
                    select d;
            }

            if (fQuery.Status != 0 && fQuery.Status > 0)
            {
                //退款状态
                fp =
                    from d in fp
                    where d.TH_Status.Equals(fQuery.Status)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<TH> d) =>
                from o in d
                orderby o.TH_Time descending
                select o);
            return new PageModel<TH>()
            {

                Models = fp,
                Total = num
            };
        }

        /// <summary>
        /// update method
        /// </summary>
        /// <param name="info">model</param>
        /// <returns></returns>
        public bool UpdateTH(TH info)
        {
            if (info == null)
            {
                return false;
            }
            TH thinfo = context.TH.FirstOrDefault((TH m) => m.Id == info.Id);
            if (thinfo == null)
            {
                return false;
            }
            int i = 0;
            thinfo.TH_Number = info.TH_Number;
            thinfo.TH_OrderNum = info.TH_OrderNum;
            thinfo.TH_Time = info.TH_Time;
            thinfo.TH_UserId = info.TH_UserId;
            thinfo.TH_UserName = info.TH_UserName;
            thinfo.TH_UserType = info.TH_UserType;
            thinfo.TH_ProductName = info.TH_ProductName;
            thinfo.TH_ProductCount = info.TH_ProductCount;
            thinfo.TH_ProductMoney = info.TH_ProductMoney;
            thinfo.TH_ProductMoneyReal = info.TH_ProductMoneyReal;
            thinfo.TH_ProductMoneyType = info.TH_ProductMoneyType;
            thinfo.TH_ToUserId = info.TH_ToUserId;
            thinfo.TH_ToUserName = info.TH_ToUserName;
            thinfo.TH_ToUserType = info.TH_ToUserType;
            thinfo.TH_Status = info.TH_Status;
            i = context.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// add method 
        /// </summary>
        /// <param name="info">model</param>
        /// <returns></returns>
        public long AddTH(TH info)
        {
            context.TH.Add(info);
            context.SaveChanges();
            return info.Id;

        }

        public TH GetTHInfo(string th)
        {
            if (string.IsNullOrWhiteSpace(th))
            {
                return null;
            }
            long newlong = long.Parse(th);
            TH finfo = new TH();
            finfo = (
            from p in context.TH
            where p.TH_Number.Equals(newlong)
            select p).FirstOrDefault();
            return finfo;
        }

        public TH GetTHById(long id)
        {

            TH finfo = new TH();
            finfo = (
            from p in context.TH
            where p.Id.Equals(id)
            select p).FirstOrDefault();
            return finfo;
        }

        /*根据订单编号 获取退货信息*/
        public TH GetTHByOrderNum(long OrderNum)
        {

            TH finfo = new TH();
            finfo = (from p in context.TH where p.TH_OrderNum.Equals(OrderNum) select p).FirstOrDefault();
            return finfo;
        }


        /*退货列表查询*/
        public PageModel<TH> GetTHPageModel(QueryModel.THQuery tq, long UserId, long SellerUserId)
        {
            IQueryable<TH> th = (from a in context.TH select a);
            if (UserId != 0)
            {
                th = (from a in th where a.TH_UserId == UserId select a);
            }
            if (SellerUserId != 0)
            {
                th = (from a in th where a.TH_ToUserId == SellerUserId select a);
            }
            int pageNum = 0;


            if (tq.Status != 0)
            {
                th = (from u in th
                      where u.TH_Status == tq.Status
                      select u);
            }
            if (tq.orderNum != 0)
            {
                th = (from u in th
                      where u.TH_OrderNum == tq.orderNum
                      select u);
            }

            th = th.GetPage(out pageNum, tq.PageNo, tq.PageSize, (IQueryable<TH> d) =>
              from o in th
              orderby o.TH_Time descending
              select o);
            return new PageModel<TH>
            {
                Models = th,
                Total = pageNum
            };
        }



        public long InsertTHMessage(Model.THMessageInfo tkm)
        {
            context.THMessageInfo.Add(tkm);
            context.SaveChanges();
            return tkm.Id;
        }

        public void InsertTHImage(List<Model.THImageInfo> tkimages)
        {
            context.THImageInfo.AddRange(tkimages);
            context.SaveChanges();
        }

        public List<Model.THMessageInfo> getTHMessage(long tkid)
        {

            List<Model.THMessageInfo> tks = (from a in context.THMessageInfo where a.THId == tkid select a).ToList<THMessageInfo>();
            return tks;
        }

        public List<Model.THImageInfo> getTHImage(long THMessageId)
        {
            List<THImageInfo> tkis = (from a in context.THImageInfo where a.THMessageId == THMessageId select a).ToList<THImageInfo>();
            return tkis;
        }

        /*更改退货单状态*/
        public void UpdateTHStatus(long id, int THStatus)
        {
            TH th = (from a in context.TH where a.TH_OrderNum == id select a).FirstOrDefault<TH>();
            th.TH_Status = THStatus;
            context.SaveChanges();
        }

        /*退货 寄货*/
        public void SendTH(long id, string wldh, string wlgs)
        {
            TH th = (from a in context.TH where a.TH_OrderNum == id select a).FirstOrDefault<TH>();
            th.TH_Status = 4;
            th.TH_WLGS = wlgs;
            th.TH_WLDH = wldh;
            context.SaveChanges();
        }
    }
}
