$(function() {
    // 设置弹出日期提示 只有年月日
    $(".dateSet").focus(function() {
        WdatePicker({dateFmt:'yyyy-MM-dd',onpicked:changeFunc_DatePicker});
    });
    // 设置弹出日期提示 只有年月日
    $(".dateNoDaySet").focus(function() {
        WdatePicker({dateFmt:'yyyy-MM',onpicked:changeFunc_DatePicker});
    });
    // 包含时 分 秒
    $(".dateTimeSet").focus(function() {
        WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',onpicked:changeFunc_DatePicker});
    });
    
});

function changeFunc_DatePicker(){
    if(typeof changeDatePicker != "undefined" && typeof changeDatePicker == "function"){
        changeDatePicker();
    }
}

/**
 * 判断是否为空
 * 
 * @param str
 */
function isNull(str){
    if(str != null) str = Trim(str)
    if(str==null || str.length<1 || str==undefined){
        return true;
    }
    return false;
}

/**
 * 去其空格
 * 
 * @param str
 */
function Trim(str){
    return str.replace(/^\s*/,'').replace(/\s*$/,'');
}

/**
 * 检查是否为大于等于0的整数
 * 
 * @param str
 */
function checkIsNumber(str){
    str = Trim(str);
    var ppp=/^[0-9]{1,20}$/;
    if(!ppp.exec(str)){
        return false;
    }
    return true;
}

/**
 * 验证是否为数字
 * 
 * @param String
 * @return Boolean
 */
function checknumber(String){
    var Letters = "1234567890.";
    var i;
    var c;
    for( i = 0; i < String.length; i++ ){
        c = String.charAt( i );
        if (Letters.indexOf( c ) ==-1){
            return true;
        }
    }
    return false;
}

/**
 * ^[1-9]d*$ //匹配正整数 ^-[1-9]d*$ //匹配负整数 ^-?[1-9]d*$ //匹配整数 ^[1-9]d*|0$
 * //匹配非负整数（正整数 + 0） ^-[1-9]d*|0$ //匹配非正整数（负整数 + 0） ^[1-9]d*.d*|0.d*[1-9]d*$
 * //匹配正浮点数 ^-([1-9]d*.d*|0.d*[1-9]d*)$ //匹配负浮点数
 * ^-?([1-9]d*.d*|0.d*[1-9]d*|0?.0+|0)$ //匹配浮点数 ^[1-9]d*.d*|0.d*[1-9]d*|0?.0+|0$
 * //匹配非负浮点数（正浮点数 + 0） ^(-([1-9]d*.d*|0.d*[1-9]d*))|0?.0+|0$ //匹配非正浮点数（负浮点数 + 0）
 * 功能： 判断是否为数字 参数： strNumber： 数字字符串 flag： 数字字符串类型 返回： 如果通过验证则返回true,否则返回false
 */
function isNumeric(strNumber, flag) { 
	if(isNaN(strNumber)) { 
		return false; 
	} 
	
	switch(flag) { 
		case null:// 数字
            return false;
        case "n":
        	return /^\+?[1-9][0-9]*$/.test(strNumber);
		case "": // 正整数 + 0
			return /^\d+$/.test(strNumber);  
		case "0": // 零和非零开头的数字：“^(0|[1-9][0-9]*)$”
			return /^(0|[1-9][0-9]*)$/.test(strNumber); 
		case "1":  // 有1位小数的正实数："^[0-9]+(.[0-9]{1,3})?$"
			return /^[0-9]+(.[0-9]{1,3})?$/.test(strNumber);
		case "2":  // 有2位小数的正实数："^[0-9]+(.[0-9]{2})?$"
			return /^[0-9]+(.[0-9]{2})?$/.test(strNumber);
		case "3":  // 非零的正整数："^\+?[1-9][0-9]*$"
			return /^\+?[1-9][0-9]*$/.test(strNumber);
		case "4":  // 非零的负整数："^\-?[1-9][0-9]*$"
			return /^\-?[1-9][0-9]*$/.test(strNumber);
		case "+":// 正数
			return /(^\+?|^\d?)\d*\.?\d+$/.test(strNumber); 
		case "-":// 负数
			return /^-\d*\.?\d+$/.test(strNumber); 
		case "i":// 整数
			return /(^-?|^\+?|\d)\d+$/.test(strNumber); 
		case "+i":// 正整数
			return /(^\d+$)|(^\+?\d+$)/.test(strNumber); 
		case "-i":// 负整数
			return /^[-]\d+$/.test(strNumber); 
		case "f":// 浮点数
			return /(^-?|^\+?|^\d?)\d*\.\d+$/.test(strNumber); 
		case "+f":// 正浮点数
			return /(^\+?|^\d?)\d*\.\d+$/.test(strNumber); 
		case "-f":// 负浮点数
			return /^[-]\d*\.\d$/.test(strNumber); 
		case "f6":// 负浮点数 6位小数
			return /^-?\d+\.\d{6}$/.test(strNumber); 
		default:// 缺省
			return true; 
	} 
}


