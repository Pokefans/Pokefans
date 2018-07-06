
$(document).ready(function() {
    $(".emojiarea").emojioneArea();



    var to = $('#input-to');
    to.tagsinput({
        tagClass: 'label label-primary',
        trimValue: true
    });


    var bcc = $("#input-bcc");
    bcc.tagsinput({
        tagClass: 'label label-primary',
        trimValue: true
    });

    var validateUser = function(ev) {
        var notfound = $("#usernotfound");
        if(!notfound.hasClass("hidden"))
            notfound.addClass("hidden");

        var user = ev.item;

        $.ajax({
            type: "POST",
            url: '/api/v1/validateuser',
            data: { username : user },
            success: function(data, status, jqxhr) {
                if(data == false) {
                    to.tagsinput('remove', user);
                    bcc.tagsinput('remove', user);
                    notfound.removeClass("hidden");
                }
            }
        });
    }

    bcc.on('beforeItemAdd', validateUser);
    to.on('beforeItemAdd', validateUser);
});
