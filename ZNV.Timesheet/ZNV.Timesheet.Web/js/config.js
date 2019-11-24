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
            url: "/Home/GetDepartmentList",
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
            url: "/Home/GetEmployeeList",
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
    },
    team: {
        placeholder: '输入科室Id或名称',
        //Does the user have to enter any data before sending the ajax request
        minimumInputLength: 0,
        allowClear: false,
        ajax: {
            //How long the user has to pause their typing before sending the next request
            delay: 150,
            //The url of the json service
            url: "/Home/GetTeamList",
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
                        return { id: obj.Id, text: obj.TeamName };
                    }),
                    pagination: {
                        more: (params.page * 100) <= data.Total
                    }
                };
            }
        }
    },
    project: {
        placeholder: '输入项目编号或名称',
        minimumInputLength: 0,
        allowClear: false,
        ajax: {
            delay: 150,
            url: 'Home/GetProjectList',
            dataType: 'json',
            async: true,
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
                        return { id: obj.Id, text: obj.ProjectName + "(" + obj.ProjectCode + ")" };
                    }),
                    pagination: {
                        more: (params.page * 100) <= data.Total
                    }
                };
            }
        }
    },
    allProject: {
        placeholder: '输入项目编号或名称',
        minimumInputLength: 0,
        allowClear: false,
        ajax: {
            delay: 150,
            url: 'Home/GetAllProjectList',
            dataType: 'json',
            async: true,
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
                        return { id: obj.Id, text: obj.ProjectName + "(" + obj.ProjectCode + ")" };
                    }),
                    pagination: {
                        more: (params.page * 100) <= data.Total
                    }
                };
            }
        }
    }
}