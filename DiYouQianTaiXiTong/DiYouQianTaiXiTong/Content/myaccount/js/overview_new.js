/**
 * Created by RYOLEEV on 2014/12/4.
 */
//非标订单地址，上线前需修改
var trustAddress = "//jdd.jr.jd.com/trade/dealingTicket/";
//变现订单地址，上线前需修改
var secondaryAddress = "//atm.jr.jd.com/detail/dealingTicket/";
//P2P订单详情页(原型上暂定写死)
var p2pAddress = "//jdd.jr.jd.com/trade/dealingTicketLoan/";
var stockRightCrowdfundOrder = "//dj.jd.com/funding/projectSubscribe.action?fineOrderId=";
var trustAddressPay= "//jdd.jr.jd.com/order/pay?plOrderId="; //非标支付链接
var p2pPayAddress = "//8.jd.com/investor/platform/";
var secondaryAddressPay= "//atm.jr.jd.com/pay/continuePay.html?investmentId="; //变现支付链接
var goldReturnAddressPay = '//mygold.jd.com/order/pay/'; //京东淘金支付

var stockAddress = "//qslc.jd.com/orderDetail/showOrderDetailList.html?plOrderId="; //劵商详情页
var stockAddressPay= "//qslc.jd.com//PlaceOrder/toContinuePay.html?plOrderId="; //劵商支付链接

var financingOrderDetailURL = "//licaishi.jd.com/order/showOrderPage.do?orderId=";//理财师订单详情页
var financingPayURL = "//licaishi.jd.com/buy/toSecondBuy.do?orderId=";//理财师支付页
var transferOrderAddress = '//dq.jd.com/rwin/order/'; //转让理财订单详情页
var transferPayAddress = '//dq.jd.com/rwin/trade/'; //转让理财支付详情页
var noAddress = "javascript:;";

var adIndex = 0;
//猜你喜欢广告位索引
var adLoveIndex=0;
eyeInitOverview();
//展示我的体验金
showMyActivityGift();
//展示赎回中的交易count
showOverViewSellCount();
//展示公告信息
overviewShowNotice();
//显示猜你喜欢广告位
showRecommendData(adLoveIndex);

//资产总览展现二维码
showMyIncomeQRCode();

//展示资产总览改版中间的PK收益图
showOverViewIncome();
//展示我的收益排名信息
getMyRankInfo();
//异步加载理财天数
showMyFinancingDays();
//资产总览 大列表数据
showOverviewSelect(0);


/**
 * 订单列表 、、新加业务需修改
 */
