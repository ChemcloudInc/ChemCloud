using ChemCloud.Core;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
	public class TradeCommentService : ServiceBase, ITradeCommentService, IService, IDisposable
	{
		public TradeCommentService()
		{
		}

		public void AddOrderComment(OrderCommentInfo info)
		{
			OrderInfo orderInfo = (
				from a in context.OrderInfo
				where a.Id == info.OrderId && a.UserId == info.UserId
				select a).FirstOrDefault();
			if (orderInfo == null)
			{
				throw new HimallException("该订单不存在，或者不属于该用户！");
			}
			if ((
				from a in context.OrderCommentInfo
				where a.OrderId == info.OrderId && a.UserId == info.UserId
				select a).Count() > 0)
			{
				throw new HimallException("您已经评论过该订单！");
			}
			info.ShopId = orderInfo.ShopId;
			info.ShopName = orderInfo.ShopName;
			info.UserName = orderInfo.UserName;
			info.CommentDate = DateTime.Now;
            context.OrderCommentInfo.Add(info);
            context.SaveChanges();
			MemberIntegralRecord memberIntegralRecord = new MemberIntegralRecord()
			{
				UserName = info.UserName,
				ReMark = string.Concat("订单号:", info.OrderId),
				MemberId = info.UserId,
				RecordDate = new DateTime?(DateTime.Now),
				TypeId = MemberIntegral.IntegralType.Comment
			};
			MemberIntegralRecordAction memberIntegralRecordAction = new MemberIntegralRecordAction()
			{
				VirtualItemTypeId = new MemberIntegral.VirtualItemType?(MemberIntegral.VirtualItemType.Comment),
				VirtualItemId = info.OrderId
			};
			memberIntegralRecord.ChemCloud_MemberIntegralRecordAction.Add(memberIntegralRecordAction);
			IConversionMemberIntegralBase conversionMemberIntegralBase = Instance<IMemberIntegralConversionFactoryService>.Create.Create(MemberIntegral.IntegralType.Comment, 0);
			Instance<IMemberIntegralService>.Create.AddMemberIntegral(memberIntegralRecord, conversionMemberIntegralBase);
		}

		public void DeleteOrderComment(long Id)
		{
			OrderCommentInfo orderCommentInfo = context.OrderCommentInfo.FindById<OrderCommentInfo>(Id);
			if (orderCommentInfo != null)
			{
				List<long?> list = (
					from d in context.OrderItemInfo.FindBy((OrderItemInfo d) => d.OrderId == orderCommentInfo.OrderId)
					select (long?)d.Id).ToList<long?>();
				List<ProductCommentInfo> productCommentInfos = context.ProductCommentInfo.FindBy((ProductCommentInfo d) => list.Contains(d.SubOrderId)).ToList();
                context.ProductCommentInfo.RemoveRange(productCommentInfos);
                context.OrderCommentInfo.Remove(orderCommentInfo);
                context.SaveChanges();
			}
		}

		public OrderCommentInfo GetOrderCommentInfo(long orderId, long userId)
		{
			return (
				from a in context.OrderCommentInfo
				where a.UserId == userId && a.OrderId == orderId
				select a).FirstOrDefault();
		}

		public PageModel<OrderCommentInfo> GetOrderComments(OrderCommentQuery query)
		{
			int num;
			IQueryable<OrderCommentInfo> orderId = context.OrderCommentInfo.AsQueryable<OrderCommentInfo>();
			if (query.OrderId.HasValue)
			{
				orderId = 
					from item in orderId
					where query.OrderId == item.OrderId
                    select item;
			}
			if (query.StartDate.HasValue)
			{
				orderId = 
					from item in orderId
					where query.StartDate <= item.CommentDate
                    select item;
			}
			if (query.EndDate.HasValue)
			{
				orderId = 
					from item in orderId
					where query.EndDate >= item.CommentDate
                    select item;
			}
			if (query.ShopId.HasValue)
			{
				orderId = 
					from item in orderId
					where query.ShopId == item.ShopId
                    select item;
			}
			if (query.UserId.HasValue)
			{
				orderId = 
					from item in orderId
					where query.UserId == item.UserId
                    select item;
			}
			if (!string.IsNullOrWhiteSpace(query.ShopName))
			{
				orderId = 
					from item in orderId
					where item.ShopName.Contains(query.ShopName)
					select item;
			}
			if (!string.IsNullOrWhiteSpace(query.UserName))
			{
				orderId = 
					from item in orderId
					where item.UserName.Contains(query.UserName)
					select item;
			}
            
			orderId = orderId.GetPage(out num, query.PageNo, query.PageSize, null);
            foreach (OrderCommentInfo info in orderId.ToList())
            {
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(info.ShopId));
                info.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
            }
			return new PageModel<OrderCommentInfo>()
			{
				Models = orderId,
				Total = num
			};
		}

		public IQueryable<OrderCommentInfo> GetOrderComments(long userId)
		{
			return 
				from item in context.OrderCommentInfo
				where item.UserId == userId
				select item;
		}
	}
}