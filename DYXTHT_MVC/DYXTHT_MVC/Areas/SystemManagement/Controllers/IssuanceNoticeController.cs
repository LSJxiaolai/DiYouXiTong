using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DYXTHT_MVC.Models;
using System.IO;
using DYXTHT_MVC.Vo;
using System.Text.RegularExpressions;

namespace DYXTHT_MVC.Areas.SystemManagement.Controllers
{
    public class IssuanceNoticeController : Controller
    {
        Models.DYXTEntities myDYXTEntities = new DYXTEntities();
        // GET: SystemManagement/IssuanceNotice
        /// <summary>
        /// 文章管理
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeManagement()
        {
            return View();
        }
        /// <summary>
        ///公告查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="noticeSUser"></param>
        /// <param name="noticeSTitles"></param>
        /// <param name="noticeTypeId"></param>
        /// <returns></returns>
        public ActionResult SelectNoticeAll(BsgridPage bsgridPage, string noticeSUser, string noticeSTitles, int noticeTypeId)
        {
            var linqNotice = from tbNoticeTable in myDYXTEntities.B_NoticeTable
                             join tbNoticeTypeTable in myDYXTEntities.B_NoticeTypeTable on tbNoticeTable.NoticeTypeID equals tbNoticeTypeTable.NoticeTypeID
                             join tbNoticeStatus in myDYXTEntities.S_NoticeStatusTable on tbNoticeTable.NoticeStatusID equals tbNoticeStatus.NoticeStatusID
                             join tbAccount in myDYXTEntities.B_AccountTable on tbNoticeTable.AccountID equals tbAccount.AccountID
                             select new NoticeVo
                             {
                                 NoticeID = tbNoticeTable.NoticeID,
                                 Title = tbNoticeTable.Title,
                                 AccountID = tbNoticeTable.AccountID,
                                 User = tbAccount.User,
                                 NoticeContent = tbNoticeTable.NoticeContent,
                                 NoticeStatusID = tbNoticeStatus.NoticeStatusID,
                                 NoticeTypeID = tbNoticeTable.NoticeTypeID,
                                 IssueTime = tbNoticeTable.IssueTime,
                                 NoticeTypeName = tbNoticeTypeTable.NoticeTypeName,
                                 NoticeStatusName = tbNoticeStatus.NoticeStatusName,
                                 ReleaseTimeStr = tbNoticeTable.IssueTime.ToString()
                             };
            if (!string.IsNullOrEmpty(noticeSUser))
            {
                linqNotice = linqNotice.Where(n => n.User.Contains(noticeSUser));
            }
            if (!string.IsNullOrEmpty(noticeSTitles))
            {
                linqNotice = linqNotice.Where(n => n.Title.Contains(noticeSTitles));
            }
            //类型ID不为空
            if (noticeTypeId > 0)
            {
                linqNotice = linqNotice.Where(p => p.NoticeTypeID == noticeTypeId);
            }
            int totalRow = linqNotice.Count();
            List<NoticeVo> notices = linqNotice.OrderByDescending(p => p.NoticeID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<NoticeVo> bsgrid = new Bsgrid<NoticeVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询类型
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectNotictype(BsgridPage bsgridPage)
        {
            var notict = from tbNoticeType in myDYXTEntities.B_NoticeTypeTable                       
                         select new NoictVo
                         {
                              NoticeTypeID = tbNoticeType.NoticeTypeID,
                              NoticeTypeName = tbNoticeType.NoticeTypeName,
                              Byname = tbNoticeType.Byname
                          };
            int totalRow = notict.Count();
            List<NoictVo> notices = notict.OrderByDescending(p => p.NoticeTypeID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<NoictVo> bsgrid = new Bsgrid<NoictVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
           
        }
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertNotice()
        {

            try
            {
                int User = Convert.ToInt32(Session["AccountID"].ToString());
            }
            catch (Exception e)
            {

                return Redirect("/LoginMain/Login");
            }

            return View();
        }
        /// <summary>
        /// 分类栏目
        /// </summary>
        /// <returns></returns>
        public ActionResult BaseInfor()
        {
            return View();
        }
        /// <summary>
        /// 添加分类栏目
        /// </summary>
        /// <param name="NoticeType"></param>
        /// <returns></returns>   
        public ActionResult InsertNoticeType(B_NoticeTypeTable NoticeType)
        {
            //定义返回
            string strMsg = "fail";
            if (!string.IsNullOrEmpty(NoticeType.NoticeTypeName))
            {
                //根据公告名称查询是否已经存在
                int oldCount = (from tbNoticeType in myDYXTEntities.B_NoticeTypeTable
                                where tbNoticeType.NoticeTypeName == NoticeType.NoticeTypeName.Trim()
                                select tbNoticeType).Count();
                if (oldCount == 0)
                {
                    try
                    {
                        myDYXTEntities.B_NoticeTypeTable.Add(NoticeType);
                        if (myDYXTEntities.SaveChanges() > 0)
                        {
                            strMsg = "success";//成功
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    strMsg = "exist";//已经存在
                }
            }
            else
            {
                strMsg = "nofull";//数据不完整
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除分类栏
        /// </summary>
        /// <param name="onticeId"></param>
        /// <returns></returns>
        public ActionResult NoticetypeDelete(int NoticeTypeID) {
            try
            {
                B_NoticeTypeTable dbNoticeTable = (from tbNoticetype in myDYXTEntities.B_NoticeTypeTable
                                           where tbNoticetype.NoticeTypeID == NoticeTypeID
                                                   select tbNoticetype).Single();
            //删除数据
            myDYXTEntities.B_NoticeTypeTable.Remove(dbNoticeTable);
            myDYXTEntities.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //"数据异常！";
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改公告类型绑定
        /// </summary>
        /// <param name="NoticeTypeID"></param>
        /// <returns></returns>
        public ActionResult updatanotictype(int NoticeTypeID)
        {
            try
            {
                var noticeType = (from tbNoticeType in myDYXTEntities.B_NoticeTypeTable
                                                where tbNoticeType.NoticeTypeID == NoticeTypeID
                                                select new  NoictVo
                                                {
                                                   NoticeTypeID=tbNoticeType.NoticeTypeID,
                                                    NoticeTypeName= tbNoticeType.NoticeTypeName.Trim(),
                                                    Byname= tbNoticeType.Byname.Trim(),
                                                    Content= tbNoticeType.Content.Trim()
                                                }
                                                ).Single();
               
                return Json(noticeType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 修改公告类型保存
        /// </summary>
        /// <param name="NoticeTypeTable"></param>
        /// <returns></returns>
        public ActionResult UpdateNoticeType(B_NoticeTypeTable NoticeTypeTable)
        {
            //定义返回
            string strMsg = "fail";
            if (!string.IsNullOrEmpty(NoticeTypeTable.NoticeTypeName))
            {
                //查询除了自身外 公告类型名称查询是否已经存在
                int oldCount = (from tbNoticeType in myDYXTEntities.B_NoticeTypeTable
                                where tbNoticeType.NoticeTypeID != NoticeTypeTable.NoticeTypeID
                                && tbNoticeType.NoticeTypeName == NoticeTypeTable.NoticeTypeName.Trim()
                                select tbNoticeType).Count();
                if (oldCount == 0)
                {
                    try
                    {
                        
                        //修改公告类型
                        myDYXTEntities.Entry(NoticeTypeTable).State = System.Data.Entity.EntityState.Modified;
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
            }
            else
            {
                strMsg = "nofull";//数据不完整
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改公告
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public ActionResult UpdateNotice(int NoticeID)
        {

            try
            {
                int User = Convert.ToInt32(Session["AccountID"].ToString());
            }
            catch (Exception e)
            {

                return Redirect("/LoginMain/Login");
            }
            ViewData["noticeId"] = NoticeID;
            return View();
        }
        /// <summary>
        /// 查询公告类型 to 下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctNoticeType()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbnotice in myDYXTEntities.B_NoticeTypeTable
                                         select new SelectVo
                                         {
                                             id = tbnotice.NoticeTypeID,
                                             text = tbnotice.NoticeTypeName

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);
        }
        //公告状态
        public ActionResult selectNoticeStatus()
        {
            List<SelectVo> noticeType = (from tbnotice in myDYXTEntities.S_NoticeStatusTable
                                         select new SelectVo
                                         {
                                             id = tbnotice.NoticeStatusID,
                                             text = tbnotice.NoticeStatusName

                                         }).ToList();
            return Json(noticeType, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 在富文本编辑器 插入 文件
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        public ActionResult UpEeditorFile(HttpPostedFileBase upload)
        {
            try
            {
                if (upload != null)
                {
                    string fileExtension = Path.GetExtension(upload.FileName);
                    //文件名称
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "-" + Guid.NewGuid() + fileExtension;
                    //保存文件的路径
                    string filePath = Server.MapPath("~/Document/Notice/") + fileName;
                    if ("(.gif)|(.jpg)|(.bmp)|(.jpeg)|(.png)".Contains(fileExtension))
                    {
                        //保存文件
                        upload.SaveAs(filePath);
                        var url = "/Document/Notice/" + fileName;

                        //获取上传的临时文件表(未保存的)
                        List<string> tempFile = new List<string>();
                        if (Session["tempEditorFile"] != null)
                        {
                            tempFile = Session["tempEditorFile"] as List<string>;
                        }
                        //未保存公告的临时文件
                        tempFile.Add(fileName);
                        //保存到session
                        Session["tempEditorFile"] = tempFile;

                        var CKEditor = System.Web.HttpContext.Current.Request["CKEditorFuncNum"];
                        // 上传成功后，我们还需要通过以下的一个脚本把图片返回到第一个 tab 选项
                        return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditor + ", \"" + url + "\");</script>");
                    }
                    else
                    {
                        return Content("<script>alert(\"只能上传图片\");</script>");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Content("<script>alert(\"上传图片失败\");</script>");
        }
        /// <summary>
        ///  查询公告信息ById
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public ActionResult SelectNoticeById(int noticeId)
        {
            try
            {
                NoticeVo notice = (from tbNoticeTable in myDYXTEntities.B_NoticeTable
                                   join tbNoticeTypeTable in myDYXTEntities.B_NoticeTypeTable on tbNoticeTable.NoticeTypeID equals tbNoticeTypeTable.NoticeTypeID
                                   join tbNoticeStatus in myDYXTEntities.S_NoticeStatusTable on tbNoticeTable.NoticeStatusID equals tbNoticeStatus.NoticeStatusID
                                   join tbAccount in myDYXTEntities.B_AccountTable on tbNoticeTable.AccountID equals tbAccount.AccountID
                                   where tbNoticeTable.NoticeID == noticeId
                                   select new NoticeVo
                                   {
                                       NoticeID = tbNoticeTable.NoticeID,
                                       Title = tbNoticeTable.Title.Trim(),
                                       ArticleLabel = tbNoticeTable.ArticleLabel.Trim(),
                                       AccountID = tbNoticeTable.AccountID,
                                       User = tbAccount.User.Trim(),
                                       NoticeContent = tbNoticeTable.NoticeContent.Trim(),
                                       NoticeStatusID = tbNoticeStatus.NoticeStatusID,
                                       NoticeTypeID = tbNoticeTable.NoticeTypeID,
                                       IssueTime = tbNoticeTable.IssueTime,
                                       NoticeTypeName = tbNoticeTypeTable.NoticeTypeName.Trim(),
                                       NoticeStatusName = tbNoticeStatus.NoticeStatusName.Trim(),
                                       ReleaseTimeStr = tbNoticeTable.IssueTime.ToString()
                                   }).Single();
                //加载公告内容
                string textFileName = Server.MapPath("~/Document/Notice/Text/") + notice.NoticeContent;
                if (System.IO.File.Exists(textFileName))
                {
                    //文件存在
                    notice.NoticeContent = System.IO.File.ReadAllText(textFileName);
                }
                else
                {
                    //文件不存在
                    notice.NoticeContent = "<p>没有找到公告内容,可能文件已经丢失;请重新编辑发布</p>";
                }

                //返回公告信息的内容
                return Json(notice, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 新增公告保存
        /// </summary>
        /// <param name="B_Notice"></param>
        /// <param name="noticeCarouseImage"></param>
        /// <returns></returns>

        [ValidateInput(false)]//由于要存入HTML，关闭验证
        public ActionResult NoticeInsert(B_NoticeTable B_Notice, HttpPostedFileBase noticeCarouseImage)
        {
            try
            {
                int User = Convert.ToInt32(Session["AccountID"].ToString());
                //定义list 存放需要保存的富文本框图片的名称
                List<string> savedImageList = new List<string>();
                if (!string.IsNullOrEmpty(B_Notice.Title))
                {
                    //检查 存放公告内容的 目录是否存在,不存在就创建
                    if (!Directory.Exists(Server.MapPath("~/Document/Notice/Text/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Document/Notice/Text/"));
                    }

                    //--->用txt文件来保存公告内容
                    //txt文件名称
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "-" + Guid.NewGuid() + ".txt";
                    //txt文件路径
                    string filePath = Server.MapPath("~/Document/Notice/Text/") + fileName;
                    //txt文件内容
                    string textCount = B_Notice.NoticeContent;

                    //-----获取最终保存的图片文件
                    MatchCollection matchs = System.Text.RegularExpressions.Regex.Matches(textCount, "(?<=/Document/Notice/).+?(?=\".+?/>)");
                    foreach (Match match in matchs)
                    {
                        savedImageList.Add(match.Value);
                    }

                    //保存文件 txt文件
                    //StreamWriter 文件路径，是否追加，文件编码
                    TextWriter textWriter = new StreamWriter(filePath, false, new System.Text.UTF8Encoding(false));
                    textWriter.Write(textCount);
                    textWriter.Close();

                    //保存公告信息
                    B_Notice.AccountID = User;//登录用户
                    B_Notice.NoticeContent = fileName;//公告内容
                    B_Notice.IssueTime = DateTime.Now;//发布时间
                                                      //保存公告数据
                    myDYXTEntities.B_NoticeTable.Add(B_Notice);
                    myDYXTEntities.SaveChanges();

                    //清理上传到富文本框中，后被删除（即未被使用的图片）
                    //获取上传的临时图片文件表(未保存的)
                    List<string> tempFile = new List<string>();
                    if (Session["tempEditorFile"] != null)
                    {
                        tempFile = Session["tempEditorFile"] as List<string>;
                    }
                    if (tempFile != null)
                    {
                        string dFilePath = Server.MapPath("~/Document/Notice/");
                        //遍历临时文件表
                        foreach (string img in tempFile)
                        {
                            //当临时文件表中的文件不存在于被保存的文件时,删除该文件,避免文件冗余
                            if (!savedImageList.Contains(img))
                            {
                                string strdeletePath = dFilePath + img;
                                try
                                {
                                    System.IO.File.Delete(strdeletePath);
                                }
                                catch (Exception e)
                                {
                                }
                            }
                        }
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改公告保存
        /// </summary>
        /// <param name="B_Notice"></param>
        /// <param name="noticeCarouseImage"></param>
        /// <returns></returns>

        [ValidateInput(false)]//由于要存入HTML，关闭验证
        public ActionResult NoticeUpdate(B_NoticeTable B_Notice, HttpPostedFileBase noticeCarouseImage)
        {
            try
            {
                int User = Convert.ToInt32(Session["AccountID"].ToString());
                //保存的插入的图片
                List<string> savedImageList = new List<string>();
                //原始的插入图片   
                List<string> oldSavedImageList = new List<string>();
                //检查公告标题不为空
                if (!string.IsNullOrEmpty(B_Notice.Title))
                {
                    B_NoticeTable dbNotice = (from tbNoticeTable in myDYXTEntities.B_NoticeTable
                                                 where tbNoticeTable.NoticeID == B_Notice.NoticeID
                                                 select tbNoticeTable).Single();

                    //检查 存放公告内容的 目录是否存在,不存在就创建
                    if (!Directory.Exists(Server.MapPath("~/Document/Notice/Text/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Document/Notice/Text/"));
                    }

                    //检查目录是否存在,不存在就创建
                    if (!Directory.Exists(Server.MapPath("~/Document/Notice/NoticeCarousel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Document/Notice/NoticeCarousel/"));
                    }

                    //--->用txt文件来保存公告内容
                    //txt文件名称
                    string fileName;
                    //txt文件路径
                    string filePath;
                    //加载公告内容
                    string oldFilePath = Server.MapPath("~/Document/Notice/Text/") + dbNotice.NoticeContent;
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        //读取原始的文本内容
                        //存在文件
                        string oldTextContent = System.IO.File.ReadAllText(oldFilePath);
                        //匹配出文件名称 保存到list oldSavedImageList
                        MatchCollection oldMatchs = Regex.Matches(oldTextContent, "(?<=Document/Notice/).+?(?=\".+?/>)");
                        foreach (Match match in oldMatchs)
                        {
                            oldSavedImageList.Add(match.Value);
                        }
                        //获取文件名称
                        fileName = dbNotice.NoticeContent;
                        //文件路径
                        filePath = oldFilePath;
                    }
                    else
                    {
                        //不存在文件
                        fileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "-" + Guid.NewGuid() + ".txt";
                        //文件路径
                        filePath = Server.MapPath("~/Document/Notice/Text/") + fileName;
                    }
                    //txt文件内容
                    string textContent = B_Notice.NoticeContent;
                    //-----获取最终保存名称到数据中的文件
                    MatchCollection matchs = Regex.Matches(textContent, "(?<=Document/Notice/).+?(?=\".+?/>)");
                    foreach (Match match in matchs)
                    {
                        savedImageList.Add(match.Value);
                    }

                    //检查没有使用的插图,将其删除
                    //遍历原来的插图文件列表oldSavedImageList,如在现在的文件列表savedImageList,则在使用;
                    //如不在,则未使用应该移除
                    foreach (string str in oldSavedImageList)
                    {
                        //判断是否存在,不存在就删除
                        if (!savedImageList.Contains(str))
                        {
                            //删除文件
                            string dfilePath = Server.MapPath("~/Document/Notice/") + str;
                            try
                            {
                                System.IO.File.Delete(dfilePath);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }

                    //获取上传的临时插图文件表(未保存的)
                    List<string> tempFile = new List<string>();
                    if (Session["tempEditorFile"] != null)
                    {
                        tempFile = Session["tempEditorFile"] as List<string>;
                    }
                    if (tempFile != null)
                    {
                        string dFilePath = Server.MapPath("~/Document/Notice/");
                        //遍历临时文件表
                        foreach (string s in tempFile)
                        {
                            //当临时文件表中的文件 不存在 于被保存的文件时,删除该文件,避免文件冗余
                            if (!savedImageList.Contains(s))
                            {
                                string strdeletePath = dFilePath + s;
                                try
                                {
                                    System.IO.File.Delete(strdeletePath);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            }
                        }
                    }

                    //保存文件 公告内容的txt文件
                    TextWriter textWriter = new StreamWriter(filePath, false, new System.Text.UTF8Encoding(false));
                    textWriter.Write(textContent);
                    textWriter.Close();

                    //更新公告信息
                    dbNotice.NoticeTypeID = B_Notice.NoticeTypeID;
                    dbNotice.NoticeStatusID = B_Notice.NoticeStatusID;
                    dbNotice.IssueTime = DateTime.Now;//发布时间
                    dbNotice.ArticleLabel = B_Notice.ArticleLabel;
                    dbNotice.Title = B_Notice.Title;

                    dbNotice.NoticeContent = fileName;//公告内容
                    dbNotice.AccountID = User;
                    myDYXTEntities.Entry(dbNotice).State = System.Data.Entity.EntityState.Modified;//改为修改状态
                    myDYXTEntities.SaveChanges();
                    //移除session
                    Session.Remove("tempEditorFile");

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        //根据公告ID查询详细
        public ActionResult NoticeDetailed(int NoticeID)
        {
            try
            {
                NoticeVo notice = (from tbNoticeTable in myDYXTEntities.B_NoticeTable
                                   join tbNoticeTypeTable in myDYXTEntities.B_NoticeTypeTable on tbNoticeTable.NoticeTypeID equals tbNoticeTypeTable.NoticeTypeID
                                   join tbNoticeStatus in myDYXTEntities.S_NoticeStatusTable on tbNoticeTable.NoticeStatusID equals tbNoticeStatus.NoticeStatusID
                                   join tbAccount in myDYXTEntities.B_AccountTable on tbNoticeTable.AccountID equals tbAccount.AccountID
                                   where tbNoticeTable.NoticeID == NoticeID
                                   select new NoticeVo
                                   {
                                       NoticeID = tbNoticeTable.NoticeID,
                                       Title = tbNoticeTable.Title,
                                       ArticleLabel = tbNoticeTable.ArticleLabel,
                                        AccountID = tbNoticeTable.AccountID,
                                        User = tbAccount.User,
                                        NoticeContent = tbNoticeTable.NoticeContent,
                                        NoticeStatusID = tbNoticeStatus.NoticeStatusID,
                                        NoticeTypeID = tbNoticeTable.NoticeTypeID,
                                        IssueTime = tbNoticeTable.IssueTime,
                                        NoticeTypeName = tbNoticeTypeTable.NoticeTypeName,
                                        NoticeStatusName = tbNoticeStatus.NoticeStatusName,
                                        ReleaseTimeStr = tbNoticeTable.IssueTime.ToString()
                                    }).Single();
            //加载公告内容
            string textFileName = Server.MapPath("~/Document/Notice/Text/") + notice.NoticeContent;
            if (System.IO.File.Exists(textFileName))
            {
                //文件存在
                notice.NoticeContent = System.IO.File.ReadAllText(textFileName);
            }
            else
            {
                //文件不存在
                notice.NoticeContent = "<p>没有找到公告内容,可能文件已经丢失;请重新编辑发布</p>";
            }
            ViewBag.notice = notice;

            }
            catch (Exception e)
            {

            }
            return View();
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="onticeId"></param>
        /// <returns></returns>
        public ActionResult NoticeDelete(int NoticeID)
        {
            try
            {
                B_NoticeTable dbNoticeTable = (from tbNoticeTable in myDYXTEntities.B_NoticeTable
                                                where tbNoticeTable.NoticeID == NoticeID
                                               select tbNoticeTable).Single();

                //删除数据
                myDYXTEntities.B_NoticeTable.Remove(dbNoticeTable);
                //--->删除插图 公告内容 txt文件路径
                string NoticeTablepath = Server.MapPath("~/Document/Notice/Text/") + dbNoticeTable.NoticeContent;

                if (System.IO.File.Exists(NoticeTablepath))
                {
                    //读取原始的文本内容
                    //存在文件
                    string oldTextContent = System.IO.File.ReadAllText(NoticeTablepath);

                    //匹配出文件名称 删除
                    MatchCollection oldMatchs = Regex.Matches(oldTextContent, "(?<=Document/Notice/).+?(?=\".+?/>)");
                    foreach (Match match in oldMatchs)
                    {
                        // 删除文件
                        string dfilePath = Server.MapPath("~/Document/Notice/") + match.Value;
                        System.IO.File.Delete(dfilePath);
                    }
                }
                //--->删除公告内容txt文件
                System.IO.File.Delete(NoticeTablepath);               
                myDYXTEntities.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //"数据异常！";
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 清除session文件和session
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearSessionFile()
        {
            string strPathfile = Server.MapPath("~/Document/Notice/");
            List<string> tempFile = new List<string>();  //获取session中的文件表
            if (Session["tempEditorFile"] != null)
            {
                tempFile = Session["tempEditorFile"] as List<string>;
                //遍历临时文件表
                foreach (string str in tempFile)
                {
                    string strdeletePath = strPathfile + str;
                    try
                    {
                        System.IO.File.Delete(strdeletePath);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }    
            //移除session
            Session.Remove("tempEditorFile");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}