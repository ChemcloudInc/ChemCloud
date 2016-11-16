using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;

namespace ChemCloud.IServices
{
    public interface ISiteMessagesService : IService, IDisposable
    {
        PageModel<SiteMessages> GetMessages(SiteMessagesQuery messageQueryModel);
        bool UpdateStatus(long Id, SiteMessages.Status status);

        bool AddMessage(SiteMessages setting);

        SiteMessages GetMessage(long Id);

        List<long> GetMemberIds(int receType);

        bool SendOrderCreatedMessage(long UserId, DateTime OrderCreatTime, string OrderNo);
        bool SendOrderPayMessage(long UserId, string OrderNo, decimal OrderMoney);
        bool SendOrderShippingMessage(long UserId, string OrderNo);
        bool SendOrderRefundMessage(long UserId, string OrderNo, string SendUserName);
        bool SendShopAuditedMessage(long UserId);
        bool SendShopResultMessage(long UserId);
        bool SendCertificationApplyMessage(long UserId);
        bool SendConfirmPayMessage(long UserId);
        bool SendCertificationResultMessage(long UserId);
        bool SendLogisticsReceMessage(long UserId, string OrderNo);
        bool SendLogisticsClearanceMessage(long UserId, string OrderNo);
        bool SendLogisticsSignInMessage(long UserId, string OrderNo);
        bool SendMemberRegisterMessage(long UserId);
        bool SendSupplierRegisterMessage(long UserId);
        bool SendGoodsPaymentMessage(long UserId);

        bool SendXunPanMessage(long UserId);

        List<SiteMessages> GetMessage(long? UserId);
        void UpdateMessage(SiteMessages model);

        bool SendSiteMessages(long Receid, int MessageModule, string MessageContent, string SendName);

        int GetMessageCount(long MemberId, int TypeId);
        int GetMessageCount1(long MemberId, int TypeId);

        bool DeleteMessage(long MessageId);

        bool BatchDeleteMessages(long[] MessageIds);

        void UpdateIsDisplay(long MemberId, int TypeId);

        IQueryable<QueryMember> GetMangers(int receType, string Search);

        PageModel<SiteMessages> PlatformGetMessages(SiteMessagesQuery messageQueryModel);

        bool SendLimitedAmountMessage(long UserId);

        bool SendApplyPassMessage(long UserId);

        bool SendApplyNoPassMessage(long UserId);

        void SendMessage(MessageDetial md, long userId, string[] urls);
    }
}
