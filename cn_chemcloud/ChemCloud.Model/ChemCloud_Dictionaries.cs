using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ChemCloud_Dictionaries : BaseModel
    {
        private long _id;
        public new long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                base.Id = value;
            }
        }

        /// <summary>
        /// 字典类型编号
        /// </summary>
        public long DictionaryTypeId { get; set; }

        [NotMapped]
        public string TypeName { get; set; }

        /// <summary>
        /// 字典key
        /// </summary>
        public string DKey { get; set; }

        /// <summary>
        /// 字典DValue
        /// </summary>
        public string DValue { get; set; }
        /// <summary>
        /// 字典英文名
        /// </summary>
        public string DValue_En { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        [NotMapped]
        public int PageNo { get; set; }

        [NotMapped]
        public int PageSize { get; set; }
    }
}
