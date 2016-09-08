using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class TKImageInfo
    {
        public long Id { get; set; }
        public string TKImage { get; set; }
        public long TKMessageId { get; set; }
    }
}
