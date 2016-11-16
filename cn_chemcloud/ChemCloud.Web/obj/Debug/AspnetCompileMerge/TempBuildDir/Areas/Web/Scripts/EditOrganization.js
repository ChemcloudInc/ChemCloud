$(function () {
    var parentroleId = '';
    var roleId = '';
    var parentId = '';
    
    //GetAllMember();
    $("#UpUserSelect").click(function () {
        parentroleId = $("#UpUserSelect option:selected").val();
        GetAllMember(parentroleId);
        //UpUserSelect 上级权限组名
    });
    $("#UserSelect").click(function () {
        roleId = $("#UserSelect").find("option:selected").val();
        //UserSelect 权限组名
    });
    
    $('#Clear').click(function () {
        $('#div3').css("display", "block");
        $('#div4').css("display", "none");
        GetAllPerson();
    });
    $('#ClearUp').click(function () {
        $('#div1').css("display", "block");
        $('#div2').css("display", "none");
        GetUpPerson(roleId);
    });
    $('#ClearUser').click(function () {
        $('#div5').css("display", "block");
        $('#div6').css("display", "none");
    });
    $('#EditOrg').click(function () {
        var orgId = $("#OrgId").val();
        var display = $('#div2').css('display');
        var display1 = $('#div4').css('display');
        var display2 = $('#div6').css('display');
        if (display == 'none') {
            var parentroleIds = parentroleId;
        }
        else {
            var parentroleIds = $("#ParentRoleId").val();
        }
        if (display1 == 'none') {
            var roleIds = roleId;
        }
        else {
            var roleIds = $("#roleId").val();
        }
        if (display2 == 'none') {
            var parentIds = $("#UpSelectList").find("option:selected").val();
        }
        else {
            var parentIds = $("#parentId").val();
        }
        var loading = showLoading();
        $.post('./UpdateOrganization', { Id: orgId, roleId: roleIds, ParentRoleId: parentroleIds, ParentId: parentIds }, function (result) {
            loading.close();
            if (result.success) {
                setTimeout(function () { $.dialog.tips("保存成功！"); }, 3000);
            }
            else {
                loading.close();
                setTimeout(function () { $.dialog.tips("保存失败！"); }, 3000);
            }
            location.href = './Management';
        });
    });
});

function GetUpPerson(roleId) {
    var userId = $("#masterId").val();
    $.post("./GetMember", { userId: userId }, function (result) {
        if (result != null) {
            if (result.data != null && result.data != "") {
                $.each(result.data, function (key, value) {
                    if (value.Username != $("#Name").val())
                    {
                        if (value.Id != roleId) {
                            $("#UpUserSelect").append($('<option>', { value: value.Id }).text(value.Username));
                        }
                    }
                    
                });
            }
        }
        else {
            var loading = showLoading();
            $.dialog.errorTips("您目前还没有子账号，请先添加子账号");
            setTimeout(3000);
            loading.close();
        }
    });
}
function GetAllPerson() {
    var userId = $("#masterId").val();
    $.post("./GetMember", { userId: userId }, function (result) {
        if (result != null) {
            if (result.data != null && result.data != "") {
                $.each(result.data, function (key, value) {
                    if (value.Username != "管理员" && value.Username != "admin" && value.Username != $("#Name").val()) 
                    {
                        $("#UserSelect").append($('<option>', { value: value.Id }).text(value.Username));
                    }
                    
                    
                });
            }
        }
        else {
            var loading = showLoading();
            $.dialog.errorTips("您目前还没有子账号，请先添加子账号");
            setTimeout(3000);
            loading.close();
        }
    });
}
function GetAllMember(roleId) {
    $.post("./GetAllMember", { roleId: roleId }, function (result) {
        if (result != null) {
            $("#UpSelectList").html("");
            if (result.data != null && result.data != "") {
                var str = "";
                $.each(result.data, function (key, value) {
                    str += "<option value=\"" + value.Id + "\">" + value.Username + "</option>";
                });
            }
            $("#UpSelectList").html(str);
            
        }
        else {
            var loading = showLoading();
            $.dialog.errorTips("您目前还没有子账号，请先添加子账号");
            setTimeout(3000);
            loading.close();
        }
    });
}


