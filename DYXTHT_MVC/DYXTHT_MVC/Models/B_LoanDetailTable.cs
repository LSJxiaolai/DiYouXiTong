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
    
    public partial class B_LoanDetailTable
    {
        public int LoanDetailID { get; set; }
        public Nullable<int> LoanID { get; set; }
        public Nullable<int> ExchangeTreetopID { get; set; }
        public Nullable<int> OverdueLoanID { get; set; }
        public Nullable<int> CreditorAttornID { get; set; }
        public Nullable<decimal> RepayPrincipal { get; set; }
        public Nullable<decimal> AlreadyPrincipal { get; set; }
        public Nullable<decimal> ArrearagePrincipal { get; set; }
        public string SeveralIssues { get; set; }
        public Nullable<System.DateTime> PayableDate { get; set; }
        public Nullable<decimal> PayableInterest { get; set; }
        public string OverdueDay { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<decimal> CurrentCouponRepay { get; set; }
    }
}
