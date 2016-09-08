using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ChemCloud.Model.Common;

namespace ChemCloud.IServices.QueryModel
{
    public class QueryCommon<T> where T : class, new()
    {
        public T ParamInfo { get; set; }

        public PageInfo PageInfo { get; set; }

    }

}
