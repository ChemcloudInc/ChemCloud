
$(function () {
    query();
    checkRoleName();
    checkEmail();
    checkPassword();
    $('#UserName').focus();
    //添加管理员
    $('.add-manager').click(function () {
        LoadAddBox();
    });
})
function checkRoleName() {
    var result = false;
    $('#UserName').focus(function () {
        $('#regName_info').show();
    }).blur(function () {
        var regName = $.trim($(this).val());
        if (regName == "用户名") {
            $('#regName_info').show();
        } else {
            $('#regName_info').hide();
            if ((checkUserNameIsValid()) && checkUserNameLenth()){
                result = true;
            }
        }
    });
    var regName = $.trim($('#UserName').val());
    if (regName == "用户名") {
        $('#regName_info').show();
    } else {
        $('#regName_info').hide();
        if ((checkUserNameIsValid()) && checkUserNameLenth()) {
            result = true;
        }
    }
    return result;
}
function checkEmail() {
    var result = true;
    var errorLabel = $('#regEmail_error');
    var reg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    $('#Email').focus(function () {
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
                if (checkRoleNameIsValid()) {
                    result = true;
                }
            }
        }
    });
    return result; 
    
}
function checkUserNameIsValid() {
    var errorLabel = $('#regName_error');
    var userName = $('#UserName').val();
    var result = false;
    $.ajax({
        type: "post",
        url: 'IsExistsUserName',
        data: { userName: userName },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.success) {
                if (data.result) {
                    errorLabel.html('用户名 ' + userName + ' 已经被占用').show();
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
function checkRoleNameIsValid() {
    var errorLabel = $('#regEmail_error');
    var result = false;
    var Email = $('#Email').val();
    $.ajax({
        type: "post",
        url: 'CheckEmail',
        data: { email: Email },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.success) {
                if (data.result) {
                    errorLabel.html('邮箱' + Email + ' 已经被占用').show();
                }
                else {
                    errorLabel.hide();
                    result = true;
                }
            }
            
        }
    });
    return result;
}
function checkValid() {
    return checkRoleName() && checkEmail();
}
function Delete(id) {
    $.dialog.confirm('确定删除该条记录吗？', function () {
        var loading = showLoading();
        $.post("./Delete", { id: id }, function (data) {
            loading.close();
            $.dialog.tips(data.msg); query()
        });
    });
}
function BatchDelete() {
    var selectedRows = $("#list").hiMallDatagrid("getSelections");
    var selectids = new Array();
    for (var i = 0; i < selectedRows.length; i++) {
        selectids.push(selectedRows[i].Id);
    }
    if (selectedRows.length == 0) {
        $.dialog.tips("你没有选择任何选项！");
    }
    else {
        $.dialog.confirm('确定删除选择的子账号吗？', function () {
            var loading = showLoading();
            $.post("./BatchDelete", { ids: selectids.join(',') }, function (data) {
                loading.close();
                $.dialog.tips(data.msg); query()
            });
        });
    }
}
function checkPasswordIsValid() {
    var result = false;

    //var reg = /^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{6,22}$/;
    var pwdTextBox = $('#PassWord');
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
    var pwdRepeatTextBox = $('#confirmPassWord');
    var repeatPassword = pwdRepeatTextBox.val(), password = $('#PassWord').val();
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
function checkPassword() {

    $('#PassWord').focus(function () {
        $('#pwd_info').show();
        $('#pwd_error').removeClass('error').addClass('focus').hide();
    }).blur(function () {
        $('#pwd_info').hide();
        checkPasswordIsValid();
    });

    $('#confirmPassWord').focus(function () {
        $('#pwdRepeat_info').show();
        $('#pwdRepeat_error').removeClass('error').addClass('focus').hide();

    }).blur(function () {
        $('#pwdRepeat_info').hide();
        checkRepeatPasswordIsValid();
    });

}
function checkUserNameLenth() {
    var result = false;
    var username = username = $('#UserName').val();
    if (username.length < 20) {
        result = true;
    } else {
        $.dialog.errorTips("用户名不能多于20个字符");
    }
    return result;
}
function query() {
    $("#list").hiMallDatagrid({
        url: './list',
        nowrap: false,
        rownumbers: true,
        NoDataMsg: '没有找到符合条件的数据',
        border: false,
        fit: true,
        fitColumns: true,
        pagination: true,
        idField: "Id",
        pageSize: 10,
        pageNumber: 1,
        queryParams: {},
        toolbar: /*"#goods-datagrid-toolbar",*/'',
        operationButtons: "#batchOperate",
        columns:
        [[
            { checkbox: true, width: 39 },
            { field: "Id", hidden: true },
            { field: "UserName", title: '管理员' },
            { field: "CreateDate", title: '创建日期' },
            { field: "RoleName", title: '权限组' },
        {
            field: "operation", operation: true, title: "操作",
            formatter: function (value, row, index) {
                var id = row.Id.toString();
                var roleid = row.RoleId.toString();
                var username = row.UserName;
                var realname = row.realName;
                var remark = row.reMark;
                var email = row.Emails;
                var model = JSON.stringify({ id: id,email:email,roleid: roleid, username: username, realname: realname, remark: remark })
                var html = ["<span class=\"btn-a\">"];
                //if (roleid == 0) {
                //    html.push("<a onclick='Change(" + model + ");'>修改</a>");
                //    html.push("<a onclick=\"Delete('" + id + "');\">删除</a>");
                //}
                html.push("<a onclick='Change(" + model + ");'>修改</a>");
                html.push("<a onclick=\"Delete('" + id + "');\">删除</a>");

                html.push("</span>");
                return html.join("");
            }
        }
        ]]
    });
}

function LoadRoleList(callback) {
    if ($("#RoleId option").length > 0) {
        callback();
        return;
    }
    var loading = showLoading();
    var result = false;
    $.ajax({
        type: 'post',
        url: 'RoleList',
        cache: false,
        async: true,
        data: {},
        dataType: "json",
        success: function (data) {
            loading.close();
            $(data).each(function (index, item) { $("#RoleId").append("<option value=" + item.Id + ">" + item.RoleName + "</option>") });
            callback();
        },
        error: function () {
            loading.close();
        }
    });
}


function Change(model) {
    var id = model.id;
    var username = model.username;
    var realname = model.realname;
    var remark = model.remark;
    var roleid = model.roleid;
    var email = model.email;
    LoadRoleList(function () {
        $("#UserName").val(username).attr("disabled", true);
        $("#div1").hide();
        $("#div2").hide();
        $("#Email").val(email).attr("disabled", true);
        $("#name-prefix").text("");
        $("#realName").val(realname);
        $("#reMark").val(remark);
        if (roleid != 0) {
            $("#RoleId").val(roleid);
            $("#roleGroupDiv").show();
        }
        else {
            $("#roleGroupDiv").hide();
        }
    });

    $.dialog({
        title: '修改子账户',
        lock: true,
        id: 'editManager',
        width: '840px',
        content: document.getElementById("addManagerform"),
        padding: '20px 10px',
        okVal: '确定',
        init: function () { $("#UserName").focus(); },
        ok: function () {
            var realName = $("#realName").val();
            var reMark = $("#reMark").val();
            var userName = $("#UserName").val();
            var email = $("#Email").val();
            var SelectedRoleId = $("#RoleId").val();
            var loading = showLoading();
            if (SelectedRoleId == null)
                SelectedRoleId = 0;
            $.post("Change",
                { id: id, email: email, roleid: SelectedRoleId, realName: realName, reMark: reMark },
                function (data) {
                    loading.close();
                    if (data.success) {
                        $.dialog.tips("修改成功", function () {
                            //if (roleid != 0 && roleid != SelectedRoleId)
                            query();
                        });
                        
                    }
                    else
                        $.dialog.tips("修改失败:" + data.msg);
                });
        }
    });
}

function LoadAddBox() {
    LoadRoleList(function () {
        $("#UserNameDiv").show();
        $("#name-prefix").text(mainUserName + ":");
        $("#UserName").val("").removeAttr("disabled");
        $("#Email").val("").removeAttr("disabled");
        $("#roleGroupDiv").show();
        $("#PassWord").val("");
        $("#RoleId").val("");
        $("#realName").val("");
        $("#reMark").val("");
        $("#div1").show();
        $("#div2").show();
    })
    $.dialog({
        title: '添加子帐号',
        id: 'addManager',
        width: '850px',
        content: document.getElementById("addManagerform"),
        lock: true,
        button: [{
            name: '确定添加', callback: function () {
                //add method
                var roleId = $("#RoleId").val();
                var username = $("#UserName").val();
                var password = $("#PassWord").val();
                var confirmPassWord = $("#confirmPassWord").val();
                var realName = $("#realName").val();
                var reMark = $("#reMark").val();
                var email = $("#Email").val();
                if (!checkPasswordIsValid())
                    return false;
                if (!checkRepeatPasswordIsValid())
                    return false;
                //this.disabled = false;
                if (!checkValid())
                    return false;
                //var object = generateRoleInfo();
                //var roleJson = JSON.stringify(object);
                AddManage(username, email, password, roleId, realName, reMark);
            }
           
            
        }]
        //init: function () { $("#UserName").focus(); }
        //,ok: function () {
            
        //}
    });
}

function AddManage(username, email,  password, roleid, realName, reMark) {
    var loading = showLoading();
    $.ajax({
        type: 'post',
        url: 'Add',
        cache: false,
        async: true,
        data: { UserName: username, PassWord: password, RoleId: roleid, realName: realName, reMark: reMark, email: email},
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.success) {
                $.dialog.tips("添加成功！");
                $("#addManagerform input").val("");
                setTimeout(3000);
                location.href = "/sellerAdmin/Manager/Management";
                //query();
            }
            else {
                $.dialog.tips("用户名或邮箱已存在，请重新输入！" + data.msg);
                setTimeout(3000);
            }
        }
    });
}


function CheckAdd(username, password) {
    var reg = /^[\u4E00-\u9FA5\@A-Za-z0-9\_\-]{4,20}$/;

    var regpwd = /^[^\s]{6,20}$/;
    var pwdOk = regpwd.test(password);
    if (username.length < 6) {
        $.dialog.tips("用户名不能小于6个字符");
        return false;
    }
    else if (!reg.test(username)) {
        $.dialog.tips('用户名需为6-20位字符，支持中英文、数字及"-"、"_"的组合');
        return false;
    }
    else if (!pwdOk) {
        $.dialog.tips("密码6-20个字符,不包含空格");
        return false;
    }
    else
        return true;

}
function generateRoleInfo() {
    //角色对象
    var role = {
        RoleName: $('#UserName').val(),
        RolePrivilegeInfo: []
    };
    var all = $("input[name='privilege']").length;
    var chkPrivileges = $("input[name='privilege']:checked");
    if (chkPrivileges.length == 0) {
        $.dialog.tips("请至少选择一个权限！");
        return;
    }
    else {
        if (chkPrivileges.length == all) {
            var PrivilegeInfo = {
                Privilege: null
            };
            PrivilegeInfo.Privilege = 0;
            role.RolePrivilegeInfo.push(PrivilegeInfo);
        }
        else {
            $(chkPrivileges).each(function (index, item) {
                var PrivilegeInfo = {
                    Privilege: null
                };
                PrivilegeInfo.Privilege = ($(item).val());
                role.RolePrivilegeInfo.push(PrivilegeInfo);
            });
        }
    }
    return role;
}

