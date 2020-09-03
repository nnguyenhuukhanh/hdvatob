var SupplierControl = {
    init: function () {

        SupplierControl.registerEvent();

    },

    registerEvent: function () {
       
        $('.viewlogSupplier').on('click', function () {
            var code = $(this).data('code');
            var url = '/Supplier/ViewlogKhachhang';
            $.get(url, { code: code }, function (data) {
                $('#ModalViewLog').modal('show');
                $('.DetailLog').html(data);
            });
        });
    },

};
SupplierControl.init();