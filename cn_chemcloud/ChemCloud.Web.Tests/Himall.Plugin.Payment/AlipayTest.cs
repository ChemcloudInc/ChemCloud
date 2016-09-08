using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemCloud.Web.Tests
{
    [TestClass]
    public class AlipayTest
    {
        [TestMethod]
        public void Alipay()
        {
            ChemCloud.Plugin.Payment.Alipay.Service service = new Plugin.Payment.Alipay.Service();
            service.WorkDirectory = @"D:\Himall2\ChemCloud.Web\Plugins\Payment\ChemCloud.Plugin.Payment.Alipay";
            string AlipayUrl = service.GetRequestUrl("http://#returnUrl", "http://#notyfyUrl", "#0001", 88, "测试商品");

        }
    }
}
