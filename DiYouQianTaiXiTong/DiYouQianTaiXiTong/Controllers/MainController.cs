using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiYouQianTaiXiTong.Models;
using DiYouQianTaiXiTong.Common;
using DiYouQianTaiXiTong.Vo;

namespace DiYouQianTaiXiTong.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        Models.DYXTEntities myModels = new DYXTEntities();
        //主页面
        public ActionResult Index()
        {
            try
            {

                string strUserId = Session["AccountID"].ToString();
                int intUserId = Convert.ToInt32(strUserId);
                B_AccountTable accouunt = (from tbAccountTable in myModels.B_AccountTable
                                           where tbAccountTable.AccountID == intUserId
                                           select tbAccountTable).Single();
                ViewBag.User = accouunt.User;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }

        // 注册页面
        public ActionResult ZhuCe()
        {
            return View();
        }
        //添加新用户、也就是注册
        public ActionResult InsertYongHu(B_AccountTable Account)
        {
            string str = "";
            try
            {
                int user = (from tbuser in myModels.B_AccountTable
                            where tbuser.User == Account.User
                            select tbuser).Count();
                if (user > 0)
                {
                    str = "Exist";
                }
                else
                {

                    string pw = AESEncryptHelper.AESEncrypt(Account.Password);
                    Account.Password = pw;
                    Account.Cancel = false;
                    myModels.B_AccountTable.Add(Account);
                    myModels.SaveChanges();
                    str = "success";
                }
            }
            catch (Exception e)
            {

            }

            return Json(str, JsonRequestBehavior.AllowGet);

        }
        //登录页面
        public ActionResult Login()
        {
            //使用ViewBag将数据返回
            string User = "";
            string Password = "";
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["User"];
            if (cookie != null)
            {
                if (cookie["User"] != null)
                {
                    User = System.Web.HttpContext.Current.Server.UrlDecode(cookie["User"]);
                }
                if (cookie["Password"] != null)
                {
                    Password = System.Web.HttpContext.Current.Server.UrlDecode(cookie["Password"]);
                }
            }
            ViewBag.UserNuber = User;
            ViewBag.Password = Password;
            return View();
        }

        //判断用户是否登录
        public ActionResult sesssinoselect()
        {

            if (Session["AccountID"] != null)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UserLogin(B_AccountTable Account)
        {

            string str = "";//
            string strUser = Request["username"];//用户名
            string strPassword = Request["password"];//密码
            string strvalidCode = Request["validCode"];//验证码
            string strSession = "";
            if (Session["vildeCode"] != null)
            {
                strSession = Session["vildeCode"].ToString();
                if (strSession.Equals(strvalidCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        var list = (from tbUser in myModels.B_AccountTable
                                    where tbUser.User == strUser.Trim()
                                    select new
                                    {
                                        tbUser.User,
                                        tbUser.Password,
                                        tbUser.AccountID
                                    }).Single();
                        string strpassword = AESEncryptHelper.AESEncrypt(strPassword);
                        if (strpassword == list.Password.Trim())
                        {

                            Session["AccountID"] = list.AccountID; // 传递 UserID  
                            Session["Password"] = list.Password;


                            var BLoginDetai = (from tblo in myModels.B_UserLoginDetailTable
                                               select new
                                               {
                                                   tblo.AccountID,
                                               }).ToList();
                            for (int i = 0; i < BLoginDetai.Count; i++)
                            {
                                if (BLoginDetai[i].AccountID == list.AccountID)
                                {
                                    B_UserLoginDetailTable BLoginss = (from tblogin in myModels.B_UserLoginDetailTable
                                                                       where tblogin.AccountID == list.AccountID
                                                                       select tblogin).Single();
                                    if (BLoginss.lastLoginTime == null || BLoginss.LoginFrequency == null)
                                    {
                                        BLoginss.RegisterTime = DateTime.Now;
                                        BLoginss.lastLoginTime = DateTime.Now;
                                        BLoginss.LoginFrequency = 1;
                                        BLoginss.EndLoginTime = DateTime.Now;
                                        myModels.Entry(BLoginss).State = System.Data.Entity.EntityState.Modified;
                                        myModels.SaveChanges();

                                    }
                                    else
                                    {
                                        BLoginss.lastLoginTime = BLoginss.EndLoginTime;
                                        BLoginss.LoginFrequency = BLoginss.LoginFrequency + 1;
                                        BLoginss.EndLoginTime = DateTime.Now;
                                        myModels.Entry(BLoginss).State = System.Data.Entity.EntityState.Modified;
                                        myModels.SaveChanges();

                                    }
                                    break;
                                }
                            }

                            str = "success";

                        }
                        else
                        {
                            str = "fail";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    str = "validCodeFail";
                }
            }
            else
            {
                str = "writevalidCode";
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

     
        public ActionResult ValideCode()
        {
            string strVildeCode = Common.ValidCodeUtils.GetRandomCode(4);//获取随机字符串
            Session["vildeCode"] = strVildeCode;//放入session
            byte[] vildeImage = Common.ValidCodeUtils.CreateImage(strVildeCode);
            return File(vildeImage, @"image/jpeg");
        }

        public ActionResult JudgeUser()
        {
            try
            {
                string str = "";
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                int B_uer = (from tbuser in myModels.B_UserTable
                             where tbuser.AccountID == accountID
                             select tbuser).Count();
                if (B_uer > 0)
                {
                    B_UserTable B_User = (from tbuser in myModels.B_UserTable
                                          where tbuser.AccountID == accountID
                                          select tbuser).Single();
                    if (B_User.StatusID == 24)
                    {
                        int B_limit = (from tblimit in myModels.B_LimitApplicationTable
                                       where tblimit.UserID == B_User.UserID
                                       select tblimit.UserID).Count();
                        if (B_limit > 0)
                        {
                            str = "GoLoan";
                        }
                        else
                        {
                            str = "fail";
                        }
                    }
                    else
                    {
                        str = "IDCardNoFail";
                    }

                }
                else
                {
                    str = "Nowrite";
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        #region 关于我们
        public ActionResult AboutOurIndex()
        {
            try
            {

                string strUserId = Session["AccountID"].ToString();
                int intUserId = Convert.ToInt32(strUserId);
                B_AccountTable accouunt = (from tbAccountTable in myModels.B_AccountTable
                                           where tbAccountTable.AccountID == intUserId
                                           select tbAccountTable).Single();
                ViewBag.User = accouunt.User;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }

        public ActionResult CertificateIndex()
        {
            try
            {

                string strUserId = Session["AccountID"].ToString();
                int intUserId = Convert.ToInt32(strUserId);
                B_AccountTable accouunt = (from tbAccountTable in myModels.B_AccountTable
                                           where tbAccountTable.AccountID == intUserId
                                           select tbAccountTable).Single();
                ViewBag.User = accouunt.User;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }
        #endregion

        #region 计算器
        // 计算器
        public ActionResult CounterIndex()
        {
            try
            {

                string strUserId = Session["AccountID"].ToString();
                int intUserId = Convert.ToInt32(strUserId);
                B_AccountTable accouunt = (from tbAccountTable in myModels.B_AccountTable
                                           where tbAccountTable.AccountID == intUserId
                                           select tbAccountTable).Single();
                ViewBag.User = accouunt.User;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }
        #endregion

        /// <summary>
        /// 查询最新五条借款标
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectLoanList()
        {
            try
            {
                List<LoanVo> satrlist = new List<LoanVo>();
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();
                int userid = list.UserID;
                var notict = (from tbloan in myModels.B_LoanTable
                              join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                              join tbLoanDeadline in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                              join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                              where tbloan.SurplusLoan != 0 && tbloan.UserID != userid && tbloan.StatusID == 7
                              orderby tbloan.LoanID descending
                              select new LoanVo
                              {
                                  LoanID = tbloan.LoanID,
                                  UserID = tbloan.UserID,
                                  Loantitle = tbloan.Loantitle,
                                  LoanMoney = tbloan.LoanMoney,
                                  Rate = tbloan.Rate,
                                  LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                  LowestTenderMoney = tbloan.LowestTenderMoney,
                                  SurplusLoan = tbloan.SurplusLoan,
                                  Scheduleinvestment = tbloan.Scheduleinvestment
                              }).ToList();

                for (int i = 0; i < 5; i++)
                {
                    if (notict.Count < 5)
                    {
                        satrlist = notict;
                        break;
                    }
                    satrlist.Add(notict[i]);

                }
                return Json(satrlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                List<LoanVo> satrlist = new List<LoanVo>();
                var notict = (from tbloan in myModels.B_LoanTable
                              join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                              join tbLoanDeadline in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                              join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                              where tbloan.SurplusLoan != 0 && tbloan.StatusID == 7
                              orderby tbloan.LoanID descending
                              select new LoanVo
                              {
                                  LoanID = tbloan.LoanID,
                                  UserID = tbloan.UserID,
                                  Loantitle = tbloan.Loantitle,
                                  LoanMoney = tbloan.LoanMoney,
                                  Rate = tbloan.Rate,
                                  LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                  LowestTenderMoney = tbloan.LowestTenderMoney,
                                  SurplusLoan = tbloan.SurplusLoan,
                                  Scheduleinvestment = tbloan.Scheduleinvestment
                              }).ToList();

                for (int i = 0; i < 5; i++)
                {
                    if (notict.Count < 5)
                    {
                        satrlist = notict;
                        break;
                    }
                    satrlist.Add(notict[i]);

                }
                return Json(satrlist, JsonRequestBehavior.AllowGet);

            }

        }

        /// <summary>
        /// 查询借款总额
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>

        public ActionResult SelectLoanMoney(BsgridPage bsgridPage)
        {
            try
            {
                var list = (from tbloan in myModels.B_LoanTable
                            select new LoanVo
                            {
                                LoanMoney = tbloan.LoanMoney
                            }).ToList();
                decimal ZongE = 0;
                decimal ZE = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    ZongE = Convert.ToDecimal(list[i].LoanMoney);
                    ZE += ZongE;
                }

                return Json(ZE.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 查询待回收总额
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectReturnMoney(BsgridPage bsgridPage)
        {
            try
            {
                var list = (from tbloan in myModels.B_LoanTable
                            select new LoanVo
                            {
                                ArrearagePrincipal = tbloan.ArrearagePrincipal
                            }).ToList();
                decimal zonge = 0;
                decimal ZE = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    zonge = Convert.ToDecimal(list[i].ArrearagePrincipal);
                    ZE += zonge;
                }
                return Json(ZE, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}