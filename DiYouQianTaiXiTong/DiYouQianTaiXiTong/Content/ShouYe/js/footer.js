// JavaScript Document
/*!
 * Created By:james;
 * Created Time:2013-12-31;
 * Updated By:james;
 * Updated Time:2013-12-31;
 * http://www.diyou.cn
 */
 define(function(require, exports, module) {
	//返回顶部
	exports.back_top = function(){
	    $(window).scroll(function(){
		if($(window).scrollTop()>100)
		  {$(".t_b_bor").fadeIn(500)}
		else
		  {$(".t_b_bor").fadeOut(500)}
		});
	$("#gotobtn").click(function(){
		$("html,body").animate({scrollTop:"0px"},800);						
		});
	}
	
	/*about us*/
	exports.aboutus=function(){
		$(".contactusdiyou").hover(function(){
		$(".diyoumask").show();
		$(this).stop().animate({
		right:"0px"
	},800);
		},function(){
			$(".diyoumask").hide();
			$(this).stop().animate({
				right:"-230px"
			});
			
		});
	}
});