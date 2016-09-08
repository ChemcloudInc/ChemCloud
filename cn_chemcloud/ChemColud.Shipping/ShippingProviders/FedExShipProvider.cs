using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

using System.Diagnostics;
using ChemColud.Shipping.ShipServiceWebReference;



namespace ChemColud.Shipping.ShippingProviders
{ /// <summary>
    ///   在线生成运单 主单
    /// </summary>
    public class FedExShipProvider : AbstractCreateShipProvider
    {
        private readonly string _accountNumber;
        private readonly string _key;
        private readonly string _meterNumber;
        private readonly string _password;


        
        private readonly bool _useProduction = false;

        /// <summary>
        ///    加载基本配置
        /// </summary>
        public FedExShipProvider()
        {
            Name = "FedExShip";
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
        public FedExShipProvider(string key, string password, string accountNumber, string meterNumber)
        {
            Name = "FedExShip";

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
        public FedExShipProvider(string key, string password, string accountNumber, string meterNumber, bool useProduction)
        {
            Name = "FedExShip";

            _key = key;
            _password = password;
            _accountNumber = accountNumber;
            _meterNumber = meterNumber;
            _useProduction = useProduction;
        }

        private ProcessShipmentRequest CreateShipmentRequest()
        {
            //
            ProcessShipmentRequest request = new ProcessShipmentRequest();
            //
            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = _key;
            request.WebAuthenticationDetail.UserCredential.Password = _password;

            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = _accountNumber;
            request.ClientDetail.MeterNumber = _meterNumber;

            request.ClientDetail.Localization = new Localization();
            request.ClientDetail.Localization.LanguageCode = "zh";
            request.ClientDetail.Localization.LocaleCode = "CN";
            //
            request.TransactionDetail = new TransactionDetail();
            request.TransactionDetail.CustomerTransactionId = "***ChemCloud CreateShip***";

            request.TransactionDetail.Localization = new Localization();
            request.TransactionDetail.Localization.LanguageCode = "zh";
            request.TransactionDetail.Localization.LocaleCode = "CN";

            //
            request.Version = new VersionId() { ServiceId = "ship", Major = 17, Intermediate = 0, Minor = 0 };
            //物流信息
            SetShipmentDetails(request);
            //发送人
            SetSender(request);
            //接收人
            SetRecipient(request);
            //费用
            SetPayment(request);
            //返回数据格式设置
            SetLabelDetails(request);
            //包裹信息
            SetPackageLineItems(request);
            //清关信息
            SetCustomsClearanceDetails(request);
            //
           // SetDocumentSpecification(request);

            return request;
        }

        /// <summary>
        /// 发票设定
        /// </summary>
        /// <param name="request"></param>

        private  void SetDocumentSpecification(ProcessShipmentRequest request)
        {
            request.RequestedShipment.ShippingDocumentSpecification = new ShippingDocumentSpecification();
            request.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes = new RequestedShippingDocumentType[1];
            request.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes[0] = RequestedShippingDocumentType.COMMERCIAL_INVOICE;
            request.RequestedShipment.ShippingDocumentSpecification.CommercialInvoiceDetail = new CommercialInvoiceDetail();
            request.RequestedShipment.ShippingDocumentSpecification.CommercialInvoiceDetail.Format = new ShippingDocumentFormat();
            request.RequestedShipment.ShippingDocumentSpecification.CommercialInvoiceDetail.Format.ImageType = ShippingDocumentImageType.PDF;
            request.RequestedShipment.ShippingDocumentSpecification.CommercialInvoiceDetail.Format.ImageTypeSpecified = true;
            request.RequestedShipment.ShippingDocumentSpecification.CommercialInvoiceDetail.Format.StockTypeSpecified = true;
            request.RequestedShipment.ShippingDocumentSpecification.CommercialInvoiceDetail.Format.StockType = ShippingDocumentStockType.PAPER_LETTER;
        }
        private void SetShipmentDetails(ProcessShipmentRequest request)
        {
            request.RequestedShipment = new RequestedShipment();
            request.RequestedShipment.ShipTimestamp = DateTime.Now; // Ship date and time
            //
            request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP;
            request.RequestedShipment.ServiceType = ServiceType.INTERNATIONAL_PRIORITY;
            request.RequestedShipment.PackagingType = PackagingType.YOUR_PACKAGING; // Packaging type YOUR_PACKAGING, ...
            //
            request.RequestedShipment.PackageCount = Shipment.PackageCount.ToString();
            request.RequestedShipment.TotalWeight = new Weight();
            request.RequestedShipment.TotalWeight.Units = WeightUnits.KG;
            request.RequestedShipment.TotalWeight.Value = Shipment.TotalPackageWeight;

            //request.RequestedShipment.RateRequestTypes = new RateRequestType[1];
            //request.RequestedShipment.RateRequestTypes[0] = RateRequestType.NONE;

            //request.RequestedShipment.MasterTrackingId = new TrackingId() { TrackingIdType = TrackingIdType.EXPRESS };
            //request.RequestedShipment.MasterTrackingId.FormId = "0430";
            //request.RequestedShipment.MasterTrackingId.TrackingNumber = "794614418720";
        }

        private void SetSender(ProcessShipmentRequest request)
        {
            request.RequestedShipment.Shipper = new Party();
            request.RequestedShipment.Shipper.Contact = new Contact();
            request.RequestedShipment.Shipper.Contact.PersonName = Shipment.OriginAddress.PersonName;
            request.RequestedShipment.Shipper.Contact.CompanyName = Shipment.OriginAddress.CompanyName;
            request.RequestedShipment.Shipper.Contact.PhoneNumber = Shipment.OriginAddress.PhoneNumber;
            request.RequestedShipment.Shipper.Address = new ShipServiceWebReference.Address();
            request.RequestedShipment.Shipper.Address.StreetLines = new string[1] { Shipment.OriginAddress.Line1 };
            request.RequestedShipment.Shipper.Address.City = Shipment.OriginAddress.City;
            request.RequestedShipment.Shipper.Address.StateOrProvinceCode = "";
            request.RequestedShipment.Shipper.Address.PostalCode = Shipment.OriginAddress.PostalCode;
            request.RequestedShipment.Shipper.Address.CountryCode = Shipment.OriginAddress.CountryCode;
        }

        private void SetRecipient(ProcessShipmentRequest request)
        {
            request.RequestedShipment.Recipient = new Party();
            // TINS is optional(Tax Payer Identification)
            //request.RequestedShipment.Recipient.Tins = new TaxpayerIdentification[1];
            //request.RequestedShipment.Recipient.Tins[0] = new TaxpayerIdentification();
            //request.RequestedShipment.Recipient.Tins[0].TinType = TinType.BUSINESS_NATIONAL;
            //request.RequestedShipment.Recipient.Tins[0].Number = "XXX"; // Replace "XXX" with the TIN number
            //
            request.RequestedShipment.Recipient.Contact = new Contact();
            request.RequestedShipment.Recipient.Contact.PersonName = Shipment.DestinationAddress.PersonName;
            request.RequestedShipment.Recipient.Contact.CompanyName = Shipment.DestinationAddress.CompanyName;
            request.RequestedShipment.Recipient.Contact.PhoneNumber = Shipment.DestinationAddress.PhoneNumber;
            //
            request.RequestedShipment.Recipient.Address = new ShipServiceWebReference.Address();
            request.RequestedShipment.Recipient.Address.StreetLines = new string[1] { Shipment.DestinationAddress.Line1 };
            request.RequestedShipment.Recipient.Address.City = Shipment.DestinationAddress.City;
            request.RequestedShipment.Recipient.Address.StateOrProvinceCode = Shipment.DestinationAddress.StateOrProvinceCode;
            request.RequestedShipment.Recipient.Address.PostalCode = Shipment.DestinationAddress.PostalCode;
            request.RequestedShipment.Recipient.Address.CountryCode = Shipment.DestinationAddress.CountryCode;
            request.RequestedShipment.Recipient.Address.Residential = false;
        }

        private void SetPayment(ProcessShipmentRequest request)
        {
            request.RequestedShipment.ShippingChargesPayment = new Payment();
            request.RequestedShipment.ShippingChargesPayment.PaymentType = PaymentType.SENDER;
            request.RequestedShipment.ShippingChargesPayment.Payor = new Payor();
            request.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty = new Party();
            request.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber = _accountNumber; // Replace "XXX" with client's account number

            request.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Contact = new Contact() { PersonName = Shipment.OriginAddress.PersonName, CompanyName = Shipment.OriginAddress.CompanyName, PhoneNumber = Shipment.OriginAddress.PhoneNumber };
            request.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Address = new ShipServiceWebReference.Address() { StreetLines = new string[1] { Shipment.OriginAddress.Line1 }, City = Shipment.OriginAddress.City, PostalCode = Shipment.OriginAddress.PostalCode };
            request.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode = Shipment.OriginAddress.CountryCode;
        }

        private void SetLabelDetails(ProcessShipmentRequest request)
        {
            request.RequestedShipment.LabelSpecification = new LabelSpecification();
            request.RequestedShipment.LabelSpecification.ImageType = ShippingDocumentImageType.PDF; // Image types PDF, PNG, DPL, ...
            request.RequestedShipment.LabelSpecification.LabelFormatType = LabelFormatType.COMMON2D;
            request.RequestedShipment.LabelSpecification.LabelPrintingOrientationSpecified = true;
            request.RequestedShipment.LabelSpecification.LabelStockTypeSpecified = true;
            request.RequestedShipment.LabelSpecification.LabelStockType = LabelStockType.PAPER_85X11_TOP_HALF_LABEL;
            request.RequestedShipment.LabelSpecification.LabelPrintingOrientation = LabelPrintingOrientationType.TOP_EDGE_OF_TEXT_FIRST;
            request.RequestedShipment.LabelSpecification.ImageTypeSpecified = true;
        }

        private void SetPackageLineItems(ProcessShipmentRequest request)
        {
            request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];


            if (Shipment.Packages.Count > 0)
            {
                Package pkg = Shipment.Packages[0];
                request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber = "1";

                // Package weight information
                request.RequestedShipment.RequestedPackageLineItems[0].Weight = new Weight();
                request.RequestedShipment.RequestedPackageLineItems[0].Weight.Value = pkg.Weight;
                request.RequestedShipment.RequestedPackageLineItems[0].Weight.Units = pkg.ShipWeightUnit == WeightUnit.KG ? WeightUnits.KG : WeightUnits.LB;
                // package dimensions
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions = new Dimensions();
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Length = pkg.Length.ToString();
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Width = pkg.Width.ToString();
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Height = pkg.Height.ToString();
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Units = pkg.ShipLinearUnit == LinearUnit.CM ? LinearUnits.CM : LinearUnits.IN;
                // insured value
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue = new Money();
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Amount = 0;
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency = pkg.Currency;
                // Reference details
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = new CustomerReference[1] { new CustomerReference() };
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[0].CustomerReferenceType = CustomerReferenceType.CUSTOMER_REFERENCE;
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[0].Value = "CustomerReference";
            }
        }

        private void SetCustomsClearanceDetails(ProcessShipmentRequest request)
        {
            request.RequestedShipment.CustomsClearanceDetail = new CustomsClearanceDetail();
            request.RequestedShipment.CustomsClearanceDetail.DutiesPayment = new Payment();
            request.RequestedShipment.CustomsClearanceDetail.DutiesPayment.PaymentType = PaymentType.SENDER;
            request.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor = new Payor();
            request.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor.ResponsibleParty = new Party();
            request.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor.ResponsibleParty.AccountNumber = _accountNumber;

            request.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor.ResponsibleParty.Contact = new Contact() { PersonName = Shipment.OriginAddress.PersonName, CompanyName = Shipment.OriginAddress.CompanyName, PhoneNumber = Shipment.OriginAddress.PhoneNumber };
            request.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor.ResponsibleParty.Address = new ShipServiceWebReference.Address() { StreetLines = new string[1] { Shipment.OriginAddress.Line1 }, City = Shipment.OriginAddress.City, PostalCode = Shipment.OriginAddress.PostalCode };
            request.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor.ResponsibleParty.Address.CountryCode = Shipment.OriginAddress.CountryCode;
            request.RequestedShipment.CustomsClearanceDetail.DocumentContent = InternationalDocumentContentType.NON_DOCUMENTS;
            //
            request.RequestedShipment.CustomsClearanceDetail.CustomsValue = new Money();
            request.RequestedShipment.CustomsClearanceDetail.CustomsValue.Amount = 0M;
            request.RequestedShipment.CustomsClearanceDetail.CustomsValue.Currency =Shipment.Currency;
            //
            SetCommodityDetails(request);
        }

        private void SetCommodityDetails(ProcessShipmentRequest request)
        {
            int count = Shipment.Packages.Count;

            List<Commodity> comList = new List<Commodity>();
            Commodity commodity;
            foreach (var package in Shipment.Packages)
            {
                commodity = new Commodity();
                commodity.NumberOfPieces = "1";
                commodity.Description = package.Description;
                commodity.CountryOfManufacture = Shipment.OriginAddress.CountryCode;
                //
                commodity.Weight = new Weight();
                commodity.Weight.Value = package.Weight;
                commodity.Weight.Units = package.ShipWeightUnit == WeightUnit.KG ? WeightUnits.KG : WeightUnits.LB;
                //
                commodity.Quantity = 1.0M;
                commodity.QuantitySpecified = true;
                commodity.QuantityUnits = "EA";
                //
                commodity.UnitPrice = new Money();
                commodity.UnitPrice.Amount = 0;
                commodity.UnitPrice.Currency = package.Currency;
                //
                commodity.CustomsValue = new Money();
                commodity.CustomsValue.Amount = 0;
                commodity.CustomsValue.Currency = package.Currency;
                comList.Add(commodity);
            }

            request.RequestedShipment.CustomsClearanceDetail.Commodities = comList.ToArray();

            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].NumberOfPieces = "1";
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].Description = "test1";
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].CountryOfManufacture = "US";
            ////
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].Weight = new Weight();
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].Weight.Value = 1.0M;
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].Weight.Units = WeightUnits.KG;
            ////
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].Quantity = 1.0M;
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].QuantitySpecified = true;
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].QuantityUnits = "EA";
            ////
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].UnitPrice = new Money();
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].UnitPrice.Amount = 40.000000M;
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].UnitPrice.Currency = "USD";
            ////
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].CustomsValue = new Money();
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].CustomsValue.Amount = 10.000000M;
            //request.RequestedShipment.CustomsClearanceDetail.Commodities[0].CustomsValue.Currency = "USD";



        }

        private ShipReply SetShipmentReply(ProcessShipmentReply reply)
        {

            ShipReply result = new ShipReply();
             //Details for each package
            foreach (CompletedPackageDetail packageDetail in reply.CompletedShipmentDetail.CompletedPackageDetails)
            {
                if (null != packageDetail.Label.Parts[0].Image)
                {
                    result.TrackNumber = packageDetail.TrackingIds[0].TrackingNumber;
                    result.TrackFormId = packageDetail.TrackingIds[0].FormId;
                    result.LabelImage=packageDetail.Label.Parts[0].Image;
                }

            }
            return result;
        }

        public override void CreateShipment()
        {
            ShipReply result = null;
            ProcessShipmentRequest request = CreateShipmentRequest();

            try
            {
                ShipService service = new ShipService();
                service.Url = Url;
                ProcessShipmentReply reply = service.processShipment(request);
                if ((reply.HighestSeverity != NotificationSeverityType.ERROR) && (reply.HighestSeverity != NotificationSeverityType.FAILURE))
                {
                    result=SetShipmentReply(reply);
                }
                else
                {
                    if (reply.Notifications.Length>0)
	                {
                        ErrorMessage = string.Format("{0} {1}",reply.Notifications[0].Code,reply.Notifications[0].Message);
	                 }
                    
                }

                ShipReplyEx = result;
            }
            catch (SoapException e)
            {
                Debug.WriteLine(e.Detail.InnerText);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
           // return result;
        }


    }
}
