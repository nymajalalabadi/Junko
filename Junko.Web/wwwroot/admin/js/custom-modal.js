function RejectSellerModal(id) {

    $.ajax({
        url: "/admin/seller/RejectSeller",
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

function OnSuccessRejectItem(response)
{

    if (response.status === 'Success')
    {
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

function StartLoading(selector = 'body') {
    $(selector).waitMe({
        effect: 'bounce',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255, 255, 255, 0.7)',
        color: '#000'
    });
}

function EndLoading(selector = 'body') {
    $(selector).waitMe('hide');
}