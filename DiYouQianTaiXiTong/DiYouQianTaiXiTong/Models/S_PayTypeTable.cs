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
    
    public partial class S_PayTypeTable
    {
        public int PayTypeID { get; set; }
        public byte[] PayTypePicture { get; set; }
        public string PayTypeName { get; set; }
        public string PaySummary { get; set; }
        public Nullable<bool> OpenDeny { get; set; }
        public Nullable<bool> AddDeny { get; set; }
    }
}
