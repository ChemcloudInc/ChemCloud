
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
    /// 消息实体
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// 消息唯一标识
        /// </summary>
        public int ID { get; set; }
         
        /// <summary>
        /// 发送者编号
        /// </summary>
        public int SendUserID { get; set; }


        /// <summary>
        /// 发送者 是 主动者
        /// </summary>
        public int SendPrimary { get; set; }

        /// <summary>
        /// 发送者姓名
        /// </summary>
        public string SendUserName { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }


        /// <summary>
        /// 发送内容
        /// </summary>
        public string  MessageContent { get; set; }
        /// <summary>
        /// 接收者编号
        /// </summary>
        public int ReviceUserID { get; set; }

        /// <summary>
        /// 接收者姓名
        /// </summary>
        public string ReviceUserName { get; set; }

        /// <summary>
        /// 已读？
        /// </summary>
        public int IsRead { get; set; }
    }
}