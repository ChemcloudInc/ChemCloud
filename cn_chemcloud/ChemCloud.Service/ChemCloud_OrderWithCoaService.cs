using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class ChemCloud_OrderWithCoaService : ServiceBase, IChemCloud_OrderWithCoaService, IService, IDisposable
    {
        public void AddOrderWithCoa(ChemCloud_OrderWithCoa co)
        {
            if (co == null)
            {
                return;
            }
            context.ChemCloud_OrderWithCoa.Add(co);
            context.SaveChanges();
        }

        public ChemCloud_OrderWithCoa GetChemCloud_OrderWithCoaByOrderid(long orderid)
        {
            return (from p in context.ChemCloud_OrderWithCoa where p.OrderId == orderid select p).FirstOrDefault();
        }
    }
}
