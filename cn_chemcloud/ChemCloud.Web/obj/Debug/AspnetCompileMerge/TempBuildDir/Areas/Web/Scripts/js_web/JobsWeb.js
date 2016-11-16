
$(document).ready(function () {

    param = getParamSelect();
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
        param = getParamSelect();//重新确定查询条件
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
});


//根据查询条件获取列表
function getJobsList(param) {
    var param1 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        async: false,
        url: "/JobsWeb/JobsList_Web",
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
                $("#span_allCount").html(json.PageInfo.Total);
                for (var i = 0; i < json.List.length; i++) {
                    if (json.List[i].JobContent.length > 200) {
                        var con = delHtmlTag(json.List[i].JobContent, 200);
                        trList += ("<tr>"
                               + "<td class='li_jobTitle'><a href='#' onclick='openMeetionInfo(" + json.List[i].Id + "," + param.query.ParamInfo.WorkType + ")' title='" + json.List[i].JobTitle + "'>" + json.List[i].JobTitle + "</a></td>"
                               + "<td class='li_userId'>" + json.List[i].UserId + "</td>"
                               + "<td class='li_payrolHighLow'>" + json.List[i].Payrol_LowHigh + "</td>"
                               + "<td class='li_workType'>" + json.List[i].WorkType + "</td>"
                               + "<td class='li_createDate'>" + json.List[i].CreateDate + " </td>"
                               + "<td class='li_xiala'><span class='xiala' id='xiala1' onclick=onMore(" + i + ")><img src='/Areas/Web/Images/2013100702.gif' alt='Alternate Text' /></span></td>"
                           + "</tr>"
                              + "<tr class='tr_detail'>"
                                  + "<td colspan='6'>"
                                      + "<div class='detailed' id='detailed" + i + "'>"
                                      + con
                                      + "<div>"
                                  + "</td>"
                              + "</tr>");
                    }
                    else {
                        var con = delHtmlTag(json.List[i].JobContent);
                        trList += ("<tr>"
                               + "<td class='li_jobTitle'><a href='#' onclick='openMeetionInfo(" + json.List[i].Id + "," + param.query.ParamInfo.WorkType + ")' title='" + json.List[i].JobTitle + "'>" + json.List[i].JobTitle + "</a></td>"
                               + "<td class='li_userId'>" + json.List[i].UserId + "</td>"
                               + "<td class='li_payrolHighLow'>" + json.List[i].Payrol_LowHigh + "</td>"
                               + "<td class='li_workType'>" + json.List[i].WorkType + "</td>"
                               + "<td class='li_createDate'>" + json.List[i].CreateDate + " </td>"
                               + "<td class='li_xiala'><span class='xiala' id='xiala1' onclick=onMore(" + i + ")><img src='/Areas/Web/Images/2013100702.gif' alt='Alternate Text' /></span></td>"
                           + "</tr>"
                              + "<tr class='tr_detail'>"
                                  + "<td colspan='6'>"
                                      + "<div class='detailed' id='detailed" + i + "'>"
                                      + con
                                      + "<div>"
                                  + "</td>"
                              + "</tr>");
                    }
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
//初始化查询参数
function getParamSelect() {

    var jobTitle = $("#input_keyword").val();
    var userType = $("#selectUserType_Web").val();
    var workType = $("#webSlectWorkType").val();
    //var languageType = $("#selectLanguage_Web").val();

    var paramSelect = {
        "query": {
            "ParamInfo": {
                "JobTitle": jobTitle,
                "UserType": userType,
                "LanguageType": 1,
                "WorkType": workType
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

function getParamAddVel() {
    var message = "";
    var payrolLow = $("#payrolLow").val();
    var payrolHigh = $("#payrolHigh").val();

    if (payrolLow.length == 0 && payrolHigh.length == 0) {

    }
    else if (payrolLow.length == 0) {
        param.query.ParamInfo.PayrolHigh = payrolHigh;
    }
    else if (payrolHigh.length == 0) {
        param.query.ParamInfo.PayrolLow = payrolLow;
    }
    else {
        if (isNaN(payrolLow)) {
            message = "最低薪资待遇格式不正确\n\r";
        }
        else if (isNaN(payrolHigh)) {
            message = "最高薪资待遇格式不正确\n\r";
        }
        else {
            if (Number(payrolHigh) < Number(payrolLow)) {
                message = "最高薪资待遇不能小于最低薪资待遇\n\r";
            }
            else {
                param.query.ParamInfo.PayrolLow = payrolLow;
                param.query.ParamInfo.PayrolHigh = payrolHigh;
            }
        }
    }
    if (message.length > 0) {
        art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
        return false;
    }
    else {
        return true;
    }
}


//-----------查看详情
function span_ModifyFun(obj) {
    var objSpan = $(obj);
    var param = getParamModify();
    param.Id = objSpan.attr("itemId");
    param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "/JobsWeb/GetObjectById_Web",
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
        title: "招聘职位：" + json.Model.JobTitle,
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

    $("#jobContent").html(json.Model.JobContent);
}

//修改参数
function getParamModify() {
    var param = {
        "Id": 0
    };
    return param;
}
//替换html标签，并截取长度
function delHtmlTag(str, length) {
    var con = str.replace(/<[^>]+>/g, "");//去掉所有的html标记
    if (con.length > 0) {
        if (con.length >= length) {
            con = con.substring(0, length);
        }
    }
    return con;
}


function openMeetionInfo(id, workType) {
    window.location.href = "/JobsWeb/JobWebDes?id=" + id + "&worktype=" + workType;
}

//加载
function Get_PageInfo(param) {
    var paramJson = JSON.stringify(param);
    $.ajax({
        type: "POST",
        async: false,
        url: "/JobsWeb/Get_PageInfo",
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

