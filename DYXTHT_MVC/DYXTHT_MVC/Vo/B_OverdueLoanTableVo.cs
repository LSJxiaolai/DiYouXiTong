using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_OverdueLoanTableVo: B_OverdueLoanTable
    {
        /// <summary>
        /// 贷款号
        /// </summary>
        public string PaymentNumber { get; set; }
        /// <summary>
        /// 借款人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 借款ID
        /// </summary>
        public int? UserID { get; set; }
        
        /// <summary>
        /// 借款标题
        /// </summary>
        public string Loantitle { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Treetoptype { get; set; }
        public int? TreetoptypeID { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 网站是否垫付
        /// </summary>
        public string WebsiteWhetherPayStatus { get; set; }
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
        private string timeStr;
        public string TimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    timeStr = dt.ToString("yyyy/MM/dd");
                }
                catch (Exception)
                {

                    timeStr = value;
                }
            }

            get
            {
                return timeStr;
            }
        }

        public string YuQiTianShu { get; set; }

        /// <summary>
        /// 网站垫付时间
        /// </summary>
        private string websiteWhetherPayTimeStr;
        public string WebsiteWhetherPayTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    websiteWhetherPayTimeStr = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    websiteWhetherPayTimeStr = value;
                }
            }

            get
            {
                return websiteWhetherPayTimeStr;
            }
        }
        /// <summary>
        /// 垫付金额
        /// </summary>
        public decimal? LoanMoney { get; set; }


    }
}