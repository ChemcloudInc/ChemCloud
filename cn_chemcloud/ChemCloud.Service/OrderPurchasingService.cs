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
using System.Data.Entity.Validation;
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
    public class OrderPurchasingService : ServiceBase, IOrderPurchasingService, IService, IDisposable
    {
        /// <summary>
        /// 新增采购订单
        /// </summary>
        /// <param name="op"></param>
        public OrderPurchasing AddOrderPurchasing(OrderPurchasing op)
        {
            OrderPurchasing OrderSynInfos = new OrderPurchasing();
            if (op == null || op.Id != 0)
            {
                return OrderSynInfos;
            }
            else
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        OrderSynInfos = op;
                        OrderSynInfos = context.OrderPurchasing.Add(OrderSynInfos);
                        context.SaveChanges();
                        transactionScope.Complete();
                    }
                    catch (DbEntityValidationException dbEx) { }
                }
                return OrderSynInfos;
            }
        }
        /// <summary>
        /// 修改采购订单
        /// </summary>
        /// <param name="op"></param>
        public void UpdateOrderPurchasing(OrderPurchasing op)
        {
            if (op == null || op.Id == 0)
            {
                return;
            }
            OrderPurchasing OP = context.OrderPurchasing.FirstOrDefault((OrderPurchasing m) => m.Id == op.Id);
            if (OP == null)
            {
                return;
            }
            OP.ProductName = op.ProductName;
            OP.ProductCount = op.ProductCount;
            OP.ProductPurity = op.ProductPurity;
            OP.ProductDesc = op.ProductDesc;
            OP.OrderTime = op.OrderTime;
            OP.BeginTime = op.BeginTime;
            OP.EndTime = op.EndTime;
            OP.Email = op.Email;
            OP.CompanyName = op.CompanyName;
            OP.ConUserName = op.ConUserName;
            OP.ConWebsite = op.ConWebsite;
            OP.ConTelPhone = op.ConTelPhone;
            OP.Status = op.Status;
            OP.IsLock = op.IsLock;
            OP.UserId = op.UserId;
            OP.ProductPrice = op.ProductPrice;
            OP.OrderNum = op.OrderNum;
            OP.ReplyContent = op.ReplyContent;
            OP.ZhifuImg = op.ZhifuImg;
            OP.KuaiDi = op.KuaiDi;
            OP.KuaiDiNo = op.KuaiDiNo;

            OP.DeliveryDate = op.DeliveryDate;
            OP.Cost = op.Cost;

            context.SaveChanges();
        }
        public void DelOrderInfo(long id)
        {

            OrderPurchasing opinfo = context.OrderPurchasing.FindById(id);
            if (opinfo != null)
            {
                context.OrderPurchasing.Remove(opinfo);
                context.SaveChanges();
            }
        }
        public List<AttachmentInfo> GetAttachmentInfosById(long Id)
        {
            IQueryable<AttachmentInfo> attachmentInfos = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == 5);
            return attachmentInfos.ToList();
        }
        public bool AddAttachment(AttachmentInfo model)
        {
            context.AttachmentInfo.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 采购订单列表查询
        /// </summary>
        /// <param name="opQuery"></param>
        /// <returns></returns>
        public Model.PageModel<OrderPurchasing> GetOrderPurchasingList(OrderPurchasingQuery opQuery)
        {
            int num = 0;
            IQueryable<OrderPurchasing> orderP = context.OrderPurchasing.AsQueryable<OrderPurchasing>();
            if (!string.IsNullOrWhiteSpace(opQuery.orderNum))
            {
                orderP =
                from d in orderP
                where d.OrderNum.Contains(opQuery.orderNum)
                orderby d.OrderTime descending
                select d;
            }
            if (!string.IsNullOrWhiteSpace(opQuery.status.ToString()) && opQuery.status != -1)
            {
                orderP =
                from d in orderP
                where d.Status == opQuery.status
                orderby d.OrderTime descending
                select d;
            }
            if (!string.IsNullOrWhiteSpace(opQuery.beginTime.ToString()) && !opQuery.beginTime.ToString().Contains("0001/1/1 0:00:00"))
            {
                orderP =
                from d in orderP
                where d.BeginTime >= opQuery.beginTime
                orderby d.OrderTime descending
                select d;
            }
            if (!string.IsNullOrWhiteSpace(opQuery.endTime.ToString()) && !opQuery.endTime.ToString().Contains("0001/1/1 0:00:00"))
            {
                orderP =
                from d in orderP
                where d.EndTime < opQuery.endTime
                orderby d.OrderTime descending
                select d;
            }
            if (!string.IsNullOrWhiteSpace(opQuery.userId.ToString()) && opQuery.userId != 0)
            {
                orderP =
                from d in orderP
                where d.UserId == opQuery.userId
                orderby d.OrderTime descending
                select d;
            }
            orderP = orderP.GetPage(out num, opQuery.PageNo, opQuery.PageSize, (IQueryable<OrderPurchasing> d) =>
                from o in d
                orderby o.OrderTime descending
                select o);
            return new PageModel<OrderPurchasing>()
            {

                Models = orderP,
                Total = num
            };
        }
        /// <summary>
        /// 采购订单信息查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OrderPurchasing GetOrderPurchasing(long Id)
        {
            return context.OrderPurchasing.FindById<OrderPurchasing>(Id);
        }

        /*采购商接受报价*/
        public bool Accep_OrderPurchasing(OrderPurchasing _OrderPurchasing)
        {
            bool result = false;
            OrderPurchasing model = context.OrderPurchasing.FindById(_OrderPurchasing.Id);
            if (model != null)
            {
                model.Status = _OrderPurchasing.Status;
                model.ZhifuImg = _OrderPurchasing.ZhifuImg;
                model.ProductPrice = _OrderPurchasing.ProductPrice;
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        /*供应商发货*/
        public bool OrderPurchasing_DeliverGoods(OrderPurchasing _OrderPurchasing)
        {
            bool result = false;
            OrderPurchasing model = context.OrderPurchasing.FindById(_OrderPurchasing.Id);
            if (model != null)
            {
                model.Status = _OrderPurchasing.Status;
                model.KuaiDi = _OrderPurchasing.KuaiDi;
                model.KuaiDiNo = _OrderPurchasing.KuaiDiNo;
                context.SaveChanges();
                result = true;
            }
            return result;
        }
    }
}
