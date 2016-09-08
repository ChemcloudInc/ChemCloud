using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.SellerAdmin.Models
{
	public class TopicModel
	{
		[Required]
		public string BackgroundImage
		{
			get;
			set;
		}

		public ICollection<ChemCloud.Model.FloorTopicInfo> FloorTopicInfo
		{
			get;
			set;
		}

		[Required]
		public string FrontCoverImage
		{
			get;
			set;
		}

		public long Id
		{
			get;
			set;
		}

		[MaxLength(20, ErrorMessage="专题名称最多20个字符")]
		[RegularExpression("^[a-zA-Z0-9_\\u4e00-\\u9fa5]+$", ErrorMessage="专题名称必须是中文、英文、字母和下划线")]
		[Required(ErrorMessage="专题名称不能为空")]
		public string Name
		{
			get;
			set;
		}

		public int Sequence
		{
			get;
			set;
		}

		[MaxLength(10, ErrorMessage="标签名称最多10个字符")]
		[RegularExpression("^[a-zA-Z0-9_\\u4e00-\\u9fa5]+$", ErrorMessage="专题名称必须是中文、英文、字母和下划线")]
		[Required(ErrorMessage="标签不能为空")]
		public string Tags
		{
			get;
			set;
		}

		public ICollection<ChemCloud.Model.TopicModuleInfo> TopicModuleInfo
		{
			get;
			set;
		}

		[Required]
		public string TopImage
		{
			get;
			set;
		}

		public TopicModel()
		{
            FloorTopicInfo = new List<ChemCloud.Model.FloorTopicInfo>();
            TopicModuleInfo = new List<ChemCloud.Model.TopicModuleInfo>();
		}
	}
}