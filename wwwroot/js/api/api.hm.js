/**
 *
 * @param {"GET" | "POST" | "PUT" | "DELETE"} method
 * @param {string} url
 * @param {{
 *  param: string,
 *  queries: Map<string, string>,
 *  data: any,
 *  requestType: "text" | "json" | "form",
 *  responseType: "text" | "json" | "html" | "xml" | "script"
 * }} options
 */
hm.sendAsync = function (method, url, options = {}) {
    var _options = {
        param: options.param || "",
        queries: options.queries || {},
        data: options.data,
        requestType: options.requestType || "json",
        responseType: options.responseType || "json"
    };

    var contentType;
    switch (_options.requestType) {
        case "text":
            contentType = "text/plain; charset=UTF-8";
            break;
        case "json":
            contentType = "application/json; charset=UTF-8";
            break;
        case "form":
            contentType = "application/x-www-form-urlencoded; charset=UTF-8";
            break;
    }

    var deferred = $.Deferred();
    var requestUrl = _options.param !== "" ? `${url}/${_options.param}` : url;

    var queryString = Object.entries(_options.queries)
        .map(([k, v]) => `${k}=${v}`)
        .join("&");
    if (queryString !== "") {
        requestUrl += `?${queryString}`;
    }

    $
        .ajax(
            requestUrl,
            {
                method,
                data: JSON.stringify(_options.data),
                contentType,
                dataType: _options.responseType,
            }
        ).done(function (data, status, jqXHR) {
            var data;

            switch (_options.responseType) {
                //case "html":
                case "xml":
                    data = jqXHR.responseXML;
                    break;
                case "json":
                    data = jqXHR.responseJSON;
                    break;
                default:
                    data = jqXHR.responseText;
                    break;
            }

            deferred.resolve(data);
        }).fail(function (jqXHR, status, errorThrown) {
            var err;

            switch (_options.responseType) {
                //case "html":
                case "xml":
                    err = jqXHR.responseXML;
                    break;
                case "json":
                    err = jqXHR.responseJSON;
                    break;
                default:
                    err = jqXHR.responseText;
                    break;
            }

            deferred.reject(err);
        });

    return deferred;
}

hm.getAsync = function (url, options) {
    return hm.sendAsync("GET", url, options);
}

hm.postAsync = function (url, data, options) {
    return hm.sendAsync("POST", url, Object.assign({}, options, { data }));
}

hm.putAsync = function (url, data, options) {
    return hm.sendAsync("PUT", url, Object.assign({}, options, { data }));
}

hm.deleteAsync = function (url, options) {
    return hm.sendAsync("DELETE", url, options);
}