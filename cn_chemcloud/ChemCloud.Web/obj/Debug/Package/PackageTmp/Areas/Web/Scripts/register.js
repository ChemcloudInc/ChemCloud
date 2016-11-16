
$(function () {
    bindCheckCode();

    //用户名
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

    checkPassword();

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


    checkCheckCode();

    $('#regName').focus();
});

//提交注册 采购商
$('#registsubmit').click(function () {
    $(this).attr('disabled', 'disabled');
    var registertype = "3"; //注册类型
    var result = checkValid();
    var parentId = 0;
    if (result) {
        var username = $('#regName').val(), password = $('#pwd').val();
        var regEmail = $('#regEmail').val();
        var introducer = $("#introducer").val();
        var loading = showLoading();
        $.post('/Register/RegisterUser', { username: username, password: password, email: regEmail, registertype: registertype, introducer: introducer }, function (data) {
            if (data.success) {
                $.post('/Register/SendMail', { username: username, email: regEmail }, function (result) {
                    if (result.success) {
                        location.href = '/Register/RegisterSuccessTip';
                    } else {
                        loading.close();
                        $.dialog.errorTips("邮件发送失败！" + data.msg);
                        $(this).removeAttr('disabled');
                    }
                });
            }
            else {
                loading.close();
                $.dialog.errorTips("注册失败！" + data.msg);
                $(this).removeAttr('disabled');
            }
        });
    } else {
        $(this).removeAttr('disabled');
    }
});

//提交注册 供应商
$('#registsubmit_').click(function () {
    $(this).attr('disabled', 'disabled');
    var registertype = "2"; //注册类型
    var result = checkValid();
    if (result) {
        var username = $('#regName').val(), password = $('#pwd').val();
        var regEmail = $('#regEmail').val();
        var introducer = $("#introducer").val();
        var loading = showLoading();
        $.post('/Register/RegisterUser1', { username: username, password: password, email: regEmail, registertype: registertype, introducer: introducer }, function (data) {
            if (data.success) {
                $.post('/Register/SendMail', { username: username, email: regEmail }, function (result) {
                    if (result.success) {
                        location.href = '/Register/RegisterSuccessTip';
                    } else {
                        loading.close();
                        $.dialog.errorTips("邮件发送失败！" + data.msg);
                        $(this).removeAttr('disabled');
                    }
                });
            }
            else {
                loading.close();
                $.dialog.errorTips("注册失败！" + data.msg);
                $(this).removeAttr('disabled');
            }
        });
    } else {
        $(this).removeAttr('disabled');
    }
});


//提交注册子账户
$('#registchildsubmit').click(function () {
    var result = checkValidChild();
    if (result) {
        var username = $('#regName').val(), password = $('#pwd').val(), email = $("#regEmail").val(), parentid = $('#introducer').val();
        var loading = showLoading();
        $.post('/Register/RegisterChild', { username: username, password: password, email: email, parentid: parentid }, function (data) {
            loading.close();
            if (data.success) {
                $.dialog.succeedTips("添加成功！", function () {

                }, 3);
            }
            else {
                $.dialog.errorTips("添加失败！" + data.msg);
            }
        });
    }
});

//校验邮箱格式
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

//检查密码
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

function bindCheckCode() {
    $('#checkCodeChangeBtn,#checkCodeImg').click(function () {
        var src = $('#checkCodeImg').attr('src');
        $('#checkCodeImg').attr('src', src);
    });
}


