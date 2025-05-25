
$(document).ready(function () {
    /*   $('.js-selectMulti').select2();*/
    //$("#tabledashboard").DataTable();
   
    //$('.js-selectMultipl').select2({
    //    theme: "bootstrap-5",
    //    width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
    //    placeholder: $(this).data('placeholder'),
    //    closeOnSelect: false,
    //});
    //Handel Modal
    $("#autoLocation").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/CreateOrders/AutocompleteLocation",
                data: { "prefix": request.term },
                type: "POST",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText)
                },
                failure: function (response) {
                    alert(response.responseText)
                }
            });
        },
        select: function (e, i) {
            $("#autoLocationID").val(i.item.val);
            $("#hdLocationId").val(i.item.val);

        },
        minLength: 1
    }).autocomplete("widget").css("max-height", "200px").css("overflow-y", "auto");
    //    .data("ui-autocomplete")._renderMenu = function (ul, items) {
    //    var that = this;
    //    $.each(items.slice(0, 5), function (index, item) {
    //        that._renderItemData(ul, item);
    //    });
    //};
    // Add keyup event listener for the Enter key
    //$("#autoLocation").keyup(function (e) {
    //    if (e.which === 13) { // Check if the pressed key is Enter (key code 13)
    //        // Trigger the selection of the first item in the autocomplete list
    //        var autocomplete = $("#autoLocation").autocomplete("widget");
    //        var firstItem = autocomplete.find("li:first-child");
    //        if (firstItem.length > 0) {
    //            $("#autoLocation").autocomplete("option", "select", function (event, ui) {
    //                // Trigger the select event for the first item
    //                $("#autoLocation").autocomplete("option", "select")(event, { item: firstItem.data("ui-autocomplete-item") });
    //            });

    //            // Close the autocomplete dropdown
    //            autocomplete.hide();
    //        }
    //    }
    //});
    $("#autoitems").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/CreateOrders/AutocompleteItems",
                data: { "prefix": request.term },
                type: "POST",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText)
                },
                failure: function (response) {
                    alert(response.responseText)
                }
            });
        },
        select: function (e, i) {
            $("#autoItemID").val(i.item.val);
        },
        minLength: 1,
    }).autocomplete("widget").css("max-height", "200px").css("overflow-y", "auto");
    //}).data("ui-autocomplete")._renderMenu = function (ul, items) {
    //    var that = this;
    //    $.each(items.slice(0, 5), function (index, item) {
    //        that._renderItemData(ul, item);
    //    });
    //};

    $('.js-removeitembasket').on('click', function () {

        var Btn = $(this);


        var orderId = Btn.data("orderid");
        var basketid = Btn.data("basketid");
        //alert(orderId);
        var RowItem = Btn.parents('.js-basketitem');
        console.log(RowItem);
        RowItem.remove();
        //$.ajax({
        //    url: '/CreateOrders/RemoveItemBasket',
        //    method: 'POST',
        //    //contentType: 'application/json',
        //    data: { OrderId: orderId, basketid: basketid },
        //    cache: false,
        //    success: function (data) {

        //        RowItem.remove();
        //    },
        //    error: function (error) {

        //        //alert('Bad');
        //    }


        //});
    });


});


function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function ShowMsgWarning(msgtext) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr.warning(msgtext);
}
function ShowMsgSuccess(msgtext) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr.success(msgtext);
}
function ShowMsgError(msgtext) {
    
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    toastr.error(msgtext);
}

function showSpinner() {
    document.getElementById("spinner-container").style.display = "block";
    document.body.classList.add("locked");
}

// Function to hide the spinner and unlock the screen
function hideSpinner() {
    document.getElementById("spinner-container").style.display = "none";
    document.body.classList.remove("locked");
}


