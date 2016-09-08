using ChemCloud.Model;
using System;
using System.Linq;

namespace ChemCloud.IServices
{
	public interface IProductDescriptionTemplateService : IService, IDisposable
	{
		void AddTemplate(ProductDescriptionTemplateInfo template);

		void DeleteTemplate(long shopId, params long[] ids);

		ProductDescriptionTemplateInfo GetTemplate(long id, long shopId);

		PageModel<ProductDescriptionTemplateInfo> GetTemplates(long shopId, int pageNumber, int pageSize, string name = null, ProductDescriptionTemplateInfo.TemplatePosition? position = null);

		IQueryable<ProductDescriptionTemplateInfo> GetTemplates(long shopId);

		void UpdateTemplate(ProductDescriptionTemplateInfo template);
	}
}