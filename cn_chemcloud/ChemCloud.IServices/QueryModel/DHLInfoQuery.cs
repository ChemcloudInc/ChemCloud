using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class DHLInfoQuery : QueryBase
    {
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public long UserId { get; set; }

        public long Number { get; set; }

        public int DocuType { get; set; }
        public DHLInfoQuery()
        {

        }
    }
}
