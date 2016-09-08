using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface IFloorProductService : IService, IDisposable
    {
        List<FloorProductInfo> GetProducts(int tab);
    }
}
