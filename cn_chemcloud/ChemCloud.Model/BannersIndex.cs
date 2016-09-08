using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class BannersIndex : BaseModel
    {
        private long _id;
        public new long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                base.Id = value;
            }
        }

        public long ShopId { get; set; }
        public string BannerTitle { get; set; }
        public string BannerDes { get; set; }
        public string Url { get; set; }
        public string TargetName { get; set; }
        public long ManagerId { get; set; }
        public int LanguageType { get; set; }

    }


    public class Result_BannersIndex : BaseModel
    {
        public long ShopId { get; set; }
        public string BannerTitle { get; set; }
        public string BannerDes { get; set; }
        public string Url { get; set; }
        public string TargetName { get; set; }
        public long ManagerId { get; set; }
        public string LanguageType { get; set; }

    }

    public class Result_BannersIndex_List
    {
        public List<Result_BannersIndex> List { get; set; }

        public PageInfo PageInfo { get; set; }

        public Result_Msg Msg { get; set; }
    }



}
