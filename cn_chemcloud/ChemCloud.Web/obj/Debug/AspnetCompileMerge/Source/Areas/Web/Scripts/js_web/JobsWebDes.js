// JavaScript Document
$(function () {

   
    var param = getParamModify();
    var id = getUrlParam("id");
    var workType = getUrlParam("worktype");
    var languageType = 1;

    if (id != null) {
        param.Id = id;
        param.WorkType = workType;
        param.LanguageType = languageType;
        get_DesInfo(param);
    }
    else {
        art.artDialog.errorTips("获取失败！", "", "1.5");//短暂提示-错误
    }
})


//获取指定记录及上下两条记录
function get_DesInfo(param) {

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
                $("#hTitle").html(json.Model.Model.JobTitle);
                $("#spanUserId").html(json.Model.Model.UserId);
                $("#spanEndDate").html(json.Model.Model.EndDate);
                $("#spanPayrolHigh_Low").html(json.Model.Model.Payrol_LowHigh);
                $("#spanCompanyTel").html(json.Model.Model.CompanyTel);
                $("#spanCompanyEmail").html(json.Model.Model.CompanyEmail);
                $("#spanWorkPlace").html(json.Model.Model.WorkPlace);
                $("#divJobContent").html(json.Model.Model.JobContent);

                if (json.List.Msg.IsSuccess) {
                    var workType = getUrlParam("worktype");
                    //上一条
                    if (json.List.List[0].Msg.IsSuccess) {
                        $("#a_PreInfo").html(json.List.List[0].Model.JobTitle);
                        $("#a_PreInfo").attr("href", "/JobsWeb/JobWebDes?id=" + json.List.List[0].Model.Id + "&worktype=" + workType);
                    }
                    else {
                        $("#a_PreInfo").css("display", "none");
                        $("#span_pre_no").css("display", "inline-block");
                        $("#span_pre_no").html(json.List.List[0].Msg.Message);
                    }

                    //下一条
                    if (json.List.List[1].Msg.IsSuccess) {
                        $("#a_NextInfo").html(json.List.List[1].Model.JobTitle);
                        $("#a_NextInfo").attr("href", "/JobsWeb/JobWebDes?id=" + json.List.List[1].Model.Id + "&worktype=" + workType);
                    }
                    else {
                        $("#a_NextInfo").css("display", "none");
                        $("#span_next_no").css("display", "inline-block");
                        $("#span_next_no").html(json.List.List[1].Msg.Message);
                    }
                }
                else {
                    $("#a_PreInfo").html(json.List[0].Msg.Message);
                    $("#a_NextInfo").html(json.List[0].Msg.Message);
                }

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

//修改参数
function getParamModify() {
    var param = {
        "Id": 0
    };
    return param;
}

function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return decodeURI(unescape(r[2])); return null; //返回参数值
}


function openMeetionInfo(id) {
    window.location.href = "/JobsWeb/JobWebDes?id=" + id;
}