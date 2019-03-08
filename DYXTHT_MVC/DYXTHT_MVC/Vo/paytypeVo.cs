using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class paytypeVo:S_PayTypeTable
    {
        public byte[] PayTypePicture { get; set; }
    }
}