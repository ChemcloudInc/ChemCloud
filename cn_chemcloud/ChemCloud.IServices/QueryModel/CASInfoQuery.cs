using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class CASInfoQuery : QueryBase
    {
        public CASInfoQuery() { }
        public string CAS_NUMBER { get; set; }
        /// <summary>
        /// 分子式
        /// </summary>
        public string Molecular_Formula { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public string CHINESE { get; set; }
        /// <summary>
        /// CAS NO
        /// </summary>
        public string CAS { get; set; }

        public string CAS_NUMBERList { get; set; }
    }
}
