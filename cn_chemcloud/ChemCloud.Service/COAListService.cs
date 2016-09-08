using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChemCloud.Service
{
    public class COAListService : ServiceBase, ICOAListService, IService, IDisposable
    {
        public COAListService() { }
        public COAList GetCoAReportInfo(string CoANo)
        {
            return (from a in context.COAList where a.CoANo == CoANo select a).FirstOrDefault<COAList>();
        }

        public void AddCoAReportInfo(COAList cr)
        {
            if (cr == null)
            {
                return;
            }
            context.COAList.Add(cr);
            context.SaveChanges();
        }


        public PageModel<COAList> GetCoAReportInfo(COAListQuery crQuery, long userid)
        {
            int pageNum = 0;
            IQueryable<COAList> CoaR = context.COAList.AsQueryable<COAList>();
            if (!string.IsNullOrWhiteSpace(crQuery.CoANo))
            {
                CoaR = (from a in CoaR where a.CoANo.Contains(crQuery.CoANo) select a);
            }
            if (!string.IsNullOrWhiteSpace(crQuery.BeginTime.ToString()) && !crQuery.BeginTime.ToString().Contains("0001/1/1 0:00:00"))
            {
                CoaR = (from a in CoaR where a.Addtime >= crQuery.BeginTime select a);
            }
            if (!string.IsNullOrWhiteSpace(crQuery.EndTime.ToString()) && !crQuery.EndTime.ToString().Contains("0001/1/1 0:00:00"))
            {
                CoaR = (from a in CoaR where a.Addtime <= crQuery.EndTime select a);
            }
            if (!string.IsNullOrWhiteSpace(crQuery.CASNo))
            {
                CoaR = (from a in CoaR where a.CASNo.Contains(crQuery.CASNo) select a);
            }
            CoaR = (from a in CoaR where a.UserId == userid select a);
            CoaR = CoaR.GetPage(out pageNum, crQuery.PageNo, crQuery.PageSize, (IQueryable<COAList> d) =>
               from o in d
               orderby o.Addtime descending
               select o);
            return new PageModel<COAList>
            {
                Models = CoaR,
                Total = pageNum

            };
        }


        public void DeleteCoAReportInfo(long id)
        {
            COAList cr = context.COAList.FindById<COAList>(id);
            if (cr.Id == 0 || cr == null)
            {
                return;
            }
            context.COAList.Remove(cr);
            context.SaveChanges();
        }

        public List<COAList> GetCoaCountByCASNo(long id, string casNo)
        {
            IQueryable<COAList> CoaInfos = context.COAList.FindBy((COAList m) => m.UserId == id && m.CASNo == casNo);
            return CoaInfos.ToList();
        }
        public int GetUserCount(long id)
        {
            DateTime dt = DateTime.Now.Date;
            return (from a in context.COAList where a.UserId == id && a.Addtime >= dt select a).ToList<COAList>().Count;
        }

        public List<COAList> GetUserCoa(long id)
        {
            return (from a in context.COAList where a.UserId == id && a.AddUserType == 2 select a).ToList<COAList>();
        }



        public bool IsExits(string key)
        {
            return (from a in context.COAList where a.CoANo == key select a).FirstOrDefault<COAList>() != null;
        }


        public List<COAList> GetUserCoaByCoaNo(long id, string CoaNo)
        {
            return (from a in context.COAList where a.UserId == id && a.CASNo == CoaNo && a.AddUserType == 2 select a).ToList<COAList>();
        }
    }
}