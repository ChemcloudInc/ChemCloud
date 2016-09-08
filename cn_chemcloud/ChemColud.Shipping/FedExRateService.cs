using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ChemColud.Shipping.RateServiceWebReference
{
     public partial class RateService
    {
        /// <summary>
        /// </summary>
        /// <param name="production"></param>
        public RateService(bool production)
        {
            if (production)
            {
                Url = "https://ws.fedex.com:443/web-services/rate";
            }
            else
            {
                Url = "https://wsbeta.fedex.com/web-services/rate";
            }

            if (IsLocalFileSystemWebService(Url))
            {
                UseDefaultCredentials = true;
                useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                useDefaultCredentialsSetExplicitly = true;
            }
        }
    }
}