jQuery.ajax({
    url: "/async/loadOverviewTradeData.action?_dc=" + new Date().getTime(),
    type: "GET",
    async: true,
    success: function (result) {
        var arr = result.detailList;
        var htmlStr = "";
        if (arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                var detail = arr[i];
                var aHref = "#";
                //保险
                if (detail.project_type == 102) {
					if(detail.ope_type == 2 && detail.sales_channel == 22){
						aHref = "//dq.jd.com/bwin/currentDetail.action?tradeAccountId="+detail.insuranceNo;
					}else{
						aHref = detail.url1;
					}
                    //众筹
                } else if (detail.project_type == 10002) {
                    aHref = "//z.jd.com/funding/order_detail.action?orderId=" + detail.buy_apply_id;
                    //票据
                } else if (detail.project_type == 10003) {
                    aHref = "//bill.jr.jd.com/buy/toPay.action?order_id=" + detail.buy_apply_id;
                    //银行理财
                } else if (detail.project_type == 106) {
                    aHref = "//bank.jr.jd.com/subscribe/querySubscribe.do?orderId=" + detail.buy_apply_id;
                    //p2p
                } else if (detail.project_type == 10007) {
                	//P2P 判断符合条件的增加转让url
					if(null != detail.url4){
						aHref = p2pAddress+detail.url4+'.html';
					}
                    //非标理财
                }else if (detail.project_type == 10005) {
                    aHref = trustAddress + detail.buy_apply_id+".html";
                    //基金
                }else if (detail.project_type == 10006) {
                    aHref = stockRightCrowdfundOrder+detail.buy_apply_id;
                    //基金
                }else if (detail.project_type == 10008) {
                	if(null != detail.url4){
						aHref = secondaryAddress+detail.url4+'.html';
					}
                    //基金
                }else if (detail.project_type == 10010) {
                	//乐猜理财
                	aHref = noAddress;
                }else if (detail.project_type == 10014) {
                	//劵商理财
                	aHref = stockAddress+detail.buy_apply_id;
                }else if (detail.project_type == 10017) {
                	//对公理财
                	aHref = "//jrb2b.jd.com/invest/order/detail?pid="+detail.item_code;
                }else if (detail.project_type == 10018) {
					//京东黄金
					aHref = "//mygold.jd.com/order/detail/"+detail.buy_apply_id+".htm";
				}else if (detail.project_type == 10026) {
					//转让理财
					aHref = transferOrderAddress + detail.buy_apply_id;
				}else if (detail.project_type == 10019) {
					//养老保障
					aHref = detail.url1;
				}else if (detail.project_type == 10023) {
					//京东黄金2.0
					if(detail.order_type != null && detail.order_type == 23){
						//黄金充值订单
						aHref = "//firm-gold.jd.com/cashOrder/rechargeDetail/"+detail.system_serial+".do";
					}else{
						aHref = "//firm-gold.jd.com/order/detail/"+detail.buy_apply_id+".do";
					}
				}else if(detail.project_type == 10015){
                	//我的理财服务
                	aHref = financingOrderDetailURL+detail.buy_apply_id+'&productId='+detail.project_id;
                }else if(detail.project_type == 10020){
					//基智账户
					aHref = "/trade/toBoradcastdetail.action?id="+detail.buy_apply_id;
				}else if(detail.project_type == 10022){
					//小金库无交易详情
				}else if(detail.project_type == 10021){
					//小金库无交易详情
				}else if(detail.project_type == 101){
					aHref = "//dx.jd.com/purchaseRedeem/purchaseRedeemActionInfo.action?plOrderId="+detail.buy_apply_id+"&mhtCode="+detail.s_merchant;
				}else {
                    aHref = "/trade/tobuyapplydetail.action?id=" + detail.buy_apply_id;
                }
                
                if (i == 0) {
                    htmlStr += '<div class="h-u-list h-u-list-start">';
                }else if(i == arr.length-1){
                	htmlStr += '<div class="h-u-list h-u-list-end">';
                }else {
                    htmlStr += '<div class="h-u-list">';
                }
                htmlStr += '<div class="h-u-l-date">';
                var date1 = "" + new Date(detail.created).format("yyyy-MM-dd");
                var date2 = "" + new Date(detail.created).format("hh:mm");
                htmlStr += '<p class="font-blue">'+date1+'</p>';
                htmlStr += '<p>'+date2+'</p>';
                htmlStr += '</div>';
                
				htmlStr += '<div class="h-u-l-msg">';
				htmlStr += '<p class="h-u-l-m-info">';
				htmlStr += '<em class="h-u-l-m-i-dot"></em>';
				htmlStr += '<span class="h-u-l-m-i-title">'+detail.projectTypeName+'</span>';
				htmlStr += '</p>';
				if(detail.order_type != null && detail.order_type == 22) htmlStr += '<p class="h-u-l-m-msg">续投 - '+detail.project_name+'</p>';
				else htmlStr += '<p class="h-u-l-m-msg">买入 - '+detail.project_name+'</p>';
				htmlStr += '</div>';
				htmlStr += '<div class="h-u-l-price">+'+formatAmount(detail.invest_amount)+'</div>';
				var tradeStatus = '';
    			if(detail.order_type == 1 && detail.system_id == 47){
    				if(detail.affirm_status_str == "待付款"){
    					tradeStatus ="红包发放中";
    				}else{
    					tradeStatus="红包已发放";
    				}
    			}else{
    				tradeStatus = detail.affirm_status_str;
    			}
        		if(tradeStatus == "待付款" || tradeStatus == "红包发放中"){
        			htmlStr += '<div class="h-u-l-info font-red">'+tradeStatus+'</div>';
        		}else{
        			htmlStr += '<div class="h-u-l-info">'+tradeStatus+'</div>';
        		}
        		var payUrl = "";
        		var clatag = "jr|keycount|jr_zczl|jyjl_btn" + (i + 1);
        		if(detail.affirm_status_str == "待付款" && detail.project_type != "10006" && detail.project_type != "10017"){
        			if(detail.project_type == "102"){
        				if(detail.system_id == 47) payUrl = "";
        				else payUrl = "javascript:toInsurancePay('"+detail.url4+"')";
        			}else if(detail.project_type == "10019"){
						if(detail.system_id == 47) payUrl = "";
						else payUrl = "javascript:toInsurancePay('"+detail.url4+"')";
					}else if(detail.project_type == "10003"){
        				if(detail.system_id == "47") payUrl = "";
        				else payUrl = "//bill.jr.jd.com/order/redirectToPay.action?plOrderId="+detail.buy_apply_id;
        			}else if(detail.project_type == "10005"){
        				if(detail.system_id == "47") payUrl = "";
        				else payUrl = trustAddressPay+detail.buy_apply_id;
					}else if(detail.project_type == "10007"){
						//P2P还款链接
						if(detail.system_id == "47") payUrl = "";
        				else payUrl = p2pPayAddress+detail.system_serial+".html";
					}else if(detail.project_type == "10008"){
						//二级市场变现 支付订单详情 
						if(detail.system_id == "47") payUrl = "";
        				else payUrl = secondaryAddressPay+detail.system_serial;
					}else if(detail.project_type == "10010"){
						//乐猜理财 支付订单详情 
						if(detail.system_id == "47") payUrl = "";
        				else payUrl = noAddress;
					}else if(detail.project_type == "10018"){
						if(detail.system_id == "47") payUrl = "";
        				else payUrl = goldReturnAddressPay +detail.buy_apply_id+'.htm';
					}else if(detail.project_type == "10023"){
						//目前黄金2.0不在PC端支付，只在APP支付
						payUrl = "";
						/*if(detail.system_id == "47") payUrl = "";
						else payUrl = goldReturnAddressPay +detail.buy_apply_id+'.htm';*/
					}else if(detail.project_type == "10015"){//我的理财服务
						payUrl = financingPayURL+detail.buy_apply_id;
					}else if(detail.project_type == "10014"){
						//劵商理财 支付订单详情 
						if(detail.system_id == "47") payUrl = "";
        				else payUrl = stockAddressPay+detail.buy_apply_id;
					}else if(detail.project_type == "106"){
						if(detail.system_id == "47") payUrl = "";
        				else payUrl = "//bank.jr.jd.com/bf/toPay.do?orderId="+detail.buy_apply_id+"&shouldPay="+detail.invest_amount+"&companyName="
        													+detail.invest_org+"&productName="+detail.project_name+"&skuMerchantId="+detail.s_merchant;
        			}else if(detail.project_type == "10020"){
						//基智账户
						if(detail.system_id == "47") payUrl = "";
						else payUrl = "//sa.jd.com/smart_account/getOrderInfo.action?orderId="+detail.system_serial;
					}else if(detail.project_type == "10022"||detail.project_type == "10021"){
						//暂时不支持用户支付
					}else if(detail.project_type == "10026"){
						if(detail.system_id == "47") payUrl = "";
						else payUrl = transferPayAddress +detail.buy_apply_id+'/pay';
					}else if(detail.project_type == "10029"){
                        //暂时不支持用户支付
                    }else{
        				if(detail.order_type == "1"){
        					//小白订单 支付走小白系统支付 
        					if(detail.system_id == "47") payUrl = ""; //活动订单屏蔽支付渠道
        					else payUrl = "//xiaobai.jr.jd.com/cashier/pay.action?orderId="+detail.buy_apply_id;
        				}else if(detail.project_type == "10002" || detail.project_type == "101"){
        					if(detail.system_id == "47") payUrl = ""; //活动订单屏蔽支付渠道
        					else if(detail.project_type == "10002" && null != detail.order_type && detail.order_type == "5"){
        						//众筹新收银台下单
        						//payUrl = "//zcashier.jd.com/funding/p/toCashier.action?orderId="+detail.buy_apply_id+"&system_id=1001";
								payUrl = "//trade-z.jd.com/funding/now_pay.action?subscribeId="+detail.system_serial+"&cashierFlag=1";
        					}else if(detail.project_type == "101"){
        					    if(detail.sales_channel=="11"){
									payUrl = '//dx.jd.com/clientOrder.htm?orderId='+detail.system_serial;
        					    }else{
									payUrl = '//myfund.jd.com/order/getOrderInfo.action?orderId='+detail.system_serial;
        					    }
        					}
        					else  payUrl = "/order/pay.action?orderId="+detail.buy_apply_id+"&system_id="+detail.system_id;
        				}else{
        					payUrl = "";
        				}
        			}
        			if(payUrl == "" || payUrl == null){
						if(detail.project_type == "10023" || detail.project_type == "10029"){
							htmlStr += '<div class="h-u-l-button"><a class="ui-button ui-button--disable" href="javascript:void(0);" clstag="'+clatag+'">支付</a>';
							htmlStr += '<div class="ui_infoPopBtn1">';
							htmlStr += '<em class="ui_infoPopBtn-ico"></em>';
							htmlStr += '<div class="ui_infoPop-bg">';
							htmlStr += '<em class="ui_infoPop-ico"></em>';
							htmlStr += '<div class="ui_infoPop">';
							htmlStr += '<p>交易请使用京东金融APP</p>';
							htmlStr += '</div>';
							htmlStr += '</div>';
							htmlStr += '</div>';
							htmlStr += '</div>';
						}else if(detail.project_type == "10022"||detail.project_type == "10021"){
						}else{
							htmlStr += '<div class="h-u-l-button"><a class="ui-button ui-button--disable" href="javascript:void(0);" clstag="'+clatag+'">支付</a></div>';
						}
        			}else if(detail.project_type == "101" && detail.ref_fund_purchasetype=="3"){

					}else{
        				htmlStr += '<div class="h-u-l-button"><a class="ui-button" href="'+payUrl+'" clstag="'+clatag+'">支付</a></div>';
        			}
        			
        		}else if (detail.project_type == "10017"){
        			htmlStr += '<div class="h-u-l-button"><a class="font-link" href="//jrb2b.jd.com/invest/order/detail?pid='+detail.item_code+'" target="_blank" clstag="'+clatag+'">交易详情</a></div>';
        		}else if (detail.project_type == "10022"||detail.project_type == "10021"||detail.project_type == "10029"){
					//小金库无交易详情
				}else if (detail.project_type == "101"){
					if(detail.affirm_status_str=='支付成功'||detail.affirm_status_str=='订单完成'){
						//交易详情页
						htmlStr += '<div class="h-u-l-button"><a class="font-link" href="'+aHref+'" target="_blank" clstag="'+clatag+'">交易详情</a></div>';
					}
				}else{
        			//交易详情页
        			htmlStr += '<div class="h-u-l-button"><a class="font-link" href="'+aHref+'" target="_blank" clstag="'+clatag+'">交易详情</a></div>';
        		}
				htmlStr += '</div>';
                										
            }
            $("#tradeBuyDataDiv").append(htmlStr);
        }
        else {
        	htmlStr += '<div class="ui-noRecord">暂无记录</div>';
            $("#tradeBuyDataDiv").html(htmlStr);
        }
    }
});


