using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    public class ChemCloud_DictionariesService : ServiceBase, IChemCloud_DictionariesService, IService, IDisposable
    {
        public bool AddChemCloud_Dictionaries(ChemCloud_Dictionaries model)
        {
            context.ChemCloud_Dictionaries.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool UpdateChemCloud_Dictionaries(ChemCloud_Dictionaries model)
        {
            bool result = true;
            ChemCloud_Dictionaries _ChemCloud_Dictionaries = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.Id == model.Id);
            if (_ChemCloud_Dictionaries != null)
            {
                _ChemCloud_Dictionaries.DictionaryTypeId = model.DictionaryTypeId;
                _ChemCloud_Dictionaries.DKey = model.DKey;
                _ChemCloud_Dictionaries.DValue = model.DValue;
                _ChemCloud_Dictionaries.DValue_En = model.DValue_En;
                _ChemCloud_Dictionaries.Remarks = model.Remarks;
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    result = false;
                }

            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool DeleteChemCloud_Dictionaries(long Id)
        {

            int i = 0;
            ChemCloud_Dictionaries _ChemCloud_Dictionaries = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.Id == Id);
            if (_ChemCloud_Dictionaries != null)
            {
                context.ChemCloud_Dictionaries.Remove(_ChemCloud_Dictionaries);
                i = context.SaveChanges();
            }
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool BatchDeleteChemCloud_Dictionaries(long[] ids)
        {
            IQueryable<ChemCloud_Dictionaries> _ChemCloud_Dictionaries
                = context.ChemCloud_Dictionaries.FindBy((ChemCloud_Dictionaries item) => ids.Contains(item.Id));
            context.ChemCloud_Dictionaries.RemoveRange(_ChemCloud_Dictionaries);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public PageModel<ChemCloud_Dictionaries> GetPage_ChemCloud_Dictionaries(ChemCloud_Dictionaries model)
        {

            IQueryable<ChemCloud_Dictionaries> ChemCloud_Dictionariess = from item in base.context.ChemCloud_Dictionaries
                                                                         select item;
            if (model.DictionaryTypeId > 0)
            {
                ChemCloud_Dictionariess = from d in ChemCloud_Dictionariess
                                          where d.DictionaryTypeId.Equals(model.DictionaryTypeId)
                                          select d;
            }

            if (!string.IsNullOrWhiteSpace(model.DKey))
            {
                ChemCloud_Dictionariess = from d in ChemCloud_Dictionariess
                                          where d.DKey.Contains(model.DKey)
                                          select d;
            }

            if (!string.IsNullOrWhiteSpace(model.DValue))
            {
                ChemCloud_Dictionariess = from d in ChemCloud_Dictionariess
                                          where d.DValue.Contains(model.DValue)
                                          select d;
            }

            Func<IQueryable<ChemCloud_Dictionaries>, IOrderedQueryable<ChemCloud_Dictionaries>> func = null;
            func = (IQueryable<ChemCloud_Dictionaries> d) =>
                    from o in d
                    orderby o.DictionaryTypeId descending
                    select o;
            int num = ChemCloud_Dictionariess.Count();
            ChemCloud_Dictionariess = ChemCloud_Dictionariess.GetPage(out num, model.PageNo, model.PageSize, func);
            return new PageModel<ChemCloud_Dictionaries>()
            {
                Models = ChemCloud_Dictionariess,
                Total = num
            };
        }

        public List<ChemCloud_Dictionaries> GetChemCloud_Dictionariess(ChemCloud_Dictionaries model)
        {
            IQueryable<ChemCloud_Dictionaries> ChemCloud_Dictionariess = from item in base.context.ChemCloud_Dictionaries
                                                                         select item;
            if (model.DictionaryTypeId > 0)
            {
                ChemCloud_Dictionariess = from d in ChemCloud_Dictionariess
                                          where d.DictionaryTypeId.Equals(model.DictionaryTypeId)
                                          select d;
            }

            if (!string.IsNullOrWhiteSpace(model.DKey))
            {
                ChemCloud_Dictionariess = from d in ChemCloud_Dictionariess
                                          where d.DKey.Contains(model.DKey)
                                          select d;
            }

            if (!string.IsNullOrWhiteSpace(model.DValue))
            {
                ChemCloud_Dictionariess = from d in ChemCloud_Dictionariess
                                          where d.DValue.Contains(model.DValue)
                                          select d;
            }

            return ChemCloud_Dictionariess.ToList<ChemCloud_Dictionaries>();
        }

        public ChemCloud_Dictionaries GetChemCloud_Dictionaries(long Id)
        {
            ChemCloud_Dictionaries _ChemCloud_Dictionaries
                = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.Id == Id);
            return _ChemCloud_Dictionaries;
        }

        public string GetValueBYKey(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
                return context.ChemCloud_Dictionaries.Where(q => q.DKey == key).FirstOrDefault() == null ? "" : context.ChemCloud_Dictionaries.Where(q => q.DKey == key).FirstOrDefault().DValue;
            else
                return "";
        }

        public List<ChemCloud_Dictionaries> GetListByType(int type)
        {
            return (from a in context.ChemCloud_Dictionaries
                    where a.DictionaryTypeId == type
                    orderby a.DValue ascending
                    select a).ToList<ChemCloud_Dictionaries>();
        }
    }
}
