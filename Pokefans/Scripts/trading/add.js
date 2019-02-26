// copyright 2016 the pokefans authors. see copying.md for legal info

$(function () {
    $("select[name!='search-index']").select2();
    $.trumbowyg.svgPath = '/Content/icons.svg';
    $('#DescriptionCode').trumbowyg({ lang: 'de' });

    var sticky = new Waypoint.Sticky({
        element: $('#add-sticky')
    })
});
