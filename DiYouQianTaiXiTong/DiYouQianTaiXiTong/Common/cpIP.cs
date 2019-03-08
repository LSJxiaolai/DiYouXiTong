using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiYouQianTaiXiTong.Common
{
    public class cpIP
    {
        /// <summary>
        /// IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;

        }

    }
}