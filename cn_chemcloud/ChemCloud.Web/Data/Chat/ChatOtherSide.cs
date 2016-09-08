
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
    using Dapper;
    /// <summary>
    /// 卖方 买方 客服 
    /// </summary>
    public class ChatOtherSide
    {

        /// <summary>
        ///  get 供应商 ID，UserName 
        /// </summary>
        /// <param name="shopID"></param>
        /// <returns></returns>
        public static IEnumerable<Models.OtherSideViewModel> GetOtherSideInfoByShop(string shopID)
        {

            string sql = @"SELECT  ChemCloud_Members.id AS UserID,[ChemCloud_Shops].ShopName AS UserName from 
							[dbo].[ChemCloud_Shops]
							,[dbo].[ChemCloud_Managers]
							,[dbo].[ChemCloud_Members]
							WHERE [ChemCloud_Shops].id=[ChemCloud_Managers].ShopId
							AND [ChemCloud_Managers].username=[ChemCloud_Members].username
							AND [ChemCloud_Shops].ID=@ShopID;";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Models.OtherSideViewModel>(sql, new
                {

                    ShopID=shopID

                });
            }

          
        }


        /// <summary>
        /// 查找 客服 ，买家 卖家 
        /// </summary>
        /// <param name="searchText">用户姓名</param>
        /// <param name="customerLimit">客服 条数</param>
        /// <param name="memberLimit">买家 卖家 总条数</param>
        /// <returns></returns>
        public static IEnumerable<Models.OtherSideViewModel> GetOtherSide(string searchText,int customerLimit,int memberLimit)
        {
            string sql = @"SELECT TOP (@CustomerLimit) Id as UserID,UserName AS UserName FROM  [dbo].[ChemCloud_Managers]
                            WHERE UserName LIKE @SearchText 
                            UNION
                            SELECT TOP (@MemberLimit) Id as UserID,UserName AS UserName FROM  [dbo].ChemCloud_Members
                            WHERE UserName LIKE @SearchText
                            UNION
	                       SELECT TOP (@MemberLimit) ChemCloud_Members.id AS UserID,[ChemCloud_Shops].ShopName AS UserName from 
							[dbo].[ChemCloud_Shops]
							,[dbo].[ChemCloud_Managers]
							,[dbo].[ChemCloud_Members]
							WHERE [ChemCloud_Shops].id=[ChemCloud_Managers].ShopId
							AND [ChemCloud_Managers].username=[ChemCloud_Members].username
                            AND  [ChemCloud_Shops].ShopName LIKE @SearchText";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Models.OtherSideViewModel>(sql, new
                {

                    SearchText = "%"+searchText+"%",
                    CustomerLimit= customerLimit,
                    MemberLimit= memberLimit,

                });
            }
        
        }
    }
}