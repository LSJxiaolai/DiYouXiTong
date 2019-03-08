using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_CreditorAttornTableVo: B_CreditorAttornTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

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

        public string WaitPeriodss { get; set; }

        public string AttornPeriodss { get; set; }

        public string AllPeriodss { get; set; }

        public string StatusName { get; set; }
        /// <summary>
        /// 购买时间
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
        public string PurchaserUser { get; set; }
    }
}