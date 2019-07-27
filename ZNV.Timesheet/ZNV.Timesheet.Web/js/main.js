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

//把字符串转换成日期格式，原格式是2016-11-16T08:44:37Z这样的
function ConvertStringToDatetime (dateString) {
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
function ConvertStringToDatetimeEx (str,contansTime=false) {
    if (str && str.startsWith('/Date') && str.endsWith(')/')) {
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
function ConvertStringToDatetimeObject (str) {
    if (str && str.startsWith('/Date') && str.endsWith(')/')) {
        var d = eval('new ' + str.substr(1, str.length - 2));
        return d;
    }
    return null;
}