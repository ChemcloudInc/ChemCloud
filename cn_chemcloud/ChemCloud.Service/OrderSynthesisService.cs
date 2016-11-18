using ChemCloud.Model.Common;
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
using System.Data.Entity.Validation;

namespace ChemCloud.Service
{
    public class OrderSynthesisService : ServiceBase, IOrderSynthesisService, IService, IDisposable
    {
        /// <summary>
        /// 新增采购订单
        /// </summary>
        /// <param name="op"></param>
        public OrderSynthesis AddOrderSynthesis(OrderSynthesis op)
        {

            OrderSynthesis OrderSynInfos = new OrderSynthesis();
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
                        OrderSynInfos = context.OrderSynthesis.Add(OrderSynInfos);
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
        public void UpdateOrderSynthesis(OrderSynthesis op)
        {
            if (op == null || op.Id == 0)
            {
                return;
            }
            OrderSynthesis OP = context.OrderSynthesis.FirstOrDefault((OrderSynthesis m) => m.Id == op.Id);
            if (OP == null)
            {
                return;
            }
            OP.ProductName = op.ProductName;
            OP.ProductCount = op.ProductCount;
            OP.ProductPurity = op.ProductPurity;
            OP.ProductDesc = op.ProductDesc;
            OP.OrderTime = op.OrderTime;
            OP.Email = op.Email;
            OP.CompanyName = op.CompanyName;
            OP.ConUserName = op.ConUserName;
            OP.ConWebsite = op.ConWebsite;
            OP.ConTelPhone = op.ConTelPhone;
            OP.Status = op.Status;
            OP.IsLock = op.IsLock;
            OP.UserId = op.UserId;
            OP.Price = op.Price;
            OP.Mol = op.Mol;
            OP.CASNo = op.CASNo;
            OP.ChemName = op.ChemName;
            OP.UserId = op.UserId;
            OP.OrderNumber = op.OrderNumber;
            OP.OperatorMessage = op.OperatorMessage;
            OP.ZhifuImg = op.ZhifuImg;
            OP.ExpressConpanyName = op.ExpressConpanyName;
            OP.ShipOrderNumber = op.ShipOrderNumber;
            OP.SynthesisCost = op.SynthesisCost;
            OP.SynthesisCycle = op.SynthesisCycle;
            context.SaveChanges();
        }

        public List<AttachmentInfo> GetAttachmentInfosById(long Id)
        {
            IQueryable<AttachmentInfo> attachmentInfos = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == 4);
            return attachmentInfos.ToList();
        }
        public List<AttachmentInfo> GetAttachmentInfosById(long Id,int type)
        {
            IQueryable<AttachmentInfo> attachmentInfos = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == type);
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
        public Model.PageModel<OrderSynthesis> GetOrderSynthesisList(OrderSynthesisQuery opQuery)
        {
            int num = 0;
            IQueryable<OrderSynthesis> orderP = context.OrderSynthesis.AsQueryable<OrderSynthesis>();
            if (!string.IsNullOrWhiteSpace(opQuery.OrderNumber))
            {
                orderP =
                from d in orderP
                where d.OrderNumber.Contains(opQuery.OrderNumber)
                orderby d.OrderTime descending
                select d;
            }

            if (!string.IsNullOrWhiteSpace(opQuery.ShopId))
            {
                orderP =
                from d in orderP
                where d.ZhifuImg.Equals(opQuery.ShopId)
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

            if (!string.IsNullOrWhiteSpace(opQuery.userId.ToString()) && opQuery.userId != 0)
            {
                orderP =
                from d in orderP
                where d.UserId == opQuery.userId
                orderby d.OrderTime descending
                select d;
            }
            orderP = orderP.GetPage(out num, opQuery.PageNo, opQuery.PageSize, (IQueryable<OrderSynthesis> d) =>
                from o in d
                orderby o.OrderTime descending
                select o);
            return new PageModel<OrderSynthesis>()
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
        public OrderSynthesis GetOrderSynthesis(long Id)
        {
            return context.OrderSynthesis.FindById<OrderSynthesis>(Id);
        }

        public void DelOrderInfo(long id)
        {

            OrderSynthesis opinfo = context.OrderSynthesis.FindById(id);
            if (opinfo != null)
            {
                context.OrderSynthesis.Remove(opinfo);
                context.SaveChanges();
            }
        }

        public Result_List<OrderSynthesis_Index> GetTopNumOrderSynthesis(int count)
        {
            Result_List<OrderSynthesis_Index> resList = new Result_List<OrderSynthesis_Index>();
            var topCountList = (from p in context.OrderItemInfo where p.ProductName != "" && p.ProductName != null orderby p.Id descending select p).ToList().Take(count);
            resList.List = topCountList.Select(x => new OrderSynthesis_Index()
            {
                ProductName = x.ProductName.Length > 18 ? (x.ProductName.Substring(0, 18) + "...") : x.ProductName,
                ProductCount = x.Quantity.ToString(),
                PackingUnit = x.PackingUnit,
                OrderTime = Convert.ToDateTime((from p in context.OrderInfo where p.Id == x.OrderId select p.OrderDate).FirstOrDefault()),
                Status = GetStatusNameByValue(Convert.ToInt32((from p in context.OrderInfo where p.Id == x.OrderId select p.OrderStatus).FirstOrDefault()))
            }).OrderByDescending(x => x.OrderTime).ToList();
            return resList;
        }

        public string GetStatusNameByValue(int status)
        {
            string strName = string.Empty;
            if (status == 0)
            {
                strName = "未确认";
            }
            else if (status == 1)
            {
                strName = "已确认";
            }
            else if (status == 2)
            {
                strName = "未付款";
            }
            else if (status == 3)
            {
                strName = "已付款";
            }
            else if (status == 4)
            {
                strName = "未发货";
            }
            else if (status == 5)
            {
                strName = "已发货";
            }
            else if (status == 6)
            {
                strName = "未签收";
            }
            else if (status == 7)
            {
                strName = "已签收";
            }
            return strName;
        }

        /*热销产品*/
        public Result_List<OrderSynthesis_Index> GetHotSelling()
        {

            Result_List<OrderSynthesis_Index> resList = new Result_List<OrderSynthesis_Index>();
            var topCountList = (from p in context.OrderItemInfo
                                where p.ProductName != "" && p.ProductName != null && (from q in context.ProductInfo select q.Id).ToList().Contains(p.ProductId)
                                group p by p.ProductId into g
                                select new
                                {
                                    ProductId = g.Key,
                                    Quantity = g.Sum(p => p.Quantity)
                                }
                ).ToList();

            ProductService _ProductService = new ProductService();

            resList.List = topCountList.Take(10).Select(p => new OrderSynthesis_Index()
            {
                ProductName = _ProductService.GetProduct(p.ProductId) == null ? "" : _ProductService.GetProduct(p.ProductId).CASNo,
                SellNum = p.Quantity

            }).OrderByDescending(x => x.SellNum).ToList();
            
            resList.List = (from p in resList.List
                            where p.ProductName != "" && p.ProductName != null
                            select new OrderSynthesis_Index
                            {
                                ProductName = p.ProductName.Length > 18 ? (p.ProductName.Substring(0, 18) + "...") : p.ProductName,
                                SellNum = p.SellNum
                            }).ToList();

            return resList;
        }

        /*采购商接受报价*/
        public bool UpdateAcceptCustomizedOrder(OrderSynthesis _OrderSynthesis)
        {
            /*   
             * strsql = string.Format("update dbo.ChemCloud_OrderSynthesis set ZhifuImg='" + shopid + "',Price='" + price + "',SynthesisCycle='" + cycel + "',Status=2 where Id='" + orderid + "';");
            */
            bool result = false;
            OrderSynthesis model = context.OrderSynthesis.FindById(_OrderSynthesis.Id);
            if (model != null)
            {
                model.Status = _OrderSynthesis.Status;
                model.ZhifuImg = _OrderSynthesis.ZhifuImg;
                model.Price = _OrderSynthesis.Price;
                model.SynthesisCycle = _OrderSynthesis.SynthesisCycle;
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        /*供应商发货*/
        public bool OrderSynthesis_DeliverGoods(OrderSynthesis _OrderSynthesis)
        {
            /*   
             * update dbo.ChemCloud_OrderSynthesis set Status='" + status + "',ExpressConpanyName='" + exname + "',ShipOrderNumber='" + exid + "'  where Id='" + id + "';
            */
            bool result = false;
            OrderSynthesis model = context.OrderSynthesis.FindById(_OrderSynthesis.Id);
            if (model != null)
            {
                model.Status = _OrderSynthesis.Status;
                model.ExpressConpanyName = _OrderSynthesis.ExpressConpanyName;
                model.ShipOrderNumber = _OrderSynthesis.ShipOrderNumber;
                context.SaveChanges();
                result = true;
            }
            return result;
        }
    }
}
