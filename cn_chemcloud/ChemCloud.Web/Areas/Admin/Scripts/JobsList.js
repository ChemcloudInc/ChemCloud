$(document).ready(function () {
    getSelectOptionList(0);
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

/*------------------------------------------  new   -------------------------------------------*/

/*------------------------------------------  审核 Start  -------------------------------------------*/
function span_ModifyFun(obj) {
    var objSpan = $(obj);
    var param = getParamModify();
    param.Id = objSpan.attr("itemId");
    param2 = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "./GetObjectById_Admin",
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
    var editor = "";
    art.dialog({
        width: 700,
        height: 400,
        top: 30,
        id: 'modify_Dialog',
        title: "修改：" + json.Model.JobTitle,
        content: '<table id="table_AddJob">'
                + '<tr><td>审核通过：</td><td><input id="IsApprovalStatus" type="checkbox" checked="checked"/></td></tr>'
                + '<tr>'
                    + '<td>职位标题：</td>'
                    + '<td colspan="2">'
                        + '<input id="jobTitle" type="text" class="inputTitle" value="' + json.Model.JobTitle + '" />'
                        + '<select id="workType" class="search_60">'
                            //+ '<option value="0"> 全职 </option>'
                            //+ '<option value="1"> 兼职 </option>'
                            //+ '<option value="2"> 外包 </option>'
                        + '</select>'
                        + '<select id="languageType" class="search_80">'
                                //+ '<option value="1"> 中文 </option>'
                                //+ '<option value="2"> 英文 </option>'
                        + '</select>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>薪资待遇：</td>'
                    + '<td colspan="2">'
                           + '<input id="payrolLow" type="text" class="inputPayrol" placeholder="最低薪资" value="' + json.Model.PayrolLow + '"/> - <input id="payrolHigh" type="text"  value="' + json.Model.PayrolHigh + '" class="inputPayrol" placeholder="最高薪资"/>'
                           + '<select id="typeOfCurrency" class="search_60">'
                                //+ '<option value="0"> $ </option>'
                                //+ '<option value="1"> ￥ </option>'
                            + '</select>'
                        + '<select id="payrollType" class="search_80">'
                            //+ '<option value="2"> 月薪 </option>'
                            //+ '<option value="3"> 年薪 </option>'
                            //+ '<option value="1"> 时新 </option>'
                            //+ '<option value="0"> 面议 </option>'
                        + '</select>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>招聘时间：</td>'
                    + '<td colspan="2">'
                        + '<input id="startDate" type="text"  value="' + json.Model.StartDate + '" onclick="dateTime_YMD_CN(this)"  class="inputDateTime" placeholder="开始时间"/> - '
                        + '<input id="endDate" type="text"  value="' + json.Model.EndDate + '" onclick="dateTime_YMD_CN(this)"  class="inputDateTime" placeholder="截止时间"/>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>工作地点：</td>'
                    + '<td colspan="2">'
                        + '<input id="workPlace" type="text" class="inputTitle" value="' + json.Model.WorkPlace + '"/>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>招聘电话：</td>'
                    + '<td>'
                        + '<input id="companyTel" type="text" class="input_200" placeholder="如：025-12345678，15888888888" value="' + json.Model.CompanyTel + '"/>'
                    + '</td>'
                    + '<td>'
                        + '招聘邮箱：<input id="companyEmail" type="text" class="input_200" value="' + json.Model.CompanyEmail + '"/>'
                    + '</td>'
                + '</tr>'
                + '<tr>'
                    + '<td>招聘详情：</td>'
                    + '<td colspan="2">'
                        + '<textarea  id="jobContent" ></textarea>'
                    + '</td>'
                + '</tr>'
                + '</table>',
        lock: true,
        fixed: true,
        ok: function () {

            var param = getParamAdd();
            param.query.ParamInfo.JobContent = editor.getContent();
            param.query.ParamInfo.UserId = json.Model.UserId;
            param.query.ParamInfo.Id = json.Model.Id;
            var isTrue = getParamAddVel(param);
            if (isTrue) {
                param = JSON.stringify(param)
                modifyFun(param);
                UE.delEditor("jobContent");

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
            UE.delEditor("jobContent");
        }
    });
    getSelectOptionList(1);;
    $("#workType").val(json.Model.WorkType);//工作类型
    $("#payrollType").val(json.Model.PayrollType);//薪资类型
    $("#typeOfCurrency").val(json.Model.TypeOfCurrency);//货币类型
    $("#languageType").val(json.Model.LanguageType);//货币类型
    editor = UE.getEditor('jobContent', { initialFrameHeight: 300, initialFrameWidth: 600, initialContent: json.Model.JobContent });
}
function modifyFun(param) {
    $.ajax({
        type: "POST",
        url: "./ModifyJob",
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
                art.artDialog.errorTips(json.Message, "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("修改失败：系统错误！", "", "1.5");//短暂提示-错误
        }
    });
}
function getParamModify() {
    var param = {
        "Id": 0
    };
    return param;
}

/*------------------------------------------  审核 End  -------------------------------------------*/

/*------------------------------------------  删除 Start  -------------------------------------------*/
function span_DeleteFun(obj) {
    var param = getParamDel();
    var objSpan = $(obj);
    param.Id = objSpan.attr("itemid")
    param2 = JSON.stringify(param)

    $.ajax({
        type: "POST",
        url: "./DeleteById_Admin",
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
                art.artDialog.succeedTips("删除成功！", "", "1.5"); //短暂提示 - 成功
                get_List();
            }
            else {
                art.artDialog.errorTips(json.Message, "", "1.5");//短暂提示-错误
            }
        },
        error: function () {
            art.artDialog.errorTips("（删除）系统错误！", "", "1.5");//短暂提示-错误
        }
    });

}
function getParamDel() {
    var param = {
        "Id": 0
    };
    return param;
}
/*------------------------------------------  删除 End  -------------------------------------------*/

/*------------------------------------------  查询 Start  -------------------------------------------*/
//根据查询条件获取列表
function get_List() {
    param = getParamSelect();//重新确定查询条件
    var isTrue = getParamSelectVel();
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
        url: "./JobsList1",
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
                $("#span_allCount").html(json.PageInfo.Total);
                for (var i = 0; i < json.List.length; i++) {
                    if (i % 2 == 0) {
                        trList += "<tr class='tr_1'>"
                            //+ "<td>"
                            //+ "<input id='checkBoxItem' type='checkbox' /></td><td>"
                            //+ (i + 1) 
                            //+ "</td>"
                            + "<td>"
                            + json.List[i].JobTitle + "</td><td>"
                            + json.List[i].UserType + "</td><td>"
                            + json.List[i].UserId + "</td><td>"
                            + json.List[i].WorkType + "</td><td>"
                            + json.List[i].PayrollType + "</td><td>"
                            + json.List[i].LanguageType + "</td><td>"
                            + json.List[i].ApprovalStatus + "</td><td><span onclick='span_DeleteFun(this)' itemId='"
                            + json.List[i].Id + "'>删除</span> <span onclick='span_ModifyFun(this)' itemId='"
                            + json.List[i].Id + "'>审阅</span> <span  onclick='span_ModifyFun_Fast(this)' itemId='"
                            + json.List[i].Id + "'>快速审核</span></td></tr>";
                    }
                    else {
                        trList += "<tr class='tr_2'>"
                            //+ "<td>"
                            //+ "<input id='checkBoxItem' type='checkbox' /></td><td>"
                            //+ (i + 1) 
                            //+ "</td>"
                            + "<td>"
                        + json.List[i].JobTitle + "</td><td>"
                        + json.List[i].UserType + "</td><td>"
                        + json.List[i].UserId + "</td><td>"
                        + json.List[i].WorkType + "</td><td>"
                        + json.List[i].PayrollType + "</td><td>"
                        + json.List[i].LanguageType + "</td><td>"
                        + json.List[i].ApprovalStatus + "</td><td><span onclick='span_DeleteFun(this)' itemId='"
                        + json.List[i].Id + "'>删除</span> <span onclick='span_ModifyFun(this)' itemId='"
                        + json.List[i].Id + "'>审阅</span> <span onclick='span_ModifyFun_Fast(this)' itemId='"
                        + json.List[i].Id + "'>快速审核</span></td></tr>";
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
            art.artDialog.errorTips("查询失败：系统错误", "", "1.5");//短暂提示-错误
        }
    });
}
function Get_PageInfo(param) {
    var paramJson = JSON.stringify(param);
    $.ajax({
        type: "POST",
        async: false,
        url: "./Get_PageInfo_Admin",
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
function getParamSelect() {

    var jobTitle = $("#txtkeyword").val();
    var selectUserType = $("#selectUserType").val();
    var selectApprovalStatus = $("#selectApprovalStatus").val();
    var languageType = $("#selectLanguageType").val();

    var paramSelect = {
        "query": {
            "ParamInfo": {
                "JobTitle": jobTitle,
                "UserType": selectUserType,
                "LanguageType": languageType,
                "ApprovalStatus": selectApprovalStatus
            },
            "PageInfo": {
                "CurrentPage": 1,
                "Total": 0,
                "PageCount": 0,
                "PageSize": 20
            }
        }
    }
    return paramSelect;
}
function getParamSelectVel() {

    //var message = "";
    //var payrolLow = $("#payrolLow").val();
    //var payrolHigh = $("#payrolHigh").val();

    //if (payrolLow.length == 0 && payrolHigh.length == 0) {

    //}
    //else if (payrolLow.length == 0) {
    //    param.query.ParamInfo.PayrolHigh = payrolHigh;
    //}
    //else if (payrolHigh.length == 0) {
    //    param.query.ParamInfo.PayrolLow = payrolLow;
    //}
    //else {
    //    if (isNaN(payrolLow)) {
    //        message = "最低薪资待遇格式不正确<br/>";
    //    }
    //    else if (isNaN(payrolHigh)) {
    //        message = "最高薪资待遇格式不正确<br/>";
    //    }
    //    else {
    //        if (Number(payrolHigh) < Number(payrolLow)) {
    //            message = "最高薪资待遇不能小于最低薪资待遇<br/>";
    //        }
    //        else {
    //            param.query.ParamInfo.PayrolLow = payrolLow;
    //            param.query.ParamInfo.PayrolHigh = payrolHigh;
    //        }
    //    }
    //}
    //if (message.length > 0) {
    //    art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
    //    return false;
    //}
    //else {
    return true;
    //}
}
/*------------------------------------------  查询 End  -------------------------------------------*/

function getSelectOptionList(type) {
    $.ajax({
        type: "POST",
        url: "./GetSelectOptionList",
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
                var userType = "";
                var approvalStatus = "";
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

                    if (json.List[i].DictionaryTypeId == 15) {
                        userType += '<option value="' + json.List[i].DKey + '"> ' + json.List[i].DValue + ' </option>';
                    }

                    if (json.List[i].DictionaryTypeId == 14) {
                        approvalStatus += '<option value="' + json.List[i].DKey + '"> ' + json.List[i].DValue + ' </option>';
                    }
                    if (json.List[i].DictionaryTypeId == 10) {
                        languageType += '<option value="' + json.List[i].DValue + '"> ' + json.List[i].Remarks + ' </option>';
                    }
                }
                if (type == 1) {
                    $("#typeOfCurrency").html(typeOfCurrency);//货币类型
                    $("#workType").html(workType);//工作类型
                    $("#payrollType").html(payrollType);//薪资类型
                    $("#languageType").html(languageType);//语言类型

                }
                else {
                    userType = '<option value="0">所有</option>' + userType;
                    $("#selectUserType").html(userType);//商家类型
                    approvalStatus = approvalStatus + '<option value="0">所有</option>';
                    $("#selectApprovalStatus").html(approvalStatus);//审核状态
                    languageType = '<option value="0">所有</option>' + languageType;
                    $("#selectLanguageType").html(languageType);//货币类型
                }
            }
            else {
                art.artDialog.errorTips("select数据获取失败", "", "1");//短暂提示-错误
                return false;
            }
        },
        error: function () {
            art.artDialog.errorTips("查询失败：系统错误", "", "1.5");//短暂提示-错误
            return false;
        }
    });
}
function getParamAddVel(param) {

    var message = "";
    if (param.query.ParamInfo.JobTitle.length == 0) {
        message = "请输入职位标题<br/>";
    }
    else if (param.query.ParamInfo.PayrolLow.length == 0) {
        message = "请输入最低薪资<br/>";
    }
    else if (isNaN(param.query.ParamInfo.PayrolLow)) {
        message = "最低薪资待遇格式不正确<br/>";
    }
    else if (param.query.ParamInfo.PayrolHigh.length == 0) {
        message = "请输入最高薪资<br/>";
    }
    else if (isNaN(param.query.ParamInfo.PayrolHigh)) {
        message = "最高薪资待遇格式不正确<br/>";
    }
    else if (Number(param.query.ParamInfo.PayrolHigh) < Number(param.query.ParamInfo.PayrolLow)) {
        message = "最高薪资待遇格不能小于最低薪资待遇<br/>";
    }
    else if (param.query.ParamInfo.TypeOfCurrency.length == 0) {
        message = "请选择货币类型<br/>";
    }
    else if (param.query.ParamInfo.PayrollType.length == 0) {
        message = "请选择薪资类型<br/>";
    }
    else if (param.query.ParamInfo.WorkType.length == 0) {
        message = "请选择工作类型<br/>";
    }
    else if (param.query.ParamInfo.StartDate.length == 0) {
        message = "请选择开始时间<br/>";
    }
    else if (param.query.ParamInfo.EndDate.length == 0) {
        message = "请选择结束而时间<br/>";
    }
    else if (param.query.ParamInfo.WorkPlace.length == 0) {
        message = "请输入工作地点<br/>";
    }
    else if (param.query.ParamInfo.CompanyTel.length == 0) {
        message = "请输入招聘电话<br/>";
    }
    else if (!isTelephone(param.query.ParamInfo.CompanyTel) && !isMobile(param.query.ParamInfo.CompanyTel)) {
        message = "招聘电话格式不正确<br/>请输入座机号或手机号";
    }
    else if (param.query.ParamInfo.CompanyEmail.length == 0) {
        message = "请输入招聘邮箱<br/>";
    }
    else if (!isEmail(param.query.ParamInfo.CompanyEmail)) {
        message = "招聘邮箱格式不正确<br/>";
    }
    else if (param.query.ParamInfo.JobContent.length == 0) {
        message = "请输入招聘详情<br/>";
    }
    var time1 = new Date(param.query.ParamInfo.StartDate);
    var time2 = new Date(param.query.ParamInfo.EndDate);
    var now = new Date();

    if (time1 > time2 || time2 <= now) {
        message = "结束时间需晚于开始时间，切不能为当天<br/>";
    }
    if (message.length > 0) {
        art.artDialog.errorTips(message, "", "1.5");//短暂提示-错误
        return false;
    }
    else {
        return true;
    }
}

