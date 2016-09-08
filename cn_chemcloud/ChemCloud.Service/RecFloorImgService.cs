using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace ChemCloud.Service
{
    class RecFloorImgService : ServiceBase, IRecFloorImgService, IService, IDisposable
    {
        public List<RecFloorImg> GetAll(int typeid)
        {
            List<RecFloorImg> recFloorImg = (
                  from a in context.RecFloorImg where a.TypeId == typeid select a).ToList<RecFloorImg>();
            return recFloorImg;
        }

        public void Update(RecFloorImg recFloorImg)
        {
            if (recFloorImg!=null)
            {
                RecFloorImg rec = (
                 from a in context.RecFloorImg where a.Id == recFloorImg.Id select a).FirstOrDefault<RecFloorImg>();
                rec.URL = recFloorImg.URL;
                rec.Tag = recFloorImg.Tag;
                rec.ImageUrl = recFloorImg.ImageUrl;
                context.SaveChanges();
            }
        }

        public void InsertInto(List<RecFloorImg> list)
        {
            context.RecFloorImg.AddRange(list);
            context.SaveChanges();
        }


        public RecFloorImg GetSingle(int Id)
        {
            return (from a in context.RecFloorImg where a.Id == Id select a).FirstOrDefault<RecFloorImg>();
        }
    }
}
