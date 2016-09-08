using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;

namespace ChemCloud.IServices
{
    public interface IMessageSettingService : IService, IDisposable
    {
        bool AddModule(MessageSetting messageSettingInfo);
        MessageSetting GetSetting(long Id);
        MessageSetting GetSettingByMessageNameId(ChemCloud.Model.MessageSetting.MessageModuleStatus MessageNameId);
        MessageSetting GetSettingByLanguageType(ChemCloud.Model.MessageSetting.MessageModuleStatus MessageNameId, long LanguageType);
        bool UpdateMessageSetting(MessageSetting messageSetting);
        PageModel<MessageSetting> GetSettings(MessageSettingQuery messageSettingQueryModel);
        void DeleteMessageSetting(long Id);
        string GetMessageByMessageModule(int messageModule);
        bool ActiveStatus(long Id, int titleId, int languageType);

        bool UnActiveStatus(long Id, int titleId, int languageType);

        void SetOtherUnActiveStatus(long Id, int titleId, int languageType);
    }
}
