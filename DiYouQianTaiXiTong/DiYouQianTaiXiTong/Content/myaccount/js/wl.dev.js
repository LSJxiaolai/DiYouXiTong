var __jrrda= ['36455653.2087009478.1513690997.1513690997.1514336869.2'];
var __jrrdb= ['36455653.3.2087009478|2.1514336869'];
var __jrrdc= ['36455653'];
var __jrrdv= ['36455653|direct|-|none|-'];
var c_domain='jdjrflow.jd.com';
(function(){c();function c(){var aj={cdomain:"",getHost:function(l){var s=/.*\:\/\/([^\/|:]*).*/;var g=l.match(s);var p="";if(typeof g!="undefined"&&null!=g){p=g[1]}return p},urlParams:function(g){var l=new RegExp("(^|&)"+g+"=([^&]*)(&|$)");var p=window.location.search.substr(1).match(l);if(p!=null){return unescape(p[2])}return null},setCookie:function(g,l,s){var p=new Date();p.setTime(p.getTime()+s);document.cookie=g+"="+escape(l)+";expires="+p.toGMTString()+";path=/;domain="+this.cdomain},getCookie:function(g){var t="";if(document.cookie&&document.cookie!=""){var s=document.cookie.split(";");for(var p=0;p<s.length;p++){var l=$.trim(s[p]);if(l.substring(0,g.length+1)==(g+"=")){t=decodeURIComponent(l.substring(g.length+1));break}}}return t}};var a4={account:"",visit:function(){var y=aj.getCookie("_jrda");var s=aj.getCookie("_jrdb");var l=new Date().getTime();var g=1;var t=30*60*1000;var p=180*24*60*60*1000;if(!y&&!s){aj.setCookie("_jrda",g,p);aj.setCookie("_jrdb",l,t)}else{if(y&&!s){g=parseInt(y)+1;aj.setCookie("_jrda",g,p);aj.setCookie("_jrdb",l,t)}else{aj.setCookie("_jrdb",l,t)}}},order:function(D){var L=aj.getHost(D),l="",p=null,C=null,t=null,y=null;var K=aj.getHost(document.location.href);if(K=="trade.z.jd.com"||L=="trade.z.jd.com"){y="10002";p=$(".module_item:first dl:first dd").html();t=$("#_projectId").val();var g=$(".f_red28:first").html();if(g!=null){C=g.substr(1,g.length-1)}}else{if(L=="licai.bx.jd.com"){y="102";p=aj.urlParams("orderId");C=aj.urlParams("shouldPay")}else{if(L=="bill.jr.jd.com"){y="10003";p=$("#orderId").val();C=$("#shouldPay").val()}else{if(L==""){L=document.location.href;if(L.match("trade.jr.jd.com")){var s=$("a[href='/centre/jrbpayin.action']").size();if(s==1){y="10000";p=aj.urlParams("order");C=aj.urlParams("amount")}else{$("a[class='loan-pub-btn']").each(function(){var W=this;var O=$(W).attr("href");if(O.indexOf("list.jr.jd.com/detail")){y="101";p=aj.urlParams("order");C=$(".bill-money").html().substr(1);t=$(".loan-pub-btn").last().attr("href").split("/")[4].split(".")[0]}})}}}else{if(L=="jrapp.jd.com"){L=document.location.href;if(L.match("m.z.jd.com")){y="10002";p=aj.urlParams("orderId");C=aj.urlParams("amount")}}}}}}if(p!=null&&C!=null){l=p+"|"+C+"|"+t+"|"+y}return l},req:function(W,t){var K=document.referrer;var D="";for(var a6 in t){D+=((a6+"="+t[a6])+"$")}D=D.substring(0,D.length-1);var y=aj.getCookie("__jdu");if(y==""){var l=aj.getCookie("__jda");if(l!=""){var s=l.split(".");y=s[1]}}var g=aj.getCookie("pin");if(!g){g=aj.getCookie("pt_pin")}if(!g){g=aj.getCookie("pwdt_id")}var p=aj.getCookie("_jrda");var L=aj.getCookie("flow_site_ad");var O=aj.getCookie("flow_outsite_ad");var aa=("https:"==document.location.protocol?"https://":"http://")+"jdjrflow.jd.com/log.gif?uid="+y+"&p="+g+"&t="+W+"&m="+this.account+"&ref="+encodeURIComponent(K)+"&v="+encodeURIComponent(D)+"&order="+this.order(K)+"&jrda="+p+"&sitead="+L+"&outsitead="+O+"&rm="+(new Date).getTime()+"&__jrrda="+__jrrda+"&__jrrdb="+__jrrdb+"&__jrrdc="+__jrrdc+"&__jrrdv="+__jrrdv;var C=new Image(1,1);C.src=aa;C.onload=function(){C.onload=null;C.onerror=null}}};if("undefined"==typeof _jraqnew||_jraqnew.length==0){var ag=document.domain.lastIndexOf(".");var av=document.domain.substring(0,ag).lastIndexOf(".");if(av>-1){aj.cdomain=document.domain.substring(av)}else{aj.cdomain="."+document.domain}a4.account="UA-J2000-1"}else{if(_jraqnew.length==1){var am=_jraqnew.pop();a4.account=am[1];var ag=document.domain.lastIndexOf(".");var av=document.domain.substring(0,ag).lastIndexOf(".");if(av>-1){aj.cdomain=document.domain.substring(av)}else{aj.cdomain="."+document.domain}}else{var a1=_jraqnew.pop();aj.cdomain=a1[1];var am=_jraqnew.pop();a4.account=am[1]}}var an="jr.jd.com,z.jd.com,zbbs.jd.com,baitiao.jd.com,baitiao.ps.jd.com,go.jd.com,loan.jd.com,bao.jd.com,baoxian.jd.com,licai.bx.jd.com,licai.jd.com,8.jd.com,8.jr.jd.com,jinku.pay.jd.com".split(",");var a3=function(W){var y=document.referrer,L=aj.cdomain;var s=y&&y.split("/")[2],O=false;var a6=__jrrdv;var p=/from=jrad_(([0-9]{1,})|JD)/;var g=p.exec(ay.location.href);var aa=/&loc=([0-9]{1,})/;var D=aa.exec(ay.location.href);if(g!=null&&D!=null&&D[1]==2){W.set(j,"jrad_"+g[1]);W.set(aE,"-");W.set(f,"referral");W.set(aA,"-")}else{if(s&&(s.indexOf(".jd.com")>-1)){for(var a7=an,C=0;C<a7.length;C++){var K=a7[C].toLowerCase();if(s.indexOf(K)>-1){O=true;break}}if(!O&&(/jrad_(([0-9]{1,})|JD)/.exec(W.get(j))==null)){W.set(j,"JD");W.set(aE,"-");W.set(f,"referral");W.set(aA,"-")}}else{if(!s){if(a6[0]!=null&&"JD"===W.get(j)){W.set(j,"direct");W.set(aE,"-");W.set(f,"none");W.set(aA,"-")}}}}if(window.jrBase&&window.jrBase.navId){W.set(n,window.jrBase.navId)}var l=navigator.userAgent.toLowerCase();if(l&&l.indexOf("deviceid=")!=-1){uas=l.split("&");for(U in uas){if(uas[U].indexOf("deviceid=")!=-1){W.set(z,uas[U].split("=")[1]);break}}}var t=aj.getCookie("qyjr_user");if(t){W.set(q,t)}};var ai=window,ay=document,aL=encodeURIComponent,ak=decodeURIComponent,Y=void 0,U="push",N="join",S="split",X="length",F="indexOf",u="prototype",Q="toLowerCase";var A={};A.util={getParameter:function(p,l){var s=new RegExp("(?:^|&|[?]|[/])"+l+"=([^&]*)");var g=s.exec(p);return g?aL(g[1]):""},Wv:function(s,g,p,l){s=s+"="+g+"; path=/; ";l&&(s+="expires="+(new Date(new Date().getTime()+l)).toGMTString()+"; ");p&&(s+="domain="+p+";");ay.cookie=s},Vv:function(y){for(var g=[],t=ay.cookie[S](";"),l=RegExp("^\\s*"+y+"=\\s*(.*?)\\s*$"),s=0;s<t[X];s++){var p=t[s]["match"](l);p&&g[U](p[1])}return g}};var aR=0;function ar(g){return(g?"_":"")+aR++}var au=ar(),al=ar(),aq=ar(),R=ar(),h=ar(),aT=ar(),ad=ar(),az=ar(),ao=ar(),at=ar(),aI=ar(),aH=ar(),aP=ar(),aY=ar(),af=ar(),ab=ar(),J=ar(),H=ar(),T=ar(),aK=ar(),v=ar(),I=ar(),m=ar(),d=ar(),aW=ar(),aG=ar(),V=ar(),aU=ar(),j=ar(),aE=ar(),f=ar(),aA=ar(),a5=ar(),e=ar(),n=ar(),z=ar(),q=ar();var aX=function(){var p={};this.set=function(t,s){p[t]=s},this.get=function(s){return p[s]!==Y?p[s]:null},this.m=function(t){var s=this.get(t);var C=s==Y||s===""?0:1*s;p[t]=C+1};this.set(au,"UA-J2011-1");this.set(R,aj.cdomain);this.set(aq,r());this.set(h,Math.round((new Date).getTime()/1000));this.set(aT,63072000000);this.set(ad,15768000000);this.set(az,1800000);this.set(aY,Z());var g=ah();this.set(af,g.name);this.set(ab,g.version);this.set(J,P());var l=aS();this.set(H,l.D);this.set(T,l.C);this.set(aK,l.language);this.set(v,l.javaEnabled);this.set(I,l.characterSet);this.set(aU,ax);this.set(a5,new Date().getTime())};var ax="baidu:wd,baidu:word,so.com:q,so.360.cn:q,360so.com:q,360sou.com:q,baidu:q1,m.baidu:word,m.baidu:w,wap.soso:key,m.so:q,page.yicha:key,sz.roboo:q,i.easou:q,wap.sogou:keyword,google:q,soso:w,sogou:query,youdao:q,ucweb:keyword,ucweb:word,114so:kw,yahoo:p,yahoo:q,live:q,msn:q,bing:q,aol:query,aol:q,daum:q,eniro:search_word,naver:query,pchome:q,images.google:q,lycos:query,ask:q,netscape:query,cnn:query,about:terms,mamma:q,voila:rdata,virgilio:qs,alice:qs,yandex:text,najdi:q,seznam:q,search:q,wp:szukaj,onet:qt,szukacz:q,yam:k,kvasir:q,ozu:q,terra:query,rambler:query".split(","),a2=function(){return Math.round((new Date).getTime()/1000)},E=function(){return Math.round(Math.random()*2147483647)},ae=function(){return E()^aw()&2147483647},r=function(){return ac(ay.domain)},aS=function(){var l={},g=ai.navigator,p=ai.screen;l.D=p?p.width+"x"+p.height:"-";l.C=p?p.colorDepth+"-bit":"-";l.language=(g&&(g.language||g.browserLanguage)||"-")[Q]();l.javaEnabled=g&&g.javaEnabled()?1:0;l.characterSet=ay.characterSet||ay.charset||"-";return l},Z=function(){var D,C,y,t;y="ShockwaveFlash";if((D=(D=window.navigator)?D.plugins:Y)&&D[X]>0){for(C=0;C<D[X]&&!t;C++){y=D[C],y.name[F]("Shockwave Flash")>-1&&(t=y.description[S]("Shockwave Flash ")[1])}}else{y=y+"."+y;try{C=new ActiveXObject(y+".7"),t=C.GetVariable("$version")}catch(s){}if(!t){try{C=new ActiveXObject(y+".6"),t="WIN 6,0,21,0",C.AllowScriptAccess="always",t=C.GetVariable("$version")}catch(p){}}if(!t){try{C=new ActiveXObject(y),t=C.GetVariable("$version")}catch(l){}}t&&(t=t[S](" ")[1][S](","),t=t[0]+"."+t[1]+" r"+t[2])}var K=A.util.Vv("_r2");D=t?(t+(K[X]>0?("_"+K[0]):"")):"-";var g=A.util.Vv("limgs");D=D+(g[X]>0?("_"+g[0]):"");return D},aB=function(g){return Y==g||"-"==g||""==g},ac=function(l){var g=1,s=0,p;if(!aB(l)){g=0;for(p=l[X]-1;p>=0;p--){s=l.charCodeAt(p),g=(g<<6&268435455)+s+(s<<14),s=g&266338304,g=s!=0?g^s>>21:g}}return g},aw=function(){var p=aS();for(var l=p,g=ai.navigator,l=g.appName+g.version+l.language+g.platform+g.userAgent+l.javaEnabled+l.D+l.C+(ay.cookie?ay.cookie:"")+(ay.referrer?ay.referrer:""),g=l.length,s=ai.history.length;s>0;){l+=s--^g++}return ac(l)},ah=function(){var g={name:"other",version:"0"},s=navigator.userAgent.toLowerCase();browserRegExp={jrapp:/jdjr[|\-]([\w.]+)/,jdapp:/jdapp[|\;]([\w.]+)/,weixin:/micromessenger[|\/]([\w.]+)/,walletclient:/[|\/]walletclient/,se360:/360se/,se360_2x:/qihu/,ie:/msie[ ]([\w.]+)/,firefox:/firefox[|\/]([\w.]+)/,chrome:/chrome[|\/]([\w.]+)/,safari:/version[|\/]([\w.]+)(\s\w.+)?\s?safari/,opera:/opera[|\/]([\w.]+)/};for(var p in browserRegExp){var l=browserRegExp[p].exec(s);if(l){g.name=p;g.version=l[1]||"0";break}}return g},P=function(){var g=/(win|android|linux|nokia|ipad|iphone|ipod|mac|sunos|solaris)/.exec(navigator.platform.toLowerCase());if(!g){return"other"}else{if(g[0]=="linux"){var l=/(android)/.exec(navigator.userAgent.toLowerCase());return l==null?g[0]:"android"}else{return g[0]}}},aQ=function(){var p="",y=["jwotest_product","jwotest_list","jwotest_cart","jwotest_orderinfo","jwotest_homepage","jwotest_other1","jwotest_other2","jwotest_other3"];for(var t=0,g=y.length;t<g;t++){var s=A.util.Vv(y[t]);if(s[X]==0){continue}var C=ak(s[0]).match(/=(.*?)&/gi),l=[];if(C==null){continue}$.each(C,function(K,D){l.push(K==0?"T"+D.substring(1,D.length-1):D.substring(1,D.length-1))});p+=l[N]("-")+";"}return p},aO=function(l){l.set(ao,ay.location.hostname);l.set(at,ay.title);l.set(aI,ay.location.pathname);l.set(aH,ay.referrer);l.set(aP,ay.location.href);var C=__jrrda,s=C[X]>0?C[0][S]("."):null;l.set(al,s?s[1]:ae());l.set(m,s?s[2]:l.get(h));l.set(d,s?s[3]:l.get(h));l.set(aW,s?s[4]:l.get(h));l.set(aG,s?s[5]:1);var t=__jrrdv,g=t[X]>0?t[0][S]("|"):null;l.set(j,g?g[1]:"direct");l.set(aE,g?g[2]:"-");l.set(f,g?g[3]:"none");l.set(aA,g?g[4]:"-");var y=__jrrdb,p=y[X]>0?y[0][S]("."):null;l.set(V,p?p[1]:0);l.set(e,aQ()||"-");return !0},aM=function(){var l=__jrrdb,g=l[X]>0?l[0][S]("."):null;return(g&&g.length==4)?g[1]*1:0},aN=function(bf){var s=ay.location.href,D=ay.referrer,bc=bf.get(R),C=A.util.getParameter(s,"utm_source"),t=[],aa=bf.get(j),W=bf.get(aE),O=bf.get(f),K=bf.get(aA),g=(__jrrdb.length==0);if(C){var l=A.util.getParameter(s,"utm_campaign"),be=A.util.getParameter(s,"utm_medium"),a6=A.util.getParameter(s,"utm_term");t[U](C);t[U](l||"-");t[U](be||"-");t[U](a6||"not set")}else{var p=D&&D[S]("/")[2],bd=false;if(p&&p[F](bc)<0){for(var a7=bf.get(aU),a9=0;a9<a7.length;a9++){var bb=a7[a9][S](":");if(p[F](bb[0][Q]())>-1&&D[F]((bb[1]+"=")[Q]())>-1){var y=/jrad_([0-9]{1,})/;var a8=y.exec(bf.get(j));if(a8!=null){bd=true;break}var ba=ak(bb[1][Q]()),L=A.util.getParameter(D,ba);t[U](bb[0]);t[U]("-");t[U]("organic");t[U](L||"not set");bd=true;break}}if(!bd){if(p[F]("zol.com.cn")>-1){t[U]("zol.com.cn");t[U]("-");t[U]("cpc");t[U]("not set")}else{t[U](p);t[U]("-");t[U]("referral");t[U]("-")}}}}if(t[X]>0&&((t[0]!=aa||t[1]!=W||t[2]!=O)||(g&&t[2]==="referral"))){bf.set(j,t[0]||"direct");bf.set(aE,t[1]||"-");bf.set(f,t[2]||"none");bf.set(aA,t[3]||"-");aC(bf)}else{if(g){aC(bf)}else{k(bf)}}},o=function(l,g){var p=g.split(".");l.set(m,p[2]);l.set(d,p[4]);l.set(aW,a2());l.m(aG);l.set(V,1)},M=function(l){var g=l.get(h);l.set(al,ae());l.set(m,g);l.set(d,g);l.set(aW,g);l.set(aG,1);l.set(V,1)},k=function(g){g.m(V)},B=function(g){return[g.get(aq),g.get(al)||"-",g.get(m)||"-",g.get(d)||"-",g.get(aW)||"-",g.get(aG)||1][N](".")},i=function(g){return[g.get(aq),g.get(V)||1,g.get(al)+"|"+g.get(aG)||1,g.get(aW)||g.get(h)][N](".")},G=function(g){return[g.get(aq),g.get(j)||ay.domain,g.get(aE)||"(direct)",g.get(f)||"direct",g.get(aA)||"-"][N]("|")},aC=function(g){var l=__jrrda;l.length==0?M(g):o(g,l[0])};var x=new aX(),aF=function(){this.a={};this.add=function(g,l){this.a[g]=l};this.get=function(g){return this.a[g]};this.toString=function(){return this.a[N]("&")}},ap=function(l,g){function s(y,t){t&&p[U](y+"="+t+";")}var p=[];s("__jrrda",B(l));s("__jrrdv",G(l));g.add("jdcc",p[N]("+"))},w=function(l,g){g.add("jdac",l.get(au)),g.add("jduid",l.get(al)),g.add("jdsid",l.get(al)+"|"+l.get(aG)),g.add("jdje",l.get(v)),g.add("jdsc",l.get(T)),g.add("jdsr",l.get(H)),g.add("jdul",l.get(aK)),g.add("jdcs",l.get(I)),g.add("jddt",l.get(at)||"-"),g.add("jdmr",aL(l.get(aH))),g.add("jdhn",l.get(ao)||"-"),g.add("jdfl",l.get(aY)),g.add("jdos",l.get(J)),g.add("jdbr",l.get(af)),g.add("jdbv",l.get(ab)),g.add("jdwb",l.get(m)),g.add("jdxb",l.get(d)),g.add("jdyb",l.get(aW)),g.add("jdzb",l.get(aG)),g.add("jdcb",l.get(V)),g.add("jdusc",l.get(j)||"direct"),g.add("jducp",l.get(aE)||"-"),g.add("jdumd",l.get(f)||"-"),g.add("jduct",l.get(aA)||"-"),g.add("jdlt",typeof jdpts!="object"?0:jdpts._st==undefined?0:l.get(a5)-jdpts._st),g.add("jdtad",l.get(e)),g.add("nav",l.get(n)||"-"),g.add("did",l.get(z)||"-"),g.add("qyu",l.get(q)||"-")},a0=function(l,g,p,s){g.add("jdac",l.get(au)),g.add("jduid",l.get(al)),g.add("jdsid",l.get(al)+"|"+l.get(aG)),g.add("jdje","-"),g.add("jdsc","-"),g.add("jdsr","-"),g.add("jdul","-"),g.add("jdcs","-"),g.add("jddt","-"),g.add("jdmr",aL(l.get(aH))),g.add("jdhn","-"),g.add("jdfl","-"),g.add("jdos","-"),g.add("jdbr","-"),g.add("jdbv","-"),g.add("jdwb","-"),g.add("jdxb","-"),g.add("jdyb","-"),g.add("jdzb",l.get(aG)),g.add("jdcb",s?(aM()+s):l.get(V)),g.add("jdusc","-"),g.add("jducp","-"),g.add("jdumd","-"),g.add("jduct","-"),g.add("jdlt",0),g.add("jdtad",p)},aZ=function(){aO(x)&&aN(x);a3(x);var l=new aF(),g=x.get(R);w(x,l);__jrrda=B(x);__jrrdb=i(x);__jrrdc=x.get(aq);__jrrdv=G(x);return l.a},aJ=function(){var g=new aF();w(x,g);return g.a},aV=function(g,l){var p=new aF();a0(x,p,g,l);return p.a},aD=function(l){if(l instanceof Array){var s="";for(var p=0,g=l.length;p<g;p++){s+=l[p]+((p==g-1)?"":"|||")}return s}return l};A.tracker={bloading:function(p,l,s){var g=aZ()}};A.tracker.bloading("J","A",new Date().getTime());$(document).bind("click",function(y){y=y||event;var W=document;var aa=y.srcElement||y.target;var C=$(aa).attr("clstag");var L="";while(!C){aa=aa.parentNode;if(!aa||(aa.nodeName=="BODY")){break}C=$(aa).attr("clstag")}if(C){var l=C.split("|"),g=l[1],K=l[2],O=l[3];if(aa.nodeName=="IMG"){aa=aa.parentNode}var p=$(aa).attr("href");if(p==undefined||p.indexOf("javascript")!=-1){p=""}else{if(p.indexOf("http")==-1){p=window.location.host+p}}switch(g){case"keycount":var D={page:K,location:O,tag:"Q",href:p};a4.req("www.130000",D);L=K+"|"+O;break}}});(function(){var g=function(){var l=aJ();var p={je:l.jdje,sc:l.jdsc,sr:l.jdsr,ul:l.jdul,cs:l.jdcs,dt:l.jddt,hn:l.jdhn,fl:l.jdfl,os:l.jdos,br:l.jdbr,bv:l.jdbv,wb:l.jdwb,xb:l.jdxb,yb:l.jdyb,zb:l.jdzb,cb:l.jdcb,usc:l.jdusc,ucp:l.jducp,umd:l.jdumd,uct:l.jduct,lt:l.jdlt,ct:new Date().getTime(),tad:l.jdtad,nav:l.nav,did:l.did,qyu:l.qyu};a4.visit();a4.req("www.110000",p)};g();if(!window.JRTRACKER){window.JRTRACKER={pvlog:g}}})()}try{var a=document.createElement("script");a.type="text/javascript";a.src="//jdjrflow.jd.com/h5ExposureStatistic.min.js";a.async="async";document.body.appendChild(a)}catch(b){console.log("注入曝光统计脚本错误  "+b.message)}})();
