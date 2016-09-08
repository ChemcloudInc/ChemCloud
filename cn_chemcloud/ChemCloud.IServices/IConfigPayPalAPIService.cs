using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices
{
    public interface IConfigPayPalAPIService : IService, IDisposable
    {
        ConfigPayPalAPI GetConfigPayPalAPIInfo(long id);

        void UpdateConfigPayPalAPIInfo(ConfigPayPalAPI configinfo);
    }
}
