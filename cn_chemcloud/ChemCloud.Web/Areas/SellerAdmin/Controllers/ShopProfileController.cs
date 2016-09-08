using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.SellerAdmin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class ShopProfileController : BaseMemberController
    {
        public ShopProfileController()
        {
        }


        /// <summary>
        /// 在下面的方法中给根据登录用户给Manager中新加了一条数据（方法：AddSellerManager）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Agreement(FormCollection form)
        {
            if (!form["agree"].Equals("on"))
            {
                return RedirectToAction("EditProfile1");
            }
            
            DateTime now = DateTime.Now;
            //WebHelper.SetCookie("SellerManager", str, now.AddDays(7));
            ManagerInfo managerInfo = ServiceHelper.Create<IManagerService>().GetSellerManager(base.CurrentUser.UserName);
            string str = UserCookieEncryptHelper.Encrypt(managerInfo.Id, "SellerAdmin");
            WebHelper.SetCookie("ChemCloud-SellerManager", str, now.AddDays(7));
            return RedirectToAction("EditProfile1");
        }

        
        [HttpPost]
        public JsonResult SendMail(string username)
        {
            string validCode = StringHelper.GenerateCheckCode();
            SaveCookie("validCode", validCode,0);
            //bool falg = ChemCloud.Email.SendMail.SendEmail(username, "Please validate Your Email", "hello,<span style=\"color:green;\">" + username + "</span><br/>Welcome to ChemCloud!<br/> Your ChemCloud ValidCode is :<span style=\"color:red;\">" + validCode + "</span>");
            bool falg = ChemCloud.Service.SendMail.SendEmail(username, "Please validate Your Email", "hello,<span style=\"color:green;\">" + username + "</span><br/>Welcome to ChemCloud!<br/> Your ChemCloud ValidCode is :<span style=\"color:red;\">" + validCode + "</span>");

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

        [HttpGet]
        public JsonResult CheckCompanyName(string companyName)
        {
            bool flag = ServiceHelper.Create<IShopService>().ExistCompanyName(companyName.Trim(), base.CurrentSellerManager.ShopId);
            return Json(!flag, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckECompanyName(string ecompanyName)
        {
            bool flag = ServiceHelper.Create<IShopService>().ExistECompanyName(ecompanyName.Trim(), base.CurrentSellerManager.ShopId);
            return Json(!flag, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditProfile1()
        {
            ViewBag.Step = 1;
            ViewBag.MenuStep = 1;
            ViewBag.Frame = "Step1";
            ViewBag.Manager = base.CurrentUser;
            ViewBag.Email = base.CurrentUser.UserName;
            ViewBag.SellerAdminAgreement = GetSellerAgreement();
            ShopProfileStep1 Step = new ShopProfileStep1();
            return View("EditProfile", Step);
        }
        [HttpPost]
        public String SetEmail()
        {
            string s = "";
            UserMemberInfo memberInfo = ServiceHelper.Create<IMemberService>().GetMember(base.CurrentUser.Id);
            string email = memberInfo.Email;
            if (!string.IsNullOrEmpty(email))
            {
                s = email;
            }
            return s;
        }

        [HttpPost]
        public JsonResult EditProfile1(ShopProfileStep1 shopProfileStep1)
        {
            ShopInfo shopInfo = new ShopInfo();
            shopInfo.Id = base.CurrentSellerManager.ShopId;
            shopInfo.CompanyName = shopProfileStep1.CompanyName;
            shopInfo.ECompanyName = shopProfileStep1.ECompanyName;
            shopInfo.CompanyAddress = shopProfileStep1.Address;
            shopInfo.ECompanyAddress = shopProfileStep1.ECompanyAddress;
            shopInfo.CompanyRegionId = shopProfileStep1.CityRegionId;
            shopInfo.CompanyRegionAddress = shopProfileStep1.Address;
            shopInfo.CompanyPhone = shopProfileStep1.Phone;
            shopInfo.CompanyEmployeeCount = shopProfileStep1.EmployeeCount;
            //CompanyRegisteredCapital = shopProfileStep1.RegisterMoney,
            shopInfo.ContactsName = shopProfileStep1.ContactName;
            shopInfo.EContactsName = shopProfileStep1.EContactsName;
            shopInfo.ContactsPhone = shopProfileStep1.ContactPhone;
            shopInfo.ContactsEmail = shopProfileStep1.Email;
            shopInfo.BusinessLicenceNumber = shopProfileStep1.BusinessLicenceNumber;
            shopInfo.TaxpayerId = shopProfileStep1.TaxpayerId;
            shopInfo.TaxRegistrationCertificate = shopProfileStep1.TaxRegistrationCertificate;
            shopInfo.TaxRegistrationCertificatePhoto = shopProfileStep1.TaxRegistrationCertificatePhoto;

            //BusinessLicenceRegionId = shopProfileStep1.BusinessLicenceArea,
            //BusinessLicenceStart = new DateTime?(shopProfileStep1.BusinessLicenceValidStart),
            //BusinessLicenceEnd = new DateTime?(shopProfileStep1.BusinessLicenceValidEnd),
            //BusinessSphere = shopProfileStep1.BusinessSphere,
            shopInfo.BusinessLicenceNumberPhoto = shopProfileStep1.BusinessLicenceNumberPhoto;  
            shopInfo.OrganizationCode = shopProfileStep1.OrganizationCode;
            shopInfo.OrganizationCodePhoto = shopProfileStep1.OrganizationCodePhoto;
            shopInfo.GeneralTaxpayerPhot = shopProfileStep1.GeneralTaxpayerPhoto;
            shopInfo.CertificatePhoto = shopProfileStep1.CertificatePhoto;
            shopInfo.Stage = new ShopInfo.ShopStage?(ShopInfo.ShopStage.FinancialInfo);
            shopInfo.BusinessLicenseCert = base.Request.Form["BusinessLicenseCert"];
            shopInfo.ProductCert = base.Request.Form["ProductCert"];
            shopInfo.OtherCert = base.Request.Form["OtherCert"];
            //legalPerson = shopProfileStep1.legalPerson,

            shopInfo.CompanyType = shopProfileStep1.CompanyType ?? "生产企业";
            //ProductsNumberIng
   

            shopInfo.ZipCode = shopProfileStep1.ZipCode;
            shopInfo.Fax = shopProfileStep1.Fax;
            shopInfo.URL = shopProfileStep1.URL;
            shopInfo.Logo = base.Request.Form["Logo"];
            shopInfo.ChemicalsBusinessLicense = base.Request.Form["ChemicalsBusinessLicense"];
            shopInfo.ChemNumber = shopProfileStep1.ChemNumber;
            shopInfo.ShopName = shopProfileStep1.CompanyName;
            shopInfo.GMPPhoto = base.Request.Form["GMPPhoto"];
            shopInfo.FDAPhoto = base.Request.Form["FDAPhoto"];
            shopInfo.ISOPhoto = base.Request.Form["ISOPhoto"];
            //{
                
            //    //CompanyFoundingDate = new DateTime?(shopProfileStep1.CompanyFoundingDate)
            //};
            ServiceHelper.Create<IShopService>().UpdateShop(shopInfo);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult EditProfile2(ShopProfileStep2 shopProfileStep2)
        {
            ShopInfo shopInfo = new ShopInfo()
            {
                Id = base.CurrentSellerManager.ShopId,
                BankAccountName = shopProfileStep2.BankAccountName,
                BankAccountNumber = shopProfileStep2.BankAccountNumber,
                BankCode = shopProfileStep2.BankCode,
                BankName = shopProfileStep2.BankName,
                BankPhoto = shopProfileStep2.BankPhoto,
                BankRegionId = shopProfileStep2.BankRegionId,
                BeneficiaryBankName = shopProfileStep2.BeneficiaryBankName,
                SWiftBic = shopProfileStep2.SWiftBic,
                BeneficiaryName = shopProfileStep2.BeneficiaryName,
                BeneficiaryAccountNum = shopProfileStep2.BeneficiaryAccountNum,
                CompanysAddress = shopProfileStep2.CompanysAddress,
                BeneficiaryBankBranchAddress = shopProfileStep2.BeneficiaryBankBranchAddress,
                AbaRoutingNumber = shopProfileStep2.AbaRoutingNumber,
                ShopStatus = ShopInfo.ShopAuditStatus.WaitAudit,
                GradeId = 1,
                EndDate = DateTime.Now.AddDays(1000),
                Stage = ShopInfo.ShopStage.Finish
            };
            ServiceHelper.Create<IShopService>().UpdateShop(shopInfo);
            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult EditProfile2()
        {
            ViewBag.MenuStep = 2;
            ViewBag.Step = 1;
            ViewBag.Frame = "Step2";
            ViewBag.Manager = base.CurrentUser;
            ViewBag.SellerAdminAgreement = GetSellerAgreement();
            return View("EditProfile", new ShopProfileStep2());
        }

        [HttpGet]
        public ActionResult EditProfile3()
        {
            ViewBag.Step = 2;
            ViewBag.Frame = "Step3";
            ViewBag.Manager = base.CurrentUser;
            ViewBag.SellerAdminAgreement = GetSellerAgreement();
            RedirectToRouteResult redirectToRouteResult = RedirectToAction("index", "home", new { area = "SellerAdmin" });
            return redirectToRouteResult;
        }

        [HttpPost]
        public JsonResult EditProfile3(string shopProfileStep3)
        {
            ShopProfileStep3 shopProfileStep31 = JsonConvert.DeserializeObject<ShopProfileStep3>(shopProfileStep3);
            if (ServiceHelper.Create<IShopService>().ExistShop(shopProfileStep31.ShopName, base.CurrentSellerManager.ShopId))
            {
                string str = string.Format("{0} 供应商名称已经存在", shopProfileStep31.ShopName);
                return Json(new { success = false, msg = str });
            }

            ShopInfo shopInfo = new ShopInfo()
            {
                Id = base.CurrentSellerManager.ShopId,
                ShopName = shopProfileStep31.ShopName,
                GradeId = shopProfileStep31.ShopGrade,
                //Stage = new ShopInfo.ShopStage?(ShopInfo.ShopStage.UploadPayOrder)
                Stage = new ShopInfo.ShopStage?(ShopInfo.ShopStage.Finish),
                EndDate = DateTime.Now.AddDays(1000),//给结束时间赋值，定位是当前时间后的1000天
                ShopStatus = ShopInfo.ShopAuditStatus.WaitConfirm
            };
            ShopInfo shopInfo1 = shopInfo;
            IEnumerable<long> categories = shopProfileStep31.Categories;
            ServiceHelper.Create<IShopService>().UpdateShop(shopInfo1, categories);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult EditProfile4(string payOrderPhoto, string remark)
        {
            ShopInfo shopInfo = new ShopInfo()
            {
                Id = base.CurrentSellerManager.ShopId,
                PayPhoto = payOrderPhoto,
                PayRemark = remark,
                Stage = new ShopInfo.ShopStage?(ShopInfo.ShopStage.Finish),
                ShopStatus = ShopInfo.ShopAuditStatus.WaitConfirm
            };
            ServiceHelper.Create<IShopService>().UpdateShop(shopInfo);
            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult EditProfile4()
        {
            ViewBag.Step = 2;
            ViewBag.Frame = "Step4";
            ViewBag.Manager = base.CurrentUser;
            ViewBag.Username = base.CurrentSellerManager.UserName;
            ViewBag.SellerAdminAgreement = GetSellerAgreement();
            return View("EditProfile");
        }

        [HttpGet]
        public ActionResult EditProfile5()
        {
            ViewBag.Step = 3;
            ViewBag.Frame = "Step5";
            ViewBag.Manager = base.CurrentUser;
            return View("EditProfile");

        }

        [HttpGet]
        public ActionResult EditProfile6()
        {
            ViewBag.Step = 2;
            ViewBag.Frame = "Step6";
            ViewBag.Manager = base.CurrentUser;
            ViewBag.SellerAdminAgreement = GetSellerAgreement();
            return View("EditProfile");
        }

        public ActionResult Finish()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCategories(long? key = null, int? level = -1)
        {
            IEnumerable<CategoryInfo> validBusinessCategoryByParentId = ServiceHelper.Create<ICategoryService>().GetValidBusinessCategoryByParentId(key.GetValueOrDefault());
            IEnumerable<KeyValuePair<long, string>> keyValuePair =
                from item in validBusinessCategoryByParentId
                select new KeyValuePair<long, string>(item.Id, item.Name);
            return Json(keyValuePair);
        }

        public string GetSellerAgreement()
        {
            AgreementInfo agreement = ServiceHelper.Create<ISystemAgreementService>().GetAgreement(AgreementInfo.AgreementTypes.Seller);
            if (agreement == null)
            {
                return "";
            }
            return agreement.AgreementContent;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Logo = base.CurrentSiteSetting.MemberLogo;
            filterContext.RouteData.Values["controller"].ToString().ToLower();
            string lower = filterContext.RouteData.Values["action"].ToString().ToLower();
            filterContext.RouteData.DataTokens["area"].ToString().ToLower();
            Log.Info(string.Concat("Executing:", lower));
            if (base.CurrentSellerManager == null && lower.IndexOf("step") != 0 && filterContext.RequestContext.HttpContext.Request.HttpMethod.ToUpper() != "POST")
            {
                if (lower != "EditProfile1".ToLower())
                {
                    RedirectToRouteResult action = RedirectToAction("EditProfile1", "ShopProfile", new { area = "SellerAdmin" });
                    filterContext.Result = action;
                    Log.Info(string.Concat("Executing1:", lower));
                    return;
                }
            }
            else if (base.CurrentSellerManager != null)
            {
                ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
                int valueOrDefault = (int)shop.Stage.GetValueOrDefault();
                ShopInfo.ShopStage? stage = shop.Stage;
                if ((stage.GetValueOrDefault() != ShopInfo.ShopStage.Finish ? false : stage.HasValue) && (shop.ShopStatus == ShopInfo.ShopAuditStatus.Open || shop.ShopStatus == ShopInfo.ShopAuditStatus.WaitAudit))
                {
                    RedirectToRouteResult redirectToRouteResult = RedirectToAction("index", "home", new { area = "SellerAdmin" });
                    filterContext.Result = redirectToRouteResult;
                    Log.Info(string.Concat("Executing2:", lower));
                    return;
                }
                ShopInfo.ShopStage? nullable = shop.Stage;
                if (((nullable.GetValueOrDefault() != ShopInfo.ShopStage.Finish ? true : !nullable.HasValue) || shop.ShopStatus == ShopInfo.ShopAuditStatus.WaitConfirm) && filterContext.RequestContext.HttpContext.Request.HttpMethod.ToUpper() != "POST" && lower.IndexOf("step") != 0)
                {
                    bool flag = true;
                    if (shop.ShopStatus == ShopInfo.ShopAuditStatus.Refuse || shop.ShopStatus == ShopInfo.ShopAuditStatus.Unusable)
                    {
                        string str = lower.ToLower().Replace("EditProfile".ToLower(), "");
                        int num = 0;
                        if (int.TryParse(str, out num) && num >= 1 && num <= 3)
                        {
                            flag = false;
                        }
                    }
                    if (flag && lower != string.Concat("EditProfile", valueOrDefault).ToLower())
                    {
                        RedirectToRouteResult action1 = RedirectToAction(string.Concat("EditProfile", valueOrDefault), "ShopProfile", new { area = "SellerAdmin" });
                        filterContext.Result = action1;
                        Log.Info(string.Concat("Executing3:", lower));
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Step0()
        {
            ViewBag.SellerAdminAgreement = GetSellerAgreement();
            return View();
        }

        [HttpGet]
        public ActionResult Step1()
        {
            DateTime now = DateTime.Now;
            ManagerInfo managerInfo = ServiceHelper.Create<IManagerService>().GetSellerManager(base.CurrentUser.UserName);
            string str1 = UserCookieEncryptHelper.Encrypt(managerInfo.Id, "SellerAdmin");
            WebHelper.SetCookie("ChemCloud-SellerManager", str1, now.AddDays(7));
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
            ShopProfileStep1 shopProfileStep1 = new ShopProfileStep1()
            {
                Address = shop.CompanyAddress,
                BusinessLicenceArea = shop.BusinessLicenceRegionId,
                BusinessLicenceNumber = shop.BusinessLicenceNumber,
                BusinessLicenceNumberPhoto = shop.BusinessLicenceNumberPhoto
            };
            if (shop.BusinessLicenceEnd.HasValue)
            {
                shopProfileStep1.BusinessLicenceValidEnd = shop.BusinessLicenceEnd.Value;
            }
            if (shop.BusinessLicenceStart.HasValue)
            {
                shopProfileStep1.BusinessLicenceValidStart = shop.BusinessLicenceStart.Value;
            }
            string empty = string.Empty;
            for (int i = 1; i < 4; i++)
            {
                if (System.IO.File.Exists(Server.MapPath(string.Concat(shop.BusinessLicenseCert, string.Format("{0}.png", i)))))
                {
                    empty = string.Concat(empty, shop.BusinessLicenseCert, string.Format("{0}.png", i), ",");
                }
            }
            char[] chrArray = new char[] { ',' };
            shopProfileStep1.BusinessLicenseCert = empty.TrimEnd(chrArray);
            shopProfileStep1.BusinessSphere = shop.BusinessSphere;
            shopProfileStep1.CityRegionId = shop.CompanyRegionId;
            if (shop.CompanyFoundingDate.HasValue)
            {
                shopProfileStep1.CompanyFoundingDate = shop.CompanyFoundingDate.Value;
            }
            shopProfileStep1.CompanyName = shop.CompanyName;
            shopProfileStep1.ContactName = shop.ContactsName;
            shopProfileStep1.ContactPhone = shop.ContactsPhone;
            shopProfileStep1.Email = shop.ContactsEmail;
            shopProfileStep1.EmployeeCount = shop.CompanyEmployeeCount;
            shopProfileStep1.GeneralTaxpayerPhoto = shop.GeneralTaxpayerPhot;
            shopProfileStep1.legalPerson = shop.legalPerson;
            shopProfileStep1.OrganizationCode = shop.OrganizationCode;
            shopProfileStep1.OrganizationCodePhoto = shop.OrganizationCodePhoto;
            shopProfileStep1.ECompanyName = shop.ECompanyName;
            shopProfileStep1.EContactsName = shop.EContactsName;
            shopProfileStep1.ECompanyAddress = shop.ECompanyAddress;
            shopProfileStep1.CompanyType = shop.CompanyType;
            shopProfileStep1.ZipCode = shop.ZipCode;
            shopProfileStep1.Fax = shop.Fax;
            shopProfileStep1.URL = shop.URL;
            shopProfileStep1.ChemNumber = shop.ChemNumber;
            shopProfileStep1.ChemicalsBusinessLicense = shop.ChemicalsBusinessLicense;
            shopProfileStep1.GMPPhoto = shop.GMPPhoto;
            shopProfileStep1.FDAPhoto = shop.FDAPhoto;
            shopProfileStep1.ISOPhoto = shop.ISOPhoto;
            shopProfileStep1.TaxpayerId = shop.TaxpayerId;
            shopProfileStep1.Logo = shop.Logo;
            shopProfileStep1.TaxRegistrationCertificate = shop.TaxRegistrationCertificate;
            shopProfileStep1.TaxRegistrationCertificatePhoto = shop.TaxRegistrationCertificatePhoto;
            shopProfileStep1.CertificatePhoto = shop.CertificatePhoto;
            shopProfileStep1.OtherCert = shop.OtherCert;
            //string str = string.Empty;
            //for (int j = 1; j < 4; j++)
            //{
            //    if (System.IO.File.Exists(Server.MapPath(string.Concat(shop.OtherCert, string.Format("{0}.png", j)))))
            //    {
            //        str = string.Concat(str, shop.OtherCert, string.Format("{0}.png", j), ",");
            //    }
            //}
            char[] chrArray1 = new char[] { ',' };
            //shopProfileStep1.OtherCert = str.TrimEnd(chrArray1);
            shopProfileStep1.Phone = shop.CompanyPhone;
            string empty1 = string.Empty;
            for (int k = 1; k < 4; k++)
            {
                if (System.IO.File.Exists(Server.MapPath(string.Concat(shop.ProductCert, string.Format("{0}.png", k)))))
                {
                    empty1 = string.Concat(empty1, shop.ProductCert, string.Format("{0}.png", k), ",");
                }
            }
            char[] chrArray2 = new char[] { ',' };
            shopProfileStep1.ProductCert = empty1.TrimEnd(chrArray2);
            shopProfileStep1.RegisterMoney = shop.CompanyRegisteredCapital;
            shopProfileStep1.taxRegistrationCert = shop.TaxRegistrationCertificate;
            ViewBag.CompanyRegionIds = ServiceHelper.Create<IRegionService>().GetRegionIdPath(shop.CompanyRegionId);
            ViewBag.BusinessLicenceRegionIds = ServiceHelper.Create<IRegionService>().GetRegionIdPath(shop.BusinessLicenceRegionId);
            string refuseReason = "";
            if (shop.ShopStatus == ShopInfo.ShopAuditStatus.Refuse)
            {
                refuseReason = shop.RefuseReason;
            }
            ViewBag.RefuseReason = refuseReason;
            ViewBag.CompanyType = shopProfileStep1.CompanyType;
            return View(shopProfileStep1);
        }

        [HttpGet]
        public ActionResult Step2()
        {
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
            ShopProfileStep2 shopProfileStep2 = new ShopProfileStep2()
            {
                BankAccountName = shop.BankAccountName,
                BankAccountNumber = shop.BankAccountNumber,
                BankCode = shop.BankCode,
                BankName = shop.BankName,
                BankPhoto = shop.BankPhoto,
                BankRegionId = shop.BankRegionId,
                BeneficiaryBankName = shop.BeneficiaryBankName,
                SWiftBic = shop.SWiftBic,
                BeneficiaryName = shop.BeneficiaryName,
                BeneficiaryAccountNum = shop.BeneficiaryAccountNum,
                CompanysAddress = shop.CompanysAddress,
                BeneficiaryBankBranchAddress = shop.BeneficiaryBankBranchAddress,
                AbaRoutingNumber = shop.AbaRoutingNumber
            };
            ViewBag.BankRegionIds = ServiceHelper.Create<IRegionService>().GetRegionIdPath(shop.BankRegionId);
            return View(shopProfileStep2);
        }

        [HttpGet]
        public ActionResult Step3()
        {
            ViewBag.ShopGrades = ServiceHelper.Create<IShopService>().GetShopGrades();
            ViewBag.ShopCategories = ServiceHelper.Create<ICategoryService>().GetMainCategory();
            ViewBag.Username = base.CurrentSellerManager.UserName;
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, true);
            List<CategoryKeyVal> categoryKeyVals = new List<CategoryKeyVal>();
            foreach (long key in shop.BusinessCategory.Keys)
            {
                CategoryKeyVal categoryKeyVal = new CategoryKeyVal()
                {
                    CommisRate = shop.BusinessCategory[key],
                    Name = ServiceHelper.Create<ICategoryService>().GetCategory(key).Name
                };
                categoryKeyVals.Add(categoryKeyVal);
            }
            ShopProfileStep3 shopProfileStep3 = new ShopProfileStep3()
            {
                ShopName = shop.ShopName,
                ShopGrade = shop.GradeId,
                BusinessCategory = ServiceHelper.Create<IShopService>().GetBusinessCategory(base.CurrentSellerManager.ShopId).ToList()
            };
            return View(shopProfileStep3);
        }

        [HttpGet]
        public ActionResult Step4()
        {
            string str;
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, true);
            //if (shop.ShopStatus != ShopInfo.ShopAuditStatus.Open )  //判断审核是否通过
            //{

            if (shop.ShopStatus != ShopInfo.ShopAuditStatus.WaitAudit)
            {
                if (shop.ShopStatus != ShopInfo.ShopAuditStatus.Refuse)
                {
                    return RedirectToAction("Finish");
                }


                ViewBag.Step = 1;
                ViewBag.MenuStep = 1;
                ViewBag.Frame = "Step1";
                ViewBag.Manager = base.CurrentSellerManager;
                return View("EditProfile", new ShopProfileStep1());
            }


            str = "step4";
            ViewBag.Text = "企业信息已经提交，请等待管理员审核";
            //}
            //else
            //{
            //    str = "step5";
            //}
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
            return View(str, shopModel);
        }

        [HttpGet]
        public ActionResult Step5()
        {
            ViewBag.Text = "付款凭证已经提交，请等待管理员核对后为您开通供应商";
            // ViewBag.Text = "企业信息已经提交，请等待管理员审核";
            if (ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false).ShopStatus == ShopInfo.ShopAuditStatus.WaitConfirm)
            {
                return View("step4");
            }
            return RedirectToAction("Finish");
        }
        [HttpGet]
        public ActionResult Step6()
        {
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
            ShopProfileStep2 shopProfileStep2 = new ShopProfileStep2()
            {
                BankAccountName = shop.BankAccountName,
                BankAccountNumber = shop.BankAccountNumber,
                BankCode = shop.BankCode,
                BankName = shop.BankName,
                BankPhoto = shop.BankPhoto,
                BeneficiaryBankName = shop.BeneficiaryBankName,
                SWiftBic = shop.SWiftBic,
                BeneficiaryName = shop.BeneficiaryName,
                BeneficiaryAccountNum = shop.BeneficiaryAccountNum,
                CompanysAddress = shop.CompanysAddress,
                BeneficiaryBankBranchAddress = shop.BeneficiaryBankBranchAddress,
                AbaRoutingNumber = shop.AbaRoutingNumber
                //BankRegionId = shop.BankRegionId,
                //TaxpayerId = shop.TaxpayerId,
                //TaxRegistrationCertificate = shop.TaxRegistrationCertificate,
                //TaxRegistrationCertificatePhoto = shop.TaxRegistrationCertificatePhoto
            };
            ViewBag.BankRegionIds = ServiceHelper.Create<IRegionService>().GetRegionIdPath(shop.BankRegionId);
            return View(shopProfileStep2);
        }
    }
}