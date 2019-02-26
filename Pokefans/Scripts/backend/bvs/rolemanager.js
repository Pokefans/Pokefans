// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
var blockAjax = false;
$(function () {
    $(".role-checkbox").change(function (ev) {
        if(blockAjax)
            return;

        var cb = $(this);
        var status = false;
        
        if (this.checked == true) {
            status = true;
        }

        $.ajax({
            url: '/api/rolemanager/' + $('#currentUserId').val() + '/set-role',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            method: "POST",
            data: JSON.stringify({ role: cb.val(), status: status }),
            success: function (data) {
                if (data == true) {
                    $('#alert-container').html('<div class="alert alert-success alert-dismissable">Status geändert.</div>');

                    if (status == true) {
                        blockAjax = true;
                        setEnabledRecursive($(ev.target), status);
                        blockAjax = false;
                    }
                }
                else {
                    blockAjax = true;
                    $(ev.target).prop("checked", !status);
                    blockAjax = false;

                    switch (data) {
                        case -1: $('#alert-container').html('<div class="alert alert-danger alert-dismissable">Fehler beim Ändern des Status: Der Benutzer ist in einer dieser Rolle übergeordneten Rolle. Entferne ihn zunächst aus dieser Rolle.</div>'); break;
                        default: $('#alert-container').html('<div class="alert alert-danger alert-dismissable">Fehler beim Ändern des Status (controller returned ' + data + ').</div>'); break;
                    }

                }
            },
            error: function (xhr, status, err) {
                $('#alert-container').html('<div class="alert alert-danger alert-dismissable">Fehler beim Ändern des Status (' + status + ';' + err + ').</div>');
                blockAjax = true;
                $(ev.target).prop("checked", !status);
                blockAjax = false;
            }
        });
    });
});

function setEnabledRecursive(target, status) {
    try {
        var vars = target.data("children").split(",");
    } catch (e) {
        return;
    }
    vars.forEach(function (id) {
        $('input[value="' + id + '"]').prop("checked", status);
        setEnabledRecursive($('input[value="' + id + '"]'), status);
    });
}