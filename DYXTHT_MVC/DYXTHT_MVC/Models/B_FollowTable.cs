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
    
    public partial class B_FollowTable
    {
        public int FollowID { get; set; }
        public Nullable<int> LoanID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Title { get; set; }
        public Nullable<decimal> Money { get; set; }
        public string YearRate { get; set; }
        public string Deadline { get; set; }
    }
}
