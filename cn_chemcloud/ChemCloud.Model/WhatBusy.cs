using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class WhatBusy : BaseModel
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
        /// 会员ID
        /// </summary>
        public long UserId { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 忙什么详情
        /// </summary>
        public string BusyCotent { get; set; }



        /// <summary>
        /// 数据来源类型（1：用户注册；2：定制合成；3：代理采购；4：招聘信息；5：会议信息；6：技术交易；）
        /// </summary>
        public int BusyType { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 是否展示（0:不显示;1:显示）
        /// </summary>
        public int IsShow { get; set; }

    }

    public class Result_WhatBusy
    {
        public long Id { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public long UserId { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 忙什么详情
        /// </summary>
        public string BusyCotent { get; set; }

        /// <summary>
        /// 数据来源类型（1：用户注册；2：定制合成；3：代理采购；4：招聘信息；5：会议信息；6：技术交易；）
        /// </summary>
        public int BusyType { get; set; }
        public string BusyTypeName { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 是否展示（0:不显示;1:显示）
        /// </summary>
        public int IsShow { get; set; }
    }


    #region 公共：返回 对象 + 提示信息 Result_Model
    /// <summary>
    /// 公共：返回 对象 + 提示信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result_Model_List_WhatBusy<T> where T : class, new()
    {
        public Result_List<Result_Model<T>> List { get; set; }
        public Result_Model<T> Model { get; set; }
        public Result_Msg Msg { get; set; }
    }
    #endregion


}
