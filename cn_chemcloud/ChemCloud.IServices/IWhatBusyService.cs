using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChemCloud.Model;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model.Common;

namespace ChemCloud.IServices
{
    public interface IWhatBusyService : IService, IDisposable
    {
        Result_List_Pager<WhatBusy> GetWhatBusyList(QueryCommon<WhatBusyQuery> query);
        Result_List<Result_WhatBusy> Get_WhatBusy_Top(int count);

        Result_Msg DeleteById(long Id, long userId);

        WhatBusy GetObjectById(int Id);
        Result_Model<WhatBusy> UpdateWhatBusy_By_WhatBusyType(QueryCommon<WhatBusyQuery> query);
        Result_Model<PageInfo> Get_PageInfo(QueryCommon<WhatBusyQuery> query);

    }
}
