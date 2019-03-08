using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MYSalesSystem.Common
{
    public class IdCardValidatorUtil
    {
        /**
                                   居民身份证最后一位数字的计算方法 
            身份证前17数字每个数字对应的系数 7 9 10 5 8 4 2  1 6 3 7 9 10 5 8 4  2  
            将前17位数字与对应的系数相乘，其结果相加，其和除以11.余数只可能是如下11个数字，  
            余数依次为      0  1  2   3  4  5  6  7  8  9  10 
            余数对应的数字  1  0  Ⅹ  9  8  7  6  5  4  3  2  
            余数对应的数字即为身份证最后一位，其中余数2对应的数字 Ⅹ 为罗马字母,代表10。
            注：Ⅹ 为罗马字母代表10，这样满足身份证为18位数字组成的国际标准。

            验证方式：
            将18位的数字与对应的系数相乘，其结果相加，其和除以11，余数为1的为合法的身份证号。
        */

        //地区数组
        private static readonly string[] StrProvinceS = { null, null, null, null, null, null, null, null, null, null, null, " 北京 ", " 天津 ", " 河北 ", " 山西 ", " 内蒙古 ", null, null, null, null, null, " 辽宁 ", " 吉林 ", " 黑龙江 ", null, null, null, null, null, null, null, " 上海 ", " 江苏 ", " 浙江 ", " 安微 ", " 福建 ", " 江西 ", " 山东 ", null, null, null, " 河南 ", " 湖北 ", " 湖南 ", " 广东 ", " 广西 ", " 海南 ", null, null, null, " 重庆 ", " 四川 ", " 贵州 ", " 云南 ", " 西藏 ", null, null, null, null, null, null, " 陕西 ", " 甘肃 ", " 青海 ", " 宁夏 ", " 新疆 ", null, null, null, null, null, " 台湾 ", null, null, null, null, null, null, null, null, null, " 香港 ", " 澳门 ", null, null, null, null, null, null, null, null, " 国外 " };

        /// <summary>
        ///验证单个身份证号码
        /// </summary>
        /// <param name="strIdCard">身份证号码</param>
        /// <returns></returns>
        public static bool CheckIdCardSign(string strIdCard)
        {
            //转为小写 主要是X结尾的情况
            strIdCard = strIdCard.ToLower();

            int iSum = 0;
            //正则表达式验证身份证的位数 前17位数字 第18位可能是数字可能是x
            Regex rg = new Regex(@"^\d{17}(\d|x)$");
            if (!rg.IsMatch(strIdCard))
            {
                return false;
            }
            
            //匹配地区 前两位
            if (StrProvinceS[int.Parse(strIdCard.Substring(0, 2))] == null)
            {
                return false;
            }

            //获取出生日期
            try
            {
                string strDateTime = strIdCard.Substring(6, 4) + "-" + strIdCard.Substring(10, 2) + "-" +
                                     strIdCard.Substring(12, 2);
            }
            catch
            {
                return false;
            }
            
            strIdCard = strIdCard.Replace("x", "a");
            //验证校验位
            for (int i = 0; i < 18; i++)
            {
                //加权因子
                int wi = Convert.ToInt32(Math.Pow(2, 18 - i - 1)) % 11;
                //这个位置对应的值
                int w = int.Parse(strIdCard[i].ToString().Trim(), NumberStyles.HexNumber);
                //计算权求和
                iSum += wi * w;
            }
            //计算最后一位的值
            int intLastNum = iSum % 11;
            if (intLastNum!=1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取单个身份证号码信息
        /// </summary>
        /// <param name="strIdCard"></param>
        /// <returns></returns>
        public static string GetIdCardSignInfo(string strIdCard)
        {
            CheckIdCardSign(strIdCard);
            double iSum = 0;
            Regex rg = new Regex(@"^\d{17}(\d|x)$");
            if (!rg.IsMatch(strIdCard))
            {
                return "非法身份证号码";
            }
            strIdCard = strIdCard.ToLower();
            strIdCard = strIdCard.Replace("x", "a");
            if (StrProvinceS[int.Parse(strIdCard.Substring(0, 2))] == null)
            {
                return " 非法地区";
            }
            try
            {
                var dateTime = DateTime.Parse(strIdCard.Substring(6, 4) + "-" + strIdCard.Substring(10, 2) + "-" + strIdCard.Substring(12, 2));
            }
            catch
            {
                return " 非法生日 ";
            }
            for (int i = 17; i >= 0; i--)
            {
                iSum += Math.Pow(2, i) % 11 * int.Parse(strIdCard[17 - i].ToString(), NumberStyles.HexNumber);

            }
            if (iSum % 11 != 1)
                return (" 非法证号 ");

            return (StrProvinceS[int.Parse(strIdCard.Substring(0, 2))] + "," + strIdCard.Substring(6, 4) + "-" + strIdCard.Substring(10, 2) + "-" + strIdCard.Substring(12, 2) + "," + (int.Parse(strIdCard.Substring(16, 1)) % 2 == 1 ? " 男 " : " 女 "));

        }
    }
}
