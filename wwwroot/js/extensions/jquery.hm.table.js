(function ($) {
    const fn = {};

    /**
     * sorts table by the value of "data-category"
     * @param {JQuery<HTMLTableElement>} table
     * @param {string} category
     * @param {"asc" | "desc"} order
     */
    fn.sortByCategory = function (table, category, order = "asc") {
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
    };

    /**
     * @param {JQuery<HTMLTableElement>} table
     * @returns {{  
         numItems: number, 
         criteria: string, 
         order: "asc" | "desc" 
     }}
    */
    fn.getOptions = function (table) {
        var form = $("form[name=tableOptions]").filter(function () {
            return $(this).data("target") === `#${table.attr("id")}`;
        });
        var numItems = $("[name=numItems]", form).val();
        var criteria = $("[name=criteria]", form).val();
        var order = $("[name=order]", form).val();

        return {
            numItems: numItems || 10,
            criteria: criteria || "id",
            order: order || "asc"
        };
    };

    /**
     * 
     * @param {JQuery<HTMLTableElement>} table
     * @param {string} url
     * @param {{silent?: boolean, valueMappers?: Map<string, (rawValue: any) => any>}} options
     */
    fn.fetchData = function (table, url, options = {}) {
        var _options = {
            silent: options.silent === undefined ? false : options.silent,
            valueMappers: options.valueMappers || {}
        }

        var deferred = $.Deferred();

        function impl() {
            return hm.getAsync(url)
                .done(function (list) {
                    var event = jQuery.Event("table.hm.fetch");
                    if (!event.isDefaultPrevented()) {
                        event.ajaxData = list;

                        list.forEach(obj => {
                            fn.addRowFromObject(table, obj, _options.valueMappers);
                        });

                        table.trigger(event);
                    }

                    deferred.resolve(list);
                })
                .fail(function (err) {
                    deferred.reject(err);
                });
        }

        fn.clear(table);

        if (_options.silent) {
            impl();
        } else {
            openLoadingDialog(
                "데이터 로딩 중",
                (close) => {
                    impl()
                        .always(function () {
                            close();
                        });
                }
            );
        }

        return deferred;
    };

    fn.clear = function (table) {
        $("tbody tr:not(.sample-row)", table).remove();

        return table;
    }

    /**
     * 
     * @param {JQuery<HTMLTableElement>} table
     * @param {object} obj
     * @param {Map<string, (rawValue: any) => any>} valueMappers
     */
    fn.addRowFromObject = function (table, obj, valueMappers) {
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
    };

    /**
     * 
     * @param {JQuery<HTMLTableElement>} table
     * @param {{numItems?: number}} options
     */
    fn.pagination = function (table, options = {}) {
        var _options = {
            numItems: options.numItems || 10
        };

        var rows = $("tbody tr:not(.sample-row)", table);
        if (rows.length !== 0) {
            rows.each(function (idx) {
                $(this).data("page", Math.floor(idx / _options.numItems + 1));
            });
        }

        var numPages = Math.ceil(rows.length / _options.numItems);
        fn.pages(table, numPages);
        table.trigger("table.hm.paginate", {
            numPages
        });

        fn.navigatePage(table, 1);

        return table;
    };

    /**
     * 
     * @param {JQuery<HTMLTableElement>} table
     * @param {number} page
     */
    fn.navigatePage = function (table, page) {
        var currentPage = fn.pages(table);
        if (page < 1 || (currentPage && page > currentPage)) {
            return table;
        }

        var event = jQuery.Event("table.hm.navigate");
        if (!event.isDefaultPrevented()) {
            fn.currentPage(table, page);

            var rows = $("tbody tr:not(.sample-row)", table);
            rows.hide()
                .filter(function () {
                    return $(this).data("page") == page;
                }).show();

            event.navigatedPage = page;

            table.trigger(event);
        }

        return table;
    };

    /**
     * 
     * @param {JQuery<HTMLTableElement>} table
     * @param {number} [value]
     * @returns {number}
     */
    fn.currentPage = function (table, value) {
        return value ?
            table.data("current-page", value) :
            table.data("current-page");
    };

    /**
     *
     * @param {JQuery<HTMLTableElement>} table
     * @param {number} [value]
     * @returns {number}
     */
    fn.pages = function (table, value) {
        return value ?
            table.data("pages", value) :
            table.data("pages");
    };

    /**
     * 
     * @param {JQuery<HTMLTableElement>} table
     * @param {string} category
     * @param {any} filter
     */
    fn.filterByCategory = function (table, category, filter) {
        var event = jQuery.Event("table.hm.filter");
        if (!event.isDefaultPrevented()) {
            var th = $(
                `th[data-category="${category}"]`,
                table
            );
            var categoryIndex = th.index();
            if (categoryIndex < 0) {
                return $();
            }

            var rows = $("tbody tr:not(.sample-row)", table);

            return rows.filter(function () {
                var currentValue = "" + $($("td", this).get(categoryIndex)).data("value");

                if (typeof filter === "string") {
                    return currentValue.includes(filter);
                } else if (typeof filter === "function") {
                    return filter(currentValue);
                }

                return currentValue === filter;
            });
        }

        return $();
    };

    //Object
    //    .entries(fn)
    //    .forEach(([k, fn]) => {
    //        $.fn[`${k}Tbl`] = function (...args) {
    //            return fn(this, ...args);
    //        };
    //    });

    Object
        .entries(fn)
        .forEach(([k, fn]) => {
            $.fn[`${k}Tbl`] = function () {
                return fn(this, arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
            };
        });

    // Table Options
    $("form[name=tableOptions] [data-action=save]").click(function () {
        openLoadingDialog(
            "데이터 정렬 중",
            (close) => {
                var form = $(this).closest("form[name=tableOptions]");
                var table = $(form.data("target"));
                var options = table.getOptionsTbl();

                table
                    .paginationTbl({
                        numItems: options.numItems
                    })
                    .sortByCategoryTbl(
                        options.criteria,
                        options.order
                    );

                close();
            }
        );
    });

    // Table Pagination
    $(".tablePagination")
        .on("click", "[data-action=prev]", function () {
            var pagination = $(this).closest(".tablePagination");
            var table = $(pagination.data("target"));
            table.navigatePageTbl(
                table.currentPageTbl() - 1
            );
        })
        .on("click", "[data-action=next]", function () {
            var pagination = $(this).closest(".tablePagination");
            var table = $(pagination.data("target"));
            table.navigatePageTbl(
                table.currentPageTbl() + 1
            );
        });

    // onTableFetch
    $("table")
        .on("table.hm.fetch", function () {
            var options = $(this).getOptionsTbl();

            $(this)
                .paginationTbl({
                    numItems: options.numItems
                })
                .sortByCategoryTbl(
                    options.criteria,
                    options.order
                );
        })
        .on("table.hm.navigate", function (e) {
            var table = $(this);

            $(".tablePagination")
                .filter(function () {
                    return $(this).data("target") === `#${table.attr("id")}`;
                })
                .find(".page-text")
                .text(`${e.navigatedPage} / ${table.pagesTbl()}`);
        });


    $("[data-bind=table]")
        .on(
            "propertychange input",
            '[data-role="search-filter"][data-action=filter]',
            function (e) {
                var valueChanged = false;

                if (e.type === "propertychange") {
                    valueChanged = e.originalEvent.propertyName === 'value';
                } else {
                    valueChanged = true;
                }

                if (valueChanged) {
                    var parent = $(this).closest("[data-bind=table]");
                    var table = $(parent.data("target"));
                    var category = $("[data-role=category]", parent).val();
                    var value = $(this).val();
                    var page = table.currentPageTbl();

                    if (value === "") {
                        if (page) {
                            table.navigatePageTbl(
                                page
                            );
                        } else {
                            $("tbody tr", table).show();
                        }
                    } else {
                        $("tbody tr", table)
                            .hide();

                        table
                            .filterByCategoryTbl(category, value)
                            .filter(function () {
                                return $(this).data("page") == page;
                            })
                            .show();
                    }
                }
            }
        );

})(jQuery);