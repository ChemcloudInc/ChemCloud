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
    public class TKService : ServiceBase, ITKService, IService, IDisposable
    {
        public long InsertTK(Model.TK tk)
        {
            context.TK.Add(tk);
            context.SaveChanges();
            return tk.Id;
        }

        public long InsertTKMessage(Model.TKMessage tkm)
        {
            context.TKMessage.Add(tkm);
            context.SaveChanges();
            return tkm.Id;
        }

        public void InsertTKImage(List<Model.TKImageInfo> tkimages)
        {
            context.TKImageInfo.AddRange(tkimages);
            context.SaveChanges();
        }

        public void UpdateTK(long id, int type)
        {
            TK tk = (from a in context.TK where a.OrderId == id select a).FirstOrDefault<TK>();
            tk.TKType = type;
            context.SaveChanges();
        }

        public bool DeleteTK(long OrderId)
        {
            bool resut = true;
            try
            {
                TK _tk = (from p in context.TK where p.OrderId == OrderId select p).ToList().FirstOrDefault();
                context.TK.Remove(_tk);
                context.SaveChanges();
            }
            catch (Exception)
            {
                resut = false;
            }
            return resut;
        }

        public List<Model.TKMessage> getTKMessage(long tkid)
        {

            List<Model.TKMessage> tks = (from a in context.TKMessage where a.TKId == tkid select a).ToList<TKMessage>();
            return tks;
        }

        public List<Model.TKImageInfo> getTKImage(long TKMessageId)
        {
            List<TKImageInfo> tkis = (from a in context.TKImageInfo where a.TKMessageId == TKMessageId select a).ToList<TKImageInfo>();
            return tkis;
        }


        public TK getTK(long orderNo)
        {
            return (from a in context.TK where a.OrderId == orderNo select a).FirstOrDefault<TK>();
        }


        public PageModel<TK> getTkList(QueryModel.TKQuery tq, long UserId, long SellerUserId)
        {
            IQueryable<TK> tk = (from a in context.TK select a);
            if (UserId != 0)
            {
                tk = (from a in tk where a.UserId == UserId select a);
            }
            if (SellerUserId != 0)
            {
                tk = (from a in tk where a.SellerUserId == SellerUserId select a);
            }
            int pageNum = 0;


            if (tq.TKType != 0)
            {
                tk = (from u in tk
                      where u.TKType == tq.TKType
                      select u);
            }
            if (tq.OrderNo != 0)
            {
                tk = (from u in tk
                      where u.OrderId == tq.OrderNo
                      select u);
            }

            tk = tk.GetPage(out pageNum, tq.PageNo, tq.PageSize, (IQueryable<TK> d) =>
              from o in tk
              orderby o.TKDate descending
              select o);
            return new PageModel<TK>
            {
                Models = tk,
                Total = pageNum

            };

        }
    }
}
