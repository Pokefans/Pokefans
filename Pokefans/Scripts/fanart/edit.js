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

    $('#comment-toolbar button').click(function () {
        // this won't work on IE below IE9
        // but seriously: if you're still using that, there are other things you should worry about

        var $code = $('#description-code')[0];
        console.log($code.selectionStart);
        console.log($code.selectionEnd);
        console.log($code.value);

        var oldValue = $code.value.substring($code.selectionStart, $code.selectionEnd);

        var text = $code.value.substring(0, $code.selectionStart);
        var after = $code.value.substring($code.selectionEnd, $code.value.length);
        
        text += $(this).data("template").replace("%s", oldValue) + after;
        $code.value = text;
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