using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices
{
    public interface IOrderExpressQueryService : IService, IDisposable
    {
        OrderExpressQuery GetOrderExpressById(string ShipOrderNumber);
    }
}
