using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class NoticeVo:B_NoticeTable
    {
        /// <summary>
        /// 分类栏目
        /// </summary>
        public string NoticeTypeName { get; set; }
        /// <summary>
        /// 编辑者
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 公告状态
        /// </summary>
        public string NoticeStatusName { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        private string releaseTimeStr;
        public string ReleaseTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    releaseTimeStr = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    releaseTimeStr = value;
                }              
            }

            get
            {
                return releaseTimeStr;
            }
        }
    }
}