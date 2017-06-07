$.ajaxSetup({ cache: false });

var DataTableHandler = {
    currentTable: {
        table: null,
        setTable: function (val) {
            this.table = val;
        }
    },
    dataTableDictionary: {
    },
    selectedRowDictionary: {
    },
    handleDataTableDefaults: function () {
        $.extend($.fn.dataTable.defaults, {

            "initComplete": function (oSettings, json) {
                $('.dataTables_filter').each(function (index) { $(this).appendTo($(this).closest('.panel').find('.table-search')); });
            },

            "drawCallback": function (settings, json) { // initialize any tooltips that are needed on the table
                $('tbody span.glyphicon-pencil').hover(function () {
                    $(this).tooltip({
                        title: 'Edit',
                        placement: 'bottom',
                        trigger: 'hover',
                        container: 'body'
                    });
                    $(this).tooltip('show');
                });                
                $('tbody span.glyphicon-remove').hover(function () {
                    $(this).tooltip({
                        title: 'Delete',
                        placement: 'bottom',
                        trigger: 'hover',
                        container: 'body'
                    });
                    $(this).tooltip('show');
                });
                $('tbody span.glyphicon-new-window').hover(function () {
                    $(this).tooltip({
                        title: 'Details',
                        placement: 'bottom',
                        trigger: 'hover',
                        container: 'body'
                    });
                    $(this).tooltip('show');
                });
            },
            "serverSide": true,
            "ordering": true,
            "searching": true,
            "dom": "rftS",
            "scroller": {
                "loadingIndicator": true
            }
        });
    }
};