$(function () {
    // Enable Tooltips Globally
    $("[data-toggle=tooltip]").tooltip({
        boundary: "window"
    });

    // Login Button
    $("#loginBtn").click(function () {
        $("#loginModal").modal({
            backdrop: "static",
            keyboard: false
        });
    });

    // Register Button
    $("#registerBtn").click(function () {
        redirectTo("/account/register");
    });

    // Numeric Only Inputs
    $("input.numeric-only").keyup(function (e) {
        if (/[^0-9]|^0+(?!$)/g.test(this.value)) {
            this.value = this.value.replace(/[^0-9]|^0+(?!$)/g, '');
        }
    });

    // Custom Tags
    $(".add-custom-tag").click(function () {
        addCustomTag(
            $(this).closest(".custom-tag-container")
        );
    });

    // Modals
    $(".modal[data-trigger-action]")
        .on("show.bs.modal", function () {
            switch ($(this).data("trigger-action")) {
                case "form-reset":
                    clearForm($("form", this));
                    break;
            }
        });

    // Table Options
    $("form[name=tableOptions] [data-action=save]").click(function () {
        openLoadingDialog(
            "데이터 정렬 중",
            (close) => {
                var form = $(this).closest("form[name=tableOptions]");
                var table = $(form.data("target"));
                var options = table.getTableOptions();

                table.sortTableByCategory(
                    options.criteria,
                    options.order
                );

                close();
            }
        );
    });
});

function addCustomTag(container) {
    addCustomTagWithText(container, "새 태그");
}

function addCustomTagWithText(container, text) {
    var newTag = $(`<div class="custom-tag"><span class="custom-tag-text" contenteditable>${text}</span></div>`);

    $(".custom-tag-text", newTag)
        .after('<span class="custom-tag-remove oi oi-x ml-2" aria-hidden="true"></span>');

    $(".custom-tag-text", newTag)
        .focusout(function () {
            if ($(this).text().length === 0) {
                newTag.remove();
            }
        });

    $(".custom-tag-remove", newTag)
        .click(function () {
            newTag.remove();
        });

    newTag.insertBefore($(".add-custom-tag", container));
}

function clearForm(form) {
    $(form)
        .find("input, select, textarea")
        .val("");

    $(form)
        .find("input[type=checkbox], input[type=radio]")
        .prop("checked", false);

    $(form)
        .find(".custom-tag-container .custom-tag")
        .remove();

    $(form).removeClass("was-validated");
}

function showLoadingDialog(form, message) {
    $("input, select, textarea", $(form))
        .prop("readonly", true);

    var loadingModal = $("#loadingModal");
    $(".loading-text", loadingModal)
        .text(message);

    loadingModal.modal({
        backdrop: "static",
        keyboard: false
    });
}

function hideLoadingDialog(form) {
    $("input, select, textarea", $(form))
        .prop("readonly", false);

    $("#loadingModal").modal("hide");
}

function openLoadingDialog(
    message,
    action, // (done: () => void): void
    boundForm
) {
    if (boundForm) {
        $("input, select, textarea", $(boundForm))
            .prop("readonly", true);
    }

    var loadingModal = $("#loadingModal");
    $(".loading-text", loadingModal)
        .text(message);

    loadingModal.modal({
        backdrop: "static",
        keyboard: false
    });

    action(() => {
        if (boundForm) {
            $("input, select, textarea", $(boundForm))
                .prop("readonly", false);
        }

        loadingModal.modal("hide");
    });
}

function openConfirmationDialog(
    title,
    message,
    onConfirm,
    onCancel
) {
    var modal = $("#confirmationModal");
    $(".modal-title", modal).text(title);
    $(".modal-message", modal).text(message);

    $("[data-action=confirm]", modal)
        .one("click", function () {
            modal.modal("hide");
            onConfirm();
        });
    $("[data-action=cancel]", modal)
        .one("click", function () {
            if (onCancel) {
                onCancel();
            }
        });

    modal.modal({
        backdrop: "static",
        keyboard: false
    });
}

function redirectTo(url) {
    window.location.href = url;
}