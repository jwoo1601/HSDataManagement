(function ($) {
    $.fn.sortTableByCategory = function (category, order) {
        return sortTableByCategory(this, category, order);
    }

    $.fn.getTableOptions = function () {
        return getTableOptions(this);
    }

    $.fn.fetchTableData = function (url, options) {
        return fetchTableData(this, url, options)
    }

    $.fn.addTableEntry = function (obj, valueMappers) {
        return addTableEntry(this, obj, valueMappers);
    }

    $.fn.tablePagination = function (options) {
        return tablePagination(this, options);
    }

    $.fn.tableNavigatePage = function (page) {
        return tableNavigatePage(this, page);
    }

    $.fn.tableCurrentPage = function () {
        return tableGetCurrentPage(this);
    }
})(jQuery);

/*
 * @param {string} category the value of "data-category" in table headers
 * @param {"asc" | "desc"} order
 */
function sortTableByCategory(table, category, order) {
    var categoryIndex = $(`th[data-category="${category}"]`, table).index();

    var sorted = $("tbody tr:not([data-sort=none])", table)
        .detach()
        .get()
        .map(row =>
            ([
                $("td", $(row)).get(categoryIndex),
                row
            ])
        )
        .sort((a, b) => {
            var [colA, rowA] = a;
            var [colB, rowB] = b;
            var valA = $(colA).data("value") || $(colA).text();
            var valB = $(colB).data("value") || $(colB).text();

            if (valA == valB) {
                return 0;
            }

            if (order && order.toLowerCase() === "desc") {
                return valA < valB ? 1 : -1;
            }

            return valA > valB ? 1 : -1;
        });

    sorted.forEach(([col, row]) => {
        $("tbody", table).append($(row));
    });

    return table;
}

/*
 * @returns { 
 *  page: number, 
 *  numItems: number, 
 *  criteria: string, 
 *  order: "asc" | "desc" 
 * }
 */
function getTableOptions(table) {
    var form = $("form[name=tableOptions]").filter(function () {
        return $(this).data("target") === `#${table.attr("id")}`;
    });
    var page = table.data("page");
    var numItems = $("[name=numItems]", form).val();
    var criteria = $("[name=criteria]", form).val();
    var order = $("[name=order]", form).val();

    return {
        page: page || 1,
        numItems: numItems || 10,
        criteria: criteria || "id",
        order: order || "asc"
    };
}

function fetchTableData(table, url, options = {}) {
    var _options = {
        silent: options.silent === undefined ? false : options.silent,
        valueMappers: options.valueMappers || {}
    }

    function impl(table, url, valueMappers) {
        return $.ajax(
            url
        ).done(function (data, status, jqXHR) {
            var list = jqXHR.responseJSON;

            list.forEach(obj => {
                addTableEntry(table, obj, valueMappers);
            });
        });
    }

    // empty table
    $("tbody tr:not(.sample-row)", table).remove();

    if (_options.silent) {
        impl(table, url, _options.valueMappers);
    } else {
        openLoadingDialog(
            "데이터 로딩 중",
            (close) => {
                impl(table, url, _options.valueMappers)
                    .then(function () {
                        close();
                    });
            }
        );
    }

    return table;
}

function addTableEntry(table, obj, valueMappers) {
    var sampleRow = $(".sample-row", table);
    if (sampleRow.length === 0) {
        return table;
    }

    var newRow = sampleRow.clone();

    Object
        .keys(obj)
        .forEach(key => {
            var th = $(
                `th[data-category="${key}"]`,
                table
            );
            var categoryIndex = th.index();
            if (categoryIndex < 0) {
                return;
            }

            var td = $("td", newRow).get(categoryIndex);

            var mapper;
            if (valueMappers[key]) {
                mapper = valueMappers[key];
            } else {
                mapper = v => v;
            }

            var value = mapper(obj[key]);
            if (typeof th.data("norender") === "undefined") {
                var renderTarget = $("[data-render-target]", td);
                if (renderTarget.length === 0) {
                    renderTarget = $(td);
                }

                renderTarget.text(value);
            }

            $(td).data("value", value);
        });

    newRow
        .data("row-id", obj.id)
        .removeClass(
            [
                "d-none",
                "sample-row"
            ]
        )
        .removeAttr("data-sort");

    $("tbody", table).append(newRow);

    return table;
}

function tablePagination(table, options = {}) {
    var _options = {
        numItems: options.numItems || 10
    };

    var rows = $("tbody tr:not(.sample-row)", table);
    if (rows.length !== 0) {
        rows.each(function (idx) {
            $(this).data("page", Math.floor(idx / _options.numItems + 1));
        });

        tableNavigatePage(table, 1);
    }

    var numPages = Math.ceil(rows.length / _options.numItems);
    table.trigger("table.hm.paginate", {
        numPages
    });

    return table;
}

function tableNavigatePage(table, page) {
    var event = jQuery.Event("table.hm.navigate");
    if (!event.isDefaultPrevented()) {
        table.data("current-page", page);

        var rows = $("tbody tr:not(.sample-row)", table);
        rows.hide()
            .filter(function () {
                return $(this).data("page") == page;
            }).show();

        event.navigatedPage = page;

        table.trigger(event);
    }

    return table;
}

function tableGetCurrentPage(table) {
    return table.data("current-page");
}

function tableFilterByCategory(table, category, filter) {
    var event = jQuery.Event("table.hm.filter");
    if (!event.isDefaultPrevented()) {
        var rows = $("tbody tr:not(.sample-row)", table);
        rows.hide()
            .filter(function () { });


        if (typeof filter === "function") {

        }
    }
}