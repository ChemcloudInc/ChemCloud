using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using ChemCloud.Core;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
namespace ChemCloud.Web
{
    public class Global : HttpApplication
    {
        public override void Init()
        {
            base.Init();

             

            //启动 Websocket
          
        }

        void Global_Disposed(object sender, EventArgs e)
        {
            
        }

        void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RegistAtStart.Regist();
            Job.Start();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(new Action<HttpConfiguration>(WebApiConfig.Register));
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AreaRegistrationOrder.RegisterAllAreasOrder();
            BundleConfig.RegisterBundles(BundleTable.Bundles);


           
        }

        private void SetupResolveRules(ContainerBuilder builder)
        {
            Assembly assembly = Assembly.Load("ChemCloud.IServices");
            Assembly assembly1 = Assembly.Load("ChemCloud.Services");
            Assembly[] assemblyArray = new Assembly[] { assembly, assembly1 };
            builder.RegisterAssemblyTypes(assemblyArray).Where((Type t) => t.Name.EndsWith("Service")).AsImplementedInterfaces<object>();
        }
    }
}