var ThemctTuhuyhdControl = {
    init: function () {

        ThemctTuhuyhdControl.registerEvent();

    },

    registerEvent: function () {

        $('#frmThemctTuhuyhd_').validate({
            rules: {
                stt: {
                    required: true
                }
            },
            messages: {
                stt: {
                    required: "Nhập số thứ tự của hoá đơn."
                }
            }

        });
        $('#btThemctTuhuyhd').on('click', function () {

            ThemctTuhuyhdControl.exportList();
        });
    },
    exportList: function () {

        var idList = [];
        $.each($('.ckId'), function (i, item) {
            if ($(this).prop('checked')) {
                idList.push({
                    Id: $(item).data('id'),
                    diengiai: $(item).data('diengiai'),
                    sotien: $(item).data('sotien')
                });
            }
        });
        //$.each($('.choncthd'), function (i, item) {
        //    debugger
        //    var chon= $(item).find('.ckId').find('checkbox').val();
        //    console.log(chon);
        //    if ($(chon).prop('checked')) {
        //        idList.push({
        //            diengiai: $(item).find('.diengiai').find("span").html(),
        //            tkno: $(item).find('.tkno').find('span').html(),
        //            tkco: $(item).find('.tkco').find("span").html(),
        //            httc: $(item).find('.httc').find('span').html(),
        //            sotien: $(item).find('.sotien').find("span").html(),
        //            serial: $(item).find('.serial').find('span').html(),
        //            tenkhach: $(item).find('.tenkhach').find("span").html(),
        //            sgtcode: $(item).find('.sgtcode').find('span').html(),
        //            ppv: $(item).find('.ppv').find("span").html(),
        //            vat: $(item).find('.vat').find('span').html(),
        //            ghichu: $(item).find('.ghichu').find("span").html(),
        //        });
        //    }

        //});
        //console.log(idList);
        // debugger
        if (idList.length > 0) {
            $('#stringId').val(JSON.stringify(idList));
            $('#frmThemctTuhuyhd_').submit();
        }
        else {
            alert('Bạn chưa chọn chi tiết nào');
        }
    }

};
ThemctTuhuyhdControl.init();