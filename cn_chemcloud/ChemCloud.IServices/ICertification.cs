
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;


namespace ChemCloud.IServices
{
    public interface ICertification:IService, IDisposable
    {
     //   void AddFieldModule(FieldCertification fieldCModuleInfo);
        FieldCertification CreateFieldModulep();
        void UpdateCertification(FieldCertification certification);
        FieldCertification GetCertification(long id);
        PageModel<FieldCertification> GetCertifications(FieldCertificationQuery FieldCertificationQuery);
        void UpdateStatus(long Id, FieldCertification.CertificationStatus status, DateTime Date, string comments = "");
        bool ExistCertification(string CompanyName, long Id = 0L);
        ManagerInfo GetManager(long Id);
    }
}