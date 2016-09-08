using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class OrderShip : BaseModel
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

        public long OrderId
        {
            get;
            set;
        }

        public long OrderItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 面单数据
        /// </summary>
        public byte[] LabelImage
        {
            get;
            set;
        }
        //物流FormId
        public string TrackFormId { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string TrackNumber { get; set; }
        
        
        /// <summary>
        /// 主数据
        /// </summary>
        public bool Master { get; set; }
    }
}