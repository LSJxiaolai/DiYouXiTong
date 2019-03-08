using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiYouQianTaiXiTong.Models;


namespace DiYouQianTaiXiTong.Vo
{
    public class LoanVo : B_LoanTable
    {
        public string Treetoptype { get;  set; }
        public string TrueName { get; set; }
        public string startTime { get; set; }
        public string housingCondition { get; set; }

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

        public string LoanDeadline { get; set; }
        public string RepaymentWayName { get;  set; }
        public string LoanDeadlineName { get; set; }
        public DateTime? PayableDate { get;  set; }
        public string Sex { get;  set; }
        public DateTime? BornDate { get;  set; }
        public string MarriageState { get; set; }
        public string MonthIncome { get;  set; }
        public string Institutions { get;  set; }
        public string EducationalBackground { get;set; }
        public DateTime? EnrolTime { get; set; }
        public string WhetherBuyCar { get;  set; }
        public string PhoneNumber { get; set; }
        public string NativePlace { get;  set; }
        public int AuthenticationID { get;  set; }
        public string StatusName { get; internal set; }
        public string UserName { get; set; }
    }
}