$(function () {
    $("#menuListTabFood").one("shown.bs.tab", function () {
        fetchFoodList();
    });

    $("#foodTable")
        .on("click", ".show-edit-dialog", function () {
            openEditFoodDialog(
                $(this).closest("tr").data("row-id")
            );
        })
        .on("click", "[data-action=delete]", function () {
            openConfirmationDialog(
                "메뉴 삭제",
                "정말로 해당 메뉴를 삭제하시겠습니까? (다시 되돌릴 수 없습니다.)",
                () => {
                    openLoadingDialog(
                        "삭제 중",
                        (close) => {
                            var row = $(this).closest("tr");

                            hm
                                .deleteAsync(
                                    "/api/nutrition-support/foods/delete",
                                    {
                                        param: row.data("row-id")
                                    }
                                )
                                .done(function () {
                                    fetchFoodList()
                                        .always(function () {
                                            close()
                                        });
                                })
                                .fail(function (err) {
                                    close();
                                    openErrorDialog(
                                        "메뉴 삭제 오류",
                                        "해당 메뉴 삭제를 실패하였습니다. 잠시 후 다시 시도해주세요."
                                    );
                                });
                        }
                    );
                }
            );
        });

    $("[data-action=select-ingredients]")
        .on("click", function () {
            var currentModal = $(this).closest(".modal").modal("hide");
            openSelectFoodIngredientsDialog(
                currentModal
            );
        });

    $("#addFoodModal")
        .on("data.hm.load", function () {
            fetchSelectFoodIngredientsTable();
        })
        .on("click", "[data-action=save]", function () {
            var modal = $(this).closest(".modal");

            openLoadingDialog(
                "저장 중",
                (close) => {
                    $("form[name=addFood]", modal)
                        .sendJSONAsyncFm({
                            ingredients: $("#selectFoodIngredientsTable tbody tr.selected")
                                .map(function () {
                                    return $(this).data("row-id")
                                })
                                .get()
                        })
                        .done(function () {
                            fetchFoodList()
                                .always(function () {
                                    modal.modal("hide");
                                });
                        })
                        .always(function () {
                            close();
                        });
                }
            );
        });

    $("#editFoodModal")
        .on("click", "[data-action=save]", function () {
            var modal = $(this).closest(".modal");

            openLoadingDialog(
                "변경사항 저장 중",
                (close) => {
                    $("form[name=editFood]", modal)
                        .sendJSONAsyncFm(
                            {
                                ingredients: $("#selectFoodIngredientsTable tbody tr.selected")
                                    .map(function () {
                                        return $(this).data("row-id")
                                    })
                                    .get()
                            },
                            {
                                param: $("[name=id]", modal).val()
                            }
                        )
                        .done(function () {
                            fetchFoodList()
                                .always(function () {
                                    modal.modal("hide");
                                });
                        })
                        .always(function () {
                            close();
                        });
                }
            );
        });

    $("#selectFoodIngredientsModal")
        .on("click", "[data-action=reload]", function () {
            openLoadingDialog(
                "데이터 로딩 중",
                (close) => {
                    fetchSelectFoodIngredientsTable()
                        .done(function () {
                            close();
                        })
                        .fail(function () {
                            close();
                            openErrorDialog(
                                "연결 오류",
                                "데이터를 불러올 수 없습니다. 다시 시도해 주십시오."
                            );
                        });
                }
            );
        })
        .on("click", "[data-action=select]", function () {
            var currentModal = $(this).closest(".modal");
            var prevModal = currentModal.data("bind-target");

            $("[data-category=numIngredients]", prevModal)
                .text(
                    $(
                        "#selectFoodIngredientsTable tbody tr.selected",
                        currentModal
                    ).length
                );

            currentModal.modal("hide");
            prevModal.modal({
                backdrop: "static"
            });
        })
        .on("click", "[data-action=cancel]", function () {
            var currentModal = $(this).closest(".modal");
            var prevModal = currentModal.data("bind-target");

            $("#selectFoodIngredientsTable tbody tr.selected")
                .removeClass("selected");

            $(".modal-footer .message", currentModal)
                .text("");

            currentModal.modal("hide");
            prevModal.modal({
                backdrop: "static"
            });
        })
        .on("click", "#selectFoodIngredientsTable tbody tr", function () {
            toggleFoodIngredient(
                $(this).data("row-id")
            );
        });
});

