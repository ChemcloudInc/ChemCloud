$(document).ready(function () {

    var endDate = new Date();
    var startDate = getDatedate_YMD(endDate);
    var startDate1 = addMonth(startDate, -6);
    dateTime_SetDefaultsValue("#startDate", "yyyy-mm", startDate1, "zh-CN", "", endDate, startDate1);
    dateTime_SetDefaultsValue("#endDate", "yyyy-mm", endDate, "zh-CN", "", endDate, endDate);

    var param = getParamSelect();
    getBannersIndexList(param);

    //查询按钮
    $("#btn_Search").click(function () {
        var param1 = getParamSelect();
        getBannersIndexList(param1);
    });

    //新增按钮
    $("#btn_Add").click(function () {
        art.dialog({
            width: 300,
            id: 'add_Dialog',
            title: '新增实时交易数据',
            content: '<table id="table_AddJob">'
                    + '<tr><td>订单类型：</td><td><select id="select_CurveType"><option value="1">代理采购</option><option value="2">定制合成</option></select></td></tr>'
                    + '<tr><td>年月：</td><td><input id="txtYearMonth" type="text"/></td></tr>'
                    + '<tr><td>中文名：</td><td><input id="txtXName_CN" type="text" /></td></tr>'
                    + '<tr><td>英文名：</td><td><input id="txtXName_Eng" type="text" /></td></tr>'
                    + '<tr><td>完成总金额：</td><td><input id="txtY_CompleteAmount" type="text"  disabled="disabled"/></td></tr>'
                    + '<tr><td>下单总金额：</td><td><input id="txtY_OrderAmount" type="text"  disabled="disabled"/></td></tr>'
                    + '<tr><td>完成订单数：</td><td><input id="txtY_CompleteNumber" type="text"  disabled="disabled"/></td></tr>'
                    + '<tr><td>下单订单数：</td><td><input id="txtY_OrderQuantity" type="text"  disabled="disabled"/></td></tr>'
                    + '<tr><td><td colspan="2"><button id="btn_StartRun" onclick="StartRunFun_Add()">点击此处开始统计</button></td></tr>'
                    + '</table>',
            lock: true,
            fixed: true,
            ok: function () {
                var res = getParamAdd();
                if (res.Msg.length > 0) {
                    art.artDialog.errorTips(res.Msg, "", "1.5");//短暂提示-错误
                    return false;
                }
                else {
                    res.Param = JSON.stringify(res.Param)
                    span_AddFun(res.Param);
                    var param = getParamSelect();
                    param.PageNo = 1;
                    getBannersIndexList(param);
                }

            },
            okValue: '提交',
            cancelValue: '取消',
            cancel: function () {
            }
        });
        dateTime_SetDefaultsValue("#txtYearMonth", "yyyy-mm", endDate, "zh-CN", "", endDate, startDate);
    });

    //上一页
    $("#preSpanBtn").click(function () {
        var currentPage = parseInt($("#currentSpanPage").html());
        if (currentPage > 1) {
            var param = getParamSelect();
            param.query.PageNo = currentPage - 1;
            getBannersIndexList(param);
        }
        else {
        }
    });
    //下一页
    $("#nextSpanBtn").click(function () {
        var currentPage = parseInt($("#currentSpanPage").html());
        var spanTotal = parseInt($("#spanTotal").html());

        if (currentPage < spanTotal) {
            var param = getParamSelect();
            param.query.PageNo = currentPage + 1;
            getBannersIndexList(param);
        }
        else {

        }
    });

});


/*------------------------------------------  方法 start  -------------------------------------------*/

