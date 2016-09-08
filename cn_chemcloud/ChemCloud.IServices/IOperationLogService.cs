using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface IOperationLogService : IService, IDisposable
	{
		void AddPlatformOperationLog(LogInfo info);

		void AddSellerOperationLog(LogInfo info);

		void DeletePlatformOperationLog(long id);

		PageModel<LogInfo> GetPlatformOperationLogs(OperationLogQuery query);
	}
}