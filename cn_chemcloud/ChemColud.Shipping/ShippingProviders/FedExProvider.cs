using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

using ChemColud.Shipping.RateServiceWebReference;
using System.Diagnostics;



namespace ChemColud.Shipping.ShippingProviders
{ /// <summary>
    ///    提供Fedex rate计算
    /// </summary>
    public class FedExProvider : AbstractShippingProvider
    {
        private readonly string _accountNumber;
        private readonly string _key;
        private readonly string _meterNumber;
        private readonly string _password;
        private readonly Dictionary<string, string> _serviceCodes = new Dictionary<string, string>
        {
            {"PRIORITY_OVERNIGHT", "优先隔夜联邦快递"},
            {"FEDEX_2_DAY", "两日联邦快递"},
            {"FEDEX_2_DAY_AM", "两日上午联邦快递"},
            {"STANDARD_OVERNIGHT", "标准隔夜联邦快递"},
            {"FIRST_OVERNIGHT", "首个隔夜联邦快递"},
            {"FEDEX_EXPRESS_SAVER", "省钱联邦快递"},
            {"FEDEX_GROUND", "FedEx Ground"},
            {"FEDEX_INTERNATIONAL_GROUND", "FedEx International Ground"},
            {"INTERNATIONAL_ECONOMY", "FedEx International Economy"},
            {"INTERNATIONAL_PRIORITY", "FedEx International Priority"},
            {"INTERNATIONAL_FIRST", "FedEx International First"}
        };
        private readonly bool _useProduction = false;

