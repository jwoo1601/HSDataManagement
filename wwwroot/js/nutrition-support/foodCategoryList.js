$(function () {
    $("#menuListTabCategory").one("shown.bs.tab", function () {
        fetchFoodCategoryList();
    });

    $("#foodCategoryTable")
        .on("click", ".show-edit-dialog", function () {
            openEditFoodCategoryDialog(
                $(this).closest("tr").data("row-id")
            );
        })
        .on("click", "[data-action=delete]", function () {
            openConfirmationDialog(
                "메뉴 삭제",
                "정말로 해당 카테고리를 삭제하시겠습니까? (다시 되돌릴 수 없습니다.)",
                () => {
                    openLoadingDialog(
                        "삭제 중",
                        (close) => {
                            var row = $(this).closest("tr");

                            hm
                                .deleteAsync(
                                    "/api/nutrition-support/foods/categories/delete",
                                    {
                                        param: row.data("row-id")
                                    }
                                )
                                .done(function () {
                                    fetchFoodCategoryList()
                                        .always(function () {
                                            close()
                                        });
                                })
                                .fail(function (err) {
                                    close();
                                    openErrorDialog(
                                        "카테고리 삭제 오류",
                                        "해당 카테고리 삭제를 실패하였습니다. 잠시 후 다시 시도해주세요."
                                    );
                                });
                        }
                    );
                }
            );
        });

    $("#addFoodCategoryModal")
        .on("click", "[data-action=save]", function () {
            var modal = $(this).closest(".modal");

            openLoadingDialog(
                "저장 중",
                (close) => {
                    $("form[name=addFoodCategory]", modal)
                        .sendJSONAsyncFm()
                        .done(function () {
                            fetchFoodCategoryList()
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

    $("#editFoodCategoryModal")
        .on("click", "[data-action=save]", function () {
            var modal = $(this).closest(".modal");

            openLoadingDialog(
                "변경사항 저장 중",
                (close) => {
                    $("form[name=editFoodCategory]", modal)
                        .sendJSONAsyncFm(
                            {},
                            {
                                param: $("[name=id]", modal).val()
                            }
                        )
                        .done(function () {
                            fetchFoodCategoryList()
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

function openAddFoodCategoryDialog() {
    openFoodCategoryDetailDialog("add");
}

function openEditFoodCategoryDialog(rowId) {
    openFoodCategoryDetailDialog("edit", { rowId });
}

/**
 * 
 * @param {"add" | "edit"} mode
 */
function openFoodCategoryDetailDialog(mode, params) {
    var modal = $(`#${mode}FoodCategoryModal`);
    var form = $(`form[name=${mode}FoodCategory]`).clearFm();

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
                        `/api/nutrition-support/foods/categories/${params.rowId}`
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
                            "카테고리 데이터를 불러올 수 없습니다. 잠시 후 다시 시도해 주십시오."
                        );
                    });
            }
        );
    }
}

function fetchFoodCategoryList() {
    var table = $("#foodCategoryTable");

    return table
        .fetchDataTbl("/api/nutrition-support/foods/categories")
        .done(function (list) {
            $("tbody tr:not(.sample-row)", table)
                .each(function () {
                    var note = list.find(
                        category => category.id === $(this).data("row-id")
                    ).note;
                    if (note) {
                        var detailButton = $("[data-action=detail]", this);
                        detailButton.tooltip({
                            container: detailButton,
                            placement: "right",
                            title: note,
                            trigger: "hover"
                        });
                    }
                });
        });
}