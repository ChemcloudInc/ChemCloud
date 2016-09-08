using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    /// <summary>
    /// 在线生成运单响应
    /// </summary>
    public class CreateShipRep
    {
        public List<ShipReply> ReplyList { get; set; }
        public string Message { get; set; }
    }
}
