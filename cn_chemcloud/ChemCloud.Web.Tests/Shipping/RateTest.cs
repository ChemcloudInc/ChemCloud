using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using ChemColud.Shipping;
using ChemColud.Shipping.ShippingProviders;

namespace ChemCloud.Web.Tests.Shipping
{
    [TestClass]
    public class RateTest
    {
        [TestMethod]
        public void RateMethod()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var fedexKey = appSettings["FedExKey"];
            var fedexPassword = appSettings["FedExPassword"];
            var fedexAccountNumber = appSettings["FedExAccountNumber"];
            var fedexMeterNumber = appSettings["FedExMeterNumber"];

            var packages = new List<Package>();
            packages.Add(new Package(0, 0, 0, 5, 0));

         

         
           //// var destination = new Address("RECIPIENT ADDRESS LINE 1", "", "", "Collierville", "TN", "38017", "US");
           // var origin = new Address("Zhong Shan Road 200", "", "", "NanJing", "", "210046", "CN");

           // var destination = new Address("SHIPPER ADDRESS LINE 1", "", "", "Austin", "TX", "73301", "US");


            var origin = new Address("SHIPPER ADDRESS LINE 1", "", "", "Austin", "TX", "73301", "US");

            var destination = new Address("栖霞区马群街道", "", "", "南京市", "", "210046", "CN");

            var rateManager = new RateManager();

            rateManager.AddProvider(new FedExProvider(fedexKey, fedexPassword, fedexAccountNumber, fedexMeterNumber));


            var shipment = rateManager.GetRates(origin, destination, packages);

            foreach (var rate in shipment.Rates)
            {
                Console.WriteLine(rate);
            }
            Console.ReadLine();
          
        }
    }
}
