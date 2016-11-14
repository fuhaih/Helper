(function ($) {
    var CNDataTable;
    CNDataTable = function (table, option) {
        var tableValue = [];
        tableValue.push(setHead(table.Columns));
        tableValue.push(setFooter(table.Columns));
        tableValue.push(setBody(table.Rows));
        this.append(tableValue.join());
        //return this.dataTable(option);
    }
    CNDataTable.TableHtml = "";
    var setCol = function (cols) {
        var col = [];
        col.push("<tr>");
        cols.forEach(function (e, index) {
            col.push("<th>");
            col.push(e);
            col.push("</th>");
        })
        col.push("</tr>");
        return col.join("");
    }
    var setHead = function (cols) {
        var head = [];
        head.push("<thead>");
        head.push(setCol(cols));
        head.push("</thead>");
        return head.join("");
    }
    var setFooter = function (cols) {
        var footer = [];
        footer.push("<footer>");
        footer.push(setCol(cols));
        footer.push("</footer>");
        return footer.join("");
    }
    var setBody = function (rows) {
        var body = [];
        body.push("<tbody>");
        rows.forEach(function (values, index) {
            body.push("<tr>");
            values.forEach(function (value, subindex) {
                body.push("<td>");
                body.push(value);
                body.push("</td>");
            });
            body.push("</tr>");
        });
        body.push("</tbody>");
        return body.join("");
    }
    $.extend(CNDataTable, {
        setCol:setCol,
        setHead: setHead,
        setFooter: setFooter,
        setBody: setBody
    });
    $.fn.CnDataTable = CNDataTable;
    return $.fn.CnDataTable;
}(window.jQuery))
$("#mainTable").CnDataTable({
    Columns: ["name", "value", "index"],
    Rows: [
    ["fuhai","13","1"],
    ["yy","14","2"],
    ["cg","15","3"]]
});