using ChemCloud.Model;
using System;
using System.Collections.Generic;


namespace ChemCloud.IServices
{
    public interface IChatRelationShipService : IService, IDisposable
    {
        List<ChatRelationShip> GetChatRelationShip(long UserID);

        void UpdateChatRelationShip(long SendUserId, long ReviceUserId);

        bool ExitsRelation(long SendUseId, long ReviceUserId);

        bool UpdateState(long SendUseId, long ReviceUserId,int state);

        bool GetNewRelationShip(long SendUserId, long ReviceUserId);
    }
}
