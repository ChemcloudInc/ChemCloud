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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace ChemCloud.Service
{
    public class FQPaymentService : ServiceBase, IFQPaymentService, IService, IDisposable
    {
        public FQPaymentService()
        {

        }
        public FQPayment GetFQPaymentInfo(long orderId, long uid)
        {
            FQPayment fpinfo = new FQPayment();
            fpinfo = (
            from p in context.FQPayment
            where p.orderId.Equals(orderId) && p.MemberId.Equals(uid)
            select p).FirstOrDefault();
            return fpinfo;
        }
        /// <summary>
        /// getlist
        /// </summary>
        /// <param name="fqQuery"></param>
        /// <returns></returns>
        public PageModel<FQPayment> GetFQPaymentListInfo(QueryModel.FQPaymentQuery fqQuery)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="fqinfo"></param>
        /// <returns></returns>
        public bool UpdateFQPayment(FQPayment fqinfo)
        {
            if (fqinfo == null)
            {
                return false;
            }
            FQPayment fq = context.FQPayment.FirstOrDefault((FQPayment m) => m.Id == fqinfo.Id);
            if (fq == null)
            {
                return false;
            }
            int i = 0;
            fq.orderId = fqinfo.orderId;
            fq.TolPrice = fqinfo.TolPrice;
            fq.RealPrice = fqinfo.RealPrice;
            fq.LeftPrice = fqinfo.LeftPrice;
            fq.MemberId = fqinfo.MemberId;
            fq.PayTime = fqinfo.PayTime;
            fq.Status = fqinfo.Status;
            i = context.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="fqinfo"></param>
        /// <returns></returns>
        public bool AddFQPayment(FQPayment fqinfo)
        {
            int i = 0;
            if (fqinfo == null || fqinfo.Id != 0)
            {
                return false;
            }
            context.FQPayment.Add(fqinfo);
            i = context.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
