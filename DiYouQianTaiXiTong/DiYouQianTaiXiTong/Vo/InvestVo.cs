using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiYouQianTaiXiTong.Models;

namespace DiYouQianTaiXiTong.Vo
{
    public class InvestVo:B_InvestTable
    {
        private string startTime;

        public DateTime? Endtime { get;  set; }
        public int? LoanDeadlineID { get;  set; }
        public string LoanDeadlineName { get; set; }
        public string LoanPurpose { get;  set; }
        public string Rate { get; set; }
        public decimal? Scheduleinvestment { get;  set; }

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

        public string StatusName { get;  set; }
        public DateTime? SubmitTime { get;  set; }
        public string TreetopType { get; set; }
        public int? TreetoptypeID { get; set; }
        public string UserName { get;  set; }
    }
}