// Copyright 2016 the pokefans authors. See copying.md for legal info.
if (document.getElementById('fanart-bans') != undefined) {
    $('#fanart-banhammer').click(function (e) {
        $.ajax({
            type: "POST",
            url: '/api/user/bans/fanart',
            data: $("#fanart-bans").serialize(), // serializes the form's elements.
            success: function (data) {
                alert(data);
            }
        });

        e.preventDefault(); // avoid to execute the actual submit of the form.
    });
}