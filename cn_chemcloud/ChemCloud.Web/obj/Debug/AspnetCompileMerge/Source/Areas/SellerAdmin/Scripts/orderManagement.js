

/*物流代发货*/
function BehalfShip(id) {
    $.dialog.confirm("确定平台代发此订单么？", function () {
        /*设置订单为代发*/
        //--IsBehalfShip 0自发1代发
        //--BehalfShipType 物流公司
        //--BehalfShipNumber 物流单号
        $.post('BehalfShip', { 'orderid': id }, function (result) {
            var html = "";
            if (result.success == true) {

                typeChoose(null);
                $.dialog.succeedTips("操作成功！");

            } else {
                $.dialog.succeedTips("操作失败！");
            }
        });

    });
}

$(function () {
    var status = GetQueryString('status');
    if (status === 0) {
        typeChoose(null);
    } else {
        $("#txtOrderStatus").val(status);
        typeChoose(status)
    }

    $('#searchButton').click(function (e) {
        var startDate = $("#inputStartDate").val();
        var endDate = $("#inputEndDate").val();

        var orderId = $.trim($('#txtOrderId').val());
        if (isNaN(orderId)) {
            $.dialog.errorTips("订单号为数字，请输入正确的查询订单号"); return false;
        }
        else if (orderId.length > 20) {
            $.dialog.errorTips("订单号长度过长，不超过20个字符"); return false;
        }
        var userName = $.trim($('#txtUserName').val());

        var orderstatus = $("#txtOrderStatus").val();

        var casno = $.trim($('#txtcasno').val());
        if (casno.length > 20) {
            alert("CAS #长度过长，不超过20个字符");
            return false;
        }
        $(".tabel-operate").find("label").remove();

        $("#list").hiMallDatagrid('reload',
            {
                startDate: startDate,
                endDate: endDate,
                orderId: orderId,
                userName: userName,
                paymentTypeGateway: $("#selectPaymentTypeName").val(),
                orderStatus: orderstatus,
                casno: casno
            });

    });


    $('.nav li').click(function (e) {
        try {
            searchClose(e);
            $(this).addClass('active').siblings().removeClass('active');
            if ($(this).attr('type') == 'statusTab') {//状态分类
                $('#txtOrderId').val('');
                $('#txtuserName').val('');
                if ($(this).attr('value') == 0 || $(this).attr('value') == 2) {
                    var html = '<a href="javascript:downloadOrderList()" class="btn btn-default btn-ssm">订单配货表</a><a href="myPreview()" class="btn btn-default btn-ssm">打印发货单</a><a href="printOrders()" class="btn btn-default btn-ssm">打印快递单</a><a href="sendGood()" class="btn btn-default btn-ssm">批量发货</a>';
                    $(".tabel-operate").html('');
                    $(".tabel-operate").append(html);
                }
                else {
                    $(".tabel-operate").children().remove();
                }
                $("#list").hiMallDatagrid('reload', { orderStatus: $(this).attr('value') || null });
            }
        }
        catch (ex) {
            alert();
        }
    });

    $("#aDownloadProductList").attr("href", "./DownloadProductList?ids=" + getSelectedIds())

    //【物流状态】：鼠标悬停时，Tip显示物流信息
    $(".shipnumber").live("mouseover mouseout", function (event) {
        var shipnumber = $.trim($(this).html());
        var ps = $(this).position();
        if (event.type == 'mouseover') {
            $.post('./GetExpressData', { expressCompanyName: "EMS", shipOrderNumber: shipnumber }, function (result) {
                var html = "";
                if (result != null) {
                    var objdata = result.ExpressContentCN;
                    var obj = jQuery.parseJSON(objdata);

                    var data = obj;
                    for (var i = 0; i < data.length; i++) {
                        html += '<span>' + data[i].time + '：' + data[i].context + '</span><br/>';
                    }
                }
                if (html == "") {
                    html += '<span>该单号暂无物流进展，请稍后再试，或检查公司和单号是否有误。<span>';
                }
                $("#float_box").children('span').html(html);
                $("#float_box").css("border", "2px solid #ccc");
                $("#float_box").css("border-radius", "4px");
                $("#float_box").css("background", "#fff");
                $("#float_box").css("border-radius", "4px");
                $("#float_box").css("padding", "6px");
                $("#float_box").css("position", "absolute");
                $("#float_box").css("left", ps.left - 300); //距离左边距
                $("#float_box").css("top", ps.top + 20); //距离上边距
                $("#float_box").show();
            });

        } else {
            $("#float_box").hide();
        }
    });
});

