using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IFinance_PaymentService : IService, IDisposable
    {
        Finance_Payment GetFinance_PaymentInfo(long uid, int usertype);

        PageModel<Finance_Payment> GetFinance_PaymentListInfo(Finance_PaymentQuery fwQuery);

        bool UpdateFinance_Payment(Finance_Payment fwinfo);

        bool AddFinance_Payment(Finance_Payment fwinfo);

        PageModel<Finance_Payment> GetFinance_PaymentList_Statistics(Finance_PaymentQuery fwQuery);
    }
}
