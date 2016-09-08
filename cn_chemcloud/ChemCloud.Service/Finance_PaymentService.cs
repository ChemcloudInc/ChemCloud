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
    public class Finance_PaymentService : ServiceBase, IFinance_PaymentService, IService, IDisposable
    {
        public Finance_PaymentService()
        {

        }
        public Finance_Payment GetFinance_PaymentInfo(long uid, int usertype)
        {
            Finance_Payment finfo = new Finance_Payment();
            finfo = (
            from p in context.Finance_Payment
            where p.PayMent_UserId.Equals(uid) && p.PayMent_UserType.Equals(usertype)
            select p).FirstOrDefault();
            return finfo;
        }

        public PageModel<Finance_Payment> GetFinance_PaymentListInfo(QueryModel.Finance_PaymentQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_Payment> fp = context.Finance_Payment.AsQueryable<Finance_Payment>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.PayMent_PayTime >= dt
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
                    where d.PayMent_PayTime <= dt
                    select d;
                }
            }
            if (fQuery.userid != 0)
            {//用户编号
                fp =
                    from d in fp
                    where d.PayMent_UserId.Equals(fQuery.userid)
                    select d;
            }
            if (fQuery.usertype != 0)
            {//用户类型
                fp =
                    from d in fp
                    where d.PayMent_UserType.Equals(fQuery.usertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.PayMent_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_Payment> d) =>
                from o in d
                orderby o.PayMent_PayTime descending
                select o);
            return new PageModel<Finance_Payment>()
            {

                Models = fp,
                Total = num
            };
        }

        public bool UpdateFinance_Payment(Finance_Payment finfo)
        {
            if (finfo == null)
            {
                return false;
            }
            Finance_Payment fw = context.Finance_Payment.FirstOrDefault((Finance_Payment m) => m.Id == finfo.Id);
            if (fw == null)
            {
                return false;
            }
            int i = 0;
            fw.PayMent_Number = finfo.PayMent_Number;
            fw.PayMent_UserId = finfo.PayMent_UserId;
            fw.PayMent_UserType = finfo.PayMent_UserType;
            fw.PayMent_OrderNum = finfo.PayMent_OrderNum;
            fw.PayMent_PayTime = finfo.PayMent_PayTime;
            fw.PayMent_PayMoney = finfo.PayMent_PayMoney;
            fw.PayMent_TotalMoney = finfo.PayMent_TotalMoney;
            fw.PayMent_BXMoney = finfo.PayMent_BXMoney;
            fw.PayMent_YFMoney = finfo.PayMent_YFMoney;
            fw.PayMent_JYMoney = finfo.PayMent_JYMoney;
            fw.PayMent_SXMoney = finfo.PayMent_SXMoney;
            fw.PayMent_PayAddress = finfo.PayMent_PayAddress;
            fw.PayMent_MoneyType = finfo.PayMent_MoneyType;
            fw.PayMent_Status = finfo.PayMent_Status;
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

        public bool AddFinance_Payment(Finance_Payment finfo)
        {
            int i = 0;
            if (finfo == null || finfo.Id != 0)
            {
                return false;
            }
            context.Finance_Payment.Add(finfo);
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
        /// 平台支付信息统计  [chenqi 2016-03-29 17:06]
        /// </summary>
        /// <param name="fQuery"></param>
        /// <returns></returns>
        public PageModel<Finance_Payment> GetFinance_PaymentList_Statistics(QueryModel.Finance_PaymentQuery fQuery)
        {

            int num = 0;

            IQueryable<Finance_Payment> fp = context.Finance_Payment.AsQueryable<Finance_Payment>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.PayMent_PayTime >= dt
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
                    where d.PayMent_PayTime <= dt
                    select d;
                }
            }

            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.PayMent_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }

            var qq = from p in fp.ToList()
                     group p by p.PayMent_PayTime.ToString("yyyy-MM-dd") into g
                     select new Finance_Payment()
                     {
                         PayMent_PayTime = DateTime.Parse(g.Key),
                         PayMent_MoneyType = g.Max(p => p.PayMent_MoneyType),
                         PayMent_PayMoney = g.Sum(p => p.PayMent_PayMoney),

                         PayMent_SXMoney = g.Sum(p => p.PayMent_SXMoney),
                         PayMent_YFMoney = g.Sum(p => p.PayMent_YFMoney),
                         PayMent_BXMoney = g.Sum(p => p.PayMent_BXMoney),
                         PayMent_JYMoney = g.Sum(p => p.PayMent_JYMoney)
                     };

            fp = qq.AsQueryable();

            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_Payment> d) =>
                from o in d
                orderby o.PayMent_PayTime descending
                select o);
            return new PageModel<Finance_Payment>()
            {

                Models = fp,
                Total = num
            };
        }
    }
}
