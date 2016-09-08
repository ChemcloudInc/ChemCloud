using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class MessageDetialQuery : QueryBase
    {
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int SendObj { get; set; }

        public int MsgType { get; set; }

        public int Languagetype { get; set; }
        public MessageDetialQuery()
        {

        }
    }
    public class MessageDetialAddQuery : QueryBase
    {
        public int ReceType { get; set; }

        public int MessageTitleId { get; set; }

        public string MessageTitle { get; set; }

        public string MessageContent { get; set; }

        public int LanguageType { get; set; }

        public string[] Ids { get; set; }
    }
}
