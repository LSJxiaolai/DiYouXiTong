using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_CapitalrecordVo: B_CapitalrecordTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 资金类型
        /// </summary>
        public string OperateTypeName { get; set; }

        public string strOpFare { get; set; }
        public string strIncome { get; set; }
        public string strExpend { get; set; }
        public string strPropertyAmounts { get; set; }
       

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