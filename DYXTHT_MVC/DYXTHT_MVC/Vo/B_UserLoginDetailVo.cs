using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_UserLoginDetailVo:B_UserLoginDetailTable
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }

        private string strRegisterTime;
        public string StrRegisterTime
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    strRegisterTime = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    strRegisterTime = value;
                }
            }

            get
            {
                return strRegisterTime;
            }
        }


        private string strlastLoginTime;
        public string StrlastLoginTime
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    strlastLoginTime = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    strlastLoginTime = value;
                }
            }

            get
            {
                return strlastLoginTime;
            }
        }


        private string strEndLoginTime;
        public string StrEndLoginTime
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    strEndLoginTime = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    strEndLoginTime = value;
                }
            }

            get
            {
                return strEndLoginTime;
            }
        }

    }
}