function toInsuranceDetail(url) {
    location.href = url + "&returnUrl=" + location.href;
}
function toInsuranceConfirm(url) {
    location.href = url + "&returnUrl=" + location.href;
}
function toInsurancePay(url) {
    location.href = url + "&returnUrl=" + location.href;
}

function chgIncomeSelect(div) {
    $("#" + div).show();
}
function chgDays4Income(days) {
    //alert(days);
    $("#latestDaysShow").html("近" + (days + 1) + "天");
    $("#latestDays").hide();
    $("#financeDays").val(days);
    var financeDays = $("#financeDays").val();
    var financeType = $("#financeType").val();
    initFinanceHistoryNewParam("incomeChatDiv", financeType, financeDays);
}

function eyeClickOverview() {
    try {
        eyeClick();
        var flag = getEyeStatus();
        if (flag == 0) showOverviewTop(0);
    } catch (e) {

    }
}

function showDebt(){
//白条欠款
    jQuery.ajax({
        url: "/async/creditData.action?_dc=" + new Date().getTime(),
        type: "GET",
        async: true,
        success: function (result) {
        	if(null != result ){
        		var totalDebt = formatAmount(result.totalDebt); //总负债
        		jQuery("#totalDebt").html(totalDebt);
        	}
        	if(null != result && parseInt(result.actStatus) == 2){
        		jQuery("#creditDebtSeven").html("近七日无须还款");
        		jQuery("#activateButton").toggle();
        		jQuery("#creditDebtSlave").html("0.00");
        		jQuery("#creditDebtPercentSlave").html("0%");//白条负债百分比
        		jQuery("#creditDebtSevenSlave").html("0.00");
        		jQuery("#availableLimitSlave").html("0.00");
        		jQuery("#creditLimitSlave").html("0.00");
        		
        	}else {
        		var creditWaitPay = formatAmount(result.creditWaitPay); //白条欠款
        		if (null != result.creditWaitPay && result.creditWaitPay > 0) jQuery("#creditDebtButton").toggle();
        		jQuery("#creditDebt").html(creditWaitPay);
        		jQuery("#creditDebtSlave").html(creditWaitPay);
        		var creditWaitPaySeven = formatAmount(result.creditWaitPaySeven); //7天白天待还
        		if (null != result.isOverdue && result.isOverdue == 1 && null != result.delinquencyBalance && result.delinquencyBalance > 0){
    				jQuery("#creditDebtSeven").html("<em class=\"font-red j_eye-line\">已逾期  " + result.delinquencyBalance + " 元</em> ");
    			}else{
    				if (null != result.creditWaitPaySeven && result.creditWaitPaySeven > 0) jQuery("#creditDebtSeven").html("近七日待付 <em class=\"font-grey3 j_eye-line\">" + creditWaitPaySeven + "</em> 元");
    				else {
    					jQuery("#creditDebtSeven").html("近七日无须还款");
    				}
    			}
        		jQuery("#creditDebtSevenSlave").html(creditWaitPaySeven);
        		var creditDebtPercent = result.creditWaitPayPercent;
        		jQuery("#creditDebtPercent").html(creditDebtPercent + "%");//白条负债百分比
        		if (creditDebtPercent == 100)  jQuery("#creditDebtPercent").addClass("size12");
        		jQuery("#creditDebtPercentSlave").html(creditDebtPercent + "%");//白条负债百分比
        		
        		var availableLimit = result.availableLimit; // 可用额度
        		var creditLimit = result.creditLimit; //总额度
        		jQuery("#availableLimitSlave").html(availableLimit);
        		jQuery("#creditLimitSlave").html(creditLimit);
        		
        		if (null != result.creditWaitPay && result.creditWaitPay > 0){
        			//代付款大于0
        			if (null != result.creditWaitPaySeven && result.creditWaitPaySeven > 0){
        				
        			}else{
        				if (null != result.isOverdue && result.isOverdue == 1 && null != result.delinquencyBalance && result.delinquencyBalance > 0){
        					
        				}else{
        					//近七日 付款无
        					jQuery("#creditDebtButton").toggle();
        					jQuery("#jdbtDetail").toggle();
        				}
        			}
        		}
        		
        	}
        	//旅游白条
        	if(null != result && parseInt(result.tourActStatus) == 2){
        		//旅游白条没有激活，则将 圈圈颜色 让给了 网商贷
        		jQuery("#jtBussinessLoanCircle").removeClass('r-b-circle6').addClass('r-b-circle5 size12');
        	}else {
        		jQuery("#tourCreditArea").toggle();
        		
        		var tourCreditWaitPay = formatAmount(result.tourCreditWaitPay); //白条欠款
        		if (null != result.tourCreditWaitPay && result.tourCreditWaitPay > 0) jQuery("#tourDebtButton").toggle();
        		jQuery("#tourDebt").html(tourCreditWaitPay);
        		jQuery("#tourDebtSlave").html(tourCreditWaitPay);
        		var tourWaitPaySeven = formatAmount(result.tourCreditWaitPaySeven); //7天白天待还
        		if (null != result.tourDelinquencyBalance && result.tourDelinquencyBalance > 0){
    				jQuery("#tourDebtSeven").html("<em class=\"font-red j_eye-line\">已逾期  " + result.tourDelinquencyBalance + " 元</em> ");
    			}else{
    				if (null != result.tourCreditWaitPaySeven & result.tourCreditWaitPaySeven > 0) jQuery("#tourDebtSeven").html("近七日待付 <em class=\"font-grey3 j_eye-line\">" + tourWaitPaySeven + "</em> 元");
    				else {
    					jQuery("#tourDebtSeven").html("近七日无须还款");
    				}
    			}
        		jQuery("#tourDebtSevenSlave").html(tourWaitPaySeven);
        		var tourCreditDebtPercent = result.tourCreditWaitPayPercent;
        		jQuery("#tourDebtPercent").html(tourCreditDebtPercent + "%");//白条负债百分比
        		if (tourCreditDebtPercent == 100)  jQuery("#tourDebtPercent").addClass("size12");
        		jQuery("#tourDebtPercentSlave").html(tourCreditDebtPercent + "%");//白条负债百分比
        		
        		var tourAvailableLimit = result.tourAvailableLimit; // 可用额度
        		var tourCreditLimit = result.tourCreditLimit; //总额度
        		jQuery("#tourAvailableLimitSlave").html(tourAvailableLimit);
        		jQuery("#tourCreditLimitSlave").html(tourCreditLimit);

        		if (null != result.tourCreditWaitPay && result.tourCreditWaitPay > 0){
        			//代付款大于0
        			if (null != result.tourCreditWaitPaySeven && result.tourCreditWaitPaySeven > 0){
        				
        			}else{
        				if (null != result.tourDelinquencyBalance && result.tourDelinquencyBalance > 0){
        					
        				}else{
        					//近七日 付款无
        					jQuery("#tourDebtButton").toggle();
        					jQuery("#tourDebtDetail").toggle();
        				}
        			}
        		}
        	}

			if(null != result && parseInt(result.tourActStatus) == 2
								&& parseInt(result.jtActStatus) == 2){
				jQuery("#elseLoanArea").toggle();
			}

			//京东金条 TODO 没激活是不是就不需要展示了&还款 详情url是啥，以及如果要激活，激活的url是啥
			if(null != result && parseInt(result.jtActStatus) == 2){
				//旅游白条没有激活，则将圈圈颜色让给了网商贷
				//jQuery("#netBussinessLoanCircle").removeClass('r-b-circle6').addClass('r-b-circle5 size12');
			}else {
				jQuery("#jtBussinessLoanArea").toggle();
				var jtCreditWaitPay = formatAmount(result.jtCreditWaitPay); //白条欠款
				if (null != result.jtCreditWaitPay && result.jtCreditWaitPay > 0) jQuery("#jtDebtButton").toggle();
				jQuery("#jtCreditDebtSlave").html(jtCreditWaitPay);
				jQuery("#jtBussinessLoanDebt").html(jtCreditWaitPay);
				var jtCreditWaitPaySeven = formatAmount(result.jtCreditWaitPaySeven); //7天白天待还
				if (null != result.jtDelinquencyBalance && result.jtDelinquencyBalance > 0){
					jQuery("#jtBussinessLoanSeven").html("<em class=\"font-red j_eye-line\">已逾期  " + result.jtDelinquencyBalance + " 元</em> ");
				}else{
					if (null != result.jtCreditWaitPaySeven & result.jtCreditWaitPaySeven > 0)
						jQuery("#jtBussinessLoanSeven").html("近七日待付 <em class=\"font-grey3 j_eye-line\">" + jtCreditWaitPaySeven + "</em> 元");
					else {
						jQuery("#jtBussinessLoanSeven").html("近七日无须还款");
					}
				}
				jQuery("#jtCreditDebtSevenSlave").html(jtCreditWaitPaySeven);
				var jtCreditWaitPayPercent = result.jtCreditWaitPayPercent;
				jQuery("#jtBussinessLoanPercent").html(jtCreditWaitPayPercent + "%");//白条负债百分比
				if (jtCreditWaitPayPercent == 100)  jQuery("#jtBussinessLoanPercent").addClass("size12");
				jQuery("#jtCreditDebtPercentSlave").html(jtCreditWaitPayPercent + "%");//白条负债百分比
				var jtAvailableLimit = result.jtAvailableLimit; // 可用额度
				var jtCreditLimit = result.jtCreditLimit; //总额度
				jQuery("#jtAvailableLimitSlave").html(jtAvailableLimit);
				jQuery("#jtCreditLimitSlave").html(jtCreditLimit);
				if (null != result.jtCreditWaitPay && result.jtCreditWaitPay > 0){
					//代付款大于0
					if (null != result.jtCreditWaitPaySeven && result.jtCreditWaitPaySeven > 0){
					}else{
						if (null != result.jtDelinquencyBalance && result.jtDelinquencyBalance > 0){
						}else{
							//近七日 付款无
							jQuery("#jtDebtButton").toggle();
							jQuery("#jdjtDetail").toggle();
						}
					}
				}
			}
        }
    });
}

