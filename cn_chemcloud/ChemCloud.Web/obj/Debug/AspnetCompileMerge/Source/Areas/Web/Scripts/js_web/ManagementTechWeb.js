// JavaScript Document
$(function () {
    var endDate = new Date();
    var startDate = getDatedate_YMD(endDate);
    var startDate1 = addDate(startDate, -30);
    var endDate1 = addDate(startDate, 30);
    dateTime_SetDefaultsValue_day("#startDate", "yyyy-mm-dd", startDate1, "zh-CN", "", endDate1, startDate1);
    dateTime_SetDefaultsValue_day("#endDate", "yyyy-mm-dd", endDate1, "zh-CN", "", endDate1, endDate1);


    //根据总页数判断，如果小于5页，则显示所有页数，如果大于5页，则显示5页。根据当前点击的页数生成
    param = getParamSelect(0);
    Get_PageInfo(param);
    getJobsList(param);

    pageCount = param.query.PageInfo.PageCount;//模拟后台总页数
    //生成分页按钮
    if (pageCount > 5) {
        page_icon(1, 5, 0);
    } else {
        page_icon(1, pageCount, 0);
    }


    //查询按钮
    $("#btn_Search").click(function () {
        param = getParamSelect(0);//重新确定查询条件
        var isTrue = getParamAddVel();
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


    //最新会议
    $("#newMeeting").click(function () {
        var param = getParamSelect(1);
        Get_PageInfo(param);

        param.query.ParamInfo.EndTime = new Date();
        getJobsList(param);
    });


    //历史会议
    $("#oldMeeting").click(function () {
 
        var param = getParamSelect(2);
        Get_PageInfo(param);

        param.query.ParamInfo.EndTime = new Date();
        getJobsList(param);
    });

})


//根据查询条件获取列表
function getJobsList(param) {
    param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        async: false,
        url: "/TechInfo/MeetingsList_Web",
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

            var tbody = $("#ul_imglist");
            var trList = "";

            if (json == null) {
                art.artDialog.errorTips("Json data returned is empty!", "", "1");//短暂提示-错误
                return false;
            }
            if (!json.Msg.IsSuccess) {
                art.artDialog.errorTips("Loading Failed", "", "1");//短暂提示-错误
                return false;
            }
            else if (json.List.length > 0) {
                //根据总页数判断，如果小于5页，则显示所有页数，如果大于5页，则显示5页。根据当前点击的页数生成
                //生成分页按钮

                pageCount = json.PageInfo.PageCount;//模拟后台总页数

                for (var i = 0; i < json.List.length; i++) {

                    if (json.List[i].AttachmentName.length > 0) {
                        var con = delHtmlTag(json.List[i].TechContent, 80);
                        trList += ("<li><div class='img_div'>"
                               + "<a href='#' onclick='openMeetionInfo(" + json.List[i].Id + ")'>"
                                   + "<img src='" + json.List[i].AttachmentName + "' alt=''>"
                                   + "<i class='fack'></i>"
                                   + "<h1 class='img_h1'>"
                                       + "<span class='img_span'>"
                                           + json.List[i].Title
                                       + "</span>"
                                   + "</h1>"
                               + "</a>"
                           + "</div>"
                           + "<div class='text_div text_div_syl'>"
                               + "<p>"
                                   + con
                               + "</p>"
                           + "</div></li>");

                    }
                    else {
                        var con = delHtmlTag(json.List[i].TechContent, 100);
                        trList += ("<li>"
                                + "<div class='a_div'>"
                               + "<a href='#' onclick='openMeetionInfo(" + json.List[i].Id + ")'>"
                                        + json.List[i].Title
                                    + "</a>"
                                + "</div>"
                                + "<div class='text_div'>"
                                    + "<p>"
                                    + con
                                       //+ json.List[i].MeetingContent.length > 100 ? json.List[i].MeetingContent.substr(0, 100) : json.List[i].MeetingContent
                                    + "</p>"
                                + "</div>"
                            + "</li>");
                    }

                }
                tbody.html(trList);
            }
            else {
                var trList = "<li>No data<li>";
                tbody.html(trList);
            }
        },
        error: function () {
            art.artDialog.errorTips("Query failed: System Error", "", "1.5");//短暂提示-错误
        }
    });
}

function delHtmlTag(str, length) {

    var con = "";
    if (str != "" && str != undefined && str != "undefined" && str != null) {
        con = str.replace(/<[^>]+>/g, "");//去掉所有的html标记
        if (con.length > 0) {
            if (con.length >= length) {
                con = con.substring(0, length);
            }
        }
        else {
            con = "";
        }
    }


    return con;
}

//初始化查询参数
function getParamSelect(type) {

    var startTime = $("#startDate").val();
    var endTime = $("#endDate").val();

    var paramSelect = {
        "query": {
            "ParamInfo": {
                "Type": type,
                "StartTime": startTime,
                "EndTime": endTime,
                "LanguageType": 1
            },
            "PageInfo": {
                "CurrentPage": 1,
                "Total": 0,
                "PageCount": 0,
                "PageSize": 8
            }
        }
    }
    return paramSelect;
}

function openMeetionInfo(id) {
    window.location.href = "/TechInfo/ManagementWebDes?id=" + id;
}

//加载
function Get_PageInfo(param) {
    var paramJson = JSON.stringify(param);
    $.ajax({
        type: "POST",
        async: false,
        url: "/TechInfo/Get_PageInfo",
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
                    art.artDialog.errorTips("Return json data is empty", "", "1.5");//短暂提示-错误
                    return false;
                }
                if (!json.Msg.IsSuccess) {
                    art.artDialog.errorTips("System Error", "", "1.5");//短暂提示-错误
                    return false;
                }
                else if (json.Model != null) {
                    param.query.PageInfo.PageCount = json.Model.PageCount;
                }
                else {
                    art.artDialog.errorTips("No data", "", "1.5");//短暂提示-错误
                    return false;
                }
            }
            else {
                art.artDialog.errorTips("System Error", "", "1.5");//短暂提示-错误
                return false;
            }
        },
        error: function () {
            art.artDialog.errorTips("System Error", "", "1.5");//短暂提示-错误
            return false;
        }
    });
}

function getParamAddVel() {

    return true;

}