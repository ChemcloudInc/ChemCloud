using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;

namespace ChemCloud.IServices
{
    public interface IProductDescriptionsService : IService, IDisposable
    {

        ProductDescriptionInfo Get_ProductDescriptionInfo_Id(long id);
    }
}