function chgFinance4Income(str, type) {
    $("#myFinanceJrbShow").html(str);
    $("#myFinanceJrb").hide();
    $("#financeType").val(type);
    var financeDays = $("#financeDays").val();
    var financeType = $("#financeType").val();
    initFinanceHistoryNewParam("incomeChatDiv", financeType, financeDays);
}

function showOverviewTop(type) {
    var flag = $("#eyeDateShow").val();
    if (flag == "0") {
    	if(type == 0){
    		$("#eyeDateShow").val("1");
    		showDebt();
    		jQuery.ajax({
    			url: "/async/browseDataNew.action?_dc=" + new Date().getTime(),
    			type: "GET",
    			async: true,
    			success: function (result) {
    				try {
    					var allBalance = formatOverViewAmount(result.data.totalMoney); //总资产
    					$("#allBalance").html(allBalance);
						if(result.enableProof!=null && result.enableProof == 'enable'){
							$("#enableProof").val('1');
						}
						else {
							$('#J_r-prove').hide()
						}
    					var jrbBalance = formatOverViewAmount(result.data.balance);         //京东小金库余额
    					$("#jrbBalance").html(jrbBalance);
    					$("#jrbBalanceSlave").html(jrbBalance);
    					$("#jrbBalanceFreeze").html(formatOverViewAmount(result.data.balanceFreeze));
    					$("#xjk_freeze").html(formatOverViewAmount(result.data.balanceFreeze));
    					$("#jrbBalancePercent").html(result.data.balancePercent + "%");
    					$("#jrbBalancePercentSlave").html(result.data.balancePercent + "%");
    					if (result.data.balancePercent == 100)$("#jrbBalancePercent").addClass("size12");
    					
    					var walletBalance = formatOverViewAmount(result.data.walletMoneyAvailable);         //钱包可用余额
    					$("#walletBalance").html(walletBalance);
    					$("#walletBalanceSlave").html(walletBalance);
    					
    					$("#walletBalancePercent").html(result.data.walletMoneyAvailablePercent + "%");
    					$("#walletBalancePercentSlave").html(result.data.walletMoneyAvailablePercent + "%");
    					if (result.data.walletMoneyAvailablePercent == 100)$("#walletBalancePercent").addClass("size12");
    					
    					var historyBalance = formatOverViewAmount(result.data.fund);   //我的理财金额
    					var historyBalanceArray = historyBalance.split(".");
    					$("#historyBalance").html(historyBalanceArray[0] + "." + historyBalanceArray[1]);
    					$("#historyBalanceSlave").html(historyBalanceArray[0] + "." + historyBalanceArray[1]);
    					$("#historyBalancePercent").html(result.data.fundPercent + "%");
    					$("#historyBalancePercentSlave").html(result.data.fundPercent + "%");
    					if (result.data.fundPercent == 100)$("#historyBalancePercent").addClass("size12");
    					var incomeFinanceYesterday = formatAmount(result.data.incomeFinanceYesterday);//我的理财昨日收益
    					//我的理财昨日收益
    					if (result.data.incomeFinanceYesterday > 0) {
    						jQuery("#incomeFinanceYesterday").html("+" + incomeFinanceYesterday);
    					}
    					else{
    						jQuery("#incomeFinanceYesterdayFont").removeClass('font-red').addClass('font-grey1');
    						jQuery("#incomeFinanceYesterday").html(incomeFinanceYesterday);
    					}
    					
    					var currIncome = formatAmount(result.data.currIncome);
    					if(currIncome >0) {
    						//京东小金库昨日收益
    						jQuery("#currIncome").html("+" + currIncome);
    					}else{
    						jQuery("#currIncomeFont").removeClass('font-red').addClass('font-grey1');
    						jQuery("#currIncome").html(currIncome);
    					}
    					
    					var totalIncome = formatAmount(result.data.totalIncome);
    					jQuery("#totalIncome").html(totalIncome);//京东小金库累计收益
    					jQuery("#totalIncomeSlave").html(totalIncome);//京东小金库累计收益
    					
    					var incomeFinanceSum = formatAmount(result.data.incomeFinanceSum);
    					jQuery("#incomeFinanceSumSlave").html(incomeFinanceSum);//我的理财累计收益
    					
    					var incomeSumYesterday = formatAmount(result.data.incomeSumYesterday);
    					
    					jQuery("#topIncomeSumYesterdaySlave").toggle();
    					if (result.data.incomeSumYesterday > 0) {
    						jQuery("#incomeSumYesterday").html("+" + incomeSumYesterday);//我的理财京东小金库昨日收益
    						jQuery("#incomeSumYesterdaySlave").html("+" + incomeSumYesterday);//我的理财京东小金库昨日收益
    						jQuery("#incomeSumYesterdaySlave1").html("+" + incomeSumYesterday);//我的理财京东小金库昨日收益
    						jQuery("#topIncomeSumYesterdaySlave").removeClass('r-h-tip').addClass('r-h-tip-red'); //改变我的资产后面昨日收益框框样式
    					}
    					else {
    						jQuery("#incomeSumYesterday").removeClass('font-red').addClass('font-blue');
    						if(result.data.incomeSumYesterday == 0){
//                    		jQuery("#incomeIco").remove();
    							jQuery("#topIncomeSumYesterdaySlave").remove();
    							//收益等于0的时候，判断是否来收益count
    							var lastestIncomeFlag = result.data.lastestIncomeFlag;
    							if(lastestIncomeFlag == "0"){
    								//今日收益还没来
    								jQuery("#incomeSumYesterdaySlave1").removeClass('font-red').addClass('font-size28-s');
        							jQuery("#incomeSumYesterdaySlave1").html("暂无收益");//我的理财京东小金库昨日收益
    							}
    							
    						}else{
    							jQuery("#topIncomeSumYesterdaySlave").removeClass('r-h-tip').addClass('r-h-tip-grey'); //改变我的资产后面昨日收益框框样式
    							
    							jQuery("#incomeSumYesterday").html(incomeSumYesterday);//我的理财京东小金库昨日收益
    							jQuery("#incomeSumYesterdaySlave").html(incomeSumYesterday);//我的理财京东小金库昨日收益
    							jQuery("#incomeSumYesterdaySlave1").removeClass('font-red').addClass('font-blue');
    							jQuery("#incomeSumYesterdaySlave1").html(incomeSumYesterday);//我的理财京东小金库昨日收益
    						}
    					}
    					
    					var incomeTotal = formatAmount(result.data.incomeTotal);
    					jQuery("#incomeFinanceSum").html(incomeTotal);//我的理财京东小金库累计收益
    					
    					//京东小金库冻结标识
    					var balanceFreeze = result.data.balanceFreeze == null?0:result.data.balanceFreeze;
    					balanceFreeze = parseFloat(balanceFreeze);
    					if(balanceFreeze>0){
    						$('#freeze_mark').show();
    					}else{
    						$('#freeze_mark').hide();
    					}
    					//网银钱包冻结标识
    					var freezeMoney = result.data.freezeMoney == null?0:result.data.freezeMoney;
    					freezeMoney = parseFloat(freezeMoney);
    					if(freezeMoney>0){
    						jQuery("#walletBalanceDong").html(freezeMoney);
    						$('#walletDongDiv').toggle();
    					}
                        var pickDate = result.pick;
                        jQuery("#incomeDate").html(pickDate);
                        
    				} catch (e) {
    					
    				}
    			}
    		});
    		
    	}else{
    	}
    }
}

