$(function () {
    $("#Email").blur(function () {
        var errorLabel = $("#regEmail_error");
        if ($.trim($('#Email').val()) == "") {
            errorLabel.css('display', 'block');
            //errorLabel.removeClass(".error-msg hide");
        } else {
            errorLabel.css('display', 'none');
            var reg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!reg.test($.trim($("#Email").val()))) {
                regEmailFormat_error.css("display", "block");
            } else {
                regEmailFormat_error.css("display", "none");
            }
        }
    });
});
function checkEmail() {
    var reg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!reg.test($.trim($("#Email").val()))) {
        regEmailFormat_error.css("display", "block");
        return false;
    }

}