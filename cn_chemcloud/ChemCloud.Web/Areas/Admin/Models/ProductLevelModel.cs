using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Models
{
    public class ProductLevelModel
    {
        public long Id
        {
            get;
            set;
        }
        public string LevelNameCN
        {
            get;
            set;
        }
        public string LevelNameEN
        {
            get;
            set;
        }
    }
}