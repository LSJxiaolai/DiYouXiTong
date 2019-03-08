using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiYouQianTaiXiTong.Models;
using DiYouQianTaiXiTong.Vo;
using DiYouQianTaiXiTong.Common;

namespace DiYouQianTaiXiTong.Areas.Loan.Controllers
{
    public class LoanController : Controller
    {
        // GET: Loan/Loan
        Models.DYXTEntities myModels = new DYXTEntities();
       
        //借款页面
        public ActionResult BorrowIndex()
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

        #region 信用标
        //信用借款标页面
        public ActionResult CreditIndex()
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
       
        //还款方式
        public ActionResult RePaymentWay()
        {
            List<SelectVo> listType = new List<SelectVo>();
            SelectVo selectvo = new SelectVo
            {
                id = 0,
                text = "--请选择--"
            };
            listType.Add(selectvo);
            List<SelectVo> listTypeS = (from tbType in myModels.S_RepaymentWayTable
                                        select new SelectVo
                                        {
                                            id = tbType.RepaymentWayID,
                                            text = tbType.RepaymentWayName,
                                        }).ToList();
            listType.AddRange(listTypeS);
            return Json(listType, JsonRequestBehavior.AllowGet);
        }

        //借款期限
        public ActionResult SelectLoanDeadline()
        {
            List<SelectVo> listType = new List<SelectVo>();
            SelectVo selectvo = new SelectVo
            {
                id = 0,
                text = "--请选择--"
            };
            listType.Add(selectvo);
            List<SelectVo> listTypeS = (from tbType in myModels.S_LoanDeadlineTable
                                        select new SelectVo
                                        {
                                            id = tbType.LoanDeadlineID,
                                            text = tbType.LoanDeadlineName,
                                        }).ToList();
            listType.AddRange(listTypeS);
            return Json(listType, JsonRequestBehavior.AllowGet);
        }
        //新增信用借款标
        public ActionResult InsertRredit(B_LoanTable B_Loan, string ChouDeadline)
        {
            string str = "";
            try
            {

                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                B_UserlimitTable Blimit = (from tblimit in myModels.B_UserlimitTable
                                           where tblimit.UserID == list.UserID
                                           select tblimit).Single();


                string strCurrentCode = "";//定义当前的编号
                var listLoan = (from tbloan in myModels.B_LoanTable
                                orderby tbloan.PaymentNumber
                                select tbloan).ToList();
                if (listLoan.Count > 0)//判断表中是否有数据
                {
                    int Count = listLoan.Count;//获取列表中的总数据
                    B_LoanTable myLoan = listLoan[Count - 1];//获取最后一个编号
                    int intCode = Convert.ToInt32(myLoan.PaymentNumber.Substring(8, 5));
                    intCode++;
                    strCurrentCode = intCode.ToString();
                    for (int i = 0; i < 5; i++)
                    {
                        strCurrentCode = strCurrentCode.Length < 5 ? "0" + strCurrentCode : strCurrentCode;//三目运算符
                    }
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + strCurrentCode;
                }
                else
                {
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "00001";//如果count=0,科室编码strCurrentCode从D0001开始
                }

                B_Loan.PaymentNumber = strCurrentCode;//贷款号
                B_Loan.SubmitTime = DateTime.Now;//提交时间
                B_Loan.AlreadyLoanMoney = B_Loan.LoanMoney;//已借金额

                decimal rate;
                rate = Convert.ToDecimal(B_Loan.Rate);
                B_Loan.PayableInterest = (B_Loan.LoanMoney * rate) / 100;//应还利息

                B_Loan.RepayPrincipal = B_Loan.PayableInterest + B_Loan.LoanMoney;//偿还本息

                B_Loan.ArrearagePrincipal = B_Loan.RepayPrincipal;//未还本息

                B_Loan.AlreadyPrincipal = Convert.ToDecimal(0.00);//已还本息
                B_Loan.Raisestandardday = ChouDeadline;
                
                B_Loan.Grossscore = B_Loan.AlreadyLoanMoney / B_Loan.LowestTenderMoney;//总份数
                B_Loan.SurplusLoan = B_Loan.Grossscore;//剩余份数
                B_Loan.AlreadyLoan = Convert.ToDecimal(0);//已借份数
                B_Loan.stopbuyback = Convert.ToDecimal(0);
                B_Loan.Scheduleinvestment = 0;//进度
                                              //B_Loan.LoanPeriods = Convert.ToInt32(B_Loan.LoanDeadlineID).ToString();
                                              //借款期数的判断

                if (B_Loan.LoanDeadlineID ==1)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 2)
                {
                    B_Loan.LoanPeriods = "2";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 3)
                {
                    B_Loan.LoanPeriods = "3";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 4)
                {
                    B_Loan.LoanPeriods = "4";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 5)
                {
                    B_Loan.LoanPeriods = "5";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 6)
                {
                    B_Loan.LoanPeriods = "6";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 7)
                {
                    B_Loan.LoanPeriods = "7";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 8)
                {
                    B_Loan.LoanPeriods = "8";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 9)
                {
                    B_Loan.LoanPeriods = "9";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 10)
                {
                    B_Loan.LoanPeriods = "10";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 11)
                {
                    B_Loan.LoanPeriods = "11";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 12)
                {
                    B_Loan.LoanPeriods = "12";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID > 12)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }

