using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemColud.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface IMargainBillService : IService, IDisposable
    {
        void AddBill(MargainBill model);

        void UpdateBill(MargainBill model);

        void UpdateBillStatu(long Id);

        void UpdateBillStatu(long Id, EnumBillStatus status);

        MargainBill GetBillById(long id, long CurrentUserid);

        PageModel<MargainBill> GetBill(BargainBillQuery billQueryModel);

        void TransferOrder(long MargainBillId); //议价单转订单

        MargainBill GetBillByNo(string BillNo);

        PageModel<MargainBill> GetBargain<Tout>(BargainQuery _MargainBill);

        void UpdateBargainPrice(long BargainDId, decimal freight);

        void SellerCloseBargain(string bargainno);

        void SellerBatchCloseBargain(string bargainno);

        void AddMargainDetail(MargainBillDetail model);//添加议价记录  chenq

        void UpdateMargainDetailMessageReply(MargainBillDetail model);
        void UpdateMargainDetailBuyerMessage(MargainBillDetail model);

        void OverBargain(long Id);//结束议价 chenq

        void SubmitOrder(MargainBill model);//下单

        void SubmitOrder(MargainBill model,ShipmentEx ship);

        void SubmitOrderBatch(MargainBill margainbill);

        void RemoveCartAfterOrder(long userid, long productid);

        bool CartSubmitOrder(List<MargainBill> margainbill);
    }
}
