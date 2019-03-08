using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiYouQianTaiXiTong.Models;

namespace DiYouQianTaiXiTong.Vo
{
    public class FollowVo:B_FollowTable
    {
        private string startTime;

        public DateTime? BornDate { get;  set; }
        public string LoanDeadlineName { get; set; }
        public decimal? LoanMoney { get;  set; }
        public string LoanPurpose { get;  set; }
        public string Loantitle { get;  set; }
        public string MonthIncome { get; set; }
        public string NativePlace { get; set; }
        public string Rate { get;  set; }
        public string Sex { get; set; }

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

        public string Treetoptype { get;  set; }
        public string UserName { get;  set; }
    }
}