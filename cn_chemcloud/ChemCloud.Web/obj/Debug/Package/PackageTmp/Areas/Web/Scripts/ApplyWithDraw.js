$(function () {
    checkScanState();
    $('#submitApply').click(function () {
        var reg = /^[0-9]+([.]{1}[0-9]{1,2})?$/;
        if (!reg.test($('#inputMoney').val())) {
            $.dialog.alert("提现金额不能为非数字字符");
            return;
        }
        if (parseFloat(balance) < parseFloat($('#inputMoney').val())) {
            $.dialog.alert("提现金额不能超出可用金额");
            return;
        }
        if (parseFloat($('#inputMoney').val()) < 1) {
            $.dialog.alert("提现金额不能小于1");
            return;
        }
        var loading = showLoading();
        $.post('ApplyWithDrawSubmit', { openid: opid, nickname: $('#nikename').text(), amount: parseFloat($('#inputMoney').val()), pwd: $('#payPwd').val() },
            function (result) {
                loading.close();
                if (result.success) {
                    $.dialog.succeedTips('提交成功!', function () {
                        JumpStep(4);
                    });
                }
                else {
                    $.dialog.errorTips(result.msg);
                }
            }
        );
    });
    //提现帐户设置 
    //$("#submitPayPal").on("click",(function () {
    //    //alert($("#sceneQR").val());
    //    //checkScanState();
    //    alert("11111111");
    //});
    $("#submitPayPal").on("click", function () {
        alert("aaaaaaaaaa");
    });
});
var opid = '';
function checkScanState() {
    $("#nikename").text($("#sceneQR").val());
    
    //$.post('GetPayPalId', { "pp": $("#sceneQR").val() }, function (result) {
    //    alert(result);
    //    if (result.result != "") {
    //        $("#nikename").text($("#sceneQR").val());
    //        alert(result.result);
    //        //JumpStep(3);
    //    } else {
    //        //setTimeout(checkScanState,0);
    //    }
    //});
};
function JumpStep(step) {
    $('div[name="stepname"]').hide();
    switch (step) {
        case 1:
            $('#stepnav').hide();
            $('#step' + step).show();
            break;
        case 2:
            $('#stepnav').show();
            $('#step' + step).show();
            break;
        case 3:
            $('#stepnav').show();
            $('div[name="step3"]').removeClass('todo').addClass('active');
            $('div[name="step3"]').prev().removeClass('active').addClass('done');
            $('#step' + step).show();
            break;
        case 4:
            $('#stepnav').show();
            $('div[name="step4"]').removeClass('todo').addClass('active');
            $('div[name="step4"]').prev().removeClass('active').addClass('done');
            $('#step' + step).show();
            break;
    }
};