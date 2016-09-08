using ChemCloud.Model;
using ChemCloud.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ChemCloud.IServices
{
  public  interface IMessageDetialService:IService, IDisposable
	{

        bool DeleteMessageDetial(long id);

        PageModel<MessageDetial> SelectAllMessageDetial(MessageDetialQuery mdq);

        void AddMessageDetial(MessageDetial md, string[] ids, string[] urls);

         object GetReadState(long id);
         int GetReadOrNoReadCount(long id, string type);

         PageModel<MessageRevice> GetMessageDetialByUserId(MessageReviceQuery mrq);

         bool BatchDeleteMessageRevice(long[] ids);

         void DeleteMessageRevice(long id);


         void UpdateReadState(long id);

         MessageRevice GetMessageById(long Id);

         MessageEnclosure GetMessageEnclosureById(long id);

         List<QueryMember> GetLanguage(List<ChemCloud_Dictionaries> Dicts);

         MessageSetting GetMessageContent(int TitleId);
  }
}