function fetchFoodList() {
    var table = $("#foodTable");

    return table
        .fetchDataTbl(
            "/api/nutrition-support/foods",
            {
                valueMappers: {
                    category: c => c.name,
                    ingredients: ig => ig.length
                }
            }
        )
        .done(function (list) {
            $("tbody tr:not(.sample-row)", table)
                .each(function () {
                    var detailButton = $("[data-action=detail]", this);
                    detailButton
                        .tooltip({
                            container: detailButton,
                            placement: "bottom",
                            title: list
                                .find(
                                    food => food.id === $(this).data("row-id")
                                )
                                .ingredients
                                .map(ing => `${ing.name} (${ing.origin})`)
                                .join(", "),
                            trigger: "hover"
                        });
                });
        });
}

function openAddFoodDialog() {
    openFoodDetailDialog("add");
}

function openEditFoodDialog(rowId) {
    openFoodDetailDialog("edit", { rowId });
}

/**
 * 
 * @param {"add" | "edit"} mode
 */
function openFoodDetailDialog(mode, params) {
    var modal = $(`#${mode}FoodModal`);
    var form = $(`form[name=${mode}Food]`).clearFm();

    openLoadingDialog(
        "로딩 중",
        (close) => {
            hm.getAsync("/api/nutrition-support/foods/categories")
                .done(function (categories) {
                    var categorySelect = $("[name=category]", form);
                    categorySelect
                        .find("option.category-entry")
                        .remove();

                    categories
                        .map(c => `<option value="${c.id}" class="category-entry">${c.name}</option>`)
                        .forEach(option => {
                            categorySelect.append(option);
                        });

                    var numIngredientsDisplay = $("[data-category=numIngredients]", form);
                    if (mode === "add") {
                        modal.trigger("data.hm.load");
                        numIngredientsDisplay.text("0");

                        close();
                        modal.modal({
                            backdrop: "static"
                        });
                    } else if (mode === "edit") {
                        fetchSelectFoodIngredientsTable()
                            .done(function () {
                                form
                                    .fetchAsyncFm(
                                        `/api/nutrition-support/foods/${params.rowId}`,
                                        {
                                            setters: {
                                                ingredients: function (ingredients) {
                                                    ingredients.forEach(ing => {
                                                        toggleFoodIngredient(ing.id);
                                                    });
                                                },
                                                category: function (category) {
                                                    this.val(category.id);
                                                }
                                            },
                                        }
                                    ).done(function (food) {
                                        numIngredientsDisplay.text(food.ingredients.length);

                                        close();
                                        modal.modal({
                                            backdrop: "static"
                                        });
                                    })
                                    .fail(function () {
                                        close();

                                        openErrorDialog(
                                            "오류",
                                            "음식 데이터를 불러올 수 없습니다. 잠시 후 다시 시도해 주십시오."
                                        );
                                    });
                            })
                            .fail(function () {
                                close();

                                openErrorDialog(
                                    "오류",
                                    "재료 목록을 불러올 수 없습니다. 잠시 후 다시 시도해 주십시오."
                                );
                            });
                    }
                })
                .fail(function (err) {
                    close();

                    openErrorDialog(
                        "오류",
                        "카테고리 목록을 불러올 수 없습니다. 잠시 후 다시 시도해 주십시오."
                    );
                });
        }
    );
}

function fetchSelectFoodIngredientsTable() {
    return $("#selectFoodIngredientsTable")
        .fetchDataTbl("/api/nutrition-support/foods/ingredients");
}

function toggleFoodIngredient(ingredientId) {
    var table = $("#selectFoodIngredientsTable");

    $("tbody tr:not(.sample-row)", table)
        .filter(function () {
            return $(this).data("row-id") === ingredientId;
        })
        .toggleClass("selected");

    var message = "";
    var numSelected = $("tbody tr.selected", table).length;
    if (numSelected !== 0) {
        message = `(${numSelected}개 선택됨)`
    }

    $("#selectFoodIngredientsModal .modal-footer .message")
        .text(message);
}

function openSelectFoodIngredientsDialog(boundModal) {
    $("#selectFoodIngredientsModal")
        .data("bind-target", boundModal)
        .modal({
            backdrop: "static",
            keyboard: false
        });
}