/*------------------------------------------  new  -------------------------------------------*/
//-----------快速审核
function span_ModifyFun_Fast(obj) {
    var objSpan = $(obj);
    var param = getParamModify();
    param.Id = objSpan.attr("itemId");
    param = JSON.stringify(param)
    $.ajax({
        type: "POST",
        url: "./ModifyJob_Fast",
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
                art.artDialog.succeedTips("审核成功！", "", "1");

                var param = getParamSelect();
                param.PageNo = 1;
                getJobsList(param);
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
function getParamAdd() {

    var jobTitle = $("#jobTitle").val();
    var payrolHigh = $("#payrolHigh").val();
    var payrolLow = $("#payrolLow").val();
    var typeOfCurrency = $("#typeOfCurrency").val();
    var payrollType = $("#payrollType").val();
    var workType = $("#workType").val();
    var workPlace = $("#workPlace").val();
    var companyTel = $("#companyTel").val();
    var companyEmail = $("#companyEmail").val();
    //var approvalStatus = $("#IsApprovalStatus").val();
    var approvalStatus = $("input[id='IsApprovalStatus']").is(':checked') ? 3 : 2;//审核状态（1：待审核；2：审核未通过；3：审核已通过）
    var languageType = $("#languageType").val();

    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();
    var nowDateTime = new Date().toLocaleString();
    var paramSelect = {
        "query": {
            "ParamInfo": {
                "JobTitle": jobTitle,
                "PayrolHigh": payrolHigh,
                "PayrolLow": payrolLow,
                "TypeOfCurrency": typeOfCurrency,
                "PayrollType": payrollType,
                "ApprovalStatus": approvalStatus,
                "WorkType": workType,
                "WorkPlace": workPlace,
                "CompanyTel": companyTel,
                "CompanyEmail": companyEmail,
                "StartDate": startDate,
                "LanguageType": languageType,
                "EndDate": endDate
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
