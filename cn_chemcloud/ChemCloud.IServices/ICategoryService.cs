using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
	public interface ICategoryService : IService, IDisposable
	{
		void AddCategory(CategoryInfo model);

		void DeleteCategory(long id);

		IQueryable<CategoryInfo> GetCategories();

		CategoryInfo GetCategory(long id);

        CategoryInfo GetCategoryByName(string name);

		IEnumerable<CategoryInfo> GetCategoryByParentId(long id);

		string GetEffectCategoryName(long shopId, long typeId);

		IEnumerable<CategoryInfo> GetFirstAndSecondLevelCategories();

		IEnumerable<CategoryInfo> GetMainCategory();

		long GetMaxCategoryId();

		IEnumerable<CategoryInfo> GetSecondAndThirdLevelCategories(params long[] ids);

		IEnumerable<CategoryInfo> GetTopLevelCategories(IEnumerable<long> categoryIds);

		IEnumerable<CategoryInfo> GetValidBusinessCategoryByParentId(long id);

		void UpdateCategory(CategoryInfo model);

		void UpdateCategoryDisplaySequence(long id, long displaySequence);

		void UpdateCategoryName(long id, string name);

        IEnumerable<CategoryInfo> GetTopLevelCategoriesNew(IEnumerable<long> categoryIds);
	}
}