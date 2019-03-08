using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_LimitApplicationVo: B_LimitApplicationTable
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string LimitType { get; set; }

        /// <summary>
        /// 种类
        /// </summary>
        public string limitkindName { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public string limitoperateName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        ///通过金额
        /// </summary>
        public decimal TongguoMoney { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        private string releaseTimeStr;
        public string ReleaseTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    releaseTimeStr = dt.ToString("yyyy/MM/dd");
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

        /// <summary>
        /// 审核时间
        /// </summary>
        private string timeStr;
        public string TimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    timeStr = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    timeStr = value;
                }
            }

            get
            {
                return timeStr;
            }
        }
    }
}