using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class MemberDetailController : BaseAdminController
    {
        public MemberDetailController()
        {
        }

        /*列表页面*/
        public ActionResult Management()
        {
            return View();
        }

        /*详细页面*/
        public ActionResult Detail(long id)
        {
            MemberDetail member = ServiceHelper.Create<IMemberDetailService>().GetHimall_MemberDetail(id);

            MemberDetailsModel memberModel = new MemberDetailsModel()
            {
                Id = member.Id,
                MemberId = member.MemberId,
                UserName = member.MemberName,
                CompanyName = member.CompanyName,
                CompanyAddress = member.CompanyAddress,
                CompanySign = member.CompanySign,
                CompanyProveFile = member.CompanyProveFile,
                CompanyIntroduction = member.CompanyIntroduction,
                LastLoginDate = member.StrLastLoginDate,
                Email = member.Email,
                CreateDate = member.CreateDate,
                ZipCode = member.ZipCode,
                Stage = "启用",
            };
            MemberDetailsModel memberModel1 = memberModel;
            return View(memberModel1);
        }

        /*分页获取会员管理*/
        [Description("分页获取会员管理JSON数据")]
        public JsonResult List(int page, int rows, string CompanyName, bool? status = null)
        {
            MemberQuery memberQuery = new MemberQuery()
            {
                UserType = 3,
                keyWords = CompanyName,
                PageNo = page,
                PageSize = rows
            };

            PageModel<UserMemberInfo> members = ServiceHelper.Create<IMemberService>().GetMembers(memberQuery);
            IEnumerable<UserMemberInfo> models =
                from item in members.Models.ToArray()
                select new UserMemberInfo()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    CreateDate = item.CreateDate,
                    Email = item.Email,
                    CompanyName = ServiceHelper.Create<IMemberDetailService>().GetMemberDetailByUid(item.Id) == null ? "" : ServiceHelper.Create<IMemberDetailService>().GetMemberDetailByUid(item.Id).CompanyName
                };
            DataGridModel<UserMemberInfo> dataGridModel = new DataGridModel<UserMemberInfo>()
            {
                rows = models,
                total = members.Total
            };
            return Json(dataGridModel);
        }

        /*批量删除*/
        [HttpPost]
        public JsonResult BatchDelete(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            ServiceHelper.Create<IMemberService>().BatchDeleteMember(nums.ToArray());
            Result result = new Result()
            {
                success = true,
                msg = "批量删除成功！"
            };
            return Json(result);
        }

        /*删除*/
        [HttpPost]
        [OperationLog(Message = "删除会员", ParameterNameList = "id")]
        public JsonResult Delete(long id)
        {
            ServiceHelper.Create<IMemberService>().DeleteMember(id);
            Result result = new Result()
            {
                success = true,
                msg = "删除成功！"
            };
            return Json(result);
        }

        /*账户激活管理页面*/
        public ActionResult AccountManager()
        {
            return View();
        }

        /*账户查询*/
        public JsonResult AccountList(int page, int rows, int disabled, string username)
        {
            MemberQuery memberQuery = new MemberQuery()
            {
                Disabled = disabled,
                UserType = 0,
                keyWords = username,
                PageNo = page,
                PageSize = rows
            };

            PageModel<UserMemberInfo> members = ServiceHelper.Create<IMemberService>().GetMembers(memberQuery);
            IEnumerable<UserMemberInfo> models =
                from item in members.Models.ToArray()
                select new UserMemberInfo()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    CreateDate = item.CreateDate,
                    Email = item.Email,
                    Disabled = item.Disabled
                };
            DataGridModel<UserMemberInfo> dataGridModel = new DataGridModel<UserMemberInfo>()
            {
                rows = models,
                total = members.Total
            };
            return Json(dataGridModel);
        }

        /*激活*/
        [HttpPost]
        [UnAuthorize]
        public JsonResult ActiveUser(long Id)
        {
            UserMemberInfo userMemberInfo = ServiceHelper.Create<IMemberService>().GetMember(Id);

            if (userMemberInfo != null)
            {
                userMemberInfo.Disabled = true;
                ServiceHelper.Create<IMemberService>().UpdateMember(userMemberInfo);
            }
            return Json(new { success = true });
        }
        /// <summary>
        /// 批量激活
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public JsonResult ActiveUsers(string param)
        {
            if(String.IsNullOrEmpty(param)){
                return Json(new { success = false });
            }
            Array Ids = param.Split(',');
            bool res = true;
            foreach(string id in Ids){
                UserMemberInfo userMemberInfo = ServiceHelper.Create<IMemberService>().GetMember(Convert.ToInt32(id));
                if (userMemberInfo != null)
                {
                    userMemberInfo.Disabled = true;
                    ServiceHelper.Create<IMemberService>().UpdateMember(userMemberInfo);
                }
                else {
                    res = false;
                }
            }
            return Json(new { success = res });
        }
    }
}