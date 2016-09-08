using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class RecommendInfoQuery :QueryBase
    {
        /// <summary>
        /// 平台货号
        /// </summary>
        public string Plat_Code { get; set; }
        public int? Status { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
