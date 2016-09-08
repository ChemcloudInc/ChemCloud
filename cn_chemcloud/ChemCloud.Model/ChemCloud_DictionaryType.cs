using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ChemCloud_DictionaryType : BaseModel
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
        /// 字典类型名称
        /// </summary>
        public string TypeName { get; set; }
        
        /// <summary>
        /// 是否启用0禁用1启用
        /// </summary>
        public string IsEnabled { get; set; }

        [NotMapped]
        public int PageNo { get; set; }

        [NotMapped]
        public int PageSize { get; set; }
    }
}
