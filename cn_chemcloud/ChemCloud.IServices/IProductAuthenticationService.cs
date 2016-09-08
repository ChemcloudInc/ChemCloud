using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface IProductAuthenticationService : IService, IDisposable
    {
        PageModel<ProductAuthentication> GetProductAuthenticationList(ProductAuthenticationQuery PAQuery);
        ProductAuthentication GetProductAuthenticationId(long Id);
        ProductAuthentication GetProductAuthenticationProductId(long ProductId);
        void AddProductAuthentication(ProductAuthentication PAInfo);
        void UpdateProductAuthentication(ProductAuthentication PAInfo);

    }
}
