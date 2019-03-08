using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiYouQianTaiXiTong.Models;
using DiYouQianTaiXiTong.Vo;
using DiYouQianTaiXiTong.Common;

namespace DiYouQianTaiXiTong.Areas.Invest.Controllers
{
    public class InvestController : Controller
    {
        // GET: Invest/Invest
        Models.DYXTEntities myModels = new DYXTEntities();
     
        //投资页面
        public ActionResult InvestIndex()
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
        //认购页面
       public ActionResult SubscribeIndex(int LoanID,BsgridPage bsgridPage)
        {

            try
            {
                string strUserId = Session["AccountID"].ToString();
                int intUserId = Convert.ToInt32(strUserId);
                B_AccountTable accouunt = (from tbAccountTable in myModels.B_AccountTable
                                           where tbAccountTable.AccountID == intUserId
                                           select tbAccountTable).Single();
                ViewBag.User = accouunt.User;

                B_LoanTable notice = (from tbloan in myModels.B_LoanTable
                              join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                              join tbLoanDeadline in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                              join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                              join tbRepaymentWayTable in myModels.S_RepaymentWayTable on tbloan.RepaymentWayID equals tbRepaymentWayTable.RepaymentWayID
                              where tbloan.LoanID == LoanID
                              select new LoanVo
                              {
                                  LoanID = tbloan.LoanID,
                                  UserID = tbloan.UserID,
                                  UserName= tbuser.UserName,
                                  Loantitle = tbloan.Loantitle,
                                  PaymentNumber=tbloan.PaymentNumber,
                                  LoanMoney = tbloan.LoanMoney,
                                  Rate = tbloan.Rate,
                                  LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                  RepaymentWayName = tbRepaymentWayTable.RepaymentWayName,
                                  LowestTenderMoney = tbloan.LowestTenderMoney,
                                  AlreadyLoan = tbloan.AlreadyLoan,
                                  SurplusLoan = tbloan.SurplusLoan,
                                  Scheduleinvestment = tbloan.Scheduleinvestment
                              }).Single();
                ViewBag.notice = notice;

                B_LoanTable Loanxinix = (from tbloan in myModels.B_LoanTable
                                         where tbloan.LoanID == LoanID
                                         select tbloan).Single();
                int userid = Convert.ToInt32(Loanxinix.UserID);
                B_UserTable userxinxi = (from tbuser in myModels.B_UserTable
                                         where tbuser.UserID == userid
                                         select new UserVo
                                         {
                                             UserName = tbuser.UserName,
                                             Sex=tbuser.Sex,
                                             MonthIncome= tbuser.MonthIncome,
                                             ReleaseTimeStr = tbuser.BornDate.ToString(),
                                             MarriageState=tbuser.MarriageState,
                                             EducationalBackground=tbuser.EducationalBackground,
                                             WhetherBuyCar= tbuser.WhetherBuyCar,
                                             housingCondition=tbuser.housingCondition,
                                             Issue=tbuser.Issue,
                                             PropertyAmounts=tbuser.PropertyAmounts,
                                             CompensatoryMoney= tbuser.CompensatoryMoney,
                                         }).Single();
                ViewBag.userxinxi = userxinxi;
                var vraloan = (from tbloan in myModels.B_LoanTable
                                where tbloan.UserID == userid
                                select tbloan).ToList();
                decimal rowloan = vraloan.Count;
                ViewBag.rowloan = rowloan;//发布借款
                var vraloanStatusID = (from tbloan in myModels.B_LoanTable
                               where tbloan.UserID == userid && tbloan.StatusID ==10
                               select tbloan).ToList();
                decimal varvraloanStatusID = vraloanStatusID.Count;
                ViewBag.varvraloanStatusID = varvraloanStatusID;//成功借款

                var vrastrloan = (from tbloanid in myModels.B_LoanTable
                                  where tbloanid.UserID == userid && (tbloanid.StatusID == 12 || tbloanid.StatusID == 13)
                                  select tbloanid).ToList();
                decimal rowvrastrloan = vrastrloan.Count;
                ViewBag.rowvrastrloan = rowvrastrloan;//还请笔数

               
            }
            catch (Exception e)
            {
                
            }

            return View();
        }

