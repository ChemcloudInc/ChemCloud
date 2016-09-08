using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ProductLevel : BaseModel
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
        public string LevelNameCN
        {
            get;
            set;
        }
        public string LevelNameEN
        {
            get;
            set;
        }
    }
    
}
