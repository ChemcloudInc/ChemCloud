using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
    class FloorProductService : ServiceBase,IFloorProductService, IService, IDisposable
    {
        public List<FloorProductInfo> GetProducts(int tab)
        {
            List<FloorProductInfo> floorProductInfo =
                (from i in context.FloorProductInfo.FindBy((FloorProductInfo i) => i.Tab == tab)
                 orderby i.Id
                 select i).ToList();

            //if (floorProductInfo.Count() == 0)
            //{
            //    FloorProductInfo[] floorProductInfoArray = new FloorProductInfo[4];
            //    for (int num = 0; num < 4; num++)
            //    {
            //        FloorProductInfo fpi = new FloorProductInfo();
            //        fpi.Tab = tab;
            //        fpi.ProductId = 0;
            //        fpi.ProductInfo = null;
            //        fpi.FloorId = 0;
            //        fpi.HomeFloorInfo = null;
            //        floorProductInfoArray[num] = fpi;
            //    }
            //    context.FloorProductInfo.AddRange(floorProductInfoArray);
            //    context.SaveChanges();
            //    floorProductInfo = floorProductInfoArray;
            //}
            return floorProductInfo;
        }
    }
}
