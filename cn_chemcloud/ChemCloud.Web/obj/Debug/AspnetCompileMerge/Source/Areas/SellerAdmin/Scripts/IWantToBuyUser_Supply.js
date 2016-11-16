$(document).ready(function () {
    //getSelectOptionList(0);
    var id = $("#IWantTobBuy").val();
    param = getParamSelect(id);
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

    //新增按钮
    $("#btn_Add").click(function () {
        var editor = "";
        art.dialog({
            width: 430,
            height: 300,
            top: 10,
            id: 'add_Dialog',
            title: '新增我要采购',
            content: '<table id="table_AddJob" style="margin:0px auto;">'
                    + '<tr>'
                        + '<td>产品名称：</td>'
                        + '<td colspan="2">'
                            + '<input id="txt_ProductName" type="text" class="inputTitle" placeholder="请输入产品名称" />'
                        + '</td>'
                    + '</tr>'
                    + '<tr>'
                        + '<td>产品单价：</td>'
                        + '<td colspan="2">'
                            + '<input id="txt_UnitPrice" type="text" class="inputTitle"  placeholder="请输入单价" />'
                        + '</td>'
                    + '</tr>'
                    + '<tr>'
                        + '<td>产品总价：</td>'
                        + '<td colspan="2">'
                            + '<input id="txt_TotalPrice" type="text" class="inputTitle" placeholder="请输入采购总价"  />'
                        + '</td>'
                    + '</tr>'
                    + '<tr>'
                        + '<td>采购数量：</td>'
                        + '<td colspan="2">'
                            + '<input id="txt_Quantity" type="text" class="inputTitle" placeholder="请输入采购数量"  />'
                        + '</td>'
                    + '</tr>'

                    + '<tr>'
                        + '<td>货币类型：</td>'
                        + '<td colspan="2">'
                            + '<select id="typeOfCurrency" class="search_80" style="margin-left:0px;">'
                            + '</select>'
                        + '</td>'
                    + '</tr>'
                     + '<tr>'
                        + '<td>计量单位：</td>'
                        + '<td colspan="2">'
                            + '<input id="txt_Unit" type="text" class="inputTitle" placeholder="请输入计量单位"  />'
                        + '</td>'
                    + '</tr>'
                    + '<tr>'
                        + '<td>截至交付：</td>'
                        + '<td colspan="2">'
                            + '<input id="txt_DeliveryDate" type="text"  onclick="dateTime_YMD_CN(this)"  class="inputDateTime" placeholder="请输入最迟交付时间"/>'
                        + '</td>'
                    + '</tr>'
                    + '<tr>'
                        + '<td>收货地址：</td>'
                        + '<td colspan="2">'
                            + '<input id="txt_Address" type="text" class="inputTitle"   placeholder="请输入收货地址"/>'
                        + '</td>'
                    + '</tr>'
                    + '<tr>'
                        + '<td>备注要求：</td>'
                        + '<td colspan="2">'
                            + '<textarea  id="txtArea_Remarks" cols="48" rows="5"  placeholder="请填写备注、要求" style="border:1px solid #CCC;"></textarea>'
                        + '</td>'
                    + '</tr>'
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
    });

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
    if (param.query.ParamInfo.ProductName.length == 0) {
        message = "请输入产品名称<br/>";
    }
    else if (param.query.ParamInfo.UnitPrice.length == 0) {
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
    else if (param.query.ParamInfo.TypeOfCurrency.length == 0) {
        message = "请选择货币类型<br/>";
    }
    else if (param.query.ParamInfo.DeliveryDate.length == 0) {
        message = "请选择最迟交付时间<br/>";
    }
    else if (param.query.ParamInfo.Address.length == 0) {
        message = "请输入收货地址<br/>";
    }
    else if (param.query.ParamInfo.Remarks.length == 0) {
        message = "请输入备注要求<br/>";
    }
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
        url: "/SellerAdmin/IWantToSupply/IWantToBuyAdd",
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

    var productName = $("#txt_ProductName").val();
    var quantity = $("#txt_Quantity").val();
    var unitPrice = $("#txt_UnitPrice").val();
    var totalPrice = $("#txt_TotalPrice").val();
    var unit = $("#txt_Unit").val();
    var deliveryDate = $("#txt_DeliveryDate").val();
    var address = $("#txt_Address").val();
    var remarks = $("#txtArea_Remarks").val();
    var typeOfCurrency = $("#typeOfCurrency").val();


    var paramSelect = {
        "query": {
            "ParamInfo": {
                "ProductName": productName,
                "Quantity": quantity,
                "UnitPrice": unitPrice,
                "TotalPrice": totalPrice,
                "Unit": unit,
                "DeliveryDate": deliveryDate,
                "Address": address,
                "Remarks": remarks,
                "TypeOfCurrency": typeOfCurrency
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
function span_ModifyFun(obj) {
    var objSpan = $(obj);
    var id = objSpan.attr("itemId");
    var param = getParamModify(id);

    param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "/SellerAdmin/IWantToSupply/GetObjectById_User",
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
        width: 430,
        height: 300,
        top: 10,
        id: 'modify_Dialog',
        title: "修改：" + json.Model.Model.ProductName,
        content: '<table id="table_AddJob" style="margin:0px auto;">'
                + '<tr>'
                    + '<td>产品名称：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_ProductName" type="text" class="inputTitle" value="' + json.Model.Model.ProductName + '" />'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>产品单价：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_UnitPrice" type="text" class="inputTitle" value="' + json.Model.Model.UnitPrice + '" />'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>产品总价：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_TotalPrice" type="text" class="inputTitle" value="' + json.Model.Model.TotalPrice + '" />'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>数量：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_Quantity" type="text" class="inputTitle" value="' + json.Model.Model.Quantity + '" />'
                    + '</td>'
                + '</tr>'

                + '<tr>'
                    + '<td>货币类型：</td>'
                    + '<td colspan="2">'
                        + '<select id="typeOfCurrency" class="search_80" style="margin-left:0px;">'
                        + '</select>'
                    + '</td>'
                + '</tr>'
                 + '<tr>'
                    + '<td>计量单位：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_Unit" type="text" class="inputTitle" value="' + json.Model.Model.Unit + '" />'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>截至交付：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_DeliveryDate" type="text"  value="' + json.Model.Model.DeliveryDate + '" onclick="dateTime_YMD_CN(this)"  class="inputDateTime" placeholder="请输入最迟交付时间"/>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>收货地址：</td>'
                    + '<td colspan="2">'
                        + '<input id="txt_Address" type="text" class="inputTitle" value="' + json.Model.Model.Address + '" placeholder="请输入收货地址"/>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>备注要求：</td>'
                    + '<td colspan="2">'
                        + '<textarea  id="txtArea_Remarks" cols="48" rows="5"  placeholder="请填写备注、要求" style="border:1px solid #CCC;">' + json.Model.Model.Remarks + '</textarea>'
                    + '</td>'
                + '</tr>'
                + '</table>',
        lock: true,
        fixed: true,
        ok: function () {

            var param = getParamAdd();

            param.query.ParamInfo.Id = json.Model.Model.Id;

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
        url: "/SellerAdmin/IWantToSupply/IWantToBuy_Update",
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
function getParamModify(id) {

    var paramSelect = {
        "query": {
            "ParamInfo": {
                "Id": id
            }
        }
    }
    return paramSelect;
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
        url: "/SellerAdmin/IWantToSupply/IWantToBuy_UpdateStatus",
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
        url: "/SellerAdmin/IWantToSupply/IWantToSupply_List",
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
                    if (json.List[i].IsMine == 1) {
                        trList += ((json.PageInfo.PageSize * (json.PageInfo.CurrentPage - 1) + i + 1)
                            + "</td><td>" + json.List[i].PurchaseNum
                            + "</td><td>" + json.List[i].UnitPrice
                            + "</td><td>" + json.List[i].TotalPrice
                            + "</td><td>" + json.List[i].Unit
                            + "</td><td>" + json.List[i].Quantity
                            + "</td><td>" + json.List[i].LatestDeliveryTime
                            + "</td><td>" + json.List[i].ShopName
                            + "</td><td>"
                            + json.List[i].StatusStr
                            + "</td><td>"
                            + (json.List[i].Status == 0 ? (json.List[i].IsMine == 1 ? "————" : "<span class='span_wytb_hui1'>————</span>") : "---")
                            + "</td></tr>"
                        );
                    }
                    else {
                        trList += ((json.PageInfo.PageSize * (json.PageInfo.CurrentPage - 1) + i + 1)
                            + "</td><td>" + json.List[i].PurchaseNum
                            + "</td><td>" + "**"
                            + "</td><td>" + "**"
                            + "</td><td>" + json.List[i].Unit
                            + "</td><td>" + json.List[i].Quantity
                            + "</td><td>" + json.List[i].LatestDeliveryTime
                            + "</td><td>" + json.List[i].ShopName
                            + "</td><td>"
                            + json.List[i].StatusStr
                            + "</td><td>"
                            + (json.List[i].Status == 0 ? (json.List[i].IsMine == 1 ? "————" : "<span class='span_wytb_hui1'>————</span>") : "---")
                            + "</td></tr>"
                        );
                    }
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
        url: "/SellerAdmin/IWantToSupply/Get_PageInfo_IWantToSupply_List",
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
function getParamSelect(id) {

    var paramSelect = {
        "query": {
            "ParamInfo": {
                "Id": id
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

    if (param.query.ParamInfo.Id.length = 0 && param.query.ParamInfo.Id < 0) {
        message = "获取我要采购Id失败";
    }

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
        url: "/SellerAdmin/IWantToSupply/IWantToSupply_UpdateStatus",
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
                location.href = "/SellerAdmin/IWantToSupply/Get_SupplyList/" + iWantToBuyId;
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
/*------------------------------------------  查询 End  -------------------------------------------*/

function getSelectOptionList(type) {
    $.ajax({
        type: "POST",
        url: "/SellerAdmin/IWantToSupply/GetSelectOptionList",
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
                var workType = "";
                var typeOfCurrency = "";
                var payrollType = "";
                var languageType = "";

                for (var i = 0; i < json.List.length; i++) {
                    if (json.List[i].DictionaryTypeId == 1) {
                        typeOfCurrency += '<option value="' + json.List[i].DValue + '"> ' + json.List[i].Remarks + ' </option>';
                    }

                    if (json.List[i].DictionaryTypeId == 17) {
                        workType += '<option value="' + json.List[i].DKey + '"> ' + json.List[i].DValue + ' </option>';
                    }

                    if (json.List[i].DictionaryTypeId == 18) {
                        payrollType += '<option value="' + json.List[i].DKey + '"> ' + json.List[i].DValue + ' </option>';
                    }

                    if (json.List[i].DictionaryTypeId == 10) {
                        languageType += '<option value="' + json.List[i].DValue + '"> ' + json.List[i].Remarks + ' </option>';
                    }
                }
                if (type == 1) {
                    $("#typeOfCurrency").html(typeOfCurrency);//货币类型
                    //$("#workType").html(workType);//工作类型
                    //$("#payrollType").html(payrollType);//薪资类型
                    //$("#languageType").html(languageType);//语言类型

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


//art.artDialog.tips("短暂提示-无图标！", "", "5");//短暂提示-无图标
//art.artDialog.alert("警告！", "");//警告
//art.artDialog.alertTips("短暂提示 - 警告！", "", "3"); //短暂提示 - 警告
//art.artDialog.succeedTips("短暂提示 - 成功！", "", "2"); //短暂提示 - 成功
//art.artDialog.errorTips("（删除）系统错误！", "", "1");//短暂提示-错误
