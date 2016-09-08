using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Service
{
    public class CapitalUserAccountService : ServiceBase, ICapitalUserAccountService, IService, IDisposable
    {
        /// <summary>
        /// 根据memid查询
        /// </summary>
        /// <param name="memid"></param>
        /// <returns></returns>
        public CapitalUserAccount GetICapitalUserAccountInfo(long memid)
        {
            CapitalUserAccount cinfo = (
                from p in context.CapitalUserAccount
                where p.userid.Equals(memid)
                select p).FirstOrDefault();
            return cinfo;
        }

        /// <summary>
        /// add capital
        /// </summary>
        /// <param name="cma"></param>
        public void AddICapitalUserAccount(CapitalUserAccount cma)
        {
            if (cma == null)
            {
                return;
            }
            context.CapitalUserAccount.Add(cma);
            context.SaveChanges();
        }


        public void UPCapitalUserAccount(CapitalUserAccount cma)
        {
            CapitalUserAccount cainfo = context.CapitalUserAccount.FirstOrDefault((CapitalUserAccount m) => m.Id == cma.Id);
            if (cainfo!=null)
            {
                cainfo.userid = cma.userid;
                cainfo.CashAccount = cma.CashAccount;
                cainfo.CashAccountType = cma.CashAccountType;
            }
            context.SaveChanges();
        }
    }
}
