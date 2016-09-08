using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class CAS_MAIN_INFOQuery : QueryBase
    {
        public string CAS_NO { get; set; }

        public int OrderBy { get; set; }

        public CAS_MAIN_INFO.CAS_Status? Status { get; set; }

        public string type
        {
            get;
            set;
        }
        public string CAS_NUMBER { get; set; }

        public string CAS_NUMBERList { get; set; }

        public CAS_MAIN_INFOQuery() { }
    }
}