function typeChoose(val) {
    //订单表格
    $("#list").hiMallDatagrid({
        url: './list',
        nowrap: false,
        rownumbers: true,
        NoDataMsg: '没有找到符合条件的数据',
        border: false,
        fit: true,
        fitColumns: true,
        pagination: true,
        idField: "OrderId",
        pageSize: 14,
        pagePosition: 'bottom',
        pageNumber: 1,
        queryParams: { orderStatus: val },
        operationButtons: "#orderOperate",
        onLoadSuccess: CheckStatus,
        columns:
        [[
            {
                checkbox: true, witdh: 50, formatter: function (value, row, index) {
                    return '<input type="checkbox">';
                }
            },
            { field: "OrderId", title: '订单号', width: 150, align: "center" },
            { field: "ProductName", title: "产品名称", width: 220, align: "center" },
            { field: "UserName", title: "采购商", width: 120, align: "center" },
            { field: "OrderDate", title: "下单日期", width: 140, align: "center" },
            {
                field: "TotalPrice", title: "订单总额", width: 110, align: "center",
                formatter: function (value, row, index) {
                    var html = "<span class='ftx-04'>" + value.toFixed(2) + "</span>";
                    return html;
                }
            },
        { field: "CoinTypeName", title: "货币", width: 60, align: "center" },
        {
            field: "OrderStatus", title: "订单状态", width: 116, align: "center",
            formatter: function (value, row, index) {
                var html = ["<span class='ordstbox'>"];
                html.push(row.OrderStatus);
                html.push("</span>");
                return html.join("");
            }
        },
        {
            field: "ShipOrderNumber", title: "物流单号", width: 100, align: "center",
            formatter: function (value, row, index) {
                if (row.ShipOrderNumber != null && row.ShipOrderNumber != "") {
                    var html = ["<span class='shipnumber'>"];
                    html.push(row.ShipOrderNumber);
                    html.push("</span>");
                    return html.join("");
                } else {
                    var html = ["<span>--</span>"];
                    return html.join("");
                }
            }
        },
        {
            field: "operation", operation: true, title: "操作", width: 300,
            formatter: function (value, row, index) {
                var id = row.OrderId.toString();
                var html = ["<span class=\"btn-a\">"];
                html.push("<a href='./Detail/" + id + "'>查看</a>");
                if (row.OrderStatus == "待支付") {

                    html.push("<a class=\"good-check\" onclick=\"OpenCloseOrder('" + id + "')\">取消</a>");
                }
                if (row.OrderStatus == "已支付") {
                    html.push("<a href='./SendGood?ids=" + id + "'>发货</a>");
                    //if (row.IsBehalfShip == "1") {
                    //    html.push("<a href='./SendGood?ids=" + id + "'>发货</a>");
                    //    html.push("<a style='color:green;text-decoration:none;' javascript:'void(0)'>已申请代发</a>");
                    //}
                    //else {
                    //    html.push("<a href='./SendGood?ids=" + id + "'>发货</a>");
                    //    html.push("<a onclick=\"BehalfShip('" + id + "')\">申请代发</a>");
                    //}
                }
                html.push("</span>");
                return html.join("");
            }
        }
        ]]
    });
}



function CheckStatus() {

    var status = $(".nav li[class='active']").attr("value");

    var sumprice = 0;
    var leg = $("#list tr").length - 1;
    $("#list tr:gt(0):lt(" + leg + ")").each(function () {
        temp = $.trim($(this).children('td').eq(6).children('div').children('span').html()) == "" ? 0 : $.trim($(this).children('td').eq(6).children('div').children('span').html());
        sumprice = sumprice + parseFloat(temp);
    });

    //查询完结 统计总金额
    $(".tabel-operate").append("<span style='color:#ff6600'>共计金额：" + sumprice.toFixed(2) + "</span>");

}


function downloadProductList() {
    var ids = getSelectedIds();
    if (ids.length <= 0) {
        $.dialog.tips('请至少选择一个订单');
        return false;
    }

    loadIframeURL("./DownloadProductList?ids=" + ids.toString());
}

