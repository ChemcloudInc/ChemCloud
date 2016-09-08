using ChemCloud.IServices;
using System;

namespace ChemCloud.Service
{
	public class GenralIntegral : ServiceBase, IConversionMemberIntegralBase
	{
		private int _Integral;

		public GenralIntegral(int Integral)
		{
            _Integral = Integral;
		}

		public int ConversionIntegral()
		{
			return _Integral;
		}
	}
}