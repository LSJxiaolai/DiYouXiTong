/**
 * Created by ryoleev on 2014/12/17 0017.
 */

function eyeClick() {
    try {
        var userMsg = $("#userMsg").val();
        if (userMsg != null && userMsg != '') {
            userMsg = "eyeFlag_ryo_" + userMsg;
            var value = $.cookie(userMsg);
            if (value == '1') {
                value = '0';
            }
            else value = '1';
            saveToCookie(userMsg, value);
        }
    } catch (e) {

    }
}
function saveToCookie(userMsg, value){
    $.cookie(userMsg, value, {expires: 31,path:'/'});
}

function eyeInitOverview() {
    try {
        var userMsg = $("#userMsg").val();
        if (userMsg != null && userMsg != '') {
            userMsg = "eyeFlag_ryo_" + userMsg;
            var value = $.cookie(userMsg);
            if (value != null && value != '') {
                if (value == '1') {
                	assetsIndex.richEyeHide();
                	showOverviewTop(1);
                }
                if (value == '0') showOverviewTop(0);
            }else showOverviewTop(0);
        }
    } catch (e) {
        showOverviewTop();
    }
}

function eyeInitFinance() {
    try {
        var userMsg = $("#userMsg").val();
        if (userMsg != null && userMsg != '') {
            userMsg = "eyeFlag_ryo_" + userMsg;
            var value = $.cookie(userMsg);
            if (value != null && value != '') {
                if (value == '1') {
                    assetsMyMoney.richEyeHide();
                    showMinorTop();
                }
                if (value == '0') showFinanceTop();
            }
            else  showFinanceTop();
        }
    } catch (e) {

    }
}

function eyeInitGold() {
    try {
        var userMsg = $("#userMsg").val();
        if (userMsg != null && userMsg != '') {
            userMsg = "eyeFlag_ryo_" + userMsg;
            var value = $.cookie(userMsg);
            if (value != null && value != '') {
                if (value == '1') assetsMyGold.richEyeHide();
                if (value == '0') showMyXjkTop();
            }else{
                showMyXjkTop();
            }
        }
    } catch (e) {

    }
}

//理财首页小金库余额 0：展示 1：隐藏
function eyeInitXJKAccountAmount() {
    try {
        var userMsg = $("#userMsg").val();
        if (userMsg != null && userMsg != '') {
            userMsg = "eyeFlag_ryo_" + userMsg;
            var value = $.cookie(userMsg);
            if (value != null && value != '') {
                if (value == '1') {
                    $("#xjk-eye").attr("class","eye-icon close-eyes");
                    $("#userXJKAmount").find(".asset-money").text("***");
                }
                if (value == '0') showXJKAccountAmount(0);
            }else{
                showXJKAccountAmount(0);
            }
        }else{
            showXJKAccountAmount(0);
        }
    } catch (e) {
        showXJKAccountAmount(0);
    }
}

function getEyeStatus() {
    var userMsg = $("#userMsg").val();
    if (userMsg != null && userMsg != '') {
        userMsg = "eyeFlag_ryo_" + userMsg;
        var value = $.cookie(userMsg);
        if (value != null && value != '' && value == '1') return '1';
        else return 0;
    }
    else return 0;
}