var eidtor;
$(function () {
    GetLanguage();
    eidtor = UE.getEditor('lawInfo', { initialFrameHeight: 400, initialFrameWidth: 600 });
    (function initRichTextEditor() {
        eidtor = UE.getEditor('lawInfo');
        eidtor.addListener('contentChange', function () {
            $('#contentError').hide();
        });
    })();
    checkTitle();
    checkAuthor();
    $("#Save").click(function () {
        if (!checkValid())
            return false;
        var title = $("#Title").val();
        var lawInfo = UE.getEditor('lawInfo').getContent();
        var status = 1;
        var author = $("#Author").val();
        var LanguageType = $("#LanguageType option:selected").val();
        var param = {
            "Title": title,
            "LawsInfo": lawInfo,
            "Status": status,
            "Author": author,
            "LanguageType": LanguageType
        }

        var param2 = JSON.stringify(param);
        $.ajax({
            type: "POST",
            url: "AddLawInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: param2,
            beforeSend: function () {
                if (LanguageType == 0) {
                    $.dialog.tips("请选择语言版本！");
                    return false;
                }
            },
            success: function (result) {
                if (result.success) {
                    if (result.parentId != "" && result.parentId != null) {
                        var parentId = result.parentId;
                        var attachmentName = $("#Title").val();
                        var type = "add";
                        UpLoadFile(parentId, attachmentName, type);
                    }
                } else {
                    var loading = showLoading();
                    loading.close();
                    $.dialog.tips("添加失败！");
                    setTimeout(3000);
                    location.href = './Management';
                }

            }
        });
        
    });
});
function GetLanguage() {
    $.post("./GetLanguage", function (result) {
        if (result != null) {
            if (result.data != null && result.data != "") {
                $.each(result.data, function (key, value) {
                    $("#LanguageType").append($('<option>', { value: value.Id }).text(value.Username));
                });
            }
        } else {

        }
    });
}
function UpLoadFile(parentId, attachmentName, type) {

    art.dialog({
        id: 'testID',
        content: '确定需要上传附件吗?',
        button: [
            {
                name: '确定',
                callback: function () {
                    location.href = "/Admin/LawInfo/Uploadfile?parentId=" + parentId + '&attachmentName=' + attachmentName + '&type=' + type;

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
function checkTitle() {
    var result = false;
    $('#Title').focus(function () {
        $('#regName_info').show();
    }).blur(function () {
        var regName = $.trim($(this).val());
        if (regName == "标题") {
            $('#regName_info').show();
        } else {
            $('#regName_info').hide();
            if (checkTitleLenth()) {
                result = true;
            }
        }
    });
    var regName = $.trim($('#Title').val());
    if (regName == "标题") {
        $('#regName_info').show();
    } else {
        $('#regName_info').hide();
        if (checkTitleLenth()) {
            result = true;
        }
    }
    return result;
}
function checkAuthor() {
    var result = false;
    $('#Author').focus(function () {
        $('#regAuthor_info').show();
    }).blur(function () {
        var regName = $.trim($(this).val());
        if (regName == "作者") {
            $('#regAuthor_info').show();
        } else {
            $('#regAuthor_info').hide();
            if (checkAuthorLenth()) {
                result = true;
            }
        }
    });
    var regName = $.trim($('#Author').val());
    if (regName == "作者") {
        $('#regAuthor_info').show();
    } else {
        $('#regAuthor_info').hide();
        if (checkAuthorLenth()) {
            result = true;
        }
    }
    return result;
}

function checkTitleLenth() {
    var result = false;
    var username = username = $('#Title').val();
    if (username.length < 50) {
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
    return checkTitle() && checkAuthor();
}