using ChemCloud.Model;
using ChemCloud.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices
{
    public interface IManagersAccountService : IService, IDisposable
    {
        ManagersAccount GetManagersAccountByManagerId(long managerId);

        PageModel<ManagersAccount> GetManagersAccountList(ManagersAccountQuery manQuery);

        void AddManagersAccountInfo(ManagersAccount maccount);

    }
}
