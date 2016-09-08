
$(document).ready(function () {
    Load_TransactionRecord();
});


//首页：Banner图片点击事件
function bannerClick(obj) {
    var liObj = $(obj);
    window.open(liObj.attr("targeturl"), "_blank");
}

//首页：实时交易加载事件
function Load_TransactionRecord() {
    var param = {}
    param = JSON.stringify(param)

    $.ajax({
        type: "POST",
        url: "/Home/Get_TransactionRecord_List",
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
            if (json.Msg.IsSuccess) {

                var arrEngNameList = new Array(json.List1.length);
                var arrValueList1 = new Array(json.List1.length);
                var arrValueList2 = new Array(json.List1.length);

                for (var i = 0; i < json.List1.length; i++) {
                    arrEngNameList[i] = json.List1[i].XName_CN;
                    arrValueList1[i] = json.List1[i].Y_CompleteAmount;
                }

                for (var i = 0; i < json.List2.length; i++) {
                    arrValueList2[i] = json.List2[i].Y_OrderAmount;
                }

                var lineChartData = {
                    labels: arrEngNameList,
                    datasets: [
                       {
                           label: "代理采购",
                           fillColor: "rgba(220,220,220,0.5)",
                           strokeColor: "rgba(220,220,220,1)",
                           pointColor: "rgba(220,220,220,1)",
                           pointStrokeColor: "#fff",
                           data: arrValueList1
                       },
                       {
                           label: "定制合成",

                           fillColor: "rgba(151,187,205,0.5)",
                           strokeColor: "rgba(151,187,205,1)",
                           pointColor: "rgba(151,187,205,1)",
                           pointStrokeColor: "#fff",
                           data: arrValueList2
                       }
                    ]
                };

                //var lineChartData = {
                //    labels: ["January", "February", "March", "April", "May", "June", "July"],
                //    datasets: [
                //        {
                //            fillColor: "rgba(220,220,220,0.5)",
                //            strokeColor: "rgba(220,220,220,1)",
                //            pointColor: "rgba(220,220,220,1)",
                //            pointStrokeColor: "#fff",
                //            data: [65, 59, 90, 81, 56, 55, 40]
                //        },
                //        {
                //            fillColor: "rgba(151,187,205,0.5)",
                //            strokeColor: "rgba(151,187,205,1)",
                //            pointColor: "rgba(151,187,205,1)",
                //            pointStrokeColor: "#fff",
                //            data: [28, 48, 40, 19, 96, 27, 100]
                //        }
                //    ]
                //}

                var myLine = new Chart(document.getElementById("canvas").getContext("2d")).Line(lineChartData);
            }
            else {
            }
        },
        error: function () {
        }
    });


   
}

