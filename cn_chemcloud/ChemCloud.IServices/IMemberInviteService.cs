using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface IMemberInviteService : IService, IDisposable
	{
		void AddInviteIntegel(UserMemberInfo RegMember, UserMemberInfo InviteMember);

		void AddInviteRecord(InviteRecordInfo info);

		PageModel<InviteRecordInfo> GetInviteList(InviteRecordQuery query);

		InviteRuleInfo GetInviteRule();

		UserInviteModel GetMemberInviteInfo(long userId);

		bool HasInviteIntegralRecord(long RegId);

		void SetInviteRule(InviteRuleInfo model);
	}
}