using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IFinance_RefundService : IService, IDisposable
    {
        Finance_Refund GetFinance_RefundInfo(long frid);

        PageModel<Finance_Refund> GetFinance_RefundListInfo(Finance_RefundQuery fQuery);

        PageModel<Finance_Refund> GetFinance_RefundListInfo1(Finance_RefundQuery fQuery);

        bool UpdateFinance_Refund(Finance_Refund finfo);

        bool AddFinance_Refund(Finance_Refund finfo);

        PageModel<Finance_Refund> GetFinance_RefundList_Statistics(Finance_RefundQuery fQuery);
    }
}
