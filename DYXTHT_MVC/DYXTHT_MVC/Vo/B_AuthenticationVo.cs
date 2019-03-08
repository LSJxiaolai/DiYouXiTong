using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_AuthenticationVo:B_AuthenticationTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        public string TrueName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 入学时间
        /// </summary>
        public string Intime { get; set; }
        /// <summary>
        /// 毕业时间
        /// </summary>
        public string Entime { get; set; }

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