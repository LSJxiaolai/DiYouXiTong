﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class CashregisterVo:B_UserCashRecordTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
      
        /// <summary>
        /// 提现银行
        /// </summary>
        public string PayTypeName { get; set; }
        /// <summary>
        /// 所在地
        /// </summary>   
        public string ProNameCityName { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string ProName { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }


        public string strCashAmount { get; set; }
        public string strAccountMoney { get; set; }
        public string strPoundage { get; set; }
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