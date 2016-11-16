
$(document).ready(function () {

    getCurrentUserType();

    //查询按钮
    $("#btn_Search").click(function () {
        param = getParam_Buy_Select();//重新确定查询条件
        var isTrue = getParamAddVel();
        if (isTrue) {
            Get_PageInfo_Buy_Web(param);
            getIWantToBuyList(param);
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
    });

    //点击分页按钮触发：采购商
    $("#pageGro1 li").live("click", function () {
        if (pageCount > 5) {
            var pageNum = parseInt($(this).html());//获取当前页数
            pageGroup1("pageGro1", pageNum, pageCount);
        } else {
            $(this).addClass("on");
            $(this).siblings("li").removeClass("on");
        }
        param.query.PageInfo.CurrentPage = parseInt($(this).html());
        getIWantToBuyList(param);
    });
    $("#pageGro1 #preSpanBtn1").click(function () {

        var currentPage = 1;
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro1 li.on").html());//获取当前页
            pageUp1("pageGro1", pageNum, pageCount);
            currentPage = pageNum;

        } else {
            var index = $("#pageGro1 ul li.on").index();//获取当前页
            if (index > 0) {
                $("#pageGro1 li").removeClass("on");//清除所有选中
                $("#pageGro1 ul li").eq(index - 1).addClass("on");//选中上一页
                currentPage = index;
            }
            else {
                art.artDialog.errorTips("已是第一页", "", "1");//短暂提示-错误
                return false;
            }
        }
        param.query.PageInfo.CurrentPage = currentPage;
        getIWantToBuyList(param);
    });
    $("#pageGro1 #nextSpanBtn1").click(function () {

        var currentPage = 1;
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro1 li.on").html());//获取当前页
            pageDown1("pageGro1", pageNum, pageCount);
            currentPage = pageNum;
        } else {
            var index = $("#pageGro1 ul li.on").index();//获取当前页
            if (index + 1 < pageCount) {
                $("#pageGro1 li").removeClass("on");//清除所有选中
                $("#pageGro1 ul li").eq(index + 1).addClass("on");//选中下一页
                currentPage = index + 2;
            }
            else {
                art.artDialog.errorTips("已是第最后一页", "", "1");//短暂提示-错误
                return false;
            }
        }
        param.query.PageInfo.CurrentPage = currentPage;
        getIWantToBuyList(param);
    });

    //点击分页按钮触发：供应商
    $("#pageGro2 li").live("click", function () {
        if (pageCount > 5) {
            var pageNum = parseInt($(this).html());//获取当前页数
            pageGroup1("pageGro2", pageNum, pageCount);
        } else {
            $(this).addClass("on");
            $(this).siblings("li").removeClass("on");
        }
        param.query.PageInfo.CurrentPage = parseInt($(this).html());
        getIWantToSupplyList(param);
    });
    $("#pageGro2 #preSpanBtn2").click(function () {

        var currentPage = 1;
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro2 li.on").html());//获取当前页
            pageUp1("pageGro2", pageNum, pageCount);
            currentPage = pageNum;

        } else {
            var index = $("#pageGro2 ul li.on").index();//获取当前页
            if (index > 0) {
                $("#pageGro2 li").removeClass("on");//清除所有选中
                $("#pageGro2 ul li").eq(index - 1).addClass("on");//选中上一页
                currentPage = index;
            }
            else {
                art.artDialog.errorTips("已是第一页", "", "1");//短暂提示-错误
                return false;
            }
        }
        param.query.PageInfo.CurrentPage = currentPage;
        getIWantToSupplyList(param);
    });
    $("#pageGro2 #nextSpanBtn2").click(function () {

        var currentPage = 1;
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro2 li.on").html());//获取当前页
            pageDown1("pageGro2", pageNum, pageCount);
            currentPage = pageNum;
        } else {
            var index = $("#pageGro2 ul li.on").index();//获取当前页
            if (index + 1 < pageCount) {
                $("#pageGro2 li").removeClass("on");//清除所有选中
                $("#pageGro2 ul li").eq(index + 1).addClass("on");//选中下一页
                currentPage = index + 2;
            }
            else {
                art.artDialog.errorTips("已是第最后一页", "", "1");//短暂提示-错误
                return false;
            }
        }
        param.query.PageInfo.CurrentPage = currentPage;
        getIWantToSupplyList(param);
    });

    //点击分页按钮触发：默认
    $("#pageGro3 li").live("click", function () {
        if (pageCount > 5) {
            var pageNum = parseInt($(this).html());//获取当前页数
            pageGroup1("pageGro3", pageNum, pageCount);
        } else {
            $(this).addClass("on");
            $(this).siblings("li").removeClass("on");
        }
        param.query.PageInfo.CurrentPage = parseInt($(this).html());
        getIWantToSupplyList_Default(param);
    });
    $("#pageGro3 #preSpanBtn3").click(function () {

        var currentPage = 1;
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro3 li.on").html());//获取当前页
            pageUp1("pageGro3", pageNum, pageCount);
            currentPage = pageNum;

        } else {
            var index = $("#pageGro3 ul li.on").index();//获取当前页
            if (index > 0) {
                $("#pageGro3 li").removeClass("on");//清除所有选中
                $("#pageGro3 ul li").eq(index - 1).addClass("on");//选中上一页
                currentPage = index;
            }
            else {
                art.artDialog.errorTips("已是第一页", "", "1");//短暂提示-错误
                return false;
            }
        }
        param.query.PageInfo.CurrentPage = currentPage;
        getIWantToSupplyList_Default(param);
    });
    $("#pageGro3 #nextSpanBtn3").click(function () {

        var currentPage = 1;
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro3 li.on").html());//获取当前页
            pageDown1("pageGro3", pageNum, pageCount);
            currentPage = pageNum;
        } else {
            var index = $("#pageGro3 ul li.on").index();//获取当前页
            if (index + 1 < pageCount) {
                $("#pageGro3 li").removeClass("on");//清除所有选中
                $("#pageGro3 ul li").eq(index + 1).addClass("on");//选中下一页
                currentPage = index + 2;
            }
            else {
                art.artDialog.errorTips("已是第最后一页", "", "1");//短暂提示-错误
                return false;
            }
        }
        param.query.PageInfo.CurrentPage = currentPage;
        getIWantToSupplyList_Default(param);
    });
});

