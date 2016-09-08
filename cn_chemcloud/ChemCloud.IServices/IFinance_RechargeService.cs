using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IFinance_RechargeService : IService, IDisposable
    {
        Finance_Recharge GetFinance_RechargeInfo(long rechargeId);
        Finance_Recharge GetFinance_RechargeInfo(long uid, int usertype, int cointype);

        PageModel<Finance_Recharge> GetFinance_RechargeListInfo(Finance_RechargeQuery fQuery);

        bool UpdateFinance_Recharge(Finance_Recharge finfo);

        bool AddFinance_Recharge(Finance_Recharge finfo);
    }
}
