function RejectProductModal(id) {
    $.ajax({
        url: "/admin/Products/RejectProduct",
        type: "get",
        data: {
            id: id
        },
        beforeSend: function () {
            StartLoading("#LargeModalBody");
        },
        success: function (response) {
            EndLoading("#LargeModalBody");

            $("#LargeModalLabel").html("ثبت اطلاعات");
            $("#LargeModalBody").html(response);

            $("#LargeModal").modal("show");
        },
        error: function () {
            EndLoading("#LargeModalBody");
            swal({
                title: "خطا",
                text: "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .",
                icon: "error",
                button: "باشه"
            });
        }
    });

}

function OnSuccessRejectProduct(response) {

    if (response.status === 'Success') {
        $("#LargeModal").modal("hide");
        ShowMessage('اعلان موفقیت', response.message);
        $('#filter-form').load(location.href + ' #filter-form');
        $('#ajax-url-item-' + response.data.id).hide(300);
    } else {
        swal({
            title: "خطا",
            text: response.message,
            icon: "error",
            button: "باشه"
        });
    }

}