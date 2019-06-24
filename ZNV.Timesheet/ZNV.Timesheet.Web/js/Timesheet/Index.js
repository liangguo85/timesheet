var taskService = abp.services.app.task;
var tableObj = null;

(function ($) {
    $(function () {
        //开始日期和结束日期设置，默认为当前月的首末日期
        $('#startDate').val(getCurrentMonthFirst());
        $('#endDate').val(getCurrentMonthLast());
        $('#btCreate').click(createData);
        $('#btBulkDelete').click(bulkDeleteData);
        $('.DTED_Lightbox_Close').click(function () {
            $(".DTED_Lightbox_Background").hide();
            $(".DTED_Lightbox_Wrapper").hide();
        });

        //首次打开页面不加载，点击查询按钮再加载
        $('#search-btn').click(function () {
            searchData();
        });
    });
})(jQuery);

/** 根据条件把数据查询出来 */
function searchData() {
    abp.ui.block();
    if (tableObj) {
        tableObj.destroy();
    }
    tableObj = $('#dtTimesheet').on('xhr.dt', function (e, settings, json, xhr) {
        //这个是ajax加载完毕后触发的事件
        abp.ui.unblock();
    }).DataTable(
        {
            "language":
            {
                "decimal": "",
                "emptyTable": "没有数据",
                "info": "共 _TOTAL_ 条记录",
                "infoEmpty": "共 0 条记录",
                "infoFiltered": "(filtered from _MAX_ total entries)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "每页 _MENU_ 条",
                "loadingRecords": "Loading...",
                "processing": "Processing...",
                "search": "查找:",
                "zeroRecords": "没有找到匹配的记录",
                "paginate": {
                    "first": "首页",
                    "last": "尾页",
                    "next": "下一页",
                    "previous": "上一页"
                },
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                }
            },
            "autoWidth": true,
            'searching': false,
            "processing": true,
            "serverSide": true,
            "pageLength": 10,
            "paging": true,
            "pagingType": "simple",
            "ajax": {
                "url": "/Timesheet/GetAllTimesheets",
                "type": "POST",
                "data": {
                    "user": "kojar.liu",
                    "startDate": $('#startDate').val(),
                    "endDate": $('#endDate').val()
                },
                "datatype": "json"
            },
            "columnDefs": [
                {
                    "targets": [0, -1],
                    "searchable": false,
                    "orderable": false
                }
            ],
            "columns": [
                {
                    "data": null, "render": function (data, type, row) {
                        return "<input type='checkbox' class='flat-red' checked>";
                    }
                },
                { "data": "Id" },
                {
                    data: null,
                    render: function (data, type, row) {
                        return new Date(data.TimesheetDate).toDateString();
                    },
                    editField: ['TimesheetDate']
                },
                { "data": "ProjectID" },
                { "data": "ProjectGroup" },
                { "data": "Workload" },
                { "data": "WorkContent" },
                { "data": "Remarks" },
                { "data": "Status" },
                { "data": "Approver" },
                {
                    data: null,
                    render: function (data, type, row) {
                        return new Date(data.ApprovedTime).toDateString();
                    },
                    editField: ['ApprovedTime']
                },
                {
                    "data": null, "render": function (data, type, row) {
                        return "<div class='btn - group - toggle'><button type='button' class='btn btn-info'>编辑</button> <button type='button' class='btn btn-danger'>删除</button></div>";
                    }
                }

        ],
            buttons: [
                { extend: "create", editor: editor },
                { extend: "edit", editor: editor },
                { extend: "remove", editor: editor }
            ]
        }
    )
}

function createData() {
    var b = $(".DTED_Lightbox_Background");
    var c = $('.DTED_Lightbox_Wrapper');
    $(top.document.body).prepend(b);
    $(top.document.body).prepend(c);
    $(b).show();
    $(c).show();
}

function bulkDeleteData() {
    abp.message.confirm(
        '你选择的项讲会被删除', //确认提示
        '确定要这么做吗？',//确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.notify.info('调用批流删除接口', '温馨提示');
            }
            else {
                abp.notify.info('放弃批量删除', '温馨提示');
            }
        }
    );
}

/**获取当前月的第一天*/
function getCurrentMonthFirst() {
    var date = new Date();
    date.setDate(1);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    if (month < 10) month = "0" + month;
    var day = date.getDate();
    if (day < 10) day = "0" + day;
    return year + '-' + month + '-' + day;
}

/**获取当前月的最后一天*/
function getCurrentMonthLast() {
    var date = new Date();
    var currentMonth = date.getMonth();
    var nextMonth = ++currentMonth;
    var nextMonthFirstDay = new Date(date.getFullYear(), nextMonth, 1);
    var oneDay = 1000 * 60 * 60 * 24;
    var lastDate = new Date(nextMonthFirstDay - oneDay);
    var year = lastDate.getFullYear();
    var month = lastDate.getMonth() + 1;
    if (month < 10) month = "0" + month;
    var day = lastDate.getDate();
    if (day < 10) day = "0" + day;
    return year + '-' + month + '-' + day;
}
