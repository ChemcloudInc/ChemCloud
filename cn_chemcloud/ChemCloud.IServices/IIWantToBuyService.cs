using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChemCloud.Model;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model.Common;

namespace ChemCloud.IServices
{
    public interface IIWantToBuyService : IService, IDisposable
    {

        #region Admin 后台
        Result_Msg DeleteById_Admin(int Id);
        Result_IWantToBuy GetObjectById_Admin(int Id);
        Result_Msg UpdateSupply_Fast(IWantToBuyModifyQuery query);
        Result_List_Pager<IWantToBuy> GetIWantToBuyList_Admin(QueryCommon<IWantToBuyQuery> query);
        Result_Model<PageInfo> Get_PageInfo_Admin(QueryCommon<IWantToBuyQuery> query);

        #endregion

        #region Web 后台

        Result_Model<IWantToBuy_Orders> AddIWantToBuyOrder(IWantToSupply query);
        Result_Model<IWantToBuy_Orders> GetOrders_ByPurchaseNum(long purchaseNum);
        Result_Msg DeleteById(int Id, long userId);
        IWantToBuy GetObjectById(int Id, long userId);
        Result_List_Pager<IWantToBuy> IWantToBuyList_User(QueryCommon<IWantToBuyQuery> query);
        Result_Model<PageInfo> Get_PageInfo_User(QueryCommon<IWantToBuyQuery> query);

        #region 供应商后台
        Result_Model<PageInfo> Get_PageInfo_IWantToSupply_List(QueryCommon<IWantToBuyQuery> query);
        Result_List_Pager<IWantToBuy> GetIWantToBuyList_SupplyUser_Pager(QueryCommon<IWantToBuyQuery> query);
        Result_Model<PageInfo> Get_PageInfo_IWantToBuyList_SupplyUser(QueryCommon<IWantToBuyQuery> query);
        Result_Model<IWantToSupply> Get_SupplyModel_ByIWantToBuyID(long userId);
        Result_Model<IWantToSupply> Get_SupplyModel_ByUserIdAndIWantToBuyId(long iWantToBuyId, long userId);
        
        #endregion

        #endregion

        #region Web 前台展示


        #region 采购商
        Result_List_Pager<IWantToBuy> Get_IWantToBuyList_Web_Buy(QueryCommon<IWantToBuyQuery> query);
        Result_Model<PageInfo> Get_PageInfo_Web_Buy(QueryCommon<IWantToBuyQuery> query);
        Result_Msg IWantToBuy_Add_Web(QueryCommon<IWantToBuyQuery> query);
        Result_Msg UpdateSupply_Status(QueryCommon<IWantToSupplyQuery> query);
        Result_Model<IWantToSupply> MaxStatusSupply_By_Num(long purchaseNum);
        Result_Msg UpdateBuy_Status(QueryCommon<IWantToBuyQuery> query);
        Result_Msg UpdateOrders_Status(QueryCommon<IWantToBuy_Orders> query);
        #endregion


        #region 供应商

        Result_Msg AddIWantToSupply(QueryCommon<IWantToSupplyQuery> query);
        Result_Msg UpdateSupply(QueryCommon<IWantToSupplyQuery> query);
        Result_Model<PageInfo> Get_PageInfo_Web_Supply(QueryCommon<IWantToBuyQuery> query);

        Result_Msg UpdateOrders(QueryCommon<IWantToBuy_Orders> query);

        #endregion


        Result_Model<IWantToBuy> GetObjectById_Web(long Id);
        Result_Model<IWantToBuy> GetObjectById_Web_Buy(long Id);
        Result_Model<IWantToSupply> GetObjectById_Web_Supply(long iWantToBuy, long supplierId);
        Result_Model<IWantToBuy_Orders> GetObjectById_Web_Supply_DeliverGoods(long supplyId);
        Result_Model<IWantToSupply> GetObjectById_Supply(long id);
        Result_List<IWantToSupply> GetObjectById_Web_SupplyList(long iWantToBuyId);
        Result_List_Pager<IWantToSupply> GetObjectById_Web_SupplyList_Pager(QueryCommon<IWantToBuyQuery> query);
        Result_List<Result_Model<IWantToBuy>> Get_PreNext_ById_Web(long id);
        Result_List_Pager<IWantToBuy> Get_IWantToBuyList_Web_Supply(QueryCommon<IWantToBuyQuery> query);


        #endregion

        #region Web、Admin 公共方法

        Result_Model<ShopInfo> GetCompanyInfo_ByUserIdAndUserType(long userId);



        Result_Msg IWantToBuy_Update(QueryCommon<IWantToBuyQuery> query);
        Result_Msg UpdateSupply_Admin(QueryCommon<IWantToBuyQuery> query);

        #endregion

    }
}
