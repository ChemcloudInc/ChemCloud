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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace ChemCloud.Service
{
    public class ManagersAccountService : ServiceBase, IManagersAccountService, IService, IDisposable
    {
        /// <summary>
        /// 根据用户编号查询详情
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public ManagersAccount GetManagersAccountByManagerId(long managerId)
        {
            ManagersAccount maccount = (
                from p in context.ManagersAccount
                where p.ManagerId.Equals(managerId)
                select p).FirstOrDefault();
            return maccount;
        }

        /// <summary>
        /// 列表查询 带条件
        /// </summary>
        /// <param name="manQuery"></param>
        /// <returns></returns>
        public PageModel<ManagersAccount> GetManagersAccountList(ManagersAccountQuery manQuery)
        {
            int num = 0;

            IQueryable<ManagersAccount> manaccount = context.ManagersAccount.AsQueryable<ManagersAccount>();
            if (!string.IsNullOrWhiteSpace(manQuery.startTime.ToString()))
            {
                DateTime i;
                if (DateTime.TryParse(manQuery.startTime, out i))
                {
                    DateTime dt = DateTime.Parse(manQuery.startTime);
                    manaccount =
                    from d in manaccount
                    where d.Datatime >= dt
                    orderby d.Datatime descending
                    select d;
                }

            }
            if (!string.IsNullOrWhiteSpace(manQuery.endTime.ToString()))
            {
                DateTime i;
                if (DateTime.TryParse(manQuery.endTime, out i))
                {
                    DateTime dts = DateTime.Parse(manQuery.endTime);
                    manaccount =
                        from d in manaccount
                        where d.Datatime <= dts
                        orderby d.Datatime descending
                        select d;
                }
            }
            if (manQuery.managerId != 0)
            {
                manaccount =
                    from d in manaccount
                    where d.ManagerId == manQuery.managerId
                    orderby d.Datatime descending
                    select d;
            }
            manaccount = manaccount.GetPage(out num, manQuery.PageNo, manQuery.PageSize, (IQueryable<ManagersAccount> d) =>
                from o in d
                orderby o.Datatime descending
                select o);
            return new PageModel<ManagersAccount>()
            {

                Models = manaccount,
                Total = num
            };
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="maccount"></param>
        public void AddManagersAccountInfo(ManagersAccount maccount)
        {
            if (maccount == null)
            {
                return;
            }
            context.ManagersAccount.Add(maccount);
            context.SaveChanges();
        }
    }
}
