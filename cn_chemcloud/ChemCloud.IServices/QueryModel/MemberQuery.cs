using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class MemberQuery : QueryBase
    {
        public int CityId
        {
            get;
            set;
        }

        public int CountyId
        {
            get;
            set;
        }

        public string keyWords
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int ProvinceId
        {
            get;
            set;
        }

        public List<int> RegionIds
        {
            get;
            set;
        }

        public bool? Status
        {
            get;
            set;
        }

        public long UserId
        {
            get;
            set;
        }

        public long ParentSellerId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户类型 2供应商 3采购商
        /// </summary>
        public int UserType { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 账户激活状态 1激活0未激活
        /// </summary>
        public int Disabled { get; set; }

        public MemberQuery()
        {
            RegionIds = new List<int>();
        }
    }
}