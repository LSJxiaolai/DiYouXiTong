using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class S_RepaymentWayTableVo: S_RepaymentWayTable
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string FundStatusName { get; set; }
    }
}