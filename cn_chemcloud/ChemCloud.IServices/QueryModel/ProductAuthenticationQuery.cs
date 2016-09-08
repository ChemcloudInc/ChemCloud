using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class ProductAuthenticationQuery : QueryBase
    {
        public ProductAuthenticationQuery() { }

        public string ProductCode { get; set; }
        public string AuthStatus { get; set; }
        public long ManageId { get; set; }

        public string ComName { get; set; }
    }
}
