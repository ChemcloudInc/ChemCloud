$(function () {

    $(".start_datetime").datetimepicker({
        language: 'zh-CN',
        format: 'yyyy-mm-dd',
        autoclose: true,
        weekStart: 1,
        minView: 2
    });
    $(".end_datetime").datetimepicker({
        language: 'zh-CN',
        format: 'yyyy-mm-dd',
        autoclose: true,
        weekStart: 1,
        minView: 2
    });
    $(".start_datetime").click(function () {
        $('.end_datetime').datetimepicker('show');
    });
    $(".end_datetime").click(function () {
        $('.start_datetime').datetimepicker('show');
    });

    $('.start_datetime').on('changeDate', function () {
        if ($(".end_datetime").val()) {
            if ($(".start_datetime").val() > $(".end_datetime").val()) {
                $('.end_datetime').val($(".start_datetime").val());
            }
        }

        $('.end_datetime').datetimepicker('setStartDate', $(".start_datetime").val());
    });
});


//  ============================ 获取 公共参数 ========结束


//  ============================ 时间相关操作 ======== 开始

//获取指定时间 【时、分、秒】
function getDatedate_HMS(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var seconds = date.getSeconds();
    var milliseconds = date.getMilliseconds();

    if (month < 10) {
        month = "0" + month.toString();
    }
    if (day < 10) {
        day = "0" + day.toString();
    }
    if (hour < 10) {
        hour = "0" + hour.toString();
    }
    if (minute < 10) {
        minute = "0" + minute.toString();
    }
    if (seconds < 10) {
        seconds = "0" + seconds.toString();
    }
    if (milliseconds < 10) {
        milliseconds = "0" + milliseconds.toString();
    }
    var datedate1 = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + seconds;
    return datedate1;
}
//获取指定时间 【年、月、日】
function getDatedate_YMD(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var seconds = date.getSeconds();
    var milliseconds = date.getMilliseconds();

    if (month < 10) {
        month = "0" + month.toString();
    }
    if (day < 10) {
        day = "0" + day.toString();
    }

    var datedate1 = year + "-" + month + "-" + day;
    return datedate1;
}
//获取指定时间 【年、月、日】
function getDatedate_YMD_1() {
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var seconds = date.getSeconds();
    var milliseconds = date.getMilliseconds();

    if (month < 10) {
        month = "0" + month.toString();
    }
    if (day < 10) {
        day = "0" + day.toString();
    }

    var datedate1 = year + "-" + month + "-" + day;
    return datedate1;
}

//获取指定时间 【年、月】
function getDatedate_YM(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var seconds = date.getSeconds();
    var milliseconds = date.getMilliseconds();

    if (month < 10) {
        month = "0" + month.toString();
    }

    var datedate1 = year + "-" + month;
    return datedate1;
}

//增加指定天数
function addDate(date, days) {
    var d = new Date(date);
    d.setDate(d.getDate() + days);
    var month = d.getMonth() + 1;
    var day = d.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    if (day < 10) {
        day = "0" + day;
    }
    var val = d.getFullYear() + "-" + month + "-" + day;
    return val;
}
//增加指定月数
function addMonth(date, months) {
    var d = new Date(date);
    d.setMonth(d.getMonth() + months);
    var month = d.getMonth() + 1;
    var day = d.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    if (day < 10) {
        day = "0" + day;
    }
    var val = d.getFullYear() + "-" + month + "-" + day;
    return val;
}



//  ============================ 时间相关操作 ======== 结束


//  ============================ datetimepicker 相关操作 ======== 开始
function dateTime_YMD_CN(obj) {
    var mydate = new Date();
    var thisObj = $(obj);
    thisObj.datetimepicker({
        language: "zh-CN",
        format: "yyyy-mm-dd",
        autoclose: true,
        minView: "month",
        viewSelect: "month"
    });
    thisObj.datetimepicker('show');
    thisObj.datetimepicker('setStartDate', mydate);
}

function dateTime_Click1(obj, format, language, startDT, endDT, initDate) {
    var mydate = new Date();
    var thisObj = $(obj);
    thisObj.datetimepicker({
        format: format,
        language: language,
        startDate: startDT,//最早的日期
        endDate: endDT,//最晚的日期
        initialDate: initDate,//初始日期
        startView: 3,
        autoclose: true,
        minView: "year",
        viewSelect: "month"
    });
    thisObj.datetimepicker('show');
    thisObj.datetimepicker('update', mydate);
}

