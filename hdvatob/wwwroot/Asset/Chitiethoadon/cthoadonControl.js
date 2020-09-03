var cthoadonControl = {
    init: function () {
        cthoadonControl.registerEvent();

    },

    registerEvent: function () {
        

        //$("#tbCthd .delete").on("click", function () {
        //    var id = $(this).data('id');
        //    var nguoitao = $(this).data('nguoitaohd');
        //    var user = $(this).data('user');
        //    if (nguoitao != user) {
        //        alert('Bạn không có quyền xoá chi tiết hoá đơn này.')
        //    }
        //    else {
        //        if (confirm("Bạn muốn xoá chi tiết hoá đơn này?")) {
        //            //$.ajax({
        //            //    type: "POST",
        //            //    url: "/Hoadon/Delete",
        //            //    data: { idhoadon: id }
        //            //});
        //            $('#row_' + id).remove();
        //        }
        //    }
        //});
        $('.viewlog_cthd').on('click', function () {
            var id = $(this).data('id');
            var url = '/Chitiethoadon/ViewlogCthoadon';
            $.get(url, { id: id }, function (data) {
                $('#ModalViewLog').modal('show');
                $('.DetailLog').html(data);
            });
        });
    },

};
cthoadonControl.init();