//--------------------------------------------------------------------  采购商 --------------------------------------------------------------------
//采购商：列表
function Get_PageInfo_Buy_Web(param) {
    var paramJson = JSON.stringify(param);
    $.ajax({
        type: "POST",
        async: false,
        url: "/IWantToBuy/Get_PageInfo_Buy_Web",
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
                    art.artDialog.errorTips("查询失败：系统错误11", "", "1.5");//短暂提示-错误
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
                art.artDialog.errorTips("查询失败：系统错误21", "", "1.5");//短暂提示-错误
                return false;
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误31", "", "1.5");//短暂提示-错误
            return false;
        }
    });
}
function getIWantToBuyList(param) {
    var param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        async: false,
        url: "/IWantToBuy/IWantToBuyList_Web",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param1,
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            var tbody = $("#tbody_list");
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
                    var trSupplyList = "";
                    var trSupplyList1 = "";
                    var trSupplyList2 = "";
                    trList += ("<tr>"
                           + "<td class='li_productname'>" + json.List[i].ProductName + "</td>"
                           + "<td class='li_quantity'>" + json.List[i].Quantity + "</td>"
                           + "<td class='li_unit'>" + json.List[i].Unit + "</td>"
                           + "<td class='li_unitprice'>" + json.List[i].UnitPrice + "</td>"
                           + "<td class='li_totalprice'>" + json.List[i].TotalPrice + "</td>"
                           + "<td class='li_createDate'>" + json.List[i].CreateDate + " </td>"
                           + "<td class='li_latestDate'>" + json.List[i].DeliveryDate + " </td>"
                           + "<td class='li_status'>" + json.List[i].StatusStr + " </td>"
                           + "<td class='li_xiala'><span class='xiala' id='xiala1' onclick=onMore(" + i + ")><img src='/Areas/Web/Images/2013100702.gif' alt='Alternate Text' /></span></td><td></td>"
                       + "</tr>"
                       + "<tr class='tr_detail'>"
                              + "<td colspan='9'>"
                                  + "<div class='detailed' id='detailed" + i + "'>"
                                  + "<table id='table" + json.List[i].Id + "' class='talbeSupplyList' >"
                                  );
                    for (var j = 0; j < json.List[i].SupplyList.length; j++) {
                        trSupplyList += ("<tr class='tr_toubiaolist'>"
                            + "<td></td>"
                            + "<td>" + json.List[i].SupplyList[j].UnitPrice + "</td>"
                            + "<td>" + json.List[i].SupplyList[j].Unit + "</td>"
                            + "<td>" + json.List[i].SupplyList[j].Quantity + "</td>"
                            + "<td>" + json.List[i].SupplyList[j].TotalPrice + "</td>"
                            + "<td>" + json.List[i].SupplyList[j].LatestDeliveryTime + "</td>"
                            + "<td>" + json.List[i].SupplyList[j].ShopName + "</td>"
                            + "<td>" + json.List[i].SupplyList[j].StatusStr + "</td>"
                            + "<td>"
                            + (json.List[i].SupplyList[j].Status == 0 ? "<span class='span_wytb_hui1' onclick='ChangeStatus(" + json.List[i].SupplyList[j].Status + "," + json.List[i].Id + "," + json.List[i].SupplyList[j].Id + ")' enabled='false'>设为中标</span>"
                            : (json.List[i].SupplyList[j].Status == 1 ? "<span class='span_wytb_lan1' onclick='ChangeStatus(" + json.List[i].SupplyList[j].Status + "," + json.List[i].Id + "," + json.List[i].SupplyList[j].Id + ")'>取消中标</span>"
                            : (json.List[i].SupplyList[j].Status == 2 ? "<span class='span_wytb_hui1' onclick='ChangeStatus(" + json.List[i].SupplyList[j].Status + "," + json.List[i].Id + "," + json.List[i].SupplyList[j].Id + ")' enabled='true'>设为中标</span>"
                            : (json.List[i].SupplyList[j].Status == 3 ? "<span class='span_wytb'>————</span>"
                            : (json.List[i].SupplyList[j].Status == 4 ? "<span class='span_wytb'>————</span>"
                            : (json.List[i].SupplyList[j].Status == 5 ? "<span class='span_wytb'>————</span>"
                            : (json.List[i].SupplyList[j].Status == 6 ? "<span class='span_wytb'>————</span>"
                            : (json.List[i].SupplyList[j].Status == 7 ? "<span class='span_wytb'>————</span>"
                            : "<span class='span_wytb_hui1'>状态异常</span>"))))))))
                            + "</td>"
                            + "</tr>");
                    }
                    trSupplyList1 = "<tr  class='tr_toubiaohead'>"
                            + "<td></td>"
                            + "<td>单价</td>"
                            + "<td>单位</td>"
                            + "<td>数量</td>"
                            + "<td>总报价</td>"
                            + "<td>交付日期</td>"
                            + "<td>竞价者</td>"
                            + "<td>竞价状态</td>"
                            + "<td>操作</td>"
                            + "</tr>";
                    trSupplyList2 = "</table>"
                                  + "<div>"
                              + "</td>"
                          + "</tr>";
                    trList = trList + trSupplyList1 + trSupplyList + trSupplyList2;

                }
                tbody.html(trList);
            }
            else {
                var trList = "<tr><td colspan='5'>无数据</td></tr>";
                tbody.html(trList);
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误", "", "1.5");//短暂提示-错误
        }
    });
}
function getParam_Buy_Select() {

    var paramSelect = {
        "query": {
            "ParamInfo": {

            },
            "PageInfo": {
                "CurrentPage": 1,
                "Total": 0,
                "PageCount": 0,
                "PageSize": 5
            }
        }
    }
    return paramSelect;
}

