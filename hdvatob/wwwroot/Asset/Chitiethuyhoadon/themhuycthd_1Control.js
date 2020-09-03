var themhuycthd_1Control = {
    init: function () {

        themhuycthd_1Control.registerEvent();

    },

    registerEvent: function () {
        $('#frmThemhuycthd_1').validate({
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
                    required: "(*)"
                },
                tygia: {
                    required: "(*)"
                },
                sotien: {
                    required: "(*)"
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
            debugger;
            var sotiennt = Number($('#txtSotiennt').val().replace(/,/g, ''));
            var tygia = Number($(this).val().replace(/,/g, ''));
            var sotien = 0
            sotien = sotiennt * tygia;
            $('#txtSotien').val(addCommas(sotien));
        });
       
    },
   

};
themhuycthd_1Control.init();