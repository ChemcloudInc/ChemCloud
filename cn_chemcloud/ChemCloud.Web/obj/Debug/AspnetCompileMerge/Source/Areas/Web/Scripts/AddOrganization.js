$(function () {
    var roleId = '';
    var parentroleId = '';
    var parentId = '';
    var roleIds = '';
    $('#regName').focus(function () {
        var regName = $.trim($(this).val());
        if (regName == "用户名" || regName == "") {
            $('#regName_info').show();
        } else {
            $('#regName_info').hide();
        }
    }).blur(function () {
        var regName = $.trim($(this).val());
        if (regName == "用户名") {
            $('#regName_info').show();
        } else {
            $('#regName_info').hide();
            $('#regName_error').hide();
            if ((checkUsernameIsValid()) && checkUserNameLenth()) {
                return true;
            } else {
                return false;
            }
        }
    });
    $("#UpUserSelect").click(function () {
        //$('#UpUserSelectList').text().html("");
        //$('#UpUserSelectList').html("");
        parentroleId = $("#UpUserSelect").find("option:selected").val();
        GetAllMember(parentroleId);
    });
    $("#UserSelect").click(function () {
        roleId = $("#UserSelect").find("option:selected").val();
    });
    $("#UpUserSelectList").click(function () {
        parentId = $("#UpUserSelectList").find("option:selected").val();
    });
    //邮箱验证
    var errorLabel = $('#regEmail_error');
    var reg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    $('#regEmail').focus(function () {
        if ($.trim($('#regEmail_info')) != "") {
            $('#regEmail_info').hide();
        } else {
            $('#regEmail_info').show();
        }
    }).blur(function () {
        var email = $.trim($(this).val());
        if (!email) {
            errorLabel.html('请输入邮箱帐号').show();
            return false;
        } else {
            var email = $.trim($(this).val());
            if (!reg.test(email)) {
                errorLabel.html('请输入正确格式的电子邮箱').show();
                return false;
            } else {
                if (checkEmailIsExist()) {
                    errorLabel.hide();
                    $('#regEmail_info').hide();
                    return true;
                } else {
                    return false;
                }
            }
        }
    });
    checkPassword();

    checkCheckCode();

    $('#regName').focus();
    GetAllPerson();
    //GetAllMember();
    $("#AddOrganization").click(function () {
        var result = checkValidChild();
        if (result) {
            var loading = showLoading();
            var parentIds = $('#parentId').val();

            var username = $('#regName').val(), password = $('#pwd').val(), email = $("#regEmail").val();
            var parentroleIds = $('#UpUserSelect option:selected').val();
            var roleIds = $('#UserSelect option:selected').val();
            var parentIds = $('#UpUserSelectList option:selected').val();
            var firstname = $('#firstName').val();
            var secondname = $('#secondName').val();
            var loading = showLoading();
            $.ajax({
                type: "post",
                url: './AddMemberInfo',
                data: { username: username, password: password, email: email, parentIds: parentIds, firstName: firstname, secondName: secondname },
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.success) {
                        $.ajax({
                            type: "post",
                            url: './AddOrganization',
                            data: { username: username, roleId: roleIds, parentroleId: parentroleIds, parentId: parentIds },
                            dataType: "json",
                            async: false,
                            success: function (result) {
                                if (result.success) {
                                    loading.close();
                                    setTimeout(function () { $.dialog.tips("添加成功！"); }, 3000);
                                }
                                else {
                                    loading.close();
                                    setTimeout(function () { $.dialog.tips("添加失败！"); }, 3000);
                                }
                            }
                        });

                    }
                    else {
                        loading.close();
                        setTimeout(function () { $.dialog.tips(result.msg); }, 3000);
                    }
                    location.href = './Management';
                }
            });
            
        }

    });
});
function checkValidChild() {
    return checkUserName() & checkPasswordIsValid() & checkemail();
}
function GetAllPerson() {
    var userId = $("#UserId").val();
    $.post("./GetMember", { userId: userId }, function (result) {
        if (result != null) {
            if (result.data != null && result.data != "") {
                $.each(result.data, function (key, value) {
                    $("#UserSelect").append($('<option>', { value: value.Id }).text(value.Username));
                    $("#UpUserSelect").append($('<option>', { value: value.Id }).text(value.Username));
                });
            }
        }
        else {
            var loading = showLoading();
            $.dialog.errorTips("您目前还没有角色组，请先添加角色组");
            setTimeout(3000);
            loading.close();
        }
    });
}
function GetAllMember(roleId) {
    $.post("./GetAllMember", { roleId: roleId }, function (result) {
        if (result != null) {
            $("#UpUserSelectList").empty();
            if (result.data != null && result.data != "") {                
                var str = "";
                $.each(result.data, function (key, value) {
                    str += "<option value=\"" + value.Id + "\">" + value.Username + "</option>";
                });               
            }
            $("#UpUserSelectList").html(str);
        }
        else {
            var loading = showLoading();
            $.dialog.errorTips("您目前还没有子账号，请先添加子账号");
            setTimeout(3000);
            loading.close();
        }
    });
}
//用户名唯一性校验
function checkUsernameIsValid() {
    var errorLabel = $('#regName_error');
    var result = false;
    var username = $.trim($('#regName').val());
    $.ajax({
        type: "post",
        url: "/Organization/CheckUserName",
        data: { username: username },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.success) {
                if (data.result) {
                    errorLabel.html('用户名 ' + username + ' 已经被占用').show();
                }
                else {
                    errorLabel.hide();
                    result = true;
                }
            }
            else {
                $.dialog.errorTips("用户名校验出错", '', 1);
            }
        }
    });
    return result;
}
function checkRepeatPasswordIsValid() {
    var result = false;

    //var reg = /^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{6,22}$/;
    var pwdRepeatTextBox = $('#pwdRepeat');
    var repeatPassword = pwdRepeatTextBox.val(), password = $('#pwd').val();
    //var result = reg.test(password);

    var result = repeatPassword == password;

    if (!result) {
        $('#pwdRepeat_error').addClass('error').removeClass('focus').show();
    }
    else {
        $('#pwdRepeat_error').removeClass('error').addClass('focus').hide();
        result = true;
    }
    return result;
}
//验证用户名长度
function checkUserNameLenth() {
    var result = false;
    var username = $.trim($('#regName').val());
    if (username.length < 20) {
        result = true;
    } else {
        $("#regName_error").html("用户名不能多于20个字符");
    }

    var reg = /^[0-9a-zA-Z]+$/;
    if (!reg.test(username)) {
        $("#regName_error").html('用户名只能输入字母和数字！').show();
        result = false;
    }
    return result;
}
//检查用户名
function checkUserName() {
    var result = false;
    var regName = $.trim($('#regName').val());
    if (regName == "用户名") {
        $('#regName_info').show();
    } else {
        $('#regName_info').hide();
        $('#regName_error').hide();
        if ((checkUsernameIsValid()) && checkUserNameLenth()) {
            result = true;
        }
    }
    return result;
}
function checkPassword() {

    $('#pwd').focus(function () {
        $('#pwd_info').show();
        $('#pwd_error').removeClass('error').addClass('focus').hide();
    }).blur(function () {
        $('#pwd_info').hide();
        checkPasswordIsValid();
    });

    $('#pwdRepeat').focus(function () {
        $('#pwdRepeat_info').show();
        $('#pwdRepeat_error').removeClass('error').addClass('focus').hide();

    }).blur(function () {
        $('#pwdRepeat_info').hide();
        checkRepeatPasswordIsValid();
    });

}
//检验密码
function checkPasswordIsValid() {
    var result = false;

    //var reg = /^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{6,22}$/;
    var pwdTextBox = $('#pwd');
    var password = pwdTextBox.val();
    var reg = /^[^\s]{6,20}$/;
    var result = reg.test(password);
    //   var result = password.length >= 6 && password.length <= 20;

    if (!result) {
        $('#pwd_error').addClass('error').removeClass('focus').show();
    }
    else {
        $('#pwd_error').removeClass('error').addClass('focus').hide();
        result = true;
    }
    return result;
}
function checkemail() {
    var result = true;
    var errorLabel = $('#regEmail_error');
    var reg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    var email = $.trim($("#regEmail").val());
    if (!email) {
        $('#regEmail_info').show();
        result = false;
    } else {
        if (!reg.test(email)) {
            errorLabel.html('请输入正确格式的电子邮箱').show();
            result = false;
        } else {
            if (checkEmailIsExist()) {
                result = true;
                $('#regEmail_info').hide();
                errorLabel.hide();
            } else {
                result = false;
            }
        }
    }
    return result;
}
function checkEmailIsExist() {
    var errorLabel = $('#regEmail_error');
    var result = false;
    var email = $('#regEmail').val();
    $.ajax({
        type: "post",
        url: "/register/CheckEmail",
        data: { email: email },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.success) {
                if (data.result) {
                    errorLabel.html('邮箱' + email + ' 已经被占用').show();
                }
                else {
                    errorLabel.hide();
                    result = true;
                }
            }
            else {
                $.dialog.errorTips("邮箱校验出错", '', 1);
            }
        }
    });
    return result;
}
function bindCheckCode() {
    $('#checkCodeChangeBtn,#checkCodeImg').click(function () {
        var src = $('#checkCodeImg').attr('src');
        $('#checkCodeImg').attr('src', src);
    });
}
function reloadImg() {
    $("#checkCodeImg").attr("src", "/Register/GetCheckCode?_t=" + Math.round(Math.random() * 10000));
}
//验证码
function checkCheckCodeIsValid() {
    var checkCode = $('#checkCode').val();
    var errorLabel = $('#checkCode_error');
    checkCode = $.trim(checkCode);

    var result = false;
    if (checkCode && $('#cellPhone').length == 0) {
        $.ajax({
            type: "post",
            url: "/register/CheckCheckCode",
            data: { checkCode: checkCode },
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.success) {
                    if (data.result) {
                        errorLabel.hide();
                        result = true;
                    }
                    else {
                        errorLabel.html('验证码错误').show();
                    }
                }
                else {
                    $.dialog.errorTips("验证码校验出错", '', 1);
                }
            }
        });
    }
    else if ($('#cellPhone').length > 0 && checkCode) {
        $.ajax({
            type: "post",
            url: "/register/CheckCode",
            data: { pluginId: "ChemCloud.Plugin.Message.SMS", code: checkCode, destination: $("#cellPhone").val() },
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.success == true) {
                    errorLabel.hide();
                    result = true;
                }
                else {
                    errorLabel.html('验证码不正确或者已经超时').show();
                }
            }
        });
    }
    else {
        errorLabel.html('请输入验证码').show();
    }
    return result;
}

function checkAgreementIsValid() {
    var result = false;
    var errorLabel = $('#checkAgreement_error');
    if ($("#readme").attr("checked") == "checked") {
        errorLabel.hide();
        result = true;
    } else {
        errorLabel.html('请仔细阅读并同意以上协议').show();
    }
    return result;
}

function checkCheckCode() {
    var errorLabel = $('#checkCode_error');
    $('#checkCode').focus(function () {
        errorLabel.hide();
    }).blur(function () {
        checkCheckCodeIsValid();
    });
}


