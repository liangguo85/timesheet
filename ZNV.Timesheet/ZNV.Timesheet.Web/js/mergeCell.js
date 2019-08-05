function MergeCommonRows(table, columnIndexsToMerge) {
    var firstColumnBrakes = [];
    // iterate through the columns instead of passing each column as function parameter:
    for (var i = 0; i < columnIndexsToMerge.length; i++) {
        var previous = null, cellToExtend = null, rowspan = 1;
        table.find("td:nth-child(" + columnIndexsToMerge[i] + ")").each(function (index, e) {
            var jthis = $(this), content = jthis.text();
            // check if current row "break" exist in the array. If not, then extend rowspan:
            if (previous == content && content !== "" && $.inArray(index, firstColumnBrakes) === -1) {
                // hide the row instead of remove(), so the DOM index won't "move" inside loop.
                jthis.addClass('hidden');
                cellToExtend.attr("rowspan", (rowspan = rowspan + 1));
            } else {
                // store row breaks only for the first column:
                if (i === 1) firstColumnBrakes.push(index);
                rowspan = 1;
                previous = content;
                cellToExtend = jthis;
            }
        });
    }
    // now remove hidden td's (or leave them hidden if you wish):
    $('td.hidden').remove();
    console.info("MergeCommonRows finished!")
}

function mergeCommonColumns(table) {
    var firstColumnBrakes = [];
    var previous = null, cellToExtend = null, colspan = 1;
    table.find("thead tr th").each(function (index, e) {
        var jthis = $(this), content = jthis.text();
        if (previous == content && content !== "" && $.inArray(index, firstColumnBrakes) === -1) {
            jthis.addClass('hidden');
            cellToExtend.attr("colspan", (colspan = colspan + 1));
        } else {
            if (index === 1) firstColumnBrakes.push(index);
            colspan = 1;
            previous = content;
            cellToExtend = jthis;
        }
    });
    // now remove hidden td's (or leave them hidden if you wish):
    $('td.hidden').remove();
    console.info("mergeCommonColumns finished!")
};