
$(function () {
    var status = GetQueryString("status");
    var li = $("li[value='" + status + "']");
    if (li.length > 0) {
        li.addClass('active').siblings().removeClass('active');
    }

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
        pageSize: 15,
        pagePosition: 'bottom',
        pageNumber: 1,
        queryParams: { orderStatus: status },
        columns:
        [[
            {
                checkbox: true, witdh: 35, formatter: function (value, row, index) {
                    if (row.OrderStatus == "已支付") {
                        return '<input type="checkbox" >';
                    } else {
                        return '<input type="checkbox" disabled>';
                    }
                }
            },
            {
                field: "OrderId", title: '订单号', width: 170
                //formatter: function (value, row, index) {
                //    return '<img src="' + row.IconSrc + '" title="' + row.PlatformText + '订单" width="16" />' + value;
                //}
            },
            { field: "CompanyName", title: "供应商", width: 180, align: "center" },
            { field: "UserName", title: "采购商", width: 150, align: "center" },
            { field: "OrderDate", title: "下单时间", width: 150, align: "center" },
            {
                field: "TotalPrice", title: "订单总额", width: 80, align: "center",
                formatter: function (value, row, index) {
                    return '￥' + value.toFixed(2);
                }
            },

            /*{ field: "PaymentTypeName", title: "支付方式", width: 80, align: "left" },*/

         {
             field: "OrderStatus", title: "订单状态", width: 104, align: "center",
             formatter: function (value, row, index) {
                 var reslut = "";
                 if (row.IsBehalfShip == "1" && row.BehalfShipNumber == null) {
                     reslut = "代发未处理";
                 }
                 else {
                     reslut = row.OrderStatus;
                 }
                 return reslut;
             }

         },

        {
            field: "IsBehalfShip", title: "发货方式", width: 200, align: "center",
            formatter: function (value, row, index) {
                var strhtml = "";
                if (row.IsBehalfShip == "1") {
                    strhtml = "<span style='color:red;'>平台代发货<span>";
                } else {
                    strhtml = "<span>供应商发货<span>";
                }
                return strhtml;
            }
        },

        {
            field: "operation", operation: true, title: "操作", width: 550,
            formatter: function (value, row, index) {
                var id = row.OrderId;
                var sta = row.OrderStatus;

                var html = ["<span class=\"btn-a\">"];
                html.push("<a style='width:40px;display: inline-block;' href='./Detail/" + id + "'>查看</a>");

                html.push("<a onclick='NewPrintOrder(" + id + ")'>打印订单</a>");

                if (sta == "待付款") {
                    html.push("<a class=\"good-check\" onclick=\"OpenConfirmPay('" + id + "')\">确认收款</a>");
                    html.push("<a class=\"good-check\" onclick=\"OpenCloseOrder('" + id + "')\">取消</a>");
                }
                if (row.OrderStatus == "已支付") {
                    html.push("<a class=\"good-check\" onclick=\"PrintInvoice('" + id + "')\">打印发票</a>");
                    if (row.IsBehalfShip == "1") {
                        //html.push("<a class=\"good-check\"  href='./BehalfShip/" + id + "'>代发货</a>");
                        html.push("<a href='/Admin/Order/CreateShip?orderid=" + id + "'>代发货</a>");
                    }
                }

                if (row.OrderStatus == "已发货") {
                    if (row.IsBehalfShip == "1") {
                        html.push("<a href='/Admin/Order/PrintShipOrder?orderid=" + id + "'>打印物流面单</a>");
                    }
                    else {
                        html.push("<a onclick='NewPrintExpress(" + id + ")'>打印物流面单</a>");
                    }

                }

                html.push("</span>");
                return html.join("");
            }
        }
        ]]
    });

    $('#searchButton').click(function (e) {
        searchClose(e);
        var startDate = $("#inputStartDate").val();
        var endDate = $("#inputEndDate").val();
        var orderId = $('#txtOrderId').val();
        var orderId = $.trim($('#txtOrderId').val());
        if (isNaN(orderId)) {
            $.dialog.errorTips("请输入正确的查询订单号"); return false;
        }
        var shopName = $('#txtShopName').val();
        var userName = $('#txtUserName').val();
        $(".tabel-operate").find("label").remove();
        $("#list").hiMallDatagrid('reload', { startDate: startDate, endDate: endDate, orderId: orderId, shopName: shopName, userName: userName, paymentTypeGateway: $("#selectPaymentTypeName").val() });
    })


    $('.nav li').click(function (e) {
        searchClose(e);
        $(this).addClass('active').siblings().removeClass('active');
        if ($(this).attr('type') == 'statusTab') {//状态分类
            $('#txtOrderId').val('');
            $('#txtShopName').val('');
            $('#txtuserName').val('');
            $("#list").hiMallDatagrid('reload', { orderStatus: $(this).attr('value') || null, pageNumber: 1, startDate: '', endDate: '', orderId: '', shopName: '', userName: '', paymentTypeGateway: '' });
        }
    });
});

