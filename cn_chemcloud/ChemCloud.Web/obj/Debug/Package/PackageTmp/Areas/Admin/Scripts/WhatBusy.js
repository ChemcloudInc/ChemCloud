$(document).ready(function () {

    param = getParamSelect();
    Get_PageInfo(param);
    getWhatBusyList(param);

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

    //更新按钮
    $("#btn_Update").click(function () {
        art.dialog({
            width: 300,
            id: 'add_Dialog',
            title: '更新大家都在忙什么',
            content: '<table id="table_AddJob">'
                    + '<tr><td>用户注册：</td><td><input id="whatBusyCount1" class="busyTypeCount" type="text" value="5" placeholder="更新条数"/></td><td><img style="width:20px; height:20px; display:none;" id="img1"></td><td><button class="btn_update" onclick="updateWhatBusy(1,this)">更新</button></td></tr>'
                    + '<tr><td>定制合成：</td><td><input id="whatBusyCount2" class="busyTypeCount" type="text" value="5" placeholder="更新条数" /></td><td><img style="width:20px; height:20px; display:none;" id="img2"></td><td><button class="btn_update" onclick="updateWhatBusy(2,this)">更新</button></td></tr>'
                    + '<tr><td>代理采购：</td><td><input id="whatBusyCount3" class="busyTypeCount" type="text" value="5" placeholder="更新条数" /></td><td><img style="width:20px; height:20px; display:none;" id="img3"></td><td><button class="btn_update" onclick="updateWhatBusy(3,this)">更新</button></td></tr>'
                    + '<tr><td>招聘信息：</td><td><input id="whatBusyCount4" class="busyTypeCount" type="text" value="5" placeholder="更新条数"/></td><td><img style="width:20px; height:20px; display:none;" id="img4"></td><td><button class="btn_update" onclick="updateWhatBusy(4,this)">更新</button></td></tr>'
                    + '<tr><td>会议信息：</td><td><input id="whatBusyCount5" class="busyTypeCount" type="text" value="5" placeholder="更新条数"/></td><td><img style="width:20px; height:20px; display:none;" id="img5"></td><td><button class="btn_update" onclick="updateWhatBusy(5,this)">更新</button></td></tr>'
                    + '<tr><td>技术交易：</td><td><input id="whatBusyCount6" class="busyTypeCount" type="text" value="5" placeholder="更新条数"/></td><td><img style="width:20px; height:20px; display:none;" id="img6"></td><td><button class="btn_update" onclick="updateWhatBusy(6,this)">更新</button></td></tr>'

                     //+ '<tr><td></td><td><input id="whatBusyCount0" class="busyTypeCount" type="text" value="5" placeholder="更新条数"/></td><td><img style="width:20px; height:20px; display:none;" id="img0"></td><td><button class="btn_update" onclick="updateWhatBusy_All(0)">更新所有</button></td></tr>'
                    + '</table>',
            lock: true,
            fixed: true,
            ok: function () {

            },
            okValue: '提交',
            cancelValue: '取消',
            cancel: function () {
            }
        });
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
        getWhatBusyList(param);
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
        getWhatBusyList(param);
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
        getWhatBusyList(param);
    });

});

function updateWhatBusy(whatBusyType, obj) {
    var param = getParamSelect();
    param.query.ParamInfo.BusyType = whatBusyType;
    param.query.ParamInfo.TopCount = $("#whatBusyCount" + whatBusyType).val();

    var param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "./UpdateWhatBusy_By_WhatBusyType",
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

            if (json != null && json.Msg.IsSuccess) {
                $(obj).attr('disabled', 'true');
                $(obj).html("更新成功");
                $("#img" + whatBusyType).attr("src", "/Areas/Web/Images/state1.png");
                $("#img" + whatBusyType).css("display", "block");
            }
            else {
                $(obj).html("更新失败");
                $("#img" + whatBusyType).attr("src", "/Areas/Web/Images/state2.png");
                $("#img" + whatBusyType).css("display", "block");
                art.artDialog.errorTips("修改过程中查询信息失败：未获取任何信息", "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("（修改）系统错误！", "", "1.5");//短暂提示-错误
        }
    });

}
function updateWhatBusy_All(whatBusyType) {
    var param = getParamSelect();
    param.query.ParamInfo.BusyType = whatBusyType;
    param.query.ParamInfo.TopCount = $("#whatBusyCount" + whatBusyType).val();

    var param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "./UpdateWhatBusy_By_WhatBusyType",
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

            if (json != null && json.Msg.IsSuccess) {
                $(obj).attr('disabled', 'true');
                $(obj).html("更新成功");
                $("#img" + whatBusyType).attr("src", "/Areas/Web/Images/state1.png");
                $("#img" + whatBusyType).css("display", "block");
            }
            else {
                $(obj).html("更新失败");
                $("#img" + whatBusyType).attr("src", "/Areas/Web/Images/state2.png");
                $("#img" + whatBusyType).css("display", "block");
                art.artDialog.errorTips("修改过程中查询信息失败：未获取任何信息", "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("（修改）系统错误！", "", "1.5");//短暂提示-错误
        }
    });

}

