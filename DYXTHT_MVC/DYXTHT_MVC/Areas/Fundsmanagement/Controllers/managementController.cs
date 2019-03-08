using DYXTHT_MVC.Common;
using DYXTHT_MVC.Models;
using DYXTHT_MVC.Vo;
using NPOI.HSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DYXTHT_MVC.Areas.Fundsmanagement.Controllers
{
    public class managementController : Controller
    {
        Models.DYXTEntities myDYXTEntities = new DYXTEntities();
        // GET: Fundsmanagement/management
        /// <summary>
        /// 资金账户管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Fundsmanag()
        {
            return View();
        }
        /// <summary>
        ///资金管理查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectFunds(BsgridPage bsgridPage)
        {
            List<FundsVo> notict = (from tbUser in myDYXTEntities.B_UserTable
                         join tbusertype in myDYXTEntities.S_UserTypeTable on tbUser.UserTypeID  equals tbusertype.UserTypeID
                         where tbUser.UserTypeID==2 || tbUser.UserTypeID==3
                         select new FundsVo
                         {
                             UserID = tbUser.UserID,
                             UserName = tbUser.UserName,
                             PropertyAmounts =tbUser.PropertyAmounts,
                             UsableMoney = tbUser.UsableMoney,
                             FreezeMoney=tbUser.FreezeMoney,
                             WaitMoney=tbUser.WaitMoney,
                             CompensatoryMoney=tbUser.CompensatoryMoney
                         }).ToList();
           
            int totalRow = notict.Count();
            for (int i = 0; i < notict.Count(); i++)
            {
                notict[i].PropertyAmountsmoney ="￥"+ Convert.ToString(notict[i].PropertyAmounts.ToString());
                notict[i].PropertyUsableMoney = "￥" + Convert.ToString(notict[i].UsableMoney.ToString());
                notict[i].PropertyFreezeMoney = "￥" + Convert.ToString(notict[i].FreezeMoney.ToString());
                notict[i].PropertyWaitMoney = "￥" + Convert.ToString(notict[i].WaitMoney.ToString());
                notict[i].PropertyCompensatoryMoney = "￥" + Convert.ToString(notict[i].CompensatoryMoney.ToString());
            }
            List<FundsVo> notices = notict.OrderBy(p => p.UserID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<FundsVo> bsgrid = new Bsgrid<FundsVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);         
        }
       /// <summary>
       /// 充值记录按钮
       /// </summary>
       /// <param name="bsgridPages"></param>
       /// <param name="UserID"></param>
       /// <returns></returns>
        public ActionResult selectchongzhijilu(BsgridPage bsgridPages,int UserID)
        {
            try
            {
                var Recharge = (from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                                join tbuser in  myDYXTEntities.B_UserTable on tbUserRechargeRecord.UserID equals tbuser.UserID
                                join tb_RechargeType in myDYXTEntities.S_RechargeTypeTable on tbUserRechargeRecord.RechargeTypeID equals tb_RechargeType.RechargeTypeID
                                join tb_PayType in myDYXTEntities.S_PayTypeTable on tbUserRechargeRecord.PayTypeID equals tb_PayType.PayTypeID
                                join tbStatus in myDYXTEntities.S_StatusTable on tbUserRechargeRecord.StatusID equals tbStatus.StatusID
                               
                                where tbUserRechargeRecord.UserID== UserID
                                select new rechargemanageVo
                                {
                                  RechargeRecordID=tbUserRechargeRecord.RechargeRecordID,
                                    UserName= tbuser.UserName,
                                    RechargeTypeName =  tb_RechargeType.RechargeTypeName,
                                    PayTypeName= tb_PayType.PayTypeName,
                                    TradeNumber=   tbUserRechargeRecord.TradeNumber,
                                    RechargeMoney= tbUserRechargeRecord.RechargeMoney,
                                    RechargePoundage=  tbUserRechargeRecord.RechargePoundage,
                                    RealityAccountMoney=   tbUserRechargeRecord.RealityAccountMoney,
                                    StatusName=  tbStatus.StatusName,
                                    ReleaseTimeStr= tbUserRechargeRecord.OperateTime.ToString(),
                                    ExamineRemarks=  tbUserRechargeRecord.ExamineRemarks,
                                    operateIP=tbUserRechargeRecord.operateIP
                                }).ToList();
                //总行数
                int intTotalRow = Recharge.Count();
                for (int i = 0; i < Recharge.Count(); i++)
                {
                    Recharge[i].strRechargeMoney = "￥" + Convert.ToString(Recharge[i].RechargeMoney.ToString());
                    Recharge[i].strRechargePoundage = "￥" + Convert.ToString(Recharge[i].RechargePoundage.ToString());
                    Recharge[i].strRealityAccountMoney = "￥" + Convert.ToString(Recharge[i].RealityAccountMoney.ToString());
                  
                }
                List<rechargemanageVo> listJieShouUser = Recharge.OrderBy(p => p.RechargeRecordID).Skip(bsgridPages.GetStartIndex()).Take(bsgridPages.pageSize).ToList();
                Bsgrid<rechargemanageVo> bsgrid = new Bsgrid<rechargemanageVo>();
                bsgrid.success = true;
                bsgrid.curPage = bsgridPages.curPage;
                bsgrid.totalRows = intTotalRow;
                bsgrid.data = listJieShouUser;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);            
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 提现记录按钮
        /// </summary>
        /// <param name="bsgridPages"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult selectTixianjilu(BsgridPage bsgridPages, int UserID)
        {
            try
            {
                var Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                                join tbuser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbuser.UserID
                                join tb_PayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tb_PayType.PayTypeID
                                join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                                join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                                join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                                orderby tbUserCashRecord.UserCashID
                                where tbUserCashRecord.UserID == UserID
                                select new CashregisterVo
                                {
                                    UserCashID = tbUserCashRecord.UserCashID,
                                    UserName = tbuser.UserName,
                                    PayTypeName = tb_PayType.PayTypeName,
                                    Subbranch = tbUserCashRecord.Subbranch,
                                    ProNameCityName = tbProvince.ProName + tbCity.CityName,
                                    CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                                    CashAmount = tbUserCashRecord.CashAmount,
                                    AccountMoney = tbUserCashRecord.AccountMoney,
                                    Poundage = tbUserCashRecord.Poundage,
                                    ReleaseTimeStr=tbUserCashRecord.CashTime.ToString(),
                                    StatusName=tbStatus.StatusName,
                                    IP=tbUserCashRecord.IP,
                                    ExamineRemarks=  tbUserCashRecord.ExamineRemarks
                                }).ToList();
                //总行数
                int intTotalRow = Recharge.Count();

                 for (int i = 0; i < Recharge.Count(); i++)
                {
                    Recharge[i].strCashAmount = "￥" + Convert.ToString(Recharge[i].CashAmount.ToString());
                    Recharge[i].strAccountMoney = "￥" + Convert.ToString(Recharge[i].AccountMoney.ToString());
                    Recharge[i].strPoundage ="￥"+Recharge[i].Poundage;                    
                }
                List<CashregisterVo> listJieShouUser = Recharge.Skip(bsgridPages.GetStartIndex()).Take(bsgridPages.pageSize).ToList();
                Bsgrid<CashregisterVo> bsgrid = new Bsgrid<CashregisterVo>();
                bsgrid.success = true;
                bsgrid.curPage = bsgridPages.curPage;
                bsgrid.totalRows = intTotalRow;
                bsgrid.data = listJieShouUser;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 资金记录按钮
        /// </summary>
        /// <param name="bsgridPages"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult selectzujinjilu(BsgridPage bsgridPages, int UserID)
        {
            try
            {
                var Recharge = (from tbCapitalrecord in myDYXTEntities.B_CapitalrecordTable
                                join tbuser in myDYXTEntities.B_UserTable on tbCapitalrecord.UserID equals tbuser.UserID
                                join tbOperateType in myDYXTEntities.S_OperateTypeTable on tbCapitalrecord.OperateTypeID equals tbOperateType.OperateTypeID
                                where tbCapitalrecord.UserID== UserID
                                select new B_CapitalrecordVo
                                {
                                    CapitalrecordID = tbCapitalrecord.CapitalrecordID,
                                    UserName = tbuser.UserName,
                                    OperateTypeName = tbOperateType.OperateTypeName,
                                    OperateTypeID = tbCapitalrecord.OperateTypeID,
                                    OpFare = tbCapitalrecord.OpFare,
                                    Income = tbCapitalrecord.Income,
                                    Expend = tbCapitalrecord.Expend,
                                    PropertyAmounts = tbCapitalrecord.PropertyAmounts,
                                    Remarks = tbCapitalrecord.Remarks,
                                    ReleaseTimeStr = tbCapitalrecord.operatetime.ToString()
                                }).ToList();
                //总行数
                int intTotalRow = Recharge.Count();
                for (int i = 0; i < Recharge.Count(); i++)
                {
                    Recharge[i].strOpFare = "￥" + Convert.ToString(Recharge[i].OpFare.ToString());
                    Recharge[i].strIncome = "￥" + Convert.ToString(Recharge[i].Income.ToString());
                    Recharge[i].strExpend = "￥" + Convert.ToString(Recharge[i].Expend.ToString());
                    Recharge[i].strPropertyAmounts = "￥" + Convert.ToString(Recharge[i].PropertyAmounts.ToString());
                   
                }
                List<B_CapitalrecordVo> listJieShouUser = Recharge.Skip(bsgridPages.GetStartIndex()).Take(bsgridPages.pageSize).ToList();
                Bsgrid<B_CapitalrecordVo> bsgrid = new Bsgrid<B_CapitalrecordVo>();
                bsgrid.success = true;
                bsgrid.curPage = bsgridPages.curPage;
                bsgrid.totalRows = intTotalRow;
                bsgrid.data = listJieShouUser;
                return Json(bsgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 金额总数
        /// </summary>
        /// <returns></returns>
        public ActionResult countmoeny() {
            var user = (from tbuser in myDYXTEntities.B_UserTable
                        select new {
                            tbuser.UsableMoney,
                            tbuser.FreezeMoney
                        }).ToList();
            decimal dfs=0;
            decimal kk = 0;
            for (int i = 0; i < user.Count; i++)
            {
                kk = Convert.ToDecimal(user[i].UsableMoney);
                dfs += kk;
            }
            decimal YY = 0;
            decimal JJ = 0;
            for (int i = 0; i < user.Count; i++)
            {
                JJ = Convert.ToDecimal(user[i].FreezeMoney);
                YY += JJ;
            }
            return Json(new { dfs, YY }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 充值金额
        /// </summary>
        /// <returns></returns>
        public ActionResult chognzhimoeny()
        {
            var moeny = (from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                        select new
                        {
                            tbUserRechargeRecord.RechargeMoney,
                            tbUserRechargeRecord.RechargePoundage
                        }).ToList();
            decimal dfs = 0;
            decimal kk = 0;
            for (int i = 0; i < moeny.Count; i++)
            {
                kk = Convert.ToDecimal(moeny[i].RechargeMoney);
                dfs += kk;
            }
            decimal YY = 0;
            decimal JJ = 0;
            for (int i = 0; i < moeny.Count; i++)
            {
                JJ = Convert.ToDecimal(moeny[i].RechargePoundage);
                YY += JJ;
            }
            return Json(new { dfs, YY }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 导出用户资金数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ExExcel()
        {
            var notict = from tbUser in myDYXTEntities.B_UserTable
                         orderby tbUser.UserID
                         select new FundsVo
                         {
                             UserID = tbUser.UserID,
                             UserName = tbUser.UserName,
                             PropertyAmounts = tbUser.PropertyAmounts,
                             UsableMoney = tbUser.UsableMoney,
                             FreezeMoney = tbUser.FreezeMoney,
                             WaitMoney = tbUser.WaitMoney,
                             CompensatoryMoney = tbUser.CompensatoryMoney
                         };
            //查询数据
            List<FundsVo> listExaminee = notict.ToList();
            //创建Excel对象 工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook excelBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("资金账户管理");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题
            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("用户名");//1
            row1.CreateCell(2).SetCellValue("资产总额");//2
            row1.CreateCell(3).SetCellValue("可用金额");//3
            row1.CreateCell(4).SetCellValue("冻结金额");//4
            row1.CreateCell(5).SetCellValue("待收金额");//5
            row1.CreateCell(6).SetCellValue("待还金额");//6   
                                                    //将数据逐步写入sheet1各个行
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);


                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].UserID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(Convert.ToDouble( listExaminee[i].PropertyAmounts));

                rowTemp.CreateCell(3).SetCellValue(Convert.ToDouble(listExaminee[i].UsableMoney));

                rowTemp.CreateCell(4).SetCellValue(Convert.ToDouble(listExaminee[i].FreezeMoney));

                rowTemp.CreateCell(5).SetCellValue(Convert.ToDouble(listExaminee[i].WaitMoney));

                rowTemp.CreateCell(6).SetCellValue(Convert.ToDouble(listExaminee[i].CompensatoryMoney));           
            }
            //输出的文件名称
            string fileName = "用户资金信息" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

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
        /// 资金记录管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Capitalrecordview()
        {

            return View();
        }
        /// <summary>
        /// 资金记录查看
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="noticeName"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <param name="academeId"></param>
        /// <returns></returns>
        public ActionResult selectCapitalrecord(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime, int academeId) {

            var Recharge = (from tbCapitalrecord in myDYXTEntities.B_CapitalrecordTable
                            join tbuser in myDYXTEntities.B_UserTable on tbCapitalrecord.UserID equals tbuser.UserID
                            join tbOperateType in myDYXTEntities.S_OperateTypeTable on tbCapitalrecord.OperateTypeID equals tbOperateType.OperateTypeID
                            select new B_CapitalrecordVo
                            {
                                CapitalrecordID = tbCapitalrecord.CapitalrecordID,
                                UserName = tbuser.UserName,
                                OperateTypeName = tbOperateType.OperateTypeName,
                                OperateTypeID = tbCapitalrecord.OperateTypeID,
                                OpFare = tbCapitalrecord.OpFare,
                                Income = tbCapitalrecord.Income,
                                Expend = tbCapitalrecord.Expend,
                                PropertyAmounts = tbCapitalrecord.PropertyAmounts,
                                Remarks = tbCapitalrecord.Remarks,
                                ReleaseTimeStr = tbCapitalrecord.operatetime.ToString()
                            }).OrderByDescending(p => p.CapitalrecordID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strOpFare = "￥" + Convert.ToString(Recharge[i].OpFare.ToString());
                Recharge[i].strIncome = "￥" + Convert.ToString(Recharge[i].Income.ToString());
                Recharge[i].strExpend = "￥" + Convert.ToString(Recharge[i].Expend.ToString());
                Recharge[i].strPropertyAmounts = "￥" + Convert.ToString(Recharge[i].PropertyAmounts.ToString());

            }


            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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
            if (academeId > 0)
            {
                Recharge = Recharge.Where(s => s.OperateTypeID == academeId).ToList();
            }
            int totalRow = myDYXTEntities.B_CapitalrecordTable.Count();

            Bsgrid<B_CapitalrecordVo> bsgrid = new Bsgrid<B_CapitalrecordVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 资金统计
        /// </summary>
        /// <returns></returns>
        public ActionResult selectcountmoeny()
        {
            var user = (from tbuser in myDYXTEntities.B_CapitalrecordTable
                        select new
                        {
                            tbuser.OpFare,
                            tbuser.Income,
                            tbuser.Expend
                        }).ToList();
            decimal dfs = 0;
            decimal kk = 0;
            for (int i = 0; i < user.Count; i++)
            {
                kk = Convert.ToDecimal(user[i].OpFare);
                dfs += kk;
            }
            decimal YY = 0;
            decimal JJ = 0;
            for (int i = 0; i < user.Count; i++)
            {
                JJ = Convert.ToDecimal(user[i].Income);
                YY += JJ;
            }
            decimal aa = 0;
            decimal bb = 0;
            for (int i = 0; i < user.Count; i++)
            {
                aa = Convert.ToDecimal(user[i].Expend);
                bb += aa;
            }
            return Json(new { dfs, YY, bb }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 资金类型
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctOperateType()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "请选择"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbnotice in myDYXTEntities.S_OperateTypeTable
                                         select new SelectVo
                                         {
                                             id = tbnotice.OperateTypeID,
                                             text = tbnotice.OperateTypeName

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 账号管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult accountnumber() {

            return View();
        }
        /// <summary>
        /// 用户账户列表
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectaccountnumber(BsgridPage bsgridPage,string UserName,int UserID)
        {
            var linquser = from tbuser in myDYXTEntities.B_UserTable
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbuser.PayTypeID equals tbPayType.PayTypeID
                           join tbProvince in myDYXTEntities.T_Province on tbuser.ProID equals tbProvince.ProID 
                           join tbCity in myDYXTEntities.T_City on tbuser.CityID equals tbCity.CityID
                           select new USerbankVo
                           {
                             UserID=tbuser.UserID,
                             UserName= tbuser.UserName,
                             TrueName= tbuser.TrueName,
                             PayTypeName= tbPayType.PayTypeName,                           
                             ProNameCityName= tbProvince.ProName + tbCity.CityName,
                             Subbranch=tbuser.Subbranch,
                             BankAccount= tbuser.BankAccount
                           };

            if (!string.IsNullOrEmpty(UserName))
            {
                linquser = linquser.Where(n => n.UserName.Contains(UserName));
            }
            if (UserID > 0)
            {
                linquser = linquser.Where(p => p.UserID == UserID);
            }
            int totalRow = linquser.Count();
            List<USerbankVo> notices = linquser.OrderByDescending(p => p.UserID).
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
        /// 所属银行
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctPayType()
        {

            List<SelectVo> Province = (from tbPayType in myDYXTEntities.S_PayTypeTable
                                       select new SelectVo
                                       {
                                           id = tbPayType.PayTypeID,
                                           text = tbPayType.PayTypeName

                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 省份下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctPro()
        {
           
            List<SelectVo> Province = (from tbProvince in myDYXTEntities.T_Province
                                         select new SelectVo
                                         {
                                             id = tbProvince.ProID,
                                             text = tbProvince.ProName

                                         }).ToList();
          
            return Json(Province, JsonRequestBehavior.AllowGet);
        }
       
        /// <summary>
        /// 根据省份类型ID查询城市类型明细ID
        /// </summary>
        /// <param name="noticeTypeId"></param>
        /// <returns></returns>
        public ActionResult SelectCityDetail(int varProNameID)
        {
            List<SelectVo> City = (from tbCity in myDYXTEntities.T_City
                                         where tbCity.ProID == varProNameID
                                   select new SelectVo
                                         {
                                             id = tbCity.CityID,
                                             text = tbCity.CityName
                                         }).ToList();
            return Json(City, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改用户银行账户的绑定
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult updatabanding(int UserID) {

            try
            {
                var userbangding = (from tbuser in myDYXTEntities.B_UserTable
                                    join tbPayType in myDYXTEntities.S_PayTypeTable on tbuser.PayTypeID equals tbPayType.PayTypeID
                                    join tbProvince in myDYXTEntities.T_Province on tbuser.ProID equals tbProvince.ProID
                                    join tbCity in myDYXTEntities.T_City on tbuser.CityID equals tbCity.CityID
                                    where tbuser.UserID == UserID
                                    select new USerbankVo
                                    {
                                        UserID = tbuser.UserID,
                                        UserName = tbuser.UserName,
                                        TrueName = tbuser.TrueName,
                                        PayTypeID = tbuser.PayTypeID,
                                        PayTypeName = tbPayType.PayTypeName,
                                        ProID= tbuser.ProID,
                                        CityID= tbuser.CityID,
                                        ProName =  tbProvince.ProName,
                                        CityName=   tbCity.CityName,
                                        Subbranch = tbuser.Subbranch.Trim(),
                                        BankAccount = tbuser.BankAccount.Trim()

                                    }).Single();

                return Json(userbangding, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateYinhangxinxi(B_UserTable UserTable)
        {
            //定义返回
            string strMsg = "fail";
            if (!string.IsNullOrEmpty(UserTable.BankAccount))
            {
                //查询除了自身外 公告类型名称查询是否已经存在
                int oldCount = (from tbUserTable in myDYXTEntities.B_UserTable
                                where tbUserTable.UserID != UserTable.UserID
                                && tbUserTable.BankAccount == UserTable.BankAccount.Trim()
                                select tbUserTable).Count();
                if (oldCount == 0)
                {
                    try
                    {
                        B_UserTable varuser = (from tbuser in myDYXTEntities.B_UserTable
                                               where tbuser.UserID == UserTable.UserID
                                               select tbuser).Single();
                        varuser.ProID = UserTable.ProID;
                        varuser.CityID = UserTable.CityID;
                        varuser.PayTypeID = UserTable.PayTypeID;
                        varuser.Subbranch = UserTable.Subbranch;
                        varuser.BankAccount = UserTable.BankAccount;
                        //修改公告类型
                        myDYXTEntities.Entry(varuser).State = System.Data.Entity.EntityState.Modified;
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
        /// 充值管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Rechargemanagement() {

            return View();
        }
        /// <summary>
        /// 充值记录查询全部
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
       public ActionResult Rechargeall(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime, int academeId) {

            var Recharge = (from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                         join tbUser in myDYXTEntities.B_UserTable on tbUserRechargeRecord.UserID equals tbUser.UserID
                         join tbRechargeType in myDYXTEntities.S_RechargeTypeTable on tbUserRechargeRecord.RechargeTypeID equals tbRechargeType.RechargeTypeID
                         join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserRechargeRecord.PayTypeID equals tbPayType.PayTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbUserRechargeRecord.StatusID equals tbStatus.StatusID
                         select new rechargemanageVo
                         {
                           RechargeRecordID=tbUserRechargeRecord.RechargeRecordID,
                           UserName= tbUser.UserName,
                            TradeNumber= tbUserRechargeRecord.TradeNumber,
                             RechargeTypeID=tbUserRechargeRecord.RechargeTypeID,
                            RechargeTypeName =  tbRechargeType.RechargeTypeName,
                             PayTypeName=  tbPayType.PayTypeName,
                             RechargeMoney=tbUserRechargeRecord.RechargeMoney,
                             RechargePoundage= tbUserRechargeRecord.RechargePoundage,
                             RealityAccountMoney= tbUserRechargeRecord.RealityAccountMoney,
                             StatusName=tbStatus.StatusName,
                             ReleaseTimeStr = tbUserRechargeRecord.OperateTime.ToString(),
                             operateIP=tbUserRechargeRecord.operateIP

                         }).OrderByDescending(p => p.RechargeRecordID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strRechargeMoney = "￥" + Convert.ToString(Recharge[i].RechargeMoney.ToString());
                Recharge[i].strRealityAccountMoney = "￥" + Convert.ToString(Recharge[i].RealityAccountMoney.ToString());
                Recharge[i].strRechargePoundage = "￥" + Convert.ToString(Recharge[i].RechargePoundage.ToString());              
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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
            if (academeId > 0)
            {
                Recharge = Recharge.Where(s => s.RechargeTypeID == academeId).ToList();
            }
            int totalRow = myDYXTEntities.B_UserRechargeRecordTable.Count();
           
            Bsgrid<rechargemanageVo> bsgrid = new Bsgrid<rechargemanageVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);           
        }
        /// <summary>
        /// 充值记录查询审核中
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult binreview(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime, int academeId)
        {

            var Recharge = (from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                           join tbUser in myDYXTEntities.B_UserTable on tbUserRechargeRecord.UserID equals tbUser.UserID
                           join tbRechargeType in myDYXTEntities.S_RechargeTypeTable on tbUserRechargeRecord.RechargeTypeID equals tbRechargeType.RechargeTypeID
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserRechargeRecord.PayTypeID equals tbPayType.PayTypeID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbUserRechargeRecord.StatusID equals tbStatus.StatusID
                           where tbUserRechargeRecord.StatusID==3
                           select new rechargemanageVo
                           {
                               RechargeRecordID = tbUserRechargeRecord.RechargeRecordID,
                               UserName = tbUser.UserName,
                               TradeNumber = tbUserRechargeRecord.TradeNumber,
                               RechargeTypeID = tbUserRechargeRecord.RechargeTypeID,
                               RechargeTypeName = tbRechargeType.RechargeTypeName,
                               PayTypeName = tbPayType.PayTypeName,
                               RechargeMoney = tbUserRechargeRecord.RechargeMoney,
                               RechargePoundage = tbUserRechargeRecord.RechargePoundage,
                               RealityAccountMoney = tbUserRechargeRecord.RealityAccountMoney,
                               StatusName = tbStatus.StatusName,
                               ReleaseTimeStr = tbUserRechargeRecord.OperateTime.ToString(),
                               operateIP = tbUserRechargeRecord.operateIP

                           }).OrderByDescending(p => p.RechargeRecordID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strRechargeMoney = "￥" + Convert.ToString(Recharge[i].RechargeMoney.ToString());
                Recharge[i].strRealityAccountMoney = "￥" + Convert.ToString(Recharge[i].RealityAccountMoney.ToString());
                Recharge[i].strRechargePoundage = "￥" + Convert.ToString(Recharge[i].RechargePoundage.ToString());
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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

            if (academeId > 0)
            {
                Recharge = Recharge.Where(s => s.RechargeTypeID == academeId).ToList();
            }
            int totalRow = myDYXTEntities.B_UserRechargeRecordTable.Count();
         
            Bsgrid<rechargemanageVo> bsgrid = new Bsgrid<rechargemanageVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 充值记录查询充值成功
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult Rechargesuccessfully(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime,int academeId)
        {

            var Recharge = (from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                           join tbUser in myDYXTEntities.B_UserTable on tbUserRechargeRecord.UserID equals tbUser.UserID
                           join tbRechargeType in myDYXTEntities.S_RechargeTypeTable on tbUserRechargeRecord.RechargeTypeID equals tbRechargeType.RechargeTypeID
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserRechargeRecord.PayTypeID equals tbPayType.PayTypeID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbUserRechargeRecord.StatusID equals tbStatus.StatusID
                           where tbUserRechargeRecord.StatusID == 16
                           select new rechargemanageVo
                           {
                               RechargeRecordID = tbUserRechargeRecord.RechargeRecordID,
                               UserName = tbUser.UserName,
                               TradeNumber = tbUserRechargeRecord.TradeNumber,
                               RechargeTypeID = tbUserRechargeRecord.RechargeTypeID,
                               RechargeTypeName = tbRechargeType.RechargeTypeName,
                               PayTypeName = tbPayType.PayTypeName,
                               RechargeMoney = tbUserRechargeRecord.RechargeMoney,
                               RechargePoundage = tbUserRechargeRecord.RechargePoundage,
                               RealityAccountMoney = tbUserRechargeRecord.RealityAccountMoney,
                               StatusName = tbStatus.StatusName,
                               ReleaseTimeStr = tbUserRechargeRecord.OperateTime.ToString(),
                               operateIP = tbUserRechargeRecord.operateIP

                           }).OrderByDescending(p => p.RechargeRecordID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strRechargeMoney = "￥" + Convert.ToString(Recharge[i].RechargeMoney.ToString());
                Recharge[i].strRealityAccountMoney = "￥" + Convert.ToString(Recharge[i].RealityAccountMoney.ToString());
                Recharge[i].strRechargePoundage = "￥" + Convert.ToString(Recharge[i].RechargePoundage.ToString());
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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

            if (academeId > 0)
            {
                Recharge = Recharge.Where(s => s.RechargeTypeID == academeId).ToList();
            }
            int totalRow = myDYXTEntities.B_UserRechargeRecordTable.Count();         
            Bsgrid<rechargemanageVo> bsgrid = new Bsgrid<rechargemanageVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 充值记录查询充值失败
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult Rechargefailed(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime, int academeId)
        {

            var Recharge =(from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                           join tbUser in myDYXTEntities.B_UserTable on tbUserRechargeRecord.UserID equals tbUser.UserID
                           join tbRechargeType in myDYXTEntities.S_RechargeTypeTable on tbUserRechargeRecord.RechargeTypeID equals tbRechargeType.RechargeTypeID
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserRechargeRecord.PayTypeID equals tbPayType.PayTypeID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbUserRechargeRecord.StatusID equals tbStatus.StatusID
                           where tbUserRechargeRecord.StatusID == 17
                           select new rechargemanageVo
                           {
                               RechargeRecordID = tbUserRechargeRecord.RechargeRecordID,
                               UserName = tbUser.UserName,
                               TradeNumber = tbUserRechargeRecord.TradeNumber,
                               RechargeTypeID = tbUserRechargeRecord.RechargeTypeID,
                               RechargeTypeName = tbRechargeType.RechargeTypeName,
                               PayTypeName = tbPayType.PayTypeName,
                               RechargeMoney = tbUserRechargeRecord.RechargeMoney,
                               RechargePoundage = tbUserRechargeRecord.RechargePoundage,
                               RealityAccountMoney = tbUserRechargeRecord.RealityAccountMoney,
                               StatusName = tbStatus.StatusName,
                               ReleaseTimeStr = tbUserRechargeRecord.OperateTime.ToString(),
                               operateIP = tbUserRechargeRecord.operateIP

                           }).OrderByDescending(p => p.RechargeRecordID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strRechargeMoney = "￥" + Convert.ToString(Recharge[i].RechargeMoney.ToString());
                Recharge[i].strRealityAccountMoney = "￥" + Convert.ToString(Recharge[i].RealityAccountMoney.ToString());
                Recharge[i].strRechargePoundage = "￥" + Convert.ToString(Recharge[i].RechargePoundage.ToString());
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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
            if (academeId > 0)
            {
                Recharge = Recharge.Where(s => s.RechargeTypeID == academeId).ToList();
            }
            int totalRow = myDYXTEntities.B_UserRechargeRecordTable.Count();        
            Bsgrid<rechargemanageVo> bsgrid = new Bsgrid<rechargemanageVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改审核绑定
        /// </summary>
        /// <param name="RechargeRecordID"></param>
        /// <returns></returns>
        public ActionResult updatabangding(int RechargeRecordID) {

            try
            {
                var Recharge = (from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                               join tbUser in myDYXTEntities.B_UserTable on tbUserRechargeRecord.UserID equals tbUser.UserID
                               join tbRechargeType in myDYXTEntities.S_RechargeTypeTable on tbUserRechargeRecord.RechargeTypeID equals tbRechargeType.RechargeTypeID
                               join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserRechargeRecord.PayTypeID equals tbPayType.PayTypeID
                               join tbStatus in myDYXTEntities.S_StatusTable on tbUserRechargeRecord.StatusID equals tbStatus.StatusID
                               where tbUserRechargeRecord.RechargeRecordID == RechargeRecordID
                               select new rechargemanageVo
                               {
                                   RechargeRecordID = tbUserRechargeRecord.RechargeRecordID,
                                   UserName = tbUser.UserName,
                                   UserID = tbUserRechargeRecord.UserID,
                                   PayTypeID=tbUserRechargeRecord.PayTypeID,
                                   RechargeTypeID= tbUserRechargeRecord.RechargeTypeID,
                                   StatusID= tbUserRechargeRecord.StatusID,
                                   TradeNumber = tbUserRechargeRecord.TradeNumber,
                                   RechargeTypeName = tbRechargeType.RechargeTypeName,
                                   PayTypeName = tbPayType.PayTypeName,
                                   RechargeMoney = tbUserRechargeRecord.RechargeMoney,
                                   RechargePoundage = tbUserRechargeRecord.RechargePoundage,
                                   RealityAccountMoney = tbUserRechargeRecord.RealityAccountMoney,                                
                                   StatusName = tbStatus.StatusName,
                                   ReleaseTimeStr = tbUserRechargeRecord.OperateTime.ToString(),
                                   operateIP = tbUserRechargeRecord.operateIP,
                                   ExamineRemarks=tbUserRechargeRecord.ExamineRemarks.Trim()

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
        /// 修改审核保存
        /// </summary>
        /// <param name="UserRechargeRecord"></param>
        /// <returns></returns>
        public ActionResult updatachogzhi(B_UserRechargeRecordTable UserRechargeRecord,string optionsRadios, DateTime ReleaseTimeStr) {
            //定义返回
            string strMsg = "fail";

            if (optionsRadios== "option1")
            {
                UserRechargeRecord.StatusID = 16;
                UserRechargeRecord.OperateTime = ReleaseTimeStr;
                var usermoney = (from tbuser in myDYXTEntities.B_UserTable
                                 where tbuser.UserID == UserRechargeRecord.UserID
                                 select tbuser).Single();
                if (usermoney.PropertyAmounts == null)
                {
                    usermoney.PropertyAmounts = UserRechargeRecord.RealityAccountMoney;

                }
                else
                {
                    usermoney.PropertyAmounts = usermoney.PropertyAmounts + UserRechargeRecord.RealityAccountMoney;

                }
                if (usermoney.UsableMoney == null)
                {

                    usermoney.UsableMoney = UserRechargeRecord.RealityAccountMoney;
                }
                else
                {
                    usermoney.UsableMoney = usermoney.UsableMoney + UserRechargeRecord.RealityAccountMoney;
                }
                myDYXTEntities.Entry(UserRechargeRecord).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {
                    B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                    WebsiteExpenses.AccountID = Convert.ToInt32(Session["AccountID"]);
                    WebsiteExpenses.OperateTypeID = 7;
                    WebsiteExpenses.OperateMoney =Convert.ToDecimal(UserRechargeRecord.RechargePoundage);
                    WebsiteExpenses.Earning= Convert.ToDecimal(UserRechargeRecord.RechargePoundage);
                    WebsiteExpenses.Expenses =Convert.ToDecimal(0);
                    WebsiteExpenses.Remark = "用户正常充值," + "扣除" + "[" + usermoney.UserName.Trim() + "]" + UserRechargeRecord.RechargePoundage.Trim() + "元充值手续费";
                    WebsiteExpenses.OperateTime = DateTime.Now;
                    myDYXTEntities.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                    myDYXTEntities.SaveChanges();

                    B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                    UserExpense.UserID = UserRechargeRecord.UserID;
                    UserExpense.OperateTypeID = 7;
                    UserExpense.OperateMoney= Convert.ToDecimal(UserRechargeRecord.RealityAccountMoney);
                    UserExpense.Balance = usermoney.PropertyAmounts;
                    UserExpense.Earning= Convert.ToDecimal(UserRechargeRecord.RealityAccountMoney);
                    UserExpense.Expenses= Convert.ToDecimal(0);
                    UserExpense.Remark= "用户成功正常充值," +"[" + usermoney.UserName.Trim() + "]" +"收入" + UserRechargeRecord.RealityAccountMoney + "元";
                    UserExpense.OperateTime = DateTime.Now;
                    myDYXTEntities.B_UserExpensesTable.Add(UserExpense);
                    myDYXTEntities.SaveChanges();

                    B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                    Capitalrecord.UserID= UserRechargeRecord.UserID;
                    Capitalrecord.OperateTypeID = 7;
                    Capitalrecord.OpFare= Convert.ToDecimal(UserRechargeRecord.RealityAccountMoney);
                    Capitalrecord.Income= Convert.ToDecimal(UserRechargeRecord.RealityAccountMoney);
                    Capitalrecord.Expend= Convert.ToDecimal(0);
                    Capitalrecord.PropertyAmounts= usermoney.PropertyAmounts;
                    Capitalrecord.Remarks= "用户成功正常充值," + "[" + usermoney.UserName.Trim() + "]" + "收入" + UserRechargeRecord.RealityAccountMoney + "元";
                    Capitalrecord.operatetime = DateTime.Now;
                    myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                    myDYXTEntities.SaveChanges();
                    strMsg = "success";
                }
            }
            else
            {
                UserRechargeRecord.StatusID = 17;
                UserRechargeRecord.OperateTime = ReleaseTimeStr;
                myDYXTEntities.Entry(UserRechargeRecord).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {
                    strMsg = "success";
                }
            }

            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 导出充值记录
        /// </summary>
        /// <returns></returns>
        public ActionResult selectExcel() {
            var chongzhi = from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                         join tbUser in myDYXTEntities.B_UserTable on tbUserRechargeRecord.UserID equals tbUser.UserID
                         join tbRechargeType in myDYXTEntities.S_RechargeTypeTable on tbUserRechargeRecord.RechargeTypeID equals tbRechargeType.RechargeTypeID
                         join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserRechargeRecord.PayTypeID equals tbPayType.PayTypeID
                         join tbStatus in myDYXTEntities.S_StatusTable on tbUserRechargeRecord.StatusID equals tbStatus.StatusID
                         orderby tbUserRechargeRecord.RechargeRecordID
                         select new rechargemanageVo
                         {
                             RechargeRecordID = tbUserRechargeRecord.RechargeRecordID,
                             UserName = tbUser.UserName,
                             TradeNumber = tbUserRechargeRecord.TradeNumber,
                             RechargeTypeName = tbRechargeType.RechargeTypeName,
                             PayTypeName = tbPayType.PayTypeName,
                             RechargeMoney = tbUserRechargeRecord.RechargeMoney,
                             RechargePoundage = tbUserRechargeRecord.RechargePoundage,
                             RealityAccountMoney = tbUserRechargeRecord.RealityAccountMoney,
                             StatusName = tbStatus.StatusName,
                             ReleaseTimeStr = tbUserRechargeRecord.OperateTime.ToString(),
                             operateIP = tbUserRechargeRecord.operateIP

                         };
            //查询数据
            List<rechargemanageVo> listExaminee = chongzhi.ToList();
            //创建Excel对象 工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook excelBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("充值记录");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题
            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("用户名");//1
            row1.CreateCell(2).SetCellValue("交易号");//2
            row1.CreateCell(3).SetCellValue("充值类型");//3
            row1.CreateCell(4).SetCellValue("充值银行");//4
            row1.CreateCell(5).SetCellValue("充值金额");//5
            row1.CreateCell(6).SetCellValue("充值手续费");//6   
            row1.CreateCell(7).SetCellValue("实际到账金额");//6   
            row1.CreateCell(8).SetCellValue("状态");//6   
            row1.CreateCell(9).SetCellValue("提交时间");//6   
            row1.CreateCell(10).SetCellValue("操作Ip");//6   
                                                    //将数据逐步写入sheet1各个行
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);


                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].RechargeRecordID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(listExaminee[i].TradeNumber);

                rowTemp.CreateCell(3).SetCellValue(listExaminee[i].RechargeTypeName);

                rowTemp.CreateCell(4).SetCellValue(listExaminee[i].PayTypeName);

                rowTemp.CreateCell(5).SetCellValue(Convert.ToDouble(listExaminee[i].RechargeMoney));

                rowTemp.CreateCell(6).SetCellValue(Convert.ToDouble(listExaminee[i].RechargePoundage));
                rowTemp.CreateCell(7).SetCellValue(Convert.ToDouble(listExaminee[i].RealityAccountMoney));
                rowTemp.CreateCell(8).SetCellValue(listExaminee[i].StatusName);
                rowTemp.CreateCell(9).SetCellValue(Convert.ToDateTime(listExaminee[i].ReleaseTimeStr));
                rowTemp.CreateCell(10).SetCellValue(listExaminee[i].operateIP);
            }
            //输出的文件名称
            string fileName = "用户充值信息" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

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
        /// 充值导出当前EXcel
        /// </summary>
        /// <param name="noticeName"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult withdrawdepositExcel(string noticeName, string releaseTime, string EnReleaseTime,int academeId)
        {
            var Recharge = (from tbUserRechargeRecord in myDYXTEntities.B_UserRechargeRecordTable
                            join tbUser in myDYXTEntities.B_UserTable on tbUserRechargeRecord.UserID equals tbUser.UserID
                            join tbRechargeType in myDYXTEntities.S_RechargeTypeTable on tbUserRechargeRecord.RechargeTypeID equals tbRechargeType.RechargeTypeID
                            join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserRechargeRecord.PayTypeID equals tbPayType.PayTypeID
                            join tbStatus in myDYXTEntities.S_StatusTable on tbUserRechargeRecord.StatusID equals tbStatus.StatusID
                            orderby tbUserRechargeRecord.RechargeRecordID
                            select new rechargemanageVo
                            {
                                RechargeRecordID = tbUserRechargeRecord.RechargeRecordID,
                                UserName = tbUser.UserName,
                                TradeNumber = tbUserRechargeRecord.TradeNumber,
                                RechargeTypeID = tbUserRechargeRecord.RechargeTypeID,
                                RechargeTypeName = tbRechargeType.RechargeTypeName,
                                PayTypeName = tbPayType.PayTypeName,
                                RechargeMoney = tbUserRechargeRecord.RechargeMoney,
                                RechargePoundage = tbUserRechargeRecord.RechargePoundage,
                                RealityAccountMoney = tbUserRechargeRecord.RealityAccountMoney,
                                StatusName = tbStatus.StatusName,
                                ReleaseTimeStr = tbUserRechargeRecord.OperateTime.ToString(),
                                operateIP = tbUserRechargeRecord.operateIP
                            }).ToList();

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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

            if (academeId > 0)
            {
                Recharge = Recharge.Where(s => s.RechargeTypeID == academeId).ToList();
            }
            //查询数据
            List<rechargemanageVo> listExaminee = Recharge.ToList();

            //创建Excel对象 工作簿(调用NPOI文件)
            HSSFWorkbook excelBook = new HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("用户提现信息");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题

            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("用户名");//1
            row1.CreateCell(2).SetCellValue("交易号");//2
            row1.CreateCell(3).SetCellValue("充值类型");//3
            row1.CreateCell(4).SetCellValue("充值银行");//4
            row1.CreateCell(5).SetCellValue("充值金额");//5
            row1.CreateCell(6).SetCellValue("充值手续费");//6   
            row1.CreateCell(7).SetCellValue("实际到账金额");//6   
            row1.CreateCell(8).SetCellValue("状态");//6   
            row1.CreateCell(9).SetCellValue("提交时间");//6   
            row1.CreateCell(10).SetCellValue("操作Ip");//6    
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);


                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].RechargeRecordID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(listExaminee[i].TradeNumber);

                rowTemp.CreateCell(3).SetCellValue(listExaminee[i].RechargeTypeName);

                rowTemp.CreateCell(4).SetCellValue(listExaminee[i].PayTypeName);

                rowTemp.CreateCell(5).SetCellValue(Convert.ToDouble(listExaminee[i].RechargeMoney));

                rowTemp.CreateCell(6).SetCellValue(Convert.ToDouble(listExaminee[i].RechargePoundage));
                rowTemp.CreateCell(7).SetCellValue(Convert.ToDouble(listExaminee[i].RealityAccountMoney));
                rowTemp.CreateCell(8).SetCellValue(listExaminee[i].StatusName);
                rowTemp.CreateCell(9).SetCellValue(Convert.ToDateTime(listExaminee[i].ReleaseTimeStr));
                rowTemp.CreateCell(10).SetCellValue(listExaminee[i].operateIP);
            }

            //输出的文件名称：文件名称+时间+文件类型
            string fileName = "学生安全教育测试成绩" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

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
        /// 查询状态
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctStatusType()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "请选择"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbnotice in myDYXTEntities.S_StatusTable
                                         select new SelectVo
                                         {
                                             id = tbnotice.StatusID,
                                             text = tbnotice.StatusName

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 查询类型
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctType()
        {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "请选择"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbnotice in myDYXTEntities.S_RechargeTypeTable
                                         select new SelectVo
                                         {
                                             id = tbnotice.RechargeTypeID,
                                             text = tbnotice.RechargeTypeName

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 提现管理
        /// </summary>
        /// <returns></returns>
        public ActionResult withdrawdeposit() {

            return View();
        }

        /// <summary>
        /// 提现记录查询全部
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult Cashwithdrawal(BsgridPage bsgridPage, string noticeName, string releaseTime,string EnReleaseTime)
        {

            List<CashregisterVo> Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                                             join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                                             join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                                             join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                                             join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                                             join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                                             select new CashregisterVo
                                             {
                                                 UserCashID = tbUserCashRecord.UserCashID,
                                                 UserName = tbUser.UserName,
                                                 TrueName = tbUser.TrueName,
                                                 PayTypeName = tbPayType.PayTypeName,
                                                 Subbranch = tbUserCashRecord.Subbranch,
                                                 ProNameCityName = tbProvince.ProName + tbCity.CityName,
                                                 CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                                                 CashAmount = tbUserCashRecord.CashAmount,
                                                 AccountMoney = tbUserCashRecord.AccountMoney,
                                                 Poundage = tbUserCashRecord.Poundage,
                                                 ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                                                 StatusName = tbStatus.StatusName
                                             }).OrderByDescending(p => p.UserCashID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strCashAmount = "￥" + Convert.ToString(Recharge[i].CashAmount.ToString());
                Recharge[i].strAccountMoney = "￥" + Convert.ToString(Recharge[i].AccountMoney.ToString());
                Recharge[i].strPoundage = "￥" + Convert.ToString(Recharge[i].Poundage.ToString());
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();           
            }                 
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000"&& releaseTime!=" 00:00:00.000"&& EnReleaseTime != " 00:00:00.000")
            {
                try
                { 
                    Recharge = Recharge.Where(p =>Convert.ToDateTime(p.ReleaseTimeStr) >=  Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.ReleaseTimeStr) <=  Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            } 
            int totalRow = myDYXTEntities.B_UserCashRecordTable.Count(); 
            Bsgrid<CashregisterVo> bsgrid = new Bsgrid<CashregisterVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 提现审核中
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectpending(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime)
        {

            var Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                           join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                           join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                           join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                           where tbUserCashRecord.StatusID==3
                           select new CashregisterVo
                           {
                               UserCashID = tbUserCashRecord.UserCashID,
                               UserName = tbUser.UserName,
                               TrueName = tbUser.TrueName,
                               PayTypeName = tbPayType.PayTypeName,
                               Subbranch = tbUserCashRecord.Subbranch,
                               ProNameCityName = tbProvince.ProName + tbCity.CityName,
                               CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                               CashAmount = tbUserCashRecord.CashAmount,
                               AccountMoney = tbUserCashRecord.AccountMoney,
                               Poundage = tbUserCashRecord.Poundage,
                               ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                               StatusName = tbStatus.StatusName
                           }).OrderByDescending(p => p.UserCashID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strCashAmount = "￥" + Convert.ToString(Recharge[i].CashAmount.ToString());
                Recharge[i].strAccountMoney = "￥" + Convert.ToString(Recharge[i].AccountMoney.ToString());
                Recharge[i].strPoundage = "￥" + Convert.ToString(Recharge[i].Poundage.ToString());
            }
            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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

            int totalRow = myDYXTEntities.B_UserCashRecordTable.Count();        
            Bsgrid<CashregisterVo> bsgrid = new Bsgrid<CashregisterVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提现审核通过
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selecttaizhouboy(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime)
        {

            var Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                           join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                           join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                           join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                           where tbUserCashRecord.StatusID ==16
                           select new CashregisterVo
                           {
                               UserCashID = tbUserCashRecord.UserCashID,
                               UserName = tbUser.UserName,
                               TrueName = tbUser.TrueName,
                               PayTypeName = tbPayType.PayTypeName,
                               Subbranch = tbUserCashRecord.Subbranch,
                               ProNameCityName = tbProvince.ProName + tbCity.CityName,
                               CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                               CashAmount = tbUserCashRecord.CashAmount,
                               AccountMoney = tbUserCashRecord.AccountMoney,
                               Poundage = tbUserCashRecord.Poundage,
                               ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                               StatusName = tbStatus.StatusName
                           }).OrderByDescending(p => p.UserCashID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strCashAmount = "￥" + Convert.ToString(Recharge[i].CashAmount.ToString());
                Recharge[i].strAccountMoney = "￥" + Convert.ToString(Recharge[i].AccountMoney.ToString());
                Recharge[i].strPoundage = "￥" + Convert.ToString(Recharge[i].Poundage.ToString());
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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
            int totalRow = myDYXTEntities.B_UserCashRecordTable.Count();
         
            Bsgrid<CashregisterVo> bsgrid = new Bsgrid<CashregisterVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 提现审核不通过
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectNOtaizhouboy(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime)
        {

            var Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                           join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                           join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                           join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                           where tbUserCashRecord.StatusID == 17
                           select new CashregisterVo
                           {
                               UserCashID = tbUserCashRecord.UserCashID,
                               UserName = tbUser.UserName,
                               TrueName = tbUser.TrueName,
                               PayTypeName = tbPayType.PayTypeName,
                               Subbranch = tbUserCashRecord.Subbranch,
                               ProNameCityName = tbProvince.ProName + tbCity.CityName,
                               CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                               CashAmount = tbUserCashRecord.CashAmount,
                               AccountMoney = tbUserCashRecord.AccountMoney,
                               Poundage = tbUserCashRecord.Poundage,
                               ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                               StatusName = tbStatus.StatusName
                           }).OrderByDescending(p => p.UserCashID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strCashAmount = "￥" + Convert.ToString(Recharge[i].CashAmount.ToString());
                Recharge[i].strAccountMoney = "￥" + Convert.ToString(Recharge[i].AccountMoney.ToString());
                Recharge[i].strPoundage = "￥" + Convert.ToString(Recharge[i].Poundage.ToString());
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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

            int totalRow = myDYXTEntities.B_UserCashRecordTable.Count();

            Bsgrid<CashregisterVo> bsgrid = new Bsgrid<CashregisterVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 用户撤销
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult Userrevocation(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime)
        {

            var Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                           join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                           join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                           join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                           where tbUserCashRecord.StatusID == 21
                           select new CashregisterVo
                           {
                               UserCashID = tbUserCashRecord.UserCashID,
                               UserName = tbUser.UserName,
                               TrueName = tbUser.TrueName,
                               PayTypeName = tbPayType.PayTypeName,
                               Subbranch = tbUserCashRecord.Subbranch,
                               ProNameCityName = tbProvince.ProName + tbCity.CityName,
                               CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                               CashAmount = tbUserCashRecord.CashAmount,
                               AccountMoney = tbUserCashRecord.AccountMoney,
                               Poundage = tbUserCashRecord.Poundage,
                               ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                               StatusName = tbStatus.StatusName
                           }).OrderByDescending(p => p.UserCashID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();
            for (int i = 0; i < Recharge.Count(); i++)
            {
                Recharge[i].strCashAmount = "￥" + Convert.ToString(Recharge[i].CashAmount.ToString());
                Recharge[i].strAccountMoney = "￥" + Convert.ToString(Recharge[i].AccountMoney.ToString());
                Recharge[i].strPoundage = "￥" + Convert.ToString(Recharge[i].Poundage.ToString());
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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

            int totalRow = myDYXTEntities.B_UserCashRecordTable.Count();

            Bsgrid<CashregisterVo> bsgrid = new Bsgrid<CashregisterVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = Recharge;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改审核绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult Updatashenhebangding(int UserCashID) {
            try
            {
                var Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                               join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                               join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                               join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                               join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                               join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                               where tbUserCashRecord.UserCashID == UserCashID
                               select new CashregisterVo
                               {
                                   UserCashID = tbUserCashRecord.UserCashID,
                                   UserID= tbUserCashRecord.UserID,
                                   PayTypeID= tbUserCashRecord.PayTypeID,
                                   ProID= tbUserCashRecord.ProID,
                                   CityID= tbUserCashRecord.CityID,
                                   StatusID= tbUserCashRecord.StatusID,
                                   UserName = tbUser.UserName,
                                   TrueName = tbUser.TrueName,
                                   PayTypeName = tbPayType.PayTypeName,
                                   Subbranch = tbUserCashRecord.Subbranch,                                  
                                   CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                                   CashAmount = tbUserCashRecord.CashAmount,
                                   AccountMoney = tbUserCashRecord.AccountMoney,
                                   Poundage = tbUserCashRecord.Poundage,
                                   ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                                   StatusName = tbStatus.StatusName,
                                   IP=tbUserCashRecord.IP,
                                   ExamineRemarks=tbUserCashRecord.ExamineRemarks
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
        /// 查看绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult selectshenhebangding(int UserCashID)
        {
            try
            {
                var Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                                join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                                join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                                join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                                join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                                join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                                where tbUserCashRecord.UserCashID == UserCashID
                                select new CashregisterVo
                                {
                                    UserCashID = tbUserCashRecord.UserCashID,
                                    UserName = tbUser.UserName,
                                    TrueName = tbUser.TrueName,
                                    PayTypeName = tbPayType.PayTypeName,
                                    Subbranch = tbUserCashRecord.Subbranch,
                                    ProNameCityName = tbProvince.ProName + tbCity.CityName,
                                    CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                                    CashAmount = tbUserCashRecord.CashAmount,
                                    AccountMoney = tbUserCashRecord.AccountMoney,
                                    Poundage = tbUserCashRecord.Poundage,
                                    ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                                    StatusName = tbStatus.StatusName,
                                    IP = tbUserCashRecord.IP,
                                    ExamineRemarks = tbUserCashRecord.ExamineRemarks
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
        /// <param name="UserRechargeRecord"></param>
        /// <returns></returns>
        public ActionResult updataconservation(B_UserCashRecordTable UserCashRecord, string optionsRadios, DateTime ReleaseTimeStr,string validCode)
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
                UserCashRecord.StatusID = 16;
                UserCashRecord.CashTime = ReleaseTimeStr;
                    var usermoney = (from tbuser in myDYXTEntities.B_UserTable
                                     where tbuser.UserID == UserCashRecord.UserID
                                     select tbuser).Single();
                    if (usermoney.PropertyAmounts >= UserCashRecord.AccountMoney)
                    {
                        usermoney.PropertyAmounts = usermoney.PropertyAmounts - UserCashRecord.CashAmount;

                    }
                    else
                    {
                        strMsg = "success";//提现金额不能大于总金额
                        UserCashRecord.StatusID = 17;
                    }
                    if (usermoney.FreezeMoney >= UserCashRecord.AccountMoney)
                    {
                        usermoney.FreezeMoney = usermoney.FreezeMoney - UserCashRecord.CashAmount;
                    }
                    else
                    {
                        strMsg = "success";
                        UserCashRecord.StatusID = 17;
                    }

                    myDYXTEntities.Entry(UserCashRecord).State = System.Data.Entity.EntityState.Modified;
                if (myDYXTEntities.SaveChanges() > 0)
                {
                        B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                        WebsiteExpenses.AccountID = Convert.ToInt32(Session["AccountID"]);
                        WebsiteExpenses.OperateTypeID = 8;
                        WebsiteExpenses.OperateMoney = Convert.ToDecimal(UserCashRecord.Poundage);
                        WebsiteExpenses.Earning = Convert.ToDecimal(UserCashRecord.Poundage);
                        WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                        WebsiteExpenses.Remark = "用户正常提现," + "扣除" + "[" + usermoney.UserName.Trim() + "]" + UserCashRecord.Poundage.Trim() + "元提现手续费";
                        WebsiteExpenses.OperateTime = DateTime.Now;
                        myDYXTEntities.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                        myDYXTEntities.SaveChanges();

                        B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                        UserExpense.UserID = UserCashRecord.UserID;
                        UserExpense.OperateTypeID = 8;
                        UserExpense.OperateMoney = Convert.ToDecimal(UserCashRecord.Poundage);
                        UserExpense.Balance = usermoney.PropertyAmounts;
                        UserExpense.Earning = Convert.ToDecimal(0);
                        UserExpense.Expenses = Convert.ToDecimal(UserCashRecord.Poundage);
                        UserExpense.Remark = "用户成功正常提现," + "[" + usermoney.UserName.Trim() + "]" + "支出" + UserCashRecord.Poundage.Trim() + "元手续费";
                        UserExpense.OperateTime = DateTime.Now;
                        myDYXTEntities.B_UserExpensesTable.Add(UserExpense);
                        myDYXTEntities.SaveChanges();

                        B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                        Capitalrecord.UserID = UserCashRecord.UserID;
                        Capitalrecord.OperateTypeID = 8;
                        Capitalrecord.OpFare = Convert.ToDecimal(UserCashRecord.Poundage);
                        Capitalrecord.Income = Convert.ToDecimal(0);
                        Capitalrecord.Expend = Convert.ToDecimal(UserCashRecord.Poundage);
                        Capitalrecord.PropertyAmounts = usermoney.PropertyAmounts;
                        Capitalrecord.Remarks = "用户成功正常提现," + "[" + usermoney.UserName.Trim() + "]" + "支出" + UserCashRecord.Poundage.Trim() + "元手续费";
                        Capitalrecord.operatetime = DateTime.Now;
                        myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                        myDYXTEntities.SaveChanges();
                        strMsg = "success";
                }
            }
            else
            {
                UserCashRecord.StatusID = 17;
                UserCashRecord.CashTime = ReleaseTimeStr;
                myDYXTEntities.Entry(UserCashRecord).State = System.Data.Entity.EntityState.Modified;
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
  
        /// <summary>
        /// 提现金额统计
        /// </summary>
        /// <returns></returns>
        public ActionResult statisticsmoeny()
        {
            var moeny = (from tbUserRechargeRecord in myDYXTEntities.B_UserCashRecordTable
                         select new
                         {
                             tbUserRechargeRecord.CashAmount,
                             tbUserRechargeRecord.Poundage
                         }).ToList();
            decimal dfs = 0;
            decimal kk = 0;
            for (int i = 0; i < moeny.Count; i++)
            {
                kk = Convert.ToDecimal(moeny[i].CashAmount);
                dfs += kk;
            }
            decimal YY = 0;
            decimal JJ = 0;
            for (int i = 0; i < moeny.Count; i++)
            {
                JJ = Convert.ToDecimal(moeny[i].Poundage);
                YY += JJ;
            }
            return Json(new { dfs, YY }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出提现记录
        /// </summary>
        /// <returns></returns>
        public ActionResult wholeExcel()
        {
            var chongzhi = from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                           join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                           join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                           join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                           join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                           join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                           orderby tbUserCashRecord.UserCashID
                           select new CashregisterVo
                           {
                               UserCashID = tbUserCashRecord.UserCashID,
                               UserName = tbUser.UserName,
                               TrueName = tbUser.TrueName,
                               PayTypeName = tbPayType.PayTypeName,
                               Subbranch = tbUserCashRecord.Subbranch,
                               ProNameCityName = tbProvince.ProName + tbCity.CityName,
                               CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                               CashAmount = tbUserCashRecord.CashAmount,
                               AccountMoney = tbUserCashRecord.AccountMoney,
                               Poundage = tbUserCashRecord.Poundage,
                               ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                               StatusName = tbStatus.StatusName
                           };
            //查询数据
            List<CashregisterVo> listExaminee = chongzhi.ToList();
            //创建Excel对象 工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook excelBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("提现记录");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题
            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("用户名称");//1
            row1.CreateCell(2).SetCellValue("真实姓名");//2
            row1.CreateCell(3).SetCellValue("提现银行");//3
            row1.CreateCell(4).SetCellValue("支行");//4
            row1.CreateCell(5).SetCellValue("所在地");//5
            row1.CreateCell(6).SetCellValue("提现账号");//6   
            row1.CreateCell(7).SetCellValue("提现总额");//6   
            row1.CreateCell(8).SetCellValue("到账金额");//6   
            row1.CreateCell(9).SetCellValue("手续费");//6   
            row1.CreateCell(10).SetCellValue("提现时间");//6   
            row1.CreateCell(11).SetCellValue("状态");//6   
                                                     //将数据逐步写入sheet1各个行
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);

                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].UserCashID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(listExaminee[i].TrueName);

                rowTemp.CreateCell(3).SetCellValue(listExaminee[i].PayTypeName);

                rowTemp.CreateCell(4).SetCellValue(listExaminee[i].Subbranch);

                rowTemp.CreateCell(5).SetCellValue(listExaminee[i].ProNameCityName);

                rowTemp.CreateCell(6).SetCellValue(listExaminee[i].CashAccountNumber);

                rowTemp.CreateCell(7).SetCellValue(Convert.ToDouble(listExaminee[i].CashAmount));

                rowTemp.CreateCell(8).SetCellValue(Convert.ToDouble(listExaminee[i].AccountMoney));

                rowTemp.CreateCell(9).SetCellValue(listExaminee[i].Poundage);

                rowTemp.CreateCell(10).SetCellValue(listExaminee[i].ReleaseTimeStr);

                rowTemp.CreateCell(11).SetCellValue(listExaminee[i].StatusName);
             
            }
            //输出的文件名称
            string fileName = "用户提现信息" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

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
        [HttpGet]
        public ActionResult ExportToExcel( string noticeName, string releaseTime, string EnReleaseTime)
        {
            var  Recharge = (from tbUserCashRecord in myDYXTEntities.B_UserCashRecordTable
                             join tbUser in myDYXTEntities.B_UserTable on tbUserCashRecord.UserID equals tbUser.UserID
                             join tbProvince in myDYXTEntities.T_Province on tbUserCashRecord.ProID equals tbProvince.ProID
                             join tbCity in myDYXTEntities.T_City on tbUserCashRecord.CityID equals tbCity.CityID
                             join tbPayType in myDYXTEntities.S_PayTypeTable on tbUserCashRecord.PayTypeID equals tbPayType.PayTypeID
                             join tbStatus in myDYXTEntities.S_StatusTable on tbUserCashRecord.StatusID equals tbStatus.StatusID
                             
                             select new CashregisterVo
                             {
                                 UserCashID = tbUserCashRecord.UserCashID,
                                 UserName = tbUser.UserName,
                                 TrueName = tbUser.TrueName,
                                 PayTypeName = tbPayType.PayTypeName,
                                 Subbranch = tbUserCashRecord.Subbranch,
                                 ProNameCityName = tbProvince.ProName + tbCity.CityName,
                                 CashAccountNumber = tbUserCashRecord.CashAccountNumber,
                                 CashAmount = tbUserCashRecord.CashAmount,
                                 AccountMoney = tbUserCashRecord.AccountMoney,
                                 Poundage = tbUserCashRecord.Poundage,
                                 ReleaseTimeStr = tbUserCashRecord.CashTime.ToString(),
                                 StatusName = tbStatus.StatusName
                             }).ToList();

            if (!string.IsNullOrEmpty(noticeName))
            {
                Recharge = Recharge.Where(n => n.UserName.Contains(noticeName)).ToList();
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
            //查询数据
            List<CashregisterVo> listExaminee = Recharge.ToList();

            //创建Excel对象 工作簿(调用NPOI文件)
            HSSFWorkbook excelBook = new HSSFWorkbook();
            //创建Excel工作表 Sheet
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("用户提现信息");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);//给sheet1添加  第一行的标题
            row1.CreateCell(0).SetCellValue("ID");//0
            row1.CreateCell(1).SetCellValue("用户名称");//1
            row1.CreateCell(2).SetCellValue("真实姓名");//2
            row1.CreateCell(3).SetCellValue("提现银行");//3
            row1.CreateCell(4).SetCellValue("支行");//4
            row1.CreateCell(5).SetCellValue("所在地");//5
            row1.CreateCell(6).SetCellValue("提现账号");//6   
            row1.CreateCell(7).SetCellValue("提现总额");//6   
            row1.CreateCell(8).SetCellValue("到账金额");//6   
            row1.CreateCell(9).SetCellValue("手续费");//6   
            row1.CreateCell(10).SetCellValue("提现时间");//6   
            row1.CreateCell(11).SetCellValue("状态");//6   
            for (int i = 0; i < listExaminee.Count; i++)
            {
                //创建标题
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);           
                rowTemp.CreateCell(0).SetCellValue(listExaminee[i].UserCashID);

                rowTemp.CreateCell(1).SetCellValue(listExaminee[i].UserName);

                rowTemp.CreateCell(2).SetCellValue(listExaminee[i].TrueName);

                rowTemp.CreateCell(3).SetCellValue(listExaminee[i].PayTypeName);

                rowTemp.CreateCell(4).SetCellValue(listExaminee[i].Subbranch);

                rowTemp.CreateCell(5).SetCellValue(listExaminee[i].ProNameCityName);

                rowTemp.CreateCell(6).SetCellValue(listExaminee[i].CashAccountNumber);

                rowTemp.CreateCell(7).SetCellValue(Convert.ToDouble(listExaminee[i].CashAmount));

                rowTemp.CreateCell(8).SetCellValue(Convert.ToDouble(listExaminee[i].AccountMoney));

                rowTemp.CreateCell(9).SetCellValue(listExaminee[i].Poundage);

                rowTemp.CreateCell(10).SetCellValue(listExaminee[i].ReleaseTimeStr);

                rowTemp.CreateCell(11).SetCellValue(listExaminee[i].StatusName);
            }

            //输出的文件名称：文件名称+时间+文件类型
            string fileName = "学生安全教育测试成绩" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".xls";

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
        /// 线下充值页面
        /// </summary>
        /// <returns></returns>
        public ActionResult tianjiarecharge()
        {
            return View();
        }

        public ActionResult selectUseryesbyno(string UserName)
        {
            string strMsg = "fail";
            int user = (from tbuser in myDYXTEntities.B_UserTable
                        where tbuser.UserName == UserName
                        select tbuser).Count();
            if (user > 0)
            {
                strMsg = "success";//成功
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加线下充值
        /// </summary>
        /// <param name="OfflineRecharge"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ActionResult insertchongzhimoney(B_OfflineRechargeTable OfflineRecharge,string UserName)
        {
            //定义返回
            string strMsg = "fail";          
                int user = (from tbuser in myDYXTEntities.B_UserTable
                            where tbuser.UserName == UserName
                            select tbuser).Count();
                if (user > 0)
                {
                    var users = (from tbuser in myDYXTEntities.B_UserTable
                                 where tbuser.UserName == UserName
                                 select tbuser).Single();
                    OfflineRecharge.UserID = users.UserID;
                    OfflineRecharge.RemitDate = DateTime.Now;
                if (users.PropertyAmounts==null)
                {
                    users.PropertyAmounts= OfflineRecharge.RemitMoney;
                 
                }
                else
                {
                    users.PropertyAmounts = users.PropertyAmounts + OfflineRecharge.RemitMoney;
                  
                }
                if (users.UsableMoney == null)
                {
                   
                    users.UsableMoney = OfflineRecharge.RemitMoney;
                }
                else
                {                   
                    users.UsableMoney = users.UsableMoney + OfflineRecharge.RemitMoney;
                }
                try
                    {
                        myDYXTEntities.B_OfflineRechargeTable.Add(OfflineRecharge);
                        if (myDYXTEntities.SaveChanges() > 0)
                        {
                            B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                            UserExpense.UserID = users.UserID;
                            UserExpense.OperateTypeID = 11;
                            UserExpense.OperateMoney = Convert.ToDecimal(OfflineRecharge.RemitMoney);
                            UserExpense.Balance = users.PropertyAmounts;
                            UserExpense.Earning = Convert.ToDecimal(OfflineRecharge.RemitMoney);
                            UserExpense.Expenses = Convert.ToDecimal(0);
                            UserExpense.Remark = "用户【" + users.UserName.Trim() + "】通过线下充值，" + "收入" + "[" + OfflineRecharge.RemitMoney + "]" + "元";
                            UserExpense.OperateTime = DateTime.Now;
                            myDYXTEntities.B_UserExpensesTable.Add(UserExpense);
                            myDYXTEntities.SaveChanges();

                            B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                            Capitalrecord.UserID = users.UserID;
                            Capitalrecord.OperateTypeID = 11;
                            Capitalrecord.OpFare = Convert.ToDecimal(OfflineRecharge.RemitMoney);
                            Capitalrecord.Income = Convert.ToDecimal(OfflineRecharge.RemitMoney);
                            Capitalrecord.Expend = Convert.ToDecimal(0);
                            Capitalrecord.PropertyAmounts = users.PropertyAmounts;
                            Capitalrecord.Remarks = "用户【" + users.UserName.Trim() + "】通过线下充值，" + "收入" + "[" + OfflineRecharge.RemitMoney + "]" + "元";
                            Capitalrecord.operatetime = DateTime.Now;
                            myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                            myDYXTEntities.SaveChanges();

                        strMsg = "success";//成功
                        }
                    }
                    catch (Exception)
                    {
                    }
                   
                }
                else
                {
                    strMsg = "exist";//用户不存在
                }           
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 网站资金流动记录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Websitefunds() {

            return View();
        }
        /// <summary>
        /// 网站资金记录查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="noticeName"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <param name="academeId"></param>
        /// <returns></returns>
        public ActionResult selectWebsitefunds(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime, int academeId) {
            var webbs = (from tbWebsiteExpenses in myDYXTEntities.B_WebsiteExpensesTable
                        join tbOperateType in myDYXTEntities.S_OperateTypeTable on tbWebsiteExpenses.OperateTypeID equals tbOperateType.OperateTypeID
                        join tbAccount in myDYXTEntities.B_AccountTable on tbWebsiteExpenses.AccountID equals tbAccount.AccountID
                        select new WebsitefundsVo
                        {
                           WebsiteExpensesID=tbWebsiteExpenses.WebsiteExpensesID,
                            OperateTypeName= tbOperateType.OperateTypeName,
                            OperateTypeID= tbWebsiteExpenses.OperateTypeID,
                            User =tbAccount.User,
                            OperateMoney= tbWebsiteExpenses.OperateMoney,
                            Earning= tbWebsiteExpenses.Earning,
                            Expenses = tbWebsiteExpenses.Expenses,
                            Remark = tbWebsiteExpenses.Remark,
                            ReleaseTimeStr = tbWebsiteExpenses.OperateTime.ToString()
                        }).OrderByDescending(p => p.WebsiteExpensesID).Skip(bsgridPage.GetStartIndex()).Take(bsgridPage.pageSize).ToList();

            for (int i = 0; i < webbs.Count(); i++)
            {
                webbs[i].strOperateMoney = "￥" + Convert.ToString(webbs[i].OperateMoney.ToString());
                webbs[i].strEarning = "￥" + Convert.ToString(webbs[i].Earning.ToString());
                webbs[i].strExpenses = "￥" + Convert.ToString(webbs[i].Expenses.ToString());
            }
            if (!string.IsNullOrEmpty(noticeName))
            {
                webbs = webbs.Where(n => n.User.Contains(noticeName)).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    webbs = webbs.Where(p => Convert.ToDateTime(p.ReleaseTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.ReleaseTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            if (academeId > 0)
            {
                webbs = webbs.Where(s => s.OperateTypeID == academeId).ToList();
            }
            int totalRow = myDYXTEntities.B_WebsiteExpensesTable.Count();
            Bsgrid<WebsitefundsVo> bsgrid = new Bsgrid<WebsitefundsVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = webbs;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);           
        }
        /// <summary>
        /// 网站资金统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Websitefundscount()
        {
            var user = (from tbuser in myDYXTEntities.B_WebsiteExpensesTable
                        select new
                        {
                            tbuser.OperateMoney,
                            tbuser.Earning,
                            tbuser.Expenses
                        }).ToList();
            decimal dfs = 0;
            decimal kk = 0;
            for (int i = 0; i < user.Count; i++)
            {
                kk = Convert.ToDecimal(user[i].OperateMoney);
                dfs += kk;
            }
            decimal YY = 0;
            decimal JJ = 0;
            for (int i = 0; i < user.Count; i++)
            {
                JJ = Convert.ToDecimal(user[i].Earning);
                YY += JJ;
            }
            decimal aa = 0;
            decimal bb = 0;
            for (int i = 0; i < user.Count; i++)
            {
                aa = Convert.ToDecimal(user[i].Expenses);
                bb += aa;
            }
            return Json(new { dfs, YY ,bb}, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///网站—— 类型
        /// </summary>
        /// <returns></returns>
        public ActionResult selectOPtype() {
            List<SelectVo> listnoticeType = new List<SelectVo>();
            SelectVo listnoticetype = new SelectVo
            {
                id = 0,
                text = "全部"
            };
            listnoticeType.Add(listnoticetype);
            List<SelectVo> noticeType = (from tbnotice in myDYXTEntities.S_OperateTypeTable
                                         select new SelectVo
                                         {
                                             id = tbnotice.OperateTypeID,
                                             text = tbnotice.OperateTypeName

                                         }).ToList();
            listnoticeType.AddRange(noticeType);
            return Json(listnoticeType, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户收支记录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Userincomeexpenditure()
        {
            return View();
        }
        /// <summary>
        /// 用户收支记录查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <param name="noticeName"></param>
        /// <param name="releaseTime"></param>
        /// <param name="EnReleaseTime"></param>
        /// <param name="academeId"></param>
        /// <returns></returns>
        public ActionResult selectUserincomeexpenditure(BsgridPage bsgridPage, string noticeName, string releaseTime, string EnReleaseTime, int academeId)
        {
            var webbs = (from tbExpensesTable in myDYXTEntities.B_UserExpensesTable
                         join tbOperateType in myDYXTEntities.S_OperateTypeTable on tbExpensesTable.OperateTypeID equals tbOperateType.OperateTypeID
                         join tbuser in myDYXTEntities.B_UserTable on tbExpensesTable.UserID equals tbuser.UserID
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

            for (int i = 0; i < webbs.Count(); i++)
            {
                webbs[i].strOperateMoney = "￥" + Convert.ToString(webbs[i].OperateMoney.ToString());
                webbs[i].strPropertyAmounts = "￥" + Convert.ToString(webbs[i].PropertyAmounts.ToString());
                webbs[i].strEarning = "￥" + Convert.ToString(webbs[i].Earning.ToString());
                webbs[i].strExpenses = "￥" + Convert.ToString(webbs[i].Expenses.ToString());
            }

            if (!string.IsNullOrEmpty(noticeName))
            {
                webbs = webbs.Where(n => n.UserName.Contains(noticeName)).ToList();
            }
            if (releaseTime != "1017-08-08 00:00:00.000" && EnReleaseTime != "4017-08-08 00:00:00.000" && releaseTime != " 00:00:00.000" && EnReleaseTime != " 00:00:00.000")
            {
                try
                {
                    webbs = webbs.Where(p => Convert.ToDateTime(p.ReleaseTimeStr) >= Convert.ToDateTime(releaseTime) && Convert.ToDateTime(p.ReleaseTimeStr) <= Convert.ToDateTime(EnReleaseTime)).ToList();
                }
                catch (Exception e)
                {

                }
            }
            if (academeId > 0)
            {
                webbs = webbs.Where(s => s.OperateTypeID == academeId).ToList();
            }
            int totalRow = myDYXTEntities.B_UserExpensesTable.Count();
            Bsgrid<UserexpensesreceiptsVo> bsgrid = new Bsgrid<UserexpensesreceiptsVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = webbs;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户资金统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Usersitefundscount()
        {
            var user = (from tbuser in myDYXTEntities.B_UserExpensesTable
                        select new
                        {
                            tbuser.OperateMoney,
                            tbuser.Earning,
                            tbuser.Expenses
                        }).ToList();
            decimal dfs = 0;
            decimal kk = 0;
            for (int i = 0; i < user.Count; i++)
            {
                kk = Convert.ToDecimal(user[i].OperateMoney);
                dfs += kk;
            }
            decimal YY = 0;
            decimal JJ = 0;
            for (int i = 0; i < user.Count; i++)
            {
                JJ = Convert.ToDecimal(user[i].Earning);
                YY += JJ;
            }
            decimal aa = 0;
            decimal bb = 0;
            for (int i = 0; i < user.Count; i++)
            {
                aa = Convert.ToDecimal(user[i].Expenses);
                bb += aa;
            }
            return Json(new { dfs, YY, bb }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 支付方式管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult modeofpayment() {                
           return View();
        }
        /// <summary>
        /// 支付列表查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult modeopaytype(BsgridPage bsgridPage) {
            var tbmopa = from tbPayType in myDYXTEntities.S_PayTypeTable
                         where tbPayType.AddDeny==true
                         select new paytypeVo
                         {
                             PayTypeID=tbPayType.PayTypeID,
                          
                             PayTypeName = tbPayType.PayTypeName,
                             PaySummary = tbPayType.PaySummary,
                             OpenDeny = tbPayType.OpenDeny,
                             AddDeny= tbPayType.AddDeny

                         };
            int totalRow = tbmopa.Count();
            List<paytypeVo> notices = tbmopa.OrderByDescending(p => p.PayTypeID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<paytypeVo> bsgrid = new Bsgrid<paytypeVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 图标绑定table
        /// </summary>
        /// <param name="PayTypeID"></param>
        /// <returns></returns>
        public ActionResult GetImage(int PayTypeID)
        {
            try
            {                       
                var studentImg = (from tbPayType in myDYXTEntities.S_PayTypeTable
                                  where tbPayType.PayTypeID == PayTypeID
                                  select new
                                  {
                                      tbPayType.PayTypePicture
                                  }).Single();

                byte[] imageData = studentImg.PayTypePicture;
                return File(imageData, @"image/jpg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        /// <summary>
        /// 支付管理
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult addeopaytype(BsgridPage bsgridPage)
        {
            var tbmopa = from tbPayType in myDYXTEntities.S_PayTypeTable                      
                         select new paytypeVo
                         {
                             PayTypeID = tbPayType.PayTypeID,
                         
                             PayTypeName = tbPayType.PayTypeName,
                             PaySummary = tbPayType.PaySummary,
                             AddDeny= tbPayType.AddDeny,
                             OpenDeny= tbPayType.OpenDeny
                         };
            int totalRow = tbmopa.Count();
            List<paytypeVo> notices = tbmopa.OrderByDescending(p => p.PayTypeID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<paytypeVo> bsgrid = new Bsgrid<paytypeVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑绑定
        /// </summary>
        /// <param name="PayTypeID"></param>
        /// <returns></returns>
        public ActionResult selectbypaytypeID(int PayTypeID) {
            try
            {
            var paytypetable =(from tbpaytype in myDYXTEntities.S_PayTypeTable
                               where tbpaytype.PayTypeID == PayTypeID
                               select new paytypeVo
                               {
                                   PayTypeID=tbpaytype.PayTypeID,
                                   PayTypeName= tbpaytype.PayTypeName,
                                   PaySummary= tbpaytype.PaySummary,
                                   OpenDeny = tbpaytype.OpenDeny,
                                   AddDeny = tbpaytype.AddDeny
                               }).Single();


                 return Json(paytypetable,JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 根据id 查询logo
        /// </summary>
        /// <param name="PayTypeID"></param>
        /// <returns></returns>
        public ActionResult GetStudentImage(int PayTypeID)
        {
            try
            {
                var studentImg = (from tbPayType in myDYXTEntities.S_PayTypeTable
                                  where tbPayType.PayTypeID == PayTypeID
                                  select new
                                  {
                                      tbPayType.PayTypePicture
                                  }).Single();
                byte[] imageData = studentImg.PayTypePicture;
                return File(imageData, @"image/jpg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        /// <summary>
        /// 新增支付类型保存
        /// </summary>
        /// <param name="PayType"></param>
        /// <param name="fileStudentImage"></param>
        /// <returns></returns>
        public ActionResult InsertStudent(S_PayTypeTable PayType, HttpPostedFileBase fileStudentImage)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tb_PayType in myDYXTEntities.S_PayTypeTable
                                      where tb_PayType.PayTypeName == PayType.PayTypeName                                        
                                      select tb_PayType).Count();
                if (oldStudentRows == 0)
                {
                    PayType.OpenDeny =false;
                    PayType.AddDeny = false;
                    byte[] imgFile = null;//读取上传的图片 //判断是否上传图片
                    if (fileStudentImage != null)
                        {
                            imgFile = new byte[fileStudentImage.ContentLength];//初始化为图片的长度
                            //读取该图片文件
                            //将图片转为流
                            //将流读取为byte[]，参数：byte[]，读取开始位置，读取字节数
                            fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                        }
                        PayType.PayTypePicture= imgFile;
                        myDYXTEntities.S_PayTypeTable.Add(PayType);
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

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="PayType"></param>
        /// <param name="fileStudentImage"></param>
        /// <returns></returns>
        public ActionResult UpdateStudent(S_PayTypeTable PayType, HttpPostedFileBase fileStudentImage) {
            string strMsg = "fail";
            try
            {
                int oldPaytypeRows = (from tbpaytype in myDYXTEntities.S_PayTypeTable
                                      where tbpaytype.PayTypeID != PayType.PayTypeID &&
                                         (tbpaytype.PayTypeName == PayType.PayTypeName)
                                      select tbpaytype).Count();
                if (oldPaytypeRows==0)
                {
                    S_PayTypeTable varaPaytype = (from tbpaytype in myDYXTEntities.S_PayTypeTable
                                          where tbpaytype.PayTypeID == PayType.PayTypeID                                    
                                          select tbpaytype).Single();
                         varaPaytype.PayTypeName = PayType.PayTypeName;
                         varaPaytype.PaySummary = PayType.PaySummary;
                        //判断是否上传图片
                        if (fileStudentImage != null)
                        {
                            byte[] imgFile = new byte[fileStudentImage.ContentLength];
                            fileStudentImage.InputStream.Read(imgFile, 0, fileStudentImage.ContentLength);
                            varaPaytype.PayTypePicture = imgFile;//更新图片                              
                        }
                        myDYXTEntities.Entry(varaPaytype).State = System.Data.Entity.EntityState.Modified;
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
        /// <summary>
        /// 支付类型中的添加按钮
        /// </summary>
        /// <param name="PayTypeID"></param>
        /// <returns></returns>
        public ActionResult upadddeny(int PayTypeID)
        {
            string strMsg = "fail";
            try
            {          
            S_PayTypeTable varPayType = (from tbpaytype in myDYXTEntities.S_PayTypeTable
                                         where tbpaytype.PayTypeID == PayTypeID
                                         select tbpaytype).Single();
            varPayType.AddDeny = true;
            varPayType.OpenDeny = false;
            myDYXTEntities.Entry(varPayType).State = System.Data.Entity.EntityState.Modified;
            myDYXTEntities.SaveChanges();
            strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="PayTypeID"></param>
        /// <returns></returns>
        public ActionResult Updataclose(int PayTypeID)
        {
            string strMsg = "fail";
            try
            {
                S_PayTypeTable varPayType = (from tbpaytype in myDYXTEntities.S_PayTypeTable
                                             where tbpaytype.PayTypeID == PayTypeID
                                             select tbpaytype).Single();
                varPayType.OpenDeny = false;
                myDYXTEntities.Entry(varPayType).State = System.Data.Entity.EntityState.Modified;
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 开启按钮
        /// </summary>
        /// <param name="PayTypeID"></param>
        /// <returns></returns>
        public ActionResult Updataopen(int PayTypeID)
        {
            string strMsg = "fail";
            try
            {
                S_PayTypeTable varPayType = (from tbpaytype in myDYXTEntities.S_PayTypeTable
                                             where tbpaytype.PayTypeID == PayTypeID
                                             select tbpaytype).Single();
                varPayType.OpenDeny = true;
                myDYXTEntities.Entry(varPayType).State = System.Data.Entity.EntityState.Modified;
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="PayTypeID"></param>
        /// <returns></returns>
        public ActionResult deletepaytype(int PayTypeID)
        {
            string strMsg = "fail";
            try
            {
                S_PayTypeTable varPayType = (from tbpaytype in myDYXTEntities.S_PayTypeTable
                                             where tbpaytype.PayTypeID == PayTypeID
                                             select tbpaytype).Single();
                //删除数据
                myDYXTEntities.S_PayTypeTable.Remove(varPayType);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 资金费用页面
        /// </summary>
        /// <returns></returns>
        public ActionResult financialcharges()
        {
            return View();
        }
        /// <summary>
        /// 资金费用查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectfinancialcharges(BsgridPage bsgridPage)
        {
            var tbmopa = from tb_FundCost in myDYXTEntities.S_FundCostTable
                         join tbCostType in myDYXTEntities.B_CostTypeTable on tb_FundCost.CostTypeID equals tbCostType.CostTypeID
                         join tbStatus in myDYXTEntities.S_FundStatustable on tb_FundCost.FundStatusID equals tbStatus.FundStatusID
                         select new financialchargesVo
                         {
                             FundCostID= tb_FundCost.FundCostID,
                             Name= tb_FundCost.Name,
                             Identifier= tb_FundCost.Identifier,
                             CostTypeName= tbCostType.CostTypeName,
                             FundStatusID= tb_FundCost.FundStatusID,
                             StatusName =  tbStatus.FundStatusName,
                             PaymentRate = tb_FundCost.PaymentRate
                         };
            int totalRow = tbmopa.Count();
            List<financialchargesVo> notices = tbmopa.OrderBy(p => p.FundCostID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<financialchargesVo> bsgrid = new Bsgrid<financialchargesVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///费用类型查询
        /// </summary>
        /// <param name="bsgridPage"></param>
        /// <returns></returns>
        public ActionResult selectfinancialtype(BsgridPage bsgridPage)
        {
            var tbmopa = from tb_CostType in myDYXTEntities.B_CostTypeTable                   
                         join tbStatus in myDYXTEntities.S_FundStatustable on tb_CostType.FundStatusID equals tbStatus.FundStatusID
                         select new CostTypeVo
                         {
                             CostTypeID = tb_CostType.CostTypeID,
                             CostTypeName = tb_CostType.CostTypeName,
                             Identifier = tb_CostType.Identifier,
                             FundStatusID = tb_CostType.FundStatusID,
                             StatusName = tbStatus.FundStatusName,
                         };
            int totalRow = tbmopa.Count();
            List<CostTypeVo> notices = tbmopa.OrderBy(p => p.CostTypeID).
               Skip(bsgridPage.GetStartIndex()).
               Take(bsgridPage.pageSize).
               ToList();
            Bsgrid<CostTypeVo> bsgrid = new Bsgrid<CostTypeVo>();
            bsgrid.success = true;
            bsgrid.totalRows = totalRow;
            bsgrid.curPage = bsgridPage.curPage;
            bsgrid.data = notices;
            return Json(bsgrid, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 类型绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult Selctcost()
        {

            List<SelectVo> Province = (from tbCostType in myDYXTEntities.B_CostTypeTable
                                       select new SelectVo
                                       {
                                           id = tbCostType.CostTypeID,
                                           text = tbCostType.CostTypeName

                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 状态绑定
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 资金费用新增保存
        /// </summary>
        /// <param name="FundCost"></param>
        /// <param name="inlineRadioOptions"></param>
        /// <returns></returns>
        public ActionResult insertzijinfeiyong(S_FundCostTable FundCost)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tbFundCost in myDYXTEntities.S_FundCostTable
                                      where tbFundCost.Name == FundCost.Name 
                                      select tbFundCost).Count();
                if (oldStudentRows==0)
                {
                   
                    myDYXTEntities.S_FundCostTable.Add(FundCost);
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
        /// <summary>
        /// 修改资金费用绑定
        /// </summary>
        /// <param name="FundCostID"></param>
        /// <returns></returns>
        public ActionResult zijinfeiyongupdata(int FundCostID)
        {
            try
            {
                var paytypetable = (from tbFundCost in myDYXTEntities.S_FundCostTable
                                    join tbStatus in myDYXTEntities.S_FundStatustable  on tbFundCost.FundStatusID equals tbStatus.FundStatusID
                                    where tbFundCost.FundCostID == FundCostID
                                    select new financialchargesVo
                                    {
                                      FundCostID=tbFundCost.FundCostID,
                                        Name = tbFundCost.Name.Trim(),
                                        Identifier = tbFundCost.Identifier.Trim(),
                                        CostTypeID = tbFundCost.CostTypeID,
                                        FundStatusID=tbFundCost.FundStatusID,
                                        PaymentRate =tbFundCost.PaymentRate.Trim()
                                    }).Single();
                return Json(paytypetable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 修改资金费用保存
        /// </summary>
        /// <param name="FundCost"></param>
        /// <returns></returns>
        public ActionResult Updatazijinfeiyong(S_FundCostTable FundCost)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tbFundCost in myDYXTEntities.S_FundCostTable
                                      where tbFundCost.Name == FundCost.Name && tbFundCost.FundCostID != FundCost.FundCostID
                                      select tbFundCost).Count();
                if (oldStudentRows == 0)
                {
                    myDYXTEntities.Entry(FundCost).State = System.Data.Entity.EntityState.Modified;
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
        /// <summary>
        /// 删除资金费用
        /// </summary>
        /// <param name="FundCostID"></param>
        /// <returns></returns>
        public ActionResult deletezijinfeiyong(int FundCostID)
        {
            string strMsg = "fail";
            try
            {
                S_FundCostTable varFundCost = (from tbFundCost in myDYXTEntities.S_FundCostTable
                                               where tbFundCost.FundCostID == FundCostID
                                               select tbFundCost).Single();
                //删除数据
                myDYXTEntities.S_FundCostTable.Remove(varFundCost);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 资金费用类型新增保存
        /// </summary>
        /// <param name="CostType"></param>
        /// <returns></returns>
        public ActionResult inserttypezijinfeiyong(B_CostTypeTable CostType)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tbCostType in myDYXTEntities.B_CostTypeTable
                                      where tbCostType.CostTypeName == CostType.CostTypeName
                                      select tbCostType).Count();
                if (oldStudentRows == 0)
                {

                    myDYXTEntities.B_CostTypeTable.Add(CostType);
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

        /// <summary>
        /// 资金费用类型修改绑定
        /// </summary>
        /// <param name="CostTypeID"></param>
        /// <returns></returns>
        public ActionResult zijinfeiyongupdatatype(int CostTypeID)
        {
            try
            {
                var varCostType = (from tbCostType in myDYXTEntities.B_CostTypeTable
                                    join tbFundStatus in myDYXTEntities.S_FundStatustable on tbCostType.FundStatusID equals tbFundStatus.FundStatusID
                                    where tbCostType.CostTypeID == CostTypeID
                                    select new B_CostTypeVo
                                    {
                                        CostTypeID = tbCostType.CostTypeID,
                                        CostTypeName = tbCostType.CostTypeName.Trim(),
                                        Identifier = tbCostType.Identifier.Trim(),
                                        FundStatusID=tbCostType.FundStatusID,
                                        FundStatusName = tbFundStatus.FundStatusName.Trim(),
                                        Remark = tbCostType.Remark.Trim()
                                    }).Single();
                return Json(varCostType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 资金费用类型修改保存
        /// </summary>
        /// <param name="CostType"></param>
        /// <returns></returns>
        public ActionResult Updatazijinfeiyongtype(B_CostTypeTable CostType)
        {
            string strMsg = "fali";
            try
            {
                //判断当前支付类型是否存在
                int oldStudentRows = (from tbCostType in myDYXTEntities.B_CostTypeTable
                                      where tbCostType.CostTypeName == CostType.CostTypeName && tbCostType.CostTypeID != CostType.CostTypeID
                                      select tbCostType).Count();
                if (oldStudentRows == 0)
                {
                    myDYXTEntities.Entry(CostType).State = System.Data.Entity.EntityState.Modified;
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

        /// <summary>
        /// 删除资金费用类型
        /// </summary>
        /// <param name="CostTypeID"></param>
        /// <returns></returns>
        public ActionResult deletezijinfeiyongtype(int CostTypeID)
        {
            string strMsg = "fail";
            try
            {
                B_CostTypeTable varFundCost = (from tbCostType in myDYXTEntities.B_CostTypeTable
                                               where tbCostType.CostTypeID == CostTypeID
                                               select tbCostType).Single();
                //删除数据
                myDYXTEntities.B_CostTypeTable.Remove(varFundCost);
                myDYXTEntities.SaveChanges();
                strMsg = "success";
            }
            catch (Exception e)
            {

            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 扣除费用
        /// </summary>
        /// <returns></returns>
        public ActionResult deductexpensesview()
        {
            return View();
        }
        /// <summary>
        ///  扣除费用类型绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult SelctItemType()
        {

            List<SelectVo> Province = (from tbItemType in myDYXTEntities.S_ItemTypeTable
                                       select new SelectVo
                                       {
                                           id = tbItemType.ItemTypeID,
                                           text = tbItemType.ItemTypeName

                                       }).ToList();

            return Json(Province, JsonRequestBehavior.AllowGet);
        }

        public ActionResult insertdeductexpenses(B_Deductexpenses Deductexpenses, string UserName, string validCode)
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
                int user = (from tbuser in myDYXTEntities.B_UserTable
                        where tbuser.UserName == UserName
                        select tbuser).Count();
            if (user > 0)
            {
                    var ItemType = (from tbItemType in myDYXTEntities.S_ItemTypeTable
                                 where tbItemType.ItemTypeID == Deductexpenses.ItemTypeID
                                 select tbItemType).Single();
                    var users = (from tbuser in myDYXTEntities.B_UserTable
                             where tbuser.UserName == UserName
                             select tbuser).Single();
                Deductexpenses.UserID = users.UserID;           
                if (users.PropertyAmounts == null)
                {
                    strMsg = "insufficientamount";
                        return Json(strMsg, JsonRequestBehavior.AllowGet);
                    }
                    else if(users.PropertyAmounts < Deductexpenses.Amountmoney)
	                 {
                        strMsg = "insufficientamount";
                        return Json(strMsg, JsonRequestBehavior.AllowGet);
                    }
                else
                {
                    users.PropertyAmounts = users.PropertyAmounts - Deductexpenses.Amountmoney;

                }
                    if (users.UsableMoney == null )
                    {

                        strMsg = "insufficientamount";
                        return Json(strMsg, JsonRequestBehavior.AllowGet);
                    } else if (users.UsableMoney < Deductexpenses.Amountmoney)
                    {
                        strMsg = "insufficientamount";
                        return Json(strMsg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        users.UsableMoney = users.UsableMoney - Deductexpenses.Amountmoney;
                    }
                try
                {
                    myDYXTEntities.B_Deductexpenses.Add(Deductexpenses);
                    if (myDYXTEntities.SaveChanges() > 0)
                    {
                            B_WebsiteExpensesTable WebsiteExpenses = new B_WebsiteExpensesTable();
                            WebsiteExpenses.AccountID = Convert.ToInt32(Session["AccountID"]);
                            WebsiteExpenses.OperateTypeID = 10;
                            WebsiteExpenses.OperateMoney = Convert.ToDecimal(Deductexpenses.Amountmoney);
                            WebsiteExpenses.Earning = Convert.ToDecimal(Deductexpenses.Amountmoney);
                            WebsiteExpenses.Expenses = Convert.ToDecimal(0);
                            WebsiteExpenses.Remark = "用户用于" + "[" + ItemType.ItemTypeName.Trim() + "]" + "扣除"+ users.UserName.Trim()+ "[" + Deductexpenses.Amountmoney + "]" + "元手续费";
                            WebsiteExpenses.OperateTime = DateTime.Now;
                            myDYXTEntities.B_WebsiteExpensesTable.Add(WebsiteExpenses);
                            myDYXTEntities.SaveChanges();

                            B_UserExpensesTable UserExpense = new B_UserExpensesTable();
                            UserExpense.UserID = users.UserID;
                            UserExpense.OperateTypeID = 10;
                            UserExpense.OperateMoney = Convert.ToDecimal(Deductexpenses.Amountmoney);
                            UserExpense.Balance = users.PropertyAmounts;
                            UserExpense.Earning = Convert.ToDecimal(0);
                            UserExpense.Expenses = Convert.ToDecimal(Deductexpenses.Amountmoney);
                            UserExpense.Remark = "用户" + users.UserName.Trim() + "用于" + "[" + ItemType.ItemTypeName.Trim() + "]" + "支出" + "[" + Deductexpenses.Amountmoney + "]" + "元手续费";
                            UserExpense.OperateTime = DateTime.Now;
                            myDYXTEntities.B_UserExpensesTable.Add(UserExpense);
                            myDYXTEntities.SaveChanges();

                            B_CapitalrecordTable Capitalrecord = new B_CapitalrecordTable();
                            Capitalrecord.UserID = users.UserID;
                            Capitalrecord.OperateTypeID = 10;
                            Capitalrecord.OpFare = Convert.ToDecimal(Deductexpenses.Amountmoney);
                            Capitalrecord.Income = Convert.ToDecimal(0);
                            Capitalrecord.Expend = Convert.ToDecimal(Deductexpenses.Amountmoney);
                            Capitalrecord.PropertyAmounts = users.PropertyAmounts;
                            Capitalrecord.Remarks = "用户" + users.UserName.Trim() + "用于" + "[" + ItemType.ItemTypeName.Trim() + "]" + "支出" + "[" + Deductexpenses.Amountmoney + "]" + "元手续费";
                            Capitalrecord.operatetime = DateTime.Now;
                            myDYXTEntities.B_CapitalrecordTable.Add(Capitalrecord);
                            myDYXTEntities.SaveChanges();

                            strMsg = "success";//成功

                    }
                }
                catch (Exception)
                {

                }

               }
              else
             {
                strMsg = "exist";//用户不存在
             }
             }
            else
            {
                strMsg = "ValidCodeErro";//验证码错误
            }
            return Json(strMsg, JsonRequestBehavior.AllowGet);
        }
       


    }
}