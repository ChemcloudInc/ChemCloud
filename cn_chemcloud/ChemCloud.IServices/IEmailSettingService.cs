using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
    public interface IEmailSettingService : IService, IDisposable
    {
        ChemCloud_EmailSetting GetChemCloud_EmailSetting();

        bool EditEmailSetting(ChemCloud_EmailSetting model);

        bool AddEmailSetting(ChemCloud_EmailSetting model);
    }
}
