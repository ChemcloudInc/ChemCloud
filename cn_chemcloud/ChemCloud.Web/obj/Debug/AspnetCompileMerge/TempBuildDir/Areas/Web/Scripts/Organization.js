function updateOrderOrName(actionName, param) {
    var loading = showLoading();
    ajaxRequest({
        type: 'post',
        url: "./" + actionName,
        param: param,
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.Successful == true) {
                $.dialog.tips('更新组织架构的' + (actionName == 'UpdateOrganization' ? '显示顺序' : '名称') + '成功.');
            }
            if ("UpdateOrganization" == actionName) {
                location.reload();
            }
        }, error: function () {
            loading.close();
        }
    });
}

function categoryTextEventBind() {
    var _order = 0;
    var _name = '';

    $('.box1 lh24').on('focus', '.text-name', function () {
        _name = parseInt($(this).val());
    });

    $('.box1 lh24').on('blur', '.text-name,', function () {
        var id = $(this).parent('td').find('.hidden_id').val();
        if ($(this).hasClass('text-name')) {
            if ($(this).val().length === 0) {
                $.dialog({
                    title: '更新组织架构',
                    lock: true,
                    width: '400px',
                    padding: '20px',
                    content: ['<div class="dialog-form">员工不能为空.</div>'].join(''),
                    button: [
				    {
				        name: '关闭',
				    }]
                });
                $(this).val(_name);
            } else {
                updateOrderOrName("UpdateOrganization", { id: id, name: $(this).val() });
            }
        }
    });
}


function initialBatchDelete() {
    $("#deleteBatch").click(function () {
        var ids = [];
        var eles = document.getElementsByClassName("caokai");
        $(eles).each(function () { //

            var curRow = $(this);

            if (curRow.find('input[type=checkbox]').attr('checked') == 'checked') {
                ids.push(curRow.find('input.hidden_id').val());
            }
        });
        if (ids.length == 0) { $.dialog.tips('不能批量删除,因为您没有选中任何员工账号.'); return; }
        $.dialog.confirm('确定删除选中的所有员工账号吗？', function () {
            var loading = showLoading();
            ajaxRequest({
                type: "POST",
                url: './BatchDeleteOrganization',
                param: { Ids: ids.join(',') },
                dataType: "json",
                success: function (success) {
                    loading.close();
                    if (success) {
                        location.href = "./Management";
                    } else {
                        $.dialog.errorTips(data.msg, _this);
                        setTimeout(3000);
                        location.href = "./Management";
                    }
                },
                error: function (e) {
                    loading.close();
                    $.dialog.errorTips("删除失败,请重试！", _this);
                }
            });
        });
    });
}


function Delete(Id) {
    $.dialog.confirm('确定删除该条记录吗？', function () {
        var loading = showLoading();
        $.post('./IsExitsOrganization', { Id: Id }, function (result) {
            loading.close();
            if (result.success) {
                $.dialog.errorTips("该角色组下有其它角色组，无法删除!");
                loading.close();
            }
            else {
                $.post('./IsAdmin', { Id: Id }, function (result) {
                    if (result.Successfly) {
                        $.dialog.errorTips("管理员角色无法删除!");
                        loading.close();
                    } else {
                        loading.close();
                        ajaxRequest({
                            type: 'POST',
                            url: "./DeleteOrganization",
                            param: { Id: Id },
                            dataType: "json",
                            success: function (data) {
                                if (data.success) {
                                    location.href = "./Management";
                                } else {
                                    $.dialog.errorTips(data.msg);
                                    loading.close();
                                }
                            }, error: function () { }
                        });
                    }
                })
                

            }
        });
    });
}

