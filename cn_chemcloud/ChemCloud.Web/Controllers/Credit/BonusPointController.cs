 
namespace ChemCloud.Web.Controllers.Credit
{


    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Razor;
    using System.Web.Routing;
    using System.Linq;
    using System.Linq.Expressions;
    using ChemCloud.Web.Models;
    using System.Web;
    using System.Collections.Specialized;
    using System.Configuration;
    


    using ChemCloud.Web.Models.Credit;
    /// <summary>
    /// 积分管理
    /// </summary>
    public class BonusPointController:Controller
    {
         

        [HttpGet()]
        /// <summary>
        /// 主界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            return this.View("~/Views/Credit/BonusPointManagementPage.cshtml");
        }

      /// <summary>
      /// 得到所有供应商的积分
      /// </summary>
      /// <param name="search"></param>
      /// <param name="limit"></param>
      /// <returns></returns>
        public ActionResult SearchBonusPoint(string search = "",int limit = 10 )
        {
            
            if (search == "")
            {
               
                return Json(new BonusPointContainerViewModel() , JsonRequestBehavior.AllowGet);
            }

            int i = 0;
            var data = Data.Credit.BonusPoint.SearchBonusPoint(search, limit);

            return Json(new BonusPointContainerViewModel(data), JsonRequestBehavior.AllowGet);
  
        }


        /// <summary>
        ///  补偿 积分值 +
        /// </summary>
        /// <param name="bonusPointUserID"></param>
        /// <param name="bonusPointValueOffset"></param>
        /// <returns></returns>
        public ActionResult PlusBonusPointValue(int bonusPointUserID, int bonusPointValueOffset)
        {
           
            Data.Credit.BonusPoint.PlusBonusPointValue(bonusPointUserID, bonusPointValueOffset);

            return null;
        }


        /// <summary>
        ///  补偿 积分值 -
        /// </summary>
        /// <param name="bonusPointUserID"></param>
        /// <param name="bonusPointValueOffset"></param>
        /// <returns></returns>
        public ActionResult MinusBonusPointValue(int bonusPointUserID, int bonusPointValueOffset)
        {

            Data.Credit.BonusPoint.MinusBonusPointValue(bonusPointUserID, bonusPointValueOffset);

            return null;
        }

    }
}