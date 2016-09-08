using ChemCloud.Core;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.PaymentPlugin
{
	public abstract class ConfigBase
	{
		public SerializableDictionary<PlatformType, bool> OpenStatus
		{
			get;
			set;
		}

		public PlatformType[] SupportPlatfoms
		{
			get;
			set;
		}

		protected ConfigBase()
		{
		}
	}
}