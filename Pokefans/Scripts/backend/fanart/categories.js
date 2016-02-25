$.fn.editableform.buttons = '<button type="submit" class="editable-submit btn btn-primary btn-sm"><i class="fa fa-check"></i></button>' +
    '<button type="button" class="editable-cancel btn btn-default btn-sm"><i class="fa fa-times"></i></button>';

$("#fanartAddCategory").click(function (e) {
    e.preventDefault();

    $.ajax({
        dataType: "json",
        type: "POST",
        url: "/api/fanart/categories/add",
        data: "name=" + document.getElementById('fanart-new-cat-name').value,
        success: function (data, textStatus, jqXHR) {
            if (data != false) {
                $("#fanart-categories").append('<tr class="hl" id="cat' + data.Id + '"><td>' + data.Id +
                    '</td><td><a href="#" class="fanart-cat-editable" data-mode="inline" data-type="text" data-type="text" data-pk="' + data.Id +
                    '" data-url="/api/fanart/categories/edit" data-title="Neuer Name">' + data.Name + '</td><td><code>' + data.Uri + '</a></code></td>' +
                    '<td><a href="#" class="fanart-cat-editable" data-name="size" data-mode="inline" data-type="text" data-pk="'+data.Id+'" data-url="/api/fanart/categories/edit" data-title="Neue Größe">'+data.MaxFileSize+'</a></td>' +
                    '<td><a href="#" class="fanart-cat-editable" data-name="dimension" data-mode="inline" data-type="text" data-pk="' + data.Id + '" data-url="/api/fanart/categories/edit" data-title="Neue Dimension">'+data.MaximumDimension+'</a></td>' +
                    '<td><button data-id="' + data.Id + '" class="edit btn btn-danger"><i class="fa fa-trash"></i></button></td>');

                $("#fanart-categories tr:last").clearQueue().queue(function (next) {
                    $(this).addClass("alert"); next();
                }).delay(200).queue(function (next) {
                    $(this).removeClass("alert"); next();
                });
                $("#fanart-categories tr:last .fanart-cat-editable").each(function (e) {
                    $(this).editable();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (window.console)
                console.log('ajax failed :( status:' + textStatus + ' ; error:' + errorThrown.toString());
        }
    });
});

$("#fanart-categories .delete").click(function (e) {
    e.preventDefault();
    var id = $(this).data("id");
    $.ajax({
        dataType: "json",
        type: "POST",
        url: "/api/fanart/categories/delete",
        data: "id="+ id,
        success: function (data, textStatus, jqXHR) {
            if(data)
            {
                $("#cat" + id).remove();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (window.console)
                console.log('ajax failed :( status:' + textStatus + ' ; error:' + errorThrown.toString());
        }
    });
});

$("#fanart-categories .fanart-cat-editable").each(function (e) {
    $(this).editable();
});