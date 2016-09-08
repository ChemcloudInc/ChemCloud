using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Linq;

namespace ChemCloud.IServices
{
	public interface ITradeCommentService : IService, IDisposable
	{
		void AddOrderComment(OrderCommentInfo info);

		void DeleteOrderComment(long id);

		OrderCommentInfo GetOrderCommentInfo(long orderId, long userId);

		PageModel<OrderCommentInfo> GetOrderComments(OrderCommentQuery query);

		IQueryable<OrderCommentInfo> GetOrderComments(long userId);
	}
}