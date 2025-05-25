$(document).ready(function () {
    //$('.js-select').select2();

    //$('#AddLocation').click(function () {
    //    let lectionRoomText = $('#ddLocation option:selected').text(); //$('#exampleDataList .form-control').val();
    //    let lectionRoomValue = $('#ddLocation option:selected').val();
    //    //console.log(lectionRoomValue.value);
    //    //console.log(lectionRoomText);

    //    if (lectionRoomText != "") {
    //        $('span.label-info').show().text(lectionRoomText);
    //        $('#exampleDataList .form-control').slideUp();
    //        $(this).attr("disabled", true);
    //        $('.rest-info').attr("disabled", false);
    //        $('#hdLocationId').val(lectionRoomValue);
    //    }

    //});

    //$('.rest-info').click(function () {
    //    let labelInfo = $('span.label-info').text();
    //    $('span.label-info').slideUp();
    //    $('#exampleDataList .form-control').slideDown();
    //    $('#exampleDataList .form-control').val(labelInfo);
    //    $('#AddLocation').attr("disabled", false);
    //    $(this).attr("disabled", true);
    //});
    $("#AddOrder").click(function () {

        var ddLocation = $("#ddLocation").val();
        var SelectItem = $("#ddlItems").val();
        var DisplayText = $("#ddlItems option:selected").text();
        var Count = $("#inputCount").val();
        var notes = $("#txtnotes").val();
        var isfound = 0;


        $("#contentValue .badge-item").each(function (index) {
            //alert(SelectItem);
            //alert($(this).data("itemid"));
            if ($(this).data("itemid") == SelectItem) {

                var prv_qty = $(this).data("qty");
                var prv_notes = $(this).data("notes");
                //$(this).remove();
                var new_qty = parseInt(prv_qty) + parseInt(Count);
                if (notes != null) {
                    notes = prv_notes;
                }

                var objData = $(this);

                var objCount = objData.find(".badge-number");// $(this).children(".badge-number");

                //objData.data("qty").vale = new_qty;
                //objData.data("notes").val = notes;
                //how to change Data Attributes value in javascript

                objData.data("qty", new_qty);
                objData.data("notes", notes);
                objCount.text(new_qty);

                //$("#contentValue").append(`
                //    <span class="badge-item mx-2 mb-3" data-itemid="${SelectItem}"
                //    data-qty="${new_qty}" data-notes="${notes}" data-itemname=${DisplayText} data-ddLocation="${ddLocation}">
                //    ${DisplayText}
                //    <span class="badge-number">${new_qty}</span>
                //    <span class="icon-exit">
                //    <i class="fa-solid fa-xmark fa-fw"></i>
                //    </span>
                //    `);
                isfound += 1;
                return false
            }
            //if (isfound != 0) {
            //    break;
            //}
        });
        if (isfound == 0) {
            $("#contentValue").append(`
                <span class="badge-item mx-2 mb-3" data-itemid="${SelectItem}"
                data-qty="${Count}" data-notes="${notes}" data-itemname="${DisplayText}" data-ddLocation="${ddLocation}">
                ${DisplayText}
                <span class="badge-number">${Count}</span>
                <span class="icon-exit">
                <i class="fa-solid fa-xmark fa-fw"></i>
                </span>
                `);
        }
        //var Count = $("#inputCount").val(1);
    });

    $("body").delegate(".icon-exit", "click", function () {

        //var SelectItem = $("#ddlItems").val();
        //alert("body");
        let parentData = $(this).parents(".badge-item");

        var orderId = parentData.data("orderid");

        //var objData = {
        //    OrderId: orderId
        //};
        //console.log(objData)
        //JsonData = JSON.stringify(orderId);


        $.ajax({
            url: '/CreateOrders/DeleteItemBasket',
            method: 'POST',
            //contentType: 'application/json',
            data: { OrderId: orderId },
            cache: false,
            success: function (data) {
                //alert('showListItems');
                //showListItems();
                // alert('showListItems2');
                parentData.remove();
            },
            error: function (error) {

                //alert('Bad');
            }


        });
        //$(this).parents(".badge-item").remove();
    });

    //Autocompleata Location///
    $("#txtLocation").on("input", function () {
        var searchTerm = $(this).val();
        $.ajax({
            url: "/CreateOrders/AutocompleteLocation",
            method: "GET",
            data: { term: searchTerm },
            success: function (data) {
                var datalist = $("#datalistLocation");
                datalist.empty();

                if (data.length > 0) {
                    data.forEach(function (item) {
                        datalist.append($("<option>", {
                            value: item.Value,
                            text: item.Text
                        }));
                    });
                }

            },
            error: function () {
                console.error('Failed to load data.');
            }
        });

    });
    ///End Autocompleata Location////
    //$("#demo .step-content .step-tab-panel button").click(function () {
    //    $(this)
    //        .parents(".step-tab-panel")
    //        .next()
    //        .addClass("active")
    //        .siblings()
    //        .removeClass("active");
    //    $("#demo .step-steps li.active").next().addClass("active");
    //});
    $("aside.aside-bar ul li").click(function () {
        $(this).addClass("active").siblings().removeClass("active");
    });
    if ($("table.table").hasClass("table-responsive-scroll")) {
        $(".scroll-table").addClass("active");
    }
    if ($("table.table").hasClass("table-responsive")) {
        $(".list-table").addClass("active");
    }
    $(".list-table").click(function () {
        if ($("table.table").hasClass("table-responsive-scroll")) {
            $("table.table")
                .addClass("table-responsive")
                .removeClass("table-responsive-scroll");
            $(this).addClass("active").siblings().removeClass("active");
        }
    });
    $(".scroll-table").click(function () {
        if ($("table.table").hasClass("table-responsive")) {
            $("table.table")
                .addClass("table-responsive-scroll")
                .removeClass("table-responsive ");
            $(this).addClass("active").siblings().removeClass("active");
        }
    });
    $(".group-button-toggle .btn-toggle").click(function () {
        $(this).toggleClass("active");
    });
    $(".expand-aside").click(function () {
        $("aside.aside-bar").toggleClass("show-expand-aside");
    });
    $(".show-aside").click(function () {
        $("aside.aside-bar").toggleClass("show-aside");
    });
    //add order modal
    $("#incrementButton").click(function () {
        if ($("#inputCount").val() < 99) {
            $("#inputCount").val(parseInt($("#inputCount").val()) + 1);
        }
    });
    $("#decrementButton").click(function () {
        if ($("#inputCount").val() > 1) {
            $("#inputCount").val(parseInt($("#inputCount").val()) - 1);
        }
    });
    //Additem modal
    $("#incrementButtonItem").click(function () {
        if ($("#inputCountitem").val() < 9) {
            $("#inputCountitem").val(parseInt($("#inputCountitem").val()) + 1);
        }
    });
    $("#decrementButtonItem").click(function () {
        if ($("#inputCountitem").val() > 1) {
            $("#inputCountitem").val(parseInt($("#inputCountitem").val()) - 1);
        }
    });
    //Edit item modal
    $("#incrementButtonItem_ed").click(function () {
        if ($("#inputCountitem_ed").val() < 9) {
            $("#inputCountitem_ed").val(parseInt($("#inputCountitem_ed").val()) + 1);
        }
    });
    $("#decrementButtonItem_ed").click(function () {
        if ($("#inputCountitem_ed").val() > 1) {
            $("#inputCountitem_ed").val(parseInt($("#inputCountitem_ed").val()) - 1);
        }
    });
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $("#imagePreview").css(
                    "background-image",
                    "url(" + e.target.result + ")"
                );
                $("#imagePreview").hide();
                $("#imagePreview").fadeIn(650);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }
    $("#imageUpload").change(function () {
        readURL(this);
    });
    //$(".js-example-templating").select2({
    //    tags: true,
    //});
});
