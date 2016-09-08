using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class ChatRelationShipService : ServiceBase, IChatRelationShipService, IService, IDisposable
    {



        public List<ChatRelationShip> GetChatRelationShip(long UserID)
        {
            return (from a in context.ChatRelationShip where a.ReviceUserId == UserID && a.state != -1 orderby a.state descending select a).ToList<ChatRelationShip>();
        }

        public void UpdateChatRelationShip(long SendUserId, long ReviceUserId)
        {
            ChatRelationShip chatRelationShip = (from a in context.ChatRelationShip where a.ReviceUserId == ReviceUserId && a.SendUserId == SendUserId select a).FirstOrDefault<ChatRelationShip>();
            if (chatRelationShip==null)
            {
                chatRelationShip = new ChatRelationShip
                {
                    SendUserId = SendUserId,
                    ReviceUserId = ReviceUserId,
                    state = 2,
                    EndDateTime = DateTime.Now
                };
                context.ChatRelationShip.Add(chatRelationShip);
                context.SaveChanges();
            }
            else
            {
                chatRelationShip.state = 2;
                chatRelationShip.EndDateTime = DateTime.Now;
                context.SaveChanges();
            }
        }


        public bool ExitsRelation(long SendUseId, long ReviceUserId)
        {
            return !((from a in context.ChatRelationShip where a.SendUserId == SendUseId && a.ReviceUserId == ReviceUserId select a) == null);
        }


        public bool UpdateState(long SendUseId, long ReviceUserId,int state)
        {
            ChatRelationShip chatRelationShip = (from a in context.ChatRelationShip where a.ReviceUserId == ReviceUserId && a.SendUserId == SendUseId select a).FirstOrDefault<ChatRelationShip>();
            if (chatRelationShip != null)
            {
                chatRelationShip.state = state;
                chatRelationShip.EndDateTime = DateTime.Now;
                context.SaveChanges();
            }
            return true;
        }


        public bool GetNewRelationShip(long SendUserId, long ReviceUserId)
        {
            ChatRelationShip chatRelationShip = (from a in context.ChatRelationShip where a.ReviceUserId == ReviceUserId && a.SendUserId == SendUserId select a).FirstOrDefault<ChatRelationShip>();
            if (chatRelationShip == null)
            {
                chatRelationShip = new ChatRelationShip
                {
                    SendUserId = SendUserId,
                    ReviceUserId = ReviceUserId,
                    state = 3,
                    EndDateTime = DateTime.Now
                };
                context.ChatRelationShip.Add(chatRelationShip);
                context.SaveChanges();
            }
            else
            {
                chatRelationShip.state = 3;
                chatRelationShip.EndDateTime = DateTime.Now;
                context.SaveChanges();
            }
            return true;
        }
    }
}
