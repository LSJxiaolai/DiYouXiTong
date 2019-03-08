using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_phoneauthenticationVo:B_phoneauthenticationTable
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
       public string StatusName { get; set; }
       /// <summary>
       /// 添加时间
       /// </summary>
        public string addtime { get; set; }
        /// <summary>
        ///通过时间
        /// </summary>
        public string transitTime { get; set; }
    }
}