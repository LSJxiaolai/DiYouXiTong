//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DiYouQianTaiXiTong.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class B_UserCashRecordTable
    {
        public int UserCashID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> PayTypeID { get; set; }
        public string Subbranch { get; set; }
        public Nullable<int> ProID { get; set; }
        public Nullable<int> CityID { get; set; }
        public string CashAccountNumber { get; set; }
        public Nullable<decimal> CashAmount { get; set; }
        public Nullable<decimal> AccountMoney { get; set; }
        public string Poundage { get; set; }
        public Nullable<System.DateTime> CashTime { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string IP { get; set; }
        public string ExamineRemarks { get; set; }
    }
}