function checkValid() {
    return checkUserName() & checkPasswordIsValid() & checkCheckCodeIsValid() & checkAgreementIsValid() & checkemail() & checkRepeatPasswordIsValid();
}
function checkValidChild() {
    return checkUserName() & checkPasswordIsValid() & checkemail();
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

function checkCheckCode() {
    var errorLabel = $('#checkCode_error');
    $('#checkCode').focus(function () {
        errorLabel.hide();
    }).blur(function () {
        checkCheckCodeIsValid();
    });
}

//用户名唯一性校验
function checkUsernameIsValid() {
    var errorLabel = $('#regName_error');
    var result = false;
    var username = $.trim($('#regName').val());
    $.ajax({
        type: "post",
        url: "/register/CheckUserName",
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

function reloadImg() {
    $("#checkCodeImg").attr("src", "/Register/GetCheckCode?_t=" + Math.round(Math.random() * 10000));
}

function checkMobile() {
    $('#cellPhone').change(function () {
        var cellPhone = $.trim($(this).val());
        if (!cellPhone)
            $('#cellPhone_error').show();
        else
            $('#cellPhone_error').hide();
    }).focus(function () {
        $('#cellPhone_info').show();
        $('#cellPhone_error').hide();
    }).blur(function () {
        $('#cellPhone_info').hide();
        checkMobileIsValid();
    });
}

function checkMobileIsValid() {

    if ($('#cellPhone').length == 0) {
        return true;
    }
    var result = false;
    var cellPhone = $('#cellPhone').val();
    var errorLabel = $('#cellPhone_error');
    var reg = /^0?(13|15|18|14|17)[0-9]{9}$/;

    if (!cellPhone || cellPhone == '手机号码') {
        errorLabel.html('请输入手机号码').show();
    }
    else if (!reg.test(cellPhone)) {
        errorLabel.html('请输入正确的手机号码').show();
    }
    else {
        $.ajax({
            type: "post",
            url: "/register/CheckMobile",
            data: { mobile: cellPhone },
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.result == false) {
                    errorLabel.hide();
                    result = true;
                }
                else {
                    errorLabel.html('手机号码 ' + cellPhone + ' 已经被占用').show();
                }
            }
        });
    }
    return result;
}

var delayTime = 120;
var delayFlag = true;
function countDown() {
    delayTime--;
    $("#sendMobileCode").attr("disabled", "disabled");
    $("#dyMobileButton").html(delayTime + '秒后重新获取');
    if (delayTime == 1) {
        delayTime = 120;
        $("#mobileCodeSucMessage").removeClass().empty();
        $("#dyMobileButton").html("获取短信验证码");
        $("#cellPhone_error").addClass("hide");
        $("#sendMobileCode").removeClass().addClass("btn").removeAttr("disabled");
        delayFlag = true;
    } else {
        delayFlag = false;
        setTimeout(countDown, 1000);
    }
}

function sendMobileCode() {
    $('#cellPhone_error').hide();
    if ($("#sendMobileCode").attr("disabled")) {
        return;
    }
    var errorLabel = $('#cellPhone_error');
    var mobile = $("#cellPhone").val();
    var reg = /^0?(13|15|18|14|17)[0-9]{9}$/;
    if (!mobile) {
        $("#cellPhone_error").removeClass().addClass("error").html("请输入手机号");
        $("#cellPhone_error").show();
        return;
    }
    if (!reg.test(mobile)) {
        $("#cellPhone_error").removeClass().addClass("error").html("手机号码格式有误，请输入正确的手机号");
        $("#cellPhone_error").show();
        return;
    }
    //$('#checkCode').removeClass("highlight2");
    // 检测手机号码是否存在
    $.post('/Register/CheckMobile', { mobile: mobile }, function (data) {
        if (data.result == false) {
            errorLabel.hide();
            sendmCode();
        }
        else {
            errorLabel.html('手机号码 ' + mobile + ' 已经被占用').show();
        }
    });

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
//验证用户名长度
function checkUserNameLenth() {
    var result = false;
    var username = $.trim($('#regName').val());
    var errorLabel = $('#regName_error');
    if (username.length <= 20) {
        result = true;
    } else {
        errorLabel.html("用户名不能多于20个字母和数字").show();
    }
    var reg = /^[0-9a-zA-Z]+$/;
    if (!reg.test(username)) {
        errorLabel.html('用户名只能输入字母和数字！').show();
        result = false;
    }
    return result;
}


//屏蔽特殊字符
function ValidateValue(textbox) {
    var IllegalString = "\`~@#;,.!#$%^&*()+{}|\\:\"<>?-=/,\'";
    var textboxvalue = textbox.value;
    var index = textboxvalue.length - 1;
    var s = textbox.value.charAt(index);
    if (IllegalString.indexOf(s) >= 0) {
        s = textboxvalue.substring(0, index);
        textbox.value = s;
    }

}
// 手机注册发送验证码target
function sendmCode() {
    if ($("#sendMobileCode").attr("disabled") || delayFlag == false) {
        return;
    }

    $("#sendMobileCode").attr("disabled", "disabled");
    jQuery.ajax({
        type: "post",
        url: "/Register/SendCode?pluginId=ChemCloud.Plugin.Message.SMS&destination=" + $("#cellPhone").val(),
        success: function (result) {
            if (result.success == true) {
                $("#cellPhone_error").hide();
                $("#dyMobileButton").html("120秒后重新获取");
                //if (obj.remain) {
                //    $("#mobileCodeSucMessage").empty().html(obj.remain);
                //} else {
                //    $("#cellPhone_error").removeClass().empty().html("验证码已发送，请查收短信。");
                //    $("#cellPhone_error").show();
                //}

                setTimeout(countDown, 1000);
                $("#sendMobileCode").removeClass().addClass("btn").attr("disabled", "disabled");
                $("#checkCode").removeAttr("disabled");
            }
        }
    });
}
