using ChemCloud.Model.Models;
using Senparc.Weixin.MP.AdvancedAPIs.Poi;
using System;
using System.Collections.Generic;
using ChemCloud.Model;


namespace ChemCloud.IServices
{
    public interface IChemCloud_OrderWithCoaService : IService, IDisposable
    {
        void AddOrderWithCoa(ChemCloud_OrderWithCoa co);

        ChemCloud_OrderWithCoa GetChemCloud_OrderWithCoaByOrderid(long orderid);
    }
}
