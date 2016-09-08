using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class TopicInfo : BaseModel
    {
        private long _id;

        private string backgroundImage
        {
            get;
            set;
        }

        public string BackgroundImage
        {
            get
            {
                return string.Concat(ImageServerUrl, backgroundImage);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(ImageServerUrl))
                {
                    backgroundImage = value;
                    return;
                }
                backgroundImage = value.Replace(ImageServerUrl, "");
            }
        }

        public string frontCoverImage
        {
            get;
            set;
        }

        public string FrontCoverImage
        {
            get
            {
                return string.Concat(ImageServerUrl, frontCoverImage);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(ImageServerUrl))
                {
                    frontCoverImage = value;
                    return;
                }
                frontCoverImage = value.Replace(ImageServerUrl, "");
            }
        }

        public virtual ICollection<MobileHomeTopicsInfo> ChemCloud_MobileHomeTopics
        {
            get;
            set;
        }

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

        public bool IsRecommend
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public PlatformType PlatForm
        {
            get;
            set;
        }

        public string SelfDefineText
        {
            get;
            set;
        }

        public long ShopId
        {
            get;
            set;
        }

        public string Tags
        {
            get;
            set;
        }

        public virtual ICollection<ChemCloud.Model.TopicModuleInfo> TopicModuleInfo
        {
            get;
            set;
        }

        private string topImage
        {
            get;
            set;
        }

        public string TopImage
        {
            get
            {
                return string.Concat(ImageServerUrl, topImage);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(ImageServerUrl))
                {
                    topImage = value;
                    return;
                }
                topImage = value.Replace(ImageServerUrl, "");
            }
        }

        public string topImage1 { get; set; }
        public string backgroundImage1 { get; set; }


        public TopicInfo()
        {
            TopicModuleInfo = new HashSet<ChemCloud.Model.TopicModuleInfo>();
            ChemCloud_MobileHomeTopics = new HashSet<MobileHomeTopicsInfo>();
        }
    }
}