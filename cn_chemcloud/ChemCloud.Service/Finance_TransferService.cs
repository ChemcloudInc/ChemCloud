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
    public class Finance_TransferService : ServiceBase, IFinance_TransferService, IService, IDisposable
    {
        public Finance_TransferService()
        {

        }

        public Finance_Transfer GetFinance_TransferInfo(long uid, int usertype)
        {
            Finance_Transfer finfo = new Finance_Transfer();
            finfo = (
            from p in context.Finance_Transfer
            where p.Trans_UserId.Equals(uid) && p.Trans_UserType.Equals(usertype)
            select p).FirstOrDefault();
            return finfo;
        }

        public PageModel<Finance_Transfer> GetFinance_TransferListInfo(QueryModel.Finance_TransferQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_Transfer> fp = context.Finance_Transfer.AsQueryable<Finance_Transfer>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.Trans_Time >= dt
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
                    where d.Trans_Time <= dt
                    select d;
                }
            }
            if (fQuery.userid != 0)
            {//用户编号
                fp =
                    from d in fp
                    where d.Trans_UserId.Equals(fQuery.userid) || d.Trans_ToUserId.Equals(fQuery.userid)
                    select d;
            }
            if (fQuery.usertype != 0)
            {//用户类型
                fp =
                    from d in fp
                    where d.Trans_UserType.Equals(fQuery.usertype)||d.Trans_ToUserType.Equals(fQuery.usertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.Trans_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_Transfer> d) =>
                from o in d
                orderby o.Trans_Time descending
                select o);
            return new PageModel<Finance_Transfer>()
            {

                Models = fp,
                Total = num
            };
        }

        public bool UpdateFinance_Transfer(Finance_Transfer finfo)
        {
            if (finfo == null)
            {
                return false;
            }
            Finance_Transfer f = context.Finance_Transfer.FirstOrDefault((Finance_Transfer m) => m.Id == finfo.Id);
            if (f == null)
            {
                return false;
            }
            int i = 0;
            f.Trans_Number = finfo.Trans_Number;
            f.Trans_UserId = finfo.Trans_UserId;
            f.Trans_UserType = finfo.Trans_UserType;
            f.Trans_Money = finfo.Trans_Money;
            f.Trans_SXMoney = finfo.Trans_SXMoney;
            f.Trans_MoneyType = finfo.Trans_MoneyType;
            f.Trans_Time = finfo.Trans_Time;
            f.Trans_Address = finfo.Trans_Address;
            f.Trans_ToUserId = finfo.Trans_ToUserId;
            f.Trans_ToUserType = finfo.Trans_ToUserType;
            f.Trans_Status = finfo.Trans_Status;
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

        public bool AddFinance_Transfer(Finance_Transfer finfo)
        {
            int i = 0;
            if (finfo == null || finfo.Id != 0)
            {
                return false;
            }
            context.Finance_Transfer.Add(finfo);
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

        public PageModel<Finance_Transfer> GetFinance_TransferList_Statistics(QueryModel.Finance_TransferQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_Transfer> fp = context.Finance_Transfer.AsQueryable<Finance_Transfer>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.Trans_Time >= dt
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
                    where d.Trans_Time <= dt
                    select d;
                }
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.Trans_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            var qq = from p in fp.ToList()
                     group p by p.Trans_Time.ToString("yyyy-MM-dd") into g
                     select new Finance_Transfer()
                     {
                         Trans_Time = DateTime.Parse(g.Key),
                         Trans_MoneyType = g.Max(p => p.Trans_MoneyType),
                         Trans_Money = g.Sum(p => p.Trans_Money),
                         Trans_SXMoney = g.Sum(p => p.Trans_SXMoney)
                     };

            fp = qq.AsQueryable();

            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_Transfer> d) =>
                from o in d
                orderby o.Trans_Time descending
                select o);
            return new PageModel<Finance_Transfer>()
            {

                Models = fp,
                Total = num
            };
        }
    }
}
