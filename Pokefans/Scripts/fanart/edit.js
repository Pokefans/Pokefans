// Copyright 2016 the pokefans authors. See copying.md for legal info.
$(function () {

    var tags = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        prefetch: '/api/v1/tags/',
        remote: {
            url: '/api/v1/tags/?q=%QUERY',
            wildcard: '%QUERY'
        },
        filter: function (list) {
            return $.map(list, function (tagname) {
                return { tag: tagname };
            });
        }
    });

    $('#fanart-tags').tagsinput({
        tagClass: 'label label-primary',
        trimValue: true,
        typeaheadjs: {
            name: 'tagname',
            displayKey: 'tag',
            valueKey: 'tag',
            source: tags.ttAdapter()
        }
    });

    $('#delete-button').click(function (e) {
        $validate = $('#delete-validator');

        if ($validate.value != $validate.data('validate')) {
            alert("Bitte tippe den Sicherheitscode ab und drücke danach auf Löschen!");
            e.preventDefault();
        }
    });

    var $success = $('#editsuccess');
    if (typeof ($success) != undefined) {
        $success.delay(4000).alert('close');
    }
});