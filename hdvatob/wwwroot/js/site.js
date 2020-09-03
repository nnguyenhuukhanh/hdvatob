$(function () {
    $('#AlertBox').removeClass('hide');
    $('#AlertBox').delay(5000).slideUp(500);
   
    $(".mytable").delegate("tr", "click", function () {
        $(this).addClass("hightlight").siblings().removeClass("hightlight");
    });
   
});


function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if ((charCode > 31 && charCode < 48) || charCode > 57) {
        return false;
    }
    return true;
}
$(document).ready(function () {

    $('.select2').select2();
    $(".datepicker").mask("99/99/9999");
    $(".time").mask("99:99");
    $('input.numbers').keyup(function (event) {
        var val = $(this).val();
        if (isNaN(val)) {
            val = val.replace(/[^0-9\.]/g, '');
            if (val.split('.').length > 2)
                val = val.replace(/\.+$/, "");
        }
        $(this).val(val);

        $(this).val(function (index, value) {
            return addCommas(value)
                ;
        });

    });
    $(function () {
        $('input[type="checkbox"].minimal').iCheck({
            checkboxClass: 'icheckbox_minimal-blue'
        });
    });

})
function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
$(document).ajaxStart(function () {
    Pace.restart()
});


$(document).ready(function () {
    
});