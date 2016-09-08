using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class ManagersAccountQuery : QueryBase
    {
        public ManagersAccountQuery() { }

        public long managerId { get; set; }
        public string startTime { get; set; }

        public string endTime { get; set; }
    }
}