        //绑定所有的借款
        public ActionResult SelectLoanList()
        {
            try
            {
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

                return Json(notict, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }

        }

        /// <summary>
        /// 确定认购
        /// </summary>
        /// <param name="bInvest"></param>
        /// <param name="Loantitle"></param>
        /// <param name="fenshu"></param>
        /// <param name="LowestTenderMoney"></param>
        /// <param name="Rate"></param>
        /// <param name="PayPassword"></param>
        /// <returns></returns>
        public ActionResult InsertLoan(B_InvestTable bInvest, string Loantitle, int fenshu, string LowestTenderMoney, string Rate, string PayPassword)
        {
            string str = "";
            int loanid = Convert.ToInt32(bInvest.LoanID);
            B_LoanTable varB_LoanTable = (from tbtbB_Loan in myModels.B_LoanTable
                                          where tbtbB_Loan.LoanID == loanid
                                          select tbtbB_Loan).Single();
            int userid = Convert.ToInt32(varB_LoanTable.UserID);
            B_UserTable varuser = (from tbuser in myModels.B_UserTable
                                   where tbuser.UserID == userid
                                   select tbuser).Single();
            string password = AESEncryptHelper.AESEncrypt(PayPassword);
            if (varuser.PayPassword.Trim().Equals(password))
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();
                bInvest.LoanTreetop = Loantitle;
                decimal intLowestTenderMoney = Convert.ToDecimal(LowestTenderMoney);
                bInvest.InvestMoney = fenshu * intLowestTenderMoney;//份数*最低投标金额           

                decimal rate = Convert.ToDecimal(Rate.ToString().Trim());  //年利率
                bInvest.ReceivableInterest = (bInvest.InvestMoney * rate) / 100;//应收利息

                bInvest.ReceivablePrincipal = bInvest.InvestMoney + bInvest.ReceivableInterest;//应收本息

                bInvest.CountermandPrincipal = Convert.ToDecimal(0.00); //已收本息
                bInvest.NotRetrievePrincipal = bInvest.ReceivablePrincipal;//未收本息       
                if (list.PropertyAmounts > bInvest.InvestMoney)
                {
                    if (bInvest.LoanID != null)
                    {

                        bInvest.InvestTime = DateTime.Now;
                        bInvest.UserID = list.UserID;
                        bInvest.StatusID = 16;
                        bInvest.WhetherAttorn = false;
                        myModels.B_InvestTable.Add(bInvest);
                        myModels.SaveChanges();

                        //修改借款标表
                        B_LoanTable B_Loan = (from tbloan in myModels.B_LoanTable
                                              where tbloan.LoanID == bInvest.LoanID
                                              select tbloan).Single();
                        //B_Loan.LoanMoney = B_Loan.LoanMoney - bInvest.InvestMoney;//借款金额

                        int investTime;
                        investTime = Convert.ToInt32(B_Loan.InvestTime.ToString());//投资次数
                        investTime = investTime + 1;
                        B_Loan.InvestTime = Convert.ToInt32(investTime).ToString();



                        int grossscore;
                        grossscore = Convert.ToInt32(B_Loan.Grossscore);//总份数;
                        double scheduleinvestment;//进度
                        scheduleinvestment = Convert.ToDouble(B_Loan.Scheduleinvestment);
                        scheduleinvestment = Convert.ToDouble(Convert.ToDouble(fenshu) / Convert.ToDouble(grossscore)) * 100;
                        B_Loan.Scheduleinvestment = Convert.ToDecimal(scheduleinvestment + Convert.ToDouble(B_Loan.Scheduleinvestment));

                        int surplusLoan;//剩余份数
                        surplusLoan = Convert.ToInt32(B_Loan.SurplusLoan);
                        surplusLoan = (surplusLoan - fenshu);
                        B_Loan.SurplusLoan = Convert.ToDecimal(surplusLoan);

                        int alreadyLoan;//已被购买份数
                        alreadyLoan = Convert.ToInt32(B_Loan.AlreadyLoan);
                        B_Loan.AlreadyLoan = Convert.ToInt32(alreadyLoan + fenshu);

                        myModels.Entry(B_Loan).State = System.Data.Entity.EntityState.Modified;
                        myModels.SaveChanges();
                        if (B_Loan.SurplusLoan == 0)
                        {
                            B_LoanTable bloan = (from tbloa in myModels.B_LoanTable
                                                 where tbloa.LoanID == bInvest.LoanID
                                                 select tbloa).Single();
                            bloan.StatusID = 28;
                            myModels.Entry(bloan).State = System.Data.Entity.EntityState.Modified;
                            myModels.SaveChanges();


                        }

                        //修改用户表的金额
                        B_UserTable listS = (from tbuser in myModels.B_UserTable
                                             where tbuser.AccountID == accountID
                                             select tbuser).Single();
                        //listS.PropertyAmounts = listS.PropertyAmounts - bInvest.InvestMoney;
                        listS.UsableMoney = listS.UsableMoney - bInvest.InvestMoney;
                        listS.FreezeMoney = listS.FreezeMoney + bInvest.InvestMoney;
                        myModels.Entry(listS).State = System.Data.Entity.EntityState.Modified;
                        myModels.SaveChanges();


                        B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                        Capitalrecord.UserID = list.UserID;
                        Capitalrecord.OperateTypeID = 4;
                        Capitalrecord.OpFare = Convert.ToDecimal(bInvest.InvestMoney);
                        Capitalrecord.Income = Convert.ToDecimal(0);
                        Capitalrecord.Expend = Convert.ToDecimal(bInvest.InvestMoney);
                        Capitalrecord.PropertyAmounts = list.PropertyAmounts;
                        Capitalrecord.Remarks = "用户" + "[" + list.UserName.Trim() + "]" + "，投资成功，可用余额减少" + bInvest.InvestMoney + "元";
                        Capitalrecord.operatetime = DateTime.Now;
                        myModels.B_CapitalrecordTable.Add(Capitalrecord);
                        myModels.SaveChanges();
                        str = "success";
                    }
                    else
                    {
                        str = "fail";
                    }
                }
            }
            else
            {
                str = "failPayPassword";
            }

            return Json(str, JsonRequestBehavior.AllowGet);
        }

