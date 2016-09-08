using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface IMemberIntegralConversionFactoryService : IService, IDisposable
	{
		IConversionMemberIntegralBase Create(MemberIntegral.IntegralType type, int use = 0);
	}
}