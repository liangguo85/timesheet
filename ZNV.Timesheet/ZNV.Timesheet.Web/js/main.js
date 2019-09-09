(
    function () {
        abp.event.on('abp.notifications.received', function (userNotification) {
            abp.notifications.showUiNotifyForUserNotification(userNotification);
        });
    }

)();

var ApproveStatus = {
    Draft: 0,
    Approving: 1,
    Approved: 2
};

/**
 * 获取截断之后的文本
 * @param {any} longString，截断之前的长文本
 */
function GetShortStringForCell(longString) {
    if (longString) {
        var dataTablCellShowLength = 10;
        if (longString.length > dataTablCellShowLength) {
            return longString.substr(0, dataTablCellShowLength) + "…";
        }
        else {
            return longString;
        }
    }
    else {
        return "";
    }
}

/** 日期格式化，如果未指定fmt，则默认为 yyyy-MM-dd */
Date.prototype.Format = function (fmt) {
    if (!fmt)
        fmt = "yyyy-MM-dd";
    var o = {
        "M+": this.getMonth() + 1,                 //月份
        "d+": this.getDate(),                    //日
        "h+": this.getHours(),                   //小时
        "m+": this.getMinutes(),                 //分
        "s+": this.getSeconds(),                 //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds()             //毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

//把字符串转换成日期格式，原格式是2016-11-16T08:44:37Z这样的
function ConvertStringToDatetime(dateString) {
    var reggie = /(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})Z/;
    var dateArray = reggie.exec(dateString);
    var dateObject = new Date(
        (+dateArray[1]),
        (+dateArray[2]) - 1,
        (+dateArray[3]),
        (+dateArray[4]),
        (+dateArray[5]),
        (+dateArray[6])
    );
    return dateObject;
}

//把字符串转换成日期格式，原格式是/Date(1479285877000)/这样的，返回字符串
function ConvertStringToDatetimeEx(str, contansTime) {
    if (str && /^\/Date(.*)\/$/.test(str)) {
        var d = eval('new ' + str.substr(1, str.length - 2));
        var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate(), d.getHours(), d.getMinutes(), d.getSeconds()];
        for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
        if (contansTime) {
            return ar_date.slice(0, 3).join('-') + ' ' + ar_date.slice(3).join(':');
        }
        else {
            return ar_date.slice(0, 3).join('-');
        }
    }
    else {
        return "";
    }
    function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
}

//把字符串转换成日期格式，原格式是/Date(1479285877000)/这样的，返回日期类型
function ConvertStringToDatetimeObject(str) {
    if (str && /^\/Date(.*)\/$/.test(str)) {
        var d = eval('new ' + str.substr(1, str.length - 2));
        return d;
    }
    return null;
}

