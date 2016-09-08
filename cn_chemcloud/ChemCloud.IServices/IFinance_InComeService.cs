using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IFinance_InComeService : IService, IDisposable
    {
        /// <summary>
        /// 获取当前用户的收入信息
        /// </summary>
        /// <param name="uid">用户编号</param>
        /// <param name="usertype">用户类型</param>
        /// <param name="moneytype">币种类型</param>
        /// <returns></returns>
        Finance_InCome GetFinance_InComeInfo(long uid, int usertype, int moneytype);

        PageModel<Finance_InCome> GetFinance_InComeListInfo(Finance_InComeQuery fwQuery);

        bool UpdateFinance_InCome(Finance_InCome fwinfo);

        bool AddFinance_InCome(Finance_InCome fwinfo);

        PageModel<Finance_InCome> GetFinance_InComeList_Statistics(ChemCloud.QueryModel.Finance_InComeQuery fQuery);
    }
}
