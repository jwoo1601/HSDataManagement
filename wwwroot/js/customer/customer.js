$(function () {
    openLoadingDialog(
        "데이터 로딩 중",
        (close) => {
            fetchCustomerList().then(function () {
                close();
            });
        }
    );

    var searchForm = $("form[name=searchForm]");
    $("input[type=search]", searchForm)
        .on("propertychange input", function (e) {
            var valueChanged = false;

            if (e.type === "propertychange") {
                valueChanged = e.originalEvent.propertyName === 'value';
            } else {
                valueChanged = true;
            }

            if (valueChanged) {
                var category = $("select", searchForm).val();
                var value = $(this).val();

                if (value === "") {
                    $("#customerTable tbody tr").show();
                } else {
                    filterCustomerTable(category, value);
                }
            }
        });

    $("select", searchForm).change(function () {
        var category = $(this).val();
        var value = $("input[type=search]", searchForm).val();

        if (value === "") {
            $("#customerTable tbody tr").show();
        } else {
            filterCustomerTable(category, value);
        }
    });

    $("#reloadBtn").click(function () {
        openLoadingDialog(
            "데이터 로딩 중",
            (close) => {
                fetchCustomerListWithSettings().then(function () {
                    close();
                });
            }
        );
    });

    $("#addCustomerBtn").click(function () {
        $("#addCustomerModal")
            .modal({
                backdrop: "static"
            });
    });

    $("#addCustomerModal button[data-action=save]").click(function () {
        saveCustomerFromModal(
            $("#addCustomerModal"),
            "add"
        );
    });

    $("#editCustomerModal button[data-action=save]").click(function () {
        var modal = $("#editCustomerModal");

        saveCustomerFromModal(
            modal,
            "edit",
            $("input[name=id]", modal).val()
        );
    });

    var sortingSetttingsForms = $("form[name=sortingSettings]");
    sortingSetttingsForms
        .on("click", "[data-action=save]", function () {
            openLoadingDialog(
                "데이터 로딩 중",
                (close) => {
                    fetchCustomerListWithSettings().then(function () {
                        close();
                    });
                }
            );
        });

    $(".pagination").on("click", "#pagePrev", function () {
        openLoadingDialog(
            "페이지 로딩 중",
            (close) => {
                setPageNumber(getPageNumber() - 1);
                fetchCustomerListWithSettings().then(function () {
                    close();
                });
            }
        );
    });

    $(".pagination").on("click", "#pageNext", function () {
        openLoadingDialog(
            "페이지 로딩 중",
            (close) => {
                setPageNumber(getPageNumber() + 1);
                fetchCustomerListWithSettings().then(function () {
                    close();
                });
            }
        );
    });
});

function saveCustomerFromModal(modal, mode, customerId) {
    var form = $("form", modal);

    openLoadingDialog(
        "저장 중",
        (close) => {
            var name = $("input[name=name]", form).val();
            var admissionDate = $("input[name=admission_date]", form).val();
            var dischargeDate = $("input[name=discharge_date]", form).val();
            var tags = $(".custom-tag-container .custom-tag", form).map(function () {
                return $(this).text();
            }).get();
            var note = $("textarea[name=note]", form).val();

            if (name.trim() === "") {
                close();
                return;
            }

            form.addClass("was-validated");

            if (form.get(0).checkValidity()) {
                var customer = {
                    name,
                    admissionDate,
                    dischargeDate,
                    tags,
                    note
                };

                var requestUrl, requestMethod;
                if (mode === "add") {
                    requestUrl = "/api/customers";
                    requestMethod = "POST";
                } else {
                    requestUrl = `/api/customers/${customerId}`;
                    requestMethod = "PUT";
                }

                hm.sendAsync(requestMethod, requestUrl, {
                    data: customer
                }).done(function () {
                    fetchCustomerListWithSettings();
                }).always(function () {
                    close();
                    $(modal).modal("hide");
                });
            } else {
                close();
            }
        },
        form
    );
}

function filterCustomerTable(category, value) {
    var table = $("#customerTable");
    $("tbody tr", table)
        .hide();

    $("tbody tr", table)
        .filter(function () {
            var matchingValue = $(`[data-category=${category}]`, this).text();

            return matchingValue.includes(value);
        })
        .show();
}

function setPageNumber(num) {
    num = Math.max(1, num);

    $("form[name=sortingSettings] input[name=pageNumber]")
        .val(num);

    $(".pagination .page-link[data-page]")
        .text(num);
}

