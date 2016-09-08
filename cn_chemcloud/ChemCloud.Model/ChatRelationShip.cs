using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class ChatRelationShip
    {
        public Int64 Id { get; set; }
        public Int64 SendUserId { get; set; }
        public Int64 ReviceUserId { get; set; }
        public int state { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
