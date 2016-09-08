using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model.Common
{
    public class HashSet_Common
    {
        /// <summary>
        /// 招聘信息——审核状态
        /// </summary>
        public static HashSet<KeyValuePair<int, string>> hashApprovalStatus = new HashSet<KeyValuePair<int, string>>() {
               new KeyValuePair<int,string>(0,"未赋值"),
               new KeyValuePair<int,string>(1,"待审核"),
               new KeyValuePair<int,string>(2,"审核未通过"),
               new KeyValuePair<int,string>(3,"审核通过")
            };

        /// <summary>
        /// 招聘信息——用户类型
        /// </summary>
        public static HashSet<KeyValuePair<int, string>> hashUserType = new HashSet<KeyValuePair<int, string>>() { 
               new KeyValuePair<int,string>(0,"未赋值"),
               new KeyValuePair<int,string>(1,"平台管理员"),
               new KeyValuePair<int,string>(2,"供应商"),
               new KeyValuePair<int,string>(3,"采购商")
            };

        /// <summary>
        /// 首页-实时交易曲线类型
        /// </summary>
        public static HashSet<KeyValuePair<int, string>> hashCurveType = new HashSet<KeyValuePair<int, string>>() {
               new KeyValuePair<int,string>(1,"代理采购"),
               new KeyValuePair<int,string>(2,"定制合成")
            };

        /// <summary>
        /// 货币类型（0：美元$；1：人民币：￥）
        /// </summary>
        public static HashSet<KeyValuePair<int, string>> hashTypeOfCurrency = new HashSet<KeyValuePair<int, string>>() {
               new KeyValuePair<int,string>(0,"$"),
               new KeyValuePair<int,string>(1,"￥")
            };


        /// <summary>
        /// 工作类型（0：全职；1：兼职）
        /// </summary>
        public static HashSet<KeyValuePair<int, string>> hashWorkType = new HashSet<KeyValuePair<int, string>>() {
               new KeyValuePair<int,string>(0,"全职"),
               new KeyValuePair<int,string>(1,"兼职"),
               new KeyValuePair<int,string>(2,"外包")
            };


        /// <summary>
        /// 薪资类型（0：面议；1：时薪；2：月薪；3：年薪）
        /// </summary>
        public static HashSet<KeyValuePair<int, string>> hashPayrollType = new HashSet<KeyValuePair<int, string>>() {
               new KeyValuePair<int,string>(0,"面议"),
               new KeyValuePair<int,string>(1,"时薪"),
               new KeyValuePair<int,string>(2,"月薪"),
               new KeyValuePair<int,string>(3,"年薪")
            };


        public static string[] ImageTypeArr = new string[]
        {
           ".jpg",".jpeg",".png",".gif",".bmp"
        };
    }
}
