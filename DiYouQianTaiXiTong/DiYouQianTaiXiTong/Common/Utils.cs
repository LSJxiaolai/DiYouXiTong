using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiYouQianTaiXiTong.Common
{
    public class Utils
    {
        #region 生成随机字母或数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            //通过调用默认构造函数而频繁创建的不同对象将具有相同的默认种子值，因而会产生几组相同的随机数。
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        /// <summary>
        /// 按位数生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                // A-Z  ASCII值  65-90
                // a-z  ASCII值  97-122
                char ch;
                int num = random.Next();
                if ((num % 3) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else if ((num % 3) == 1)
                {
                    ch = (char)(0x61 + ((ushort)(num % 0x1a)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        #endregion

        #region 读取或写入cookie
        #region 写入cookie
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddDays(7);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddDays(7);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region 读取cookie
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            return "";
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null &&
                HttpContext.Current.Request.Cookies[strName][key] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());

            return "";
        }

        #endregion

        #region 修改cookie
        public static void ModCookie(string strName, string strValue)
        {
            if (HttpContext.Current.Request.Cookies != null &&
                HttpContext.Current.Request.Cookies[strName] != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
                cookie.Value = strValue;
            }
        }

        public static void ModCookie(string strName, string key, string strValue)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null &&
               HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
                if (cookie == null)
                {
                    cookie = new HttpCookie(strName);
                }
                cookie[key] = UrlEncode(strValue);
            }
        }

        public static void ModCookie(string strName, int expires)
        {
            if (HttpContext.Current.Request.Cookies != null &&
                HttpContext.Current.Request.Cookies[strName] != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
        }
        #endregion

        #region 删除cookie
        ///<summary>
        /// 删除cookies
        ///</summary>
        public static void DelCookie(string strName)
        {


            if (HttpContext.Current.Request.Cookies != null &&
            HttpContext.Current.Request.Cookies[strName] != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
                cookie.Expires = DateTime.Today.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(strName);
            }

        }
        #endregion
        #endregion

        #region URL处理
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }


        #endregion
    }
}