function query() {
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
        idField: "Id",
        pageSize: 15,
        pagePosition: 'bottom',
        pageNumber: 1,
        queryParams: {},
        columns:
        [[
            { field: "OrderId", title: '订单号', width: 120 },
            { field: "CompanyName", title: "供应商", width: 150, align: "center" },
            { field: "UserName", title: "评价会员", width: 80, align: "center" },

            {
                field: "PackMark", title: "包装满意度", width: 120, align: "center", fromatter: function (value, row, index) {
                    if (row.PackMark == 1) { return "非常不满"; }
                    if (row.PackMark == 2) { return "不满"; }
                    if (row.PackMark == 3) { return "一般"; }
                    if (row.PackMark == 4) { return "满意"; }
                    if (row.PackMark == 5) { return "赞一个"; }
                }
            },
            {
                field: "DeliveryMark", title: "送货满意度", width: 120, align: "center", fromatter: function (value, row, index) {
                    if (row.DeliveryMark == "1") { return "非常不满"; }
                    if (row.DeliveryMark == "2") { return "不满"; }
                    if (row.DeliveryMark == "3") { return "一般"; }
                    if (row.DeliveryMark == "4") { return "满意"; }
                    if (row.DeliveryMark == "5") { return "赞一个"; }
                }
            },
            {
                field: "ServiceMark", title: "服务满意度", width: 130, align: "center", fromatter: function (value, row, index) {
                    if (row.ServiceMark == "1") { return "非常不满"; }
                    if (row.ServiceMark == "2") { return "不满"; }
                    if (row.ServiceMark == "3") { return "一般"; }
                    if (row.ServiceMark == "4") { return "满意"; }
                    if (row.ServiceMark == "5") { return "赞一个"; }
                }
            },

            { field: "CommentDate", title: "评价日期", width: 70, align: "center" },
            {
                field: "operation", operation: true, title: "操作",
                formatter: function (value, row, index) {
                    var id = row.OrderId.toString();
                    var html = ["<span class=\"btn-a\">"];
                    html.push("<a onclick=\"deleteOrderComment('" + row.Id + "');\">删除</a>");
                    html.push("</span>");
                    return html.join("");
                }
            }
        ]]
    });
}


$(function () {

    query();

    $('#searchButton').click(function (e) {
        searchClose(e);
        var startDate = $("#inputStartDate").val();
        var endDate = $("#inputEndDate").val();
        var orderId = $.trim($('#txtOrderId').val());
        var shopName = $.trim($('#txtShopName').val());
        var productName = $.trim($('#txtProductName').val());
        var userName = $.trim($('#txtUserName').val());
        $("#list").hiMallDatagrid('reload', { startDate: startDate, endDate: endDate, orderId: orderId, shopName: shopName, productName: productName, userName: userName });
    })
});

function deleteOrderComment(id) {
    $.dialog.confirm('确定删除该评价吗？', function () {
        var loading = showLoading();
        $.post("./Delete", { id: id }, function (data) { loading.close(); $.dialog.tips(data.msg); query(); });
        var pageNo = $("#list").hiMallDatagrid('options').pageNumber;
        $("#list").hiMallDatagrid('reload', { pageNumber: pageNo });
    });
}

