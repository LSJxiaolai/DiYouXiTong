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
    
    public partial class B_UserExpensesTable
    {
        public int UserExpensesID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> OperateTypeID { get; set; }
        public Nullable<decimal> OperateMoney { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> Earning { get; set; }
        public Nullable<decimal> Expenses { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> OperateTime { get; set; }
    }
}