using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices
{
    public interface IStatisticsMoneyService : IService, IDisposable
    {
        PageModel<StatisticsMoney> GetList(StatisticsQuery statisQuery);
        PageModel<OrderMoneyModel> GetList1(StatisticsQuery statisQuery);

        decimal GetAddMoney(StatisticsQuery smQuery);

        StatisticsMoney GetLastStatisticsMoneyByUid(long uid,int utype);
        decimal GetRemoveMoney(StatisticsQuery smQuery);

        void Add(StatisticsMoney sm);

        decimal GetMoney(StatisticsQuery smQuery);

        decimal getMyMoney(long uid,int userType);

        decimal GetMoneyByUidType(long uid, int userType);
    }
}
