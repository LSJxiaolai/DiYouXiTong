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
    
    public partial class B_LimitApplicationTable
    {
        public int LimitApplicationID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> LimitTypeID { get; set; }
        public Nullable<int> limitkindID { get; set; }
        public Nullable<int> limitoperateID { get; set; }
        public decimal ApplicationMoney { get; set; }
        public Nullable<decimal> PassLimit { get; set; }
        public string ApplicationRemark { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> ApplicationTime { get; set; }
        public Nullable<System.DateTime> ExamineTime { get; set; }
        public string LoanPurpose { get; set; }
        public string AuditingRemark { get; set; }
    }
}
