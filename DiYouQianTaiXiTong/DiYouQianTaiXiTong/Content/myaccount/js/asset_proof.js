$(function() {
    /**
     * 弹窗-资产证明
     * assetsCommon.ui_popMidObj('弹窗ID');
     */
    $('#J_r-prove').click(function() {
        var enable = $("#enableProof").val();
        if(enable != '1')return;
        if($("#proofIdDiv").hasClass("i-input--warning")) $("#proofIdDiv").removeClass("i-input--warning");
        if($("#proofNameDiv").hasClass("i-input--warning")) $("#proofNameDiv").removeClass("i-input--warning");
        $("#proofName").val("");
        $("#proofIdNo").val("");
        assetsCommon.ui_popMidObj('#J_pop1');
        jQuery.ajax({
            url: "/async/isProofAble.action?_dc=" + new Date().getTime(),
            type: "POST",
            async: false,
            success: function (result) {
                if(result.userProofUsed == '1'){
                    $("#applyProofBtn").html('<a class="ui-button" href="javascript:verifyUser();">申请并下载</a>');
                }
                else {
                    $("#applyProofBtn").html('<a class="ui-button ui-button--disable" href="javascript:;">24小时内只限申请两次</a>');
                }
            }
        });
    });
});



function verifyUser(){
    if($("#proofIdDiv").hasClass("i-input--warning")) $("#proofIdDiv").removeClass("i-input--warning");
    if($("#proofNameDiv").hasClass("i-input--warning")) $("#proofNameDiv").removeClass("i-input--warning");
    var userName = $("#proofName").val();
    var idNo = $("#proofIdNo").val();
    if(userName == ''){
        $("#proofNameDiv").addClass('i-input--warning');
        return;
    }
    if(idNo == ''){
        $("#proofIdDiv").addClass('i-input--warning');
        return;
    }
    jQuery.ajax({
        url: "/async/proofUserVerify.action?_dc=" + new Date().getTime(),
        type: "POST",
        async: true,
        data:{
            proofName:userName,
            proofIdNo:idNo,
            type:0
        },
        success: function (result) {
            if (result.res == 'success'){
                //window.location.href=result.location;
                document.forms[1].action = "/async/exportAssetProof.action";
                document.forms[1].submit();
                $("#proofName").val("");
                $("#proofIdNo").val("");
                assetsCommon.ui_popMidClose('#J_pop1');
            }
            else if(result.res == 'nameErr'){
                $("#proofNameDiv").addClass('i-input--warning');
            }else if(result.res == 'idErr'){
                $("#proofIdDiv").addClass('i-input--warning');
                $("#proofIdNo").val("");
            }
        }
    });
}