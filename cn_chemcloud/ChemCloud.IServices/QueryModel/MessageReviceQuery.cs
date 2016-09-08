using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class MessageReviceQuery : QueryBase
    {
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int ReadFlag { get; set; }

        public long UserId { get; set; }

        public int Languagetype { get; set; } 
        public int ReceType { get; set; }
        public MessageReviceQuery()
        {

        }
    }
}
