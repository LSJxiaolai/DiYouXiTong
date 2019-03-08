using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class CostTypeVo:B_CostTypeTable
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
    }
}