using ChemCloud.DBUtility;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemColud.Shipping;
using ChemColud.Shipping.ShippingProviders;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class ShipmentService : ServiceBase, IShipmentService, IService, IDisposable
    {

        private Dictionary<string, string> dicList = new Dictionary<string, string>() { 
        {"6","FedEx Envelope"},{"2","FedEx Pak"},{"13","FedEx Small Box"},{"23","FedEx Medium Box"},
        {"33","FedEx Large Box"},{"43","FedEx Extra Large Box"},{"4","FedEx Tube"},{"5","FedEx 10kg Box"},
        {"25","FedEx 25kg Box"},{"1","自备包装"}
        };



        private static object obj = new object();
        public ShipmentService()
        {

        }

        private long GenerateNumber()
        {
            long num;
            lock (ShipmentService.obj)
            {
                string empty = string.Empty;
                Guid guid = Guid.NewGuid();
                Random random = new Random(BitConverter.ToInt32(guid.ToByteArray(), 0));
                for (int i = 0; i < 5; i++)
                {
                    int num1 = random.Next();
                    char chr = (char)(48 + (ushort)(num1 % 10));
                    empty = string.Concat(empty, chr.ToString());
                }
                DateTime now = DateTime.Now;
                num = long.Parse(string.Concat(now.ToString("yyyyMMddfff"), empty));
            }
            return num;
        }

        /// <summary>
        /// 保存物流信息
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="orderId"></param>
        public void SaveShipmentEx(ShipmentEx shipment, long orderId)
        {
            try
            {
                Shipment ship = new Shipment();

                Address oriAddress = shipment.OriginAddress;

                Address destAddress = shipment.DestinationAddress;

                Rate rate = null;

                if (shipment.RateValue != null)
                {
                    rate = shipment.RateValue;
                }
                else
                {
                    rate = new Rate();
                    rate.GuaranteedDelivery = DateTime.Now;
                }

                ship.Id = GenerateNumber();
                ship.OrderId = orderId;
                ship.PackageCount = shipment.PackageCount;
                ship.TotalPackageWeight = shipment.TotalPackageWeight;
                if (!string.IsNullOrWhiteSpace(rate.Name))
                    ship.RateName = rate.Name.Trim();
                else
                    ship.RateName = null;
                ship.GurDeliveryDate = rate.GuaranteedDelivery == null ? DateTime.Now : Convert.ToDateTime(rate.GuaranteedDelivery);
                ship.RateProvider = rate.Provider;
                ship.RateProviderCode = rate.ProviderCode;
                ship.TotalCharges = rate.TotalCharges;
                context.Shipment.Add(ship);
                ShipmentAddress orgAddress = new ShipmentAddress();
                orgAddress.ShipmentId = ship.Id;
                orgAddress.City = oriAddress.City;
                orgAddress.CountryCode = oriAddress.CountryCode;
                orgAddress.CountryName = oriAddress.CountryName;
                orgAddress.Line1 = oriAddress.Line1;
                orgAddress.Line2 = oriAddress.Line2;
                orgAddress.Line3 = oriAddress.Line3;
                orgAddress.State = "Origin";
                orgAddress.PostalCode = oriAddress.PostalCode;

                context.ShipmentAddress.Add(orgAddress);

                ShipmentAddress DAddress = new ShipmentAddress();
                DAddress.ShipmentId = ship.Id;
                DAddress.City = destAddress.City;
                DAddress.CountryCode = destAddress.CountryCode;
                DAddress.CountryName = destAddress.CountryName;
                DAddress.Line1 = destAddress.Line1;
                DAddress.Line2 = destAddress.Line2;
                DAddress.Line3 = destAddress.Line3;
                DAddress.State = "Dest";
                DAddress.PostalCode = destAddress.PostalCode;

                context.ShipmentAddress.Add(DAddress);

                //包裹
                List<Package> packages = shipment.Packages;
                ShipmentPackage shipPgk = null;
                foreach (Package item in packages)
                {
                    shipPgk = new ShipmentPackage();
                    shipPgk.ShipmentId = ship.Id;
                    shipPgk.Currency = item.Currency;
                    shipPgk.Height = item.Height;
                    shipPgk.InsuredValue = item.InsuredValue;
                    shipPgk.Length = item.Length;
                    shipPgk.PackageType = item.PackageType;

                    shipPgk.ShipLinearUnit = item.ShipLinearUnit == LinearUnit.CM ? "CM" : "IN";
                    shipPgk.ShipWeightUnit = item.ShipWeightUnit == WeightUnit.KG ? "KG" : "LB";

                    shipPgk.Weight = item.Weight;
                    shipPgk.Width = item.Width;

                    context.ShipmentPackage.Add(shipPgk);
                }


                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

            }

        }
        /// <summary>
        /// 保存物流信息
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int SaveShipment(ShipmentEx shipment, long orderId)
        {
            int result = 0;
            try
            {

                List<string> sqlList = new List<string>();
                long ident = -1;

                Rate rate = null;

                if (shipment.Rates.Count > 0)
                {
                    rate = shipment.Rates[0];
                }
                else
                {
                    rate = new Rate();
                }
                long shipObj = GenerateNumber();

                //物流信息
                StringBuilder sqlString = new StringBuilder();

                sqlString.AppendFormat(@"INSERT INTO ChemCloud_Shipment (Id,OrderId,PackageCount,TotalPackageWeight,GurDeliveryDate,RateName,RateProvider,RateProviderCode,TotalCharges)
                       VALUES({0},{1},{2},{3},'{4}','{5}','{6}','{7}',{8})", shipObj, orderId, shipment.PackageCount, shipment.TotalPackageWeight, rate.GurDeliveryDate, rate.Name, rate.Provider, rate.ProviderCode, rate.TotalCharges);
                sqlList.Add(sqlString.ToString());

                sqlString.Clear();


                ident = shipObj;
                Address oriAddress = shipment.OriginAddress;
                //始运地
                sqlString.AppendFormat(@"INSERT INTO ChemCloud_ShipmentAddress (ShipmentId,CountryCode,CountryName,PostalCode,City,State,Line1,Line2,Line3)
                       VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                       ident, oriAddress.CountryCode, oriAddress.CountryName, oriAddress.PostalCode, oriAddress.City, "Origin", oriAddress.Line1, oriAddress.Line2, oriAddress.Line3);
                sqlList.Add(sqlString.ToString());

                sqlString.Clear();

                //目的地

                Address destAddress = shipment.DestinationAddress;

                sqlString.AppendFormat(@"INSERT INTO ChemCloud_ShipmentAddress (ShipmentId,CountryCode,CountryName,PostalCode,City,State,Line1,Line2,Line3)
                       VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                       ident, destAddress.CountryCode, destAddress.CountryName, destAddress.PostalCode, destAddress.City, "Dest", destAddress.Line1, destAddress.Line2, destAddress.Line3);
                sqlList.Add(sqlString.ToString());

                //包裹
                List<Package> packages = shipment.Packages;
                foreach (Package item in packages)
                {
                    sqlString.Clear();

                    sqlString.AppendFormat(@"INSERT INTO ChemCloud_ShipmentPackage (ShipmentId,Weight,ShipWeightUnit,Currency,PackageType,Length,Width,Height,InsuredValue,ShipLinearUnit)
                       VALUES({0},{1},'{2}','{3}','{4}','{5}',{6},{7},{8},'{9}')",
                      ident, item.Weight, item.ShipWeightUnit, item.Currency, item.PackageType, item.Length, item.Weight, item.Height, item.InsuredValue, item.ShipLinearUnit);
                    sqlList.Add(sqlString.ToString());
                }


                using (TransactionScope transactionScope = new TransactionScope())
                {
                    result = DbHelperSQL.ExecuteSqlTran(sqlList);

                    transactionScope.Complete();
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }


        /// <summary>
        /// 货物fed物流信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ShipmentEx GetShipmentByOrder(long orderId)
        {
            ShipmentEx shipEx = null;
            try
            {
                //shipEx = new ShipmentEx();
                Shipment ship = context.Shipment.FirstOrDefault(x => x.OrderId == orderId);

                if (ship != null)
                {
                    ShipmentAddress orgAddress = context.ShipmentAddress.FirstOrDefault(x => (x.ShipmentId == ship.Id) && (x.State == "Origin"));
                    ShipmentAddress destAddress = context.ShipmentAddress.FirstOrDefault(x => (x.ShipmentId == ship.Id) && (x.State == "Dest"));

                    List<ShipmentPackage> pkgList = context.ShipmentPackage.Where(x => x.ShipmentId == ship.Id).ToList();

                    Address orgAddressEx = new Address(orgAddress.Line1, orgAddress.Line2,
                        orgAddress.Line3, orgAddress.City, "", orgAddress.PostalCode, orgAddress.CountryCode);
                    orgAddressEx.CountryName = orgAddress.CountryName;

                    Address destAddressEx = new Address(destAddress.Line1, destAddress.Line2,
                      destAddress.Line3, destAddress.City, "", destAddress.PostalCode, destAddress.CountryCode);
                    destAddressEx.CountryName = destAddress.CountryName;
                    List<Package> pkgExList = new List<Package>();
                    Package package = null;
                    string packType = string.Empty;
                    foreach (ShipmentPackage item in pkgList)
                    {
                        WeightUnit wUnit = item.ShipWeightUnit == "KG" ? WeightUnit.KG : WeightUnit.LB;
                        LinearUnit lUnit = item.ShipLinearUnit == "CM" ? LinearUnit.CM : LinearUnit.IN;
                        package = new Package(item.Length, item.Width, item.Height, item.Weight, item.InsuredValue, wUnit, lUnit);

                        dicList.TryGetValue(item.PackageType, out packType);
                        dicList.TryGetValue(item.PackageType, out packType);
                        package.PackageType = packType;
                        pkgExList.Add(package);

                    }
                    Rate rate = new Rate(ship.RateProvider, ship.RateProviderCode, ship.RateName, ship.TotalCharges, ship.GurDeliveryDate);

                    shipEx = new ShipmentEx(orgAddressEx, destAddressEx, pkgExList);
                    shipEx.RateValue = rate;


                }
            }
            catch
            {

            }
            return shipEx;
        }

        /// <summary>
        /// 计算fedex物流费用
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        public ShipmentEx CalcPrice(FedExQueryAdv ship)
        {
            ShipmentEx result = null;
            try
            {
                string countyCode = string.Empty;
                string countyName = string.Empty;
                CountryInfo county = null;

                var packages = new List<Package>();

                Package pkg = null;

                ShippingAddressInfo orgAddress = context.ShippingAddressInfo.SingleOrDefault(x => x.Id == ship.OrigId);

                //获取始发地

                CountryInfo ortCity = context.CountryInfo.SingleOrDefault(x => x.ID == orgAddress.RegionId);

                county = GetCountry(ortCity.PID);

                if (county != null)
                {
                    countyCode = county.Code;
                    countyName = county.ContryNameEN;
                }

                var origin = new Address(orgAddress.Address, "", "", ortCity.ContryNameEN, "", ortCity.Code, countyCode);
                origin.CountryName = countyName;



                //获取目的地

                ShippingAddressInfo destAddress = context.ShippingAddressInfo.SingleOrDefault(x => x.Id == ship.DestId);
                CountryInfo destCity = context.CountryInfo.SingleOrDefault(x => x.ID == destAddress.RegionId);
                county = null;
                county = GetCountry(destCity.PID);

                if (county != null)
                {
                    countyCode = county.Code;
                    countyName = county.ContryNameEN;
                }

                var destination = new Address(destAddress.Address, "", "", destCity.ContryNameEN, "", destCity.Code, countyCode);
                destination.CountryName = countyName;


                foreach (ShipPackage item in ship.PackagesList)
                {
                    if (item.Num > 0)
                    {
                        for (int i = 0; i < item.Num; i++)
                        {
                            pkg = GetPackage(item);
                            pkg.Currency = ship.CoinType;
                            pkg.PackageCount = 1;
                            pkg.PackageType = "自备包装";
                            packages.Add(pkg);
                        }
                    }
                    //pkg = GetPackage(item);
                    //pkg.Currency = ship.CoinType;
                    //pkg.PackageCount = item.Num;
                    //pkg.PackageType = "自备包装";
                    //packages.Add(pkg);
                }

                var rateManager = new RateManager();

                rateManager.AddProvider(new FedExProvider(ship.FedexKey, ship.FedexPassword, ship.FedexAccountNumber, ship.FedexMeterNumber));


                result = rateManager.GetRates(origin, destination, packages);

                if (result.Rates != null && result.Rates.Count > 0)
                {
                    result.RateValue = result.Rates[0];
                }


            }
            catch (Exception ex)
            {

            }

            return result;
        }
        /// <summary>
        /// 获取用户收货地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public Address GetAddress(long userId, int regionId)
        {
            Address destination = null;
            List<ShippingAddressInfo> List = context.ShippingAddressInfo.Where(x => x.UserId == userId && x.RegionId == regionId).ToList();
            if (List.Count > 0)
            {
                ShippingAddressInfo destAddress = List[0];
                CountryInfo destCity = context.CountryInfo.SingleOrDefault(x => x.ID == destAddress.RegionId);
                CountryInfo county = null;

                // CountryInfo destCity = context.CountryInfo.SingleOrDefault(x => x.ID == regionId);
                county = null;
                county = GetCountry(destCity.PID);

                string countyCode = string.Empty;
                string countyName = string.Empty;

                if (county != null)
                {
                    countyCode = county.Code;
                    countyName = county.ContryNameEN;
                }

                destination = new Address("", "", "", destCity.ContryNameEN, "", destAddress.PostCode, countyCode);
                destination.CountryName = countyName;

                //美国和加拿大需要州缩写
                if (countyCode == "US")
                {
                    CountryInfo parentContry = GetParentCounty(destCity.PID);
                    if (parentContry != null)
                    {
                        destination.StateOrProvinceCode = parentContry.Code;
                    }

                }

            }

            return destination;
        }

        /// <summary>
        /// 获得物流信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ShipmentEx GetShipmnet(long orderId)
        {

            ShipmentEx shipEx = null;
            try
            {
                //shipEx = new ShipmentEx();
                Shipment ship = context.Shipment.FirstOrDefault(x => x.OrderId == orderId);

                if (ship != null)
                {
                    ShipmentAddress orgAddress = context.ShipmentAddress.FirstOrDefault(x => (x.ShipmentId == ship.Id) && (x.State == "Origin"));
                    ShipmentAddress destAddress = context.ShipmentAddress.FirstOrDefault(x => (x.ShipmentId == ship.Id) && (x.State == "Dest"));

                    List<ShipmentPackage> pkgList = context.ShipmentPackage.Where(x => x.ShipmentId == ship.Id).ToList();

                    Address orgAddressEx = new Address(orgAddress.Line1, orgAddress.Line2,
                        orgAddress.Line3, orgAddress.City, "", orgAddress.PostalCode, orgAddress.CountryCode);
                    orgAddressEx.CountryName = orgAddress.CountryName;

                    Address destAddressEx = new Address(destAddress.Line1, destAddress.Line2,
                      destAddress.Line3, destAddress.City, "", destAddress.PostalCode, destAddress.CountryCode);
                    destAddressEx.CountryName = destAddress.CountryName;

                    List<Package> pkgExList = new List<Package>();
                    Package package = null;
                    foreach (ShipmentPackage item in pkgList)
                    {
                        WeightUnit wUnit = item.ShipWeightUnit == "KG" ? WeightUnit.KG : WeightUnit.LB;
                        LinearUnit lUnit = item.ShipLinearUnit == "CM" ? LinearUnit.CM : LinearUnit.IN;
                        package = new Package(item.Length, item.Width, item.Height, item.Weight, item.InsuredValue, wUnit, lUnit);
                        package.PackageType = item.PackageType;
                        pkgExList.Add(package);

                    }
                    Rate rate = new Rate(ship.RateProvider, ship.RateProviderCode, ship.RateName, ship.TotalCharges, ship.GurDeliveryDate);

                    shipEx = new ShipmentEx(orgAddressEx, destAddressEx, pkgExList);
                    shipEx.RateValue = rate;



                }
            }
            catch
            {

            }
            return shipEx;
        }
        /// <summary>
        /// 计算Fedex费用
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>

        public ShipmentEx CalcPriceAdv(FedExQueryAdv ship)
        {
            ShipmentEx result = null;
            try
            {
                string countyCode = string.Empty;
                string countyName = string.Empty;
                CountryInfo county = null;

                var packages = new List<Package>();

                Package pkg = null;

                //获取始发地

                //ShippingAddressInfo orgAddress = context.ShippingAddressInfo.SingleOrDefault(x => x.Id == ship.OrigId);

                //CountryInfo ortCity = context.CountryInfo.SingleOrDefault(x => x.ID == orgAddress.RegionId);

                //county = GetCountry(ortCity.PID);

                //if (county != null)
                //{
                //    countyCode = county.Code;
                //    countyName = county.ContryNameEN;
                //}

                //var origin = new Address(orgAddress.Address, "", "", ortCity.ContryNameEN, "", ortCity.Code, countyCode);
                //origin.CountryName = countyName;


                CountryInfo ortCity = context.CountryInfo.SingleOrDefault(x => x.ID == ship.OrigId);

                county = GetCountry(ortCity.PID);

                if (county != null)
                {
                    countyCode = county.Code;
                    countyName = county.ContryNameEN;
                }

                var origin = new Address(ship.OrigAddress, "", "", ortCity.ContryNameEN, "", ship.OrigPostCode, countyCode);
                origin.CountryName = countyName;


                //获取目的地
                ShippingAddressInfo destAddress = context.ShippingAddressInfo.SingleOrDefault(x => x.Id == ship.DestId);
                CountryInfo destCity = context.CountryInfo.SingleOrDefault(x => x.ID == destAddress.RegionId);
                county = null;
                county = GetCountry(destCity.PID);

                if (county != null)
                {
                    countyCode = county.Code;
                    countyName = county.ContryNameEN;
                }

                var destination = new Address(destAddress.Address, "", "", destCity.ContryNameEN, "", destAddress.PostCode, countyCode);
                destination.CountryName = countyName;



                foreach (ShipPackage item in ship.PackagesList)
                {
                    //if (item.Num > 0)
                    //{
                    //    for (int i = 0; i < item.Num; i++)
                    //    {
                    //        pkg = GetPackage(item);
                    //        pkg.Currency = ship.CoinType;
                    //        pkg.PackageCount = 1;
                    //        pkg.PackageType = "自备包装";
                    //        packages.Add(pkg);
                    //    }
                    //}
                    pkg = GetPackage(item);
                    pkg.Currency = ship.CoinType;
                    pkg.PackageCount = item.Num;
                    pkg.PackageType = "自备包装";
                    packages.Add(pkg);
                }

                var rateManager = new RateManager();

                rateManager.AddProvider(new FedExProvider(ship.FedexKey, ship.FedexPassword, ship.FedexAccountNumber, ship.FedexMeterNumber));


                result = rateManager.GetRates(origin, destination, packages);

                if (result.Rates != null && result.Rates.Count > 0)
                {
                    result.RateValue = result.Rates[0];
                }


            }
            catch (Exception ex)
            {

            }

            return result;
        }
        /// <summary>
        /// 设置包裹单位
        /// </summary>
        /// <param name="pkg"></param>
        /// <returns></returns>
        private Package GetPackage(ShipPackage pkg)
        {
            Package package = null;
            if (!string.IsNullOrEmpty(pkg.PackingUnit))
            {

                string pkgUnit = pkg.PackingUnit.ToUpper();
                if (pkgUnit.Contains("ML"))
                {
                    if (pkgUnit.Contains("-"))
                    {
                        pkgUnit = pkgUnit.Substring(0, pkgUnit.LastIndexOf("-"));
                    }
                    else
                    {
                        pkgUnit = pkgUnit.Substring(0, pkgUnit.LastIndexOf("ML"));
                    }

                }
                else
                {
                    if (pkgUnit.Contains("-"))
                    {
                        pkgUnit = pkgUnit.Substring(0, pkgUnit.LastIndexOf("-"));
                    }
                    else
                    {
                        pkgUnit = pkgUnit.Substring(0, pkgUnit.LastIndexOf("G"));
                    }

                }
                int unitCount = Convert.ToInt32(pkgUnit);

                List<Package> dicList = ChemPackage.GetPackageList();

                dicList = dicList.Where(x => x.TotalSize >= unitCount).OrderBy(x => x.TotalSize).ToList();


                if (dicList.Count > 0)
                {
                    package = dicList[0];
                }

            }

            return package;
        }
        /// <summary>
        /// 设置包裹单位信息
        /// </summary>
        /// <param name="pkg"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        private Package GetPackage(ShipPackage pkg, decimal thickness)
        {
            Package package = null;
            if (!string.IsNullOrEmpty(pkg.PackingUnit))
            {

                string pkgUnit = pkg.PackingUnit.ToUpper();
                if (pkgUnit.Contains("ML"))
                {
                    if (pkgUnit.Contains("-"))
                    {
                        pkgUnit = pkgUnit.Substring(0, pkgUnit.LastIndexOf("-"));
                    }
                    else
                    {
                        pkgUnit = pkgUnit.Substring(0, pkgUnit.LastIndexOf("ML"));
                    }

                }
                else
                {
                    if (pkgUnit.Contains("-"))
                    {
                        pkgUnit = pkgUnit.Substring(0, pkgUnit.LastIndexOf("-"));
                    }
                    else
                    {
                        pkgUnit = pkgUnit.Substring(0, pkgUnit.LastIndexOf("G"));
                    }

                }
                int unitCount = Convert.ToInt32(pkgUnit);

                List<Package> dicList = ChemPackage.GetPackageList(thickness);

                dicList = dicList.Where(x => x.TotalSize >= unitCount).OrderBy(x => x.TotalSize).ToList();


                if (dicList.Count > 0)
                {
                    package = dicList[0];
                }

            }

            return package;
        }
        /// <summary>
        /// 获取地址信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CountryInfo GetCountry(int id)
        {
            CountryInfo result = null;
            CountryInfo orgCountry = context.CountryInfo.SingleOrDefault(x => x.ID == id);

            if (orgCountry.PID != 0)
            {
                result = GetCountry(orgCountry.PID);
            }
            else
            {
                result = orgCountry;
            }

            return result;
        }

        /// <summary>
        /// 获得父级地址信息
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public CountryInfo GetParentCounty(int pid)
        {
            CountryInfo result = null;
            result = context.CountryInfo.SingleOrDefault(x => x.ID == pid);



            return result;
        }

        /// <summary>
        /// 获得国家信息
        /// </summary>
        /// <returns></returns>
        public List<CountryInfo> GetCountyList()
        {
            return context.CountryInfo.Where(x => x.PID == 0).ToList();
        }

        /// <summary>
        /// 获得城市信息
        /// </summary>
        /// <returns></returns>
        public List<CountryInfo> GetCityList()
        {
            return context.CountryInfo.ToList();
        }

        /// <summary>
        /// 计算顺丰物流费用
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        public ShipmentEx CalcSFCostAdv(FedExQueryAdv ship)
        {
            ShipmentEx result = null;
            try
            {
                SFRateRequest rateRequest = new SFRateRequest();
                string countyCode = string.Empty;
                string countyName = string.Empty;
                CountryInfo county = null;

                var packages = new List<Package>();

                Package pkg = null;


                CountryInfo ortCity = context.CountryInfo.SingleOrDefault(x => x.ID == ship.OrigId);

                if (ortCity != null)
                {
                    county = GetCountry(ortCity.PID);

                    if (county != null)
                    {
                        countyCode = county.Code;
                        countyName = county.ContryNameEN;

                        if (countyCode.ToUpper() != "CN")
                        {
                            rateRequest.OriginCode = county.SFCode;
                            rateRequest.ParentOriginCode = county.SFCode;
                        }
                        else
                        {
                            rateRequest.OriginCode = ortCity.SFCode;
                            rateRequest.ParentOriginCode = ortCity.SFCode;
                        }
                    }


                }


                var origin = new Address(ship.OrigAddress, "", "", ortCity.ContryNameEN, "", ship.OrigPostCode, countyCode);
                origin.CountryName = countyName;


                //获取目的地

                ShippingAddressInfo destAddress = context.ShippingAddressInfo.SingleOrDefault(x => x.Id == ship.DestId);
                CountryInfo destCity = context.CountryInfo.SingleOrDefault(x => x.ID == destAddress.RegionId);

                if (destCity != null)
                {
                    county = null;
                    county = GetCountry(destCity.PID);

                    if (county != null)
                    {
                        countyCode = county.Code;
                        countyName = county.ContryNameEN;
                        if (countyCode.ToUpper() != "CN")
                        {
                            rateRequest.DestCode = county.SFCode;
                            rateRequest.ParentDestCode = county.SFCode;
                        }
                        else
                        {
                            rateRequest.DestCode = destCity.SFCode;
                            rateRequest.ParentDestCode = destCity.SFCode;
                        }
                    }


                }

                var destination = new Address(destAddress.Address, "", "", destCity.ContryNameEN, "", destAddress.PostCode, countyCode);
                destination.CountryName = countyName;

                rateRequest.Volume = 0;
                rateRequest.QueryType = 5;
                rateRequest.Lang = "sc";
                rateRequest.Region = "cn";

                foreach (ShipPackage item in ship.PackagesList)
                {
                    pkg = GetPackage(item, 6000);
                    pkg.Currency = ship.CoinType;
                    pkg.PackageCount = item.Num;
                    pkg.PackageType = "自备包装";
                    packages.Add(pkg);

                   
                }

                rateRequest.Weight = Math.Round(packages.Sum(x => x.Weight), 2);
                // rateRequest.Time = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm");//"2016-07-06T13%3A30%3A00%2B08%3A00"

                DateTime dtNow = DateTime.Now;
                rateRequest.Time = string.Format("{0}T{1}%3A{2}%3A{3}%2B08%3A00", dtNow.ToString("yyyy-MM-dd"), dtNow.ToString("hh"), dtNow.ToString("mm"), dtNow.ToString("ss"));

                var rateManager = new RateManager();

                rateManager.AddProvider(new SFProvider(rateRequest));


                result = rateManager.GetRates(origin, destination, packages);

                if (result.Rates != null && result.Rates.Count > 0)
                {
                    result.RateValue = result.Rates[0];
                }


            }
            catch (Exception ex)
            {

            }

            return result;
        }

        /// <summary>
        /// 生成运单
        /// </summary>
        public ShipReply CreateShip(CreateShipQuery shipQuery)
        {
            ShipReply reply = new ShipReply();
            FedExShipService shipService = new FedExShipService(shipQuery.Url, shipQuery.Key, shipQuery.Password, shipQuery.AccountNumber, shipQuery.MeterNumber);
            List<Package> packages = new List<Package>();
            Package pkg;
            foreach (ShipPackage item in shipQuery.ShipPkgs)
            {
                //pkg = GetPackage(item);
                //pkg.Currency = shipQuery.CoinType;
                //pkg.Description = item.Description;
                //pkg.PackageCount = item.Num;
                //pkg.PackageType = "自备包装";
                //packages.Add(pkg);

                if (item.Num > 0)
                {
                    for (int i = 0; i < item.Num; i++)
                    {
                        pkg = GetPackage(item);
                        pkg.Currency = shipQuery.CoinType;
                        pkg.Description = item.Description;
                        pkg.PackageCount = 1;
                        pkg.PackageType = "自备包装";
                        packages.Add(pkg);
                    }
                }
            }
            try
            {
                ShipmentEx shipEx = new ShipmentEx(shipQuery.Origin, shipQuery.Dest, packages);
                shipEx.Currency = shipQuery.CoinType;

                CreateShipRep shipRep= shipService.CreateShip(shipEx);
                List<ShipReply> replyList = shipRep.ReplyList;
                reply.ErrorMessage = shipRep.Message;
                if (replyList != null)
                {
                    foreach (ShipReply item in replyList)
                    {
                        OrderShip orderShip = new OrderShip() { OrderId = shipQuery.OrderId, OrderItemId = shipQuery.OrderItemId, TrackFormId = item.TrackFormId, TrackNumber = item.TrackNumber };
                        //  orderShip.LabelImage = item.LabelImage;
                        string result = System.Text.Encoding.UTF8.GetString(item.LabelImage);
                        orderShip.LabelImage = item.LabelImage;
                        orderShip.Master = item.Master;
                        context.OrderShip.Add(orderShip);

                    }
                    context.SaveChanges();
                    List<ShipReply>  tmpShip=replyList.Where(x => x.Master == true).ToList();
                    if (tmpShip.Count>0)
                    {
                        reply = tmpShip[0];
                    }
                    
                }

            }
            catch (DbEntityValidationException dbEx)
            {

            }
            catch (Exception ex)
            {

            }

            return reply;
        }

        /// <summary>
        /// 查询订单的所有面单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderShip> GetOrderShip(long orderId)
        {
            List<OrderShip> result = new List<OrderShip>();
            result = context.OrderShip.Where(x => x.OrderId == orderId).ToList();
            return result;
        }

        /// <summary>
        /// 获取面单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       public OrderShip GetShipOrder(long id) 
       {
           OrderShip orderShip = null;
           orderShip = context.OrderShip.SingleOrDefault(x=>x.Id==id);

           return orderShip;
       }

    }
}
