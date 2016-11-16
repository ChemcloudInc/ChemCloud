$(function () {
    var amount = '';
    checkPermissionGroup();
    $('#AddPermission').click(function () {
        var result1 = checkFormatt();
        if (result1 == false) {
            return false;
        }
        var result = checkPermissionGroup();
        var rolename = $('#regName').val();
        if (result) {
            var loading = showLoading();
            $.ajax({
                type: "post",
                url: 'AddPermissionGroup',
                data: { RoleName: rolename },
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.success) {
                        if (result.roleId != '' && result.roleId != null) {
                            var jsonbill = '';
                            jsonbill += "{";
                            jsonbill += "\"_LimitedAmounts\":[";
                            $('#limitedAmount').find("input").each(function (i, item) {
                                if (this.name == "limitedAmount") {
                                    jsonbill += "{";
                                    jsonbill += "\"RoleId\":\"" + result.roleId + "\","
                                    jsonbill += "\"Money\":\"" + $(item).attr('value') + "\",";
                                } else if (this.name == "cointype") {
                                    jsonbill += "\"CoinType\":\"" + $(item).attr('value') + "\"";
                                    jsonbill += "},";
                                }

                            });
                            jsonbill = jsonbill.substring(0, jsonbill.length - 1);
                            jsonbill += "]}";
                            $.ajax({
                                type: "post",
                                url: 'AddLimitedAmount',
                                data: { json: jsonbill },
                                dataType: "json",
                                async: false,
                                success: function (result) {
                                    if (result.success) {
                                        loading.close();
                                        $.dialog.tips("添加成功！");
                                        setTimeout(function () {
                                            location.href = "./Management";
                                        }, 1000);
                                        //页面跳转
                                    }
                                },
                                error: function () {
                                    loading.close();
                                    setTimeout(function () { $.dialog.tips("添加失败！"); }, 1000);
                                }
                            });

                        }
                    }
                    else {
                        loading.close();
                        setTimeout(function () { $.dialog.tips("添加失败！"); }, 3000);
                    }
                }
            });
            
        }

    })
});
function checkPermissionGroup() {
    var result = false;
    var username = $.trim($('#regName').val());
    if (username.length < 20) {
        result = true;
    } else {
        $("#regName_error").html("角色组不能多于20个字符");
    }
    var reg = /^[\w\u4e00-\u9fa5\-_][\s\w\u4e00-\u9fa5\-_]*[\w\u4e00-\u9fa5\-_]$/;
    if (!reg.test(username)) {
        $("#regName_error").html('角色组名称只能输入中文、字母、数字、空格和下划线！').show();
        result = false;
    }
    return result;
}
function checkFormatt() {
    var result = true;
    $('#limitedAmount').find("input").each(function (i, item) {
        if (this.name == "limitedAmount") {
            var countval = $.trim($(this).val());
            var reg = /^[-+]?\d*$/;
            if (!reg.test(countval)) {
                $.dialog.tips("限制金额项目中只能输入数字");
                result = false;
            }
        }
    });
    return result;
}