//展示我的京东小金库体验金
function showMyActivityGift(){
    jQuery.ajax({
        url: "/async/findActivityGift.action?_dc=" + new Date().getTime(),
        type: "GET",
        async: true,
        success: function (response) {
        	if (null != response && response.result!=false) {
                //有体验金 置图标
        		$("#overViewDetail").append('<em class="r-b-l-ico"></em>');
            }
        	
        }
    });
}

//提示我的 赎回中的交易单 数量
function showOverViewSellCount(){
    jQuery.ajax({
        url: "/trade/overViewSellCount.action?_dc=" + new Date().getTime(),
        type: "GET",
        async: true,
        success: function (response) {
        	if (null != response && response.resultCount >0) {
                //将提示信息展示
        		$("#sellingTips").toggle();
        		$("#sellingTips").html('有'+response.resultCount+'笔赎回中的交易，可能会影响您的资产总额。');
            }
        	
        }
    });
}

function toFreezeList(){
	$('#freezeForm').submit();
}

function overviewShowNotice(){
	var content='';
	var address = 1;
	jQuery.ajax({
		url: "/async/browseNoticeControlData.action?address="+address+"&_dc=" + new Date().getTime(),
		type: "GET",
		async: true,
		success: function (result) {
			content = result.noticeContent;
			if(content!=""){
				var htmlStr = content;
				jQuery("#notice_overview").html(htmlStr);
				assetsCommon.hornTip();
			}
		},
		error:function(result){
		}
	});
}

