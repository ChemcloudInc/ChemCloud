using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface ISystemAgreementService : IService, IDisposable
	{
		void AddAgreement(AgreementInfo model);

		AgreementInfo GetAgreement(AgreementInfo.AgreementTypes type);

		bool UpdateAgreement(AgreementInfo model);
	}
}