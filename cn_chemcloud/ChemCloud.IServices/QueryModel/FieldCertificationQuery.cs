using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices.QueryModel
{
    public class FieldCertificationQuery : QueryBase
    {

        public long? Id { get; set; }
        public int OrderBy
        { get; set; }
        public FieldCertification.CertificationStatus? Status
        {
            get;
            set;
        }
        public string CompanyName { get; set; } 
        public FieldCertificationQuery()
        {

        }
    }
}
