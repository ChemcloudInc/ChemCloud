using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;

namespace ChemCloud.IServices
{
    public interface IRecFloorImgService : IService, IDisposable
    {
        List<RecFloorImg> GetAll(int typeid);

        RecFloorImg GetSingle(int Id);

        void Update(RecFloorImg recFloorImg);


        void InsertInto(List<RecFloorImg> list);

       
    }
}
