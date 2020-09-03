var CreateHoadonControl = {
    init: function () {

        CreateHoadonControl.registerEvent();

    },

    registerEvent: function () {
        $(document).ready(function () {
            //var id = $('#txtidhoadon').val();
            //EditHoadonControl.loadcthd(id);
            //var hide = $('#btEdit').val();
            //if (hide == 'hide') {
            //    $('#btEdit').val("Cập nhật");
            //    $('#btEdit').addClass('disabled');
            //}
        });
        $('#btSearchkh_i').on('click', function () {
            var search = $('#itxtmakh').val();
            var url = "/Supplier/listSearchkhach_i";
            $.get(url, { search: search }, function (data) {
                $("#iModalTimkh").modal('show')
                $('.ilisttimkh').html(data);
            });
        });
        //$('#itxtmakh').blur(function () {
        //    var code = $('#itxtmakh').val();
        //    var url = "/Supplier/getSupplierbyId";
        //    if (code.length ==5 ) {
        //        $.get(url, { code: code }, function (result) {
        //            var data = JSON.parse(result);
        //            if (data != null) {
                        
        //                $('#itxtmakh').val(data.code);
        //                $('#itxttencty').val(data.realname);
        //                $('#itxtdienthoai').val(data.telephone);
        //                $('#itxtdiachi').val(data.address);
        //                $('#itxtmst').val(data.taxcode);
        //            }
        //            else {
        //                $('#vt_makh').val("");
        //                $('#itxttencty').val('KHONG CO MA KHACH HANG');
        //                $('#itxtdienthoai').val("");
        //                $('#itxtdiachi').val("");
        //                $('#itxtmst').val("");
        //            }

        //        });
        //    } else {
        //        $('#itxttencty').val('KHONG CO MA KHACH HANG');
        //        $('#itxtdienthoai').val("");
        //        $('#itxtdiachi').val("");
        //        $('#itxtmst').val("");
        //    }
        //});



    },
    loadcthd: function (id) {
        $.ajax({
            url: '/Chitiethoadon/listCthd',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#chitiethoadon').html(data);
            }
        });
    }
};
CreateHoadonControl.init();