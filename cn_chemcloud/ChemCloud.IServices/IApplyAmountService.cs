using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model.Common;

namespace ChemCloud.IServices
{
    public interface IApplyAmountService : IService, IDisposable
    {
        ApplyAmountInfo GetApplyById(long Id);
        PageModel<ApplyAmountInfo> GetApplyAmounts(int? Status, DateTime? Start_datetime, DateTime? End_datetime, int page, int rows, long userId, int Applicant);
        ApplyAmountInfo GetApplyByOrderId(long userId, long orderId);
        bool AddApplyAmount(ApplyAmountInfo ApplyAmount);

        bool UpdateAuthor(long Id, long ParentId);
        bool UpdateApplyStatus(long Id, int status, long AuthorId);
        Result_Msg Update_Apply(QueryCommon<ApplyAmountInfo> query);

        bool Delete(long Id);

        bool BatchDelete(long[] Ids);

        bool IsPassAuth(long UserId, long OrderId);

        ApplyAmountInfo GetApplyByUserId(long USerId, long OrderId);
    }
}
