using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface IMemberDetailService : IService, IDisposable
    {

        //获取买家资质信息
       MemberDetail GetHimall_MemberDetail(long id, bool businessCategoryOn = false);

        //编辑买家资质信息
        void UpdateHimall_MemberDetail(MemberDetail _MemberDetail);

        PageModel<MemberDetail> GetMembers(MemberDetailQuery query);

        UserMemberInfo GetMemberInfoById(long id);

        MemberDetail GetMemberDetailByUid(long id);

    }
}