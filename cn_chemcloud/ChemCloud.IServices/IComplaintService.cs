using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface IComplaintService : IService, IDisposable
	{
		void AddComplaint(OrderComplaintInfo info);

		void DealComplaint(long id);

		PageModel<OrderComplaintInfo> GetOrderComplaints(ComplaintQuery complaintQuery);

		void SellerDealComplaint(long id, string reply);

		void UserApplyArbitration(long id, long userId);

		void UserDealComplaint(long id, long userId);
	}
}