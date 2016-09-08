using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IFinance_WalletService : IService, IDisposable
    {
        Finance_Wallet GetWalletInfo(long uid, int usertype, int cointype);

        PageModel<Finance_Wallet> GetFinance_WalletListInfo(Finance_WalletQuery fwQuery);

        bool UpdateFinance_Wallet(Finance_Wallet fwinfo);

        bool AddFinance_Wallet(Finance_Wallet fwinfo);

        List<Finance_Wallet> GetWalletList(long uid, int usertype);

        List<Finance_Wallet> GetWalletList(int cointype);

        /// <summary>
        /// 验证支付密码
        /// </summary>
        /// <param name="useid"></param>
        /// <param name="password"></param>
        /// <param name="cointype"></param>
        /// <returns></returns>
        bool CheckPaymentpassword(long useid, string password, int cointype);

        /// <summary>
        /// 判断当前用户 是否设置了支付密码
        /// </summary>
        /// <param name="useid"></param>
        /// <returns></returns>
        bool IsNullWalletPayPassword(long useid, int cointype);
    }
}
