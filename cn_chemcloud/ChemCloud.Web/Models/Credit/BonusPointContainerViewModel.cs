using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemCloud.Web.Models.Credit
{
    /// <summary>
    /// 积分 容器
    /// </summary>
    public class BonusPointContainerViewModel
    {
        public BonusPointContainerViewModel()
        {
            this.rows = new List<BonusPointViewModel>();
            this.totalRows = 0;
        }

        public BonusPointContainerViewModel(IEnumerable<BonusPointViewModel> rows)
        {
            this.rows = rows;
            this.totalRows = rows.Count();
        }

        public BonusPointContainerViewModel(IEnumerable<BonusPointViewModel> rows,int totalRows)
        {
            this.rows = rows;
            this.totalRows = totalRows;
        }

        public IEnumerable<BonusPointViewModel> rows { get; set; }

        public int totalRows { get; set; }
    }
}