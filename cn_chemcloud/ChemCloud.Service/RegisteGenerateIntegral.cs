using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ChemCloud.Service
{
	public class RegisteGenerateIntegral : ServiceBase, IConversionMemberIntegralBase
	{
		public RegisteGenerateIntegral()
		{
		}

		public int ConversionIntegral()
		{
			MemberIntegralRule memberIntegralRule = context.MemberIntegralRule.FirstOrDefault((MemberIntegralRule m) => m.TypeId == 9);
			if (memberIntegralRule == null)
			{
				throw new Exception(string.Format("找不到因注册产生会员积分的规则", new object[0]));
			}
			return memberIntegralRule.Integral;
		}
	}
}