

$(function () {
    query();
    checkRoleName();//添加管理员
    checkPassword();
    $('#UserName').focus();
    $('.add-manager').click(function () {
        LoadAddBox();
    });
})
function IsSelf(id) {
    var result = false;
    $.post("./IsSelf", { id: id }, function (data) {
        if (data.success) {
            result = true;
        }
        return result;
    });
}
function Delete(id) {
    $.dialog.confirm('确定删除该条记录吗？', function () {
        var loading = showLoading();
        $.post("./Delete", { id: id }, function (data) { $.dialog.tips(data.msg); query(); loading.close(); });
    });
}
function ResetPwd(id) {
    $.dialog.confirm('确定重置密码吗？', function () {
        var loading = showLoading();
        $.post("./ResetPwd", { id: id }, function (data) { $.dialog.tips(data.msg); query(); loading.close(); });
    });
}
function BatchDelete() {
    var selectedRows = $("#list").hiMallDatagrid("getSelections");
    var selectids = new Array();
    for (var i = 0; i < selectedRows.length; i++) {
        selectids.push(selectedRows[i].Id);
    }
    if (selectedRows.length == 0) {
        $.dialog.errorTips("你没有选择任何选项！");
    }
    else {
        $.dialog.confirm('确定删除选择的管理员吗？', function () {
            var loading = showLoading();
            $.post("./BatchDelete", { ids: selectids.join(',') }, function (data) { $.dialog.tips(data.msg); query(); loading.close(); });
        });
    }
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
                var username = row.UserName.toString();

                var html = ["<span class=\"btn-a\">"];
                if (row.RoleName != "系统管理员") {
                    html.push("<a onclick=\"ChangePermission('" + id + "','" + username + "'," + roleid + ");\">修改</a>");
                }
                if (row.UserName == $("#userName").val()) {
                    html.push("<a onclick=\"ChangePassWord('" + id + "','" + username + "','" + roleid + "');\">修改密码</a>");
                }
                if (row.RoleId != 0) {
                    html.push("<a onclick=\"Delete('" + id + "');\">删除</a>");
                } if ($("#userRoleId").val() == 0) {
                    html.push("<a onclick=\"ResetPwd('" + id + "');\">重置密码</a>");
                }
                html.push("</span>");
                return html.join("");
            }
        }
        ]]
    });
}

function checkValid() {
    var result = false;
    if (checkPassWordValid() && checkRepeatPasswordIsValid()) {
        result = true;
    } else {
        result = false;
    }
    return result;
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

function ChangePermission(id, username, roleid) {
    $("#RoleId ").empty();
    $("#RoleId").find("option[value=" + roleid + "]").attr("selected", true);
    LoadRoleList(function () {
        $("#UserName").val(username).attr("disabled", true);
        $("#div1").attr("style", "display:none;")
        $("#div2").attr("style", "display:none;");
        $("#roleGroupDiv").attr("style", "display:block;");
       
        $("#RoleId").find("option[value=" + roleid + "]").attr("selected", true);
    });

    $.dialog({
        title: '修改权限组',
        lock: true,
        id: 'ChangePermission',
        width: '840px',
        content: document.getElementById("addManagerform"),
        okVal: '确定',
        init: function () {
            $("#RoleId").focus();
        },
        ok: function () {
            var SelectedRoleId = $("#RoleId option:selected").val();
            if (SelectedRoleId == null)
                SelectedRoleId = roleid;
            EditManage(id, SelectedRoleId);
        }
    });
}
function ChangePassWord(id, username, roleid) {
    LoadRoleList(function () {
        $("#UserName").val(username).attr("disabled", true);
        $("#PassWord").val("");
        $("#confirmPassWord").val("");
        $("#div1").attr("style", "display:block;")
        $("#div2").attr("style", "display:block;");
        $("#roleGroupDiv").attr("style", "display:none;");
    });

    $.dialog({
        title: '修改密码',
        lock: true,
        id: 'ChangePwd',
        width: '840px',
        content: document.getElementById("addManagerform"),
        okVal: '确定',
        init: function () {
            $("#PassWord").focus();
        },
        ok: function () {
            var password = $("#PassWord").val();
            if (password == null || password == "") {
                return false;
            }
            if (!checkValid())
                return false;
            EditPassword(id, password);
        }
    });
    //if (IsSelf(id)) {


    //}
}

function LoadAddBox() {
    LoadRoleList(function () {
        $("#UserNameDiv").show();
        $("#UserName").val("").removeAttr("disabled");
        $("#roleGroupDiv").show();
        $("#PassWord").val("");
        $("#confirmPassWord").val("");
        $("#RoleId").val("")
    })
    $.dialog({
        title: '添加管理员',
        id: 'addManager',
        width: '840px',
        content: document.getElementById("addManagerform"),
        lock: true,
        okVal: '确认添加',
        init: function () {
            $("#UserName").focus();
        },
        ok: function () {
            var roleId = $("#RoleId").val();
            var username = $("#UserName").val();
            var password = $("#PassWord").val();
            if (!checkValid())
                return false;
            if (!checkRoleName())
                return false;
            AddManage(username, password, roleId);
        }
    });
}
function EditManage(id, roleid) {
    var loading = showLoading();
    $.ajax({
        type: 'post',
        url: 'ChangeManager',
        cache: false,
        async: true,
        data: { id: id, roleid: roleid },
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.success) {
                $.dialog.succeedTips(data.msg);
                setTimeout(3000);
                $("#addManagerform input").val("");
                location.href = "/Admin/Manager/Management";
            }
            else {
                $.dialog.errorTips(data.msg);
                setTimeout(3000);
            }
        }

    });
}
function EditPassword(id, password) {
    var loading = showLoading();
    $.ajax({
        type: 'post',
        url: 'ChangePassWord',
        cache: false,
        async: true,
        data: { id: id, password: password },
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.success) {
                $.dialog.succeedTips(data.msg);
                setTimeout(3000);
                $("#addManagerform input").val("");
                location.href = "/Admin/Manager/Management";
            }
            else {
                $.dialog.errorTips(data.msg);
                setTimeout(3000);
            }
        }

    });
}
function AddManage(username, password, roleid) {
    var loading = showLoading();
    $.ajax({
        type: 'post',
        url: 'Add',
        cache: false,
        async: true,
        data: { UserName: username, PassWord: password, RoleId: roleid },
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.success) {
                $.dialog.tips("添加成功！");
                $("#addManagerform input").val("");
                setTimeout(3000);
                location.href = "./Management";
            }
            else {
                $.dialog.errorTips("用户名已存在，请重新输入！" + data.msg);
                setTimeout(3000);
            }
        }

    });
}

