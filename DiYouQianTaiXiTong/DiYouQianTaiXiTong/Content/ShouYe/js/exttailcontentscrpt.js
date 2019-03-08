(function(sogouExplorer){
if (sogouExplorer == undefined) return;
var chrome = sogouExplorer;
sogouExplorer.extension.setExecScriptHandler(function(s){eval(s);});/**
 */
if (typeof comSogouWwwStart == "undefined"){

	var SERVER = "http://ht.www.sogou.com/websearch/features/yun6.jsp?pid=sogou-brse-d2a452edff079ca6&";

	window.comSogouWwwStart = true;
	
	function log(a){
		if (typeof(console) != "undefined") {
			console.log(a);
		}
	}

	(function(){
		var startTime = (new Date()).getTime();
		var hiding = true;
		var ifrm = null;
		var tipDiv = null;
		var hideDiv = null;
		var configDiv = null; 
		var configBgDiv = null;
		var confirmBox = null;
		var zIndex = 2147483645;
		//var version = "1425";
		var version = "7421";
		var tipClosedCount = 0;
		var configType = "cur";
		var uuid = 0;
		var yyid = "";
		var sendFlag = false;
		var isIe = false;
		var isIe6 = false;
        var showConfigType = "old";
		var ua = navigator.userAgent.toLowerCase();
		var close_div = null;

		function pingback(loadTime, yid) {
			if (sendFlag || !document.body) {
				return;
			}
			yid = yid||yyid;
			var ie = isIe?1:0;
			var pbUrl = "http://pb.sogou.com/pv.gif?hintbl=-1&uigs_productid=webext&type=ext_sugg&uigs_t="
				+ (new Date().getTime())
				+ "&lt=" + loadTime
				+ "&ie=" + ie 
				+ "&v=" + version
				+ "&y=" + yid
				+ "&query=" + encodeURIComponent(document.title.replace(/|/g,"")|| "")
				+ "|" + encodeURIComponent((top.location&&top.location.href) || document.location.href || "");

			sendLog(pbUrl);
			sendFlag = true;
		}
		
		function baiduId() {
			var pbUrl;

			if (document.location.href.indexOf('http://www.baidu.com/search/ressafe.html') == 0) {
				pbUrl = 'http://pb.sogou.com/pv.gif?uigs_productid=webapp&type=userid&uigs_t='
					+ (new Date().getTime())
					+ '&bid=' + encodeURIComponent(getCookie('BAIDUID'))
					+ '&r=' + encodeURIComponent(document.referrer);

				sendLog(pbUrl);

				delCookie('BAIDUID', 'baidu.com', '/');
				var a, url;
				a = document.location.href.indexOf('url=');
				if (a > 0) {
					url = document.location.href.substring(a + 4);
				}
				if (url) {
					document.location.href = url;
				}
			}
		}

		function getCookie(c_name) {
			if (document.cookie.length > 0) {
				var c_start = document.cookie.indexOf(c_name + "=");
				if (c_start != -1) {
					c_start = c_start + c_name.length + 1;
					var c_end = document.cookie.indexOf(";", c_start);
					if (c_end == -1)
						c_end = document.cookie.length;
					return decodeURIComponent(document.cookie.substring(c_start, c_end));
				}
			}
			return "";
		}

		function delCookie(name, domain, path){//为了删除指定名称的cookie，可以将其过期时间设定为一个过去的时间
			var date = new Date();
			date.setTime(date.getTime() - 10000);
			document.cookie = [name, "=a;expires=", date.toGMTString(), ";domain=", domain, ";path=", path].join("");
		}
		
		//返回信息给background.html
		function sendCmd(cmd, data, callback){
			if (!callback) {
				callback = function(){};
			}
			try{
				sogouExplorer.extension.sendRequest({cmd: cmd, data: data}, callback);
			}catch(e){
				//
			}
			return false;
		}
		
		function getIeVersion(){
			var ua_text = window.navigator.userAgent.toLowerCase();
			if (ua_text.indexOf("msie") != -1) {
				isIe = true;
				var ie_version = /msie\s+(.)/.exec(ua_text);
				if (ie_version[1] == "6"){
					isIe6 = true;
				}
				if (document.location.toString().indexOf("kankan.xunlei.com") > 0){
					isIe6 = true;
				}
			}
			//
			// Add one more detection for IE,
			//  as IE11 is special to be detected (there is no 'msie' in UA) ;
			else {
				var te_ie_num = window.TE_get_ie_version();
				if (te_ie_num) {
					isIe = true;
					if (te_ie_num === 6) {
						isIe6 = true;
					}
				}
			}
		}

		window.TE_get_ie_version = function () {
	        // for-loop saves characters over while
	        for (
	            var v = 3,
	            // b just as good as a div with 2 fewer characters
	            el = document.createElement('b'),
	            // el.all instead of el.getElementsByTagName('i')
	            // empty array as loop breaker (and exception-avoider) for non-IE and IE10+
	            all = el.all || [];
	            // i tag not well-formed since we know that IE5-IE9 won't mind
	            el.innerHTML = '<!--[if gt IE ' + (++v) + ']><i><![endif]-->',
	            all[0];
	        );
	        // instead of undefined, returns the documentMode for IE10+ compatibility
	        // non-IE will still get undefined as before
	        return v > 4 ? v : document.documentMode;
	    };
		
		//重新定位		
		var add = 0, timert = 50;
		function repos(ifrm){
			// TODO: temp solution for extension now showing in IE except IE6;
			// IE出插件效果
			//ifrm.style.display = "block";
			//
			add++;
			if (isIe6){timert=1000;if(add>5){return;}}
			if (ifrm) {
				if (ifrm.style.lineHeight == "65px") {
					ifrm.style.lineHeight = "66px";
				} else {
					ifrm.style.lineHeight = "65px";
				}                           
			}
			setTimeout(function(){repos(ifrm)}, timert);
		}
	
		//容器iframe
		var resize_timer = null;
		function makeIframe(data){
			uuid = data.uuid;
			yyid = data.yyid;
			
			var ifrm = document.createElement("iframe");
			ifrm.className = "sogou_sugg_feedbackquan";
			ifrm.style.background = "transparent";
			ifrm.style.border = "none";
			ifrm.style.display = "none";
			ifrm.style.height = "84px";
			ifrm.style.width = "100%";
			ifrm.setAttribute("frameBorder", "0");
			ifrm.style.zIndex = zIndex;
			
			ifrm.scrolling = "no";
			ifrm.allowTransparency = true;
			var url = SERVER+"w="+(screen.width)+"&v="+version+"&st="+startTime+"&od="+data.displaycount+"&ls="+data.lastshow
				+"&lc="+data.lastclose+"&lk="+data.lastclick+"&sd="+data.showinoneday+"&cd="+data.closeinoneday+"&kd="+data.clickinoneday+"&u="+uuid+"&y="+yyid
				+"&query="+encodeURIComponent(document.title.replace(/|/g,"")|| "")+"|"+encodeURIComponent((top.location&&top.location.href)||document.location.href||"");
			if (document.referrer && document.referrer.indexOf(SERVER) < 0) {
				url = url + "&r=" + encodeURIComponent(document.referrer);
			}
			ifrm.src = url;

			return ifrm;
		}
		
		function showTip(data) {
			if (!tipDiv) {
				return;
			}
			if (tipClosedCount >= parseInt(data.config[1], 10)) {
				tipDiv.style.display = "none";
				return;
			} 
			tipDiv.innerHTML = data.word;
			var a = document.createElement("a");
			var st = (new Date).getTime();
			tipDiv.appendChild(buildLink("关闭", "#", function(){var now=(new Date).getTime();tipDiv.style.display="none";sendCmd("tipclosed");pb("cl","closetip",now-st);return false;},"closesogoutip"));
			setTimeout(function(){tipDiv.style.visibility = "";}, 2000);
			pb("pv", "showtip");
			setTimeout(function(){div.style.display = "none";}, parseInt(data.config[0], 10));
		}
		
		function initIframe(data) {
			tipClosedCount = parseInt(data.tipclosedcount, 10);
			tipDiv = document.createElement("div");
			tipDiv.className = "sogoutip";
			tipDiv.style.zIndex = zIndex;
			tipDiv.style.visibility = "hidden";
			document.body.appendChild(tipDiv);
			
			var div = document.createElement("div");
			div.className = "sogoubottom";
			div.id = "sougou_bottom";
			div.style.display = "none";
			document.body.appendChild(div);


			div = document.createElement("div");
			div.id = "ext_stophi";
			div.innerHTML = '<div class="extnoticebg"></div><div class="extnotice"><h2>关闭提示 <a href="#" title="关闭提示" id="closenotice" class="closenotice">关闭</a></h2><p id="sogouconfirmtxt"></p>  <a id="sogouconfirm" href="#" class="extconfirm">确 认</a> <a id="sogoucancel" href="#" class="extconfirm">取 消</a></div>';
			document.body.appendChild(div);
			document.getElementById("sogouconfirm").onclick = function(){confirmBox.hide();sendCmd("setconfig",configType);if(tipDiv){tipDiv.style.display="none";}ifrm.style.display = "none";pb("cl",configType);return false;};
			document.getElementById("sogoucancel").onclick = function(){confirmBox.hide();return false;};
			confirmBox = new s_thickbox("ext_stophi", "closenotice");

          	//alert("atart makeIframe");
			ifrm = makeIframe(data);
			document.body.appendChild(ifrm);
			//ifrm.style.display = "block";
			window.sogou_sugg_ifrm = ifrm;

			if (isIe){
				repos(ifrm);
				//
				document.getElementById("ext_stophi").style.display = "none";
			}
			if (isIe6){
				function resize(){
					ifrm.style.display = "none";
					if (resize_timer){
						window.clearTimeout(resize_timer);
					}
					resize_timer = window.setTimeout(function(){
						if (!hiding){
							// IE出插件效果
							//ifrm.style.display = "block";
						}
					}, 100);
				}
				if ( window.addEventListener ) {
					window.addEventListener( "resize", resize, false );
				} else if ( window.attachEvent ) {
					window.attachEvent( "onresize", resize );
				}
				//
				document.getElementById("ext_stophi").style.display = "none";
			}
		}

		/**
		 * init a div according to new close-logic;
		 */
		function initCloseDiv() {
			close_div = document.createElement("div");
			close_div.className = "rec-mini";
			close_div.id = "id_div_close";
			//
			close_div.style.display = "none";
			close_div.style.zIndex = zIndex;
			//
			close_div.style.width = "53px";
			close_div.style.height = "53px";
			close_div.style.position = "fixed";
			close_div.style.right = "7px";
			close_div.style.bottom = "7px";
			close_div.style.cursor = "pointer";
			close_div.style.background = "url(http://ht.www.sogou.com/images/mini.png) no-repeat";
			//
			close_div.onmouseover = function () {
				close_div.style.backgroundPosition = "-60px top";
			};
			close_div.onmouseout = function () {
				close_div.style.backgroundPosition = "0 0";
			};
			close_div.onclick = function () {
				ifrm.style.display = "block";
				close_div.style.display = "none";
			};
			//
			document.body.appendChild(close_div);
		}
				
		function showConfig(data) {
            if(showConfigType === "1"){
				if (!configDiv) {
	                configDiv = document.createElement("div");
					//configDiv.className = "extoptbox";
	                configDiv.className = "rec-config-pop1";
					configDiv.style.zIndex = zIndex;
					configDiv.style.display = "none";
					//configDiv.innerHTML = '<a id="currentSiteConfig" href="#">本站点默认关闭</a> <a id="allSiteConfig" href="#">所有站点默认关闭</a> <div class="extfeedback"><a target="_blank" href="http://www.sogou.com/complain/ext_feedback.html">反馈</a></div>';
					configDiv.innerHTML = ' <ul class="rec-config-option"><li><input type="checkbox" class="rec-checkbox1" id="currentSiteConfig"> <label for="current-site">本站默认关闭</label></li><li><input type="checkbox" class="rec-checkbox1" id="allSiteConfig"><label for="all-site"> 所有站点默认关闭</label></li></ul><a target="_blank" href="http://www.sogou.com/complain/ext_feedback.html" class="rec-feedback">反馈</a>';
	                document.body.appendChild(configDiv);
					//configBgDiv.appendChild(configDiv);
					bind(document, "click", function(){configDiv.style.display = "none";});
					document.getElementById("currentSiteConfig").onclick = function(){
						document.getElementById("sogouconfirmtxt").innerHTML = "您确定在本站点都不展现搜狗推荐吗？";
						configType = "cur";
						confirmBox.show();
						return false;
					};
					document.getElementById("allSiteConfig").onclick = function(){
						document.getElementById("sogouconfirmtxt").innerHTML = "您确定在所有页面都不展现搜狗推荐吗？";
						configType = "all";
						confirmBox.show();
						return false;
					};
				}
				//configBgDiv.style.display = "";
				configDiv.style.display = "";
            } else if(showConfigType === "6" || showConfigType === "7") {
                if (!configDiv) {
                    configDiv = document.createElement("div");
					//configDiv.className = "extoptbox";
                    configDiv.className = "rec-config-pop6";
                    configDiv.style.zIndex = zIndex;
                    configDiv.style.display = "none";
                    if(showConfigType === "7"){
                    configDiv.innerHTML = ' <ul class="rec-config-option"><li><input type="checkbox" class="rec-checkbox1" id="currentSiteConfig"> <label for="current-site">本站点默认关闭</label></li><li><input type="checkbox" class="rec-checkbox1" id="allSiteConfig"><label for="all-site"> 所有站点默认关闭</label></li></ul><a target="_blank" href="http://www.sogou.com/complain/ext_feedback.html" class="rec-feedback">反馈</a>';
                    }else{
                        configDiv.innerHTML = ' <ul class="rec-config-option"><li><input type="checkbox" class="rec-checkbox1" id="currentSiteConfig"> <label for="current-site" class="rec-blue">本站点默认关闭</label></li><li><input type="checkbox" class="rec-checkbox1" id="allSiteConfig"><label for="all-site" class="rec-blue"> 所有站点默认关闭</label></li></ul><a target="_blank" href="http://www.sogou.com/complain/ext_feedback.html" class="rec-feedback">反馈</a>';
                    }
                    document.body.appendChild(configDiv);
					//configBgDiv.appendChild(configDiv);
                    bind(document, "click", function(){configDiv.style.display = "none";});
                    document.getElementById("currentSiteConfig").onclick = function(){
                        document.getElementById("sogouconfirmtxt").innerHTML = "您确定在本站点都不展现搜狗推荐吗？";
                        configType = "cur";
                        confirmBox.show();
                        return false;
                    };
                    document.getElementById("allSiteConfig").onclick = function(){
                        document.getElementById("sogouconfirmtxt").innerHTML = "您确定在所有页面都不展现搜狗推荐吗？";
                        configType = "all";
                        confirmBox.show();
                        return false;
                    };
                }
                //configBgDiv.style.display = "";
                configDiv.style.display = "";
            } else {
                if (!configDiv) {
                    configBgDiv = document.createElement("div");
                    configBgDiv.className = "extoptboxbg";
                    configBgDiv.style.zIndex = zIndex;
                    configBgDiv.style.display = "none";
                    document.body.appendChild(configBgDiv);

                    configDiv = document.createElement("div");
                    configDiv.className = "extoptbox";
                    configDiv.style.zIndex = zIndex;
                    configDiv.style.display = "none";
                    configDiv.innerHTML = '<a id="currentSiteConfig" href="#">本站点默认关闭</a> <a id="allSiteConfig" href="#">所有站点默认关闭</a> <div class="extfeedback"><a target="_blank" href="http://www.sogou.com/complain/ext_feedback.html">反馈</a></div>';
                    document.body.appendChild(configDiv);
                    bind(document, "click", function(){configDiv.style.display = "none";configBgDiv.style.display = "none";});
                    document.getElementById("currentSiteConfig").onclick = function(){
                        document.getElementById("sogouconfirmtxt").innerHTML = "您确定在本站点都不展现搜狗推荐吗？";
                        configType = "cur";
                        confirmBox.show();
                        return false;
                    };
                    document.getElementById("allSiteConfig").onclick = function(){
                        document.getElementById("sogouconfirmtxt").innerHTML = "您确定在所有页面都不展现搜狗推荐吗？";
                        configType = "all";
                        confirmBox.show();
                        return false;
                    };
                }
                configBgDiv.style.display = "";
                configDiv.style.display = "";
            }
		}

		
		function buildLink(txt, url, func, cls) {
			var a = document.createElement("a");
			a.innerHTML = txt;  
			a.href = url;
			if (func) {
				a.onclick = func;
			}
			if (cls) {
				a.className = cls;
			}
			return a;
		}
		
		function bind(elem, evt, func){
			if (elem){
				return elem.addEventListener?elem.addEventListener(evt,func,false):elem.attachEvent("on"+evt,func);
			}
		}
		
		function pb(type, act, time) {
			var url = ["http://pb.sogou.com/",type,".gif?uigs_productid=webext&type=ext_sugg&act=", act,"&uigs_t=", (new Date()).getTime(), "&pid=sogou-brse-596dedf4498e258e&u=",uuid,"&y=", yyid, "&v=", version];
			if (time) {
				url.push("&rt=");
				url.push(time);
			}

			sendLog(url.join(''));
		}
		
		//提示框
		function s_thickbox(divid, closeid, width, height) {
			//add style for a node
			var css = function(a, b, c){if (a != null){a.style[b] = c;}}
			var body = document.body;
			var html = document.body.parentNode;
			var showdiv = document.getElementById(divid);
			var overlay = document.getElementById("ext_overlay");
			var userAgent = navigator.userAgent.toLowerCase();
			isIe6 = userAgent.indexOf("msie 6") != -1 && ! window.opera;
			var isMacFF = userAgent.indexOf('mac') != -1 && userAgent.indexOf('firefox')!=-1;
		
			//create background div and iframe
			if (typeof document.body.style.maxHeight === "undefined") {//if IE
				if (document.getElementById("ext_HideSelect") === null) {//iframe to hide select elements in IE
					tb_append("iframe", "ext_HideSelect");
				}
			}
			if(overlay === null){
				overlay = tb_append("div", "ext_overlay");
				overlay.style.display = "none";
			}
			if(isMacFF){
				overlay.className = "ext_overlayMacFFBGHack";//use png overlay so hide flash
			}
			else{
				overlay.className = "ext_overlayBG";//use background and opacity
			}
		
			//bind event
			if (document.getElementById(closeid)) {document.getElementById(closeid).onclick = tb_remove;}
			overlay.onclick = tb_remove;
			overlay.style.zIndex = zIndex + 1;
			showdiv.style.zIndex = zIndex + 2;
		
			function tb_remove() {
				showdiv.style.display = "none";
				overlay.style.display = "none";
				if (typeof document.body.style.maxHeight == "undefined") {//if IE
					css(body, "width", "auto");
					css(body, "height", "auto");
					css(html, "width", "auto");
					css(html, "height", "auto");
					css(html, "overflow", "");
					document.getElementById("ext_HideSelect").style.display="none";
				}
				return false;
			}
		
			function tb_position() {
				css(showdiv, "marginLeft", '-' + parseInt((showdiv.offsetWidth / 2),10) + 'px');
		
				if (!isIe6) { // take away IE6
					css(showdiv, "marginTop", '-' + parseInt((showdiv.offsetHeight / 2),10) + 'px');
				}
			}
		
			function tb_append(tag, id){
				var t = document.createElement(tag);
				t.id = id;
				document.body.appendChild(t);
				return t;
			}
		
			function tb_show() {
				if (typeof document.body.style.maxHeight === "undefined") {//if IE
					css(body, "width", "100%");
					css(body, "height", "100%");
					css(html, "width", "100%");
					css(html, "height", "100%");
					css(html, "overflow", "hidden");
					document.getElementById("ext_HideSelect").style.display="";
				}
				showdiv.style.display = "block";
				overlay.style.display = "block";
				tb_position();
			}
			this.show = tb_show;
			this.hide = tb_remove;
		}

		sogouExplorer.extension.onRequest.addListener(function (request, sender, sendResponse){
	  		try {
				if (!request || !request.cmd){
					//命令错误，直接返回
					return;
				}

				if (request.cmd == "show" && ifrm){
					hiding = false;
					ifrm.style.display = "block";
					document.getElementById("sougou_bottom").style.display = "block";
					//showTip(request.data);
					
					if (document.location.href.indexOf('sohu.com') > 0) {
						document.body.className = document.body.className+" sohuIEHINT";
					}

					var pbUrl = "http://pb.sogou.com/pv.gif?&uigs_productid=webext&type=ext_sugg&uigs_t="
						+ (new Date().getTime())
						+ "&sceneid=" + request.data.sceneId
						+ "&v=" + version
						+ "&stype=show";

					sendLog(pbUrl);
				}else if (request.cmd == "close" && ifrm){
					hiding = true;
					ifrm.style.display = "none";
					if (document.location.href.indexOf('sohu.com') > 0) {
						document.body.className = document.body.className.replace(/sohuIEHINT/g, "");
					}
					if(tipDiv){
						tipDiv.style.display="none";
					}
				}else if (request.cmd == "init"){
					initIframe(request.data);
					//
					//initCloseDiv();
				}else if (request.cmd == "config"){
					if(request.type === "old"){
						showConfigType = "old";
					}else if(request.type === "1") {
						showConfigType = "1";
					}else if(request.type === "6") {
						showConfigType = "6";
					}else if(request.type === "7") {
						showConfigType = "7";
					}
					showConfig(request.data);
				}else if (request.cmd == "closeconfig"){					
					if (configDiv) {
						configDiv.style.display = "none";
						configBgDiv.style.display = "none";
					}
				}else if (request.cmd == "loadtime"){
					pingback(request.data, request.yyid);
				} else if(request.cmd == "tab_set_height") {
					if(request.data.height) {
						ifrm.style.height = request.data.height;
					}

					if(request.data.width) {
						ifrm.style.width = request.data.width;
					}

				}
			} catch (e) {
			}
		});

		function init() {
			setTimeout(function(){
				pingback(5000);
			}, 5000);
			baiduId();
			sendCmd("isforbidden", [document.location.toString(),isIe6]);
			if(ua.indexOf('msie') > -1 && ua.indexOf('se 2.x') > -1) {
				pb('pv', 'sogouie');
			}
		}

		function sendLog(url) {
			sogouExplorer.extension.sendRequest({cmd: 'sendLog', url: url}, function(status) {
				if(status && status.failed) {
					new Image().src = url;
				}
			});
		}

		bind(window, 'message', function(e) {
			if(e && typeof e.data !== 'string') {
				return;
			}

			var data = e.data.split('\n');

			if(data[0] === 'sg_transfer_send' && (e.origin === 'http://ht.www.sogou.com' || e.origin === 'https://tuijian.sogou.com')){
				sendLog(data[1]);
			}
		});

		// https下不出插件
		if(location.protocol == 'http:') {
			setTimeout(function(){
				if(document && document.location) {
					try{
						getIeVersion();
						if (isIe) {
							if (document.body.readyState == "complete" || document.body.readyState == "loaded") {
								init();
							} else {
								bind(document.body, "load", init);
							}
						} else {
							init();
						}
					}catch(e){}
				}
			}, 1);
		}

	})();
}

/**
 */
 
//alert("content script stop js loaded "+document.location);
if (typeof comSogouWwwStop == "undefined"){
	
	var SERVER = "http://ht.www.sogou.com/websearch/features/yun6.jsp?pid=sogou-brse-d2a452edff079ca6&";
	
	window.comSogouWwwStop = true;
	
	setTimeout(function(){ 
		if (!document.location || document.location.toString().indexOf(SERVER) != 0){
			return;
		}

		var windowcloseit = getById("windowcloseit"),
			hideBtn = getById("hide-btn"),
			hide = getById('hide-list'),
			show = getById('show-list');

		function getById(id) {
			return document.getElementById(id);
		}

		function bind(elem, evt, func){
			if (elem){
				return elem.addEventListener?elem.addEventListener(evt,func,false):elem.attachEvent("on"+evt,func);
			}
		}

		// 获取推荐词，展现在插件弹层中
		function storeHint() {
			var hint = [];
			var i = 0; 
			var a = document.getElementById("hint_" + i);
			var storeClick = function(){sogouExplorer.extension.sendRequest({cmd: "click"});};
			while(a) {
				bind(a, "click", storeClick);
				hint.push({"text":a.innerHTML, "url":a.href});
				i++;
				a = document.getElementById("hint_" + i);
			}
			return hint;
		}

		function showAll() {
			var contentUl = getById('content'),
                prev = getById('prev'),
                next = getById('next');

			hide.onclick = function() {
				hide.style.display = 'none';
                contentUl.style.left = 0;
				show.style.display = 'block';
                next.style.display = 'block';
                prev.style.display = 'none';
				hideBtn.className = 'sec-logo';

				sogouExplorer.extension.sendRequest({cmd: "set_height", data: {height: "84px", width: "100%"}});
			};
		}

		if (windowcloseit){
			// send 'ext_hide_it' to extension;
			windowcloseit.onclick = function(){
				sogouExplorer.extension.sendRequest({cmd: "set_height", data:{height: '112px', width: '171px'}});
				setTimeout(function() {
					sogouExplorer.extension.sendRequest({cmd: "ext_hide_it"});
				},20);
			};

			var flag = false;

			document.body.onclick = function(){
				if (flag) {
					flag = false;
				} else {
					sogouExplorer.extension.sendRequest({cmd: "closeconfig"});
				}
			};
			// document.getElementById("bbhidden").onclick = function(){
			// 	sogouExplorer.extension.sendRequest({cmd: "hide"});
			// 	return false;
			// }
			var sogoutip = document.getElementById("sogoutip");
			var sceneId = document.body.getAttribute("sid");
			var tip = {
				word:'',
                sceneId: sceneId,
				config:[]
			};
			if(sogoutip) {
				tip.word = sogoutip.innerHTML;
				tip.config = sogoutip.title.split(",");
			}
			var hint = storeHint();
			sogouExplorer.extension.sendRequest({cmd: "show", data: {hint:hint,tip:tip}});
		}else{
			if (document.getElementById("windowcloseitnow")){
				sogouExplorer.extension.sendRequest({cmd: "closeit", data: true});
			}
		}

		if(hideBtn) {
			showAll();
			hideBtn.onclick = function() {
				sogouExplorer.extension.sendRequest({cmd: "closeit"});
			};
		}

		sogouExplorer.extension.onRequest.addListener(function (request, sender, sendResponse){
			try {
				if(!request || !request.cmd) {
					//命令错误，直接返回
					return;
				}

				if(request.cmd === "tab_hide_it") {
					//ifrm.style.display = "none";
					//close_div.style.display = "block";
					//	缩到左边
					show.style.display = "none";
					hide.style.display = "block";
				}
			} catch (e) {}
		});

	}, 1);
}


})(window.external.sogouExplorer(window,-1709349363));
(function(sogouExplorer){
if (sogouExplorer == undefined) return;
var chrome = sogouExplorer;
sogouExplorer.extension.setExecScriptHandler(function(s){eval(s);});!function(){function e(e){sogouExplorer.extension.sendRequest(e)}function t(){var e="";return e=document.getSelection?document.getSelection().toString():document.selection.createRange().text,e=e.trim(),1==e.length&&1===e.charCodeAt(0)&&(e=""),e}function n(e,t,n){e.addEventListener?e.addEventListener(t,n,!0):e.attachEvent?e.attachEvent("on"+t,n):e["on"+t]=n}function i(e,t,n){e.removeEventListener?e.removeEventListener(t,n,!0):e.detachEvent?e.detachEvent("on"+t,n):e["on"+t]=null}function o(e){e=e||window.event,e.target=e.target||e.srcElement;var t;e.which=e.which||(t=e.button,1&t?1:2&t?3:4&t?2:0);var n=document.documentElement,i=document.body;return e.pageX=e.pageX||e.clientX+(n&&n.scrollLeft||i&&i.scrollLeft||0)-(n&&n.clientLeft||i&&i.clientLeft||0),e.pageY=e.pageY||e.clientY+(n&&n.scrollTop||i&&i.scrollTop||0)-(n&&n.clientTop||i&&i.clientTop||0),e}function r(){try{document.execCommand("copy")}catch(e){}}function a(e,t){for(;e;){if(t(e)===!0)return e;e=e.parentNode}return null}function c(e){var t=!0,n=e.tagName.toLowerCase(),i=a(e,function(e){return e&&e.hasAttribute&&e.hasAttribute("contenteditable")});return(i||0==e.disabled&&("input"==n||"textarea"==n))&&(t=!1),t}function u(t,n){(0!=S||0!=t)&&n&&e({cmd:"panel",type:t,data:n,uuid:M.create()})}function d(e){for(var t=0;e&&1===e.nodeType;)t+=e.offsetLeft,e=e.offsetParent;return t}function m(e){return Math.max(e.scrollWidth,e.offsetWidth)}function l(){var e;if("getSelection"in window){var t=getSelection(),n=t.getRangeAt(0);e=n.commonAncestorContainer}else{var n=document.selection.createRange();e=n.parentElement}return e}function s(e){var t=l();if(1==t.nodeType){var n=d(t),i=n+m(t);return e>n&&i>e}return!0}function f(e,t,n){var i=e[t];e[t]=e[n],e[n]=i}function h(e){e.x1>e.x2&&f(e,"x1","x2"),e.y1>e.y2&&f(e,"y1","y2")}function v(e,t,n){h(n);var i=16;return t<n.y1-i||t>n.y2+i?!0:(e<n.x1||e>n.x2)&&!s(e)?!0:!1}function w(){try{"getSelection"in window?getSelection().removeAllRanges():document.selection.empty()}catch(e){}}function p(e){var t=arguments.callee;if("number"!=typeof t.offset){var n=document.body.scrollTop,i=document.createElement("div");i.style.cssText="position: absolute; left: 0; top: 0;",document.body.appendChild(i),t.offset=-i.getBoundingClientRect().top-n,document.body.removeChild(i),i=null}if(e){var o=e.getBoundingClientRect(),r=t.offset;return{top:o.top+r,right:o.right+r,bottom:o.bottom+r,left:o.left+r}}}function g(e,t){for(var n,i=document.getElementsByTagName("iframe"),o=0,r=i.length;r>o;++o){var a=i[o],c=a.getAttribute("name");if(c===e){n=a;break}}if(!n)for(var o=0,r=i.length;r>o;++o){var a=i[o];if(a.src===t){n=a;break}}var u={width:window.innerWidth||document.documentElement.clientWidth,height:window.innerHeight||document.documentElement.clientHeight,left:0,top:0};if(n){var d=p(n);u.left=d.left,u.top=d.top}return u}function y(){function e(e){if(r){e=o(e);var n={clientX:e.clientX,clientY:e.clientY},i=t();if(i){if(a.x2=e.pageX,a.y2=e.pageY,"mouseup"==e.type&&!e.shiftKey&&a.x2==a.x1&&a.y2==a.y1)return;setTimeout(function(){b(n)},0)}}r=!1}function i(e){var n=t();return n?void 0:u(0,e.type||"key")}if(!y.hasrun){y.hasrun=!0;var r=!1,a={};n(document,"mouseup",e),n(document,"dblclick",e),n(document,"mousedown",function(e){if(u(0,"mousedown"),e=o(e),1==e.which&&(r=c(e.target))){var n=t();if(n&&!e.shiftKey){var i=v(e.pageX,e.pageY,a);i&&w()}a.x1=e.pageX,a.y1=e.pageY}}),n(window,"resize",function(){u(0,"resize")}),n(document,"keydown",i),n(document,"keyup",i),n(document,"focusin",function(e){e=o(e);var t=e.target,n=t.nodeName.toUpperCase();("INPUT"==n||"TEXTAREA"==n)&&u(0,"focusin")}),n(document,"mousewheel",function(){u(0,"mousewheel-ie")}),n(window,"mousewheel",function(e){u(0,"mousewheel")}),n(window,"hashchange",function(e){var n=t();n&&w(),u(0,"hashchange")})}}function x(t,n){var i=window==top;if(i)a={x:t.clientX,y:t.clientY,w:window.innerWidth||document.documentElement.clientWidth,h:window.innerHeight||document.documentElement.clientHeight},n(a);else{var o=this.name;try{var r=p(frameElement),a={x:t.clientX+r.left,y:t.clientY+r.top,w:parent.innerWidth||parent.document.documentElement.clientWidth,h:parent.innerHeight||parent.document.documentElement.clientHeight};n(a)}catch(c){var u=t.clientX,d=t.clientY;e({cmd:"get-relative-position-and-size",id:R(function(e){a={x:u+e.left,y:d+e.top,w:e.width,h:e.height},n(a)}),data:{name:o,url:location.href}})}}}function b(e){function n(e){u(1,{text:i,zoom:100*X.zoom(),position:e})}if(!L){var i=t();return i&&"输入法手写拼音关闭"!=i?void x(e,n):void u(0,"no-selection")}}function P(e,t,n){var i,o=0,r=function(){n.timer=null,o=+new Date,e.apply(this,i)};return function(){i=arguments;var a=+new Date,c=t-(a-o);0>c?(n.timer&&(clearTimeout(n.timer),n.timer=null),o=a,e.apply(this,i)):!n.timer&&n.trailing&&(n.timer=setTimeout(r,c))}}function E(e,t,o){S=!!e,o&&(e&&t?n(document,"mousemove",k):(W.timer&&clearTimeout(W.timer),i(document,"mousemove",k)))}function C(){if(!C.hasRun){C.hasRun=!0;for(var e="prefix_",t=document.getElementsByTagName("iframe"),n=t.length-1;n>=0;n--){t[n].name||t[n].setAttribute("name",e+n);var i=t[n].getAttribute("src");i&&"about:blank"!=i&&(t[n].src+="#"+e+n)}}}if(!window._se_multi_trident_insert){window._se_multi_trident_insert=!0;for(var z=location.host,T=["wx.qq.com"],A=0;A<T.length;++A)if(T[A].indexOf(z)>-1)return;var X=function(){var e=function(){return window.devicePixelRatio||1},t=function(){return{zoom:1,devicePxPerCssPx:1}},n=function(){var t=Math.round(screen.deviceXDPI/screen.logicalXDPI*100)/100;return{zoom:t,devicePxPerCssPx:t*e()}},i=function(){var t=Math.round(document.documentElement.offsetHeight/window.innerHeight*100)/100;return{zoom:t,devicePxPerCssPx:t*e()}},o=function(){var t=Math.round(top.window.outerWidth/top.window.innerWidth*100)/100;return{zoom:t,devicePxPerCssPx:t*e()}},r=function(){var t=Math.round(window.outerWidth/window.innerWidth*100)/100;return{zoom:t,devicePxPerCssPx:t*e()}},a=function(){var t=90==Math.abs(window.orientation)?screen.height:screen.width,n=t/window.innerWidth;return{zoom:n,devicePxPerCssPx:n*e()}},c=function(){var t=function(e){return e.replace(/;/g," !important;")},n=document.createElement("div");n.innerHTML="1<br>2<br>3<br>4<br>5<br>6<br>7<br>8<br>9<br>0",n.setAttribute("style",t("font: 100px/1em sans-serif; -webkit-text-size-adjust: none; text-size-adjust: none; height: auto; width: 1em; padding: 0; overflow: visible;"));var i=document.createElement("div");i.setAttribute("style",t("width:0; height:0; overflow:hidden; visibility:hidden; position: absolute;")),i.appendChild(n),document.body.appendChild(i);var o=1e3/n.clientHeight;return o=Math.round(100*o)/100,document.body.removeChild(i),{zoom:o,devicePxPerCssPx:o*e()}},u=function(){var e=l("min--moz-device-pixel-ratio","",0,10,20,1e-4);return e=Math.round(100*e)/100,{zoom:e,devicePxPerCssPx:e}},d=function(){return{zoom:u().zoom,devicePxPerCssPx:e()}},m=function(){var t=window.top.outerWidth/window.top.innerWidth;return t=Math.round(100*t)/100,{zoom:t,devicePxPerCssPx:t*e()}},l=function(e,t,n,i,o,r){function a(n,i,o){var u=(n+i)/2;if(0>=o||r>i-n)return u;var d="("+e+":"+u+t+")";return c(d).matches?a(u,i,o-1):a(n,u,o-1)}var c,u,d,m;window.matchMedia?c=window.matchMedia:(u=document.getElementsByTagName("head")[0],d=document.createElement("style"),u.appendChild(d),m=document.createElement("div"),m.className="mediaQueryBinarySearch",m.style.display="none",document.body.appendChild(m),c=function(e){d.sheet.insertRule("@media "+e+"{.mediaQueryBinarySearch {text-decoration: underline} }",0);var t="underline"==getComputedStyle(m,null).textDecoration;return d.sheet.deleteRule(0),{matches:t}});var l=a(n,i,o);return m&&(u.removeChild(d),document.body.removeChild(m)),l},s=function(){var e=t;return isNaN(screen.logicalXDPI)||isNaN(screen.systemXDPI)?window.navigator.msMaxTouchPoints?e=i:window.chrome&&!(window.opera||navigator.userAgent.indexOf(" Opera")>=0)?e=o:Object.prototype.toString.call(window.HTMLElement).indexOf("Constructor")>0?e=r:"orientation"in window&&"webkitRequestAnimationFrame"in window?e=a:"webkitRequestAnimationFrame"in window?e=c:navigator.userAgent.indexOf("Opera")>=0?e=m:window.devicePixelRatio?e=d:u().zoom>.001&&(e=u):e=n,e}();return{zoom:function(){return s().zoom},device:function(){return s().devicePxPerCssPx}}}();String.prototype.trim=String.prototype.trim||function(){return this.replace(/^\s+|\s+$/,"")};var R=function(){var e="_sogou_search_translate_",t=1;return function(n){var i=e+ ++t;return window[i]=n,i}}(),M=function(){var e;return{get:function(){return e},create:function(){return e=(""+Math.random()).slice(2)+(new Date).getTime()}}}(),W={trailing:!0},k=P(function(t){t=o(t);var n={clientX:t.clientX,clientY:t.clientY};x(n,function(t){e({cmd:"opacity-update",data:{position:t,zoom:100*X.zoom()}})})},100,W);window==top&&("complate"==document.readyState?C():n(document,"DOMContentLoaded",C)),sogouExplorer.extension.onRequest.addListener(function(t,n,i){var o=t.cmd;switch(o){case"config-update":var a=t.data;L=1==!!a.disableAll;break;case"copy":t.uuid==M.get()&&r();break;case"run":var c=t.id,u=t.data,d=window[c];"function"==typeof d&&(d(u),window[c]=null);break;case"get-relative-position-and-size":if(top==window){var u=t.data,m=g(u.name,u.url);e({cmd:"run",id:t.id,data:m})}break;case"sync-layer-status":E(t.data,t.helper,t.support)}});var L=!1,S=!1;y()}}();
})(window.external.sogouExplorer(window,736977059));
(function(sogouExplorer){
if (sogouExplorer == undefined) return;
var chrome = sogouExplorer;
sogouExplorer.extension.setExecScriptHandler(function(s){eval(s);});try{
	if (window._com_sogou_recommend_on_connect === undefined){
		sogouExplorer.extension.sendRequest({type:"event", info:"documentReady"});
		window._com_sogou_recommend_on_connect = null;
		window._com_sogou_recommend_on_connect = function(port){
			port.onMessage.addListener(function(msg){
				try{
					if (msg == null) return;
					if(msg.code == "connect"){
						var currentUrl = document.URL;
						var start = currentUrl.indexOf("#");
						if (start > 0){
							currentUrl = currentUrl.substring(0, start);
						}
						if (msg.url != currentUrl) return;
						
						if(typeof(window._com_sogou_recommend_frameName) !== "undefined" && window._com_sogou_recommend_frameName === msg.frameName) return;
						window._com_sogou_recommend_frameName = msg.frameName;
						
						port.postMessage({code:"connect"});
						//ע��excanvas.js
						if(msg.collectionInfo && msg.collectionInfo.collectionKey && msg.excanvasJs){
							window.sogouExcanvasJs = decodeURIComponent(msg.excanvasJs);
						}
						
						//ע��findConstant.js
						if(msg.findConstantJs){
							var findConstantJs = decodeURIComponent(msg.findConstantJs);
							eval(findConstantJs);
						}
						
						//ע��parser.js
						if(msg.parserKey && msg.parserJs){
							var parserJs = decodeURIComponent(msg.parserJs);
							eval(parserJs);
							_com_sogou_recommend_parser_page(msg.parserKey, port);
						}else{
							_com_sogou_recommend_parser_callback({"port": port});
						}
						
						//ע��injector.js
						if(msg.injectorJs){
							var injectorJs = decodeURIComponent(msg.injectorJs);
							eval(injectorJs);
						}
						
						//ע��favorites.js
						// if(msg.favoritesJs && currentUrl.indexOf("http://zhushou.gouwu.sogou.com/favorites/index.html") > -1){
							// var favoritesJs = decodeURIComponent(msg.favoritesJs);
							// eval(favoritesJs);
						// }
						
						//ע��collection.js
						if(msg.collectionInfo && msg.collectionInfo.collectionKey && msg.collectionInfo.collectionJs){
							window.sogouFindCollectionInfo = {userLoginInfo: msg.collectionInfo.userLoginInfo};
							var collectionJs = decodeURIComponent(msg.collectionInfo.collectionJs);
							eval(collectionJs);
							_com_sogou_recommend_collection_page(msg.collectionInfo.collectionKey, port);
						}
					}else if(msg.code == "loginStatusChanged"){
						if(window.sogouFindCollection && window.sogouFindCollection.loginStatusChange){
							window.sogouFindCollectionInfo = {userLoginInfo: msg.info.userLoginInfo};
							window.sogouFindCollection.loginStatusChange(msg.info.type, msg.info.collected);
						}
					}else if(msg.code == "collectionSuccess"){
						if(window.sogouFindCollection && window.sogouFindCollection.collectionSuccess){
							window.sogouFindCollection.collectionSuccess(msg.type);
						}
					}else if(msg.code == "collectionFailed"){
						if(window.sogouFindCollection && window.sogouFindCollection.collectionFailed){
							window.sogouFindCollection.collectionFailed(msg.type);
						}
					}else if(msg.code == "sendMessageFromFind"){
						if(window.sogouFindCollection && window.sogouFindCollection.sendMessageFromFind){
							window.sogouFindCollection.sendMessageFromFind(msg);
						}
					}else if(msg.code == "collectionSuccessUnlogged"){
						if(window.sogouFindCollection && window.sogouFindCollection.collectionSuccessUnlogged){
							window.sogouFindCollection.collectionSuccessUnlogged(msg.type);
						}
					}else if(msg.code == "collectionFailedUnlogged"){
						if(window.sogouFindCollection && window.sogouFindCollection.collectionFailedUnlogged){
							window.sogouFindCollection.collectionFailedUnlogged(msg.type);
						}
					}else if(msg.code == "collectionFullUnlogged"){
						if(window.sogouFindCollection && window.sogouFindCollection.collectionFullUnlogged){
							window.sogouFindCollection.collectionFullUnlogged(msg.type);
						}
					}else if(msg.code == "collectionToMuchUnlogged"){
						if(window.sogouFindCollection && window.sogouFindCollection.collectionToMuchUnlogged){
							window.sogouFindCollection.collectionToMuchUnlogged(msg.type);
						}
					}
				}catch(e){
				}
			});
		};
		
		sogouExplorer.extension.onConnect.addListener(window._com_sogou_recommend_on_connect);
		
		window._com_sogou_recommend_parser_callback = function(info){
			var port = info.port;
			var key = info.key;
			if(key.indexOf("pb_") === -1){
				port.postMessage({code:"parseResult", result:info});
			}else{
				port.postMessage({code:"pingback", result:info});
			}
		};
		
		window._com_sogou_recommend_parser_delaycallback = function(info){
			var port = info.port;
			port.postMessage({code:"parseDelayResult", result:info});
		};
		
		window._com_sogou_recommend_collection_callback = function(info){
			var port = info.port;
			port.postMessage({code:"collectionResult", result:info});
		};
		
		//ҳ�����
		window._com_sogou_recommend_parser_page = function(key, port, count){
			if(typeof(count) == "undefined") count = 0;
			if(document.title != null && document.title != "" && document.title != undefined){
				var result = {"title": encodeURIComponent(document.title), "referer": encodeURIComponent(document.referrer), "port": port, "status": "injectsuccess|hastitle", key: key};
				if(window._sogou_parser && window._sogou_parser.startParse){
					window._sogou_parser.startParse(document, key, result, window._com_sogou_recommend_parser_callback, window._com_sogou_recommend_parser_delaycallback);
				}else{
					result.crumb="";
					window._com_sogou_recommend_parser_callback(result);
				}
			}else{
				try{
					var retryCount = 10, retryInterval = 500;
					if(window._sogou_parser && window._sogou_parser.RETRYCOUNT && window._sogou_parser.RETRYINTERVAL){
						retryCount = window._sogou_parser.RETRYCOUNT;
						retryInterval = window._sogou_parser.RETRYINTERVAL;
					}
					
					if(count < retryCount){
						setTimeout(function(){window._com_sogou_recommend_parser_page(key, port, count + 1);}, retryInterval);
					}else{
						var result = {"title":"", "referer":encodeURIComponent(document.referrer), "port": port, "status": "injectsuccess|notitle", key: key};
						if(window._sogou_parser && window._sogou_parser.startParse){
							window._sogou_parser.startParse(document, key, result, window._com_sogou_recommend_parser_callback, window._com_sogou_recommend_parser_delaycallback);
						}else{
							result.crumb="";
							window._com_sogou_recommend_parser_callback(result);
						}
					}
				}catch(e){
				}
			}
		};
		
		//��Ʒ�ղ�
		window._com_sogou_recommend_collection_page = function(key, port, count){
			if(typeof count == "undefined")	count = 0;
			var retryCount = 10, retryInterval = 500;
			if(window.findConstant && window.findConstant.COLLECTION_RETRY_COUNT && window.findConstant.COLLECTION_RETRY_INTERVAL){
				retryCount = window.findConstant.COLLECTION_RETRY_COUNT;
				retryInterval = window.findConstant.COLLECTION_RETRY_INTERVAL;
			}
			
			if(document.readyState == "interactive" || document.readyState == "complete" || count > retryCount){
				var result = {};
				result.port = port;
				window.sogouFindCollection.startCollection(key, result, window._com_sogou_recommend_collection_callback);
			}else{
				setTimeout(function(){
					window._com_sogou_recommend_collection_page(key, port, count + 1);
				}, retryInterval);
			}
		};
	}
}catch(e){
}
})(window.external.sogouExplorer(window,1488806921));
(function(){var jsNode = document.getElementById("tailjs-138870250_13064");if (jsNode){jsNode.parentNode.removeChild(jsNode);}})();