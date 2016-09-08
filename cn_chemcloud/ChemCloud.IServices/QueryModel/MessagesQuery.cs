using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
   public class MessagesQuery : QueryBase
    {
        public long Id { get; set; }
        public long SendUserId { get; set; }

        public long ReviceUserId { get; set; }

        public DateTime SendTime { get; set; }

        public string MessageContent { get; set; }
        public int IsRead { get; set; }

        public MessagesQuery()
        {

        }

    }
}
