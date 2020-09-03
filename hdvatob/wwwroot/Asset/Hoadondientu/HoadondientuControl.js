var HoadondientuControl = {
    init: function () {

        HoadondientuControl.registerEvent();

    },

    registerEvent: function () {

        $(function () {
            $('.mytable').DataTable({
                'paging': true,
                'lengthChange': true,
                'searching': true,
                'ordering': true,
                'info': true,
                'autoWidth': false
            })
        })
    },

};
HoadondientuControl.init();