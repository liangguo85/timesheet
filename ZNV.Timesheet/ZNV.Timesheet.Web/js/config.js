select2 = {
    department: {
        placeholder: '输入部门编码或名称',
        //Does the user have to enter any data before sending the ajax request
        minimumInputLength: 0,
        allowClear: true,
        ajax: {
            //How long the user has to pause their typing before sending the next request
            delay: 150,
            //The url of the json service
            url: "/Team/GetDepartmentList",
            dataType: 'json',
            async: true,
            //Our search term and what page we are on
            data: function (params) {
                return {
                    pageSize: 100,
                    pageNum: params.page || 1,
                    searchTerm: params.term,
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: $.map(data.Results, function (obj) {
                        return { id: obj.DeptCode1, text: obj.FullDeptName + "(" + obj.DeptCode1 + ")" };
                    }),
                    pagination: {
                        more: (params.page * 100) <= data.Total
                    }
                };
            }
        }
    },
    employee: {
        placeholder: '输入工号或名字',
        //Does the user have to enter any data before sending the ajax request
        minimumInputLength: 0,
        allowClear: true,
        ajax: {
            //How long the user has to pause their typing before sending the next request
            delay: 150,
            //The url of the json service
            url: "/Team/GetEmployeeList",
            dataType: 'json',
            async: true,
            //Our search term and what page we are on
            data: function (params) {
                return {
                    pageSize: 100,
                    pageNum: params.page || 1,
                    searchTerm: params.term,
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: $.map(data.Results, function (obj) {
                        return { id: obj.EmployeeCode, text: obj.EmployeeName + "(" + obj.EmployeeCode + ") - " + obj.DeptName };
                    }),
                    pagination: {
                        more: (params.page * 100) <= data.Total
                    }
                };
            }
        }
    }
}