        /// <summary>
        ///    加载基本配置
        /// </summary>
        public FedExProvider()
        {
            Name = "FedEx";
            var appSettings = ConfigurationManager.AppSettings;
            _key = appSettings["FedExKey"];
            _password = appSettings["FedExPassword"];
            _accountNumber = appSettings["FedExAccountNumber"];
            _meterNumber = appSettings["FedExMeterNumber"];
            _useProduction = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="password"></param>
        /// <param name="accountNumber"></param>
        /// <param name="meterNumber"></param>
        public FedExProvider(string key, string password, string accountNumber, string meterNumber)
        {
            Name = "FedEx";

            _key = key;
            _password = password;
            _accountNumber = accountNumber;
            _meterNumber = meterNumber;
            _useProduction = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="password"></param>
        /// <param name="accountNumber"></param>
        /// <param name="meterNumber"></param>
        /// <param name="useProduction"></param>
        public FedExProvider(string key, string password, string accountNumber, string meterNumber, bool useProduction)
        {
            Name = "FedEx";

            _key = key;
            _password = password;
            _accountNumber = accountNumber;
            _meterNumber = meterNumber;
            _useProduction = useProduction;
        }

        private RateRequest CreateRateRequest()
        {
            // Build the RateRequest
            var request = new RateRequest();

            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = _key;
            request.WebAuthenticationDetail.UserCredential.Password = _password;

            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = _accountNumber;
            request.ClientDetail.MeterNumber = _meterNumber;

            request.TransactionDetail = new TransactionDetail();
            request.TransactionDetail.CustomerTransactionId = "***Rate Available Services Request using VC#***";

            request.Version = new VersionId();

            request.ReturnTransitAndCommit = true;
            request.ReturnTransitAndCommitSpecified = true;

            SetShipmentDetails(request);

            return request;
        }

        public override void GetRates()
        {
            var request = CreateRateRequest();
            var service = new RateService(_useProduction);
            try
            {
                // Call the web service passing in a RateRequest and returning a RateReply
                var reply = service.getRates(request);
                //
                if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING)
                {
                    ProcessReply(reply);
                }
                //ShowNotifications(reply);
            }
            catch (SoapException e)
            {
                Debug.WriteLine(e.Detail.InnerText);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void ProcessReply(RateReply reply)
        {
            foreach (var rateReplyDetail in reply.RateReplyDetails)
            {
                var netCharge = rateReplyDetail.RatedShipmentDetails.Max(x => x.ShipmentRateDetail.TotalNetCharge.Amount);

                var key = rateReplyDetail.ServiceType.ToString();
                var deliveryDate = rateReplyDetail.DeliveryTimestampSpecified ? rateReplyDetail.DeliveryTimestamp : DateTime.Now.AddDays(30);
                AddRate(key, _serviceCodes[key], netCharge, deliveryDate);
            }
        }

        private void SetDestination(RateRequest request)
        {
            request.RequestedShipment.Recipient = new Party();
            request.RequestedShipment.Recipient.Address = new RateServiceWebReference.Address();
            request.RequestedShipment.Recipient.Address.StreetLines = new string[3] { Shipment.DestinationAddress.Line1,Shipment.DestinationAddress.Line2,Shipment.DestinationAddress.Line3 };
            request.RequestedShipment.Recipient.Address.City = Shipment.DestinationAddress.City;
            request.RequestedShipment.Recipient.Address.StateOrProvinceCode = Shipment.DestinationAddress.State;
            request.RequestedShipment.Recipient.Address.PostalCode = Shipment.DestinationAddress.PostalCode;
            request.RequestedShipment.Recipient.Address.CountryCode = Shipment.DestinationAddress.CountryCode;
            
        }

        private void SetOrigin(RateRequest request)
        {
            request.RequestedShipment.Shipper = new Party();
            request.RequestedShipment.Shipper.Address = new RateServiceWebReference.Address();
            request.RequestedShipment.Shipper.Address.StreetLines = new string[3] { Shipment.OriginAddress.Line1,Shipment.OriginAddress.Line2,Shipment.OriginAddress.Line3 };
            request.RequestedShipment.Shipper.Address.City = Shipment.OriginAddress.City;
            request.RequestedShipment.Shipper.Address.StateOrProvinceCode = Shipment.OriginAddress.State;
            request.RequestedShipment.Shipper.Address.PostalCode = Shipment.OriginAddress.PostalCode;
            request.RequestedShipment.Shipper.Address.CountryCode = Shipment.OriginAddress.CountryCode;

        }

        private void SetPackageLineItems(RateRequest request)
        {
            request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[Shipment.PackageCount];

            var i = 0;
            foreach (var package in Shipment.Packages)
            {
                request.RequestedShipment.RequestedPackageLineItems[i] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[i].SequenceNumber = (i + 1).ToString();
                //request.RequestedShipment.RequestedPackageLineItems[i].GroupPackageCount = "1";

                request.RequestedShipment.RequestedPackageLineItems[i].GroupPackageCount = package.PackageCount.ToString();
                // package weight
                request.RequestedShipment.RequestedPackageLineItems[i].Weight = new Weight();
                switch (package.ShipWeightUnit)
                {
                    case WeightUnit.KG:
                        request.RequestedShipment.RequestedPackageLineItems[i].Weight.Units = WeightUnits.KG;
                        break;
                    case WeightUnit.LB:
                        request.RequestedShipment.RequestedPackageLineItems[i].Weight.Units = WeightUnits.LB;
                        break;
                    
                }
                //request.RequestedShipment.RequestedPackageLineItems[i].Weight.Units = WeightUnits.LB;
                request.RequestedShipment.RequestedPackageLineItems[i].Weight.UnitsSpecified = true;
                request.RequestedShipment.RequestedPackageLineItems[i].Weight.Value = package.RoundedWeight;
                request.RequestedShipment.RequestedPackageLineItems[i].Weight.ValueSpecified = true;
                // package dimensions
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions = new Dimensions();
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Length = package.RoundedLength.ToString();
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Width = package.RoundedWidth.ToString();
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Height = package.RoundedHeight.ToString();
                 switch (package.ShipLinearUnit)
                 {
                     case LinearUnit.CM:
                         request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Units = LinearUnits.CM;
                         break;
                     case LinearUnit.IN:
                         request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Units = LinearUnits.IN;
                         break;

                 }

              //  request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Units = LinearUnits.IN;
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.UnitsSpecified = true; 
                // package insured value
                request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue = new Money();
                request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Amount = package.InsuredValue;
                request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.AmountSpecified = true;
                request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Currency = package.Currency;
                i++;
            }
        }

        private void SetShipmentDetails(RateRequest request)
        {
            request.RequestedShipment = new RequestedShipment();
            request.RequestedShipment.ShipTimestamp = DateTime.Now; // Shipping date and time
            request.RequestedShipment.ShipTimestampSpecified = true;
            request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP; //Drop off types are BUSINESS_SERVICE_CENTER, DROP_BOX, REGULAR_PICKUP, REQUEST_COURIER, STATION
            request.RequestedShipment.DropoffTypeSpecified = true;
            request.RequestedShipment.PackagingType = PackagingType.YOUR_PACKAGING;
            request.RequestedShipment.PackagingTypeSpecified = true;

            SetOrigin(request);

            SetDestination(request);

            SetPackageLineItems(request);

            request.RequestedShipment.RateRequestTypes = new RateRequestType[1];
            request.RequestedShipment.RateRequestTypes[0] = RateRequestType.LIST;
            request.RequestedShipment.PackageCount = Shipment.PackageCount.ToString();
            
        }

        private static void ShowNotifications(RateReply reply)
        {
            Debug.WriteLine("Notifications");
            for (var i = 0; i < reply.Notifications.Length; i++)
            {
                var notification = reply.Notifications[i];
                Debug.WriteLine("Notification no. {0}", i);
                Debug.WriteLine(" Severity: {0}", notification.Severity);
                Debug.WriteLine(" Code: {0}", notification.Code);
                Debug.WriteLine(" Message: {0}", notification.Message);
                Debug.WriteLine(" Source: {0}", notification.Source);
            }
        }
    }
}
