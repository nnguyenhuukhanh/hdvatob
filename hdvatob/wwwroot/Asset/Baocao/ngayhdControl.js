var ngayhdControl = {
    init: function () {

        ngayhdControl.registerEvent();

    },

    registerEvent: function () {
        $('.mytable').DataTable({
            'paging': true,
            'lengthChange': false,
            'searching': false,
            'ordering': true,
            'info': true,
            'autoWidth': false
        });
        $('#frmDoanhthungayhd').validate({
            rules: {
                tungay: {
                    required: true
                },
                denngay: {
                    required: true
                }
            },
            messages: {
                tungay: {
                    required: "Vui lòng nhập từ ngày"
                },
                denngay: {
                    required: "Vui lòng nhập đến ngày"
                },
            }

        });

    },

};
ngayhdControl.init();