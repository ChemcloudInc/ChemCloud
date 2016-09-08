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
    public class Finance_RechargeService : ServiceBase, IFinance_RechargeService, IService, IDisposable
    {
        public Finance_RechargeService()
        {

        }
        public Finance_Recharge GetFinance_RechargeInfo(long rechargeId) {
            Finance_Recharge finfo = new Finance_Recharge();
            finfo = (
            from p in context.Finance_Recharge
            where p.Recharge_Number.Equals(rechargeId)
            select p).FirstOrDefault();
            return finfo;
        }
        public Finance_Recharge GetFinance_RechargeInfo(long uid, int usertype, int cointype)
        {
            Finance_Recharge finfo = new Finance_Recharge();
            finfo = (
            from p in context.Finance_Recharge
            where p.Recharge_UserId.Equals(uid) && p.Recharge_UserType.Equals(usertype) && p.Recharge_MoneyType.Equals(cointype)
            select p).FirstOrDefault();
            return finfo;
        }

        public PageModel<Finance_Recharge> GetFinance_RechargeListInfo(QueryModel.Finance_RechargeQuery fQuery)
        {
            int num = 0;
            IQueryable<Finance_Recharge> fp = context.Finance_Recharge.AsQueryable<Finance_Recharge>();
            if (!string.IsNullOrWhiteSpace(fQuery.starttime))
            {//开始时间
                DateTime dt;
                if (DateTime.TryParse(fQuery.starttime, out dt))
                {
                    dt = DateTime.Parse(fQuery.starttime);
                    fp =
                    from d in fp
                    where d.Recharge_Time >= dt
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
                    where d.Recharge_Time <= dt
                    select d;
                }
            }
            if (fQuery.userid != 0)
            {//用户编号
                fp =
                    from d in fp
                    where d.Recharge_UserId.Equals(fQuery.userid)
                    select d;
            }
            if (fQuery.usertype != 0)
            {//用户类型
                fp =
                    from d in fp
                    where d.Recharge_UserType.Equals(fQuery.usertype)
                    select d;
            }
            if (fQuery.moneytype != 0)
            {//支付币种
                fp =
                    from d in fp
                    where d.Recharge_MoneyType.Equals(fQuery.moneytype)
                    select d;
            }
            fp = fp.GetPage(out num, fQuery.PageNo, fQuery.PageSize, (IQueryable<Finance_Recharge> d) =>
                from o in d
                orderby o.Recharge_Time descending
                select o);
            return new PageModel<Finance_Recharge>()
            {

                Models = fp,
                Total = num
            };
        }

        public bool UpdateFinance_Recharge(Finance_Recharge finfo)
        {
            if (finfo == null)
            {
                return false;
            }
            Finance_Recharge fw = context.Finance_Recharge.FirstOrDefault((Finance_Recharge m) => m.Id == finfo.Id);
            if (fw == null)
            {
                return false;
            }
            int i = 0;
            fw.Recharge_Number = finfo.Recharge_Number;
            fw.Recharge_UserId = finfo.Recharge_UserId;
            fw.Recharge_UserType = finfo.Recharge_UserType;
            fw.Recharge_Time = finfo.Recharge_Time;
            fw.Recharge_Address = finfo.Recharge_Address;
            fw.Recharge_Money = finfo.Recharge_Money;
            fw.Recharge_MoneyLeft = finfo.Recharge_MoneyLeft;
            fw.Recharge_MoneyType = finfo.Recharge_MoneyType;
            fw.Recharge_Type = finfo.Recharge_Type;
            fw.Recharge_Status = finfo.Recharge_Status;
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

        public bool AddFinance_Recharge(Finance_Recharge finfo)
        {
            int i = 0;
            if (finfo == null || finfo.Id != 0)
            {
                return false;
            }
            context.Finance_Recharge.Add(finfo);
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
    }
}
