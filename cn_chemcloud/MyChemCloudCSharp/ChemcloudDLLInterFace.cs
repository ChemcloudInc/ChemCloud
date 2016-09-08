using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MyChemCloudCSharp
{
    public class ChemcloudDLLInterFace
    {
        [DllImport("Mychemcloud.dll", EntryPoint = "chemcloudDLL", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool chemcloudDLL(String In, int nType, ref StringBuilder OutData);

        public static string ParseData(String In, int nType, ref String OutData)
        {
            try
            {
                StringBuilder outdata = new StringBuilder(1000);

                bool bRet = chemcloudDLL(In, nType, ref outdata);

                return outdata.ToString();
            }
            catch (Exception ex)
            {
                ChemcloudDLLInterFace.WriteLog("marvin异常" + ex.ToString());
                return "";
            }
        }

        public static void WriteLog(string txt)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\log\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path += DateTime.Now.ToString("yyyyMMdd") + "-" + DateTime.Now.ToString("HH") + ".txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                FileStream fs;
                StreamWriter sw;
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(DateTime.Now.ToString("HH:mm:ss") + " " + txt + "\r\n");
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                WriteLog("程序发生异常（WriteLog）。详情：" + ex.Message);
            }
        }

    }
}