//采购商：设为中标，取消中标
function ChangeStatus(status, iWantToBuyId, Id, currentStatus) {
    if (getParam_UpdateStatusVal(status)) {
        var param = getParam_UpdateStatus(status, iWantToBuyId, Id);
        param = JSON.stringify(param)
        $.ajax({
            type: "POST",
            url: "/IWantToBuy/IWantToBuy_UpdateStatus",
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
                    location.href = "/Iwanttobuy/Iwanttobuy";
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
}
function getParam_UpdateStatus(status, iWantToBuyId, Id) {
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
                "PageSize": 5
            }
        }
    }
    return paramSelect;
}
function getParam_UpdateStatusVal(status) {
    var message = "";
    switch (status) {
        case 0://公示中
            message = "";
            break;
        case 1://废弃采购
            message = "";
            break;
        case 2://终止公示
            message = "";
            break;
        case 3://确已定
            message = "确已定，无法不能修改";
            break;
        case 4://已下单
            message = "已下单，无法不能修改";
            break;
        case 5://已支付
            message = "已支付，无法不能修改";
            break;
        case 6://已发货
            message = "已发货，无法不能修改";
            break;
        case 7://已收货
            message = "已收货，无法不能修改";
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

//采购商：新增
function IWantToBuy_Add_Web() {
    var param = getParamAdd();
    var isTrue = getParamAddVel(param);

    if (isTrue) {
        param = JSON.stringify(param)
        span_AddFun(param);
    }
    else {
        return false;
    }
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
    var typeOfCurrency = $("#select_TypeOfCurrency").val();


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
                "PageSize": 5
            }
        }
    }
    return paramSelect;
}
function getParamAddVel() {
    var message = "";

    var productName = $("#txt_ProductName").val();
    var quantity = $("#txt_Quantity").val();
    var unitPrice = $("#txt_UnitPrice").val();
    var totalPrice = $("#txt_TotalPrice").val();
    var unit = $("#txt_Unit").val();
    var deliveryDate = $("#txt_DeliveryDate").val();
    var address = $("#txt_Address").val();
    var remarks = $("#txtArea_Remarks").val();

    if (productName.length == 0) {
        message += "请输入产品名称<br/>";
    }
    if (quantity.length == 0) {
        message += "请输入数量<br/>";
    }
    //else if (!parseInt(quantity)) {
    //    message += "数量必须为整数<br/>";
    //}
    if (unitPrice.length == 0) {
        message += "请输入单价<br/>";
    }
    else if (isNaN(unitPrice)) {
        message += "单价必须为数字<br/>";
    }
    if (totalPrice.length == 0) {
        message += "请输入总价<br/>";
    }
    else if (isNaN(totalPrice)) {
        message += "总价必须为数字<br/>";
    }
    if (unit.length == 0) {
        message += "请输入单位<br/>";
    }
    if (deliveryDate.length == 0) {
        message += "请选择交付日期<br/>";
    }
    if (address.length == 0) {
        message += "请输入收货地址<br/>";
    }
    //if (remarks.length == 0) {
    //    message += "请输入备注信息<br/>";
    //}

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
        url: "/IWantToBuy/IWantToBuy_Add_Web",
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
                art.artDialog.succeedTips("提交成功！", "", "1.5"); //短暂提示 - 成功
                location.href = "/Iwanttobuy/Iwanttobuy";
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

//采购商：下拉
function onMore(id) {
    console.log(id);
    var id_ = "#detailed" + id;
    if ($(id_).css("display") == 'block') {
        $(id_).css("display", "none");
        $(id_).parent().parent().css("display", "none");
    } else {
        $(id_).css("display", "block");
        $(id_).parent().parent().css("display", "table-row");
    }
}


//--------------------------------------------------------------------  供应商 --------------------------------------------------------------------
//供应商：列表
function Get_PageInfo_Supply_Web(param) {
    var paramJson = JSON.stringify(param);
    $.ajax({
        type: "POST",
        async: false,
        url: "/IWantToBuy/Get_PageInfo_Supply_Web",
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
function getIWantToSupplyList(param) {
    var param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        async: false,
        url: "/IWantToBuy/IWantToSupplyList_Web",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param1,
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            var tbody = $("#table_Supplier_tbody_list");
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
                    trList += ("<tr>"
                           + "<td class='li_productname'>" + json.List[i].ProductName + "</td>"
                           + "<td class='li_totalprice'>" + json.List[i].TotalPrice + "</td>"
                           + "<td class='li_createDate'>" + json.List[i].CreateDate + " </td>"
                           + "<td class='li_latestDate'>" + json.List[i].DeliveryDate + " </td>"
                           + "<td class='li_status'>" + json.List[i].StatusStr + " </td>"
                           //+ "<td class='li_xiala'><span class='span_wytb_hui' onclick=onMore1(" + i + ")>我要报价</span></td>"
                           + "<td class='li_xiala' style='text-align:center;'>" + (json.List[i].SupplyModel.Id == 0 ? "<span class='span_wytb_hui2' onclick=onMore1(" + i + ")>我要报价</span>" : "<span class='span_wytb_hui' onclick=onMore1(" + i + ")>修改报价</span>") + "</td>"
                       + "</tr>"

                      + (json.List[i].SupplyModel.Model.Id == 0 ? ("<tr class='tr_detail toubiao' id='toubiao" + i + "'>"
                              + "<td class='li_productname'>单价：<input type='text' class='input_toubiao unitPrice' ></td>"
                              + "<td class='li_totalprice'>总价：<input type='text' class='input_toubiao totalPrice' ></td>"
                              + "<td class='li_createDate'>数量：<input type='text' class='input_toubiao quantity' ></td>"
                              + "<td class='li_latestDate'>交付时间：<input type='text' onclick=dateTime_YMD_CN(this)  class='input_toubiao deliveryDate'></td>"
                              + "<td class='li_status'></td>"
                          + "") : ("<tr class='tr_detail toubiao' id='toubiao" + i + "'>"
                              + "<td class='li_productname'>单价：<input type='text' class='input_toubiao unitPrice' value=" + json.List[i].SupplyModel.Model.UnitPrice + "></td>"
                              + "<td class='li_totalprice'>总价：<input type='text' class='input_toubiao totalPrice' value=" + json.List[i].SupplyModel.Model.TotalPrice + "></td>"
                              + "<td class='li_createDate'>数量：<input type='text' class='input_toubiao quantity' value=" + json.List[i].SupplyModel.Model.Quantity + "></td>"
                              + "<td class='li_latestDate'>交付时间：<input type='text' onclick=dateTime_YMD_CN(this) value=" + json.List[i].SupplyModel.Model.LatestDeliveryTime + " class='input_toubiao deliveryDate'></td>"
                              + "<td class='li_latestDate'>放弃<input type='checkbox' class='checkbox_fangqi' " + (json.List[i].SupplyModel.Model.Status == 3 ? "checked=checked" : "") + "'></td>"
                          ))
                          + "<td class='li_xiala'><span class='span_wytb_lan' onclick=IWantToSupply_Add(this," + json.List[i].SupplyModel.Model.Id + "," + json.List[i].Id + "," + json.List[i].Status + ")>提交确认</span></td>"
                          + "</tr>"
                            );
                }
                tbody.html(trList);
            }
            else {
                var trList = "<tr><td colspan='5'>无数据</td></tr>";
                tbody.html(trList);
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误", "", "1.5");//短暂提示-错误
        }
    });
}
function getParam_Supply_Select(obj, supplyId, iWantToBuyId) {

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
                "PageSize": 5
            }
        }
    }
    return paramSelect;
}

