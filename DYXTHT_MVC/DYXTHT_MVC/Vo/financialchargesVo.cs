using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class financialchargesVo:S_FundCostTable
    {
        /// <summary>
        /// 费用类型
        /// </summary>
        public string CostTypeName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
    }
}