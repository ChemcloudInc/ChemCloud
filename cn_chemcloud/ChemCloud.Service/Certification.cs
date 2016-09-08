using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;
namespace ChemCloud.Service
{

    public class Certification : ServiceBase, ICertification, IService, IDisposable
    {

        /// <summary>
        /// 新增一条数据
        /// </summary>
        /// <returns></returns>
        public FieldCertification CreateFieldModulep()
        {
            FieldCertification fcf = new FieldCertification()
            {
                Status = FieldCertification.CertificationStatus.Unusable,
                Certificationcost = new decimal(0),
                AnnualSales = new decimal(0),
                Certification = "",
                EnterpriseHonor = "",
                ProductDetails = "",
                TelegraphicMoneyOrder = ""
            };
            FieldCertification fc = fcf;
            context.FieldCertification.Add(fc);
            context.SaveChanges();
            return fc;
        }
        /// <summary>
        /// 更新认证状态
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="status"></param>
        /// <param name="comments"></param>
        public void UpdateStatus(long Id, FieldCertification.CertificationStatus status, DateTime Date, string comments = "")
        {
            FieldCertification nullable = context.FieldCertification.FindById<FieldCertification>(Id);
            nullable.Status = status;
            if (!string.IsNullOrWhiteSpace(comments))
            {
                nullable.RefuseReason = comments;
            }
            /*审核通过*/
            if (status == FieldCertification.CertificationStatus.Open)
            {
                nullable.CertificationDate = Date;

                ManagerInfo _ManagerInfo = context.ManagerInfo.Where(q => q.CertificationId == Id).FirstOrDefault();
                if (_ManagerInfo != null)
                {
                    ShopInfo _ShopInfo = context.ShopInfo.FindById<ShopInfo>(_ManagerInfo.ShopId);
                    if (_ShopInfo != null)
                    {
                        _ShopInfo.SortType = 2; //0默认/1审核后/2实地/3付费；
                        context.SaveChanges();
                    }
                }
            }
            /*接受申请*/
            if (status == FieldCertification.CertificationStatus.Receive)
            {
                nullable.Status = FieldCertification.CertificationStatus.Receive;
                nullable.ToAcceptTheDate = Date;
            }
            /*支付待审核*/
            if (status == FieldCertification.CertificationStatus.PayandWaitAudit)
            {
                nullable.Status = FieldCertification.CertificationStatus.PayandWaitAudit;
                nullable.FeedbackDate = Date;
            }
            /*拒绝*/
            if (status == FieldCertification.CertificationStatus.Refuse)
            {
                nullable.Status = FieldCertification.CertificationStatus.Refuse;
                nullable.RefuseReason = comments;
                nullable.CertificationDate = Date;
            }
            context.SaveChanges();
        }
        public bool ExistCertification(string CompanyName, long Id = 0L)
        {
            FieldCertification FieldCertificationInfo = (
                from p in context.FieldCertification
                where p.CompanyName.Equals(CompanyName) && p.Id != Id
                select p).FirstOrDefault();
            if (FieldCertificationInfo != null && FieldCertificationInfo.Status != FieldCertification.CertificationStatus.Refuse)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 修改表中的数据
        /// </summary>
        /// <param name="Certification"></param>
        public void UpdateCertification(FieldCertification certification)
        {
            FieldCertification CName = context.FieldCertification.FindById<FieldCertification>(certification.Id);
            CName.EnterpriseHonor = certification.EnterpriseHonor ?? CName.EnterpriseHonor;
            CName.ProductDetails = certification.ProductDetails ?? CName.ProductDetails;
            DateTime? applicationDate = certification.ApplicationDate;
            CName.ApplicationDate = (applicationDate.HasValue ? new DateTime?(applicationDate.GetValueOrDefault()) : CName.ApplicationDate);
            CName.Status = certification.Status == 0 ? CName.Status : certification.Status;
            DateTime? toAcceptTheDate = certification.ToAcceptTheDate;
            CName.ToAcceptTheDate = (toAcceptTheDate.HasValue ? new DateTime?(toAcceptTheDate.GetValueOrDefault()) : CName.ToAcceptTheDate);
            DateTime? certificationDate = certification.CertificationDate;
            CName.CertificationDate = (certificationDate.HasValue ? new DateTime?(certificationDate.GetValueOrDefault()) : CName.CertificationDate);
            DateTime? feedbackDate = certification.FeedbackDate;
            CName.FeedbackDate = (feedbackDate.HasValue ? new DateTime?(feedbackDate.GetValueOrDefault()) : CName.FeedbackDate);
            CName.RefuseReason = certification.RefuseReason ?? CName.RefuseReason;
            CName.Certificationcost = (certification.Certificationcost == new decimal(0) ? CName.Certificationcost : certification.Certificationcost);
            CName.AnnualSales = (certification.AnnualSales == new decimal(0) ? CName.AnnualSales : certification.AnnualSales);
            CName.Certification = certification.Certification ?? CName.Certification;
            CName.EnterpriseHonor = certification.EnterpriseHonor ?? CName.EnterpriseHonor;
            CName.ProductDetails = certification.ProductDetails ?? CName.ProductDetails;
            CName.TelegraphicMoneyOrder = certification.TelegraphicMoneyOrder ?? CName.TelegraphicMoneyOrder;
            context.SaveChanges();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessCategoryOn"></param>
        /// <returns></returns>
        public FieldCertification GetCertification(long Id)
        {
            FieldCertification nums = context.FieldCertification.FindById<FieldCertification>(Id);
            ManagerInfo managerInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.CertificationId.Equals(nums.Id));
            if (managerInfo != null)
            {
                ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(managerInfo.ShopId));
                nums.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
                nums.CompanyAddress = (shopInfo == null ? "" : shopInfo.CompanyAddress);
                nums.LegalPerson = (shopInfo == null ? "" : shopInfo.legalPerson);
                nums.CompanyRegisteredCapital = shopInfo.CompanyRegisteredCapital;
            }
            return nums;

        }

