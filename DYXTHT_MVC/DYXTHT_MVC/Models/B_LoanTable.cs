//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DYXTHT_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class B_LoanTable
    {
        public int LoanID { get; set; }
        public string PaymentNumber { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Loantitle { get; set; }
        public string LoanPurpose { get; set; }
        public Nullable<decimal> LoanMoney { get; set; }
        public Nullable<decimal> ArrearagePrincipal { get; set; }
        public Nullable<decimal> PayableInterest { get; set; }
        public Nullable<decimal> AlreadyPrincipal { get; set; }
        public Nullable<decimal> RepayPrincipal { get; set; }
        public string LoanPeriods { get; set; }
        public Nullable<decimal> AlreadyLoanMoney { get; set; }
        public string Rate { get; set; }
        public Nullable<int> TreetoptypeID { get; set; }
        public Nullable<int> RepaymentWayID { get; set; }
        public string LoanDescribe { get; set; }
        public string ManageRemark { get; set; }
        public Nullable<System.DateTime> RemoveTreetopTime { get; set; }
        public Nullable<System.DateTime> Endtime { get; set; }
        public Nullable<System.DateTime> SubmitTime { get; set; }
        public Nullable<System.DateTime> sichuantime { get; set; }
        public Nullable<int> LoanDeadlineID { get; set; }
        public string recallaccount { get; set; }
        public string InvestTime { get; set; }
        public string Raisestandardday { get; set; }
        public Nullable<System.DateTime> Raisestandard { get; set; }
        public Nullable<decimal> Scheduleinvestment { get; set; }
        public Nullable<decimal> AlreadyLoan { get; set; }
        public Nullable<decimal> SurplusLoan { get; set; }
        public Nullable<decimal> stopbuyback { get; set; }
        public Nullable<decimal> Grossscore { get; set; }
        public Nullable<decimal> LowestTenderMoney { get; set; }
        public Nullable<decimal> HighestTenderMoney { get; set; }
        public Nullable<int> StatusID { get; set; }
    }
}