        //交易记录
        public ActionResult SelectJiaoYiRecord(int LoanID, BsgridPage bsgridPage)
        {
            try
            {

                B_LoanTable loan = (from tbloan in myModels.B_LoanTable
                                    where tbloan.LoanID == LoanID
                                    select tbloan).Single();

                var list = (from tbin in myModels.B_InvestTable
                            join tbuser in myModels.B_UserTable on tbin.UserID equals tbuser.UserID
                            join tbloan in myModels.B_LoanTable on tbin.LoanID equals tbloan.LoanID
                            join tbstate in myModels.S_StatusTable on tbin.StatusID equals tbstate.StatusID
                            where tbloan.LoanID == loan.LoanID
                            select new InvestVo
                            {
                                UserID = tbloan.UserID,
                                InvestID = tbin.InvestID,
                                InvestMoney = tbin.InvestMoney,
                                InvestTime = tbin.InvestTime,
                                StartTime = tbin.InvestTime.ToString(),
                                UserName = tbuser.UserName,
                                StatusName = tbstate.StatusName,
                            });
                int TotaRow = list.Count();
                List<InvestVo> listT = list.OrderBy(p => p.InvestID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<InvestVo> bsgrid = new Bsgrid<InvestVo>();
                bsgrid.success = true;
                bsgrid.totalRows = TotaRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = listT;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //计算投资金额        
        public ActionResult CountInvestMoney(int LoanID, BsgridPage bsgridPage)
        {
            try
            {
                B_LoanTable loan = (from tbloan in myModels.B_LoanTable
                                    where tbloan.LoanID == LoanID
                                    select tbloan).Single();

                var list = (from tbin in myModels.B_InvestTable
                            join tbuser in myModels.B_UserTable on tbin.UserID equals tbuser.UserID
                            join tbloan in myModels.B_LoanTable on tbin.LoanID equals tbloan.LoanID
                            join tbstate in myModels.S_StatusTable on tbin.StatusID equals tbstate.StatusID
                            where tbin.LoanID == loan.LoanID
                            select new InvestVo
                            {
                                UserID = tbloan.UserID,
                                InvestID = tbin.InvestID,
                                InvestMoney = tbin.InvestMoney,
                                InvestTime = tbin.InvestTime,
                                StartTime = tbin.InvestTime.ToString(),
                                UserName = tbuser.UserName,
                                StatusName = tbstate.StatusName,
                            }).ToList();
                decimal ZongE = 0;
                decimal ZE = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    ZongE = Convert.ToDecimal(list[i].InvestMoney);
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

        //计算投资次数
        public ActionResult CountInvestTime(int LoanID, BsgridPage bsgridPage)
        {
            try
            {
                B_LoanTable loan = (from tbloan in myModels.B_LoanTable
                                    where tbloan.LoanID == LoanID
                                    select tbloan).Single();

                var list = (from tbin in myModels.B_InvestTable
                            join tbuser in myModels.B_UserTable on tbin.UserID equals tbuser.UserID
                            join tbloan in myModels.B_LoanTable on tbin.LoanID equals tbloan.LoanID
                            join tbstate in myModels.S_StatusTable on tbin.StatusID equals tbstate.StatusID
                            where tbin.LoanID == loan.LoanID
                            select new InvestVo
                            {
                                UserID = tbloan.UserID,
                                InvestID = tbin.InvestID,
                                InvestMoney = tbin.InvestMoney,
                                InvestTime = tbin.InvestTime,
                                StartTime = tbin.InvestTime.ToString(),
                                UserName = tbuser.UserName,
                                StatusName = tbstate.StatusName,
                            }).Count();
                return Json(list, JsonRequestBehavior.AllowGet);

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InsertFollow(B_FollowTable BFollow, int LoanID)
        {
            try
            {
                string str = "";
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();
                try
                {
                    int B_Follow = (from dbfollow in myModels.B_FollowTable
                                    where dbfollow.LoanID == LoanID && dbfollow.UserID == list.UserID
                                    select new
                                    {
                                        dbfollow.LoanID,
                                        dbfollow.UserID,
                                    }).Count();
                    if (B_Follow > 0)
                    {
                        str = "Exist";
                    }
                    else
                    {

                        BFollow.LoanID = BFollow.LoanID;
                        BFollow.UserID = list.UserID;
                        myModels.B_FollowTable.Add(BFollow);
                        myModels.SaveChanges();
                        str = "success";
                    }
                    return Json(str, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);

            }


        }

     
    }
}