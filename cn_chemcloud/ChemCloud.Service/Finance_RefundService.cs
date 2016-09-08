using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace ChemCloud.Service
{
    public class Finance_RefundService : ServiceBase, IFinance_RefundService, IService, IDisposable
    {
        public Finance_RefundService()
        {

        }

        public Finance_Refund GetFinance_RefundInfo(long frid)
        {
            Finance_Refund finfo = new Finance_Refund();
            finfo = (
            from p in context.Finance_Refund
            where p.Refund_Number.Equals(frid)
            select p).FirstOrDefault();
            return finfo;
        }

        public PageModel<Finance_Refund> GetFinance_RefundListInfo(QueryModel.Finance_RefundQuery fQuery)
        {
            int num = 0;
            //IQueryable<Finance_Refund> fp = from a in context.Finance_Refund where a.Refund_Status != 1 && a.Refund_Status != 2 select a;  old vesion
            IQueryable<Finance_Refund> fp = from a in context.Finance_Refund select a;
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.Refund_Time >= dt
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
                    where d.Refund_Time <= dt
                    select d;
                }
            }
            if (fQuery.orderNum!=0)
            {//订单号
                fp =
                    from d in fp
                    where d.Refund_OrderNum.Equals(fQuery.orderNum)
                    select d;
            }
            if (fQuery.userid != 0)
            {//供应商用户编号
                fp =
                    from d in fp
                    where d.Refund_UserId.Equals(fQuery.userid)
                    select d;
            }
            if (fQuery.usertype != 0)
            {//供应商用户类型
                fp =
                    from d in fp
                    where d.Refund_UserType.Equals(fQuery.usertype)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(fQuery.musername))
            {//采购商用户名称
                fp =
                    from d in fp
                    where d.Refund_ToUserName.Equals(fQuery.musername)
                    select d;
            }
            if (fQuery.muserid!=0)
            {//采购商用户编号
                fp =
                    from d in fp
                    where d.Refund_ToUserId.Equals(fQuery.muserid)
                    select d;
            }
            if (fQuery.musertype != 0)
            {//采购商用户类型
                fp =
                    from d in fp
                    where d.Refund_ToUserType.Equals(fQuery.musertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.Refund_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_Refund> d) =>
                from o in d
                orderby o.Refund_Time 
                select o);
            return new PageModel<Finance_Refund>()
            {

                Models = fp,
                Total = num
            };
        }

        public PageModel<Finance_Refund> GetFinance_RefundListInfo1(QueryModel.Finance_RefundQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_Refund> fp = from a in context.Finance_Refund where a.Refund_Status == 1 || a.Refund_Status == 2 select a;
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.Refund_Time >= dt
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
                    where d.Refund_Time <= dt
                    select d;
                }
            }
            if (fQuery.orderNum != 0)
            {//订单号
                fp =
                    from d in fp
                    where d.Refund_OrderNum.Equals(fQuery.orderNum)
                    select d;
            }
            if (fQuery.userid != 0)
            {//供应商用户编号
                fp =
                    from d in fp
                    where d.Refund_UserId.Equals(fQuery.userid)
                    select d;
            }
            if (fQuery.usertype != 0)
            {//供应商用户类型
                fp =
                    from d in fp
                    where d.Refund_UserType.Equals(fQuery.usertype)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(fQuery.musername))
            {//采购商用户名称
                fp =
                    from d in fp
                    where d.Refund_ToUserName.Equals(fQuery.musername)
                    select d;
            }
            if (fQuery.muserid != 0)
            {//采购商用户编号
                fp =
                    from d in fp
                    where d.Refund_ToUserId.Equals(fQuery.muserid)
                    select d;
            }
            if (fQuery.musertype != 0)
            {//采购商用户类型
                fp =
                    from d in fp
                    where d.Refund_ToUserType.Equals(fQuery.musertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.Refund_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_Refund> d) =>
                from o in d
                orderby o.Refund_Time 
                select o);
            return new PageModel<Finance_Refund>()
            {

                Models = fp,
                Total = num
            };
        }
        public bool UpdateFinance_Refund(Finance_Refund finfo)
        {
            if (finfo == null)
            {
                return false;
            }
            Finance_Refund f = context.Finance_Refund.FirstOrDefault((Finance_Refund m) => m.Id == finfo.Id);
            if (f == null)
            {
                return false;
            }
            int i = 0;
            f.Refund_Number = finfo.Refund_Number;
            f.Refund_OrderNum = finfo.Refund_OrderNum;
            f.Refund_UserId = finfo.Refund_UserId;
            f.Refund_UserType = finfo.Refund_UserType;
            f.Refund_Money = finfo.Refund_Money;
            f.Refund_MoneyType = finfo.Refund_MoneyType;
            f.Refund_SXMoney = finfo.Refund_SXMoney;
            f.Refund_ISChujing = finfo.Refund_ISChujing;
            f.Refund_Address = finfo.Refund_Address;
            f.Refund_Time = finfo.Refund_Time;
            f.Refund_Status = finfo.Refund_Status;
            f.Refund_ToUserId = finfo.Refund_ToUserId;
            f.Refund_ToUserType = finfo.Refund_ToUserType;
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

        public bool AddFinance_Refund(Finance_Refund finfo)
        {
            int i = 0;
            if (finfo == null || finfo.Id != 0)
            {
                return false;
            }
            context.Finance_Refund.Add(finfo);
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

        public PageModel<Finance_Refund> GetFinance_RefundList_Statistics(QueryModel.Finance_RefundQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_Refund> fp = context.Finance_Refund.AsQueryable<Finance_Refund>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.Refund_Time >= dt
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
                    where d.Refund_Time <= dt
                    select d;
                }
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.Refund_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }

            var qq = from p in fp.ToList()
                     group p by p.Refund_Time.ToString("yyyy-MM-dd") into g
                     select new Finance_Refund()
                     {
                         Refund_Time = DateTime.Parse(g.Key),
                         Refund_MoneyType = g.Max(p => p.Refund_MoneyType),
                         Refund_Money = g.Sum(p => p.Refund_Money)
                     };

            fp = qq.AsQueryable();

            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_Refund> d) =>
                from o in d
                orderby o.Refund_Time descending
                select o);
            return new PageModel<Finance_Refund>()
            {

                Models = fp,
                Total = num
            };
        }
    }
}
