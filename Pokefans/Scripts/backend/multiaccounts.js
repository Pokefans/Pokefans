var currentUserId = 0;
$('#multiaccount-infoModal').on('show.bs.modal', function (event) {
    $('#alert-container').html('');
    var button = $(event.relatedTarget); // Button that triggered the modal
    var modal = $(this);
    currentUserId = button.data("id");
    modal.find('#account-title').text(button.data("user"));
    modal.find('#account-text').html($("<div/>").html(button.data("content")).text());
    modal.find('#account-date').text(button.data("time"));
    modal.find('#account-moderator').text(button.data("moderator"));

    var url = '';
    if (window.location.hash) {
        url = window.location.href.replace(window.location.hash, '') + '#' + button.data('id');
    }
    else {
        url = window.location.href + '#' + button.data('id');
    }
    modal.find('#modal-direct-link').attr('href', url);
    window.location.hash = button.data('id');
});
$('#multiaccount-infoModal').on('hide.bs.modal', function (event) {
    window.location.hash = '';
});
// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
$(document).ready(function () {
    $('#account-exception').click(function () {
        $.ajax({
            url: '/api/multiaccount/' + currentUserId + '/set-exception',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            method: "POST",
            success: function (data) {
                if (data == true) {
                    $('#alert-container').html('<div class="alert alert-success alert-dismissable">Status geändert.</div>');
                }
                else {
                    $('#alert-container').html('<div class="alert alert-danger alert-dismissable">Fehler beim Ändern des Status (controller).</div>');
                }
            },
            error: function (xhr, status, err) {
                $('#alert-container').html('<div class="alert alert-danger alert-dismissable">Fehler beim Ändern des Status (' + status + ').</div>');
            }
        });
    });
    $('#account-no-multiaccount').click(function () {
        $.ajax({
            url: '/api/multiaccount/' + currentUserId + '/set-no-multiaccount',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            method: "POST",
            success: function (data) {
                if (data == true) {
                    $('#alert-container').html('<div class="alert alert-success alert-dismissable">Status geändert.</div>');
                }
                else {
                    $('#alert-container').html('<div class="alert alert-danger alert-dismissable">Fehler beim Ändern des Status (controller).</div>');
                }
            },
            error: function (xhr, status, err) {
                $('#alert-container').html('<div class="alert alert-danger alert-dismissable">Fehler beim Ändern des Status (' + status + ').</div>');
            }
        });
    });
    if (window.location.hash) {
        var hash = window.location.hash.substring(1); //Puts hash in variable, and removes the # character
        if (parseInt(hash) != NaN)
            $("button[data-id=" + hash + "]").click();
    }
});