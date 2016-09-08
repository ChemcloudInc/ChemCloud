using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.SellerAdmin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class ShopController : BaseSellerController
    {
        public ShopController()
        {
        }
        [HttpPost]
        public JsonResult SendMail(string username)
        {
            string validCode = StringHelper.GenerateCheckCode();

            SaveCookie("validCode", validCode, 0);

            bool falg = ChemCloud.Service.SendMail.SendEmail(username,
                "Please validate Your Email", "hello,<span style=\"color:green;\">" + username
                + "</span><br/>Welcome to ChemCloud!<br/> Your ChemCloud ValidCode is:<span style=\"color:red;\">"
                + validCode + "</span>");

            Result result = new Result();
            result.msg = falg.ToString();
            result.success = falg;
            return Json(result);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="cookieTime">Cookie过期时间(分钟),0为关闭页面失效</param>
        /// <param name="isencryption">Cookie是否加密</param>
        /// <param name="cookiepath">Cookie路径</param>
        public static void SaveCookie(string cookieName, string cookieValue, double cookieTime, bool isencryption = false, string cookiepath = "/")
        {

            var myCookie = new HttpCookie(cookieName);
            DateTime now = DateTime.Now;
            myCookie.Value = cookieValue;
            myCookie.Path = cookiepath;
            if (Math.Abs(cookieTime) > 0)
            {
                myCookie.Expires = now.AddHours(cookieTime);
                if (System.Web.HttpContext.Current.Response.Cookies[cookieName] != null)
                {
                    System.Web.HttpContext.Current.Response.Cookies.Remove(cookieName);
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            else
            {
                if (System.Web.HttpContext.Current.Response.Cookies[cookieName] != null)
                {
                    System.Web.HttpContext.Current.Response.Cookies.Remove(cookieName);
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
        [HttpPost]
        public JsonResult EditProfile1(ShopProfileStep1 shopProfileStep1)
        {
            string item = base.Request.Params["CompanyName"] ?? "";
            string str = base.Request.Params["CompanyAddress"] ?? "";
            string item1 = base.Request.Params["CompanyRegionId"] ?? "";
            item1 = (string.IsNullOrWhiteSpace(item1) ? base.Request.Params["NewCompanyRegionId"] : item1);
            int num = 0;
            if (!int.TryParse(item1, out num))
            {
                num = 0;
            }
            string str1 = base.Request.Params["CompanyRegionAddress"] ?? "";
            string item2 = base.Request.Params["CompanyPhone"] ?? "";
            string str2 = base.Request.Params["CompanyEmployeeCount"] ?? "";
            string item3 = base.Request.Params["CompanyRegisteredCapital"] ?? "";
            string str3 = base.Request.Params["ContactsName"] ?? "";
            string item4 = base.Request.Params["ContactsPhone"] ?? "";
            string str4 = base.Request.Params["ContactsEmail"] ?? "";
            string item5 = base.Request.Params["BusinessLicenseCert"] ?? "";
            string str5 = base.Request.Params["ProductCert"] ?? "";
            string item6 = base.Request.Params["OtherCert"] ?? "";

            string ECompanyName = base.Request.Params["ECompanyName"] ?? "";
            string ECompanyAddress = base.Request.Params["ECompanyAddress"] ?? "";
            string EContactsName = base.Request.Params["EContactsName"] ?? "";
            string ZipCode = base.Request.Params["ZipCode"] ?? "";
            string Fax = base.Request.Params["Fax"] ?? "";
            string URL = shopProfileStep1.URL ?? "";
            string ChemNumber = base.Request.Params["ChemNumber"] ?? "";
            string ChemicalsBusinessLicense = base.Request.Params["ChemicalsBusinessLicense"] ?? "";
            string GMPPhoto = base.Request.Params["GMPPhoto"] ?? "";
            string FDAPhoto = base.Request.Params["FDAPhoto"] ?? "";
            string ISOPhoto = base.Request.Params["ISOPhoto"] ?? "";
            string BusinessLicenceNumber = base.Request.Params["BusinessLicenceNumber"] ?? "";
            string BusinessSphere = base.Request.Params["BusinessSphere"] ?? "";
            string legalPerson = base.Request.Params["legalPerson"] ?? "";
            string OrganizationCode = base.Request.Params["OrganizationCode"] ?? "";
            string BankAccountName = base.Request.Params["BankAccountName"] ?? "";
            string BankAccountNumber = base.Request.Params["BankAccountNumber"] ?? "";
            string BankCode = base.Request.Params["BankCode"] ?? "";
            string BankName = base.Request.Params["BankName"] ?? "";
            string BankPhoto = base.Request.Params["BankPhoto"] ?? "";
            string item7 = base.Request.Params["CompanyRegionId"] ?? "";
            string logo = base.Request.Params["Logo"] ?? "";
            item7 = (string.IsNullOrWhiteSpace(item2) ? base.Request.Params["NewCompanyRegionId"] : item7);
            int num1 = 0;
            if (!int.TryParse(item7, out num))
            {
                num1 = 0;
            }
            string BeneficiaryBankName = base.Request.Params["BeneficiaryBankName"];
            string SWiftBic = base.Request.Params["SWiftBic"];
            string BeneficiaryName = base.Request.Params["BeneficiaryName"];
            string BeneficiaryAccountNum = base.Request.Params["BeneficiaryAccountNum"];
            string CompanysAddress = base.Request.Params["CompanysAddress"];
            string BeneficiaryBankBranchAddress = base.Request.Params["BeneficiaryBankBranchAddress"];
            string AbaRoutingNumber = base.Request.Params["AbaRoutingNumber"];

            ShopInfo shopInfo = new ShopInfo()
            {
                Id = base.CurrentSellerManager.ShopId,
                CompanyName = item,
                CompanyAddress = str,
                CompanyRegionId = num,
                CompanyRegionAddress = str1,
                CompanyPhone = item2,
                CompanyEmployeeCount = (CompanyEmployeeCount)int.Parse(str2),
                //CompanyRegisteredCapital = decimal.Parse(item3),
                ContactsName = str3,
                ContactsPhone = item4,
                ContactsEmail = str4,
                BusinessLicenseCert = item5,
                ProductCert = str5,
                ECompanyName = ECompanyName,
                ECompanyAddress = ECompanyAddress,
                EContactsName = EContactsName,
                ZipCode = ZipCode,
                Fax = Fax,
                URL = URL,
                ChemNumber = ChemNumber,
                ChemicalsBusinessLicense = ChemicalsBusinessLicense,
                GMPPhoto = GMPPhoto,
                FDAPhoto = FDAPhoto,
                ISOPhoto = ISOPhoto,
                BusinessLicenceNumber = BusinessLicenceNumber,
                BusinessSphere = BusinessSphere,
                legalPerson = legalPerson,
                OrganizationCode = OrganizationCode,
                OtherCert = item6,
                BankAccountName = BankAccountName,
                BankAccountNumber = BankAccountNumber,
                BankCode = BankCode,
                BankName = BankName,
                BankPhoto = BankPhoto,
                Logo = logo,
                BankRegionId = num1,
                BeneficiaryBankName = BeneficiaryBankName,
                SWiftBic = SWiftBic,
                BeneficiaryName = BeneficiaryName,
                BeneficiaryAccountNum = BeneficiaryAccountNum,
                CompanysAddress = CompanysAddress,
                BeneficiaryBankBranchAddress = BeneficiaryBankBranchAddress,
                AbaRoutingNumber = AbaRoutingNumber
            };
            ServiceHelper.Create<IShopService>().UpdateShop(shopInfo);
            return Json(new { success = true });
        }

        public ActionResult FreightSetting()
        {
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
            ShopFreightModel shopFreightModel = new ShopFreightModel()
            {
                FreeFreight = shop.FreeFreight,
                Freight = shop.Freight
            };
            return View(shopFreightModel);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult SaveFreightSetting(ShopFreightModel shopFreight)
        {
            ServiceHelper.Create<IShopService>().UpdateShopFreight(base.CurrentSellerManager.ShopId, shopFreight.Freight, shopFreight.FreeFreight);
            return Json(new { success = true });
        }

        public ActionResult ShopDetail()
        {
            long shopId = base.CurrentSellerManager.ShopId;
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(shopId, true);
            ShopModel shopModel = new ShopModel(shop)
            {
                BusinessCategory = new List<CategoryKeyVal>()
            };
            foreach (long key in shop.BusinessCategory.Keys)
            {
                List<CategoryKeyVal> businessCategory = shopModel.BusinessCategory;
                CategoryKeyVal categoryKeyVal = new CategoryKeyVal()
                {
                    CommisRate = shop.BusinessCategory[key],
                    Name = ServiceHelper.Create<ICategoryService>().GetCategory(key).Name
                };
                businessCategory.Add(categoryKeyVal);
            }
            ViewBag.CompanyRegionIds = ServiceHelper.Create<IRegionService>().GetRegionIdPath(shop.CompanyRegionId);
            ViewBag.BankRegionIds = ServiceHelper.Create<IRegionService>().GetRegionIdPath(shop.BankRegionId);
            ViewBag.BusinessLicenseCert = shop.BusinessLicenseCert;
            ViewBag.Logo = shop.Logo;
            ViewBag.GMPPhoto = shop.GMPPhoto;
            ViewBag.ISOPhoto = shop.ISOPhoto;
            ViewBag.FDAPhoto = shop.FDAPhoto;
            string[] strArrays = new string[3];
            string[] strArrays1 = new string[3];
            string[] strArrays2 = new string[3];
            if (!string.IsNullOrWhiteSpace(shop.BusinessLicenseCert))
            {
                strArrays = shop.BusinessLicenseCert.Split(new char[] { ',' });
            }
            
            if (!string.IsNullOrWhiteSpace(shop.ProductCert))
            {
                strArrays1 = shop.ProductCert.Split(new char[] { ',' });
            }
            
            //if (!string.IsNullOrWhiteSpace(shop.OtherCert))
            //{
            //    strArrays2 = shop.OtherCert.Split(new char[] { ',' });
            //}
            
            ViewBag.BusinessLicenseCerts = strArrays;
            ViewBag.ProductCerts = strArrays1;
            ViewBag.OtherCerts = shop.OtherCert;
            return View(shopModel);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangePassword(string oldpassword, string password)
        {
            if (string.IsNullOrWhiteSpace(oldpassword) || string.IsNullOrWhiteSpace(password))
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "密码不能为空！"
                };
                return Json(result);
            }
            UserMemberInfo currentUser = base.CurrentUser;
            if (SecureHelper.MD5(string.Concat(SecureHelper.MD5(oldpassword), currentUser.PasswordSalt)) != currentUser.Password)
            {
                Result result1 = new Result()
                {
                    success = false,
                    msg = "旧密码错误"
                };
                return Json(result1);
            }
            ServiceHelper.Create<IMemberService>().ChangePassWord(currentUser.Id, password);
            Result result2 = new Result()
            {
                success = true,
                msg = "修改成功"
            };
            return Json(result2);
        }
        [HttpPost]
        public JsonResult CheckOldPassWord(string password)
        {
            UserMemberInfo currentUser = base.CurrentUser;
            string str = SecureHelper.MD5(string.Concat(SecureHelper.MD5(password), currentUser.PasswordSalt));
            if (currentUser.Password == str)
            {
                return Json(new Result()
                {
                    success = true
                });
            }
            return Json(new Result()
            {
                success = false
            });
        }
    }
}