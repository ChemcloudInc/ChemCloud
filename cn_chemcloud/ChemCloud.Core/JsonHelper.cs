using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using System.Collections;

namespace ChemCloud.Core
{
    public class JsonHelper
    {
        /// <summary>
        /// 把dataset数据转换成json的格式
        /// </summary>
        /// <param name="ds">dataset数据集</param>
        /// <returns>json格式的字符串</returns>
        public static string GetJsonByDataset(DataSet ds)
        {
            int cid = 0; string imgstr = ""; int cataname = 0;
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                //如果查询到的数据为空则返回标记ok:false
                return "{\"ok\":false}";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"Total\":" + ds.Tables[0].Rows.Count + ",");
            foreach (DataTable dt in ds.Tables)
            {
                sb.Append(string.Format("\"{0}\":[", dt.TableName));

                foreach (DataRow dr in dt.Rows)
                {

                    //if (!string.IsNullOrEmpty(dr["PUB_CID"].ToString()))
                    //{
                    //    cid = int.Parse(dr["PUB_CID"].ToString());
                    //    cataname = cid / 5000;
                    //    imgstr = System.Configuration.ConfigurationManager.AppSettings["cidimg"] + "/" + cataname + "/" + cid + ".png";
                    //    dr["STRUCTURE_2D"] = imgstr;
                    //}

                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }

                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("],");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("}");
            return sb.ToString();
        }



        public static string GetJsonByDatasetAdv(DataSet ds)
        {
            int cid = 0; string imgstr = ""; int cataname = 0;
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                //如果查询到的数据为空则返回标记ok:false
                return "{\"ok\":false}";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"Total\":" + ds.Tables[0].Rows.Count + ",");
            foreach (DataTable dt in ds.Tables)
            {
                sb.Append(string.Format("\"{0}\":[", dt.TableName));

                foreach (DataRow dr in dt.Rows)
                {

                    //if (!string.IsNullOrEmpty(dr["PUB_CID"].ToString()))
                    //{
                    //    cid = int.Parse(dr["PUB_CID"].ToString());
                    //    cataname = cid / 5000;
                    //    imgstr = System.Configuration.ConfigurationManager.AppSettings["cidimg"] + "/" + cataname + "/" + cid + ".png";
                    //    dr["STRUCTURE_2D"] = imgstr;
                    //}

                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n"));
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }

                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("],");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// 将object转换成为string
        /// </summary>
        /// <param name="ob">obj对象</param>
        /// <returns></returns>
        public static string ObjToStr(object ob)
        {
            if (ob == null)
            {
                return string.Empty;
            }
            else
                return ob.ToString();
        }

 
        /// <summary>
        /// DataTable转为JSON格式，通过拼接字符串方法
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTable2Json(DataTable dt)
        {
            if (dt.Rows.Count < 1)
                return "";
            StringBuilder sb = new StringBuilder("[");

            int iCols = dt.Columns.Count;

            //表字段存放的数组
            string[] aColName = new string[iCols];
            for (int i = 0; i < iCols; i++)
                aColName[i] = dt.Columns[i].ColumnName;

            foreach (DataRow row in dt.Rows)
            {
                sb.Append("{");

                for (int i = 0; i < iCols - 1; i++)
                {
                    sb.AppendFormat("\"{0}\":\"{1}\",", aColName[i], row[i].ToString());
                }
                sb.AppendFormat("\"{0}\":\"{1}\"", aColName[iCols - 1], row[iCols - 1].ToString());

                sb.Append("},");
            }
            return sb.ToString().TrimEnd(',') + "]";
        }

        

    }
}
