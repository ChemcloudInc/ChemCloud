using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Linq;

namespace ChemCloud.Service
{
	public class InvitationMemberRegisteGenerateIntegral : ServiceBase, IConversionMemberIntegralBase
	{
		public InvitationMemberRegisteGenerateIntegral()
		{
		}

		public int ConversionIntegral()
		{
			InviteRuleInfo inviteRuleInfo = context.InviteRuleInfo.FirstOrDefault();
			if (inviteRuleInfo == null)
			{
				throw new Exception(string.Format("找不到邀请会员注册产生会员积分的规则", new object[0]));
			}
			return inviteRuleInfo.InviteIntegral.Value;
		}
	}
}