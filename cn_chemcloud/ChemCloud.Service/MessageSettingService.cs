using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class MessageSettingService : ServiceBase, IMessageSettingService, IService, IDisposable
    {
        public MessageSettingService()
        {

        }
        public bool AddModule(MessageSetting messageSettingInfo)
        {
            context.MessageSetting.Add(messageSettingInfo);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public string GetMessageByMessageModule(int messageModule)
        {
            string MessageContent = "";
            MessageSetting message = context.MessageSetting.FirstOrDefault((MessageSetting m) => m.MessageNameId == (MessageSetting.MessageModuleStatus)messageModule);
            if (message != null)
            {
                MessageContent = message.MessageContent;
            }
            return MessageContent;
        }

        public MessageSetting GetSetting(long Id)
        {
            MessageSetting nums = context.MessageSetting.FindById<MessageSetting>(Id);
            ChemCloud_Dictionaries dict = (from item in context.ChemCloud_Dictionaries
                                 where item.DValue == nums.Languagetype.ToString() && item.DictionaryTypeId == 10
                                 select item).FirstOrDefault();
            nums.LanguageName = dict.DKey;
            return nums;
        }

        public MessageSetting GetSettingByMessageNameId(ChemCloud.Model.MessageSetting.MessageModuleStatus MessageNameId)
        {
            MessageSetting nums = context.MessageSetting.Where(q => q.MessageNameId == MessageNameId).ToList().FirstOrDefault();
            return nums;
        }
        public MessageSetting GetSettingByLanguageType(ChemCloud.Model.MessageSetting.MessageModuleStatus MessageNameId, long LanguageType)
        {
            MessageSetting nums = context.MessageSetting.Where(q => q.MessageNameId == MessageNameId && q.Languagetype == LanguageType).ToList().FirstOrDefault();
            return nums;
        }
        public bool UpdateMessageSetting(MessageSetting messageSetting)
        {
            MessageSetting messageSettings = context.MessageSetting.FindById<MessageSetting>(messageSetting.Id);
            messageSettings.MessageContent = messageSetting.MessageContent;
            messageSettings.CreatDate = DateTime.Now;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public void DeleteMessageSetting(long Id)
        {
            context.MessageSetting.Remove(context.MessageSetting.FindById<MessageSetting>(Id));
            context.SaveChanges();
        }

        public PageModel<MessageSetting> GetSettings(MessageSettingQuery messageSettingQueryModel)
        {
            IQueryable<MessageSetting> MessageSettings = context.MessageSetting.AsQueryable<MessageSetting>();
            MessageSettings =
                    from item in MessageSettings
                    select item;
            if (messageSettingQueryModel.Id > 0)
            {
                MessageSettings =
                    from item in MessageSettings
                    where item.Id == messageSettingQueryModel.Id
                    select item;
            }
            if (messageSettingQueryModel.MessageNameId.HasValue && messageSettingQueryModel.MessageNameId.Value != 0)
            {
                MessageSetting.MessageModuleStatus value = messageSettingQueryModel.MessageNameId.Value;
                DateTime dateTime = DateTime.Now.Date.AddSeconds(-1);
                MessageSetting.MessageModuleStatus ActiveStatu = value;

                if (ActiveStatu == MessageSetting.MessageModuleStatus.OrderCreated)
                {
                    MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 1
                        select d;
                }
                else
                {
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.OrderPay)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 2
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.OrderShipping)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 3
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.OrderRefund)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 4
                        select d;
                    }
                    //if (ActiveStatu == MessageSetting.MessageModuleStatus.FindPassWord)
                    //{
                    //    MessageSettings =
                    //    from d in MessageSettings
                    //    where (int)d.MessageNameId == 5
                    //    select d;
                    //}
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.ShopAudited)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 6
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.ShopResult)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 7
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.CertificationApply)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 8
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.ConfirmPay)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 9
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.CertificationResult)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 10
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.LogisticsRece)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 11
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.LogisticsClearance)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 12
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.LogisticsSignIn)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 13
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.MemberRegister)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 14
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.SupplierRegister)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 15
                        select d;
                    }

                    if (ActiveStatu == MessageSetting.MessageModuleStatus.GoodsPayment)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 16
                        select d;
                    }

                    if (ActiveStatu == MessageSetting.MessageModuleStatus.RegisterMailContent)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 17
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.RegisterMailContent_GYS)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 18
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.XunPan)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 19
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.CustomMessage)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 20
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.LimitedAount)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 21
                        select d;
                    }
                    if (ActiveStatu == MessageSetting.MessageModuleStatus.ApplyPass)
                    {
                        MessageSettings =
                        from d in MessageSettings
                        where (int)d.MessageNameId == 22
                        select d;
                    }
                }

            }
            if (messageSettingQueryModel.Status.HasValue && messageSettingQueryModel.Status.Value != 0)
            {

                MessageSettings = from d in MessageSettings where (int)d.ActiveStatus == messageSettingQueryModel.Status select d;
            }
            int num = MessageSettings.Count();
            foreach(MessageSetting ms in MessageSettings.ToList())
            {
                ChemCloud_Dictionaries dicts = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m)=>m.DictionaryTypeId == 10 && m.DValue == ms.Languagetype.ToString());
                ms.LanguageName = dicts.DKey;
            }
            MessageSettings = MessageSettings.GetPage(out num, messageSettingQueryModel.PageNo, messageSettingQueryModel.PageSize);
            return new PageModel<MessageSetting>()
            {
                Models = MessageSettings,
                Total = num
            };
        }
        public bool ActiveStatus(long Id, int titleId, int languageType)
        {
            try
            {
                MessageSetting messageInfo = context.MessageSetting.FirstOrDefault((MessageSetting m) => m.Id == Id && m.MessageNameId == (MessageSetting.MessageModuleStatus)titleId && m.Languagetype == languageType);
                if (messageInfo != null)
                {
                    int i = 0;
                    messageInfo.ActiveStatus = 1;
                    i = context.SaveChanges();
                    if (i > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
        }
        public bool UnActiveStatus(long Id, int titleId, int languageType)
        {
            try
            {
                MessageSetting messageInfo = context.MessageSetting.FirstOrDefault((MessageSetting m) => m.Id == Id && m.MessageNameId == (MessageSetting.MessageModuleStatus)titleId && m.Languagetype == languageType);
                if (messageInfo != null)
                {
                    int i = 0;
                    messageInfo.ActiveStatus = 2;
                    i = context.SaveChanges();
                    if (i > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
        }
        public void SetOtherUnActiveStatus(long Id, int titleId, int languageType)
        {
            try
            {
                IQueryable<MessageSetting> messageInfo = from item in base.context.MessageSetting
                                                         where item.Id != Id && item.MessageNameId == (MessageSetting.MessageModuleStatus)titleId && item.Languagetype == item.Languagetype
                                                         select item;//context.MessageSetting.FindBy(MessageSetting m) => m.Id != Id && m.MessageNameId == (MessageSetting.MessageModuleStatus)titleId && m.Languagetype == languageType);
                if (messageInfo != null)
                {
                    foreach (MessageSetting list in messageInfo.ToList())
                    {
                        if (list.ActiveStatus == 1)
                        {
                            list.ActiveStatus = 2;
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
                
            }
            
        }
    }
}
