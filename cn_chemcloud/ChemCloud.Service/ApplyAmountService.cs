using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Model.Common;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChemCloud.Service
{
    public class ApplyAmountService : ServiceBase, IApplyAmountService, IService, IDisposable
    {
        public ApplyAmountInfo GetApplyById(long Id)
        {
            ApplyAmountInfo ApplyAmount = context.ApplyAmountInfo.FirstOrDefault((ApplyAmountInfo m) => m.Id == Id);
            UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(ApplyAmount.ApplyUserId));
            ApplyAmount.ApplyName = (userInfo == null ? "" : userInfo.UserName);
            if (ApplyAmount != null)
            {
                UserMemberInfo userInfos = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(ApplyAmount.AuthorId));
                ApplyAmount.AuthorName = (userInfo == null ? "" : userInfos.UserName);
                ChemCloud_Dictionaries dicts = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 1 && m.DValue == ApplyAmount.CoinType.ToString());
                ApplyAmount.CoinName = dicts.DKey;
            }
            return ApplyAmount;
        }
        public ApplyAmountInfo GetApplyByOrderId(long userId, long orderId)
        {
            ApplyAmountInfo ApplyAmount = context.ApplyAmountInfo.FirstOrDefault((ApplyAmountInfo m) => m.ApplyUserId == userId && m.OrderId == orderId);
            return ApplyAmount;
        }
        public ApplyAmountInfo GetApplyByUserId(long UserId, long OrderId)
        {
            ApplyAmountInfo ApplyAmount = context.ApplyAmountInfo.FirstOrDefault((ApplyAmountInfo m) => m.ApplyUserId == UserId && m.OrderId == OrderId);
            return ApplyAmount;
        }
        /// <summary>
        /// 限额申请列表
        /// </summary>
        /// <param name="Status">申请状态</param>
        /// <param name="Start_datetime">开始时间</param>
        /// <param name="End_datetime">结束时间</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="userId">当前用户</param>
        /// <param name="Applicant">申请人（0：子账户申请；1：我的申请）</param>
        /// <returns></returns>
        public PageModel<ApplyAmountInfo> GetApplyAmounts(int? Status, DateTime? Start_datetime, DateTime? End_datetime, int page, int rows, long userId, int Applicant)
        {
            PageModel<ApplyAmountInfo> res = new PageModel<ApplyAmountInfo>();
            IQueryable<ApplyAmountInfo> GetApplyAmounts = from item in base.context.ApplyAmountInfo
                                                          //where item.AuthorId == userId
                                                          select item;
            if (Applicant == 0)
            {
                GetApplyAmounts = from d in GetApplyAmounts
                                  where d.AuthorId == userId
                                  select d;
            }
            else
            {
                GetApplyAmounts = from d in GetApplyAmounts
                                  where d.ApplyUserId == userId
                                  select d;
            }
            if (Status.HasValue && Status.Value != 3)
            {
                GetApplyAmounts = from d in GetApplyAmounts
                                  where (int)d.ApplyStatus == Status.Value
                                  select d;
            }
            if (Start_datetime != null && End_datetime != null)
            {
                End_datetime = Convert.ToDateTime(End_datetime).AddDays(1).AddMilliseconds(-1);
                GetApplyAmounts = GetApplyAmounts.Where(x => x.ApplyDate >= Start_datetime && x.ApplyDate <= End_datetime).OrderByDescending(x => x.ApplyDate);
            }
            else if (Start_datetime != null)
            {
                GetApplyAmounts = GetApplyAmounts.Where(x => x.ApplyDate >= Start_datetime).OrderByDescending(x => x.ApplyDate);
            }
            else if (End_datetime != null)
            {
                End_datetime = Convert.ToDateTime(End_datetime).AddDays(1).AddMilliseconds(-1);
                GetApplyAmounts = GetApplyAmounts.Where(x => x.ApplyDate <= End_datetime).OrderByDescending(x => x.ApplyDate);
            }

            Func<IQueryable<ApplyAmountInfo>, IOrderedQueryable<ApplyAmountInfo>> func = null;
            func = (IQueryable<ApplyAmountInfo> d) =>
                    from o in d
                    orderby o.ApplyStatus ascending
                    select o;
            int num = GetApplyAmounts.Count();
            GetApplyAmounts = GetApplyAmounts.GetPage(out num, page, rows, func);
            foreach (ApplyAmountInfo list in GetApplyAmounts.ToList())
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(list.ApplyUserId));
                list.ApplyName = (userInfo == null ? "" : userInfo.UserName);
                if (list.AuthorId != 0)
                {
                    UserMemberInfo userInfos = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(list.AuthorId));
                    list.AuthorName = (userInfo == null ? "" : userInfos.UserName);
                    ChemCloud_Dictionaries dicts = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 1 && m.DValue == list.CoinType.ToString());
                    list.CoinName = dicts.DKey;
                }
            }
            res.Models = GetApplyAmounts;
            res.Total = num;
            return res;
        }
        public bool AddApplyAmount(ApplyAmountInfo ApplyAmount)
        {
            int i = 0;
            try
            {
                // 写数据库
                context.ApplyAmountInfo.Add(ApplyAmount);
                i = context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

            }
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool UpdateAuthor(long Id, long ParentId)
        {
            ApplyAmountInfo ApplyAmount = GetApplyById(Id);
            ApplyAmount.AuthorId = ParentId;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool UpdateApplyStatus(long Id, int status, long AuthorId)
        {
            ApplyAmountInfo ApplyAmount = GetApplyById(Id);
            ApplyAmount.AuthorId = AuthorId;
            ApplyAmount.AuthDate = DateTime.Now;
            ApplyAmount.ApplyStatus = status;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool Delete(long Id)
        {
            ApplyAmountInfo ApplyAmount = GetApplyById(Id);
            context.ApplyAmountInfo.Remove(ApplyAmount);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool BatchDelete(long[] Ids)
        {
            IQueryable<ApplyAmountInfo> ApplyAmount = context.ApplyAmountInfo.FindBy((ApplyAmountInfo item) => Ids.Contains(item.Id));
            context.ApplyAmountInfo.RemoveRange(ApplyAmount);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool IsPassAuth(long UserId, long OrderId)
        {
            ApplyAmountInfo ApplyAmount = context.ApplyAmountInfo.FirstOrDefault((ApplyAmountInfo m) => m.ApplyUserId == UserId && m.ApplyAmount == OrderId);
            if (ApplyAmount != null)
            {
                if (ApplyAmount.ApplyStatus == 1)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        #region 修改“限额申请”
        /// <summary>
        /// 修改“限额申请”
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Msg Update_Apply(QueryCommon<ApplyAmountInfo> query)
        {
            Result_Msg res = new Result_Msg();
            ApplyAmountInfo ApplyAmount = GetApplyById(query.ParamInfo.Id);
            ApplyAmount.ApplyDate = query.ParamInfo.ApplyDate;
            ApplyAmount.ApplyAmount = query.ParamInfo.ApplyAmount;
            ApplyAmount.CoinType = query.ParamInfo.CoinType;
            ApplyAmount.ApplyStatus = query.ParamInfo.ApplyStatus;

            int i = context.SaveChanges();

            if (i > 0)
            {
                res.IsSuccess = true;
            }
            else
            {
                res.IsSuccess = false;
            }
            return res;
        }
        #endregion
    }
}