        public ManagerInfo GetManager(long Id)
        {
            ManagerInfo managerInfo = context.ManagerInfo.FirstOrDefault<ManagerInfo>((ManagerInfo m) => m.CertificationId == Id);
            return managerInfo;
        }
        /// <summary>
        /// 根据FieldCertification
        /// </summary>
        /// <param name="FieldCertificationModel"></param>
        /// <returns></returns>

        public PageModel<FieldCertification> GetCertifications(FieldCertificationQuery FieldCertificationQueryModel)
        {
            //  IQueryable<FieldCertification> CertificationShop = context.FieldCertification.AsQueryable<FieldCertification>();
            IQueryable<FieldCertification> CertificationShop = from item in base.context.FieldCertification
                                                               where true
                                                               where ((int)item.Status) != 1
                                                               select item;
            long? Id = FieldCertificationQueryModel.Id;
            if ((Id.GetValueOrDefault() <= 0 ? false : Id.HasValue))
            {
                CertificationShop =
                    from item in CertificationShop
                    where item.Id == FieldCertificationQueryModel.Id.Value
                    select item;
            }
            if (FieldCertificationQueryModel.Status.HasValue && FieldCertificationQueryModel.Status.Value != 0)
            {
                FieldCertification.CertificationStatus value = FieldCertificationQueryModel.Status.Value;
                DateTime dateTime = DateTime.Now.Date.AddSeconds(-1);
                FieldCertification.CertificationStatus Status = value;
                if (FieldCertificationQueryModel.Status != null && Status != FieldCertification.CertificationStatus.Unusable)
                {
                    CertificationShop = from d in CertificationShop
                                        where (int)d.Status == (int)FieldCertificationQueryModel.Status.Value
                                        select d;
                }
            }
            Func<IQueryable<FieldCertification>, IOrderedQueryable<FieldCertification>> func = null;
            func = (IQueryable<FieldCertification> d) =>
                    from o in d
                    orderby o.ApplicationDate descending
                    select o;
            int num = CertificationShop.Count();
            CertificationShop = CertificationShop.GetPage(out num, FieldCertificationQueryModel.PageNo, FieldCertificationQueryModel.PageSize, func);
            foreach (FieldCertification list in CertificationShop.ToList())
            {
                ManagerInfo managerInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.CertificationId.Equals(list.Id));
                if (managerInfo != null)
                {
                    ShopInfo shopInfo = context.ShopInfo.FirstOrDefault((ShopInfo m) => m.Id.Equals(managerInfo.ShopId));
                    list.CompanyName = (shopInfo == null ? "" : shopInfo.CompanyName);
                    list.CompanyAddress = (shopInfo == null ? "" : shopInfo.CompanyAddress);
                    list.LegalPerson = (shopInfo == null ? "" : shopInfo.legalPerson);
                    list.CompanyRegisteredCapital = (shopInfo == null ? 0 : shopInfo.CompanyRegisteredCapital);
                }
            }
            return new PageModel<FieldCertification>()
            {
                Models = CertificationShop,
                Total = num
            };
        }
    }
}
