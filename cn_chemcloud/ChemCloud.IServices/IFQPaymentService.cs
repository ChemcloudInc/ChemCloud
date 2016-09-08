using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IFQPaymentService : IService, IDisposable
    {
        FQPayment GetFQPaymentInfo(long orderId,long uid);

        PageModel<FQPayment> GetFQPaymentListInfo(FQPaymentQuery fqQuery);

        bool UpdateFQPayment(FQPayment fqinfo);

        bool AddFQPayment(FQPayment fqinfo);
    }
}
