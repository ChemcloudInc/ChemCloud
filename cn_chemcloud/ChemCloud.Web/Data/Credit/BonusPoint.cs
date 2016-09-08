using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChemCloud.Web.Models;
using Dapper;
namespace ChemCloud.Web.Data.Credit
{
    /// <summary>
    /// 积分
    /// </summary>
    public class BonusPoint
    {


        /// <summary>
        /// 补偿 积分值 +
        /// </summary>
        /// <param name="bonusPointUserID"></param>
        /// <param name="bonusPointValueOffset"></param>
        /// <returns></returns>
        public static int PlusBonusPointValue(int bonusPointUserID, int bonusPointValueOffset)
        {

            string sql = @"UPDATE ChemCloud_Members SET BonusPoint = (BonusPoint + (@BonusPointValueOffset))
                           WHERE ChemCloud_Members.Id=@BonusPointUserID";
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Execute(sql, new
                {

                    BonusPointValueOffset = bonusPointValueOffset,
                    BonusPointUserID = bonusPointUserID

                });
            }



        }

        /// <summary>
        /// 补偿 积分值 -
        /// </summary>
        /// <param name="bonusPointUserID"></param>
        /// <param name="bonusPointValueOffset"></param>
        /// <returns></returns>
        public  static int MinusBonusPointValue(int bonusPointUserID,int  bonusPointValueOffset)
        {

            string sql = @"UPDATE ChemCloud_Members SET BonusPoint = (BonusPoint - (@BonusPointValueOffset))
                           WHERE ChemCloud_Members.Id=@BonusPointUserID";
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Execute(sql, new
                {

                    BonusPointValueOffset = bonusPointValueOffset,
                    BonusPointUserID = bonusPointUserID

                });
            }


         
        }

        /// <summary>
        /// 采购商 积分
        /// </summary>
        /// <param name="shopName"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<Models.Credit.BonusPointViewModel> SearchBonusPoint(string userName = "", int pageSize = 10)
        {
            String sql = @"SELECT TOP (@PageSize)  ChemCloud_Members.id AS BonusPointUserID,
                         ChemCloud_Members.UserName AS BonusPointUserName,
                         [ChemCloud_Members].BonusPoint AS BonusPointValue
                        FROM 
							[ChemCloud_Members]
							WHERE [ChemCloud_Members].UserType='2'
						  AND	[ChemCloud_Members].UserName LIKE @UserName
                            UNION
                           SELECT TOP (@PageSize)  ChemCloud_Members.id AS BonusPointUserID,
                            [ChemCloud_Shops].ShopName AS BonusPointUserName,
                            [ChemCloud_Members].BonusPoint AS BonusPointValue
                        FROM 
							[ChemCloud_Shops]
							,[ChemCloud_Managers]
							,[ChemCloud_Members]
							WHERE [ChemCloud_Shops].id=[ChemCloud_Managers].ShopId
							AND [ChemCloud_Managers].username=[ChemCloud_Members].username
							AND [ChemCloud_Shops].ShopName LIKE @UserName 
                            AND [ChemCloud_Members].UserType='3'";


            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DataConstantConfigure.strConn))
            {
                conn.Open();
                return conn.Query<Models.Credit.BonusPointViewModel>(sql, new
                {

                    UserName = "%"+userName+"%",
                    PageSize = pageSize

                });
            }

        }
         
    }
}