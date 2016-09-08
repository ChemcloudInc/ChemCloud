using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChemColud.Shipping
{
    /// <summary>
    /// 顺丰费率服务
    /// </summary>
    public class SFRateService
    {
        private SFRateRequest rateRequest;
        private string URL = "http://www.sf-express.com/sf-service-web/service/rate?";
        public SFRateService(SFRateRequest _rateRequest)
        {
            rateRequest = _rateRequest;
        }

        public List<SFRateRep> GetRates()
        {
            List<SFRateRep> result = new List<SFRateRep>();
            StringBuilder sb = new StringBuilder();

            sb.Append(URL);
            sb.AppendFormat("origin={0}&dest={1}&parentOrigin={2}&parentDest={3}&weight={4}&time={5}", rateRequest.OriginCode, rateRequest.DestCode, rateRequest.ParentOriginCode, rateRequest.ParentDestCode, rateRequest.Weight, rateRequest.Time);
            sb.AppendFormat("&volume={0}&queryType={1}&lang={2}&region={3}", rateRequest.Volume, rateRequest.QueryType, rateRequest.Lang, rateRequest.Region);

            
            using (var webClient = new System.Net.WebClient())
            {
                //定义对象的编码语言
                webClient.Encoding = System.Text.Encoding.UTF8;
                var json = webClient.DownloadString(sb.ToString());

                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SFRateRep>>(json);
            }

            return result;
        }
    }
}
