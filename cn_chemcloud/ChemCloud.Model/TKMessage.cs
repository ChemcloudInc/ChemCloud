using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;


namespace ChemCloud.Model
{
    public class TKMessage : BaseModel
    {
        public long Id { get; set; }
        public long TKId { get; set; }
        public string MessageContent { get; set; }
        public DateTime MessageDate { get; set; }
        public int MessageAttitude { get; set; }
        public long UserId { get; set; }
        public string ReturnName { get; set; }
       
    }
}
