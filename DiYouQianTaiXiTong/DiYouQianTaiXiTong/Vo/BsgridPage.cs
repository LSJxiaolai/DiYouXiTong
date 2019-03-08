using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiYouQianTaiXiTong.Vo
{
    public class BsgridPage
    {
        /// <summary>
        /// 页面大小
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int curPage { get; set; }

        public string sortName { get; set; }

        public string sortOrder { get; set; }

        /// <summary>
        /// 分页开始序号
        /// </summary>
        /// <returns></returns>
        public int GetStartIndex()
        {
            return (curPage - 1) * pageSize;
        }
        /// <summary>
        /// 分页结束序号
        /// </summary>
        /// <returns></returns>
        public int GetEndIndex()
        {
            return curPage * pageSize - 1;
        }
    }
}