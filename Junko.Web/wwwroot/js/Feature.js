$('#add_feature_button').on('click', function (e) {
    e.preventDefault();

    var featureTitle = $('#product_feature_title_input').val();
    var featureValue = $('#product_feature_value_input').val();

    if (featureTitle !== '' && featureValue !== '')
    {
        var currentFeaturesCount = $('#list_of_product_features tr');
        var index = currentFeaturesCount.length;

        var isExistsSelectedFeature = $('[feature-title-hidden-input][value="' + featureTitle + '"]');

        if (isExistsSelectedFeature.length === 0)
        {
            var featureTitleNode = `<input type="hidden" value="${featureTitle}"  name="ProductFeatures[${index}].FeatureTitle" 
            feature-title-hidden-input="${featureTitle}-${featureValue}">`;

            var featureValueNode = `<input type="hidden" value="${featureValue}"  name="ProductFeatures[${index}].FeatureValue" 
            feature-value-hidden-input="${featureTitle}-${featureValue}" >`;

            $('#create_product_form').append(featureTitleNode);
            $('#create_product_form').append(featureValueNode);

            var featureTableNode = `<tr feature-table-item="${featureTitle}-${featureValue}"> <td> ${featureTitle} </td>  <td> ${featureValue} </td>  <td> <a class="btn btn-danger text-white" onclick="removeProductFeature('${featureTitle}-${featureValue}')">حذف</a> </td>  </tr>`;

            $('#list_of_product_features').append(featureTableNode);


            $('#product_feature_title_input').val('');
            $('#product_feature_value_input').val('');

        } else {
            ShowMessage('اخطار', 'ویژگی وارد شده تکراری می باشد', 'warning');
            $('#product_feature_title_input').val('').focus();
        }
    } else {
        ShowMessage('اخطار', 'لطفا نام ویژگی و مقدار آن را به درستی وارد نمایید', 'warning');
    }
});


function removeProductFeature(index) {
    $('[feature-title-hidden-input="' + index + '"]').remove();
    $('[feature-value-hidden-input="' + index + '"]').remove();
    $('[feature-table-item="' + index + '"]').remove();

    reOrderProductFeatureHiddenInputs();
}

function reOrderProductFeatureHiddenInputs() {
    var hiddenFeatureTitles = $('[feature-title-hidden-input]');

    $.each(hiddenFeatureTitles, function (index, value) {
        var hiddenFeatureTitle = $(value);

        var titleValue = $(value).attr('feature-title-hidden-input');

        var featureValue = $('[feature-value-hidden-input="' + titleValue + '"]');

        $(hiddenFeatureTitle).attr('name', 'ProductFeatures[' + index + '].FeatureTitle');
        $(featureValue).attr('name', 'ProductFeatures[' + index + '].FeatureValue');
    });
}

