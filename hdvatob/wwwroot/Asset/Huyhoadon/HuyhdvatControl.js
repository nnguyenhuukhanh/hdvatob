var HuyhdvatControl = {
    init: function () {

        HuyhdvatControl.registerEvent();

    },

    registerEvent: function () {


        $('.viewlogHuyHoadon').on('click', function () {
            var idhoadon = $(this).data('idhoadon');
            var url = '/Huyhoadon/Viewloghoadon';
            $.get(url, { idhoadon: idhoadon }, function (data) {
                $('#ModalViewLog').modal('show');
                $('.DetailLog').html(data);
            });
        });
    },

};
HuyhdvatControl.init();