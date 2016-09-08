using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IFinance_WithDrawService : IService, IDisposable
    {
        Finance_WithDraw GetFinance_WithDrawInfo(long uid, int usertype);

        PageModel<Finance_WithDraw> GetFinance_WithDrawListInfo(Finance_WithDrawQuery fQuery);

        bool UpdateFinance_WithDraw(Finance_WithDraw finfo);

        bool AddFinance_WithDraw(Finance_WithDraw finfo);

        PageModel<Finance_WithDraw> GetFinance_WithDrawList_Statistics(ChemCloud.QueryModel.Finance_WithDrawQuery fQuery);

        Finance_WithDraw GetFinance_WithDrawInfo(long id);
    }
}
