using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DYXTHT_MVC.Models;
using DYXTHT_MVC.Vo;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;

namespace DYXTHT_MVC.Areas.AuthenticationManagement.Controllers
{
    public class AuthenticationController : Controller
    {
        Models.DYXTEntities myDYXTEntities = new DYXTEntities();
        // GET: AuthenticationManagement/Authentication
        #region 实名认证
        /// <summary>
        /// 实名认证页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RealnameAuthentication()
        {
            return View();
        }
      

        //public ActionResult IDCard(string cardNo)
        //{
        //    string idcarno;
        //    string idcaedvali;
        //    string idvalu;        
        //    idcarno = Common.IDCard.IDValidator(cardNo);
        //    idcaedvali = Common.IDCard.IDValidator2(cardNo);
        //    idvalu = Common.IDCard.IDValidator3(cardNo);
        //    return Json(idcarno, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// 实名认证查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectUser(BsgridPage bsgridPage, string SUserName, string STrueName, string SIDCardNo, string SeUserName, int SeUserID)
        {
            var notict = from tbUser in myDYXTEntities.B_UserTable
                         join tbStatus in myDYXTEntities.S_StatusTable on tbUser.StatusID equals tbStatus.StatusID
                         select new USerbankVo
                         {
                             UserID = tbUser.UserID,
                             UserName = tbUser.UserName,
                             TrueName = tbUser.TrueName,
                             IDCardNo = tbUser.IDCardNo,
                             Sex = tbUser.Sex,
                             StatusName = tbStatus.StatusName,
                             ReleaseTimeStr = tbUser.Time.ToString(),
                         };

            if (!string.IsNullOrEmpty(SUserName))
            {
                notict = notict.Where(n => n.UserName.Contains(SUserName));
            }
            if (!string.IsNullOrEmpty(STrueName))
            {
                notict = notict.Where(n => n.TrueName.Contains(STrueName));
            }
            if (!string.IsNullOrEmpty(SIDCardNo))
            {
                notict = notict.Where(n => n.IDCardNo.Contains(SIDCardNo));
            }
            if (!string.IsNullOrEmpty(SeUserName))
            {
                notict = notict.Where(n => n.UserName.Contains(SeUserName));
            }
            //类型ID不为空
            if (SeUserID > 0)
            {
                notict = notict.Where(p => p.UserID == SeUserID);
            }
            int totalRow = notict.Count();
            List<USerbankVo> notices = notict.OrderByDescending(p => p.UserID).
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
        /// 审核绑定
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult Shenhebangding(int UserID)
        {

            try
            {
                var Recharge = (from tbUser in myDYXTEntities.B_UserTable
                                where tbUser.UserID == UserID
                                select new USerbankVo
                                {
                                    UserID = tbUser.UserID,
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
        /// 审核
        /// </summary>
        /// <param name="User"></param>
        /// <param name="optionsRadios"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        public ActionResult Updatabaocun(B_UserTable User, string optionsRadios, string validCode)
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
                    B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                           where tbuser.UserID == User.UserID
                                           select tbuser).Single();

                    varUser.StatusID = 24;
                    varUser.Integral = varUser.Integral + 10;
                    varUser.ExamineRemarks = User.ExamineRemarks;
                    myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }
                }
                else
                {
                    B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                           where tbuser.UserID == User.UserID
                                           select tbuser).Single();

                    varUser.StatusID = 25;
                    varUser.ExamineRemarks = User.ExamineRemarks;
                    myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
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
        /// 状态绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctStatus()
        {

            List<SelectVo> Province = (from tbStatus in myDYXTEntities.S_StatusTable
                                       where tbStatus.StatusID==2 || tbStatus.StatusID == 24|| tbStatus.StatusID == 25
                                       select new SelectVo
                                       {
                                           id = tbStatus.StatusID,
                                           text = tbStatus.StatusName
                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 实名认证修改绑定
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult updatabangding(int UserID)
        {
            try
            {
                var Recharge = (from tbUser in myDYXTEntities.B_UserTable
                                join tbStatus in myDYXTEntities.S_StatusTable on tbUser.StatusID equals tbStatus.StatusID
                                where tbUser.UserID == UserID
                                select new USerbankVo
                                {
                                    UserID = tbUser.UserID,
                                    UserName = tbUser.UserName.Trim(),
                                    TrueName = tbUser.TrueName.Trim(),
                                    IDCardNo = tbUser.IDCardNo.Trim(),
                                    Sex = tbUser.Sex.Trim(),
                                    StatusID = tbUser.StatusID,
                                    StatusName = tbStatus.StatusName,
                                    ReleaseTimeStr = tbUser.Time.ToString(),
                                    ExamineRemarks = tbUser.ExamineRemarks.Trim()
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
        /// 用户认证修改保存
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public ActionResult Updatayonghurenzheng(B_UserTable User)
        {
            string strMsg = "fali";
            try
            {
                B_UserTable strvarUser = (from tbuser in myDYXTEntities.B_UserTable
                                       where tbuser.UserID == User.UserID
                                       select tbuser).Single();
                if (strvarUser.StatusID == User.StatusID)
                {
                    B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                           where tbuser.UserID == User.UserID
                                           select tbuser).Single();
                    varUser.UserName = User.UserName;
                    varUser.TrueName = User.TrueName;
                    varUser.Sex = User.Sex;
                    varUser.StatusID = User.StatusID;                
                    varUser.ExamineRemarks = User.ExamineRemarks;
                    myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
                else
                {
                    if (User.StatusID == 2 || User.StatusID == 25)
                    {
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == User.UserID
                                               select tbuser).Single();
                        varUser.UserName = User.UserName;
                        varUser.TrueName = User.TrueName;
                        varUser.Sex = User.Sex;
                        varUser.StatusID = User.StatusID;
                        varUser.Integral = varUser.Integral - 10;
                        varUser.ExamineRemarks = User.ExamineRemarks;
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }
                    else
                    {
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == User.UserID
                                               select tbuser).Single();
                        varUser.UserName = User.UserName;
                        varUser.TrueName = User.TrueName;
                        varUser.Sex = User.Sex;
                        varUser.StatusID = User.StatusID;
                        varUser.Integral = varUser.Integral + 10;
                        varUser.ExamineRemarks = User.ExamineRemarks;
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }
                }
               
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查看绑定
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult selectrenzhengbangding(int UserID)
        {
            try
            {
                var Recharge = (from tbUser in myDYXTEntities.B_UserTable
                                join tbStatus in myDYXTEntities.S_StatusTable on tbUser.StatusID equals tbStatus.StatusID
                                where tbUser.UserID == UserID
                                select new USerbankVo
                                {
                                    UserID = tbUser.UserID,
                                    UserName = tbUser.UserName,
                                    TrueName = tbUser.TrueName,
                                    IDCardNo = tbUser.IDCardNo,
                                    Sex = tbUser.Sex,
                                    StatusID = tbUser.StatusID,
                                    StatusName = tbStatus.StatusName,
                                    ReleaseTimeStr = tbUser.Time.ToString(),
                                    ExamineRemarks = tbUser.ExamineRemarks.Trim()
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

        #region 学历认证
        public ActionResult Educationabackground()
        {
            return View();
        }
        /// <summary>
        /// 学历认证查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selsectEducationaback(BsgridPage bsgridPage, string SUserName, string SeUserName, int SeUserID, int StatusID)
        {

            var notict = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                         join tbuser in myDYXTEntities.B_UserTable on tbAuthentication.UserID equals tbuser.UserID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbAuthentication.StatusID equals tbStatus.StatusID
                         select new B_AuthenticationVo
                         {
                             AuthenticationID = tbAuthentication.AuthenticationID,
                             UserName = tbuser.UserName,
                             TrueName = tbuser.TrueName,
                             Institutions = tbAuthentication.Institutions,
                             Specialty = tbAuthentication.Specialty,
                             EducationalBackground = tbAuthentication.EducationalBackground,
                             Intime = tbAuthentication.EnrolTime.ToString(),
                             Entime = tbAuthentication.GraduationTime.ToString(),
                             StatusID=tbAuthentication.StatusID,
                             StatusName =  tbStatus.StatusName
                         }).OrderByDescending(p => p.AuthenticationID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
          
            if (!string.IsNullOrEmpty(SUserName))
            {
                notict = notict.Where(n => n.UserName.Contains(SUserName)).ToList();
            }
            if (!string.IsNullOrEmpty(SeUserName))
            {
                notict = notict.Where(n => n.UserName.Contains(SeUserName)).ToList();
            }
            if (SeUserID > 0)
            {
                notict = notict.Where(p => p.AuthenticationID == SeUserID).ToList();
            }
            //类型ID不为空
            if (StatusID > 0)
            {
                notict = notict.Where(p => p.StatusID == StatusID).ToList();
            }
            int totalRow =myDYXTEntities.B_AuthenticationTable.Count();           
            Bsgrid<B_AuthenticationVo> bsgrid = new Bsgrid<B_AuthenticationVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notict;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 学历图片
        /// </summary>
        /// <param name="AuthenticationID"></param>
        /// <returns></returns>
        public ActionResult GetImage(int AuthenticationID)
        {
            try
            {
                var studentImg = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                  where tbAuthentication.AuthenticationID == AuthenticationID
                                  select new
                                  {
                                      tbAuthentication.Diploma
                                  }).Single();

                byte[] imageData = studentImg.Diploma;
                return File(imageData, @"image/jpg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// 审核绑定
        /// </summary>
        /// <param name="AuthenticationID"></param>
        /// <returns></returns>
        public ActionResult Shenhebangdingxueli(int AuthenticationID)
        {

            try
            {
                var Recharge = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                where tbAuthentication.AuthenticationID == AuthenticationID
                                select new B_AuthenticationVo
                                {
                                    AuthenticationID = tbAuthentication.AuthenticationID,
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
        public ActionResult Updataxuelibaocun(B_AuthenticationTable Authentication, string optionsRadios, string validCode,string ExamineRemarks)
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
                    B_AuthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                                     where tbAuthentication.AuthenticationID == Authentication.AuthenticationID
                                                     select tbAuthentication).Single();
                    varAuthentication.StatusID = 2;
                    varAuthentication.ReviewRemarks = ExamineRemarks;
                    varAuthentication.Checktime = DateTime.Now;
                    myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {                      
                        strMsg = "success";
                    }
                }
                else if (optionsRadios == "option2")
                {
                    B_AuthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                                               where tbAuthentication.AuthenticationID == Authentication.AuthenticationID
                                                               select tbAuthentication).Single();
                    varAuthentication.StatusID = 24;
                    varAuthentication.ReviewRemarks = ExamineRemarks;
                    varAuthentication.Checktime = DateTime.Now;
                    myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == varAuthentication.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral + 10;
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }

                }
                else{
                    B_AuthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                                               where tbAuthentication.AuthenticationID == Authentication.AuthenticationID
                                                               select tbAuthentication).Single();
                    varAuthentication.StatusID = 25;
                    varAuthentication.ReviewRemarks = ExamineRemarks;
                    varAuthentication.Checktime = DateTime.Now;
                    myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult SelctStatustype()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> Province = (from tbStatus in myDYXTEntities.S_StatusTable
                                       where  tbStatus.StatusID==24|| tbStatus.StatusID==25|| tbStatus.StatusID==2
                                       select new SelectVo
                                       {
                                           id = tbStatus.StatusID,
                                           text = tbStatus.StatusName
                                       }).ToList();
            listnoticeType.AddRange(Province);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult updataxuelibangding(int AuthenticationID)
        {
            try
            {
                var Recharge = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                join tbuser in myDYXTEntities.B_UserTable on tbAuthentication.UserID equals tbuser.UserID
                                join tbStatus in myDYXTEntities.S_StatusTable on tbAuthentication.StatusID equals tbStatus.StatusID
                                where tbAuthentication.AuthenticationID == AuthenticationID
                                select new B_AuthenticationVo
                                {
                                    AuthenticationID = tbAuthentication.AuthenticationID,
                                    UserID= tbAuthentication.UserID,
                                    UserName = tbuser.UserName.Trim(),
                                    TrueName = tbuser.TrueName.Trim(),
                                    Institutions = tbAuthentication.Institutions,
                                    Specialty = tbAuthentication.Specialty,
                                    EducationalBackground = tbAuthentication.EducationalBackground.Trim(),
                                    Intime = tbAuthentication.EnrolTime.ToString(),
                                    Entime = tbAuthentication.GraduationTime.ToString(),
                                    StatusID = tbAuthentication.StatusID,
                                    StatusName = tbStatus.StatusName.Trim(),
                                    ReviewRemarks= tbAuthentication.ReviewRemarks.Trim()
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
        /// 根据id 查询毕业照片
        /// </summary>
        /// <param name="PayTypeID"></param>
        /// <returns></returns>
        public ActionResult GetStudentImage(int AuthenticationID)
        {
            try
            {
                var studentImg = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                  where tbAuthentication.AuthenticationID == AuthenticationID
                                  select new
                                  {
                                      tbAuthentication.Diploma
                                  }).Single();
                byte[] imageData = studentImg.Diploma;
                return File(imageData, @"image/jpg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="PayType"></param>
        /// <param name="fileStudentImage"></param>
        /// <returns></returns>
        public ActionResult UpdateStudent(B_AuthenticationTable Authentication, HttpPostedFileBase fileStudentImage,string Intime,string Entime,string UserName,string TrueName)
        {
            string strMsg = "fail";
            try
            {
                B_AuthenticationTable strvarUser = (from tbuser in myDYXTEntities.B_AuthenticationTable
                                          where tbuser.UserID == Authentication.UserID
                                          select tbuser).Single();
                if (strvarUser.StatusID == Authentication.StatusID)
                {

                    B_AuthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                                               where tbAuthentication.AuthenticationID == Authentication.AuthenticationID
                                                               select tbAuthentication).Single();
                    //判断是否上传图片
                    varAuthentication.Institutions = Authentication.Institutions;
                    varAuthentication.Specialty = Authentication.Specialty;
                    varAuthentication.EducationalBackground = Authentication.EducationalBackground;
                    varAuthentication.EnrolTime = Convert.ToDateTime(Intime);
                    varAuthentication.GraduationTime = Convert.ToDateTime(Entime);
                    varAuthentication.StatusID = Authentication.StatusID;
                    varAuthentication.ReviewRemarks = Authentication.ReviewRemarks;
                    varAuthentication.Checktime = DateTime.Now;
                    if (fileStudentImage != null)
                    {
                        byte[] imgFile = new byte[fileStudentImage.ContentLength];
                        fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                        varAuthentication.Diploma = imgFile;//更新图片                              
                    }
                    myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
                    myDYXTEntities.SaveChanges();

                    int oldPaytypeRows = (from tbUser in myDYXTEntities.B_UserTable
                                          where tbUser.UserName == UserName && tbUser.UserID != Authentication.UserID
                                          select tbUser).Count();
                    if (oldPaytypeRows == 0)
                    {
                        B_UserTable varUSer = (from tbUser in myDYXTEntities.B_UserTable
                                               where tbUser.UserID == Authentication.UserID
                                               select tbUser).Single();
                        varUSer.UserName = UserName;
                        varUSer.TrueName = TrueName;
                        myDYXTEntities.Entry(varUSer).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";

                    }
                    else
                    {
                        strMsg = "exsit";
                    }
                }
                else
                {
                    if (Authentication.StatusID == 2 || Authentication.StatusID == 25)
                    {
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == Authentication.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral - 10;                       
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        B_AuthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                                                   where tbAuthentication.AuthenticationID == Authentication.AuthenticationID
                                                                   select tbAuthentication).Single();
                        //判断是否上传图片
                        varAuthentication.Institutions = Authentication.Institutions;
                        varAuthentication.Specialty = Authentication.Specialty;
                        varAuthentication.EducationalBackground = Authentication.EducationalBackground;
                        varAuthentication.EnrolTime = Convert.ToDateTime(Intime);
                        varAuthentication.GraduationTime = Convert.ToDateTime(Entime);
                        varAuthentication.StatusID = Authentication.StatusID;
                        varAuthentication.ReviewRemarks = Authentication.ReviewRemarks;
                        varAuthentication.Checktime = DateTime.Now;
                        if (fileStudentImage != null)
                        {
                            byte[] imgFile = new byte[fileStudentImage.ContentLength];
                            fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                            varAuthentication.Diploma = imgFile;//更新图片                              
                        }
                        myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();

                        int oldPaytypeRows = (from tbUser in myDYXTEntities.B_UserTable
                                              where tbUser.UserName == UserName && tbUser.UserID != Authentication.UserID
                                              select tbUser).Count();
                        if (oldPaytypeRows == 0)
                        {
                            B_UserTable varUSer = (from tbUser in myDYXTEntities.B_UserTable
                                                   where tbUser.UserID == Authentication.UserID
                                                   select tbUser).Single();
                            varUSer.UserName = UserName;
                            varUSer.TrueName = TrueName;
                            myDYXTEntities.Entry(varUSer).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();
                            strMsg = "success";

                        }
                        else
                        {
                            strMsg = "exsit";
                        }

                    }
                    else
                    {
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == Authentication.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral + 10;
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        B_AuthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                                                   where tbAuthentication.AuthenticationID == Authentication.AuthenticationID
                                                                   select tbAuthentication).Single();
                        //判断是否上传图片
                        varAuthentication.Institutions = Authentication.Institutions;
                        varAuthentication.Specialty = Authentication.Specialty;
                        varAuthentication.EducationalBackground = Authentication.EducationalBackground;
                        varAuthentication.EnrolTime = Convert.ToDateTime(Intime);
                        varAuthentication.GraduationTime = Convert.ToDateTime(Entime);
                        varAuthentication.StatusID = Authentication.StatusID;
                        varAuthentication.ReviewRemarks = Authentication.ReviewRemarks;
                        varAuthentication.Checktime = DateTime.Now;
                        if (fileStudentImage != null)
                        {
                            byte[] imgFile = new byte[fileStudentImage.ContentLength];
                            fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                            varAuthentication.Diploma = imgFile;//更新图片                              
                        }
                        myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();

                        int oldPaytypeRows = (from tbUser in myDYXTEntities.B_UserTable
                                              where tbUser.UserName == UserName && tbUser.UserID != Authentication.UserID
                                              select tbUser).Count();
                        if (oldPaytypeRows == 0)
                        {
                            B_UserTable varUSer = (from tbUser in myDYXTEntities.B_UserTable
                                                   where tbUser.UserID == Authentication.UserID
                                                   select tbUser).Single();
                            varUSer.UserName = UserName;
                            varUSer.TrueName = TrueName;
                            myDYXTEntities.Entry(varUSer).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();
                            strMsg = "success";

                        }
                        else
                        {
                            strMsg = "exsit";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 认证记录
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="SUserName"></param>
        /// <param name="SeUserName"></param>
        /// <param name="SeUserID"></param>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        public ActionResult selsectrenzhengJilu(BsgridPage bsgridPage, string SNserName, string InstitutionsID, string EducationalBackgroundID)
        {

            var notict = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                          join tbuser in myDYXTEntities.B_UserTable on tbAuthentication.UserID equals tbuser.UserID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbAuthentication.StatusID equals tbStatus.StatusID
                          where tbAuthentication.StatusID !=2
                          select new B_AuthenticationVo
                          {
                              AuthenticationID = tbAuthentication.AuthenticationID,
                              UserName = tbuser.UserName,
                              TrueName = tbuser.TrueName,
                              Institutions = tbAuthentication.Institutions,
                              Specialty = tbAuthentication.Specialty,
                              EducationalBackground = tbAuthentication.EducationalBackground,
                              Intime = tbAuthentication.EnrolTime.ToString(),
                              Entime = tbAuthentication.GraduationTime.ToString(),
                              StatusID = tbAuthentication.StatusID,
                              StatusName = tbStatus.StatusName,
                              ReviewRemarks = tbAuthentication.ReviewRemarks,
                              ReleaseTimeStr = tbAuthentication.Checktime.ToString()
                          }).OrderByDescending(p => p.AuthenticationID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            if (!string.IsNullOrEmpty(SNserName))
            {
                notict = notict.Where(n => n.UserName.Contains(SNserName)).ToList();
            }
            if (!string.IsNullOrEmpty(InstitutionsID))
            {
                notict = notict.Where(n => n.Institutions.Contains(InstitutionsID)).ToList();
            }
            if (!string.IsNullOrEmpty(EducationalBackgroundID))
            {
                notict = notict.Where(n => n.EducationalBackground.Contains(EducationalBackgroundID)).ToList();
            }

            int totalRow = myDYXTEntities.B_AuthenticationTable.Count();
            Bsgrid<B_AuthenticationVo> bsgrid = new Bsgrid<B_AuthenticationVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notict;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 认证记录绑定图片
        /// </summary>
        /// <param name="AuthenticationID"></param>
        /// <returns></returns>
        public ActionResult GetDiplomaImage(int AuthenticationID)
        {
            try
            {
                var studentImg = (from tbAuthentication in myDYXTEntities.B_AuthenticationTable
                                  where tbAuthentication.AuthenticationID == AuthenticationID
                                  select new
                                  {
                                      tbAuthentication.Diploma
                                  }).Single();

                byte[] imageData = studentImg.Diploma;
                return File(imageData, @"image/jpg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

        #region 手机认证
        public ActionResult ShouJiRenzheng()
        {
            return View();
        }

        public ActionResult selsectB_phoneauthenticationTable(BsgridPage bsgridPage,string SUserNameID, string cellnumberID,int StatusID)
        {
            var notict = (from tbphoneauthentication in myDYXTEntities.B_phoneauthenticationTable
                          join tbuser in myDYXTEntities.B_UserTable on tbphoneauthentication.UserID equals tbuser.UserID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbphoneauthentication.StatusID equals tbStatus.StatusID
                          select new B_phoneauthenticationVo
                          {
                              PhoneauID = tbphoneauthentication.PhoneauID,
                              UserName = tbuser.UserName,
                              Cellnumber = tbphoneauthentication.Cellnumber,
                              StatusID = tbphoneauthentication.StatusID,
                              StatusName = tbStatus.StatusName,
                              addtime = tbphoneauthentication.Addtime.ToString(),
                              transitTime = tbphoneauthentication.TransitTime.ToString()                       
                          }).OrderByDescending(p => p.PhoneauID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            if (!string.IsNullOrEmpty(SUserNameID))
            {
                notict = notict.Where(n => n.UserName.Contains(SUserNameID)).ToList();
            }
            if (!string.IsNullOrEmpty(cellnumberID))
            {
                notict = notict.Where(n => n.Cellnumber.Contains(cellnumberID)).ToList();
            }
            if (StatusID > 0)
            {
                notict = notict.Where(p => p.StatusID == StatusID).ToList();
            }
            int totalRow =myDYXTEntities.B_phoneauthenticationTable.Count();
            Bsgrid<B_phoneauthenticationVo> bsgrid = new Bsgrid<B_phoneauthenticationVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notict;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加手机认证
        /// </summary>     
        /// <param name="UserName"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        public ActionResult insertbaocunUser(string UserName, string extend_field5,string captcha)
        {
            //定义返回
            string strMsg = "fail";          
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["mobilecode"];
                //判断验证码
                if (cookie != null)
                {
                    if (captcha == System.Web.HttpContext.Current.Server.UrlDecode(cookie["mobile_code"]))
                    {                     
                          var varuser = (from tbstruser in myDYXTEntities.B_UserTable
                                           where tbstruser.UserName == UserName
                                           select tbstruser).Single();
                            B_phoneauthenticationTable phoneauthentication = new B_phoneauthenticationTable();
                            phoneauthentication.UserID = varuser.UserID;
                            phoneauthentication.StatusID = 24;
                            phoneauthentication.Cellnumber = extend_field5;
                            phoneauthentication.Addtime = DateTime.Now;
                            phoneauthentication.TransitTime= DateTime.Now; 
                            myDYXTEntities.B_phoneauthenticationTable.Add(phoneauthentication);
                            myDYXTEntities.SaveChanges();
                            strMsg = "success";//成功
                        
                    }
                    else
                    {
                        strMsg = "failCodeErro";//验证码不正确
                    }
            }
            else
            {
                strMsg = "ValidCodeErro";//验证码过期
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 手机号码是否已存在
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ActionResult selectUsercallnumber(string extend_field5)
        {
            string strMsg = "";
            int user = (from tbphoneauthentication in myDYXTEntities.B_phoneauthenticationTable
                        where tbphoneauthentication.Cellnumber == extend_field5
                        select tbphoneauthentication).Count();
            if (user > 0)
            {
                strMsg = "success";//成功
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 发送验证码接口
        /// </summary>
        public static string PostUrl = ConfigurationManager.AppSettings["WebReference.Service.PostUrl"];
        public ActionResult FaSongYanZhengMa() {

            string strMsg = "";
            try
            {
            string account = "C40370261";//用户名是登录用户中心->验证码、通知短信->帐户及签名设置->APIID
            string password = "4ba6f015cfda3626cbc3e18e8b870243"; //密码是请登录用户中心->验证码、通知短信->帐户及签名设置->APIKEY
            string mobile = Request.QueryString["mobile"];//获取发送的手机号码
            Random rad = new Random();
            int mobile_code = rad.Next(1000, 10000);
            string content = "您的验证码是：" + mobile_code + "。请不要把验证码泄露给其他人。";

                HttpCookie cookie = new HttpCookie("mobilecode");
                cookie.Expires = DateTime.Now.AddMinutes(1);//保存1分钟
                cookie["mobile_code"] =mobile_code.ToString();
                Response.Cookies.Add(cookie);

            string postStrTpl = "account={0}&password={1}&mobile={2}&content={3}";
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(string.Format(postStrTpl, account, password, mobile, content));
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;

            Stream newStream = myRequest.GetRequestStream();
            //Send the data. Send the data
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                if (myResponse.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    //Response.Write(reader.ReadToEnd());
                    string res = reader.ReadToEnd();
                    int len1 = res.IndexOf("</code>");
                    int len2 = res.IndexOf("<code>");
                    string code = res.Substring((len2 + 6), (len1 - len2 - 6));
                    //你好吗?
                    int len3 = res.IndexOf("</msg>");
                    int len4 = res.IndexOf("<msg>");
                    string msg = res.Substring((len4 + 5), (len3 - len4 - 5));
                    Response.Write(msg);
                    Response.End();
                    strMsg = "success";
                }
                else
                {
                    strMsg = "fail";
                    //访问失败（断网）
                }        
            }
            catch (Exception e)
            {
                Console.Write(e);
                strMsg = "fail";
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
       

        public ActionResult ShenhePhoneauID(int PhoneauID)
        {
            try
            {
                var Recharge = (from tbphoneauthentication in myDYXTEntities.B_phoneauthenticationTable
                                where tbphoneauthentication.PhoneauID == PhoneauID
                                select new B_phoneauthenticationVo
                                {
                                    PhoneauID = tbphoneauthentication.PhoneauID,
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
        public ActionResult Updatashenhebaocun(B_phoneauthenticationTable phoneauthentication, string optionsRadios, string validCode)
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
                    B_phoneauthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_phoneauthenticationTable
                                                               where tbAuthentication.PhoneauID == phoneauthentication.PhoneauID
                                                               select tbAuthentication).Single();
                    varAuthentication.StatusID = 2;
                    varAuthentication.ReviewRemarks = phoneauthentication.ReviewRemarks;
                    varAuthentication.TransitTime = DateTime.Now;
                    myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }
                }
                else if (optionsRadios == "option2")
                {
                    B_phoneauthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_phoneauthenticationTable
                                                                    where tbAuthentication.PhoneauID == phoneauthentication.PhoneauID
                                                                    select tbAuthentication).Single();
                    varAuthentication.StatusID = 24;
                    varAuthentication.ReviewRemarks = phoneauthentication.ReviewRemarks;
                    varAuthentication.TransitTime = DateTime.Now;
                    myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }

                }
                else
                {
                    B_phoneauthenticationTable varAuthentication = (from tbAuthentication in myDYXTEntities.B_phoneauthenticationTable
                                                                    where tbAuthentication.PhoneauID == phoneauthentication.PhoneauID
                                                                    select tbAuthentication).Single();
                    varAuthentication.StatusID = 25;
                    varAuthentication.ReviewRemarks = phoneauthentication.ReviewRemarks;
                    varAuthentication.TransitTime = DateTime.Now;
                    myDYXTEntities.Entry(varAuthentication).State = System.Data.Entity.EntityState.Modified;
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
        /// 修改手机认证绑定
        /// </summary>
        /// <param name="PhoneauID"></param>
        /// <returns></returns>
        public ActionResult updatashoujirenzhengbangding(int PhoneauID)
        {
            try
            {
                var Recharge = (from tbphoneauthentication in myDYXTEntities.B_phoneauthenticationTable
                                join tbuser in myDYXTEntities.B_UserTable on tbphoneauthentication.UserID equals tbuser.UserID
                                join tbStatus in myDYXTEntities.S_StatusTable on tbphoneauthentication.StatusID equals tbStatus.StatusID
                                where tbphoneauthentication.PhoneauID == PhoneauID
                                select new B_phoneauthenticationVo
                                {
                                    PhoneauID = tbphoneauthentication.PhoneauID,
                                    UserID = tbphoneauthentication.UserID,
                                    UserName = tbuser.UserName,
                                    Cellnumber = tbphoneauthentication.Cellnumber.Trim(),
                                    StatusID=tbphoneauthentication.StatusID,
                                    StatusName = tbStatus.StatusName,
                                    addtime = tbphoneauthentication.Addtime.ToString(),
                                    transitTime = tbphoneauthentication.TransitTime.ToString(),                               
                                    ReviewRemarks = tbphoneauthentication.ReviewRemarks.Trim()
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
        /// 修改手机认证保存
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public ActionResult Updatashoujirenzheng(B_phoneauthenticationTable phoneauthentication)
        {
            string strMsg = "fali";
            try
            {

                B_phoneauthenticationTable varphoneauthentication = (from tbphoneauthentication in myDYXTEntities.B_phoneauthenticationTable
                                                      where tbphoneauthentication.PhoneauID == phoneauthentication.PhoneauID
                                                       select tbphoneauthentication).Single();
                varphoneauthentication.UserID = phoneauthentication.UserID;
                varphoneauthentication.Cellnumber = phoneauthentication.Cellnumber;
                varphoneauthentication.StatusID = phoneauthentication.StatusID;
                varphoneauthentication.TransitTime = DateTime.Now;
                varphoneauthentication.ReviewRemarks = phoneauthentication.ReviewRemarks;
                myDYXTEntities.Entry(varphoneauthentication).State = System.Data.Entity.EntityState.Modified;
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 发送记录查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="DXUserName"></param>
        /// <param name="DXCellnumber"></param>
        /// <param name="SendstatusID"></param>
        /// <returns></returns>
        public ActionResult selctsendjilu(BsgridPage bsgridPage,string DXUserName, string DXCellnumber, int SendstatusID)
        {

            var notict = (from tbSent in myDYXTEntities.B_SentTable
                          join tbuser in myDYXTEntities.B_UserTable on tbSent.UserID equals tbuser.UserID
                          join tbSendstatus in myDYXTEntities.S_SendstatusTable on tbSent.SendstatusID equals tbSendstatus.SendstatusID                    
                          select new B_SentVo
                          {
                              SentID= tbSent.SentID,
                              UserName=tbuser.UserName,
                              Cellnumber=tbSent.Cellnumber,
                              SendstatusID= tbSent.SendstatusID,
                              SendstatusName = tbSendstatus.SendstatusName,
                              ReleaseTimeStr = tbSent.Timeofdeparture.ToString()
                          }).OrderByDescending(p => p.SentID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            if (!string.IsNullOrEmpty(DXUserName))
            {
                notict = notict.Where(n => n.UserName.Contains(DXUserName)).ToList();
            }
            if (!string.IsNullOrEmpty(DXCellnumber))
            {
                notict = notict.Where(n => n.Cellnumber.Contains(DXCellnumber)).ToList();
            }
            if (SendstatusID > 0)
            {
                notict = notict.Where(p => p.SendstatusID == SendstatusID).ToList();
            }
            int totalRow = myDYXTEntities.B_SentTable.Count();
            Bsgrid<B_SentVo> bsgrid = new Bsgrid<B_SentVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notict;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询中的发送状态
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctSendstatusype()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "查看全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> Province = (from tbStatus in myDYXTEntities.S_SendstatusTable
                                       select new SelectVo
                                       {
                                           id = tbStatus.SendstatusID,
                                           text = tbStatus.SendstatusName
                                       }).ToList();
            listnoticeType.AddRange(Province);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 发送状态
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctSendtype()
        {

            List<SelectVo> Province = (from tbStatus in myDYXTEntities.S_Transmitstatus
                                       select new SelectVo
                                       {
                                           id = tbStatus.TransmitstatusID,
                                           text = tbStatus.TransmitstatusName
                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发送短信新增保存
        /// </summary>
        /// <param name="Sent"></param>
        /// <param name="UserName"></param>
        /// <param name="validCode"></param>
        /// <param name="Transmitstatus"></param>
        /// <returns></returns>
        public ActionResult insertFasongduangxin(B_SentTable Sent, string UserName, string validCode,int Transmitstatus)
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
                int userid = (from tbuser in myDYXTEntities.B_UserTable
                              where tbuser.UserName == UserName
                              select tbuser).Count();
                if (userid > 0)
                {                   
                        if (Transmitstatus==1)
                        {
                            var varuser = (from tbstruser in myDYXTEntities.B_UserTable
                                           where tbstruser.UserName == UserName
                                           select tbstruser).Single();
                            Sent.UserID = varuser.UserID;
                            Sent.SendstatusID = 1;
                            Sent.Timeofdeparture = DateTime.Now;
                            myDYXTEntities.B_SentTable.Add(Sent);
                            myDYXTEntities.SaveChanges();
                            strMsg = "success";//成功
                        }
                        else if (Transmitstatus == 2)                   
                        {
                            var varuser = (from tbstruser in myDYXTEntities.B_UserTable
                                           where tbstruser.UserName == UserName
                                           select tbstruser).Single();
                            Sent.UserID = varuser.UserID;
                            Sent.SendstatusID = 2;
                            Sent.Timeofdeparture = DateTime.Now;
                            myDYXTEntities.B_SentTable.Add(Sent);
                            myDYXTEntities.SaveChanges();
                            strMsg = "success";//成功
                       }
                        else
                      {
                        var varuser = (from tbstruser in myDYXTEntities.B_UserTable
                                       where tbstruser.UserName == UserName
                                       select tbstruser).Single();
                        Sent.UserID = varuser.UserID;
                        Sent.SendstatusID = 3;
                        Sent.Timeofdeparture = DateTime.Now;
                        myDYXTEntities.B_SentTable.Add(Sent);
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";//成功
                    }                                        
                }
                else
                {
                    strMsg = "failUser";
                }
            }
            else
            {
                strMsg = "ValidCodeErro";//验证码错误
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult shoujihaoma(string UserName) {
            try
            {
               
                    var varuserDY = (from tbstruser in myDYXTEntities.B_UserTable
                                   where tbstruser.UserName == UserName
                                   select tbstruser).Single();
              
             
                return Json(varuserDY, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json("fail", JsonRequestBehavior.AllowGet);
            }
          
        }
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="SentID"></param>
        /// <returns></returns>
        public ActionResult selectchakanxinxi(int SentID)
        {
            try
            {
                var varsent = (from tbSent in myDYXTEntities.B_SentTable
                               join tbUser in myDYXTEntities.B_UserTable on tbSent.UserID equals tbUser.UserID
                               join tbSendstatus in myDYXTEntities.S_SendstatusTable on tbSent.SendstatusID equals tbSendstatus.SendstatusID
                               where tbSent.SentID== SentID
                               select new B_SentVo
                               {
                                   SentID = tbSent.SentID,
                                   UserName = tbUser.UserName,
                                   Cellnumber = tbSent.Cellnumber,
                                   SendstatusID = tbSent.SendstatusID,
                                   SendstatusName = tbSendstatus.SendstatusName,
                                   ReleaseTimeStr = tbSent.Timeofdeparture.ToString(),
                                   DepartureContent = tbSent.DepartureContent,
                               }).Single();
                return Json(varsent,JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #region 视频认证
        public ActionResult VideoAttestation()
        {
            return View();
        }

        public ActionResult selectVideoauthenticationTable(BsgridPage bsgridPage,string Usernameid,string SUserNameID,int SEUserID,int StatusID)
        {

            var notict = from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                          join tbuser in myDYXTEntities.B_UserTable on tbVideoauthentication.UserID equals tbuser.UserID
                          join tbStatus in myDYXTEntities.S_StatusTable on tbVideoauthentication.StatusID equals tbStatus.StatusID
                          select new B_VideoauthenticationVo
                          {
                              VideoauthenticationID = tbVideoauthentication.VideoauthenticationID,
                              UserID=tbVideoauthentication.UserID,
                              UserName = tbuser.UserName,
                              TrueName= tbuser.TrueName,
                              ReleaseTimeStr = tbVideoauthentication.ApplicationDate.ToString(),
                              StatusName = tbStatus.StatusName,
                              StatusID=tbVideoauthentication.StatusID
                            
                          };
            if (!string.IsNullOrEmpty(Usernameid))
            {
                notict = notict.Where(n => n.UserName.Contains(Usernameid));
            }
            if (!string.IsNullOrEmpty(SUserNameID))
            {
                notict = notict.Where(n => n.UserName.Contains(SUserNameID));
            }
            if (SEUserID > 0)
            {
                notict = notict.Where(p => p.UserID == SEUserID);
            }
            if (StatusID > 0)
            {
                notict = notict.Where(p => p.StatusID == StatusID);
            }
           
            int totalRow = notict.Count();
            List<B_VideoauthenticationVo> notices = notict.OrderByDescending(p => p.VideoauthenticationID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_VideoauthenticationVo> bsgrid = new Bsgrid<B_VideoauthenticationVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Shenhevideo(int VideoauthenticationID)
        {

            try
            {
                var Recharge = (from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                                where tbVideoauthentication.VideoauthenticationID == VideoauthenticationID
                                select new B_VideoauthenticationVo
                                {
                                    VideoauthenticationID = tbVideoauthentication.VideoauthenticationID,
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
        /// <param name="Videoauthentication"></param>
        /// <param name="optionsRadios"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        public ActionResult Updatavideobaocun(B_VideoauthenticationTable Videoauthentication, string optionsRadios, string validCode)
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
                    B_VideoauthenticationTable vavideoau = (from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                                                            where tbVideoauthentication.VideoauthenticationID == Videoauthentication.VideoauthenticationID
                                                           select tbVideoauthentication).Single();

                    vavideoau.StatusID = 2;
                    vavideoau.Integral = Videoauthentication.Integral;
                    vavideoau.ReviewRemarks = Videoauthentication.ReviewRemarks;
                    myDYXTEntities.Entry(vavideoau).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }
                }
                else if(optionsRadios == "option2")
                {
                    B_VideoauthenticationTable vavideoau = (from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                                                            where tbVideoauthentication.VideoauthenticationID == Videoauthentication.VideoauthenticationID
                                                            select tbVideoauthentication).Single();

                    vavideoau.StatusID = 24;
                    vavideoau.Integral = Videoauthentication.Integral;
                    vavideoau.ReviewRemarks = Videoauthentication.ReviewRemarks;
                    myDYXTEntities.Entry(vavideoau).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == vavideoau.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral +Convert.ToDecimal(Videoauthentication.Integral);
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }
                }
                else
                {
                    B_VideoauthenticationTable vavideoau = (from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                                                            where tbVideoauthentication.VideoauthenticationID == Videoauthentication.VideoauthenticationID
                                                            select tbVideoauthentication).Single();

                    vavideoau.StatusID = 25;
                    vavideoau.Integral = Videoauthentication.Integral;
                    vavideoau.ReviewRemarks = Videoauthentication.ReviewRemarks;
                    myDYXTEntities.Entry(vavideoau).State = System.Data.Entity.EntityState.Modified;
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
        /// 修改回填
        /// </summary>
        /// <param name="VideoauthenticationID"></param>
        /// <returns></returns>
        public ActionResult updataB_VideoauthenticationTable(int VideoauthenticationID)
        {
            try
            {
                var Recharge = (from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                                join tbUSer in myDYXTEntities.B_UserTable  on tbVideoauthentication.UserID equals tbUSer.UserID
                                join tbStatus in myDYXTEntities.S_StatusTable on tbVideoauthentication.StatusID equals tbStatus.StatusID
                                where tbVideoauthentication.VideoauthenticationID == VideoauthenticationID
                                select new B_VideoauthenticationVo
                                {
                                    VideoauthenticationID = tbVideoauthentication.VideoauthenticationID,
                                    UserName = tbUSer.UserName.Trim(),
                                    TrueName = tbUSer.TrueName.Trim(),
                                    ReleaseTimeStr = tbVideoauthentication.ApplicationDate.ToString(),
                                    StatusName = tbStatus.StatusName.Trim(),
                                    StatusID = tbVideoauthentication.StatusID,
                                    Integral= tbVideoauthentication.Integral,
                                    ReviewRemarks = tbVideoauthentication.ReviewRemarks.Trim()
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
        /// 视频认证中的修改
        /// </summary>
        /// <param name="Videoauthentication"></param>
        /// <param name="TrueName"></param>
        /// <param name="UserName"></param>
        /// <param name="ReleaseTimeStr"></param>
        /// <returns></returns>
        public ActionResult UpdataB_Videoauthentication(B_VideoauthenticationTable Videoauthentication,string ReleaseTimeStr)
        {
            string strMsg = "fali";
            try
            {
                B_VideoauthenticationTable strvarUser = (from tbuser in myDYXTEntities.B_VideoauthenticationTable
                                                    where tbuser.VideoauthenticationID == Videoauthentication.VideoauthenticationID
                                                    select tbuser).Single();

                if (strvarUser.StatusID == Videoauthentication.StatusID)
                {
                    B_VideoauthenticationTable vavideoau = (from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                                                            where tbVideoauthentication.VideoauthenticationID == Videoauthentication.VideoauthenticationID
                                                            select tbVideoauthentication).Single();

                    vavideoau.Integral = Videoauthentication.Integral;
                    vavideoau.ApplicationDate = Convert.ToDateTime(ReleaseTimeStr);
                    vavideoau.ReviewRemarks = Videoauthentication.ReviewRemarks;
                    vavideoau.StatusID = Videoauthentication.StatusID;
                    myDYXTEntities.Entry(vavideoau).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {                        
                        strMsg = "success";
                    }
                }
                else
                {
                    if (Videoauthentication.StatusID == 2 || Videoauthentication.StatusID == 25)
                    {
                        
                        B_VideoauthenticationTable vavideoau = (from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                                                                where tbVideoauthentication.VideoauthenticationID == Videoauthentication.VideoauthenticationID
                                                                select tbVideoauthentication).Single();
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == vavideoau.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral -Convert.ToDecimal(Videoauthentication.Integral);
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                       

                        vavideoau.Integral = Videoauthentication.Integral;
                        vavideoau.ApplicationDate = Convert.ToDateTime(ReleaseTimeStr);
                        vavideoau.ReviewRemarks = Videoauthentication.ReviewRemarks;
                        vavideoau.StatusID = Videoauthentication.StatusID;
                        myDYXTEntities.Entry(vavideoau).State = System.Data.Entity.EntityState.Modified;
                        if (myDYXTEntities.SaveChanges() > 0)
                        {                            
                            strMsg = "success";
                        }


                    }
                    else
                    {
                        B_VideoauthenticationTable vavideoau = (from tbVideoauthentication in myDYXTEntities.B_VideoauthenticationTable
                                                                where tbVideoauthentication.VideoauthenticationID == Videoauthentication.VideoauthenticationID
                                                                select tbVideoauthentication).Single();
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == vavideoau.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral + Convert.ToDecimal(Videoauthentication.Integral);
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                       

                        vavideoau.Integral = Videoauthentication.Integral;
                        vavideoau.ApplicationDate = Convert.ToDateTime(ReleaseTimeStr);
                        vavideoau.ReviewRemarks = Videoauthentication.ReviewRemarks;
                        vavideoau.StatusID = Videoauthentication.StatusID;
                        myDYXTEntities.Entry(vavideoau).State = System.Data.Entity.EntityState.Modified;
                        if (myDYXTEntities.SaveChanges() > 0)
                        {                           
                            strMsg = "success";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 材料审核
        public ActionResult selectMaterialAttestation()
        {
            return View();
        }

        public ActionResult selctB_MaterialAttestationTable(BsgridPage bsgridPage,string Usernameid,string SUserNameID,int ItemTypeID,int SEUserID,int StatusID)
        {

            var notict = from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                          join tbuser in myDYXTEntities.B_UserTable on tbMaterialAttestation.UserID equals tbuser.UserID
                          join tbSendstatus in myDYXTEntities.S_StatusTable on tbMaterialAttestation.StatusID equals tbSendstatus.StatusID
                          join tbItemType in myDYXTEntities.S_ItemTypeTable on tbMaterialAttestation.ItemTypeID equals tbItemType.ItemTypeID
                          select new B_MaterialAttestationVo
                          {
                              MaterialAttestationID = tbMaterialAttestation.MaterialAttestationID,
                              UserID= tbMaterialAttestation.UserID,
                              ItemTypeID= tbMaterialAttestation.ItemTypeID,
                              StatusID= tbMaterialAttestation.StatusID,
                              UserName = tbuser.UserName,
                              ItemTypeName = tbItemType.ItemTypeName,
                              Integral = tbMaterialAttestation.Integral,
                              StatusName = tbSendstatus.StatusName,
                              ReleaseTimeStr = tbMaterialAttestation.AddTime.ToString()
                          };
            if (!string.IsNullOrEmpty(Usernameid))
            {
                notict = notict.Where(n => n.UserName.Contains(Usernameid));
            }
            if (!string.IsNullOrEmpty(SUserNameID))
            {
                notict = notict.Where(n => n.UserName.Contains(SUserNameID));
            }
            if (ItemTypeID > 0)
            {
                notict = notict.Where(p => p.ItemTypeID == ItemTypeID);
            }
            if (SEUserID > 0)
            {
                notict = notict.Where(p => p.UserID == SEUserID);
            }
            if (StatusID > 0)
            {
                notict = notict.Where(p => p.StatusID == StatusID);
            }
            int totalRow = notict.Count();
            List<B_MaterialAttestationVo> notices = notict.OrderByDescending(p => p.MaterialAttestationID).
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
        /// 图片查询
        /// </summary>
        /// <param name="MaterialAttestationID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 类型
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctS_ItemTypeTable()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "不限"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> Province = (from tbStatus in myDYXTEntities.S_ItemTypeTable
                                       select new SelectVo
                                       {
                                           id = tbStatus.ItemTypeID,
                                           text = tbStatus.ItemTypeName
                                       }).ToList();
            listnoticeType.AddRange(Province);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 审核绑定
        /// </summary>
        /// <param name="MaterialAttestationID"></param>
        /// <returns></returns>
        public ActionResult shenheMaterialAttestation(int MaterialAttestationID)
        {

            try
            {
                var Recharge = (from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                                join tbuser in myDYXTEntities.B_UserTable on tbMaterialAttestation.UserID equals tbuser.UserID
                                join tbSendstatus in myDYXTEntities.S_StatusTable on tbMaterialAttestation.StatusID equals tbSendstatus.StatusID
                                join tbItemType in myDYXTEntities.S_ItemTypeTable on tbMaterialAttestation.ItemTypeID equals tbItemType.ItemTypeID
                                where tbMaterialAttestation.MaterialAttestationID == MaterialAttestationID
                                select new B_MaterialAttestationVo
                                {
                                    MaterialAttestationID = tbMaterialAttestation.MaterialAttestationID,
                                    UserID = tbMaterialAttestation.UserID,
                                    ItemTypeID = tbMaterialAttestation.ItemTypeID,
                                    StatusID = tbMaterialAttestation.StatusID,
                                    UserName = tbuser.UserName,
                                    ItemTypeName = tbItemType.ItemTypeName,
                                    Integral = tbMaterialAttestation.Integral.Trim(),
                                    Remarks = tbMaterialAttestation.Remarks.Trim(),
                                    ReviewRemarks=tbMaterialAttestation.ReviewRemarks.Trim(),
                                    StatusName = tbSendstatus.StatusName,
                                    ReleaseTimeStr = tbMaterialAttestation.AddTime.ToString()
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
        /// 审核绑定图片
        /// </summary>
        /// <param name="MaterialAttestationID"></param>
        /// <returns></returns>
        public ActionResult GetMaterialAttestationImage(int MaterialAttestationID)
        {
            try
            {
                var studentImg = (from tbMaterialAttestationID in myDYXTEntities.B_MaterialAttestationTable
                                  where tbMaterialAttestationID.MaterialAttestationID == MaterialAttestationID
                                  select new
                                  {
                                      tbMaterialAttestationID.Picture
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
        /// <summary>
        /// 审核保存
        /// </summary>
        /// <param name="MaterialAttestation"></param>
        /// <param name="optionsRadios"></param>
        /// <returns></returns>
        public ActionResult UpdataMaterialAttestationbaocun(B_MaterialAttestationTable MaterialAttestation, string optionsRadios)
        {
            //定义返回
                string strMsg = "fail";
            if (MaterialAttestation.Integral== null || MaterialAttestation.Integral ==Convert.ToString(0))
            {
                B_MaterialAttestationTable varMaterialAttestation = (from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                                                                     where tbMaterialAttestation.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                                     select tbMaterialAttestation).Single();

                varMaterialAttestation.StatusID = 25;
                varMaterialAttestation.Integral = MaterialAttestation.Integral;
                varMaterialAttestation.ReviewRemarks = MaterialAttestation.ReviewRemarks;
                myDYXTEntities.Entry(varMaterialAttestation).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {
                    strMsg = "success";
                }
            }
            else
            {

                if (optionsRadios == "option1")
                {
                    B_MaterialAttestationTable varMaterialAttestation = (from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                                                                         where tbMaterialAttestation.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                                         select tbMaterialAttestation).Single();

                    varMaterialAttestation.StatusID = 2;
                    varMaterialAttestation.Integral = MaterialAttestation.Integral;
                    varMaterialAttestation.ReviewRemarks = MaterialAttestation.ReviewRemarks;
                    myDYXTEntities.Entry(varMaterialAttestation).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }
                }
                else if (optionsRadios == "option2")
                {
                    B_MaterialAttestationTable varMaterialAttestation = (from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                                                                         where tbMaterialAttestation.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                                         select tbMaterialAttestation).Single();

                    varMaterialAttestation.StatusID = 24;
                    varMaterialAttestation.Integral = MaterialAttestation.Integral;
                    varMaterialAttestation.ReviewRemarks = MaterialAttestation.ReviewRemarks;
                    myDYXTEntities.Entry(varMaterialAttestation).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == varMaterialAttestation.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral + Convert.ToDecimal(MaterialAttestation.Integral);
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }
                }
                else
                {
                    B_MaterialAttestationTable varMaterialAttestation = (from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                                                                         where tbMaterialAttestation.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                                         select tbMaterialAttestation).Single();

                    varMaterialAttestation.StatusID = 25;
                    varMaterialAttestation.Integral = MaterialAttestation.Integral;
                    varMaterialAttestation.ReviewRemarks = MaterialAttestation.ReviewRemarks;
                    myDYXTEntities.Entry(varMaterialAttestation).State = System.Data.Entity.EntityState.Modified;
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                        strMsg = "success";
                    }
                }
            }  
          
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="MaterialAttestation"></param>
        /// <param name="fileStudentImage"></param>
        /// <param name="ReleaseTimeStr"></param>
        /// <returns></returns>
        public ActionResult UpdateconservationMaterialAttestation(B_MaterialAttestationTable MaterialAttestation, HttpPostedFileBase fileStudentImage, string ReleaseTimeStr)
        {
            string strMsg = "fail";
            try
            {
                B_MaterialAttestationTable strvarUser = (from tbuser in myDYXTEntities.B_MaterialAttestationTable
                                                         where tbuser.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                         select tbuser).Single();

                if (strvarUser.StatusID == MaterialAttestation.StatusID)
                {
                    if (MaterialAttestation.Integral == null || MaterialAttestation.Integral == Convert.ToString(0))
                    {
                        B_MaterialAttestationTable varMaterialAttestation = (from tbMaterialAttestationID in myDYXTEntities.B_MaterialAttestationTable
                                                                             where tbMaterialAttestationID.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                                             select tbMaterialAttestationID).Single();
                        //判断是否上传图片
                        varMaterialAttestation.ItemTypeID = MaterialAttestation.ItemTypeID;
                        varMaterialAttestation.Integral = MaterialAttestation.Integral;
                        varMaterialAttestation.ReviewRemarks = MaterialAttestation.ReviewRemarks;
                        varMaterialAttestation.StatusID = 25;
                        if (fileStudentImage != null)
                        {
                            byte[] imgFile = new byte[fileStudentImage.ContentLength];
                            fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                            varMaterialAttestation.Picture = imgFile;//更新图片                              
                        }
                        myDYXTEntities.Entry(varMaterialAttestation).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }
                    else
                    {
                        B_MaterialAttestationTable varMaterialAttestation = (from tbMaterialAttestationID in myDYXTEntities.B_MaterialAttestationTable
                                                                             where tbMaterialAttestationID.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                                             select tbMaterialAttestationID).Single();
                        //判断是否上传图片
                        varMaterialAttestation.ItemTypeID = MaterialAttestation.ItemTypeID;
                        varMaterialAttestation.Integral = MaterialAttestation.Integral;
                        varMaterialAttestation.ReviewRemarks = MaterialAttestation.ReviewRemarks;
                        varMaterialAttestation.StatusID = MaterialAttestation.StatusID;
                        if (fileStudentImage != null)
                        {
                            byte[] imgFile = new byte[fileStudentImage.ContentLength];
                            fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                            varMaterialAttestation.Picture = imgFile;//更新图片                              
                        }
                        myDYXTEntities.Entry(varMaterialAttestation).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }

                }
                else
                {
                    if (MaterialAttestation.StatusID == 2 || MaterialAttestation.StatusID == 25)
                    {
                        B_MaterialAttestationTable varMaterialAttestation = (from tbMaterialAttestationID in myDYXTEntities.B_MaterialAttestationTable
                                                                             where tbMaterialAttestationID.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                                             select tbMaterialAttestationID).Single();

                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == varMaterialAttestation.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral - Convert.ToDecimal(MaterialAttestation.Integral);
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();

                        if (MaterialAttestation.Integral == null || MaterialAttestation.Integral == Convert.ToString(0))
                        {
                           
                            //判断是否上传图片
                            varMaterialAttestation.ItemTypeID = MaterialAttestation.ItemTypeID;
                            varMaterialAttestation.Integral = MaterialAttestation.Integral;
                            varMaterialAttestation.ReviewRemarks = MaterialAttestation.ReviewRemarks;
                            varMaterialAttestation.StatusID = 25;
                            if (fileStudentImage != null)
                            {
                                byte[] imgFile = new byte[fileStudentImage.ContentLength];
                                fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                                varMaterialAttestation.Picture = imgFile;//更新图片                              
                            }
                            myDYXTEntities.Entry(varMaterialAttestation).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();
                            strMsg = "success";
                        }
                        else
                        {                           
                            //判断是否上传图片
                            varMaterialAttestation.ItemTypeID = MaterialAttestation.ItemTypeID;
                            varMaterialAttestation.Integral = MaterialAttestation.Integral;
                            varMaterialAttestation.ReviewRemarks = MaterialAttestation.ReviewRemarks;
                            varMaterialAttestation.StatusID = MaterialAttestation.StatusID;
                            if (fileStudentImage != null)
                            {
                                byte[] imgFile = new byte[fileStudentImage.ContentLength];
                                fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                                varMaterialAttestation.Picture = imgFile;//更新图片                              
                            }
                            myDYXTEntities.Entry(varMaterialAttestation).State = System.Data.Entity.EntityState.Modified;
                            myDYXTEntities.SaveChanges();
                            strMsg = "success";
                        }
                    }
                    else
                    {
                        B_MaterialAttestationTable varMaterialAttestation = (from tbMaterialAttestationID in myDYXTEntities.B_MaterialAttestationTable
                                                                             where tbMaterialAttestationID.MaterialAttestationID == MaterialAttestation.MaterialAttestationID
                                                                             select tbMaterialAttestationID).Single();
                        B_UserTable varUser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == varMaterialAttestation.UserID
                                               select tbuser).Single();
                        varUser.Integral = varUser.Integral + Convert.ToDecimal(MaterialAttestation.Integral);
                        myDYXTEntities.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();

                    }
                }



                }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="MaterialAttestationID"></param>
        /// <returns></returns>
        public ActionResult deleteMaterialAttestation(int MaterialAttestationID)
        {
            string strMsg = "fail";
            try
            {
                B_MaterialAttestationTable varFundCost = (from tbMaterialAttestation in myDYXTEntities.B_MaterialAttestationTable
                                                          where tbMaterialAttestation.MaterialAttestationID == MaterialAttestationID
                                                          select tbMaterialAttestation).Single();
                //删除数据
                myDYXTEntities.B_MaterialAttestationTable.Remove(varFundCost);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 证明材料类型
        public ActionResult materialtype()
        {
            return View();
        }

        public ActionResult selctItemType(BsgridPage bsgridPage,string ItemTypeNameid)
        {

            var notict = from tbItemTypeTable in myDYXTEntities.S_ItemTypeTable                       
                         select new S_ItemTypeTableVo
                         {
                             ItemTypeID = tbItemTypeTable.ItemTypeID,
                             ItemTypeName = tbItemTypeTable.ItemTypeName,
                             Identifier = tbItemTypeTable.Identifier,
                             Maxintegral = tbItemTypeTable.Maxintegral,
                             Termofvalidity = tbItemTypeTable.Termofvalidity,
                             ReleaseTimeStr = tbItemTypeTable.AddTime.ToString()                           
                         };
            if (!string.IsNullOrEmpty(ItemTypeNameid))
            {
                notict = notict.Where(n => n.ItemTypeName.Contains(ItemTypeNameid));
            }
            int totalRow = notict.Count();
            List<S_ItemTypeTableVo> notices = notict.OrderByDescending(p => p.ItemTypeID).
              Skip(bsgridPage.GetStartIndex()).
              Take(bsgridPage.pageSize).
              ToList();
            Bsgrid<S_ItemTypeTableVo> bsgrid = new Bsgrid<S_ItemTypeTableVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 材料证明类型新增
        /// </summary>
        /// <param name="ItemType"></param>
        /// <returns></returns>
        public ActionResult insertItemType(S_ItemTypeTable ItemType)
        {
                 //定义返回
                 string strMsg = "fail";
           
                int intItemType = (from tbItemType in myDYXTEntities.S_ItemTypeTable
                              where tbItemType.ItemTypeName == ItemType.ItemTypeName
                              select tbItemType).Count();
                if (intItemType > 0)
                {
                   strMsg = "failUser";                                                       
                }
                else
                {
                    if (ItemType.Termofvalidity == "0")
                    {
                        ItemType.Termofvalidity = "长期";
                        ItemType.AddTime = DateTime.Now;
                        myDYXTEntities.S_ItemTypeTable.Add(ItemType);
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";//成功 
                    }
                    else
                    {
                        ItemType.Termofvalidity = ItemType.Termofvalidity + "个月";
                        ItemType.AddTime = DateTime.Now;
                        myDYXTEntities.S_ItemTypeTable.Add(ItemType);
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";//成功 
                    }
            }
           
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult updatacailiaozhengming(int ItemTypeID)
        {
            try
            {
                var Recharge = (from tbItemTypeTable in myDYXTEntities.S_ItemTypeTable
                                where tbItemTypeTable.ItemTypeID == ItemTypeID
                                select new S_ItemTypeTableVo
                                {
                                    ItemTypeID = tbItemTypeTable.ItemTypeID,
                                    ItemTypeName = tbItemTypeTable.ItemTypeName.Trim(),
                                    Identifier = tbItemTypeTable.Identifier.Trim(),
                                    Describe=tbItemTypeTable.Describe.Trim(),
                                    Maxintegral = tbItemTypeTable.Maxintegral.Trim(),
                                    Termofvalidity = tbItemTypeTable.Termofvalidity.Trim(),
                                    ReleaseTimeStr = tbItemTypeTable.AddTime.ToString()
                                }).Single();
                return Json(Recharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdataItemTypebaoxun(S_ItemTypeTable ItemType)
        {
            string strMsg = "fali";
            try
            {
                int varUser = (from tbItemType in myDYXTEntities.S_ItemTypeTable
                                           where tbItemType.ItemTypeName == ItemType.ItemTypeName&& tbItemType.ItemTypeID!= ItemType.ItemTypeID
                               select tbItemType).Count();
                if (varUser>0)
                {
                    strMsg = "faliItemtype";
                }
                else
                {
                    //S_ItemTypeTable varItemType = (from tbItemType in myDYXTEntities.S_ItemTypeTable
                    //                               where tbItemType.ItemTypeName == ItemType.ItemTypeName
                    //                               select tbItemType).Single();
                    if (ItemType.Termofvalidity == "0"|| ItemType.Termofvalidity =="长期")
                    {
                        ItemType.Termofvalidity = "长期";
                        ItemType.AddTime = DateTime.Now;
                        myDYXTEntities.Entry(ItemType).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }
                    else
                    {
                        ItemType.Termofvalidity = ItemType.Termofvalidity;
                        ItemType.AddTime = DateTime.Now;
                        myDYXTEntities.Entry(ItemType).State = System.Data.Entity.EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                    }
                  
                }                                           
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteItemtype(int ItemTypeID)
        {
            string strMsg = "fail";
            try
            {
                S_ItemTypeTable varFundCost = (from tbMaterialAttestation in myDYXTEntities.S_ItemTypeTable
                                               where tbMaterialAttestation.ItemTypeID == ItemTypeID
                                               select tbMaterialAttestation).Single();
                //删除数据
                myDYXTEntities.S_ItemTypeTable.Remove(varFundCost);
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