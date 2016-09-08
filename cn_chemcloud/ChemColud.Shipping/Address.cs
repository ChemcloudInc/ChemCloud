using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    public class Address
    {
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line1">详细地址1（门牌、街道的组合）</param>
        /// <param name="line2">详细地址2</param>
        /// <param name="line3">详细地址3</param>
        /// <param name="city">城市乡镇等的名称</param>
        /// <param name="state">美国的州、加拿大的省等行政区域缩写</param>
        /// <param name="postalCode">邮编</param>
        /// <param name="countryCode">用于标识某个国家/地区,两个字母组成</param>
        public Address(string line1, string line2, string line3, string city, string state, string postalCode, string countryCode)
        {
            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            City = city;
            State = state;
            PostalCode = postalCode;
            CountryCode = countryCode;
            IsResidential = false;
        }

        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string PostalCode { get; set; }

        public string PersonName { get; set; }

        public string StateOrProvinceCode { get; set; }

        public string CompanyName { get; set; }

        public string PhoneNumber { get; set; }

        public string State { get; set; }
        public bool IsResidential { get; set; }

        public string GetCountryName()
        {
            if (!string.IsNullOrEmpty(CountryName))
            {
                return CountryName;
            }

            if (string.IsNullOrEmpty(CountryCode))
            {
                return string.Empty;
            }


            return string.Empty;
        }

        public bool IsCanadaAddress()
        {
            return !string.IsNullOrEmpty(CountryCode) && string.Equals(CountryCode, "CA", StringComparison.OrdinalIgnoreCase);
        }


    }

}