function SaveWL(str) {
    art.dialog({
        title: '设置物流单号',
        id: 'testID',
        content: '物流单号:<input type=\"text\" class=\"admintext\">',
        button: [
            {
                name: '保存',
                callback: function () {
                    var addWLContent = $(".admintext").val();
                    if (addWLContent == "") {
                        $(".admintext").focus();
                        return false;
                    }
                    $.post('./AddWLInfo', { "orderId": str, "wlnum": addWLContent }, function (result) {
                        if (result == "yes") {
                            location.reload(true);
                        }
                    });
                },
                focus: true
            },
            {
                name: '取消'
            }
        ]
    });
};

function OpenConfirmPay(orderId) {

    $.dialog({
        title: '确认收款',
        lock: true,
        id: 'goodCheck',
        content: ['<div class="dialog-form">',
            '<div class="form-group">',
                '<p class="help-top">收款备注</p>',
                '<textarea id="txtPayRemark" class="form-control" cols="40" rows="2" onkeyup="this.value = this.value.slice(0, 50)" ></textarea>\
                 <p id="valid" style="visibility:hidden;color:red">请填写未通过理由</p> ',
            '</div>',
        '</div>'].join(''),
        padding: '10px',
        init: function () { $("#txtPayRemark").focus(); },
        button: [
        {
            name: '确认收款',
            callback: function () {
                ConfirmPay(orderId, $('#txtPayRemark').val());
            },
            focus: true
        }]
    });
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

function ConfirmPay(orderId, payRemark) {
    var loading = showLoading();
    $.post('./ConfirmPay', { orderId: orderId, payRemark: payRemark }, function (result) {
        if (result.success) {
            $.dialog.succeedTips("操作成功！");
            var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
            $("#list").hiMallDatagrid('reload', { pageNumber: pageNo });
        }
        else
            $.dialog.errorTips("操作失败" + result.msg);
        loading.close();
    });
}

function CloseOrder(orderId) {
    var loading = showLoading();
    $.post('./CloseOrder', { orderId: orderId }, function (result) {
        if (result.success) {
            $.dialog.succeedTips("操作成功！");
            var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
            $("#list").hiMallDatagrid('reload', { pageNumber: pageNo });
        }
        else
            $.dialog.errorTips("操作失败");
        loading.close();
    });
}

//打印订单
function NewPrintOrder(orderid) {
    window.open("/Admin/Order/NewPrintOrder?orderid=" + orderid, "_blank");
}

function NewPrintExpress(orderid) {
    window.open("/Admin/Order/NewPrintExpress?orderid=" + orderid, "_blank");
}

//打印发票
function PrintInvoice(orderId) {
    window.open("/Admin/Order/PrintInvoice?Ids=" + orderId, "_blank");
}

//批量打印发票
function BatchPrintInvoice() {
    var ids = getSelectedIds();
    if (ids.length <= 0) {
        $.dialog.tips('请至少选择一个订单');
        return false;
    }
    window.open("/Admin/Order/PrintInvoice?Ids=" + ids.toString(), "_blank");
}

//勾选框选择
function getSelectedIds() {
    var selecteds = $("#list").hiMallDatagrid('getSelections');
    var ids = [];
    $.each(selecteds, function () {
        ids.push(this.OrderId);
    });
    return ids;
}
