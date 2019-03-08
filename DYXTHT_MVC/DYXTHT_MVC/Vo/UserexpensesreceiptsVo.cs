using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class UserexpensesreceiptsVo:B_UserExpensesTable
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal? PropertyAmounts { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string OperateTypeName { get; set; }

        public string strOperateMoney { get; set; }
        public string strPropertyAmounts { get; set; }
        public string strEarning { get; set; }
        public string strExpenses { get; set; }

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
    }
}