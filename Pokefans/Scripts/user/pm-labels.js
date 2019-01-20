// Copyright 2016 the pokefans authors. See copying.md for legal info.
$.fn.editableform.buttons = '<button type="submit" class="editable-submit btn btn-primary btn-sm"><i class="fa fa-check"></i></button>' +
    '<button type="button" class="editable-cancel btn btn-default btn-sm"><i class="fa fa-times"></i></button>';


$("#labels .delete").click(function (e) {
    e.preventDefault();
    var id = $(this).data("id");
    $.ajax({
        dataType: "json",
        type: "POST",
        url: "/api/v1/labels/delete",
        data: "id="+ id,
        success: function (data, textStatus, jqXHR) {
            if(data)
            {
                $("#label" + id).remove();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (window.console)
                console.log('ajax failed :( status:' + textStatus + ' ; error:' + errorThrown.toString());
        }
    });
});

$("#labels .editable").each(function (e) {
    $(this).editable();
});

$("#colorspinner").click(function(e) {
    e.preventDefault();
    var color = randomColor();
    $("#label-color").val(color);
    $("#preview").css("background-color", color);
});

$("#label-color").on('input', function() {
    $("#preview").css("background-color", this.value);
});

$("#label-label").on('input', function() {
    $("#preview").text(this.value)
});

function randomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

$(document).ready(function() {
    $("#colorspinner").click();
});