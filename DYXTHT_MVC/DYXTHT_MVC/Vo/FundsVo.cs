using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;

namespace DYXTHT_MVC.Vo
{
    public class FundsVo:B_UserTable
    {
        public string PropertyAmountsmoney { set; get; }
        public string PropertyUsableMoney { set; get; }
        public string PropertyFreezeMoney { set; get; }
        public string PropertyWaitMoney { set; get; }
        public string PropertyCompensatoryMoney { set; get; }
    }
}