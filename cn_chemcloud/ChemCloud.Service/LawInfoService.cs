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
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service 
{
    public class LawInfoService : ServiceBase,ILawInfoService,IService,IDisposable
    {
        public PageModel<LawInfo> GetLawInfos(LawInfoQuery model,long? userId)
        {
            int pageNum = 0;
            IQueryable<LawInfo> lawInfo = from item in base.context.LawInfo
                                                 select item;
            
            string begin = model.BeginTime.ToString("yyyy/MM/dd");
            string end = model.EndTime.ToString("yyyy/MM/dd");
            if (!string.IsNullOrWhiteSpace(begin) && !begin.Equals("0001/01/01") && !string.IsNullOrWhiteSpace(end) && !end.Equals("0001/01/01"))
            {
                lawInfo = (from a in lawInfo where a.CreateTime > model.BeginTime && a.CreateTime < model.EndTime select a);
            }
            if (!string.IsNullOrWhiteSpace(model.Title))
            {
                lawInfo = (from a in lawInfo where a.Title.Contains(model.Title) select a);
            }

            lawInfo = lawInfo.GetPage(out pageNum, model.PageNo, model.PageSize, (IQueryable<LawInfo> d) =>
               from o in d
               orderby o.CreateTime descending
               select o);
            foreach (LawInfo list in lawInfo.ToList())
            {
                if (list.UserId != 0)
                {
                    ManagerInfo manaInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == list.Auditor && m.ShopId == 0);
                    list.UserName = (manaInfo == null ? "" : manaInfo.UserName);
                    ChemCloud_Dictionaries dicts = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DictionaryTypeId == 10 && m.DValue == list.LanguageType.ToString());
                    list.Language = dicts.DKey;
                }
                
            }
            return new PageModel<LawInfo>
            {
                Models = lawInfo,
                Total = pageNum
            };
        }
        
        public AttachmentInfo GetAttachmentInfo(long Id)
        {
            AttachmentInfo model = context.AttachmentInfo.FirstOrDefault((AttachmentInfo m) => m.ParentId == Id && m.Type == 2);
            return model;
        }
        public LawInfo GetLawInfo(long Id)
        {
            LawInfo model = context.LawInfo.FindById(Id);
            if (model != null)
            {
                ManagerInfo manaInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id == model.Auditor && m.ShopId == 0);
                model.UserName = (manaInfo == null ? "" : manaInfo.UserName);
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
        public LawInfo AddLawInfo(LawInfo lawInfo)
        {
            LawInfo lawInfos;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                lawInfo.CreateTime = DateTime.Now;
                lawInfos = lawInfo;
                lawInfos = context.LawInfo.Add(lawInfos);
                context.SaveChanges();
                transactionScope.Complete();
            }
            return lawInfos;
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
        public bool UpdateLawInfo(LawInfo model)
        {
            try
            {
                LawInfo models = context.LawInfo.FindById(model.Id);
                if (models != null)
                {
                    models.Title = model.Title;
                    models.LawsInfo = model.LawsInfo;
                    models.Author = model.Author;
                    models.CreateTime = DateTime.Now;
                    models.UserId = model.UserId;
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
        public bool UpdateAttachment(AttachmentInfo model)
        {
            try
            {
                AttachmentInfo models = context.AttachmentInfo.FindById(model.Id);
                if (models != null)
                {
                    models.ParentId = model.ParentId;
                    models.AttachmentName = model.AttachmentName;
                    models.UserId = model.UserId;
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
        public bool DeleteLawInfo(long Id)
        {

            try
            {
                LawInfo models = context.LawInfo.FindById(Id);
                IQueryable<AttachmentInfo> model = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == 2);
                if (models != null)
                {
                    int i = 0;
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        context.LawInfo.Remove(models);
                        if (model != null)
                            context.AttachmentInfo.RemoveRange(model.ToList());
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
                IQueryable<LawInfo> model = context.LawInfo.FindBy((LawInfo item) => Ids.Contains(item.Id));
                IQueryable<AttachmentInfo> models = context.AttachmentInfo.FindBy((AttachmentInfo m) => Ids.Contains(m.Id) && m.Type == 2);
                if (model != null)
                {
                    int i = 0;
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        context.LawInfo.RemoveRange(model);
                        if (models != null)
                            context.AttachmentInfo.RemoveRange(models);
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

        public bool UpdateLawInfoStatus(long Id, int status, long userId)
        {
            try
            {
                LawInfo model = context.LawInfo.FindById(Id);
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
            IQueryable<AttachmentInfo> attachmentInfos = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == 2);
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
        public Result_List_Pager<Result_LawInfo> GetLawInfoList_Web(QueryCommon<LawInfoQuery_Web> query)
        {
            Result_List_Pager<Result_LawInfo> res = new Result_List_Pager<Result_LawInfo>()
            {
                List = new List<Result_LawInfo>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                var linqList = from listMeet in context.LawInfo  select listMeet;
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.Title != null && query.ParamInfo.Title != string.Empty)
                {
                    linqList = linqList.Where(x => x.Title.IndexOf(query.ParamInfo.Title) != -1);
                }
                if (query.ParamInfo.StartTime != null && query.ParamInfo.EndTime != null)
                {
                    linqList = linqList.Where(x => x.CreateTime >= query.ParamInfo.StartTime && x.CreateTime <= query.ParamInfo.EndTime);
                }
                if (!string.IsNullOrWhiteSpace(query.ParamInfo.LawsInfo))
                {
                    linqList = linqList.Where(x => x.LawsInfo.Contains(query.ParamInfo.LawsInfo));
                }
                int total = linqList.Count();
                List<LawInfo> list = linqList.OrderBy(p => p.CreateTime).OrderByDescending(p => p.Id).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.List = list.Select(x => new Result_LawInfo()
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreateTime = x.CreateTime,
                    Author = x.Author,
                    LawsInfo = x.LawsInfo,
                    UserId = x.UserId
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

        #region 法律法规 详情
        /// <summary>
        /// 会议中心 详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Result_LawInfo GetObjectById_Web(int Id)
        {
            LawInfo model = (from p in context.LawInfo where p.Id == Id select p).FirstOrDefault();
            Result_LawInfo res = new Result_LawInfo()
            {
                Id = model.Id,
                Title = model.Title,
                CreateTime = model.CreateTime,
                Author = model.Author,
                LawsInfo = model.LawsInfo,
                UserId = model.UserId
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
                List<AttachmentInfo> list = (from p in context.AttachmentInfo where p.ParentId == Id && p.Type == 2 select p).ToList();
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
        public Result_List<Result_Model<LawInfo>> Get_PreNext_ById_Web(long id)
        {
            Result_List<Result_Model<LawInfo>> res = new Result_List<Result_Model<LawInfo>>()
            {
                List = new List<Result_Model<LawInfo>>(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = (from listMeet in context.LawInfo select listMeet).OrderBy(p => p.CreateTime).OrderByDescending(p => p.Id).ToList();
                int indexNo = linqList.IndexOf(linqList.First(x => x.Id == id));

                if (indexNo > 0)
                {
                    LawInfo pre = linqList.Skip(indexNo - 1).Take(1).First();
                    if (pre != null)
                    {
                        res.List.Add(new Result_Model<LawInfo>()
                        {
                            Model = pre,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<LawInfo>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取上一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<LawInfo>()
                    {
                        Msg = new Result_Msg() { IsSuccess = false, Message = "上一条没有了" }
                    });
                }

                if (indexNo < (linqList.Count - 1))
                {
                    LawInfo next = linqList.Skip(indexNo + 1).Take(1).First();

                    if (next != null)
                    {
                        res.List.Add(new Result_Model<LawInfo>()
                        {
                            Model = next,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<LawInfo>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取下一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<LawInfo>()
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
        public Result_Model<PageInfo> Get_PageInfo_Web(QueryCommon<LawInfoQuery_Web> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from listMeet in context.LawInfo  select listMeet;
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.Title != null && query.ParamInfo.Title != string.Empty)
                {
                    linqList = linqList.Where(x => x.Title.IndexOf(query.ParamInfo.Title) != -1);
                }
                if (query.ParamInfo.StartTime != null && query.ParamInfo.EndTime != null)
                {
                    linqList = linqList.Where(x => x.CreateTime >= query.ParamInfo.StartTime && x.CreateTime <= query.ParamInfo.EndTime);
                }
                if (!string.IsNullOrWhiteSpace(query.ParamInfo.LawsInfo))
                {
                    linqList = linqList.Where(x => x.LawsInfo.Contains(query.ParamInfo.LawsInfo));
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
