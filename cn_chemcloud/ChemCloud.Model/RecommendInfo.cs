using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class RecommendInfo : BaseModel
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
        public long CID { get; set; }

        public string CASNO { get; set; }

        public string ProductName { get; set; }

        public string Structure_2D { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public DateTime ActiveTime { get; set; }

        public DateTime CreateDate { get; set; }

        public long UserId { get; set; }

        public long ActiverId { get; set; }
        /// <summary>
        /// 平台货号
        /// </summary>
        public string Plat_Code { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string UserName { get; set; }

        [NotMapped]
        public List<RecommendInfo> _RecommendInfos { get; set; }
    }
}
