using ChemCloud.Model;
using ChemCloud.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface ICOAListService : IService, IDisposable
	{
        COAList GetCoAReportInfo(string CoANo);
        void AddCoAReportInfo(COAList cr);

        void DeleteCoAReportInfo(long id);
        PageModel<COAList> GetCoAReportInfo(COAListQuery opQuery,long userid);

        int GetUserCount(long id);

        List<COAList> GetCoaCountByCASNo(long id, string casNo);

        List<COAList> GetUserCoa(long id);
        List<COAList> GetUserCoaByCoaNo(long id, string CoaNo);
        bool IsExits(string key);		
	}
}