function ajaxRequestForOrganizationTree(target, Id, url, layer) {
    var loading = showLoading();
    $.ajax({
        type: 'post',
        url: url,
        cache: false,
        data: { id: Id },
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.Successfly === true) {
                var p = $(target).parents('.level-' + layer);
                if (data.Organization.length == 0) {
                    $(target).removeClass("glyphicon-plus-sign");
                    return;
                }
                for (var i = 0; i < data.Organization.length; i++) {
                    $(target).addClass('glyphicon-minus-sign').removeClass('glyphicon-plus-sign');
                    var left = layer == 1 ? 5 : (layer - 1) * 50;
                    var pix = '├───';
                    var className = "invisible";
                    var sub = ['<tr class="level-' + (layer + 1) + ' caokai">',
                        '<td class="td-choose"><input type="checkbox" name=""/></td>',
                        '<td><s class="line" style="margin-left:' + left + 'px">' + pix + '</s>'];
                    sub.push('<span class="glyphicon glyphicon-plus-sign" onclick="childrenClick(this,' + layer + ')"></span>');
                    sub.push('<input class="hidden_id" type="hidden" value="' + data.Organization[i].Id + '">');
                    var str = data.Organization[i].UserName+"("+data.Organization[i].RoleName + ")";
                    sub.push('<input class="text-name" type="text" value="' + str + '" style="width:200px" disabled="disabled"/>');
                    sub.push('<td class="td-operate">');
                    sub.push('<span class="btn-a">');
                    sub.push('</span>');
                    sub.push('</td>');
                    sub.push('</tr>');
                    p.after(sub.join(''));
                }
                //var elems = p.parent().find(".level-" + (layer + 1)).find(".glyphicon-plus-sign");
                //for (var i = 0; i < elems.length; i++) {
                //    $(elems[i]).click();
                //}
            }
            flag = true;
        },
        error: function () {

        }
    });
};
$(function () {

    //新增分类
    categoryTextEventBind();
    //dialogInitial();
    //initialdeleteCategory();
    $('.check-all').click(function () {
        var checkbox = $('.table').find('input[type=checkbox]');
        if (this.checked) {
            checkbox.each(function () { this.checked = true })
        } else {
            checkbox.each(function () { this.checked = false })
        }
    });


    $('.level-1 .glyphicon').click(function () {
        var p = $(this).parents('.level-1');
        if ($(this).hasClass('glyphicon-plus-sign')) {
            var Id = $(this).next('input').val();
            var url = "./GetOrganization";
            ajaxRequestForOrganizationTree(this, Id, url, 1);
        } else {
            $(this).removeClass('glyphicon-minus-sign').addClass('glyphicon-plus-sign');
            p.nextUntil('.level-1').remove();
        }
    });

    //$('.level-1 .glyphicon').click();

    $('#AddOrg').click(function () {
        location.href = "./AddNewOrg";
    });
    $('#AddByOrg').click(function () {
        var ids = [];
        var eles = document.getElementsByClassName("caokai");
        $(eles).each(function () { //

            var curRow = $(this);

            if (curRow.find('input[type=checkbox]').attr('checked') == 'checked') {
                ids.push(curRow.find('input.hidden_id').val());
            }
        });
        if (ids.length == 1)
        {
            var Id = ids.toString();
            location.href = "./AddByParent?Id=" + Id;
        }else if(ids.length > 1){
            var loading = showLoading();
            $.dialog.errorTips("只能选择一个操作对象");
            loading.close();
        }else if(ids.length == 0){
            var loading = showLoading();
            $.dialog.errorTips("请先选择一个操作对象");
            loading.close();
        }
        
    });
    $('#EditOrg').click(function () {
        var ids = [];
        var eles = document.getElementsByClassName("caokai");
        $(eles).each(function () { //

            var curRow = $(this);

            if (curRow.find('input[type=checkbox]').attr('checked') == 'checked') {
                ids.push(curRow.find('input.hidden_id').val());
            }
        });
        if (ids.length == 1) {
            var Id = ids.toString();
            location.href = "./Edit?Id=" + Id;
        } else if (ids.length > 1) {
            var loading = showLoading();
            $.dialog.errorTips("只能选择一个操作对象");
            loading.close();
        } else if (ids.length == 0) {
            var loading = showLoading();
            $.dialog.errorTips("请先选择一个操作对象");
            loading.close();
        }
    });
    $('#DeleOrg').click(function () {
        var ids = [];
        var eles = document.getElementsByClassName("caokai");
        $(eles).each(function () { //
            var curRow = $(this);
            if (curRow.find('input[type=checkbox]').attr('checked') == 'checked') {
                ids.push(curRow.find('input.hidden_id').val());
            }
        });
        if (ids.length == 1) {
            var Id = ids.toString();
            Delete(Id);
        } else if (ids.length > 1) {
            var loading = showLoading();
            $.dialog.errorTips("只能选择一个操作对象");
            loading.close();
        } else if (ids.length == 0) {
            var loading = showLoading();
            $.dialog.errorTips("请先选择一个操作对象");
            loading.close();
        }
    });
});


function childrenClick(a, layer) {
    var p = $(a).parents('.level-' + (layer + 1));
    if ($(a).hasClass('glyphicon-plus-sign')) {
        var category = $(a).next('input').val();
        var url = "./GetOrganization";
        ajaxRequestForOrganizationTree(a, category, url, layer + 1);
    } else {
        $(a).removeClass('glyphicon-minus-sign').addClass('glyphicon-plus-sign');
        p.nextUntil('.level-' + (layer + 1)).remove();
        
    }
}
