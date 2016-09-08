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
    public class ChemCloud_DictionaryTypeService : ServiceBase, IChemCloud_DictionaryTypeService, IService, IDisposable
    {
        public bool AddChemCloud_DictionaryType(ChemCloud_DictionaryType model)
        {
            context.ChemCloud_DictionaryType.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool UpdateChemCloud_DictionaryType(ChemCloud_DictionaryType model)
        {
            bool result = true;
            ChemCloud_DictionaryType _ChemCloud_DictionaryType = context.ChemCloud_DictionaryType.FirstOrDefault((ChemCloud_DictionaryType m) => m.Id == model.Id);
            if (_ChemCloud_DictionaryType != null)
            {
                _ChemCloud_DictionaryType.TypeName = model.TypeName;
                _ChemCloud_DictionaryType.IsEnabled = model.IsEnabled;
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

        public bool DeleteChemCloud_DictionaryType(long Id)
        {

            int i = 0;
            ChemCloud_DictionaryType _ChemCloud_DictionaryType = context.ChemCloud_DictionaryType.FirstOrDefault((ChemCloud_DictionaryType m) => m.Id == Id);
            if (_ChemCloud_DictionaryType != null)
            {
                context.ChemCloud_DictionaryType.Remove(_ChemCloud_DictionaryType);
                i = context.SaveChanges();
            }
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool BatchDeleteChemCloud_DictionaryType(long[] ids)
        {
            IQueryable<ChemCloud_DictionaryType> _ChemCloud_DictionaryType
                = context.ChemCloud_DictionaryType.FindBy((ChemCloud_DictionaryType item) => ids.Contains(item.Id));
            context.ChemCloud_DictionaryType.RemoveRange(_ChemCloud_DictionaryType);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public PageModel<ChemCloud_DictionaryType> GetPage_ChemCloud_DictionaryType(ChemCloud_DictionaryType model)
        {

            IQueryable<ChemCloud_DictionaryType> ChemCloud_DictionaryTypes = from item in base.context.ChemCloud_DictionaryType
                                                                             select item;

            if (!string.IsNullOrWhiteSpace(model.TypeName))
            {
                ChemCloud_DictionaryTypes = from d in ChemCloud_DictionaryTypes
                                            where d.TypeName.Contains(model.TypeName)
                                            select d;
            }

            if (!string.IsNullOrWhiteSpace(model.IsEnabled))
            {
                ChemCloud_DictionaryTypes = from d in ChemCloud_DictionaryTypes
                                            where d.IsEnabled.Equals(model.IsEnabled)
                                            select d;
            }

            Func<IQueryable<ChemCloud_DictionaryType>, IOrderedQueryable<ChemCloud_DictionaryType>> func = null;
            func = (IQueryable<ChemCloud_DictionaryType> d) =>
                    from o in d
                    orderby o.Id descending
                    select o;
            int num = ChemCloud_DictionaryTypes.Count();
            ChemCloud_DictionaryTypes = ChemCloud_DictionaryTypes.GetPage(out num, model.PageNo, model.PageSize, func);
            return new PageModel<ChemCloud_DictionaryType>()
            {
                Models = ChemCloud_DictionaryTypes,
                Total = num
            };
        }

        public List<ChemCloud_DictionaryType> GetChemCloud_DictionaryTypes(ChemCloud_DictionaryType model)
        {
            IQueryable<ChemCloud_DictionaryType> ChemCloud_DictionaryTypes = from item in base.context.ChemCloud_DictionaryType
                                                                             select item;
            if (!string.IsNullOrWhiteSpace(model.TypeName))
            {
                ChemCloud_DictionaryTypes = from d in ChemCloud_DictionaryTypes
                                            where d.TypeName.Contains(model.TypeName)
                                            select d;
            }

            if (!string.IsNullOrWhiteSpace(model.IsEnabled))
            {
                ChemCloud_DictionaryTypes = from d in ChemCloud_DictionaryTypes
                                            where d.IsEnabled.Equals(model.IsEnabled)
                                            select d;
            }
            return ChemCloud_DictionaryTypes.ToList<ChemCloud_DictionaryType>();
        }

        public ChemCloud_DictionaryType GetChemCloud_DictionaryType(long Id)
        {
            ChemCloud_DictionaryType _ChemCloud_DictionaryType
                = context.ChemCloud_DictionaryType.FirstOrDefault((ChemCloud_DictionaryType m) => m.Id == Id);
            return _ChemCloud_DictionaryType;
        }
    }
}
