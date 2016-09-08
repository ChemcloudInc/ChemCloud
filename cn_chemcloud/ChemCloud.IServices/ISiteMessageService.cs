using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Core.Plugins.Message;

namespace ChemCloud.IServices
{
    public interface ISiteMessageService : IService, IDisposable
    {
        
        void SendMessageOnFindPassWord(SiteMessage info);

        void SendMessageOnOrderCreate(SiteMessage info);

        void SendMessageOnOrderPay(SiteMessage info);

        void SendMessageOnOrderRefund(SiteMessage info);

        void SendMessageOnOrderShipping(SiteMessage info);

        void SendMessageOnShopAudited(SiteMessage info);

        void SendMessageOnShopSuccess(SiteMessage info);
        void SendMessageOnFinishCertification(SiteMessage info);
        void SendMessageOnReceCertification(SiteMessage info);

        void SendMessageOnStartCertification(SiteMessage info);
      
        void UpdateMemberContacts(MemberContactsInfo info);
    }
}
