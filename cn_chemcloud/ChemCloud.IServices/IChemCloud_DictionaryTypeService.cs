using ChemCloud.Model.Models;
using Senparc.Weixin.MP.AdvancedAPIs.Poi;
using System;
using System.Collections.Generic;
using ChemCloud.Model;

namespace ChemCloud.IServices
{
    public interface IChemCloud_DictionaryTypeService : IService, IDisposable
    {
        bool AddChemCloud_DictionaryType(ChemCloud_DictionaryType model);

        bool UpdateChemCloud_DictionaryType(ChemCloud_DictionaryType model);

        bool DeleteChemCloud_DictionaryType(long Id);

        bool BatchDeleteChemCloud_DictionaryType(long[] ids);

        PageModel<ChemCloud_DictionaryType> GetPage_ChemCloud_DictionaryType(ChemCloud_DictionaryType model);

        List<ChemCloud_DictionaryType> GetChemCloud_DictionaryTypes(ChemCloud_DictionaryType model);

        ChemCloud_DictionaryType GetChemCloud_DictionaryType(long Id);
    }
}
