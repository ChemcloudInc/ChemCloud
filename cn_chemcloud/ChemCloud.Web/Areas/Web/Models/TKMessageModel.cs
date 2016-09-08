using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ChemCloud.Web.Areas.Web.Models
{
    public class TKMessageModel
    {
        public TKMessageModel()
        {
            tkis = new List<TKImageInfo>();
        }
        public long Id { get; set; }
        public long TKId { get; set; }
        public string MessageContent { get; set; }
        public DateTime MessageDate { get; set; }
        public int MessageAttitude { get; set; }
        public long UserId { get; set; }
        public string ReturnName { get; set; }

        public List<TKImageInfo> tkis { get; set; }
    }

   
}