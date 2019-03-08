using DYXTHT_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DYXTHT_MVC.Vo
{
    public class WebsitefundsVo: B_WebsiteExpensesTable
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string OperateTypeName { get; set; }

        public string strOperateMoney { get; set; }
        public string strEarning { get; set; }
        public string strExpenses { get; set; }

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