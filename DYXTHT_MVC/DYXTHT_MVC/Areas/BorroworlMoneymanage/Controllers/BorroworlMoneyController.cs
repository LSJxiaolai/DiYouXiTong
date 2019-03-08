using DYXTHT_MVC.Models;
using DYXTHT_MVC.Common;
using DYXTHT_MVC.Vo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DYXTHT_MVC.Areas.BorroworlMoneymanage.Controllers
{
    public class BorroworlMoneyController : Controller
    {
        Models.DYXTEntities myDYXTEntities = new DYXTEntities();
        // GET: BorroworlMoneymanage/BorroworlMoney
        #region 初审价段


        /// <summary>
        /// 初审价段页面
        /// </summary>
        /// <returns></returns>
        public ActionResult preliminaryexamination()
        {
            return View();
        }
        /// <summary>
        /// 初审价段
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectfirsttrial(BsgridPage bsgridPage, string LoantitleID, string PaymentNumberID, string UserNameID, int TreetoptypeID)
        {
            var varloan = from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 1
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              ReleaseTimeStr = tbLoan.SubmitTime.ToString(),
                              StatusName = tbStatus.StatusName
                          };
            if (!string.IsNullOrEmpty(LoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(LoantitleID));
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                varloan = varloan.Where(n => n.PaymentNumber.Contains(PaymentNumberID));
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(UserNameID));
            }
            if (TreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == TreetoptypeID);
            }
            int totalRow = varloan.Count();
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 标种绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult SelcttbTreetoptype()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe
                                         select new SelectVo
                                         {
                                             id = tbTreetoptype.TreetoptypeID,
                                             text = tbTreetoptype.Treetoptype

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 修改标种
        /// </summary>
        /// <returns></returns>
        public ActionResult SelcttbTreetoptypes()
        {
            List<SelectVo> noticeType = (from tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe
                                         select new SelectVo
                                         {
                                             id = tbTreetoptype.TreetoptypeID,
                                             text = tbTreetoptype.Treetoptype

                                         }).ToList();

            return Json(noticeType, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 借款期限
        /// </summary>
        /// <returns></returns>
        public ActionResult SelcttbLoanDeadline()
        {
            List<SelectVo> noticeType = (from tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable
                                         select new SelectVo
                                         {
                                             id = tbLoanDeadline.LoanDeadlineID,
                                             text = tbLoanDeadline.LoanDeadlineName

                                         }).ToList();

            return Json(noticeType, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 还款方式
        /// </summary>
        /// <returns></returns>
        public ActionResult SelcttbRepaymentWay()
        {
            List<SelectVo> noticeType = (from tbTreetoptype in myDYXTEntities.S_RepaymentWayTable
                                         select new SelectVo
                                         {
                                             id = tbTreetoptype.RepaymentWayID,
                                             text = tbTreetoptype.RepaymentWayName

                                         }).ToList();

            return Json(noticeType, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public ActionResult SelcttbStatus()
        {
            List<SelectVo> noticeType = (from tbStatus in myDYXTEntities.S_StatusTable
                                         where tbStatus.StatusID == 1 || tbStatus.StatusID == 5 || tbStatus.StatusID == 7
                                               || tbStatus.StatusID == 8 || tbStatus.StatusID == 9 || tbStatus.StatusID == 26
                                               || tbStatus.StatusID == 27
                                         select new SelectVo
                                         {
                                             id = tbStatus.StatusID,
                                             text = tbStatus.StatusName

                                         }).ToList();

            return Json(noticeType, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 修改绑定
        /// </summary>
        /// <param name="LoanID"></param>
        /// <returns></returns>
        public ActionResult UpdataUsertype(int LoanID)
        {
            try
            {
                var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                               join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                               join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                               join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                               join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                               where tbLoan.LoanID == LoanID
                               select new B_LoanTableVo
                               {
                                   LoanID = tbLoan.LoanID,
                                   PaymentNumber = tbLoan.PaymentNumber,
                                   UserName = tbUSer.UserName,
                                   Loantitle = tbLoan.Loantitle,
                                   LoanMoney = tbLoan.LoanMoney,
                                   Rate = tbLoan.Rate,
                                   LoanDeadlineID = tbLoan.LoanDeadlineID,
                                   Treetoptype = tbTreetoptype.Treetoptype,
                                   TreetoptypeID = tbLoan.TreetoptypeID,
                                   RepaymentWayID = tbLoan.RepaymentWayID,
                                   StatusID = tbLoan.StatusID,
                                   UserID = tbLoan.UserID,
                                   RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                                   ReleaseTimeStr = tbLoan.SubmitTime.ToString(),
                                   StatusName = tbStatus.StatusName
                               }).Single();
                return Json(varloan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        /// <summary>
        /// 审核保存
        /// </summary>
        /// <param name="strUser"></param>
        /// <returns></returns>
        public ActionResult Updataloanbaocun(B_LoanTable strLoan, string ReleaseTimeStr, string optionsRadios)
        {
            //定义返回
            string strMsg = "fail";


            try
            {
                B_LoanTable varloan = (from tbloan in myDYXTEntities.B_LoanTable
                                       where tbloan.LoanID == strLoan.LoanID
                                       select tbloan).Single();
                //if (strLoan.TreetoptypeID != 6)
                //{
                //    varloan.Endtime = varloan.SubmitTime.Value.AddMonths(Convert.ToInt32(strLoan.LoanDeadlineID));
                //}
                //if (strLoan.TreetoptypeID == 6)
                //{
                //    int jj = Convert.ToInt32(strLoan.LoanDeadlineID) - 12;
                //    varloan.Endtime = varloan.SubmitTime.Value.AddDays(jj);
                //}
                if (varloan.Raisestandardday.Trim() == "1天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(1);
                }
                if (varloan.Raisestandardday.Trim() == "2天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(2);
                }
                if (varloan.Raisestandardday.Trim() == "3天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(3);
                }
                if (varloan.Raisestandardday.Trim() == "4天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(4);
                }
                if (varloan.Raisestandardday.Trim() == "5天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(5);
                }
                if (varloan.Raisestandardday.Trim() == "6天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(6);
                }
                if (varloan.Raisestandardday.Trim() == "7天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(7);
                }
                if (varloan.Raisestandardday.Trim() == "8天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(8);
                }
                if (varloan.Raisestandardday.Trim() == "9天")
                {
                    varloan.Raisestandard = DateTime.Now.AddDays(9);
                }
                varloan.LoanDeadlineID = strLoan.LoanDeadlineID;
                varloan.TreetoptypeID = strLoan.TreetoptypeID;
                varloan.RepaymentWayID = strLoan.RepaymentWayID;            
                varloan.ManageRemark = strLoan.ManageRemark;
                if (strLoan.StatusID == 26 || strLoan.StatusID == 27)
                {
                    varloan.RemoveTreetopTime = DateTime.Now;

                }
                if (optionsRadios == "option1")
                {
                    varloan.StatusID =7;
                }
                else
                {
                    varloan.StatusID =5;
                }
                  
                    myDYXTEntities.Entry(varloan).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {
                    strMsg = "success";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="LoanID"></param>
        /// <returns></returns>
        public ActionResult selectloanxinixi(int LoanID)
        {
            try
            {
                var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                               join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                               join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                               join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                               join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                               join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                               where tbLoan.LoanID == LoanID
                               select new B_LoanTableVo
                               {
                                   LoanID = tbLoan.LoanID,
                                   PaymentNumber = tbLoan.PaymentNumber,
                                   UserName = tbUSer.UserName,
                                   Loantitle = tbLoan.Loantitle,
                                   LoanMoney = tbLoan.LoanMoney,
                                   Rate = tbLoan.Rate,
                                   LoanDeadlineID = tbLoan.LoanDeadlineID,
                                   LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                   Treetoptype = tbTreetoptype.Treetoptype,
                                   TreetoptypeID = tbLoan.TreetoptypeID,
                                   RepaymentWayID = tbLoan.RepaymentWayID,
                                   StatusID = tbLoan.StatusID,
                                   UserID = tbLoan.UserID,
                                   RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                                   ManageRemark = tbLoan.ManageRemark,
                                   ReleaseTimeStr = tbLoan.SubmitTime.ToString(),
                                   StatusName = tbStatus.StatusName
                               }).Single();
                return Json(varloan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        #region 借款中       
        /// <summary>
        /// 借款中查看
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectborrowmoney(BsgridPage bsgridPage)
        {
            var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                           join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                           join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                           join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                           join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                           where tbLoan.StatusID == 7|| tbLoan.StatusID==8
                           select new B_LoanTableVo
                           {
                               LoanID = tbLoan.LoanID,
                               PaymentNumber = tbLoan.PaymentNumber,
                               UserName = tbUSer.UserName,
                               Loantitle = tbLoan.Loantitle,
                               LoanMoney = tbLoan.LoanMoney,
                               Rate = tbLoan.Rate,
                               LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                               Treetoptype = tbTreetoptype.Treetoptype,
                               TreetoptypeID = tbLoan.TreetoptypeID,
                               RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                               InvestTime = tbLoan.InvestTime,
                               Scheduleinvestment = tbLoan.Scheduleinvestment,
                               EndTimeStr = tbLoan.Raisestandard.ToString(),
                               ReleaseTimeStr = tbLoan.SubmitTime.ToString(),
                               StatusName = tbStatus.StatusName,
                               Raisestandard = tbLoan.Raisestandard
                           }).ToList();
            int totalRow = varloan.Count();
            for (int i = 0; i < varloan.Count(); i++)
            {
                varloan[i].scheduleinvestment = Convert.ToString(varloan[i].Scheduleinvestment) +"%";
            }

            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;

            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        public ActionResult chehuiloanbiao(int LoanID)
        {
            try
            {
                var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                               where tbLoan.LoanID == LoanID
                               select new B_LoanTableVo
                               {
                                   LoanID = tbLoan.LoanID,
                               }).Single();
                return Json(varloan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        /// <summary>
        /// 撤标
        /// </summary>
        /// <param name="LoanID"></param>
        /// <returns></returns>
        public ActionResult chebiaojiekuang(B_LoanTable strLoan)
        {
            try
            {
                //定义返回
                string strMsg = "fail";
                var varchebiao = (from tblosn in myDYXTEntities.B_LoanTable
                                  where tblosn.LoanID == strLoan.LoanID
                                  select tblosn).Single();
                varchebiao.StatusID = 27;
                varchebiao.recallaccount = strLoan.recallaccount;
                varchebiao.ManageRemark = strLoan.ManageRemark;
                varchebiao.RemoveTreetopTime = DateTime.Now;
                myDYXTEntities.Entry(varchebiao).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {

                    try
                    {
                        var varinves = (from tbinvest in myDYXTEntities.B_InvestTable
                                        where tbinvest.LoanID == strLoan.LoanID
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

                        //B_InvestTable varinves = (from tbinvest in myDYXTEntities.B_InvestTable
                        //                          where tbinvest.LoanID == strLoan.LoanID
                        //                          select tbinvest).Single();
                        //B_UserTable varsuer = (from tbUser in myDYXTEntities.B_UserTable
                        //                       where tbUser.UserID == varinves.UserID
                        //                       select tbUser).Single();
                        //varsuer.PropertyAmounts = varsuer.PropertyAmounts + varinves.InvestMoney;
                        //varsuer.UsableMoney = varsuer.UsableMoney + varinves.InvestMoney;
                        //myDYXTEntities.Entry(varsuer).State = System.Data.Entity.EntityState.Modified;
                        //myDYXTEntities.SaveChanges();
                    }
                    catch (Exception e)
                    {
                    }
                    strMsg = "success";
                }
                return Json(strMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 失败借款标
        public ActionResult selectfileborrowmoney(BsgridPage bsgridPage)
        {
            var varloan = from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 5
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              ManageRemark = tbLoan.ManageRemark,
                              InvestTime = tbLoan.InvestTime,
                              EndTimeStr = tbLoan.Endtime.ToString(),
                              ReleaseTimeStr = tbLoan.SubmitTime.ToString(),
                              StatusName = tbStatus.StatusName
                          };
            int totalRow = varloan.Count();
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 已过期
        public ActionResult selectbeoverdue(BsgridPage bsgridPage)
        {
            var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 9
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              ManageRemark = tbLoan.ManageRemark,
                              InvestTime = tbLoan.InvestTime,
                              Scheduleinvestment = tbLoan.Scheduleinvestment,
                              EndTimeStr = tbLoan.Raisestandard.ToString(),
                              ReleaseTimeStr = tbLoan.SubmitTime.ToString(),
                              StatusName = tbStatus.StatusName
                          }).ToList();
            int totalRow = varloan.Count();
            for (int i = 0; i < varloan.Count(); i++)
            {
                varloan[i].scheduleinvestment = Convert.ToString(varloan[i].Scheduleinvestment) + "%";
            }
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 延期保存
        /// </summary>
        /// <param name="LoanID"></param>
        /// <param name="tianshu"></param>
        /// <returns></returns>
        public ActionResult yanqitianshu(int LoanID, int tianshu)
        {
            //定义返回
            string strMsg = "fail";
            try
            {
                B_LoanTable varloan = (from tbloan in myDYXTEntities.B_LoanTable
                                       where tbloan.LoanID == LoanID
                                       select tbloan).Single();
                varloan.Raisestandard = varloan.Raisestandard.Value.AddDays(tianshu);
                varloan.StatusID = 7;               
                myDYXTEntities.Entry(varloan).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {
                    strMsg = "success";
                }

            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 流标
        public ActionResult selectsichuan(BsgridPage bsgridPage)
        {
            var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 26
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              ManageRemark = tbLoan.ManageRemark,
                              InvestTime = tbLoan.InvestTime,
                              Scheduleinvestment = tbLoan.Scheduleinvestment,
                              EndTimeStr = tbLoan.RemoveTreetopTime.ToString(),
                              ReleaseTimeStr = tbLoan.SubmitTime.ToString(),
                              StatusName = tbStatus.StatusName
                          }).ToList();
            int totalRow = varloan.Count();
            for (int i = 0; i < varloan.Count(); i++)
            {
                varloan[i].scheduleinvestment = Convert.ToString(varloan[i].Scheduleinvestment) + "%";
            }
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 用户撤标
        public ActionResult selectUserchebiao(BsgridPage bsgridPage, string SLoantitleID, string SPaymentNumberID, string SUserNameID, int STreetoptypeID)
        {
            var varloan = from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 27
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              ManageRemark = tbLoan.ManageRemark,
                              InvestTime = tbLoan.InvestTime,
                              Scheduleinvestment = tbLoan.Scheduleinvestment,
                              EndTimeStr = tbLoan.RemoveTreetopTime.ToString(),
                              ReleaseTimeStr = tbLoan.SubmitTime.ToString(),
                              StatusName = tbStatus.StatusName
                          };
            if (!string.IsNullOrEmpty(SLoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(SLoantitleID));
            }
            if (!string.IsNullOrEmpty(SPaymentNumberID))
            {
                varloan = varloan.Where(n => n.PaymentNumber.Contains(SPaymentNumberID));
            }
            if (!string.IsNullOrEmpty(SUserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(SUserNameID));
            }
            if (STreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == STreetoptypeID);
            }
            int totalRow = varloan.Count();
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region 满标阶段
        /// <summary>
        /// 满标待审核页面
        /// </summary>
        /// <returns></returns>
        public ActionResult selectmanbiaoview()
        {
            return View();
        }
        public ActionResult selectfullscale(BsgridPage bsgridPage, string LoantitleID, string PaymentNumberID, string UserNameID, int TreetoptypeID)
        {
            var varloan = from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 28
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              AlreadyLoanMoney=tbLoan.AlreadyLoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              InvestTime=tbLoan.InvestTime,
                              StatusName = tbStatus.StatusName
                          };
            if (!string.IsNullOrEmpty(LoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(LoantitleID));
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                varloan = varloan.Where(n => n.PaymentNumber.Contains(PaymentNumberID));
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(UserNameID));
            }
            if (TreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == TreetoptypeID);
            }
            int totalRow = varloan.Count();
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// 满标状态
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult SelcttbmanbiaoStatus()
        //{
        //    List<SelectVo> noticeType = (from tbStatus in myDYXTEntities.S_StatusTable
        //                                 where tbStatus.StatusID == 6 || tbStatus.StatusID == 10 || tbStatus.StatusID == 8 || tbStatus.StatusID == 11
        //                                 || tbStatus.StatusID ==28
        //                                 select new SelectVo
        //                                 {
        //                                     id = tbStatus.StatusID,
        //                                     text = tbStatus.StatusName

        //                                 }).ToList();

        //    return Json(noticeType, JsonRequestBehavior.AllowGet);

        //}
        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="strLoan"></param>
        /// <param name="ReleaseTimeStr"></param>
        /// <returns></returns>
        public ActionResult Updatafullscaleloanbaocun(B_LoanTable strLoan, string ReleaseTimeStr, string optionsRadios)
        {
            //定义返回
            string strMsg = "fail";


            try
            {
                B_LoanTable varloan = (from tbloan in myDYXTEntities.B_LoanTable
                                       where tbloan.LoanID == strLoan.LoanID
                                       select tbloan).Single();
                if (strLoan.TreetoptypeID != 6)
                {
                    varloan.Endtime = varloan.SubmitTime.Value.AddMonths(Convert.ToInt32(strLoan.LoanDeadlineID));
                }
                if (strLoan.TreetoptypeID == 6)
                {
                    int jj = Convert.ToInt32(strLoan.LoanDeadlineID) - 12;
                    varloan.Endtime = varloan.SubmitTime.Value.AddDays(jj);
                }
                varloan.LoanDeadlineID = strLoan.LoanDeadlineID;
                varloan.TreetoptypeID = strLoan.TreetoptypeID;
                varloan.RepaymentWayID = strLoan.RepaymentWayID;
              
                varloan.ManageRemark = strLoan.ManageRemark;
                if (optionsRadios == "option2")
                {
                    varloan.StatusID = 6;
                    try
                    {

                        var varinves = (from tbinvest in myDYXTEntities.B_InvestTable
                                        where tbinvest.LoanID == varloan.LoanID
                                        select tbinvest).ToList();
                        for (int j = 0; j < varinves.Count; j++)
                        {
                            int VarUserID =Convert.ToInt32(varinves[j].UserID);
                            B_UserTable varsuer = (from tbUser in myDYXTEntities.B_UserTable
                                                   where tbUser.UserID == VarUserID
                                                   select tbUser).Single();
                            varsuer.PropertyAmounts = varsuer.PropertyAmounts + varinves[j].InvestMoney;
                            varsuer.UsableMoney = varsuer.UsableMoney + varinves[j].InvestMoney;
                            myDYXTEntities.Entry(varsuer).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();
                        }                      
                    }
                    catch (Exception e)
                    {

                    }                  
                }
                if (optionsRadios == "option1")
                {
                    varloan.StatusID =10;
                    try
                    {
                        B_UserTable varsuer = (from tbUser in myDYXTEntities.B_UserTable
                                               where tbUser.UserID == varloan.UserID
                                               select tbUser).Single();
                        varsuer.PropertyAmounts = varsuer.PropertyAmounts + varloan.LoanMoney;
                        varsuer.UsableMoney = varsuer.UsableMoney + varloan.LoanMoney;
                        myDYXTEntities.Entry(varsuer).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();

                       //查询借款标
                        B_LoanTable strloanID = (from tbloan in myDYXTEntities.B_LoanTable
                                               where tbloan.LoanID == strLoan.LoanID
                                               select tbloan).Single();
                        int strintLoanUserID = Convert.ToInt32(strloanID.UserID);//获取借款人ID
                        B_UserTable varuserupdatemoney = (from tbUser in myDYXTEntities.B_UserTable
                                                     where tbUser.UserID == strintLoanUserID
                                                     select tbUser).Single();
                        varuserupdatemoney.CompensatoryMoney = varuserupdatemoney.CompensatoryMoney + strloanID.RepayPrincipal;
                        myDYXTEntities.Entry(varuserupdatemoney).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();//修改用户的代还金额
                        //获取借款标ID
                        int strintLoanID = Convert.ToInt32(strloanID.LoanID);
                        //根据借款标查询投资人Id
                        var strInvest= (from tbInvest in myDYXTEntities.B_InvestTable
                                        where tbInvest.LoanID == strintLoanID
                                        select tbInvest).ToList();
                        for (int i = 0; i < strInvest.Count; i++)
                        {

                            int UserID =Convert.ToInt32(strInvest[i].UserID);//获取到投资人

                            B_UserTable varuserupdate= (from tbUser in myDYXTEntities.B_UserTable
                                                        where tbUser.UserID == UserID
                                                        select tbUser).Single();

                            int jifen = Convert.ToInt32(strInvest[i].InvestMoney) / 1000;

                            varuserupdate.Integral = varuserupdate.Integral + jifen;

                            varuserupdate.FreezeMoney = varuserupdate.FreezeMoney - strInvest[i].InvestMoney;
                            varuserupdate.WaitMoney = varuserupdate.WaitMoney + strInvest[i].InvestMoney;
                            myDYXTEntities.Entry(varuserupdate).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();//修改用户的冻结金额和代收金额


                            var strSpreadUserTable = (from tbSpreadUser in myDYXTEntities.B_SpreadUserTable
                                                      where tbSpreadUser.UserID == UserID
                                                      select tbSpreadUser).ToList();
                            for (int j = 0; j < strSpreadUserTable.Count; j++)
                            {                             
                                int intSpreadTypeID = Convert.ToInt32(strSpreadUserTable[i].SpreadTypeID);

                                B_SpreadTypeTable varB_SpreadType= (from tbSpreadType in myDYXTEntities.B_SpreadTypeTable
                                                                    where tbSpreadType.SpreadTypeID == intSpreadTypeID
                                                                    select tbSpreadType).Single();
                                decimal intSpreadTypeTable =Convert.ToDecimal(varB_SpreadType.Scale.Trim());//获取提成比例
                                int SpreadRenID = Convert.ToInt32(strSpreadUserTable[i].SpreadRenID);//获取推广人
                                int IntstrUserID = Convert.ToInt32(strSpreadUserTable[i].UserID);//获取推广客户
                                B_PersonalSpreadMessage varPersonalSpreadMessage = (from tbPersonalSpreadMessage in myDYXTEntities.B_PersonalSpreadMessage
                                                                                    where tbPersonalSpreadMessage.SpreadUserID == SpreadRenID
                                                                                     select tbPersonalSpreadMessage).Single();
                                varPersonalSpreadMessage.InvestTime = varPersonalSpreadMessage.InvestTime + 1;
                                varPersonalSpreadMessage.InvestAmount = varPersonalSpreadMessage.InvestAmount + varloan.LoanMoney;
                                varPersonalSpreadMessage.InvestTiCheng = varPersonalSpreadMessage.InvestTiCheng + (varloan.LoanMoney * intSpreadTypeTable);
                                myDYXTEntities.Entry(varPersonalSpreadMessage).State = System.Data.Entity.EntityState.Modified;
                                myDYXTEntities.SaveChanges();
                               
                                B_UserTable varSpreadRenID = (from tbUser in myDYXTEntities.B_UserTable
                                                       where tbUser.UserID == SpreadRenID
                                                       select tbUser).Single();
                                varSpreadRenID.PropertyAmounts = varSpreadRenID.PropertyAmounts + (varloan.LoanMoney * intSpreadTypeTable);
                                varSpreadRenID.UsableMoney = varSpreadRenID.UsableMoney + (varloan.LoanMoney * intSpreadTypeTable);
                                myDYXTEntities.Entry(varPersonalSpreadMessage).State = System.Data.Entity.EntityState.Modified;
                                myDYXTEntities.SaveChanges();

                                B_SpreadRecordTable strSpreadRecord = new B_SpreadRecordTable();
                                strSpreadRecord.SpreadUserID = SpreadRenID;
                                strSpreadRecord.SpreadCustomID = IntstrUserID;
                                strSpreadRecord.SpreadTypeID = intSpreadTypeID;
                                strSpreadRecord.FundType = "本金";
                                strSpreadRecord.SubmitTime = DateTime.Now;
                                strSpreadRecord.SpreadAmount = Convert.ToDecimal(varloan.LoanMoney * intSpreadTypeTable);
                                strSpreadRecord.Remark = "推广客户【" + varsuer.UserName.Trim() + "】投资【" + strloanID.Loantitle.Trim() +"】"+Convert.ToDecimal(varloan.LoanMoney * intSpreadTypeTable).ToString("0")+ "元成功所得的推广费。";
                                myDYXTEntities.B_SpreadRecordTable.Add(strSpreadRecord);
                                myDYXTEntities.SaveChanges();
                            }
                        }


                    }
                    catch (Exception e)
                    {
                       
                    }
                }
                //修改保存
                myDYXTEntities.Entry(varloan).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {
                    B_UserTable usermoney = (from tbUser in myDYXTEntities.B_UserTable
                                           where tbUser.UserID == varloan.UserID
                                           select tbUser).Single();

                    B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                    Capitalrecord.UserID = varloan.UserID;
                    Capitalrecord.OperateTypeID = 12;
                    Capitalrecord.OpFare = Convert.ToDecimal(varloan.LoanMoney);
                    Capitalrecord.Income = Convert.ToDecimal(varloan.LoanMoney);
                    Capitalrecord.Expend = Convert.ToDecimal(0);
                    Capitalrecord.PropertyAmounts = usermoney.PropertyAmounts;
                    Capitalrecord.Remarks = "用户" + "[" + usermoney.UserName.Trim() + "]" + "，借款成功，余额增加" + varloan.LoanMoney + "元";
                    Capitalrecord.operatetime = DateTime.Now;
                    myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                    myDYXTEntities.SaveChanges();                 
                    strMsg = "success";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        #region 满标审核失败
        public ActionResult selectfilefullscale(BsgridPage bsgridPage, string SbLoantitleID, string SbPaymentNumberID, string SbUserNameID, int SbTreetoptypeID)
        {
            var varloan = from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 6
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              AlreadyLoanMoney = tbLoan.AlreadyLoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              InvestTime = tbLoan.InvestTime,
                              StatusName = tbStatus.StatusName
                          };
            if (!string.IsNullOrEmpty(SbLoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(SbLoantitleID));
            }
            if (!string.IsNullOrEmpty(SbPaymentNumberID))
            {
                varloan = varloan.Where(n => n.PaymentNumber.Contains(SbPaymentNumberID));
            }
            if (!string.IsNullOrEmpty(SbUserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(SbUserNameID));
            }
            if (SbTreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == SbTreetoptypeID);
            }
            int totalRow = varloan.Count();
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 正在还款借标
        public ActionResult selectjiekuanfullscale(BsgridPage bsgridPage, string JkLoantitleID, string JkPaymentNumberID, string JkUserNameID, int JkTreetoptypeID)
        {
            var varloan = from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 10|| tbLoan.StatusID == 8|| tbLoan.StatusID == 11
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              AlreadyLoanMoney = tbLoan.AlreadyLoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              InvestTime = tbLoan.InvestTime,
                              StatusName = tbStatus.StatusName
                          };
            if (!string.IsNullOrEmpty(JkLoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(JkLoantitleID));
            }
            if (!string.IsNullOrEmpty(JkPaymentNumberID))
            {
                varloan = varloan.Where(n => n.PaymentNumber.Contains(JkPaymentNumberID));
            }
            if (!string.IsNullOrEmpty(JkUserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(JkUserNameID));
            }
            if (JkTreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == JkTreetoptypeID);
            }
            int totalRow = varloan.Count();

            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 已还完款 
        public ActionResult selectyihaiwanjiekuanfullscale(BsgridPage bsgridPage, string YhLoantitleID, string YhPaymentNumberID, string YhUserNameID, int YhTreetoptypeID)
        {
            var varloan = from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 12
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              AlreadyLoanMoney = tbLoan.AlreadyLoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              InvestTime = tbLoan.InvestTime,
                              StatusName = tbStatus.StatusName
                          };
            if (!string.IsNullOrEmpty(YhLoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(YhLoantitleID));
            }
            if (!string.IsNullOrEmpty(YhPaymentNumberID))
            {
                varloan = varloan.Where(n => n.PaymentNumber.Contains(YhPaymentNumberID));
            }
            if (!string.IsNullOrEmpty(YhUserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(YhUserNameID));
            }
            if (YhTreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == YhTreetoptypeID);
            }
            int totalRow = varloan.Count();
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 提前还款
        public ActionResult selecttiqianhaikuanfullscale(BsgridPage bsgridPage, string SLoantitleID, string SPaymentNumberID, string SUserNameID, int STreetoptypeID)
        {
            var varloan = from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 13
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,
                              LoanMoney = tbLoan.LoanMoney,
                              AlreadyLoanMoney = tbLoan.AlreadyLoanMoney,
                              Rate = tbLoan.Rate,
                              LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                              Treetoptype = tbTreetoptype.Treetoptype,
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                              InvestTime = tbLoan.InvestTime,
                              StatusName = tbStatus.StatusName
                          };
            if (!string.IsNullOrEmpty(SLoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(SLoantitleID));
            }
            if (!string.IsNullOrEmpty(SPaymentNumberID))
            {
                varloan = varloan.Where(n => n.PaymentNumber.Contains(SPaymentNumberID));
            }
            if (!string.IsNullOrEmpty(SUserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(SUserNameID));
            }
            if (STreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == STreetoptypeID);
            }
            int totalRow = varloan.Count();
            List<B_LoanTableVo> notices = varloan.OrderBy(p => p.LoanID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region 流转标借款 
        public ActionResult roambiaoloanview()
        {
            return View();
        }
        /// <summary>
        /// 全部查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="PaymentNumberID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectExchangeTreetopLoan(BsgridPage bsgridPage,string LoantitleID,string PaymentNumberID,string UserNameID, string releaseTime, string EnReleaseTime)
        {

            List<B_LoanTableVo> Recharge = (from tbLoan in myDYXTEntities.B_LoanTable
                                             join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                                             join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID                                            
                                             join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                                             where tbLoan.TreetoptypeID==7&&(tbLoan.StatusID==8|| tbLoan.StatusID==11|| tbLoan.StatusID == 1)
                                             select new B_LoanTableVo
                                             {
                                                 LoanID = tbLoan.LoanID,
                                                 PaymentNumber= tbLoan.PaymentNumber,
                                                 UserName= tbUser.UserName,
                                                 Loantitle= tbLoan.Loantitle,
                                                 LoanMoney = tbLoan.LoanMoney,
                                                 Rate= tbLoan.Rate,
                                                 LoanDeadlineName=tbLoanDeadline.LoanDeadlineName,
                                                 LowestTenderMoney = tbLoan.LowestTenderMoney,
                                                 Grossscore = tbLoan.Grossscore,
                                                 AlreadyLoan = tbLoan.AlreadyLoan,
                                                 SurplusLoan = tbLoan.SurplusLoan,
                                                 stopbuyback = tbLoan.stopbuyback,
                                                AddTimeStr= tbLoan.SubmitTime.ToString(),
                                                 StatusName= tbStatus.StatusName
                                             }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            if (!string.IsNullOrEmpty(LoantitleID))
            {
                Recharge = Recharge.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                Recharge = Recharge.Where(n => n.PaymentNumber.Contains(PaymentNumberID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.AddTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.AddTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = Recharge.Count();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="ExchangeTreetopID"></param>
        /// <returns></returns>
        public ActionResult selectExchangeTreetopXinxi(int LoanID)
        {
            try
            {
                var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                               join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                               join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                               join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                               where tbLoan.LoanID== LoanID
                               select new B_LoanTableVo
                               {
                                   LoanID = tbLoan.LoanID,
                                   PaymentNumber = tbLoan.PaymentNumber,
                                   UserName = tbUser.UserName,
                                   Loantitle = tbLoan.Loantitle,
                                   LoanMoney = tbLoan.LoanMoney,
                                   Rate = tbLoan.Rate,
                                   LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                   LowestTenderMoney = tbLoan.LowestTenderMoney,
                                   Grossscore = tbLoan.Grossscore,
                                   AlreadyLoan = tbLoan.AlreadyLoan,
                                   SurplusLoan = tbLoan.SurplusLoan,
                                   stopbuyback = tbLoan.stopbuyback,
                                   AddTimeStr = tbLoan.SubmitTime.ToString(),
                                   StatusName = tbStatus.StatusName
                               }).Single();
                return Json(varloan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
        }
        /// <summary>
        /// 初审查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="PaymentNumberID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectshenheExchangeTreetopLoan(BsgridPage bsgridPage, string LoantitleID, string PaymentNumberID, string UserNameID, string releaseTime, string EnReleaseTime)
        {

            List<B_LoanTableVo> Recharge = (from tbLoan in myDYXTEntities.B_LoanTable
                                            join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                                            join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                                            join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                                            where tbLoan.TreetoptypeID == 7 && tbLoan.StatusID == 1
                                            select new B_LoanTableVo
                                            {
                                                LoanID = tbLoan.LoanID,
                                                PaymentNumber = tbLoan.PaymentNumber,
                                                UserName = tbUser.UserName,
                                                Loantitle = tbLoan.Loantitle,
                                                LoanMoney = tbLoan.LoanMoney,
                                                Rate = tbLoan.Rate,
                                                LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                                LowestTenderMoney = tbLoan.LowestTenderMoney,
                                                Grossscore = tbLoan.Grossscore,
                                                AlreadyLoan = tbLoan.AlreadyLoan,
                                                SurplusLoan = tbLoan.SurplusLoan,
                                                stopbuyback = tbLoan.stopbuyback,
                                                AddTimeStr = tbLoan.SubmitTime.ToString(),
                                                StatusName = tbStatus.StatusName
                                            }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            if (!string.IsNullOrEmpty(LoantitleID))
            {
                Recharge = Recharge.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                Recharge = Recharge.Where(n => n.PaymentNumber.Contains(PaymentNumberID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.AddTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.AddTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = Recharge.Count();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询审核ID
        /// </summary>
        /// <param name="ExchangeTreetopID"></param>
        /// <returns></returns>
        public ActionResult selectExchangeTreetopID(int LoanID)
        {
            try
            {
                var varloan = (from tbLoan in myDYXTEntities.B_LoanTable                           
                               where tbLoan.LoanID == LoanID
                               select new {
                                   tbLoan.LoanID
                               }).Single();
                return Json(varloan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 修改审核
        /// </summary>
        /// <param name="ExchangeTreetop"></param>
        /// <param name="optionsRadios"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        public ActionResult updataExchangeTreetoppreserve(B_LoanTable ExchangeTreetop, string optionsRadios, string validCode)
        {
            //定义返回
            string strMsg = "fail";
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
            if (strSessionValidCode.Equals(validCode.Trim(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (optionsRadios == "option1")
                {
                    B_LoanTable varExchangeTreetopLoan = (from tbLoan in myDYXTEntities.B_LoanTable
                                                          where tbLoan.LoanID == ExchangeTreetop.LoanID
                                                          select tbLoan).Single();
                    varExchangeTreetopLoan.StatusID=8;
                    varExchangeTreetopLoan.ManageRemark = ExchangeTreetop.ManageRemark;
                    myDYXTEntities.Entry(varExchangeTreetopLoan).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
                else
                {
                    B_LoanTable varExchangeTreetopLoan = (from tbLoan in myDYXTEntities.B_LoanTable
                                                          where tbLoan.LoanID == ExchangeTreetop.LoanID
                                                          select tbLoan).Single();
                    varExchangeTreetopLoan.StatusID = 17;
                    varExchangeTreetopLoan.ManageRemark = ExchangeTreetop.ManageRemark;
                    myDYXTEntities.Entry(varExchangeTreetopLoan).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();


                    strMsg = "success";
                }
            }
            else
            {
                strMsg = "ValidCodeErro";//验证码错误
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 流转中查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="PaymentNumberID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectliuzhaungzhongExchangeTreetopLoan(BsgridPage bsgridPage, string LoantitleID, string PaymentNumberID, string UserNameID, string releaseTime, string EnReleaseTime)
        {
            List<B_LoanTableVo> Recharge = (from tbLoan in myDYXTEntities.B_LoanTable
                                            join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                                            join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                                            join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                                            where tbLoan.TreetoptypeID == 7 && tbLoan.StatusID == 8
                                            select new B_LoanTableVo
                                            {
                                                LoanID = tbLoan.LoanID,
                                                PaymentNumber = tbLoan.PaymentNumber,
                                                UserName = tbUser.UserName,
                                                Loantitle = tbLoan.Loantitle,
                                                LoanMoney = tbLoan.LoanMoney,
                                                Rate = tbLoan.Rate,
                                                LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                                LowestTenderMoney = tbLoan.LowestTenderMoney,
                                                Grossscore = tbLoan.Grossscore,
                                                AlreadyLoan = tbLoan.AlreadyLoan,
                                                SurplusLoan = tbLoan.SurplusLoan,
                                                stopbuyback = tbLoan.stopbuyback,
                                                AddTimeStr = tbLoan.SubmitTime.ToString(),
                                                StatusName = tbStatus.StatusName
                                            }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            if (!string.IsNullOrEmpty(LoantitleID))
            {
                Recharge = Recharge.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                Recharge = Recharge.Where(n => n.PaymentNumber.Contains(PaymentNumberID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.AddTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.AddTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = Recharge.Count();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 回购中查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="PaymentNumberID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selecthuigouExchangeTreetopLoan(BsgridPage bsgridPage, string LoantitleID, string PaymentNumberID, string UserNameID, string releaseTime, string EnReleaseTime)
        {

            List<B_LoanTableVo> Recharge = (from tbLoan in myDYXTEntities.B_LoanTable
                                            join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                                            join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                                            join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                                            where tbLoan.TreetoptypeID == 7 && tbLoan.StatusID == 11
                                            select new B_LoanTableVo
                                            {
                                                LoanID = tbLoan.LoanID,
                                                PaymentNumber = tbLoan.PaymentNumber,
                                                UserName = tbUser.UserName,
                                                Loantitle = tbLoan.Loantitle,
                                                LoanMoney = tbLoan.LoanMoney,
                                                Rate = tbLoan.Rate,
                                                LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                                LowestTenderMoney = tbLoan.LowestTenderMoney,
                                                Grossscore = tbLoan.Grossscore,
                                                AlreadyLoan = tbLoan.AlreadyLoan,
                                                SurplusLoan = tbLoan.SurplusLoan,
                                                stopbuyback = tbLoan.stopbuyback,
                                                AddTimeStr = tbLoan.SubmitTime.ToString(),
                                                StatusName = tbStatus.StatusName
                                            }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            if (!string.IsNullOrEmpty(LoantitleID))
            {
                Recharge = Recharge.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                Recharge = Recharge.Where(n => n.PaymentNumber.Contains(PaymentNumberID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.AddTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.AddTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = Recharge.Count();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 已回购借款
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="PaymentNumberID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectYihuigouExchangeTreetopLoan(BsgridPage bsgridPage, string LoantitleID, string PaymentNumberID, string UserNameID, string releaseTime, string EnReleaseTime)
        {

            List<B_LoanTableVo> Recharge = (from tbLoan in myDYXTEntities.B_LoanTable
                                            join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                                            join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                                            join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                                            where tbLoan.TreetoptypeID == 7 && tbLoan.StatusID == 29
                                            select new B_LoanTableVo
                                            {
                                                LoanID = tbLoan.LoanID,
                                                PaymentNumber = tbLoan.PaymentNumber,
                                                UserName = tbUser.UserName,
                                                Loantitle = tbLoan.Loantitle,
                                                LoanMoney = tbLoan.LoanMoney,
                                                Rate = tbLoan.Rate,
                                                LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                                LowestTenderMoney = tbLoan.LowestTenderMoney,
                                                Grossscore = tbLoan.Grossscore,
                                                AlreadyLoan = tbLoan.AlreadyLoan,
                                                SurplusLoan = tbLoan.SurplusLoan,
                                                stopbuyback = tbLoan.stopbuyback,
                                                AddTimeStr = tbLoan.SubmitTime.ToString(),
                                                StatusName = tbStatus.StatusName
                                            }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            if (!string.IsNullOrEmpty(LoantitleID))
            {
                Recharge = Recharge.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                Recharge = Recharge.Where(n => n.PaymentNumber.Contains(PaymentNumberID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.AddTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.AddTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = Recharge.Count();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 失败借款
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="PaymentNumberID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectfailborrowmoney(BsgridPage bsgridPage, string LoantitleID, string PaymentNumberID, string UserNameID, string releaseTime, string EnReleaseTime)
        {

            List<B_LoanTableVo> Recharge = (from tbLoan in myDYXTEntities.B_LoanTable
                                            join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                                            join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                                            join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                                            where tbLoan.TreetoptypeID == 7 && tbLoan.StatusID == 17
                                            select new B_LoanTableVo
                                            {
                                                LoanID = tbLoan.LoanID,
                                                PaymentNumber = tbLoan.PaymentNumber,
                                                UserName = tbUser.UserName,
                                                Loantitle = tbLoan.Loantitle,
                                                LoanMoney = tbLoan.LoanMoney,
                                                Rate = tbLoan.Rate,
                                                LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                                LowestTenderMoney = tbLoan.LowestTenderMoney,
                                                Grossscore = tbLoan.Grossscore,
                                                AlreadyLoan = tbLoan.AlreadyLoan,
                                                SurplusLoan = tbLoan.SurplusLoan,
                                                stopbuyback = tbLoan.stopbuyback,
                                                AddTimeStr = tbLoan.SubmitTime.ToString(),
                                                StatusName = tbStatus.StatusName
                                            }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            if (!string.IsNullOrEmpty(LoantitleID))
            {
                Recharge = Recharge.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                Recharge = Recharge.Where(n => n.PaymentNumber.Contains(PaymentNumberID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.AddTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.AddTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = Recharge.Count();
            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 逾期借款
        /// <summary>
        ///逾期借款页面
        /// </summary>
        /// <returns></returns>
        public ActionResult overdueloanview()
        {
            return View();
        }
        /// <summary>
        /// 	逾期借款 
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="TreetoptypeID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectoverdueloan(BsgridPage bsgridPage,string LoantitleID,string UserNameID,int TreetoptypeID, string releaseTime, string EnReleaseTime)
        {

            List<B_OverdueLoanTableVo> Recharge = (from tbOverdueLoan in myDYXTEntities.B_OverdueLoanTable
                                                   join tbLoan in myDYXTEntities.B_LoanTable on tbOverdueLoan.LoanID equals tbLoan.LoanID
                                                   join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                                                   join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                                                   join tbStatuss in myDYXTEntities.S_StatusTable on tbOverdueLoan.WebsiteWhetherPayStatusID equals tbStatuss.StatusID
                                                   join tbStatus in myDYXTEntities.S_StatusTable on tbOverdueLoan.StatusID equals tbStatus.StatusID
                                                   select new B_OverdueLoanTableVo
                                                   {
                                                       OverdueLoanID = tbOverdueLoan.OverdueLoanID,
                                                       PaymentNumber = tbLoan.PaymentNumber,
                                                       UserName = tbUser.UserName,
                                                       Loantitle = tbLoan.Loantitle,
                                                       Treetoptype = tbTreetoptype.Treetoptype,
                                                       TreetoptypeID=tbLoan.TreetoptypeID,
                                                       ReleaseTimeStr = tbOverdueLoan.PayableTime.ToString(),
                                                       PayablePrincipal = tbOverdueLoan.PayablePrincipal,
                                                       OverdueDay = tbOverdueLoan.OverdueDay,
                                                       WebsiteWhetherPayStatus = tbStatuss.StatusName,
                                                       StatusName = tbStatus.StatusName,
                                                       TimeStr=tbOverdueLoan.RealityRepaymentTime.ToString()
                                                   }).OrderByDescending(p => p.OverdueLoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            if (!string.IsNullOrEmpty(LoantitleID))
            {
                Recharge = Recharge.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (TreetoptypeID > 0)
            {
                Recharge = Recharge.Where(p => p.TreetoptypeID == TreetoptypeID).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.ReleaseTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.ReleaseTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = myDYXTEntities.B_OverdueLoanTable.Count();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].YuQiTianShu = Convert.ToString(Recharge[i].OverdueDay) + "天";
            }
   
            Bsgrid<B_OverdueLoanTableVo> bsgrid = new Bsgrid<B_OverdueLoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 网站垫付
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="TreetoptypeID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectoverdueadvance(BsgridPage bsgridPage, string LoantitleID, string UserNameID, int TreetoptypeID, string releaseTime, string EnReleaseTime)
        {

            List<B_OverdueLoanTableVo> Recharge = (from tbOverdueLoan in myDYXTEntities.B_OverdueLoanTable
                                                   join tbLoan in myDYXTEntities.B_LoanTable on tbOverdueLoan.LoanID equals tbLoan.LoanID
                                                   join tbUser in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUser.UserID
                                                   join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                                                   join tbStatuss in myDYXTEntities.S_StatusTable on tbOverdueLoan.WebsiteWhetherPayStatusID equals tbStatuss.StatusID
                                                   join tbStatus in myDYXTEntities.S_StatusTable on tbOverdueLoan.StatusID equals tbStatus.StatusID
                                                   where tbOverdueLoan.WebsiteWhetherPayStatusID==33
                                                   select new B_OverdueLoanTableVo
                                                   {
                                                       OverdueLoanID = tbOverdueLoan.OverdueLoanID,
                                                       PaymentNumber = tbLoan.PaymentNumber,
                                                       UserName = tbUser.UserName,
                                                       Loantitle = tbLoan.Loantitle,
                                                       Treetoptype = tbTreetoptype.Treetoptype,
                                                       TreetoptypeID = tbLoan.TreetoptypeID,
                                                       ReleaseTimeStr = tbOverdueLoan.PayableTime.ToString(),
                                                       PayablePrincipal = tbOverdueLoan.PayablePrincipal,
                                                       OverdueDay = tbOverdueLoan.OverdueDay,
                                                       WebsiteWhetherPayStatus = tbStatuss.StatusName,
                                                       StatusName = tbStatus.StatusName,
                                                       WebsiteWhetherPaymoney = tbOverdueLoan.WebsiteWhetherPaymoney,
                                                       WebsiteWhetherPayTimeStr = tbOverdueLoan.WebsiteWhetherPayTime.ToString(),
                                                       TimeStr = tbOverdueLoan.RealityRepaymentTime.ToString()
                                                   }).OrderByDescending(p => p.OverdueLoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            if (!string.IsNullOrEmpty(LoantitleID))
            {
                Recharge = Recharge.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (TreetoptypeID > 0)
            {
                Recharge = Recharge.Where(p => p.TreetoptypeID == TreetoptypeID).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.ReleaseTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.ReleaseTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = myDYXTEntities.B_OverdueLoanTable.Count();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].YuQiTianShu = Convert.ToString(Recharge[i].OverdueDay).Trim() + "天";
            }

            Bsgrid<B_OverdueLoanTableVo> bsgrid = new Bsgrid<B_OverdueLoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 网站垫付绑定
        /// </summary>
        /// <param name="OverdueLoanID"></param>
        /// <returns></returns>
        public ActionResult selectwebadvance(int OverdueLoanID)
        {
            try
            {
                var varloan = (from tbOverdueLoan in myDYXTEntities.B_OverdueLoanTable
                               join tbLoan in myDYXTEntities.B_LoanTable on tbOverdueLoan.LoanID equals tbLoan.LoanID
                               join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID                             
                               join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID                              
                               join tbStatus in myDYXTEntities.S_StatusTable on tbOverdueLoan.WebsiteWhetherPayStatusID equals tbStatus.StatusID
                               where tbOverdueLoan.OverdueLoanID == OverdueLoanID
                               select new B_OverdueLoanTableVo
                               {
                                   OverdueLoanID = tbOverdueLoan.OverdueLoanID,
                                   UserID=tbLoan.UserID,
                                   LoanID= tbOverdueLoan.LoanID,
                                   UserName = tbUSer.UserName,
                                   Loantitle = tbLoan.Loantitle,
                                   Treetoptype= tbTreetoptype.Treetoptype,
                                   ReleaseTimeStr= tbOverdueLoan.PayableTime.ToString(),
                                   PayablePrincipal=tbOverdueLoan.PayablePrincipal,
                                   OverdueDay=tbOverdueLoan.OverdueDay,
                                   LoanMoney = tbLoan.LoanMoney,                                 
                                   StatusName = tbStatus.StatusName
                               }).Single();
                varloan.YuQiTianShu = varloan.OverdueDay + "天";
                return Json(varloan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        /// <summary>
        /// 网站垫付保存
        /// </summary>
        /// <param name="strOverdueLoan"></param>
        /// <param name="LoanMoney"></param>
        /// <param name="LoanID"></param>
        /// <returns></returns>
        public ActionResult QueRenadvance(B_OverdueLoanTable strOverdueLoan,decimal LoanMoney,int LoanID)
        {
            //定义返回
            string strMsg = "fail";
            try
            {
                B_OverdueLoanTable varOverdueLoan = (from tbOverdueLoan in myDYXTEntities.B_OverdueLoanTable
                                                     where tbOverdueLoan.OverdueLoanID == strOverdueLoan.OverdueLoanID
                                                     select tbOverdueLoan).Single();
                varOverdueLoan.WebsiteWhetherPayStatusID = 32;
                varOverdueLoan.WebsiteWhetherPaymoney = LoanMoney;
                varOverdueLoan.WebsiteWhetherPayTime = DateTime.Now;
                myDYXTEntities.Entry(varOverdueLoan).State = System.Data.Entity.EntityState.Modified;
                myDYXTEntities.SaveChanges();
                try
                {
                    var varInvest = (from tbInvest in myDYXTEntities.B_InvestTable
                                     where tbInvest.LoanID == LoanID
                                     select tbInvest).ToList();
                    for (int i = 0; i < varInvest.Count; i++)
                    {
                        int UserID = Convert.ToInt32(varInvest[i].UserID);
                        B_UserTable varUser = (from tbUser in myDYXTEntities.B_UserTable
                                               where tbUser.UserID == UserID
                                               select tbUser).Single();
                        varUser.PropertyAmounts = varUser.PropertyAmounts + varInvest[i].InvestMoney;
                        varUser.UsableMoney = varUser.UsableMoney + varInvest[i].InvestMoney;
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                    }
                }
                catch (Exception e)
                {

                }
                B_LoanTable varloan = (from tbloan in myDYXTEntities.B_LoanTable
                                       where tbloan.LoanID == LoanID
                                       select tbloan).Single();
                B_WebsiteReceivableDetail varWebsiteReceivableDetail = new B_WebsiteReceivableDetail();
                varWebsiteReceivableDetail.OverdueLoanID = varOverdueLoan.OverdueLoanID;
                varWebsiteReceivableDetail.Loantitle = varloan.Loantitle;
                varWebsiteReceivableDetail.ReceivableDate = varOverdueLoan.PayableTime;
                varWebsiteReceivableDetail.UserID = varloan.UserID;
                varWebsiteReceivableDetail.GatheringMoney = varOverdueLoan.WebsiteWhetherPaymoney;
                varWebsiteReceivableDetail.ReceivableCapital= varOverdueLoan.WebsiteWhetherPaymoney;
                varWebsiteReceivableDetail.ReceivableAccrual = varOverdueLoan.PayablePrincipal - varOverdueLoan.WebsiteWhetherPaymoney;
                varWebsiteReceivableDetail.OverdueDays = varOverdueLoan.OverdueDay;
                varWebsiteReceivableDetail.StatusID = 31;
                myDYXTEntities.B_WebsiteReceivableDetail.Add(varWebsiteReceivableDetail);
                myDYXTEntities.SaveChanges();
             
                B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                int strAccountID = Convert.ToInt32(Session["AccountID"]);
                WebsiteExpenses.AccountID = strAccountID;
                B_UserTable strUser = (from tbUser in myDYXTEntities.B_UserTable
                                       where tbUser.AccountID == strAccountID
                                       select tbUser).Single();
                WebsiteExpenses.OperateTypeID = 13;
                WebsiteExpenses.OperateMoney = Convert.ToDecimal(LoanMoney);
                WebsiteExpenses.Earning = Convert.ToDecimal(0);
                WebsiteExpenses.Expenses = Convert.ToDecimal(LoanMoney);
                int intUserID =Convert.ToInt32( varloan.UserID);
                B_UserTable strvarUser = (from tbUser in myDYXTEntities.B_UserTable
                                          where tbUser.UserID == intUserID
                                          select tbUser).Single();
                WebsiteExpenses.Remark = "【"+strvarUser.UserName.Trim()+"】" + "借款逾期；"+ "[" + strUser.UserName.Trim() + "]"+ "操作网站正常垫付[" + LoanMoney + "]元";
                WebsiteExpenses.OperateTime = DateTime.Now;
                myDYXTEntities.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult selectoverdueadvancemingxizhang(BsgridPage bsgridPage,int TreetoptypeID, string releaseTime, string EnReleaseTime)
        {

            List<B_WebsiteReceivableDetailVo> Recharge = (from tbWebsiteReceivable in myDYXTEntities.B_WebsiteReceivableDetail                                               
                                                   join tbUser in myDYXTEntities.B_UserTable on tbWebsiteReceivable.UserID equals tbUser.UserID
                                                   join tbStatuss in myDYXTEntities.S_StatusTable on tbWebsiteReceivable.StatusID equals tbStatuss.StatusID                                                
                                                   select new B_WebsiteReceivableDetailVo
                                                   {
                                                       WebsiteReceivableDetailID=tbWebsiteReceivable.WebsiteReceivableDetailID,
                                                       Loantitle=tbWebsiteReceivable.Loantitle,
                                                       TimeStr=tbWebsiteReceivable.ReceivableDate.ToString(),
                                                       UserName=tbUser.UserName,
                                                       GatheringMoney= tbWebsiteReceivable.GatheringMoney,
                                                       ReceivableCapital=tbWebsiteReceivable.ReceivableCapital,
                                                       ReceivableAccrual= tbWebsiteReceivable.ReceivableAccrual,
                                                       OverdueDays= tbWebsiteReceivable.OverdueDays,
                                                       WebsiteWhetherPayTimeStr=tbWebsiteReceivable.RealityTime.ToString(),
                                                       RealityAmount=tbWebsiteReceivable.RealityAmount,
                                                       StatusID= tbWebsiteReceivable.StatusID,
                                                       StatusName =tbStatuss.StatusName,
                                                   }).OrderByDescending(p => p.WebsiteReceivableDetailID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();


            if (TreetoptypeID > 0)
            {
                Recharge = Recharge.Where(p => p.StatusID == TreetoptypeID).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    Recharge = Recharge.Where(p => Convert.ToDateTime(p.TimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.TimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = myDYXTEntities.B_WebsiteReceivableDetail.Count();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].YuQiTianShu = Convert.ToString(Recharge[i].OverdueDays).Trim() + "天";
            }

            Bsgrid<B_WebsiteReceivableDetailVo> bsgrid = new Bsgrid<B_WebsiteReceivableDetailVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 网站垫付明细账 状态
        /// </summary>
        /// <returns></returns>
        public ActionResult selectStatusstr()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbStatus in myDYXTEntities.S_StatusTable
                                         where tbStatus.StatusID==30||tbStatus.StatusID==31
                                         select new SelectVo
                                         {
                                             id = tbStatus.StatusID,
                                             text = tbStatus.StatusName

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region 借款额度
        public ActionResult borrowmoneylimit()
        {
            return View();
        }
        /// <summary>
        ///	额度列表
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="UserNameID"></param>
        /// <returns></returns>
        public ActionResult selectborrowmoneylimit(BsgridPage bsgridPage,string UserNameID)
        {

            List<B_UserlimitTableVo> Recharge = (from tbUserlimit in myDYXTEntities.B_UserlimitTable                                                
                                                   join tbUser in myDYXTEntities.B_UserTable on tbUserlimit.UserID equals tbUser.UserID                                                 
                                                   select new B_UserlimitTableVo
                                                   {
                                                       UserlimitID=tbUserlimit.UserlimitID,
                                                       UserID=tbUserlimit.UserID,
                                                       UserName = tbUser.UserName,
                                                       Creditlimetl= tbUserlimit.Creditlimetl,
                                                       Cavailable=tbUserlimit.Cavailable,
                                                       Cfreeze=tbUserlimit.Cfreeze,
                                                       Warrantlimetl=tbUserlimit.Warrantlimetl,
                                                       Wavailable=tbUserlimit.Wavailable,
                                                       Wfreeze= tbUserlimit.Wfreeze,
                                                       Mortgagelimitli= tbUserlimit.Mortgagelimitli,
                                                       Mavailable= tbUserlimit.Mavailable,
                                                       Mfreeze=tbUserlimit.Mfreeze,
                                                       Roamlitil=tbUserlimit.Roamlitil,
                                                       Ravailable= tbUserlimit.Ravailable,
                                                       Rfreeze=tbUserlimit.Rfreeze
                                                   }).OrderBy(p => p.UserlimitID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();          
            int totalRow = myDYXTEntities.B_UserlimitTable.Count();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].Creditlimetlmoeny = Convert.ToString(Recharge[i].Cavailable) +"|"+ Convert.ToString(Recharge[i].Cfreeze);
                Recharge[i].Warrantlimetlmoney = Convert.ToString(Recharge[i].Wavailable) + "|" + Convert.ToString(Recharge[i].Wfreeze);
                Recharge[i].Mortgagelimitlimoney = Convert.ToString(Recharge[i].Mavailable) + "|" + Convert.ToString(Recharge[i].Mfreeze);
                Recharge[i].Roamlitillimoney = Convert.ToString(Recharge[i].Ravailable) + "|" + Convert.ToString(Recharge[i].Rfreeze);
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            Bsgrid<B_UserlimitTableVo> bsgrid = new Bsgrid<B_UserlimitTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        //用户记录
        public ActionResult selectUserLimitrecord(BsgridPage bsgridPage,int UserID)
        {
            try
            {
                var Recharge = (from tblimitrecord in myDYXTEntities.B_limitrecordTable
                                join tbUser in myDYXTEntities.B_UserTable on tblimitrecord.UserID equals tbUser.UserID
                                join tbLimitType in myDYXTEntities.S_LimitTypeTable on tblimitrecord.LimitTypeID equals tbLimitType.LimitTypeID
                                join tblimitkind in myDYXTEntities.S_limitkindTable on tblimitrecord.limitkindID equals tblimitkind.limitkindID
                                join tblimitoperate in myDYXTEntities.S_limitoperateTable on tblimitrecord.limitoperateID equals tblimitoperate.limitoperateID
                                where tblimitrecord.UserID== UserID
                                select new B_limitrecordTableVo
                                {
                                    limitrecordID = tblimitrecord.limitrecordID,
                                    UserID= tblimitrecord.UserID,
                                    UserName = tbUser.UserName,
                                    LimitTypeID = tblimitrecord.LimitTypeID,
                                    LimitType = tbLimitType.LimitType,
                                    limitkindName = tblimitkind.limitkindName,
                                    limitoperateName = tblimitoperate.limitoperateName,
                                    Summoney = tblimitrecord.Summoney,
                                    Remark = tblimitrecord.Remark,
                                    TimeStr = tblimitrecord.Time.ToString()                                                     
                                }).ToList();
                int totalRow = Recharge.Count();
                List<B_limitrecordTableVo> notices = Recharge.OrderByDescending(p => p.limitrecordID).
                Skip(bsgridPage.GetStartIndex()).
                Take(bsgridPage.pageSize).
                ToList();
                Bsgrid<B_limitrecordTableVo> bsgrid = new Bsgrid<B_limitrecordTableVo>();
                bsgrid.success = true;
                bsgrid.totalRows = totalRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = notices;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
        }
        /// <summary>
        /// 修改申请绑定
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult updataborrowbangding(int UserID)
        {
            try
            {
                B_UserlimitTable varB_UserlimitTable = (from tbUserlimitTable in myDYXTEntities.B_UserlimitTable
                                                        join tbuser in myDYXTEntities.B_UserTable on tbUserlimitTable.UserID equals tbuser.UserID
                                                        where tbUserlimitTable.UserID == UserID
                                                        select new B_UserlimitTableVo
                                                        {
                                                           UserID=tbUserlimitTable.UserID,
                                                           UserName=tbuser.UserName
                                                        }).Single();
                return Json(varB_UserlimitTable,JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("", JsonRequestBehavior.AllowGet);

            }       
        }
        public ActionResult preserveborrow(int UserID, string optionsRadios,decimal ApplicationMoney) {
            string strMsg = "";
            try
            {
              
                B_UserlimitTable varUserlimit = (from tbUserlimit in myDYXTEntities.B_UserlimitTable
                                                 where tbUserlimit.UserID == UserID
                                                 select tbUserlimit).Single();
                if (optionsRadios== "option1")
                {
                    varUserlimit.Creditlimetl = varUserlimit.Creditlimetl + ApplicationMoney;
                    varUserlimit.Cavailable = varUserlimit.Cavailable+ ApplicationMoney;
                }
                else if (optionsRadios == "option2")
                {
                    varUserlimit.Warrantlimetl = varUserlimit.Warrantlimetl + ApplicationMoney;
                    varUserlimit.Wavailable= varUserlimit.Wavailable+ ApplicationMoney;
                }
                else if (optionsRadios == "option3")
                {
                    varUserlimit.Mortgagelimitli = varUserlimit.Mortgagelimitli+ApplicationMoney;
                    varUserlimit.Mavailable= varUserlimit.Mavailable+ ApplicationMoney;
                }
                else if(optionsRadios == "option4")
                {
                    varUserlimit.Roamlitil= varUserlimit.Roamlitil + ApplicationMoney;
                    varUserlimit.Ravailable= varUserlimit.Ravailable + ApplicationMoney;
                }
                myDYXTEntities.Entry(varUserlimit).State = System.Data.Entity.EntityState.Modified;
                myDYXTEntities.SaveChanges();
                strMsg = "success";
                return Json(strMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(strMsg, JsonRequestBehavior.AllowGet);

            }
        }
        /// <summary>
        /// 额度审核列表
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="UserNameID"></param>
        /// <param name="StatusID"></param>
        /// <param name="LimitTypeID"></param>
        /// <returns></returns>
        public ActionResult Auditingborrowmoneylimit(BsgridPage bsgridPage,string UserNameID, int StatusID,int LimitTypeID)
        {

            List<B_LimitApplicationVo> Recharge = (from tbLimitApplication in myDYXTEntities.B_LimitApplicationTable           
                                                   join tbUser in myDYXTEntities.B_UserTable on tbLimitApplication.UserID equals tbUser.UserID
                                                   join tbLimitType in myDYXTEntities.S_LimitTypeTable on tbLimitApplication.LimitTypeID equals tbLimitType.LimitTypeID
                                                   join tblimitkind in myDYXTEntities.S_limitkindTable on tbLimitApplication.limitkindID equals tblimitkind.limitkindID
                                                   join tblimitoperate in myDYXTEntities.S_limitoperateTable on tbLimitApplication.limitoperateID equals tblimitoperate.limitoperateID
                                                   join tbStatus in myDYXTEntities.S_StatusTable on tbLimitApplication.StatusID equals tbStatus.StatusID
                                                   select new B_LimitApplicationVo
                                                 {
                                                    LimitApplicationID= tbLimitApplication.LimitApplicationID,
                                                    UserName=tbUser.UserName,
                                                    LimitTypeID=  tbLimitApplication.LimitTypeID,
                                                    LimitType =tbLimitType.LimitType,
                                                    limitkindName=  tblimitkind.limitkindName,
                                                    limitoperateName=  tblimitoperate.limitoperateName,
                                                    ApplicationMoney=tbLimitApplication.ApplicationMoney,
                                                    PassLimit=    tbLimitApplication.PassLimit,
                                                    ApplicationRemark=   tbLimitApplication.ApplicationRemark,
                                                    StatusID=  tbLimitApplication.StatusID,
                                                    StatusName =tbStatus.StatusName,
                                                    ReleaseTimeStr=   tbLimitApplication.ApplicationTime.ToString(),
                                                    TimeStr=tbLimitApplication.ExamineTime.ToString()
                                                   }).ToList();
            int totalRow = Recharge.Count();
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (StatusID > 0)
            {
                Recharge = Recharge.Where(p => p.StatusID == StatusID).ToList();
            }
            if (LimitTypeID > 0)
            {
                Recharge = Recharge.Where(p => p.LimitTypeID == LimitTypeID).ToList();
            }
            List<B_LimitApplicationVo> notices = Recharge.OrderByDescending(p => p.LimitApplicationID).
             Skip(bsgridPage.GetStartIndex()).
             Take(bsgridPage.pageSize).
             ToList();
            Bsgrid<B_LimitApplicationVo> bsgrid = new Bsgrid<B_LimitApplicationVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审核绑定
        /// </summary>
        /// <param name="LimitApplicationID"></param>
        /// <returns></returns>
        public ActionResult selectlimitxinxi(int LimitApplicationID)
        {
            try
            {
                var varloan = (from tbLimitApplication in myDYXTEntities.B_LimitApplicationTable
                               join tbUser in myDYXTEntities.B_UserTable on tbLimitApplication.UserID equals tbUser.UserID
                               join tbLimitType in myDYXTEntities.S_LimitTypeTable on tbLimitApplication.LimitTypeID equals tbLimitType.LimitTypeID
                               join tblimitkind in myDYXTEntities.S_limitkindTable on tbLimitApplication.limitkindID equals tblimitkind.limitkindID
                               where tbLimitApplication.LimitApplicationID == LimitApplicationID
                               select new B_LimitApplicationVo
                               {
                                 LimitApplicationID=  tbLimitApplication.LimitApplicationID,
                                   UserName=  tbUser.UserName,
                                   LimitTypeID= tbLimitApplication.LimitTypeID,
                                   LimitType =   tbLimitType.LimitType,
                                   limitkindName=  tblimitkind.limitkindName,
                                   ApplicationMoney=tbLimitApplication.ApplicationMoney,
                                   ApplicationRemark = tbLimitApplication.ApplicationRemark,
                                   TongguoMoney= tbLimitApplication.ApplicationMoney
                               }).Single();
                return Json(varloan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 审核保存
        /// </summary>
        /// <param name="LimitApplication"></param>
        /// <param name="optionsRadios"></param>
        /// <param name="validCode"></param>
        /// <param name="TongguoMoney"></param>
        /// <returns></returns>
        public ActionResult selectshenheedushenqing(B_LimitApplicationTable LimitApplication, string optionsRadios, string validCode, decimal TongguoMoney)
        {
            //定义返回
            string strMsg = "fail";
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
            if (strSessionValidCode.Equals(validCode.Trim(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (optionsRadios == "option1")
                {
                    B_LimitApplicationTable vaarLimitApplication = (from tbLimitApplication in myDYXTEntities.B_LimitApplicationTable
                                                                    where tbLimitApplication.LimitApplicationID == LimitApplication.LimitApplicationID
                                                                    select tbLimitApplication).Single();
                    //B_UserlimitTable varUserlimit = (from tbUserlimit in myDYXTEntities.B_UserlimitTable
                    //                                 where tbUserlimit.UserID == vaarLimitApplication.UserID
                    //                                 select tbUserlimit).Single();
                    vaarLimitApplication.PassLimit = TongguoMoney;
                    vaarLimitApplication.ExamineTime = DateTime.Now;
                    vaarLimitApplication.StatusID = 4;
                    vaarLimitApplication.AuditingRemark = LimitApplication.AuditingRemark;
                    myDYXTEntities.Entry(vaarLimitApplication).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();
                    if (vaarLimitApplication.LimitTypeID==1)
                    {
                        B_UserlimitTable varUserlimit = (from tbUserlimit in myDYXTEntities.B_UserlimitTable
                                                         where tbUserlimit.UserID == vaarLimitApplication.UserID
                                                         select tbUserlimit).Single();
                        varUserlimit.Creditlimetl = varUserlimit.Creditlimetl + TongguoMoney;
                        varUserlimit.Cavailable = varUserlimit.Cavailable + TongguoMoney;
                        myDYXTEntities.Entry(varUserlimit).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                    }
                    else if(vaarLimitApplication.LimitTypeID==2)
                    {
                        B_UserlimitTable varUserlimit = (from tbUserlimit in myDYXTEntities.B_UserlimitTable
                                                         where tbUserlimit.UserID == vaarLimitApplication.UserID
                                                         select tbUserlimit).Single();
                        varUserlimit.Warrantlimetl = varUserlimit.Warrantlimetl + TongguoMoney;
                        varUserlimit.Wavailable = varUserlimit.Wavailable + TongguoMoney;
                        myDYXTEntities.Entry(varUserlimit).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                    }
                    else if(vaarLimitApplication.LimitTypeID == 3)
                    {
                        B_UserlimitTable varUserlimit = (from tbUserlimit in myDYXTEntities.B_UserlimitTable
                                                         where tbUserlimit.UserID == vaarLimitApplication.UserID
                                                         select tbUserlimit).Single();
                        varUserlimit.Mortgagelimitli = varUserlimit.Mortgagelimitli + TongguoMoney;
                        varUserlimit.Mavailable = varUserlimit.Mavailable + TongguoMoney;
                        myDYXTEntities.Entry(varUserlimit).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                    }
                    else
                    {
                        B_UserlimitTable varUserlimit = (from tbUserlimit in myDYXTEntities.B_UserlimitTable
                                                         where tbUserlimit.UserID == vaarLimitApplication.UserID
                                                         select tbUserlimit).Single();
                        varUserlimit.Roamlitil = varUserlimit.Roamlitil + TongguoMoney;
                        varUserlimit.Ravailable = varUserlimit.Ravailable + TongguoMoney;
                        myDYXTEntities.Entry(varUserlimit).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                    }
                    B_limitrecordTable varlimitrecord = new B_limitrecordTable();
                    varlimitrecord.UserID = vaarLimitApplication.UserID;
                    varlimitrecord.LimitTypeID = vaarLimitApplication.LimitTypeID;
                    varlimitrecord.limitkindID = vaarLimitApplication.limitkindID;
                    varlimitrecord.limitoperateID = vaarLimitApplication.limitoperateID;
                    varlimitrecord.Summoney = TongguoMoney;
                    varlimitrecord.Remark = "审请额度审核通过";
                    varlimitrecord.Time = DateTime.Now;
                    myDYXTEntities.B_limitrecordTable.Add(varlimitrecord);
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
                else
                {
                    B_LimitApplicationTable vaarLimitApplication = (from tbLimitApplication in myDYXTEntities.B_LimitApplicationTable
                                                                    where tbLimitApplication.LimitApplicationID == LimitApplication.LimitApplicationID
                                                                    select tbLimitApplication).Single();
                    //B_UserlimitTable varUserlimit = (from tbUserlimit in myDYXTEntities.B_UserlimitTable
                    //                                 where tbUserlimit.UserID == vaarLimitApplication.UserID
                    //                                 select tbUserlimit).Single();
                    vaarLimitApplication.PassLimit = TongguoMoney;
                    vaarLimitApplication.ExamineTime = DateTime.Now;
                    vaarLimitApplication.StatusID = 35;
                    vaarLimitApplication.AuditingRemark = LimitApplication.AuditingRemark;
                    myDYXTEntities.Entry(vaarLimitApplication).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();

                    B_limitrecordTable varlimitrecord = new B_limitrecordTable();
                    varlimitrecord.UserID = vaarLimitApplication.UserID;
                    varlimitrecord.LimitTypeID = vaarLimitApplication.LimitTypeID;
                    varlimitrecord.limitkindID = vaarLimitApplication.limitkindID;
                    varlimitrecord.limitoperateID = vaarLimitApplication.limitoperateID;
                    varlimitrecord.Summoney = TongguoMoney;
                    varlimitrecord.Remark = "审请额度审核不通过";
                    varlimitrecord.Time = DateTime.Now;
                    myDYXTEntities.B_limitrecordTable.Add(varlimitrecord);
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
            }
            else
            {
                strMsg = "ValidCodeErro";//验证码错误
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public ActionResult selectEDzhuangtai()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbStatus in myDYXTEntities.S_StatusTable
                                         where tbStatus.StatusID==2||tbStatus.StatusID==4||tbStatus.StatusID==35
                                         select new SelectVo
                                         {
                                             id = tbStatus.StatusID,
                                             text = tbStatus.StatusName

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 额度类型
        /// </summary>
        /// <returns></returns>
        public ActionResult selectLimitType()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbStatus in myDYXTEntities.S_LimitTypeTable                                     
                                         select new SelectVo
                                         {
                                             id = tbStatus.LimitTypeID,
                                             text = tbStatus.LimitType

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="LimitApplicationID"></param>
        /// <returns></returns>
        public ActionResult selectLimitApplication(int LimitApplicationID)
        {
            try
            {
                var varloan = (from tbLimitApplication in myDYXTEntities.B_LimitApplicationTable
                               join tbUser in myDYXTEntities.B_UserTable on tbLimitApplication.UserID equals tbUser.UserID
                               join tbLimitType in myDYXTEntities.S_LimitTypeTable on tbLimitApplication.LimitTypeID equals tbLimitType.LimitTypeID
                               join tblimitkind in myDYXTEntities.S_limitkindTable on tbLimitApplication.limitkindID equals tblimitkind.limitkindID
                               join tblimitoperate in myDYXTEntities.S_limitoperateTable on tbLimitApplication.limitoperateID equals tblimitoperate.limitoperateID
                               join tbStatus in myDYXTEntities.S_StatusTable on tbLimitApplication.StatusID equals tbStatus.StatusID
                               where tbLimitApplication.LimitApplicationID== LimitApplicationID
                               select new B_LimitApplicationVo
                               {
                                   LimitApplicationID = tbLimitApplication.LimitApplicationID,
                                   UserName = tbUser.UserName,
                                   LimitTypeID = tbLimitApplication.LimitTypeID,
                                   LimitType = tbLimitType.LimitType,
                                   limitkindName = tblimitkind.limitkindName,
                                   limitoperateName = tblimitoperate.limitoperateName,
                                   ApplicationMoney = tbLimitApplication.ApplicationMoney,
                                   PassLimit = tbLimitApplication.PassLimit,
                                   ApplicationRemark = tbLimitApplication.ApplicationRemark,
                                   StatusID = tbLimitApplication.StatusID,
                                   StatusName = tbStatus.StatusName,
                                   ReleaseTimeStr = tbLimitApplication.ApplicationTime.ToString(),
                                   TimeStr = tbLimitApplication.ExamineTime.ToString(),
                                   AuditingRemark = tbLimitApplication.AuditingRemark
                               }).Single();
                return Json(varloan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
        }
        /// <summary>
        /// 额度记录
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="UserNameID"></param>
        /// <param name="LimitTypeID"></param>
        /// <returns></returns>
        public ActionResult selectlimitrecord(BsgridPage bsgridPage,string UserNameID,int LimitTypeID)
        {

            List<B_limitrecordTableVo> Recharge = (from tblimitrecord in myDYXTEntities.B_limitrecordTable
                                                   join tbUser in myDYXTEntities.B_UserTable on tblimitrecord.UserID equals tbUser.UserID
                                                   join tbLimitType in myDYXTEntities.S_LimitTypeTable on tblimitrecord.LimitTypeID equals tbLimitType.LimitTypeID
                                                   join tblimitkind in myDYXTEntities.S_limitkindTable on tblimitrecord.limitkindID equals tblimitkind.limitkindID
                                                   join tblimitoperate in myDYXTEntities.S_limitoperateTable on tblimitrecord.limitoperateID equals tblimitoperate.limitoperateID
                                                
                                                   select new B_limitrecordTableVo
                                                   {
                                                       limitrecordID = tblimitrecord.limitrecordID,
                                                       UserName = tbUser.UserName,
                                                       LimitTypeID = tblimitrecord.LimitTypeID,
                                                       LimitType = tbLimitType.LimitType,
                                                       limitkindName = tblimitkind.limitkindName,
                                                       limitoperateName = tblimitoperate.limitoperateName,
                                                       Summoney=tblimitrecord.Summoney,
                                                       Remark=tblimitrecord.Remark,
                                                       TimeStr = tblimitrecord.Time.ToString()
                                                   }).ToList();
            int totalRow = Recharge.Count();
            if (!string.IsNullOrEmpty(UserNameID))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }      
            if (LimitTypeID > 0)
            {
                Recharge = Recharge.Where(p => p.LimitTypeID == LimitTypeID).ToList();
            }
            List<B_limitrecordTableVo> notices = Recharge.OrderByDescending(p => p.limitrecordID).
             Skip(bsgridPage.GetStartIndex()).
             Take(bsgridPage.pageSize).
             ToList();
            Bsgrid<B_limitrecordTableVo> bsgrid = new Bsgrid<B_limitrecordTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 额度类型查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectS_LimitType(BsgridPage bsgridPage)
        {

            List<S_LimitTypeTableVo> Recharge = (from tbLimitType in myDYXTEntities.S_LimitTypeTable 
                                                 join tbFundStatus in myDYXTEntities.S_FundStatustable on tbLimitType.FundStatusID equals tbFundStatus.FundStatusID
                                                   select new S_LimitTypeTableVo
                                                   {
                                                     LimitTypeID= tbLimitType.LimitTypeID,
                                                       LimitType=  tbLimitType.LimitType,
                                                       IdentificationName =tbLimitType.IdentificationName,
                                                       Name=tbLimitType.Name,
                                                       FundStatusName = tbFundStatus.FundStatusName,
                                                       Remark=tbLimitType.Remark
                                                   }).ToList();
            int totalRow = Recharge.Count();          
            List<S_LimitTypeTableVo> notices = Recharge.OrderBy(p => p.LimitTypeID).
             Skip(bsgridPage.GetStartIndex()).
             Take(bsgridPage.pageSize).
             ToList();
            Bsgrid<S_LimitTypeTableVo> bsgrid = new Bsgrid<S_LimitTypeTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <returns></returns>
        public ActionResult selectFundStatus()
        {
            List<SelectVo> noticeType = (from tbTreetoptype in myDYXTEntities.S_FundStatustable
                                         select new SelectVo
                                         {
                                             id = tbTreetoptype.FundStatusID,
                                             text = tbTreetoptype.FundStatusName

                                         }).ToList();

            return Json(noticeType, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 修改绑定
        /// </summary>
        /// <param name="LimitTypeID"></param>
        /// <returns></returns>
        public ActionResult SelectlimittypeById(int LimitTypeID)
        {
            try
            {
                S_LimitTypeTableVo varLimitType = (from tbLimitType in myDYXTEntities.S_LimitTypeTable
                                                   join tbFundStatus in myDYXTEntities.S_FundStatustable on tbLimitType.FundStatusID equals tbFundStatus.FundStatusID
                                                   where tbLimitType.LimitTypeID == LimitTypeID
                                                  select new S_LimitTypeTableVo
                                               {
                                                  LimitTypeID = tbLimitType.LimitTypeID,
                                                  LimitType = tbLimitType.LimitType.Trim(),
                                                  IdentificationName = tbLimitType.IdentificationName.Trim(),
                                                   Name = tbLimitType.Name.Trim(),
                                                      FundStatusID= tbLimitType.FundStatusID,
                                                    FundStatusName = tbFundStatus.FundStatusName,
                                                    Remark = tbLimitType.Remark.Trim()
                                               }).Single();
                return Json(varLimitType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
         /// <summary>
         /// 修改类型保存
         /// </summary>
         /// <param name="LimitTypeID"></param>
         /// <param name="FundStatusID"></param>
         /// <param name="Name"></param>
         /// <param name="Remark"></param>
         /// <returns></returns>
        public ActionResult UpdataLimitTypess(int LimitTypeID,int FundStatusID,string Name ,string Remark)
        {
            string strMed = "fail";                 
            var varaccon = (from tbLimitType in myDYXTEntities.S_LimitTypeTable
                            where tbLimitType.LimitTypeID == LimitTypeID
                            select tbLimitType).Single();
            varaccon.Name = Name;
            varaccon.FundStatusID = FundStatusID;
            varaccon.Remark = Remark;
            myDYXTEntities.Entry(varaccon).State = System.Data.Entity.EntityState.Modified;
            myDYXTEntities.SaveChanges();
            strMed = "success";
            return Json(strMed, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 债权转让
        /// <summary>
        /// 债权转让视图
        /// </summary>
        /// <returns></returns>
        public ActionResult obligatoryright()
        {
            return View();
        }
        /// <summary>
        /// 所有转让
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectobligatoryright(BsgridPage bsgridPage)
        {
            List<B_CreditorAttornTableVo> Recharge = (from tbCreditorAttorn in myDYXTEntities.B_CreditorAttornTable
                                                      join tbUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.UserID equals tbUser.UserID
                                                      join tbStatus in myDYXTEntities.S_StatusTable on tbCreditorAttorn.StatusID equals tbStatus.StatusID
                                                      select new B_CreditorAttornTableVo
                                                      {
                                                          CreditorAttornID = tbCreditorAttorn.CreditorAttornID,
                                                          UserName = tbUser.UserName,
                                                          TenderTitle = tbCreditorAttorn.TenderTitle,
                                                          Rate = tbCreditorAttorn.Rate,
                                                          WaitPeriods = tbCreditorAttorn.WaitPeriods,
                                                          AttornPeriods = tbCreditorAttorn.AttornPeriods,
                                                          AllPeriods = tbCreditorAttorn.AllPeriods,
                                                          WaitCapital = tbCreditorAttorn.WaitCapital,
                                                          WaitAccrual = tbCreditorAttorn.WaitAccrual,
                                                          AttornPrice = tbCreditorAttorn.AttornPrice,
                                                          ReleaseTimeStr= tbCreditorAttorn.SubmitTime.ToString(),
                                                          StatusName=tbStatus.StatusName
                                                      }).ToList();
      
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].WaitPeriodss = Convert.ToString(Recharge[i].WaitPeriods) + "期";
                Recharge[i].AttornPeriodss = Convert.ToString(Recharge[i].AttornPeriods) + "期";
                Recharge[i].AllPeriodss = Convert.ToString(Recharge[i].AllPeriods) + "期";             
            }
            int totalRow = Recharge.Count();
            List<B_CreditorAttornTableVo> notices = Recharge.OrderBy(p => p.CreditorAttornID).
             Skip(bsgridPage.GetStartIndex()).
             Take(bsgridPage.pageSize).
             ToList();
            Bsgrid<B_CreditorAttornTableVo> bsgrid = new Bsgrid<B_CreditorAttornTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 所有债权导出全部
        /// </summary>
        /// <returns></returns>
        public ActionResult ExExcel()
        {
            var notict = from tbCreditorAttorn in myDYXTEntities.B_CreditorAttornTable
                         join tbUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.UserID equals tbUser.UserID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbCreditorAttorn.StatusID equals tbStatus.StatusID
                         orderby tbCreditorAttorn.CreditorAttornID
                         select new B_CreditorAttornTableVo
                         {
                             CreditorAttornID = tbCreditorAttorn.CreditorAttornID,
                             UserName = tbUser.UserName,
                             TenderTitle = tbCreditorAttorn.TenderTitle,
                             Rate = tbCreditorAttorn.Rate,
                             WaitPeriods = tbCreditorAttorn.WaitPeriods,
                             AttornPeriods = tbCreditorAttorn.AttornPeriods,
                             AllPeriods = tbCreditorAttorn.AllPeriods,
                             WaitCapital = tbCreditorAttorn.WaitCapital,
                             WaitAccrual = tbCreditorAttorn.WaitAccrual,
                             AttornPrice = tbCreditorAttorn.AttornPrice,
                             ReleaseTimeStr = tbCreditorAttorn.SubmitTime.ToString(),
                             StatusName = tbStatus.StatusName
                         };
            //查询数据
            List<B_CreditorAttornTableVo> listExaminee = notict.ToList();
            //创建Excel对象 工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook excelBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("债权转让");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题
            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("转让者");//1
            row1.CreateCell(2).SetCellValue("投标标头");//2
            row1.CreateCell(3).SetCellValue("利率");//3
            row1.CreateCell(4).SetCellValue("待收期数");//4
            row1.CreateCell(5).SetCellValue("转让期数");//5
            row1.CreateCell(6).SetCellValue("总期数");//6   
            row1.CreateCell(7).SetCellValue("待收本金");//6   
            row1.CreateCell(8).SetCellValue("待收利息");//6   
            row1.CreateCell(9).SetCellValue("转让价格");//6   
            row1.CreateCell(10).SetCellValue("提交时间");//6  
            row1.CreateCell(11).SetCellValue("查看");
                                                   //将数据逐步写入sheet1各个行
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);


                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].CreditorAttornID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(Convert.ToString(listExaminee[i].TenderTitle));

                rowTemp.CreateCell(3).SetCellValue(Convert.ToDouble(listExaminee[i].Rate));

                rowTemp.CreateCell(4).SetCellValue(Convert.ToDouble(listExaminee[i].WaitPeriods));

                rowTemp.CreateCell(5).SetCellValue(Convert.ToDouble(listExaminee[i].AttornPeriods));

                rowTemp.CreateCell(6).SetCellValue(Convert.ToDouble(listExaminee[i].AllPeriods));
                rowTemp.CreateCell(7).SetCellValue(Convert.ToDouble(listExaminee[i].WaitCapital));
                rowTemp.CreateCell(8).SetCellValue(Convert.ToDouble(listExaminee[i].WaitAccrual));
                rowTemp.CreateCell(9).SetCellValue(Convert.ToDouble(listExaminee[i].AttornPrice));
                rowTemp.CreateCell(10).SetCellValue(Convert.ToString(listExaminee[i].ReleaseTimeStr));
                rowTemp.CreateCell(11).SetCellValue(Convert.ToString(listExaminee[i].StatusName));
            }
            //输出的文件名称
            string fileName = "债权转让信息" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

            //把Excel转为流，输出
            //创建文件流
            System.IO.MemoryStream bookStream = new System.IO.MemoryStream();
            //将工作薄写入文件流
            excelBook.Write(bookStream);
            //输出之前调用Seek（偏移量，游标位置) 把0位置指定为开始位置
            bookStream.Seek(0, System.IO.SeekOrigin.Begin);
            //Stream对象,文件类型,文件名称
            return File(bookStream, "application/vnd.ms-excel", fileName);
        }
        /// <summary>
        /// 正在转让
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectzhengzaiobligatoryright(BsgridPage bsgridPage)
        {
            List<B_CreditorAttornTableVo> Recharge = (from tbCreditorAttorn in myDYXTEntities.B_CreditorAttornTable
                                                      join tbUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.UserID equals tbUser.UserID
                                                      join tbStatus in myDYXTEntities.S_StatusTable on tbCreditorAttorn.StatusID equals tbStatus.StatusID
                                                      where tbCreditorAttorn.StatusID==37
                                                      select new B_CreditorAttornTableVo
                                                      {
                                                          CreditorAttornID = tbCreditorAttorn.CreditorAttornID,
                                                          UserName = tbUser.UserName,
                                                          TenderTitle = tbCreditorAttorn.TenderTitle,
                                                          Rate = tbCreditorAttorn.Rate,
                                                          WaitPeriods = tbCreditorAttorn.WaitPeriods,
                                                          AttornPeriods = tbCreditorAttorn.AttornPeriods,
                                                          AllPeriods = tbCreditorAttorn.AllPeriods,
                                                          WaitCapital = tbCreditorAttorn.WaitCapital,
                                                          WaitAccrual = tbCreditorAttorn.WaitAccrual,
                                                          AttornPrice = tbCreditorAttorn.AttornPrice,
                                                          ReleaseTimeStr = tbCreditorAttorn.SubmitTime.ToString(),
                                                          StatusName = tbStatus.StatusName
                                                      }).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].WaitPeriodss = Convert.ToString(Recharge[i].WaitPeriods) + "期";
                Recharge[i].AttornPeriodss = Convert.ToString(Recharge[i].AttornPeriods) + "期";
                Recharge[i].AllPeriodss = Convert.ToString(Recharge[i].AllPeriods) + "期";
            }
            int totalRow = Recharge.Count();
            List<B_CreditorAttornTableVo> notices = Recharge.OrderBy(p => p.CreditorAttornID).
             Skip(bsgridPage.GetStartIndex()).
             Take(bsgridPage.pageSize).
             ToList();
            Bsgrid<B_CreditorAttornTableVo> bsgrid = new Bsgrid<B_CreditorAttornTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 正在转让导出
        /// </summary>
        /// <returns></returns>
        public ActionResult zhengzaiExExcel()
        {
            var notict = from tbCreditorAttorn in myDYXTEntities.B_CreditorAttornTable
                         join tbUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.UserID equals tbUser.UserID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbCreditorAttorn.StatusID equals tbStatus.StatusID
                         orderby tbCreditorAttorn.CreditorAttornID
                         where tbCreditorAttorn.StatusID==37
                         select new B_CreditorAttornTableVo
                         {
                             CreditorAttornID = tbCreditorAttorn.CreditorAttornID,
                             UserName = tbUser.UserName,
                             TenderTitle = tbCreditorAttorn.TenderTitle,
                             Rate = tbCreditorAttorn.Rate,
                             WaitPeriods = tbCreditorAttorn.WaitPeriods,
                             AttornPeriods = tbCreditorAttorn.AttornPeriods,
                             AllPeriods = tbCreditorAttorn.AllPeriods,
                             WaitCapital = tbCreditorAttorn.WaitCapital,
                             WaitAccrual = tbCreditorAttorn.WaitAccrual,
                             AttornPrice = tbCreditorAttorn.AttornPrice,
                             ReleaseTimeStr = tbCreditorAttorn.SubmitTime.ToString(),
                             StatusName = tbStatus.StatusName
                         };
            //查询数据
            List<B_CreditorAttornTableVo> listExaminee = notict.ToList();
            //创建Excel对象 工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook excelBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("正在转让");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题
            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("转让者");//1
            row1.CreateCell(2).SetCellValue("投标标头");//2
            row1.CreateCell(3).SetCellValue("利率");//3
            row1.CreateCell(4).SetCellValue("待收期数");//4
            row1.CreateCell(5).SetCellValue("转让期数");//5
            row1.CreateCell(6).SetCellValue("总期数");//6   
            row1.CreateCell(7).SetCellValue("待收本金");//6   
            row1.CreateCell(8).SetCellValue("待收利息");//6   
            row1.CreateCell(9).SetCellValue("转让价格");//6   
            row1.CreateCell(10).SetCellValue("提交时间");//6  
            row1.CreateCell(11).SetCellValue("查看");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);


                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].CreditorAttornID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(Convert.ToString(listExaminee[i].TenderTitle));

                rowTemp.CreateCell(3).SetCellValue(Convert.ToDouble(listExaminee[i].Rate));

                rowTemp.CreateCell(4).SetCellValue(Convert.ToDouble(listExaminee[i].WaitPeriods));

                rowTemp.CreateCell(5).SetCellValue(Convert.ToDouble(listExaminee[i].AttornPeriods));

                rowTemp.CreateCell(6).SetCellValue(Convert.ToDouble(listExaminee[i].AllPeriods));
                rowTemp.CreateCell(7).SetCellValue(Convert.ToDouble(listExaminee[i].WaitCapital));
                rowTemp.CreateCell(8).SetCellValue(Convert.ToDouble(listExaminee[i].WaitAccrual));
                rowTemp.CreateCell(9).SetCellValue(Convert.ToDouble(listExaminee[i].AttornPrice));
                rowTemp.CreateCell(10).SetCellValue(Convert.ToString(listExaminee[i].ReleaseTimeStr));
                rowTemp.CreateCell(11).SetCellValue(Convert.ToString(listExaminee[i].StatusName));
            }
            //输出的文件名称
            string fileName = "债权正在转让信息" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

            //把Excel转为流，输出
            //创建文件流
            System.IO.MemoryStream bookStream = new System.IO.MemoryStream();
            //将工作薄写入文件流
            excelBook.Write(bookStream);
            //输出之前调用Seek（偏移量，游标位置) 把0位置指定为开始位置
            bookStream.Seek(0, System.IO.SeekOrigin.Begin);
            //Stream对象,文件类型,文件名称
            return File(bookStream, "application/vnd.ms-excel", fileName);
        }
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectchexiaoobligatoryright(BsgridPage bsgridPage)
        {
            List<B_CreditorAttornTableVo> Recharge = (from tbCreditorAttorn in myDYXTEntities.B_CreditorAttornTable
                                                      join tbUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.UserID equals tbUser.UserID
                                                      join tbStatus in myDYXTEntities.S_StatusTable on tbCreditorAttorn.StatusID equals tbStatus.StatusID
                                                      where tbCreditorAttorn.StatusID == 34
                                                      select new B_CreditorAttornTableVo
                                                      {
                                                          CreditorAttornID = tbCreditorAttorn.CreditorAttornID,
                                                          UserName = tbUser.UserName,
                                                          TenderTitle = tbCreditorAttorn.TenderTitle,
                                                          Rate = tbCreditorAttorn.Rate,
                                                          WaitPeriods = tbCreditorAttorn.WaitPeriods,
                                                          AttornPeriods = tbCreditorAttorn.AttornPeriods,
                                                          AllPeriods = tbCreditorAttorn.AllPeriods,
                                                          WaitCapital = tbCreditorAttorn.WaitCapital,
                                                          WaitAccrual = tbCreditorAttorn.WaitAccrual,
                                                          AttornPrice = tbCreditorAttorn.AttornPrice,
                                                          ReleaseTimeStr = tbCreditorAttorn.SubmitTime.ToString(),
                                                          StatusName = tbStatus.StatusName
                                                      }).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].WaitPeriodss = Convert.ToString(Recharge[i].WaitPeriods) + "期";
                Recharge[i].AttornPeriodss = Convert.ToString(Recharge[i].AttornPeriods) + "期";
                Recharge[i].AllPeriodss = Convert.ToString(Recharge[i].AllPeriods) + "期";
            }
            int totalRow = Recharge.Count();
            List<B_CreditorAttornTableVo> notices = Recharge.OrderBy(p => p.CreditorAttornID).
             Skip(bsgridPage.GetStartIndex()).
             Take(bsgridPage.pageSize).
             ToList();
            Bsgrid<B_CreditorAttornTableVo> bsgrid = new Bsgrid<B_CreditorAttornTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 撤销转让导出
        /// </summary>
        /// <returns></returns>
        public ActionResult chexiaoExExcel()
        {
            var notict = from tbCreditorAttorn in myDYXTEntities.B_CreditorAttornTable
                         join tbUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.UserID equals tbUser.UserID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbCreditorAttorn.StatusID equals tbStatus.StatusID
                         orderby tbCreditorAttorn.CreditorAttornID
                         where tbCreditorAttorn.StatusID == 34
                         select new B_CreditorAttornTableVo
                         {
                             CreditorAttornID = tbCreditorAttorn.CreditorAttornID,
                             UserName = tbUser.UserName,
                             TenderTitle = tbCreditorAttorn.TenderTitle,
                             Rate = tbCreditorAttorn.Rate,
                             WaitPeriods = tbCreditorAttorn.WaitPeriods,
                             AttornPeriods = tbCreditorAttorn.AttornPeriods,
                             AllPeriods = tbCreditorAttorn.AllPeriods,
                             WaitCapital = tbCreditorAttorn.WaitCapital,
                             WaitAccrual = tbCreditorAttorn.WaitAccrual,
                             AttornPrice = tbCreditorAttorn.AttornPrice,
                             ReleaseTimeStr = tbCreditorAttorn.SubmitTime.ToString(),
                             StatusName = tbStatus.StatusName
                         };
            //查询数据
            List<B_CreditorAttornTableVo> listExaminee = notict.ToList();
            //创建Excel对象 工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook excelBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("撤销");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题
            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("转让者");//1
            row1.CreateCell(2).SetCellValue("投标标头");//2
            row1.CreateCell(3).SetCellValue("利率");//3
            row1.CreateCell(4).SetCellValue("待收期数");//4
            row1.CreateCell(5).SetCellValue("转让期数");//5
            row1.CreateCell(6).SetCellValue("总期数");//6   
            row1.CreateCell(7).SetCellValue("待收本金");//6   
            row1.CreateCell(8).SetCellValue("待收利息");//6   
            row1.CreateCell(9).SetCellValue("转让价格");//6   
            row1.CreateCell(10).SetCellValue("提交时间");//6  
            row1.CreateCell(11).SetCellValue("查看");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);


                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].CreditorAttornID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(Convert.ToString(listExaminee[i].TenderTitle));

                rowTemp.CreateCell(3).SetCellValue(Convert.ToDouble(listExaminee[i].Rate));

                rowTemp.CreateCell(4).SetCellValue(Convert.ToDouble(listExaminee[i].WaitPeriods));

                rowTemp.CreateCell(5).SetCellValue(Convert.ToDouble(listExaminee[i].AttornPeriods));

                rowTemp.CreateCell(6).SetCellValue(Convert.ToDouble(listExaminee[i].AllPeriods));
                rowTemp.CreateCell(7).SetCellValue(Convert.ToDouble(listExaminee[i].WaitCapital));
                rowTemp.CreateCell(8).SetCellValue(Convert.ToDouble(listExaminee[i].WaitAccrual));
                rowTemp.CreateCell(9).SetCellValue(Convert.ToDouble(listExaminee[i].AttornPrice));
                rowTemp.CreateCell(10).SetCellValue(Convert.ToString(listExaminee[i].ReleaseTimeStr));
                rowTemp.CreateCell(11).SetCellValue(Convert.ToString(listExaminee[i].StatusName));
            }
            //输出的文件名称
            string fileName = "债权撤销信息" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

            //把Excel转为流，输出
            //创建文件流
            System.IO.MemoryStream bookStream = new System.IO.MemoryStream();
            //将工作薄写入文件流
            excelBook.Write(bookStream);
            //输出之前调用Seek（偏移量，游标位置) 把0位置指定为开始位置
            bookStream.Seek(0, System.IO.SeekOrigin.Begin);
            //Stream对象,文件类型,文件名称
            return File(bookStream, "application/vnd.ms-excel", fileName);
        }

        /// <summary>
        /// 转让成功
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectsueeccsbligatoryright(BsgridPage bsgridPage)
        {
            List<B_CreditorAttornTableVo> Recharge = (from tbCreditorAttorn in myDYXTEntities.B_CreditorAttornTable
                                                      join tbUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.UserID equals tbUser.UserID
                                                      join tbPurchaserUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.PurchaserUserID equals tbPurchaserUser.UserID
                                                      join tbStatus in myDYXTEntities.S_StatusTable on tbCreditorAttorn.StatusID equals tbStatus.StatusID
                                                      where tbCreditorAttorn.StatusID == 36
                                                      select new B_CreditorAttornTableVo
                                                      {
                                                          CreditorAttornID = tbCreditorAttorn.CreditorAttornID,
                                                          UserName = tbUser.UserName,
                                                          TenderTitle = tbCreditorAttorn.TenderTitle,
                                                          Rate = tbCreditorAttorn.Rate,
                                                          WaitPeriods = tbCreditorAttorn.WaitPeriods,
                                                          AttornPeriods = tbCreditorAttorn.AttornPeriods,
                                                          AllPeriods = tbCreditorAttorn.AllPeriods,
                                                          WaitCapital = tbCreditorAttorn.WaitCapital,
                                                          WaitAccrual = tbCreditorAttorn.WaitAccrual,
                                                          AttornPrice = tbCreditorAttorn.AttornPrice,
                                                          ReleaseTimeStr = tbCreditorAttorn.SubmitTime.ToString(),
                                                          PurchaserUser= tbPurchaserUser.UserName,
                                                          TimeStr=tbCreditorAttorn.PurchaseTime.ToString(),
                                                          StatusName = tbStatus.StatusName
                                                      }).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].WaitPeriodss = Convert.ToString(Recharge[i].WaitPeriods) + "期";
                Recharge[i].AttornPeriodss = Convert.ToString(Recharge[i].AttornPeriods) + "期";
                Recharge[i].AllPeriodss = Convert.ToString(Recharge[i].AllPeriods) + "期";
            }
            int totalRow = Recharge.Count();
            List<B_CreditorAttornTableVo> notices = Recharge.OrderBy(p => p.CreditorAttornID).
             Skip(bsgridPage.GetStartIndex()).
             Take(bsgridPage.pageSize).
             ToList();
            Bsgrid<B_CreditorAttornTableVo> bsgrid = new Bsgrid<B_CreditorAttornTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出转让成功
        /// </summary>
        /// <returns></returns>
        public ActionResult chenggongzhuanranExExcel()
        {
            var notict = from tbCreditorAttorn in myDYXTEntities.B_CreditorAttornTable
                         join tbUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.UserID equals tbUser.UserID
                         join tbPurchaserUser in myDYXTEntities.B_UserTable on tbCreditorAttorn.PurchaserUserID equals tbPurchaserUser.UserID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbCreditorAttorn.StatusID equals tbStatus.StatusID
                         orderby tbCreditorAttorn.CreditorAttornID
                         where tbCreditorAttorn.StatusID == 36
                         select new B_CreditorAttornTableVo
                         {
                             CreditorAttornID = tbCreditorAttorn.CreditorAttornID,
                             UserName = tbUser.UserName,
                             TenderTitle = tbCreditorAttorn.TenderTitle,
                             Rate = tbCreditorAttorn.Rate,
                             WaitPeriods = tbCreditorAttorn.WaitPeriods,
                             AttornPeriods = tbCreditorAttorn.AttornPeriods,
                             AllPeriods = tbCreditorAttorn.AllPeriods,
                             WaitCapital = tbCreditorAttorn.WaitCapital,
                             WaitAccrual = tbCreditorAttorn.WaitAccrual,
                             AttornPrice = tbCreditorAttorn.AttornPrice,
                             ReleaseTimeStr = tbCreditorAttorn.SubmitTime.ToString(),
                            
                             PurchaserUser = tbPurchaserUser.UserName,
                             TimeStr = tbCreditorAttorn.PurchaseTime.ToString(),
                             StatusName = tbStatus.StatusName
                         };
            //查询数据
            List<B_CreditorAttornTableVo> listExaminee = notict.ToList();
            //创建Excel对象 工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook excelBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("转让成功");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题
            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("转让者");//1
            row1.CreateCell(2).SetCellValue("投标标头");//2
            row1.CreateCell(3).SetCellValue("利率");//3
            row1.CreateCell(4).SetCellValue("待收期数");//4
            row1.CreateCell(5).SetCellValue("转让期数");//5
            row1.CreateCell(6).SetCellValue("总期数");//6   
            row1.CreateCell(7).SetCellValue("待收本金");//6   
            row1.CreateCell(8).SetCellValue("待收利息");//6   
            row1.CreateCell(9).SetCellValue("转让价格");//6   
            row1.CreateCell(10).SetCellValue("提交时间");//6  
            row1.CreateCell(11).SetCellValue("购买者");//6
            row1.CreateCell(12).SetCellValue("购买时间");//6
            row1.CreateCell(13).SetCellValue("查看");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);


                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].CreditorAttornID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(Convert.ToString(listExaminee[i].TenderTitle));

                rowTemp.CreateCell(3).SetCellValue(Convert.ToDouble(listExaminee[i].Rate));

                rowTemp.CreateCell(4).SetCellValue(Convert.ToDouble(listExaminee[i].WaitPeriods));

                rowTemp.CreateCell(5).SetCellValue(Convert.ToDouble(listExaminee[i].AttornPeriods));

                rowTemp.CreateCell(6).SetCellValue(Convert.ToDouble(listExaminee[i].AllPeriods));
                rowTemp.CreateCell(7).SetCellValue(Convert.ToDouble(listExaminee[i].WaitCapital));
                rowTemp.CreateCell(8).SetCellValue(Convert.ToDouble(listExaminee[i].WaitAccrual));
                rowTemp.CreateCell(9).SetCellValue(Convert.ToDouble(listExaminee[i].AttornPrice));
                rowTemp.CreateCell(10).SetCellValue(Convert.ToString(listExaminee[i].ReleaseTimeStr));
                rowTemp.CreateCell(11).SetCellValue(Convert.ToString(listExaminee[i].PurchaserUser));
                rowTemp.CreateCell(12).SetCellValue(Convert.ToString(listExaminee[i].TimeStr));
                rowTemp.CreateCell(13).SetCellValue(Convert.ToString(listExaminee[i].StatusName));
            }
            //输出的文件名称
            string fileName = "债权转让成功信息" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

            //把Excel转为流，输出
            //创建文件流
            System.IO.MemoryStream bookStream = new System.IO.MemoryStream();
            //将工作薄写入文件流
            excelBook.Write(bookStream);
            //输出之前调用Seek（偏移量，游标位置) 把0位置指定为开始位置
            bookStream.Seek(0, System.IO.SeekOrigin.Begin);
            //Stream对象,文件类型,文件名称
            return File(bookStream, "application/vnd.ms-excel", fileName);
        }
        /// <summary>
        /// 网站应收明细账
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult overdueadvancemingxizhang(BsgridPage bsgridPage)
        {

            List<B_WebsiteReceivableDetailVo> Recharge = (from tbWebsiteReceivable in myDYXTEntities.B_WebsiteReceivableDetail
                                                          join tbUser in myDYXTEntities.B_UserTable on tbWebsiteReceivable.UserID equals tbUser.UserID
                                                          join tbStatuss in myDYXTEntities.S_StatusTable on tbWebsiteReceivable.StatusID equals tbStatuss.StatusID
                                                          select new B_WebsiteReceivableDetailVo
                                                          {
                                                              WebsiteReceivableDetailID = tbWebsiteReceivable.WebsiteReceivableDetailID,
                                                              Loantitle = tbWebsiteReceivable.Loantitle,
                                                              TimeStr = tbWebsiteReceivable.ReceivableDate.ToString(),
                                                              UserName = tbUser.UserName,
                                                              GatheringMoney = tbWebsiteReceivable.GatheringMoney,
                                                              ReceivableCapital = tbWebsiteReceivable.ReceivableCapital,
                                                              ReceivableAccrual = tbWebsiteReceivable.ReceivableAccrual,
                                                              OverdueDays = tbWebsiteReceivable.OverdueDays,
                                                              WebsiteWhetherPayTimeStr = tbWebsiteReceivable.RealityTime.ToString(),
                                                              RealityAmount = tbWebsiteReceivable.RealityAmount,
                                                              StatusID = tbWebsiteReceivable.StatusID,
                                                              StatusName = tbStatuss.StatusName,
                                                          }).ToList();


            int totalRow = Recharge.Count();
            List<B_WebsiteReceivableDetailVo> notices = Recharge.OrderBy(p => p.WebsiteReceivableDetailID).
           Skip(bsgridPage.GetStartIndex()).
           Take(bsgridPage.pageSize).
           ToList();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].YuQiTianShu = Convert.ToString(Recharge[i].OverdueDays).Trim() + "天";
            }

            Bsgrid<B_WebsiteReceivableDetailVo> bsgrid = new Bsgrid<B_WebsiteReceivableDetailVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 还款信息 
        public ActionResult repaymentView()
        {
            return View();
        }
        /// <summary>
        /// 收款信息
        /// </summary>
        /// <returns></returns>
        public ActionResult selectgathering()
        {
            return View();
        }
        /// <summary>
        /// 全部还款
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="TreetoptypeID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectrepayment(BsgridPage bsgridPage, string LoantitleID, string UserNameID, int TreetoptypeID, string releaseTime, string EnReleaseTime)
        {
            var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                          join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                          join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                          join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                          join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                          where tbLoan.StatusID == 10 || tbLoan.StatusID == 8 || tbLoan.StatusID == 11 || tbLoan.StatusID ==13|| tbLoan.StatusID ==12 || tbLoan.StatusID==30
                          select new B_LoanTableVo
                          {
                              LoanID = tbLoan.LoanID,
                              PaymentNumber = tbLoan.PaymentNumber,
                              UserName = tbUSer.UserName,
                              Loantitle = tbLoan.Loantitle,                        
                              Treetoptype = tbTreetoptype.Treetoptype,
                              YimeStr=tbLoan.Endtime.ToString(),
                              TreetoptypeID = tbLoan.TreetoptypeID,
                              RepayPrincipal=tbLoan.RepayPrincipal,
                              StatusName = tbStatus.StatusName
                          }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            if (!string.IsNullOrEmpty(LoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (TreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == TreetoptypeID).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    varloan = varloan.Where(p => Convert.ToDateTime(p.YimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.YimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = myDYXTEntities.B_LoanTable.Count();

            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = varloan;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 已还款
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="TreetoptypeID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectaccountpaid(BsgridPage bsgridPage, string LoantitleID, string UserNameID, int TreetoptypeID, string releaseTime, string EnReleaseTime)
        {
            var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                           join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                           join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                           join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                           join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                           where  tbLoan.StatusID == 13 || tbLoan.StatusID == 12 || tbLoan.StatusID == 30
                           select new B_LoanTableVo
                           {
                               LoanID = tbLoan.LoanID,
                               PaymentNumber = tbLoan.PaymentNumber,
                               UserName = tbUSer.UserName,
                               Loantitle = tbLoan.Loantitle,
                               Treetoptype = tbTreetoptype.Treetoptype,
                               YimeStr = tbLoan.Endtime.ToString(),
                               TreetoptypeID = tbLoan.TreetoptypeID,
                               RepayPrincipal = tbLoan.RepayPrincipal,
                               StatusName = tbStatus.StatusName
                           }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            if (!string.IsNullOrEmpty(LoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (TreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == TreetoptypeID).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    varloan = varloan.Where(p => Convert.ToDateTime(p.YimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.YimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = myDYXTEntities.B_LoanTable.Count();

            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = varloan;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 正在还款
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="LoantitleID"></param>
        /// <param name="UserNameID"></param>
        /// <param name="TreetoptypeID"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        public ActionResult selectNorepayment(BsgridPage bsgridPage, string LoantitleID, string UserNameID, int TreetoptypeID, string releaseTime, string EnReleaseTime)
        {
            var varloan = (from tbLoan in myDYXTEntities.B_LoanTable
                           join tbUSer in myDYXTEntities.B_UserTable on tbLoan.UserID equals tbUSer.UserID
                           join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbLoan.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                           join tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe on tbLoan.TreetoptypeID equals tbTreetoptype.TreetoptypeID
                           join tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable on tbLoan.RepaymentWayID equals tbRepaymentWay.RepaymentWayID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbLoan.StatusID equals tbStatus.StatusID
                           where tbLoan.StatusID == 10 || tbLoan.StatusID == 8 || tbLoan.StatusID == 11 
                           select new B_LoanTableVo
                           {
                               LoanID = tbLoan.LoanID,
                               PaymentNumber = tbLoan.PaymentNumber,
                               UserName = tbUSer.UserName,
                               Loantitle = tbLoan.Loantitle,
                               Treetoptype = tbTreetoptype.Treetoptype,
                               YimeStr = tbLoan.Endtime.ToString(),
                               TreetoptypeID = tbLoan.TreetoptypeID,
                               RepayPrincipal = tbLoan.RepayPrincipal,
                               StatusName = tbStatus.StatusName
                           }).OrderByDescending(p => p.LoanID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            if (!string.IsNullOrEmpty(LoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (TreetoptypeID > 0)
            {
                varloan = varloan.Where(p => p.TreetoptypeID == TreetoptypeID).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    varloan = varloan.Where(p => Convert.ToDateTime(p.YimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.YimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            int totalRow = myDYXTEntities.B_LoanTable.Count();

            Bsgrid<B_LoanTableVo> bsgrid = new Bsgrid<B_LoanTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = varloan;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 投资信息 
        public ActionResult investmentview()
        {
            return View();
        }

        public ActionResult selecinvestmentxinxi(BsgridPage bsgridPage,string LoantitleID,string UserNameID,string PaymentNumberID,int StatusID)
        {
            var varloan = (from tbInvest in myDYXTEntities.B_InvestTable
                           join tbUSer in myDYXTEntities.B_UserTable on tbInvest.UserID equals tbUSer.UserID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbInvest.StatusID equals tbStatus.StatusID
                           join tbloan in myDYXTEntities.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID                                                                           
                           select new B_InvestTableVo
                           {
                               InvestID=tbInvest.InvestID,
                               UserName= tbUSer.UserName,
                               InvestMoney=tbInvest.InvestMoney,
                               ReleaseTimeStr=tbInvest.InvestTime.ToString(),
                               StatusName = tbStatus.StatusName,
                               WhetherAttorn= tbInvest.WhetherAttorn,
                               InvestReason= tbInvest.InvestReason,
                               Loantitle=tbloan.Loantitle,
                               PaymentNumber= tbloan.PaymentNumber,
                               LoanMoney= tbloan.LoanMoney,
                               StatusID= tbInvest.StatusID
                           }).ToList();
            for (int i = 0; i < varloan.Count; i++)
            {
                if (varloan[i].WhetherAttorn==false)
                {
                    varloan[i].YESNo = "否";
                }
                else
                {
                    varloan[i].YESNo = "是";
                }
            }
            if (!string.IsNullOrEmpty(LoantitleID))
            {
                varloan = varloan.Where(n => n.Loantitle.Contains(LoantitleID)).ToList();
            }
            if (!string.IsNullOrEmpty(UserNameID))
            {
                varloan = varloan.Where(n => n.UserName.Contains(UserNameID)).ToList();
            }
            if (!string.IsNullOrEmpty(PaymentNumberID))
            {
                varloan = varloan.Where(n => n.PaymentNumber.Contains(PaymentNumberID)).ToList();
            }
            if (StatusID > 0)
            {
                varloan = varloan.Where(p => p.StatusID == StatusID).ToList();
            }
            int totalRow = varloan.Count();
            List<B_InvestTableVo> notices = varloan.OrderBy(p => p.LoanID).
              Skip(bsgridPage.GetStartIndex()).
              Take(bsgridPage.pageSize).
              ToList();
            Bsgrid<B_InvestTableVo> bsgrid = new Bsgrid<B_InvestTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public ActionResult SelcttbinvestmentStatus()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbStatus in myDYXTEntities.S_StatusTable
                                         where tbStatus.StatusID == 16 || tbStatus.StatusID == 17 || tbStatus.StatusID == 27
                                               || tbStatus.StatusID == 30 || tbStatus.StatusID == 26                            
                                         select new SelectVo
                                         {
                                             id = tbStatus.StatusID,
                                             text = tbStatus.StatusName

                                         }).ToList();

            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  标种类型 
        public ActionResult Treetoptypeview()
        {
            return View();
        }
        public ActionResult selectTreetoptypexinxi(BsgridPage bsgridPage)
        {

            List<S_TreetoptypetalbeVo> Recharge = (from tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe
                                                    join tbStatuss in myDYXTEntities.S_FundStatustable on tbTreetoptype.FundStatusID equals tbStatuss.FundStatusID
                                                    join tbLimitType in myDYXTEntities.S_LimitTypeTable on tbTreetoptype.LimitTypeID equals tbLimitType.LimitTypeID
                                                    join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbTreetoptype.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                                                    select new S_TreetoptypetalbeVo
                                                    {
                                                       TreetoptypeID=tbTreetoptype.TreetoptypeID,
                                                        Treetoptype= tbTreetoptype.Treetoptype,
                                                        TreetopName= tbTreetoptype.TreetopName,
                                                        Identifier=tbTreetoptype.Identifier,
                                                        FundStatusName= tbStatuss.FundStatusName,
                                                        YearRate=tbTreetoptype.YearRate,
                                                        LoanDeadlineName=tbLoanDeadline.LoanDeadlineName,
                                                        FreezeBail=tbTreetoptype.FreezeBail,
                                                        Describe=tbTreetoptype.Describe,
                                                                                                 
                                                    }).ToList();
            for (int i = 0; i < Recharge.Count; i++)
            {
                Recharge[i].yearRate = Convert.ToString(Recharge[i].YearRate).Trim() + "%";
                Recharge[i].freezeBail = Convert.ToString(Recharge[i].FreezeBail).Trim() + "%";
                
            }
            int totalRow = Recharge.Count();
            List<S_TreetoptypetalbeVo> notices = Recharge.OrderByDescending(p => p.TreetoptypeID).
              Skip(bsgridPage.GetStartIndex()).
              Take(bsgridPage.pageSize).
              ToList();
            Bsgrid<S_TreetoptypetalbeVo> bsgrid = new Bsgrid<S_TreetoptypetalbeVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult selectLimitTypetreetop()
        {
            List<SelectVo> noticeType = (from tbStatus in myDYXTEntities.S_LimitTypeTable
                                         select new SelectVo
                                         {
                                             id = tbStatus.LimitTypeID,
                                             text = tbStatus.LimitType

                                         }).ToList();

            return Json(noticeType, JsonRequestBehavior.AllowGet);

        }

        public ActionResult selecLoanDeadlinetreetop()
        {
            List<SelectVo> noticeType = (from tbStatus in myDYXTEntities.S_LoanDeadlineTable
                                         select new SelectVo
                                         {
                                             id = tbStatus.LoanDeadlineID,
                                             text = tbStatus.LoanDeadlineName

                                         }).ToList();

            return Json(noticeType, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UpdataTreetoptype(int TreetoptypeID)
        {
            try
            {
                S_TreetoptypetalbeVo Recharge = (from tbTreetoptype in myDYXTEntities.S_Treetoptypetalbe
                                                       join tbStatuss in myDYXTEntities.S_FundStatustable on tbTreetoptype.FundStatusID equals tbStatuss.FundStatusID
                                                       join tbLimitType in myDYXTEntities.S_LimitTypeTable on tbTreetoptype.LimitTypeID equals tbLimitType.LimitTypeID
                                                       join tbLoanDeadline in myDYXTEntities.S_LoanDeadlineTable on tbTreetoptype.LoanDeadlineID equals tbLoanDeadline.LoanDeadlineID
                                                       where tbTreetoptype.TreetoptypeID== TreetoptypeID
                                                       select new S_TreetoptypetalbeVo
                                                       {
                                                           TreetoptypeID = tbTreetoptype.TreetoptypeID,
                                                           Treetoptype = tbTreetoptype.Treetoptype,
                                                           TreetopName = tbTreetoptype.TreetopName,
                                                           Identifier = tbTreetoptype.Identifier,
                                                           FundStatusID=tbTreetoptype.FundStatusID,
                                                           LimitTypeID= tbTreetoptype.LimitTypeID,
                                                           LoanDeadlineID=tbTreetoptype.LoanDeadlineID,
                                                           ExamineTime=tbTreetoptype.ExamineTime,
                                                           FundStatusName = tbStatuss.FundStatusName,
                                                           YearRate = tbTreetoptype.YearRate,
                                                           LoanDeadlineName = tbLoanDeadline.LoanDeadlineName,
                                                           FreezeBail = tbTreetoptype.FreezeBail,
                                                           Describe = tbTreetoptype.Describe,
                                                           LowestTenderMoney=  tbTreetoptype.LowestTenderMoney,
                                                           HighestTenderMoney=  tbTreetoptype.HighestTenderMoney,
                                                           Overduefatalism=tbTreetoptype.Overduefatalism
                                                       }).Single();
                return Json(Recharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdataTreetoptypebaocun(S_Treetoptypetalbe strTreetoptype)
        {
            try
            {

            string strMed = "fail";
            var varaccon = (from tbLimitType in myDYXTEntities.S_Treetoptypetalbe
                            where tbLimitType.TreetoptypeID == strTreetoptype.TreetoptypeID
                            select tbLimitType).Single();
            varaccon.TreetopName = strTreetoptype.TreetopName.Trim();
            varaccon.Describe = strTreetoptype.Describe.Trim();
            varaccon.FundStatusID = strTreetoptype.FundStatusID;
            varaccon.LimitTypeID = strTreetoptype.LimitTypeID;
            varaccon.YearRate = strTreetoptype.YearRate;
            varaccon.LoanDeadlineID = strTreetoptype.LoanDeadlineID;
            varaccon.ExamineTime = strTreetoptype.ExamineTime.Trim();
            varaccon.LowestTenderMoney = strTreetoptype.LowestTenderMoney.Trim();
            varaccon.HighestTenderMoney = strTreetoptype.HighestTenderMoney.Trim();
            varaccon.FreezeBail = strTreetoptype.FreezeBail.Trim();
            varaccon.Overduefatalism = strTreetoptype.Overduefatalism.Trim();
            myDYXTEntities.Entry(varaccon).State = System.Data.Entity.EntityState.Modified;
            myDYXTEntities.SaveChanges();
            strMed = "success";
            return Json(strMed, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 还款方式 
        public ActionResult repaymentway()
        {
            return View();
        }
        public ActionResult selectrepaymentwayxinxi(BsgridPage bsgridPage)
        {

            List<S_RepaymentWayTableVo> Recharge = (from tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable    
                                                          join tbStatuss in myDYXTEntities.S_FundStatustable on tbRepaymentWay.FundStatusID equals tbStatuss.FundStatusID
                                                          select new S_RepaymentWayTableVo
                                                          {
                                                              RepaymentWayID=tbRepaymentWay.RepaymentWayID,
                                                              RepaymentWayName= tbRepaymentWay.RepaymentWayName,
                                                              IdentificationName=tbRepaymentWay.IdentificationName,
                                                              Title=tbRepaymentWay.Title,
                                                              FundStatusName= tbStatuss.FundStatusName,
                                                              AlgorithmMessage = tbRepaymentWay.AlgorithmMessage

                                                          }).ToList();



            int totalRow = Recharge.Count();
            List<S_RepaymentWayTableVo> notices = Recharge.OrderByDescending(p => p.RepaymentWayID).
              Skip(bsgridPage.GetStartIndex()).
              Take(bsgridPage.pageSize).
              ToList();
            Bsgrid<S_RepaymentWayTableVo> bsgrid = new Bsgrid<S_RepaymentWayTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Updatarepaymentway(int RepaymentWayID)
        {
            try
            {
                S_RepaymentWayTableVo varLimitType = (from tbRepaymentWay in myDYXTEntities.S_RepaymentWayTable
                                                   join tbStatuss in myDYXTEntities.S_FundStatustable on tbRepaymentWay.FundStatusID equals tbStatuss.FundStatusID
                                                   where tbRepaymentWay.RepaymentWayID == RepaymentWayID
                                                      select new S_RepaymentWayTableVo
                                                   {
                                                          RepaymentWayID = tbRepaymentWay.RepaymentWayID,
                                                          RepaymentWayName = tbRepaymentWay.RepaymentWayName,
                                                          IdentificationName = tbRepaymentWay.IdentificationName,
                                                          Title = tbRepaymentWay.Title,
                                                          FundStatusID=tbRepaymentWay.FundStatusID,
                                                          FundStatusName = tbStatuss.FundStatusName,
                                                          AlgorithmMessage = tbRepaymentWay.AlgorithmMessage
                                                      }).Single();
                return Json(varLimitType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Updatapaymentwaybaocun(S_RepaymentWayTable RepaymentWay)
        {
            try
            {

         
            string strMed = "fail";
            var varaccon = (from tbLimitType in myDYXTEntities.S_RepaymentWayTable
                            where tbLimitType.RepaymentWayID == RepaymentWay.RepaymentWayID
                            select tbLimitType).Single();
            varaccon.Title = RepaymentWay.Title.Trim();
            varaccon.FundStatusID = RepaymentWay.FundStatusID;
            varaccon.AlgorithmMessage = RepaymentWay.AlgorithmMessage.Trim();
            myDYXTEntities.Entry(varaccon).State = System.Data.Entity.EntityState.Modified;
            myDYXTEntities.SaveChanges();
            strMed = "success";
            return Json(strMed, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 借款类型
        public ActionResult lendmoneytypeview()
        {
            return View();
        }
        public ActionResult selectExpensestypexinxi(BsgridPage bsgridPage)
        {

            List<B_ExpensestypeTableVo> Recharge = (from tbExpensestype in myDYXTEntities.B_ExpensestypeTable
                                                    join tbUserType in myDYXTEntities.S_UserTypeTable on tbExpensestype.UserTypeID equals tbUserType.UserTypeID
                                                    join tbStatuss in myDYXTEntities.S_FundStatustable on tbExpensestype.FundStatusID equals tbStatuss.FundStatusID
                                                    join tbLoanexpenses in myDYXTEntities.S_LoanexpensesType on tbExpensestype.LoanexpensesTypeID equals tbLoanexpenses.LoanexpensesTypeID
                                                    select new B_ExpensestypeTableVo
                                                    {
                                                        ExpensestypeID = tbExpensestype.ExpensestypeID,
                                                        ExpensestypeName= tbExpensestype.ExpensestypeName,
                                                        Identifier=tbExpensestype.Identifier,
                                                        UserTypeName= tbUserType.UserTypeName,
                                                        FundStatusName= tbStatuss.FundStatusName,
                                                        LoanexpensesName=tbLoanexpenses.LoanexpensesName,
                                                        bucklemoney= tbExpensestype.bucklemoney
                                                    }).ToList();
            int totalRow = Recharge.Count();
            List<B_ExpensestypeTableVo> notices = Recharge.OrderBy(p => p.ExpensestypeID).
              Skip(bsgridPage.GetStartIndex()).
              Take(bsgridPage.pageSize).
              ToList();
            Bsgrid<B_ExpensestypeTableVo> bsgrid = new Bsgrid<B_ExpensestypeTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelctFundStatus()
        {

            List<SelectVo> Province = (from tbCostType in myDYXTEntities.S_FundStatustable
                                       select new SelectVo
                                       {
                                           id = tbCostType.FundStatusID,
                                           text = tbCostType.FundStatusName

                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelctUsertype()
        {

            List<SelectVo> Province = (from tbCostType in myDYXTEntities.S_UserTypeTable
                                       where tbCostType.UserTypeID==6|| tbCostType.UserTypeID==7
                                       select new SelectVo
                                       {
                                           id = tbCostType.UserTypeID,
                                           text = tbCostType.UserTypeName

                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelctLoanexpensesType()
        {

            List<SelectVo> Province = (from tbCostType in myDYXTEntities.S_LoanexpensesType
                                       where tbCostType.FundStatusID==1
                                       select new SelectVo
                                       {
                                           id = tbCostType.LoanexpensesTypeID,
                                           text = tbCostType.LoanexpensesName

                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }

        public ActionResult insertExpensestype(B_ExpensestypeTable strExpensestype)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tbFundCost in myDYXTEntities.B_ExpensestypeTable
                                      where tbFundCost.ExpensestypeName == strExpensestype.ExpensestypeName
                                      select tbFundCost).Count();
                if (oldStudentRows == 0)
                {

                    myDYXTEntities.B_ExpensestypeTable.Add(strExpensestype);
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
                else
                {
                    strMsg = "exsit";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Updatabinding(int ExpensestypeID)
        {
            try
            {
                var paytypetable = (from tbExpensestype in myDYXTEntities.B_ExpensestypeTable
                                    join tbUserType in myDYXTEntities.S_UserTypeTable on tbExpensestype.UserTypeID equals tbUserType.UserTypeID
                                    join tbStatuss in myDYXTEntities.S_FundStatustable on tbExpensestype.FundStatusID equals tbStatuss.FundStatusID
                                    join tbLoanexpenses in myDYXTEntities.S_LoanexpensesType on tbExpensestype.LoanexpensesTypeID equals tbLoanexpenses.LoanexpensesTypeID
                                    where tbExpensestype.ExpensestypeID== ExpensestypeID
                                    select new B_ExpensestypeTableVo
                                    {
                                        ExpensestypeID = tbExpensestype.ExpensestypeID,
                                        FundStatusID= tbExpensestype.FundStatusID,
                                        LoanexpensesTypeID=  tbExpensestype.LoanexpensesTypeID,
                                        UserTypeID= tbExpensestype.UserTypeID,
                                        ExpensestypeName = tbExpensestype.ExpensestypeName,
                                        Identifier = tbExpensestype.Identifier,
                                        UserTypeName = tbUserType.UserTypeName,
                                        FundStatusName = tbStatuss.FundStatusName,
                                        LoanexpensesName = tbLoanexpenses.LoanexpensesName,
                                        bucklemoney = tbExpensestype.bucklemoney
                                    }).Single();
                return Json(paytypetable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdataExpensestype(B_ExpensestypeTable strExpensestype)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tbFundCost in myDYXTEntities.B_ExpensestypeTable
                                      where tbFundCost.ExpensestypeName == strExpensestype.ExpensestypeName && tbFundCost.ExpensestypeID != strExpensestype.ExpensestypeID
                                      select tbFundCost).Count();
                if (oldStudentRows == 0)
                {
                    myDYXTEntities.Entry(strExpensestype).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
                else
                {
                    strMsg = "exsit";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult deletezijinfeiyong(int ExpensestypeID)
        {
            string strMsg = "fail";
            try
            {
                B_ExpensestypeTable varFundCost = (from tbFundCost in myDYXTEntities.B_ExpensestypeTable
                                               where tbFundCost.ExpensestypeID == ExpensestypeID
                                                   select tbFundCost).Single();
                //删除数据
                myDYXTEntities.B_ExpensestypeTable.Remove(varFundCost);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult selectLoanexpensesTypexinxi(BsgridPage bsgridPage)
        {

            List<S_LoanexpensesTypeVo> Recharge = (from tbLoanexpensesType in myDYXTEntities.S_LoanexpensesType
                                                
                                                    join tbStatuss in myDYXTEntities.S_FundStatustable on tbLoanexpensesType.FundStatusID equals tbStatuss.FundStatusID
                                                  
                                                    select new S_LoanexpensesTypeVo
                                                    {
                                                      LoanexpensesTypeID=tbLoanexpensesType.LoanexpensesTypeID,
                                                        LoanexpensesName=tbLoanexpensesType.LoanexpensesName,
                                                        Identifier=tbLoanexpensesType.Identifier,
                                                        FundStatusID= tbLoanexpensesType.FundStatusID,
                                                        FundStatusName= tbStatuss.FundStatusName
                                                    }).ToList();
            int totalRow = Recharge.Count();
            List<S_LoanexpensesTypeVo> notices = Recharge.OrderBy(p => p.LoanexpensesTypeID).
              Skip(bsgridPage.GetStartIndex()).
              Take(bsgridPage.pageSize).
              ToList();
            Bsgrid<S_LoanexpensesTypeVo> bsgrid = new Bsgrid<S_LoanexpensesTypeVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }


        public ActionResult inserttypeLoanexpensesType(S_LoanexpensesType strLoanexpensesType)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tbCostType in myDYXTEntities.S_LoanexpensesType
                                      where tbCostType.LoanexpensesName == strLoanexpensesType.LoanexpensesName
                                      select tbCostType).Count();
                if (oldStudentRows == 0)
                {

                    myDYXTEntities.S_LoanexpensesType.Add(strLoanexpensesType);
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
                else
                {
                    strMsg = "exsit";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Updataloanexpensestype(int LoanexpensesTypeID)
        {
            try
            {
                var paytypetable = (from tbLoanexpensesType in myDYXTEntities.S_LoanexpensesType

                                    join tbStatuss in myDYXTEntities.S_FundStatustable on tbLoanexpensesType.FundStatusID equals tbStatuss.FundStatusID
                                    where tbLoanexpensesType.LoanexpensesTypeID== LoanexpensesTypeID
                                    select new S_LoanexpensesTypeVo
                                    {
                                        LoanexpensesTypeID = tbLoanexpensesType.LoanexpensesTypeID,
                                        LoanexpensesName = tbLoanexpensesType.LoanexpensesName,
                                        Identifier = tbLoanexpensesType.Identifier,
                                        FundStatusID = tbLoanexpensesType.FundStatusID,
                                        FundStatusName = tbStatuss.FundStatusName
                                    }).Single();
                return Json(paytypetable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Updataloanexpensestypebaocun(S_LoanexpensesType strLoanexpensesType)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tbCostType in myDYXTEntities.S_LoanexpensesType
                                      where tbCostType.LoanexpensesName == strLoanexpensesType.LoanexpensesName && tbCostType.LoanexpensesTypeID != strLoanexpensesType.LoanexpensesTypeID
                                      select tbCostType).Count();
                if (oldStudentRows == 0)
                {
                    myDYXTEntities.Entry(strLoanexpensesType).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
                else
                {
                    strMsg = "exsit";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult deleteLoanexpensesType(int LoanexpensesTypeID)
        {
            string strMsg = "fail";
            try
            {
                S_LoanexpensesType varFundCost = (from tbFundCost in myDYXTEntities.S_LoanexpensesType

                                                                where tbFundCost.LoanexpensesTypeID == LoanexpensesTypeID
                                                                select tbFundCost).Single();
                //删除数据
                myDYXTEntities.S_LoanexpensesType.Remove(varFundCost);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion

       


    }
}