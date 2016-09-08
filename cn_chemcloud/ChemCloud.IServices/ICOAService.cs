using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface ICOAService : IService, IDisposable
    {
        void AddCOA(ChemCloud_COA model);//添加COA报告

        int SearchCOAListCount(string CertificateNumber, int PageNo, int PageSize);//COA查询页数

        PageModel<ChemCloud_COA> SearchCOAList(string CertificateNumber, int PageNo, int PageSize);//翻页查询



    }
}