//供应商：新增（提交报价）
function IWantToSupply_Add(obj, supplyId, iWantToBuyId, buyStatus) {

    var param = getParam_Supply_Add(obj, supplyId, iWantToBuyId);
    var isTrue = getParamVel_Supply_Add(param, buyStatus);

    if (isTrue) {
        param = JSON.stringify(param)
        span_AddFun_Supply(param);
    }
    else {
        return false;
    }
}
function getParam_Supply_Add(obj, supplyId, iWantToBuyId) {

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
                "PageSize": 5
            }
        }
    }
    return paramSelect;
}
function getParamVel_Supply_Add(param, buyStatus) {
    var message = "";

    var quantity = param.query.ParamInfo.Quantity;
    var unitPrice = param.query.ParamInfo.UnitPrice;
    var totalPrice = param.query.ParamInfo.TotalPrice;
    var deliveryDate = param.query.ParamInfo.LatestDeliveryTime;

    if (buyStatus == 1) {
        message += "该采购已废弃，不能参与报价，且报价不能修改";
    }
    else if (buyStatus == 2) {
        message += "该采购已终止，不能受理报价，且报价不能修改";
    }
    else if (buyStatus == 3) {
        message += "该采购已确定，不再受理报价，且报价不能修改";
    }
    else if (buyStatus == 4) {
        message += "该采购已下单确认，不再受理报价，且报价不能修改";
    }
    else if (buyStatus == 5) {
        message += "该采购已付款，不再受理报价，且报价不能修改";
    }
    else {
        if (quantity.length == 0) {
            message += "请输入数量<br/>";
        }
        //else if (parseInt(quantity)) {
        //    message += "数量必须为整数<br/>";
        //}
        if (unitPrice.length == 0) {
            message += "请输入单价<br/>";
        }
        else if (isNaN(unitPrice)) {
            message += "单价必须为数字<br/>";
        }
        if (totalPrice.length == 0) {
            message += "请输入总价<br/>";
        }
        else if (isNaN(totalPrice)) {
            message += "总价必须为数字<br/>";
        }
        if (deliveryDate.length == 0) {
            message += "请选择交付日期<br/>";
        }
    }
    if (message.length > 0) {
        art.artDialog.errorTips(message, "", "3");//短暂提示-错误
        return false;
    }
    else {
        return true;
    }

}
function span_AddFun_Supply(param) {
    $.ajax({
        type: "POST",
        url: "/IWantToBuy/IWantToSupply_AddOrUpdate",
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
                art.artDialog.succeedTips(json.Message, "", "1.5"); //短暂提示 - 成功

                location.href = "/Iwanttobuy/Iwanttobuy";
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

//供应商：下拉
function onMore1(id) {
    console.log(id);
    var id_ = "#toubiao" + id;
    if ($(id_).css("display") == 'table-row') {
        $(id_).css("display", "none");
    } else {
        $(id_).css("display", "table-row");
    }
}


//默认加载：列表
function Get_PageInfo_Default_Web(param) {
    var paramJson = JSON.stringify(param);
    $.ajax({
        type: "POST",
        async: false,
        url: "/IWantToBuy/Get_PageInfo_Default_Web",
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
function getIWantToSupplyList_Default(param) {
    var param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        async: false,
        url: "/IWantToBuy/IWantToSupplyList_Web_Default",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param1,
        success: function (data) {
            var json = "";
            if (data.hasOwnProperty('d')) {
                json = data.d;
            } else {
                json = data;
            }

            var tbody = $("#table_LoginOut_tbody_list");
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
                    trList += ("<tr>"
                           + "<td class='li_productname'>" + json.List[i].ProductName + "</td>"
                           + "<td class='li_totalprice'>" + json.List[i].TotalPrice + "</td>"
                           + "<td class='li_createDate'>" + json.List[i].CreateDate + " </td>"
                           + "<td class='li_latestDate'>" + json.List[i].DeliveryDate + " </td>"
                           + "<td class='li_status'>" + json.List[i].StatusStr + " </td>"
                           //+ "<td class='li_xiala'><span class='span_wytb_hui' onclick=onMore1(" + i + ")>我要报价</span></td>"
                           + "<td style='text-align:center;'><span class='span_wytb_hui2' onclick=getParamVel_Default()>我要报价</span></td>"
                       + "</tr>");

                    //+ (json.List[i].SupplyModel.Id == 0 ? ("<tr class='tr_detail toubiao' id='toubiao" + i + "'>"
                    //        + "<td class='li_productname'>单价：<input type='text' class='input_toubiao unitPrice' ></td>"
                    //        + "<td class='li_totalprice'>总价：<input type='text' class='input_toubiao totalPrice' ></td>"
                    //        + "<td class='li_createDate'>数量：<input type='text' class='input_toubiao quantity' ></td>"
                    //        + "<td class='li_latestDate'>交付时间：<input type='text' onclick=dateTime_YMD_CN(this)  class='input_toubiao deliveryDate'></td>"
                    //        + "<td class='li_status'></td>"
                    //    + "") : ("<tr class='tr_detail toubiao' id='toubiao" + i + "'>"
                    //        + "<td class='li_productname'>单价：<input type='text' class='input_toubiao unitPrice' value=" + json.List[i].SupplyModel.UnitPrice + "></td>"
                    //        + "<td class='li_totalprice'>总价：<input type='text' class='input_toubiao totalPrice' value=" + json.List[i].SupplyModel.TotalPrice + "></td>"
                    //        + "<td class='li_createDate'>数量：<input type='text' class='input_toubiao quantity' value=" + json.List[i].SupplyModel.Quantity + "></td>"
                    //        + "<td class='li_latestDate'>交付时间：<input type='text' onclick=dateTime_YMD_CN(this) value=" + json.List[i].SupplyModel.LatestDeliveryTime + " class='input_toubiao deliveryDate'></td>"
                    //        + "<td class='li_latestDate'>放弃<input type='checkbox' class='checkbox_fangqi' " + (json.List[i].SupplyModel.Status == 3 ? "checked=checked" : "") + "'></td>"
                    //    ))
                    //    + "<td class='li_xiala'><span class='span_wytb_lan' onclick=IWantToSupply_Add(this," + json.List[i].SupplyModel.Id + "," + json.List[i].Id + "," + json.List[i].Status + ")>提交确认</span></td>"
                    //    + "</tr>"
                    //      );
                }
                tbody.html(trList);
            }
            else {
                var trList = "<tr><td colspan='5'>无数据</td></tr>";
                tbody.html(trList);
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误", "", "1.5");//短暂提示-错误
        }
    });
}
function getParam_Supply_Select_Default(obj, supplyId, iWantToBuyId) {

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
                "PageSize": 5
            }
        }
    }
    return paramSelect;
}
function getParamVel_Default() {
    //var message = "";

    //if (buyStatus == 1) {
    //    message += "该采购已废弃，不能参与报价，且报价不能修改";
    //}
    //else if (buyStatus == 2) {
    //    message += "该采购已终止，不能受理报价，且报价不能修改";
    //}
    //else if (buyStatus == 3) {
    //    message += "该采购已确定，不再受理报价，且报价不能修改";
    //}
    //else if (buyStatus == 4) {
    //    message += "该采购已下单确认，不再受理报价，且报价不能修改";
    //}
    //else if (buyStatus == 5) {
    //    message += "该采购已付款，不再受理报价，且报价不能修改";
    //}

    //if (message.length > 0) {
    //    art.artDialog.errorTips(message, "", "3");//短暂提示-错误
    //    return false;
    //}
    //else {
    //    return true;
    //}
    art.artDialog.errorTips("您尚未登录，请登陆后再行操作", "", "3");//短暂提示-错误
}


