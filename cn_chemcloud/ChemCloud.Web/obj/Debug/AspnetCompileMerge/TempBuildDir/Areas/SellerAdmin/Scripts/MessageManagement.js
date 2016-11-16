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
            queryParams: { Status: val },
            operationButtons: "#saleOff",
            onLoadSuccess: CheckStatus,
            columns:
			[[
                 {
                     checkbox: true, width: 20, align: "center", formatter: function (value, row, index) {
                         return '<input type="checkbox">';
                     }
                 },
                 { field: "Id", title: "Id", hidden: true },
                { field: "MessageModule", title: "消息名称", width: 100, align: "center" },
				{ field: "Status", title: "阅读状态", width: 100, align: "center" },
                { field: "SendName", title: "发送人", width: 100 },
                { field: "SendTime", title: "发送时间", width: 100, align: "center",
                formatter: function (value, row, index) {
                    var da = new Date(parseInt(row.SendTime.replace("/Date(", "").replace(")/", "").split("+")[0]));
                    return da.getFullYear() + "-" + (da.getMonth() + 1) + "-" + da.getDate() + " " +da.getHours() + ":" + da.getMinutes() + ":" + da.getSeconds();
                       
                 }
                },
		    	{
		    	    field: "operation", operation: true, title: "操作", width: 120,
		    	    formatter: function (value, row, index) {
		    	        var html = ["<span class=\"btn-a\">"];
		    	        html.push('<a href="./Detail?id=' + row.Id + '">查看</a>');
		    	        html.push("<a onclick=\"Delete('" + row.Id + "')\">删除</a>");
		    	        if (row.Status == "未读") {
		    	            html.push("<a onclick=\"UpdateType('" + row.Id + "')\">标为已读</a>");
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
        var Status = $('select[name="Status"]').val();
        var MessageName = $('select[name="MessageName"]').val();
        $(".tabel-operate").find("label").remove();
        $("#list").hiMallDatagrid('reload', { startDate: startDate, endDate: endDate, Status: Status, MessageModule: MessageName });
    });


    $('.nav li').click(function (e) {
        try {
            searchClose(e);
            $(this).addClass('active').siblings().removeClass('active');
            if ($(this).attr('type') == 'statusTab') {//状态分类
                $('#txtOrderId').val('');
                $('#txtuserName').val('');

                $("#list").hiMallDatagrid('reload', { Status: $(this).attr('value') || null });
            }
        }
        catch (ex) {
            alert();
        }
    });

});


function UpdateType(MId) {
    var loading = showLoading();
    $.post('./UpdateType', { Id: MId }, function (result) {
        if (result.success) {
            $.dialog.succeedTips("操作成功！");
        }
        else
            $.dialog.errorTips("操作失败");
        loading.close();
    });
}


function Delete(id) {
    $.dialog.confirm('确定删除该条记录吗？', function () {
        var loading = showLoading();
        $.post("./Delete", { id: id }, function (data) {
            loading.close();
            $.dialog.tips(data.msg);
            query();
        });
    });
}
function BatchDelete() {
    var selectedRows = $("#gridlist").hiMallDatagrid("getSelections");
    var selectids = new Array();
    for (var i = 0; i < selectedRows.length; i++) {
        selectids.push(selectedRows[i].Id);
    }
    if (selectedRows.length == 0) {
        $.dialog.errorTips("你没有选择任何选项！");
    }
    else {
        $.dialog.confirm('确定删除选择的消息吗？', function () {
            var loading = showLoading();
            $.post("./BatchDelete", { ids: selectids.join(',') }, function (data) { $.dialog.tips(data.msg); query(); loading.close(); });
        });
    }
}
