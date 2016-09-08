//-------------------------------------------------------  
// <copyright  company='南京沃斯多克'> 
// Copyright 南京沃斯多克 All rights reserved.
// </copyright>  
//------------------------------------------------------- 
namespace ChemCloud.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using ChemCloud.Web.Models;

    using Dapper;

    /// <summary>
    /// 消息相关
    /// </summary>
    public static class ChatMessage
    {

        /// <summary>
        /// 获得两个人所有的消息记录 
        /// </summary>
        /// <param name="sendUserId">发送者编号</param>
        /// <param name="reviceUserID">接收者编号</param>

        public static IEnumerable<Models.MessageViewModel> GetMessageRecords(string sendUserID, string reviceUserID)
        {
            string sql = @"SELECT  * FROM ChemCloud_Messages
                          WHERE (SendUserID=@SendUserID  AND ReviceUserID=@ReviceUserID)
                          OR (SendUserID=@ReviceUserID AND ReviceUserID=@SendUserID)";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Models.MessageViewModel>(
                    sql, new
                {

                    SendUserID = sendUserID,
                    ReviceUserID = reviceUserID
                });
            }
        }

        /// <summary>
        /// 获得两个人的消息记录 
        /// </summary>
        /// <param name="sendUserId">发送者编号</param>
        /// <param name="reviceUserID">接收者编号</param>
        /// <param name="maxLimit">最大数量</param>
        public static IEnumerable<Models.MessageViewModel> GetMessageRecords(string sendUserID, string reviceUserID, int maxLimit)
        {
            string sql = @"SELECT TOP (@MaxLimit) *
                          FROM ChemCloud_Messages
                          WHERE (SendUserID=@SendUserID AND ReviceUserID=@ReviceUserID) 
                          OR (SendUserID=@ReviceUserID AND ReviceUserID=@SendUserID)";
                                     
           
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
               return conn.Query<Models.MessageViewModel>(sql, new
                {
                    MaxLimit = maxLimit,
                    SendUserID = sendUserID,
                    ReviceUserID = reviceUserID
                });
            }   
        }

        /// <summary>
        /// 获得两个人的消息记录 
        /// </summary>
        /// <param name="sendUserId">发送者编号</param>
        /// <param name="reviceUserID">接收者编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static IEnumerable<Models.MessageViewModel> GetMessageRecords(string sendUserID, string reviceUserID,DateTime beginTime, DateTime endTime)
        {
            string sql = @"SELECT  * FROM ChemCloud_Messages
                         WHERE ((SendUserID=@SendUserID AND ReviceUserID=@ReviceUserID) OR (SendUserID=@ReviceUserID AND ReviceUserID=@SendUserID))
                         AND (SendTime>@BeginTime AND SendTime<@EndTime)
                         ORDER BY Id DESC";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Models.MessageViewModel>(sql, new
                {

                    SendUserID = sendUserID,
                    ReviceUserID = reviceUserID,
                    BeginTime = beginTime,
                    EndTime=endTime
                });
            }
        }


        /// <summary>
        /// 保存信息到数据库吧
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int SaveMessageRecord(Models.MessageViewModel item)
        {
            string sql = @"INSERT INTO ChemCloud_Messages
                           (SendUserID
                           ,MessageContent
                           ,ReviceUserID
                           ,SendTime
                           ,IsRead
                           ,SendUserName
                           ,ReviceUserName)
                     VALUES
                           (@SendUserID
                           ,@MessageContent
                           ,@ReviceUserID
                           ,@SendTime
                           ,@IsRead
                           ,@SendUserName
                           ,@ReviceUserName)";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Execute(sql,item);
            }
        }
    }
}