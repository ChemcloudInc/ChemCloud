$(document).ready(function () {
    getSelectOptionList(0);

    param = getParamSelect();
    Get_PageInfo(param);
    getBannersIndexList(param);

    pageCount = param.query.PageInfo.PageCount;//模拟后台总页数
    //生成分页按钮
    if (pageCount > 5) {
        page_icon(1, 5, 0);
    } else {
        page_icon(1, pageCount, 0);
    }

    //查询按钮
    $("#btn_Search").click(function () {
        get_List();
    });

    //点击分页按钮触发
    $("#pageGro li").live("click", function () {
        if (pageCount > 5) {
            var pageNum = parseInt($(this).html());//获取当前页数
            pageGroup(pageNum, pageCount);
        } else {
            $(this).addClass("on");
            $(this).siblings("li").removeClass("on");
        }
        param.query.PageInfo.CurrentPage = parseInt($(this).html());
        getBannersIndexList(param);
    });

    //点击上一页触发
    $("#pageGro #preSpanBtn").click(function () {

        var currentPage = 1;
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro li.on").html());//获取当前页
            pageUp(pageNum, pageCount);
            currentPage = pageNum;

        } else {
            var index = $("#pageGro ul li.on").index();//获取当前页
            if (index > 0) {
                $("#pageGro li").removeClass("on");//清除所有选中
                $("#pageGro ul li").eq(index - 1).addClass("on");//选中上一页
                currentPage = index;
            }
            else {
                art.artDialog.errorTips("已是第一页", "", "1");//短暂提示-错误
                return false;
            }
        }
        param.query.PageInfo.CurrentPage = currentPage;
        getBannersIndexList(param);
    });

    //点击下一页触发
    $("#pageGro #nextSpanBtn").click(function () {

        var currentPage = 1;
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro li.on").html());//获取当前页
            pageDown(pageNum, pageCount);
            currentPage = pageNum;
        } else {
            var index = $("#pageGro ul li.on").index();//获取当前页
            if (index + 1 < pageCount) {
                $("#pageGro li").removeClass("on");//清除所有选中
                $("#pageGro ul li").eq(index + 1).addClass("on");//选中下一页
                currentPage = index + 2;
            }
            else {
                art.artDialog.errorTips("已是第最后一页", "", "1");//短暂提示-错误
                return false;
            }
        }
        param.query.PageInfo.CurrentPage = currentPage;
        getBannersIndexList(param);
    });


    //新增按钮
    $("#btn_Add").click(function () {
        art.dialog({
            width: 500,
            height: 200,
            id: 'add_Dialog',
            title: '新增首页幻灯片信息',
            content: '<table id="table_AddJob">'
                    + '<tr><td>Banner标题：</td><td colspan="2"><input id="bannerTitle" type="text" /></td></tr>'
                    + '<tr><td>Banner图片：</td><td style="width:200px;"><form id="ajaxForm" method="post" enctype="multipart/form-data" ><input type="file" name="file" /></form></td><td><button id="btnUpdateImg" type="button" onclick="updateBtnClickFun()">上传</button></td></tr>'
                    + '<tr><td>语言类型：</td><td colspan="2">'
                    + '<select id="languageType" class="search_80_1">'
                    + '</select>'
                    + '</td></tr>'
                    + '<tr><td>链接地址：</td><td colspan="2"><input id="bannerUrl" type="text" /></td></tr>'
                    + '<tr><td></td><td colspan="2">例如：http://www.chemcloud.com 如果无连接，请输入“#”</td></tr>'
                    + '</table>',
            lock: true,
            fixed: true,
            ok: function () {
                var param = getParamAdd();

                var isTrue = getParamAddVel(param);
                if (isTrue) {
                    param = JSON.stringify(param)
                    span_AddFun(param);

                    var param = getParamSelect();
                    param.query.PageInfo.CurrentPage = 1;
                    getBannersIndexList(param);
                }
            },
            okValue: '提交',
            cancelValue: '取消',
            cancel: function () {
            }
        });
        getSelectOptionList(1);
    });
});


/*------------------------------------------  方法 start  -------------------------------------------*/

