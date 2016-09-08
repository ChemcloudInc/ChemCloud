using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class TKQuery : QueryBase
    {
        public int TKType { get; set; }

        public long OrderNo { get; set; }
    }
}
