function AjaxHelperPost(_data, _url, _isAsync = true, callBack) {
    $.ajax({
        async: _isAsync,
        url: _url,
        type: "POST",
        dataType: 'json',
        data: { "dataObj": _data },
        success: function (result) {
            if (result.success) {
                ShowNotification(result.message);
            }
            else {
                ShowNotification(result.message);
            }
            var Response = {};
            Response.status = result.success;
            Response.message = result.message;
            Response.data = result.resData;
            if (callBack) {
                callBack(Response);
            }
        },
        error: function (xhr, resp, text) {
            ShowNotification(text);
        }
    });
}

function AjaxHelperGet(_data, _url, _isAsync = true, callBack) {
    $.ajax({
        async: _isAsync,
        url: _url,
        type: "GET",
        dataType: 'json',
        data: { "dataObj": _data },
        success: function (result) {
            if (result.success) {
                ShowNotification(result.message);
            }
            else {
                ShowNotification(result.message);
            }
            var Response = {};
            Response.status = result.success;
            Response.message = result.message;
            Response.data = result.resData;
            if (callBack) {
                callBack(Response);
            }
        },
        error: function (xhr, resp, text) {
            ShowNotification(text);
        }
    });
}

function ShowNotification(Msg) {
    alert(Msg);
}

function updateViewData(divname, ViewURL) {
    $.ajax({
        url: ViewURL,
        type: 'GET',
        dataType: 'html',
        data: {},
        success: function (data) {
            if (data == 1 || data == "1") {
                goToLoginView();
            }
            else {
                $("#" + divname).html("").append(data);
            }
        }
    });
}