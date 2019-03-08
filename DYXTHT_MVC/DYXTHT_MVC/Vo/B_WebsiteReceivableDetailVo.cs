using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_WebsiteReceivableDetailVo: B_WebsiteReceivableDetail
    {
        /// <summary>
        /// 借款人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 逾期天数
        /// </summary>
        public string YuQiTianShu { get; set; }
        
        /// <summary>
        /// 应收日期
        /// </summary>
        private string timeStr;
        public string TimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    timeStr = dt.ToString("yyyy/MM/dd");
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
        /// <summary>
        /// 实还时间
        /// </summary>
        private string websiteWhetherPayTimeStr;
        public string WebsiteWhetherPayTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    websiteWhetherPayTimeStr = dt.ToString("yyyy/MM/dd");
                }
                catch (Exception)
                {

                    websiteWhetherPayTimeStr = value;
                }
            }

            get
            {
                return websiteWhetherPayTimeStr;
            }
        }
    }
}