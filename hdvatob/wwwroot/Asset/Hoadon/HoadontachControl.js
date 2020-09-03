var HoadontachControl = {
    init: function () {

        HoadontachControl.registerEvent();

    },

    registerEvent: function () {
        $('#tbHoadontach').on('click', 'tbody tr', function (event) {
            $(this).addClass('highlight').siblings().removeClass('highlight');
        })

        $('#tbHoadontach').on('click', 'tr', function () {
            id = $(this).data('id');
            data = { Idhoadon: id };
            $.get('/Hoadon/listCttachve', data, function (res) {
                if (res) {
                    $('.chitiettachve').html(res);
                }
            })
        });

        $('#btHoadontach').on('click', function () {
            HoadontachControl.exportList();
        });
       
    },
    exportList: function () {
        var idList = [];
        $.each($('.ckId'), function (i, item) {
            if ($(this).prop('checked')) {
                idList.push({
                    Idhoadon: $(item).data('id')                  
                });
            }
        });

        console.log(idList);

        if (idList.length !== 0) {
            $('#stringHoadontach').val(JSON.stringify(idList));
            $('#frmHoadontach_').submit();
        }
        else {
            alert("Vui lòng chọn ít nhất 1 hoá đơn.")
        }



    }

};
HoadontachControl.init();