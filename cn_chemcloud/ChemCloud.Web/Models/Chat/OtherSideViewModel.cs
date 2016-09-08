
//-------------------------------------------------------  
// <copyright  company='南京沃斯多克'> 
// Copyright 南京沃斯多克 All rights reserved.
// </copyright>  
//------------------------------------------------------- 
namespace ChemCloud.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    /// <summary>
    /// 卖方 买方 客服
    /// </summary>
    public class OtherSideViewModel
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        public string ShopName { get; set; }

        /// <summary>
        /// 返回结果 0：成功，其它不成功
        /// </summary>
        public int Result { get; set; }
    }
}