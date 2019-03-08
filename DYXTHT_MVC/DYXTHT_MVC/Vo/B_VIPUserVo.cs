using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_VIPUserVo:B_VIPUserTable
    {
        public string UserName { get; set; }

        public string UserTypeName { get; set; }

        public string StatusName { get; set; }

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

        private string enTimeStr;
        public string ENTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    enTimeStr = dt.ToString("yyyy/MM/dd");
                }
                catch (Exception)
                {

                    enTimeStr = value;
                }
            }

            get
            {
                return enTimeStr;
            }
        }
        private string ITimeStr;
        public string ETimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    ITimeStr = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    ITimeStr = value;
                }
            }

            get
            {
                return ITimeStr;
            }
        }
    }
}