var CommentPopup;
/**审批操作牵涉到的comment输入 */
function ShowApproveComment(actionName, tsIdList) {
    var formDiv = $('<div class="form-row"/>');
    var divComment = $('<div style="text-align: center;"/>');
    var textArea = $('<textarea id="txtareaComment" style="width:100%" rows="3"></textarea>');
    var btComment = $('<input id="submitcomment" disabled="disabled" type="button" class="btn btn-primary btn-lg" />');
    textArea.change(function () {
        var comment = $(this).val();
        if (comment && comment.trim() != '') {
            btComment.removeAttr('disabled');
        }
        else {
            btComment.attr('disabled', 'disabled');
        }
    });
    divComment.append(textArea);
    formDiv.append(divComment);
    var divButton = $('<div style="text-align: center;"/>');
    divButton.append(btComment);
    btComment.val(actionName);
    btComment.click(function () {
        var comment = $(textArea).val();
        if (comment && comment.trim() != '') {
            if (actionName == "审批通过") {
                commApprove(tsIdList, comment);
            }
            else if (actionName == "驳回") {
                commReject(tsIdList, comment);
            }
            else if (actionName == "提交") {
                commSubmit(tsIdList, comment);
            }
            else if (actionName == "撤回") {
                commRollBack(tsIdList, comment);
            }
        }
        else {
            alert('comment为空');
        }
    });
    formDiv.append(divButton);

    CommentPopup = formDiv.dialog({
        autoOpen: true,
        resizable: false,
        modal: true,
        title: actionName,
        width: 300,
        close: function () {
            CommentPopup.dialog('destroy').remove();
        }
    });

    //公用的驳回方法，参数是对应的工时id列表，多个则用英文逗号隔开
    function commReject(tsIdList, comment) {
        $.ajax({
            type: "POST",
            url: 'TimesheetPending/CommReject',
            data: {
                tsIdList: tsIdList,
                comment: comment
            },
            success: function (data) {
                if (data.success) {
                    dataTable.ajax.reload();
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                    CommentPopup.dialog('destroy').remove();
                }
                else {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error"
                    })
                }
            }
        })
    }

    //公用的Approve方法，参数是对应的工时id列表，多个则用英文逗号隔开
    function commApprove(tsIdList, comment) {
        $.ajax({
            type: "POST",
            url: 'TimesheetPending/CommApprove',
            data: {
                tsIdList: tsIdList,
                comment: comment
            },
            success: function (data) {
                if (data.success) {
                    dataTable.ajax.reload();
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                    CommentPopup.dialog('destroy').remove();
                }
                else {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error"
                    })
                }
            }
        })
    }

    //公用的Submit方法，参数是对应的工时id列表，多个则用英文逗号隔开
    function commSubmit(tsIdList, comment) {
        $.ajax({
            type: "POST",
            url: 'Timesheet/SubmitFormForWeek',
            data: {
                tsfw: tsIdList,
                comment: comment
            },
            success: function (data) {
                if (data.success) {
                    dataTable.ajax.reload();
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                    CommentPopup.dialog('destroy').remove();
                    if (Popup) {
                        Popup.dialog('destroy').remove();
                    }
                }
                else {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error"
                    })
                }
            }
        })
    }

    //公用的RollBack方法，参数是对应的工时id列表，多个则用英文逗号隔开
    function commRollBack(tsIdList, comment) {
        $.ajax({
            type: "POST",
            url: 'Timesheet/CommRollBack',
            data: {
                tsfw: tsIdList,
                comment: comment
            },
            success: function (data) {
                if (data.success) {
                    dataTable.ajax.reload();
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                    CommentPopup.dialog('destroy').remove();
                    if (Popup) {
                        Popup.dialog('destroy').remove();
                    }
                }
                else {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error"
                    })
                }
            }
        })
    }
}

var SelectUserPopup;
/**转办弹出的选择转办人 */
function ShowSelectUser(tsIdList) {
    var formDiv = $('<div class="form-row"/>');
    var divComment = $('<div style="text-align: center;"/>');
    var selectUser = $('<select id="selectUser" class="form-control select2"></select>');

    var btOk = $('<input id="submitcomment" type="button" class="btn btn-primary btn-lg" value="确定" />');

    divComment.append(selectUser);
    formDiv.append(divComment);
    var divButton = $('<div style="text-align: center;"/>');
    divButton.append(btOk);
    btOk.click(function () {
        var user = $(selectUser).val();
        if (user && user.trim() != '') {
            $.ajax({
                type: "POST",
                url: 'TimesheetPending/CommTransfer',
                data: {
                    tsIdList: tsIdList,
                    transferUser: user
                },
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        $.notify(data.message, {
                            globalPosition: "top center",
                            className: "success"
                        })
                        CommentPopup.dialog('destroy').remove();
                    }
                    else {
                        $.notify(data.message, {
                            globalPosition: "top center",
                            className: "error"
                        })
                    }
                }
            })
        }
        else {
            alert('请选择需要转办的人员');
        }
    });
    formDiv.append(divButton);

    SelectUserPopup = formDiv.dialog({
        autoOpen: true,
        resizable: false,
        modal: true,
        title: "选择转办人员",
        width: 300,
        close: function () {
            SelectUserPopup.dialog('destroy').remove();
        },
        open: function () {
            if ($.ui && $.ui.dialog && !$.ui.dialog.prototype._allowInteractionRemapped && $(this).closest(".ui-dialog").length) {
                if ($.ui.dialog.prototype._allowInteraction) {
                    $.ui.dialog.prototype._allowInteraction = function (e) {
                        if ($(e.target).closest('.select2-drop').length) return true;
                        return ui_dialog_interaction.apply(this, arguments);
                    };
                    $.ui.dialog.prototype._allowInteractionRemapped = true;
                }
                else {
                    $.error("You must upgrade jQuery UI or else.");
                }
            }
        },
        _allowInteraction: function (event) {
            return !!$(event.target).is(".select2-input") || this._super(event);
        }
    });

    $(selectUser).select2(
        {
            placeholder: '输入工号或名字',
            minimumInputLength: 0,
            allowClear: false,
            ajax: {
                delay: 150,
                url: 'ProjectManagement/GetEmployeeList',
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
                            return { id: obj.EmployeeCode, text: obj.EmployeeName + "(" + obj.EmployeeCode + ")" };
                        }),
                        pagination: {
                            more: (params.page * 100) <= data.Total
                        }
                    };
                }
            }
        });
}

