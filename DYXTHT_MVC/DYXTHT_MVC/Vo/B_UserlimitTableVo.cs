using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_UserlimitTableVo: B_UserlimitTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName{ get; set; }
        /// <summary>
        /// 信用可用
        /// </summary>
        public string Creditlimetlmoeny { get; set; }
        /// <summary>
        /// 担保可用
        /// </summary>
        public string Warrantlimetlmoney { get; set; }
        /// <summary>
        /// 抵押可用
        /// </summary>
        public string Mortgagelimitlimoney { get; set; }
        /// <summary>
        /// 流转额度
        /// </summary>
        public string Roamlitillimoney { get; set; }
    }
}