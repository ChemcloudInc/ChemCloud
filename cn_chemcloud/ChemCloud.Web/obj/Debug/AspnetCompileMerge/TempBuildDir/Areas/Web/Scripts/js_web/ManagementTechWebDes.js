// JavaScript Document
$(function () {
    var param = getParamModify();
    var id = getUrlParam("id");
    if (id != null) {
        param.Id = id;
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
        url: "/TechInfo/GetObjectById_Web",
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
                $("#hTitle").html(json.Model.Title);
                $("#spanMeetingTime").html(json.Model.PublishTime);
                $("#spanMeetingPlace").html(json.Model.Author);
                $("#divMeetingContent").html(json.Model.TechContent);
                $("#platName").html(json.Model.techName);
                $("#platTel").html(json.Model.techTel);
                $("#platEmail").html(json.Model.techEmail);
                if (json.PreNextRow.Msg.IsSuccess) {

                    //上一条
                    if (json.PreNextRow.List[0].Msg.IsSuccess) {
                        $("#a_PreInfo").html(json.PreNextRow.List[0].Model.Title);
                        $("#a_PreInfo").attr("href", "/TechInfo/ManagementWebDes?id=" + json.PreNextRow.List[0].Model.Id);
                    }
                    else {
                        $("#a_PreInfo").html(json.PreNextRow.List[0].Msg.Message);
                    }

                    //下一条
                    if (json.PreNextRow.List[1].Msg.IsSuccess) {
                        $("#a_NextInfo").html(json.PreNextRow.List[1].Model.Title);
                        $("#a_NextInfo").attr("href", "/TechInfo/ManagementWebDes?id=" + json.PreNextRow.List[1].Model.Id);
                    }
                    else {
                        $("#a_NextInfo").html(json.PreNextRow.List[1].Msg.Message);
                    }
                }
                else {
                    $("#a_PreInfo").html(json.PreNextRow[0].Msg.Message);
                    $("#a_NextInfo").html(json.PreNextRow[0].Msg.Message);
                }

                if (json.AttachmentList.Msg.IsSuccess) {
                    var liList = "";
                    for (var i = 0; i < json.AttachmentList.List.length; i++) {
                        liList += "<a href='" + json.AttachmentList.List[i].AttachmentName + "'>" + json.AttachmentList.List[i].FileName + "</a>";
                    }
                    $("#attachmentList").html(liList);
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
    if (r != null) return unescape(r[2]); return null; //返回参数值
}


function openMeetionInfo(id) {
    window.location.href = "/TechInfo/ManagementWebDes?id=" + id;
}