                //获取验证码
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (B_Loan.LoanMoney <= Blimit.Cavailable)//判断借款额度是否大于可用额度
                        {
                            if (B_Loan.RepaymentWayID != null && B_Loan.LoanDeadlineID != null)
                            {

                                B_Loan.UserID = list.UserID;
                                B_Loan.StatusID = 1;
                                B_Loan.TreetoptypeID = 3;
                                B_Loan.InvestTime = "0";
                                myModels.B_LoanTable.Add(B_Loan);
                                myModels.SaveChanges();

                                B_UserlimitTable Nlimit = (from tlimit in myModels.B_UserlimitTable
                                                           where tlimit.UserID == list.UserID
                                                           select tlimit).Single();
                                Nlimit.Cavailable = Nlimit.Cavailable - B_Loan.LoanMoney;//可用金额
                                Nlimit.Cfreeze = Nlimit.Cfreeze + B_Loan.LoanMoney;//冻结金额
                                myModels.Entry(Nlimit).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();

                                str = "success";
                            }
                            else
                            {
                                str = "fail";
                            }
                        }
                        else
                        {
                            str = "bigLoanMoney";
                        }

                    }
                    else
                    {
                        str = "validCodefail";
                    }
                }
                else
                {
                    str = "validCodefailNull";
                }

            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 净值标
        //净值标页面
        public ActionResult WorthIndex()
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

        //新增净值标
        public ActionResult InsertJingTreeTop(B_LoanTable B_Loan, string ChouDeadline)
        {
            string str = "";
            try
            {

                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                B_UserlimitTable Blimit = (from tblimit in myModels.B_UserlimitTable
                                           where tblimit.UserID == list.UserID
                                           select tblimit).Single();

                string strCurrentCode = "";//定义当前的编号
                var listLoan = (from tbloan in myModels.B_LoanTable
                                orderby tbloan.PaymentNumber
                                select tbloan).ToList();
                if (listLoan.Count > 0)//判断表中是否有数据
                {
                    int Count = listLoan.Count;//获取列表中的总数据
                    B_LoanTable myLoan = listLoan[Count - 1];//获取最后一个编号
                    int intCode = Convert.ToInt32(myLoan.PaymentNumber.Substring(8, 5));
                    intCode++;
                    strCurrentCode = intCode.ToString();
                    for (int i = 0; i < 5; i++)
                    {
                        strCurrentCode = strCurrentCode.Length < 5 ? "0" + strCurrentCode : strCurrentCode;//三目运算符
                    }
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + strCurrentCode;
                }
                else
                {
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "00001";//如果count=0,科室编码strCurrentCode从D0001开始
                }

                B_Loan.PaymentNumber = strCurrentCode;//贷款号
                B_Loan.SubmitTime = DateTime.Now;//提交时间
                B_Loan.AlreadyLoanMoney = B_Loan.LoanMoney;//已借金额

                decimal rate;
                rate = Convert.ToDecimal(B_Loan.Rate);
                B_Loan.PayableInterest = (B_Loan.LoanMoney * rate) / 100;//应还利息

                B_Loan.RepayPrincipal = B_Loan.PayableInterest + B_Loan.LoanMoney;//偿还本息

                B_Loan.ArrearagePrincipal = B_Loan.RepayPrincipal;//未还本息

                B_Loan.AlreadyPrincipal = Convert.ToDecimal(0.00);//已还本息
                B_Loan.Raisestandardday = ChouDeadline;
          
                B_Loan.Grossscore = B_Loan.AlreadyLoanMoney / B_Loan.LowestTenderMoney;//总份数
                B_Loan.SurplusLoan = B_Loan.Grossscore;//剩余份数
                B_Loan.AlreadyLoan = Convert.ToDecimal(0);//已借份数
                B_Loan.stopbuyback = Convert.ToDecimal(0);
                B_Loan.Scheduleinvestment = 0;//进度
                                              //B_Loan.LoanPeriods = Convert.ToInt32(B_Loan.LoanDeadlineID).ToString();
                                              //借款期数的判断
                if (B_Loan.LoanDeadlineID == 1)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 2)
                {
                    B_Loan.LoanPeriods = "2";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 3)
                {
                    B_Loan.LoanPeriods = "3";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 4)
                {
                    B_Loan.LoanPeriods = "4";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 5)
                {
                    B_Loan.LoanPeriods = "5";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 6)
                {
                    B_Loan.LoanPeriods = "6";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 7)
                {
                    B_Loan.LoanPeriods = "7";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 8)
                {
                    B_Loan.LoanPeriods = "8";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 9)
                {
                    B_Loan.LoanPeriods = "9";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 10)
                {
                    B_Loan.LoanPeriods = "10";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 11)
                {
                    B_Loan.LoanPeriods = "11";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 12)
                {
                    B_Loan.LoanPeriods = "12";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID > 12)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }

                //获取验证码
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (B_Loan.LoanMoney <= Blimit.Ravailable)//判断借款额度是否大于可用额度
                        {
                            if (B_Loan.RepaymentWayID != null && B_Loan.LoanDeadlineID != null)
                            {

                                B_Loan.UserID = list.UserID;
                                B_Loan.StatusID = 1;
                                B_Loan.TreetoptypeID = 4;
                                B_Loan.InvestTime = "0";
                                myModels.B_LoanTable.Add(B_Loan);
                                myModels.SaveChanges();

                                B_UserlimitTable Nlimit = (from tlimit in myModels.B_UserlimitTable
                                                           where tlimit.UserID == list.UserID
                                                           select tlimit).Single();
                                Nlimit.Ravailable = Nlimit.Ravailable - B_Loan.LoanMoney;//可用金额
                                Nlimit.Rfreeze = Nlimit.Rfreeze + B_Loan.LoanMoney;//冻结金额
                                myModels.Entry(Nlimit).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();

                                str = "success";
                            }
                            else
                            {
                                str = "fail";
                            }
                        }
                        else
                        {
                            str = "bigLoanMoney";
                        }
                    }
                    else
                    {
                        str = "validCodefail";
                    }
                }
                else
                {
                    str = "validCodefailNull";
                }

            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 天标
        //天标页面
        public ActionResult DayLoanIndex()
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
        //新增天标
        public ActionResult InsertTianTreeTop(B_LoanTable B_Loan, string ChouDeadline)
        {
            string str = "";
            try
            {

                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                B_UserlimitTable Blimit = (from tblimit in myModels.B_UserlimitTable
                                           where tblimit.UserID == list.UserID
                                           select tblimit).Single();

                string strCurrentCode = "";//定义当前的编号
                var listLoan = (from tbloan in myModels.B_LoanTable
                                orderby tbloan.PaymentNumber
                                select tbloan).ToList();
                if (listLoan.Count > 0)//判断表中是否有数据
                {
                    int Count = listLoan.Count;//获取列表中的总数据
                    B_LoanTable myLoan = listLoan[Count - 1];//获取最后一个编号
                    int intCode = Convert.ToInt32(myLoan.PaymentNumber.Substring(8, 5));
                    intCode++;
                    strCurrentCode = intCode.ToString();
                    for (int i = 0; i < 5; i++)
                    {
                        strCurrentCode = strCurrentCode.Length < 5 ? "0" + strCurrentCode : strCurrentCode;//三目运算符
                    }
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + strCurrentCode;
                }
                else
                {
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "00001";//如果count=0,科室编码strCurrentCode从D0001开始
                }

                B_Loan.PaymentNumber = strCurrentCode;//贷款号
                B_Loan.SubmitTime = DateTime.Now;//提交时间
                B_Loan.AlreadyLoanMoney = B_Loan.LoanMoney;//已借金额

                decimal rate;
                rate = Convert.ToDecimal(B_Loan.Rate);
                B_Loan.PayableInterest = (B_Loan.LoanMoney * rate) / 100;//应还利息

                B_Loan.RepayPrincipal = B_Loan.PayableInterest + B_Loan.LoanMoney;//偿还本息

                B_Loan.ArrearagePrincipal = B_Loan.RepayPrincipal;//未还本息

                B_Loan.AlreadyPrincipal = Convert.ToDecimal(0.00);//已还本息
                B_Loan.Raisestandardday = ChouDeadline;
          
                B_Loan.Grossscore = B_Loan.AlreadyLoanMoney / B_Loan.LowestTenderMoney;//总份数
                B_Loan.SurplusLoan = B_Loan.Grossscore;//剩余份数
                B_Loan.AlreadyLoan = Convert.ToDecimal(0);//已借份数
                B_Loan.stopbuyback = Convert.ToDecimal(0);
                B_Loan.Scheduleinvestment = 0;//进度
                                              //B_Loan.LoanPeriods = Convert.ToInt32(B_Loan.LoanDeadlineID).ToString();
                                              //借款期数的判断
                if (B_Loan.LoanDeadlineID == 1)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 2)
                {
                    B_Loan.LoanPeriods = "2";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 3)
                {
                    B_Loan.LoanPeriods = "3";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 4)
                {
                    B_Loan.LoanPeriods = "4";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 5)
                {
                    B_Loan.LoanPeriods = "5";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 6)
                {
                    B_Loan.LoanPeriods = "6";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 7)
                {
                    B_Loan.LoanPeriods = "7";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 8)
                {
                    B_Loan.LoanPeriods = "8";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 9)
                {
                    B_Loan.LoanPeriods = "9";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 10)
                {
                    B_Loan.LoanPeriods = "10";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 11)
                {
                    B_Loan.LoanPeriods = "11";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 12)
                {
                    B_Loan.LoanPeriods = "12";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID > 12)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                //获取验证码
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (B_Loan.LoanMoney <= Blimit.Ravailable)//判断借款额度是否大于可用额度
                        {
                            if (B_Loan.RepaymentWayID != null && B_Loan.LoanDeadlineID != null)
                            {

                                B_Loan.UserID = list.UserID;
                                B_Loan.StatusID = 1;
                                B_Loan.TreetoptypeID = 6;
                                B_Loan.InvestTime = "0";
                                myModels.B_LoanTable.Add(B_Loan);
                                myModels.SaveChanges();

                                B_UserlimitTable Nlimit = (from tlimit in myModels.B_UserlimitTable
                                                           where tlimit.UserID == list.UserID
                                                           select tlimit).Single();
                                Nlimit.Ravailable = Nlimit.Ravailable - B_Loan.LoanMoney;//可用金额
                                Nlimit.Rfreeze = Nlimit.Rfreeze + B_Loan.LoanMoney;//冻结金额
                                myModels.Entry(Nlimit).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();

                                str = "success";
                            }
                            else
                            {
                                str = "fail";
                            }
                        }
                        else
                        {
                            str = "bigLoanMoney";
                        }
                    }
                    else
                    {
                        str = "validCodefail";
                    }
                }
                else
                {
                    str = "validCodefailNull";
                }

            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 担保标
        //担保标页面
        public ActionResult WarrantIndex()
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

        //新增担保标
        public ActionResult InsertDanBaoTreeTop(B_LoanTable B_Loan, string ChouDeadline)
        {
            string str = "";
            try
            {

                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                B_UserlimitTable Blimit = (from tblimit in myModels.B_UserlimitTable
                                           where tblimit.UserID == list.UserID
                                           select tblimit).Single();

                string strCurrentCode = "";//定义当前的编号
                var listLoan = (from tbloan in myModels.B_LoanTable
                                orderby tbloan.PaymentNumber
                                select tbloan).ToList();
                if (listLoan.Count > 0)//判断表中是否有数据
                {
                    int Count = listLoan.Count;//获取列表中的总数据
                    B_LoanTable myLoan = listLoan[Count - 1];//获取最后一个编号
                    int intCode = Convert.ToInt32(myLoan.PaymentNumber.Substring(8, 5));
                    intCode++;
                    strCurrentCode = intCode.ToString();
                    for (int i = 0; i < 5; i++)
                    {
                        strCurrentCode = strCurrentCode.Length < 5 ? "0" + strCurrentCode : strCurrentCode;//三目运算符
                    }
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + strCurrentCode;
                }
                else
                {
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "00001";//如果count=0,科室编码strCurrentCode从D0001开始
                }

                B_Loan.PaymentNumber = strCurrentCode;//贷款号
                B_Loan.SubmitTime = DateTime.Now;//提交时间
                B_Loan.AlreadyLoanMoney = B_Loan.LoanMoney;//已借金额

                decimal rate;
                rate = Convert.ToDecimal(B_Loan.Rate);
                B_Loan.PayableInterest = (B_Loan.LoanMoney * rate) / 100;//应还利息

                B_Loan.RepayPrincipal = B_Loan.PayableInterest + B_Loan.LoanMoney;//偿还本息

                B_Loan.ArrearagePrincipal = B_Loan.RepayPrincipal;//未还本息

                B_Loan.AlreadyPrincipal = Convert.ToDecimal(0.00);//已还本息
                B_Loan.Raisestandardday = ChouDeadline;
            
                B_Loan.Grossscore = B_Loan.AlreadyLoanMoney / B_Loan.LowestTenderMoney;//总份数
                B_Loan.SurplusLoan = B_Loan.Grossscore;//剩余份数
                B_Loan.AlreadyLoan = Convert.ToDecimal(0);//已借份数
                B_Loan.stopbuyback = Convert.ToDecimal(0);
                B_Loan.Scheduleinvestment = 0;//进度
                                              //B_Loan.LoanPeriods = Convert.ToInt32(B_Loan.LoanDeadlineID).ToString();
                                              //借款期数的判断
                if (B_Loan.LoanDeadlineID == 1)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 2)
                {
                    B_Loan.LoanPeriods = "2";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 3)
                {
                    B_Loan.LoanPeriods = "3";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 4)
                {
                    B_Loan.LoanPeriods = "4";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 5)
                {
                    B_Loan.LoanPeriods = "5";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 6)
                {
                    B_Loan.LoanPeriods = "6";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 7)
                {
                    B_Loan.LoanPeriods = "7";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 8)
                {
                    B_Loan.LoanPeriods = "8";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 9)
                {
                    B_Loan.LoanPeriods = "9";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 10)
                {
                    B_Loan.LoanPeriods = "10";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 11)
                {
                    B_Loan.LoanPeriods = "11";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 12)
                {
                    B_Loan.LoanPeriods = "12";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID > 12)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                //获取验证码
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (B_Loan.LoanMoney <= Blimit.Wavailable)//判断借款额度是否大于可用额度
                        {
                            if (B_Loan.RepaymentWayID != null && B_Loan.LoanDeadlineID != null)
                            {

                                B_Loan.UserID = list.UserID;
                                B_Loan.StatusID = 1;
                                B_Loan.TreetoptypeID = 5;
                                B_Loan.InvestTime = "0";
                                myModels.B_LoanTable.Add(B_Loan);
                                myModels.SaveChanges();

                                B_UserlimitTable Nlimit = (from tlimit in myModels.B_UserlimitTable
                                                           where tlimit.UserID == list.UserID
                                                           select tlimit).Single();
                                Nlimit.Wavailable = Nlimit.Wavailable - B_Loan.LoanMoney;//可用金额
                                Nlimit.Wfreeze = Nlimit.Wfreeze + B_Loan.LoanMoney;//冻结金额
                                myModels.Entry(Nlimit).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();

                                str = "success";
                            }
                            else
                            {
                                str = "fail";
                            }
                        }
                        else
                        {
                            str = "bigLoanMoney";
                        }
                    }
                    else
                    {
                        str = "validCodefail";
                    }
                }
                else
                {
                    str = "validCodefailNull";
                }

            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 抵押标
        //抵押标页面
        public ActionResult MortgageIndex()
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

        //新增抵押标
        public ActionResult InsertDiYaTreeTop(B_LoanTable B_Loan, string ChouDeadline)
        {
            string str = "";
            try
            {

                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                B_UserlimitTable Blimit = (from tblimit in myModels.B_UserlimitTable
                                           where tblimit.UserID == list.UserID
                                           select tblimit).Single();

                string strCurrentCode = "";//定义当前的编号
                var listLoan = (from tbloan in myModels.B_LoanTable
                                orderby tbloan.PaymentNumber
                                select tbloan).ToList();
                if (listLoan.Count > 0)//判断表中是否有数据
                {
                    int Count = listLoan.Count;//获取列表中的总数据
                    B_LoanTable myLoan = listLoan[Count - 1];//获取最后一个编号
                    int intCode = Convert.ToInt32(myLoan.PaymentNumber.Substring(8, 5));
                    intCode++;
                    strCurrentCode = intCode.ToString();
                    for (int i = 0; i < 5; i++)
                    {
                        strCurrentCode = strCurrentCode.Length < 5 ? "0" + strCurrentCode : strCurrentCode;//三目运算符
                    }
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + strCurrentCode;
                }
                else
                {
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "00001";//如果count=0,科室编码strCurrentCode从D0001开始
                }

                B_Loan.PaymentNumber = strCurrentCode;//贷款号
                B_Loan.SubmitTime = DateTime.Now;//提交时间
                B_Loan.AlreadyLoanMoney = B_Loan.LoanMoney;//已借金额

                decimal rate;
                rate = Convert.ToDecimal(B_Loan.Rate);
                B_Loan.PayableInterest = (B_Loan.LoanMoney * rate) / 100;//应还利息

                B_Loan.RepayPrincipal = B_Loan.PayableInterest + B_Loan.LoanMoney;//偿还本息

                B_Loan.ArrearagePrincipal = B_Loan.RepayPrincipal;//未还本息

                B_Loan.AlreadyPrincipal = Convert.ToDecimal(0.00);//已还本息
                B_Loan.Raisestandardday = ChouDeadline;
              
                B_Loan.Grossscore = B_Loan.AlreadyLoanMoney / B_Loan.LowestTenderMoney;//总份数
                B_Loan.SurplusLoan = B_Loan.Grossscore;//剩余份数
                B_Loan.AlreadyLoan = Convert.ToDecimal(0);//已借份数
                B_Loan.stopbuyback = Convert.ToDecimal(0);
                B_Loan.Scheduleinvestment = 0;//进度
                                              //B_Loan.LoanPeriods = Convert.ToInt32(B_Loan.LoanDeadlineID).ToString();
                                              //借款期数的判断
                if (B_Loan.LoanDeadlineID == 1)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 2)
                {
                    B_Loan.LoanPeriods = "2";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 3)
                {
                    B_Loan.LoanPeriods = "3";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 4)
                {
                    B_Loan.LoanPeriods = "4";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 5)
                {
                    B_Loan.LoanPeriods = "5";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 6)
                {
                    B_Loan.LoanPeriods = "6";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 7)
                {
                    B_Loan.LoanPeriods = "7";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 8)
                {
                    B_Loan.LoanPeriods = "8";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 9)
                {
                    B_Loan.LoanPeriods = "9";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 10)
                {
                    B_Loan.LoanPeriods = "10";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 11)
                {
                    B_Loan.LoanPeriods = "11";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 12)
                {
                    B_Loan.LoanPeriods = "12";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID > 12)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }

                //获取验证码
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (B_Loan.LoanMoney <= Blimit.Mavailable)//判断借款额度是否大于可用额度
                        {
                            if (B_Loan.RepaymentWayID != null && B_Loan.LoanDeadlineID != null)
                            {

                                B_Loan.UserID = list.UserID;
                                B_Loan.StatusID = 1;
                                B_Loan.TreetoptypeID = 2;
                                B_Loan.InvestTime = "0";
                                myModels.B_LoanTable.Add(B_Loan);
                                myModels.SaveChanges();

                                B_UserlimitTable Nlimit = (from tlimit in myModels.B_UserlimitTable
                                                           where tlimit.UserID == list.UserID
                                                           select tlimit).Single();
                                Nlimit.Mavailable = Nlimit.Mavailable - B_Loan.LoanMoney;//可用金额
                                Nlimit.Mfreeze = Nlimit.Mfreeze + B_Loan.LoanMoney;//冻结金额
                                myModels.Entry(Nlimit).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();

                                str = "success";
                            }
                            else
                            {
                                str = "fail";
                            }
                        }
                        else
                        {
                            str = "bigLoanMoney";
                        }
                    }
                    else
                    {
                        str = "validCodefail";
                    }
                }
                else
                {
                    str = "validCodefailNull";
                }

            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 秒标
        //秒标页面
        public ActionResult SecondIndex()
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

        //新增秒标
        public ActionResult InsertMiaoTreeTop(B_LoanTable B_Loan, string ChouDeadline)
        {
            string str = "";
            try
            {

                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();
                B_UserlimitTable Blimit = (from tblimit in myModels.B_UserlimitTable
                                           where tblimit.UserID == list.UserID
                                           select tblimit).Single();

                string strCurrentCode = "";//定义当前的编号
                var listLoan = (from tbloan in myModels.B_LoanTable
                                orderby tbloan.PaymentNumber
                                select tbloan).ToList();
                if (listLoan.Count > 0)//判断表中是否有数据
                {
                    int Count = listLoan.Count;//获取列表中的总数据
                    B_LoanTable myLoan = listLoan[Count - 1];//获取最后一个编号
                    int intCode = Convert.ToInt32(myLoan.PaymentNumber.Substring(8, 5));
                    intCode++;
                    strCurrentCode = intCode.ToString();
                    for (int i = 0; i < 5; i++)
                    {
                        strCurrentCode = strCurrentCode.Length < 5 ? "0" + strCurrentCode : strCurrentCode;//三目运算符
                    }
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + strCurrentCode;
                }
                else
                {
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "00001";//如果count=0,科室编码strCurrentCode从D0001开始
                }

                B_Loan.PaymentNumber = strCurrentCode;//贷款号
                B_Loan.SubmitTime = DateTime.Now;//提交时间
                B_Loan.AlreadyLoanMoney = B_Loan.LoanMoney;//已借金额

                decimal rate;
                rate = Convert.ToDecimal(B_Loan.Rate);
                B_Loan.PayableInterest = (B_Loan.LoanMoney * rate) / 100;//应还利息

                B_Loan.RepayPrincipal = B_Loan.PayableInterest + B_Loan.LoanMoney;//偿还本息

                B_Loan.ArrearagePrincipal = B_Loan.RepayPrincipal;//未还本息

                B_Loan.AlreadyPrincipal = Convert.ToDecimal(0.00);//已还本息
                B_Loan.Raisestandardday = ChouDeadline;
              
                B_Loan.Grossscore = B_Loan.AlreadyLoanMoney / B_Loan.LowestTenderMoney;//总份数
                B_Loan.SurplusLoan = B_Loan.Grossscore;//剩余份数
                B_Loan.AlreadyLoan = Convert.ToDecimal(0);//已借份数
                B_Loan.stopbuyback = Convert.ToDecimal(0);
                B_Loan.Scheduleinvestment = 0;//进度
                                              //B_Loan.LoanPeriods = Convert.ToInt32(B_Loan.LoanDeadlineID).ToString();
                                              //借款期数的判断
                if (B_Loan.LoanDeadlineID == 1)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 2)
                {
                    B_Loan.LoanPeriods = "2";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 3)
                {
                    B_Loan.LoanPeriods = "3";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 4)
                {
                    B_Loan.LoanPeriods = "4";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 5)
                {
                    B_Loan.LoanPeriods = "5";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 6)
                {
                    B_Loan.LoanPeriods = "6";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 7)
                {
                    B_Loan.LoanPeriods = "7";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 8)
                {
                    B_Loan.LoanPeriods = "8";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 9)
                {
                    B_Loan.LoanPeriods = "9";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 10)
                {
                    B_Loan.LoanPeriods = "10";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 11)
                {
                    B_Loan.LoanPeriods = "11";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 12)
                {
                    B_Loan.LoanPeriods = "12";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID > 12)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }

                //获取验证码
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (B_Loan.LoanMoney <= Blimit.Ravailable)//判断借款额度是否大于可用额度
                        {
                            if (B_Loan.RepaymentWayID != null && B_Loan.LoanDeadlineID != null)
                            {

                                B_Loan.UserID = list.UserID;
                                B_Loan.StatusID = 1;
                                B_Loan.TreetoptypeID = 1;
                                B_Loan.InvestTime = "0";
                                myModels.B_LoanTable.Add(B_Loan);
                                myModels.SaveChanges();

                                B_UserlimitTable Nlimit = (from tlimit in myModels.B_UserlimitTable
                                                           where tlimit.UserID == list.UserID
                                                           select tlimit).Single();
                                Nlimit.Ravailable = Nlimit.Ravailable - B_Loan.LoanMoney;//可用金额
                                Nlimit.Rfreeze = Nlimit.Rfreeze + B_Loan.LoanMoney;//冻结金额
                                myModels.Entry(Nlimit).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();

                                str = "success";
                            }
                            else
                            {
                                str = "fail";
                            }
                        }
                        else
                        {
                            str = "bigLoanMoney";
                        }
                    }
                    else
                    {
                        str = "validCodefail";
                    }
                }
                else
                {
                    str = "validCodefailNull";
                }

            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 流转标
        //流转标页面
        public ActionResult RoamIndex()
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

        //新增流转标
        public ActionResult InsertLiuZhuanTreeTop(B_LoanTable B_Loan, string ChouDeadline)
        {
            string str = "";
            try
            {

                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                B_UserlimitTable Blimit = (from tblimit in myModels.B_UserlimitTable
                                           where tblimit.UserID == list.UserID
                                           select tblimit).Single();

                string strCurrentCode = "";//定义当前的编号
                var listLoan = (from tbloan in myModels.B_LoanTable
                                orderby tbloan.PaymentNumber
                                select tbloan).ToList();
                if (listLoan.Count > 0)//判断表中是否有数据
                {
                    int Count = listLoan.Count;//获取列表中的总数据
                    B_LoanTable myLoan = listLoan[Count - 1];//获取最后一个编号
                    int intCode = Convert.ToInt32(myLoan.PaymentNumber.Substring(8, 5));
                    intCode++;
                    strCurrentCode = intCode.ToString();
                    for (int i = 0; i < 5; i++)
                    {
                        strCurrentCode = strCurrentCode.Length < 5 ? "0" + strCurrentCode : strCurrentCode;//三目运算符
                    }
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + strCurrentCode;
                }
                else
                {
                    strCurrentCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "00001";//如果count=0,科室编码strCurrentCode从D0001开始
                }

                B_Loan.PaymentNumber = strCurrentCode;//贷款号
                B_Loan.SubmitTime = DateTime.Now;//提交时间
                B_Loan.AlreadyLoanMoney = B_Loan.LoanMoney;//已借金额

                decimal rate;
                rate = Convert.ToDecimal(B_Loan.Rate);
                B_Loan.PayableInterest = (B_Loan.LoanMoney * rate) / 100;//应还利息

                B_Loan.RepayPrincipal = B_Loan.PayableInterest + B_Loan.LoanMoney;//偿还本息

                B_Loan.ArrearagePrincipal = B_Loan.RepayPrincipal;//未还本息

                B_Loan.AlreadyPrincipal = Convert.ToDecimal(0.00);//已还本息
                B_Loan.Raisestandardday = ChouDeadline;
             
                B_Loan.Grossscore = B_Loan.AlreadyLoanMoney / B_Loan.LowestTenderMoney;//总份数
                B_Loan.SurplusLoan = B_Loan.Grossscore;//剩余份数
                B_Loan.AlreadyLoan = Convert.ToDecimal(0);//已借份数
                B_Loan.stopbuyback = Convert.ToDecimal(0);
                B_Loan.Scheduleinvestment = 0;//进度
                                              //B_Loan.LoanPeriods = Convert.ToInt32(B_Loan.LoanDeadlineID).ToString();
                                              //借款期数的判断
                if (B_Loan.LoanDeadlineID == 1)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 2)
                {
                    B_Loan.LoanPeriods = "2";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 3)
                {
                    B_Loan.LoanPeriods = "3";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 4)
                {
                    B_Loan.LoanPeriods = "4";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 5)
                {
                    B_Loan.LoanPeriods = "5";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 6)
                {
                    B_Loan.LoanPeriods = "6";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 7)
                {
                    B_Loan.LoanPeriods = "7";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 8)
                {
                    B_Loan.LoanPeriods = "8";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 9)
                {
                    B_Loan.LoanPeriods = "9";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 10)
                {
                    B_Loan.LoanPeriods = "10";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 11)
                {
                    B_Loan.LoanPeriods = "11";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID == 12)
                {
                    B_Loan.LoanPeriods = "12";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }
                if (B_Loan.LoanDeadlineID > 12)
                {
                    B_Loan.LoanPeriods = "1";
                    int qishu = Convert.ToInt32(B_Loan.LoanPeriods);
                    B_Loan.EveryTrancheMoney = Convert.ToDecimal(B_Loan.RepayPrincipal / qishu);
                }

                //获取验证码
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (B_Loan.LoanMoney <= Blimit.Ravailable)//判断借款额度是否大于可用额度
                        {
                            if (B_Loan.RepaymentWayID != null && B_Loan.LoanDeadlineID != null)
                            {

                                B_Loan.UserID = list.UserID;
                                B_Loan.StatusID = 1;
                                B_Loan.TreetoptypeID =7;
                                B_Loan.InvestTime = "0";
                                myModels.B_LoanTable.Add(B_Loan);
                                myModels.SaveChanges();

                                B_UserlimitTable Nlimit = (from tlimit in myModels.B_UserlimitTable
                                                           where tlimit.UserID == list.UserID
                                                           select tlimit).Single();
                                Nlimit.Ravailable = Nlimit.Ravailable - B_Loan.LoanMoney;//可用金额
                                Nlimit.Rfreeze = Nlimit.Rfreeze + B_Loan.LoanMoney;//冻结金额
                                myModels.Entry(Nlimit).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();

                                str = "success";
                            }
                            else
                            {
                                str = "fail";
                            }
                        }
                        else
                        {
                            str = "bigLoanMoney";
                        }
                    }
                    else
                    {
                        str = "validCodefail";
                    }
                }
                else
                {
                    str = "validCodefailNull";
                }

            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}