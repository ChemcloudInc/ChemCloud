var eidtor;
$(function () {
    getSelectOptionList(0);
    $("#LanguageType").val($("#Language").val());//语言类型
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
        var LanguageType = $("#LanguageType option:selected").val();
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
            "Id":id,
            "Title": title,
            "LawsInfo": lawInfo,
            "Status": status,
            "Author": author,
            "LanguageType": LanguageType
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
                var languageType = "";
                for (var i = 0; i < json.List.length; i++) {

                    if (json.List[i].DictionaryTypeId == 10) {
                        languageType += '<option value="' + json.List[i].DValue + '"> ' + json.List[i].DKey + ' </option>';
                    }
                }
                if (type == 1) {
                    $("#LanguageType").html(languageType);//语言类型
                }
                else {
                    languageType = '<option value="0">所有</option>' + languageType;
                    $("#LanguageType").html(languageType);//货币类型
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
function UpLoadFile() {
    art.dialog({
        id: 'testID',
        content: '确定需要上传附件吗?',
        button: [
            {
                name: '确定',
                callback: function () {
                    var parentId = $("#Id").val();
                    var attachmentName = $("#Title").val();
                    var type = "edit";
                    window.location.href = "/Admin/LawInfo/EditUploadfile?parentId=" + parentId + '&attachmentName=' + attachmentName + '&type=' + type;
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
                        var attachmentName = $("#Title").val();
                        var type = "add";
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