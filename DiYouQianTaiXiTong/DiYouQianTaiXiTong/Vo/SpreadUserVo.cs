using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiYouQianTaiXiTong.Models;

namespace DiYouQianTaiXiTong.Vo
{
    public class SpreadUserVo:B_SpreadUserTable
    {
        private string startTime;

        public decimal? InvestAmount { get;  set; }
        public string Name { get; set; }
        public decimal? RepaymentAmount { get;  set; }

        public string StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                try
                {
                    startTime = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    startTime = value;
                }
            }
        }

        public string UserName { get;  set; }
    }
}