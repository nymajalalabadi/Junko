function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'facebook',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}
$('#submitOrderForm').on('click', function ()
{
    $('#addProductToOrderForm').submit();

    open_waiting();
});

function changeProductPriceBasedOnColor(colorId, priceOfColor, colorName)
{
    var basePrice = parseInt($('#ProductBasePrice').val(), 0);

    $('.current_price').html((basePrice + priceOfColor) + ' تومان' + ' ( ' + colorName + ' )');

    $('#add_product_to_order_ProductColorId').val(colorId);
}

function onSuccessAddProductToOrder(res)
{
    if (res.status === 'Success')
    {
        ShowMessage('اعلان', res.message);
    }
    else
    {
        ShowMessage('اعلان', res.message, 'warning');
    }

    setTimeout(function () {
        close_waiting();
    }, 3000);
}

function changeProductSize(sizeId)
{
    $('#add_product_to_order_ProductSizeId').val(sizeId);
}

$('#number_of_products_in_basket').on('change', function (e)
{
    var numberOfProducts = parseInt(e.target.value, 0);

    $('#add_product_to_order_Count').val(numberOfProducts);
});
