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
    public static class DataConstantConfigure
    {
        public static string strConn = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];  
    }
}