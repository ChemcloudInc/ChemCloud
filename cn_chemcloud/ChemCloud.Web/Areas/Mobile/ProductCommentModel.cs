using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Mobile
{
	public class ProductCommentModel
	{
		public string Content
		{
			get;
			set;
		}

		public int Mark
		{
			get;
			set;
		}

		public long OrderItemId
		{
			get;
			set;
		}

		public long ProductId
		{
			get;
			set;
		}

		public ProductCommentModel()
		{
		}
	}
}