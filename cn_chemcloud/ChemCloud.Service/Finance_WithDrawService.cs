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
    public class Finance_WithDrawService : ServiceBase, IFinance_WithDrawService, IService, IDisposable
    {
        public Finance_WithDrawService()
        {

        }

        public Finance_WithDraw GetFinance_WithDrawInfo(long uid, int usertype)
        {
            Finance_WithDraw finfo = new Finance_WithDraw();
            finfo = (
            from p in context.Finance_WithDraw
            where p.Withdraw_UserId.Equals(uid) && p.Withdraw_UserType.Equals(usertype)
            select p).FirstOrDefault();
            return finfo;
        }

        public PageModel<Finance_WithDraw> GetFinance_WithDrawListInfo(QueryModel.Finance_WithDrawQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_WithDraw> fp = context.Finance_WithDraw.AsQueryable<Finance_WithDraw>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.Withdraw_Time >= dt
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
                    where d.Withdraw_Time <= dt
                    select d;
                }
            }
            if (fQuery.userid != 0)
            {//用户编号
                fp =
                    from d in fp
                    where d.Withdraw_UserId.Equals(fQuery.userid)
                    select d;
            }
            if (fQuery.usertype != 0)
            {//用户类型
                fp =
                    from d in fp
                    where d.Withdraw_UserType.Equals(fQuery.usertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.Withdraw_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_WithDraw> d) =>
                from o in d
                orderby o.Withdraw_Time descending
                select o);
            return new PageModel<Finance_WithDraw>()
            {

                Models = fp,
                Total = num
            };
        }

        public bool UpdateFinance_WithDraw(Finance_WithDraw finfo)
        {
            if (finfo == null)
            {
                return false;
            }
            Finance_WithDraw f = context.Finance_WithDraw.FirstOrDefault((Finance_WithDraw m) => m.Id == finfo.Id);
            if (f == null)
            {
                return false;
            }
            int i = 0;
            f.Withdraw_Number = finfo.Withdraw_Number;
            f.Withdraw_UserId = finfo.Withdraw_UserId;
            f.Withdraw_UserType = finfo.Withdraw_UserType;
            f.Withdraw_Money = finfo.Withdraw_Money;
            f.Withdraw_MoneyType = finfo.Withdraw_MoneyType;
            f.Withdraw_BankName = finfo.Withdraw_BankName;
            f.Withdraw_BankUserName = finfo.Withdraw_BankUserName;
            f.Withdraw_Account = finfo.Withdraw_Account;
            f.Withdraw_Time = finfo.Withdraw_Time;
            f.Withdraw_Status = finfo.Withdraw_Status;
            f.Withdraw_shenhe = finfo.Withdraw_shenhe;
            f.Withdraw_shenheUid = finfo.Withdraw_shenheUid;
            f.Withdraw_shenheUname = finfo.Withdraw_shenheUname;
            f.Withdraw_shenheDesc = finfo.Withdraw_shenheDesc;
            f.Withdraw_shenheTime = finfo.Withdraw_shenheTime;
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

        public bool AddFinance_WithDraw(Finance_WithDraw finfo)
        {
            int i = 0;
            if (finfo == null || finfo.Id != 0)
            {
                return false;
            }
            context.Finance_WithDraw.Add(finfo);
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

        public PageModel<Finance_WithDraw> GetFinance_WithDrawList_Statistics(QueryModel.Finance_WithDrawQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_WithDraw> fp = context.Finance_WithDraw.AsQueryable<Finance_WithDraw>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.Withdraw_Time >= dt
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
                    where d.Withdraw_Time <= dt
                    select d;
                }
            }

            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.Withdraw_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            var qq = from p in fp.ToList()
                     group p by p.Withdraw_Time.ToString("yyyy-MM-dd") into g
                     select new Finance_WithDraw()
                     {
                         Withdraw_Time = DateTime.Parse(g.Key),
                         Withdraw_MoneyType = g.Max(p => p.Withdraw_MoneyType),
                         Withdraw_Money = g.Sum(p => p.Withdraw_Money)
                     };

            fp = qq.AsQueryable();

            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_WithDraw> d) =>
                from o in d
                orderby o.Withdraw_Time descending
                select o);
            return new PageModel<Finance_WithDraw>()
            {

                Models = fp,
                Total = num
            };
        }

        public Finance_WithDraw GetFinance_WithDrawInfo(long id)
        {
            Finance_WithDraw finfo = new Finance_WithDraw();
            finfo = (
            from p in context.Finance_WithDraw
            where p.Id.Equals(id)
            select p).FirstOrDefault();
            return finfo;
        }
    }
}
