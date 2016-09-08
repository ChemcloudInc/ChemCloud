using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChemCloud.Service
{
    public class MessageDetialService : ServiceBase, IMessageDetialService, IService, IDisposable
    {
        public bool DeleteMessageDetial(long id)
        {
            MessageDetial md = (from a in context.MessageDetial where a.Id == id select a).FirstOrDefault<MessageDetial>();
            context.MessageDetial.Remove(md);
            List<MessageRevice> list = (from a in context.MessageRevice where a.MsgId == md.Id select a).ToList<MessageRevice>();
            context.MessageRevice.RemoveRange(list);
            List<MessageEnclosure> meList = (from a in context.MessageEnclosure where a.MsgId == md.Id select a).ToList<MessageEnclosure>();
            context.MessageEnclosure.RemoveRange(meList);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public MessageEnclosure GetMessageEnclosureById(long id)
        {
            MessageEnclosure me = context.MessageEnclosure.FirstOrDefault((MessageEnclosure m) => m.MsgId == id);
            return me;
        }
        public PageModel<MessageDetial> SelectAllMessageDetial(MessageDetialQuery mdq)
        {
            int pageNum = 0;
            IQueryable<MessageDetial> mdr = context.MessageDetial.AsQueryable<MessageDetial>();
            if (mdq.MsgType != 0)
            {
                mdr = (from a in mdr where a.MsgType == mdq.MsgType select a);
            }
            string begin = mdq.BeginTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string end = mdq.EndTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            if (!string.IsNullOrWhiteSpace(begin) && !begin.Equals("0001/01/01") && !string.IsNullOrWhiteSpace(end) && !end.Equals("0001/01/01"))
            {
                mdr = (from a in mdr where a.SendTime >= mdq.BeginTime && a.SendTime <= mdq.EndTime select a);
            }
            if (mdq.SendObj != 0)
            {
                mdr = (from a in mdr where a.SendObj == mdq.SendObj select a);
            }
            foreach (MessageDetial mdrs in mdr.ToList())
            {
                ManagerInfo manaInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == mdrs.ManagerId);
                mdrs.SendName = (manaInfo == null ? "" : manaInfo.UserName);
                if (mdrs.MessageTitleId != 20)
                    mdrs.MessageTitle = ((MessageSetting.MessageModuleStatus)mdrs.MessageTitleId).ToDescription();
            }
            mdr = mdr.GetPage(out pageNum, mdq.PageNo, mdq.PageSize, (IQueryable<MessageDetial> d) =>
               from o in d
               orderby o.SendTime descending
               select o);
            return new PageModel<MessageDetial>
            {
                Models = mdr,
                Total = pageNum
            };
        }

        public void AddMessageDetial(MessageDetial md, string[] ids, string[] urls)
        {
            try
            {
                context.MessageDetial.Add(md);
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

            }
            string[] idArray = ids;
            List<MessageRevice> mrList = new List<MessageRevice>();
            foreach (string item in idArray)
            {
                MessageRevice mr = new MessageRevice();
                mr.UserId = Convert.ToInt64(item);
                mr.ReadFlag = 1;
                mr.VisiableFlag = 0;
                mr.MsgId = md.Id;
                mr.SendTime = md.SendTime;
                mr.IsShow = 0;
                mrList.Add(mr);
            }
            List<MessageEnclosure> meList = new List<MessageEnclosure>();
            if (urls != null)
            {
                foreach (string item in urls)
                {
                    MessageEnclosure me = new MessageEnclosure();
                    me.MsgId = md.Id;
                    me.Url = item;
                    meList.Add(me);
                }
                context.MessageEnclosure.AddRange(meList);
            }
            context.MessageRevice.AddRange(mrList);
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {

            }

        }

        public List<QueryMember> GetLanguage(List<ChemCloud_Dictionaries> Dicts)
        {
            List<QueryMember> UserInfos = new List<QueryMember>();
            QueryMember Infos = new QueryMember();
            Infos.Id = 0;
            Infos.Username = "请选择";
            UserInfos.Add(Infos);
            foreach (ChemCloud_Dictionaries list in Dicts)
            {
                QueryMember Info = new QueryMember();
                Info.Id = long.Parse(list.DValue);
                Info.Username = list.DKey;
                UserInfos.Add(Info);
            }
            return UserInfos;
        }

        public object GetReadState(long id)
        {
            MessageDetial md = (from a in context.MessageDetial where a.Id == id select a).FirstOrDefault<MessageDetial>();
            List<MessageRevice> mrList = (from a in context.MessageRevice where a.MsgId == md.Id select a).ToList<MessageRevice>();
            int Read = 0;
            int Noread = 0;
            foreach (MessageRevice item in mrList)
            {
                if (item.ReadFlag == 0)
                {
                    Noread++;
                }
                else
                {
                    Read++;
                }
            }
            return new { Read = Read, Noread = Noread };
        }
        public int GetReadOrNoReadCount(long id, string type)
        {
            MessageDetial md = (from a in context.MessageDetial where a.Id == id select a).FirstOrDefault<MessageDetial>();
            List<MessageRevice> mrList = (from a in context.MessageRevice where a.MsgId == md.Id select a).ToList<MessageRevice>();
            int Read = 0;
            int Noread = 0;
            foreach (MessageRevice item in mrList)
            {
                if (item.ReadFlag == 0)
                {
                    Noread++;
                }
                else
                {
                    Read++;
                }
            }
            if (type == "read")
            {
                return Read;
            }
            else
            {
                return Noread;
            }
        }


        public PageModel<MessageRevice> GetMessageDetialByUserId(MessageReviceQuery mrq)
        {
            int pageNum = 0;
            IQueryable<MessageRevice> mr = from item in base.context.MessageRevice
                                           where item.UserId == mrq.UserId && item.MessageDetial.LanguageType == mrq.Languagetype
                                           select item;
            foreach (MessageRevice mrs in mr.ToList())
            {
                MessageDetial md = (from item in base.context.MessageDetial
                                    where item.Id == mrs.MsgId
                                    select item).FirstOrDefault();
                mrs.MessageDetial.LanguageType = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
                mrs.MessageDetial.ManagerId = 2;
                mrs.MessageDetial.MessageContent = md.MessageContent;
                mrs.MessageDetial.MessageTitleId = md.MessageTitleId;
                if (md.MessageTitleId != 20)
                    mrs.MessageDetial.MessageTitle = ((MessageSetting.MessageModuleStatus)md.MessageTitleId).ToDescription();
                mrs.MessageDetial.MessageTitle = md.MessageTitle;
                mrs.MessageDetial.MsgType = md.MsgType;
                mrs.MessageDetial.SendObj = md.SendObj;
            }
            if (mrq.ReadFlag != 0)
            {
                mr = (from a in mr where a.ReadFlag == mrq.ReadFlag select a);
            }
            string begin = mrq.BeginTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string end = mrq.EndTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            if (!string.IsNullOrWhiteSpace(begin) && !begin.Equals("0001/01/01") && !string.IsNullOrWhiteSpace(end) && !end.Equals("0001/01/01"))
            {
                mr = (from a in mr where a.SendTime >= mrq.BeginTime && a.SendTime <= mrq.EndTime select a);
            }
            //mr = (from a in mr where a.IsShow == 0 select a);
            foreach (MessageRevice mrs in mr.ToList())
            {
                ManagerInfo manaInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == mrs.MessageDetial.ManagerId);
                mrs.MessageDetial.SendName = (manaInfo == null ? "" : manaInfo.UserName);
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == mrs.UserId);
                mrs.UserName = userInfo.UserName;
                //mrs.MessageDetial.MessageTitle = ((MessageSetting.MessageModuleStatus)mrs.MessageDetial.MessageTitleId).ToDescription();
            }
            mr = mr.GetPage(out pageNum, mrq.PageNo, mrq.PageSize, (IQueryable<MessageRevice> d) =>
               from o in d
               orderby o.SendTime descending
               select o);
            return new PageModel<MessageRevice>
            {
                Models = mr,
                Total = pageNum
            };
        }

        public MessageRevice GetMessageById(long Id)
        {
            MessageRevice mr = context.MessageRevice.FirstOrDefault((MessageRevice m) => m.Id == Id);
            MessageDetial md = context.MessageDetial.FirstOrDefault((MessageDetial m) => m.Id == mr.MsgId);
            mr.MessageDetial.LanguageType = md.LanguageType;
            mr.MessageDetial.ManagerId = md.ManagerId;
            mr.MessageDetial.MessageContent = md.MessageContent;
            mr.MessageDetial.MessageTitleId = md.MessageTitleId;
            mr.MessageDetial.MessageTitle = ((MessageSetting.MessageModuleStatus)md.MessageTitleId).ToDescription();
            mr.MessageDetial.MsgType = md.MsgType;
            mr.MessageDetial.SendObj = md.SendObj;
            ManagerInfo manaInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == mr.MessageDetial.ManagerId);
            mr.MessageDetial.SendName = (manaInfo == null ? "" : manaInfo.UserName);
            UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == mr.UserId);
            mr.UserName = userInfo.UserName;
            return mr;
        }


        public bool BatchDeleteMessageRevice(long[] ids)
        {
            IQueryable<MessageDetial> MessageDetialInfo = context.MessageDetial.FindBy((MessageDetial item) => ids.Contains(item.Id));
            IQueryable<MessageRevice> MessageReviceInfo = context.MessageRevice.FindBy((MessageRevice item) => ids.Contains(item.MsgId));
            context.MessageDetial.RemoveRange(MessageDetialInfo);
            context.MessageRevice.RemoveRange(MessageReviceInfo);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public void DeleteMessageRevice(long id)
        {
            MessageRevice mr = (from a in context.MessageRevice where a.Id == id select a).FirstOrDefault<MessageRevice>();
            context.MessageRevice.Remove(mr);
            context.SaveChanges();
        }

        public void UpdateReadState(long id)
        {
            MessageRevice mr = (from a in context.MessageRevice where a.Id == id select a).FirstOrDefault<MessageRevice>();
            mr.ReadFlag = 2;
            mr.ReadTime = DateTime.Now;
            context.SaveChanges();
        }

        public MessageSetting GetMessageContent(int TitleId)
        {
            MessageSetting messageInfo = context.MessageSetting.FirstOrDefault((MessageSetting m) => (int)m.MessageNameId == TitleId && m.ActiveStatus == 1);
            return messageInfo;
        }
    }
}
