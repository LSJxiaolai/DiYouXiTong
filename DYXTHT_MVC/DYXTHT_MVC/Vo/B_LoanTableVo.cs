using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_LoanTableVo:B_LoanTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName{ get; set; }
        /// <summary>
        /// 借款（回购）期限
        /// </summary>
        public string LoanDeadlineName { get; set; }
        /// <summary>
        /// 借款类型
        /// </summary>
        public string Treetoptype { get; set; }
        /// <summary>
        /// 还款方式
        /// </summary>
        public string RepaymentWayName { get; set; }

        private string releaseTimeStr;
        public string ReleaseTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    releaseTimeStr = dt.ToString("yyyy/MM/dd  HH:mm");
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


        private string endTimeStr;
        public string EndTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    endTimeStr = dt.ToString("yyyy/MM/dd  HH:mm");
                }
                catch (Exception)
                {

                    endTimeStr = value;
                }
            }

            get
            {
                return endTimeStr;
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 进度
        /// </summary>
        public string scheduleinvestment { get; set; }

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

        private string yimeStr;
        public string YimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    yimeStr = dt.ToString("yyyy/MM/dd");
                }
                catch (Exception)
                {

                    yimeStr = value;
                }
            }

            get
            {
                return yimeStr;
            }
        }

    }
}