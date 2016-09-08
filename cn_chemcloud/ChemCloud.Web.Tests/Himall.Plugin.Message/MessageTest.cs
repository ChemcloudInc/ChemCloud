using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemCloud.Web.Tests.Message
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void SMS()
        {
            ChemCloud.Plugin.Message.SMS.Service service = new Plugin.Message.SMS.Service();
            service.WorkDirectory = @"D:\Himall2\ChemCloud.Web\Plugins\Message\ChemCloud.Plugin.Message.SMS";
            string ret = service.SendTestMessage("1550769****", "测试", "测试");
        }
    }
}
