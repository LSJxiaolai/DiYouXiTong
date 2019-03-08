var _loadMyYihaodianLeftMenu = {
    initPage: function () {
        var a = document.getElementsByTagName("head")[0]; var b = document.createElement("link"); b.href = "//img.yihaodianimg.com/myyhd/member/css/leftmenu.css"; b.rel = "stylesheet"; b.type = "text/css";
        //a.appendChild(b)
    }
}; _loadMyYihaodianLeftMenu.initPage(); LLW_Css(); function LLW_Css() { var a = getCookie("provinceId"); if (a == 2) { $("dl:eq(1)").find("dd").eq(0).find("a").css("float", "left"); $("dl:eq(1)").find("dd").eq(0).find("a").after("<span class='nav_label respect_card'></span>") } } function getCookie(a) { if (document.cookie.length > 0) { c_start = document.cookie.indexOf(a + "="); if (c_start != -1) { c_start = c_start + a.length + 1; c_end = document.cookie.indexOf(";", c_start); if (c_end == -1) { c_end = document.cookie.length } return unescape(document.cookie.substring(c_start, c_end)) } } return "" };