/**
 * 功能： 日期比较 参数： strDate1： 日期字符串1 strDate2： 日期字符串2 返回：
 * 如果strDate1>strDate2返回1,否则返回false
 */ 
function compareDate(strDate1, strDate2) {
	var nStart1;
	var nEnd1;
	var nYear1;
	var nMonth1;
	var nDay1;
	var nStart2;
	var nEnd2;
	var nYear2;
	var nMonth2;
	var nDay2;
	strDate1 = trimString(strDate1, 0);
	strDate2 = trimString(strDate2, 0);
	if ((nStart1 = strDate1.indexOf("/", 0)) < 0) {
		nStart1 = strDate1.indexOf("-", 0);
		nEnd1 = strDate1.indexOf("-", nStart1 + 1);
	} else {
		nEnd1 = strDate1.indexOf("/", nStart1 + 1);
	}
	nYear1 = eval(strDate1.substring(0, nStart1));
	nMonth1 = eval(strDate1.substring(nStart1 + 1, nEnd1));
	nDay1 = eval(strDate1.substring(nEnd1 + 1, strDate1.length));
	if ((nStart2 = strDate2.indexOf("/", 0)) < 0) {
		nStart2 = strDate2.indexOf("-", 0);
		nEnd2 = strDate2.indexOf("-", nStart2 + 1);
	} else {
		nEnd2 = strDate2.indexOf("/", nStart2 + 1);
	}
	nYear2 = eval(strDate2.substring(0, nStart2));
	nMonth2 = eval(strDate2.substring(nStart2 + 1, nEnd2));
	nDay2 = eval(strDate2.substring(nEnd2 + 1, strDate2.length));
	if (nYear1 > nYear2) {
		return (1);
	} else {
		if (nYear1 < nYear2) {
			return (-1);
		} else {
			if (nMonth1 > nMonth2) {
				return (1);
			} else {
				if (nMonth1 < nMonth2) {
					return (-1);
				} else {
					if (nDay1 > nDay2) {
						return (1);
					} else {
						if (nDay1 < nDay2) {
							return (-1);
						} else {
							return (0);
						}
					}
				}
			}
		}
	}
}

/*
 * 参数 date(Date()),dateStr(String:yyyy-MM-dd) Return： date>dateStr 1, date =
 * dateStr 0, date<dateStr -1
 */
function compareDate2Str(date,dateStr){
    var y1 = date.getFullYear();
    var m1 = date.getMonth() + 1;
    var d1 = date.getDate();

    var dateStrArr = dateStr.split("-");
    var y2 = new Number(dateStrArr[0]);
    var m2 = new Number(dateStrArr[1]);
    var d2 = new Number(dateStrArr[2]);

    if(y1>y2){
        return 1;
    }else if(y1 == y2){
        if(m1>m2){
            return 1;
        }else if(m1==m2){
            if(d1>d2){
                return 1;
            }else if(d1==d2){
                return 0;
            }else{
                return -1;
            }
        }else{
            return -1;
        }
    }else{
        return -1;
    }
}

