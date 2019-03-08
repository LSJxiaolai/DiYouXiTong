using DYXTHT_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DYXTHT_MVC.Vo;

namespace DYXTHT_MVC.Areas.PopularizeManage.Controllers
{
    public class PopularController : Controller
    {
        // GET: PopularizeManage/Popular
        Models.DYXTEntities myDYXTEntities = new DYXTEntities();
        #region 推广管理建立关联 
        public ActionResult SpreadUserView()
        {
            return View();
        }
        /// <summary>
        /// 全部用户
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectSpreadUser(BsgridPage bsgridPage)
        {
            var VarVip = from tbSpreadUser in myDYXTEntities.B_SpreadUserTable
                         join tbUser in myDYXTEntities.B_UserTable on tbSpreadUser.UserID equals tbUser.UserID
                         join tbSpreadType in myDYXTEntities.B_SpreadTypeTable on tbSpreadUser.SpreadTypeID equals tbSpreadType.SpreadTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbSpreadUser.StatusID equals tbStatus.StatusID
                         select new B_SpreadUserVo
                         {
                             SpreadUserID = tbSpreadUser.SpreadUserID,
                             UserName = tbUser.UserName,
                             PostBox = tbUser.PostBox,
                             Name = tbSpreadType.Name,
                             StatusName = tbStatus.StatusName,
                             ReleaseTimeStr = tbSpreadUser.RelevanceTime.ToString()
                         };

            int totalRow = VarVip.Count();
            List<B_SpreadUserVo> notices = VarVip.OrderBy(p => p.SpreadUserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_SpreadUserVo> bsgrid = new Bsgrid<B_SpreadUserVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 关联用户
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectSpreadUserbyID(BsgridPage bsgridPage)
        {
            var VarVip = from tbSpreadUser in myDYXTEntities.B_SpreadUserTable
                         join tbUser in myDYXTEntities.B_UserTable on tbSpreadUser.UserID equals tbUser.UserID
                         join tbSpaeraduser in myDYXTEntities.B_UserTable on tbSpreadUser.SpreadRenID equals tbSpaeraduser.UserID
                         join tbSpreadType in myDYXTEntities.B_SpreadTypeTable on tbSpreadUser.SpreadTypeID equals tbSpreadType.SpreadTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbSpreadUser.StatusID equals tbStatus.StatusID
                         where tbSpreadUser.StatusID == 19
                         select new B_SpreadUserVo
                         {
                             SpreadUserID = tbSpreadUser.SpreadUserID,
                             UserName = tbUser.UserName,
                             PostBox = tbUser.PostBox,
                             Name = tbSpreadType.Name,
                             SpreadRenName = tbSpaeraduser.UserName,
                             StatusName = tbStatus.StatusName,
                             ReleaseTimeStr = tbSpreadUser.RelevanceTime.ToString()
                         };

            int totalRow = VarVip.Count();
            List<B_SpreadUserVo> notices = VarVip.OrderBy(p => p.SpreadUserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_SpreadUserVo> bsgrid = new Bsgrid<B_SpreadUserVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 未关联用户
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectSpreadUserbygxID(BsgridPage bsgridPage)
        {
            var VarVip = from tbSpreadUser in myDYXTEntities.B_SpreadUserTable
                         join tbUser in myDYXTEntities.B_UserTable on tbSpreadUser.UserID equals tbUser.UserID
                         join tbSpreadType in myDYXTEntities.B_SpreadTypeTable on tbSpreadUser.SpreadTypeID equals tbSpreadType.SpreadTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbSpreadUser.StatusID equals tbStatus.StatusID
                         where tbSpreadUser.StatusID == 20
                         select new B_SpreadUserVo
                         {
                             SpreadUserID = tbSpreadUser.SpreadUserID,
                             UserName = tbUser.UserName,
                             PostBox = tbUser.PostBox,
                             Name = tbSpreadType.Name,
                             StatusName = tbStatus.StatusName,
                             ReleaseTimeStr = tbSpreadUser.RelevanceTime.ToString()
                         };

            int totalRow = VarVip.Count();
            List<B_SpreadUserVo> notices = VarVip.OrderBy(p => p.SpreadUserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_SpreadUserVo> bsgrid = new Bsgrid<B_SpreadUserVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectSpreadTypexinxi()
        {

            List<SelectVo> Province = (from tB_SpreadType in myDYXTEntities.B_SpreadTypeTable
                                       select new SelectVo
                                       {
                                           id = tB_SpreadType.SpreadTypeID,
                                           text = tB_SpreadType.Name
                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectUpdataSpreadUser(int SpreadUserID)
        {

            try
            {
                var Recharge = (from tbSpreadUser in myDYXTEntities.B_SpreadUserTable
                                join tbUser in myDYXTEntities.B_UserTable on tbSpreadUser.UserID equals tbUser.UserID
                                where tbSpreadUser.SpreadUserID == SpreadUserID
                                select new B_SpreadUserVo
                                {
                                    SpreadUserID = tbSpreadUser.SpreadUserID,
                                    SpreadTypeID = tbSpreadUser.SpreadTypeID,
                                    UserName = tbUser.UserName
                                }).Single();

                return Json(Recharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SelectSpreadRen(string SpreadRen, string UserName)
        {

            try
            {
                //定义返回
                string strMsg = "fail";
                int inruser = (from tbUser in myDYXTEntities.B_UserTable
                               where tbUser.UserName == SpreadRen && UserName != SpreadRen
                               select tbUser).Count();
                if (inruser == 0)
                {
                    strMsg = "fail";
                }
                else
                {
                    strMsg = "success";
                }
                return Json(strMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdataSperadtypeBiao(B_SpreadUserTable SpreadUser, string SpreadRen)
        {

            //定义返回
            string strMsg = "fail";
            try
            {
                B_SpreadUserTable spuser = (from tbspuser in myDYXTEntities.B_SpreadUserTable
                                            where tbspuser.SpreadUserID == SpreadUser.SpreadUserID
                                            select tbspuser).Single();

                B_UserTable inruser = (from tbUser in myDYXTEntities.B_UserTable
                                       where tbUser.UserName == SpreadRen
                                       select tbUser).Single();
                spuser.SpreadRenID = inruser.UserID;
                spuser.RelevanceTime = DateTime.Now;
                spuser.StatusID = 19;
                //修改公告类型
                myDYXTEntities.Entry(spuser).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {
                    int varPersonalSpreadMessage = (from tbPersonalSpreadMessage in myDYXTEntities.B_PersonalSpreadMessage
                                                    where tbPersonalSpreadMessage.SpreadUserID == inruser.UserID
                                                    select tbPersonalSpreadMessage).Count();
                    if (varPersonalSpreadMessage == 0)
                    {
                        B_PersonalSpreadMessage PersonalSpreadMessage = new B_PersonalSpreadMessage();
                        PersonalSpreadMessage.SpreadUserID = inruser.UserID;
                        PersonalSpreadMessage.SpreadPeople = 1;
                        PersonalSpreadMessage.InvestTime = 0;
                        PersonalSpreadMessage.InvestAmount = 0;
                        PersonalSpreadMessage.InvestTiCheng = 0;
                        PersonalSpreadMessage.RepaymentTime = 0;
                        PersonalSpreadMessage.RepaymentAmount = 0;
                        PersonalSpreadMessage.RepaymentTiCheng = 0;
                        myDYXTEntities.B_PersonalSpreadMessage.Add(PersonalSpreadMessage);
                        myDYXTEntities.SaveChanges();
                    }
                    else
                    {
                        B_PersonalSpreadMessage strPersonalSpreadMessage = (from tbPersonalSpreadMessage in myDYXTEntities.B_PersonalSpreadMessage
                                                                            where tbPersonalSpreadMessage.SpreadUserID == inruser.UserID
                                                                            select tbPersonalSpreadMessage).Single();
                        strPersonalSpreadMessage.SpreadPeople = strPersonalSpreadMessage.SpreadPeople + 1;
                        myDYXTEntities.Entry(strPersonalSpreadMessage).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                    }


                    strMsg = "success";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectSpreadUserGL(int SpreadUserID)
        {

            try
            {
                var Recharge = (from tbSpreadUser in myDYXTEntities.B_SpreadUserTable
                                join tbUser in myDYXTEntities.B_UserTable on tbSpreadUser.UserID equals tbUser.UserID
                                join tbSpaeraduser in myDYXTEntities.B_UserTable on tbSpreadUser.SpreadRenID equals tbSpaeraduser.UserID
                                join tbSpreadType in myDYXTEntities.B_SpreadTypeTable on tbSpreadUser.SpreadTypeID equals tbSpreadType.SpreadTypeID
                                join tbStatus in myDYXTEntities.S_StatusTable on tbSpreadUser.StatusID equals tbStatus.StatusID
                                where tbSpreadUser.SpreadUserID == SpreadUserID
                                select new B_SpreadUserVo
                                {
                                    SpreadUserID = tbSpreadUser.SpreadUserID,
                                    UserName = tbUser.UserName,
                                    PostBox = tbUser.PostBox,
                                    Name = tbSpreadType.Name,
                                    SpreadRenName = tbSpaeraduser.UserName,
                                    StatusName = tbStatus.StatusName,
                                    ReleaseTimeStr = tbSpreadUser.RelevanceTime.ToString()
                                }).Single();

                return Json(Recharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 网站平台用户个人推广信息
        public ActionResult PersonalSpreadMessageView()
        {
            return View();
        }
        public ActionResult SelectPersonalSpreadMessage(BsgridPage bsgridPage, string SpreadUserNameID)
        {
            var VarVip = from tbPersonalSpreadMessage in myDYXTEntities.B_PersonalSpreadMessage
                         join tbUser in myDYXTEntities.B_UserTable on tbPersonalSpreadMessage.SpreadUserID equals tbUser.UserID
                         select new B_PersonalSpreadMessageVo
                         {
                             PersonalSpreadID = tbPersonalSpreadMessage.PersonalSpreadID,
                             SpreadUserName = tbUser.UserName,
                             SpreadUserID = tbPersonalSpreadMessage.SpreadUserID,
                             SpreadPeople = tbPersonalSpreadMessage.SpreadPeople,
                             InvestTime = tbPersonalSpreadMessage.InvestTime,
                             InvestAmount = tbPersonalSpreadMessage.InvestAmount,
                             InvestTiCheng = tbPersonalSpreadMessage.InvestTiCheng,
                             RepaymentTime = tbPersonalSpreadMessage.RepaymentTime,
                             RepaymentAmount = tbPersonalSpreadMessage.RepaymentAmount,
                             RepaymentTiCheng = tbPersonalSpreadMessage.RepaymentTiCheng
                         };

            if (!string.IsNullOrEmpty(SpreadUserNameID))
            {
                VarVip = VarVip.Where(n => n.SpreadUserName.Contains(SpreadUserNameID));
            }
            int totalRow = VarVip.Count();
            List<B_PersonalSpreadMessageVo> notices = VarVip.OrderBy(p => p.PersonalSpreadID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_PersonalSpreadMessageVo> bsgrid = new Bsgrid<B_PersonalSpreadMessageVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult selectTuiGuangJiLu(BsgridPage bsgridPage,int SpreadUserID)
        {
            var varuserjilu = (from tbSpreadUser in myDYXTEntities.B_SpreadUserTable
                               join tbuser in myDYXTEntities.B_UserTable on tbSpreadUser.UserID equals tbuser.UserID
                               join tbSpreadRen in myDYXTEntities.B_UserTable on tbSpreadUser.SpreadRenID equals tbSpreadRen.UserID
                               join tbSpreadType in myDYXTEntities.B_SpreadTypeTable on tbSpreadUser.SpreadTypeID equals tbSpreadType.SpreadTypeID
                               where tbSpreadUser.SpreadRenID== SpreadUserID
                               select new B_SpreadUserVo
                               {
                                   SpreadUserID = tbSpreadUser.SpreadUserID,
                                   UserName = tbuser.UserName,
                                   SpreadRenName = tbSpreadRen.UserName,
                                   Name = tbSpreadType.Name,
                                   ReleaseTimeStr = tbSpreadUser.RelevanceTime.ToString()
                               }).ToList();
            int totalRow = varuserjilu.Count();
            List<B_SpreadUserVo> notices = varuserjilu.OrderBy(p => p.SpreadTypeID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_SpreadUserVo> bsgrid = new Bsgrid<B_SpreadUserVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 推广资金设置 
        public ActionResult Popularizefundsetup()
        {
            return View();
        }
        /// <summary>
        /// 资金设置查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectSpreadType(BsgridPage bsgridPage)
        {
            var VarVip = from tbSpreadType in myDYXTEntities.B_SpreadTypeTable
                         join tbFundStatus in myDYXTEntities.S_FundStatustable on tbSpreadType.FundStatusID equals tbFundStatus.FundStatusID
                         select new B_SpreadTypeVo
                         {
                             SpreadTypeID = tbSpreadType.SpreadTypeID,
                             FundStatusID = tbSpreadType.FundStatusID,
                             Name = tbSpreadType.Name,
                             Title = tbSpreadType.Title,
                             IdentificationName = tbSpreadType.IdentificationName,
                             FundStatusName = tbFundStatus.FundStatusName,
                             CapitalType = tbSpreadType.CapitalType,
                             Scale = tbSpreadType.Scale
                         };

            int totalRow = VarVip.Count();
            List<B_SpreadTypeVo> notices = VarVip.OrderBy(p => p.SpreadTypeID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_SpreadTypeVo> bsgrid = new Bsgrid<B_SpreadTypeVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectFundStatus()
        {

            List<SelectVo> Province = (from tbFundStatus in myDYXTEntities.S_FundStatustable
                                       select new SelectVo
                                       {
                                           id = tbFundStatus.FundStatusID,
                                           text = tbFundStatus.FundStatusName
                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectUpdatafundstatus(int SpreadTypeID)
        {

            try
            {
                var Recharge = (from tbSpreadType in myDYXTEntities.B_SpreadTypeTable
                                join tbFundStatus in myDYXTEntities.S_FundStatustable on tbSpreadType.FundStatusID equals tbFundStatus.FundStatusID
                                where tbSpreadType.SpreadTypeID == SpreadTypeID
                                select new B_SpreadTypeVo
                                {
                                    SpreadTypeID = tbSpreadType.SpreadTypeID,
                                    FundStatusID = tbSpreadType.FundStatusID,
                                    Name = tbSpreadType.Name.Trim(),
                                    Title = tbSpreadType.Title.Trim(),
                                    IdentificationName = tbSpreadType.IdentificationName.Trim(),
                                    FundStatusName = tbFundStatus.FundStatusName.Trim(),
                                    CapitalType = tbSpreadType.CapitalType.Trim(),
                                    Scale = tbSpreadType.Scale.Trim()
                                }).Single();

                return Json(Recharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Updatefundstatua(B_SpreadTypeTable SpreadType)
        {
            //定义返回
            string strMsg = "fail";

            //查询除了自身外查询是否已经存在
            int oldCount = (from tbSpreadType in myDYXTEntities.B_SpreadTypeTable
                            where tbSpreadType.SpreadTypeID != SpreadType.SpreadTypeID
                            && tbSpreadType.Name == SpreadType.Name.Trim() && tbSpreadType.Title == SpreadType.Title.Trim()
                            select tbSpreadType).Count();
            if (oldCount == 0)
            {
                try
                {
                    //修改公告类型
                    myDYXTEntities.Entry(SpreadType).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                strMsg = "exist";//已经存在
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 推广记录管理 
        public ActionResult spreadrecordxiew()
        {
            return View();
        }
        public ActionResult SelectSpreadRecord(BsgridPage bsgridPage)
        {
            var VarVip = from tbSpreadRecord in myDYXTEntities.B_SpreadRecordTable
                         join tbUser in myDYXTEntities.B_UserTable on tbSpreadRecord.SpreadUserID equals tbUser.UserID
                         join tbSpreadCustomUser in myDYXTEntities.B_UserTable on tbSpreadRecord.SpreadCustomID equals tbSpreadCustomUser.UserID
                         join tbSpreadType in myDYXTEntities.B_SpreadTypeTable on tbSpreadRecord.SpreadTypeID equals tbSpreadType.SpreadTypeID
                         select new B_SpreadRecordVo
                         {
                             SpreadRecordID = tbSpreadRecord.SpreadRecordID,
                             SpreadUserName = tbUser.UserName.Trim(),
                             SpreadCustomName = tbSpreadCustomUser.UserName.Trim(),
                             SpreadTypeName = tbSpreadType.Name.Trim(),
                             FundType = tbSpreadRecord.FundType.Trim(),
                             Scale = tbSpreadType.Scale.Trim(),
                             SpreadAmount = tbSpreadRecord.SpreadAmount,
                             ReleaseTimeStr = tbSpreadRecord.SubmitTime.ToString().Trim(),
                             Remark = tbSpreadRecord.Remark.Trim()
                         };

            int totalRow = VarVip.Count();
            List<B_SpreadRecordVo> notices = VarVip.OrderByDescending(p => p.SpreadRecordID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_SpreadRecordVo> bsgrid = new Bsgrid<B_SpreadRecordVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}