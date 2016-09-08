using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices
{
    public interface IProductAnalysisService : IService, IDisposable
    {
        ProductAnalysis AddProductAnalysis(ProductAnalysis op);

        ProductAnalysis GetProductAnalysis(long Id);

        Model.PageModel<ProductAnalysis> GetProductAnalysisList(ProductAnalysis paQuery);

        bool Delete(long Id);

        bool UpdateAnalysisStatus(long Id, int status);

        bool UpdateAnalysisAddAttachment(long Id, string AnalysisAttachments);
    }
}
