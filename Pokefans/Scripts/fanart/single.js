// Copyright 2016 the pokefans authors. See copying.md for legal info.
$(function () {
    $.ajaxSetup({
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        }
    });
    var status = [];
    $(".rating-star").children("i").each(function () {
        status.push($(this).attr("class"))
    })
    if ($(".rating-star").data("active") != "False") { //thanks obama
        $(".rating-star i").hover(function () {
            $(this).prevAll().andSelf().removeClass("fa-star fa-star-o fa-star-half-full").addClass("fa-star");
            $(this).nextAll().removeClass("fa-star fa-star-o fa-star-half-full").addClass("fa-star-o");
        },
        function () {
            var i = 0;
            $(".rating-star").children("i").each(function () {
                $(this).attr("class", status[i++]);
            })
        });

        $(".rating-star i").click(function () {
            var rating = $(this).prevAll().andSelf().length;
            var id = $(".rating-star").data("id");

            $.post("//api." + hostname() + "/v1/fanart/rate", { fanartId: id, rating: rating })
                .done(function (data) {
                    $(".rating-star").tooltip({ title: "Danke für deine Bewertung!" });
                    
                   var $elems = $(".rating-star i");
                    $elems.each().removeClass("fa-star fa-star-o fa-star-half-full");

                    // rebuild star display
                    for (i = 0; i < 5; i++) {
                        if (data < i && (data + 1) > i)
                            $elems.get(i).addClass("fa-star-half-full");
                        else if (data < i)
                            $elems.get(i).addClass("fa-star")
                        else
                            $elems,get(i).addClass("fa-star-o")
                    }
                })
                .fail(function (data) {
                    $(".rating-star").tooltip({ title: data })
                });
        });
    }

    // moderation tools
    $("#deleteform").submit(function (ev) {
        ev.preventDefault();

        $("#deleteconfirmmodal").modal('show');
    })
    $("#deleteconfirmed").click(function () {
        $form = $("#deleteform");
        $.post({
            url: $form.attr('action'),
            dataType: 'json',
            data: $form.serialize(),
            success: function (data) {
                window.location.href("/");
            },
            error: function (xhr, err) {
                alert('Es ist ein Fehler aufgetreten :(');
            }
        });
    })

    $("#moveform").submit(function (ev) {
        $form = $("#deleteform");
        $.post({
            url: $form.attr('action'),
            dataType: 'json',
            data: $form.serialize(),
            success: function (data) {
                window.location.href("/");
            },
            error: function (xhr, err) {
                alert('Es ist ein Fehler aufgetreten :(');
            }
        });
    });
});

function hostname() {
    return location.hostname.split('.').slice(-2).join('.');
}