/**
 * 展示资产总览改版  推荐数据
 * @param index
 * @returns
 */
function showRecommendData(index){
    jQuery.ajax({
        url: "/async/browseRecommendDataNew.action?index="+index+"&count=6&_dc=" + new Date().getTime(),
        type: "GET",
        async: false,
        success: function (response) {
        	var adLoveList = response.list;
        	var htmlStr = "";
        	if(adLoveList != null && adLoveList.length != 0){
        		for(var i=0;i<adLoveList.length;i++){
            		htmlStr += '<div class="u-l-list">';
					var url1 =  adLoveList[i].url;
            		if(adLoveList[i].itemName==null||adLoveList[i].itemName==undefined){
            			htmlStr += '    <a class="u-l-l-title" href="#" target="_blank">--</a>';
            		}else{

						if(null != url1 && (url1.endWith("htm") == true || url1.endWith("html") == true)){
							htmlStr += '<a class="u-l-l-title" target="_blank" clstag="jr|keycount|jr_zczl|jr_cnxh_'+response.recrule+'_'+(i+1)+'" href="'+adLoveList[i].url+'?from=cnxh_1_'+response.recrule+'_'+(i+1)+'">'+adLoveList[i].itemName+'</a>';
						}else{
							htmlStr += '<a class="u-l-l-title" target="_blank" clstag="jr|keycount|jr_zczl|jr_cnxh_'+response.recrule+'_'+(i+1)+'" href="'+adLoveList[i].url+'&from=cnxh_1_'+response.recrule+'_'+(i+1)+'">'+adLoveList[i].itemName+'</a>';
						}
            		}

					if(null == adLoveList[i].profit || adLoveList[i].profit == '0.00' || adLoveList[i].profit == 'null' || adLoveList[i].profit == 'NULL'){
						htmlStr += ' <div class="u-l-l-number"><em class="u-l-l-num">--</em>%</div>';
					}else{
						htmlStr += ' <div class="u-l-l-number"><em class="u-l-l-num">'+formatnumber(adLoveList[i].profit,2)+'</em>%</div>';
					}

					htmlStr += '    <div class="u-l-l-tip">'+adLoveList[i].increase_word+'</div>';
                 	var riskName = '';
                 	var lowMoney = '';
                 	var tips = '';
                 	if(adLoveList[i].itemSource!=null&&adLoveList[i].itemSource!=undefined&&(adLoveList[i].itemSource=="1" || adLoveList[i].itemSource=="3")){
                		if(adLoveList[i].riskLevelName==null||adLoveList[i].riskLevelName==undefined||adLoveList[i].riskLevelName=="UNKNOWN"){
                			riskName = '-----';
                    	}else{
                    		riskName = adLoveList[i].riskLevelName+'风险';
                    	}
                	}
                	if(adLoveList[i].itemSource!=null&&adLoveList[i].itemSource!=undefined&&(adLoveList[i].itemSource=="1" || adLoveList[i].itemSource=="3"||adLoveList[i].itemSource=="4"||adLoveList[i].itemSource=="5")){
                		if(adLoveList[i].minInvestAmount==null||adLoveList[i].minInvestAmount==undefined){
                			lowMoney = '-----';
                		}else{
                			lowMoney = Math.floor(adLoveList[i].minInvestAmount)+'元起购';
                		}
                	}else{
                		if(adLoveList[i].amount==null||adLoveList[i].amount==undefined){
                			lowMoney = '-----';
                		}else{
                			lowMoney += Math.floor(adLoveList[i].amount)+'元/份';
                		}
                	}
                	
                	if(adLoveList[i].itemSource!=null&&adLoveList[i].itemSource!=undefined&&(adLoveList[i].itemSource=="2"||adLoveList[i].itemSource=="4"||adLoveList[i].itemSource=="5")){
                		if(adLoveList[i].insPeriodBenefit==null||adLoveList[i].insPeriodBenefit==undefined||adLoveList[i].insPeriodBenefit=='NULL'){
                			tips = '随时可取';
                		}else{
                			tips = '建议持有'+adLoveList[i].insPeriodBenefit;
                		}
                	}
                	
                	if(adLoveList[i].itemSource!=null&&adLoveList[i].itemSource!=undefined&&(adLoveList[i].itemSource=="1" || adLoveList[i].itemSource=="3")){
                		htmlStr += '<div class="u-l-l-info">'+riskName+' | '+lowMoney+'</div>';
                	}
                	if(adLoveList[i].itemSource!=null&&adLoveList[i].itemSource!=undefined&&(adLoveList[i].itemSource=="2"||adLoveList[i].itemSource=="4"||adLoveList[i].itemSource=="5")){
                		htmlStr += '<div class="u-l-l-info">'+lowMoney+' | '+tips+'</div>';
                	}
                	
                	htmlStr += '             <div class="u-l-l-button">';

					if(null != url1 && (url1.endWith("htm") == true || url1.endWith("html") == true)){
						htmlStr += '<a class="ui-button" clstag="jr|keycount|jr_zczl|jr_cnxh_'+response.recrule+'_'+(i+1)+'" target="_blank" href="'+adLoveList[i].url+'?from=cnxh_1_'+response.recrule+'_'+(i+1)+'">立即抢购</a>';
					}else{
						htmlStr += '<a class="ui-button" clstag="jr|keycount|jr_zczl|jr_cnxh_'+response.recrule+'_'+(i+1)+'" target="_blank" href="'+adLoveList[i].url+'&from=cnxh_1_'+response.recrule+'_'+(i+1)+'">立即抢购</a>';
					}
                	htmlStr += '             </div>';
                	htmlStr += '        </div>';
            	}
        	}else{
        		htmlStr += '<div class="ui-noRecord">暂无记录</div>';
        	}
        	jQuery("#drLikeDiv").html(htmlStr);
        }
    });
}

