var CapnhathuycthdControl = {
    init: function () {

        CapnhathuycthdControl.registerEvent();

    },

    registerEvent: function () {

        $(document).ready(function () {
            debugger
            var hide = $('#btEdit').val();
            if (hide == 'hide') {
                $('#btEdit').val("Cập nhật");
                $('#btEdit').addClass('disabled');
            }
        });

        $('#frmCapnhathuycthd').validate({
            rules: {
                sotiennt: {
                    required: true
                },
                tygia: {
                    required: true
                },
                sotien: {
                    required: true
                }
            },
            messages: {
                sotiennt: {
                    required: "Nhập số tiền"
                },
                tygia: {
                    required: "Nhập tỷ giá"
                },
                sotien: {
                    required: "Nhập tiền VND"
                }
            }

        });
        $('#txtSotiennt').on('blur', function () {
            var sotiennt = Number($(this).val().replace(/,/g, ''));
            var tygia = Number($('#txtTygia').val().replace(/,/g, ''));
            var sotien = 0
            sotien = sotiennt * tygia;
            $('#txtSotien').val(addCommas(sotien));     
            

        });
        $('#txtTygia').on('blur', function () {
            var sotiennt = Number($('#txtSotiennt').val().replace(/,/g, ''));
            var tygia = Number($(this).val().replace(/,/g, ''));
            var sotien = 0
            sotien = sotiennt * tygia;
            $('#txtSotien').val(addCommas(sotien));

        });

    },


};
CapnhathuycthdControl.init();