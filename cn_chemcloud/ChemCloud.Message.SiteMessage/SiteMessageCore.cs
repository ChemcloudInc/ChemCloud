using Himall.Core;
using Himall.Core.Plugins.Message;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Himall.Plugin.Message.SiteMessage
{
    internal class SiteMessageCore
    {
        public static string WorkDirectory
        {
            get;
            set;
        }

        public SiteMessageCore()
        {
        }

        
        public static MessageContent GetMessageContentConfig()
        {
            MessageContent messageContent = Cache.Get("MessageContent") as MessageContent;
            if (messageContent == null)
            {
                FileStream fileStream = new FileStream(string.Concat(SiteMessageCore.WorkDirectory, "\\Data\\MessageContent.xml"), FileMode.Open);
                try
                {
                    messageContent = (MessageContent)(new XmlSerializer(typeof(MessageContent))).Deserialize(fileStream);
                    Cache.Insert("MessageContent", messageContent);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        ((IDisposable)fileStream).Dispose();
                    }
                }
            }
            return messageContent;
        }
        public static void SaveMessageContentConfig(MessageContent config)
        {
            FileStream fileStream = new FileStream(string.Concat(SiteMessageCore.WorkDirectory, "\\Data\\MessageContent.xml"), FileMode.Create);
            try
            {
                (new XmlSerializer(typeof(MessageContent))).Serialize(fileStream,config);
                Cache.Insert("MessageContent", config);
            }
            finally
            {
                if (fileStream != null)
                {
                    ((IDisposable)fileStream).Dispose();
                }
            }
        }
    }
}