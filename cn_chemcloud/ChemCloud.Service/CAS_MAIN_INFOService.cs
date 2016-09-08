using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace ChemCloud.Service
{
    public class CAS_MAIN_INFOService : ServiceBase, ICAS_MAIN_INFOService, IService, IDisposable
    {
        public CAS_MAIN_INFOService()
        {
        }
        public CAS_MAIN_INFO GetCASByNo(string CASNo)
        {
            CAS_MAIN_INFO CASInfo = (
                from p in context.CAS_MAIN_INFO
                where p.CAS_NUMBER.Equals(CASNo)
                select p).FirstOrDefault();
            return CASInfo;
        }
        public Model.PageModel<Model.CAS_MAIN_INFO> GetCasList(QueryModel.CAS_MAIN_INFOQuery casQuery)
        {
            int num = 0;

            IQueryable<CAS_MAIN_INFO> cas = context.CAS_MAIN_INFO.AsQueryable<CAS_MAIN_INFO>();
            if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBER))
            {
                cas =
                    from d in cas
                    where d.CAS_NUMBER.Equals(casQuery.CAS_NUMBER)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(casQuery.type))
            {
                int i;
                int k;
                if (int.TryParse(casQuery.type, out i))
                {
                    k = int.Parse(casQuery.type);
                    cas =
                    from d in cas
                    where d.IS_AUDIT == k
                    select d;
                }
            }
            cas = cas.GetPage(out num, casQuery.PageNo, casQuery.PageSize, (IQueryable<CAS_MAIN_INFO> d) =>
                from o in d
                orderby o.ADD_DATE descending
                select o);
            return new PageModel<CAS_MAIN_INFO>()
            {

                Models = cas,
                Total = num
            };
        }


        public Model.PageModel<Model.CAS_MAIN_INFO> PageSearchCasList(QueryModel.CAS_MAIN_INFOQuery casQuery)
        {
            int num = 0;
            try
            {
                IQueryable<CAS_MAIN_INFO> cas = context.CAS_MAIN_INFO.AsQueryable<CAS_MAIN_INFO>();

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBER))
                {
                    cas =
                        from d in cas
                        where d.CAS_NUMBER.Contains(casQuery.CAS_NUMBER) || d.CHINESE.Equals(casQuery.CAS_NUMBER) || d.CHINESE_ALIAS.Equals(casQuery.CAS_NUMBER) || d.ENGLISH.Equals(casQuery.CAS_NUMBER) || d.ENGLISH_ALIAS.Equals(casQuery.CAS_NUMBER)
                        select d;
                }

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBERList))
                {
                    List<string> listcas = new List<string>();
                    string[] array = casQuery.CAS_NUMBERList.Split('~');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(array[i]))
                        {
                            listcas.Add(array[i]);
                        }
                    }
                    cas =
                        from d in cas
                        where (from p in listcas select p).Contains(d.CAS_NUMBER)
                        select d;
                }


                cas = cas.GetPage(out num, casQuery.PageNo, casQuery.PageSize, (IQueryable<CAS_MAIN_INFO> d) =>
                    from o in d
                    orderby o.ADD_DATE descending
                    select o);
                return new PageModel<CAS_MAIN_INFO>()
                {

                    Models = cas,
                    Total = num
                };
            }
            catch (Exception) { return null; }
        }

        public CAS_MAIN_INFO GetCASByCAS_NO(string CAS_NO)
        {
            CAS_MAIN_INFO CASInfo = (
                 from a in context.CAS_MAIN_INFO

                 where a.CAS_NO == CAS_NO

                 select a).FirstOrDefault();
            return CASInfo;
        }

        public void AddCAS(CAS_MAIN_INFO casinfo)
        {
            context.CAS_MAIN_INFO.Add(casinfo);
            context.SaveChanges();
        }
        public void UpdateCASStatus(string CAS_NO, string CAS_STATUS)
        {
            CAS_MAIN_INFO nullable = context.CAS_MAIN_INFO.FirstOrDefault((CAS_MAIN_INFO m) => m.CAS_NO == CAS_NO);
            if (!string.IsNullOrEmpty(CAS_STATUS))
            {
                int i;
                if (int.TryParse(CAS_STATUS, out i))
                {
                    nullable.IS_AUDIT = int.Parse(CAS_STATUS);
                }
            }
            context.SaveChanges();
        }


        public void UpdateCAS(CAS_MAIN_INFO casinfo)
        {
            CAS_MAIN_INFO cas = context.CAS_MAIN_INFO.FirstOrDefault((CAS_MAIN_INFO m) => m.CAS_NO == casinfo.CAS_NO);
            cas.CAS_NO = casinfo.CAS_NO;
            cas.CAS_NUMBER = casinfo.CAS_NUMBER;
            cas.CHINESE = casinfo.CHINESE;
            cas.CHINESE_ALIAS = casinfo.CHINESE_ALIAS;
            cas.ENGLISH = casinfo.ENGLISH;
            cas.ENGLISH_ALIAS = casinfo.ENGLISH_ALIAS;
            cas.MOLECULAR_FORMULA = casinfo.ENGLISH_ALIAS;
            cas.MOLECULAR_WEIGHT = casinfo.MOLECULAR_WEIGHT;
            cas.PRECISE_QUALITY = casinfo.ENGLISH_ALIAS;
            cas.PSA = casinfo.PSA;
            cas.DENSITY = casinfo.DENSITY;
            cas.BOILING_POINT = casinfo.BOILING_POINT;
            cas.FLASH_POINT = casinfo.FLASH_POINT;
            cas.REFRACTIVE_INDEX = casinfo.REFRACTIVE_INDEX;
            cas.VAPOR_PRESSURE = casinfo.VAPOR_PRESSURE;
            cas.CAS_CATEGORY = casinfo.CAS_CATEGORY;
            cas.SOURCE_FROM = casinfo.SOURCE_FROM;
            cas.SOURCE_FROM_LINK = casinfo.SOURCE_FROM_LINK;
            cas.AVAILABLE_DATE = casinfo.AVAILABLE_DATE;
            cas.STRUCTURE_2D = casinfo.STRUCTURE_2D;
            cas.EXTERNAL_ID = casinfo.EXTERNAL_ID;
            cas.EXTERNAL_LINK = casinfo.EXTERNAL_LINK;
            cas.COMMENT = casinfo.COMMENT;
            cas.VERSION = casinfo.VERSION;
            cas.ADD_DATE = DateTime.Now;
            cas.ADD_AUTHOR = casinfo.ADD_AUTHOR;
            cas.IS_DEL = casinfo.IS_DEL;
            cas.DEL_UID = casinfo.DEL_UID;
            cas.DEL_TIME = casinfo.DEL_TIME;
            cas.IS_UPDATE = casinfo.IS_UPDATE;
            cas.UPDATE_UID = casinfo.UPDATE_UID;
            cas.UPDATE_TIME = DateTime.Now;
            cas.AUDIT_UID = casinfo.AUDIT_UID;
            cas.AUDIT_TIME = casinfo.AUDIT_TIME;
            cas.AUDIT_DESC = casinfo.AUDIT_DESC;
            //cas.IS_LOCK = casinfo.IS_LOCK;
            context.SaveChanges();
        }


        public PageModel<CAS_MAIN_INFO> SearchCasInfoList(QueryModel.CAS_MAIN_INFOQuery casQuery, string islike)
        {
            int num = 0;
            try
            {
                IQueryable<CAS_MAIN_INFO> cas = context.CAS_MAIN_INFO.AsQueryable<CAS_MAIN_INFO>();

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBER))
                {
                    string search = casQuery.CAS_NUMBER;
                    #region 模糊查询
                    if (islike == "0")
                    {
                        if (Regex.IsMatch(search, @"[0-9?-]") && !Regex.IsMatch(search, @"[A-Za-z]+"))
                        {
                            cas = (from u in cas
                                   where u.CAS_NUMBER.Contains(search)
                                   orderby u.UPDATE_TIME descending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                        else if (Regex.IsMatch(search, @"[\u4E00-\u9FA5]"))
                        {
                            cas = (from u in cas
                                   where u.CHINESE.Contains(search) || u.CHINESE_ALIAS.Contains(search)
                                   orderby u.UPDATE_TIME descending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                        else
                        {
                            cas = (from u in cas
                                   where u.ENGLISH.Contains(search) || u.ENGLISH_ALIAS.Contains(search)
                                   orderby u.UPDATE_TIME descending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                    }
                    #endregion
                    #region 精确查询
                    else
                    {
                        if (Regex.IsMatch(search, @"[0-9?-]") && !Regex.IsMatch(search, @"[A-Za-z]+"))
                        {
                            cas = (from u in cas
                                   where u.CAS_NUMBER.Equals(search)
                                   orderby u.UPDATE_TIME descending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                        else if (Regex.IsMatch(search, @"[\u4E00-\u9FA5]"))
                        {
                            cas = (from u in cas
                                   where u.CHINESE.Equals(search) || u.CHINESE_ALIAS.Equals(search)
                                   orderby u.UPDATE_TIME descending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                        else
                        {
                            cas = (from u in cas
                                   where u.ENGLISH.Equals(search) || u.ENGLISH_ALIAS.Equals(search)
                                   orderby u.UPDATE_TIME descending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                    }
                    #endregion
                }
                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBERList))
                {
                    List<string> listcas = new List<string>();
                    string[] array = casQuery.CAS_NUMBERList.Split('~');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(array[i]))
                        {
                            listcas.Add(array[i]);
                        }
                    }
                    cas =
                        (from d in cas
                         where (from p in listcas select p).Contains(d.CAS_NUMBER)
                         orderby d.UPDATE_TIME descending
                         select d).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                }
                return new PageModel<CAS_MAIN_INFO>()
                {
                    Models = cas,
                    Total = cas.Count<CAS_MAIN_INFO>()
                };
            }
            catch (Exception) { return null; }
        }


        public int SearchCasInfoListCount(QueryModel.CAS_MAIN_INFOQuery casQuery, string islike)
        {
            int num = 0;
            try
            {
                IQueryable<CAS_MAIN_INFO> cas = context.CAS_MAIN_INFO.AsQueryable<CAS_MAIN_INFO>();

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBER))
                {

                    string search = casQuery.CAS_NUMBER;

                    if (islike == "0")
                    {
                        if (Regex.IsMatch(search, @"[0-9?-]") && !Regex.IsMatch(search, @"[A-Za-z]+"))
                        {
                            cas = (from u in cas
                                   where u.CAS_NUMBER.Contains(search)
                                   orderby u.ADD_DATE descending
                                   select u);
                        }
                        else if (Regex.IsMatch(search, @"[\u4E00-\u9FA5]"))
                        {
                            cas = (from u in cas
                                   where u.CHINESE.Contains(search) || u.CHINESE_ALIAS.Contains(search)
                                   orderby u.ADD_DATE descending
                                   select u);
                        }
                        //else if (Regex.IsMatch(search, @"[A-Za-z0-9]+"))
                        else
                        {
                            cas = (from u in cas
                                   where u.ENGLISH.Contains(search) || u.ENGLISH_ALIAS.Contains(search)
                                   orderby u.ADD_DATE descending
                                   select u);
                        }
                    }
                    else //精确查询
                    {
                        if (Regex.IsMatch(search, @"[0-9?-]") && !Regex.IsMatch(search, @"[A-Za-z]+"))
                        {
                            cas = (from u in cas
                                   where u.CAS_NUMBER.Equals(search)
                                   orderby u.ADD_DATE descending
                                   select u);
                        }
                        else if (Regex.IsMatch(search, @"[\u4E00-\u9FA5]"))
                        {
                            cas = (from u in cas
                                   where u.CHINESE.Equals(search) || u.CHINESE_ALIAS.Equals(search)
                                   orderby u.ADD_DATE descending
                                   select u);
                        }
                        //else if (Regex.IsMatch(search, @"[A-Za-z0-9]+"))
                        else
                        {
                            cas = (from u in cas
                                   where u.ENGLISH.Equals(search) || u.ENGLISH_ALIAS.Equals(search)
                                   orderby u.ADD_DATE descending
                                   select u);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBERList))
                {
                    List<string> listcas = new List<string>();
                    string[] array = casQuery.CAS_NUMBERList.Split('~');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(array[i]))
                        {
                            listcas.Add(array[i]);
                        }
                    }
                    cas =
                        from d in cas
                        where (from p in listcas select p).Contains(d.CAS_NUMBER)
                        select d;
                }
                cas = cas.GetPage(out num, casQuery.PageNo, casQuery.PageSize, (IQueryable<CAS_MAIN_INFO> d) =>
                    from o in d
                    orderby o.ADD_DATE descending
                    select o);
                return num;
            }
            catch (Exception) { return 0; }
        }
        public CAS_MAIN_INFO GetCASByCID(string CID)
        {
            CAS_MAIN_INFO CASInfo = (
                 from a in context.CAS_MAIN_INFO

                 where a.PUB_CID == CID

                 select a).FirstOrDefault();
            return CASInfo;
        }
    }
}
