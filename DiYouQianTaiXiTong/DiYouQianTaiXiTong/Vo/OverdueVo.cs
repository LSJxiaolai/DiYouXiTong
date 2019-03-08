using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiYouQianTaiXiTong.Models;

namespace DiYouQianTaiXiTong.Vo
{
    public class OverdueVo:B_OverdueLoanTable
    {
        private string startTime;

        public decimal? AlreadyPrincipal { get; set; }
        public string LoanPeriods { get; set; }
        public string LoanPurpose { get; set; }

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
    }
}