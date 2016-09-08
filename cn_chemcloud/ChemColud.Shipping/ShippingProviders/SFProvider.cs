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
    public class SFProvider : AbstractShippingProvider
    {

        private SFRateRequest rateRequest;
        /// <summary>
        ///    加载基本配置
        /// </summary>
        public SFProvider(SFRateRequest _rateRequest)
        {
            Name = "SF";
            rateRequest = _rateRequest;
        }

        public override void GetRates()
        {
            try
            {
                SFRateService service = new SFRateService(rateRequest);

                List<SFRateRep> rateList = service.GetRates();
                DateTime? dt=null;

                string name = string.Empty;
                foreach (var item in rateList)
                {
                    if (!String.IsNullOrEmpty(item.deliverTime))
                    {
                        dt = Convert.ToDateTime(item.deliverTime);
                    }

                    name = string.Format("{0}({1})", item.limitTypeName, item.cargoTypeName);
                    AddRate(item.cargoTypeCode, name, item.freight, dt);
                }

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




    }
}
