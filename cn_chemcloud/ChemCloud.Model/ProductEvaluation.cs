using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemCloud.Model
{
    public class ProductEvaluation : BaseModel
    {
        public DateTime BuyTime
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        }

        public string EvaluationContent
        {
            get;
            set;
        }

        public int EvaluationRank
        {
            get;
            set;
        }

        public bool EvaluationStatus
        {
            get;
            set;
        }

        public DateTime EvaluationTime
        {
            get;
            set;
        }

        public new long Id
        {
            get;
            set;
        }

        public long OrderId
        {
            get;
            set;
        }

        public ProductCommentInfo ProductComment
        {
            get;
            set;
        }

        public long ProductId
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }

        public string ReplyContent
        {
            get;
            set;
        }

        public DateTime ReplyTime
        {
            get;
            set;
        }

        public string Size
        {
            get;
            set;
        }

        public string ThumbnailsUrl
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }
        public long CommentId{
            get;
            set;

        }
        public ProductEvaluation()
        {
        }
        
        
        [NotMapped]
        /// <summary>
        /// 供应商
        /// </summary>
        public string ShopName { get; set; }

         [NotMapped]
        /// <summary>
        /// 交易完成日期
        /// </summary>
        public string FinishDate { get; set; }
    }
}