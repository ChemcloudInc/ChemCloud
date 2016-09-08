using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class COAList:BaseModel
    {
        /// <summary>
        /// 编号 自动标识
        /// </summary>
        private long _id;
        public  long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                base.Id = value;
            }
        }
        /// <summary>
        /// COA添加时间
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// COA添加的用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// COA添加的路径
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// COA编号
        /// </summary>
        public string CoANo { get; set; }
        /// <summary>
        /// COA添加的用户类型
        /// </summary>
        public int AddUserType { get; set; }

        public string CASNo { get; set; }
    }
}
