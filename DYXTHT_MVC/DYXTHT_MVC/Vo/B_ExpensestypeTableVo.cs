using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_ExpensestypeTableVo: B_ExpensestypeTable
    {
        public string UserTypeName { get; set; }

        public string FundStatusName { get; set; }

        public string LoanexpensesName { get; set; }
    }
}