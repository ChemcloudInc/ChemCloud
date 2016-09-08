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
        /// ע�� ������� ·��
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterCreditRoutes(RouteCollection routes)
        {
            ////����������
            routes.MapRoute("BonusPoint_Index", "BonusPoint/Index", new
            {
                controller = "BonusPoint",
                action = "Index",
            });


            ////�õ����� �Ļ���
            routes.MapRoute("BonusPoint_SearchBonusPoint", "BonusPoint/SearchBonusPoint", new
            {
                controller = "BonusPoint",
                action = "SearchBonusPoint",
            });



            ////�ύ���� Offset -
            routes.MapRoute("BonusPoint_MinusBonusPointValue", "BonusPoint/MinusBonusPointValue", new
            {
                controller = "BonusPoint",
                action = "MinusBonusPointValue",
            });

            ////�ύ���� Offset +
            routes.MapRoute("BonusPoint_PlusBonusPointValue", "BonusPoint/PlusBonusPointValue", new
            {
                controller = "BonusPoint",
                action = "PlusBonusPointValue",
            });
             
        }
        /// <summary>
        /// ע�� ���� ·��
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterChatMessageRoutes(RouteCollection routes)
        {
            ////����������
            routes.MapRoute("ChatMessage_Index", "ChatMessage/Index", new
            {
                controller = "ChatMessage",
                action = "Index",
            });

         
            

            ////����������¼
            routes.MapRoute("ChatMessage_GetRecentMessageRecordPage", "ChatMessage/GetRecentMessageRecordPage", new
            {
                controller = "ChatMessage",
                action = "GetRecentMessageRecordPage",
            });

            ////��ʷ�������¼
            routes.MapRoute("ChatMessage_GetHistroyMessageRecordPage", "ChatMessage/GetHistroyMessageRecordPage", new
            {
                controller = "ChatMessage",
                action = "GetHistroyMessageRecordPage",
            });

            ////������Ϣ�����ݿ�
            routes.MapRoute("ChatMessage_SaveMessageRecord", "ChatMessage/SaveMessageRecord", new
            {
                controller = "ChatMessage",
                action = "SaveMessageRecord",
            });


            ////get ��Ӧ�� ID��UserName  ChatOtherSide/GetOtherSideInfoByShop
            routes.MapRoute("ChatOtherSide_GetOtherSideInfoByShop", "ChatOtherSide/GetOtherSideInfoByShop", new
            {
                controller = "ChatOtherSide",
                action = "GetOtherSideInfoByShop",
            });

            ////��������ҳ��
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

            ////ע�� ���� ·��
            RouteConfig.RegisterChatMessageRoutes(routes);

            RouteConfig.RegisterCreditRoutes(routes);

        }
	}
}