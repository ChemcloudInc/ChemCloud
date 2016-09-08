using ChemCloud.IServices;
using ChemCloud.ServiceProvider;
using Quartz;
using System;

namespace ChemCloud.Service.Job
{
	public class OrderJob : IJob
	{
		public OrderJob()
		{
		}

		public void Execute(IJobExecutionContext context)
		{
			IOrderService create = Instance<IOrderService>.Create;
			create.AutoCloseOrder();
			create.AutoConfirmOrder();
			Instance<IGiftsOrderService>.Create.AutoConfirmOrder();
		}
	}
}