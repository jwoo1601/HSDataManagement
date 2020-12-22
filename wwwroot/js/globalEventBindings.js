/*
 * Hyosung Management Global Event Bindings
 * © 2020 HyosungManagement
 */

$(function () {
    // #region Bootstrap Global Setup
    // Enable global tooltips
    $("[data-toggle=tooltip]").tooltip({
        boundary: "window"
    });
    // #endregion

    // #region moment.js Global Setup
    //moment.local("ko");
    // #endregion

    // Login Button
    $("#loginBtn").click(function () {
        //$("#loginModal").modal({
        //    backdrop: "static",
        //    keyboard: false
        //});

        redirectTo("/login");
        //redirectTo(`/login?redirect_url=${window.location.pathname}`);
    });

    // Register Button
    $("#registerBtn").click(function () {
        redirectTo("/register");
    });

    // Numeric Only Inputs
    $("input.numeric-only").keyup(function (e) {
        if (/[^0-9]|^0+(?!$)/g.test(this.value)) {
            this.value = this.value.replace(/[^0-9]|^0+(?!$)/g, '');
        }
    });

    // Modals
    $(".modal[data-trigger-action]")
        .on("show.bs.modal", function () {
            switch ($(this).data("trigger-action")) {
                case "form-reset":
                    $("form", this).clearFm();
                    break;
                case "clear-table":
                    $("table", this).clearTbl();
                    break;
            }
        });

    // Forms
    $("form")
        .not("[data-legacy]")
        .on("submit", function (e) {
            e.preventDefault();
        });
});

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

/**
 * 
 * @param {any} message
 * @param {any} action
 * @param {<JQuery<HTMLFormElement>=} boundForm
 */
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

/**
 * 
 * @param {string} title
 * @param {string} message
 * @param {() => void} onConfirm
 * @param {() => void} [onCancel]
 */
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

function openErrorDialog(title, message) {
    var modal = $("#errorModal");
    $(".modal-title", modal).text(title);
    $(".modal-message", modal).text(message);

    modal.modal({
        backdrop: "static"
    });
}

function redirectTo(url) {
    window.location.href = url;
}