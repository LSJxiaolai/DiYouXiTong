using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class S_TreetoptypetalbeVo: S_Treetoptypetalbe
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string FundStatusName { get; set; }
        /// <summary>
        /// 额度类型
        /// </summary>
        public string LimitType { get; set; }

        /// <summary>
        /// 借款期限
        /// </summary>
        public string LoanDeadlineName { get; set; }

        public string yearRate { get; set; }
        public string freezeBail { get; set; }
        
    }
}