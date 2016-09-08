using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.IServices.QueryModel;


namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class ChatController : BaseMemberController
    {
        // GET: Web/Chat
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUser()
        {
            List<ChatRelationShip> chatRelationShips = ServiceHelper.Create<IChatRelationShipService>().GetChatRelationShip(base.CurrentUser.Id);
            List<Memberinfo> member = new List<Memberinfo>();
            foreach (ChatRelationShip item in chatRelationShips)
            {
                Memberinfo m = new Memberinfo
                {
                    State = item.state,
                    UserId = item.SendUserId,
                    UserName = (ServiceHelper.Create<IMemberService>().GetMember(item.SendUserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(item.SendUserId).UserName)//(ServiceHelper.Create<IMemberService>().GetMember(item.SendUserId).UserName
                };
                member.Add(m);
            }
            return Json(member);
        }

        public JsonResult UpdateUserState(int SendUserId, int state)
        {

            return Json(ServiceHelper.Create<IChatRelationShipService>().UpdateState((long)SendUserId, base.CurrentUser.Id, state));

        }

        public JsonResult UpdateMessageState(int SendUserId)
        {
            return Json(ServiceHelper.Create<IMessagesService>().UpdateMessage(SendUserId, base.CurrentUser.Id));
        }

        public JsonResult GetStepChat(int SendUserId)
        {
            return Json(ServiceHelper.Create<IMessagesService>().GetStepMessages(base.CurrentUser.Id, SendUserId, 50));
        }

        public JsonResult GetChat(int SendUserId)
        {
            List<Messages> messages = ServiceHelper.Create<IMessagesService>().GetMessages(base.CurrentUser.Id, (long)SendUserId);
            return Json(messages);
        }

        public JsonResult SendChat(int ReviceUserId, string content)
        {
            ServiceHelper.Create<IChatRelationShipService>().UpdateChatRelationShip(base.CurrentUser.Id, (long)ReviceUserId);
            return Json(ServiceHelper.Create<IMessagesService>().InsertMessage(base.CurrentUser.Id, (long)ReviceUserId, content));
        }

        public JsonResult List(int SendUserId, int pagenum)
        {
            int pagecount = ServiceHelper.Create<IMessagesService>().GetPageCount(CurrentUser.Id, SendUserId, 50);
            if (pagenum > pagecount)
            {
                return Json(new
                {
                    success = false,
                    messages = "已经是最后一行"
                });
            }
            else
            {
                List<Messages> messages = ServiceHelper.Create<IMessagesService>().GetStepMessages(CurrentUser.Id, SendUserId, pagenum, 50);
                return Json(new
                {
                    success = true,
                    messages = messages
                });

            }

        }

        public JsonResult GetPageCount(int SendUserId)
        {
            int pagecount = ServiceHelper.Create<IMessagesService>().GetPageCount(CurrentUser.Id, SendUserId, 50);
            return Json(new
            {
                success = true,
                pagecount = pagecount
            });
        }

        public JsonResult GetState()
        {
            Result res = new Result();
            res.success = ServiceHelper.Create<IMessagesService>().getNoMessage(base.CurrentUser.Id);
            return Json(res);
        }
    }

    public class Memberinfo
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long  State { get; set; }
    }
}