using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model.Common
{
    public class Result_Common
    {

    }

    #region 公共:分页相关信息类 PageInfo
    /// <summary>
    /// 公共:分页相关信息类
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

    }
    #endregion

    #region 公共:返回错误提示信息类 Result_Msg
    /// <summary>
    /// 公共:返回错误提示信息类
    /// </summary>
    public class Result_Msg
    {
        /// <summary>
        /// 返回是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
    }
    #endregion

    #region 公共：返回 对象 + 提示信息 Result_Model
    /// <summary>
    /// 公共：返回 对象 + 提示信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result_Model<T> where T : class, new()
    {
        public T Model { get; set; }
        public Result_Msg Msg { get; set; }

    }
    #endregion

    #region 公共：返回 对象 + 列表 + 提示信息 Result_Model_List
    /// <summary>
    /// 公共：返回  对象 + 列表 + 提示信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result_Model_List<T, M> where T : class, new()
    {
        public M Model { get; set; }
        public List<T> List { get; set; }
        public Result_Msg Msg { get; set; }

    }
    #endregion

    #region 公共：返回 列表 + 提示信息 Result_List
    /// <summary>
    /// 公共：返回 列表 + 提示信息
    /// </summary>
    /// <typeparam name="T">列表对应类</typeparam>
    public class Result_List<T> where T : class, new()
    {
        public List<T> List { get; set; }
        public Result_Msg Msg { get; set; }

    }
    #endregion

    #region 公共：返回 双列表 + 提示信息 Result_List1
    /// <summary>
    ///  公共：返回 双列表 + 提示信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result_List1<T> where T : class, new()
    {
        public List<T> List1 { get; set; }

        public List<T> List2 { get; set; }

        public Result_Msg Msg { get; set; }

    }
    #endregion

    #region 公共：返回 双列表 + 提示信息 Result_List_Pager1
    /// <summary>
    ///  公共：返回 双列表 + 提示信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result_List_Pager1<T> where T : class, new()
    {
        public List<T> List1 { get; set; }

        public List<T> List2 { get; set; }

        public PageInfo PageInfo { get; set; }

        public Result_Msg Msg { get; set; }

    }
    #endregion

    #region 公共：返回 分页信息及列表 Result_List_Pager
    /// <summary>
    /// 公共：返回 分页信息及列表 Result_List_Pager
    /// </summary>
    /// <typeparam name="T">信息对应类</typeparam>
    public class Result_List_Pager<T> where T : class, new()
    {
        public List<T> List { get; set; }

        public PageInfo PageInfo { get; set; }

        public Result_Msg Msg { get; set; }
    }
    #endregion
}