/**
 * 功能： 去掉字符串的前后空格 参数： strVal 字符串 返回： 去掉前后空格的字符串
 */ 
function trimString(strVal)
{
    var reVal;
    var strTmp;
    strTmp = strVal + "";
    if (strTmp.length == 0)
        return (strTmp);
    reVal = /^(\s|　)*/;
    strTmp = strTmp.replace (reVal, '');
    reVal = /(\s|　)*$/;
    return (strTmp.replace (reVal, ''));
}





/**
 * 功能： 日期合法性检查 参数： strDate： 日期字符串 返回： 如果通过验证返回true,否则返回false
 */ 
function isDate (strDate)
{
    var nStart;
    var nEnd;
    var nYear;
    var nMonth;
    var nDay;
    var nFact;
    var arrDay = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
    strDate = (trimString (strDate));
    if (strDate.length == 0)
        return (false);
    reVal = /^([1-2]\d{3})[\/|\-](0?[1-9]|10|11|12)[\/|\-]([1-2]?[0-9]|0[1-9]|30|31)$/;
    if (!reVal.test (strDate))
        return (false);
    nStart = strDate.indexOf ("/", 0);
    nEnd = strDate.indexOf ("/", nStart + 1);
    nYear = eval (strDate.substring (0, nStart));
    nMonth = eval (strDate.substring (nStart + 1, nEnd));
    nDay = eval (strDate.substring (nEnd + 1, strDate.length));
    nFact = arrDay[nMonth - 1];
    if (nMonth == 2)
    {
        if ((nYear % 4 == 0 && nYear %100 != 0) || (nYear % 400 == 0))
            nFact ++;
    };
    if (nDay > nFact)
        return (false);
    return (true);
}

/**
 * 判断是不是人民币
 * @param s
 * @returns {Boolean}
 */
function isMoney( s ){   
 var regu = "^[1-9][0-9]*(\\.)?([0-9]){0,2}$|^[0-9](\\.)?([0-9]){0,2}$";
 var re = new RegExp(regu);
 if (re.test(s)) {
    return true;
 } else {
    return false;
 }
}
 
// =======================================================
// 函数名 ： 计算字符串长度（汉字两位，英文数字一位）
// 参数 ： 空
// 返回值 ： 字符串长度
// 修改历史 ：
// =======================================================
function Len(str)
{
     var i,sum;
     sum=0;
     for(i=0;i<str.length;i++)
     {
         if ((str.charCodeAt(i)>=0) && (str.charCodeAt(i)<=255))
             sum=sum+1;
         else
             sum=sum+2;
     }
     return sum;
}

// =======================================================
// 函数名 ： 获取当前时间
// 参数 ： 空
// 返回值 ： 但前时间字符串，格式为：2007-10-31
// 修改历史 ：
// =======================================================
function getNowDate()
{
      var d,s,t;
	  d=new Date();
	  s=d.getFullYear().toString(10)+"-";
	  t=d.getMonth()+1;
	  s+=(t>9?"":"0")+t+"-";
	  t=d.getDate();
	  s+=(t>9?"":"0")+t+" ";
	  // t=d.getHours();
	  // s+=(t>9?"":"0")+t+":";
	  // t=d.getMinutes();
	  // s+=(t>9?"":"0")+t+":";
	  // t=d.getSeconds();
	  // s+=(t>9?"":"0")+t;
      return s;
}

function calculate_byte(sTargetStr) {
    var sTmpStr, sTmpChar;
    var nOriginLen = 0;
    var nStrLength = 0;
    sTmpStr = new String(sTargetStr);
    nOriginLen = sTmpStr.length;
    for ( var i=0 ; i < nOriginLen ; i++ ) {
            sTmpChar = sTmpStr.charAt(i);
            if (escape(sTmpChar).length > 4) {
                    nStrLength += 2;
            } else if (sTmpChar!='\r') {
                    nStrLength ++;
            }
    }
    return nStrLength; 
}

