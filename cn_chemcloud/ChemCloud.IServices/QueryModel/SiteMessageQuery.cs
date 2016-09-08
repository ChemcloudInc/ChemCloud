using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices.QueryModel
{
    public class SiteMessageQuery : QueryBase
    {
        public long? Id { get; set; }
        public long? ShopId { get; set; }
        public SiteMessageQuery()
        {

        }
    }
    
}
