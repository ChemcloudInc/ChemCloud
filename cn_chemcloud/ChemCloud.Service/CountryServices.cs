using ChemCloud.Core;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace  ChemCloud.Service
{
    public class CountryServices : ServiceBase, ICountryServices, IService, IDisposable
    {

        public List<CountryInfo> GetCountry()
        {
            return (from a in context.CountryInfo where a.PID == 0 select a).ToList<CountryInfo>();
        }

        public List<CountryInfo> GetProvince(int Pid)
        {
            return (from a in context.CountryInfo where a.PID == Pid select a).ToList<CountryInfo>();
        }

        public List<CountryInfo> GetCity(int Pid)
        {
            return (from a in context.CountryInfo where a.PID == Pid select a).ToList<CountryInfo>();
        }

        public List<CountryInfo> GetEdit(List<string> address)
        {
            List<CountryInfo> countryinfo = new List<CountryInfo>();
            foreach (string item in address)
            {
                CountryInfo Country = (from a in context.CountryInfo where a.ContryNameCN == item select a).FirstOrDefault<CountryInfo>();
                countryinfo.Add(Country);
            }
            return countryinfo;
        }

        public string GetAreaName(int RegionID)
        {
            string areaName = "";
            CountryInfo Country = (from a in context.CountryInfo where a.ID == RegionID select a).FirstOrDefault<CountryInfo>();
            CountryInfo Country2 = new CountryInfo();
            CountryInfo Country1 = new CountryInfo();
            if (Country.PID != 0)
            {
                Country1 = (from a in context.CountryInfo where a.ID == Country.PID select a).FirstOrDefault<CountryInfo>();
                if (Country1.PID != 0)
                {
                    Country2 = (from a in context.CountryInfo where a.ID == Country1.PID select a).FirstOrDefault<CountryInfo>();
                }
            }
            areaName += string.IsNullOrEmpty(Country2.ContryNameCN) ? "" : Country2.ContryNameCN + " ";
            areaName += string.IsNullOrEmpty(Country1.ContryNameCN) ? "" : Country1.ContryNameCN + " ";
            areaName += string.IsNullOrEmpty(Country.ContryNameCN) ? "" : Country.ContryNameCN;
            return areaName;
        }
    }
}
