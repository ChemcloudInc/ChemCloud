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
    public class MeetInfoService : ServiceBase, IMeetInfoService, IService, IDisposable
    {
        /// <summary>
        /// 获取会议信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PageModel<MeetingInfo> GetMeetingInfos(MeetingInfoQuery model)
        {
            int pageNum = 0;
            IQueryable<MeetingInfo> MeetingInfos = from item in base.context.MeetingInfo
                                                   select item;
            string begin = model.BeginTime.ToString("yyyy/MM/dd");
            string end = model.EndTime.ToString("yyyy/MM/dd");
            if (!string.IsNullOrWhiteSpace(begin) && !begin.Equals("0001/01/01") && !string.IsNullOrWhiteSpace(end) && !end.Equals("0001/01/01"))
            {
                MeetingInfos = (from a in MeetingInfos where a.MeetingTime < model.BeginTime && a.MeetingTime > model.EndTime select a);
            }
            MeetingInfos = MeetingInfos.GetPage(out pageNum, model.PageNo, model.PageSize, (IQueryable<MeetingInfo> d) =>
               from o in d
               orderby o.MeetingTime descending
               select o);
            return new PageModel<MeetingInfo>
            {
                Models = MeetingInfos,
                Total = pageNum
            };
        }
        /// <summary>
        /// 根据主表ID获取附件信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AttachmentInfo GetAttachmentInfo(long Id)
        {
            AttachmentInfo model = context.AttachmentInfo.FirstOrDefault((AttachmentInfo m) => m.ParentId == Id && m.Type == 1);
            return model;
        }
        /// <summary>
        /// 根据ID获取会议信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MeetingInfo GetMeetInfo(long Id)
        {
            MeetingInfo model = context.MeetingInfo.FindById(Id);
            return model;
        }
        /// <summary>
        /// 根据ID获取附件信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AttachmentInfo GetAttachmentInfoById(long Id)
        {
            AttachmentInfo model = context.AttachmentInfo.FindById(Id);
            return model;
        }
        /// <summary>
        /// 新增会议信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="time"></param>
        /// <param name="place"></param>
        /// <param name="content"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MeetingInfo AddMeetInfo(MeetingInfo meetInfo)
        {
            MeetingInfo meetInfos;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                meetInfo.CreatDate = DateTime.Now;
                meetInfos = meetInfo;
                meetInfos = context.MeetingInfo.Add(meetInfos);
                context.SaveChanges();
                transactionScope.Complete();
            }
            return meetInfos;
        }
        /// <summary>
        /// 新增附件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddAttachment(AttachmentInfo model)
        {
            context.AttachmentInfo.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 更新会议信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMeetInfo(MeetingInfo model)
        {
            try
            {
                MeetingInfo models = context.MeetingInfo.FindById(model.Id);
                if (models != null)
                {
                    models.Title = model.Title;
                    models.MeetingTime = model.MeetingTime;
                    models.MeetingPlace = model.MeetingPlace;
                    models.MeetingContent = model.MeetingContent;
                    models.CreatDate = DateTime.Now;
                    models.ContinueTime = model.ContinueTime;
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
        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 删除会议信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteMeetingInfo(long Id)
        {
            int i = 0;
            try
            {
                IQueryable<AttachmentInfo> model = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == 1);
                MeetingInfo models = context.MeetingInfo.FindById(Id);
                if (models != null)
                {
                    using (TransactionScope transactionScope = new TransactionScope()) //EF框架的事物处理
                    {
                        context.MeetingInfo.Remove(models);
                        if (model != null) //如果model为空，说明没有处于这条记录的附件
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
        /// <summary>
        /// 批量删除会议信息
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool BatchDelete(long[] Ids)
        {
            try
            {
                IQueryable<MeetingInfo> model = context.MeetingInfo.FindBy((MeetingInfo item) => Ids.Contains(item.Id));
                IQueryable<AttachmentInfo> models = context.AttachmentInfo.FindBy((AttachmentInfo m) => Ids.Contains(m.Id) && m.Type == 1);
                if (model != null)
                {
                    int i = 0;
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        context.MeetingInfo.RemoveRange(model);
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
        public PageModel<MeetingInfo> GetMeetingInfosByOneWeek(MeetingInfoQuery model)
        {
            DateTime oldtime = DateTime.Now.AddDays(-7);
            DateTime nowtime = DateTime.Now;
            int pageNum = 0;
            IQueryable<MeetingInfo> MeetingInfos = from item in base.context.MeetingInfo
                                                   where item.MeetingTime >= oldtime && item.MeetingTime <= nowtime
                                                   //where item.MeetingTime <= DateTime.Now && item.MeetingTime >= DateTime.Now.AddDays(-7)
                                                   select item;
            MeetingInfos = MeetingInfos.GetPage(out pageNum, model.PageNo, model.PageSize, (IQueryable<MeetingInfo> d) =>
               from o in d
               orderby o.MeetingTime descending
               select o);
            return new PageModel<MeetingInfo>
            {
                Models = MeetingInfos,
                Total = pageNum
            };
        }
        public PageModel<MeetingInfo> GetMeetingInfosByThreeWeeks(MeetingInfoQuery model)
        {
            DateTime oldtime = DateTime.Now.AddDays(-21);
            DateTime nowtime = DateTime.Now;
            int pageNum = 0;
            IQueryable<MeetingInfo> MeetingInfos = from item in base.context.MeetingInfo
                                                   where item.MeetingTime >= oldtime && item.MeetingTime <= nowtime
                                                   //where item.MeetingTime <= DateTime.Now && item.MeetingTime >= DateTime.Now.AddDays(-7)
                                                   select item;
            MeetingInfos = MeetingInfos.GetPage(out pageNum, model.PageNo, model.PageSize, (IQueryable<MeetingInfo> d) =>
               from o in d
               orderby o.MeetingTime descending
               select o);
            return new PageModel<MeetingInfo>
            {
                Models = MeetingInfos,
                Total = pageNum
            };
        }

        public PageModel<MeetingInfo> GetMeetingInfosByThreeMouth(MeetingInfoQuery model)
        {
            DateTime oldtime = DateTime.Now.AddDays(-90);
            DateTime nowtime = DateTime.Now;
            int pageNum = 0;
            IQueryable<MeetingInfo> MeetingInfos = from item in base.context.MeetingInfo
                                                   where item.MeetingTime >= oldtime && item.MeetingTime <= nowtime
                                                   //where item.MeetingTime <= DateTime.Now && item.MeetingTime >= DateTime.Now.AddDays(-7)
                                                   select item;
            MeetingInfos = MeetingInfos.GetPage(out pageNum, model.PageNo, model.PageSize, (IQueryable<MeetingInfo> d) =>
               from o in d
               orderby o.MeetingTime descending
               select o);
            return new PageModel<MeetingInfo>
            {
                Models = MeetingInfos,
                Total = pageNum
            };
        }
        public List<AttachmentInfo> GetAttachmentInfosById(long Id)
        {
            IQueryable<AttachmentInfo> attachmentInfos = context.AttachmentInfo.FindBy((AttachmentInfo m) => m.ParentId == Id && m.Type == 1);
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
        #region 前台 会议中心

        #region 会议列表
        /// <summary>
        /// 会议列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Result_List_Pager<Result_MeetingInfo> GetMeetingsList_Web(QueryCommon<MeetingInfoQuery_Web> query)
        {
            Result_List_Pager<Result_MeetingInfo> res = new Result_List_Pager<Result_MeetingInfo>()
            {
                List = new List<Result_MeetingInfo>(),
                PageInfo = new PageInfo(),
                Msg = new Result_Msg() { IsSuccess = true }
            };
            try
            {
                var linqList = from listMeet in context.MeetingInfo select listMeet;
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.Type == 1)//最新
                {
                    linqList = linqList.Where(x => x.MeetingTime >= query.ParamInfo.EndTime);
                }
                if (query.ParamInfo.Type == 2)//历史
                {
                    linqList = linqList.Where(x => x.MeetingTime < query.ParamInfo.EndTime);
                }
                if (query.ParamInfo.Type == 0)
                {
                    linqList = linqList.Where(x => x.MeetingTime >= query.ParamInfo.StartTime && x.MeetingTime < query.ParamInfo.EndTime);
                }
                int total = linqList.Count();

                List<MeetingInfo> list = linqList.OrderBy(p => p.MeetingTime).OrderByDescending(p => p.Id).Skip((query.PageInfo.CurrentPage - 1) * query.PageInfo.PageSize).Take(query.PageInfo.PageSize).ToList();

                res.List = list.Select(x => new Result_MeetingInfo()
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatDate = x.CreatDate,
                    MeetingContent = x.MeetingContent,
                    MeetingPlace = x.MeetingPlace,
                    MeetingTime = x.MeetingTime.ToString("yyyy-MM-dd HH:mm"),
                    ContinueTime = x.ContinueTime,
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

        #region 会议中心 详情
        /// <summary>
        /// 会议中心 详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Result_MeetingInfo GetObjectById_Web(int Id)
        {
            MeetingInfo model = (from p in context.MeetingInfo where p.Id == Id select p).FirstOrDefault();
            Result_MeetingInfo res = new Result_MeetingInfo()
            {
                Id = model.Id,
                Title = model.Title,
                CreatDate = model.CreatDate,
                MeetingContent = model.MeetingContent,
                MeetingPlace = model.MeetingPlace,
                MeetingTime = model.MeetingTime.ToString("yyyy-MM-dd HH:mm"),
                ContinueTime = model.ContinueTime,
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
        public Result_List<Result_Model<MeetingInfo>> Get_PreNext_ById_Web(long id)
        {
            Result_List<Result_Model<MeetingInfo>> res = new Result_List<Result_Model<MeetingInfo>>()
            {
                List = new List<Result_Model<MeetingInfo>>(),
                Msg = new Result_Msg() { IsSuccess = true }
            };

            try
            {
                var linqList = (from listMeet in context.MeetingInfo select listMeet).OrderBy(p => p.MeetingTime).OrderByDescending(p => p.Id).ToList();
                int indexNo = linqList.IndexOf(linqList.First(x => x.Id == id));

                if (indexNo > 0)
                {
                    MeetingInfo pre = linqList.Skip(indexNo - 1).Take(1).First();
                    if (pre != null)
                    {
                        res.List.Add(new Result_Model<MeetingInfo>()
                        {
                            Model = pre,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<MeetingInfo>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取上一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<MeetingInfo>()
                    {
                        Msg = new Result_Msg() { IsSuccess = false, Message = "上一条没有了" }
                    });
                }

                if (indexNo < (linqList.Count - 1))
                {
                    MeetingInfo next = linqList.Skip(indexNo + 1).Take(1).First();

                    if (next != null)
                    {
                        res.List.Add(new Result_Model<MeetingInfo>()
                        {
                            Model = next,
                            Msg = new Result_Msg() { IsSuccess = true }
                        });
                    }
                    else
                    {
                        res.List.Add(new Result_Model<MeetingInfo>()
                        {
                            Msg = new Result_Msg() { IsSuccess = false, Message = "获取下一条信息失败" }
                        });
                    }
                }
                else
                {
                    res.List.Add(new Result_Model<MeetingInfo>()
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
        public Result_Model<PageInfo> Get_PageInfo_Web(QueryCommon<MeetingInfoQuery_Web> query)
        {
            Result_Model<PageInfo> resPage = new Result_Model<PageInfo>() { Model = new PageInfo(), Msg = new Result_Msg() { IsSuccess = true } };
            try
            {
                var linqList = from listMeet in context.MeetingInfo select listMeet;
                if (query.ParamInfo.LanguageType != 0)
                {
                    //语言类型（0：所有 ；1：中文；2：英文；）
                    linqList = linqList.Where(x => x.LanguageType == query.ParamInfo.LanguageType);
                }
                if (query.ParamInfo.Type == 1)//最新
                {
                    linqList = linqList.Where(x => x.MeetingTime >= query.ParamInfo.EndTime);
                }
                if (query.ParamInfo.Type == 2)//历史
                {
                    linqList = linqList.Where(x => x.MeetingTime < query.ParamInfo.EndTime);
                }
                if (query.ParamInfo.Type == 0)
                {
                    linqList = linqList.Where(x => x.MeetingTime >= query.ParamInfo.StartTime && x.MeetingTime < query.ParamInfo.EndTime);
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
