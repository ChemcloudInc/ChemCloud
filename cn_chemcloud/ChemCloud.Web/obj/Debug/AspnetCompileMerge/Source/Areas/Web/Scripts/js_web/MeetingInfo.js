// JavaScript Document
$(function () {

    var endDate = new Date();
    var startDate = getDatedate_YMD(endDate);
    var startDate1 = addDate(startDate, -30);
    var endDate1 = addDate(startDate, 30);
    dateTime_SetDefaultsValue_day("#startDate", "yyyy-mm-dd", startDate1, "zh-CN", "", endDate1, startDate1);
    dateTime_SetDefaultsValue_day("#endDate", "yyyy-mm-dd", endDate1, "zh-CN", "", endDate1, endDate1);


    //根据总页数判断，如果小于5页，则显示所有页数，如果大于5页，则显示5页。根据当前点击的页数生成
    var param = getParamSelect();
    pageCount = 1;//模拟后台总页数
    getJobsList(param);

    //生成分页按钮
    if (pageCount > 5) {
        page_icon(1, 5, 0);
    } else {
        page_icon(1, pageCount, 0);
    }


    //点击分页按钮触发
    $("#pageGro li").live("click", function () {
        if (pageCount > 5) {
            var pageNum = parseInt($(this).html());//获取当前页数
            pageGroup(pageNum, pageCount);
        } else {
            $(this).addClass("on");
            $(this).siblings("li").removeClass("on");
        }

        var param = getParamSelect();
        param.PageNo = parseInt($(this).html());
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
            }
            currentPage = index - 1;
        }
        if (currentPage > 0) {
            var param = getParamSelect();
            param.PageNo = currentPage + 1;
            getJobsList(param);
        }
        else {
            art.artDialog.errorTips("已是第一页", "", "1");//短暂提示-错误
        }
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
                $("#pageGro ul li").eq(index + 1).addClass("on");//选中上一页
            }
            currentPage = index + 1;
        }
        if (currentPage < pageCount) {
            var param = getParamSelect();
            param.PageNo = currentPage + 1;
            getJobsList(param);
        }
        else {
            art.artDialog.errorTips("已是第最后一页", "", "1");//短暂提示-错误
        }
    });

    //查询按钮
    $("#btn_Search").click(function () {
        var param = getParamSelect();
        getJobsList(param);
    });


})


//根据查询条件获取列表
function getJobsList(param) {
    param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        async: false,
        url: "/MeetingInfo/MeetingsList_Web",
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

            var tbody = $("#ul_jobs_list");
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
                //根据总页数判断，如果小于5页，则显示所有页数，如果大于5页，则显示5页。根据当前点击的页数生成
                //生成分页按钮

                pageCount = json.PageInfo.PageCount;//模拟后台总页数

                for (var i = 0; i < json.List.length; i++) {
                    trList += ("<li><table>"
                            + "<tr><td><a itemId='" + json.List[i].Id + "' onclick='span_ModifyFun(this)'>" + json.List[i].Title + "</a></td><td style='text-align:right;'><label class='label_title1'>会议时间：</label><label>" + json.List[i].MeetingTime + "</label></td></tr>"
                            + "<tr class='tr_time'><td colspan='2'><label class='label_title'>会议地点：</label><label class='label_EndDate'>" + json.List[i].MeetingPlace + "</label></td></tr>"
                            + "<tr><td colspan='2'>" + json.List[i].MeetingContent.substring(0, 150) + "</td></tr>"
                            + "</table></li>");
                }
                tbody.html(trList);
            }
            else {
                var trList = "<li>无任何数据<li>";
                tbody.html(trList);
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误", "", "1.5");//短暂提示-错误
        }
    });
}
//初始化查询参数
function getParamSelect() {

    var title = $("#input_keyword").val();
    var startTime = $("#startDate").val();
    var endTime = $("#endDate").val();

    var param = {
        "Title": title,
        "StartTime": startTime,
        "EndTime": endTime,
        "IsAsc": false,
        "PageNo": 1,
        "PageSize": 10,
        "Sort": "desc"
    }
    return param;
}


//-----------修改
function span_ModifyFun(obj) {
    var objSpan = $(obj);
    var param = getParamModify();
    param.Id = objSpan.attr("itemId");
    param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "/MeetingInfo/GetObjectById_Web",
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
            art.artDialog.errorTips("获取信息失败！", "", "1.5");//短暂提示-错误
        }
    });
}
function open_ModifyDialog(json) {
    art.dialog({
        width: 800,
        height: 400,
        id: 'modify_Dialog',
        title: "会议标题：" + json.Model.Title,
        content: '<table>'
                //+ '<tr><td valign="top"><input id="jobTitle" type="text" value="' + json.Model.JobTitle + '" /></td></tr>'
                + '<tr><td valign="top"><lable  id="jobContent"  style="width:750px;"></lable></td></tr>'
                + '</table>',
        lock: true,
        fixed: true,
        ok: function () {

        },
        okValue: '提交'

    });

    $("#jobContent").html(json.Model.MeetingContent);
}

//修改参数
function getParamModify() {
    var param = {
        "Id": 0
    };
    return param;
}
function getParamModify1() {

    var jobTitle = $("#jobTitle").val();
    var endDate = $("#endDate").val();
    var nowDateTime = new Date().toLocaleString();

    var param = {
        "JobTitle": jobTitle,
        //"CreateDate": nowDateTime,
        "UpdateDate": nowDateTime,
        "StartDate": nowDateTime,
        "EndDate": endDate,
    };
    return param;
}