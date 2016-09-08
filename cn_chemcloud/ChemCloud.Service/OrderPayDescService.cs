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
    public class OrderPayDescService : ServiceBase, IOrderPayDescService, IService, IDisposable
    {
        public OrderPayDescService()
        {
        }

        public void Add(OrderPayDesc model)
        {
            if (model == null)
            {
                return;
            }
            context.OrderPayDesc.Add(model);
            context.SaveChanges();
        }

        public OrderPayDesc GetInfo(long orderId, long uid, int userType)
        {
            OrderPayDesc orderinfo = new OrderPayDesc();
            if (orderId != 0)
            {
                orderinfo = (from p in context.OrderPayDesc where p.OrderId == orderId select p).FirstOrDefault();
            }
            if (uid != 0)
            {
                orderinfo = (from p in context.OrderPayDesc where p.UserId == uid select p).FirstOrDefault();
            }
            if (userType != 0)
            {
                orderinfo = (from p in context.OrderPayDesc where p.UserType == userType select p).FirstOrDefault();
            }
            return orderinfo;
        }

        public void Update(OrderPayDesc model)
        {
            if (model == null)
            {
                return;
            }
            OrderPayDesc order = context.OrderPayDesc.FirstOrDefault((OrderPayDesc m) => m.Id == model.Id);
            if (order == null)
            {
                return;
            }
            order.OrderId = model.OrderId;
            order.UserId = model.UserId;
            order.UserType = model.UserType;
            order.PayTime = model.PayTime;
            order.ReciveTime = model.ReciveTime;
            order.TotalPrice = model.TotalPrice;
            order.RealPrice = model.RealPrice;
            order.Isfenqi = model.Isfenqi;
            order.Status = model.Status;
            order.CoinType = model.CoinType;
            context.SaveChanges();
        }
    }
}
