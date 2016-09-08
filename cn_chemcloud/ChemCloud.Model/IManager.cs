using System;

namespace ChemCloud.Model
{
	public interface IManager
	{
		DateTime CreateDate
		{
			get;
			set;
		}

		string Description
		{
			get;
			set;
		}

		long Id
		{
			get;
			set;
		}

		string Password
		{
			get;
			set;
		}

		string PasswordSalt
		{
			get;
			set;
		}

		long RoleId
		{
			get;
			set;
		}

		string RoleName
		{
			get;
			set;
		}

		long ShopId
		{
			get;
			set;
		}
        long CertificationId
        {
            get;
            set;
        }
		string UserName
		{
			get;
			set;
		}
	}
}