function showMyIncomeQRCode(){
	//理财页面加载显示二维码
	jQuery.ajax({
	    url: "/ajaxFinance/getEncryptionPin.action?&_dc=" + new Date().getTime(),
	    type: "GET",
	    async: true,
	    success: function(result){
	    	if(null != result){
	    		var encodePin = result.encodePin;
	    		
	    		/**
	             * 晒收益二维码显示
	             * assetsIndex.richQr('加密后的 京东pin');
	             */
	            assetsIndex.richQr(encodePin);
	    	}
	    	
	    }
	});
}

//历史收益 波浪图 后端数据格式
function showOverViewIncome(){
	jQuery.ajax({
	    url: "/centre/getOverviewInData.action?&_dc=" + new Date().getTime(),
	    type: "GET",
	    async: true,
	    success: function(result){
	    	if(null != result){
	    		var indexData = {
	    	            success: true,
	    	            maxIncome: result.maxIncome,
	    	            status: 3,
	    	            data: result.incomeData
	    	        };
				// 首页图表渲染
				assetsIndex.chart(indexData);
	    	}
	    	
	    }
	});
        
}

/**
 * 异步调取新的资产总览页面 今日排名 以及排名变化
 *
 */
function getMyRankInfo(){
	jQuery.ajax({
	    url: "/centre/getUserRankInfo.action?&_dc=" + new Date().getTime(),
	    type: "GET",
	    async: true,
	    success: function(result){
	    	if(null != result){
	    		var rankFlag = result.rankFlag;
	    		var rankChange = result.rankChange;
	    		var nowRank = result.nowRank;
	    		if(rankFlag == "-1"){
	    			//向下
	    			jQuery("#rankClazz").removeClass("p-r-h-tip-red").addClass("p-r-h-tip-grey");
	    		}
	    		if(null != rankChange) jQuery("#rankChange").html(rankChange);
	    		if(null != nowRank){
	    			jQuery("#todayRank").html(nowRank);
	    		}else{//今日收益排名没获取到
	    			$("#rankSpan").css("visibility","hidden");
	    		}
	    	}
	    	
	    }
	});
}

