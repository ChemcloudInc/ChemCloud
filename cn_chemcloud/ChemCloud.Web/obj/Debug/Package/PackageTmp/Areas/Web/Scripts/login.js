
$(function () {

    /*输入框*/
    $('#loginname').focus();

    /*回车*/
    document.onkeydown = function (e) {
        if (e.keyCode == 13) {
            submit();
        }
    }

    /*登录*/
    $('#submit_login').click(function () {
        submit();
    });

    /*验证码*/
    bindCheckCode();

    /*记住用户名*/
    initUsenameBox();

});

/*登录提交事件*/
function submit() {
    $('#submit_login').attr("disabled", "disabled");
    var result = checkUsername() & checkPassword();
    if (result) {
        var username = $('#loginname').val();
        var password = $('#password').val();
        var checkCode = $('#checkbox1').val();
        var keep = $('#checkbox1').attr('checked');
        keep = keep ? true : false;
        $.post('/Login/Login', { username: username, password: password, checkCode: checkCode, keep: keep },
            function (data) {
                $('#submit_login').removeAttr("disabled");
                if (data.success) {//登录成功      
                    $("#checkCode_error").html("");
                    $.cookie('ChemCloud-DefaultUserName', username, { path: "/", expires: 365 });
                    /* 
                    var returnUrl = decodeURIComponent(QueryString('returnUrl')).replace('&amp;', '&');
                     if (returnUrl)
                         location.href = returnUrl;
                     else
                     */
                    if (data.IsChildSeller == true) {
                        var options = { path: "/" };
                        if ($('#autoLogin').attr('checked'))
                            options.expires = 365;
                        $.cookie('ChemCloud-SellerManager', data.userId, options);
                        $.cookie('ChemCloud-SellerUsername', username, { path: "/", expires: 365 });
                        location.href = "/sellerAdmin"; /*跳转至供应商中心*/
                    }
                    else {
                        location.href = '/userCenter'; /*跳转至采购商中心*/
                    }
                }
                else {
                    $("#checkCode_error").html(data.msg);
                    /*
                    var isFirstShowCheckcode = false;
                    refreshCheckCode();
                    if (data.errorTimes > data.minTimesWithoutCheckCode) {//需要验证码
                        if ($('#checkCodeArea').css('display') == 'none') {
                            isFirstShowCheckcode = true;
                            $('#checkCode_error').html(data.msg).show();
                        }
    
                        $('#checkCodeArea').show();
                        $('#autoentry').css('margin-top', 0);
                    }
                    else {
                        $('#checkCodeArea').hide();
                        $('#autoentry').removeAttr('style');
                    }
                    if (!isFirstShowCheckcode) {
                        $('#loginpwd_error').html(data.msg).show();
                        $('#password').focus();
                    }
                    else {
                        $('#checkCodeBox').focus();
                    }
                    */
                }
            });
    }
}

function checkCheckCode() {
    var result = false;
    if ($('#checkCodeArea').css('display') == 'none')
        result = true;
    else {
        var checkCode = $('#checkCodeBox').val();
        var errorLabel = $('#checkCode_error');
        if (checkCode && checkCode.length == 4) {
            $.ajax({
                type: "post",
                url: "/login/checkCode",
                data: { checkCode: checkCode },
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.success) {
                        result = true;
                        errorLabel.hide();
                    }
                    else {
                        $('#checkCodeBox').focus();
                        errorLabel.html('验证码错误').show();
                    }
                }
            });
        }
        else {
            $('#checkCodeBox').focus();
            if (!checkCode)
                errorLabel.html('请填写验证码').show();
            else
                errorLabel.html('验证码错误').show();
        }
    }
    return result;
}

function checkUsername() {
    var result = false;
    var username = $('#loginname').val();
    var loginError = $('#loginname_error');
    if (!username) {
        $("#checkCode_error").html("请输入用户名！");
    }
    else {
        result = true;
        loginError.hide();
    }
    return result;
}

function checkPassword() {
    var result = false;
    var password = $('#password').val();
    var passwordError = $('#loginpwd_error');
    if (!password) {
        $("#checkCode_error").html("请输入密码！");
    }
    else {
        result = true;
        passwordError.hide();
    }
    return result;
};

function refreshCheckCode() {
    var path = $('#checkCodeImg').attr('src').split('?')[0];
    path += '?time=' + new Date().getTime();
    $('#checkCodeImg').attr('src', path);
    $('#checkCodeBox').val('');
}

function bindCheckCode() {
    $('#checkCodeImg,#checkCodeChange').click(function () {
        refreshCheckCode();
    });
}

function initUsenameBox() {
    var defaultUsername = $.cookie('ChemCloud-DefaultUserName');
    if (defaultUsername) {
        $('#loginname').val(defaultUsername);
        $('#password').focus();
    }
}
