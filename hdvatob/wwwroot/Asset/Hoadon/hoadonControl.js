var hoadonControl = {
    init: function () {
     
        hoadonControl.registerEvent();

    },

    registerEvent: function () {
    
        $("#tbHoadon .delete").off('click').on("click", function () {
            var id = $(this).data('idhoadon');
            var nguoitao = $(this).data('nguoitaohd');
            var user = $(this).data('user');
            if (nguoitao != user) {
                alert('Bạn không có quyền xoá hoá đơn này.')
            }
            else {
                if (confirm("Lưu ý khi xoá sẽ xoá luôn các chi tiết của hoá đơn này?")) {
                    $.ajax({
                        type: "POST",
                        url: "/Hoadon/Delete",
                        data: { idhoadon: id }
                    });
                    $('#row_' + id).remove();
                }
            }
        });
        $('#tbHoadon').off('click').on('click', 'tbody tr', function () {
            $(this).addClass('highlight').siblings().removeClass('highlight');
            var id = $(this).data('idhoadon');
            data = { id: id };
            $.get('/Chitiethoadon/ListCthd_', data, function (res) {
                if (res) {
                    $('#chitiethoadon').html(res);
                }
            })
        });
        //$(function () {
        //    $('#tbHoadon').on('click', 'tbody tr', function (event) {
        //        debugger
        //        $(this).addClass('highlight').siblings().removeClass('highlight');
        //        var code = $(this).find("td").eq(0).html();
        //    })
        //});
        $('.viewlogHoadon').on('click', function () {
            var idhoadon = $(this).data('idhoadon');
            var url = '/Hoadon/Viewloghoadon';
            $.get(url, { idhoadon: idhoadon }, function (data) {
                $('#ModalViewLog').modal('show');
                $('.DetailLog').html(data);
            });
        });
    },
   
};
hoadonControl.init();