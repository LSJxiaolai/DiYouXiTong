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
    
    public partial class B_WebsiteReceivableDetail
    {
        public int WebsiteReceivableDetailID { get; set; }
        public Nullable<int> OverdueLoanID { get; set; }
        public string Loantitle { get; set; }
        public Nullable<System.DateTime> ReceivableDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<decimal> GatheringMoney { get; set; }
        public Nullable<decimal> ReceivableCapital { get; set; }
        public Nullable<decimal> ReceivableAccrual { get; set; }
        public Nullable<int> OverdueDays { get; set; }
        public Nullable<System.DateTime> RealityTime { get; set; }
        public Nullable<decimal> RealityAmount { get; set; }
        public Nullable<int> StatusID { get; set; }
    }
}
