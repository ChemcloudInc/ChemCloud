using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChemCloud.Web
{
	public class RouteConfig
	{
		public RouteConfig()
		{
		}
        
        /// <summary>
        /// 注册 信赖相关 路由
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterCreditRoutes(RouteCollection routes)
        {
            ////积分主界面
            routes.MapRoute("BonusPoint_Index", "BonusPoint/Index", new
            {
                controller = "BonusPoint",
                action = "Index",
            });


            ////得到所有 的积分
            routes.MapRoute("BonusPoint_SearchBonusPoint", "BonusPoint/SearchBonusPoint", new
            {
                controller = "BonusPoint",
                action = "SearchBonusPoint",
            });



            ////提交积分 Offset -
            routes.MapRoute("BonusPoint_MinusBonusPointValue", "BonusPoint/MinusBonusPointValue", new
            {
                controller = "BonusPoint",
                action = "MinusBonusPointValue",
            });

            ////提交积分 Offset +
            routes.MapRoute("BonusPoint_PlusBonusPointValue", "BonusPoint/PlusBonusPointValue", new
            {
                controller = "BonusPoint",
                action = "PlusBonusPointValue",
            });
             
        }
        /// <summary>
        /// 注册 聊天 路由
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterChatMessageRoutes(RouteCollection routes)
        {
            ////聊天主界面
            routes.MapRoute("ChatMessage_Index", "ChatMessage/Index", new
            {
                controller = "ChatMessage",
                action = "Index",
            });

         
            

            ////最近的聊天记录
            routes.MapRoute("ChatMessage_GetRecentMessageRecordPage", "ChatMessage/GetRecentMessageRecordPage", new
            {
                controller = "ChatMessage",
                action = "GetRecentMessageRecordPage",
            });

            ////历史的聊天记录
            routes.MapRoute("ChatMessage_GetHistroyMessageRecordPage", "ChatMessage/GetHistroyMessageRecordPage", new
            {
                controller = "ChatMessage",
                action = "GetHistroyMessageRecordPage",
            });

            ////保存消息到数据库
            routes.MapRoute("ChatMessage_SaveMessageRecord", "ChatMessage/SaveMessageRecord", new
            {
                controller = "ChatMessage",
                action = "SaveMessageRecord",
            });


            ////get 供应商 ID，UserName  ChatOtherSide/GetOtherSideInfoByShop
            routes.MapRoute("ChatOtherSide_GetOtherSideInfoByShop", "ChatOtherSide/GetOtherSideInfoByShop", new
            {
                controller = "ChatOtherSide",
                action = "GetOtherSideInfoByShop",
            });

            ////搜索好友页面
            routes.MapRoute("ChatOtherSide_GetSearchOtherSidePage", "ChatOtherSide/GetSearchOtherSidePage", new
            {
                controller = "ChatOtherSide",
                action = "GetSearchOtherSidePage",
            });



        }


		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoute("Default", "common/{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }, null);

            ////注册 聊天 路由
            RouteConfig.RegisterChatMessageRoutes(routes);

            RouteConfig.RegisterCreditRoutes(routes);

        }
	}
}