using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DYXTHT_MVC.Vo;
using DYXTHT_MVC.Models;
using System.Data.Entity;

namespace DYXTHT_MVC.Areas.Systemmodulemanagement.Controllers
{
    public class SystemmoduleController : Controller
    {
        // GET: Systemmodulemanagement/Systemmodule
        Models.DYXTEntities myDYXTEntities = new DYXTEntities();
        /// <summary>
        /// 系统设置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Systemmoduleview()
        {
            return View();
        }
        public ActionResult selectsystemodule(BsgridPage bsgridPage)
        {
            var varB_Module = from tb_Module in myDYXTEntities.B_ModuleTable
                              join tbuser in myDYXTEntities.B_UserTable on tb_Module.UserID equals tbuser.UserID
                              where tb_Module.ModuleStatusID == null
                              select new B_ModuleVo
                              {
                                  ModuleID = tb_Module.ModuleID,
                                  identificationName = tb_Module.identificationName,
                                  Name = tb_Module.Name,
                                  Describe = tb_Module.Describe,
                                  Edition = tb_Module.Edition,
                                  UserName = tbuser.UserName,
                                  ReleaseTimeStr = tb_Module.UpdateTime.ToString()
                              };
            int totalRow = varB_Module.Count();
            List<B_ModuleVo> notices = varB_Module.OrderBy(p => p.ModuleID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_ModuleVo> bsgrid = new Bsgrid<B_ModuleVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 已安装模块
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectsyste(BsgridPage bsgridPage)
        {
            var varB_Module = from tb_Module in myDYXTEntities.B_ModuleTable
                              join tbuser in myDYXTEntities.B_UserTable on tb_Module.UserID equals tbuser.UserID
                              join tbmodue in myDYXTEntities.S_ModuleStatusTable on tb_Module.ModuleStatusID equals tbmodue.ModuleStatusID
                              select new B_ModuleVo
                              {
                                  ModuleID = tb_Module.ModuleID,
                                  UserName = tbuser.UserName,
                                  identificationName = tb_Module.identificationName,
                                  Byname = tb_Module.Byname,
                                  Name = tb_Module.Name,
                                  Edition = tb_Module.Edition,
                                  ReleaseTimeStr = tb_Module.UpdateTime.ToString(),
                                  Describe = tb_Module.Describe,
                                  ModuleStatus = tbmodue.ModuleStatus
                              };
            int totalRow = varB_Module.Count();
            List<B_ModuleVo> notices = varB_Module.OrderBy(p => p.ModuleID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_ModuleVo> bsgrid = new Bsgrid<B_ModuleVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelctModuleStatus()
        {

            List<SelectVo> Province = (from tbModuleStatus in myDYXTEntities.S_ModuleStatusTable
                                       select new SelectVo
                                       {
                                           id = tbModuleStatus.ModuleStatusID,
                                           text = tbModuleStatus.ModuleStatus
                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑绑定
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public ActionResult UpdataModule(int ModuleID)
        {
            try
            {
                var varB_Module = (from tb_Module in myDYXTEntities.B_ModuleTable
                                   join tbuser in myDYXTEntities.B_UserTable on tb_Module.UserID equals tbuser.UserID
                                   join tbmodue in myDYXTEntities.S_ModuleStatusTable on tb_Module.ModuleStatusID equals tbmodue.ModuleStatusID
                                   where tb_Module.ModuleID == ModuleID
                                   select new B_ModuleVo
                                   {
                                       ModuleID = tb_Module.ModuleID,
                                       UserID = tb_Module.UserID,
                                       identificationName = tb_Module.identificationName,
                                       Byname = tb_Module.Byname,
                                       Name = tb_Module.Name,
                                       Edition = tb_Module.Edition,
                                       Describe = tb_Module.Describe,
                                       ModuleStatusID = tb_Module.ModuleStatusID
                                   }).Single();
                return Json(varB_Module, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="ModuleTable"></param>
        /// <returns></returns>
        public ActionResult Updatabianji(B_ModuleTable ModuleTable)
        {
            //定义返回
            string strMsg = "fail";
            //查询除了自身外查询是否已经存在
            int oldCount = (from tbModuleTable in myDYXTEntities.B_ModuleTable
                            where tbModuleTable.ModuleID != ModuleTable.ModuleID
                            && tbModuleTable.Name.Trim() == ModuleTable.Name.Trim()
                            select tbModuleTable).Count();
            if (oldCount == 0)
            {
                try
                {
                    ModuleTable.UpdateTime = DateTime.Now;
                    //修改公告类型
                    myDYXTEntities.Entry(ModuleTable).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult DeleteModule(int ModuleID)
        {
            string strMsg = "fail";
            try
            {
                B_ModuleTable varFundCost = (from tModuleTable in myDYXTEntities.B_ModuleTable
                                             where tModuleTable.ModuleID == ModuleID
                                             select tModuleTable).Single();
                //删除数据
                myDYXTEntities.B_ModuleTable.Remove(varFundCost);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 管理员管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GunanLiUserTypeview()
        {
            return View();
        }
        public ActionResult SelectUsertype(BsgridPage bsgridPage, string SeUserName, int SeUserNameID)
        {
            var varB_Module = from tbUser in myDYXTEntities.B_UserTable
                              join tbUserType in myDYXTEntities.S_UserTypeTable on tbUser.UserTypeID equals tbUserType.UserTypeID
                              where tbUserType.UserTypeID != 2 && tbUserType.UserTypeID != 3&& tbUserType.UserTypeID != 1
                              select new USerbankVo
                              {
                                  UserID = tbUser.UserID,
                                  UserName = tbUser.UserName,
                                  TrueName = tbUser.TrueName,
                                  UserTypeName = tbUserType.UserTypeName
                              };
            if (!string.IsNullOrEmpty(SeUserName))
            {
                varB_Module = varB_Module.Where(n => n.UserName.Contains(SeUserName));
            }
            if (SeUserNameID > 0)
            {
                varB_Module = varB_Module.Where(p => p.UserID == SeUserNameID);
            }
            int totalRow = varB_Module.Count();
            List<USerbankVo> notices = varB_Module.OrderBy(p => p.UserID).
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
        /// 管理类型
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctUsertype()
        {

            List<SelectVo> Province = (from tbModuleStatus in myDYXTEntities.S_UserTypeTable
                                       where tbModuleStatus.UserTypeID != 2 && tbModuleStatus.UserTypeID != 3
                                       select new SelectVo
                                       {
                                           id = tbModuleStatus.UserTypeID,
                                           text = tbModuleStatus.UserTypeName
                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改绑定
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult UpdataUsertype(int UserID)
        {
            try
            {
                var varB_Module = (from tbUser in myDYXTEntities.B_UserTable
                                   join tbusertype in myDYXTEntities.S_UserTypeTable on tbUser.UserTypeID equals tbusertype.UserTypeID
                                   where tbUser.UserID == UserID
                                   select new USerbankVo
                                   {
                                       UserID = tbUser.UserID,
                                       UserName = tbUser.UserName,
                                       TrueName = tbUser.TrueName,
                                       UserTypeID = tbUser.UserTypeID
                                   }).Single();
                return Json(varB_Module, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="strUser"></param>
        /// <returns></returns>
        public ActionResult UpdataUser(B_UserTable strUser)
        {
            //定义返回
            string strMsg = "fail";
            //查询除了自身外查询是否已经存在
            int oldCount = (from tbUSer in myDYXTEntities.B_UserTable
                            where tbUSer.UserID != strUser.UserID
                            && tbUSer.UserName.Trim() == strUser.UserName.Trim()
                            select tbUSer).Count();
            if (oldCount == 0)
            {
                try
                {
                    //修改公告类型
                    myDYXTEntities.Entry(strUser).State = System.Data.Entity.EntityState.Modified;
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
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult Deleteuser(int UserID)
        {
            string strMsg = "fail";
            try
            {
                B_UserTable varFundCost = (from tModuleTable in myDYXTEntities.B_UserTable
                                           where tModuleTable.UserID == UserID
                                           select tModuleTable).Single();
                //删除数据
                myDYXTEntities.B_UserTable.Remove(varFundCost);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 管理员记录
        /// </summary>
        /// <returns></returns>
        public ActionResult managertakenotes()
        {
            return View();
        }

        public ActionResult Selectmanager(BsgridPage bsgridPage)
        {
            var varB_Module = from tb_ManagerRecord in myDYXTEntities.B_ManagerRecord
                              join tb_user in myDYXTEntities.B_UserTable on tb_ManagerRecord.UserID equals tb_user.UserID
                              select new B_ManagerRecordVo
                              {
                                  RecordID = tb_ManagerRecord.RecordID,
                                  UserName = tb_user.UserName,
                                  BearFruit = tb_ManagerRecord.BearFruit,
                                  Content = tb_ManagerRecord.Content,
                                  ReleaseTimeStr = tb_ManagerRecord.LoginTime.ToString(),
                                  LoginIP= tb_ManagerRecord.LoginIP

                              };
            int totalRow = varB_Module.Count();
            List<B_ManagerRecordVo> notices = varB_Module.OrderByDescending(p => p.RecordID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<B_ManagerRecordVo> bsgrid = new Bsgrid<B_ManagerRecordVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        #region 管理员权限    
        public ActionResult Jurisdiction()//权限维护视图
        {
            return View();
        }
        public ActionResult InsertJurisdiction()//新增权限视图
        {
            return View();
        }
        public ActionResult UpdateJurisdiction()//修改权限视图
        {
            return View();
        }
        //查询角色
        public ActionResult SelectUserTypeAll(BsgridPage bsgridPage)
        {
            try
            {

                var linqItem = from tbUserType in myDYXTEntities.S_UserTypeTable
                               where tbUserType.UserTypeID != 1//1:超级管理员
                               select new S_UserTypeTableVo
                               {
                                   UserTypeID = tbUserType.UserTypeID,
                                   UserTypeName = tbUserType.UserTypeName,
                                   Describe = tbUserType.Describe,
                                   FoundTime = tbUserType.FoundTime,
                                   ToVoidNo = tbUserType.ToVoidNo//作废否
                               };
                int totalRow = linqItem.Count();
                List<S_UserTypeTableVo> UserTypeVos = linqItem.OrderByDescending(p => p.UserTypeID).
                    Skip(bsgridPage.GetStartIndex()).
                    Take(bsgridPage.pageSize).
                    ToList();

                Bsgrid<S_UserTypeTableVo> bsgrid = new Bsgrid<S_UserTypeTableVo>();
                bsgrid.success = true;
                bsgrid.totalRows = totalRow;
                bsgrid.curPage = bsgridPage.curPage;
                bsgrid.data = UserTypeVos;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("shibai", JsonRequestBehavior.AllowGet);
            }

        }


        //查询模块
        public ActionResult SelectModular()
        {
            try
            {
                var linqItem = (from tbModular in myDYXTEntities.B_ModuleTable
                                select new
                                {
                                    tbModular.ModuleID,
                                    tbModular.Name
                                }).ToList();
                return Json(linqItem, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }

        }

        //查询角色权限通过角色ID
        public ActionResult SelectJurisdiction(int UserTypeID)
        {
            try
            {
                var linqItem = (from tbJurisdiction in myDYXTEntities.B_Jurisdictio
                                join tbModularDetail in myDYXTEntities.S_ModularDetailITable on tbJurisdiction.ModularDetailID equals tbModularDetail.ModularDetailID
                                join tbUserType in myDYXTEntities.S_UserTypeTable on tbJurisdiction.UserTypeID equals tbUserType.UserTypeID
                                where tbJurisdiction.UserTypeID == UserTypeID
                                select new
                                {
                                    tbJurisdiction.ModularDetailID,
                                    tbUserType.UserTypeName,
                                    tbUserType.Describe
                                }).ToList();
                return Json(linqItem, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }

        }

        //查询模块明细
        public ActionResult SelectModularDetail(int ModuleID)
        {
            try
            {
                //查询模块
                var listMoKuaiMingXi = (from tbModular in myDYXTEntities.B_ModuleTable
                                        join tbModularDetail in myDYXTEntities.S_ModularDetailITable on tbModular.ModuleID equals tbModularDetail.ModuleID
                                        join tbOpType in myDYXTEntities.S_OpTypeTable on tbModularDetail.OpTypeID equals tbOpType.OpTypeID
                                        where tbModular.ModuleID == ModuleID
                                        select new
                                        {
                                            tbModularDetail.ModularDetailID,
                                            tbOpType.Optype
                                        }).ToList();
                return Json(listMoKuaiMingXi, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }

        }

        //新增
        public ActionResult InsertUserType(string Name, string Describe, Array MoKuaiMingXiiiD)
        {
            try
            {
                //声明字符串接收模块明细信息数组的信息
                string z = ((string[])MoKuaiMingXiiiD)[0];
                //声明字符串数组接收模块明细信息数据
                string[] ints = z.Split(',');
                //实例化用户类型表   新增数据
                S_UserTypeTable modelsUserType = new S_UserTypeTable();
                modelsUserType.UserTypeName = Name.Trim();
                modelsUserType.Describe = Describe.Trim();
                modelsUserType.FoundTime = DateTime.Now.ToString();
                modelsUserType.ToVoidNo = false;
                //modelsUserType.typeClass = Session["UserTypeClass"].ToString().Trim();
                myDYXTEntities.S_UserTypeTable.Add(modelsUserType);
                myDYXTEntities.SaveChanges();

                //获取新增的用户类型ID
                var XinZengID = modelsUserType.UserTypeID;
                //实例化权限表
                B_Jurisdictio modelsJurisdiction = new B_Jurisdictio();
                for (int d = 0; d < ints.Length; d++)
                {
                    modelsJurisdiction.ModularDetailID = Convert.ToInt32(ints[d]);
                    modelsJurisdiction.UserTypeID = XinZengID;
                    myDYXTEntities.B_Jurisdictio.Add(modelsJurisdiction);
                    myDYXTEntities.SaveChanges();
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);

            }

        }


        //修改
        public ActionResult UpdateUserType(int UserTypeID, string UserType, string Describe, Array XinZengshuzu, Array ShanChushuzu)
        {
            try
            {
                //声明字符串获取新增数组
                string x = ((string[])XinZengshuzu)[0];
                string[] intx = x.Split(',');
                //声明字符串获取删除数组
                string s = ((string[])ShanChushuzu)[0];
                string[] ints = s.Split(',');
                //声明权限表
                B_Jurisdictio modelsJurisdiction = new B_Jurisdictio();
                for (int i = 0; i < intx.Length; i++)
                {
                    if (intx[i] != "")
                    {
                        modelsJurisdiction.UserTypeID = UserTypeID;
                        modelsJurisdiction.ModularDetailID = Convert.ToInt32(intx[i]);
                        myDYXTEntities.B_Jurisdictio.Add(modelsJurisdiction);
                        myDYXTEntities.SaveChanges();
                    }
                }
                for (int i = 0; i < ints.Length; i++)
                {
                    if (ints[i] != "")
                    {
                        Delete(UserTypeID, Convert.ToInt32(ints[i]));
                    }
                }
                var db = (from tb in myDYXTEntities.S_UserTypeTable
                          where tb.UserTypeID == UserTypeID
                          select new
                          {
                              tb.ToVoidNo,
                              tb.FoundTime,                             
                          }).ToList();
                S_UserTypeTable modelUserType = new S_UserTypeTable();
                modelUserType.UserTypeID = UserTypeID;
                modelUserType.UserTypeName = UserType.Trim();
                modelUserType.Describe = Describe.Trim();
                modelUserType.FoundTime = db[0].FoundTime;
                modelUserType.ToVoidNo = db[0].ToVoidNo;
              
                myDYXTEntities.Entry(modelUserType).State = EntityState.Modified;
                myDYXTEntities.SaveChanges();

              
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }

        }
        //删除角色权限1
        public string Delete(int UserTypeID, int ModularDetailID)
        {
            try
            {
                //根据用户类型ID和模块明细ID查询列表
                var listDep = myDYXTEntities.B_Jurisdictio.Where(m => m.UserTypeID == UserTypeID && m.ModularDetailID == ModularDetailID).ToList();
                //移除列表数据
                myDYXTEntities.B_Jurisdictio.Remove(listDep.Single());
                myDYXTEntities.SaveChanges();
                return "success";
            }
            catch
            {
                return "failed";
            }
        }


        //删除角色权限2
        public string Delete2(int UserTypeID)
        {
            try
            {
                //根据用户类型查询权限信息
                var listDep = myDYXTEntities.B_Jurisdictio.Where(m => m.UserTypeID == UserTypeID).ToList();
                //移除列表数据
                for (int i = 0; i < listDep.Count; i++)
                {
                    myDYXTEntities.B_Jurisdictio.Remove(listDep[i]);
                    myDYXTEntities.SaveChanges();
                }
                return "success";
            }
            catch
            {
                return "failed";
            }
        }

        //删除
        public ActionResult DeleteUserType(int ID)
        {
            try
            {
                var listDep = myDYXTEntities.S_UserTypeTable.Where(m => m.UserTypeID == ID).ToList();
                myDYXTEntities.S_UserTypeTable.Remove(listDep.Single());
                myDYXTEntities.SaveChanges();
                Delete2(ID);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }


        //是否存在该用户
        public bool ExistUserType(int UserTypeID)
        {
            //查询用户角色明细信息 By 用户类型ID
            var listUser = (from tdUserRoleDetail in myDYXTEntities.B_UserTable
                            where tdUserRoleDetail.UserTypeID == UserTypeID
                            select new
                            {
                                tdUserRoleDetail.UserID
                            }).ToList();

            if (listUser.Count == 0)
            {
                //用户角色明细总数为0返回true
                return true;
            }
            else
            {
                return false;
            }
        }

        //修改启用和作废的状态
        public ActionResult ZuoFeiFou(int UserTypeID, bool DuanDuan)//DuanDuan记录操作按钮：true启用,false作废
        {
            string strMsg = "failed";
            try
            {

                if (DuanDuan == false)
                {
                    ////作废

                    //判断用户角色明细信息是否存在：true用户角色明细信息为空，
                    if (ExistUserType(UserTypeID) == true)
                    {
                        //根据主键ID查询用户类型信息
                        var listdd = (from db in myDYXTEntities.S_UserTypeTable
                                      where db.UserTypeID == UserTypeID
                                      select new
                                      {
                                          db.UserTypeName,//用户类型
                                          db.Describe,
                                          db.FoundTime,
                                          db.ToVoidNo
                                      }).ToList();
                        //用户类型存在
                        if (listdd.Count > 0)
                        {
                            //修改用户类型
                            S_UserTypeTable modelUserType = new S_UserTypeTable();
                            modelUserType.UserTypeID = UserTypeID;
                            modelUserType.UserTypeName = listdd[0].UserTypeName;
                            modelUserType.Describe = listdd[0].Describe.Trim();
                            modelUserType.FoundTime = listdd[0].FoundTime;
                            modelUserType.ToVoidNo = DuanDuan;//作废否=false作废
                            myDYXTEntities.Entry(modelUserType).State = EntityState.Modified;
                            myDYXTEntities.SaveChanges();
                            strMsg = "success";
                        }
                        else
                        {
                            strMsg = "failed";
                        }
                    }
                    else
                    {
                        //提示用户该类型已存在
                        strMsg = "exist";
                    }
                }
                else
                {   ////启用


                    //用户类型不存在
                    var listdd = (from db in myDYXTEntities.S_UserTypeTable
                                  where db.UserTypeID == UserTypeID
                                  select new
                                  {
                                      db.UserTypeName,
                                      db.Describe,
                                      db.FoundTime,
                                      db.ToVoidNo
                                  }).ToList();
                    if (listdd.Count > 0)
                    {
                        S_UserTypeTable modelUserType = new S_UserTypeTable();
                        modelUserType.UserTypeID = UserTypeID;
                        modelUserType.UserTypeName = listdd[0].UserTypeName;
                        modelUserType.Describe = listdd[0].Describe.Trim();
                        modelUserType.FoundTime = listdd[0].FoundTime;
                        modelUserType.ToVoidNo = DuanDuan;//作废否=true作废
                        myDYXTEntities.Entry(modelUserType).State = EntityState.Modified;
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";

                    }
                    else
                    {
                        strMsg = "failed";
                    }
                }
            }
            catch
            {
                strMsg = "failed";

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}