using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
   public class MessageEnclosure:BaseModel
    {
       public MessageEnclosure()
       {
       }

       public long Id { get; set; }
       public long MsgId { get; set; }

       public string Url { get; set; }
    }
}