/*------------------------------------------  方法 start  -------------------------------------------*/

//删除
function span_DeleteFun(obj) {
    var param = getParamSelect();
    var objSpan = $(obj);
    param.query.ParamInfo.Id = objSpan.attr("itemid")
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
                get_List();
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
//根据查询条件获取列表
function get_List() {
    param = getParamSelect();//重新确定查询条件
    var isTrue = getParamSelectVel();
    if (isTrue) {
        Get_PageInfo(param);
        getWhatBusyList(param);
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
function getWhatBusyList(param) {
    param.query.ParamInfo.BusyType = $("#selectBusyType").val();
    var param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "./GetWhatBusyList",
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
                        trList += "<tr class='tr_1'><td>" + (json.PageInfo.PageSize * (json.PageInfo.CurrentPage - 1) + i + 1)+"</td><td>"
                        + json.List[i].UserName + "</td><td>"
                        + json.List[i].BusyCotent + "</td><td>"
                        + json.List[i].BusyTypeName + "</td><td>"
                        + json.List[i].CreateDate + "</td><td>"
                       //+ (json.List[i].IsShow == 1 ? "是" : "否") + "</td><td>"
                          + "<span onclick='span_DeleteFun(this)' itemId='"
                        + json.List[i].Id + "'>删除</span> "
                        //+ "<span onclick='span_ModifyFun(this)' itemId='" + json.List[i].Id + "'>更新</span>"
                        + "</td>"
                        + "</tr>";
                    }
                    else {
                        trList += "<tr class='tr_2'><td>" + (json.PageInfo.PageSize * (json.PageInfo.CurrentPage - 1) + i + 1)+ "</td><td>"
                        + json.List[i].UserName + "</td><td>"
                        + json.List[i].BusyCotent + "</td><td>"
                        + json.List[i].BusyTypeName + "</td><td>"
                        + json.List[i].CreateDate + "</td><td>"
                        //+ (json.List[i].IsShow == 1 ? "是" : "否") + "</td><td>"
                        + "<span onclick='span_DeleteFun(this)' itemId='"
                        + json.List[i].Id + "'>删除</span>"
                        //+ "<span onclick='span_ModifyFun(this)' itemId='" + json.List[i].Id + "'>更新</span>"
                        + "</td>"
                        + "</tr>";
                    }

                }
                tbody.html(trList);
                $("#currentSpanPage").html(json.PageInfo.CurrentPage);
                $("#spanTotal").html(json.PageInfo.PageCount);

                //上一页样式
                if (json.PageInfo.CurrentPage == 1) {
                    $("#preSpanBtn").addClass("prev-disabled");
                }
                else if (json.PageInfo.CurrentPage > 1) {
                    $("#preSpanBtn").removeClass("prev-disabled");
                }

                //下一页样式
                if (json.PageInfo.CurrentPage == json.PageInfo.PageCount) {
                    $("#nextSpanBtn").addClass("prev-disabled");
                }
                else if (json.PageInfo.CurrentPage < json.PageInfo.PageCount) {
                    $("#nextSpanBtn").removeClass("prev-disabled");
                }

            }
            else {
                var trList = "<tr><td colspan='10'>无任何数据</td></tr>";
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
    param.query.ParamInfo.BusyType = $("#selectBusyType").val();
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
    return true;
}
/*------------------------------------------  方法 end  -------------------------------------------*/



/*------------------------------------------  参数 start  -------------------------------------------*/

//初始化查询参数
function getParamSelect() {
    var paramSelect = {
        "query": {
            "ParamInfo": {
                "BusyType": selectBusyType
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

/*------------------------------------------  参数 end  -------------------------------------------*/


//art.artDialog.tips("短暂提示-无图标！", "", "5");//短暂提示-无图标
//art.artDialog.alert("警告！", "");//警告
//art.artDialog.alertTips("短暂提示 - 警告！", "", "3"); //短暂提示 - 警告
//art.artDialog.succeedTips("短暂提示 - 成功！", "", "2"); //短暂提示 - 成功
//art.artDialog.errorTips("（删除）系统错误！", "", "1");//短暂提示-错误
