using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IFinance_TransferService : IService, IDisposable
    {
        Finance_Transfer GetFinance_TransferInfo(long uid, int usertype);

        PageModel<Finance_Transfer> GetFinance_TransferListInfo(Finance_TransferQuery fQuery);

        bool UpdateFinance_Transfer(Finance_Transfer finfo);

        bool AddFinance_Transfer(Finance_Transfer finfo);
        PageModel<Finance_Transfer> GetFinance_TransferList_Statistics(ChemCloud.QueryModel.Finance_TransferQuery fQuery);
    }
}
