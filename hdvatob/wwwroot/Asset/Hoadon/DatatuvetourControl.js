var DatatuvetourControl = {
    init: function () {

        DatatuvetourControl.registerEvent();

    },

    registerEvent: function () {

       

        $('#frmDatatuvetour').validate({
            rules: {
                tungay: {
                    required: true
                },
                denngay: {
                    required: true
                },
                tour: {
                    required: true
                },
                tygia: {
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
                tour: {
                    required: "Vui lòng chọn tour"
                },
                tygia: {
                    required: "Nhập tỷ giá >0"
                }
            }

        });
        $('#btSavedata').on('click', function () {

            DatatuvetourControl.exportList();

        })     
        $('#tbDatavetour').on('click', 'tbody tr', function (event) {
            $(this).addClass('highlight').siblings().removeClass('highlight');
        })

        $('#tbDatavetour').on('click', 'tbody tr', function () {
           // $(this).addClass('highlight').siblings().removeClass('highlight');
            var serial = $(this).data('serial');
            var tour = $('#cboTour').val();
            var idhoadon = $("#txtIdhoadon").val();
            var ppv = $('#txtPpv').val();
            var tygia = $('#txtTygia').val();
            var tkno = $('#cboTkno').val();
            var tkco = $('#cboTkco').val();

            data = { Idhoadon: idhoadon,tour:tour,serial:serial,ppv:ppv,tygia:tygia,tkno:tkno,tkco:tkco};
            $.get('/Chitiethoadon/ListCtVetourBySerial', data, function (res) {
                if (res) {
                    $('.ctDatatuvetour').html(res);
                }
            })
        });
    },
    exportList: function () {

        var idList = [];
        $.each($('.ckId'), function (i, item) {
            if ($(this).prop('checked')) {
                idList.push({
                    serial: $(item).data('serial'),
                    tuyentq: $(item).data('tuyentq'),
                    sgtcode: $(item).data('sgtcode'),
                    tenkhach: $(item).data('tenkhach'),
                    dotuoi: $(item).data('dotuoi'),
                    sotiennt: $(item).data('sotiennt'),
                    doanhthunn: $(item).data('doanhthunn'),
                    loaitien: $(item).data('loaitien'),
                    tygia: $(item).data('tygia'),
                    diengiai: $(item).data('diengiai'),
                    ghichu: $(item).data('ghichu'),
                    xuatve: $(item).data('xuatve'),
                    vat:$(item).data('vat')
                });
            }
            
        });
       //console.log(idList);
       // debugger
        if (idList.length > 0) {
            $('#stringId').val(JSON.stringify(idList));
            $('#formDataVetour').submit();
        }
        else {
            alert('Bạn chưa chọn chi tiết nào');
        }
    }

};
DatatuvetourControl.init();