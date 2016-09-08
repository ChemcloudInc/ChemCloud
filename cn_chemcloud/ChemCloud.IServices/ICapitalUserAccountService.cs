using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices
{
    public interface ICapitalUserAccountService : IService, IDisposable
    {
        CapitalUserAccount GetICapitalUserAccountInfo(long memid);

        void AddICapitalUserAccount(CapitalUserAccount cma);

        void UPCapitalUserAccount(CapitalUserAccount cma);
    }
}
