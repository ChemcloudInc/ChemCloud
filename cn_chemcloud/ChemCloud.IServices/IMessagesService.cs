using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;

namespace ChemCloud.IServices
{
   public interface IMessagesService: IService, IDisposable
    {
       List<Messages> GetMessages(Int64 ReviceUserID,Int64 SendUserID);

       List<Messages> GetStepMessages(long ReviceUserId, long SendUserId, int Count);

       List<Messages> GetStepMessages(long ReviceUserId, long SendUserId, int pagenum, int Count);

       bool InsertMessage(long SendUserID, long ReviceUserID, string MessageContent);


       bool UpdateMessage(long SendUserId,long ReviceUserId);

       int GetPageCount(long SendUserId, long ReviceUserId, int Count);

       bool getNoMessage(long ReviceUserId);
    }
}
                                                                                                                                    