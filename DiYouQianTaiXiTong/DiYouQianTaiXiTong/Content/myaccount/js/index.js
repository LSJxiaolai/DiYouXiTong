/* @update: 2017-5-8 18:14:14 */
//"use strict";var assetsIndex;assetsIndex={},assetsIndex.richEye=function(){return $("#J_r-eye").click(function(){return $(this).hasClass("r-eye")?($(this).addClass("r-eye--none").removeClass("r-eye"),$(".j_eye-hide").hide(),$(".j_eye-line").attr("eye-line",function(){return $(this).html()}),$(".j_eye-line").html("---"),$(".j_eye-paid").attr("eye-paid",function(){return $(this).children("em").html()>0?$(this).children("em").html():void 0}).html(function(){return $(this).children("em").html()>0?"\u8fd1\u4e03\u65e5\u6709\u5f85\u4ed8\u6b3e":void 0})):($(this).addClass("r-eye").removeClass("r-eye--none"),$(".j_eye-hide").show(),$(".j_eye-line").html(function(){return $(this).attr("eye-line")}),$(".j_eye-paid").html(function(){return $(this).attr("eye-paid")?'\u4e03\u65e5\u5f85\u4ed8 <em class="font-grey3">'+$(this).attr("eye-paid")+"</em> \u5143":void 0}))})},assetsIndex.richEyeHide=function(){return $("#J_r-eye").addClass("r-eye--none").removeClass("r-eye"),$(".j_eye-hide").hide(),$(".j_eye-line").attr("eye-line",function(){return $(this).html()}),$(".j_eye-line").html("---"),$(".j_eye-paid").attr("eye-paid",function(){return $(this).children("em").html()>0?$(this).children("em").html():void 0}).html(function(){return $(this).children("em").html()>0?"\u8fd1\u4e03\u65e5\u6709\u5f85\u4ed8\u6b3e":void 0})},assetsIndex.richPop=function(){var e;return e=null,$(".mya-rich").on({mouseenter:function(){return clearTimeout(e),$(this).find(".r-b-c-pop").css("top",function(){return 20-$(this).innerHeight()/2}),$(".r-b-circle-box").removeClass("r-b-circle-box--hover"),$(this).addClass("r-b-circle-box--hover")},mouseleave:function(){return e=setTimeout(function(e){return function(){return $(e).removeClass("r-b-circle-box--hover")}}(this),200)}},".r-b-circle-box")},assetsIndex.richQrShare=function(e){var t,r,n;return t=$("#J_r-h-t-q-b-qrCode"),t.length&&!t.children().length?(r="http://m.jr.jd.com/spe/userIncome_0420/html/share.html?pin=",n=r+e,seajs.use("//static.360buyimg.com/finance/assets/common/moudle/js/jrQrcode",function(){return t.qrcode({width:120,height:120,text:n})})):void 0},assetsIndex.richQr=function(e){var t;return t=null,$(".mya-rich").on({mouseenter:function(){return $("#J_r-eye").hasClass("r-eye--none")?void 0:(clearTimeout(t),$(this).addClass("r-h-t-qrBtn--hover"),assetsIndex.richQrShare(e))},mouseleave:function(){return $("#J_r-eye").hasClass("r-eye--none")?void 0:t=setTimeout(function(e){return function(){return $(e).removeClass("r-h-t-qrBtn--hover")}}(this),200)}},".r-h-t-qrBtn")},assetsIndex.richIcoPop=function(){var e;return e=null,$(".mya-rich").on({mouseenter:function(){return clearTimeout(e),$(this).find(".r-b-p-box").css("left",function(){return 6-$(this).innerWidth()/2}),$(".r-b-pop").removeClass("r-b-pop--hover"),$(this).addClass("r-b-pop--hover")},mouseleave:function(){return e=setTimeout(function(e){return function(){return $(e).removeClass("r-b-pop--hover")}}(this),200)}},".r-b-pop")},assetsIndex.richTip=function(){return $(".mya-rich").find(".r-h-tip").click(function(){return $(this).remove()})},assetsIndex.profitPop=function(){var e;return e=null,$(".mya-profit").on({mouseenter:function(){return clearTimeout(e),$(this).find(".p-r-h-p-box").css("left",function(){return 6-$(this).innerWidth()/2}),$(".p-r-h-pop").removeClass("p-r-h-pop--hover"),$(this).addClass("p-r-h-pop--hover")},mouseleave:function(){return e=setTimeout(function(e){return function(){return $(e).removeClass("p-r-h-pop--hover")}}(this),200)}},".p-r-h-pop")},assetsIndex.selectTab=function(){var e;return e=$(".ui-select-listBox").find(".ui-select-listBox-list--now").index(),$(".mya-select .s-table-bg").hide().eq(e).show(),assetsCommon.ui_selectListBox(function(e){return $(".mya-select .s-table-bg").hide().eq(e).show()},200)},assetsIndex.chart=function(e){return seajs.use("//static.360buyimg.com/finance/assets/index/2.0.0/js/indexChart",function(t){return new t({renderWrap:"#J_indexChart",width:910,height:60,data:e})})},assetsIndex.pk=function(){var e;return e=jrBase.parseParameter(window.location.href),"1"===e.showpk&&assetsCommon.pk(),$("#J_p-r-pkBtn").click(function(){return assetsCommon.pk()})},assetsIndex.init=function(){return this.richEye(),this.richPop(),this.richIcoPop(),this.profitPop(),this.pk(),this.selectTab()},$(function(){return assetsIndex.init()});