//---------------------------------------------  根据登录账户，加载不同页面 --------------------------------------------
function getCurrentUserType() {
    var param1 = getParam_Buy_Select();
    var param2 = JSON.stringify(param1)
    $.ajax({
        type: "POST",
        async: false,
        url: "/IWantToBuy/GetCurrentUserType",
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

            if (json == null) {
                art.artDialog.errorTips("返回json数据为空！", "", "1");//短暂提示-错误
                return false;
            }
            if (!json.Msg.IsSuccess) {
                //采购商
                $("#table_Form").css("display", "none");
                $("#iWantToBuyList").css("display", "none");
                $("#table_Supplier").css("display", "none");

                //供应商
                $("#table_LoginOut").css("display", "table");
                $("#pageGro1").css("display", "none");
                $("#pageGro2").css("display", "none");
                $("#pageGro3").css("display", "block");

                param = getParam_Supply_Select_Default();
                Get_PageInfo_Default_Web(param);
                getIWantToSupplyList_Default(param);

                pageCount = param.query.PageInfo.PageCount;//模拟后台总页数
                //生成分页按钮
                if (pageCount > 5) {
                    page_icon1("pageGro3", 1, 5, 0);
                } else {
                    page_icon1("pageGro3", 1, pageCount, 0);
                }
            }
            else {
                //3：采购商；2：供应商；
                if (json.Model.UserType == 3) {
                    //采购商
                    $("#table_Form").css("display", "table");
                    $("#iWantToBuyList").css("display", "table");

                    //供应商
                    $("#table_Supplier").css("display", "none");
                    $("#pageGro1").css("display", "block");
                    $("#pageGro2").css("display", "none");
                    $("#pageGro3").css("display", "none");

                    param = getParam_Buy_Select();
                    Get_PageInfo_Buy_Web(param);
                    getIWantToBuyList(param);

                    pageCount = param.query.PageInfo.PageCount;//模拟后台总页数
                    //生成分页按钮
                    if (pageCount > 5) {
                        page_icon1("pageGro1", 1, 5, 0);
                    } else {
                        page_icon1("pageGro1", 1, pageCount, 0);
                    }
                }
                else if (json.Model.UserType == 2) {
                    //采购商
                    $("#table_Form").css("display", "none");
                    $("#iWantToBuyList").css("display", "none");

                    //供应商
                    $("#table_Supplier").css("display", "table");
                    $("#pageGro1").css("display", "none");
                    $("#pageGro2").css("display", "block");
                    $("#pageGro3").css("display", "none");

                    param = getParam_Supply_Select();
                    Get_PageInfo_Supply_Web(param);
                    getIWantToSupplyList(param);

                    pageCount = param.query.PageInfo.PageCount;//模拟后台总页数
                    //生成分页按钮
                    if (pageCount > 5) {
                        page_icon1("pageGro2", 1, 5, 0);
                    } else {
                        page_icon1("pageGro2", 1, pageCount, 0);
                    }
                }
                else {

                }
            }
        }
    });
}
function openMeetionInfo(id) {

}