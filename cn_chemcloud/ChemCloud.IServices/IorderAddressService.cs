using ChemCloud.Model;
using System;
using System.Linq;

namespace ChemCloud.IServices
{
	public interface IorderAddressService : IService, IDisposable
	{
		void AddorderAddress(orderAddressInfo shipinfo);

		void DeleteorderAddress(long id, long userId);

	orderAddressInfo GetDefaultUserorderAddressByUserId(long userId);

    orderAddressInfo GetUserorderAddress(long orderAddressId);

		IQueryable<orderAddressInfo> GetUserorderAddressByUserId(long userId);

		void SetDefaultorderAddress(long id, long userId);

		void SetQuickorderAddress(long id, long userId);

		void UpdateorderAddress(orderAddressInfo shipinfo);
	}
}