using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class BannerIndexQuery
    {
        public long Id { get; set; }

        public string BannerTitle { get; set; }
        public string BannerDes { get; set; }
        public string Url { get; set; }
        public string TargetName { get; set; }
        public long ManagerId { get; set; }
        public int LanguageType { get; set; }

    }

    public class BannerIndexAddQuery
    {
        public long Id { get; set; }

        public long ShopId { get; set; }
        public string BannerTitle { get; set; }
        public string BannerDes { get; set; }
        public string Url { get; set; }
        public string TargetName { get; set; }
        public long ManagerId { get; set; }
        public int LanguageType { get; set; }


    }

    public class BannerIndexModifyQuery : BannerIndexAddQuery
    {
        public long Id { get; set; }

    }
}
