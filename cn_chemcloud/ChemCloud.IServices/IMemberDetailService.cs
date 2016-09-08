using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface IMemberDetailService : IService, IDisposable
    {

        //��ȡ���������Ϣ
       MemberDetail GetHimall_MemberDetail(long id, bool businessCategoryOn = false);

        //�༭���������Ϣ
        void UpdateHimall_MemberDetail(MemberDetail _MemberDetail);

        PageModel<MemberDetail> GetMembers(MemberDetailQuery query);

        UserMemberInfo GetMemberInfoById(long id);

        MemberDetail GetMemberDetailByUid(long id);

    }
}