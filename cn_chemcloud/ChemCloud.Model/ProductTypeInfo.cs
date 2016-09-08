using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ProductTypeInfo : BaseModel
	{
		private long _id;

		public virtual ICollection<ChemCloud.Model.AttributeInfo> AttributeInfo
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.CategoryInfo> CategoryInfo
		{
			get;
			set;
		}

		[NotMapped]
		public string ColorValue
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

		public bool IsSupportColor
		{
			get;
			set;
		}

		public bool IsSupportSize
		{
			get;
			set;
		}

		public bool IsSupportVersion
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		[NotMapped]
		public string SizeValue
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.SpecificationValueInfo> SpecificationValueInfo
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.TypeBrandInfo> TypeBrandInfo
		{
			get;
			set;
		}

		[NotMapped]
		public string VersionValue
		{
			get;
			set;
		}

		public ProductTypeInfo()
		{
            AttributeInfo = new HashSet<ChemCloud.Model.AttributeInfo>();
            CategoryInfo = new HashSet<ChemCloud.Model.CategoryInfo>();
            SpecificationValueInfo = new HashSet<ChemCloud.Model.SpecificationValueInfo>();
            TypeBrandInfo = new HashSet<ChemCloud.Model.TypeBrandInfo>();
		}

		public ProductTypeInfo(bool initialSpec) : this()
		{
            ColorValue = "";
            SizeValue = "";
            VersionValue = "";
		}
	}
}