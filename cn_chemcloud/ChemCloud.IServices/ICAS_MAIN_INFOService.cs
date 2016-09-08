using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.QueryModel;

namespace ChemCloud.IServices
{
    public interface ICAS_MAIN_INFOService : IService, IDisposable
    {
        CAS_MAIN_INFO GetCASByNo(string CASNo);

        PageModel<CAS_MAIN_INFO> GetCasList(CAS_MAIN_INFOQuery casQuery);

        PageModel<CAS_MAIN_INFO> PageSearchCasList(CAS_MAIN_INFOQuery casQuery);

        int SearchCasInfoListCount(CAS_MAIN_INFOQuery casQuery, string islike);
        PageModel<CAS_MAIN_INFO> SearchCasInfoList(CAS_MAIN_INFOQuery casQuery, string islike);

        CAS_MAIN_INFO GetCASByCAS_NO(string CASNo);

        void UpdateCASStatus(string CAS_NO, string CAS_STATUS);

        void UpdateCAS(CAS_MAIN_INFO casinfo);

        void AddCAS(CAS_MAIN_INFO casinfo);

        CAS_MAIN_INFO GetCASByCID(string CID);
    }
}
