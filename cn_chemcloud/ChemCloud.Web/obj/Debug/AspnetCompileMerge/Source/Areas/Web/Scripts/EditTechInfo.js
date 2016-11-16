var eidtor;
$(function () {
    getSelectOptionList(1);
    $("#LanguageType").val($("#Language").val());//语言类型
    eidtor = UE.getEditor('techContent', { initialFrameHeight: 400, initialFrameWidth: 600 });
    (function initRichTextEditor() {
        eidtor = UE.getEditor('techContent');
        eidtor.addListener('contentChange', function () {
            $('#contentError').hide();
        });
    })();
    checkEmail();

    $('#Save').click(function () {
        var title = $.trim($("#techTitle").val());
        var author = $.trim($("#techAuthor").val());
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
        var id = $.trim($("#Id").val());
        var email = $.trim($("#techEmail").val());
        var content = UE.getEditor('techContent').getContent();
        var status = 1;
        var tel = $.trim($("#techTel").val());
        var LanguageType = $("#LanguageType option:selected").val();
        var param = {
            "Id": id,
            "Title": title,
            "TechContent": content,
            "Status": status,
            "Author": author,
            "Tel": tel,
            "Email": email,
            "LanguageType": LanguageType
        }

        var param2 = JSON.stringify(param);

        $.ajax({
            type: "POST",
            url: "UpdateTechInfo",
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
                        art.artDialog.succeedTips("保存成功！", "", "1.5"); //短暂提示 - 成功
                        location.href = './Management';
                    }
                } else {
                    var loading = showLoading();
                    loading.close();
                    art.artDialog.errorTips("保存失败！", "", "1.5"); //短暂提示 - 成功
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
        content: '确定需要上传附件吗?',
        button: [
            {
                name: '确定',
                callback: function () {
                    var parentId = $("#Id").val();
                    var attachmentName = $("#techTitle").val();
                    var type = "edit";
                    window.location.href = "/TechInfo/EditUploadfile?parentId=" + parentId + '&attachmentName=' + attachmentName + '&type=' + type;
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
        content: '确定需要上传附件吗?',
        button: [
            {
                name: '确定',
                callback: function () {
                    var parentId = $("#Id").val();
                    var attachmentName = $("#techTitle").val();
                    var type = "add";
                    window.location.href = "/TechInfo/Uploadfile?parentId=" + parentId + '&attachmentName=' + attachmentName + '&type=' + type;
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

function checkEmail() {
    var result = true;
    var errorLabel = $('#regEmail_error');
    var reg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var email = $.trim($(techEmail).val());
    if (!reg.test(email)) {
        errorLabel.html('请输入正确格式的电子邮箱').show();
        result = false;
    } else {
        $('#regEmail_info').hide();
        errorLabel.hide();
        result = true;
    }
    return result;


}
function checkTitleLenth() {
    var result = false;
    var username = username = $('#techTitle').val();
    if (username.length < 25) {
        result = true;
    } else {
        $.dialog.errorTips("标题不能多于25个中文字符");
    }
    return result;
}
function checkAuthorLenth() {
    var result = false;
    var username = username = $('#techAuthor').val();
    if (username.length < 15) {
        result = true;
    } else {
        $.dialog.errorTips("作者不能多于15个中文字符");
    }
    return result;
}
function checkValid() {
    return checkTitleLenth() && checkAuthorLenth() && checkEmail();
}


function getSelectOptionList(type) {
    $.ajax({
        type: "POST",
        url: "/SellerAdmin/Jobs/GetSelectOptionList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            }
            else {
                json = data;
            }
            if (json == null) {
                art.artDialog.errorTips("获取select内容失败，返回json数据为空！", "", "1");//短暂提示-错误
                return false;
            }
            if (!json.Msg.IsSuccess) {
                art.artDialog.errorTips(json.Msg.Message, "", "1");//短暂提示-错误
                return false;
            }
            else if (json.List.length > 0) {
                var workType = "";
                var typeOfCurrency = "";
                var payrollType = "";
                var languageType = "";

                for (var i = 0; i < json.List.length; i++) {
                    if (json.List[i].DictionaryTypeId == 1) {
                        typeOfCurrency += '<option value="' + json.List[i].DValue + '"> ' + json.List[i].Remarks + ' </option>';
                    }
                }
                if (type == 10) {
                    $("#LanguageType").html(typeOfCurrency);//货币类型
                }
            }
            else {
                art.artDialog.errorTips("select数据获取失败", "", "1");//短暂提示-错误
                return false;
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误", "", "1.5");//短暂提示-错误
            return false;
        }
    });
}
