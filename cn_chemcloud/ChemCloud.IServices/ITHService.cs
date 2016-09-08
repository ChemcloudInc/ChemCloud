using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface ITHService : IService, IDisposable
    {
        TH GetTHInfo(string thid);

        PageModel<TH> GetTHListInfo(THQuery fwQuery);

        Model.PageModel<TH> GetTHListInfo1(THQuery fQuery);

        bool UpdateTH(TH info);

        long AddTH(TH info);
        TH GetTHById(long id);

        PageModel<TH> GetTHPageModel(THQuery tq, long UserId, long SellerUserId);

        TH GetTHByOrderNum(long OrderNum);

        long InsertTHMessage(Model.THMessageInfo tkm);

        void InsertTHImage(List<Model.THImageInfo> tkimages);

        List<Model.THMessageInfo> getTHMessage(long tkid);

        List<Model.THImageInfo> getTHImage(long THMessageId);

        void UpdateTHStatus(long id, int THStatus);
        void SendTH(long id, string wldh, string wlgs);
    }
}
