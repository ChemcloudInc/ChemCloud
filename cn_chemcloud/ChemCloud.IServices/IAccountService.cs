using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface IAccountService : IService, IDisposable
	{
		void ConfirmAccount(long id, string managerRemark);

		AccountInfo GetAccount(long id);

		PageModel<AccountDetailInfo> GetAccountDetails(AccountQuery query);

		PageModel<AccountMetaModel> GetAccountMeta(AccountQuery query);

		PageModel<AccountPurchaseAgreementInfo> GetAccountPurchaseAgreements(AccountQuery query);

		PageModel<AccountInfo> GetAccounts(AccountQuery query);
	}
}