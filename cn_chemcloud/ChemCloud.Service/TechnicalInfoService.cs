using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Model.Common;
using ChemCloud.QueryModel;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class TechnicalInfoService : ServiceBase, ITechnicalInfoService, IService, IDisposable
    {
        public PageModel<TechnicalInfo> GetTechInfos(TechnicalInfoQuery model, long? userId)
        {
            int pageNum = 0;
            IQueryable<TechnicalInfo> techInfo = from item in base.context.TechnicalInfo
                                                 select item;
            if (userId.HasValue && userId.Value != 0)
            {
                techInfo = (from a in techInfo where a.PublisherId == userId.Value select a);
            }
            string begin = model.BeginTime.ToString("yyyy/MM/dd");
            string end = model.EndTime.ToString("yyyy/MM/dd");
            if (!string.IsNullOrWhiteSpace(begin) && !begin.Equals("0001/01/01") && !string.IsNullOrWhiteSpace(end) && !end.Equals("0001/01/01"))
            {
                techInfo = (from a in techInfo where a.PublishTime > model.BeginTime && a.PublishTime < model.EndTime select a);
            }
            if (model.Status.HasValue && model.Status != 0)
            {
                techInfo = (from a in techInfo where a.Status == model.Status select a);
            }
            if (!string.IsNullOrWhiteSpace(model.Title))
            {
                techInfo = (from a in techInfo where a.Title.Contains(model.Title) select a);
            }

            techInfo = techInfo.GetPage(out pageNum, model.PageNo, model.PageSize, (IQueryable<TechnicalInfo> d) =>
               from o in d
               orderby o.PublishTime descending
               select o);
            foreach (TechnicalInfo list in techInfo.ToList())
            {
                if (list != null)
                {
                    UserMemberInfo userInfo = context.UserMemberInfo.FindById(list.PublisherId);
                    list.PublisherName = (userInfo == null ? "" : userInfo.UserName);
                    ManagerInfo manaInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == list.Auditor && m.ShopId == 0);
                    list.AuditorName = (manaInfo == null ? "" : manaInfo.UserName);
                    ChemCloud_Dictionaries dicts = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 10 && m.DValue == list.LanguageType.ToString());
                    list.Language = dicts == null ? "" : dicts.DKey;
                }

            }
            return new PageModel<TechnicalInfo>
            {
                Models = techInfo,
                Total = pageNum
            };
        }
        public AttachmentInfo GetAttachmentInfo(long Id)
        {
            AttachmentInfo model = context.AttachmentInfo.FirstOrDefault((AttachmentInfo m) => m.ParentId == Id && m.Type == 2);
            return model;
        }
        public TechnicalInfo GetTechInfo(long Id)
        {
            TechnicalInfo model = context.TechnicalInfo.FindById(Id);
            if (model != null)
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FindById(model.PublisherId);
                model.PublisherName = (userInfo == null ? "" : userInfo.UserName);
                ManagerInfo manaInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == model.Auditor && m.ShopId == 0);
                model.AuditorName = (manaInfo == null ? "" : manaInfo.UserName);
                ChemCloud_Dictionaries dicts = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 10 && m.DValue == model.LanguageType.ToString());
                model.Language = dicts.DKey;
            }

            return model;
        }
        public AttachmentInfo GetAttachmentInfoById(long Id)
        {
            AttachmentInfo model = context.AttachmentInfo.FindById(Id);
            return model;
        }
        public TechnicalInfo AddTechnicalInfo(TechnicalInfo techInfo)
        {
            TechnicalInfo techInfos;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                techInfo.PublishTime = DateTime.Now;
                techInfos = techInfo;
                techInfos = context.TechnicalInfo.Add(techInfos);
                context.SaveChanges();
                transactionScope.Complete();
            }
            return techInfos;
        }
        public bool AddAttachment(AttachmentInfo model)
        {
            context.AttachmentInfo.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool UpdateTechInfo(TechnicalInfo model)
        {
            try
            {
                TechnicalInfo models = context.TechnicalInfo.FindById(model.Id);
                if (models != null)
                {
                    models.Title = model.Title;
                    models.TechContent = model.TechContent;
                    models.Tel = model.Tel;
                    models.Email = model.Email;
                    models.Author = model.Author;
                    models.PublishTime = DateTime.Now;
                    models.PublisherId = model.PublisherId;
                    models.LanguageType = model.LanguageType;
                    int i = context.SaveChanges();
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
        public Result_Msg UpdateAttachment(AttachmentInfo model)
        {
            Result_Msg res = new Result_Msg();
            try
            {
                AttachmentInfo models = context.AttachmentInfo.FindById(model.Id);
                if (models != null)
                {
                    models.ParentId = model.ParentId;
                    models.AttachmentName = model.AttachmentName;
                    models.UserId = model.UserId;
                    models.UploadTime = DateTime.Now;

                    int i = context.SaveChanges();
                    if (i > 0)
                    {
                        res.IsSuccess = true;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Message = "修改失败";
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = "所有该的记录不存在";
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;
        }
        public bool DeleteTechInfo(long Id)
        {

            try
            {
                TechnicalInfo models = context.TechnicalInfo.FindById(Id);
                IQueryable<AttachmentInfo> model = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == 3);
                if (models != null)
                {
                    int i = 0;
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        context.TechnicalInfo.Remove(models);
                        if (model != null)
                            context.AttachmentInfo.RemoveRange(model);
                        i = context.SaveChanges();
                        transactionScope.Complete();
                    }
                    if (i > 0)
                    {
                        return true;
                    }
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

        public bool BatchDelete(long[] Ids)
        {
            try
            {
                IQueryable<TechnicalInfo> model = context.TechnicalInfo.FindBy((TechnicalInfo item) => Ids.Contains(item.Id));
                IQueryable<AttachmentInfo> models = context.AttachmentInfo.FindBy((AttachmentInfo m) => Ids.Contains(m.Id) && m.Type == 2);
                if (model != null)
                {
                    int i = 0;
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        context.TechnicalInfo.RemoveRange(model);
                        if (models != null)
                            context.AttachmentInfo.RemoveRange(models.ToList());
                        i = context.SaveChanges();
                        transactionScope.Complete();
                    }
                    if (i > 0)
                    {
                        return true;
                    }
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

        public bool UpdateTechinfoStatus(long Id, int status, long userId)
        {
            try
            {
                TechnicalInfo model = context.TechnicalInfo.FindById(Id);
                if (model != null)
                {
                    model.Status = status;
                    model.Auditor = userId;
                    int i = context.SaveChanges();
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

        public List<AttachmentInfo> GetAttachmentInfosById(long Id)
        {
            IQueryable<AttachmentInfo> attachmentInfos = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == 3);
            return attachmentInfos.ToList();
        }
        public bool DeleteAttachment(long Id)
        {
            try
            {
                AttachmentInfo model = context.AttachmentInfo.FindById(Id);
                if (model != null)
                {
                    context.AttachmentInfo.Remove(model);
                    int i = context.SaveChanges();
                    if (i > 0)
                    {
                        return true;
                    }
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
        #region 前台 会议中心

        #region 会议列表
        /// <summary>
        /// 会议列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<Result_TechnicalInfo> GetMeetingsList_Web(QueryCommon<TechnicalInfoQuery_Web> query)
        {
            Result_List_Pager<Result_TechnicalInfo> res = new Result_List_Pager<Result_TechnicalInfo>()
            {
                List = new List<Result_TechnicalInfo>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {

                var linqList = from listMeet in context.TechnicalInfo where listMeet.Status == 2 select listMeet;
                string name = (from a in context.SiteSettingsInfo where a.Key == "techName" select a).FirstOrDefault().Value;
                string tel = (from a in context.SiteSettingsInfo where a.Key == "techTel" select a).FirstOrDefault().Value;
                string Email = (from a in context.SiteSettingsInfo where a.Key == "techEmail" select a).FirstOrDefault().Value;
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.Type == 1)//最新
                {
                    linqList = linqList.Where(x => x.PublishTime >= query.ParamInfo.EndTime);
                }
                if (query.ParamInfo.Type == 2)//历史
                {
                    linqList = linqList.Where(x => x.PublishTime < query.ParamInfo.EndTime);
                }
                if (query.ParamInfo.Type == 0)
                {
                    linqList = linqList.Where(x => x.PublishTime >= query.ParamInfo.StartTime && x.PublishTime < query.ParamInfo.EndTime);
                }

                int total = linqList.Count();
                List<TechnicalInfo> list = linqList.OrderBy(p => p.PublishTime).OrderByDescending(p => p.Id).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.List = list.Select(x => new Result_TechnicalInfo()
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublishTime = x.PublishTime,
                    Author = x.Author,
                    TechContent = x.TechContent,
                    Tel = x.Tel,
                    Email = x.Email,
                    UserId = x.PublisherId,
                    techName = name,
                    techTel = tel,
                    techEmail = Email
                }).ToList();
                res.PageInfo.CurrentPage = query.PageInfo.CurrentPage;
                res.PageInfo.PageSize = query.PageInfo.PageSize;
                res.PageInfo.Total = total;
                res.PageInfo.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }

        #endregion

        #region 会议中心 详情
        /// <summary>
        /// 会议中心 详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Result_TechnicalInfo GetObjectById_Web(int Id)
        {
            TechnicalInfo model = (from p in context.TechnicalInfo where p.Id == Id select p).FirstOrDefault();
            string name = (from a in context.SiteSettingsInfo where a.Key == "techName" select a).FirstOrDefault().Value;
            string tel = (from a in context.SiteSettingsInfo where a.Key == "techTel" select a).FirstOrDefault().Value;
            string Email = (from a in context.SiteSettingsInfo where a.Key == "techEmail" select a).FirstOrDefault().Value;
            Result_TechnicalInfo res = new Result_TechnicalInfo()
            {
                Id = model.Id,
                Title = model.Title,
                PublishTime = model.PublishTime,
                Author = model.Author,
                TechContent = model.TechContent,
                Tel = model.Tel,
                Email = model.Email,
                techName = name,
                techTel = tel,
                techEmail = Email
            };
            return res;
        }
        #endregion

        #region 附件列表
        /// <summary>
        /// 附件列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Result_List<Result_AttachmentInfo> GetObjectList_ById_Web(long Id)
        {
            Result_List<Result_AttachmentInfo> res = new Result_List<Result_AttachmentInfo>()
            {
                Msg = new Result_Msg()
                {
                    IsSuccess = true
                }
            };

            try
            {
                List<AttachmentInfo> list = (from p in context.AttachmentInfo where p.ParentId == Id && p.Type == 1 select p).ToList();
                res.List = list.Select(x => new Result_AttachmentInfo()
                {
                    Id = x.Id,
                    AttachmentName = x.AttachmentName,
                    ParentId = x.ParentId,
                    Type = x.Type,
                    UserId = x.UserId
                }).ToList();
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return res;
        }
        #endregion

        #region 前后两条记录
        /// <summary>
        /// 前后两条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result_List<Result_Model<TechnicalInfo>> Get_PreNext_ById_Web(long id)
        {
            Result_List<Result_Model<TechnicalInfo>> res = new Result_List<Result_Model<TechnicalInfo>>()
            {
                List = new List<Result_Model<TechnicalInfo>>(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = (from listMeet in context.TechnicalInfo where listMeet.Status == 2 select listMeet).OrderBy(p => p.PublishTime).OrderByDescending(p => p.Id).ToList();
                int indexNo = linqList.IndexOf(linqList.First(x => x.Id == id));

                if (indexNo > 0)
                {
                    TechnicalInfo pre = linqList.Skip(indexNo - 1).Take(1).First();
                    if (pre != null)
                    {
                        res.List.Add(new Result_Model<TechnicalInfo>()
                        {
                            Model = pre,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<TechnicalInfo>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取上一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<TechnicalInfo>()
                    {
                        Msg = new Result_Msg() { IsSuccess = false, Message = "上一条没有了" }
                    });
                }

                if (indexNo < (linqList.Count - 1))
                {
                    TechnicalInfo next = linqList.Skip(indexNo + 1).Take(1).First();

                    if (next != null)
                    {
                        res.List.Add(new Result_Model<TechnicalInfo>()
                        {
                            Model = next,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<TechnicalInfo>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取下一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<TechnicalInfo>()
                    {
                        Msg = new Result_Msg() { IsSuccess = false, Message = "下一条没有了" }
                    });
                }

            }
            catch (Exception ex)
            {
                res.Msg.IsSuccess = false;
                res.Msg.Message = "查询失败，失败原因：" + ex.Message;
            }
            return res;

        }
        #endregion

        #region 分页信息
        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_Model<PageInfo> Get_PageInfo_Web(QueryCommon<TechnicalInfoQuery_Web> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from listMeet in context.TechnicalInfo where listMeet.Status == 2 select listMeet;
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.Type == 1)//最新
                {
                    linqList = linqList.Where(x => x.PublishTime >= query.ParamInfo.EndTime);
                }
                if (query.ParamInfo.Type == 2)//历史
                {
                    linqList = linqList.Where(x => x.PublishTime < query.ParamInfo.EndTime);
                }
                if (query.ParamInfo.Type == 0)
                {
                    linqList = linqList.Where(x => x.PublishTime >= query.ParamInfo.StartTime && x.PublishTime < query.ParamInfo.EndTime);
                }
                int total = linqList.Count();

                resPage.Model.CurrentPage = query.PageInfo.CurrentPage;
                resPage.Model.PageSize = query.PageInfo.PageSize;
                resPage.Model.Total = total;
                resPage.Model.PageCount = Convert.ToInt32(Math.Ceiling((double)total / (double)query.PageInfo.PageSize));
            }
            catch (Exception ex)
            {
                resPage.Msg = new Result_Msg() { IsSuccess = false, Message = ex.Message };
            }
            return resPage;
        }
        #endregion

        #endregion
    }
}
