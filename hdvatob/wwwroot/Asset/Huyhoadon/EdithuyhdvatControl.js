var EdithuyhdvatControl = {
    init: function () {

        EdithuyhdvatControl.registerEvent();

    },

    registerEvent: function () {
        $(document).ready(function () {
            var id = $('#txtidhoadon').val();
            EdithuyhdvatControl.loadhuycthd(id);
            var hide = $('#btEdit').val();
            if (hide == 'hide') {
                $('#btEdit').val("Cập nhật");
                $('#btEdit').addClass('disabled');
            }
        });
        $('#btSearchkh_u').on('click', function () {
            var search = $('#utxtmakh').val();
            var url = "/Supplier/listSearchkhach";
            $.get(url, { search: search }, function (data) {
                $("#uModalTimkh").modal('show')
                $('.listtimkh').html(data);
            });
        });

        $('#utxtmakh').blur(function () {
            var code = $('#utxtmakh').val();
            var url = "/Supplier/getSupplierbyId";
            if (code.length == 5) {
                $.get(url, { code: code }, function (data) {
                    if (data != null) {
                        $('#utxtmakh').val(data.code);
                        $('#utxttencty').val(data.name);
                        $('#utxtdienthoai').val(data.telephone);
                        $('#utxtdiachi').val(data.address);
                        $('#utxtmst').val(data.taxcode);
                    }
                    //else {
                    //    $('#utxtmakh').val("");
                    //    $('#utxttencty').val('KHONG CO MA KHACH HANG');
                    //    $('#utxtdienthoai').val("");
                    //    $('#utxtdiachi').val("");
                    //    $('#utxtmst').val("");
                    //}

                });
            } else {
                $('#utxttencty').val('KHONG CO MA KHACH HANG');
                $('#utxtdienthoai').val("");
                $('#utxtdiachi').val("");
                $('#utxtmst').val("");
            }
        });
        $('.previewhoadonhuy').on('click', function () {
            var idhoadon = $(this).data('id');
            var chinhanh = $(this).data('chinhanh');
            var url = '/Invoice/PreviewHoadonHuy';
            $.get(url, { idhoadon: idhoadon ,chinhanh:chinhanh}, function (data) {
                $('#ModalViewLog').modal('show');
                $('.DetailLog').html(data);
            });
        });


    },
    loadhuycthd: function (id) {
        $.ajax({
            url: '/Huycthdvat/ListHuyCthdvat',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#chitiethoadon').html(data);
            }
        });
    }
};
EdithuyhdvatControl.init();