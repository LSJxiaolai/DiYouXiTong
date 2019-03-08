using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiYouQianTaiXiTong.Models;

namespace DiYouQianTaiXiTong.Vo
{
    public class LoanDetail:B_LoanDetailTable
    {
        private string startTime;

        public decimal? LoanMoney { get; set; }
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

        public string Treetoptype { get; set; }
    }
}