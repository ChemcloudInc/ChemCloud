using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace ChemCloud.Service
{
    public class Finance_WalletService : ServiceBase, IFinance_WalletService, IService, IDisposable
    {
        public Finance_WalletService()
        {

        }
        public Finance_Wallet GetWalletInfo(long uid, int usertype, int cointype)
        {
            Finance_Wallet finfo = new Finance_Wallet();
            finfo = (
            from p in context.Finance_Wallet
            where p.Wallet_UserId.Equals(uid) && p.Wallet_UserType.Equals(usertype) && p.Wallet_MoneyType.Equals(cointype)
            select p).FirstOrDefault();
            return finfo;
        }

        public PageModel<Finance_Wallet> GetFinance_WalletListInfo(QueryModel.Finance_WalletQuery fwQuery)
        {
            throw new NotImplementedException();
        }

        public bool UpdateFinance_Wallet(Finance_Wallet fwinfo)
        {
            if (fwinfo == null)
            {
                return false;
            }
            Finance_Wallet fw = context.Finance_Wallet.FirstOrDefault((Finance_Wallet m) => m.Id == fwinfo.Id);
            if (fw == null)
            {
                return false;
            }
            int i = 0;
            fw.Wallet_UserId = fwinfo.Wallet_UserId;
            fw.Wallet_UserType = fwinfo.Wallet_UserType;
            fw.Wallet_UserMoney = fwinfo.Wallet_UserMoney;
            fw.Wallet_UserMoneyLock = fwinfo.Wallet_UserMoneyLock;
            fw.Wallet_UserLeftMoney = fwinfo.Wallet_UserLeftMoney;
            fw.Wallet_UserBankName = fwinfo.Wallet_UserBankName;
            fw.Wallet_UserBankNumber = fwinfo.Wallet_UserBankNumber;
            fw.Wallet_UserBankUserName = fwinfo.Wallet_UserBankUserName;
            fw.Wallet_UserBankAddress = fwinfo.Wallet_UserBankAddress;
            fw.Wallet_DoLastTime = fwinfo.Wallet_DoLastTime;
            fw.Wallet_DoIpAddress = fwinfo.Wallet_DoIpAddress;
            fw.Wallet_DoUserName = fwinfo.Wallet_DoUserName;
            fw.Wallet_DoUserId = fwinfo.Wallet_DoUserId;
            fw.Wallet_MoneyType = fwinfo.Wallet_MoneyType;
            fw.Wallet_PayPassword = fwinfo.Wallet_PayPassword;
            fw.Wallet_Status = fwinfo.Wallet_Status;
            i = context.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddFinance_Wallet(Finance_Wallet fwinfo)
        {
            int i = 0;
            if (fwinfo == null || fwinfo.Id != 0)
            {
                return false;
            }
            context.Finance_Wallet.Add(fwinfo);
            i = context.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Finance_Wallet> GetWalletList(long uid, int usertype)
        {
            return (from a in context.Finance_Wallet where a.Wallet_UserId == uid && a.Wallet_UserType == usertype select a).ToList<Finance_Wallet>();
        }

        public List<Finance_Wallet> GetWalletList(int cointype)
        {
            return (from a in context.Finance_Wallet where a.Wallet_Status.Equals(1) && a.Wallet_MoneyType.Equals(cointype) select a).ToList<Finance_Wallet>();
        }

        /// <summary>
        /// 验证支付密码
        /// </summary>
        /// <param name="useid"></param>
        /// <param name="password"></param>
        /// <param name="cointype"></param>
        /// <returns></returns>
        public bool CheckPaymentpassword(long useid, string password, int cointype)
        {
            bool result = false;
            Finance_Wallet finfo = (
            from p in context.Finance_Wallet
            where p.Wallet_UserId.Equals(useid) && p.Wallet_PayPassword.Equals(password) && p.Wallet_MoneyType.Equals(cointype)
            select p).FirstOrDefault();
            if (finfo != null) { result = true; }
            return result;
        }

        /// <summary>
        /// 判断当前用户 是否设置了支付密码
        /// </summary>
        /// <param name="useid"></param>
        /// <returns></returns>
        public bool IsNullWalletPayPassword(long useid, int cointype)
        {
            bool result = false;
            Finance_Wallet finfo = (
            from p in context.Finance_Wallet
            where p.Wallet_UserId.Equals(useid) && p.Wallet_MoneyType.Equals(cointype)
            select p).FirstOrDefault();
            if (finfo != null && !string.IsNullOrEmpty(finfo.Wallet_PayPassword)) { result = true; }
            return result;
        }
    }
}
