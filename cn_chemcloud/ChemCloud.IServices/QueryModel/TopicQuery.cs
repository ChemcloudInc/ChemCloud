using ChemCloud.Core;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class TopicQuery : QueryBase
	{
		public bool? IsRecommend
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public ChemCloud.Core.PlatformType PlatformType
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

		public TopicQuery()
		{
            PlatformType = ChemCloud.Core.PlatformType.PC;
		}
	}
}