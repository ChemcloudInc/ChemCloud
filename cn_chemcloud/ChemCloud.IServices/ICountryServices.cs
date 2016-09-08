using ChemCloud.Core;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface ICountryServices: IService, IDisposable
    {
        List<CountryInfo> GetCountry();

        List<CountryInfo> GetProvince(int Pid);
        List<CountryInfo> GetCity(int Pid);

        List<CountryInfo> GetEdit(List<string> address);

        string GetAreaName(int RegionID);

    }
}
