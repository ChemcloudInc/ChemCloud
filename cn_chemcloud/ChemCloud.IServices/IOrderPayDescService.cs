using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface IOrderPayDescService : IService, IDisposable
    {
        void Add(OrderPayDesc model);

        OrderPayDesc GetInfo(long orderId,long uid,int userType);

        void Update(OrderPayDesc model);
    }
}
