$(function () {

    var status = GetQueryString('status');

    if (status == 1) {
        typeChoose('1');
    }
    else if (status == 2) {
        typeChoose('2');
    } else {
        typeChoose()
    }

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
            idField: "Id",
            pageSize: 15,
            pagePosition: 'bottom',
            pageNumber: 1,
            queryParams: { BillStatus: val },
            operationButtons: "#orderOperate",
            onLoadSuccess: CheckStatus,
            columns:
			[[
                {
                    checkbox: true, witdh: 35, formatter: function (value, row, index) {
                        return '<input type="checkbox">';
                    }
                },
                { field: "BillNo", title: "询盘单号", width: 100, align: "center" },
				{ field: "MemberName", title: "采购商名称", width: 100, align: "center" },
                { field: "BuyerEmail", title: "采购商邮箱", width: 100, align: "center" },
                { field: "CASNo", title: "CAS#", width: 100, align: "center" },
                { field: "ProudctNum", title: "数量", width: 100, align: "center" },
                { field: "strCreateDate", title: "询盘日期", width: 100 },
                {
                    field: "BillStatus", title: "询盘状态", width: 100, align: "center",
                    formatter: function (value, row, index) {
                        if (row.BillStatus == "1") {
                            return row.BillStatus = "已提交";
                        }
                        else
                            if (row.BillStatus == "2") {
                                return row.BillStatus = "议价中";
                            }
                            else
                                if (row.BillStatus == "4") {
                                    return row.BillStatus = "议价成功";
                                }
                                else
                                    if (row.BillStatus == "3") {
                                        return row.BillStatus = "结束议价";
                                    }
                    }
                },
		    	{
		    	    field: "operation", operation: true, title: "操作", width: 120,
		    	    formatter: function (value, row, index) {
		    	        var html = ["<span class=\"btn-a\">"];
		    	        html.push('<a href="./Detail?updatePrice=true&Id=' + row.Id + '">查看</a>');
		    	        html.push('<a onclick="OpenClosebargain(' + row.BillNo + ') ">删除</a>');
		    	        html.push("</span>");
		    	        return html.join("");
		    	    }
		    	}
			]]
        });
    }

    function ChangeDateFormat(jsondate) {
        jsondate = jsondate.replace("/Date(", "").replace(")/", "");
        if (jsondate.indexOf("+") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("+"));
        }
        else if (jsondate.indexOf("-") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("-"));
        }

        var date = new Date(parseInt(jsondate, 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

        return date.getFullYear()
            + "-"
            + month
            + "-"
            + currentDate
            + ""
            + " "
            + date.getHours()
            + ":"
            + date.getMinutes();
    }

    function CheckStatus() {
        var status = $(".nav li[class='active']").attr("value");
        if (status && status != '1') {

            $(".td-choose").hide();
        }
        else {
            $(".td-choose").show();
        }
    }

    $('#searchButton').click(function (e) {
        searchClose(e);
        var startDate = $("#inputStartDate").val();
        var endDate = $("#inputEndDate").val();
        var BillNo = $.trim($('#txtBillNo').val());
        var buyName = $.trim($('#txtUserName').val());
        if (isNaN(BillNo)) {
            $.dialog.errorTips("请输入正确的查询订单号"); return false;
        }
        var userName = $.trim($('#txtUserName').val());
        $(".tabel-operate").find("label").remove();
        $("#list").hiMallDatagrid('reload', { startDate: startDate, endDate: endDate, BillNo: BillNo, buyName: buyName });
    });


    $('.nav li').click(function (e) {
        try {
            searchClose(e);
            $(this).addClass('active').siblings().removeClass('active');
            if ($(this).attr('type') == 'statusTab') {//状态分类
                $('#txtOrderId').val('');
                $('#txtuserName').val('');

                $("#list").hiMallDatagrid('reload', { BillStatus: $(this).attr('value') || null });
            }
        }
        catch (ex) {
            alert();
        }
    });

});
function OpenClosebargain(bargainno) {

    $.dialog({
        title: '删除询盘',
        lock: true,
        id: 'goodCheck',
        content: ['<div class="dialog-form">',
            '<div class="form-group">',
                '<p class="help-top">确认要删除此条询盘吗？</p>',
            '</div>',
        '</div>'].join(''),
        padding: '10px',
        button: [
        {
            name: '确认删除',
            callback: function () {
                CloseBargain(bargainno);
            },
            focus: true
        }]
    });
}

function CloseBargain(bargainno) {
    var loading = showLoading();
    $.post('./CloseBargain', { bargainno: bargainno }, function (result) {
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

function batchdelete() {
    var orderIds = getSelectedIds();
    if (orderIds.length <= 0) {
        $.dialog.tips('请至少选择一个询盘');
        return false;
    }

    $.dialog({
        title: '批量删除询盘',
        lock: true,
        id: 'goodCheck',
        content: ['<div class="dialog-form">',
            '<div class="form-group">',
                '<p class="help-top">确认要删除所选的询盘吗？</p>',
            '</div>',
        '</div>'].join(''),
        padding: '10px',
        button: [
        {
            name: '确认删除',
            callback: function () {
                funcbatchdelete(orderIds);
            },
            focus: true
        }]
    });
}

function funcbatchdelete(Ids) {
    var loading = showLoading();

    $.post('./BatchCloseBargain', { 'Ids': "'" + Ids + "'" }, function (result) {
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

function getSelectedIds() {
    var selecteds = $("#list").hiMallDatagrid('getSelections');
    var ids = [];
    $.each(selecteds, function () {
        ids.push(this.Id);
    });
    return ids;
}

