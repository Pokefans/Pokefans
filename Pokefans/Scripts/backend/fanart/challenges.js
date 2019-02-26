// Copyright 2016 the pokefans authors. See copying.md for legal info.
$('#addChallenge').click(function (e) {
    e.preventDefault();

    $.ajax({
        dataType: "json",
        type: "POST",
        url: '/api/fanart/challenges/add',
        data: $('#challenge-form').serialize(),
        success: function (data, textStatus, jqXHR) {
            if (data != false) {
                $('#fanart-challenges').append('<tr><td>' + data.Id + '</td><td>' + data.Name + '</td><td>' + data.ExpireDate + '</td><td>' + data.Tag.Name + '</td><td><a href="/fanart/challenges/' + data.Id + '"><i class="fa fa-info-circle"></i></a></td></tr>');
                $("#fanart-challenges tr:last").clearQueue().queue(function (next) {
                    $(this).addClass("alert"); next();
                }).delay(200).queue(function (next) {
                    $(this).removeClass("alert"); next();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (window.console)
                console.log('ajax failed :( status:' + textStatus + ' ; error:' + errorThrown.toString());
        }
    });
});