//-----------修改
function span_ModifyFun(obj) {
    var objSpan = $(obj);
    var param = getParamModify();
    param.Id = objSpan.attr("itemId");
    var param2 = JSON.stringify(param)
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
        title: '更新实时交易数据',
        content: '<table id="table_AddJob">'
                + '<tr><td>订单类型：</td><td><select id="select_CurveType" disabled="disabled"><option value="1">代理采购</option><option value="2">定制合成</option></select></td></tr>'
                + '<tr><td>年月：</td><td><input id="txtYearMonth" value="' + json.YearMonth + '"  type="text"   disabled="disabled"/></td></tr>'
                + '<tr><td>中文名：</td><td><input id="txtXName_CN" type="text" value="' + json.XName_CN + '" /></td></tr>'
                + '<tr><td>英文名：</td><td><input id="txtXName_Eng" type="text" value="' + json.XName_Eng + '"  /></td></tr>'
                + '<tr><td>完成总金额：</td><td><input id="txtY_CompleteAmount" value="' + json.Y_CompleteAmount + '"  type="text"  disabled="disabled"/></td></tr>'
                + '<tr><td>下单总金额：</td><td><input id="txtY_OrderAmount" value="' + json.Y_OrderAmount + '"  type="text"  disabled="disabled"/></td></tr>'
                + '<tr><td>完成订单数：</td><td><input id="txtY_CompleteNumber" value="' + json.Y_CompleteNumber + '"  type="text"  disabled="disabled"/></td></tr>'
                + '<tr><td>下单订单数：</td><td><input id="txtY_OrderQuantity" value="' + json.Y_OrderQuantity + '"  type="text"  disabled="disabled"/></td></tr>'
                + '<tr><td><td colspan="2"><button id="btn_StartRun" itemId="' + json.Id + '" onclick="StartRunFun_Refresh(this)">点击此处更新数据</button></td></tr>'
                + '</table>',
        lock: true,
        fixed: true,
        ok: function () {
            var param = getParamModify1();
            param.Id = json.Id;

            modifyFun(param);

            var param = getParamSelect();
            param.PageNo = 1;
            getBannersIndexList(param);
        },
        okValue: '提交',
        cancelValue: '取消',
        cancel: function () {

        }
    });
    $("#select_CurveType").val(json.CurveType);
}
function modifyFun(param) {
    param.Param.Id = $("#btn_StartRun").attr("itemId");
    var param1 = JSON.stringify(param.Param)

    $.ajax({
        type: "POST",
        url: "./ModifyTransactionRecord",
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
        url: "./TransactionRecordAdd",
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
function getBannersIndexList(param) {
    var param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "./GetTransactionRecordList",
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
                        trList += "<tr class='tr_1'><td>" + (i + 1) + "</td><td>"
                        + json.List[i].CurveType + "</td><td>"
                        + json.List[i].YearMonth + "</td><td>"
                        + json.List[i].XName_CN + "</td><td>"
                        + json.List[i].XName_Eng + "</td><td>"
                        + json.List[i].Y_CompleteAmount + "</td><td>"
                        + json.List[i].Y_OrderAmount + "</td><td>"
                        + json.List[i].Y_CompleteNumber + "</td><td>"
                        + json.List[i].Y_OrderQuantity + "</td><td>"
                        //+ json.List[i].IsTrue + "</td><td>"
                        + "<span onclick='span_DeleteFun(this)' itemId='"
                        + json.List[i].Id + "'>删除</span> <span onclick='span_ModifyFun(this)' itemId='"
                        + json.List[i].Id + "'>更新</span></td></tr>";
                    }
                    else {
                        trList += "<tr class='tr_2'><td>" + (i + 1) + "</td><td>"
                        + json.List[i].CurveType + "</td><td>"
                        + json.List[i].YearMonth + "</td><td>"
                        + json.List[i].XName_CN + "</td><td>"
                        + json.List[i].XName_Eng + "</td><td>"
                        + json.List[i].Y_CompleteAmount + "</td><td>"
                        + json.List[i].Y_OrderAmount + "</td><td>"
                        + json.List[i].Y_CompleteNumber + "</td><td>"
                        + json.List[i].Y_OrderQuantity + "</td><td>"
                        //+ json.List[i].IsTrue + "</td><td>"
                        + "<span onclick='span_DeleteFun(this)' itemId='"
                        + json.List[i].Id + "'>删除</span> <span onclick='span_ModifyFun(this)' itemId='"
                        + json.List[i].Id + "'>更新</span></td></tr>";
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

//新增——统计指定月份、指定类型订单数据
function StartRunFun_Add() {

    var res = getParamComputeFun();
    if (res.Msg.length > 0) {
        art.artDialog.errorTips(res.Msg, "", "1.5");//短暂提示-错误
    }
    else {
        res.Param = JSON.stringify(res.Param)
        $.ajax({
            type: "POST",
            url: "./ComputeFun",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: res.Param,
            beforeSend: function () {
            },
            success: function (data) {
                var json = "";
                if (data.hasOwnProperty('d')) {
                    json = data.d;
                } else {
                    json = data;
                }
                if (json.Msg.IsSuccess) {
                    //$("#txtXName_CN").val(json.Model.XName_CN);
                    //$("#txtXName_Eng").val(json.Model.XName_Eng);
                    $("#txtY_CompleteAmount").val(json.Model.Y_CompleteAmount);
                    $("#txtY_OrderAmount").val(json.Model.Y_OrderAmount);
                    $("#txtY_CompleteNumber").val(json.Model.Y_CompleteNumber);
                    $("#txtY_OrderQuantity").val(json.Model.Y_OrderQuantity);
                }
                else {
                    art.artDialog.errorTips(json.Msg.Message, "", "2");//短暂提示-错误
                }
            },
            error: function () {
                art.artDialog.errorTips("新增失败：系统错误！", "", "1.5");//短暂提示-错误
            }
        });
    }
}
//更新——统计指定月份、指定类型订单数据
function StartRunFun_Refresh(obj) {

    var res = getParamComputeFun();
    var objSpan = $(obj);
    res.Param.Id = objSpan.attr("itemid")

    if (res.Msg.length > 0) {
        art.artDialog.errorTips(res.Msg, "", "1.5");//短暂提示-错误
    }
    else if (res.Param.Id.length == 0) {
        art.artDialog.errorTips("获取本条信息数据Id失败", "", "1.5");//短暂提示-错误
    }
    else {
        res.Param = JSON.stringify(res.Param)
        $.ajax({
            type: "POST",
            url: "./ComputeFun",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: res.Param,
            beforeSend: function () {
            },
            success: function (data) {
                var json = "";
                if (data.hasOwnProperty('d')) {
                    json = data.d;
                } else {
                    json = data;
                }
                if (json.Msg.IsSuccess) {
                    //$("#txtXName_CN").val(json.Model.XName_CN);
                    //$("#txtXName_Eng").val(json.Model.XName_Eng);
                    $("#txtY_CompleteAmount").val(json.Model.Y_CompleteAmount);
                    $("#txtY_OrderAmount").val(json.Model.Y_OrderAmount);
                    $("#txtY_CompleteNumber").val(json.Model.Y_CompleteNumber);
                    $("#txtY_OrderQuantity").val(json.Model.Y_OrderQuantity);
                }
                else {
                    art.artDialog.errorTips(json.Msg.Message, "", "2");//短暂提示-错误
                }
            },
            error: function () {
                art.artDialog.errorTips("新增失败：系统错误！", "", "1.5");//短暂提示-错误
            }
        });
    }
}

/*------------------------------------------  方法 end  -------------------------------------------*/



/*------------------------------------------  参数 start  -------------------------------------------*/

//新增参数
function getParamAdd() {

    var res = {
        "Param": {},
        "Msg": ""
    };

    var curveType = $("#select_CurveType").val();
    var yearMonth = $("#txtYearMonth").val();
    var xName_CN = $("#txtXName_CN").val();
    var xName_Eng = $("#txtXName_Eng").val();
    var y_CompleteAmount = $("#txtY_CompleteAmount").val();
    var y_OrderAmount = $("#txtY_OrderAmount").val();
    var y_CompleteNumber = $("#txtY_CompleteNumber").val();
    var y_OrderQuantity = $("#txtY_OrderQuantity").val();

    if (curveType.length == 0) {
        res.Msg += "订单类型错误";
    }
    else if (yearMonth.length == 0) {
        res.Msg += "请选择输入年月";
    }
    else if (xName_CN.length == 0) {
        res.Msg += "中文名不能为空";
    }
    else if (xName_Eng.length == 0) {
        res.Msg += "英文名不能为空";
    }
    else if (y_CompleteAmount.length == 0) {
        res.Msg += "完成总金额不能为空";
    }
    else if (y_OrderAmount.length == 0) {
        res.Msg += "下单总金额不能为空";
    }
    else if (y_CompleteNumber.length == 0) {
        res.Msg += "完成订单数不能为空";
    }
    else if (y_OrderQuantity.length == 0) {
        res.Msg += "下单订单数不能为空";
    }

    res.Param = {
        "CurveType": curveType,
        "YearMonth": yearMonth,
        "XName_CN": xName_CN,
        "XName_Eng": xName_Eng,
        "Y_CompleteAmount": y_CompleteAmount,
        "Y_OrderAmount": y_OrderAmount,
        "Y_CompleteNumber": y_CompleteNumber,
        "Y_OrderQuantity": y_OrderQuantity
    };
    return res;
}

//查找指定年月及类型订单
function getParamComputeFun() {

    var res = {
        "Param": {},
        "Msg": ""
    };

    var curveType = $("#select_CurveType").val();
    var yearMonth = $("#txtYearMonth").val();

    if (curveType.length == 0) {
        res.Msg += "订单类型错误";
    }
    else if (yearMonth.length == 0) {
        res.Msg += "请选择输入年月";
    }


    res.Param = {
        "CurveType": curveType,
        "YearMonth": yearMonth,
        "Id": 0
    };
    return res;
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
function getParamModify1() {

    var res = {
        "Param": {},
        "Msg": ""
    };

    var curveType = $("#select_CurveType").val();
    var yearMonth = $("#txtYearMonth").val();
    var xName_CN = $("#txtXName_CN").val();
    var xName_Eng = $("#txtXName_Eng").val();
    var y_CompleteAmount = $("#txtY_CompleteAmount").val();
    var y_OrderAmount = $("#txtY_OrderAmount").val();
    var y_CompleteNumber = $("#txtY_CompleteNumber").val();
    var y_OrderQuantity = $("#txtY_OrderQuantity").val();

    if (curveType.length == 0) {
        res.Msg += "订单类型错误";
    }
    else if (yearMonth.length == 0) {
        res.Msg += "请选择输入年月";
    }
    else if (xName_CN.length == 0) {
        res.Msg += "中文名不能为空";
    }
    else if (xName_Eng.length == 0) {
        res.Msg += "英文名不能为空";
    }
    else if (y_CompleteAmount.length == 0) {
        res.Msg += "完成总金额不能为空";
    }
    else if (y_OrderAmount.length == 0) {
        res.Msg += "下单总金额不能为空";
    }
    else if (y_CompleteNumber.length == 0) {
        res.Msg += "完成订单数不能为空";
    }
    else if (y_OrderQuantity.length == 0) {
        res.Msg += "下单订单数不能为空";
    }

    res.Param = {
        "CurveType": curveType,
        "YearMonth": yearMonth,
        "XName_CN": xName_CN,
        "XName_Eng": xName_Eng,
        "Y_CompleteAmount": y_CompleteAmount,
        "Y_OrderAmount": y_OrderAmount,
        "Y_CompleteNumber": y_CompleteNumber,
        "Y_OrderQuantity": y_OrderQuantity
    };
    return res;
}
//初始化查询参数
function getParamSelect() {

    var startDate = new Date($("#startDate").val());
    var endDate = new Date($("#endDate").val());
    var curveType = $("#curveType").val();

    if (startDate > endDate) {
        art.artDialog.errorTips("起始日期不能大于结束日期", "", "1.5");//短暂提示-错误
    }
    var param = {
        "query":
           {
               "StartDate": startDate,
               "EndDate": endDate,
               "CurveType": curveType,
               "IsAsc": false,
               "PageNo": 1,
               "PageSize": 10,
               "Sort": "desc"
           }
    }
    return param;
}

/*------------------------------------------  参数 end  -------------------------------------------*/


//art.artDialog.tips("短暂提示-无图标！", "", "5");//短暂提示-无图标
//art.artDialog.alert("警告！", "");//警告
//art.artDialog.alertTips("短暂提示 - 警告！", "", "3"); //短暂提示 - 警告
//art.artDialog.succeedTips("短暂提示 - 成功！", "", "2"); //短暂提示 - 成功
//art.artDialog.errorTips("（删除）系统错误！", "", "1");//短暂提示-错误