function getPageNumber() {
    return parseInt(
        $("form[name=sortingSettings] input[name=pageNumber]").val()
    );
}

function fetchCustomerListWithSettings() {
    var sortingSetttingsForms = $("form[name=sortingSettings]");
    var pageNumber = $("[name=pageNumber]").val();
    var numItems = $("[name=numItems]").val();
    var criteria = $("[name=sortingCriteria]", sortingSetttingsForms).val();
    var order = $("[name=sortingOrder]", sortingSetttingsForms).val();

    return fetchCustomerList({
        page: pageNumber,
        limit: numItems,
        criteria,
        order
    });
}

function fetchCustomerList(options) {
    var requestUrl = "/customer/display";
    var queries = [];
    options = options || {};

    if (options.page) {
        queries.push(`page=${options.page}`);
    }
    if (options.limit) {
        queries.push(`limit=${options.limit}`);
    }
    if (options.criteria) {
        queries.push(`criteria=${options.criteria}`);
    }
    if (options.order) {
        queries.push(`order=${options.order}`);
    }

    if (queries.length > 0) {
        requestUrl += "?" + queries.join("&");
    }

    return hm
        .getAsync(requestUrl, {
            responseType: "html"
        })
        .done(function (partialView) {
            $("#customerTable tbody")
                .html(partialView);

            bindCustomerActions();

            hm.getAsync("/api/customers")
                .done(function (customers) {
                    $("#numCustomers").text(
                        `${$("#customerTable tbody tr").length} / ${customers.length}`
                    );
                });
        });
}

function bindCustomerActions() {
    var customerRows = $("[data-customer-id]");

    $("[data-action=edit]", customerRows)
        .click(function () {
            openLoadingDialog(
                "로딩 중",
                (close) => {
                    var row = $(this).closest("tr");
                    var id = row.data("customer-id");

                    $.ajax(`/api/customers/${id}`)
                        .done(function (data, status, jqXHR) {
                            var customer = jqXHR.responseJSON;
                            var editModal = $("#editCustomerModal");

                            $("form", editModal).clearFm();

                            $("input[name=id]", editModal)
                                .val(`${id}`);
                            $("input[name=name]", editModal)
                                .val(customer.name);
                            $("input[name=admission_date]", editModal)
                                .val(customer.admission_date);
                            $("input[name=discharge_date]", editModal)
                                .val(customer.discharge_date);
                            customer.tags.forEach(tag => {
                                $(".custom-tag-container", editModal)
                                    .customTagFm(tag);
                            });
                            $("textarea[name=note]", editModal)
                                .val(customer.note);

                            $("#editCustomerModal")
                                .modal({
                                    backdrop: "static"
                                });
                        }).fail(function () {
                            row.remove();
                        }).then(function () {
                            close();
                        });
                }
            );
        });

    $("[data-action=visibility]", customerRows)
        .click(function () {
            openLoadingDialog(
                "변경 중",
                (close) => {
                    var row = $(this).closest("tr");
                    var id = row.data("customer-id");
                    var visibility = !JSON.parse(row.data("visibility"));
                    var options = {
                        isVisible: visibility
                    };

                    $.ajax(`/api/customers/options/${id}`, {
                        method: "PUT",
                        data: JSON.stringify(options),
                        contentType: "application/json"
                    }).done(function () {
                        var visibilityString = visibility.toString();

                        row.data("visibility", visibilityString);
                        $("input[name=visibility]", row)
                            .prop("checked", visibility);
                        $("[data-action=visibility] .btn-text", row)
                            .text(visibility ? "숨김" : "보임");
                    }).then(function () {
                        close();
                    });
                }
            );
        });

    $("[data-action=delete]", customerRows)
        .click(function () {
            openConfirmationDialog(
                "고객 정보 제거",
                `정말로 해당 고객의 정보를 모두 제거하시겠습니까? (제거 후 다시 되돌릴 수 없습니다.)`,
                () => {
                    openLoadingDialog(
                        "삭제 중",
                        (close) => {
                            var row = $(this).closest("tr");
                            var id = row.data("customer-id");

                            $.ajax(`/api/customers/delete/${id}`, {
                                method: "DELETE"
                            }).then(function () {
                                fetchCustomerListWithSettings();

                                close();
                            });
                        }
                    );
                }
            );
        });
}