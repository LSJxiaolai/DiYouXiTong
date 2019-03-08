using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiYouQianTaiXiTong.Models;
using DiYouQianTaiXiTong.Vo;
using DiYouQianTaiXiTong.Common;
using System.Text;
using System.Net;
using System.Configuration;
using System.IO;

namespace DiYouQianTaiXiTong.Areas.Personal.Controllers
{
    public class PersonalController : Controller
    {
        // GET: Personal/Personal
        Models.DYXTEntities myModels = new DYXTEntities();

        #region 基础资料

        #region 个人信息
        //填写个人信息页面
        public ActionResult PersonalIndex()
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

        //绑定账户名
        public ActionResult SelectAName()
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbAcc in myModels.B_AccountTable
                            where tbAcc.AccountID == accountID
                            select new
                            {
                                User = tbAcc.User,
                            }).Single();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        //填写资料保存
        public ActionResult InsertMessage(B_UserTable B_Account, string captcha)
        {
            string str = "";
            try
            {

                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["mobilecode"];
                //判断验证码
                if (cookie != null)
                {
                    if (captcha == System.Web.HttpContext.Current.Server.UrlDecode(cookie["mobile_code"]))
                    {


                        int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                        B_Account.AccountID = accountID;
                        B_Account.UserName = Request.Form["userName"];
                        B_Account.TrueName = Request.Form["trueName"];
                        B_Account.PhoneNumber = Request.Form["extend_field5"];
                        B_Account.Sex = Request.Form["Sex"];
                        //B_Account.BornDate = Convert.ToDateTime(Request.Form["bornDate"]);
                        B_Account.NativePlace = Request.Form["nativePlace"];
                        B_Account.MarriageState = Request.Form["marriageState"];
                        B_Account.Issue = Request.Form["issue"];
                        B_Account.EducationalBackground = Request.Form["educationalBackground"];
                        B_Account.MonthIncome = Request.Form["monthIncome"];
                        B_Account.SocialSecurity = Request.Form["socialSecurity"];
                        B_Account.housingCondition = Request.Form["housingCondition"];
                        B_Account.WhetherBuyCar = Request.Form["whetherBuyCar"];
                        B_Account.PostBox = Request.Form["PostBox"];
                        try
                        {
                            //判断该用户是否已存在
                            int list = (from tbuser in myModels.B_UserTable
                                        where tbuser.AccountID == accountID
                                        select new
                                        {
                                            tbuser.AccountID,
                                        }).Count();
                            if (list > 0)
                            {

                                str = "Exist";


                            }
                            else
                            {
                                if (B_Account.UserName != null)
                                {
                                    B_Account.Integral = 50;
                                    B_Account.UserTypeID = 2;
                                    B_Account.Time = DateTime.Now;
                                    B_Account.PropertyAmounts = Convert.ToDecimal(0.00);
                                    B_Account.UsableMoney = Convert.ToDecimal(0.00);
                                    B_Account.FreezeMoney = Convert.ToDecimal(0.00);
                                    B_Account.WaitMoney = Convert.ToDecimal(0.00);
                                    B_Account.CompensatoryMoney = Convert.ToDecimal(0.00);
                                    myModels.B_UserTable.Add(B_Account);
                                    myModels.SaveChanges();

                                    B_UserLoginDetailTable account = new B_UserLoginDetailTable();
                                    account.AccountID = accountID;
                                    account.PostBox = B_Account.PostBox;
                                    account.RegisterTime = DateTime.Now;
                                    account.lastLoginTime = DateTime.Now;
                                    account.EndLoginTime = DateTime.Now;
                                    account.LoginFrequency = 1;
                                    myModels.B_UserLoginDetailTable.Add(account);
                                    myModels.SaveChanges();


                                    B_UserTable tbuser = (from tUser in myModels.B_UserTable
                                                          where tUser.AccountID == accountID
                                                          select tUser).Single();
                                    B_phoneauthenticationTable phuser = new B_phoneauthenticationTable();
                                    phuser.UserID = tbuser.UserID;
                                    phuser.Cellnumber = tbuser.PhoneNumber;
                                    phuser.StatusID = 24;
                                    phuser.Addtime = DateTime.Now;
                                    phuser.TransitTime = DateTime.Now;
                                    myModels.B_phoneauthenticationTable.Add(phuser);
                                    myModels.SaveChanges();

                                    B_UserlimitTable Bser = new B_UserlimitTable();
                                    Bser.UserID = B_Account.UserID;
                                    Bser.Creditlimetl = 0;
                                    Bser.Cavailable = 0;
                                    Bser.Cfreeze = 0;
                                    Bser.Warrantlimetl = 0;
                                    Bser.Wavailable = 0;
                                    Bser.Wfreeze = 0;
                                    Bser.Mortgagelimitli = 0;
                                    Bser.Mavailable = 0;
                                    Bser.Mfreeze = 0;
                                    Bser.Roamlitil = 0;
                                    Bser.Ravailable = 0;
                                    Bser.Rfreeze = 0;
                                    myModels.B_UserlimitTable.Add(Bser);
                                    myModels.SaveChanges();


                                    str = "success";
                                }
                                else
                                {
                                    str = "fail";
                                }
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
                        str = "failCodeErro";//验证码不正确
                    }
                }
                else
                {
                    str = "ValidCodeErro";//验证码过期
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        //个人资料页面
        public ActionResult BaseIndex()
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

        //查看个人信息
        public ActionResult SelectMessage()
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select new
                            {
                                UserID = tbuser.UserID,
                                UserName = tbuser.UserName.Trim(),
                                TrueName = tbuser.TrueName,
                                IDCardNo = tbuser.IDCardNo,
                                PhoneNumber = tbuser.PhoneNumber,
                                Sex = tbuser.Sex,
                                BornDate = tbuser.BornDate.ToString(),
                                NativePlace = tbuser.NativePlace,
                                MarriageState = tbuser.MarriageState,
                                Issue = tbuser.Issue,
                                EducationalBackground = tbuser.EducationalBackground,
                                MonthIncome = tbuser.MonthIncome,
                                SocialSecurity = tbuser.SocialSecurity,
                                housingCondition = tbuser.housingCondition,
                                WhetherBuyCar = tbuser.WhetherBuyCar,

                            }).Single();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //修改资料页面
        public ActionResult UpdateBaseIndex()
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

        //传递ID
        public ActionResult SelectMessageByID(int userID)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                accountID = userID;
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select new
                            {
                                UserID = tbuser.UserID,
                                UserName = tbuser.UserName.Trim(),
                                Sex = tbuser.Sex,
                                BornDate = tbuser.BornDate.ToString(),
                                NativePlace = tbuser.NativePlace,
                                MarriageState = tbuser.MarriageState,
                                Issue = tbuser.Issue,
                                EducationalBackground = tbuser.EducationalBackground,
                                MonthIncome = tbuser.MonthIncome,
                                SocialSecurity = tbuser.SocialSecurity,
                                housingCondition = tbuser.housingCondition,
                                WhetherBuyCar = tbuser.WhetherBuyCar,

                            }).Single();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //修改保存
        public ActionResult UpdateMessage(B_UserTable B_user)
        {
            
            try
            {
               
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                B_UserTable user = (from tbuser in myModels.B_UserTable
                                    where tbuser.AccountID == intAccountID
                                    select tbuser).Single();
                if (B_user.UserName.Trim() != null)
                {

                    user.AccountID = intAccountID;
                    user.UserName = B_user.UserName.Trim();
                    user.Sex = B_user.Sex.Trim();
                    user.BornDate = B_user.BornDate;
                    user.NativePlace = B_user.NativePlace.Trim();
                    user.MarriageState = B_user.MarriageState.Trim().Trim();
                    user.Issue = B_user.Issue.Trim();
                    user.EducationalBackground = B_user.EducationalBackground.Trim();
                    user.MonthIncome = B_user.MonthIncome.Trim();
                    user.SocialSecurity = B_user.SocialSecurity.Trim();
                    user.housingCondition = B_user.housingCondition.Trim();
                    user.WhetherBuyCar = B_user.WhetherBuyCar.Trim();
                    myModels.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    myModels.SaveChanges();
                    return Json("success", JsonRequestBehavior.AllowGet);

                }
                return Json("BasicMessage",JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            int user = (from tbphoneauthentication in myModels.B_phoneauthenticationTable
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
        public ActionResult FaSongYanZhengMa()
        {

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
                cookie["mobile_code"] = mobile_code.ToString();
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
        #endregion

        #region 认证中心
        //认证页面
        public ActionResult AttestationIndex()
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

        //绑定项目
        public ActionResult SelectItem(BsgridPage bsgridPage)
        {

            var list = from tbitem in myModels.S_ItemTypeTable
                       select new ItemVo
                       {
                           ItemTypeID = tbitem.ItemTypeID,
                           ItemTypeName = tbitem.ItemTypeName,
                           Maxintegral = tbitem.Maxintegral,
                       };
            int toTalRow = list.Count();
            List<ItemVo> item = list.OrderBy(p => p.ItemTypeID)
                .Skip(bsgridPage.GetStartIndex())
                .Take(bsgridPage.pageSize)
                .ToList();

            Bsgrid<ItemVo> bsgrid = new Bsgrid<ItemVo>();
            bsgrid.success = true;
            bsgrid.totalRows = toTalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = item;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        //传递项目ID
        public ActionResult SelectItemByID(int itemID)
        {
            ItemVo list = (from tbitem in myModels.S_ItemTypeTable
                           where tbitem.ItemTypeID == itemID
                           select new ItemVo
                           {
                               ItemTypeID = tbitem.ItemTypeID,
                           }).Single();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //绑定学历认证的姓名
        public ActionResult SelectUserName()
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select new
                            {
                                UserName = tbuser.UserName,
                                TrueName = tbuser.TrueName
                            }).Single();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        //新增学历认证
        public ActionResult InsertEducationalBackground(B_AuthenticationTable pwAuthen, HttpPostedFileBase fileStudentImage)
        {
            try
            {
                string strMsg = "";
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                try
                {
                    B_UserTable buser = (from tbuser in myModels.B_UserTable
                                         where tbuser.AccountID == accountID
                                         select tbuser).Single();
                    int list = (from tbAuthen in myModels.B_AuthenticationTable
                                where tbAuthen.UserID == buser.UserID
                                select new
                                {
                                    tbAuthen.UserID,
                                }).Count();
                    if (list > 0)
                    {

                        strMsg = "Exsit";
                    }
                    else
                    {
                        int UserID = buser.UserID;
                        byte[] imgFile = null;
                        if (fileStudentImage != null && fileStudentImage.ContentLength > 0)
                        {
                            imgFile = new byte[fileStudentImage.ContentLength];// 初始化为图片的长度
                                                                               //读取该图片文件
                                                                               //将图片转为流结束位置
                                                                               //将流读取为byte[]，参数：byte[]，读取开始位置，读取字节数
                            fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);

                        }

                        pwAuthen.UserID = UserID;
                        pwAuthen.Diploma = imgFile;
                        pwAuthen.StatusID = 2;
                        myModels.B_AuthenticationTable.Add(pwAuthen);
                        myModels.SaveChanges();
                        strMsg = "success";
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return Json(strMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        //查询图片
        public ActionResult GetStudentImage()
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                B_UserTable buser = (from tbuser in myModels.B_UserTable
                                     where tbuser.AccountID == accountID
                                     select tbuser).Single();
                try
                {
                    var userImg = (from tbuser in myModels.B_AuthenticationTable
                                   where tbuser.UserID == buser.UserID
                                   select new
                                   {
                                       tbuser.Diploma

                                   }).Single();
                    byte[] imageData = userImg.Diploma;
                    return File(imageData, @"image/jpg");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }
        //查看学历认证
        public ActionResult SelectEducationalBackground()
        {
            try
            {
                string str = "";
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                B_UserTable buser = (from tbuser in myModels.B_UserTable
                                     where tbuser.AccountID == accountID
                                     select tbuser).Single();

                try
                {
                    int bAuthen = (from tbauthen in myModels.B_AuthenticationTable
                                   where tbauthen.UserID == buser.UserID
                                   select new
                                   {
                                       tbauthen.UserID,
                                   }).Count();


                    if (bAuthen > 0)
                    {
                        var listAuthen = (from dbAuhen in myModels.B_AuthenticationTable
                                          join dbuser in myModels.B_UserTable on dbAuhen.UserID equals dbuser.UserID
                                          where dbAuhen.UserID == buser.UserID
                                          select new
                                          {
                                              UserID = dbAuhen.UserID,
                                              Institutions = dbAuhen.Institutions,
                                              Specialty = dbAuhen.Specialty,
                                              EducationalBackground = dbAuhen.EducationalBackground,
                                              EnrolTime = dbAuhen.EnrolTime.ToString(),
                                              GraduationTime = dbAuhen.GraduationTime.ToString(),
                                          }).Single();
                        str = "success";
                        return Json(listAuthen, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        str = "NoUp";
                        //return Json(str, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        //实名认证
        public ActionResult SelectShiMing()
        {

            try
            {
                string striD = "";
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var strvaruser = (from tbuser in myModels.B_UserTable
                                  select tbuser).ToList();
                for (int i = 0; i < strvaruser.Count; i++)
                {
                    if (strvaruser[i].AccountID == accountID)
                    {
                        if (strvaruser[i].StatusID == 2)
                        {
                            striD = "fileStatus";
                            break;
                        }
                        else
                        {
                            var varuser = (from tbuser in myModels.B_UserTable
                                           where tbuser.StatusID == 24 && tbuser.AccountID == accountID
                                           select tbuser).ToList();
                            if (strvaruser[i].StatusID == 24 && strvaruser[i].AccountID == accountID)
                            {

                                var list = (from tbuser in myModels.B_UserTable
                                            where tbuser.AccountID == accountID
                                            select new
                                            {
                                                UserName = tbuser.UserName,
                                                TrueName = tbuser.TrueName,
                                                Sex = tbuser.Sex,
                                                IDCardNo = tbuser.IDCardNo,
                                            }).Single();

                                return Json(list, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }
                    else
                    {
                        striD = "fileuser";

                    }

                }
                return Json(striD, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        //身份证绑定提交
        public ActionResult idshengzhengselect(B_UserTable userid)
        {
            try
            {
                string strMge = "";
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                int strvaruser = (from tbuser in myModels.B_UserTable
                                  where tbuser.AccountID == accountID
                                  select tbuser).Count();
                if (strvaruser > 0)
                {
                    B_UserTable varUser = (from tbuser in myModels.B_UserTable
                                           where tbuser.AccountID == accountID
                                           select tbuser).Single();
                    varUser.TrueName = userid.TrueName;
                    varUser.IDCardNo = userid.IDCardNo;
                    varUser.StatusID = 2;
                    myModels.Entry(varUser).State = System.Data.Entity.EntityState.Modified;
                    myModels.SaveChanges();
                    strMge = "success";
                }
                else
                {
                    strMge = "fileuser";
                }
                return Json(strMge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult IDCard(string cardNo)
        {
            string idcarno;
            idcarno = Common.IDCard.IDValidator(cardNo);
            return Json(idcarno, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IDCardtow(string cardNo)
        {
            string idcarno;
            idcarno = Common.IDCard.IDValidator2(cardNo);
            return Json(idcarno, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IDCardtheerr(string cardNo)
        {
            string idcarno;
            idcarno = Common.IDCard.IDValidator3(cardNo);
            return Json(idcarno, JsonRequestBehavior.AllowGet);
        }

        //手机认证
        public ActionResult SelectPhone()
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select new
                            {
                                PhoneNumber = tbuser.PhoneNumber,
                            }).Single();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //修改手机号码
        public ActionResult UpdatePhone(B_UserTable BUser, string extend_field5, string captcha)
        {
            string str = "";
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["mobilecode"];
                //判断验证码
                if (cookie != null)
                {
                    if (captcha == System.Web.HttpContext.Current.Server.UrlDecode(cookie["mobile_code"]))
                    {

                        int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                        var list = (from tbuser in myModels.B_UserTable
                                    where tbuser.AccountID == accountID
                                    select tbuser).Single();
                        list.PhoneNumber = extend_field5;
                        myModels.Entry(list).State = System.Data.Entity.EntityState.Modified;
                        myModels.SaveChanges();
                        str = "success";

                    }
                    else
                    {
                        str = "failCodeErro";//验证码不正确
                    }
                }
                else
                {
                    str = "ValidCodeErro";//验证码过期
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //视频认证
        public ActionResult InssertVideo(B_VideoauthenticationTable BVideo)
        {
            string str = "";
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                try
                {
                    int listT = (from tbVideo in myModels.B_VideoauthenticationTable
                                 join tbuser in myModels.B_UserTable on tbVideo.UserID equals tbuser.UserID
                                 where tbVideo.UserID == list.UserID
                                 select new
                                 {
                                     tbVideo.UserID,
                                 }).Count();

                    if (listT > 0)
                    {
                        str = "Exist";
                    }
                    else
                    {

                        BVideo.UserID = list.UserID;
                        BVideo.StatusID = 2;
                        BVideo.ApplicationDate = DateTime.Now;
                        myModels.B_VideoauthenticationTable.Add(BVideo);
                        myModels.SaveChanges();
                        str = "success";
                    }
                }
                catch (Exception e)
                {
                    str = "";
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 会员资料

        //注册会员页面
        public ActionResult RegisterMember()
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

        //会员资料页面
        public ActionResult MemberDataIndex()
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

        public ActionResult InsertVIP(B_VIPUserTable BVIP)
        {

            string str = "";
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                B_UserTable user = (from tbuser in myModels.B_UserTable
                                    where tbuser.AccountID == intAccountID
                                    select tbuser).Single();
                BVIP.UserID = user.UserID;
                BVIP.VipDeadline = Request.Form["VipDeadline"];
                if (BVIP.VipDeadline == "1")
                {
                    if (user.PropertyAmounts >= 10 && user.UsableMoney >= 10 && user.PropertyAmounts != null && user.UsableMoney != null)
                    {

                        try
                        {
                            int list = (from tbvip in myModels.B_VIPUserTable
                                        where tbvip.UserID == user.UserID
                                        select new
                                        {
                                            tbvip.UserID,
                                        }).Count();

                            if (list > 0)
                            {
                                str = "Exist";
                            }
                            else
                            {
                                BVIP.WhetherPayment = 10;
                                BVIP.StatusID = 2;
                                BVIP.UserTypeID = 4;
                                myModels.B_VIPUserTable.Add(BVIP);
                                myModels.SaveChanges();
                                str = "success";
                            }
                        }
                        catch (Exception e)
                        {
                            str = "VIpfail";
                        }
                    }
                    else
                    {
                        str = "NoMoney";
                    }

                }
                if (BVIP.VipDeadline == "3")
                {
                    if (user.PropertyAmounts >= 30 && user.UsableMoney >= 30 && user.PropertyAmounts != null && user.UsableMoney != null)
                    {

                        try
                        {
                            int list = (from tbvip in myModels.B_VIPUserTable
                                        where tbvip.UserID == user.UserID
                                        select new
                                        {
                                            tbvip.UserID,
                                        }).Count();

                            if (list > 0)
                            {
                                str = "Exist";
                            }
                            else
                            {
                                BVIP.WhetherPayment = 30;
                                BVIP.StatusID = 2;
                                BVIP.UserTypeID = 4;
                                myModels.B_VIPUserTable.Add(BVIP);
                                myModels.SaveChanges();
                                str = "success";
                            }
                        }
                        catch (Exception e)
                        {
                            str = "VIpfail";
                        }
                    }
                    else
                    {
                        str = "NoMoney";
                    }
                }
                if (BVIP.VipDeadline == "6")
                {
                    if (user.PropertyAmounts >= 60 && user.UsableMoney >= 60 && user.PropertyAmounts != null && user.UsableMoney != null)
                    {

                        try
                        {
                            int list = (from tbvip in myModels.B_VIPUserTable
                                        where tbvip.UserID == user.UserID
                                        select new
                                        {
                                            tbvip.UserID,
                                        }).Count();

                            if (list > 0)
                            {
                                str = "Exist";
                            }
                            else
                            {
                                BVIP.WhetherPayment = 60;
                                BVIP.StatusID = 2;
                                BVIP.UserTypeID = 4;
                                myModels.B_VIPUserTable.Add(BVIP);
                                myModels.SaveChanges();
                                str = "success";
                            }
                        }
                        catch (Exception e)
                        {
                            str = "VIpfail";
                        }
                    }
                    else
                    {
                        str = "NoMoney";
                    }
                }
                if (BVIP.VipDeadline == "12")
                {
                    if (user.PropertyAmounts >= 120 && user.UsableMoney >= 120 && user.PropertyAmounts != null && user.UsableMoney != null)
                    {

                        try
                        {
                            int list = (from tbvip in myModels.B_VIPUserTable
                                        where tbvip.UserID == user.UserID
                                        select new
                                        {
                                            tbvip.UserID,
                                        }).Count();

                            if (list > 0)
                            {
                                str = "Exist";
                            }
                            else
                            {
                                BVIP.WhetherPayment = 120;
                                BVIP.StatusID = 2;
                                BVIP.UserTypeID = 4;
                                myModels.B_VIPUserTable.Add(BVIP);
                                myModels.SaveChanges();
                                str = "success";
                            }
                        }
                        catch (Exception e)
                        {
                            str = "VIpfail";
                        }
                    }
                    else
                    {
                        str = "NoMoney";
                    }
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //绑定会员名称
        public ActionResult BindingName()
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == intAccountID
                            select new
                            {
                                tbuser.UserName,
                            }).Single();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //查看会员信息
        public ActionResult CheckMyVIP()
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var list = (from tbVip in myModels.B_VIPUserTable
                            join tbuser in myModels.B_UserTable on tbVip.UserID equals tbuser.UserID
                            where tbuser.AccountID == intAccountID
                            select new
                            {
                                UserName = tbuser.UserName,
                                StartTime = tbVip.StartTime.ToString(),
                                VipDeadline = tbVip.VipDeadline,
                                EndTime = tbVip.EndTime.ToString()
                            }).Single();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //续费会员
        public ActionResult UpdataVIp(B_VIPUserTable BVIP)
        {
            try
            {
                string str = "";
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                B_VIPUserTable bvip = (from tbvip in myModels.B_VIPUserTable
                                       join tbuser in myModels.B_UserTable on tbvip.UserID equals tbuser.UserID
                                       where tbuser.AccountID == intAccountID
                                       select tbvip).Single();

                if (BVIP.VipDeadline != null)
                {

                    if (bvip.StatusID != 2)
                    {

                        if (BVIP.VipDeadline.Trim() == "1")
                        {
                            decimal str1 = Convert.ToDecimal(bvip.VipDeadline);
                            decimal str2 = Convert.ToDecimal(BVIP.VipDeadline);
                            bvip.VipDeadline = Convert.ToString(str1 + str2);
                            bvip.WhetherPayment = bvip.WhetherPayment + 10;
                            bvip.EndTime = bvip.EndTime.Value.AddMonths(1);


                            var dbuser = (from tbuser in myModels.B_UserTable
                                          where tbuser.UserID == bvip.UserID
                                          select tbuser).Single();
                            if (dbuser.PropertyAmounts >= 10 && dbuser.UsableMoney >= 10 && dbuser.PropertyAmounts != null && dbuser.UsableMoney != null)
                            {
                                dbuser.PropertyAmounts = dbuser.PropertyAmounts - 10;
                                dbuser.UsableMoney = dbuser.UsableMoney - 10;
                                B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                                WebsiteExpenses.AccountID = intAccountID;
                                WebsiteExpenses.OperateTypeID = 9;
                                WebsiteExpenses.OperateMoney = Convert.ToDecimal(10);
                                WebsiteExpenses.Earning = Convert.ToDecimal(10);
                                WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                                WebsiteExpenses.Remark = "用户续费一个月," + "扣除" + "[" + dbuser.UserName.Trim() + "]10元会员费";
                                WebsiteExpenses.OperateTime = DateTime.Now;
                                myModels.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                                myModels.SaveChanges();

                                B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                                UserExpense.UserID = dbuser.UserID;
                                UserExpense.OperateTypeID = 9;
                                UserExpense.OperateMoney = Convert.ToDecimal(10);
                                UserExpense.Balance = dbuser.PropertyAmounts;
                                UserExpense.Earning = Convert.ToDecimal(0);
                                UserExpense.Expenses = Convert.ToDecimal(10);
                                UserExpense.Remark = "用户续费一个月," + "扣除" + "[" + dbuser.UserName.Trim() + "]10元会员费";
                                UserExpense.OperateTime = DateTime.Now;
                                myModels.B_UserExpensesTable.Add(UserExpense);
                                myModels.SaveChanges();

                                B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                                Capitalrecord.UserID = dbuser.UserID;
                                Capitalrecord.OperateTypeID = 9;
                                Capitalrecord.OpFare = Convert.ToDecimal(10);
                                Capitalrecord.Income = Convert.ToDecimal(0);
                                Capitalrecord.Expend = Convert.ToDecimal(10);
                                Capitalrecord.PropertyAmounts = dbuser.PropertyAmounts;
                                Capitalrecord.Remarks = "用户续费一个月," + "扣除" + "[" + dbuser.UserName.Trim() + "]10元会员费";
                                Capitalrecord.operatetime = DateTime.Now;
                                myModels.B_CapitalrecordTable.Add(Capitalrecord);
                                myModels.SaveChanges();

                                myModels.Entry(bvip).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();
                                str = "success";

                            }
                            else
                            {
                                str = "NoMoney";
                            }
                        }
                        if (BVIP.VipDeadline == "3")
                        {
                            decimal str1 = Convert.ToDecimal(bvip.VipDeadline);
                            decimal str2 = Convert.ToDecimal(BVIP.VipDeadline);
                            bvip.VipDeadline = Convert.ToString(str1 + str2);
                            bvip.WhetherPayment = bvip.WhetherPayment + 30;
                            bvip.EndTime = bvip.EndTime.Value.AddMonths(3);

                            var dbuser = (from tbuser in myModels.B_UserTable
                                          where tbuser.UserID == bvip.UserID
                                          select tbuser).Single();
                            if (dbuser.PropertyAmounts >= 30 && dbuser.UsableMoney >= 30 && dbuser.PropertyAmounts != null && dbuser.UsableMoney != null)
                            {
                                dbuser.PropertyAmounts = dbuser.PropertyAmounts - 30;
                                dbuser.UsableMoney = dbuser.UsableMoney - 30;
                                B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                                WebsiteExpenses.AccountID = intAccountID;
                                WebsiteExpenses.OperateTypeID = 9;
                                WebsiteExpenses.OperateMoney = Convert.ToDecimal(30);
                                WebsiteExpenses.Earning = Convert.ToDecimal(30);
                                WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                                WebsiteExpenses.Remark = "用户续费三个月," + "扣除" + "[" + dbuser.UserName.Trim() + "]30元会员费";
                                WebsiteExpenses.OperateTime = DateTime.Now;
                                myModels.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                                myModels.SaveChanges();

                                B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                                UserExpense.UserID = dbuser.UserID;
                                UserExpense.OperateTypeID = 9;
                                UserExpense.OperateMoney = Convert.ToDecimal(30);
                                UserExpense.Balance = dbuser.PropertyAmounts;
                                UserExpense.Earning = Convert.ToDecimal(0);
                                UserExpense.Expenses = Convert.ToDecimal(30);
                                UserExpense.Remark = "用户续费三个月," + "扣除" + "[" + dbuser.UserName.Trim() + "]30元会员费";
                                UserExpense.OperateTime = DateTime.Now;
                                myModels.B_UserExpensesTable.Add(UserExpense);
                                myModels.SaveChanges();

                                B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                                Capitalrecord.UserID = dbuser.UserID;
                                Capitalrecord.OperateTypeID = 9;
                                Capitalrecord.OpFare = Convert.ToDecimal(30);
                                Capitalrecord.Income = Convert.ToDecimal(0);
                                Capitalrecord.Expend = Convert.ToDecimal(30);
                                Capitalrecord.PropertyAmounts = dbuser.PropertyAmounts;
                                Capitalrecord.Remarks = "用户续费三个月," + "扣除" + "[" + dbuser.UserName.Trim() + "]30元会员费";
                                Capitalrecord.operatetime = DateTime.Now;
                                myModels.B_CapitalrecordTable.Add(Capitalrecord);
                                myModels.SaveChanges();

                                myModels.Entry(bvip).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();
                                str = "success";

                            }
                            else
                            {
                                str = "NoMoney";
                            }
                        }
                        if (BVIP.VipDeadline == "6")
                        {
                            decimal str1 = Convert.ToDecimal(bvip.VipDeadline);
                            decimal str2 = Convert.ToDecimal(BVIP.VipDeadline);
                            bvip.VipDeadline = Convert.ToString(str1 + str2);
                            bvip.WhetherPayment = bvip.WhetherPayment + 60;
                            bvip.EndTime = bvip.EndTime.Value.AddMonths(6);

                            var dbuser = (from tbuser in myModels.B_UserTable
                                          where tbuser.UserID == bvip.UserID
                                          select tbuser).Single();
                            if (dbuser.PropertyAmounts >= 60 && dbuser.UsableMoney >= 60 && dbuser.PropertyAmounts != null && dbuser.UsableMoney != null)
                            {
                                dbuser.PropertyAmounts = dbuser.PropertyAmounts - 60;
                                dbuser.UsableMoney = dbuser.UsableMoney - 60;
                                B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                                WebsiteExpenses.AccountID = intAccountID;
                                WebsiteExpenses.OperateTypeID = 9;
                                WebsiteExpenses.OperateMoney = Convert.ToDecimal(60);
                                WebsiteExpenses.Earning = Convert.ToDecimal(60);
                                WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                                WebsiteExpenses.Remark = "用户续费6个月" + "扣除" + "[" + dbuser.UserName.Trim() + "]60元会员费";
                                WebsiteExpenses.OperateTime = DateTime.Now;
                                myModels.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                                myModels.SaveChanges();

                                B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                                UserExpense.UserID = dbuser.UserID;
                                UserExpense.OperateTypeID = 9;
                                UserExpense.OperateMoney = Convert.ToDecimal(60);
                                UserExpense.Balance = dbuser.PropertyAmounts;
                                UserExpense.Earning = Convert.ToDecimal(0);
                                UserExpense.Expenses = Convert.ToDecimal(60);
                                UserExpense.Remark = "用户续费6个月" + "扣除" + "[" + dbuser.UserName.Trim() + "]60元会员费";
                                UserExpense.OperateTime = DateTime.Now;
                                myModels.B_UserExpensesTable.Add(UserExpense);
                                myModels.SaveChanges();

                                B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                                Capitalrecord.UserID = dbuser.UserID;
                                Capitalrecord.OperateTypeID = 9;
                                Capitalrecord.OpFare = Convert.ToDecimal(60);
                                Capitalrecord.Income = Convert.ToDecimal(0);
                                Capitalrecord.Expend = Convert.ToDecimal(60);
                                Capitalrecord.PropertyAmounts = dbuser.PropertyAmounts;
                                Capitalrecord.Remarks = "用户续费六个月," + "扣除" + "[" + dbuser.UserName.Trim() + "]60元会员费";
                                Capitalrecord.operatetime = DateTime.Now;
                                myModels.B_CapitalrecordTable.Add(Capitalrecord);
                                myModels.SaveChanges();

                                myModels.Entry(bvip).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();
                                str = "success";
                            }
                            else
                            {
                                str = "NoMoney";
                            }
                        }
                        if (BVIP.VipDeadline == "12")
                        {
                            decimal str1 = Convert.ToDecimal(bvip.VipDeadline);
                            decimal str2 = Convert.ToDecimal(BVIP.VipDeadline);
                            bvip.VipDeadline = Convert.ToString(str1 + str2);
                            bvip.WhetherPayment = bvip.WhetherPayment + 120;
                            bvip.EndTime = bvip.EndTime.Value.AddMonths(12);

                            var dbuser = (from tbuser in myModels.B_UserTable
                                          where tbuser.UserID == bvip.UserID
                                          select tbuser).Single();
                            if (dbuser.PropertyAmounts >= 120 && dbuser.UsableMoney >= 120 && dbuser.PropertyAmounts != null && dbuser.UsableMoney != null)
                            {
                                dbuser.PropertyAmounts = dbuser.PropertyAmounts - 120;
                                dbuser.UsableMoney = dbuser.UsableMoney - 120;
                                B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                                WebsiteExpenses.AccountID = intAccountID;
                                WebsiteExpenses.OperateTypeID = 9;
                                WebsiteExpenses.OperateMoney = Convert.ToDecimal(120);
                                WebsiteExpenses.Earning = Convert.ToDecimal(120);
                                WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                                WebsiteExpenses.Remark = "用户续费一年" + "扣除" + "[" + dbuser.UserName.Trim() + "]120元会员费";
                                WebsiteExpenses.OperateTime = DateTime.Now;
                                myModels.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                                myModels.SaveChanges();

                                B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                                UserExpense.UserID = dbuser.UserID;
                                UserExpense.OperateTypeID = 9;
                                UserExpense.OperateMoney = Convert.ToDecimal(120);
                                UserExpense.Balance = dbuser.PropertyAmounts;
                                UserExpense.Earning = Convert.ToDecimal(0);
                                UserExpense.Expenses = Convert.ToDecimal(120);
                                UserExpense.Remark = "用户续费一年" + "扣除" + "[" + dbuser.UserName.Trim() + "]120元会员费";
                                UserExpense.OperateTime = DateTime.Now;
                                myModels.B_UserExpensesTable.Add(UserExpense);
                                myModels.SaveChanges();

                                B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                                Capitalrecord.UserID = dbuser.UserID;
                                Capitalrecord.OperateTypeID = 9;
                                Capitalrecord.OpFare = Convert.ToDecimal(120);
                                Capitalrecord.Income = Convert.ToDecimal(0);
                                Capitalrecord.Expend = Convert.ToDecimal(120);
                                Capitalrecord.PropertyAmounts = dbuser.PropertyAmounts;
                                Capitalrecord.Remarks = "用户续费一年," + "扣除" + "[" + dbuser.UserName.Trim() + "]120元会员费";
                                Capitalrecord.operatetime = DateTime.Now;
                                myModels.B_CapitalrecordTable.Add(Capitalrecord);
                                myModels.SaveChanges();

                                myModels.Entry(bvip).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();
                                str = "success";
                            }
                            else
                            {
                                str = "NoMoney";
                            }
                        }
                        //myModels.Entry(bvip).State = System.Data.Entity.EntityState.Modified;
                        //myModels.SaveChanges();
                        //str = "success";
                    }
                    else
                    {
                        str = "Noshenhe";
                    }

                }
                else
                {
                    str = "fail";
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #endregion

        #region 我的资产

        //账户中心页面
        public ActionResult AccountCentre()
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
        //查询账户金额情况
        public ActionResult CheckAccount()
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select new
                            {
                                PropertyAmounts = tbuser.PropertyAmounts.ToString(),
                                UsableMoney = tbuser.UsableMoney.ToString(),
                                FreezeMoney = tbuser.FreezeMoney.ToString(),
                            }).Single();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        #region 额度申请
        //额度申请页面
        public ActionResult LimitIndex()
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
        //申请类型下拉款
        public ActionResult SelectLimitType()
        {
            try
            {
                var listType = from tbType in myModels.S_LimitTypeTable
                               select new
                               {
                                   id = tbType.LimitTypeID,
                                   name = tbType.LimitType
                               };
                return Json(listType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null,JsonRequestBehavior.AllowGet);
            }
        }

        //申请新增
        public ActionResult InsertLimit(B_LimitApplicationTable B_Limit)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                B_UserTable user = (from tbuser in myModels.B_UserTable
                                    where tbuser.AccountID == intAccountID
                                    select tbuser).Single();
                B_Limit.UserID = user.UserID;
                B_Limit.LimitTypeID = Convert.ToInt32(Request.Form["ApplyType"]);
                B_Limit.ApplicationMoney = Convert.ToDecimal(Request.Form["ApplyLimit"]);
                B_Limit.LoanPurpose = Request.Form["ApplyPurpose"];
                //B_Limit.ApplicationTime =Convert.ToDateTime(Request.Form["ApplyTime"]);
                B_Limit.ApplicationRemark = Request.Form["Remark"];
                B_Limit.ApplicationTime = DateTime.Now;

                if (user.Integral < 60)
                {
                    return Json("NoIntegral", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (user.Integral >= 60 && user.Integral < 70)//60-70积分
                    {
                        if (B_Limit.LimitTypeID != null && B_Limit.ApplicationMoney <= 5000)
                        {
                            B_Limit.StatusID = 2;
                            B_Limit.limitkindID = 1;
                            B_Limit.limitoperateID = 1;
                            B_Limit.PassLimit = Convert.ToDecimal(0);
                            myModels.B_LimitApplicationTable.Add(B_Limit);
                            myModels.SaveChanges();
                            return Json("success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("70Money", JsonRequestBehavior.AllowGet);//否则不
                        }

                    }
                    else
                    {
                        if (user.Integral >= 70 && user.Integral < 80)
                        {
                            if (B_Limit.LimitTypeID != null && B_Limit.ApplicationMoney  <= 20000)
                            {
                                B_Limit.StatusID = 2;
                                B_Limit.limitkindID = 1;
                                B_Limit.limitoperateID = 1;
                                B_Limit.PassLimit = Convert.ToDecimal(0);
                                myModels.B_LimitApplicationTable.Add(B_Limit);
                                myModels.SaveChanges();
                                return Json("success1", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json("80Money", JsonRequestBehavior.AllowGet);//否则不
                            }
                        }
                        else
                        {
                            if (user.Integral >= 80 && user.Integral < 90)
                            {
                                if (B_Limit.LimitTypeID != null && B_Limit.ApplicationMoney <= 50000)
                                {
                                    B_Limit.StatusID = 2;
                                    B_Limit.limitkindID = 1;
                                    B_Limit.limitoperateID = 1;
                                    B_Limit.PassLimit = Convert.ToDecimal(0);
                                    myModels.B_LimitApplicationTable.Add(B_Limit);
                                    myModels.SaveChanges();
                                    return Json("success2", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json("90Money", JsonRequestBehavior.AllowGet);//否则不
                                }
                            }
                            else
                            {
                                if (user.Integral >= 90 && user.Integral < 100)
                                {
                                    if (B_Limit.LimitTypeID != null && B_Limit.ApplicationMoney <= 100000)
                                    {
                                        B_Limit.StatusID = 2;
                                        B_Limit.limitkindID = 1;
                                        B_Limit.limitoperateID = 1;
                                        B_Limit.PassLimit = Convert.ToDecimal(0);
                                        myModels.B_LimitApplicationTable.Add(B_Limit);
                                        myModels.SaveChanges();
                                        return Json("success3", JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        return Json("100Money", JsonRequestBehavior.AllowGet);//否则不
                                    }
                                }
                                else
                                {
                                    if (user.Integral >= 100)
                                    {
                                        if (B_Limit.LimitTypeID != null && B_Limit.ApplicationMoney <= 200000)
                                        {
                                            B_Limit.StatusID = 2;
                                            B_Limit.limitkindID = 1;
                                            B_Limit.limitoperateID = 1;
                                            B_Limit.PassLimit = Convert.ToDecimal(0);
                                            myModels.B_LimitApplicationTable.Add(B_Limit);
                                            myModels.SaveChanges();
                                            return Json("success4", JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json("highMoney", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        //申请记录的下拉框
        public ActionResult SelectApplyType()
        {

            List<SelectVo> listType = new List<SelectVo>();
            SelectVo selectvo = new SelectVo
            {
                id = 0,
                text = "--请选择--"
            };
            listType.Add(selectvo);
            List<SelectVo> listTypeS = (from tbType in myModels.S_LimitTypeTable
                                        select new SelectVo
                                        {
                                            id = tbType.LimitTypeID,
                                            text = tbType.LimitType
                                        }).ToList();
            listType.AddRange(listTypeS);
            return Json(listType, JsonRequestBehavior.AllowGet);
        }
        //查询申请记录
        public ActionResult SelectLimitRecord(BsgridPage bsgridPage, string loanPurpose, int limitTypeID)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var listLimit = from tblimt in myModels.B_LimitApplicationTable
                                join tblimType in myModels.S_LimitTypeTable on tblimt.LimitTypeID equals tblimType.LimitTypeID
                                join tbstaute in myModels.S_StatusTable on tblimt.StatusID equals tbstaute.StatusID
                                join tbuser in myModels.B_UserTable on tblimt.UserID equals tbuser.UserID
                                where tbuser.AccountID == accountID
                                select new LimitVo
                                {
                                    LimitApplicationID = tblimt.LimitApplicationID,
                                    LimitTypeID = tblimt.LimitTypeID,
                                    StatusID = tblimt.StatusID,
                                    LimitType = tblimType.LimitType,
                                    ApplicationMoney = tblimt.ApplicationMoney,
                                    LoanPurpose = tblimt.LoanPurpose,
                                    ApplicationTime = tblimt.ApplicationTime,
                                    ApplicationRemark = tblimt.ApplicationRemark,
                                    StatusName = tbstaute.StatusName,
                                    StartTime = tblimt.ApplicationTime.ToString(),
                                };
                //借款通途不能为空
                if (!string.IsNullOrEmpty(loanPurpose))
                {
                    listLimit = listLimit.Where(n => n.LoanPurpose.Contains(loanPurpose));
                }

                //申请类型不能为空
                if (limitTypeID > 0)
                {
                    listLimit = listLimit.Where(p => p.LimitTypeID == limitTypeID);
                }

                int totalRow = listLimit.Count();
                List<Vo.LimitVo> limits = listLimit.OrderByDescending(p => p.LimitApplicationID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Vo.Bsgrid<Vo.LimitVo> bsgrid = new Bsgrid<LimitVo>();
                bsgrid.success = true;
                bsgrid.totalRows = totalRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = limits;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region 银行卡
        //添加银行卡页面
        public ActionResult CreditCardIndex()
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

        //银行名称
        public ActionResult SelectBankName()
        {
            try
            {
                List<SelectVo> listType = new List<SelectVo>();
                SelectVo selectvo = new SelectVo
                {
                    id = 0,
                    text = "--请选择--"
                };
                listType.Add(selectvo);
                List<SelectVo> listTypeS = (from tbType in myModels.S_PayTypeTable
                                            select new SelectVo
                                            {
                                                id = tbType.PayTypeID,
                                                text = tbType.PayTypeName,
                                            }).ToList();
                listType.AddRange(listTypeS);
                return Json(listType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        //城市
        public ActionResult SelectCity(int proID)
        {
            try
            {
                List<SelectVo> listTypeS = (from tbType in myModels.T_City
                                            where tbType.ProID == proID
                                            select new SelectVo
                                            {
                                                id = tbType.CityID,
                                                text = tbType.CityName,
                                            }).ToList();

                return Json(listTypeS, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        //省
        public ActionResult SelectProvince()
        {
            try
            {
                List<SelectVo> listType = new List<SelectVo>();
                SelectVo selectvo = new SelectVo
                {
                    id = 0,
                    text = "--请选择--"
                };
                listType.Add(selectvo);
                List<SelectVo> listTypeS = (from tbType in myModels.T_Province
                                            select new SelectVo
                                            {
                                                id = tbType.ProID,
                                                text = tbType.ProName,
                                            }).ToList();
                listType.AddRange(listTypeS);
                return Json(listType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        //添加银行卡保存
        public ActionResult InsertAccount(B_UserTable Buser)
        {
            try
            {
                string str = "";
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                B_UserTable user = (from tbuser in myModels.B_UserTable
                                    where tbuser.AccountID == accountID
                                    select tbuser).Single();
                string strpassword = AESEncryptHelper.AESEncrypt(Buser.PayPassword);
                if (user.BankAccount == null)
                {
                    if (Buser.BankAccount.Length == 18)
                    {
                        if (Buser.PayTypeID != null && Buser.ProID != null && Buser.CityID != null)
                        {
                            user.BankAccount = Buser.BankAccount;
                            user.PayPassword = strpassword;
                            user.PayTypeID = Buser.PayTypeID;
                            user.ProID = Buser.ProID;
                            user.Subbranch = Buser.Subbranch;
                            user.CityID = Buser.CityID;
                            myModels.Entry(user).State = System.Data.Entity.EntityState.Modified;
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
                        str = "BankAccountFail";
                    }

                }
                else
                {
                    B_UserTable buser = (from tbuser in myModels.B_UserTable
                                         where tbuser.AccountID == accountID
                                         select tbuser).Single();
                    buser.PayPassword = user.PayPassword;
                    myModels.Entry(buser).State = System.Data.Entity.EntityState.Modified;
                    myModels.SaveChanges();
                    str = "Eixst";
                }

                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        //银行卡信息页面
        public ActionResult CreditCardMessage()
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

        //查询银行信息
        public ActionResult SelectBankCar()
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            join tbType in myModels.S_PayTypeTable on tbuser.PayTypeID equals tbType.PayTypeID
                            where tbuser.AccountID == accountID
                            select new
                            {
                                BankAccount = tbuser.BankAccount.ToString(),
                                Subbranch = tbuser.Subbranch.ToString(),
                                TrueName = tbuser.TrueName.ToString(),
                                PropertyAmounts = tbuser.PropertyAmounts.ToString(),
                                UsableMoney = tbuser.UsableMoney.ToString(),
                                FreezeMoney = tbuser.FreezeMoney.ToString(),
                                PayTypeID = tbuser.PayTypeID.ToString(),
                                UserID = tbuser.UserID.ToString(),
                                PayTypeName = tbType.PayTypeName.ToString(),
                            }).Single();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region 充值 提现
        //充值 提现页面
        public ActionResult RechargeIndex()
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

        //充值类型
        public ActionResult RechargeType()
        {
            List<SelectVo> listType = new List<SelectVo>();
            SelectVo selectvo = new SelectVo
            {
                id = 0,
                text = "--请选择--"
            };
            listType.Add(selectvo);
            List<SelectVo> listTypeS = (from tbType in myModels.S_RechargeTypeTable
                                        select new SelectVo
                                        {
                                            id = tbType.RechargeTypeID,
                                            text = tbType.RechargeTypeName,
                                        }).ToList();
            listType.AddRange(listTypeS);
            return Json(listType, JsonRequestBehavior.AllowGet);
        }
        //充值账号的银行名
        public ActionResult PayType()
        {
            List<SelectVo> listType = new List<SelectVo>();
            SelectVo selectvo = new SelectVo
            {
                id = 0,
                text = "--请选择--"
            };
            listType.Add(selectvo);
            List<SelectVo> listTypeS = (from tbType in myModels.S_PayTypeTable
                                        select new SelectVo
                                        {
                                            id = tbType.PayTypeID,
                                            text = tbType.PayTypeName,
                                        }).ToList();
            listType.AddRange(listTypeS);
            return Json(listType, JsonRequestBehavior.AllowGet);
        }
        //绑定账户原有的金额
        public ActionResult SelectMoney(B_UserTable Buser)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from rbuser in myModels.B_UserTable
                            where rbuser.AccountID == accountID
                            select new
                            {
                                rbuser.PropertyAmounts
                            }).Single();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        //新增充值
        public ActionResult InsertRecharge(B_UserRechargeRecordTable B_Recharge)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                B_UserTable Buser = (from tbuser in myModels.B_UserTable
                                     where tbuser.AccountID == accountID
                                     select tbuser).Single();
                B_Recharge.UserID = Buser.UserID;



                string strCurrentCode = "";//定义当前编码的字符串
                var listDep = (from tbDmp in myModels.B_UserRechargeRecordTable
                               orderby tbDmp.TradeNumber
                               select tbDmp).ToList();
                if (listDep.Count > 0)//判断listDep中是否有数据
                {
                    //获取数据的条数
                    int count = listDep.Count;

                    B_UserRechargeRecordTable modelDep = listDep[count - 1];
                    // Models.Employee_Department modelDep = listDep[count-1]; //不需要添加引用
                    //获取最后一个交易号
                    int intCode = Convert.ToInt32(modelDep.TradeNumber.Substring(8, 5));//Substring(1,4)截取字符串,1代表字符串起始位置，4表示截取长度
                                                                                        //最后一个科室编码+1
                    intCode++;
                    //对新的交易号格式化
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
                B_Recharge.TradeNumber = strCurrentCode;//交易号
                string pw = cpIP.GetIP();
                B_Recharge.operateIP = pw;//获取IP地址
                B_Recharge.RechargeTypeID = Convert.ToInt32(Request.Form["RechargeTypeName"].ToString());//充值方式
                B_Recharge.PayTypeID = Convert.ToInt32(Request.Form["PayTypeName"].ToString());//充值类型
                B_Recharge.RechargeMoney = Convert.ToDecimal(Request.Form["RechargeMoney"].ToString());//充值金额
                B_Recharge.OperateTime = DateTime.Now;//充值时间
                B_Recharge.RechargePoundage = Request.Form["RechargePoundage"];//手续费
                B_Recharge.RealityAccountMoney = Convert.ToDecimal(Request.Form["RealityAccountMoney"]);//到账金额
                if (B_Recharge.RechargeTypeID != null && B_Recharge.PayTypeID != null
                    && B_Recharge.RechargeMoney != null)
                {
                    B_Recharge.StatusID = 3;
                    myModels.B_UserRechargeRecordTable.Add(B_Recharge);
                    myModels.SaveChanges();
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        //查询充值记录
        public ActionResult SelectRechargeRcord(BsgridPage bsgridPage, int payTypeID, int rechargeTypeID)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = from tbrecord in myModels.B_UserRechargeRecordTable
                           join tbRType in myModels.S_RechargeTypeTable on tbrecord.RechargeTypeID equals tbRType.RechargeTypeID
                           join tbPType in myModels.S_PayTypeTable on tbrecord.PayTypeID equals tbPType.PayTypeID
                           join tbStaut in myModels.S_StatusTable on tbrecord.StatusID equals tbStaut.StatusID
                           join tbuser in myModels.B_UserTable on tbrecord.UserID equals tbuser.UserID
                           where tbuser.AccountID == accountID
                           select new RechargeVo
                           {
                               RechargeRecordID = tbrecord.RechargeRecordID,
                               RechargeTypeID = tbrecord.RechargeTypeID,
                               StatusID = tbrecord.StatusID,
                               PayTypeID = tbrecord.PayTypeID,
                               TradeNumber = tbrecord.TradeNumber,
                               PayTypeName = tbPType.PayTypeName,
                               RechargeTypeName = tbRType.RechargeTypeName,
                               RechargeMoney = tbrecord.RechargeMoney,
                               OperateTime = tbrecord.OperateTime,
                               StatusName = tbStaut.StatusName,
                               StartTime = tbrecord.OperateTime.ToString(),
                           };
                //申请类型不能为空
                if (payTypeID > 0)
                {
                    list = list.Where(p => p.PayTypeID == payTypeID);
                }
                if (rechargeTypeID > 0)
                {
                    list = list.Where(p => p.RechargeTypeID == rechargeTypeID);
                }

                int totalRow = list.Count();
                List<Vo.RechargeVo> listts = list.OrderByDescending(p => p.RechargeRecordID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Vo.Bsgrid<Vo.RechargeVo> bsgrid = new Bsgrid<RechargeVo>();
                bsgrid.success = true;
                bsgrid.totalRows = totalRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = listts;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //市
        public ActionResult SelectCashCity(int proID)
        {
            List<SelectVo> listTypeS = (from tbType in myModels.T_City
                                        where tbType.ProID == proID
                                        select new SelectVo
                                        {
                                            id = tbType.CityID,
                                            text = tbType.CityName,
                                        }).ToList();

            return Json(listTypeS, JsonRequestBehavior.AllowGet);
        }
        //省
        public ActionResult SelectCashProvince()
        {
            List<SelectVo> listType = new List<SelectVo>();
            SelectVo selectvo = new SelectVo
            {
                id = 0,
                text = "--请选择--"
            };
            listType.Add(selectvo);
            List<SelectVo> listTypeS = (from tbType in myModels.T_Province
                                        select new SelectVo
                                        {
                                            id = tbType.ProID,
                                            text = tbType.ProName,
                                        }).ToList();
            listType.AddRange(listTypeS);
            return Json(listType, JsonRequestBehavior.AllowGet);
        }

        //绑定账户的真实姓名
        public ActionResult SelectTrueName()
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select new
                            {
                                tbuser.TrueName,
                            }).Single();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        //新增提现
        public ActionResult InsertCash(B_UserCashRecordTable BCash)
        {
            try
            {
                string str = "";
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                B_UserTable Buser = (from tbuser in myModels.B_UserTable
                                     where tbuser.AccountID == accountID
                                     select tbuser).Single();
                BCash.UserID = Buser.UserID;
                BCash.CashTime = DateTime.Now;
                string pw = cpIP.GetIP();
                BCash.IP = pw;
                if (BCash.CashAccountNumber.Length != 18)
                {
                    str = "lose";
                }
                else
                {
                    if (BCash.CashAmount <= Buser.PropertyAmounts)
                    {
                        if (BCash.PayTypeID != null && BCash.CashAccountNumber != null)
                        {
                            BCash.StatusID = 3;
                            myModels.B_UserCashRecordTable.Add(BCash);
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
                        str = "Nocount";
                    }
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        //提现记录
        public ActionResult SelectCashRecord(BsgridPage bsgridPage, int payTypeID)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = from tbcash in myModels.B_UserCashRecordTable
                           join tbCType in myModels.S_PayTypeTable on tbcash.PayTypeID equals tbCType.PayTypeID
                           join tbStuts in myModels.S_StatusTable on tbcash.StatusID equals tbStuts.StatusID
                           join tbUser in myModels.B_UserTable on tbcash.UserID equals tbUser.UserID
                           where tbUser.AccountID == accountID
                           select new CashVo
                           {
                               UserCashID = tbcash.UserCashID,
                               PayTypeID = tbcash.PayTypeID,
                               StatusID = tbcash.StatusID,
                               CashAccountNumber = tbcash.CashAccountNumber,
                               CashAmount = tbcash.CashAmount,
                               AccountMoney = tbcash.AccountMoney,
                               Poundage = tbcash.Poundage,
                               CashTime = tbcash.CashTime,
                               StartTime = tbcash.CashTime.ToString(),
                               PayTypeName = tbCType.PayTypeName,
                               StatusName = tbStuts.StatusName,
                           };
                if (payTypeID > 0)
                {
                    list = list.Where(p => p.PayTypeID == payTypeID);
                }

                int IntTotalRow = list.Count();
                List<CashVo> lists = list.OrderByDescending(p => p.UserCashID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<CashVo> bsgrid = new Bsgrid<CashVo>();
                bsgrid.totalRows = IntTotalRow;
                bsgrid.success = true;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = lists;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region 我的交易

        #region 我的投标

        //投标管理页面
        public ActionResult MyAccountIndex1()
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

        //绑定标种
        public ActionResult SelectTreetoptype()
        {
            try
            {
                List<SelectVo> listType = new List<SelectVo>();
                SelectVo selectvo = new SelectVo
                {
                    id = 0,
                    text = "--请选择--"
                };
                listType.Add(selectvo);
                List<SelectVo> listTypeS = (from tbType in myModels.S_Treetoptypetalbe
                                            select new SelectVo
                                            {
                                                id = tbType.TreetoptypeID,
                                                text = tbType.Treetoptype
                                            }).ToList();
                listType.AddRange(listTypeS);
                return Json(listType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        //查询投资记录
        public ActionResult SelectInvestRecord(BsgridPage bsgridPage, int treetoptypeID)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();

                var list = (from tbInvest in myModels.B_InvestTable
                            join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                            join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                            join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                            join tbstate in myModels.S_StatusTable on tbInvest.StatusID equals tbstate.StatusID
                            where tbInvest.UserID == listS.UserID && tbloan.TreetoptypeID != 7
                            select new InvestVo
                            {
                                InvestID = tbInvest.InvestID,
                                UserID = tbloan.UserID,
                                LoanID = tbInvest.LoanID,
                                TreetoptypeID = tbloan.TreetoptypeID,
                                InvestMoney = tbInvest.InvestMoney,
                                LoanPurpose = tbloan.LoanPurpose,
                                InvestTime = tbInvest.InvestTime,
                                StartTime = tbInvest.InvestTime.ToString(),
                                UserName = tbuser.UserName,
                                StatusName = tbstate.StatusName,
                                TreetopType = tbtree.Treetoptype
                            });
                if (treetoptypeID > 0)
                {
                    list = list.Where(p => p.TreetoptypeID == treetoptypeID);
                }
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
        public ActionResult CountInvestMoney(BsgridPage bsgridPage)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                try
                {
                    var list = (from tbInvest in myModels.B_InvestTable
                                join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                                where tbInvest.UserID == listS.UserID && tbloan.TreetoptypeID != 7
                                select new InvestVo
                                {
                                    InvestMoney = tbInvest.InvestMoney,
                                }).ToList();
                    decimal ZongE = 0;
                    decimal ZE = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        ZongE = Convert.ToDecimal(list[i].InvestMoney);
                        ZE += ZongE;
                    }
                    return Json(ZE, JsonRequestBehavior.AllowGet);
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
        //查询流转标投资记录
        public ActionResult SelectChangeInvest(BsgridPage bsgridPage)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var list = (from tbInvest in myModels.B_InvestTable
                            join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                            join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                            join tbstate in myModels.S_StatusTable on tbInvest.StatusID equals tbstate.StatusID
                            where tbInvest.UserID == listS.UserID && tbloan.TreetoptypeID == 7
                            select new InvestVo
                            {
                                InvestID = tbInvest.InvestID,
                                UserID = tbloan.UserID,
                                InvestMoney = tbInvest.InvestMoney,
                                LoanPurpose = tbloan.LoanPurpose,
                                InvestTime = tbInvest.InvestTime,
                                StartTime = tbInvest.InvestTime.ToString(),
                                UserName = tbuser.UserName,
                                StatusName = tbstate.StatusName,
                                LoanTreetop = tbInvest.LoanTreetop,
                            });
                int TotaRow = list.Count();
                List<InvestVo> listT = list.OrderByDescending(p => p.InvestID)
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

        //查询流转投资资金
        public ActionResult CountInvestCMoney()
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                try
                {
                    var list = (from tbInvest in myModels.B_InvestTable
                                join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                                where tbInvest.UserID == listS.UserID && tbloan.TreetoptypeID == 7
                                select new InvestVo
                                {
                                    InvestMoney = tbInvest.InvestMoney,
                                }).ToList();
                    decimal ZongE = 0;
                    decimal ZE = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        ZongE = Convert.ToDecimal(list[i].InvestMoney);
                        ZE += ZongE;
                    }
                    return Json(ZE, JsonRequestBehavior.AllowGet);
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

        //查询投标中的借款
        public ActionResult SelectNowInvest(BsgridPage bsgridPage, int treetoptypeID)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var list = (from tbInvest in myModels.B_InvestTable
                            join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                            join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                            join tbdelin in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbdelin.LoanDeadlineID
                            join tbstatu in myModels.S_StatusTable on tbloan.StatusID equals tbstatu.StatusID
                            join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                            where tbInvest.UserID == listS.UserID && tbloan.Scheduleinvestment != 100 && tbloan.StatusID == 7 && tbInvest.LoanID == tbloan.LoanID
                            select new InvestVo
                            {
                                InvestID = tbInvest.InvestID,
                                LoanID = tbInvest.LoanID,
                                TreetoptypeID = tbloan.TreetoptypeID,
                                LoanDeadlineID = tbloan.LoanDeadlineID,
                                UserID = tbloan.UserID,
                                StatusID = tbloan.StatusID,
                                UserName = tbuser.UserName,
                                LoanPurpose = tbloan.LoanPurpose,
                                TreetopType = tbtree.Treetoptype,
                                Rate = tbloan.Rate,
                                InvestMoney = tbInvest.InvestMoney,
                                Scheduleinvestment = tbloan.Scheduleinvestment,
                                LoanDeadlineName = tbdelin.LoanDeadlineName,
                                SubmitTime = tbloan.SubmitTime,
                                StartTime = tbloan.SubmitTime.ToString(),
                                StatusName = tbstatu.StatusName,
                            });

                int TotaRow = list.Count();
                List<InvestVo> listT = list.OrderByDescending(p => p.InvestID)
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
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 借款管理
        //借款管理页面
        public ActionResult LoanManageIndex()
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

        //查询全部借款
        public ActionResult SelectAllLoanBiao(BsgridPage bsgridPage, int treetoptypeID)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var loanlist = (from tbloan in myModels.B_LoanTable
                                join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                                join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                                join tbRepey in myModels.S_RepaymentWayTable on tbloan.RepaymentWayID equals tbRepey.RepaymentWayID
                                join tbline in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbline.LoanDeadlineID
                                join tbstatc in myModels.S_StatusTable on tbloan.StatusID equals tbstatc.StatusID
                                where tbloan.UserID == listS.UserID && (tbloan.StatusID == 7 || tbloan.StatusID == 1)
                                //tbloan.Scheduleinvestment != 100 &&
                                select new LoanVo
                                {
                                    LoanID = tbloan.LoanID,
                                    UserID = tbloan.UserID,
                                    TreetoptypeID = tbloan.TreetoptypeID,
                                    RepaymentWayID = tbloan.RepaymentWayID,
                                    LoanDeadlineID = tbloan.LoanDeadlineID,
                                    LoanPurpose = tbloan.LoanPurpose,
                                    Treetoptype = tbtree.Treetoptype,
                                    RepaymentWayName = tbRepey.RepaymentWayName,
                                    LoanMoney = tbloan.LoanMoney,
                                    Rate = tbloan.Rate,
                                    SurplusLoan = tbloan.SurplusLoan,
                                    AlreadyLoan = tbloan.AlreadyLoan,
                                    LoanDeadlineName = tbline.LoanDeadlineName,
                                    StatusName = tbstatc.StatusName,
                                    SubmitTime = tbloan.SubmitTime,
                                    StartTime = tbloan.SubmitTime.ToString()
                                });
                if (treetoptypeID > 0)
                {
                    loanlist = loanlist.Where(p => p.TreetoptypeID == treetoptypeID);
                }
                int TotaRow = loanlist.Count();
                List<LoanVo> ListT = loanlist.OrderBy(p => p.LoanID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<LoanVo> bsgrid = new Bsgrid<LoanVo>();
                bsgrid.totalRows = TotaRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.success = true;
                bsgrid.data = ListT;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //撤标页面
        public ActionResult CheBiaoIndex()
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
        //传递借款信息byID
        public ActionResult SelectLoan(int loanID)
        {
            LoanVo list = (from tbloan in myModels.B_LoanTable
                           join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                           where tbloan.LoanID == loanID
                           select new LoanVo
                           {
                               LoanID = tbloan.LoanID,
                               TreetoptypeID = tbloan.TreetoptypeID,
                               Loantitle = tbloan.Loantitle,
                               LoanPurpose = tbloan.LoanPurpose,
                               LoanMoney = tbloan.LoanMoney,
                               SurplusLoan = tbloan.SurplusLoan,
                               Rate = tbloan.Rate
                           }).Single();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //撤销借款标保存
        public ActionResult RevokeLoan(B_LoanTable B_loan)
        {
            string str = "";
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (B_loan.LoanID < 0)
                        {
                            str = "fail";
                        }
                        else
                        {
                            B_LoanTable Bloan = (from tbloan in myModels.B_LoanTable
                                                 where tbloan.LoanID == B_loan.LoanID
                                                 select tbloan).Single();
                            Bloan.StatusID = 27;
                            Bloan.RemoveTreetopTime = DateTime.Now;
                            myModels.Entry(Bloan).State = System.Data.Entity.EntityState.Modified;
                            myModels.SaveChanges();

                            try
                            {
                                var BInvest = (from tbInvest in myModels.B_InvestTable
                                               where tbInvest.LoanID == B_loan.LoanID
                                               select tbInvest).ToList();
                                for (int i = 0; i < BInvest.Count; i++)
                                {
                                    int varloanID = Convert.ToInt32(BInvest[i].InvestID);
                                    var bInvest = (from tbInvest in myModels.B_InvestTable
                                                   where tbInvest.InvestID == varloanID
                                                   select tbInvest).Single();
                                    bInvest.StatusID = 27;
                                    bInvest.NotRetrievePrincipal = Convert.ToDecimal(0.00);
                                    bInvest.CountermandPrincipal = bInvest.ReceivablePrincipal;
                                    bInvest.ReceivableInterest = bInvest.ReceivableInterest;
                                    myModels.Entry(bInvest).State = System.Data.Entity.EntityState.Modified;
                                    myModels.SaveChanges();

                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                            try
                            {
                                var B_Invest = (from tbInvest in myModels.B_InvestTable
                                                where tbInvest.LoanID == B_loan.LoanID
                                                select tbInvest).ToList();

                                for (int i = 0; i < B_Invest.Count; i++)
                                {


                                    int userID = Convert.ToInt32(B_Invest[i].UserID);
                                    B_UserTable BUser = (from tbuser in myModels.B_UserTable
                                                         where tbuser.UserID == userID
                                                         select tbuser).Single();
                                    BUser.PropertyAmounts = BUser.PropertyAmounts + B_Invest[i].InvestMoney;
                                    BUser.UsableMoney = BUser.UsableMoney + B_Invest[i].InvestMoney;
                                    myModels.Entry(BUser).State = System.Data.Entity.EntityState.Modified;
                                    myModels.SaveChanges();


                                }

                                str = "success";
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }

                    }
                    else
                    {
                        str = "validCodeFail";
                    }
                }

                //return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        //还款页面
        public ActionResult RepaymentIndex()
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

        //查询满标的借款标
        public ActionResult SelectManLoan(BsgridPage bsgridPage, int treetoptypeID)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                var Maoloan = (from tbloan in myModels.B_LoanTable
                               join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                               join tbline in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbline.LoanDeadlineID
                               where tbloan.UserID == list.UserID && tbloan.RepayPrincipal != 0
                               && (tbloan.StatusID == 10 || tbloan.StatusID == 11 || tbloan.StatusID == 8 || tbloan.StatusID == 34)
                               select new LoanVo
                               {
                                   LoanID = tbloan.LoanID,
                                   TreetoptypeID = tbloan.TreetoptypeID,
                                   LoanDeadlineID = tbloan.LoanDeadlineID,
                                   LoanPurpose = tbloan.LoanPurpose,
                                   Treetoptype = tbtree.Treetoptype,
                                   LoanMoney = tbloan.LoanMoney,
                                   Rate = tbloan.Rate,
                                   LoanDeadline = tbline.LoanDeadlineName,
                                   SubmitTime = tbloan.SubmitTime,
                                   StartTime = tbloan.SubmitTime.ToString(),
                                   RepayPrincipal = tbloan.RepayPrincipal,
                                   AlreadyPrincipal = tbloan.AlreadyPrincipal,
                                   ArrearagePrincipal = tbloan.ArrearagePrincipal,
                                   LoanPeriods = tbloan.LoanPeriods,
                                   PayableInterest = tbloan.PayableInterest,
                               });

                if (treetoptypeID > 0)
                {
                    Maoloan = Maoloan.Where(p => p.TreetoptypeID == treetoptypeID);
                }

                int TotaRow = Maoloan.Count();
                List<LoanVo> LoanList = Maoloan.OrderBy(p => p.LoanID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<LoanVo> bsgrid = new Bsgrid<LoanVo>();
                bsgrid.success = true;
                bsgrid.totalRows = TotaRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = LoanList;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }

        //通过满标借款ID传递信息
        public ActionResult SelectManBiaoLoan(int loanID)
        {
            LoanVo list = (from tbloan in myModels.B_LoanTable
                           join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                           where tbloan.LoanID == loanID
                           select new LoanVo
                           {
                               LoanID = tbloan.LoanID,
                               TreetoptypeID = tbloan.TreetoptypeID,
                               Rate = tbloan.Rate,
                               LoanPurpose = tbloan.LoanPurpose,
                               LoanMoney = tbloan.LoanMoney,
                               LoanPeriods = tbloan.LoanPeriods,
                               RepayPrincipal = tbloan.RepayPrincipal,
                               ArrearagePrincipal = tbloan.ArrearagePrincipal,
                               EveryTrancheMoney=tbloan.EveryTrancheMoney

                           }).Single();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //还款保存
        public ActionResult InsertRepayment(B_LoanDetailTable BDetail, int LoanID)
        {
            string str = "";
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();

                int loanPeriods = Convert.ToInt32(Request.Form["LoanPeriods"]);

                string Rate = Request.Form["Rate"];//传递过来的利率
                decimal rate;
                rate = Convert.ToDecimal(Rate.ToString().Trim());

                decimal loanMoney = Convert.ToDecimal(Request.Form["LoanMoney"]);//借款金额
                decimal repayPrincipal = Convert.ToDecimal(Request.Form["RepayPrincipal"]);//应还金额
                decimal arrearagePrincipal = Convert.ToDecimal(Request.Form["ArrearagePrincipal"]);//未还金额
                decimal everyTrancheMoney = Convert.ToDecimal(Request.Form["EveryTrancheMoney"]);//每期还款金额
                decimal dArrearagePrincipal = Convert.ToDecimal(Request.Form["DArrearagePrincipal"]);//当期还款金额
                //loanID = Convert.ToInt32(Request.Form["LoanID"]);
                //BDetail.LoanID =LoanID;
                string validCode = Request.Form["validCode"];
                string Code = "";
                if (Session["vildeCode"] != null)
                {
                    Code = Session["vildeCode"].ToString();
                    if (Code.Equals(validCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (list.PropertyAmounts >= dArrearagePrincipal)//判断用户总金额是否大于当期还款金额
                        {

                            if (dArrearagePrincipal == everyTrancheMoney)//当期还款金额是否等于每期还款金额
                            {
                                var B_loan = (from tbloan in myModels.B_LoanTable
                                              where tbloan.LoanID == LoanID
                                              select tbloan).Single();
                                if (B_loan.StatusID == 34)
                                {
                                    try
                                    {
                                        var OverdueLoanTable = (from tbover in myModels.B_OverdueLoanTable
                                                                where tbover.LoanID == B_loan.LoanID
                                                                select tbover).Single();
                                        if (OverdueLoanTable.WebsiteWhetherPayStatusID == 32)
                                        {
                                            var BWest = (from tbwet in myModels.B_WebsiteReceivableDetail
                                                         where tbwet.OverdueLoanID == OverdueLoanTable.OverdueLoanID
                                                         select tbwet).Single();
                                            BWest.StatusID = 30;
                                            BWest.RealityTime = DateTime.Now;
                                            BWest.RealityAmount = BWest.ReceivableCapital + BWest.ReceivableAccrual;
                                            myModels.Entry(BWest).State = System.Data.Entity.EntityState.Modified;
                                            myModels.SaveChanges();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }

                                }
                                var BD = (from tbBD in myModels.B_LoanDetailTable
                                          where tbBD.LoanID == B_loan.LoanID
                                          select new
                                          {
                                              tbBD.LoanID,
                                          }).Count();
                                if (BD > 0)
                                {

                                    var BD1 = (from tbBD in myModels.B_LoanDetailTable
                                               where tbBD.LoanID == B_loan.LoanID
                                               select tbBD).Single();
                                    BD1.RepayPrincipal = BD1.RepayPrincipal + dArrearagePrincipal;//偿还本息
                                    BD1.AlreadyPrincipal = BD1.AlreadyPrincipal + dArrearagePrincipal;//已还本息
                                    BD1.ArrearagePrincipal = BD1.RepayPrincipal - BD1.AlreadyPrincipal;//未还本息
                                    //BD1.PayableInterest = BD1.PayableInterest +Convert.ToDecimal(BD1.PayableInterest / loanPeriods);//应还利息
                                    BD1.CurrentCouponRepay = BD1.CurrentCouponRepay + dArrearagePrincipal;
                                    BD1.PayableDate = DateTime.Now;
                                    BD1.StatusID = 12;
                                    myModels.Entry(BD1).State = System.Data.Entity.EntityState.Modified;
                                    myModels.SaveChanges();
                                } else
                                {
                                    BDetail.RepayPrincipal = dArrearagePrincipal;//偿还本息
                                    BDetail.AlreadyPrincipal = BDetail.RepayPrincipal;//已还本息
                                    BDetail.ArrearagePrincipal = repayPrincipal - BDetail.AlreadyPrincipal;//未还本息
                                    BDetail.PayableInterest = ((loanMoney * rate) / 100 / loanPeriods); //应还利息
                                    BDetail.CurrentCouponRepay = dArrearagePrincipal;//当期还款
                                    BDetail.SeveralIssues = Convert.ToInt32(loanPeriods).ToString();//第几期
                                    BDetail.PayableDate = DateTime.Now;
                                    //BDetail.StatusID = BDetail.StatusID;
                                    BDetail.LoanID = LoanID;
                                    myModels.B_LoanDetailTable.Add(BDetail);
                                    myModels.SaveChanges();
                                }
                                

                                //修改借款表
                                B_LoanTable Bloan = (from tbloan in myModels.B_LoanTable
                                                     where tbloan.LoanID == BDetail.LoanID
                                                     select tbloan).Single();
                                Bloan.AlreadyPrincipal = Bloan.AlreadyPrincipal+ dArrearagePrincipal;//已还本息
                                Bloan.ArrearagePrincipal = Bloan.RepayPrincipal - Bloan.AlreadyPrincipal;//未还本息
                                Bloan.LoanPeriods =Convert.ToInt32(loanPeriods - 1).ToString();//剩余期数
                                if (Bloan.ArrearagePrincipal == 0)
                                {
                                    Bloan.StatusID = 12;
                                    myModels.Entry(Bloan).State = System.Data.Entity.EntityState.Modified;
                                    myModels.SaveChanges();
                                }
                                else {
                                    Bloan.StatusID = Bloan.StatusID;
                                    myModels.Entry(Bloan).State = System.Data.Entity.EntityState.Modified;
                                    myModels.SaveChanges();
                                }

                                //修改借款用户表
                                B_UserTable Buser = (from tbuser in myModels.B_UserTable
                                                     where tbuser.UserID == list.UserID
                                                     select tbuser).Single();
                                Buser.PropertyAmounts = Buser.PropertyAmounts - dArrearagePrincipal;//总金额减去当期还款金额
                                Buser.UsableMoney = Buser.UsableMoney - dArrearagePrincipal;//可用金额减去当期还款金额
                                myModels.Entry(Buser).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();

                                //修改投资表
                                try
                                {
                                    var BInvest = (from tbInvest in myModels.B_InvestTable
                                                   where tbInvest.LoanID == Bloan.LoanID
                                                   select tbInvest).ToList();
                                    for (int i = 0; i < BInvest.Count; i++)
                                    {
                                        int varloanID = Convert.ToInt32(BInvest[i].InvestID);
                                        var bInvest = (from tbInvest in myModels.B_InvestTable
                                                       where tbInvest.InvestID == varloanID
                                                       select tbInvest).Single();
                                        bInvest.CountermandPrincipal = bInvest.CountermandPrincipal+(dArrearagePrincipal/ BInvest.Count);//已收本息
                                        bInvest.NotRetrievePrincipal = bInvest.ReceivablePrincipal - bInvest.CountermandPrincipal; //未收本息=应收本息-已收本息
                                        if (bInvest.NotRetrievePrincipal == 0)
                                        {
                                            bInvest.StatusID = 30;
                                            myModels.Entry(bInvest).State = System.Data.Entity.EntityState.Modified;
                                            myModels.SaveChanges();
                                        }
                                        else {
                                            bInvest.StatusID = bInvest.StatusID;
                                            myModels.Entry(bInvest).State = System.Data.Entity.EntityState.Modified;
                                            myModels.SaveChanges();
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                //修改投资用户表
                                try
                                {
                                    var B_Invest = (from tbInvest in myModels.B_InvestTable
                                                    where tbInvest.LoanID == Bloan.LoanID
                                                    select tbInvest).ToList();
                                    for (int i = 0; i < B_Invest.Count; i++)
                                    {
                                        int userID = Convert.ToInt32(B_Invest[i].UserID);
                                        B_UserTable dbuser = (from tbuser in myModels.B_UserTable
                                                              where tbuser.UserID == userID
                                                              select tbuser).Single();
                                        dbuser.PropertyAmounts = dbuser.PropertyAmounts + B_Invest[i].CountermandPrincipal;//总金额
                                        dbuser.UsableMoney = dbuser.UsableMoney + B_Invest[i].CountermandPrincipal;
                                        myModels.Entry(dbuser).State = System.Data.Entity.EntityState.Modified;
                                        myModels.SaveChanges();
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }

                                B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                                Capitalrecord.UserID = list.UserID;
                                Capitalrecord.OperateTypeID = 3;
                                Capitalrecord.OpFare = Capitalrecord.OpFare + dArrearagePrincipal;
                                Capitalrecord.Income = Convert.ToDecimal(0);
                                Capitalrecord.Expend = Capitalrecord.Expend + dArrearagePrincipal;
                                Capitalrecord.PropertyAmounts = list.PropertyAmounts;
                                Capitalrecord.Remarks = "用户" + "[" + list.UserName.Trim() + "]" + "，还款成功，余额减少" + dArrearagePrincipal + "元";
                                Capitalrecord.operatetime = DateTime.Now;
                                myModels.B_CapitalrecordTable.Add(Capitalrecord);
                                myModels.SaveChanges();
                                str = "success";
                            }
                            else
                            {
                                str = "OneTime";
                            }
                        }
                        else
                        {
                            str = "NoMoney";
                        }
                    }
                    else
                    {
                        str = "CodeFail";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Json(str, JsonRequestBehavior.AllowGet);

        }

        //查询已还清的借款
        public ActionResult SelectAlreadyRepayment(BsgridPage bsgridPage, int treetoptypeID)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();
                var Bloan = from tbdetail in myModels.B_LoanDetailTable
                            join tbloan in myModels.B_LoanTable on tbdetail.LoanID equals tbloan.LoanID
                            join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                            join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                            where tbloan.UserID == list.UserID && (tbloan.StatusID == 12 || tbloan.StatusID == 34)
                            select new LoanVo
                            {
                                LoanID = tbloan.LoanID,
                                UserID = tbloan.UserID,
                                TreetoptypeID = tbloan.TreetoptypeID,
                                LoanPurpose = tbloan.LoanPurpose,
                                LoanMoney = tbloan.LoanMoney,
                                Treetoptype = tbtree.Treetoptype,
                                RepayPrincipal = tbloan.RepayPrincipal,
                                AlreadyPrincipal = tbloan.AlreadyPrincipal,
                                ArrearagePrincipal = tbloan.ArrearagePrincipal,
                                PayableDate = tbdetail.PayableDate,
                                StartTime = tbdetail.PayableDate.ToString(),
                            };
                if (treetoptypeID > 0)
                {
                    Bloan = Bloan.Where(p => p.TreetoptypeID == treetoptypeID);
                }

                int TotaRow = Bloan.Count();
                List<LoanVo> LoanList = Bloan.OrderBy(p => p.LoanID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<LoanVo> bsgrid = new Bsgrid<LoanVo>();
                bsgrid.success = true;
                bsgrid.totalRows = TotaRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = LoanList;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //查询还款明细
        public ActionResult SelectLoanDetail(BsgridPage bsgridPage)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();
                var LoanDetail = from tbdetail in myModels.B_LoanDetailTable
                                 join tbloan in myModels.B_LoanTable on tbdetail.LoanID equals tbloan.LoanID
                                 join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                                 where tbloan.UserID == list.UserID
                                 select new LoanDetail
                                 {
                                     LoanDetailID = tbdetail.LoanDetailID,
                                     LoanID = tbdetail.LoanID,
                                     LoanPurpose = tbloan.LoanPurpose,
                                     LoanMoney = tbloan.LoanMoney,
                                     Treetoptype = tbtree.Treetoptype,
                                     RepayPrincipal = tbdetail.RepayPrincipal,
                                     AlreadyPrincipal = tbdetail.AlreadyPrincipal,
                                     ArrearagePrincipal = tbdetail.ArrearagePrincipal,
                                     PayableInterest = tbdetail.PayableInterest,
                                     PayableDate = tbdetail.PayableDate,
                                     StartTime = tbdetail.PayableDate.ToString(),
                                 };
                int TotaRow = LoanDetail.Count();
                List<LoanDetail> LoanList = LoanDetail.OrderBy(p => p.LoanDetailID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<LoanDetail> bsgrid = new Bsgrid<LoanDetail>();
                bsgrid.success = true;
                bsgrid.totalRows = TotaRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = LoanList;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //查询逾期还款
        public ActionResult SelectOverdue(BsgridPage bsgridPage)
        {
            try
            {
                int accountID = Convert.ToInt32(Session["AccountID"].ToString());
                var list = (from tbuser in myModels.B_UserTable
                            where tbuser.AccountID == accountID
                            select tbuser).Single();
                var Overdue = from tbdue in myModels.B_OverdueLoanTable
                              join tbloan in myModels.B_LoanTable on tbdue.LoanID equals tbloan.LoanID
                              where tbloan.UserID == list.UserID
                              select new OverdueVo
                              {
                                  OverdueLoanID = tbdue.OverdueLoanID,
                                  LoanID = tbdue.LoanID,
                                  PayablePrincipal = tbdue.PayablePrincipal,
                                  OverdueDay = tbdue.OverdueDay,
                                  PayableTime = tbdue.PayableTime,
                                  StartTime = tbdue.PayableTime.ToString(),
                                  LoanPurpose = tbloan.LoanPurpose,
                                  LoanPeriods = tbloan.LoanPeriods,
                                  AlreadyPrincipal = tbloan.AlreadyPrincipal,
                              };
                int TotaRow = Overdue.Count();
                List<OverdueVo> LoanList = Overdue.OrderBy(p => p.OverdueLoanID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<OverdueVo> bsgrid = new Bsgrid<OverdueVo>();
                bsgrid.success = true;
                bsgrid.totalRows = TotaRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = LoanList;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 收款管理 

        //我的收款页面
        public ActionResult GatheringIndex()
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
        //回收中的借款
        public ActionResult SelectRetrieve(BsgridPage bsgridPage, int treetoptypeID)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var list = (from tbInvest in myModels.B_InvestTable
                            join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                            join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                            join tbdeline in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbdeline.LoanDeadlineID
                            join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                            join tbstatc in myModels.S_StatusTable on tbInvest.StatusID equals tbstatc.StatusID
                            where tbInvest.UserID == listS.UserID && tbInvest.CountermandPrincipal == 0
                            select new InvestVo
                            {
                                InvestID = tbInvest.InvestID,
                                UserID = tbInvest.UserID,
                                LoanID = tbInvest.LoanID,
                                TreetoptypeID = tbloan.TreetoptypeID,
                                UserName = tbuser.UserName,
                                LoanPurpose = tbloan.LoanPurpose,
                                TreetopType = tbtree.Treetoptype,
                                Rate = tbloan.Rate,
                                LoanDeadlineName = tbdeline.LoanDeadlineName,
                                InvestMoney = tbInvest.InvestMoney,
                                CountermandPrincipal = tbInvest.CountermandPrincipal,
                                InvestTime = tbInvest.InvestTime,
                                StartTime = tbInvest.InvestTime.ToString(),
                                StatusName = tbstatc.StatusName,
                            });
                if (treetoptypeID > 0)
                {
                    list = list.Where(p => p.TreetoptypeID == treetoptypeID);
                }
                int TotaRow = list.Count();
                List<InvestVo> listT = list.OrderByDescending(p => p.InvestID)
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
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //未收款的明细账
        public ActionResult SelectGathering(BsgridPage bsgridPage, int treetoptypeID)
        {

            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var list = (from tbInvest in myModels.B_InvestTable
                            join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                            join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                            join tbstatu in myModels.S_StatusTable on tbInvest.StatusID equals tbstatu.StatusID
                            join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                            where tbInvest.UserID == listS.UserID && tbInvest.CountermandPrincipal == 0 && tbInvest.StatusID == 16
                            select new InvestVo
                            {
                                InvestID = tbInvest.InvestID,
                                UserID = tbInvest.UserID,
                                LoanID = tbInvest.LoanID,
                                TreetoptypeID = tbloan.TreetoptypeID,
                                StatusID = tbInvest.StatusID,
                                UserName = tbuser.UserName,
                                LoanPurpose = tbloan.LoanPurpose,
                                TreetopType = tbtree.Treetoptype,
                                ReceivablePrincipal = tbInvest.ReceivablePrincipal,//应收本息
                                CountermandPrincipal = tbInvest.CountermandPrincipal,//已收本息
                                Endtime = tbloan.Endtime,
                                StartTime = tbloan.Endtime.ToString(),
                                StatusName = tbstatu.StatusName,

                            });
                if (treetoptypeID > 0)
                {
                    list = list.Where(p => p.TreetoptypeID == treetoptypeID);
                }
                int TotaRow = list.Count();
                List<InvestVo> listT = list.OrderByDescending(p => p.InvestID)
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
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //借出明细账
        public ActionResult SlectloanDetail(BsgridPage bsgridPage, int treetoptypeID)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();

                var list = from tbInvest in myModels.B_InvestTable
                           join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                           join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                           join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                           where tbInvest.UserID == listS.UserID
                           select new InvestVo
                           {
                               InvestID = tbInvest.InvestID,
                               UserID = tbInvest.UserID,
                               LoanID = tbInvest.LoanID,
                               TreetoptypeID = tbloan.TreetoptypeID,
                               UserName = tbuser.UserName,
                               TreetopType = tbtree.Treetoptype,
                               InvestMoney = tbInvest.InvestMoney,
                               ReceivablePrincipal = tbInvest.ReceivablePrincipal,
                               CountermandPrincipal = tbInvest.CountermandPrincipal,
                               NotRetrievePrincipal = tbInvest.NotRetrievePrincipal,
                               ReceivableInterest = tbInvest.ReceivableInterest,
                               InvestReason = tbInvest.InvestReason
                           };

                if (treetoptypeID > 0)
                {
                    list = list.Where(p => p.TreetoptypeID == treetoptypeID);
                }
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
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //回收完的借款
        public ActionResult SelectPayOffLoan(BsgridPage bsgridPage, int treetoptypeID)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();

                var list = from tbInvest in myModels.B_InvestTable
                           join tbloan in myModels.B_LoanTable on tbInvest.LoanID equals tbloan.LoanID
                           join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                           join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                           join tbdeline in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbdeline.LoanDeadlineID
                           where tbInvest.UserID == listS.UserID && tbInvest.CountermandPrincipal != 0
                           select new InvestVo
                           {
                               InvestID = tbInvest.InvestID,
                               UserID = tbInvest.UserID,
                               LoanID = tbInvest.LoanID,
                               TreetoptypeID = tbloan.TreetoptypeID,
                               LoanDeadlineID = tbloan.LoanDeadlineID,
                               UserName = tbuser.UserName,
                               LoanPurpose = tbloan.LoanPurpose,
                               TreetopType = tbtree.Treetoptype,
                               Rate = tbloan.Rate,
                               LoanDeadlineName = tbdeline.LoanDeadlineName,
                               InvestMoney = tbInvest.InvestMoney,
                               CountermandPrincipal = tbInvest.CountermandPrincipal,
                           };

                if (treetoptypeID > 0)
                {
                    list = list.Where(p => p.TreetoptypeID == treetoptypeID);
                }
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
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region 我的服务

        #region 我的关注
        //关注页面
        public ActionResult FollowIndex()
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

        //查询我关注的标
        public ActionResult SelectFollowLoan(BsgridPage bsgridPage)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var list = from tbfollow in myModels.B_FollowTable
                           join tbloan in myModels.B_LoanTable on tbfollow.LoanID equals tbloan.LoanID
                           join tbtree in myModels.S_Treetoptypetalbe on tbloan.TreetoptypeID equals tbtree.TreetoptypeID
                           join tbdeline in myModels.S_LoanDeadlineTable on tbloan.LoanDeadlineID equals tbdeline.LoanDeadlineID
                           where tbfollow.UserID == listS.UserID
                           select new FollowVo
                           {
                               FollowID = tbfollow.FollowID,
                               LoanID = tbfollow.LoanID,
                               UserID = tbfollow.UserID,
                               Loantitle = tbloan.Loantitle,
                               LoanPurpose = tbloan.LoanPurpose,
                               LoanMoney = tbloan.LoanMoney,
                               Rate = tbloan.Rate,
                               Treetoptype = tbtree.Treetoptype,
                               LoanDeadlineName = tbdeline.LoanDeadlineName,
                           };
                int TotalRow = list.Count();
                List<FollowVo> ListT = list.OrderBy(p => p.FollowID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<FollowVo> bsgrid = new Bsgrid<FollowVo>();
                bsgrid.success = true;
                bsgrid.totalRows = TotalRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = ListT;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        //查询我关注的用户
        public ActionResult SelectFollowuser(BsgridPage bsgridPage)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var list = from tbfollow in myModels.B_FollowTable
                           join tbloan in myModels.B_LoanTable on tbfollow.LoanID equals tbloan.LoanID
                           join tbuser in myModels.B_UserTable on tbloan.UserID equals tbuser.UserID
                           where tbfollow.UserID == listS.UserID
                           select new FollowVo
                           {
                               FollowID = tbfollow.FollowID,
                               LoanID = tbfollow.LoanID,
                               UserID = tbloan.UserID,
                               UserName = tbuser.UserName,
                               Sex = tbuser.Sex,
                               BornDate = tbuser.BornDate,
                               NativePlace = tbuser.NativePlace,
                               MonthIncome = tbuser.MonthIncome,
                               StartTime = tbuser.BornDate.ToString()
                           };
                int TotalRow = list.Count();
                List<FollowVo> ListT = list.OrderBy(p => p.FollowID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<FollowVo> bsgrid = new Bsgrid<FollowVo>();
                bsgrid.success = true;
                bsgrid.totalRows = TotalRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = ListT;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //取消关注
        public ActionResult DeleteFollowLoan(int followID)
        {
            try
            {
                var list = (from tbfollow in myModels.B_FollowTable
                            where tbfollow.FollowID == followID
                            select tbfollow).Single();
                myModels.Entry(list).State = System.Data.Entity.EntityState.Deleted;
                myModels.B_FollowTable.Remove(list);
                myModels.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 我的推广
        //推广页面
        public ActionResult SpreadIndex()
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

        //查询用户
        public ActionResult Selectuser()
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                List<SelectVo> listType = new List<SelectVo>();
                SelectVo selectvo = new SelectVo
                {
                    id = 0,
                    text = "--请选择--"
                };
                listType.Add(selectvo);
                List<SelectVo> listTypeS = (from tbuser in myModels.B_UserTable
                                            where tbuser.UserID != listS.UserID
                                            select new SelectVo
                                            {
                                                id = tbuser.UserID,
                                                text = tbuser.UserName
                                            }).ToList();
                listType.AddRange(listTypeS);
                return Json(listType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //绑定推广类型
        public ActionResult SelectSpreadLeiXing()
        {
            List<SelectVo> listType = new List<SelectVo>();
            SelectVo selectvo = new SelectVo
            {
                id = 0,
                text = "--请选择--"
            };
            listType.Add(selectvo);
            List<SelectVo> listTypeS = (from tbtype in myModels.B_SpreadTypeTable
                                        select new SelectVo
                                        {
                                            id = tbtype.SpreadTypeID,
                                            text = tbtype.Name
                                        }).ToList();
            listType.AddRange(listTypeS);
            return Json(listType, JsonRequestBehavior.AllowGet);
        }

        //新增推广
        public ActionResult InsertSpread(B_SpreadUserTable BSpread)
        {
            try
            {
                string str = "";
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                BSpread.SpreadRenID = Convert.ToInt32(Request.Form["UserName"]);
                BSpread.SpreadTypeID = Convert.ToInt32(Request.Form["Name"]);

                try
                {
                    int BlistSpread = (from tbp in myModels.B_SpreadUserTable
                                       where tbp.UserID == listS.UserID && tbp.SpreadRenID == BSpread.SpreadRenID
                                       select tbp).Count();
                    if (BlistSpread > 0)
                    {
                        str = "AlreadySpread";
                    }
                    else
                    {
                        try
                        {
                            int list = (from tbPerson in myModels.B_PersonalSpreadMessage
                                        where tbPerson.SpreadUserID == BSpread.SpreadRenID
                                        select tbPerson).Count();
                            if (list > 0)
                            {
                                var Bper = (from tbper in myModels.B_PersonalSpreadMessage
                                            where tbper.SpreadUserID == BSpread.SpreadRenID
                                            select tbper).Single();
                                Bper.SpreadPeople = Bper.SpreadPeople + 1;
                                myModels.Entry(Bper).State = System.Data.Entity.EntityState.Modified;
                                myModels.SaveChanges();
                            }
                            else
                            {
                                B_PersonalSpreadMessage Bperlist = new B_PersonalSpreadMessage();
                                Bperlist.SpreadUserID = BSpread.SpreadRenID;
                                int spreadPeople;
                                spreadPeople = Convert.ToInt32(Bperlist.SpreadPeople);
                                spreadPeople = spreadPeople + 1;
                                Bperlist.SpreadPeople = Convert.ToDecimal(spreadPeople.ToString());
                                Bperlist.InvestTime = 0;
                                Bperlist.InvestAmount = 0;
                                Bperlist.InvestTiCheng = 0;
                                Bperlist.RepaymentTime = 0;
                                Bperlist.RepaymentAmount = 0;
                                Bperlist.RepaymentTiCheng = 0;
                                myModels.B_PersonalSpreadMessage.Add(Bperlist);
                                myModels.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            return Json(null, JsonRequestBehavior.AllowGet);
                        }
                        BSpread.UserID = listS.UserID;
                        BSpread.StatusID = 20;
                        BSpread.RelevanceTime = DateTime.Now;
                        myModels.B_SpreadUserTable.Add(BSpread);
                        myModels.SaveChanges();

                        var userlist = (from tuser in myModels.B_UserTable
                                        where tuser.UserID == BSpread.SpreadRenID
                                        select tuser).Single();

                        if (BSpread.SpreadTypeID == 1)
                        {
                            B_SpreadRecordTable BRecord = new B_SpreadRecordTable();
                            BRecord.SpreadUserID = BSpread.SpreadUserID;
                            BRecord.SpreadCustomID = BSpread.SpreadRenID;
                            BRecord.SpreadTypeID = BSpread.SpreadTypeID;
                            BRecord.FundType = "本金";
                            BRecord.SpreadAmount = Convert.ToDecimal(0.10);
                            BRecord.SubmitTime = DateTime.Now;
                            BRecord.Remark = "推广用户【" + userlist.UserName.Trim() + "】" + "投资做生意";
                            myModels.B_SpreadRecordTable.Add(BRecord);
                            myModels.SaveChanges();
                            str = "success";
                        }
                        else
                        {
                            B_SpreadRecordTable BRecord = new B_SpreadRecordTable();
                            BRecord.SpreadUserID = BSpread.SpreadUserID;
                            BRecord.SpreadCustomID = BSpread.SpreadRenID;
                            BRecord.SpreadTypeID = BSpread.SpreadTypeID;
                            BRecord.FundType = "本金";
                            BRecord.SpreadAmount = Convert.ToDecimal(0.10);
                            BRecord.SubmitTime = DateTime.Now;
                            BRecord.Remark = "推广用户【" + userlist.UserName.Trim() + "】" + "做生意急周转，需借款";
                            myModels.B_SpreadRecordTable.Add(BRecord);
                            myModels.SaveChanges();
                            str = "success";
                        }
                    }
                }
                catch (Exception e)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        //我要推广
        public ActionResult SelectSpread(BsgridPage bsgridPage)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var listSp = (from tbsp in myModels.B_SpreadUserTable
                              join tbMess in myModels.B_PersonalSpreadMessage on tbsp.SpreadRenID equals tbMess.SpreadUserID
                              join tbuser in myModels.B_UserTable on tbsp.SpreadRenID equals tbuser.UserID
                              where tbsp.UserID == listS.UserID
                              select new SpreadUserVo
                              {
                                  SpreadUserID = tbsp.SpreadUserID,
                                  SpreadRenID = tbsp.SpreadRenID,
                                  UserID = tbsp.UserID,
                                  UserName = tbuser.UserName,
                                  InvestAmount = tbMess.InvestAmount,
                                  RepaymentAmount = tbMess.RepaymentAmount,

                              });
                int totalrow = listSp.Count();
                List<SpreadUserVo> listt = listSp.OrderBy(p => p.SpreadUserID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<SpreadUserVo> bsgrid = new Bsgrid<SpreadUserVo>();
                bsgrid.success = true;
                bsgrid.totalRows = totalrow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = listt;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //推广记录
        public ActionResult SelectSpreadRecord(BsgridPage bsgridPage, string username)
        {
            try
            {
                int intAccountID = Convert.ToInt32(Session["AccountID"]);
                var listS = (from dbuser in myModels.B_UserTable
                             where dbuser.AccountID == intAccountID
                             select dbuser).Single();
                var list = from tbspread in myModels.B_SpreadUserTable
                           join tbuser in myModels.B_UserTable on tbspread.SpreadRenID equals tbuser.UserID
                           join tbType in myModels.B_SpreadTypeTable on tbspread.SpreadTypeID equals tbType.SpreadTypeID
                           where tbspread.UserID == listS.UserID
                           select new SpreadUserVo
                           {
                               SpreadUserID = tbspread.SpreadUserID,
                               SpreadRenID = tbspread.SpreadRenID,
                               SpreadTypeID = tbspread.SpreadTypeID,
                               UserName = tbuser.UserName,
                               Name = tbType.Name,
                           };
                //标题不为空
                if (!string.IsNullOrEmpty(username))
                {
                    list = list.Where(n => n.UserName.Contains(username));
                }
                int TotaRow = list.Count();
                List<SpreadUserVo> ListT = list.OrderBy(p => p.SpreadUserID)
                    .Skip(bsgridPage.GetStartIndex())
                    .Take(bsgridPage.pageSize)
                    .ToList();

                Bsgrid<SpreadUserVo> bsgrid = new Bsgrid<SpreadUserVo>();
                bsgrid.success = true;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.totalRows = TotaRow;
                bsgrid.data = ListT;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

    }
}