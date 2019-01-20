// Copyright 2016 the pokefans authors. See copying.md for legal info.
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