function validateSize(str,len){
	if(calculate_byte(str)>len){
		alert('字数不能超过'+len);
		return true;
	}
	return false;
}

function trimText(ob,len){
	ob.value=Trim(ob.value); 
	if(calculate_byte(ob.value)>len){
		var sum=0;
		 var strTemp=ob.value;
		 var i;
		 for(i=0;i <strTemp.length;i++) 
	     { 
	            if((strTemp.charCodeAt(i)>=0)   &&  (strTemp.charCodeAt(i)<=255)) 
	           		 sum=sum+1; 
	            else 
	           		 sum=sum+2; 
	            if(sum>len)
	            	break;
	     } 
		 ob.value=ob.value.substring(0,i);
	}
}

function getDays(strDateStart,strDateEnd){
   var strSeparator = "-"; // 日期分隔符
   var oDate1;
   var oDate2;
   var iDays;
   oDate1= strDateStart.split(strSeparator);
   oDate2= strDateEnd.split(strSeparator);
   var strDateS = new Date(oDate1[0], oDate1[1]-1, oDate1[2]);
   var strDateE = new Date(oDate2[0], oDate2[1]-1, oDate2[2]);
   iDays = parseInt(Math.abs(strDateS - strDateE ) / 1000 / 60 / 60 /24);// 把相差的毫秒数转换为天数
   return iDays ;
}

//增加弹出框的功能
function open_window(url,name,iWidth,iHeight)
{
	var url; // 转向网页的地址;
	var name; // 网页名称，可为空;
	var iWidth; // 弹出窗口的宽度;
	var iHeight; // 弹出窗口的高度;
	var iTop = (window.screen.availHeight-30-iHeight)/2; // 获得窗口的垂直位置;
	var iLeft = (window.screen.availWidth-10-iWidth)/2; // 获得窗口的水平位置;
	window.open(url,name,'height='+iHeight+',,innerHeight='+iHeight+',width='+iWidth+',innerWidth='+iWidth+',top='+iTop+',left='+iLeft+',toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=yes,status=no');
}

/** 
 * 判断是不是enter键
 */
function isEnterDown(e) {
    var keynum;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if (keynum != 13)return false;
    return true;
}


/**
 * 错误信息 弹出框
 * @param titleMsg   <h3> 号字体
 * @param content 普通字体
 */
function alertErrorMsg(titleMsg,content){
    var msgddd='<div class="thickcon">';
    msgddd=msgddd+'<div class="tip-msg">';
    msgddd=msgddd+'<div class="tip-fail clearfix">';
    msgddd=msgddd+'<span class="icon"></span>';
    msgddd=msgddd+'<div class="font-gray fl">';
    msgddd=msgddd+'<h3>'+titleMsg+'</h3>';
    msgddd=msgddd+'<p>'+content+'</p>';
    msgddd=msgddd+'</div>';
    msgddd=msgddd+'</div>';
    msgddd=msgddd+'<div class="tip-op" style="text-align: center">';
    msgddd=msgddd+'<a herf="#" class="btn btn-yellow btn-m thickclose">确定</a>';
    msgddd=msgddd+'</div>';
    msgddd=msgddd+'</div>';
    msgddd=msgddd+'</div>';
    msgddd=msgddd+'<a href="#" class="thickclose" >×</a>';
    msgddd=msgddd+' ';
    $.alert(msgddd);
}

/**
 * 错误信息 弹出框
 * @param titleMsg   <h3> 号字体
 * @param content 普通字体
 */
function alertSuccessMsg(titleMsg,content){
    var msgddd=' <div class="thickcon"  >     ';
    msgddd=msgddd+'     <div class="tip-msg">                                        ';
    msgddd=msgddd+'         <div class="tip-succ clearfix">                          ';
    msgddd=msgddd+'             <span class="icon"></span>                           ';
    msgddd=msgddd+'            <div class="font-gray fl">                            ';
    msgddd=msgddd+'                <h3>'+titleMsg+'</h3>                         ';
    msgddd=msgddd+'                <p>'+content+'</p>                         ';
    msgddd=msgddd+'            </div>                                                 ';
    msgddd=msgddd+'       </div>                                                      ';
    msgddd=msgddd+'       <div class="tip-op" style="text-align: center">                                         ';
    msgddd=msgddd+'         <a herf="#" class="btn btn-yellow btn-m thickclose">确定</a>';
    msgddd=msgddd+'      </div>                          ';
    msgddd=msgddd+'     </div>                          ';
    msgddd=msgddd+'</div>                               ';
    msgddd=msgddd+' <a href="#" class="thickclose" >×</a>';
    msgddd=msgddd+' ';
    $.alert(msgddd);
}

