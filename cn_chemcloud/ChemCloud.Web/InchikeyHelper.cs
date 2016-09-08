using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ChemCloud.Web
{
    public class InchikeyHelper
    {
        /// <summary>
        /// 根据mol解析inchkey
        /// </summary>
        /// <param name="strmol"></param>
        /// <returns></returns>
        public static string GetInchikey(string strmol)
        {
            StringBuilder input = new StringBuilder();
            input.AppendLine(strmol);
            String outstr = "";
            string inhkey = MyChemCloudCSharp.ChemcloudDLLInterFace.ParseData(input.ToString(), 4, ref outstr);
            if (inhkey != "empty")
            {
                if (!string.IsNullOrEmpty(inhkey))
                {
                    inhkey = inhkey.Substring(6);
                }
                else { inhkey = ""; }
            }
            else { inhkey = ""; }

            return inhkey;
        }
    }
}