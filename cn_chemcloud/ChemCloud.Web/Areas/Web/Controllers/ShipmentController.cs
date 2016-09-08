using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.OAuth;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Web.Models;


using Newtonsoft.Json;
using ChemColud.Shipping;
using System.Xml;
using System.IO;




namespace ChemCloud.Web.Areas.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class ShipmentController : BaseMemberController
    {
        // GET: Shipment/Index
        public ActionResult Index(long orderId)
        {
            ShipmentEx ship = ServiceHelper.Create<IShipmentService>().GetShipmnet(orderId);

            if (ship != null)
            {
                ViewBag.model = ship;
            }
            return View();
        }


        private CountryInfo GetCountry(List<CountryInfo> countryInfo, int id)
        {
            CountryInfo result = null;
            CountryInfo orgCountry = countryInfo.SingleOrDefault(x => x.ID == id);

            if (orgCountry.PID != 0)
            {
                result = GetCountry(countryInfo, orgCountry.PID);
            }
            else
            {
                result = orgCountry;
            }

            return result;
        }


        public ActionResult UpdateSFCode()
        {
            List<CountryInfo> countyList = ServiceHelper.Create<IShipmentService>().GetCityList();
            List<CountryInfo> cityList = countyList.Where(x => x.PID == 336).ToList();

            string url = string.Empty;
            url = string.Format("http://www.sf-express.com/sf-service-web/service/region/A000086000/subRegions/origins?originCode=A330781000&level=-1&lang=sc&region=cn");

            using (var webClient = new System.Net.WebClient())
            {
                webClient.Encoding = System.Text.Encoding.UTF8;//定义对象的编码语言,此处或者是gb2312
                var json = webClient.DownloadString(url);

                List<SFCountry> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SFCountry>>(json);

                //更新国家
                foreach (SFCountry item in list)
                {
                    List<CountryInfo> tmpList = cityList.Where(x => item.name.Contains(x.ContryNameCN)).ToList();
                    if (tmpList.Count > 0)
                    {
                        string strSql = string.Format("Update ChemCloud_Country Set sfcode='{0}' where id={1}", item.code, tmpList[0].ID);
                        DBUtility.DbHelperSQL.ExecuteSql(strSql);
                        UpdateSFChina(tmpList[0].ID, item.code, 0);
                    }


                }


            }

            return View();
        }

        private void UpdateSFChina(int pid, string code, int level)
        {
            List<CountryInfo> countyList = ServiceHelper.Create<IShipmentService>().GetCityList();
            List<CountryInfo> cityList = countyList.Where(x => x.PID == pid).ToList();
            string url = string.Empty;
            url = string.Format("http://www.sf-express.com/sf-service-web/service/region/{0}/subRegions/origins?originCode=A330781000&level={1}&lang=sc&region=cn", code, level);

            using (var webClient = new System.Net.WebClient())
            {
                webClient.Encoding = System.Text.Encoding.UTF8;//定义对象的编码语言,此处或者是gb2312
                var json = webClient.DownloadString(url);

                List<SFCountry> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SFCountry>>(json);
                level++;
                //更新国家
                foreach (SFCountry item in list)
                {
                    List<CountryInfo> tmpList = cityList.Where(x => item.name.Contains(x.ContryNameCN)).ToList();
                    if (tmpList.Count > 0)
                    {
                        string strSql = string.Format("Update ChemCloud_Country Set sfcode='{0}' where id={1}", item.code, tmpList[0].ID);
                        DBUtility.DbHelperSQL.ExecuteSql(strSql);

                        UpdateSFChina(tmpList[0].ID, item.code, level);
                    }


                }


            }
        }

        public ActionResult Test()
        {
            List<CountryInfo> countyList = ServiceHelper.Create<IShipmentService>().GetCityList();

            List<CountryInfo> cityList = countyList.Where(x => x.PID != 0).ToList();
            string url = string.Empty;
            foreach (CountryInfo item in cityList)
            {
                CountryInfo tCountry = GetCountry(countyList, item.PID);
                string code = item.Code;
                url = string.Format("http://dct.dhl.com/data/postLoc?start=0&max=1&queryBy=1&cntryCd={0}&cityNmStart={1}", tCountry.Code, item.ContryNameEN);

                XmlDocument doc = GetXMLFromUrl(url);

                if (doc != null)
                {
                    XmlNodeList studentNodeList = doc.SelectNodes("postalLocationResponse/postalLocationList/postalLocation");
                    if (studentNodeList != null)
                    {
                        foreach (XmlNode studentNode in studentNodeList)
                        {
                            string name = studentNode.LastChild.InnerText;

                            string strSql = string.Format("Update ChemCloud_Country Set code='{0}' where id={1}", name, item.ID);
                            DBUtility.DbHelperSQL.ExecuteSql(strSql);
                        }
                    }
                }
            }




            return View();
        }

        private void UpdateCountry()
        {
            object response = LoadFromXml(@"C:\liup\country.xml", typeof(dhlCountryResponse));

            if (response != null)
            {
                List<CountryInfo> countryList = ServiceHelper.Create<IShipmentService>().GetCountyList();
                dhlCountryResponse dhlResponse = response as dhlCountryResponse;

                foreach (CountryInfo item in countryList)
                {
                    List<dhlCountry> dhlCountryList = dhlResponse.dhlCountryList.Where(x => x.countryDhlName.Contains(item.ContryNameEN.ToUpper())).ToList();

                    if (dhlCountryList.Count > 0)
                    {
                        string code = dhlCountryList[0].countryCode;


                        string strSql = string.Format("Update ChemCloud_Country Set code='{0}' where id={1}", code, item.ID);
                        DBUtility.DbHelperSQL.ExecuteSql(strSql);
                    }
                }
                // ServiceHelper.Create<IShipmentService>().UpdateCountyList();


            }
        }

        private object LoadFromXml(string filePath, Type type)
        {
            object result = null;


            using (StreamReader reader = new StreamReader(filePath))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                result = xmlSerializer.Deserialize(reader);
            }


            return result;
        }
        private XmlDocument GetXMLFromUrl(string strUrl)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(strUrl);
            return doc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public string CalcPrice(FedExQueryAdv ship)
        {
            var result = string.Empty;
            switch (ship.ShipType)
            {
                case "FedEx":
                    result = CalcFedex(ship);
                    break;
                case "SF":
                    result = CalcSF(ship);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Fedex物流计算
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private string CalcFedex(FedExQueryAdv ship)
        {
            var result = string.Empty;
            //1：CNY 2：USD
            ship.CoinType = ConfigurationManager.AppSettings["CoinType"] == "1" ? "CNY" : "USD";

            var appSettings = ConfigurationManager.AppSettings;
            ship.FedexKey = appSettings["FedExKey"];
            ship.FedexPassword = appSettings["FedExPassword"];
            ship.FedexAccountNumber = appSettings["FedExAccountNumber"];
            ship.FedexMeterNumber = appSettings["FedExMeterNumber"];

            ShipmentEx shipment = null;

            if (ship.ShopId > 0)
            {
                //获得采购商地址
                ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(ship.ShopId, true);
                ship.OrigId = shop.CompanyRegionId;
                ship.OrigAddress = shop.CompanyAddress;
                ship.OrigPostCode = shop.ZipCode;

                shipment = ServiceHelper.Create<IShipmentService>().CalcPriceAdv(ship);
                if (shipment != null)
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(shipment);
            }
            else
            {
                shipment = ServiceHelper.Create<IShipmentService>().CalcPrice(ship);
                if (shipment != null)
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(shipment);
            }




            return result;
        }

        private string CalcSF(FedExQueryAdv ship)
        {
            var result = string.Empty;
            //1：CNY 2：USD
            ship.CoinType = ConfigurationManager.AppSettings["CoinType"] == "1" ? "CNY" : "USD";


            ShipmentEx shipment = null;


            //获得采购商地址
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(ship.ShopId, true);
            if (shop != null)
            {
                ship.OrigId = shop.CompanyRegionId;
                ship.OrigAddress = shop.CompanyAddress;
                ship.OrigPostCode = shop.ZipCode;
            }
           

            shipment = ServiceHelper.Create<IShipmentService>().CalcSFCostAdv(ship);
            if (shipment != null)
                result = Newtonsoft.Json.JsonConvert.SerializeObject(shipment);

            return result;
        }


        /// <summary>
        /// Fedex物流费计算
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public string CalcPriceAdv(FedExQueryAdv ship)
        {
            var result = string.Empty;
            //1：CNY 2：USD
            ship.CoinType = ConfigurationManager.AppSettings["CoinType"] == "1" ? "CNY" : "USD";

            var appSettings = ConfigurationManager.AppSettings;
            ship.FedexKey = appSettings["FedExKey"];
            ship.FedexPassword = appSettings["FedExPassword"];
            ship.FedexAccountNumber = appSettings["FedExAccountNumber"];
            ship.FedexMeterNumber = appSettings["FedExMeterNumber"];

            ShipmentEx shipment = null;

            if (ship.ShopId > 0)
            {
                //获得采购商地址
                ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(ship.ShopId, true);
                ship.OrigId = shop.CompanyRegionId;
                ship.OrigAddress = shop.CompanyAddress;
                ship.OrigPostCode = shop.ZipCode;

                shipment = ServiceHelper.Create<IShipmentService>().CalcPriceAdv(ship);
                if (shipment != null)
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(shipment);
            }
            else
            {
                shipment = ServiceHelper.Create<IShipmentService>().CalcPrice(ship);
                if (shipment != null)
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(shipment);
            }




            return result;
        }
    }

    public class dhlCountryResponse
    {
        public int count { get; set; }

        public List<dhlCountry> dhlCountryList { get; set; }
    }

    public class dhlCountry
    {
        public string countryCode { get; set; }
        public string countryDhlName { get; set; }
        public string isoCurrCd { get; set; }
    }

    public class SFCountry
    {
        public string distId { get; set; }

        public string level { get; set; }
        public string code { get; set; }

        public string parentId { get; set; }
        public string countryCode { get; set; }

        public string name { get; set; }

        public string lang { get; set; }

        public bool available { get; set; }

        public bool opening { get; set; }
    }
}


