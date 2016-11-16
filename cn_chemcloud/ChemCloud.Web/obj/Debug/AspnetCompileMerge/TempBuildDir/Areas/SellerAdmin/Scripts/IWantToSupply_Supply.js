$(document).ready(function () {
    //getSelectOptionList(0);

    param = getParamSelect();
    var isTrue = getParamSelectVel(param);
    if (isTrue) {
        Get_PageInfo(param);
        getJobsList(param);
        pageCount = param.query.PageInfo.PageCount;//模拟后台总页数
        //生成分页按钮
        if (pageCount > 5) {
            page_icon(1, 5, 0);
        } else {
            page_icon(1, pageCount, 0);
        }
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
        getJobsList(param);
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
        getJobsList(param);
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
        getJobsList(param);
    });


    $(".webInputPayrol").keyup(function () {
        $(this).val($(this).val().replace(/[^0-9.]/g, ''));
    }).bind("paste", function () {  //CTR+V事件处理    
        $(this).val($(this).val().replace(/[^0-9.]/g, ''));
    }).css("ime-mode", "disabled"); //CSS设置输入法不可用    

    $(".payrolHigh").keyup(function () {
        $(this).val($(this).val().replace(/[^0-9.]/g, ''));
    }).bind("paste", function () {  //CTR+V事件处理    
        $(this).val($(this).val().replace(/[^0-9.]/g, ''));
    }).css("ime-mode", "disabled"); //CSS设置输入法不可用  

    //上一页
    $("#preSpanBtn").click(function () {
        var currentPage = parseInt($("#currentSpanPage").html());
        if (currentPage > 1) {
            var param = getParamSelect();
            param.PageNo = currentPage - 1;
            getJobsList(param);
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
            param.PageNo = currentPage + 1;
            getJobsList(param);
        }
        else {

        }
    });

});
/*------------------------------------------  新增 Start  -------------------------------------------*/
function getParamAddVel(param) {

    var message = "";
    if (param.query.ParamInfo.UnitPrice.length == 0) {
        message = "请输入单价<br/>";
    }
    else if (isNaN(param.query.ParamInfo.UnitPrice)) {
        message = "单价格式不正确<br/>";
    }
    else if (param.query.ParamInfo.TotalPrice.length == 0) {
        message = "请输入总价<br/>";
    }
    else if (isNaN(param.query.ParamInfo.TotalPrice)) {
        message = "总价格式不正确<br/>";
    }
    else if (Number(param.query.ParamInfo.TotalPrice) < Number(param.query.ParamInfo.UnitPrice)) {
        message = "总价不能小于单价<br/>";
    }
    else if (param.query.ParamInfo.Quantity.length == 0) {
        message = "请输入数量<br/>";
    }
    else if (isNaN(param.query.ParamInfo.Quantity) && param.query.ParamInfo.Quantity < 1) {
        message = "数量必须为数字,切不能小于1<br/>";
    }
        //else if (param.query.ParamInfo.TypeOfCurrency.length == 0) {
        //    message = "请选择货币类型<br/>";
        //}
    else if (param.query.ParamInfo.LatestDeliveryTime.length == 0) {
        message = "请选择最迟交付时间<br/>";
    }
    //else if (param.query.ParamInfo.Address.length == 0) {
    //    message = "请输入收货地址<br/>";
    //}
    //else if (param.query.ParamInfo.Remarks.length == 0) {
    //    message = "请输入备注要求<br/>";
    //}
    var now = new Date();
    if (message.length > 0) {
        art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
        return false;
    }
    else {
        return true;
    }
}
function span_AddFun(param) {
    $.ajax({
        type: "POST",
        url: "/IWantToBuyUser/IWantToBuyAdd",
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
                param.query.PageInfo.CurrentPage = 1;
                getJobsList(param);
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
function getParamAdd() {

    var quantity = $("#txt_Quantity").val();
    var unitPrice = $("#txt_UnitPrice").val();
    var totalPrice = $("#txt_TotalPrice").val();
    //var unit = $("#txt_Unit").val();
    var deliveryDate = $("#txt_DeliveryDate").val();
    //var typeOfCurrency = $("#typeOfCurrency").val();


    var paramSelect = {
        "query": {
            "ParamInfo": {

                //"ProductName": productName,
                "Quantity": quantity,
                "UnitPrice": unitPrice,
                "TotalPrice": totalPrice,
                //"Unit": unit,
                "LatestDeliveryTime": deliveryDate
                //"Address": address,
                //"Remarks": remarks,
                //"TypeOfCurrency": typeOfCurrency
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
/*------------------------------------------  新增 End  -------------------------------------------*/

/*------------------------------------------  修改 Start  -------------------------------------------*/
function span_ModifyFun(iWantToBuyId) {
    var param = getParamModify(iWantToBuyId);

    param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "/SellerAdmin/IWantToSupply/GetObjectById_Supply",
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
                open_ModifyDialog(json, iWantToBuyId)
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
function open_ModifyDialog(json, iWantToBuyId) {
    art.dialog({
        width: 430,
        height: 200,
        top: 10,
        id: 'modify_Dialog',
        title: "我的报价",
        content: '<table id="table_AddJob" style="margin:0px auto;">'
                + '<tr>'
                    + '<td>报价单价：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_UnitPrice" type="text" class="inputTitle" value="' + json.Model.UnitPrice + '"/>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>报价总价：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_TotalPrice" type="text" class="inputTitle"  value="' + json.Model.TotalPrice + '" />'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>数量：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_Quantity" type="text" class="inputTitle" value="' + json.Model.Quantity + '" />'
                    + '</td>'
                + '</tr>'

                + '<tr>'
                    + '<td>最迟交付：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_DeliveryDate" type="text" onclick="dateTime_YMD_CN(this)"   value="' + (json.Model.LatestDeliveryTime == null ? "" : json.Model.LatestDeliveryTime) + '" class="inputDateTime" placeholder="请输入最迟交付时间"/>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>放弃竞价：</td>'
                    + '<td colspan="2">'
                        + '<input type="checkbox">'
                    + '</td>'
                + '</tr>'
                + '</table>',
        lock: true,
        fixed: true,
        ok: function () {

            var param = getParamAdd();

            param.query.ParamInfo.Id = json.Model.Id;
            param.query.ParamInfo.IWantToBuyID = iWantToBuyId;

            if (getParamModifyVal(json.Model.Status)) {
                var isTrue = getParamAddVel(param);
                if (isTrue) {
                    param = JSON.stringify(param)
                    modifyFun(param);

                    var param = getParamSelect();
                    param.PageNo = 1;
                    getJobsList(param);
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        },
        okValue: '提交',
        cancelValue: '取消',
        cancel: function () {

        }
    });
    getSelectOptionList(1);;
    $("#typeOfCurrency").val(json.Model.Model.TypeOfCurrency);//货币类型
}
function modifyFun(param) {
    $.ajax({
        type: "POST",
        url: "/SellerAdmin/IWantToSupply/IWantToSupplyAdd",
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
                getJobsList(param);
            }
            else {
                art.artDialog.errorTips(json.Message, "", "3");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("修改失败：系统错误！", "", "1.5");//短暂提示-错误
        }
    });
}
function getParamModify(iWantToBuyId) {
    var paramSelect = {
        "query": {
            "ParamInfo": {
                "IWantToBuyID": iWantToBuyId
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
function getParamAdd_IWantToSupply_All() {

    var unitPrice = parentTr.find("input.unitPrice").val();
    var totalPrice = parentTr.find("input.totalPrice").val();
    var quantity = parentTr.find("input.quantity").val();
    var deliveryDate = parentTr.find("input.deliveryDate").val();

    var paramSelect = {
        "query": {
            "ParamInfo": {
                "Quantity": quantity,
                "UnitPrice": unitPrice,
                "TotalPrice": totalPrice,
                "LatestDeliveryTime": deliveryDate,
                "Unit": ""
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
function getParamAdd_IWantToSupply(obj, supplyId, iWantToBuyId) {

    var parentTr = $(obj).parent().parent();

    var unitPrice = parentTr.find("input.unitPrice").val();
    var totalPrice = parentTr.find("input.totalPrice").val();
    var quantity = parentTr.find("input.quantity").val();
    var deliveryDate = parentTr.find("input.deliveryDate").val();
    var isCheck = parentTr.find("input.checkbox_fangqi").attr("checked") == "checked";

    var paramSelect = {
        "query": {
            "ParamInfo": {
                "Id": supplyId,
                "Quantity": quantity,
                "UnitPrice": unitPrice,
                "TotalPrice": totalPrice,
                "LatestDeliveryTime": deliveryDate,
                "Unit": "",
                "Status": (isCheck ? 3 : 0),//竞价状态（0：竞价中；1：竞价成功；2：竞价失败；3：放弃竞价；）
                "IWantToBuyID": iWantToBuyId
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
//修改验证
function getParamModifyVal(status) {
    var message = "";
    switch (status) {
        case 0://竞价中
            message = "";
            break;
        case 1://竞价成功
            message = "已确定，不能修改";
            break;
        case 2://竞价失败
            message = "";
            break;
        case 3://放弃降价
            message = "";
            break;
        case 4://已下单
            message = "已下单，不能修改";
            break;
        case 5://已支付
            message = "已支付，不能修改";
            break;
        case 6://已发货
            message = "已发货，不能修改";
            break;
        case 7://已收货
            message = "已收货，不能修改";
            break;
        default:
    }

    if (message.length > 0) {
        art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
        return false;
    }
    else {
        return true;
    }

}
/*------------------------------------------  修改 End  -------------------------------------------*/

/*------------------------------------------  修改状态 Start  -------------------------------------------*/
function span_UpdateStatusFun(obj, status) {
    var objSpan = $(obj);
    var Id = objSpan.attr("itemid");

    var param = getParamModify(Id);
    param.query.ParamInfo.Status = status;

    param2 = JSON.stringify(param);
    $.ajax({
        type: "POST",
        url: "/IWantToBuyUser/IWantToBuy_UpdateStatus",
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
                art.artDialog.succeedTips("修改成功！", "", "1.5"); //短暂提示 - 成功
                get_List();
            }
            else {
                art.artDialog.errorTips(json.Message, "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("（修改）系统错误！", "", "1.5");//短暂提示-错误
        }
    });

}
function getParamDel() {
    var param = {
        "Id": 0
    };
    return param;
}
/*------------------------------------------  修改状态 End  -------------------------------------------*/

/*------------------------------------------  查询 Start  -------------------------------------------*/
//根据查询条件获取列表
function get_List() {
    param = getParamSelect();//重新确定查询条件
    var isTrue = getParamSelectVel(param);
    if (isTrue) {
        Get_PageInfo(param);
        getJobsList(param);
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
function getJobsList(param) {
    param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "/SellerAdmin/IWantToSupply/SupplyList",
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
                        trList += "<tr class='tr_2'><td>"
                    }
                    else {
                        trList += "<tr class='tr_1'><td>";
                    }
                    trList += ((json.PageInfo.PageSize * (json.PageInfo.CurrentPage - 1) + i + 1)
                        + "</td><td>" + json.List[i].ProductName
                        + "</td><td>" + json.List[i].UnitPrice
                        + "</td><td>" + json.List[i].TotalPrice
                        + "</td><td>" + json.List[i].Quantity
                        + "</td><td>" + json.List[i].Unit
                        //+ "</td><td>" + json.List[i].CreateDate
                        + "</td><td>" + json.List[i].DeliveryDate
                        + "</td><td>" + json.List[i].StatusStr
                        + "</td><td>" + (json.List[i].Status >= 3 ? (json.List[i].IsMine == 1 ? ("<span style='color:blue;'>《 我 》</span>") : (json.List[i].IsMine == 2 ? ("<span>" + json.List[i].ShopName + "</span>") : "-------")) : ("--"))
                    + "</td><td style='text-align:left;'>"
                    + (json.List[i].WhetherParticipation == 1 ? ("<span onclick='span_ModifyFun(" + json.List[i].Id + ")'>我的报价</span> | ") : (json.List[i].Status == 0 ? "<span onclick='span_ModifyFun(" + json.List[i].Id + ")'>我要报价</span> | " : "———— | "))
                    + "<a href='/SellerAdmin/IWantToSupply/Get_SupplyList/" + json.List[i].Id + "'>竞价列表</a> | <a href='/ChatMessage/Index' target='_black'>在线咨询</a>  "
                    + (json.List[i].IsMine == 1 ? (json.List[i].Status == 5 ? " | <span onclick='DeliverGoodsFun(" + json.List[i].Id + "," + json.List[i].Status + ")'>去发货</span>" : (json.List[i].Status == 6 ? " | <span onclick='DeliverGoodsFun(" + json.List[i].Id + "," + json.List[i].Status + ")'>查看物流</span>" : (json.List[i].Status == 7 ? " | <span onclick='DeliverGoodsFun(" + json.List[i].Id + "," + json.List[i].Status + ")'>查看物流</span>" : ""))) : "")
                    + "</td></tr>"
                    );
                }
                tbody.html(trList);

            }
            else {
                var trList = "<tr><td colspan='6'>无任何数据</td></tr>";
                tbody.html(trList);

            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误6", "", "1.5");//短暂提示-错误
        }
    });
}
function Get_PageInfo(param) {
    var paramJson = JSON.stringify(param);
    $.ajax({
        type: "POST",
        async: false,
        url: "/SellerAdmin/IWantToSupply/Get_PageInfo_Member",
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
function getParamSelect() {
    //var selectAllOrMine = $("#selectAllOrMine").val();
    var productName = $("#txtkeyword").val();
    var createDate1 = $("#inputStartDate1").val();
    var createDate2 = $("#inputEndDate1").val();
    var deliveryDate1 = $("#inputStartDate2").val();
    var deliveryDate2 = $("#inputEndDate2").val();
    var paramSelect = {
        "query": {
            "ParamInfo": {
                //"AllOrMine": selectAllOrMine
                "CreateDate1": createDate1,
                "CreateDate2": createDate2,
                "DeliveryDate1": deliveryDate1,
                "DeliveryDate2": deliveryDate2,

                "ProductName": productName
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
function getParamSelectVel(param) {

    var message = "";

    if (message.length > 0) {
        art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
        return false;
    }
    else {
        return true;
    }
}

function ChangeStatus(status, iWantToBuyId, Id) {
    var param = getParamUpdateStatus(status, iWantToBuyId, Id);
    param = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "/IWantToBuyUser/IWantToSupply_UpdateStatus",
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
                location.href = "/IWantToBuyUser/Get_SupplyList/" + iWantToBuyId;
            }
            else {
                art.artDialog.errorTips(json.Message, "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("操作失败：系统错误！", "", "1.5");//短暂提示-错误
        }
    });
}
function getParamUpdateStatus(status, iWantToBuyId, Id) {
    var paramSelect = {
        "query": {
            "ParamInfo": {
                "Status": status,
                "IWantToBuyID": iWantToBuyId,
                "Id": Id
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

//发货
function DeliverGoodsFun(supplyId, status) {
    var param = getParam_DeliverGoods(supplyId);

    param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "/SellerAdmin/IWantToSupply/GetObjectById_Supply_DeliverGoods",
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
                open_ModifyDialog_DeliverGoods(json)
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
function open_ModifyDialog_DeliverGoods(json) {
    art.dialog({
        width: 430,
        height: 200,
        top: 10,
        id: 'modify_Dialog',
        title: "物流信息",
        content: '<table id="table_AddJob" style="margin:0px auto;">'
                + '<tr>'
                    + '<td>订单编号：</td>'
                    + '<td>'
                        + '<input id="purchaseNum" type="text" class="inputTitle" disabled="disabled" value="' + json.Model.PurchaseNum + '"/>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>物流公司：</td>'
                    + '<td>'
                            + '<select id="logisticsType" class="inputTitle" style="margin-left:0px;">'
                            + '</select>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>物流编号：</td>'
                    + '<td>'
                        + '<input id="logisticsNum" type="text" class="inputTitle" value="' + json.Model.LogisticsNum + '" />'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>物流备注：</td>'
                    + '<td>'
                        + '<textarea rows="3" cols="40" id="logisticsDes" >' + json.Model.LogisticsDes + '</textarea>'
                    + '</td>'
                + '</tr>'
                + '</table>',
        lock: true,
        fixed: true,
        ok: function () {

            var param = getParam_DeliverGoods_Add();
            param.query.ParamInfo.Id = json.Model.Id;

            var isTrue = getParam_DeliverGoods_Val(param);
            if (isTrue) {

                param = JSON.stringify(param)
                modify_DeliverGoodsFun(param);

                var param = getParamSelect();
                param.PageNo = 1;
                getJobsList(param);
            }
            else {
                return false;
            }
        },
        okValue: '提交',
        cancelValue: '取消',
        cancel: function () {

        }
    });
    getSelectOptionList(1);;
    $("#logisticsType").val(json.Model.LogisticsType);//物流公司
}
function modify_DeliverGoodsFun(param) {
    $.ajax({
        type: "POST",
        url: "/SellerAdmin/IWantToSupply/IWantToSupply_DeliverGoods",
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
                art.artDialog.succeedTips("成功！", "", "1.5"); //短暂提示 - 成功

                var param = getParamSelect();
                param.PageNo = 1;
                getJobsList(param);
            }
            else {
                art.artDialog.errorTips(json.Message, "", "3");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("修改失败：系统错误！", "", "1.5");//短暂提示-错误
        }
    });
}

function getParam_DeliverGoods(supplyId) {
    var paramSelect = {
        "query": {
            "ParamInfo": {
                "Id": supplyId
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
function getParam_DeliverGoods_Add() {

    var purchaseNum = $("#purchaseNum").val();
    var logisticsType = $("#logisticsType").val();
    var logisticsNum = $("#logisticsNum").val();
    var logisticsDes = $("#logisticsDes").val();

    var paramSelect = {
        "query": {
            "ParamInfo": {
                "PurchaseNum": purchaseNum,
                "LogisticsType": logisticsType,
                "LogisticsNum": logisticsNum,
                "LogisticsDes": logisticsDes
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
function getParam_DeliverGoods_Val(param) {
    var message = "";

    //if (param.query.ParamInfo.LogisticsType.length == 0) {
    //    message = "请选择物流公司";
    //}
    //if (param.query.ParamInfo.LogisticsNum.length == 0) {
    //    message = "请输入物流单号";
    //}
    //if (param.query.ParamInfo.LogisticsDes.length == 0) {
    //    message = "";
    //}

    if (message.length > 0) {
        art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
        return false;
    }
    else {
        return true;
    }

}

/*------------------------------------------  查询 End  -------------------------------------------*/

function getSelectOptionList(type) {
    $.ajax({
        type: "POST",
        url: "/IWantToBuyUser/GetSelectOptionList",
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
                var logisticsType = "";
                var typeOfCurrency = "";

                for (var i = 0; i < json.List.length; i++) {
                    if (json.List[i].DictionaryTypeId == 1) {
                        typeOfCurrency += '<option value="' + json.List[i].DValue + '"> ' + json.List[i].Remarks + ' </option>';
                    }

                    if (json.List[i].DictionaryTypeId == 109) {
                        logisticsType += '<option value="' + json.List[i].DKey + '"> ' + json.List[i].DValue + ' </option>';
                    }
                }
                if (type == 1) {
                    $("#typeOfCurrency").html(typeOfCurrency);//货币类型
                    $("#logisticsType").html(logisticsType);//物流公司
                }
                else {
                    //workType = '<option value="3">所有</option>' + workType;
                    //languageType = '<option value="0">所有</option>' + languageType;
                    //$("#selectWorkType").html(workType);//货币类型
                    //$("#selectLanguageType").html(languageType);//货币类型
                }
            }
            else {
                art.artDialog.errorTips("select数据获取失败", "", "1");//短暂提示-错误
                return false;
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误5", "", "1.5");//短暂提示-错误
            return false;
        }
    });
}

function SupplyListFun(id) {
    location.href = "/SellerAdmin/IWantToSupply/Get_SupplyList/" + id;
}

function onlinetalk() {
    top.location.href = "/ChatMessage/Index";
}
//art.artDialog.tips("短暂提示-无图标！", "", "5");//短暂提示-无图标
//art.artDialog.alert("警告！", "");//警告
//art.artDialog.alertTips("短暂提示 - 警告！", "", "3"); //短暂提示 - 警告
//art.artDialog.succeedTips("短暂提示 - 成功！", "", "2"); //短暂提示 - 成功
//art.artDialog.errorTips("（删除）系统错误！", "", "1");//短暂提示-错误