/*
时间控件——添加初始加载时的value值
obj:时间空间input对象：如 "#startDate"
formatStr:时间格式：如 "yyyy-mm"
defaultTime:基准时间：如 new Date()
*/
//
function dateTime_SetDefaultsValue(obj, formatStr, defaultTime, language, startDT, endDT, initDate) {
    var thisObj = $(obj);
    thisObj.datetimepicker({
        format: formatStr,
        language: language,
        startDate: startDT,//最早的日期
        endDate: endDT,//最晚的日期
        initialDate: initDate,//初始日期
        startView: 3,
        autoclose: true,
        minView: "year",
        viewSelect: "month"
    });

    thisObj.datetimepicker('update', defaultTime);
    thisObj.on('changeDate', function (ev) {

    });
}

function dateTime_SetDefaultsValue_day(obj, formatStr, defaultTime, language, startDT, endDT, initDate) {
    var thisObj = $(obj);
    thisObj.datetimepicker({
        format: formatStr,
        language: language,
        startDate: startDT,//最早的日期
        //endDate: endDT,//最晚的日期
        initialDate: initDate,//初始日期
        startView: 3,
        autoclose: true,
        minView: "month",
        viewSelect: "month"
    });

    thisObj.datetimepicker('update', defaultTime);
    thisObj.on('changeDate', function (ev) {

    });
}
//  ============================ datetimepicker 相关操作 ======== 开始

//function dateTime_Click1(obj) {
//    alert(5);
//    var mydate = new Date();
//    var thisObj = $(obj);
//    thisObj.datetimepicker({
//        //startDate: "2015-01-01",//最早的日期
//        //endDate: "2015-01-01",//最晚的日期

//        //daysOfWeekDisabled: [0, 6],//禁用的星期，如禁用周末则[0,6]；都好隔开
//        //weekStart: 0,//0 (Sunday) to 6 (Saturday)

//        startView: 3,//时间空间被打开时显示的界面： 0：'hour'；1：'day'；2：'month'；3：'year'；4：'decade'；
//        minView: "month", //插件可以精确到最小那个时间，比如1的话就只能选择到天，不能选择小时了
//        maxView: "year", //显示的最大界面

//        todayBtn: true, //今日按钮
//        todayHighlight: true,//凸显当天日期
//        //keyboardNavigation: true,//是否允许键盘选择时间

//        forceParse:true,///当选择器关闭的时候，是否强制解析输入框中的值。也就是说，当用户在输入框中输入了不正确的日期，选择器将会尽量解析输入的值，并将解析后的正确值按照给定的格式format设置到输入框中
//        //minuteStep:5,//构建小时视图的最小增量，
//        //pickerPosition:'bottom-right',//默认值：“bottom-right”（其他值）：“bottom-left”），此选项仅在组件实现中可用。有了它你可以将选择器的输入字段下才。
//        //viewSelect: hour,//数字或字符串。默认：同minview（支持的价值观是： 'decade', 'year', 'month', 'day', 'hour'）
//          viewSelect : 0,//默认和minView相同
//        initialDate: "2015-01-01",//初始日期
//        //showMeridian: false,//该选项将启用天和小时的意见经络意见。
//        format: "yyyy-mm-dd", //选择日期后，文本框显示的日期格式 yyyy-mm-dd hh:ii
//        language: 'zh-CN', //汉化 
//        autoclose: true， //选择日期后自动关闭 
//    });
//    thisObj.datetimepicker('show');
//    thisObj.datetimepicker('setStartDate', mydate);
//}


//方法
//.datetimepicker(options):初始化一个的DateTimePicker。
//remove:移除$('#datetimepicker').datetimepicker('remove');
//show:显示$('#datetimepicker').datetimepicker('show');
//hide:隐藏$('#datetimepicker').datetimepicker('hide');
//update:更新，以当前值更新datetimepicker值；$('#datetimepicker').datetimepicker('update');
//setStartDate:   startDate——设置一个新的下限时间
//setEndDate:   endDate ——设置一个新的上限时间
//setDaysOfWeekDisabled:  daysOfWeekDisabled ——设置禁用的星期

//事件 Events
//show
//hide
//changeDate：日期更改时触发 
/*
$('#date-end')
.datetimepicker()
.on('changeDate', function(ev){
    if (ev.date.valueOf() < date-start-display.valueOf()){
        ....
    }
});
*/
//changeYear:年修改是触发
//changeMonth:月修改是触发
//outOfRange：超出范围——当选择的日期在startDate之前或endDate之后，或者
//Events
//Events




