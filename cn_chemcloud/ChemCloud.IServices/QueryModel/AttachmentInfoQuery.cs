using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class AttachmentInfoQuery : QueryBase
    {
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
