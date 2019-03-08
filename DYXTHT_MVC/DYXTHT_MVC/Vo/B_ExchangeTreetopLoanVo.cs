using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_ExchangeTreetopLoanVo: B_ExchangeTreetopLoan
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 回购期限
        /// </summary>
        public string LoanDeadlineName { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        private string addTimeStr;
        public string AddTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    addTimeStr = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    addTimeStr = value;
                }
            }

            get
            {
                return addTimeStr;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
    }
}