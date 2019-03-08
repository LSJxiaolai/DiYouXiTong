using DYXTHT_MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DYXTHT_MVC.Models;
using System.Net;
using DYXTHT_MVC.Vo;
using SpeechLib;
using System.Net.Sockets;

namespace DYXTHT_MVC.Controllers
{
    public class LoginMainController : Controller
    {
        Models.DYXTEntities myDYXTEntities = new DYXTEntities();
        // GET: LoginMain
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }     
        /// <summary>
        ///  主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Mainview()
        {
            
            try
            {
                string strUserId = Session["AccountID"].ToString();
                int intUserId = Convert.ToInt32(strUserId);
                B_AccountTable accouunt = (from tbAccountTable in myDYXTEntities.B_AccountTable
                                           where tbAccountTable.AccountID == intUserId
                                           select tbAccountTable).Single();
                ViewBag.User = accouunt.User;

                SpVoice speech = new SpVoice();//new一个
                int speechRate = -1; //语音朗读速度
                int volume = 100; //音量
                bool paused = false;//是否暂停
                string testspeech = "You are welcome to use," + accouunt.User; //测试朗读内容
                if (paused)
                {
                    speech.Resume();
                    paused = false;
                }
                else
                {
                    speech.Rate =speechRate;
                    speech.Volume = volume;
                    speech.Speak(testspeech, SpeechVoiceSpeakFlags.SVSFlagsAsync);//开始语音朗读
                }

                return View();
              
            }
            catch (Exception e)
            {

                return Redirect("/LoginMain/Login");
            }
        }
        /// <summary>
        /// 留言
        /// </summary>
        /// <returns></returns>
        public ActionResult sccussoselect()
        {
            try
            {              
                var varWebLiuYan = (from tbWebLiuYan in myDYXTEntities.B_WebLiuYan
                                            join tbaccouunt in myDYXTEntities.B_AccountTable on tbWebLiuYan.AccountID equals tbaccouunt.AccountID
                                            orderby tbWebLiuYan.WebliuyanID descending
                                            select new  B_WebLiuYanVo 
                                           {
                                               WebliuyanID = tbWebLiuYan.WebliuyanID,
                                               User=tbaccouunt.User,
                                               ReleaseTimeStr = tbWebLiuYan.LiuYanTime.ToString(),
                                               Content=tbWebLiuYan.Content,
                                                MailBox= tbWebLiuYan.MailBox,
                                                QQ=  tbWebLiuYan.QQ,
                                                Callnumber= tbWebLiuYan.Callnumber
                                            }
                                         ).ToList();
                return Json(varWebLiuYan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }        

        public ActionResult Shouye() {
            return View();
        }
        public ActionResult UpdataMima()
        {
            return View();
        }
        public ActionResult UserLogin(B_AccountTable B_Account)
        {
            string strMed = "fail";
            string struser = Request["User"]; ;//用户
            string strPassword = Request["password"];
            string strvalidCode = Request["validCode"];//验证码
            string strSessionValidCode = ""; //获取 session中的验证码                                             
            try
            {
                strSessionValidCode = Session["ValidCode"].ToString();
            }
            catch (Exception)
            {
                return Json("loginerror", JsonRequestBehavior.AllowGet);
            }
            //判断验证码
            if (strSessionValidCode.Equals(strvalidCode.Trim(), StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    //根据 UserNuber 查询用户
                    var dbUser = (from tbUser in myDYXTEntities.B_AccountTable
                                  where tbUser.User == struser.Trim()
                                  select new
                                  {
                                      tbUser.AccountID,
                                      tbUser.User,
                                      tbUser.Password
                                  }).Single();
                    //将用户输入的密码进行AES265后与数据库中的密码对比
                    string password = AESEncryptHelper.AESEncrypt(strPassword);
                    if (dbUser.Password.Trim().Equals(password))
                    {
                        B_UserTable dbuser = (from tbuser in myDYXTEntities.B_UserTable
                                              where tbuser.AccountID == dbUser.AccountID
                                              select tbuser).Single();
                        if (dbuser.UserTypeID == 1|| dbuser.UserTypeID==4|| dbuser.UserTypeID==12)
                        {
                            Session["AccountID"] = dbUser.AccountID;

                            B_UserTable varFundCost = (from tbuser in myDYXTEntities.B_UserTable
                                                       where tbuser.AccountID == dbUser.AccountID
                                                       select tbuser).Single();
                            B_ManagerRecord ManagerRecord = new B_ManagerRecord();
                            ManagerRecord.UserID = varFundCost.UserID;
                            ManagerRecord.BearFruit = "成功".Trim();
                            ManagerRecord.Content = "用户" + varFundCost.UserName.Trim() + "在“" + DateTime.Now + "”登录后台";
                            ManagerRecord.LoginTime = DateTime.Now;
                            string IP = "";
                            try
                            {

                                string pHostName = Dns.GetHostName();//Dns类


                                IPHostEntry myAddress = Dns.GetHostEntry(pHostName);
                                //myAddress.Aliases
                                IPAddress[] myIPAddress = myAddress.AddressList;

                                foreach (IPAddress add in myIPAddress)
                                {
                                    if (add.AddressFamily == AddressFamily.InterNetwork)
                                    {
                                        IP = add.ToString();
                                    }
                                }

                                   

                            }
                            catch (Exception e)
                            {
                               
                            }
                            ManagerRecord.LoginIP = IP;
                            myDYXTEntities.B_ManagerRecord.Add(ManagerRecord);
                            myDYXTEntities.SaveChanges();

                            strMed = "strsuccess";//登录成功
                        }
                        else
                        {
                            strMed = "strfail";//登录成功
                        }
                    }
                    else
                    {
                        strMed = "strpassword";//密码错误
                    }
                }
                catch (Exception e)
                {

                    strMed = "usernoexsit";//没有此用户
                }
            }
            else
            {
                strMed = "ValidCodeErro";//验证码错误
            }
            return Json(strMed, JsonRequestBehavior.AllowGet);
        }

        public ActionResult updataPasswords(string YPassword, string QPassword)
        {
            string strMsg = "fali";
            try
            {
                int yAccountID = Convert.ToInt32(Session["AccountID"]);
                int dbUser = (from tbUser in myDYXTEntities.B_AccountTable
                              where tbUser.AccountID == yAccountID
                              select tbUser).Count();
                if (dbUser > 0)
                {
                    var varuser = (from tbUser in myDYXTEntities.B_AccountTable
                                   where tbUser.AccountID == yAccountID
                                   select new
                                   {
                                       tbUser.AccountID,
                                       tbUser.User,
                                       tbUser.Password
                                   }).Single();
                    //将用户输入的密码进行AES265后与数据库中的密码对比
                    string password = AESEncryptHelper.AESEncrypt(YPassword);
                    if (varuser.Password.Trim().Equals(password))
                    {
                        var struser = (from tbUser in myDYXTEntities.B_AccountTable
                                       where tbUser.AccountID == yAccountID
                                       select tbUser).Single();
                        string qPassword = AESEncryptHelper.AESEncrypt(QPassword);
                        struser.Password = qPassword;
                        myDYXTEntities.Entry(struser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }
                    else
                    {
                        strMsg = "strpassword";//密码错误
                    }
                }
                else
                {
                    strMsg = "nonexistent";
                }

            }
            catch (Exception e)
            {
                
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <returns></returns>
        public ActionResult validcode()
        {
            string strRandon = Common.ValidCodeUtils.GetRandomCode(4);//生成一个随机字符串 验证码
            Session["validcode"] = strRandon;//将验证码放入Session
            byte[] robyty = ValidCodeUtils.CreateImage(strRandon);//byte[] 根据验证码生成图片
            return File(robyty, @"image/jpeg");
        }

        public ActionResult selectjiekuanfullscale()
        {
            try
            {
                //判断VIP用户是否到期
                var vipUser = (from tbVIPuser in myDYXTEntities.B_VIPUserTable
                               select tbVIPuser).ToList();
                for (int i = 0; i < vipUser.Count; i++)
                {
                    DateTime nowTime = DateTime.Now;//当前时间
                    DateTime dtTime = Convert.ToDateTime(vipUser[i].EndTime);//结束时间
                    if (nowTime> dtTime)
                    {
                        int intuserID = Convert.ToInt32(vipUser[i].UserID);
                        B_UserTable varsuer = (from tbUser in myDYXTEntities.B_UserTable
                                               where tbUser.UserID == intuserID
                                               select tbUser).Single();
                        varsuer.UserTypeID = 2;
                        myDYXTEntities.Entry(varsuer).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                    }
                }


                var varstrloan = (from tbLoan in myDYXTEntities.B_LoanTable                              
                               where tbLoan.StatusID == 7 || tbLoan.StatusID == 8|| tbLoan.StatusID==9
                                  select tbLoan).ToList();           
                try
                {
                    for (int i = 0; i < varstrloan.Count(); i++)
                    {
                        int intloanID = varstrloan[i].LoanID;
                        B_LoanTable varloanll = (from tbloan in myDYXTEntities.B_LoanTable
                                                 where tbloan.LoanID == intloanID
                                                 select tbloan).Single();
                        DateTime loantime = DateTime.Now;//获取当前时间
                        DateTime valtiem = Convert.ToDateTime(varstrloan[i].Endtime);//获取到期是时间
                        DateTime Raistime = Convert.ToDateTime(varstrloan[i].Raisestandard);//获取筹标时间
                        DateTime varsichuantime = Convert.ToDateTime(varstrloan[i].sichuantime);//过期时间
                     
                        if (Raistime >= loantime && varstrloan[i].Scheduleinvestment == Convert.ToDecimal("100"))
                        {
                            varloanll.StatusID = 28;//满标待审
                            myDYXTEntities.Entry(varloanll).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();
                        }
                        else if (loantime > Raistime)
                        {
                            varloanll.StatusID = 9;//已过期
                            varloanll.sichuantime = varloanll.Raisestandard.Value.AddDays(3);
                            myDYXTEntities.Entry(varloanll).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();
                        }
                        if (varloanll.StatusID == 9 && loantime > varsichuantime && varstrloan[i].Scheduleinvestment < Convert.ToDecimal("100"))
                        {

                            varloanll.StatusID = 26;//流标
                            varloanll.RemoveTreetopTime = DateTime.Now;
                            myDYXTEntities.Entry(varloanll).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();

                            var varinves = (from tbinvest in myDYXTEntities.B_InvestTable
                                            where tbinvest.LoanID == varloanll.LoanID
                                            select tbinvest).ToList();
                            for (int j = 0; j < varinves.Count; j++)
                            {
                                int VarUserID = Convert.ToInt32(varinves[j].UserID);
                                B_UserTable varsuer = (from tbUser in myDYXTEntities.B_UserTable
                                                       where tbUser.UserID == VarUserID
                                                       select tbUser).Single();
                                varsuer.PropertyAmounts = varsuer.PropertyAmounts + varinves[j].InvestMoney;
                                varsuer.UsableMoney = varsuer.UsableMoney + varinves[j].InvestMoney;
                                myDYXTEntities.Entry(varsuer).State = System.Data.Entity.EntityState.Modified;
                                myDYXTEntities.SaveChanges();
                            }

                        }
                    }
                }
                catch (Exception e)
                {

                }


                //判断借款标是否逾期
                var varloan = (from tbLoan in myDYXTEntities.B_LoanTable                        
                          where tbLoan.StatusID == 10 || tbLoan.StatusID == 8 || tbLoan.StatusID == 11|| tbLoan.StatusID==34
                           select tbLoan).ToList();

            for (int i = 0; i < varloan.Count; i++)
            {
                DateTime nowTime = DateTime.Now;//当前时间
                DateTime dtTime =Convert.ToDateTime(varloan[i].Endtime);//结束时间
               
                if (nowTime> dtTime&& varloan[i].StatusID != 34)
                {
                    TimeSpan YuQitianshu =nowTime - dtTime;
                    double YQTS = YuQitianshu.TotalDays;
                    int yqts = Convert.ToInt32(YQTS);//逾期天数
                    decimal varrete = Convert.ToDecimal(varloan[i].Rate)*Convert.ToDecimal(0.01);
                    decimal demoney=Convert.ToDecimal(varloan[i].LoanMoney) * varrete;//利息
                    decimal strfx =Convert.ToDecimal(varloan[i].LoanMoney) * Convert.ToDecimal(0.008)* yqts;//罚息
                    B_OverdueLoanTable strOverdueLoan = new B_OverdueLoanTable();
                    strOverdueLoan.LoanID = varloan[i].LoanID;
                    strOverdueLoan.PayableTime = varloan[i].Endtime;
                    strOverdueLoan.OverdueDay = yqts;
                    strOverdueLoan.PayablePrincipal = varloan[i].LoanMoney + demoney + strfx;
                    strOverdueLoan.WebsiteWhetherPayStatusID = 33;
                    strOverdueLoan.StatusID = 31;
                    strOverdueLoan.RealityRepaymentTime= varloan[i].Endtime;
                    myDYXTEntities.B_OverdueLoanTable.Add(strOverdueLoan);                  
                    myDYXTEntities.SaveChanges();

                    int intloan =Convert.ToInt32(varloan[i].LoanID);
                    var varloanID = (from tbLoan in myDYXTEntities.B_LoanTable
                                     where tbLoan.LoanID == intloan
                                     select tbLoan).Single();
                    varloanID.StatusID = 34;
                    varloanID.RepayPrincipal = strOverdueLoan.PayablePrincipal;
                    varloanID.ArrearagePrincipal= strOverdueLoan.PayablePrincipal;
                    myDYXTEntities.Entry(varloanID).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();
                }
                else if(nowTime > dtTime && varloan[i].StatusID == 34)
                {                 
                    int intloan = Convert.ToInt32(varloan[i].LoanID);
                    B_OverdueLoanTable strOverdueLoan = (from tbOverdueLoan in myDYXTEntities.B_OverdueLoanTable
                                                        where tbOverdueLoan.LoanID == intloan
                                                        select tbOverdueLoan).Single();
                    TimeSpan YuQitianshu = nowTime - dtTime;
                    double YQTS = YuQitianshu.TotalDays;
                    int yqts = Convert.ToInt32(YQTS);//逾期天数

                    if (strOverdueLoan.OverdueDay!=yqts)
                    {
                        decimal varrete = Convert.ToDecimal(varloan[i].Rate) * Convert.ToDecimal(0.01);
                        decimal demoney = Convert.ToDecimal(varloan[i].LoanMoney) * varrete;//利息
                        decimal strfx = Convert.ToDecimal(varloan[i].LoanMoney) * Convert.ToDecimal(0.008) * yqts;//罚息                                                                                                                             
                        strOverdueLoan.OverdueDay = yqts;
                        strOverdueLoan.PayablePrincipal = strOverdueLoan.PayablePrincipal + demoney + strfx;
                        myDYXTEntities.Entry(strOverdueLoan).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        int intloans = Convert.ToInt32(varloan[i].LoanID);
                        var varloanID = (from tbLoan in myDYXTEntities.B_LoanTable
                                         where tbLoan.LoanID == intloans
                                         select tbLoan).Single();                  
                        varloanID.RepayPrincipal = strOverdueLoan.PayablePrincipal;
                        varloanID.ArrearagePrincipal = strOverdueLoan.PayablePrincipal;
                        myDYXTEntities.Entry(varloanID).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();

                        try
                        {                       
                        B_WebsiteReceivableDetail varWebsiteReceivableDetail = (from tbWebsiteReceivableDetail in myDYXTEntities.B_WebsiteReceivableDetail
                                                                                where tbWebsiteReceivableDetail.OverdueLoanID == strOverdueLoan.OverdueLoanID
                                                                                select tbWebsiteReceivableDetail).Single();
                        varWebsiteReceivableDetail.ReceivableAccrual = demoney + strfx;
                        varWebsiteReceivableDetail.OverdueDays = yqts;
                        myDYXTEntities.Entry(varWebsiteReceivableDetail).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        }
                        catch (Exception e)
                        {                     
                        }
                    }                  
                }

            }
            }
            catch (Exception e)
            {
      
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }


        #region 访问权限控制模块

        //模块权限查询 By 用户ID
        public ActionResult MoKuaiQuanXianSelect(int AccountID)
        {
            var caozuoid = (from tdAccount in myDYXTEntities.B_AccountTable
                            join tdUser in myDYXTEntities.B_UserTable on tdAccount.AccountID equals tdUser.AccountID
                            join tdJurisdiction in myDYXTEntities.B_Jurisdictio on tdUser.UserTypeID equals tdJurisdiction.UserTypeID
                            join tdModularDetail in myDYXTEntities.S_ModularDetailITable on tdJurisdiction.ModularDetailID equals tdModularDetail.ModularDetailID
                            join tdModular in myDYXTEntities.B_ModuleTable on tdModularDetail.ModuleID equals tdModular.ModuleID
                            where tdAccount.AccountID == AccountID
                            select new
                            {
                                ID = tdModularDetail.ModuleID,
                                Name = tdModular.Name.Trim()
                            }).ToList();

            return Json(caozuoid, JsonRequestBehavior.AllowGet);
        }


        #region 权限访问
        public ActionResult MoKuaiQuanXianOpTypeSelect()
        {
            int AccountID = Convert.ToInt32(Session["AccountID"]);
            var caozuoid = (from tdAccount in myDYXTEntities.B_AccountTable
                            join tdUser in myDYXTEntities.B_UserTable on tdAccount.AccountID equals tdUser.AccountID
                            join tdJurisdiction in myDYXTEntities.B_Jurisdictio on tdUser.UserTypeID equals tdJurisdiction.UserTypeID
                            join tdModularDetail in myDYXTEntities.S_ModularDetailITable on tdJurisdiction.ModularDetailID equals tdModularDetail.ModularDetailID
                            join tdModular in myDYXTEntities.B_ModuleTable on tdModularDetail.ModuleID equals tdModular.ModuleID
                            join tboptype in myDYXTEntities.S_OpTypeTable on tdModularDetail.OpTypeID equals tboptype.OpTypeID
                            where tdAccount.AccountID == AccountID
                            select new
                            {
                                ID = tdModularDetail.ModuleID,
                                Name = tdModular.Name.Trim(),
                                Optype = tboptype.Optype.Trim(),
                            }).ToList();
            return Json(caozuoid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        //查询操作类型 by  模块ID and 用户类型ID (发布公告为例子)
        //public ActionResult OpTypeSelect(int ModularID)
        //{
        //    //获取登陆时保存的session值
        //    int UserTypeID = Convert.ToInt32(Session["UserTypeID"]);
        //    // 查询操作类型 by  模块ID and 用户类型ID
        //    var caozuoid = (from tdJurisdiction in myModels.PW_Jurisdiction
        //                    join tdModularDetail in myModels.SYS_ModularDetail on tdJurisdiction.ModularDetailID equals tdModularDetail.ModularDetailID
        //                    join tdOpType in myModels.SYS_OpType on tdModularDetail.OpTypeID equals tdOpType.OpTypeID
        //                    where tdModularDetail.ModularID == ModularID && tdJurisdiction.UserTypeID == UserTypeID
        //                    select new
        //                    {
        //                        Explain = tdOpType.Explain.Trim()
        //                    }).ToList();
        //    int sum = caozuoid.Count();
        //    return Json(caozuoid, JsonRequestBehavior.AllowGet);
        //}

        #endregion
    }
}