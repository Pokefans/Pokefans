// copyright 2016 the pokefans authors. see copying.md for legal info

$(function () {
    $('select').select2();
    $.trumbowyg.svgPath = '/Content/icons.svg';
    $('#DescriptionCode').trumbowyg({ lang: 'de' });
});