//-----------修改
function span_ModifyFun(obj) {
    var objSpan = $(obj);
    var param = getParamModify();
    param.Id = objSpan.attr("itemId");
    param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "./GetObjectById",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param2,
        beforeSend: function () {
        },
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            if (json != null) {
                open_ModifyDialog(json)
            }
            else {
                art.artDialog.errorTips("修改过程中查询信息失败：未获取任何信息", "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("（修改）系统错误！", "", "1.5");//短暂提示-错误
        }
    });
}
function open_ModifyDialog(json) {
    art.dialog({
        width: 500,
        height: 200,
        id: 'modify_Dialog',
        title: "修改：" + json.BannerTitle,
        content: '<table id="table_AddJob">'
        + '<tr><td>Banner标题：</td><td colspan="2"><input id="bannerTitle" type="text" value="' + json.BannerTitle + '" /></td></tr>'
        + '<tr><td>Banner图片：</td><td style="width:200px;"><form id="ajaxForm" method="post" enctype="multipart/form-data" ><input type="file" name="file" /></form></td><td><button id="btnUpdateImg" updateimgurl="' + json.BannerDes + '" type="button" onclick="updateBtnClickFun()">上传</button></td></tr>'
        + '<tr><td>语言类型：</td><td colspan="2">'
        + '<select id="languageType" class="search_80_1">'
        + '</select>'
        + '</td></tr>'
        + '<tr><td>链接地址：</td><td colspan="2"><input id="bannerUrl" type="text" value="' + json.Url + '" /></td></tr>'
        + '<tr><td></td><td colspan="2">例如：http://www.chemcloud.com 如果无连接，请输入“#”</td></tr>'
        + '</table>',
        lock: true,
        fixed: true,
        ok: function () {
            var param = getParamAdd();
            param.query.ParamInfo.Id = json.Id;
            param = JSON.stringify(param)
            modifyFun(param);

            var param = getParamSelect();
            param.query.PageInfo.CurrentPage = 1;
            getBannersIndexList(param);

        },
        okValue: '提交',
        cancelValue: '取消',
        cancel: function () {
            
        }
    });
    getSelectOptionList(1);
    $("#languageType").val(json.LanguageType);//语言类型
}
function modifyFun(param) {
    $.ajax({
        type: "POST",
        url: "./ModifyBannerIndex",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        beforeSend: function () {
        },
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            if (json.IsSuccess) {
                art.artDialog.succeedTips("修改成功！", "", "1.5"); //短暂提示 - 成功

                var param = getParamSelect();
                param.PageNo = 1;
                getBannersIndexList(param);
            }
            else {
                art.artDialog.errorTips(json.Message, "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("修改失败：系统错误！", "", "1.5");//短暂提示-错误
        }
    });
}

//删除
function span_DeleteFun(obj) {
    var param = getParamDel();
    var objSpan = $(obj);
    param.Id = objSpan.attr("itemid")
    param2 = JSON.stringify(param)

    $.ajax({
        type: "POST",
        url: "./DeleteById",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param2,
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            if (json.IsSuccess) {
                art.artDialog.succeedTips("删除成功！", "", "1.5"); //短暂提示 - 成功

                var param = getParamSelect();
                param.PageNo = 1;
                getBannersIndexList(param);
            }
            else {
                art.artDialog.errorTips(json.Message, "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("（删除）系统错误！", "", "1.5");//短暂提示-错误
        }
    });

}

//新增
function span_AddFun(param) {
    $.ajax({
        type: "POST",
        url: "./BannerAdd",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        beforeSend: function () {
        },
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            if (json.IsSuccess) {
                art.artDialog.succeedTips("新增成功！", "", "1.5"); //短暂提示 - 成功
                var param = getParamSelect();
                param.PageNo = 1;
                getBannersIndexList(param);
            }
            else {
                art.artDialog.errorTips(json.Message, "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("新增失败：系统错误！", "", "1.5");//短暂提示-错误
        }
    });
}

//根据查询条件获取列表
//根据查询条件获取列表
function get_List() {
    param = getParamSelect();//重新确定查询条件
    var isTrue = getParamSelectVel();
    if (isTrue) {
        Get_PageInfo(param);
        getBannersIndexList(param);
        pageCount = param.query.PageInfo.PageCount;//模拟后台总页数
        if (pageCount > 5) {
            page_icon(1, 5, 0);
        } else {
            page_icon(1, pageCount, 0);
        }
    }
    else {
        return false;
    }
}
function getBannersIndexList(param) {
    var param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "./GetBannerList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param1,
        beforeSend: function () {
        },
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            var tbody = $("#tbody-list");
            var trList = "";

            if (json == null) {
                art.artDialog.errorTips("返回json数据为空！", "", "1");//短暂提示-错误
                return false;
            }
            if (!json.Msg.IsSuccess) {
                art.artDialog.errorTips(json.Msg.Message, "", "1");//短暂提示-错误
                return false;
            }
            else if (json.List.length > 0) {
                for (var i = 0; i < json.List.length; i++) {
                    if (i % 2 == 0) {
                        trList += "<tr class='tr_1'><td class='u-name'>" + (i + 1) + "</td><td class='u-name'>" + json.List[i].BannerTitle + "</td><td>" + json.List[i].BannerDes + "</td><td>" + json.List[i].Url + "</td><td>" + json.List[i].LanguageType + "</td><td><span onclick='span_DeleteFun(this)' itemId='" + json.List[i].Id + "'>删除</span> <span onclick='span_ModifyFun(this)' itemId='" + json.List[i].Id + "'>修改</span></td></tr>";
                    }
                    else {
                        trList += "<tr class='tr_2'><td class='u-name'>" + (i + 1) + "</td><td class='u-name'>" + json.List[i].BannerTitle + "</td><td>" + json.List[i].BannerDes + "</td><td>" + json.List[i].Url + "</td><td>" + json.List[i].LanguageType + "</td><td><span onclick='span_DeleteFun(this)' itemId='" + json.List[i].Id + "'>删除</span> <span onclick='span_ModifyFun(this)' itemId='" + json.List[i].Id + "'>修改</span></td></tr>";
                    }
                }
                tbody.html(trList);
            }
            else {
                var trList = "<tr><td colspan='6'>无任何数据</td></tr>";
                tbody.html(trList);
                $("#currentSpanPage").html(1);

            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误", "", "1.5");//短暂提示-错误
        }
    });
}
function Get_PageInfo(param) {
    var paramJson = JSON.stringify(param);
    $.ajax({
        type: "POST",
        async: false,
        url: "./Get_PageInfo",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: paramJson,
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            if (json.Msg.IsSuccess) {

                if (json == null) {
                    art.artDialog.errorTips("返回json数据为空", "", "1.5");//短暂提示-错误
                    return false;
                }
                if (!json.Msg.IsSuccess) {
                    art.artDialog.errorTips("查询失败：系统错误1", "", "1.5");//短暂提示-错误
                    return false;
                }
                else if (json.Model != null) {
                    param.query.PageInfo.PageCount = json.Model.PageCount;
                }
                else {
                    art.artDialog.errorTips("无任何数据", "", "1.5");//短暂提示-错误
                    return false;
                }
            }
            else {
                art.artDialog.errorTips("查询失败：系统错误2", "", "1.5");//短暂提示-错误
                return false;
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误3", "", "1.5");//短暂提示-错误
            return false;
        }
    });
}
function getParamSelectVel() {

    //var message = "";
    //var payrolLow = $("#payrolLow").val();
    //var payrolHigh = $("#payrolHigh").val();

    //if (payrolLow.length == 0 && payrolHigh.length == 0) {

    //}
    //else if (payrolLow.length == 0) {
    //    param.query.ParamInfo.PayrolHigh = payrolHigh;
    //}
    //else if (payrolHigh.length == 0) {
    //    param.query.ParamInfo.PayrolLow = payrolLow;
    //}
    //else {
    //    if (isNaN(payrolLow)) {
    //        message = "最低薪资待遇格式不正确<br/>";
    //    }
    //    else if (isNaN(payrolHigh)) {
    //        message = "最高薪资待遇格式不正确<br/>";
    //    }
    //    else {
    //        if (Number(payrolHigh) < Number(payrolLow)) {
    //            message = "最高薪资待遇不能小于最低薪资待遇<br/>";
    //        }
    //        else {
    //            param.query.ParamInfo.PayrolLow = payrolLow;
    //            param.query.ParamInfo.PayrolHigh = payrolHigh;
    //        }
    //    }
    //}
    //if (message.length > 0) {
    //    art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
    //    return false;
    //}
    //else {
    return true;
    //}
}
function getParamAddVel(param) {
    var message = "";
    if (param.query.ParamInfo.BannerTitle.length == 0) {
        art.artDialog.errorTips("请输入Banner标题", "", "1.5");//短暂提示-错误
        return false;
    }
    else if (param.query.ParamInfo.BannerDes.length == 0) {
        art.artDialog.errorTips("请输入选择Banner图片", "", "1.5");//短暂提示-错误
        return false;
    }
    else if (param.query.ParamInfo.Url.length == 0) {
        art.artDialog.errorTips("请输入链接地址", "", "1.5");//短暂提示-错误
        return false;
    }

    if (message.length > 0) {
        art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
        return false;
    }
    else {
        return true;
    }
}

//上传
function updateBtnClickFun() {
    $("#ajaxForm").ajaxSubmit(options);
    return false;   //阻止表单默认提交
}
var options = {
    success: function (data, status) {
        var json = "";
        if (data.hasOwnProperty('d')) {
            json = data.d;
        } else {
            json = data;
        }
        if (json.Msg.IsSuccess) {
            art.artDialog.succeedTips("图片上传成功！", "", "1"); //短暂提示 - 成功
            $("#btnUpdateImg").attr("updateImgUrl", json.Model.PathUrl);
        }
        else {
            art.artDialog.errorTips("图片上传失败！失败原因：" + json.Msg.Message, "", "1"); //短暂提示 - 成功

        }
    },
    url: "./UploadImage1",    //默认是form的action，如果申明，则会覆盖
    type: "POST",    // 默认值是form的method("GET" or "POST")，如果声明，则会覆盖
    dataType: "json",    // html（默认）、xml、script、json接受服务器端返回的类型
    clearForm: true,    // 成功提交后，清除所有表单元素的值
    timeout: 3000    // 限制请求的时间，当请求大于3秒后，跳出请求
}


/*------------------------------------------  方法 end  -------------------------------------------*/



/*------------------------------------------  参数 start  -------------------------------------------*/

//新增参数
function getParamAdd() {

    var bannerTitle = $("#bannerTitle").val();
    var bannerUrl = $("#bannerUrl").val();
    var languageType = $("#languageType").val();
    var bannerDes = $("#btnUpdateImg").attr("updateImgUrl");
    var param = {
        "query": {
            "ParamInfo": {
                "BannerTitle": bannerTitle,
                "Url": bannerUrl,
                "LanguageType": languageType,
                "BannerDes": bannerDes
            },
            "PageInfo": {
                "CurrentPage": 1,
                "Total": 0,
                "PageCount": 0,
                "PageSize": 10
            }
        }
    }

    return param;
}

//删除参数
function getParamDel() {
    var param = {
        "Id": 0
    };
    return param;
}

//修改参数
function getParamModify() {
    var param = {
        "Id": 0
    };
    return param;
}
//修改参数1

//初始化查询参数
function getParamSelect() {

    var bannerTitle = $("#txtkeyword").val();
    var languageType = $("#selectLanguageType").val();
    var paramSelect = {
        "query": {
            "ParamInfo": {
                "BannerTitle": bannerTitle,
                "LanguageType": languageType
            },
            "PageInfo": {
                "CurrentPage": 1,
                "Total": 0,
                "PageCount": 0,
                "PageSize": 10
            }
        }
    }
    return paramSelect;
}


function getSelectOptionList(type) {
    $.ajax({
        type: "POST",
        url: "./GetSelectOptionList",
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
                        languageType += '<option value="' + json.List[i].DValue + '"> ' + json.List[i].Remarks + ' </option>';
                    }
                }
                if (type == 1) {
                    $("#languageType").html(languageType);//语言类型
                }
                else {
                    languageType = '<option value="0">所有</option>' + languageType;
                    $("#selectLanguageType").html(languageType);//货币类型
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


/*------------------------------------------  参数 end  -------------------------------------------*/


//art.artDialog.tips("短暂提示-无图标！", "", "5");//短暂提示-无图标
//art.artDialog.alert("警告！", "");//警告
//art.artDialog.alertTips("短暂提示 - 警告！", "", "3"); //短暂提示 - 警告
//art.artDialog.succeedTips("短暂提示 - 成功！", "", "2"); //短暂提示 - 成功
//art.artDialog.errorTips("（删除）系统错误！", "", "1");//短暂提示-错误
