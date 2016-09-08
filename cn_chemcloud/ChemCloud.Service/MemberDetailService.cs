using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class MemberDetailService : ServiceBase, IMemberDetailService, IService, IDisposable
    {
        /// <summary>
        /// 获取会员资料列表
        /// </summary>
        public MemberDetailService()
        {
        }

        /*采购商列表*/
        public PageModel<MemberDetail> GetMembers(MemberDetailQuery query)
        {
            int num = 0;

            IQueryable<MemberDetail> Himall_Member = context.MemberDetail.AsQueryable<MemberDetail>();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                Himall_Member =
                    from d in Himall_Member
                    where d.CompanyName.Equals(query.CompanyName)
                    select d;
            }

            Himall_Member = Himall_Member.GetPage(out num, query.PageNo, query.PageSize, (IQueryable<MemberDetail> d) =>
                from o in d
                orderby o.MemberId
                select o);
            return new PageModel<MemberDetail>()
            {

                Models = Himall_Member,
                Total = num
            };
        }

        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserMemberInfo GetMemberInfoById(long id)
        {
            return context.UserMemberInfo.FindById<UserMemberInfo>(id);
        }



        public ShopGradeInfo GetShopGrade(long id)
        {
            return context.ShopGradeInfo.FindById<ShopGradeInfo>(id);
        }

        /// <summary>
        /// 获取会员资料信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessCategoryOn"></param>
        /// <returns></returns>
        public MemberDetail GetHimall_MemberDetail(long id, bool businessCategoryOn = false)
        {
            MemberDetail nums = context.MemberDetail.FirstOrDefault((MemberDetail m) => m.MemberId.Equals(id));
            if (nums == null)
            {
                return null;
            }
            UserMemberInfo memberinfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(id));

            nums.MemberName = (memberinfo == null ? "" : memberinfo.UserName);

            nums.Email = (memberinfo == null ? "" : memberinfo.Email);

            nums.StrLastLoginDate = (memberinfo == null ? DateTime.Now : memberinfo.LastLoginDate);

            return nums;
        }

        /// <summary>
        /// 编辑买家资质信息
        /// </summary>
        /// <param name="_MemberDetail"></param>
        public void UpdateHimall_MemberDetail(MemberDetail _MemberDetail)
        {
            MemberDetail entity = context.MemberDetail.FirstOrDefault((MemberDetail m) => m.MemberId.Equals(_MemberDetail.MemberId));
            if (entity != null)
            {
                //编辑
                entity.MemberId = _MemberDetail.MemberId;
                entity.CompanyName = _MemberDetail.CompanyName;
                entity.CityRegionId = _MemberDetail.CityRegionId;
                entity.CompanyAddress = _MemberDetail.CompanyAddress;
                entity.CompanyIntroduction = _MemberDetail.CompanyIntroduction;
                //entity.CompanySign = _MemberDetail.CompanySign;
                //entity.CompanyProveFile = _MemberDetail.CompanyProveFile;
                entity.ZipCode = _MemberDetail.ZipCode;
                context.SaveChanges();
            }
            else
            {
                //新增
                _MemberDetail.CreateDate = DateTime.Now;
                _MemberDetail.Stage = ShopInfo.ShopAuditStatus.WaitAudit;
                context.MemberDetail.Add(_MemberDetail);
                context.SaveChanges();
            }
        }



        public MemberDetail GetMemberDetailByUid(long id)
        {
            return context.MemberDetail.FirstOrDefault((MemberDetail m) => m.MemberId.Equals(id));
        }
    }
}