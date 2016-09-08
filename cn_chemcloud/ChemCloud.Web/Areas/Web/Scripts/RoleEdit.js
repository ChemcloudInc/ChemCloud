$(function () {
    checkPermissionGroup();
    $('#EditPermission').click(function () {
        var result = checkPermissionGroup();

        var rolename = $('#regName').val();
        var Id = $('#roleId').val();
        if (result) {
            var loading = showLoading();
            $.post('./UpdatePermissionGroup', { Id : Id, roleName: rolename }, function (result) {
                loading.close();
                if (result.success) {
                    var jsonbill = '';
                    jsonbill += "{";
                    jsonbill += "\"_LimitedAmounts\":[";
                    $('#limitedAmount').find("input").each(function (i, item) {
                        if (this.name == "LimitedId") {
                            jsonbill += "{";
                            jsonbill += "\"Id\":\"" + $(item).attr('value') + "\",";
                        } else if (this.name == "LimitedRoleId") {
                            jsonbill += "\"RoleId\":\"" + $(item).attr('value') + "\",";
                        } else if (this.name == "limitedAmount") {
                            jsonbill += "\"Money\":\"" + $(item).attr('value') + "\",";
                        }
                        else if (this.name == "cointype") {
                            jsonbill += "\"CoinType\":\"" + $(item).attr('value') + "\"";
                            jsonbill += "},";
                        }
                    });
                    jsonbill = jsonbill.substring(0, jsonbill.length - 1);
                    jsonbill += "]}";
                    $.post('./UpdateLimitedAmount', { json: jsonbill }, function (result) {
                        loading.close();
                        if (result.success) {
                            loading.close();
                            setTimeout(function () { $.dialog.tips("保存成功！"); }, 3000);
                        }
                        else {
                            loading.close();
                            setTimeout(function () { $.dialog.tips("保存失败！"); }, 3000);
                        }
                    });
                }
                else {
                    loading.close();
                    $.dialog.errorTips(result.msg);
                    setTimeout(3000);
                }
                location.href = './Management';
            })
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
