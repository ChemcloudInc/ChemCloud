using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices.QueryModel
{
    public class MemberDetailQuery : QueryBase
    {
        public long Id { get; set; }

        public long MemberId { get; set; }

        public string CompanyName { get; set; }

        public string MemberName { get; set; }
        public MemberDetailQuery()
        {

        }
    }

    public class MemberDetailAddQuery : QueryBase
    {
        public string CompanyName { get; set; }
        public int CityRegionId { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanySign { get; set; }

        public string CompanyProveFile { get; set; }

        public string CompanyIntroduction { get; set; }

        public string ZipCode { get; set; }

        
    }
}
