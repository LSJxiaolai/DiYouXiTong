using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DYXTHT_MVC.Models;
using DYXTHT_MVC.Vo;
using DYXTHT_MVC.Common;

namespace DYXTHT_MVC.Areas.UserManagement.Controllers
{
    public class UsermanagementController : Controller
    {
        Models.DYXTEntities myDYXTEntities = new DYXTEntities();
        // GET: UserManagement/Usermanagement
        #region 用户列表     
        public ActionResult Usertabulation()
        {
            return View();
        }
        /// <summary>
        /// 查询用户登录信息
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="UserNameID"></param>
        /// <param name="PostBoxID"></param>
        /// <returns></returns>
        public ActionResult UserLoginDetail(BsgridPage bsgridPage, string UserNameID, string PostBoxID)
        {
            var notict = from tbUserLoginDetail in myDYXTEntities.B_UserLoginDetailTable
                         join tbAccount in myDYXTEntities.B_AccountTable on tbUserLoginDetail.AccountID equals tbAccount.AccountID
                         join tbuser in myDYXTEntities.B_UserTable on tbAccount.AccountID equals tbuser.AccountID
                         select new B_UserLoginDetailVo
                         {
                             UserLoginDetailID = tbUserLoginDetail.UserLoginDetailID,
                             UserName = tbuser.UserName,
                             PostBox = tbUserLoginDetail.PostBox,
                             LoginFrequency = tbUserLoginDetail.LoginFrequency,
                             StrRegisterTime = tbUserLoginDetail.RegisterTime.ToString(),
                             StrlastLoginTime = tbUserLoginDetail.lastLoginTime.ToString(),
                             lastLoginIP=tbUserLoginDetail.lastLoginIP,
                             StrEndLoginTime = tbUserLoginDetail.EndLoginTime.ToString(),
                             EndLogin=tbUserLoginDetail.EndLogin
                         };
            if (!string.IsNullOrEmpty(UserNameID))
            {
                notict = notict.Where(n => n.UserName.Contains(UserNameID));
            }
            if (!string.IsNullOrEmpty(PostBoxID))
            {
                notict = notict.Where(n => n.PostBox.Contains(PostBoxID));
            }
            int totalRow = notict.Count();
            List<B_UserLoginDetailVo> notices = notict.OrderBy(p => p.UserLoginDetailID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_UserLoginDetailVo> bsgrid = new Bsgrid<B_UserLoginDetailVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改绑定
        /// </summary>
        /// <param name="UserLoginDetailID"></param>
        /// <returns></returns>
        public ActionResult Userfrombiangding(int UserLoginDetailID)
        {
            try
            {
                B_UserLoginDetailTable notict = (from tbUserLoginDetail in myDYXTEntities.B_UserLoginDetailTable
                                                 join tbAccount in myDYXTEntities.B_AccountTable on tbUserLoginDetail.AccountID equals tbAccount.AccountID
                                                 join tbuser in myDYXTEntities.B_UserTable on tbAccount.AccountID equals tbuser.AccountID
                                                 where tbUserLoginDetail.UserLoginDetailID == UserLoginDetailID
                                                 select new B_UserLoginDetailVo
                                                 {
                                                     UserLoginDetailID = tbUserLoginDetail.UserLoginDetailID,
                                                     UserName = tbuser.UserName,
                                                     PostBox = tbUserLoginDetail.PostBox,
                                                     LoginFrequency = tbUserLoginDetail.LoginFrequency,
                                                     StrRegisterTime = tbUserLoginDetail.RegisterTime.ToString(),
                                                     StrlastLoginTime = tbUserLoginDetail.lastLoginTime.ToString(),
                                                     StrEndLoginTime = tbUserLoginDetail.EndLoginTime.ToString()
                                                 }).Single();
                return Json(notict, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        /// <summary>
        /// 修改用户信息保存
        /// </summary>
        /// <param name="UserLoginDetail"></param>
        /// <param name="StrRegisterTime"></param>
        /// <param name="StrlastLoginTime"></param>
        /// <param name="StrEndLoginTime"></param>
        /// <returns></returns>
        public ActionResult UpdataUSerxinxi(B_UserLoginDetailTable UserLoginDetail, string StrRegisterTime, string StrlastLoginTime, string StrEndLoginTime)
        {
            string strMsg = "fali";
            try
            {

                B_UserLoginDetailTable varUserLoginDetail = (from tbB_UserLoginDetail in myDYXTEntities.B_UserLoginDetailTable
                                                             where tbB_UserLoginDetail.UserLoginDetailID == UserLoginDetail.UserLoginDetailID
                                                             select tbB_UserLoginDetail).Single();
                varUserLoginDetail.PostBox = UserLoginDetail.PostBox;
                varUserLoginDetail.LoginFrequency = UserLoginDetail.LoginFrequency;
                varUserLoginDetail.RegisterTime = Convert.ToDateTime(StrRegisterTime);
                varUserLoginDetail.lastLoginTime = Convert.ToDateTime(StrlastLoginTime);
                varUserLoginDetail.EndLoginTime = Convert.ToDateTime(StrEndLoginTime);
                myDYXTEntities.Entry(varUserLoginDetail).State = System.Data.Entity.EntityState.Modified;
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 添加用户
        public ActionResult InserUser()
        {
            return View();
        }

        public ActionResult insertUserPwP(B_AccountTable Account, B_UserTable User)
        {
            string strMed = "fail";
            string password = AESEncryptHelper.AESEncrypt(Account.Password);
            string Paypassword = AESEncryptHelper.AESEncrypt(User.PayPassword);
            int intaccon = (from tbaccounnt in myDYXTEntities.B_AccountTable
                            where tbaccounnt.User == User.UserName
                            select tbaccounnt).Count();
            if (intaccon == 0)
            {
                Account.User = User.UserName;
                Account.Password = password;
                Account.Cancel = false;
                myDYXTEntities.B_AccountTable.Add(Account);
                myDYXTEntities.SaveChanges();

                int varuser = (from tbUSer in myDYXTEntities.B_UserTable
                               where tbUSer.UserName == User.UserName
                               select tbUSer).Count();
                if (varuser == 0)
                {
                    var varaccon = (from tbaccounnt in myDYXTEntities.B_AccountTable
                                    where tbaccounnt.User == User.UserName
                                    select tbaccounnt).Single();

                    User.AccountID = varaccon.AccountID;
                    User.PayPassword = Paypassword;
                    User.UserTypeID = 2;
                    User.Time = DateTime.Now;
                    User.PropertyAmounts = 0;
                    User.UsableMoney = 0;
                    User.FreezeMoney = 0;
                    User.WaitMoney = 0;
                    User.CompensatoryMoney = 0;
                    myDYXTEntities.B_UserTable.Add(User);
                    myDYXTEntities.SaveChanges();
                    strMed = "success";
                }
                else
                {
                    strMed = "UserNamefail";//用户名已存在
                }
            }
            else
            {
                strMed = "AccountUserfail";//账号名已存在
            }

            return Json(strMed, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 用户信息
        public ActionResult Userselectxinxi()
        {
            return View();
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="UserNameID"></param>
        /// <returns></returns>
        public ActionResult SelectUSerxinxi(BsgridPage bsgridPage, string UserNameID)
        {
            var notict = from tbUser in myDYXTEntities.B_UserTable
                         join tbUsertype in myDYXTEntities.S_UserTypeTable on tbUser.UserTypeID equals tbUsertype.UserTypeID
                         join tbAccount in myDYXTEntities.B_AccountTable on tbUser.AccountID equals tbAccount.AccountID
                         join tbUserLoginDetail in myDYXTEntities.B_UserLoginDetailTable on tbUser.AccountID equals tbUserLoginDetail.AccountID
                         select new USerbankVo
                         {
                             UserID = tbUser.UserID,
                             UserName = tbUser.UserName,
                             PostBox = tbUser.PostBox,
                             Integral = tbUser.Integral,
                             UserTypeName = tbUsertype.UserTypeName,
                             ReleaseTimeStr = tbUserLoginDetail.EndLoginTime.ToString()

                         };
            if (!string.IsNullOrEmpty(UserNameID))
            {
                notict = notict.Where(n => n.UserName.Contains(UserNameID));
            }

            int totalRow = notict.Count();
            List<USerbankVo> notices = notict.OrderBy(p => p.UserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<USerbankVo> bsgrid = new Bsgrid<USerbankVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///基本信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult selectfrombiangding(int UserID)
        {
            try
            {
                B_UserTable varUser = (from tbUser in myDYXTEntities.B_UserTable
                                       where tbUser.UserID == UserID
                                       select new USerbankVo
                                       {
                                           UserID = tbUser.UserID,
                                           UserName = tbUser.UserName,
                                           TrueName = tbUser.TrueName,
                                           Sex = tbUser.Sex,
                                           BornDates = tbUser.BornDate.ToString(),
                                           NativePlace = tbUser.NativePlace,
                                           IDCardNo = tbUser.IDCardNo,
                                           PhoneNumber = tbUser.PhoneNumber
                                       }).Single();
                return Json(varUser, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        /// <summary>
        ///用户编辑
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult selectfromUSerdetailed(int UserID)
        {
            try
            {
                B_UserTable varUser = (from tbUser in myDYXTEntities.B_UserTable
                                       join tbUsertype in myDYXTEntities.S_UserTypeTable on tbUser.UserTypeID equals tbUsertype.UserTypeID
                                       where tbUser.UserID == UserID
                                       select new USerbankVo
                                       {
                                           UserID = tbUser.UserID,
                                           UserName = tbUser.UserName.Trim(),
                                           UserTypeName = tbUsertype.UserTypeName.Trim(),
                                           TrueName = tbUser.TrueName.Trim(),
                                           Sex = tbUser.Sex.Trim(),
                                           BornDates = tbUser.BornDate.ToString(),
                                           IDCardNo = tbUser.IDCardNo.Trim(),
                                           PhoneNumber = tbUser.PhoneNumber.Trim(),
                                           NativePlace = tbUser.NativePlace.Trim(),
                                           PostBox = tbUser.PostBox.Trim(),
                                           EducationalBackground = tbUser.EducationalBackground.Trim(),
                                           MarriageState = tbUser.MarriageState.Trim(),
                                           Issue = tbUser.Issue.Trim(),
                                           SocialSecurity = tbUser.SocialSecurity.Trim(),
                                           housingCondition = tbUser.housingCondition.Trim(),
                                           WhetherBuyCar = tbUser.WhetherBuyCar.Trim(),
                                           PropertyAmounts = tbUser.PropertyAmounts,
                                           UsableMoney = tbUser.UsableMoney,
                                           FreezeMoney = tbUser.FreezeMoney,
                                           WaitMoney = tbUser.WaitMoney,
                                           CompensatoryMoney = tbUser.CompensatoryMoney
                                       }).Single();
                return Json(varUser, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="UserDetai"></param>
        /// <param name="BornDates"></param>
        /// <returns></returns>
        public ActionResult UpdataUserdetailed(B_UserTable UserDetai, string BornDates)
        {
            string strMsg = "fali";
            try
            {

                B_UserTable varUserLoginDetail = (from tbB_UserLoginDetail in myDYXTEntities.B_UserTable
                                                  where tbB_UserLoginDetail.UserID == UserDetai.UserID
                                                  select tbB_UserLoginDetail).Single();
                varUserLoginDetail.TrueName = UserDetai.TrueName;
                varUserLoginDetail.Sex = UserDetai.Sex;
                varUserLoginDetail.BornDate = Convert.ToDateTime(BornDates);
                varUserLoginDetail.IDCardNo = UserDetai.IDCardNo;
                varUserLoginDetail.PhoneNumber = UserDetai.PhoneNumber;
                varUserLoginDetail.NativePlace = UserDetai.NativePlace;
                varUserLoginDetail.PostBox = UserDetai.PostBox;
                varUserLoginDetail.EducationalBackground = UserDetai.EducationalBackground;
                varUserLoginDetail.MarriageState = UserDetai.MarriageState;
                varUserLoginDetail.Issue = UserDetai.Issue;
                varUserLoginDetail.SocialSecurity = UserDetai.SocialSecurity;
                varUserLoginDetail.housingCondition = UserDetai.housingCondition;
                varUserLoginDetail.WhetherBuyCar = UserDetai.WhetherBuyCar;
                myDYXTEntities.Entry(varUserLoginDetail).State = System.Data.Entity.EntityState.Modified;
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPictureImage(int MaterialAttestationID)
        {
            try
            {
                var studentImg = (from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                                  where tbMaterialAttestation.MaterialAttestationID == MaterialAttestationID
                                  select new
                                  {
                                      tbMaterialAttestation.Picture
                                  }).Single();

                byte[] imageData = studentImg.Picture;
                return File(imageData, @"image/jpg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public ActionResult SelectUSermaterial(BsgridPage bsgridPage, int UserID)
        {
            var notict = from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                         join tbuser in myDYXTEntities.B_UserTable on tbMaterialAttestation.UserID equals tbuser.UserID
                         join tbSendstatus in myDYXTEntities.S_StatusTable on tbMaterialAttestation.StatusID equals tbSendstatus.StatusID
                         join tbItemType in myDYXTEntities.S_ItemTypeTable on tbMaterialAttestation.ItemTypeID equals tbItemType.ItemTypeID
                         where tbMaterialAttestation.UserID == UserID
                         select new B_MaterialAttestationVo
                         {
                             MaterialAttestationID = tbMaterialAttestation.MaterialAttestationID,
                             UserID = tbMaterialAttestation.UserID,
                             ItemTypeID = tbMaterialAttestation.ItemTypeID,
                             StatusID = tbMaterialAttestation.StatusID,
                             UserName = tbuser.UserName,
                             ItemTypeName = tbItemType.ItemTypeName,
                             Integral = tbMaterialAttestation.Integral,
                             StatusName = tbSendstatus.StatusName,
                             ReleaseTimeStr = tbMaterialAttestation.AddTime.ToString(),
                             Remarks = tbMaterialAttestation.Remarks,
                             ReviewRemarks = tbMaterialAttestation.ReviewRemarks
                         };


            int totalRow = notict.Count();
            List<B_MaterialAttestationVo> notices = notict.OrderBy(p => p.MaterialAttestationID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_MaterialAttestationVo> bsgrid = new Bsgrid<B_MaterialAttestationVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 资金详细
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult selectUserincomeexpenditure(BsgridPage bsgridPage, int UserID)
        {
            var webbs = (from tbExpensesTable in myDYXTEntities.B_UserExpensesTable
                         join tbOperateType in myDYXTEntities.S_OperateTypeTable on tbExpensesTable.OperateTypeID equals tbOperateType.OperateTypeID
                         join tbuser in myDYXTEntities.B_UserTable on tbExpensesTable.UserID equals tbuser.UserID
                         where tbExpensesTable.UserID == UserID
                         select new UserexpensesreceiptsVo
                         {
                             UserExpensesID = tbExpensesTable.UserExpensesID,
                             UserName = tbuser.UserName,
                             OperateTypeName = tbOperateType.OperateTypeName,
                             OperateTypeID = tbExpensesTable.OperateTypeID,
                             OperateMoney = tbExpensesTable.OperateMoney,
                             PropertyAmounts = tbuser.PropertyAmounts,
                             Earning = tbExpensesTable.Earning,
                             Expenses = tbExpensesTable.Expenses,
                             Remark = tbExpensesTable.Remark,
                             ReleaseTimeStr = tbExpensesTable.OperateTime.ToString()
                         }).OrderByDescending(p => p.UserExpensesID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            int totalRow = myDYXTEntities.B_UserExpensesTable.Count();
            Bsgrid<UserexpensesreceiptsVo> bsgrid = new Bsgrid<UserexpensesreceiptsVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = webbs;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region VIP管理
        public ActionResult VIPview()
        {
            return View();
        }
        /// <summary>
        /// 查询VIP所有用户
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectVIPUSer(BsgridPage bsgridPage)
        {
            var VarVip = from tbVIPUSer in myDYXTEntities.B_VIPUserTable
                         join tbUser in myDYXTEntities.B_UserTable on tbVIPUSer.UserID equals tbUser.UserID
                         join tbusertype in myDYXTEntities.S_UserTypeTable on tbVIPUSer.UserTypeID equals tbusertype.UserTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbVIPUSer.StatusID equals tbStatus.StatusID
                         select new B_VIPUserVo
                         {
                             VIPUserID = tbVIPUSer.VIPUserID,
                             UserID = tbVIPUSer.UserID,
                             UserName = tbUser.UserName,
                             UserTypeName = tbusertype.UserTypeName,
                             VipDeadline = tbVIPUSer.VipDeadline,
                             ReleaseTimeStr = tbVIPUSer.StartTime.ToString(),
                             ENTimeStr = tbVIPUSer.EndTime.ToString(),
                             StatusName = tbStatus.StatusName,
                             WhetherPayment = tbVIPUSer.WhetherPayment
                         };

            int totalRow = VarVip.Count();
            List<B_VIPUserVo> notices = VarVip.OrderByDescending(p => p.VIPUserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_VIPUserVo> bsgrid = new Bsgrid<B_VIPUserVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询待审核
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectVIPUSerbyDSH(BsgridPage bsgridPage)
        {
            var VarVip = from tbVIPUSer in myDYXTEntities.B_VIPUserTable
                         join tbUser in myDYXTEntities.B_UserTable on tbVIPUSer.UserID equals tbUser.UserID
                         join tbusertype in myDYXTEntities.S_UserTypeTable on tbVIPUSer.UserTypeID equals tbusertype.UserTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbVIPUSer.StatusID equals tbStatus.StatusID
                         where tbVIPUSer.StatusID == 2
                         select new B_VIPUserVo
                         {
                             VIPUserID = tbVIPUSer.VIPUserID,
                             UserID = tbVIPUSer.UserID,
                             UserName = tbUser.UserName,
                             UserTypeName = tbusertype.UserTypeName,
                             VipDeadline = tbVIPUSer.VipDeadline,
                             ReleaseTimeStr = tbVIPUSer.StartTime.ToString(),
                             ENTimeStr = tbVIPUSer.EndTime.ToString(),
                             StatusName = tbStatus.StatusName,
                             WhetherPayment = tbVIPUSer.WhetherPayment
                         };

            int totalRow = VarVip.Count();
            List<B_VIPUserVo> notices = VarVip.OrderByDescending(p => p.VIPUserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_VIPUserVo> bsgrid = new Bsgrid<B_VIPUserVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询已审核
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectVIPUSerbyYS(BsgridPage bsgridPage)
        {
            var VarVip = from tbVIPUSer in myDYXTEntities.B_VIPUserTable
                         join tbUser in myDYXTEntities.B_UserTable on tbVIPUSer.UserID equals tbUser.UserID
                         join tbusertype in myDYXTEntities.S_UserTypeTable on tbVIPUSer.UserTypeID equals tbusertype.UserTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbVIPUSer.StatusID equals tbStatus.StatusID
                         where tbVIPUSer.StatusID == 18
                         select new B_VIPUserVo
                         {
                             VIPUserID = tbVIPUSer.VIPUserID,
                             UserID = tbVIPUSer.UserID,
                             UserName = tbUser.UserName,
                             UserTypeName = tbusertype.UserTypeName,
                             VipDeadline = tbVIPUSer.VipDeadline,
                             ReleaseTimeStr = tbVIPUSer.StartTime.ToString(),
                             ENTimeStr = tbVIPUSer.EndTime.ToString(),
                             StatusName = tbStatus.StatusName,
                             WhetherPayment = tbVIPUSer.WhetherPayment
                         };

            int totalRow = VarVip.Count();
            List<B_VIPUserVo> notices = VarVip.OrderByDescending(p => p.VIPUserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_VIPUserVo> bsgrid = new Bsgrid<B_VIPUserVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询审核失败
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult SelectVIPUSerbySHSB(BsgridPage bsgridPage)
        {
            var VarVip = from tbVIPUSer in myDYXTEntities.B_VIPUserTable
                         join tbUser in myDYXTEntities.B_UserTable on tbVIPUSer.UserID equals tbUser.UserID
                         join tbusertype in myDYXTEntities.S_UserTypeTable on tbVIPUSer.UserTypeID equals tbusertype.UserTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbVIPUSer.StatusID equals tbStatus.StatusID
                         where tbVIPUSer.StatusID == 25
                         select new B_VIPUserVo
                         {
                             VIPUserID = tbVIPUSer.VIPUserID,
                             UserID = tbVIPUSer.UserID,
                             UserName = tbUser.UserName,
                             UserTypeName = tbusertype.UserTypeName,
                             VipDeadline = tbVIPUSer.VipDeadline,
                             ReleaseTimeStr = tbVIPUSer.StartTime.ToString(),
                             ENTimeStr = tbVIPUSer.EndTime.ToString(),
                             StatusName = tbStatus.StatusName,
                             WhetherPayment = tbVIPUSer.WhetherPayment
                         };

            int totalRow = VarVip.Count();
            List<B_VIPUserVo> notices = VarVip.OrderByDescending(p => p.VIPUserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_VIPUserVo> bsgrid = new Bsgrid<B_VIPUserVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 审核绑定
        /// </summary>
        /// <param name="AuthenticationID"></param>
        /// <returns></returns>
        public ActionResult selectshenhe(int VIPUserID)
        {

            try
            {
                var Recharge = (from tbVIPUser in myDYXTEntities.B_VIPUserTable
                                where tbVIPUser.VIPUserID == VIPUserID
                                select new B_VIPUserVo
                                {
                                    VIPUserID = tbVIPUser.VIPUserID,
                                    UserID = tbVIPUser.UserID
                                }).Single();

                return Json(Recharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 审核保存
        /// </summary>
        /// <param name="User"></param>
        /// <param name="optionsRadios"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        public ActionResult updatashenheVIP(B_VIPUserTable VIPUser, string optionsRadios, string validCode)
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
                    B_VIPUserTable varVipUser = (from tbVIPUser in myDYXTEntities.B_VIPUserTable
                                                 where tbVIPUser.VIPUserID == VIPUser.VIPUserID
                                                 select tbVIPUser).Single();
                    varVipUser.StatusID = 2;
                    varVipUser.ExamineRemarks = VIPUser.ExamineRemarks;
                    varVipUser.ExamineTime = DateTime.Now;
                    myDYXTEntities.Entry(varVipUser).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }
                }
                else if (optionsRadios == "option2")
                {
                    B_VIPUserTable varVipUser = (from tbVIPUser in myDYXTEntities.B_VIPUserTable
                                                 where tbVIPUser.VIPUserID == VIPUser.VIPUserID
                                                 select tbVIPUser).Single();
                    int intuserID = Convert.ToInt32(varVipUser.UserID);
                    B_UserTable varsuer = (from tbUser in myDYXTEntities.B_UserTable
                                           where tbUser.UserID == intuserID
                                           select tbUser).Single();
                    varsuer.UserTypeID = 3;
                    myDYXTEntities.Entry(varsuer).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();

                    varVipUser.StatusID = 18;
                    varVipUser.ExamineRemarks = VIPUser.ExamineRemarks;
                    varVipUser.ExamineTime = DateTime.Now;
                    DateTime date = DateTime.Now;
                    varVipUser.StartTime = date;
                    if (varVipUser.VipDeadline.Trim() == "1")
                    {

                        var usermoney = (from tbuser in myDYXTEntities.B_UserTable
                                         where tbuser.UserID == VIPUser.UserID
                                         select tbuser).Single();
                        if (usermoney.PropertyAmounts >= 10 && usermoney.UsableMoney >= 10 && usermoney.PropertyAmounts != null && usermoney.UsableMoney != null)
                        {
                            usermoney.PropertyAmounts = usermoney.PropertyAmounts - 10;
                            usermoney.UsableMoney = usermoney.UsableMoney - 10;
                            B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                            WebsiteExpenses.AccountID = Convert.ToInt32(Session["AccountID"]);
                            WebsiteExpenses.OperateTypeID = 9;
                            WebsiteExpenses.OperateMoney = Convert.ToDecimal(10);
                            WebsiteExpenses.Earning = Convert.ToDecimal(10);
                            WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                            WebsiteExpenses.Remark = "用户申请VIP一个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]10元会员费";
                            WebsiteExpenses.OperateTime = DateTime.Now;
                            myDYXTEntities.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                            myDYXTEntities.SaveChanges();

                            B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                            UserExpense.UserID = usermoney.UserID;
                            UserExpense.OperateTypeID = 9;
                            UserExpense.OperateMoney = Convert.ToDecimal(10);
                            UserExpense.Balance = usermoney.PropertyAmounts;
                            UserExpense.Earning = Convert.ToDecimal(0);
                            UserExpense.Expenses = Convert.ToDecimal(10);
                            UserExpense.Remark = "用户申请VIP一个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]10元会员费";
                            UserExpense.OperateTime = DateTime.Now;
                            myDYXTEntities.B_UserExpensesTable.Add(UserExpense);
                            myDYXTEntities.SaveChanges();

                            B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                            Capitalrecord.UserID = usermoney.UserID;
                            Capitalrecord.OperateTypeID = 9;
                            Capitalrecord.OpFare = Convert.ToDecimal(10);
                            Capitalrecord.Income = Convert.ToDecimal(0);
                            Capitalrecord.Expend = Convert.ToDecimal(10);
                            Capitalrecord.PropertyAmounts = usermoney.PropertyAmounts;
                            Capitalrecord.Remarks = "用户申请VIP一个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]10元会员费";
                            Capitalrecord.operatetime = DateTime.Now;
                            myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                            myDYXTEntities.SaveChanges();
                            varVipUser.EndTime = date.AddMonths(1);
                            myDYXTEntities.Entry(varVipUser).State = System.Data.Entity.EntityState.Modified;
                            if (myDYXTEntities.SaveChanges() > 0)
                            {
                                strMsg = "success";
                            }
                        }
                        else
                        {
                            strMsg = "failmoney";//余额不足
                        }
                    }
                    if (varVipUser.VipDeadline.Trim() == "3")
                    {
                        var usermoney = (from tbuser in myDYXTEntities.B_UserTable
                                         where tbuser.UserID == VIPUser.UserID
                                         select tbuser).Single();
                        if (usermoney.PropertyAmounts >= 30 && usermoney.UsableMoney >= 30 && usermoney.PropertyAmounts != null && usermoney.UsableMoney != null)
                        {
                            usermoney.PropertyAmounts = usermoney.PropertyAmounts - 30;
                            usermoney.UsableMoney = usermoney.UsableMoney - 30;
                            B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                            WebsiteExpenses.AccountID = Convert.ToInt32(Session["AccountID"]);
                            WebsiteExpenses.OperateTypeID = 9;
                            WebsiteExpenses.OperateMoney = Convert.ToDecimal(30);
                            WebsiteExpenses.Earning = Convert.ToDecimal(30);
                            WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                            WebsiteExpenses.Remark = "用户申请VIP三个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]30元会员费";
                            WebsiteExpenses.OperateTime = DateTime.Now;
                            myDYXTEntities.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                            myDYXTEntities.SaveChanges();

                            B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                            UserExpense.UserID = usermoney.UserID;
                            UserExpense.OperateTypeID = 9;
                            UserExpense.OperateMoney = Convert.ToDecimal(30);
                            UserExpense.Balance = usermoney.PropertyAmounts;
                            UserExpense.Earning = Convert.ToDecimal(0);
                            UserExpense.Expenses = Convert.ToDecimal(30);
                            UserExpense.Remark = "用户申请VIP三个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]30元会员费";
                            UserExpense.OperateTime = DateTime.Now;
                            myDYXTEntities.B_UserExpensesTable.Add(UserExpense);
                            myDYXTEntities.SaveChanges();

                            B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                            Capitalrecord.UserID = usermoney.UserID;
                            Capitalrecord.OperateTypeID = 9;
                            Capitalrecord.OpFare = Convert.ToDecimal(30);
                            Capitalrecord.Income = Convert.ToDecimal(0);
                            Capitalrecord.Expend = Convert.ToDecimal(30);
                            Capitalrecord.PropertyAmounts = usermoney.PropertyAmounts;
                            Capitalrecord.Remarks = "用户申请VIP三个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]30元会员费";
                            Capitalrecord.operatetime = DateTime.Now;
                            myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                            myDYXTEntities.SaveChanges();
                            varVipUser.EndTime = date.AddMonths(3);
                            myDYXTEntities.Entry(varVipUser).State = System.Data.Entity.EntityState.Modified;
                            if (myDYXTEntities.SaveChanges() > 0)
                            {
                                strMsg = "success";
                            }
                        }
                        else
                        {
                            strMsg = "failmoney";//余额不足
                        }
                    }
                    if (varVipUser.VipDeadline.Trim() == "6")
                    {
                        var usermoney = (from tbuser in myDYXTEntities.B_UserTable
                                         where tbuser.UserID == VIPUser.UserID
                                         select tbuser).Single();
                        if (usermoney.PropertyAmounts >= 60 && usermoney.UsableMoney >= 60 && usermoney.PropertyAmounts != null && usermoney.UsableMoney != null)
                        {
                            usermoney.PropertyAmounts = usermoney.PropertyAmounts - 60;
                            usermoney.UsableMoney = usermoney.UsableMoney - 60;
                            B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                            WebsiteExpenses.AccountID = Convert.ToInt32(Session["AccountID"]);
                            WebsiteExpenses.OperateTypeID = 9;
                            WebsiteExpenses.OperateMoney = Convert.ToDecimal(60);
                            WebsiteExpenses.Earning = Convert.ToDecimal(60);
                            WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                            WebsiteExpenses.Remark = "用户申请VIP六个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]60元会员费";
                            WebsiteExpenses.OperateTime = DateTime.Now;
                            myDYXTEntities.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                            myDYXTEntities.SaveChanges();

                            B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                            UserExpense.UserID = usermoney.UserID;
                            UserExpense.OperateTypeID = 9;
                            UserExpense.OperateMoney = Convert.ToDecimal(60);
                            UserExpense.Balance = usermoney.PropertyAmounts;
                            UserExpense.Earning = Convert.ToDecimal(0);
                            UserExpense.Expenses = Convert.ToDecimal(60);
                            UserExpense.Remark = "用户申请VIP六个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]60元会员费";
                            UserExpense.OperateTime = DateTime.Now;
                            myDYXTEntities.B_UserExpensesTable.Add(UserExpense);
                            myDYXTEntities.SaveChanges();

                            B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                            Capitalrecord.UserID = usermoney.UserID;
                            Capitalrecord.OperateTypeID = 9;
                            Capitalrecord.OpFare = Convert.ToDecimal(60);
                            Capitalrecord.Income = Convert.ToDecimal(0);
                            Capitalrecord.Expend = Convert.ToDecimal(60);
                            Capitalrecord.PropertyAmounts = usermoney.PropertyAmounts;
                            Capitalrecord.Remarks = "用户申请VIP六个月," + "扣除" + "[" + usermoney.UserName.Trim() + "]60元会员费";
                            Capitalrecord.operatetime = DateTime.Now;
                            myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                            myDYXTEntities.SaveChanges();
                            varVipUser.EndTime = date.AddMonths(6);
                            myDYXTEntities.Entry(varVipUser).State = System.Data.Entity.EntityState.Modified;
                            if (myDYXTEntities.SaveChanges() > 0)
                            {
                                strMsg = "success";
                            }
                        }
                        else
                        {
                            strMsg = "failmoney";//余额不足
                        }
                    }
                    if (varVipUser.VipDeadline.Trim() == "12")
                    {
                        var usermoney = (from tbuser in myDYXTEntities.B_UserTable
                                         where tbuser.UserID == VIPUser.UserID
                                         select tbuser).Single();
                        if (usermoney.PropertyAmounts >= 120 && usermoney.UsableMoney >= 120 && usermoney.PropertyAmounts != null && usermoney.UsableMoney != null)
                        {
                            usermoney.PropertyAmounts = usermoney.PropertyAmounts - 120;
                            usermoney.UsableMoney = usermoney.UsableMoney - 120;
                            B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                            WebsiteExpenses.AccountID = Convert.ToInt32(Session["AccountID"]);
                            WebsiteExpenses.OperateTypeID = 9;
                            WebsiteExpenses.OperateMoney = Convert.ToDecimal(120);
                            WebsiteExpenses.Earning = Convert.ToDecimal(120);
                            WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                            WebsiteExpenses.Remark = "用户申请VIP一年," + "扣除" + "[" + usermoney.UserName.Trim() + "]120元会员费";
                            WebsiteExpenses.OperateTime = DateTime.Now;
                            myDYXTEntities.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                            myDYXTEntities.SaveChanges();

                            B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                            UserExpense.UserID = usermoney.UserID;
                            UserExpense.OperateTypeID = 9;
                            UserExpense.OperateMoney = Convert.ToDecimal(120);
                            UserExpense.Balance = usermoney.PropertyAmounts;
                            UserExpense.Earning = Convert.ToDecimal(0);
                            UserExpense.Expenses = Convert.ToDecimal(120);
                            UserExpense.Remark = "用户申请VIP一年," + "扣除" + "[" + usermoney.UserName.Trim() + "]120元会员费";
                            UserExpense.OperateTime = DateTime.Now;
                            myDYXTEntities.B_UserExpensesTable.Add(UserExpense);
                            myDYXTEntities.SaveChanges();

                            B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                            Capitalrecord.UserID = usermoney.UserID;
                            Capitalrecord.OperateTypeID = 9;
                            Capitalrecord.OpFare = Convert.ToDecimal(120);
                            Capitalrecord.Income = Convert.ToDecimal(0);
                            Capitalrecord.Expend = Convert.ToDecimal(120);
                            Capitalrecord.PropertyAmounts = usermoney.PropertyAmounts;
                            Capitalrecord.Remarks = "用户申请VIP一年," + "扣除" + "[" + usermoney.UserName.Trim() + "]120元会员费";
                            Capitalrecord.operatetime = DateTime.Now;
                            myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                            myDYXTEntities.SaveChanges();
                            varVipUser.EndTime = date.AddMonths(12);
                            myDYXTEntities.Entry(varVipUser).State = System.Data.Entity.EntityState.Modified;
                            if (myDYXTEntities.SaveChanges() > 0)
                            {
                                strMsg = "success";
                            }
                        }
                        else
                        {
                            strMsg = "failmoney";//余额不足
                        }

                    }


                }
                else
                {
                    B_VIPUserTable varVipUser = (from tbVIPUser in myDYXTEntities.B_VIPUserTable
                                                 where tbVIPUser.VIPUserID == VIPUser.VIPUserID
                                                 select tbVIPUser).Single();
                    varVipUser.StatusID = 25;
                    varVipUser.ExamineRemarks = VIPUser.ExamineRemarks;
                    varVipUser.ExamineTime = DateTime.Now;
                    myDYXTEntities.Entry(varVipUser).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }
                }
            }
            else
            {
                strMsg = "ValidCodeErro";//验证码错误
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="VIPUserID"></param>
        /// <returns></returns>
        public ActionResult selectVipuserxinxi(int VIPUserID)
        {

            try
            {
                var Recharge = (from tbVIPUSer in myDYXTEntities.B_VIPUserTable
                                join tbUser in myDYXTEntities.B_UserTable on tbVIPUSer.UserID equals tbUser.UserID
                                join tbusertype in myDYXTEntities.S_UserTypeTable on tbVIPUSer.UserTypeID equals tbusertype.UserTypeID
                                join tbStatus in myDYXTEntities.S_StatusTable on tbVIPUSer.StatusID equals tbStatus.StatusID
                                where tbVIPUSer.VIPUserID == VIPUserID
                                select new B_VIPUserVo
                                {
                                    VIPUserID = tbVIPUSer.VIPUserID,
                                    UserID = tbVIPUSer.UserID,
                                    UserName = tbUser.UserName,
                                    UserTypeName = tbusertype.UserTypeName,
                                    VipDeadline = tbVIPUSer.VipDeadline,
                                    ReleaseTimeStr = tbVIPUSer.StartTime.ToString(),
                                    ENTimeStr = tbVIPUSer.EndTime.ToString(),
                                    StatusName = tbStatus.StatusName,
                                    WhetherPayment = tbVIPUSer.WhetherPayment,
                                    ETimeStr = tbVIPUSer.ExamineTime.ToString(),
                                    ExamineRemarks = tbVIPUSer.ExamineRemarks,
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
    }
}