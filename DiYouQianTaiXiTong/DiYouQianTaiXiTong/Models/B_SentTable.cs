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
    
    public partial class B_SentTable
    {
        public int SentID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Cellnumber { get; set; }
        public Nullable<int> SendstatusID { get; set; }
        public Nullable<System.DateTime> Timeofdeparture { get; set; }
        public string DepartureContent { get; set; }
    }
}