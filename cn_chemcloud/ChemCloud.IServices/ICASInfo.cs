using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface ICASInfoService : IService, IDisposable
    {
        CASInfo GetCASByNo(string CASNo);

        CASInfo GetCASByPUB_CID(int PUB_CID);

        string GetCASByInchikey(string Inchikey);

        PageModel<CASInfo> GetCasList(CASInfoQuery casQuery);

        PageModel<CASInfo> PageSearchCasList(CASInfoQuery casQuery);

        int SearchCasInfoListCount(CASInfoQuery casQuery, string islike);
        PageModel<CASInfo> SearchCasInfoList(CASInfoQuery casQuery, string islike);

        CASInfo GetCASByCAS_NO(string CASNo);

        CASInfo GetCASInfoByCid(int cid);

        //void UpdateCASStatus(string CAS_NO, string CAS_STATUS);

        void UpdateCAS(CASInfo casinfo);

        void AddCAS(CASInfo casinfo);

        CASInfo GetCASByCID(string CID);
        CASInfo GetCASByCASNO(string CAS_NO);
    }
}
