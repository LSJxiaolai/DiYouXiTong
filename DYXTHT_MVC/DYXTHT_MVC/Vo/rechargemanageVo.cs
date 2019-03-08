using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class rechargemanageVo:B_UserRechargeRecordTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 充值类型
        /// </summary>
        public string RechargeTypeName { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string PayTypeName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }

        public string strRechargeMoney { get; set; }
        public string strRealityAccountMoney { get; set; }
        public string strRechargePoundage { get; set; }

        
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