function checkRoleName() {
    var result = false;
    $('#UserName').focus(function () {
        $('#regName_info').show();
    }).blur(function () {
        var regName = $.trim($(this).val());
        if (regName == "用户名" || regName == "") {
            $('#regName_info').attr("style", "display:none;");
            //$("#regName_error");
            $('#regName_error').show();
        } else {
            $('#regName_info').hide();
            if ((checkUserNameIsValid()) && checkUserNameLenth()) {
                result = true;
            }
        }
    });
    var regName = $.trim($('#UserName').val());
    if (regName == "用户名" || regName == "") {
        $('#regName_info').hide();
        $('#regName_error').show();
    } else {
        $('#regName_info').hide();
        if ((checkUserNameIsValid()) && checkUserNameLenth()) {
            result = true;
        }
    }
    return result;
}
function checkPassWordValid() {
    var result = false;
    $('#confirmPassWord').focus(function () {
        $('#pwdRepeat_info').show();
    }).blur(function () {
        var regPwd = $.trim($("#confirmPassWord").val());
        var Pwd = $.trim($("#PassWord").val());
        if (regPwd == Pwd) {
            result = true;
        }
        else {
            $('#pwdRepeat_error').show();
            result = false;
        }
    });
    var regPwd = $.trim($("#confirmPassWord").val());
    var Pwd = $.trim($("#PassWord").val());
    if (regPwd == Pwd) {
        result = true;
    }
    else {
        $('#pwdRepeat_error').show();
        result = false;
    }
    return result;
}
function checkEmail() {
    var result = false;
    $('#Email').focus(function () {
        $('#Email').show();
    }).blur(function () {
        var Email = $.trim($(this).val());
        if (checkRoleNameIsValid()) {
            result = true;
        }
        if (result) {
            $.dialog.errorTips('邮箱 ' + Email + ' 已经存在');
            setTimeout(3000);
        }
    });
}
function checkUserNameLenth() {
    var result = false;
    var errorLabel = $('#regName_error');
    var username = $('#UserName').val();
    if (username.length < 20) {
        result = true;
    } else {
        errorLabel.html("用户名不能多于20个字符").show();
    }
    return result;
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
        $('#pwd_error').hide();
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
function CheckAdd(username, password) {
    var reg = /^[\u4E00-\u9FA5\@A-Za-z0-9\_\-]{4,20}$/;

    var regpwd = /^[^\s]{6,20}$/;
    var pwdOk = regpwd.test(password);
    if (username.length < 4) {
        $.dialog.tips("用户名不能小于4个字符");
        return false;
    }
    else if (!reg.test(username)) {
        $.dialog.tips('用户名需为4-20位字符，支持中英文、数字及"-"、"_"的组合');
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