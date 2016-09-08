using ChemCloud.IServices;
using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
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
    public class ConfigPayPalAPIService : ServiceBase, IConfigPayPalAPIService, IService, IDisposable
    {
        public ConfigPayPalAPIService() { }
        public ConfigPayPalAPI GetConfigPayPalAPIInfo(long id)
        {
            if (id == 0)
            {
                return null;
            }
            return context.ConfigPayPalAPI.FindById<ConfigPayPalAPI>(id);
        }

        public void UpdateConfigPayPalAPIInfo(ConfigPayPalAPI configinfo)
        {
            if (configinfo == null)
            {
                return;
            }
            ConfigPayPalAPI cinfo = context.ConfigPayPalAPI.FirstOrDefault((ConfigPayPalAPI m) => m.Id == configinfo.Id);
            if (cinfo == null)
            {
                return;
            }
            cinfo.PayPalId = configinfo.PayPalId;
            cinfo.PayPalPwd = configinfo.PayPalPwd;
            cinfo.PayPalSinature = configinfo.PayPalSinature;
            cinfo.PayPalEnvenment = configinfo.PayPalEnvenment;
            context.SaveChanges();
        }
    }
}
