using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DYXTHT_MVC.Models;
namespace DYXTHT_MVC.Vo
{
    public class B_MaterialAttestationVo: B_MaterialAttestationTable
    {
        public string UserName { get; set; }
        public string ItemTypeName { get; set; }
        public string StatusName { get; set; }
        private string releaseTimeStr;
        public string ReleaseTimeStr
        {
            set
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(value);
                    releaseTimeStr = dt.ToString("yyyy/MM/dd  HH:mm:ss");
                }
                catch (Exception)
                {

                    releaseTimeStr = value;
                }
            }

            get
            {
                return releaseTimeStr;
            }
        }
    }
}