// JavaScript Document
define(function(require, exports, module) {
    exports.borrow_content = function(){
        //登录弹窗 wqs 12-12
        $('#tender_login').on('click',function(){
            diyou.use('dialogs',function(dia){dia.dialog('用户登录','/?user&q=login&type=ajax')});
            return false;
        })
        //倒计时 wqs 12-12
        exports.cutnd_down_time();
		//金额输入框清空
		
        require('validate');
		var availMoney=parseFloat($("#availMoney").attr("data-money"));
		var minMoney=parseFloat($("#MaxMin").attr("data-min"));
        var maxMoney=parseFloat($("#MaxMin").attr("data-max"));
        var maxValue;
        if(!maxMoney){
           maxValue=availMoney; 
        }else{
           maxValue=(availMoney<maxMoney)?availMoney:maxMoney;
        }
        
		$('#tender_form').validate({
			    onfocusout:function(element){$(element).valid()},
			    onkeyup:function(element){$(element).valid()},
				errorElement:'span',
                errorPlacement:function(error,element){error.appendTo(element.parent())},
        		rules: {
					money: {
						digits: true,
						max:maxValue,
						min:minMoney
        				
        			}
        		},
        		messages: {
        			money: {
						digits:"格式错误，请输入整数",
						max:'输入金额不能大于可投金额或最高投标金额',
						min:'输入金额不能小于最低投标金额'
                        
        			}
        		},submitHandler:function(form) {
					exports.identy();
        			return false;
                    }
    	    });

		
		
    }
 exports.identy=function(){
    if(user_id==""){//登录弹窗 wqs 12-12
              diyou.use('dialogs',function(dia){dia.dialog('请先登录','/?user&q=login&type=ajax')});
              return false;
            }
            var acc = $("#money").val();
            if(acc=="亲，您余额不足。请先充值"){
                acc='';
            }
            if(borrow_self==1){
                diyou.use('dialogs',function(dy){dy.error('不能投自己的标','')})
            }else{
                //email
                if(email_status!=1){
                    diyou.use('dialogs',function(dia){dia.dialog('邮箱认证','/?user&m=approve/safe&type=email','',this.location.href)});
                    return false;
                }
                //realname
                /*if(realname_status==-1){
                    diyou.use('dialogs',function(dia){dia.dialog('实名信息提交','/?user&m=approve/safe&type=t_realname')});
                    return false;
                }*/
                //phone
                if(phone_status!=1){
                    diyou.use('dialogs',function(dia){dia.dialog('绑定手机','/?user&m=approve/safe&type=phone')});
                    return false;
                }
                
             if (paypwd_status!=1){
                    diyou.use('dialogs',function(dy){dy.dialog('请先设置支付密码','/?user&m=approve/safe&type=paypwd_new')});
                    return false;
             }else if(borrow_type!='roam'){
                    diyou.use('dialogs',function(dy){dy.dialog('马上投标','/?user&m=borrow/tender/new&borrow_nid='+borrow_nid+'&account='+acc)});
                    return false;
                }else{
                    diyou.use('dialogs',function(dy){dy.dialog('我要投资','/?user&m=borrow/tender/roam_new&borrow_nid='+borrow_nid+'&account='+acc)});
                    return false;
                }
            }
 }   
	//add nums
	exports.add_nums = function(increase,decrease,roam_account,account_min,tender_all){
	   $("#tender_income").toggle(function(){
          $(".window_by_income_show").css({'display':'block'});
          $(this).html("缩起收款明细");
          },
          function(){
          $(".window_by_income_show").css({'display':'none'});
          $(this).html("展开收款明细");
          })
	   $('#'+increase).bind('click',function(){
	        if(user_id==""){//wqs 12-12
	            diyou.use('dialogs',function(dia){dia.dialog('用户登录','/?user&q=login&type=ajax')});
                return false;
	        }
			var wait=$(this).attr("data-nid");
			var roam = $('#'+roam_account);
			var roam_nums = parseInt(roam.val());
			++roam_nums;
			if(roam_nums<wait)
			roam.val(roam_nums+'份');
			else{roam.val(wait+'份');}
			roam.trigger('input');
		})
		$('#'+decrease).bind('click',function(){
		    if(user_id==""){//wqs 12-12
	            diyou.use('dialogs',function(dia){dia.dialog('用户登录','/?user&q=login&type=ajax')});
                return false;
	        }
			var roam = $('#'+roam_account);
			var roam_nums = parseInt(roam.val());
			--roam_nums;
			if(roam_nums>0)
			roam.val(roam_nums+'份');
			else{roam.val('0份');}
            roam.trigger('input');			
		})
		$('#'+roam_account).bind('input propertychange',function(){
			exports.ramo_money(tender_all,roam_account,account_min);
		})
        exports.ramo_money(tender_all,roam_account,account_min);
        exports.roam_tender_all('tender_all_',tender_all,roam_account,account_min);
        
        //设置交易密码  wqs  12-18
        $('.set_pwd').bind('click',function(){
            diyou.use('dialogs',function(dy){dy.dialog('请先设置支付密码','/?user&m=approve/safe&type=paypwd_new')});
			return false;
        })
	}
    //流转标收益 wqs 2013-12-27
    exports.ramo_money = function(tender_all,roam_account,account_min){
		if(user_id==""){
			return false;}
            var nums = parseInt($("#"+roam_account).val());
		    var _account_min=$('#'+account_min).val();
		    var _money=nums*_account_min;
			if(_money!=''){
				$.ajax({
    				type:"get",
    				url:'/?user&m=borrow/tender',
    				data:'&p=get_income&account='+_money+'&borrow_nid='+borrow_nid,
					cache:false,
    				success:function(data){
						if (data!=""){
						   if(tender_all=='_tender_all'){
						      var obj = eval("("+data+")");
    							$('#income_all').html('￥'+obj.all);
    							$('#interest').html(obj.lixi);
    							$('#award').html(obj.award);
                                var _display = '<ul><li><span>收款时间</span><span>类型</span><span>收款金额</span></li>';
                             $.each(obj.list,function(i,_obj){
    						   _display += ' <li ';
    						   if (i%2==0){
    						   _display += '';
    						   }
    						   if (_obj.type=="lixi"){
    							_title = "利息";
    						   } else if (_obj.type=="award"){
    							_title = "奖励";
    							_obj.date_time = "标审核通过";
    							   }
    						   _display += '><span>'+_obj.date_time+'</span><span>'+_title+'</span><span>￥'+_obj.account+'</span></li>';
    						   }) 
    						 _display += '</ul>';
        					$("#_tender_detail").html(_display);		
						   }else{
						      var obj = eval("("+data+")");
							  $('#'+tender_all).html(obj.all);
						   }	
						}
    				}
    			});
			}else{
				$("#tender_all").html('0.00');		
				$("#income_all").html('0.00');
			}
			//修改 流转标马上认购的份数，输入0份下面显示的预计收益出错

    }
    //流转标弹窗 全部投入
    exports.roam_tender_all = function(id,tender_all,roam_account,account_min){
        $('#'+id).click(function(){
            var _account_min=parseInt($('#account_min').val());
        	var user_balance=parseInt($('#user_balance').val());
            var portion_wait = parseInt($('#portion_wait').val());
        	var tender_account=(_account_min*portion_wait)>user_balance?Math.floor(user_balance/_account_min):portion_wait;
        	$('#'+roam_account).val(tender_account+"份");
            
            exports.ramo_money(tender_all,roam_account,account_min);
        })
        
    }
    //弹窗 流转标 wqs 2013-12-26
    exports.roam_tender = function(tid){
        $('#'+tid).click(function(){
            
            if(user_id==""){
	            diyou.use('dialogs',function(dia){dia.dialog('用户登录','/?user&q=login&type=ajax')});
                return false;
	        }
            if(borrow_self==1){
				diyou.use('dialogs',function(dy){dy.error('不能投自己的标','')})
                return false;
			}
            
             //email
            if(email_status!=1){
				diyou.use('dialogs',function(dia){dia.dialog('邮箱认证','/?user&m=approve/safe&type=email','',this.location.href)});
                return false;
            }
            /*//realname
            if(realname_status==-1){
                diyou.use('dialogs',function(dia){dia.dialog('实名信息提交','/?user&m=approve/safe&type=t_realname')});
                return false;
            }*/
            //phone
            if(phone_status!=1){
                diyou.use('dialogs',function(dia){dia.dialog('绑定手机','/?user&m=approve/safe&type=phone')});
                return false;
            }
            var num = $('#roam_account').val();
            diyou.use('dialogs',function(dy){dy.dialog('我要投资','/?user&m=borrow/tender/roam_new&borrow_nid='+borrow_nid+'&num='+encodeURIComponent(num))});
			return false;            
        })        
    }
    exports.roam_new = function(){
        //开始投资
        $("#tender_new_form").bind("submit", function() {
            var safe_code = $('#safe_code');
            if(safe_code.length >0 && $('#safe_code').val()==''){
                $('#safe_code_error').html('手机宝令不能为空');
                return false;
            }
            require("submitform");
			require("encypt");
			var string=encypt('paypassword');
			$("#paypassword").val(string);
            $("#tender_new_form").ajaxSubmit({
                success: function(result, status) {
                    if (result == 1) {
                        diyou.use('dialogs', function(dy) {
                            dy.success('投资成功', 'this')
                        })
                    } else {
                        diyou.use('dialogs', function(dy) {
                            dy.error(result, '')
                        })
                    }
                    return false;
                }

            });
            return false; // cancel conventional submit*/
        })
    }
    //流转标 弹窗
    exports.num_check = function(){
        $('#_increase').click(function(){
	        if(user_id==""){//wqs 12-12
	            diyou.use('dialogs',function(dia){dia.dialog('用户登录','/?user&q=login&type=ajax')});
                return false;
	        }
			var wait=$(this).attr("data-nid");
			var roam = $('#_roam_account');
			var roam_nums = parseInt(roam.val());
			++roam_nums;
			if(roam_nums<wait)
			roam.val(roam_nums+'份');
			else{roam.val(wait+'份');}
			roam.trigger('input');
		})
    }
    
    //ahui 
    exports.tender_list = function(){
        exports._tender_list(0);
	   $(".ui_filter_tag").click(function(){
           $(this).parent().find('li').removeClass("active");
           $(this).parent().find('input').removeAttr("checked");
           $(this).addClass('active');
           $(this).find('input').attr("checked",'checked');
           exports._tender_list(0);
	   })
       
        $(".main_box_tit li").click(function(){
		
           $(this).parent().find('li').removeClass("on");
           $(this).addClass('on');
           var _order =  $(this).attr('data-order');
           if ($(this).hasClass("up")){
               $(this).parent().find('li').removeClass("up down");
              $(this).removeClass('up');
              $(this).addClass('down');
              _order += '_down';
           }else if ($(this).hasClass("down")){
              $(this).parent().find('li').removeClass("up down");
              $(this).removeClass('down');
              $(this).addClass('up');
              _order += '_up';
            }else{
               $(this).parent().find('li').removeClass("up down");
               $(this).addClass('down');
              _order += '_down';
            }
            if ($(this).attr('data-order')!=''){
                $(".main_box_tit").attr("data",_order);
            }else{
                $(".main_box_tit").attr("data",'');
            }
            exports._tender_list(0);
	   })
    }
    //列表页
    exports._tender_list = function(page){
        var _borrow_style=$('input:hidden[name="borrow_style"]').val();
        var _borrow_type=$('input:radio[name="borrow_type"]:checked').val();
        var _account_status=$('input:radio[name="account_status"]:checked').val();
        var _borrow_interestrate=$('input:radio[name="borrow_interestrate"]:checked').val();
        var _spread_month=$('input:radio[name="spread_month"]:checked').val();
        var _order=$(".main_box_tit").attr("data");
        if (_borrow_style!='change'){
        _data = '&t=tender_borrow_ajax&page='+page+'&order='+_order+'&borrow_type='+_borrow_type+'&account_status='+_account_status+'&borrow_interestrate='+_borrow_interestrate+'&spread_month='+_spread_month;
       }else{
          _data = '&t=tender_change_ajax&page='+page+'&order='+_order+'&borrow_type='+_borrow_type+'&account_status='+_account_status+'&borrow_interestrate='+_borrow_interestrate+'&spread_month='+_spread_month;
       }
        $.ajax({
        url: "/?ajax",
        data: _data,
        success: function(data){
            $(".main_box_cont").html(data);
            //倒计时
             exports.cutnd_down_time();
        }
        });
    }
    
    //列表页
    exports.pages = function(){
        require("pages");
         $("#pages").pagination($("#pages").attr('data-total'), {
            callback: pageselectCallback,//PageCallback() 为翻页调用次函数。
            prev_text: "<",
            next_text: ">",
            items_per_page: parseInt($("#pages").attr('data-epage')), //每页的数据个数
            num_display_entries: 5, //两侧首尾分页条目数
            current_page: parseInt($("#pages").attr('data-page')),   //当前页码
            //current_page:0,
            num_edge_entries: 10,
            load_first_page:false
        });
        function pageselectCallback(page_id, jq) {
            exports._tender_list(page_id);
            return false;
        }
    }
    exports.pages_change = function(){
        require("pages");
         $("#pages").pagination($("#pages").attr('data-total'), {
            callback: pageselectCallback,//PageCallback() 为翻页调用次函数。
            prev_text: "<",
            next_text: ">",
            items_per_page: parseInt($("#pages").attr('data-epage')), //每页的数据个数
            num_display_entries: 5, //两侧首尾分页条目数
            current_page: parseInt($("#pages").attr('data-page')),   //当前页码
            //current_page:0,
            num_edge_entries: 10,
            load_first_page:false
        });
        function pageselectCallback(page_id, jq) {
            exports.change_list(page_id);
            return false;
        }
    }
    
    //倒计时 wqs  详情页引用
    exports.cutnd_down_time = function(){
         addTimer = function () {     
            var list = [],     
                interval;     
        
            return function (id, time) {	
                 if (!interval) 
				 {setTimeout(go,100);
				 interval = setInterval(go, 1000);}   
                list.push({ ele: document.getElementById(id), time: time });     
            }     
        
            function go() {     
                for (var i = 0; i < list.length; i++) {     
                    list[i].ele.innerHTML = getTimerString(list[i].time ? list[i].time -= 1 : 0);     
                    if (!list[i].time)     
                        list.splice(i--, 1);     
                }     
            }     
        
            function getTimerString(time) {   
                if(time<0){
                    return "已过期";
                }
                    d = Math.floor(time / 86400),     
                    h = Math.floor((time % 86400) / 3600),     
                    m = Math.floor(((time % 86400) % 3600) / 60),     
                    s = Math.floor(((time % 86400) % 3600) % 60);     
                if (time>0 && d>0)     
                    return ""+d + "天" + h + "小时" + m + "分" + s + "秒";       
                else return ""+h + "小时" + m + "分" + s + "秒";     
            }     
        } ();
            
            $(".borrow_end_time").each(function(){
                var _time = $(this).attr('data');
                var _id=$(this).attr("id");
                addTimer(_id,_time);
                
             }) ; 
    }
    
    //投标弹窗 wqs 12-13
    exports.tender_new = function(){
       //初始化投资页的时候显示收益
        exports.tender_income($('#tender_money').val(),$('#borrow_nid').val());
        $('#tender_all').click(function(){
            var borrow_account_wait=parseInt($('#borrow_account_wait').val());
        	var user_balance=parseInt($('#user_balance').val());
        	var tender_account=borrow_account_wait>user_balance?user_balance:borrow_account_wait;
        	$('#tender_money').val(tender_account);
            //全部投入的时候显示收益
            exports.tender_income($('#tender_money').val(),$('#borrow_nid').val());
        })
        
        //开始投资
        //开始投资
        $("#tender_new_form").bind("submit",function(){
       	    var acc = $("#tender_money").val();
            if (acc==''){
                alert('投资金额不能为空');
                return false;
            }
       	    var paypassword = $("#paypassword").val();
            if (paypassword==''){
                alert('请输入支付密码');
                return false;
            }
            var safe_code = $('#safe_code');
            if(safe_code.length >0 && $('#safe_code').val()==''){
                $('#safe_code_error').html('手机宝令不能为空');
                return false;
            }
			require("submitform");
			require("encypt");
			var string=encypt('paypassword');
			$("#paypassword").val(string);
			diyou.use('index',function(dy){dy.setBtndisable('tender_new_submit','投标中...','#666666',true)});	
   			$("#tender_new_form").ajaxSubmit({
				 success: function (result, status) {
					
			
					if(result==1){
					   diyou.use('dialogs',function(dy){dy.success('投资成功','this')})
					}else{
					   diyou.use('dialogs',function(dy){dy.error(result,'')})
					}
					 diyou.use('index',function(dy){dy.setBtndisable('tender_new_submit','马上投标','#0697DA',false)});
					return false;
				}

			 });
			 return false; // cancel conventional submit
		 })
		 
    }
    exports.tender_income = function (_money,borrow_nid){
            //var _money = $('#tender_money').val();
            //var borrow_nid=$('#borrow_nid').val();
                var account_wait = parseFloat($('#borrow_account_wait').val());
                if(_money>account_wait){
                    $("#pssword_error").after('<span for="tender_money" class="error">投资金额不能大于可投金额</span>');
                }
    			if(_money ==''){
    			   $('#income_all').html('￥0.00');
                   $('#interest').html('0');
    			   $('#award').html('0');
                   $("#tender_detail").html('');
    			}
    			else{
    				$.ajax({
        				type:"get",
        				url:'/?user&m=borrow/tender',
        				data:'&p=get_income&account='+_money+'&borrow_nid='+borrow_nid,
    					cache:false,
        				success:function(data){
    						if (data!=""){
    							var obj = eval("("+data+")");
    							$('#income_all').html('￥'+obj.all);
    							$('#interest').html(obj.lixi);
    							$('#award').html(obj.award);
                                var _display = '<ul><li><span>收款时间</span><span>类型</span><span>收款金额</span></li>';
                             $.each(obj.list,function(i,_obj){
    						   _display += ' <li ';
    						   if (i%2==0){
    						   _display += '';
    						   }
    						   if (_obj.type=="lixi"){
    							_title = "利息";
    						   } else if (_obj.type=="award"){
    							_title = "奖励";
    							_obj.date_time = "标审核通过";
    							   }
    						   _display += '><span>'+_obj.date_time+'</span><span>'+_title+'</span><span>￥'+_obj.account+'</span></li>';
    						   }) 
    						 _display += '</ul>';
        					$("#tender_detail").html(_display);							
    						}
        				}
        			});
    			}
    }
    //get income hsd
	exports.get_income = function(){
		require('validate');
		var account_wait = parseFloat($('#borrow_account_wait').val());
		$('#tender_new_form').validate({
			    onkeyup:function(element) {$(element).valid()},
                onfocusout:function(element) {
					$(element).valid();
					 if(!$(element).valid()){
			diyou.use('index',function(dy){dy.setBtndisable('tender_new_submit','马上投标','#666666',true)});	
			}else{
				diyou.use('index',function(dy){dy.setBtndisable('tender_new_submit','马上投标','#0697DA',false)});	
				var _money = $('#tender_money').val();
					//var borrow_nid=$('#borrow_nid').val();
					$.ajax({
						type:"get",
						url:'/?user&m=borrow/tender',
						data:'&p=get_income&account='+_money+'&borrow_nid='+borrow_nid,
						cache:false,
						success:function(data){
							if (data!=""){
								var obj = eval("("+data+")");
								var abc = obj.all;
								if(abc==undefined){
									abc = '0.00';
								}
								$('#income_all').html('￥'+abc);
								$('#interest').html(obj.lixi);
								$('#award').html(obj.award);
								var _display = '<ul><li><span>收款时间</span><span>类型</span><span>收款金额</span></li>';
							 $.each(obj.list,function(i,_obj){
							   _display += ' <li ';
							   if (i%2==0){
							   _display += '';
							   }
							   if (_obj.type=="lixi"){
								_title = "利息";
							   } else if (_obj.type=="award"){
								_title = "奖励";
								_obj.date_time = "标审核通过";
								   }
							   _display += '><span>'+_obj.date_time+'</span><span>'+_title+'</span><span>￥'+_obj.account+'</span></li>';
							   }) 
							 _display += '</ul>';
							$("#tender_detail").html(_display);							
							}
						}
					});
				}    
					
				},
			    errorElement:'span',
                errorPlacement:function(error,element){error.appendTo(element.parent())},
        		rules: {
					account: {
						required:true,
        				digits: true,
						max:account_wait
        			}
        		},
        		messages: {
        			account: {
                        required:'投资金额不能为空',
						digits:"格式错误，投资金额只能输入整数",
						max:'投资金额不能大于可投金额'
        			}
        		}
    	    });
	   
  
  $("#tender_income").toggle(function(){
  $(".window_by_income_show").css({'display':'block'});
  $(this).html("缩起收款明细");
  },
  function(){
  $(".window_by_income_show").css({'display':'none'});
  $(this).html("展开收款明细");
  })
       $(document).ready(function(){ $('#tender_money').trigger('input');})
   }
   //实名认证弹窗 投资流程 wqs 12-30
   exports.t_realname = function(form_id){
        //获取验证码        
         $('#get_phone_code').click(function(){
            $('#phone_warn').html('');
            var phone_reg = /^1[3|4|5|7|8][0-9]\d{8,8}$/;
            if ($("#phone").val()==''){
                $("#phone_warn").html('手机号不能为空');
            }else if(!phone_reg.test($("#phone").val())){
             $("#phone_warn").html('手机号格式不正确');
            } else{
               $.ajax({
                　　type:'post',            
                　　url:'/?user&m=approve/phone&action=approve_code&type=realname_approve',  
                    data:'phone='+$('#phone').val(), 
                　　dataType:'text', 
                　　success:function(msg){
                        var results = eval('('+msg+')');
                        if(results.code == 'success'){
                            //$('#get_phone_code').attr('value','发送成功');
                            diyou.use('index',function(dy){dy._get_vericode('get_phone_code','valicode')});
                        }else{
                            $("#phone_warn").html(results.msg);
                        }
                　　},            
                　　error:function(){
                        diyou.use('dialogs',function(dia){dia.error('对不起发生错误了！','')});
                    }
                })
               }
        });
        
        //提交 验证
        require('validate');           
        $("#"+form_id).validate({
            errorPlacement:function(error,element){error.appendTo(element.parent())},
            onfocusout:function(element) {$(element).valid()},
            onkeyup:function(element) {$(element).valid()},
            errorElement:'span',
    		rules: {
    			card_id: {required: true,idcard: true},
    			realname: {required: true,zh_ch: true},
                phone:{required: true,phone: true},
                valicode:"required"    		
    		},
    		messages: {
    			card_id: {required: "请输入身份证号",idcard: "请输入正确的身份证号"},
    			realname: {required: "请输入真实姓名",zh_ch: "请输入正确的姓名"},
                phone:{required: "请输入手机号",phone: "请输入正确的手机号"},
                valicode:"请输入验证码" 
    		}
	    });
        $('#'+form_id).bind('submit',function(){
            $('#phone_warn').html('');
            require('submitform');
            $("#"+form_id).ajaxSubmit({
                  url:"/?user&m=approve/t_realname",
    			  success:function(result,status){
    			    var results = eval('('+result+')');
    				if(results.code == 'success')
    				{
                       diyou.use('dialogs',function(dia){dia.success(results.msg,'this')});                  
                    
                    }else{
    				   $('#phone_warn').html(results.msg);
                    }
                  }
			});
            return false;
        });
   }
   
   //进度条显示效果
    exports.pro_show = function(){
		var all_pro = $('.j_pro');
		var all_pro_nums = $('.j_pro').length;
		var pro_nid=[];
		for(var i=0;i<all_pro_nums;i++){
			pro_nid[i]= all_pro.eq(i).children('.bar_bg_c2').attr('data-nid');
			if(pro_nid[i]!='100.00%'){
				all_pro.eq(i).children('.bar_bg_c2').animate({'width':pro_nid[i]},800);
			}
			else{
				all_pro.eq(i).children('.bar_bg_c2').css({'width':pro_nid[i]});
			}
			
		}
    }
	
	
exports.comment_form=function(form_id){
	$('#'+form_id).bind('submit',function(){
		var con=$("#contents").val();
	var valicode=$("#valicode").val();
	if (con==""){
        diyou.use('dialogs',function(dy){dy.error('留言内容不能为空！','')});
		return false;
	}
    var con_len=$("#contents").val().length;
    if(con_len>120){
        diyou.use('dialogs',function(dy){dy.error('留言内容不能超过120个字符！','')});
        return false;
    }

    if (valicode==""){
        diyou.use('dialogs',function(dy){dy.error('验证码不能为空','')});
		return false;
	}
	$.post("/?user&m=borrow/comments&type=new",{contents:con,code:"borrow",article_id:article,borrow_user:borrow_user,borrow_nid:borrow_nid,valicode:valicode},function(result){
		if (result=='1'){
		  diyou.use('dialogs',function(dy){dy.success('评论成功','this')});
        }else if(result=='2'){
            diyou.use('dialogs',function(dy){dy.error('评论失败','')});
		}else{
		  diyou.use('dialogs',function(dy){dy.error('验证码错误','')});
        }
		
	});
		return false;
		})
	
	}	
exports.comment_list=function(article){
	if(user_id!=""){
		diyou.use("pagelist",function(p){p.pages('#content_list','/?ajax&t=comments&type=list&id='+article)});
		}	
	}   
   
})