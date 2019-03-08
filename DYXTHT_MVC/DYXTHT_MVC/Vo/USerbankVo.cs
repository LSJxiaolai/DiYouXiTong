using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class USerbankVo:B_UserTable
    {

        /// <summary>
        /// 所属银行
        /// </summary>
        public string PayTypeName { get; set; }
        /// <summary>
        /// 所在地
        /// </summary>   
        public string ProNameCityName { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string ProName { get; set; }
       /// <summary>
       /// 城市
       /// </summary>
        public string CityName { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }

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
        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserTypeName { get; set; }


        public string BornDates { get; set; }

    }
}