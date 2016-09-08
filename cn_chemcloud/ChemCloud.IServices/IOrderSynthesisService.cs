using ChemCloud.Model.Common;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices
{

    public interface IOrderSynthesisService : IService, IDisposable
    {

        OrderSynthesis GetOrderSynthesis(long Id);

        OrderSynthesis AddOrderSynthesis(OrderSynthesis os);

        void UpdateOrderSynthesis(OrderSynthesis opos);

        void DelOrderInfo(long Id);

        PageModel<OrderSynthesis> GetOrderSynthesisList(OrderSynthesisQuery opQuery);

        Result_List<OrderSynthesis_Index> GetTopNumOrderSynthesis(int count);

        List<AttachmentInfo> GetAttachmentInfosById(long Id);

        List<AttachmentInfo> GetAttachmentInfosById(long Id,int type);

        bool AddAttachment(AttachmentInfo model);

        Result_List<OrderSynthesis_Index> GetHotSelling();

        bool UpdateAcceptCustomizedOrder(OrderSynthesis _OrderSynthesis);

        bool OrderSynthesis_DeliverGoods(OrderSynthesis _OrderSynthesis);
    }
}
