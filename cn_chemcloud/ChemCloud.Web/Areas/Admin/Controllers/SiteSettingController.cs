using AutoMapper;
using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class SiteSettingController : BaseAdminController
    {
        public SiteSettingController()
        {
        }

        public ActionResult Edit()
        {
            //邮箱设置
            ViewBag.EmailSetting = ServiceHelper.Create<IEmailSettingService>().GetChemCloud_EmailSetting();

            SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
            Mapper.CreateMap<SiteSettingsInfo, SiteSettingModel>().ForMember((SiteSettingModel a) => a.SiteIsOpen, (IMemberConfigurationExpression<SiteSettingsInfo> b) => b.MapFrom<bool>((SiteSettingsInfo s) => s.SiteIsClose));
            return View(Mapper.Map<SiteSettingsInfo, SiteSettingModel>(siteSettings));
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(SiteSettingModel siteSettingModel)
        {
            //if (string.IsNullOrWhiteSpace(siteSettingModel.WXLogo))
            //{
            //    Result result = new Result()
            //    {
            //        success = false,
            //        msg = "请上传微信Logo",
            //        status = 1
            //    };
            //    return Json(result);
            //}
            siteSettingModel.WXLogo = "/Storage/Plat/Site/wxlogo.jpg";
            string mapPath = IOHelper.GetMapPath(siteSettingModel.Logo);
            string str = IOHelper.GetMapPath(siteSettingModel.MemberLogo);
            string mapPath1 = IOHelper.GetMapPath(siteSettingModel.QRCode);
            string str1 = string.Concat("logo", (new FileInfo(mapPath)).Extension);
            string str2 = string.Concat("memberLogo", (new FileInfo(mapPath)).Extension);
            string str3 = string.Concat("qrCode", (new FileInfo(mapPath)).Extension);
            string str4 = "/Storage/Plat/Site/";
            string mapPath2 = IOHelper.GetMapPath(str4);
            //if (!Directory.Exists(mapPath2))
            //{
            //    Directory.CreateDirectory(mapPath2);
            //}
            //if (!siteSettingModel.Logo.Contains("/Storage"))
            //{
            //    IOHelper.CopyFile(mapPath, mapPath2, false, str1);
            //}
            //if (!siteSettingModel.MemberLogo.Contains("/Storage"))
            //{
            //    IOHelper.CopyFile(str, mapPath2, false, str2);
            //}
            //if (!siteSettingModel.QRCode.Contains("/Storage"))
            //{
            //    IOHelper.CopyFile(mapPath1, mapPath2, false, str3);
            //}
            //if (!siteSettingModel.WXLogo.Contains("/Storage"))
            //{
            //    string str5 = string.Concat(str4, "wxlogo.png");
            //    string mapPath3 = IOHelper.GetMapPath(siteSettingModel.WXLogo);
            //    string mapPath4 = IOHelper.GetMapPath(str5);
            //    using (Image image = Image.FromFile(mapPath3))
            //    {
            //        image.Save(string.Concat(mapPath3, ".png"), ImageFormat.Png);
            //        if (System.IO.File.Exists(mapPath4))
            //        {
            //            System.IO.File.Delete(mapPath4);
            //        }
            //        ImageHelper.CreateThumbnail(string.Concat(mapPath3, ".png"), mapPath4, 100, 100);
            //    }
            //    siteSettingModel.WXLogo = str5;
            //}
            Result result1 = new Result();
            SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
            siteSettings.SiteName = siteSettingModel.SiteName;
            siteSettings.SiteIsClose = siteSettingModel.SiteIsOpen;
            siteSettings.Logo = string.Concat(str4, str1);
            siteSettings.MemberLogo = string.Concat(str4, str2);
            siteSettings.QRCode = string.Concat(str4, str3);
            siteSettings.FlowScript = siteSettingModel.FlowScript;
            siteSettings.Site_SEOTitle = siteSettingModel.Site_SEOTitle;
            siteSettings.Site_SEOKeywords = siteSettingModel.Site_SEOKeywords;
            siteSettings.Site_SEODescription = siteSettingModel.Site_SEODescription;
            siteSettings.MobileVerifOpen = siteSettingModel.MobileVerifOpen;
            siteSettings.WXLogo = siteSettingModel.WXLogo;

            siteSettings.PlatformCollectionAddress = siteSettingModel.PlatformCollectionAddress;
            siteSettings.InsurancefeeMaxValue = siteSettingModel.InsurancefeeMaxValue;
            siteSettings.MaterialMallURL = siteSettingModel.MaterialMallURL;
            siteSettings.BBSURL = siteSettingModel.BBSURL;
            siteSettings.ImageFilePath = siteSettingModel.ImageFilePath;

            siteSettings.PurchasingAmount = siteSettingModel.PurchasingAmount;
            siteSettings.SynthsisAmount = siteSettingModel.SynthsisAmount;
            siteSettings.SupplierCertificationAmount = siteSettingModel.SupplierCertificationAmount;
            siteSettings.ProductCertificationAmount = siteSettingModel.ProductCertificationAmount;
            siteSettings.SortCost = siteSettingModel.SortCost;
            siteSettings.techName = siteSettingModel.techName;
            siteSettings.techTel = siteSettingModel.techTel;
            siteSettings.techEmail = siteSettingModel.techEmail;
            siteSettings.isShowHaoCai = siteSettingModel.isShowHaoCai;
            siteSettings.isShowHuiYiZhongXin = siteSettingModel.isShowHuiYiZhongXin;
            siteSettings.isShowDaShujuZhongXin = siteSettingModel.isShowDaShujuZhongXin;
            siteSettings.isShowJiShuJiaoYiZhongXin = siteSettingModel.isShowJiShuJiaoYiZhongXin;
            siteSettings.isShowRenCaiShiChang = siteSettingModel.isShowRenCaiShiChang;
            siteSettings.isShowLunTan = siteSettingModel.isShowLunTan;
            siteSettings.isShowFaLvFaGui = siteSettingModel.isShowFaLvFaGui;
            siteSettings.CustomerTel = siteSettingModel.CustomerTel;
            siteSettings.Hotkeywords = siteSettingModel.Hotkeywords;
            siteSettings.Keyword = siteSettingModel.Keyword;
            if (siteSettingModel.FirstAmount > 0)
                siteSettings.FirstAmount = siteSettingModel.FirstAmount;
            else
                siteSettings.FirstAmount = 0;
            if (siteSettingModel.SecondAmount > 0)
               siteSettings.SecondAmount = siteSettingModel.SecondAmount;
            else
                siteSettings.SecondAmount = 0;
            if(siteSettingModel.leijiFirstAmount > 0)
              siteSettings.leijiFirstAmount = siteSettingModel.leijiFirstAmount;
            else
                siteSettings.leijiFirstAmount = 0;
            if (siteSettingModel.leijiSecondAmount > 0)
                siteSettings.leijiSecondAmount = siteSettingModel.leijiSecondAmount;
            else
                siteSettings.leijiSecondAmount = 0;
            if (siteSettingModel.Runtime != null)
                siteSettings.Runtime = siteSettingModel.Runtime;
            else
                siteSettings.Runtime = DateTime.Now.Date;

            siteSettings.PlatCall = siteSettingModel.PlatCall;
            siteSettings.Operationreport = siteSettingModel.Operationreport;

            ServiceHelper.Create<ISiteSettingService>().SetSiteSettings(siteSettings);
            result1.success = true;
            return Json(result1);
        }



        [HttpPost]
        public ActionResult EmailSettingEdit(string Id, string SmtpServer, string SendMailId, string SendMailPassword)
        {
            ChemCloud_EmailSetting model = new ChemCloud_EmailSetting()
            {
                Id = long.Parse(Id),
                SmtpServer = SmtpServer,
                SendMailId = SendMailId,
                SendMailPassword = SendMailPassword
            };
            Result result1 = new Result();
            if (model.Id > 0)
            {
                if (ServiceHelper.Create<IEmailSettingService>().EditEmailSetting(model))
                {
                    result1.success = true;
                    result1.msg = "编辑成功！";
                }
                else
                {
                    result1.success = false;
                    result1.msg = "编辑失败！";
                }
            }
            else
            {
                if (ServiceHelper.Create<IEmailSettingService>().AddEmailSetting(model))
                {
                    result1.success = true;
                    result1.msg = "添加成功！";
                }
                else
                {
                    result1.success = false;
                    result1.msg = "添加失败！";
                }
            }
            return Json(result1);
        }
    }
}