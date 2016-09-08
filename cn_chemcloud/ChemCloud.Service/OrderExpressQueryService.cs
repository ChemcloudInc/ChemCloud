using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Service
{
    public class OrderExpressQueryService : ServiceBase, IOrderExpressQueryService, IService, IDisposable
    {
        public OrderExpressQueryService() { }
        public Model.OrderExpressQuery GetOrderExpressById(string ShipOrderNumber)
        {
            return (from a in context.OrderExpressQuery where a.ShipOrderNumber==ShipOrderNumber select a ).FirstOrDefault<OrderExpressQuery>();
        }
    }
}