/**
 * 异步加载理财天数
 */
function showMyFinancingDays(){
	jQuery.ajax({
	    url: "/centre/getMyFinancingDays.action?&_dc=" + new Date().getTime(),
	    type: "GET",
	    async: true,
	    success: function(result){
	    	if(null != result){
	    		var days = result.days;
	    		jQuery("#financingDays").html(days);
	    	}
	    	
	    }
	});
}

jQuery().ready(function () {
	var showFlag = $("#showFlag").val();
	if(showFlag == 1){
		$('#J_p-r-pkBtn').click();
	}
});

/**
 * 构建资产总览  大列表html
 * @returns {String}
 */
function buildOverviewSelectHtml(json){
	var html = "";
	html += '<colgroup>';
	html += '<col width="120">';
	html += '<col width="280">';
	html += '<col width="300">';
	html += '<col width="">';
	html += '</colgroup>';
	html += '<tr>';
	html += '<th>年化收益率</th>';
	html += '<th>期限/锁定期</th>';
	html += '<th class="t-left">产品名称</th>';
	/*html += '<th>起购金额(元)</th>';*/
	html += '<th>操作</th>';
	html += '</tr>';
	//if(null != json && json.errorCode == "0" && json.list != null && json.list.length != 0){
	if(null != json && json.length != 0){
		//数据条件没问题 开始拼接html
		 for (var i = 0; i < json.length; i++) {
             var data = json[i];
             html += '<tr>';
             html += '<td><p class="font-red"><em class="font-size22">' + data.itemYield + '</em>%</p><p class="font-grey1">'+data.rateDesc+'</p></td>';
			 if(!isNaN(data.investPeriod)){
				//数字
				 if(data.tips != null && data.tips != ''){
					 //if(data.type != null && data.type == 'bxlc') html += '<td><div><span class="font-red">锁定期</span><em class="font-size22">'+data.invest_period+'</em>天<div class="ui_infoPopBtn2"><em class="ui_infoPopBtn-ico"></em><div class="ui_infoPop-bg"><em class="ui_infoPop-ico"></em><div class="ui_infoPop"><p>'+data.tips+'</p></div></div></div></div></td>';
					 html += '<td><p><em class="font-size22">'+data.investPeriod+'</em>天</p><p class="font-grey1">'+data.tips+'</p></td>';
				 }else{
					 //if(data.type != null && data.type == 'bxlc') html += '<td><p><span class="font-red">锁定期</span><em class="font-size22">' + data.invest_period + '</em>天</p></td>';
					 html += '<td><p><em class="font-size22">' + data.investPeriod + '</em>天</p></td>';

				 }
			 }else{
				 if(data.tips != null && data.tips != ''){
					 //if(data.type != null && data.type == 'bxlc') html += '<td><div><span class="font-red">锁定期</span><em class="font-size20">'+data.invest_period+'</em><div class="ui_infoPopBtn2"><em class="ui_infoPopBtn-ico"></em><div class="ui_infoPop-bg"><em class="ui_infoPop-ico"></em><div class="ui_infoPop"><p>'+data.tips+'</p></div></div></div></div></td>';
					 /*html += '<td>'+ data.invest_period+'<div class="ui_infoPopBtn2"><em class="ui_infoPopBtn-ico"></em><div class="ui_infoPop-bg"><em class="ui_infoPop-ico"></em><div class="ui_infoPop"><p>'+data.tips+'</p></div></div></div></td>';*/
					 html += '<td><p><em class="font-size16">' + data.investPeriod + '</em></p><p class="font-grey1">'+data.tips+'</p></td>';
				 }else{
					// if(data.type != null && data.type == 'bxlc') html += '<td><p><span class="font-red">锁定期</span><em class="font-size20">' + data.invest_period + '</em></p></td>';
					 html += '<td><p><em class="font-size16">' + data.investPeriod + '</em></p></td>';

				 }
			 }

             html += '<td class="t-left"><p><a class="t-name" href="' + data.url + '" target="_blank" clstag="jr|keycount|jr_zczl|jr_yxlc_'+data.type+'_'+data.id+'">' + data.itemName + '</a></p></td>';
             /*html += '<td><p>' + formatAmount(data.mininvest_amount) + '</p></td>';*/
			 if(data.type != null && data.type == 'jklc'){ //小金库产品理财
				 html += '<td><p><a class="ui-button" id="xjkAUrl" href="//trade.jr.jd.com/xjkin/topayin.action" target="_blank" clstag="jr|keycount|jr_zczl|jr_yxlc_'+data.type+'_'+data.id+'">立即购买</a></p></td>';
			 }else{
				 html += '<td><p><a class="ui-button" href="' + data.url + '" target="_blank" clstag="jr|keycount|jr_zczl|jr_yxlc_'+data.type+'_'+data.id+'">立即购买</a></p></td>';
			 }
             html += '</tr>';
		 }
		
	}
	return html;
}


/**
 * 优选理财 jsf接口
 * @param type
 */
function showOverviewSelect(type) {
	$("#overviewSelect" + type).html('<div class="ui_loading"></div>');
	//type 0：全部 ；1: 0 - 3个月内；2:3 - 6个月；3:6 - 12个月；4:12个月以上
	jQuery.ajax({
		type : "GET",
		async: true,
		url : "/async/getDalicaiInfos.action?termNo=" + type,
		//dataType : "jsonp",
		success : function(json){
			var flag = json.flag;
			if(null != flag && flag == '1' && json.data != null){
				$("#overviewSelect" + type).html(buildOverviewSelectHtml(json.data));
				//alert($("#xjkAUrl").length);
				//if($("#xjkAUrl").length > 0){
				//	//大理财列表 存在小金库理财  异步去加载url 填充
				//	getMyXJKUrl();
				//}

			}else{
				$("#overviewSelect" + type).html('<div class="ui-noRecord">暂无记录</div>');
			}
		},
		error:function(){
			$("#overviewSelect" + type).html('<div class="ui-noRecord">暂无记录</div>');
		}
	});

}