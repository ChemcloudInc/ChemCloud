var eidtor;
$(function () {
    GetLanguage();
    eidtor = UE.getEditor('techContent', { initialFrameHeight: 400, initialFrameWidth: 600 });
    (function initRichTextEditor() {
        eidtor = UE.getEditor('techContent');
        eidtor.addListener('contentChange', function () {
            $('#contentError').hide();
        });
    })();
    checkTitle();
    checkEmail();
    checkAuthor();
    $("#AddTechInfo").click(function () {
        if (!checkValid())
            return false;
        var title = $("#techTitle").val();
        var techContent = UE.getEditor('techContent').getContent();
        var status = 1;
        var author = $("#techAuthor").val();
        var tel = $("#techTel").val();
        var email = $("#techEmail").val();
        var LanguageType = $("#LanguageType option:selected").val();
        var param = {
            "Title": title,
            "TechContent": techContent,
            "Status": status,
            "Author": author,
            "Tel": tel,
            "Email": email,
            "LanguageType": LanguageType
        }
        var param2 = JSON.stringify(param);
        $.ajax({
            type: "POST",
            url: "AddTechnicalInfo",
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
                        var attachmentName = $("#techTitle").val();
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
                    location.href = "/TechInfo/Uploadfile?parentId=" + parentId + '&attachmentName=' + attachmentName + '&type=' + type;

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
    $('#techTitle').focus(function () {
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
    var regName = $.trim($('#techTitle').val());
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
    $('#techAuthor').focus(function () {
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
    var regName = $.trim($('#techAuthor').val());
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
function checkEmail() {
    var result = true;
    var errorLabel = $('#regEmail_error');
    var reg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    $('#techEmail').focus(function () {
        $('#regEmail_info').show();
    }).blur(function () {
        var email = $.trim($(this).val());
        if (!email) {
            errorLabel.html('请输入邮箱帐号').show();
            result = false;
        }
        else {
            var email = $.trim($(this).val());
            if (!reg.test(email)) {
                errorLabel.html('请输入正确格式的电子邮箱').show();
                result = false;
            } else {
                $('#regEmail_info').hide();
                errorLabel.hide();
                result = true;
            }
        }
    });
    return result;


}
function checkTitleLenth() {
    var result = false;
    var username = username = $('#techTitle').val();
    if (username.length < 100) {
        result = true;
    } else {
        $.dialog.errorTips("标题不能多于50个中文字符");
    }
    return result;
}
function checkAuthorLenth() {
    var result = false;
    var username = username = $('#techAuthor').val();
    if (username.length < 25) {
        result = true;
    } else {
        $.dialog.errorTips("作者不能多于25个中文字符");
    }
    return result;
}
function checkValid() {
    return checkTitle() && checkEmail() && checkAuthor();
}