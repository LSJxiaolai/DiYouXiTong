using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiYouQianTaiXiTong.Vo
{
    public class Bsgrid<T>
    {
        public int curPage { get; set; }
        public List<T> data { get; set; }
        public bool success { get; set; }
        public int totalRows { get; set; }
    }
}