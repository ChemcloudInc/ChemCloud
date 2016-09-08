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
    public interface IBannersIndexService : IService, IDisposable
    {
        Result_List<Result_BannersIndex> GetBannerList_IndexPage(int languageType);
        Result_List_Pager<BannersIndex> GetBannerList(QueryCommon<BannerIndexQuery> query);
        Result_Model<PageInfo> Get_PageInfo(QueryCommon<BannerIndexQuery> query);

        Result_Msg DeleteById(int Id, long userId);

        Result_BannersIndex GetObjectById(int Id, long userId);

        Result_Msg BannerAdd(QueryCommon<BannerIndexQuery> query);

        Result_Msg ModifyBannerIndex(QueryCommon<BannerIndexQuery> query);

    }
}
