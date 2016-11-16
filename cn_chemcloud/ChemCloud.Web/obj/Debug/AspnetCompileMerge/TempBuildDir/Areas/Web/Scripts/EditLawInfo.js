var eidtor;
$(function () {
    eidtor = UE.getEditor('lawInfo', { initialFrameHeight: 400, initialFrameWidth: 600 });
    (function initRichTextEditor() {
        eidtor = UE.getEditor('lawInfo');
        eidtor.addListener('contentChange', function () {
            $('#contentError').hide();
        });
    })();
    $('#Save').click(function () {
        var id = $.trim($("#Id").val());
        var title = $.trim($("#Title").val());
        var author = $.trim($("#Author").val());
        var lawInfo = UE.getEditor('lawInfo').getContent();
        var status = 1;
        if (title == "" || title == null) {
            $.dialog.tips("标题不能为空！");
            return false;
        }
        if (author == "" || author == null) {
            $.dialog.tips("作者不能为空！");
            return false;
        }
        if (!checkValid())
            return false;
        var param = {
            "Id": id,
            "Title": title,
            "LawsInfo": lawInfo,
            "Status": status,
            "Author": author
        }

        var param2 = JSON.stringify(param);

        $.ajax({
            type: "POST",
            url: "UpdateLawInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: param2,
            beforeSend: function () {
            },
            success: function (result) {
                if (result.success) {
                    if (result.parentId != "" && result.parentId != null) {
                        var loading = showLoading();
                        loading.close();
                        $.dialog.tips("保存成功！");
                        setTimeout(3000);
                        location.href = './Management';
                    }
                } else {
                    var loading = showLoading();
                    loading.close();
                    $.dialog.tips("保存失败！");
                    setTimeout(3000);
                    location.href = './Management';
                }

            }
        });

    });
    $("#Uploadfile").click(function () {
        UpLoadFile();
    });
    $("#NewUploadFile").click(function () {
        NewUploadFile();
    });

});
function UpLoadFile() {
    art.dialog({
        id: 'testID',
        content: '确定需要编辑附件吗?',
        button: [
            {
                name: '确定',
                callback: function () {
                    var parentId = $("#Id").val();
                    var attachmentName = $("#Title").val();
                    var type = "edit";
                    location.href = "/LawInfo/EditUploadfile?parentId=" + parentId + '&attachmentName=' + attachmentName + '&type=' + type;
                }
            },
            {
                name: '取消',
                callback: function () {
                    location.href = './Management';
                }
            }
        ]
    });
    
}
function NewUploadFile() {
    art.dialog({
        id: 'testID',
        content: '确定需要添加附件吗?',
        button: [
            {
                name: '确定',
                callback: function () {
                    var parentId = $("#Id").val();
                    var attachmentName = $("#Title").val();
                    var type = "add";
                    location.href = "/LawInfo/Uploadfile?parentId=" + parentId + '&attachmentName=' + attachmentName + '&type=' + type;

                }
            },
            {
                name: '取消',
                callback: function () {
                    location.href = './Management';
                }
            }
        ]
    });
    
}
function checkTitleLenth() {
    var result = false;
    var username = username = $('#Title').val();
    if (username.length < 100) {
        result = true;
    } else {
        $.dialog.errorTips("标题不能多于100个中文字符");
    }
    return result;
}
function checkAuthorLenth() {
    var result = false;
    var username = username = $('#Author').val();
    if (username.length < 15) {
        result = true;
    } else {
        $.dialog.errorTips("作者不能多于15个中文字符");
    }
    return result;
}
function checkValid() {
    return checkTitleLenth() && checkAuthorLenth();
}