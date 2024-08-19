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

        var colorNameNode =
            `<input type="hidden" value="${colorName}"  name="ProductColors[${index}].ColorName" color-name-hidden-input="${index}">`;

        var colorPriceNode =
            `<input type="hidden" value="${colorPrice}"  name="ProductColors[${index}].Price"  color-price-hidden-input="${index}">`;

        $('#create_product_form').append(colorNameNode);
        $('#create_product_form').append(colorPriceNode);

        var colorTableNode =
            `<tr color-table-item="${index}"> <td> ${colorName} </td>  <td> ${colorPrice} </td>  <td> <a class="btn btn-danger text-white" onclick="removeProductColor(${index})">حذف</a> </td>  </tr>`;

        $('#list_of_product_colors').append(colorTableNode);

        $('#product_color_name_input').val('');
        $('#product_color_price_input').val('');
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
}

$('#add_size_button').on('click', function (e)
{
    e.preventDefault();

    var sizeName = $('#product_size_name_input').val();

    if (sizeName !== '')
    {
        var currentSizesCount = $('#list_of_product_sizes tr');
        var index = currentSizesCount.length; 

        var sizeNameNode =
            `<input type="hidden" value="${sizeName}" name="ProductSizes[${index}].Size" size-name-hidden-input="${index}">`;

        $('#create_product_form').append(sizeNameNode);

        var sizeTableNode =
            `<tr size-table-item="${index}"> <td> ${sizeName} </td> <td> <a class="btn btn-danger text-white" 
            onclick="removeProductSize(${index})">حذف</a> </td>  </tr>`;

        $('#list_of_product_sizes').append(sizeTableNode);


        $('#product_size_name_input').val('');
    }
    else
    {
        ShowMessage('اخطار', 'لطفا سایز را به درستی وارد نمایید', 'warning');
    }
});

function removeProductSize(index)
{
    $('[size-name-hidden-input="' + index + '"]').remove();
    $('[size-table-item="' + index + '"]').remove();
}

///////create Product