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
    public class Finance_InComeService : ServiceBase, IFinance_InComeService, IService, IDisposable
    {
        public Finance_InComeService()
        {

        }
        public Finance_InCome GetFinance_InComeInfo(long uid, int usertype, int moneytype)
        {
            Finance_InCome finfo = new Finance_InCome();
            finfo = (
            from p in context.Finance_InCome
            where p.InCome_UserId.Equals(uid) && p.InCome_UserType.Equals(usertype) && p.InCome_MoneyType.Equals(moneytype)
            select p).FirstOrDefault();
            return finfo;
        }

        public PageModel<Finance_InCome> GetFinance_InComeListInfo(QueryModel.Finance_InComeQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_InCome> fp = context.Finance_InCome.AsQueryable<Finance_InCome>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.InCome_StartTime >= dt
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
                    where d.InCome_StartTime <= dt
                    select d;
                }
            }
            if (fQuery.userid != 0)
            {//用户编号
                fp =
                    from d in fp
                    where d.InCome_UserId.Equals(fQuery.userid)
                    select d;
            }
            if (fQuery.usertype != 0)
            {//用户类型
                fp =
                    from d in fp
                    where d.InCome_UserType.Equals(fQuery.usertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.InCome_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_InCome> d) =>
                from o in d
                orderby o.InCome_StartTime descending
                select o);
            return new PageModel<Finance_InCome>()
            {

                Models = fp,
                Total = num
            };
        }

        public bool UpdateFinance_InCome(Finance_InCome fwinfo)
        {
            if (fwinfo == null)
            {
                return false;
            }
            Finance_InCome fw = context.Finance_InCome.FirstOrDefault((Finance_InCome m) => m.Id == fwinfo.Id);
            if (fw == null)
            {
                return false;
            }
            int i = 0;
            fw.InCome_Number = fwinfo.InCome_Number;
            fw.InCome_UserId = fwinfo.InCome_UserId;
            fw.InCome_UserType = fwinfo.InCome_UserType;
            fw.InCome_StartTime = fwinfo.InCome_StartTime;
            fw.InCome_EndTime = fwinfo.InCome_EndTime;
            fw.InCome_Money = fwinfo.InCome_Money;
            fw.InCome_MoneyType = fwinfo.InCome_MoneyType;
            fw.InCome_OrderNum = fwinfo.InCome_OrderNum;
            fw.InCome_Address = fwinfo.InCome_Address;
            fw.InCome_Type = fwinfo.InCome_Type;
            fw.InCome_Status = fwinfo.InCome_Status;
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

        public bool AddFinance_InCome(Finance_InCome fwinfo)
        {
            int i = 0;
            if (fwinfo == null || fwinfo.Id != 0)
            {
                return false;
            }
            context.Finance_InCome.Add(fwinfo);
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


        public PageModel<Finance_InCome> GetFinance_InComeList_Statistics(QueryModel.Finance_InComeQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_InCome> fp = context.Finance_InCome.AsQueryable<Finance_InCome>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.InCome_StartTime >= dt
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
                    where d.InCome_StartTime <= dt
                    select d;
                }
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.InCome_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }


            var qq = from p in fp.ToList()
                     group p by p.InCome_StartTime.ToString("yyyy-MM-dd") into g
                     select new Finance_InCome()
                     {
                         InCome_StartTime = DateTime.Parse(g.Key),
                         InCome_MoneyType = g.Max(p => p.InCome_MoneyType),
                         InCome_Money = g.Sum(p => p.InCome_Money)
                     };

            fp = qq.AsQueryable();

            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_InCome> d) =>
                from o in d
                orderby o.InCome_StartTime descending
                select o);
            return new PageModel<Finance_InCome>()
            {

                Models = fp,
                Total = num
            };
        }
    }
}
