using ChemCloud.Model;
using System;
using System.Linq;

namespace ChemCloud.IServices
{
	public interface ITypeService : IService, IDisposable
	{
		void AddType(ProductTypeInfo model);

		void DeleteType(long id);

		ProductTypeInfo GetType(long id);

		PageModel<ProductTypeInfo> GetTypes(string search, int pageNo, int pageSize);

		IQueryable<ProductTypeInfo> GetTypes();

		void UpdateType(ProductTypeInfo model);
	}
}