function downloadOrderList() {
    var ids = getSelectedIds();
    if (ids.length <= 0) {
        $.dialog.tips('请至少选择一个订单');
        return false;
    }

    window.open("/SellerAdmin/Order/DownloadOrderList?ids=" + ids.toString());

}

function loadIframeURL(url) {
    var iFrame;
    iFrame = document.createElement("iframe");
    iFrame.setAttribute("src", url);
    iFrame.setAttribute("style", "display:none;");
    iFrame.setAttribute("height", "0px");
    iFrame.setAttribute("width", "0px");
    iFrame.setAttribute("frameborder", "0");
    document.body.appendChild(iFrame);
    // 发起请求后这个iFrame就没用了，所以把它从dom上移除掉
    //iFrame.parentNode.removeChild(iFrame);
    iFrame = null;
}

function myPreview() {
    var orderIds = getSelectedIds();
    if (orderIds.length <= 0) {
        $.dialog.tips('请至少选择一个订单');
        return false;
    }

    var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    var strBodyStyle = "<style>body{margin:0; padding:0;font-family: 'microsoft yahei',Helvetica;font-size:12px;color: #333;}.table-hd{ margin:0;line-height:30px; float:left; background: #f5f5f5;padding:0 10px;  margin-top:30px;}.table-hd strong{font-size:14px;font-weight:normal; float:left}.table-hd span{ font-weight:normal; font-size:12px;float:right}table{border: 1px solid #ddd;width:100%;border-collapse: collapse;border-spacing: 0; font-size:12px; float:left}table th,table td{border:1px solid #ddd;padding: 8px; text-align:center}table th{border-top:0;}</style>";
    $.post('./GetOrderPrint', { ids: orderIds.toString() }, function (result) {
        if (result.success) {
            var strFormHtml = strBodyStyle + "<body>" + result.msg + "</body>"; //这里的”divdiv1“是标签的名称。

            LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4"); //A4纸张纵向打印
            //LODOP.SET_PRINT_STYLE("FontSize", 9);
            LODOP.ADD_PRINT_HTM("0%", "0%", "100%", "100%", strFormHtml);//四个数值分别表示Top,Left,Width,Height
            LODOP.PREVIEW(); //打印预览
            //LODOP.PRINT(); //直接打印
        }
    });
}

function sendGood() {
    var orderIds = getSelectedIds();
    if (orderIds.length <= 0) {
        $.dialog.tips('请至少选择一个订单');
        return false;
    }

    location.href = "./SendGood?ids=" + orderIds.toString();
}

function OpenCloseOrder(orderId) {
    $.dialog({
        title: '取消订单',
        lock: true,
        id: 'goodCheck',
        content: ['<div class="dialog-form">',
            '<div class="form-group">',
                '<p class="help-top">确认要取消订单吗？取消后订单将会是关闭状态。</p>',
            '</div>',
        '</div>'].join(''),
        padding: '10px',
        button: [
        {
            name: '确认取消',
            callback: function () {
                CloseOrder(orderId);
            },
            focus: true
        }]
    });
}

function CloseOrder(orderId) {
    var loading = showLoading();
    $.post('./CloseOrder', { orderId: orderId }, function (result) {
        loading.close();
        if (result.success) {
            $.dialog.succeedTips("操作成功！");
            var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
            $("#list").hiMallDatagrid('reload', { pageNumber: pageNo });
        }
        else
            $.dialog.errorTips("操作失败");
    });
}

function getSelectedIds() {
    var selecteds = $("#list").hiMallDatagrid('getSelections');
    var ids = [];
    $.each(selecteds, function () {
        ids.push(this.OrderId);
    });
    return ids;
}

function printOrders() {
    var ids = getSelectedIds();
    if (ids.length == 0)
        $.dialog.tips('请至少选择一个订单');
    else
        location.href = "print?orderIds=" + ids.toString();
}

function NewPrintOrder(orderid) {
    window.open("/SellerAdmin/Order/NewPrintOrder?orderid=" + orderid, "_blank");
}

function NewPrintInvoice(id) {
    window.open("/SellerAdmin/Order/PrintInvoice?Id=" + id, "_blank");
}

function NewPrintExpress(orderid) {
    window.open("/SellerAdmin/Order/NewPrintExpress?orderid=" + orderid, "_blank");
}


