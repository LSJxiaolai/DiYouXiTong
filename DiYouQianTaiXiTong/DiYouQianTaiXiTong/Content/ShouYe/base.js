var themes_dir = '/dyweb/dythemes/diyou/js/';
diyou.config({
  alias: {
	'submitform':themes_dir+'jquery.form',
	'automail':themes_dir+'jquery.automail',
	'index':themes_dir+'index',
	'dialogs':themes_dir+'dialogs',
    'borrow':themes_dir+'borrow',
    'tender':themes_dir+'tender',
    'pages':themes_dir+'jquery.pagination',
    'pagelist':themes_dir+'pagelist',
    'users':themes_dir+'users',
	'account':themes_dir+'account',
    'rating':themes_dir+'rating',
    'users_safe':themes_dir+'users_safe',
	'users_api':themes_dir+'users_api',
    'users_borrow':themes_dir+'users_borrow',
    'users_account':themes_dir+'users_account',
    'users_invest':themes_dir+'users_invest',
    'users_amount':themes_dir+'users_amount',
	'users_change':themes_dir+'users_change',
	'users_spread':themes_dir+'users_spread',    
    'users_reg':themes_dir+'users_reg',//注册
	'users_login':themes_dir+'users_login',//登录
	'users_reg_email':themes_dir+'users_reg_email',
	'users_check':themes_dir+'users_check',
    'datepicker':themes_dir+'datepicker/WdatePicker',
    'fancybox':themes_dir+'jquery.fancybox', //fancybox 图片浏览
	'validate':themes_dir+'jquery.validate', //表单验证
	'snow':themes_dir+'snow.min', //圣诞节雪花飘插件
    'tool':themes_dir+'tool',
    'footer':themes_dir+'footer',
	'users_credit':themes_dir+'users_credit',
    'help':themes_dir+'help',//帮助中心分页
	'KeleyiClipboard':themes_dir+'KeleyiClipboard',
	'cookie':themes_dir+'jquery.cookie',//cookie插件
	'mall':themes_dir+'mall',
	'users_loan':themes_dir+'users_loan',
	'encypt':themes_dir+'encypt'
	  }
});
define(function(require, exports, module) {
    require('dialogs');
});
