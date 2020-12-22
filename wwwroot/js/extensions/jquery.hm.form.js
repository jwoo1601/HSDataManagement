(function ($) {
    const selectors = {
        customTag: ".custom-tag",
        customTagContainer: ".custom-tag-container",
        customTagText: ".custom-tag-text",
        customTagRemove: ".custom-tag-remove",
        customTagAdd: ".add-custom-tag"
    }

    const fn = {};

    fn.clear = function (form) {
        form.get(0).reset();

        $(form)
            .find(selectors.customTag, selectors.customTagContainer)
            .remove();

        $(form).removeClass("was-validated");

        return form;
    };

    /**
     * 
     * @param {JQuery<HTMLFormElement>} form
     * @param {string} url
     * @param {{
     *  setters: Map<string, (this: JQuery<HTMLElement>, value: any): void>,
     *  ignore: string[]
     * }} [options]
     */
    fn.fetchAsync = function (form, url, options = {}) {
        const _options = {
            setters: options.setters || {},
            ignore: options.ignore || []
        }

        return hm.getAsync(url)
            .done(function (data) {
                Object
                    .entries(data)
                    .filter(([k, v]) => !_options.ignore.includes(k))
                    .forEach(([k, v]) => {
                        var target = $(`[name="${k}"], [data-name="${k}"]`, form);
                        if (target.length === 0) {
                            return;
                        }

                        if (_options.setters[k]) {
                            _options.setters[k].call(target, v);
                        } else {
                            target.val(v);
                        }
                    });
            });
    }

    fn.sendJSONAsync = function (form, additionalData = {}, options = {}) {
        var _options = Object.assign({}, options, {
            validators: options.validators || {}
        });

        $("input, select, textarea", form)
            .removeClass("is-valid")
            .removeClass("is-invalid");

        form.addClass("was-validated");

        var rawForm = form.get(0);
        if (!rawForm.checkValidity()) {
            return $.Deferred().reject();
        }

        var method = form.attr("method");
        var url = form.attr("action");

        var data = {};
        $("input, select, textarea", form)
            .not("[data-ignore]")
            .not("[type=checkbox]")
            .not("[type=radio]")
            .each(function () {
                var key = $(this).get(0).name;
                var value = $(this).val();

                data[key] = value;
            });

        $("input[type=checkbox], input[type=radio]", form)
            .not("[data-ignore]")
            .filter(function () {
                return $(this).prop("checked");
            })
            .each(function () {
                var key = $(this).get(0).name;
                var value = $(this).prop("checked");

                data[key] = value;
            });

        const tagContainer = $(".custom-tag-container .custom-tag", form);
        if (tagContainer.length > 0) {
            data[tagContainer.data("name")] = tagContainer
                .map(function () {
                    return $(this).text();
                })
                .get();
        }

        data = Object.assign({}, data, additionalData);

        return hm.sendAsync(method, url, Object.assign({}, _options, {
            data
        })).fail(function (err) {
            if (!err.errors) {
                openErrorDialog(
                    "오류",
                    err
                );

                return;
            }

            Object
                .entries(err.errors)
                .forEach(([k, v]) => {
                    var input = $(`[name=${k}], [data-name=${k}]`, form).addClass("is-invalid");

                    var rawInput = input.get(0);
                    if (rawInput && rawInput.setCustomValidity) {
                        rawInput.setCustomValidity(v[0]);
                    }

                    var invalidMessage = input.siblings(".invalid-feedback, .invalid-tooltip");
                    if (invalidMessage.length > 0) {
                        invalidMessage.text(v[0]);
                    } else {
                        invalidMessage = $(`<div class="invalid-feedback">${v[0]}</div>`).insertAfter(input);
                    }
                });
        });
    };

    fn.customTag = function (form, text) {
        form.addClass(selectors.customTagContainer);

        var newTag = $(
            `<div class="custom-tag"><span class="custom-tag-text" contenteditable>${text || "새 태그"}</span></div>`
        );

        $(selectors.customTagText, newTag)
            .after('<span class="custom-tag-remove oi oi-x ml-2" aria-hidden="true"></span>');

        $(selectors.customTagText, newTag)
            .focusout(function () {
                if ($(this).text().length === 0) {
                    newTag.remove();
                }
            });

        $(selectors.customTagRemove, newTag)
            .click(function () {
                newTag.remove();
            });

        newTag.insertBefore($(selectors.customTagAdd, form));

        return newTag;
    };

    //Object
    //    .entries(fn)
    //    .forEach(([k, fn]) => {
    //        $.fn[`${k}Fm`] = function (...args) {
    //            return fn.call(this, ...args);
    //        };
    //    });

    Object
        .entries(fn)
        .forEach(([k, fn]) => {
            $.fn[`${k}Fm`] = function () {
                return fn(this, arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
            };
        });

    // Custom Tags
    $(selectors.customTagAdd).click(function () {
        $(this)
            .closest(selectors.customTagContainer)
            .customTagFm();
    });
})(jQuery);