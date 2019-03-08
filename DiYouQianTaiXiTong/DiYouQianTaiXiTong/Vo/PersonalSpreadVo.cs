using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiYouQianTaiXiTong.Models;

namespace DiYouQianTaiXiTong.Vo
{
    public class PersonalSpreadVo : B_PersonalSpreadMessage
    {
        public int? SpreadRenID { get; set; }
        public int? UserID { get; set; }
       
        private string startTime;
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