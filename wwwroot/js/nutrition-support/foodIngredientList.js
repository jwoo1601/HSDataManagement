$(function () {
    $("#menuListTabIngredient").one("shown.bs.tab", function () {
        fetchFoodIngredientList();
    });

    $("#foodIngredientTable")
        .on("click", ".show-edit-dialog", function () {
            openEditFoodIngredientDialog(
                $(this).closest("tr").data("row-id")
            );
        })
        .on("click", "[data-action=delete]", function () {
            openConfirmationDialog(
                "메뉴 삭제",
                "정말로 해당 재료를 삭제하시겠습니까? (다시 되돌릴 수 없습니다.)",
                () => {
                    openLoadingDialog(
                        "삭제 중",
                        (close) => {
                            var row = $(this).closest("tr");

                            hm
                                .deleteAsync(
                                    "/api/nutrition-support/foods/ingredients/delete",
                                    {
                                        param: row.data("row-id")
                                    }
                                )
                                .done(function () {
                                    fetchFoodIngredientList()
                                        .always(function () {
                                            close()
                                        });
                                })
                                .fail(function (err) {
                                    close();
                                    openErrorDialog(
                                        "재료 삭제 오류",
                                        "해당 재료 삭제를 실패하였습니다. 잠시 후 다시 시도해주세요."
                                    );
                                });
                        }
                    );
                }
            );
        });

    $("#addFoodIngredientModal")
        .on("click", "[data-action=save]", function () {
            var modal = $(this).closest(".modal");

            openLoadingDialog(
                "저장 중",
                (close) => {
                    $("form[name=addFoodIngredient]", modal)
                        .sendJSONAsyncFm()
                        .done(function () {
                            fetchFoodIngredientList()
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

    $("#editFoodIngredientModal")
        .on("click", "[data-action=save]", function () {
            var modal = $(this).closest(".modal");

            openLoadingDialog(
                "변경사항 저장 중",
                (close) => {
                    $("form[name=editFoodIngredient]", modal)
                        .sendJSONAsyncFm(
                            {},
                            {
                                param: $("[name=id]", modal).val()
                            }
                        )
                        .done(function () {
                            fetchFoodIngredientList()
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
});

function openAddFoodIngredientDialog() {
    openFoodIngredientDetailDialog("add");
}

function openEditFoodIngredientDialog(rowId) {
    openFoodIngredientDetailDialog("edit", { rowId });
}

/**
 * 
 * @param {"add" | "edit"} mode
 */
function openFoodIngredientDetailDialog(mode, params) {
    var modal = $(`#${mode}FoodIngredientModal`);
    var form = $(`form[name=${mode}FoodIngredient]`).clearFm();

    if (mode === "add") {
        close();
        modal.modal({
            backdrop: "static"
        });
    } else if (mode === "edit") {
        openLoadingDialog(
            "로딩 중",
            (close) => {
                form
                    .fetchAsyncFm(
                        `/api/nutrition-support/foods/ingredients/${params.rowId}`
                    )
                    .done(function () {
                        close();
                        modal.modal({
                            backdrop: "static"
                        });
                    })
                    .fail(function () {
                        close();

                        openErrorDialog(
                            "오류",
                            "재료 데이터를 불러올 수 없습니다. 잠시 후 다시 시도해 주십시오."
                        );
                    });
            }
        );
    }
}

function fetchFoodIngredientList() {
    return $("#foodIngredientTable")
        .fetchDataTbl("/api/nutrition-support/foods/ingredients");
}