var TachvetourControl = {
    init: function () {

        TachvetourControl.registerEvent();

    },

    registerEvent: function () {
        
        $('#frmTachvetour').validate({
            rules: {
                serial: {
                    required: true
                },
                makh: {
                    required: true
                }
            },
            messages: {
                serial: {
                    required: "Nhập số vé tour"
                },
                makh: {
                    required: "Nhập mã kh"
                }
            }

        });

        $('#btSearchkh_t').on('click', function () {
            var search = $('#ttxtmakh').val();
            var url = "/Supplier/listSearchkhach_t";
            $.get(url, { search: search }, function (data) {
                $("#tModalTimkh").modal('show')
                $('.listtimkh_t').html(data);
            });
        });
        $('#ttxtmakh').blur(function () {
            var code = $('#ttxtmakh').val();
            var url = "/Supplier/getSupplierbyId";
            if (code.length == 5) {
                $.get(url, { code: code }, function (data) {
                    if (data != null) {
                        $('#ttxtmakh').val(data.code);
                        $('#ttxttenkhach').val(data.name);
                    }
                });
            }
            // else {
            //    $('#ttxtmakh').val("");
            //    $('#ttxttenkhach').val('KHONG CO MA KHACH HANG');             
            //}
        });
        $('#btTachhd').on('click', function () {           
            TachvetourControl.exportList();
        });
        $('.sotiennt').keyup(function () {
            // initialize the sum (total price) to zero
            var sum = 0;
            // we use jQuery each() to loop through all the textbox with 'price' class
            // and compute the sum for each loop
            $('.sotiennt').each(function () {
                sum += Number($(this).val().replace(/,/g, ''));
            });
            // set the computed value to 'totalPrice' textbox
            $('#totalPrice').html(sum.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
            var tonggiatri = $('#tonggiatri').html();
            var total = $('#totalPrice').html();
            if (tonggiatri !== total) {
                $('#btTachhd').prop('disabled', true);
            } else {
                $('#btTachhd').prop('disabled', false); 
            }

        });
    },
    exportList: function () {
        var idList = [];
        
        $.each($('.chon_'), function (i, item) {
            //if ($(this).prop('checked')) {
                idList.push({
                    ngayct: $(item).find('.ngayct').find("span").html(),
                    makh: $(item).find('.makh').find('span').html(),
                    tenkh: $(item).find('.tenkh').find('span').html(),
                    tenkhach: $(item).find('.tenkhach').find('span').html(),
                    sgtcode: $(item).find('.sgtcode').find('span').html(),
                    diachi: $(item).find('.diachi').find('span').html(),
                    dienthoai: $(item).find('.dienthoai').find('span').html(),
                    msthue: $(item).find('.msthue').find('span').html(), 
                    serial: $(item).find('.serial').find('span').html(),
                    ghichu:$(item).find('.ghichu').find('span').html()
                });
            //}

        });

        var idListcthd = [];
        $.each($('.choncthd'), function (i, item) {
            //if ($(this).prop('checked')) {
            idListcthd.push({
                diengiai: $(item).find('.diengiai').find("span").html(),
                serial: $(item).find('.serial').find('span').html(),
                xuatve: $(item).find('.xuatve').find('span').html(),
                tenkhach: $(item).find('.tenkhach').find('span').html(),
                sgtcode: $(item).find('.sgtcode').find('span').html(),
                sotiennt: $(item).find('.sotiennt').find('input').val()  ,
                vat: $(item).find('.vat').find('span').html(),
                loaitien: $(item).find('.loaitien').find('span').html(),
                ghichu: $(item).find('.ghichu').find('span').html(),
                doanhthunn: $(item).find('.doanhthunn').find('span').html(),
                stt: $(item).find('.stt').find('span').html()
            });
            //}

        });
        //console.log(idList);
        //console.log(idListcthd);
        $('#stringHoadon').val(JSON.stringify(idList));
        $('#stringcthd').val(JSON.stringify(idListcthd));
        $('#frmTachvetour_').submit();
       
    }

};
TachvetourControl.init();