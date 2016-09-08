using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class WhatBusyQuery : QueryBase
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
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 是否展示（0:不显示;1:显示）
        /// </summary>
        public int IsShow { get; set; }


        public int TopCount { get; set; }
    }

}
