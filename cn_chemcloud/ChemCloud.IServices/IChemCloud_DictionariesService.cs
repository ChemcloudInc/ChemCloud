using ChemCloud.Model.Models;
using Senparc.Weixin.MP.AdvancedAPIs.Poi;
using System;
using System.Collections.Generic;
using ChemCloud.Model;

namespace ChemCloud.IServices
{
    public interface IChemCloud_DictionariesService : IService, IDisposable
    {
        bool AddChemCloud_Dictionaries(ChemCloud_Dictionaries model);

        bool UpdateChemCloud_Dictionaries(ChemCloud_Dictionaries model);

        bool DeleteChemCloud_Dictionaries(long Id);

        bool BatchDeleteChemCloud_Dictionaries(long[] ids);

        PageModel<ChemCloud_Dictionaries> GetPage_ChemCloud_Dictionaries(ChemCloud_Dictionaries model);

        List<ChemCloud_Dictionaries> GetChemCloud_Dictionariess(ChemCloud_Dictionaries model);

        ChemCloud_Dictionaries GetChemCloud_Dictionaries(long Id);

        string GetValueBYKey(string key);
        List<ChemCloud_Dictionaries> GetListByType(int type);
    }
}
