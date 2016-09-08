using ChemCloud.Model;
using ChemCloud.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices
{
    public interface IOrderPurchasingService : IService, IDisposable
    {
        OrderPurchasing GetOrderPurchasing(long Id);
        OrderPurchasing AddOrderPurchasing(OrderPurchasing op);

        void UpdateOrderPurchasing(OrderPurchasing op);
        void DelOrderInfo(long Id);

        PageModel<OrderPurchasing> GetOrderPurchasingList(OrderPurchasingQuery opQuery);

        List<AttachmentInfo> GetAttachmentInfosById(long Id);

        bool AddAttachment(AttachmentInfo model);

        bool OrderPurchasing_DeliverGoods(OrderPurchasing _OrderPurchasing);

        bool Accep_OrderPurchasing(OrderPurchasing _OrderPurchasing);
    }
}
