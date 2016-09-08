using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface ITKService : IService, IDisposable
    {
        long InsertTK(TK tk);
        long InsertTKMessage(TKMessage tkm);
        void InsertTKImage(List<TKImageInfo> tkimages);
        void UpdateTK(long id, int type);

        PageModel<TK> getTkList(TKQuery tq, long UserId, long SellerUserId);
        List<TKMessage> getTKMessage(long tkid);
        List<TKImageInfo> getTKImage(long TKMessageId);
        TK getTK(long orderNo);
        bool DeleteTK(long OrderId);
    }
}
