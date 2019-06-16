$(function () {
    $('#example1').DataTable(
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
            'searching': false,
            "processing": true,
            "serverSide": true,
            "pageLength": 5,
            "ajax": {
                "url": "/ProjectManagement/GetAllProjects",
                "type": "POST",
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
                { "data": "IsApproval" },
                { "data": "ProjectCode" },
                { "data": "ProjectName" },
                { "data": "ProjectManagerID" },
                { "data": "ProductManagerID" },
                { "data": "ProjectType" },
                { "data": "ProjectLevel" },
                { "data": "ProjectKind" },
                { "data": "ProductionLineAttribute" },
                { "data": "ProjectStatus" },
                { "data": "IsEnabled" },
                { "data": "EffectiveDate" },
                { "data": "ExpirationDate" },
                {
                    "data": null, "render": function (data, type, row) {
                        return "<div class='btn - group - toggle'><button type='button' class='btn btn-info'>编辑</button> <button type='button' class='btn btn-danger'>删除</button></div>";
                    }
                }

            ]
        }
    )
    $('#example2').DataTable({
        'paging': true,
        'lengthChange': false,
        'searching': false,
        'ordering': true,
        'info': true,
        'autoWidth': false
    })

    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    })
    //Red color scheme for iCheck
    $('input[type="checkbox"].minimal-red, input[type="radio"].minimal-red').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    })
    //Flat red color scheme for iCheck
    $('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    })
})