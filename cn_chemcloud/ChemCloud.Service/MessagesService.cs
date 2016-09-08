using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;


namespace ChemCloud.Service
{
    public class MessagesService : ServiceBase, IMessagesService, IService, IDisposable
    {

        public List<Messages> GetMessages(long ReviceUserID, long SendUserID)
        {
            return (
                from a in context.Messages
                where a.ReviceUserID == ReviceUserID && a.SendUserId == SendUserID && a.IsRead == 0
                select a).ToList<Messages>();
        }


        public bool InsertMessage(long SendUserID, long ReviceUserID, string MessageContent)
        {
            Messages message = new Messages
            {
                IsRead = 0,
                ReviceUserID = ReviceUserID,
                SendUserId = SendUserID,
                MessageContent = MessageContent,
                SendTime = DateTime.Now
            };
            context.Messages.Add(message);
            context.SaveChanges();
            return true;
        }


        public bool UpdateMessage(long SendUserId, long ReviceUserId)
        {
            List<Messages> messages = (from a in context.Messages where a.SendUserId == SendUserId && a.ReviceUserID == ReviceUserId && a.IsRead == 0 select a).ToList<Messages>();
            foreach (Messages item in messages)
            {
                item.IsRead = 1;
                context.SaveChanges();
            }
            return true;

        }


        public List<Messages> GetStepMessages(long ReviceUserId, long SendUserId, int Count)
        {
            List<Messages> messages = (from a in context.Messages where (a.SendUserId == SendUserId && a.ReviceUserID == ReviceUserId) || (a.SendUserId == ReviceUserId && a.ReviceUserID == SendUserId) orderby a.SendTime descending select a).Take(Count).ToList<Messages>();
            return messages;

        }


        public List<Messages> GetStepMessages(long ReviceUserId, long SendUserId, int pagenum,int Count)
        {
            List<Messages> messages = (from a in context.Messages where (a.SendUserId == SendUserId && a.ReviceUserID == ReviceUserId) || (a.SendUserId == ReviceUserId && a.ReviceUserID == SendUserId) orderby a.SendTime descending select a).Skip((pagenum-1)*Count).Take(Count).ToList<Messages>();
            return messages;
            
        }


        public int GetPageCount(long SendUserId, long ReviceUserId,int Count)
        {
            List<Messages> messages = (from a in context.Messages where (a.SendUserId == SendUserId && a.ReviceUserID == ReviceUserId) || (a.SendUserId == ReviceUserId && a.ReviceUserID == SendUserId) select a).ToList<Messages>();
            return messages.Count % Count == 0 ? (messages.Count / Count) : ((messages.Count / Count) + 1);

        }


        public bool getNoMessage(long ReviceUserId)
        {
            return (from a in context.Messages where a.ReviceUserID == ReviceUserId && a.IsRead == 0 select a).ToList<Messages>().Count > 0;
        }
    }
}
