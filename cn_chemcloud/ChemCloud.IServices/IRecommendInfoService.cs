using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IRecommendInfoService : IService,IDisposable
    {
        bool AddRecommendInfo(RecommendInfo model);

        bool UpdateRecommendInfo(long id,decimal price);

        bool DeleteRecommendInfo(long id);

        bool BatchDelete(long[] ids);

        RecommendInfo GetRecommendInfo(long id);

        RecommendInfo GetRecommendInfoByCID(long cid);

        PageModel<RecommendInfo> GetRecommendInfos(RecommendInfoQuery model);

        bool UpdateRecommendInfoStatus(long Id, int status, long userId);

        bool BatchUpdateRecommendInfoStatus(long[] ids, int status, long userId);

        bool BatchAddRecommendInfo(List<RecommendInfo> modelList);

        List<RecommendInfo> GetRecommendByStatus();

        List<ProductInfo> GetRecommendInfosLikePlatCode(string code);
    }
}