/***
 * 
 * 金额格式化脚本，保留小数后两位（四舍五入）
 */
function formatAmount(num) {
	/*if(null != num)
		num = num.toString().replace(/\$|\,/g,'');
    if(isNaN(num))
    	num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num*100+0.50000000001);
    cents = num%100;
    num = Math.floor(num/100).toString();
    if(cents<10)
    	cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
    	num = num.substring(0,num.length-(4*i+3))+','+
    num.substring(num.length-(4*i+3));
    return (((sign)?'':'-') + num + '.' + cents);*/
    if(null != num)
        num = num.toString().replace(/\$|\,/g,'');
    else
        num = "0";
    if(isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num*100+0.00000001);
    cents = num%100;
    num = Math.floor(num/100).toString();
    if(cents<10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
        num = num.substring(0,num.length-(4*i+3))+','+
            num.substring(num.length-(4*i+3));
    return (((sign)?'':'-') + num + '.' + cents);
};

/***
 * 
 * 金额格式化脚本，保留小数后两位（四舍五入）
 */
function formatOverViewAmount(num) {
	/*if(null == num) return "-.-";
	if(null != num)
		num = num.toString().replace(/\$|\,/g,'');
	if(isNaN(num))
		num = "0";
	sign = (num == (num = Math.abs(num)));
	num = Math.floor(num*100+0.50000000001);
	cents = num%100;
	num = Math.floor(num/100).toString();
	if(cents<10)
		cents = "0" + cents;
	for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
		num = num.substring(0,num.length-(4*i+3))+','+
		num.substring(num.length-(4*i+3));
	return (((sign)?'':'-') + num + '.' + cents);*/
    if(null != num)
        num = num.toString().replace(/\$|\,/g,'');
    else
        num = "0";
    if(isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num*100+0.00000001);
    cents = num%100;
    num = Math.floor(num/100).toString();
    if(cents<10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
        num = num.substring(0,num.length-(4*i+3))+','+
            num.substring(num.length-(4*i+3));
    return (((sign)?'':'-') + num + '.' + cents);
};

/***
 * 
 * 金额格式化脚本，保留小数后两位（舍弃）
 */
function formatAmountDown(num) {
	if(null != num)
		num = num.toString().replace(/\$|\,/g,'');
	else
		num = "0";
	if(isNaN(num))
		num = "0";
	sign = (num == (num = Math.abs(num)));
	num = Math.floor(num*100+0.00000001);
	cents = num%100;
	num = Math.floor(num/100).toString();
	if(cents<10)
		cents = "0" + cents;
	for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
		num = num.substring(0,num.length-(4*i+3))+','+
		num.substring(num.length-(4*i+3));
	return (((sign)?'':'-') + num + '.' + cents);
};

/***
 * 
 * 黄金格式化脚本，保留小数后两位（舍弃）
 */
function formatGoldDown(num) {
	if(null != num)
		num = num.toString().replace(/\$|\,/g,'');
	else
		num = "0";
	if(isNaN(num))
		num = "0";
	sign = (num == (num = Math.abs(num)));
	num = Math.floor(num*10000+0.00000001);
	cents = num%10000;
	num = Math.floor(num/10000).toString();
	if(cents<10)
		cents = "000" + cents;
	else if(cents<100)
		cents = "00" + cents;
	else if(cents<1000)
		cents = "0" + cents;
	for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
		num = num.substring(0,num.length-(4*i+3))+','+
		num.substring(num.length-(4*i+3));
	return (((sign)?'':'-') + num + '.' + cents);
};

/**
 * 正则验证手机号码
 * */
function checkMobile(mobile){
	var m = /^1[3|4|5|8][0-9]\d{4,8}$/;
	if(!m.test(mobile)){
		return false;
	}
	return true;
};

/**
 * 正则验证email格式
 * */
function checkEmail(mail){
	var em=/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	if(!em.test(mail)){
		return false;
	}
	return true;
};



//控制textarea输入时字数的限制
function strLenCalc(obj, checklen, maxlen, buttonId) {
	var len = obj.val().length;
	if(len <= maxlen) {
		jQuery("#"+checklen).html("还可输入 <em style='position:relative;top:-5px;width:200px;font-family:Georgia,Tahoma,Arial;font-size:24px;vertical-align:middle;color:#777777;'>" + (maxlen-len) + "/"+maxlen+"</em> 个字"); 
		jQuery("#"+buttonId).removeAttr("disabled"); 
	} else { 
		jQuery("#"+checklen).html("超过 <em style='position:relative;top:-5px;width:200px;font-family:Georgia,Tahoma,Arial;font-size:24px;vertical-align:middle;color:#FF0000;'>" + (maxlen-len)*-1 + "/"+maxlen+"</em> 个字"); 
		jQuery("#"+buttonId).attr("disabled", "disabled"); 
	} 
}

function formatDate(number,format){
    if(number === '' && number === null){
        return '';
    }
    else{
        var date = new Date(number);
        return date.format(format);
    }
}

Date.prototype.format = function(fmt)
{ //author: meizz
    var o = {
        "M+" : this.getMonth()+1,                 //月份
        "d+" : this.getDate(),                    //日
        "h+" : this.getHours(),                   //小时
        "m+" : this.getMinutes(),                 //分
        "s+" : this.getSeconds(),                 //秒
        "q+" : Math.floor((this.getMonth()+3)/3), //季度
        "S"  : this.getMilliseconds()             //毫秒
    };
    if(/(y+)/.test(fmt))
        fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
    for(var k in o)
        if(new RegExp("("+ k +")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
    return fmt;
}

function fixSpec(amount) {
    if (null == amount) return "--.--";
    else {
        return formatAmountDown(amount);
    }
}

function goldFixSpec(amount) {
    if (null == amount) return "--.--";
    else {
        return formatGoldDown(amount);
    }
}

function fixData(amount) {
    if (null == amount) return "0.00";
    else return formatAmountDown(amount);
}

function fixIncome(amount) {
    if (null == amount) return "--.--";
    else {
        if (amount > 0) return "+" + formatAmountDown(amount);
        else return formatAmountDown(amount);
    }
}

function goldFixIncome(amount) {
    if (null == amount) return "--.--";
    else {
        if (amount > 0) return "+" + formatGoldDown(amount);
        else return formatGoldDown(amount);
    }
}

function formatnumber(value, num) {
    var a, b, c, i;
    a = value.toString();
    b = a.indexOf(".");
    c = a.length;
    if (num == 0) {
        if (b != -1) {
            a = a.substring(0, b);
        }
    } else {
        if (b == -1) {
            a = a + ".";
            for (i = 1; i <= num; i++) {
                a = a + "0";
            }
        } else {
            a = a.substring(0, b + num + 1);
            for (i = c; i <= b + num; i++) {
                a = a + "0";
            }
        }
    }
    return a;
}
function GetDateStr(AddDayCount) {
    var dd = new Date();
    dd.setDate(dd.getDate()+AddDayCount);//获取AddDayCount天后的日期
    var y = dd.getFullYear();
    var m = dd.getMonth()+1;//获取当前月份的日期
    var d = dd.getDate();
    if(d<10)
        d = '0'+d;
    if(m<10)
        m = '0'+m;
    return m+"月"+d+'日';
}

String.prototype.endWith=function(endStr){
    var d=this.length-endStr.length;
    return (d>=0&&this.lastIndexOf(endStr)==d)
}

