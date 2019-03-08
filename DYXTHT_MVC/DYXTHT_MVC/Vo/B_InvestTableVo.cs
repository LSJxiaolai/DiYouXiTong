using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_InvestTableVo: B_InvestTable
    {
        /// <summary>
        /// 投资人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 投资时间
        /// </summary>
        private string releaseTimeStr;
        public string ReleaseTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    releaseTimeStr = dt.ToString("yyyy/MM/dd  HH:mm:ss");
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
        /// <summary>
        /// 借款标题
        /// </summary>
        public string Loantitle { get; set; }
        /// <summary>
        /// 贷款号
        /// </summary>
        public string PaymentNumber { get; set; }

        public decimal? LoanMoney { get; set; }

        public string YESNo { get; set; }
    }
}