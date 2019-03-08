/**
 * 动态显示公告信息
 */
function dynamicShowNotice(address) {
	var content='';
	jQuery.ajax({
		url: "/async/browseNoticeControlData.action?address="+address+"&_dc=" + new Date().getTime(),
	    type: "GET",
	    async: false,
	    success: function (result) {
	    	content = result.noticeContent;
	    },
	    error:function(result){
	}
	});
	return content;
}