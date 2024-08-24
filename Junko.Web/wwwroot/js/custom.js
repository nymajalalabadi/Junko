function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

/////ckeditor

$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});

/////ckeditor

function FillPageId(pageId) {
    $("#CurrentPage").val(pageId);
    $("#filter-form").submit();
}


///////create Product

$("[main_category_checkbox]").on('change', function (e) {

    var isChecked = $(this).is(':checked');

    var selectedCategoryId = $(this).attr('main_category_checkbox');

    if (isChecked)
    {
        $('#sub_categories_' + selectedCategoryId).slideDown(300);
    }
    else
    {
        $('#sub_categories_' + selectedCategoryId).slideUp(300);
        $('[parent-category-id="' + selectedCategoryId + '"]').prop('checked', false);
    }
});

$('#add_color_button').on('click', function (e)
{
    e.preventDefault();

    var colorName = $('#product_color_name_input').val();
    var colorPrice = $('#product_color_price_input').val();

    if (colorName !== '' && colorPrice !== '')
    {
        var currentColorsCount = $('#list_of_product_colors tr');
        var index = currentColorsCount.length;

        var isExistsSelectedColor = $('[color-name-hidden-input][value="' + colorName + '"]');

        if (isExistsSelectedColor.length === 0)
        {
            var colorNameNode =
                `<input type="hidden" value="${colorName}"  name="ProductColors[${index}].ColorName" color-name-hidden-input="${colorName}-${colorPrice}">`;

            var colorPriceNode =
                `<input type="hidden" value="${colorPrice}"  name="ProductColors[${index}].Price"color-price-hidden-input="${colorName}-${colorPrice}">`;

            $('#create_product_form').append(colorNameNode);
            $('#create_product_form').append(colorPriceNode);

            var colorTableNode =
                `<tr color-table-item="${colorName}-${colorPrice}"> <td> ${colorName} </td>  <td> ${colorPrice} </td>  <td> 
            <a class="btn btn-danger text-white" onclick="removeProductColor('${colorName}-${colorPrice}')">حذف</a> </td>  </tr>`;

            $('#list_of_product_colors').append(colorTableNode);

            $('#product_color_name_input').val('');
            $('#product_color_price_input').val('');
        }
        else
        {
            ShowMessage('اخطار', 'رنگ وارد شده تکراری می باشد', 'warning');
            $('#product_color_name_input').val('').focus();
        }
    }
    else
    {
        ShowMessage('اخطار', 'لطفا نام رنگ و قیمت آن را به درستی وارد نمایید', 'warning');
    }
});

function removeProductColor(index)
{
    $('[color-name-hidden-input="' + index + '"]').remove();
    $('[color-price-hidden-input="' + index + '"]').remove();
    $('[color-table-item="' + index + '"]').remove();

    reOrderProductColorHiddenInputs();
}

function reOrderProductColorHiddenInputs()
{
    var hiddenColors = $('[color-name-hidden-input]');

    $.each(hiddenColors, function (index, value)
    {
        var hiddenColor = $(value);

        var colorPrice = $(value).attr('color-name-hidden-input');

        var hiddenPrice = $('[color-price-hidden-input="' + colorPrice + '"]');

        $(hiddenColor).attr('name', 'ProductColors[' + index + '].ColorName')
        $(hiddenPrice).attr('name', 'ProductColors[' + index + '].Price');
    });
}

$('#add_size_button').on('click', function (e)
{
    e.preventDefault();

    var sizeName = $('#product_size_name_input').val();
    var countName = $('#product_count_name_input').val();

    if (sizeName !== '' && countName !== '')
    {
        var currentSizesCount = $('#list_of_product_sizes tr');
        var index = currentSizesCount.length; 

        var isExistsSelectedSize = $('[size-name-hidden-input][value="' + sizeName + '"]');

        if (isExistsSelectedSize.length === 0)
        {
            var sizeNameNode =
                `<input type="hidden" value="${sizeName}" name="ProductSizes[${index}].Size" 
            size-name-hidden-input="${sizeName}-${countName}">`;

            var countNameNode =
                `<input type="hidden" value="${countName}" name="ProductSizes[${index}].Count" 
            count-name-hidden-input="${sizeName}-${countName}">`;

            $('#create_product_form').append(sizeNameNode);
            $('#create_product_form').append(countNameNode);

            var sizeTableNode =
                `<tr size-table-item="$${sizeName}-${countName}"> <td> ${sizeName} </td> <td> ${countName}  </td> <td> 
            <a class="btn btn-danger text-white" onclick="removeProductSize('${sizeName}-${countName}')">حذف</a> </td>  </tr>`;

            $('#list_of_product_sizes').append(sizeTableNode);


            $('#product_size_name_input').val('');
            $('#product_count_name_input').val('');
        }
        else
        {
            ShowMessage('اخطار', 'رنگ وارد شده تکراری می باشد', 'warning');
            $('#product_size_name_input').val('').focus();
        }
    }
    else
    {
        ShowMessage('اخطار', 'لطفا سایز را به درستی وارد نمایید', 'warning');
    }
});

function removeProductSize(index)
{
    $('[size-name-hidden-input="' + index + '"]').remove();
    $('[count-name-hidden-input="' + index + '"]').remove();
    $('[size-table-item="' + index + '"]').remove();

    reOrderProductSizeHiddenInputs();
}

function reOrderProductSizeHiddenInputs()
{
    var hiddenSizes = $('[size-name-hidden-input]');

    $.each(hiddenSizes, function (index, value)
    {
        var hiddenSize = $(value);

        var sizeCount = $(value).attr('size-name-hidden-input');

        var hiddenCount = $('[count-name-hidden-input="' + sizeCount + '"]');

        $(hiddenSize).attr('name', 'ProductSizes[' + index + '].Size')
        $(hiddenCount).attr('name', 'ProductSizes[' + index + '].Count');
    });
}

///////create Product