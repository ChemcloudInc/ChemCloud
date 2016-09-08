using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class COAListQuery : QueryBase
    {
        //条件
       public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public string CoANo { get; set; }

        public string CASNo { get; set; }
        public long UserId { get; set; }

        

        public COAListQuery()
        {

        }
    }
}
